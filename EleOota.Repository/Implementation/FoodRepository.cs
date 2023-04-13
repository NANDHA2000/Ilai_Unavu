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
    public class FoodRepository : BaseRepository, IFoodRepository
    {
        private const string IdQuery = "@FoodType";
        private readonly IUnitOfWork _unitOfWork;

        public FoodRepository(
            IOptions<DatabaseAdvancedSettingsOptions> settingsOptions,
                       IQueryBuilder queryBuilder,
            IUnitOfWork unitOfWork) : base(settingsOptions, queryBuilder, unitOfWork, TableNames.FoodType, IdQuery)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<FoodType>> GetAllFoodTypes(int id)
        {
            var data = new
            {
                CompId = id
            };

            return await QueryProcedureAsync<FoodType>("sp_GetFoodType", data);
        }
        public async Task<IEnumerable<WeekDayCount>> GetNumberOfWeekDays(int id)
        {
            var data = new
            {
                CompId = id
            };
            return await QueryProcedureAsync<WeekDayCount>("Sp_GetWeekDaysCount", data);
        }
        public async Task<IEnumerable<WeekDayFoodType>> GetWeekDayFoodTypes()
        {
            return await QueryProcedureAsync<WeekDayFoodType>("SP_Weekdays_FoodType", null);
        }
        public async Task<bool> PostSubmitRequest(List<FoodRequest> foodRequest)
        {
            foreach (var request in foodRequest)
            {
                var SubmitData = new
                {
                    CompId = request.CompID,
                    EmpId = request.EmpID,
                    Requestdate = request.RequestDate,
                    FoodOptType = request.FoodOptType,
                    WeekDayId = request.WeekDayid
                };
                await QueryProcedureAsync<FoodRequest>("SP_FoodRequest", SubmitData);

            }
            return true;

        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await QueryProcedureAsync<Employee>("Sp_GetAllEmployees", null);
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

        public async Task<IEnumerable<Company>> GetCompanyList()
        {
            return await QueryProcedureAsync<Company>("Sp_GetCompanyList", null);
        }
        public async Task<IEnumerable<Company>> GetWeekdayList()
        {
            return await QueryProcedureAsync<Company>("usp_GetWeekdayList", null);
        }

        public async Task<AvailableFoodTypeWeekDays> FoodTypeWeekDaysList()
        {
            IEnumerable<AvailableWeekDay> weekDayFoodType = await QueryProcedureAsync<AvailableWeekDay>("usp_WeeksDays", null);
            IEnumerable<AvailableFoodType> foodType = await QueryProcedureAsync<AvailableFoodType>("Usp_FoodTypes", null);
            AvailableFoodTypeWeekDays foodTypeWeekDays = new AvailableFoodTypeWeekDays()
            {
                AvailableFoodTypes = foodType,
                AvailableWeekDays = weekDayFoodType
            };
            return foodTypeWeekDays;
        }

        public async Task<bool> SaveFoodTypeWeekDaysOrder(CompanyFoodTypeOrderPerWeek foodTypeWeekDaysOrder)
        {
            try
            {
                await _unitOfWork.Begin();
                var SubmitCompData = new
                {
                    CompanyId = foodTypeWeekDaysOrder.CompanyId
                };
                await QueryProcedureAsync<bool>("usp_MoveFoodOrderToHistory", SubmitCompData);
                IEnumerable<WeekDayFoodType> weekDaysOrder = foodTypeWeekDaysOrder.CompanyFoodWeekDaysOrder;
                foreach (WeekDayFoodType orderedWeekDays in weekDaysOrder)
                {
                    if (orderedWeekDays.Status)
                    {
                        var SubmitData = new
                        {
                            CompId = foodTypeWeekDaysOrder.CompanyId,
                            WeekDayId = orderedWeekDays.WeekDayId
                        };
                        await QueryProcedureAsync<bool>("usp_InsertFoodOrderByWeeksdays", SubmitData);
                    }
                }

                IEnumerable<FoodType> foodTypeOrder = foodTypeWeekDaysOrder.CompanyFoodTypesOrder;
                foreach (FoodType orderedFooodTypes in foodTypeOrder)
                {
                    if (orderedFooodTypes.Status)
                    {
                        var SubmitData = new
                        {
                            CompId = foodTypeWeekDaysOrder.CompanyId,
                            FoodtypeID = orderedFooodTypes.FoodtypeID
                        };
                        await QueryProcedureAsync<bool>("usp_InsertFoodOrderByFoodType", SubmitData);
                    }
                }
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
            return true;
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


        public async Task<bool> SaveFoodMenu(List<FoodMenu> foodMenuList)
        {
            try
            {
                await _unitOfWork.Begin();                
                foreach (FoodMenu foodmenu in foodMenuList)
                {
                        var SubmitData = new
                        {
                            FoodName = foodmenu.FoodName,
                            WeekDayId = foodmenu.WeekDayId,
                            FoodTypeId = foodmenu.FoodTypeId,
                        };
                        await QueryProcedureAsync<bool>("usp_SaveFoodMenu", SubmitData);                   
                }                
                await _unitOfWork.Commit();
            }
            catch (Exception)
            {
                await _unitOfWork.Rollback();
                throw;
            }
            return true;
        }

    }

}


