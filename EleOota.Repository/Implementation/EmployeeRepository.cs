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
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        private const string IdQuery = "@FoodType";


        public EmployeeRepository(
            IOptions<DatabaseAdvancedSettingsOptions> settingsOptions,
                       IQueryBuilder queryBuilder,
            IUnitOfWork unitOfWork) : base(settingsOptions, queryBuilder, unitOfWork, TableNames.FoodType, IdQuery)
        {

        }

        public async Task<IEnumerable<Employee>> GetAllEmployees(Employee employee)
        {
            return await QueryProcedureAsync<Employee>("Sp_GetAllEmployees", employee);
        }
        public async Task<Employee> GetEmployeeById(int id)
        {
            var SubmitData = new
            {
                id = id,
            };
            var data = await QueryProcedureAsync<Employee>("Sp_GetAllEmployeeById", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }
        public async Task<FoodRequest> GetFoodRequestByEmployeeId(int id)
        {
            var SubmitData = new
            {
                id = id,
            };
            var data = await QueryProcedureAsync<FoodRequest>("Sp_GetFoodRequestByEmployeeId", SubmitData);
            if (data.Count() > 0)
                return data.FirstOrDefault();
            else return null;
        }
        public async Task<bool> AddEmployee(Employee employee)
        {
            await QueryProcedureAsync<Employee>("usp_insertemployee", employee);
            return true;
        }

        public async Task<bool> ModifyEmployee(Employee employee)
        {
            var SubmitData = new
            {
                Empid = employee.Empid,
                EmpName = employee.EmpName,
                EmpMail = employee.EmpMail,
                StatusId = employee.StatusId
            };
            await QueryProcedureAsync<Employee>("usp_UpdateEmployee", SubmitData);
            return true;
        }
        public async Task<bool> DeleteEmployeeById(int Id)
        {
            var SubmitData = new
            {
                EmployeeId = Id
            };
            await QueryProcedureAsync<EmployeeRaw>("usp_DeleteEmployeeById", SubmitData);
            return true;
        }
    }

}


