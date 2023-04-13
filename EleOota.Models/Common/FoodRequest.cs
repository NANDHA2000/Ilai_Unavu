using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class FoodRequest
    {
        public int CompID { get; set; }
        public int EmpID { get; set; }
        public DateTime RequestDate { get; set; }
        public string FoodOptType { get; set; }
        public int WeekDayid { get; set; }
    }
}
