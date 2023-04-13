using EleOota.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<bool> AddEmployee(Employee employee);
        Task<IEnumerable<Employee>> GetAllEmployees(Employee employee);
        Task<Employee> GetEmployeeById(int id);
        Task<FoodRequest> GetFoodRequestByEmployeeId(int id);
        Task<bool> ModifyEmployee(Employee employee);
        Task<bool> DeleteEmployeeById(int Id);
    }
}
