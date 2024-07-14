using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class DataProvider
    {
        private static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null) _ins = new DataProvider();
                return _ins;
            }
            set { _ins = value; }
        }
        public QLMAEntities DB { get; set; }
        public int Current_UserID { get; set; }
        public double Kcal_UserID { get; set; }
        public DataProvider()
        {
            DB = new QLMAEntities();
            Current_UserID = new int();
            Kcal_UserID = new double();
        }
    }
}
