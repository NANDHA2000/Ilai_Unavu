using EleOota.Models.Common;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Interface
{
    public interface ILoginService
    {
        Task<ResponseModel<bool>> SendOtp(EmailParam Email);
        Task<VerifyParam> VerifyOtp(LoginParam login);
        Task<ResponseModel<bool>> ExistingData(ExistingData login);
        Task<ResponseModel<bool>> VerifyFeedbackUser(EmailParam Email);
    }
}