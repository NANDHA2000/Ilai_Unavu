using EleOota.Framework.Models.Settings;
using EleOota.Repository.Helpers;
using EleOota.Repository.Infrastructure.Interface;
using EleOota.Repository.Interface;
using Microsoft.Extensions.Options;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EleOota.Repository.Implementation
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {

        private const string IdQuery = "@emailOTP";

        public LoginRepository(
            IOptions<DatabaseAdvancedSettingsOptions> settingsOptions,
                       IQueryBuilder queryBuilder,
            IUnitOfWork unitOfWork) : base(settingsOptions, queryBuilder, unitOfWork,TableNames.OTP,IdQuery)
        {



        }
        public async Task<SendOtpResponse> SendOtp(EmailParam Email, string otp)
        {
            var SubmitData = new
            {
                email = Email.Email,
                Otp = otp    
            };
            var data = await QueryProcedureAsync<SendOtpResponse>("Sp_loginform", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }
        public async Task<VerifyParam> VerifyOTP(LoginParam login)
        {
            var SubmitData = new
            {
                email = login.Email,
                Otp = login.Otp
            };
            var data = await QueryProcedureAsync<VerifyParam>("Sp_loginVerify", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }
        public async Task<ExistingDataResponse> DataExists(ExistingData Submit)
        {
            var SubmitData = new
            {
                CurrentDate = Submit.CurrentDate,
                EmployeeId = Submit.Empid
            };
            var data = await QueryProcedureAsync<ExistingDataResponse>("SP_SubmitExists", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }

        public async Task<User> VerifyUser(User user)
        {
            var SubmitData = new
            {
                UserMail = user.Email,
                Status=user.UserStaus
            };
            var data = await QueryProcedureAsync<User>("usp_GetUsers", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }
        public async Task<ExistingDataResponse> VerifyFeedbackUser(User user)
        {
            var SubmitData = new
            {
                UserMail = user.Email,
                Status = user.UserStaus
            };
            var data = await QueryProcedureAsync<ExistingDataResponse>("usp_VerifyFeedbackUser", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }
    }
}
