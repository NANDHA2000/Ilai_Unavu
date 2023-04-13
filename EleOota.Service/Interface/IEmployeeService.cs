using EleOota.Models.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Interface
{
    public interface IEmployeeService
    {
        Task<ResponseModel<IEnumerable<Employee>>> GetAllEmployees(Employee employeeReq);
        Task<Employee> GetEmployeeById(int id);
        Task<FoodRequest> GetFoodRequestByEmployeeId(int id);
        Task<ResponseModel<bool>> ModifyEmployee(Employee employeeRequest);
        Task<ResponseModel<EmployeeUpload>> UploadEmployee(IFormFile file, int companyId);
        Task<ResponseModel<bool>> DeleteEmployeeById(int Id);
    }
}
