using EleOota.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Repository.Interface
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesDetailsList(Company companyReq);
        Task<Company> GetCompanyById(int id);
        Task<bool> AddComapny(Company company);
        Task<bool> ModifyCompany(Company company);
        Task<CompanyFoodTypeOrderPerWeek> FoodTypePerWeekDaysByCopmany(int id);
        Task<IEnumerable<CompanyFoodOrder>> CompanyFoodCount(string companyName);
        Task<IEnumerable<CompanyFoodOrder>> CompanyFoodCount(string companyName, bool isNextWeek);
    }
}
