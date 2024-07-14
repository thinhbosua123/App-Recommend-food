using LiveCharts.Defaults;
using LiveCharts;
using Project.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.RightsManagement;

namespace Project.Pages
{
    /// <summary>
    /// Interaction logic for RecommendPage.xaml
    /// </summary>
    public partial class RecommendPage : Page, INotifyPropertyChanged
    {
        public List<FoodDays> meal { get; set; }
        public List<UserFood> FoodUser { get; set; }
        public List<FoodDays> Breakfast_food { get; set; }
        public List<FoodDays> Lunch_food { get; set; }
        public List<FoodDays> Dinner_food { get; set; }
        private List<FoodDays> _foodList;
        private int IsLoadFood;
        private bool tab_selected;
        public List<FoodDays> foodList
        {
            get
            {
                if (_foodList == null) _foodList = new List<FoodDays>();
                return _foodList;
            }
            set
            {
                _foodList = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        CollectionView view;
        public FoodPage foodPage1;
        public RecommendPage(FoodPage foodPage)
        {
            this.DataContext = this;
            Breakfast_food = new List<FoodDays>();
            Lunch_food = new List<FoodDays>();
            Dinner_food = new List<FoodDays>();
            meal = new List<FoodDays>();
            IsLoadFood = 1;
            foodPage1 = foodPage;
            InitializeComponent();
        }
        private void recommend_page_Loaded(object sender, RoutedEventArgs e)
        {
            tab_selected = false;
            FoodUser = DataProvider.Ins.DB.UserFood.Where(p => p.UserID == DataProvider.Ins.Current_UserID).ToList();
            Food food = new Food();
            List<Food> foods = DataProvider.Ins.DB.Food.ToList();
            if (Breakfast_food.Count() == 0)
            {

                foreach (UserFood user in FoodUser)
                {
                    food = DataProvider.Ins.DB.Food.SingleOrDefault(p => p.FoodID == user.FoodID);
                    switch (food.MealTime)
                    {
                        case 3:
                            {
                                Breakfast_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                        case 4:
                            {
                                Lunch_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                        case 5:
                            {
                                Dinner_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                        case 7:
                            {
                                Breakfast_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                                Lunch_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                        case 8:
                            {
                                Breakfast_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                                Dinner_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                        case 9:
                            {
                                Lunch_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                                Dinner_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                        case 12:
                            {
                                Breakfast_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                                Lunch_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                                Dinner_food.Add(new FoodDays(food, user.Last_eat, user.Favorite));
                            }
                            break;
                    }
                }
            }
            if (IsLoadFood-- == 1)
            {
                tab_control.SelectedIndex = 0;
            }
            Gauge_Kcal.To = DataProvider.Ins.Kcal_UserID;
            if (Breakfast_RecommendFood_lv.Items.IsEmpty == true)
            {
                RecommendAlgorithm();
            }
            uc_MealChart.breakfast = 0;
            uc_MealChart.lunch = 0;
            uc_MealChart.dinner = 0;
            foreach (FoodDays item in Lunch_RecommendFood_lv.Items)
            {
                uc_MealChart.lunch += (int)item.Food.Kcal;
            }
            foreach (FoodDays item in Dinner_RecommendFood_lv.Items)
            {
                uc_MealChart.dinner += (int)item.Food.Kcal;
            }
            foreach (FoodDays item in Breakfast_RecommendFood_lv.Items)
            {
                uc_MealChart.breakfast += (int)item.Food.Kcal;
            }
            uc_MealChart.Loaded();

            FUser user1 = DataProvider.Ins.DB.FUser.SingleOrDefault(p => p.UserID == DataProvider.Ins.Current_UserID);
            int Fat = (int)(user1.UWeight * 1.5);
            int Protein = (int)(user1.UWeight * 1.5);
            int Carbs = (int)(user1.UWeight * 6.5);
            uc_NutriChart.seriProtein.Values = new ChartValues<ObservableValue> { new ObservableValue(Protein) };
            uc_NutriChart.seriFat.Values = new ChartValues<ObservableValue> { new ObservableValue(Fat) };
            uc_NutriChart.seriCarbs.Values = new ChartValues<ObservableValue> { new ObservableValue(Carbs) };
        }

        private void Bf_DelSelected_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FoodDays food = button.DataContext as FoodDays;
            Gauge_Kcal.Value -= (double)food.Food.Kcal;
            Breakfast_RecommendFood_lv.Items.Remove(food);
            uc_MealChart.breakfast -= (int)food.Food.Kcal;
            uc_MealChart.Loaded();
            if (Gauge_Kcal.Value <= Gauge_Kcal.To)
            {
                kcal_txt.Visibility = Visibility.Hidden;
            }
        }
        private void Lun_DelSelected_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FoodDays food = button.DataContext as FoodDays;
            Gauge_Kcal.Value -= (double)food.Food.Kcal;
            Lunch_RecommendFood_lv.Items.Remove(food);
            uc_MealChart.lunch -= (int)food.Food.Kcal;
            uc_MealChart.Loaded();
            if (Gauge_Kcal.Value <= Gauge_Kcal.To)
            {
                kcal_txt.Visibility = Visibility.Hidden;
            }
        }
        private void Din_DelSelected_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FoodDays food = button.DataContext as FoodDays;
            Gauge_Kcal.Value -= (double)food.Food.Kcal;
            Dinner_RecommendFood_lv.Items.Remove(food);
            uc_MealChart.dinner -= (int)food.Food.Kcal;
            uc_MealChart.Loaded();
            if (Gauge_Kcal.Value <= Gauge_Kcal.To)
            {
                kcal_txt.Visibility = Visibility.Hidden;
            }
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;

            view = (CollectionView)CollectionViewSource.GetDefaultView(lvRecommendation.Items);

            if (cb.SelectedIndex == 0)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Food.Kcal", ListSortDirection.Ascending));
            }
            else if (cb.SelectedIndex == 1)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Food.FoodName", ListSortDirection.Ascending));
            }
            else if (cb.SelectedIndex == 2)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            }
            else if (cb.SelectedIndex == 3)
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription("Favourite", ListSortDirection.Descending));
            }
        }

