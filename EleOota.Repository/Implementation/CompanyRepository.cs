using EleOota.Framework.Models.Settings;
using EleOota.Models.Common;
using EleOota.Repository.Helpers;
using EleOota.Repository.Infrastructure.Interface;
using EleOota.Repository.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;

namespace EleOota.Repository.Implementation
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        private const string IdQuery = "@FoodType";


        public CompanyRepository(
            IOptions<DatabaseAdvancedSettingsOptions> settingsOptions,
                       IQueryBuilder queryBuilder,
            IUnitOfWork unitOfWork) : base(settingsOptions, queryBuilder, unitOfWork, TableNames.FoodType, IdQuery)
        {

        }

        public async Task<IEnumerable<Company>> GetCompaniesDetailsList(Company companyReq)
        {
            return await QueryProcedureAsync<Company>("usp_GetCompaniesDetailsList", companyReq);
        }
        public async Task<Company> GetCompanyById(int id)
        {
            var SubmitData = new
            {
                id = id,
            };
            var data = await QueryProcedureAsync<Company>("Sp_GetCompanyById", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }


        public async Task<bool> AddComapny(Company company)
        {
            await QueryProcedureAsync<Company>("usp_insertCompany", company);
            return true;
        }
        public async Task<bool> ModifyCompany(Company company)
        {

            await QueryProcedureAsync<Company>("usp_updateCompany", company);
            return true;
        }
        public async Task<IEnumerable<CompanyFoodOrder>> CompanyFoodCount(string companyName)
        {
            var submitData = new
            {
                CompanyName = companyName
            };
            IEnumerable<CompanyFoodOrder> weekDayFoodTypes = await QueryProcedureAsync<CompanyFoodOrder>("Sp_CompanyFoodCount", submitData);
            return weekDayFoodTypes;
        }

        public async Task<IEnumerable<CompanyFoodOrder>> CompanyFoodCount(string companyName, bool isNextWeek)
        {
            var submitData = new
            {
                CompanyName = companyName,
                IsNextWeek = isNextWeek
            };
            IEnumerable<CompanyFoodOrder> weekDayFoodTypes = await QueryProcedureAsync<CompanyFoodOrder>("Sp_CompanyFoodCountByRequestWeek", submitData);
            return weekDayFoodTypes;
        }

        public async Task<CompanyFoodTypeOrderPerWeek> FoodTypePerWeekDaysByCopmany(int id)
        {
            //string compId = id.ToString();
            var SubmitData = new
            {
                compId = id
            };
            IEnumerable<WeekDayFoodType> companyWeekType = await QueryProcedureAsync<WeekDayFoodType>("usp_CompanyFoodWeekDay", SubmitData);
            IEnumerable<FoodType> companyFoodType = await QueryProcedureAsync<FoodType>("usp_CompanyFoodType", SubmitData);
            CompanyFoodTypeOrderPerWeek foodTypePerWeekDays = new CompanyFoodTypeOrderPerWeek
            {
                CompanyFoodTypesOrder = companyFoodType,
                CompanyFoodWeekDaysOrder = companyWeekType
            };

            //if (data.Count() > 0)
            //    return data.FirstOrDefault();else
            return foodTypePerWeekDays;
        }
    }
}



