using EleOota.Models.Common;
using EleOota.Repository.Interface;
using EleOota.Service.Interface;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EleOota.Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyService _companyService;

        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyService companyService)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(_employeeRepository));
            _companyService = companyService ?? throw new ArgumentNullException(nameof(_companyService)); ;
        }

        public async Task<ResponseModel<IEnumerable<Employee>>> GetAllEmployees(Employee employeeReq)
        {
            string message = null;
            IEnumerable<Employee> employee = null;
            if (employeeReq.CompanyId == 0)
            {
                message = "Invalid copmany ID";
            }
            else
            {
                employee = await _employeeRepository.GetAllEmployees(employeeReq);
            }
            ResponseModel<IEnumerable<Employee>> response = new ResponseModel<IEnumerable<Employee>>()
            {
                IsError = string.IsNullOrEmpty(message) ? false : true,
                Message = message,
                Data = employee
            };
            return response;
        }


        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _employeeRepository.GetEmployeeById(id);
        }

        public async Task<FoodRequest> GetFoodRequestByEmployeeId(int id)
        {
            return await _employeeRepository.GetFoodRequestByEmployeeId(id);
        }


        public async Task<ResponseModel<EmployeeUpload>> UploadEmployee(IFormFile file, int companyId)
        {
            string message = string.Empty;
            message = await CheckEmployeeCompanyById(companyId);

            DataSet dsexcelRecords = new DataSet();
            ResponseModel<EmployeeUpload> response = null;
            string folderName = Path.Combine("Uploads", "Employee");
            string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo dir = new DirectoryInfo(pathToSave);
            string fileextension = Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')).ToLower();
            if (fileextension != ".xls" && fileextension != ".xlsx")
            {
                message = "Invalid file type. Upload excel file";
            }

            if (!dir.Exists)
            {
                dir.Create();
            }
            if (string.IsNullOrEmpty(message))
            {
                if (file.Length > 0)
                {
                    dsexcelRecords = ReadExcelToDs(file, pathToSave);
                    int uploadedEmployee = 0, failedEmployee = 0;
                    if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                    {
                        List<EmployeeUploadExcel> empUploadError = new List<EmployeeUploadExcel>();
                        DataTable dtEmplolyeeRecords = dsexcelRecords.Tables[0];
                        if (dtEmplolyeeRecords.Rows.Count <= 1)
                        {
                            message = "Invalid Excel file. column headers & Employee details  are required";
                        }
                        for (int i = 0; i < dtEmplolyeeRecords.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                if (Convert.ToString(dtEmplolyeeRecords.Rows[i][0]).ToUpper() != "NAME" ||
                                         Convert.ToString(dtEmplolyeeRecords.Rows[i][1]).ToUpper() != "EMAIL")
                                {
                                    message = "Invalid Excel format";
                                    break;
                                }
                            }
                            else
                            {
                                string EmployeeAddError = string.Empty;
                                Employee employee = new Employee();
                                employee.CompanyId = companyId;
                                try
                                {
                                    EmployeeAddError = await validateEmployeeOnInsert(dtEmplolyeeRecords, i, EmployeeAddError, employee);
                                    if (string.IsNullOrEmpty(EmployeeAddError))
                                    {
                                        await _employeeRepository.AddEmployee(employee);
                                        uploadedEmployee++;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    EmployeeAddError += " Exception:  " + ex.Message;
                                }

                                if (!string.IsNullOrEmpty(EmployeeAddError))
                                {
                                    empUploadError.Add(new EmployeeUploadExcel()
                                    {
                                        EmpName = employee.EmpName,
                                        EmpMail = employee.EmpMail,
                                        Error = EmployeeAddError
                                    });
                                    failedEmployee++;
                                }
                            }
                        }


                        // let's convert our object data to Datatable for a simplified logic.
                        // Datatable is the easiest way to deal with complex datatypes for easy reading and formatting. 
                        //string errorFileFolderName = Path.Combine("Uploads", "Employee","ErrorFiles");
                        //string errorFilePathToSave = Path.Combine(Directory.GetCurrentDirectory(), errorFileFolderName);
                        //string errorfileName = DateTime.Now.ToShortDateString()+DateTime.Now.ToLongTimeString()+"xlsx";
                        //string errorFullPath = Path.Combine(errorFilePathToSave, errorfileName);
                        //DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(empUploadError), (typeof(DataTable)));
                        //FileInfo filePath = new FileInfo(errorFullPath);

                        //using (var excelPack = new ExcelPackage(filePath))
                        //{
                        //    var ws = excelPack.Workbook.Worksheets.Add("WriteTest");
                        //    ws.Cells.LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light8);
                        //    excelPack.Save();
                        //}
                        if (string.IsNullOrEmpty(message))
                            response = PrepareErrorRespose(uploadedEmployee, failedEmployee, empUploadError);
                    }
                    else
                        message = "Selected file is empty.";
                }
                else
                {
                    message = "Selected file is empty.";
                }
            }


            if (!string.IsNullOrEmpty(message))
            {
                response = new ResponseModel<EmployeeUpload>()
                {
                    IsError = true,
                    Message = message,
                    Data = null
                };
            }
            return response;
        }

        private static ResponseModel<EmployeeUpload> PrepareErrorRespose(int uploadedEmployee, int failedEmployee, List<EmployeeUploadExcel> empUploadError)
        {
            ResponseModel<EmployeeUpload> response;
            EmployeeUpload empUploadFailed = new EmployeeUpload()
            {
                FailedEmployeeRecord = empUploadError,
                UploadError = failedEmployee > 0 ? "Some of the employee record has not been added" : " Employee data added successfully ",
                UploadedRecordCount = uploadedEmployee,
                FailedRecordCount = failedEmployee
            };
            response = new ResponseModel<EmployeeUpload>()
            {
                IsError = failedEmployee > 0 ? true : false,
                Message = "",
                Data = empUploadFailed
            };
            return response;
        }

        private async Task<string> CheckEmployeeCompanyById(int companyId)
        {
            if (companyId == 0)
                return "Invalid Company id ";
            string message = string.Empty;
            Company company = new Company
            {
                CompanyId = companyId,
                CompanyStatus=-1
            };
            IEnumerable<Company> CompanIdExists = await _companyService.GetCompaniesDetailsList(company);
            if (!CompanIdExists.Any())
            {
                message = "Invalid Company id ";
            }

            return message;
        }

        private async Task<string> validateEmployeeOnInsert(DataTable dtEmplolyeeRecords, int i, string EmployeeAddError, Employee employee)
        {
            employee.EmpName = Convert.ToString(dtEmplolyeeRecords.Rows[i][0]);
            employee.EmpMail = Convert.ToString(dtEmplolyeeRecords.Rows[i][1]);

            if (string.IsNullOrEmpty(employee.EmpName))
            {
                EmployeeAddError = "Emplpoyee Name is missing ";
            }

            if (string.IsNullOrEmpty(employee.EmpMail))
            {
                EmployeeAddError += "Emplpoyee email is missing ";
            }
            bool isEmail = Regex.IsMatch(employee.EmpMail, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                EmployeeAddError += "Invalid email address ";
            }
            var employeeReq = new Employee
            {
                EmpMail = employee.EmpMail
            };
            var CompanExists = await _employeeRepository.GetAllEmployees(employeeReq);
            if (CompanExists.Any())
            {
                EmployeeAddError += "Email address already existing ";
            }

            return EmployeeAddError;
        }

        private  DataSet ReadExcelToDs(IFormFile file, string pathToSave)
        {
            DataSet dsexcelRecords;
            IExcelDataReader reader = null;
            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string fullPath = Path.Combine(pathToSave, DateTime.Now.ToString("ddMMyyyyHHmmtt")) + fileName;
            //var dbPath = Path.Combine(folderName, fileName);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);

                string fileextension = Path.GetExtension(fullPath).ToLower();
                if (fileextension == ".xls")
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (fileextension == ".xlsx")
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //else
                //    message = "The file format is not supported.";
                dsexcelRecords = reader.AsDataSet();
                reader.Close();
            }


            return dsexcelRecords;
        }

        public async Task<ResponseModel<bool>> ModifyEmployee(Employee employeeRequest)
        {
            Employee employeeReq = new Employee
            {
                Empid = employeeRequest.Empid
            };
            IEnumerable<Employee> empoyeeIdExists = await _employeeRepository.GetAllEmployees(employeeReq);
            string message = string.Empty;
            bool isError = false;
            if (empoyeeIdExists.Any())
            {
                employeeReq = new Employee
                {
                    EmpMail = employeeRequest.EmpMail
                };
                empoyeeIdExists = await _employeeRepository.GetAllEmployees(employeeReq);
                IEnumerable<Employee> employeeResult = empoyeeIdExists.Where(s => s.Empid != employeeRequest.Empid);
                if (employeeResult.Any())
                {
                    isError = true;
                    message = "Employee email is already in use";
                }
                else
                {
                    await _employeeRepository.ModifyEmployee(employeeRequest);
                    message = "Employee updated successfully";
                }
            }
            else
            {
                isError = true;
                message = "Invalid Employee to update";
            }
            var res = new ResponseModel<bool>()
            {
                IsError = isError,
                Message = message,
                Data = !isError
            };
            return res;
        }
        public async Task<ResponseModel<bool>> DeleteEmployeeById(int Id)
        {
            string message = "Employee deleted successfully"; 
            bool isError = false;
            await _employeeRepository.DeleteEmployeeById(Id);
            var res = new ResponseModel<bool>()
            {
                IsError = isError,
                Message = message,
                Data = !isError
            };
            return res;
        }
    }
}
    

