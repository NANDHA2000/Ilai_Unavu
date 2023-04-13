using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class FoodMenu
    {
        public string FoodName { get; set; }
        public int WeekDayId { get; set; }
        public int FoodTypeId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
