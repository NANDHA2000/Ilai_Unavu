using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class WeekDayFoodType
    {
        public int CompId { get; set; }
        public string CompanyName { get; set; }
        public int EmployeeCount { get; set; }
        public int FoodTypeCount { get; set; }
        public string FoodTypeName { get; set; }
        public int WeekDaysCount { get; set; }
        public string WeekDayName { get; set; }
        public int WeekDayId { get; set; }
        public bool Status { get; set; }
    }
}