        private void tab_control_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tab_selected = true;
            if (tab_control.SelectedIndex == 0)
            {
                foodList = Breakfast_food;
            }
            else if (tab_control.SelectedIndex == 1)
            {
                foodList = Lunch_food;
            }
            else if (tab_control.SelectedIndex == 2)
            {
                foodList = Dinner_food;
            }
            ComboBox_sort.Text = null;
            tab_selected = false;
        }

        private void lvRecommendation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab_selected == true)
            {
                return;
            }
            FoodDays food = (FoodDays)lvRecommendation.SelectedItem;
            if (food != null)
            {
                Gauge_Kcal.Value += (double)food.Food.Kcal;
                switch (tab_control.SelectedIndex)
                {
                    case 0:
                        {
                            Breakfast_RecommendFood_lv.Items.Add(food);
                            uc_MealChart.breakfast +=(int) food.Food.Kcal;
                        }
                        break;
                    case 1:
                        {
                            Lunch_RecommendFood_lv.Items.Add(food);
                            uc_MealChart.lunch += (int)food.Food.Kcal;
                        }
                        break;
                    case 2:
                        {
                            Dinner_RecommendFood_lv.Items.Add(food);
                            uc_MealChart.dinner += (int)food.Food.Kcal;
                        }
                        break;
                }
                uc_MealChart.Loaded();
                if (Gauge_Kcal.Value > Gauge_Kcal.To)
                {
                    kcal_txt.Visibility = Visibility.Visible;
                }
            }
        }


        private void RecommendAlgorithm()
        {
            Random rd = new Random();
            int rdom = rd.Next(3);
            int i = 1;
            List<FoodDays> havent_eaten_food = new List<FoodDays>();
            //breakfast recommend
            havent_eaten_food = Breakfast_food.FindAll(p => p.Date == new DateTime()).ToList();
            FoodDays adding_food = new FoodDays();
            //Mon chinh
            if (rdom == 1)
            {
                adding_food = getRecommendFood("Món nước", havent_eaten_food, Breakfast_food);
            }
            else
            {
                adding_food = getRecommendFood("Cơm", havent_eaten_food, Breakfast_food);
            }
            Breakfast_RecommendFood_lv.Items.Add(adding_food);
            gaugeRevaluate(adding_food);

            //lunch recommend
            havent_eaten_food = Lunch_food.FindAll(p => p.Date == new DateTime()).ToList();
            rdom = rd.Next(4);
            if (rdom == 1)
            {
                //Mon nuoc
                adding_food = getRecommendFood("Món nước", havent_eaten_food, Lunch_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Món nước", Lunch_food, havent_eaten_food, i);
                    i++;
                }
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
            }
            else if (rdom == 2)
            {
                //Do bien + canh
                adding_food = getRecommendFood("Đồ biển", havent_eaten_food, Lunch_food);
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
                adding_food = getRecommendFood("Canh", havent_eaten_food, Lunch_food);
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
            }
            else
            {
                //Com + canh
                adding_food = getRecommendFood("Cơm", havent_eaten_food, Lunch_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Cơm", Lunch_food, havent_eaten_food, i);
                    i++;
                }
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
                adding_food = getRecommendFood("Canh", havent_eaten_food, Lunch_food);
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
            }

            //Dinner recommend
            havent_eaten_food = Dinner_food.FindAll(p => p.Date == new DateTime()).ToList();
            rdom = rd.Next(4);
            if (rdom == 1)
            {
                //Mon nuoc
                adding_food = getRecommendFood("Món nước", havent_eaten_food, Dinner_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Món nước", Dinner_food, havent_eaten_food, i);
                    i++;
                }
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
            }
            else if (rdom == 2)
            {
                //Do bien + canh
                adding_food = getRecommendFood("Đồ biển", havent_eaten_food, Dinner_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Đồ biển", Dinner_food, havent_eaten_food, i);
                    i++;
                }
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
                adding_food = getRecommendFood("Canh", havent_eaten_food, Dinner_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Canh", Dinner_food, havent_eaten_food, i);
                    i++;
                }
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
            }
            else
            {
                //Com + canh
                adding_food = getRecommendFood("Cơm", havent_eaten_food, Dinner_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Cơm", Dinner_food, havent_eaten_food, i);
                    i++;
                }
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
                adding_food = getRecommendFood("Canh", havent_eaten_food, Dinner_food);
                i = 1;
                while (isIncluded(adding_food))
                {
                    adding_food = regettingRecommend("Canh", Dinner_food, havent_eaten_food, i);
                    i++;
                }
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                gaugeRevaluate(adding_food);
            }
            //Optional 
            //Thuc uong
            //Breakfast
            adding_food = getRecommendFood("Thức uống", havent_eaten_food, Breakfast_food);
            if (Gauge_Kcal.Value + (double)adding_food.Food.Kcal > Gauge_Kcal.To)
            {
                adding_food = leastKcalHaventEatenFRecommend("Thức uống", Breakfast_food, havent_eaten_food);
            }
            if (Gauge_Kcal.Value + (double)adding_food.Food.Kcal < Gauge_Kcal.To)
            {
                Breakfast_RecommendFood_lv.Items.Add(adding_food);
                Gauge_Kcal.Value += (double)adding_food.Food.Kcal;
            }
            //Lunch
            havent_eaten_food = Lunch_food.FindAll(p => p.Date == new DateTime()).ToList();
            i = 1;
            adding_food = getRecommendFood("Thức uống", havent_eaten_food, Lunch_food);
            while (isIncluded(adding_food))
            {
                adding_food = regettingRecommend("Thức uống", Lunch_food, havent_eaten_food, i);
                i++;
            }
            if (Gauge_Kcal.Value + (double)adding_food.Food.Kcal < Gauge_Kcal.To)
            {
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                Gauge_Kcal.Value += (double)adding_food.Food.Kcal;
            }
            //Dinner
            havent_eaten_food = Dinner_food.FindAll(p => p.Date == new DateTime()).ToList();
            adding_food = getRecommendFood("Thức uống", havent_eaten_food, Dinner_food);
            i = 1;
            while (isIncluded(adding_food))
            {
                adding_food = regettingRecommend("Thức uống", Dinner_food, havent_eaten_food, i);
                i++;
            }
            if (Gauge_Kcal.Value + (double)adding_food.Food.Kcal < Gauge_Kcal.To)
            {
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                Gauge_Kcal.Value += (double)adding_food.Food.Kcal;
            }
            //An vat
            //Lunch
            havent_eaten_food = Lunch_food.FindAll(p => p.Date == new DateTime()).ToList();
            adding_food = getRecommendFood("Ăn vặt", havent_eaten_food, Lunch_food);
            i = 1;
            while (isIncluded(adding_food))
            {
                adding_food = regettingRecommend("Ăn vặt", Lunch_food, havent_eaten_food, i);
                i++;
            }
            if (Gauge_Kcal.Value + (double)adding_food.Food.Kcal < Gauge_Kcal.To)
            {
                Lunch_RecommendFood_lv.Items.Add(adding_food);
                Gauge_Kcal.Value += (double)adding_food.Food.Kcal;
            }
            //Dinner
            havent_eaten_food = Dinner_food.FindAll(p => p.Date == new DateTime()).ToList();
            adding_food = getRecommendFood("Ăn vặt", havent_eaten_food, Dinner_food);
            i = 1;
            while (isIncluded(adding_food))
            {
                adding_food = regettingRecommend("Ăn vặt", Dinner_food, havent_eaten_food, i);
                i++;
            }
            if (Gauge_Kcal.Value + (double)adding_food.Food.Kcal < Gauge_Kcal.To)
            {
                Dinner_RecommendFood_lv.Items.Add(adding_food);
                Gauge_Kcal.Value += (double)adding_food.Food.Kcal;
            }
        }
        private FoodDays getRecommendFood(string type, List<FoodDays> havent_eaten_food, List<FoodDays> MealTimeFoods)
        {
            Random rd = new Random();
            FoodDays adding_food;
            List<FoodDays> foods = havent_eaten_food.FindAll(p => p.Food.Type == type).ToList();
            if (foods.Count != 0)
            {
                adding_food = foods[rd.Next(foods.Count())];
            }
            else
            {
                foods = MealTimeFoods.FindAll(p => p.Food.Type == type).ToList();
                adding_food = foods.OrderBy(p => p.Date).ToList().Last();
            }
            return adding_food;
        }
        private bool isIncluded(FoodDays adding_food)
        {
            foreach (FoodDays fd in Breakfast_RecommendFood_lv.Items)
            {
                if (fd.Food.FoodID == adding_food.Food.FoodID)
                {
                    return true;
                }
            }
            foreach (FoodDays fd in Lunch_RecommendFood_lv.Items)
            {
                if (fd.Food.FoodID == adding_food.Food.FoodID)
                {
                    return true;
                }
            }
            return false;
        }
        private FoodDays regettingRecommend(string type, List<FoodDays> MealTimeFoods, List<FoodDays> havent_eaten_food, int index)
        {
            FoodDays adding_food;
            Random rd = new Random();
            List<FoodDays> foods = havent_eaten_food.FindAll(p => p.Food.Type == type).ToList();
            if (foods != null)
            {
                adding_food = foods[rd.Next(foods.Count())];
                return adding_food;
            }
            else
            {
                foods = MealTimeFoods.FindAll(p => p.Food.Type == type).ToList();
                adding_food = foods.OrderBy(p => p.Date).ToList()[foods.Count() - (index + 1)];
                return adding_food;
            }
        }
        private FoodDays leastKcalHaventEatenFRecommend(string type, List<FoodDays> MealTimeFoods, List<FoodDays> havent_eaten_food)
        {
            FoodDays adding_food;
            List<FoodDays> foods = havent_eaten_food.FindAll(p => p.Food.Type == type).ToList();
            adding_food = foods.OrderBy(p => p.Food.Kcal).First();
            return adding_food;
        }

        private void gaugeRevaluate(FoodDays adding_food)
        {
            Gauge_Kcal.Value += (double)adding_food.Food.Kcal;
            if (Gauge_Kcal.Value > Gauge_Kcal.To)
            {
                kcal_txt.Visibility = Visibility.Visible;
            }
        }
        private void Bf_re_tbtn_Checked(object sender, RoutedEventArgs e)
        {
            Lun_re_tbtn.IsChecked = false;
            Din_re_tbtn.IsChecked = false;
        }

        private void Lun_re_tbtn_Checked(object sender, RoutedEventArgs e)
        {
            Bf_re_tbtn.IsChecked = false;
            Din_re_tbtn.IsChecked = false;
        }

        private void Din_re_tbtn_Checked(object sender, RoutedEventArgs e)
        {
            Bf_re_tbtn.IsChecked = false;
            Lun_re_tbtn.IsChecked = false;
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            if (Bf_re_tbtn.IsChecked == false && Lun_re_tbtn.IsChecked == false && Din_re_tbtn.IsChecked == false)
            {
                MessageBox.Show("Vui lòng tick vào ô bữa ăn bạn muốn chấp nhận nhé!");
                return;
            }
            else if (Bf_re_tbtn.IsChecked == true)
            {
                foreach (FoodDays fd in Breakfast_RecommendFood_lv.Items)
                {
                    meal.Add(fd);
                }
            }
            else if (Lun_re_tbtn.IsChecked == true)
            {
                foreach (FoodDays fd in Lunch_RecommendFood_lv.Items)
                {
                    meal.Add(fd);
                }
            }
            else if (Din_re_tbtn.IsChecked == true)
            {
                foreach (FoodDays fd in Dinner_RecommendFood_lv.Items)
                {
                    meal.Add(fd);
                }
            }
            foreach (FoodDays f in meal)
            {
                foodPage1.SelectedFood_lv.Items.Add(f);
                foodPage1.Gauge_Kcal.Value += (double)f.Food.Kcal;
            }
            if(foodPage1.Gauge_Kcal.Value > foodPage1.Gauge_Kcal.To)
            {
                foodPage1.kcal_txt.Visibility = Visibility.Visible;
            }
            meal.Clear();
            MessageBox.Show("Xong rùi! Bạn vào trang món ăn để xem công thức nhé!!!");
        }
        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvRecommendation.Items.Filter = UserFilter;
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as FoodDays).Food.FoodName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
