using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Project.Model;

namespace Project.Pages
{
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        private double _lastLecture;
        private double _trend;
        private HistoryInDay[] _userhistory;
        public HistoryPage()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(new StackedColumnSeries
            {
                Title = "Sáng",
                Values = new ChartValues<int>(),
                StackMode = StackMode.Values,
                DataLabels = true
            });
            SeriesCollection.Add(new StackedColumnSeries
            {
                Title = "Trưa",
                Values = new ChartValues<int>(),
                StackMode = StackMode.Values,
                DataLabels = true
            });
            SeriesCollection.Add(new StackedColumnSeries
            {
                Title = "Tối",
                Values = new ChartValues<int>(),
                StackMode = StackMode.Values,
                DataLabels = true
            });
            Formatter = value => value + " Kcal";
            //SeriesCollection = new SeriesCollection
            //{
            //    new StackedColumnSeries
            //    {
            //        Values = new ChartValues<double> {4, 5, 6, 8},
            //        StackMode = StackMode.Values, // this is not necessary, values is the default stack mode
            //        DataLabels = true
            //    },
            //    new StackedColumnSeries
            //    {
            //        Values = new ChartValues<double> {2, 5, 6, 7},
            //        StackMode = StackMode.Values,
            //        DataLabels = true
            //    }
            //};

            ////adding series updates and animates the chart
            //SeriesCollection.Add(new StackedColumnSeries
            //{
            //    Values = new ChartValues<double> { 6, 2, 7 },
            //    StackMode = StackMode.Values
            //});

            ////adding values also updates and animates
            //SeriesCollection[2].Values.Add(4d);

            //Labels = new[] { "Chrome", "Mozilla", "Opera", "IE" };
            //Formatter = value => value + " Mill";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        //===================================================================================
        #region backend
        //---------------------------------------------------------------------------------
        // STATIC DATABASE
        // Get Data
        static private List<UserHistory> _HistoryData = null;

        static public List<UserHistory> HistoryData
        {
            get
            {
                refresh_History();
                return _HistoryData;
            }
        }

        static private bool refresh_History()
        {
            if (_HistoryData == null || _HistoryData != DataProvider.Ins.DB.UserHistory.ToList())
            {
                _HistoryData = DataProvider.Ins.DB.UserHistory.ToList();
                return true;
            }
            return false;
        }

        //Get User Data
        static private List<UserHistory> _UserHistory = null;
        static public List<UserHistory> UserHistory
        {
            get
            {
                if(refresh_History())
                {
                    refresh_UserHistory();
                }
                return _UserHistory;
            }
        }

        static private void refresh_UserHistory()
        {
            _UserHistory = HistoryData.Where(x => (int)(x.UserID) == DataProvider.Ins.Current_UserID).ToList();
            _UserHistory.OrderBy(x => x.eatDate);
        }

        //---------------------------------------------------------------------------------
        struct HistoryInDay
        {
            private List<Food> _Morning;
            private List<Food> _Lunch;
            private List<Food> _Dinner;
            private DateTime _date;
            public HistoryInDay(DateTime date, List<Food> morning, List<Food> lunch, List<Food> dinner)
            {
                _date = date;
                _Morning = morning;
                _Lunch = lunch;
                _Dinner = dinner;
            }

            public List<Food> Morning
            {
                get { return _Morning; }
            }

            public List<Food> Lunch
            {
                get { return _Lunch; }
            }

            public List<Food> Dinner
            {
                get { return _Dinner; }
            }

            public int MorningKcal
            {
                get 
                {
                    if (_Morning.Count == 0)
                    {
                        return 0;
                    }
                    return (int)(_Morning.Sum(x => x.Kcal));
                }
            }

            public int LunchKcal
            {
                get 
                {
                    if (_Lunch.Count == 0)
                    {
                        return 0;
                    }
                    return (int)(_Lunch.Sum(x => x.Kcal));
                }
            }

            public int DinnerKcal
            {
                get 
                {
                    if (_Dinner.Count == 0)
                    {
                        return 0;
                    }
                    return (int)(_Dinner.Sum(x => x.Kcal));
                }
            }

            public DateTime Date
            {
                get { return _date; }
            }
        } 

        private int CompareDate(DateTime a, DateTime b)
        {
            if (a.Year < b.Year)
            {
                return -1;
            }
            if (a.Year > b.Year)
            {
                return 1;
            }
            if (a.DayOfYear < b.DayOfYear)
            {
                return -1;
            }
            if (a.DayOfYear > b.DayOfYear)
            {
                return 1;
            }
            return 0;
        }

        private HistoryInDay[] GetHistory()
        {
            HistoryInDay[] history = new HistoryInDay[7];
            DateTime date = DateTime.Now;
            TimeSpan sub = new TimeSpan(1, 0, 0, 0);
            for (int i = 6; i>=0; i--)
            {
                history[i] = new HistoryInDay(date, new List<Food>(), new List<Food>(), new List<Food>());
                date -= sub;
            }

            if (UserHistory.Count() == 0)
            {
                return history;
            }
            date = DateTime.Now;
            int count = 6;
            for(int i = UserHistory.Count-1; i>=0; i--)
            {
                UserHistory his = UserHistory[i];
                while (CompareDate(date,(DateTime)(his.eatDate)) == 1)
                {
                    date -= sub;
                    count--;
                }
                if (count < 0)
                {
                    break;
                }

                // Add to History in date
                HistoryInDay historyInDay = history[count];
                switch(his.Meal)
                {
                    case 3:
                        {
                            historyInDay.Morning.Add(his.Food);
                            break;
                        }
                    case 4:
                        {
                            historyInDay.Lunch.Add(his.Food);
                            break;
                        }
                    case 5:
                        {
                            historyInDay.Dinner.Add(his.Food);
                            break;
                        }
                }

            }
            return history;
        }


        //---------------------------------------------------------------------------------
        #endregion

        #region front-end
        private void Get_FoodList(int index)
        {
            Clear_FoodList();
            if (index < 0 || index > 6)
            {
                return;
            }
            HistoryInDay db = _userhistory[index];
            foreach(Food f in db.Morning)
            {
                lvHistory1.Items.Add(f);
            }
            foreach (Food f in db.Lunch)
            {
                lvHistory2.Items.Add(f);
            }
            foreach (Food f in db.Dinner)
            {
                lvHistory3.Items.Add(f);
            }
        }

        private void Clear_FoodList()
        {
            lvHistory1.Items.Clear();
            lvHistory2.Items.Clear();
            lvHistory3.Items.Clear();
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _userhistory = GetHistory();
            combobox.Items.Clear();
            Clear_FoodList();
            List<string> labelList = new List<string>();

            List<int> morningvalue = new List<int>();
            List<int> lunchvalue = new List<int>();
            List<int> dinnervalue = new List<int>();
            foreach (HistoryInDay his in _userhistory)
            {
                morningvalue.Add(his.MorningKcal);
                lunchvalue.Add(his.LunchKcal);
                dinnervalue.Add(his.DinnerKcal);
                labelList.Add(his.Date.ToString("dd/MM"));
                combobox.Items.Add(his.Date.ToString("dd/MM"));
            }
            SeriesCollection[0].Values = new ChartValues<int>(morningvalue);
            SeriesCollection[1].Values = new ChartValues<int>(lunchvalue);
            SeriesCollection[2].Values = new ChartValues<int>(dinnervalue);
            Labels = labelList.ToArray();
            combobox.SelectedIndex = 6;
            Get_FoodList(combobox.SelectedIndex);
        }

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Get_FoodList(combobox.SelectedIndex);
        }

        
    }
}
