using System.Collections.Generic;
using FeedBackSendMail.Models;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Auth;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using EleOota.Models.Common;
using Service.Models;
using EleOota.Service.Interface;

namespace FeedBackSendMail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class FeedBackMailController : ControllerBase
    {
        private readonly IConfiguration config;
        ILoginService _loginService;
        public FeedBackMailController(IConfiguration configuration, ILoginService loginService)
        {
            config = configuration;
            _loginService = loginService;
        }
        [HttpPost]
        [Route("SendFeedBackMail")]
        public async Task<string> SendFeedBackMail([FromBody] FeedBackMailDTO item)
        {
            if (item != null && item.Dishes != null && item.Dishes.Count > 0)
            {
                item.Company = string.IsNullOrEmpty(item.Company) ? "----" : item.Company;
                item.Email = string.IsNullOrEmpty(item.Email) ? "----" : item.Email;
                item.Mobile = string.IsNullOrEmpty(item.Mobile) ? "----" : item.Mobile;
                string str = ""; string str2 = ""; string str1 = "";
                string html = config.GetValue<string>("EmailSettings:Html");
                int rownumber = 0;
                if (item.Dishes.Count > 0)
                {
                    str += "<table width='100%' style=" + "'border-collapse: collapse; width: 100%;'" + " class=" + "'table1'" + "><tr>" +
                         "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #378737;  color: white;'" + " > SL.NO. </th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #378737;  color: white;'" + " > Dishes</th>" +
                             "<th style=" + "'border:1px solid black;text-align:center;font-size:13px;background-color: #378737;  color: white;'" + " > Rating </th>" +
                             "</tr>";
                    for (int j = 0; j < item.Dishes.Count; j++)
                    {
                        rownumber += 1;
                        str1 += "<tr> " +
                       "<td style=" + "'border: 1px solid black; text-align:center;font-size:14px;font-weight:bold;'" + " > " + rownumber + "</td> " +
                       "<td style=" + "'border: 1px solid black; text-align:left;font-size:14px;font-weight:bold;font-family: sans-serif;'" + " >" + item.Dishes[j].Dish + "</td>" +
                       "<td align=" + "left " + "style=" + "'font-size:14px;border: 1px solid black;font-family: sans-serif;'" + ">" +
                       item.Dishes[j].Rating + "</td>" +
                       "</tr>";
                    }
                    str2 = "</table>";
                }
                string message = str + str1 + str2;
                html = html.Replace("[Feedbackday]", item.Day).Replace("[FeedbackType]", item.Type).Replace("[Feedbaclfrom]", item.Name).Replace("[Feedbackmsg]", item.Feedback).Replace("[Feedbackmobile]", item.Mobile).Replace("[Feedbackemail]", item.Email).Replace("[MSG]", message).Replace("[Feedbackdate]", item.FeedbackDate).Replace("[Company]", item.Company);

                var ApiKey = config.GetValue<string>("EmailSettings:ApiKey");
                var uploadtype = config.GetValue<string>("EmailSettings:uploadtype");
                var SenderMail = config.GetValue<string>("EmailSettings:Sender");
                var To = config.GetValue<string>("EmailSettings:To");
                var From = config.GetValue<string>("EmailSettings:From");
                var client = new SendGridClient(ApiKey);
                var from = new EmailAddress(SenderMail, From);
                var subject = item.Day + " - " + item.Type + " FeedBack By " + item.Name;
                var to = new EmailAddress(To, From);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", html);
                var response = await client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    if (uploadtype.Equals("txt"))
                    {
                        var jsonStr = JsonConvert.SerializeObject(item, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
                        AppendBlobTxt(jsonStr);
                    }
                    else if (uploadtype.Equals("csv"))
                    {
                        AppendBlob(item);
                    }

                    return "Feedback Submitted Successfully!";
                }
            }
            return "";
        }

        [HttpPost]
        [Route("AppendBlob")]
        public async void AppendBlob(FeedBackMailDTO items)
        {
            List<FeedBackMailDTO> FeedList = new List<FeedBackMailDTO>();
            List<FileFeedBackMailDTO> loadFeedList = new List<FileFeedBackMailDTO>();
            var configMap = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FileFeedBackMailDTO, FeedBackMailDTO>().ReverseMap();
                cfg.CreateMap<FileFeedBackMailDTO, Items>().ForMember(dest => dest.Dish, opt => opt.MapFrom(src => src.Dishes)).ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Rating)).ReverseMap();
            });

            var StorageConnectionString = config.GetValue<string>("EmailSettings:StorageConnectionString");
            var containername = config.GetValue<string>("EmailSettings:containername");
            var filename = config.GetValue<string>("EmailSettings:filenamecsv");
            var STORAGE_ACCOUNT_NAME = config.GetValue<string>("EmailSettings:STORAGE_ACCOUNT_NAME");
            var STORAGE_PRIMARY_KEY = config.GetValue<string>("EmailSettings:STORAGE_PRIMARY_KEY");

            StorageCredentials creds = new StorageCredentials(STORAGE_ACCOUNT_NAME, STORAGE_PRIMARY_KEY);
            CloudStorageAccount strAcc = new CloudStorageAccount(creds, true);
            CloudBlobClient serviceClient = strAcc.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(containername);
            await container.CreateIfNotExistsAsync();
            CloudAppendBlob appBlob = container.GetAppendBlobReference(DateTime.Now.Year + "/" + DateTime.Now.ToString("MMMM") + "/" + filename);
            Boolean FileCreated = false;
            if (!await appBlob.ExistsAsync())
            {
                FileCreated = true;
                await appBlob.CreateOrReplaceAsync();
            }
            if (await appBlob.ExistsAsync())
            {
                FeedList.Add(items);
                if (FeedList != null && FeedList.Count > 0)
                {
                    foreach (FeedBackMailDTO item in FeedList)
                    {
                        foreach (Items dishes in item.Dishes)
                        {
                            var mapper = new Mapper(configMap);
                            var Mapdishes = mapper.Map<FeedBackMailDTO, FileFeedBackMailDTO>(item);
                            mapper.Map<Items, FileFeedBackMailDTO>(dishes, Mapdishes);
                            loadFeedList.Add(Mapdishes);
                        }
                    }
                }
                using (var writer = new StringWriter())
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture) { LeaveOpen = false, HasHeaderRecord = (FileCreated == true ? true : false) }))
                {
                    csv.WriteRecords(loadFeedList);
                    string csvContent = writer.ToString();
                    Stream streamToUploadToBlob = GenerateStreamFromString(csvContent);
                    await appBlob.AppendFromStreamAsync(streamToUploadToBlob);
                }
            }
        }

        [HttpPost]
        [Route("AppendBlobTxt")]
        public async void AppendBlobTxt(string jsonStr)
        {
            var containername = config.GetValue<string>("EmailSettings:containername");
            var filename = config.GetValue<string>("EmailSettings:filenametxt");
            var STORAGE_ACCOUNT_NAME = config.GetValue<string>("EmailSettings:STORAGE_ACCOUNT_NAME");
            var STORAGE_PRIMARY_KEY = config.GetValue<string>("EmailSettings:STORAGE_PRIMARY_KEY");

            StorageCredentials creds = new StorageCredentials(STORAGE_ACCOUNT_NAME, STORAGE_PRIMARY_KEY);
            CloudStorageAccount strAcc = new CloudStorageAccount(creds, true);
            CloudBlobClient serviceClient = strAcc.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(containername);
            await container.CreateIfNotExistsAsync();
            CloudAppendBlob appBlob = container.GetAppendBlobReference(DateTime.Now.Year + "/" + DateTime.Now.ToString("MMMM") + "/" + filename);
            Boolean FileCreated = false;
            if (!await appBlob.ExistsAsync())
            {
                FileCreated = true;
                await appBlob.CreateOrReplaceAsync();
            }
            if (await appBlob.ExistsAsync())
            {
                if (FileCreated == true)
                {
                    jsonStr = "[" + jsonStr + "]";
                }
                else
                {
                    jsonStr = ",[" + jsonStr + "]";
                }

                var appendFileStream = GenerateStreamFromString(jsonStr);
                await appBlob.AppendFromStreamAsync(appendFileStream);
            }
        }

        [HttpGet]
        [Route("Getfoodlist")]
        public async Task<dynamic> Getfoodlist()
        {
            var StorageConnectionString = config.GetValue<string>("EmailSettings:StorageConnectionString");
            var containername = config.GetValue<string>("EmailSettings:foodlistcontainername");
            var filename = config.GetValue<string>("EmailSettings:filenamefoodlists");
            if (!string.IsNullOrEmpty(StorageConnectionString) && !string.IsNullOrEmpty(containername) && !string.IsNullOrEmpty(filename))
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                CloudBlobClient serviceClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = serviceClient.GetContainerReference(containername);
                if (await container.ExistsAsync())
                {
                    CloudBlob file = container.GetBlobReference(filename);
                    if (await file.ExistsAsync())
                    {
                        CloudAppendBlob blob = container.GetAppendBlobReference(filename);
                        MemoryStream stream = new MemoryStream();
                        await blob.DownloadToStreamAsync(stream);
                        var jsonstring = Encoding.UTF8.GetString((stream as MemoryStream).ToArray()).ToString();
                        dynamic result = System.Text.Json.JsonSerializer.Deserialize<dynamic>(jsonstring);
                        return result;
                    }
                }
            }
            return "";
        }

        [Route("VerifyFeedbackUser")]
        [HttpGet]
        public async Task<ResponseModel<bool>> VerifyFeedbackUser([FromQuery] EmailParam emailParam)
        {
            return await _loginService.VerifyFeedbackUser(emailParam);
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}