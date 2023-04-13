using EleOota.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Interface
{
    public interface IFoodService
    {
        Task<List<FoodType>> GetAllFoodTypes(int id);
        Task<List<WeekDayCount>> GetNumberOfWeekDays(int id);
        Task<List<WeekDayFoodType>> GetWeekDayFoodTypes();
        Task<ResponseModel<bool>> PostSubmitRequest(List<FoodRequest> foodRequest);
        Task<List<Employee>> GetAllEmployees();
        Task<Employee> GetEmployeeById(int id);
        Task<FoodRequest> GetFoodRequestByEmployeeId(int id);
        Task<List<Company>> GetCompanyList();
        Task<Company> GetCompanyById(int id);
        Task<AvailableFoodTypeWeekDays> FoodTypeWeekDaysList();
        Task<ResponseModel<bool>> SaveFoodTypeWeekDaysOrderByCompany(CompanyFoodTypeOrderPerWeek foodRequest);
        Task<ResponseModel<IEnumerable<FoodMenu>>> UploadFoodMenu(IFormFile file);
        Task<FileData> DownloadFoodMenu();

    }
}
