using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class FoodDays
    {
        public Food Food { get; set; }
        public DateTime Date { get; set; }
        public string _Date
        {
            get
            {
                if (Date == DateTime.MinValue) return null;
                return Date.ToString();
            }
            set
            {
                _Date = value;
            }
        }
        public int Favourite { get; set; }
        public FoodDays()
        {
            Food = new Food();
            Date = DateTime.Now;
            Favourite = 0;
        }
        public FoodDays(Food food, object time, int? like)
        {
            this.Food = food;
            if (time != null)
            {
                Date = (DateTime)time;
            }
            Favourite = (int)like;
        }
    }
}
