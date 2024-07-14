using Project.Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Project.RegisterPage;
using System.Security.RightsManagement;

namespace Project
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Screen.Content = new Log_MainPage(this);
            
        }
        public bool IsLogin { get; set; }

        public int UserID { get; set; }
        #region Window

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {

            if (this.WindowState != WindowState.Minimized)
            {
                this.WindowState = (WindowState.Minimized);
                this.btnMaximize.Content = "🗗";
            }
            else { this.WindowState = WindowState.Normal; }
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {

            if (this.WindowState != WindowState.Maximized)
            {
                this.WindowState = (WindowState.Maximized);
                this.btnMaximize.Content = "🗗";
            }
            else { this.WindowState = WindowState.Normal; }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

        #endregion

        #region Register


        public void next_step(FUser newUser)
        {
            Screen.Content = new Re_Info(this, newUser);
        }

        public void finish_register(FUser newUser)
        {
            Re_Info page = ((Re_Info)Screen.Content);
            if (page.IsRegister)
            {
                DataProvider.Ins.DB.FUser.Add(newUser);
                DataProvider.Ins.DB.SaveChanges();
                Screen.Content = new Log_MainPage(this);
                MessageBox.Show("Đăng kí thành công","Thông báo");
            }
        }

        public void turn_back()
        {
            Screen.Content = new Log_MainPage(this);
        }

        public void enter_Register()
        {
            Screen.Content = new Re_Account(this);
        }
        public void enter_Forget()
        {
            Screen.Content = new ForgetPassPage(this);
        }
        #endregion
    }
}
