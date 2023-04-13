using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Repository.Interface
{
    public interface ILoginRepository
    {
        Task<SendOtpResponse> SendOtp(EmailParam Email, string otp);
        Task<VerifyParam> VerifyOTP(LoginParam login);
        Task<ExistingDataResponse> DataExists(ExistingData Submit);
        Task<User> VerifyUser(User user);
        Task<ExistingDataResponse> VerifyFeedbackUser(User user);
    }
}
