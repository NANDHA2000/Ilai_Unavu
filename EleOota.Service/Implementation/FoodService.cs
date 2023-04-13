using EleOota.Models.Common;
using EleOota.Repository.Interface;
using EleOota.Service.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;


namespace EleOota.Service.Implementation
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IConfiguration config;

        public FoodService(IFoodRepository foodRepository, IConfiguration configuration)
        {
            _foodRepository = foodRepository ?? throw new ArgumentNullException(nameof(_foodRepository));
            config = configuration;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            IEnumerable<Employee> employee;
            employee = await _foodRepository.GetAllEmployees();
            return employee.ToList();
        }

        public async Task<List<FoodType>> GetAllFoodTypes(int id)
        {
            IEnumerable<FoodType> foodType;
            foodType = await _foodRepository.GetAllFoodTypes(id);
            return foodType.ToList();
        }

        public async Task<List<Company>> GetCompanyList()
        {
            IEnumerable<Company> company;
            company = await _foodRepository.GetCompanyList();
            return company.ToList();
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _foodRepository.GetEmployeeById(id);

        }
        public async Task<Company> GetCompanyById(int id)
        {
            return await _foodRepository.GetCompanyById(id);
        }

        public async Task<FoodRequest> GetFoodRequestByEmployeeId(int id)
        {
            return await _foodRepository.GetFoodRequestByEmployeeId(id);
        }

        public async Task<List<WeekDayCount>> GetNumberOfWeekDays(int id)
        {
            IEnumerable<WeekDayCount> CompanyWeekData;
            CompanyWeekData = await _foodRepository.GetNumberOfWeekDays(id);
            return CompanyWeekData.ToList();
        }
        public async Task<List<WeekDayFoodType>> GetWeekDayFoodTypes()
        {
            IEnumerable<WeekDayFoodType> WeekAndFood;
            WeekAndFood = await _foodRepository.GetWeekDayFoodTypes();
            return WeekAndFood.ToList();
        }
        public async Task<ResponseModel<bool>> PostSubmitRequest(List<FoodRequest> foodRequest)
        {
            await _foodRepository.PostSubmitRequest(foodRequest);
            var res = new ResponseModel<bool>()
            {
                IsError = false,
                Message = "Request Sent Successfully",
                Data = true
            };
            return res;
        }
        public async Task<ResponseModel<bool>> SaveFoodTypeWeekDaysOrderByCompany(CompanyFoodTypeOrderPerWeek foodOrder)
        {
            string message = string.Empty;
            if (foodOrder.CompanyId == 0)
                message = "Company ID is required " + Environment.NewLine;

            //if (foodOrder.CompanyFoodTypesOrder == null || !foodOrder.CompanyFoodTypesOrder.Any(s => s.Status == true))
            //    message += "Food type with status 'True' is required " + Environment.NewLine;

            //if (foodOrder.CompanyFoodWeekDaysOrder == null || !foodOrder.CompanyFoodWeekDaysOrder.Any(s => s.Status == true))
            //    message += "weekday with status 'True' is required ";

            if (foodOrder.CompanyFoodTypesOrder == null)
                message += "Food type is required " + Environment.NewLine;

            if (foodOrder.CompanyFoodWeekDaysOrder == null)
                message += "Weekday is required ";

            AvailableFoodTypeWeekDays availableFoodTypeWeeksDays = await FoodTypeWeekDaysList();
            IEnumerable<AvailableWeekDay> availableWeekDays = availableFoodTypeWeeksDays.AvailableWeekDays;
            IEnumerable<WeekDayFoodType> weekDaysOrder = foodOrder.CompanyFoodWeekDaysOrder;
            bool IsListTypeError = false;
            foreach (WeekDayFoodType orderedWeekDays in weekDaysOrder)
            {
                if (orderedWeekDays.Status)
                {
                    if (!availableWeekDays.Any(s => s.WeekDayID == orderedWeekDays.WeekDayId))
                    {
                        IsListTypeError = true;
                    }
                }
            }
            IEnumerable<FoodType> foodTypeOrder = foodOrder.CompanyFoodTypesOrder;
            IEnumerable<AvailableFoodType> availablefoodTypes = availableFoodTypeWeeksDays.AvailableFoodTypes;
            foreach (FoodType orderedFooodTypes in foodTypeOrder)
            {
                if (orderedFooodTypes.Status)
                {
                    if (!availablefoodTypes.Any(s => s.FoodtypeID == orderedFooodTypes.FoodtypeID))
                    {
                        IsListTypeError = true;
                    }
                }
            }
            if (IsListTypeError)
            {
                message = "Please check value provided for the field for food type and Weekday";
            }
            if (string.IsNullOrEmpty(message))
                await _foodRepository.SaveFoodTypeWeekDaysOrder(foodOrder);


            bool isError = string.IsNullOrEmpty(message) ? false : true;
            var res = new ResponseModel<bool>()
            {
                IsError = isError,
                Message = isError ? message : "Food Order saved Successfully",
                Data = !isError
            };
            return res;
        }

        public async Task<AvailableFoodTypeWeekDays> FoodTypeWeekDaysList()
        {
            return await _foodRepository.FoodTypeWeekDaysList();
        }
        private async Task AppendBlobTxt(string jsonStr)
        {
            var containername = config.GetValue<string>("EmailSettings:foodlistcontainername");
            var filename = config.GetValue<string>("EmailSettings:filenamefoodlists");
            var STORAGE_ACCOUNT_NAME = config.GetValue<string>("EmailSettings:STORAGE_ACCOUNT_NAME");
            var STORAGE_PRIMARY_KEY = config.GetValue<string>("EmailSettings:STORAGE_PRIMARY_KEY");

            StorageCredentials creds = new StorageCredentials(STORAGE_ACCOUNT_NAME, STORAGE_PRIMARY_KEY);
            CloudStorageAccount strAcc = new CloudStorageAccount(creds, true);
            CloudBlobClient serviceClient = strAcc.CreateCloudBlobClient();
            CloudBlobContainer container = serviceClient.GetContainerReference(containername);

            await container.CreateIfNotExistsAsync();

            if (await container.ExistsAsync())
            {
                CloudBlob file = container.GetBlobReference(filename);
                if (await file.ExistsAsync())
                {
                    CloudAppendBlob appBlob = container.GetAppendBlobReference(filename);
                    await appBlob.CreateOrReplaceAsync();
                    //jsonStr = "[" + jsonStr + "]";

                    var appendFileStream = GenerateStreamFromString(jsonStr);
                    await appBlob.AppendFromStreamAsync(appendFileStream);
                }
            }
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
        public async Task<ResponseModel<IEnumerable<FoodMenu>>> UploadFoodMenu(IFormFile file)
        {
            string message = string.Empty;
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            if (file.Length > 0)
            {
                await AppendBlobTxt(result.ToString());
                message = "DATA UPDATED SUCCESSFULLY";
            }
            else
                message = "Selected file is empty.";

            ResponseModel<IEnumerable<FoodMenu>> response = new ResponseModel<IEnumerable<FoodMenu>>()
            {
                Message = message
            };

            return response;
        }
        public async Task<FileData> DownloadFoodMenu()
        {
            var storageConnectionString = config.GetValue<string>("EmailSettings:StorageConnectionString");
            var containername = config.GetValue<string>("EmailSettings:foodlistcontainername");
            var filename = config.GetValue<string>("EmailSettings:filenamefoodlists");

            BlobContainerClient blobContainerClient = new BlobContainerClient(storageConnectionString, containername);
            BlobClient blobClient = new BlobClient(storageConnectionString, containername, filename);
            if (await blobContainerClient.ExistsAsync())
            {
                if (await blobClient.ExistsAsync())
                {
                    using (var memStream = new MemoryStream())
                    {
                        await blobClient.DownloadToAsync(memStream);
                        memStream.Position = 0;
                        var contentType = (await blobClient.GetPropertiesAsync()).Value.ContentType;
                        return new FileData
                        {
                            FileStream = memStream.ToArray(),
                            FileContentType = contentType,
                            FileName = blobClient.Name
                        };
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        private void AddToFoodMenuList(List<FoodMenu> foodMenuList, DataRow detailsRow, FoodMenu foodMenu, int index)
        {
            if (detailsRow[index] != null && detailsRow[index].ToString() != "")
            {
                foodMenu.FoodTypeId = 1;
                foodMenu.FoodName = detailsRow[index].ToString();
            }
            foodMenuList.Add(new FoodMenu()
            {
                FoodName = foodMenu.FoodName,
                FoodTypeId = foodMenu.FoodTypeId,
                WeekDayId = foodMenu.WeekDayId,
                ErrorMessage = foodMenu.ErrorMessage
            });
        }
    }
}
