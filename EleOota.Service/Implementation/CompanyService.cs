using EleOota.Models.Common;
using EleOota.Repository.Interface;
using EleOota.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EleOota.Service.Implementation
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(_companyRepository));
        }


        public async Task<List<Company>> GetCompaniesDetailsList(Company companyReq)
        {
            IEnumerable<Company> company;
            company = await _companyRepository.GetCompaniesDetailsList(companyReq);
            return company.ToList();
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await _companyRepository.GetCompanyById(id);
        }

        public async Task<ResponseModel<bool>> AddCompany(Company companyRequest)
        {
            var companyReq = new Company
            {
                CompanyMail = companyRequest.CompanyMail,
                CompanyStatus=-1
            };
            var CompanExists = await _companyRepository.GetCompaniesDetailsList(companyReq);
            if (CompanExists.Any())
            {
                var res = new ResponseModel<bool>()
                {
                    IsError = true,
                    Message = "Email address already existing",
                    Data = false
                };
                return res;
            }
            else
            {
                await _companyRepository.AddComapny(companyRequest);
                var res = new ResponseModel<bool>()
                {
                    IsError = false,
                    Message = "Request Sent Successfully",
                    Data = true
                };
                return res;
            }
        }


        public async Task<ResponseModel<bool>> ModifyCompany(Company companyRequest)
        {
            Company companyReq = new Company
            {
                CompanyId = companyRequest.CompanyId,
                CompanyStatus=-1
            };
            IEnumerable<Company> CompanIdExists = await _companyRepository.GetCompaniesDetailsList(companyReq);
            string message = string.Empty;
            bool isError = false;
            if (CompanIdExists.Any())
            {
                companyReq = new Company
                {
                    CompanyMail = companyRequest.CompanyMail,
                    CompanyStatus = -1
                };
                CompanIdExists = await _companyRepository.GetCompaniesDetailsList(companyReq);
                IEnumerable<Company> CompanyResult = CompanIdExists.Where(s => s.CompanyId != companyRequest.CompanyId);
                if (CompanyResult.Any())
                {
                    isError = true;
                    message = "Company email is already in use";
                }
                else
                {
                    companyRequest.CompanyStatus = -1;
                    await _companyRepository.ModifyCompany(companyRequest);
                    message = "Company updated successfully";
                }
            }
            else
            {
                isError = true;
                message = "Invalid Company to update";
            }
            var res = new ResponseModel<bool>()
            {
                IsError = isError,
                Message = message,
                Data = !isError
            };
            return res;
        }

        public async Task<ResponseModel<bool>> ChangeCompanyStatus(Company companyRequest)
        {
            Company companyReq = new Company
            {
                CompanyId = companyRequest.CompanyId,
                CompanyStatus = -1
            };
            IEnumerable<Company> CompanIdExists = await _companyRepository.GetCompaniesDetailsList(companyReq);
            string message = string.Empty;
            bool isError = false;
            if (CompanIdExists.Any())
            {
                var company = CompanIdExists.FirstOrDefault();
                if (company.CompanyStatus == companyRequest.CompanyStatus)
                {
                    isError = true;
                    message = "Company is already in the desired status.";
                }
                else
                {
                    companyRequest.CompanyMail = null;
                    companyRequest.CompanyName = null;
                    await _companyRepository.ModifyCompany(companyRequest);
                    message = "Company status updated successfully";
                }
            }
            else
            {
                isError = true;
                message = "Invalid Company to update";
            }
            var res = new ResponseModel<bool>()
            {
                IsError = isError,
                Message = message,
                Data = !isError
            };
            return res;
        }

        public async Task<CompanyFoodTypeOrderPerWeek> FoodTypePerWeekDaysByCopmany(int id)
        {
            return await _companyRepository.FoodTypePerWeekDaysByCopmany(id);
        }
        public async Task<ResponseModel<IEnumerable<CompanyFoodOrder>>> CompanyFoodCount(string companyName)
        {
            IEnumerable<CompanyFoodOrder> weekDayFoodType = null;
            string message = string.Empty;
            bool isError = true;
            if (string.IsNullOrEmpty(companyName))
                message = "Company Name is required";

            if (string.IsNullOrEmpty(message))
            {
                weekDayFoodType = await _companyRepository.CompanyFoodCount(companyName);
                isError = false;
            }

            return new ResponseModel<IEnumerable<CompanyFoodOrder>>()
            {
                IsError = isError,
                Message = message,
                Data = weekDayFoodType
            };
        }
        public async Task<ResponseModel<IEnumerable<CompanyFoodOrder>>> CompanyFoodCount(string companyName, bool isNextWeek)
        {
            IEnumerable<CompanyFoodOrder> weekDayFoodType = null;
            string message = string.Empty;
            bool isError = true;
            if (string.IsNullOrEmpty(companyName))
                message = "Company Name is required";

            if (string.IsNullOrEmpty(message))
            {
                weekDayFoodType = await _companyRepository.CompanyFoodCount(companyName, isNextWeek);
                isError = false;
            }

            return new ResponseModel<IEnumerable<CompanyFoodOrder>>()
            {
                IsError = isError,
                Message = message,
                Data = weekDayFoodType
            };
        }


    }
}
