using EleOota.Service.Interface;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EleOota.Models.Common;

namespace EleOota.Service.Implementation
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailConfig _emailConfig;
        public EmailServices(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }
        public async Task<ResponseModel<bool>> SendEmail(EmailParam Email, string otp)
        {
            try
            {
                var client = new SendGridClient(_emailConfig.ApiKey);
                var from = new EmailAddress(_emailConfig.FromEmail, "EleOota");
                var subject = "Eleoota OTP Verification";
                var to = new EmailAddress(Email.Email);
                var plainTextContent = "Test";
                var mailBody = " Please use the below OTP for Verification" +
                    "<br><br>OTP :" + otp ;
                var htmlContent = mailBody;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.IsSuccessStatusCode)
                {
                    return new ResponseModel<bool>()
                    {
                        IsError = false,
                        Message = "Email sent successfully",
                        Data = true
                    };
                }
                else
                {
                    return new ResponseModel<bool>()
                    {
                        IsError = true,
                        Message = "Error while sending Email",
                        Data = false
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseModel<bool>()
                {
                    IsError = true,
                    Message = "Error while sending Email",
                    Data = false
                };
            }
        }

      
    }
}
