using EleOota.Models.Common;
using EleOota.Service.Interface;
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
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService ?? throw new ArgumentNullException(nameof(foodService));
        }

        [Route("GetFoodList")]
        [HttpGet]
        public async Task<ActionResult<List<FoodType>>> GetAllFoodTypes(int id)
        {
            return await _foodService.GetAllFoodTypes(id);
        }

        [Route("GetNumberOfWeekDays")]
        [HttpGet]
        public async Task<ActionResult<List<WeekDayCount>>> GetNumberOfWeekDays(int id)
        {
            return await _foodService.GetNumberOfWeekDays(id);
        }

        [Route("GetWeekDayAndFoodType")]
        [HttpGet]
        public async Task<ActionResult<List<WeekDayFoodType>>> GetWeekDayFoodTypes()
        {
            return await _foodService.GetWeekDayFoodTypes();
        }
        [Route("AddSubmitRequest")]
        [HttpPost]
        public async Task<ResponseModel<bool>> PostSubmitRequest(List<FoodRequest> foodRequest)
        {
            return await _foodService.PostSubmitRequest(foodRequest);
        }

        [Route("GetCompanyList")]
        [HttpGet]
        public async Task<List<Company>> GetCompanyList()
        {
            return await _foodService.GetCompanyList();
        }

        [Route("GetCompanyById")]
        [HttpGet]
        public async  Task<ActionResult<Company>> GetCompanyById(int id)
        {
            return await _foodService.GetCompanyById(id);
        }

        [Route("GetAllEmployees")]
        [HttpGet]
        public async  Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            return await _foodService.GetAllEmployees();
        }

        [Route("GetEmployeeById")]
        [HttpGet]
        public async  Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            return await _foodService.GetEmployeeById(id);
        } 
        [Route("GetFoodRequestByEmployeeId")]
        [HttpGet]
        public async Task<ActionResult<FoodRequest>> GetFoodRequestByEmployeeId(int id)
        {
            return await _foodService.GetFoodRequestByEmployeeId(id);
        }
        [Route("FoodTypeWeekdayList")]
        [HttpGet]
        public async Task<ActionResult<AvailableFoodTypeWeekDays>> AvailableFoodTypeWeekDays()
        {
            return await _foodService.FoodTypeWeekDaysList();
        }
        [Route("FoodTypeByWeekDaysOrder")]
        [HttpPost]
        public async Task<ResponseModel<bool>> SaveFoodTypeWeekDaysOrderByCompany(CompanyFoodTypeOrderPerWeek foodRequestbyCompany)
        {
            return await _foodService.SaveFoodTypeWeekDaysOrderByCompany(foodRequestbyCompany);
        }
        [Route("UploadFood")]
        [HttpPost, DisableRequestSizeLimit]

        public async Task<ResponseModel<IEnumerable<FoodMenu>>> UploadFood()
        {
            if (Request.HasFormContentType)
            {
                IFormFile file = Request.Form.Files[0];
                return await _foodService.UploadFoodMenu(file);
            }
            else
            {
                return new ResponseModel<IEnumerable<FoodMenu>> ()
                {
                    IsError = true,
                    Message = "Pleas upload employee excel",
                    Data = null
                };
            }
        }

        [HttpGet]
        [Route("DownloadFoodMenu")]
        public async Task<IActionResult> DownloadFoodMenu()
        {
            var fileData = await _foodService.DownloadFoodMenu();
            return File(fileData.FileStream, fileData.FileContentType, fileData.FileName);
        }

    }
}
