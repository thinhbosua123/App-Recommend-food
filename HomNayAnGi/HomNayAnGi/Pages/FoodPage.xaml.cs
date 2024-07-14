using MaterialDesignThemes.Wpf;
using Project.Model;
using Project.Pages.SubFoodPage;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace Project.Pages
{
    /// <summary>
    /// Interaction logic for FoodPage.xaml
    /// </summary>
    public partial class FoodPage : Page , INotifyPropertyChanged
    {
        public List<UserFood> FoodUser { get; set; }
        public List<FoodDays> Com { get; set; }
        public List<FoodDays> MonNuoc { get; set; }
        public List<FoodDays> DoBien { get; set; }
        public List<FoodDays> Canh { get; set; }
        public List<FoodDays> ThucUong { get; set; }
        public List<FoodDays> AnVat { get; set; }
        private List<FoodDays> _foodList;
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

        private int IsLoadFood;
        public FUser User { get; set; }
        CollectionView view;
        public FoodPage()
        {
            InitializeComponent();
            this.DataContext = this;
            User = new FUser();
            
            FoodUser = new List<UserFood>();
            textchangebytime();
            IsLoadFood = 1;
            if(lvDataBinding.Items.Count > 0)
            {
                lvDataBinding.Items.Filter = UserFilter;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as FoodDays).Food.FoodName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        public void textchangebytime()
        {

            int time = Convert.ToInt32(DateTime.Now.Hour.ToString());
            if (time >= 4 && time < 11)
            {
                HelloTime_tb.Text = "Chào buổi sáng !";
            }
            else if (time >= 11 && time <= 12)
            {
                HelloTime_tb.Text = "Chào buổi trưa !";
            }
            else if (time > 12 && time < 18)
            {
                HelloTime_tb.Text = "Chào buổi chiều !";
            }
            else HelloTime_tb.Text = "Chào buổi tối !";
        }

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            FoodDays food = (FoodDays)lvDataBinding.SelectedItem;
            if(food != null)
            {
                Gauge_Kcal.Value +=(double) food.Food.Kcal;
                SelectedFood_lv.Items.Add(food);
                if (Gauge_Kcal.Value > Gauge_Kcal.To)
                {
                    kcal_txt.Visibility = Visibility.Visible;
                }
            }
            
        }


        private void foodpage_Loaded(object sender, RoutedEventArgs e)
        {
            User = DataProvider.Ins.DB.FUser.SingleOrDefault(p => p.UserID == DataProvider.Ins.Current_UserID);
            if(User != null)
            {
                if (SelectedFood_lv.Items.Count == 0) Gauge_Kcal.Value = (double)User.ComsumedCalo;
                Food food = new Food();
                FoodUser = DataProvider.Ins.DB.UserFood.Where(p => p.UserID == DataProvider.Ins.Current_UserID).ToList();
                //MessageBox.Show(FoodUser.Count().ToString());
                Com = new List<FoodDays>();
                MonNuoc = new List<FoodDays>();
                Canh = new List<FoodDays>();
                DoBien = new List<FoodDays>();
                ThucUong = new List<FoodDays>();
                AnVat = new List<FoodDays>();
                foreach (UserFood user in FoodUser)
                {
                    food = DataProvider.Ins.DB.Food.SingleOrDefault(p => p.FoodID == user.FoodID);
                    switch (food.Type)
                    {
                        case "Cơm":
                            Com.Add(new FoodDays(food, user.Last_eat, user.Favorite));

                            break;
                        case "Món nước":
                            MonNuoc.Add(new FoodDays(food, user.Last_eat, user.Favorite));

                            break;
                        case "Canh":
                            Canh.Add(new FoodDays(food, user.Last_eat, user.Favorite));

                            break;
                        case "Thức uống":
                            ThucUong.Add(new FoodDays(food, user.Last_eat, user.Favorite));

                            break;
                        case "Đồ biển":
                            DoBien.Add(new FoodDays(food, user.Last_eat, user.Favorite));

                            break;
                        default:
                            AnVat.Add(new FoodDays(food, user.Last_eat, user.Favorite));

                            break;
                    }

                }
                if (IsLoadFood-- == 1)
                {
                    ComRadioBtn.IsChecked = true;
                }
                Gauge_Kcal.To = DataProvider.Ins.Kcal_UserID;
            }

        }
        private void ComButton_Checked(object sender, RoutedEventArgs e)
        {
            ComboBox_sort.Text = null;
            foodList = Com;
            

        }
        private void MNButton_Checked(object sender, RoutedEventArgs e)
        {
            ComboBox_sort.Text = null;
            foodList = MonNuoc;
            
            
        }

        private void DBButton_Checked(object sender, RoutedEventArgs e)
        {
            
            ComboBox_sort.Text = null;
            foodList = DoBien;
            
        }

        private void CanhButton_Checked(object sender, RoutedEventArgs e)
        {
            
            ComboBox_sort.Text = null;
            foodList = Canh;
            
        }

        private void TUButton_Checked(object sender, RoutedEventArgs e)
        {
            
            ComboBox_sort.Text = null;
            foodList = ThucUong;
            
        }

        private void AVButton_Checked(object sender, RoutedEventArgs e)
        {
            
            ComboBox_sort.Text = null;
            foodList = AnVat;
            
            
            /*foreach (Food fo in foodList)
            {
                lvDataBinding.Items.Add(fo);
            }*/
            //lvDataBinding.ItemsSource = foodList;
            /*view = (CollectionView)CollectionViewSource.GetDefaultView(lvDataBinding.Items);
            view.Filter = UserFilter;*/
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvDataBinding.Items.Filter = UserFilter;
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            MessageBox.Show(comboBox.SelectedValue.ToString());
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            
            view = (CollectionView)CollectionViewSource.GetDefaultView(lvDataBinding.Items);
            
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
            
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            InsertFoodWindow insertFoodWindow = new InsertFoodWindow();
            insertFoodWindow.Owner = Window.GetWindow(this);
            
            insertFoodWindow.ShowDialog();
        }

        private void ReviewRecipeBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if(SelectedFood_lv.Items.Count == 0)
            {
                MessageBox.Show("Hiện tại món đã chọn đang trống !");
            }
            else
            {
                RecipeWindow recipeWindow = new RecipeWindow();
                recipeWindow.recipe_lv.ItemsSource = SelectedFood_lv.Items;
                foreach(FoodDays foodDays in SelectedFood_lv.Items)
                {
                    foreach(FoodDays food in foodList)
                    {
                        if(food.Food.FoodID == foodDays.Food.FoodID)
                        {
                            food.Date = DateTime.Now;
                        }
                    }    
                    UserFood user = DataProvider.Ins.DB.UserFood.SingleOrDefault(p => p.UserID == DataProvider.Ins.Current_UserID && p.FoodID == foodDays.Food.FoodID);
                    UserHistory history = new UserHistory();
                    history.UserID = user.UserID;
                    history.FoodID = user.FoodID;
                    history.eatDate = DateTime.Now;
                    int time = Convert.ToInt32(DateTime.Now.Hour.ToString());
                    if (time >= 4 && time < 11) // Sang
                    {
                        history.Meal = 3;
                    }
                    else if (time >= 11 && time < 18) // trua - Chieu
                    {
                        history.Meal = 4;
                    }
                    else // Toi
                    {
                        history.Meal = 5;   
                    }
                    DataProvider.Ins.DB.UserHistory.Add(history);
                    DataProvider.Ins.DB.SaveChanges();
                    user.Last_eat = DateTime.Now;
                    
                }
                User.ComsumedCalo =(int) Gauge_Kcal.Value;
                lvDataBinding.Items.Refresh();
                DataProvider.Ins.DB.SaveChanges();
                recipeWindow.Owner = Window.GetWindow(this);
                recipeWindow.ShowDialog();
                SelectedFood_lv.Items.Clear();
            }    
            
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(lvDataBinding.Items.Count.ToString());
            if (MessageBox.Show("Bạn có chắc chắn xóa món ăn này không ?","Xóa món ăn",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Button button = (Button)sender;
                FoodDays food = button.DataContext as FoodDays;
                if(food.Food.FoodID > 60)
                {
                    DataProvider.Ins.DB.Food.Remove(DataProvider.Ins.DB.Food.SingleOrDefault(p=>p.FoodID == food.Food.FoodID));
                }
                DataProvider.Ins.DB.UserFood.Remove(DataProvider.Ins.DB.UserFood.SingleOrDefault(p => p.FoodID == food.Food.FoodID && p.UserID == DataProvider.Ins.Current_UserID));
                DataProvider.Ins.DB.SaveChanges();
                foodList.Remove(food);
                lvDataBinding.Items.Refresh();
                lvDataBinding.Items.Filter = UserFilter;
            }
            
        }


        private void DelSelected_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FoodDays food = button.DataContext as FoodDays;
            Gauge_Kcal.Value -= (double)food.Food.Kcal;
            SelectedFood_lv.Items.Remove(food);
            if (Gauge_Kcal.Value <= Gauge_Kcal.To)
            {
                kcal_txt.Visibility = Visibility.Hidden;
            }
        }

        private void favorite_button_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = sender as ToggleButton;
            FoodDays foodDays = toggleButton.DataContext as FoodDays;
            //foodDays.Favourite = Convert.ToInt16(checkBox.IsChecked);
            UserFood userFood = DataProvider.Ins.DB.UserFood.SingleOrDefault(p => p.UserID == DataProvider.Ins.Current_UserID && p.FoodID == foodDays.Food.FoodID);
            userFood.Favorite = Convert.ToInt16(toggleButton.IsChecked);
            DataProvider.Ins.DB.SaveChanges();
        }

        private void ResetKcalButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn tạo lại thanh kcal hằng ngày ?" , "Thông báo" , MessageBoxButton.OKCancel , MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Gauge_Kcal.Value = 0;
                foreach(FoodDays f in SelectedFood_lv.Items)
                {
                    Gauge_Kcal.Value +=(double) f.Food.Kcal;
                    User.ComsumedCalo =(int) Gauge_Kcal.Value;
                }
                DataProvider.Ins.DB.SaveChanges();
                
            }    
        }
    }
}
