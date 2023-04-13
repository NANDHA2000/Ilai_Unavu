using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class AvailableFoodTypeWeekDays
    {
        public IEnumerable<AvailableFoodType> AvailableFoodTypes { get; set; }
        public IEnumerable<AvailableWeekDay> AvailableWeekDays { get; set; }
    }
}
