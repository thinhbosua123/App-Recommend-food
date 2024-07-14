using Project.Model;
using Project.Pages;
using Project.UserControlXAML.AccountPage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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

namespace Project.RegisterPage
{
    /// <summary>
    /// Interaction logic for Re_Account.xaml
    /// </summary>
    public partial class Re_Info : Page, INotifyPropertyChanged
    {
        private string _Name_HelperText;
        public string Name_HelperText
        {
            get { return _Name_HelperText; }
            set
            {
                _Name_HelperText = value;
                OnPropertyChanged("Name_HelperText");
            }
        }

        private FUser user;

        public Re_Info(LoginWindow main, FUser user)
        {
            InitializeComponent();
            MainWindow = main;
            this.user = user;
            IsRegister = false;
            //Fullname.Text = user.UName;
            //age.Text = user.Age.ToString();
            //Gender.SelectedItem = Gender.Items[(int)user.Sex];
            //weight.Text = user.UWeight.ToString();
            //height.Text = user.UHeight.ToString();
            //Mode.SelectedItem = Gender.Items[(int)user.UStatus];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        public bool IsRegister { get; set; }

        public LoginWindow MainWindow { get; set; }


        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!profile_name_checking())
            {
                MessageBox.Show("Vui lòng nhập tên người dùng","Thông báo");
            }
            else if (!gender_checking())
            {
                MessageBox.Show("Vui lòng chọn giới tính", "Thông báo");
            }
            else if (!number_int_checking(age.Text))
            {
                MessageBox.Show("Vui lòng nhập tuổi", "Thông báo");
            }
            else if (!number_int_checking(weight.Text))
            {
                MessageBox.Show("Vui lòng nhập cân nặng", "Thông báo");
            }
            else if (!number_int_checking(height.Text))
            {
                MessageBox.Show("Vui lòng nhập chiều cao", "Thông báo");
            }
            else
            {
                user.UName = Fullname.Text;
                user.Age = Convert.ToInt32(age.Text);
                user.Sex = Gender.SelectedIndex;
                user.UWeight = Convert.ToInt32(weight.Text);
                user.UHeight = Convert.ToInt32(height.Text);
                user.UStatus = Mode.SelectedIndex;
                user.ComsumedCalo = 0;
                user.Avatar = AccountPage.defaultAvatar;
                IsRegister = true;
                MainWindow.finish_register(user);
            }
        }

        #region Event


        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.turn_back();
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            profile_name_checking();
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            profile_name_checking();
        }

        private void age_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !number_int_checking(e.Text);
        }

        private void height_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !number_int_checking(height.Text.Insert(height.CaretIndex, e.Text));
        }

        private void weight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !number_int_checking(weight.Text.Insert(weight.CaretIndex, e.Text));
        }

        private void age_LostFocus(object sender, RoutedEventArgs e)
        {
            if (age.Text == "" || age.Text == "0")
            {
                age.Text = "1";
            }
        }

        private void height_LostFocus(object sender, RoutedEventArgs e)
        {
            if (height.Text == "" || height.Text == "0")
            {
                height.Text = "1";
            }
        }

        private void weight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (weight.Text == "" || weight.Text == "0")
            {
                weight.Text = "1";
            }
        }
        #endregion


        #region Checking
        private bool profile_name_checking()
        {
            if (Fullname.Text.Length == 0)
            {
                Fullname.Foreground = Brushes.Red;
                Name_HelperText = "Tên không được để trống";
                return false;
            }
            else if (!Fullname.Foreground.Equals(new SolidColorBrush(Color.FromArgb(221, 0, 0, 0))))
            {
                Fullname.Foreground = new SolidColorBrush(Color.FromArgb(221, 0, 0, 0));
                Name_HelperText = "Nhập tên";
            }
            return true;
        }

        //private bool number_checking(string text)
        //{
        //    try
        //    {
        //        Convert.ToDouble(text);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        private bool number_int_checking(string text)
        {
            try
            {
                Convert.ToInt32(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool gender_checking()
        {
            if (Gender.SelectedIndex == -1)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
