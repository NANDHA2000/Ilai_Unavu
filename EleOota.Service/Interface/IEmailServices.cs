using EleOota.Models.Common;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Interface
{
    public interface IEmailServices
    {
        Task<ResponseModel<bool>> SendEmail(EmailParam Email, string otp);
    }
}
