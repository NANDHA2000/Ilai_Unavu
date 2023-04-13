using EleOota.Models.Common;
using EleOota.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedBackSendMail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        }


        [Route("GetEmployeeById")]
        [HttpGet]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            return await _employeeService.GetEmployeeById(id);
        }
        [Route("GetFoodRequestByEmployeeId")]
        [HttpGet]
        public async Task<ActionResult<FoodRequest>> GetFoodRequestByEmployeeId(int id)
        {
            return await _employeeService.GetFoodRequestByEmployeeId(id);
        }
        //public async Task<ActionResult<FoodRequest>> UploadEmployee()
        //{
        //    return await _foodService.GetFoodRequestByEmployeeId(id);
        //}
        [Route("UploadEmployee")]
        [HttpPost, DisableRequestSizeLimit]

        public async Task<ResponseModel<EmployeeUpload>> UploadEmployee(int companyID)
        {
            if (Request.HasFormContentType)
            {
                IFormFile file = Request.Form.Files[0];
                return await _employeeService.UploadEmployee(file, companyID);
            }
            else
            {
                return new ResponseModel<EmployeeUpload>()
                {
                    IsError = true,
                    Message = "Please upload employee excel",
                    Data = null
                };
            }
        }

        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<ResponseModel<IEnumerable<Employee>>> GetAllEmployees(int companyId)
        {
            Employee employeeReq = new Employee()
            {
                CompanyId = companyId
            };
            return await _employeeService.GetAllEmployees(employeeReq);
        }

        [Route("ModifyEmployee")]
        [HttpPut]
        public async Task<ResponseModel<bool>> ModifyEmployee(Employee employee)
        {
            return await _employeeService.ModifyEmployee(employee);
        }
        [Route("DeleteEmployeeById")]
        [HttpDelete]
        public async Task<ResponseModel<bool>> DeleteEmployeeById(int employeeId)
        {            
            return await _employeeService.DeleteEmployeeById(employeeId);
        }
    }
}
