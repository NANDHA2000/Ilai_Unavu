using System;
using System.Collections.Generic;
using System.Text;

namespace EleOota.Models.Common
{
    public class menuNode
    {
        public IEnumerable<DayNode> Days { get; set; }
    }
    public class DayNode
    {
        public IEnumerable<WeekDayNode> Monday { get; set; }
        public IEnumerable<WeekDayNode> Tuesday { get; set; }
        public IEnumerable<WeekDayNode> Wednesday { get; set; }
        public IEnumerable<WeekDayNode> Thursday { get; set; }
        public IEnumerable<WeekDayNode> Friday { get; set; }
        public IEnumerable<WeekDayNode> Saturday { get; set; }
    }
    public class WeekDayNode
    {
        public IEnumerable<DishNode> breakfast { get; set; }
        public IEnumerable<DishNode> lunch { get; set; }
        public IEnumerable<DishNode> dinner { get; set; }
        public IEnumerable<DishNode> snacks { get; set; }
    }
    public class DishNode
    {
        public int Id { get; set; }
        public string Dish { get; set; }
        public int Excellent { get; set; }
        public int Satisified { get; set; }
        public int Need_improvement { get; set; }
    }
}
