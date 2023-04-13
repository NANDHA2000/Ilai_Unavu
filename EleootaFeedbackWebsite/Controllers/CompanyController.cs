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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService ?? throw new ArgumentNullException(nameof(companyService));
        }


        [Route("GetCompanyList")]
        [HttpGet]
        public async Task<List<Company>> GetCompanyDetailsList([FromQuery] Company company)
        {
            return await _companyService.GetCompaniesDetailsList(company);
        }


        [Route("GetTest")]
        [HttpGet]
        public string GetTest()
        {
            return "Testing";
        }
        //[Route("GetCompanyById")]
        //[HttpGet]
        //public async Task<ActionResult<Company>> GetCompanyById(int id)
        //{
        //    return await _companyService.GetCompanyById(id);
        //}
        [Route("AddCompany")]
        [HttpPost]
        public async Task<ResponseModel<bool>> AddCompany(Company company)
        {

            return await _companyService.AddCompany(company);
        }


        [Route("ModifyCompany")]
        [HttpPut]
        public async Task<ResponseModel<bool>> ModifyCompany(Company company)
        {
            return await _companyService.ModifyCompany(company);
        }

        [Route("ModifyCompanyStatus")]
        [HttpPatch]
        public async Task<ResponseModel<bool>> ModifyCompanyStatus(Company company)
        {
            return await _companyService.ChangeCompanyStatus(company);
        }
        [Route("ViewFoodRequest")]
        [HttpGet]
        public async Task<ActionResult<CompanyFoodTypeOrderPerWeek>> ViewFoodRequestCompanyID(int id)
        {
            return await _companyService.FoodTypePerWeekDaysByCopmany(id);
        }

        [Route("CompanyFoodCount")]
        [HttpGet]
        public async Task<ResponseModel<IEnumerable<CompanyFoodOrder>>> CompanyFoodCount(string companyName)
        {
            return await _companyService.CompanyFoodCount(companyName);
        }

        [Route("CompanyFoodCountByWeek")]
        [HttpGet]
        public async Task<ResponseModel<IEnumerable<CompanyFoodOrder>>> CompanyFoodCount(string companyName, bool isNextWeek)
        {
            return await _companyService.CompanyFoodCount(companyName, isNextWeek);
        }
    }
}
