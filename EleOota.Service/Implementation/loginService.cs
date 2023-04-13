using EleOota.Models.Common;
using EleOota.Repository.Interface;
using EleOota.Service.Interface;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IEmailServices _emailService;
        private readonly ILoginRepository _loginRepository;
        public LoginService(IEmailServices emailService, ILoginRepository loginRepository)
        {
            _emailService = emailService;
            _loginRepository = loginRepository ?? throw new ArgumentNullException(nameof(Service));
        }
        public async Task<ResponseModel<bool>> SendOtp(EmailParam Email)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(Email.Email))
            {
                message = "Email is required";
            }
            User user = new User()
            {
                Email = Email.Email,
                UserStaus = 1
            };
            SendOtpResponse data = new SendOtpResponse();
            User login = await _loginRepository.VerifyUser(user);
            if (login == null || login.UserId == 0)
            {
                message = "Invalid or Inactive User. Please contact administrator regarding the issue";
            }
            else
            {
                Random generator = new Random();
                string otp = generator.Next(0, 1000000).ToString("D6");
                await _emailService.SendEmail(Email, otp);
                data = await _loginRepository.SendOtp(Email, otp);
            }
            try
            {
                if (string.IsNullOrEmpty(message) && data.IsExists)
                {
                    return new ResponseModel<bool>()
                    {
                        IsError = false,
                        Message = "OTP Sent Successfully",
                        Data = true,
                        Empid = data.Empid
                    };
                }
                else
                {
                    return new ResponseModel<bool>()
                    {
                        IsError = true,
                        Message = message,
                        Data = false,
                        Empid = data.Empid
                    };
                }
            }
            catch (Exception)
            {
                return new ResponseModel<bool>()
                {
                    IsError = true,
                    Message = "please contact the administrator regarding the issue",
                    Data = false
                };
            }
        }

        public async Task<VerifyParam> VerifyOtp(LoginParam login)
        {
            return await _loginRepository.VerifyOTP(login);
        }
        public async Task<ResponseModel<bool>> ExistingData(ExistingData login)
        {
            var data = await _loginRepository.DataExists(login);
            if (data.IsExists)
            {
                return new ResponseModel<bool>()
                {
                    IsError = false,
                    Message = "Data Already exists",
                    Data = true
                };
            }
            else
            {
                return new ResponseModel<bool>()
                {
                    IsError = true,
                    Message = "Data doesn't exists",
                    Data = false
                };
            }
        }

        public async Task<ResponseModel<bool>> VerifyFeedbackUser(EmailParam Email)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(Email.Email))
            {
                message = "Email is required";
            }
            User user = new User()
            {
                Email = Email.Email,
                UserStaus = 1
            };

            ResponseModel<bool> res;

            bool IsExists = (await _loginRepository.VerifyFeedbackUser(user)).IsExists;
            if (IsExists)
            {
                res = new ResponseModel<bool>()
                {
                    IsError = !IsExists,
                    Message = "User has an existing email or domain",
                    Data = IsExists,
                    Empid = 0
                };                
            }
            else
            {
                res = new ResponseModel<bool>()
                {
                    IsError = !IsExists,
                    Message = "Invalid or Inactive User. Please contact administrator regarding the issue",
                    Data = IsExists,
                    Empid = 0
                };
            }
            return res;
        }
    }
}