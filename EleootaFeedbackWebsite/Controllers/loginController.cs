using EleOota.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Models;
using System.Threading.Tasks;
using System;
using EleOota.Models.Common;
using System.Collections.Generic;

namespace EleOota.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }
        
        [Route("SendOtp")]
        [HttpPost]
        public async Task<ResponseModel<bool>> SendOtp(EmailParam emailParam)
        {
            return await _loginService.SendOtp(emailParam);
        }
        
        [Route("VerifyOTP")]
        [HttpPost]
        public async Task<ActionResult<VerifyParam>> VerifyOtp(LoginParam param)
        {
            return await _loginService.VerifyOtp(param);
        }
        [Route("DataExists")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel<bool>>> DataExists(ExistingData param)
        {
            return await _loginService.ExistingData(param);
        }
    }
}
