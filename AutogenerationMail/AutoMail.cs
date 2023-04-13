using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace AutogenerationMail
{
    public class AutoMail
    {
        [FunctionName("AutoMail")]
        [return: SendGrid(ApiKey = "SendGridApiKey")]
        public SendGridMessage Run([TimerTrigger("0 30 4 * * Sun")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var sqlConnection = "Server=kalpita.database.windows.net;Database=eleoota;User id=eleoota; password=&Pg$oeV7NbW4L$;MultipleActiveResultSets=true";
            //var sqlConnection = "Server=kalpita.database.windows.net;Database=eleoota_dev;User id=eleoota_dev; password=^ooQnN^K$4Jwzf;MultipleActiveResultSets=true";
            DataTable table = new DataTable();
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(sqlConnection);
            con.Open();
            SqlCommand sqlcommand = new SqlCommand("Sp_WeeklyCount", con);
            sqlcommand.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(sqlcommand);
            da.Fill(ds);
            table = ds.Tables[0];
            var companyName = string.Empty;
            var Breakfast = "0";
            var Lunch = "0";
            var PotLuck = "0";
            var Dinner = "0";
            var WeekdayName = string.Empty;
            con.Close();

            DataTable tables = new DataTable();
            DataSet dss = new DataSet();
            SqlConnection conn = new SqlConnection(sqlConnection);
            con.Open();
            SqlCommand totalcount = new SqlCommand("Sp_TotalCount", conn);
            totalcount.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sqlData = new SqlDataAdapter(totalcount);
            sqlData.Fill(dss);
            tables = dss.Tables[0];
            var breakfast = "0";
            var lunch = "0";
            var potLuck = "0";
            var dinner = "0";
            var weekdayName = string.Empty;
            con.Close();

            var msg = new SendGridMessage();
            if (ds.Tables[0].Rows.Count >= 1)
            {
                var opentr = "<tr>";
                var closetr = "</tr>";
                var opentd = "<td style='border:1pt solid rgb(165, 165, 165)'>";
                var closetd = "</td>";

                var opentrs = "<tr>";
                var closetrs = "</tr>";
                var opentds = "<td style='border:1pt solid rgb(165, 165, 165)'>";
                var closetds = "</td>";

                string details = "";
                string totalDetails = "";

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    companyName = dr["CompanyName"].ToString();
                    Breakfast = dr["Breakfast"].ToString();
                    Lunch = dr["Lunch"].ToString();
                    PotLuck = dr["PotLuck"].ToString();
                    Dinner = dr["Dinner"].ToString();
                    WeekdayName = dr["WeekdayName"].ToString();

                    details += opentr;
                    details += opentd + companyName + closetd;
                    details += opentd + Breakfast + closetd;
                    details += opentd + Lunch + closetd;
                    details += opentd + PotLuck + closetd;
                    details += opentd + Dinner + closetd;
                    details += opentd + WeekdayName + closetd;
                    details += closetr;
                }

                foreach (DataRow d in dss.Tables[0].Rows)
                {
                    weekdayName = d["WeekdayName"].ToString();
                    breakfast = d["Breakfast"].ToString();
                    lunch = d["Lunch"].ToString();
                    potLuck = d["PotLuck"].ToString();
                    dinner = d["Dinner"].ToString();


                    totalDetails += opentrs;
                    totalDetails += opentds + weekdayName + closetds;
                    totalDetails += opentds + breakfast + closetds;
                    totalDetails += opentds + lunch + closetds;
                    totalDetails += opentds + potLuck + closetds;
                    totalDetails += opentds + dinner + closetds;
                    totalDetails += closetrs;
                }

                msg = new SendGridMessage()
                {
                    From = new EmailAddress("no-reply@kalpitatechnologies.com", "EleOota"),
                    Subject = "Weekly food Record by Companies",
                    HtmlContent = "Hi Team, please find the below details from the companies for this week's food count<br>" +
                    "<table style='border-collapse:collapse;width:100%;text-align:center'>" +
                    "<tr style='font-size:13px;background-color:#378737;color:white;width:100%'>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>Company Name</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>Breakfast</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>Lunch</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>PotLuck</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>Dinner</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>WeekdayName</th></tr>" +
                    details +
                    "</table>" +

                    "<br>" +
                    "<br>" +

                    "<table style='border-collapse:collapse;width:100%;text-align:center'>" +
                    "<tr style='font-size:13px;background-color:#378737;color:white;width:100%'>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>weekdayName</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>breakfast</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>lunch</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>potLuck</th>" +
                    "<th style='border:1pt solid rgb(165, 165, 165)'>dinner</th></tr>" +
                    totalDetails +
                    "</table>"
                };
                msg.AddTo(new EmailAddress("sophia.shrine@kalpitatechnologies.com"));
            }
            else
            {
                msg = new SendGridMessage()
                {
                    From = new EmailAddress("no-reply@kalpitatechnologies.com", "EleOota"),
                    Subject = "Weekly food Record by Companies - No Count",
                    HtmlContent = "Hi Team, <br> There is no food request for this week."
                };
                msg.AddTo(new EmailAddress("sophia.shrine@kalpitatechnologies.com"));
            }
            return msg;
        }
    }
}