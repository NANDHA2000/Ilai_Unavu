using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using static System.Net.WebRequestMethods;

namespace AutogenerationMail
{
    public class AutoMailCompany
    {
        [FunctionName("AutoMailCompany")]
        [return: SendGrid(ApiKey = "SendGridApiKey")]
        public async Task Run([TimerTrigger("0 30 4 * * Sun")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var sqlConnection = "Server=kalpita.database.windows.net;Database=eleoota;User id=eleoota; password=&Pg$oeV7NbW4L$;MultipleActiveResultSets=true";
            //var sqlConnection = "Server=kalpita.database.windows.net;Database=eleoota_dev;User id=eleoota_dev; password=^ooQnN^K$4Jwzf;MultipleActiveResultSets=true";
            DataTable table = new DataTable();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(sqlConnection);
            con.Open(); 
            SqlCommand sqlcommand = new SqlCommand("Sp_Company", con);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcommand);
            da.Fill(ds);
            table = ds.Tables[0];
            con.Close();

            foreach (DataRow row in table.Rows)
            { 
                DataTable tables = new DataTable();
                DataSet dss = new DataSet();
                SqlConnection conn = new SqlConnection(sqlConnection);
                con.Open();
                SqlCommand totalcount = new SqlCommand("Sp_CompanyFoodCount", conn);
                totalcount.CommandType = CommandType.StoredProcedure;
                totalcount.Parameters.AddWithValue("CompanyName",row.Field<string>("CompanyName"));
                SqlDataAdapter sqlData = new SqlDataAdapter(totalcount);
                sqlData.Fill(dss);
                tables = dss.Tables[0];
                var CompanyName = string.Empty;
                var breakfast = "0";
                var lunch = "0";
                var potLuck = "0";
                var dinner = "0";
                var weekdayName = string.Empty;
                con.Close();

                if (ds.Tables[0].Rows.Count >= 1)
                {
                    var opentrs = "<tr>";
                    var closetrs = "</tr>";
                    var opentds = "<td style='border:1pt solid rgb(165, 165, 165)'>";
                    var closetds = "</td>";
                    string totalDetails = "";

                    foreach (DataRow d in dss.Tables[0].Rows)
                    {
                        CompanyName = d["CompanyName"].ToString();
                        weekdayName = d["WeekdayName"].ToString();
                        breakfast = d["Breakfast"].ToString();
                        lunch = d["Lunch"].ToString();
                        potLuck = d["PotLuck"].ToString();
                        dinner = d["Dinner"].ToString();


                        totalDetails += opentrs;
                        totalDetails += opentds + CompanyName + closetds;
                        totalDetails += opentds + weekdayName + closetds;
                        totalDetails += opentds + breakfast + closetds;
                        totalDetails += opentds + lunch + closetds;
                        totalDetails += opentds + potLuck + closetds;
                        totalDetails += opentds + dinner + closetds;
                        totalDetails += closetrs;
                    }

                    var client = new SendGridClient("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");
                    var from = new EmailAddress("no-reply@kalpitatechnologies.com", "EleOota");
                    var subject = "Individual Company's Weekly Count";
                    var to = new EmailAddress(row.Field<string>("CompanyMail"));
                    var plainTextContent = "Test";
                    var mailBody = "Hi Team, please find the below details for this week's food count by Employees<br>" +
                        "<table style='border-collapse:collapse;width:100%;text-align:center'>" +
                        "<tr style='font-size:13px;background-color:#378737;color:white;width:100%'>" +

                        "<th style='border:1pt solid rgb(165, 165, 165)'>Company Name</th>" +
                        "<th style='border:1pt solid rgb(165, 165, 165)'>WeekdayName</th>" +
                        "<th style='border:1pt solid rgb(165, 165, 165)'>Breakfast</th>" +
                        "<th style='border:1pt solid rgb(165, 165, 165)'>Lunch</th>" +
                        "<th style='border:1pt solid rgb(165, 165, 165)'>PotLuck</th>" +
                        "<th style='border:1pt solid rgb(165, 165, 165)'>Dinner</th></tr>" +
                        totalDetails +
                        "</table>";
                    var htmlContent = mailBody;
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    await client.SendEmailAsync(msg);
                }
            }
        }
    }
}