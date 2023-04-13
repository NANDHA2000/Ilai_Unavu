using EleOota.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Repository.Interface
{
    public interface IFoodRepository
    {
        Task<IEnumerable<FoodType>> GetAllFoodTypes(int id);
        Task<IEnumerable<WeekDayCount>> GetNumberOfWeekDays(int id);
        Task<IEnumerable<WeekDayFoodType>> GetWeekDayFoodTypes();
        Task<bool> PostSubmitRequest(List<FoodRequest> foodRequest);
        Task<IEnumerable<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<FoodRequest> GetFoodRequestByEmployeeId(int id);
        Task<IEnumerable<Company>> GetCompanyList();
        Task<Company> GetCompanyById(int id);
        Task<AvailableFoodTypeWeekDays> FoodTypeWeekDaysList();
        Task<bool> SaveFoodTypeWeekDaysOrder(CompanyFoodTypeOrderPerWeek foodTypeWeekDaysOrder);
        Task<bool> SaveFoodMenu(List<FoodMenu> foodMenuList);
    }
}
