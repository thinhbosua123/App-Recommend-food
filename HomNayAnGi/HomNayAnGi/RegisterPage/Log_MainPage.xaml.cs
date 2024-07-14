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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project.RegisterPage
{
    /// <summary>
    /// Interaction logic for Log_MainPage.xaml
    /// </summary>
    public partial class Log_MainPage : Page
    {
        public Log_MainPage(LoginWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        public LoginWindow MainWindow;
        public bool IsLogin { get; set; }

        public int UserID { get; set; }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            int count = DataProvider.Ins.DB.FUser.Where(p => (p.Username == txtUser.Text && p.Passwrd == txtPassword.Password)).Count();
            if (count > 0)
            {

                MainWindow.IsLogin = true;
                FUser user = DataProvider.Ins.DB.FUser.SingleOrDefault(p => p.Username == txtUser.Text);
                DataProvider.Ins.Current_UserID = user.UserID;
                double kcal = 0;
                if (user.Sex == 1)// nam
                {
                    kcal = (double)(6.25 * user.UHeight + 10 * user.UWeight - 5 * user.Age + 5);
                }
                else // nu
                {
                    kcal = (double)(6.25 * user.UHeight + 10 * user.UWeight - 5 * user.Age - 161);
                }
                if (user.UStatus == 1) // giam can
                {
                    kcal -= 500;
                }
                DataProvider.Ins.Kcal_UserID = kcal*1.5;
                MainWindow.Close();
            }
            else
            {
                MainWindow.IsLogin = false;
                txtUser.Text = null;
                txtPassword.Password = null;
                MessageBox.Show("Tài khoản hoặc mật khẩu bị sai");
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.enter_Register();
        }

        private void ForgetPass_tb_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.enter_Forget();
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btnLogin.Focus();
                btnLogin.Click += btnLogin_Click;
                btnLogin.Click -= btnLogin_Click;
            }
        }

    }
}
