using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EleOota.Models.Common
{
    public class CompanyFoodTypeOrderPerWeek
    {
        [Required]
        public int CompanyId { get; set; }
        public IEnumerable<FoodType> CompanyFoodTypesOrder { get; set; }
        public IEnumerable<WeekDayFoodType> CompanyFoodWeekDaysOrder { get; set; }
    }
}
