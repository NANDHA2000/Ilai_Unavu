using EleOota.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Interface
{
    public interface ICompanyService
    {
       
        Task<List<Company>> GetCompaniesDetailsList(Company companyReq);
        Task<Company> GetCompanyById(int id);
        Task<ResponseModel<bool>> AddCompany(Company company);
        Task<ResponseModel<bool>> ModifyCompany(Company companyRequest);
        Task<CompanyFoodTypeOrderPerWeek> FoodTypePerWeekDaysByCopmany(int id);
        Task<ResponseModel<IEnumerable<CompanyFoodOrder>>> CompanyFoodCount(string companyName);
        Task<ResponseModel<IEnumerable<CompanyFoodOrder>>> CompanyFoodCount(string companyName, bool isNextWeek);
        Task<ResponseModel<bool>> ChangeCompanyStatus(Company companyRequest);
    }
}
