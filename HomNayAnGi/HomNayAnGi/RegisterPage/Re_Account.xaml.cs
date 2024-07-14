using Project.Model;
using Project.UserControlXAML.AccountPage;
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
    /// Interaction logic for Re_Account.xaml
    /// </summary>
    public partial class Re_Account : Page
    {
        public Re_Account(LoginWindow main)
        {
            InitializeComponent();
            MainWindow = main;
        }

        public bool IsRegister { get; set; }

        public LoginWindow MainWindow { get; set; }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.turn_back();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            int count = DataProvider.Ins.DB.FUser.Where(p => (p.Username == txtUser.Text)).Count();
            if (count > 0)
            {
                IsRegister = false;
                txtUser.Text = "";
                txtPassword.Password = "";
                txtrePassword.Password = "";
                MessageBox.Show("Username đã tồn tại");
            }
            else if (!test_empty() || !test_password() || !test_length())
            {
                IsRegister = false;
                txtUser.Text = "";
                txtPassword.Password = "";
                txtrePassword.Password = "";
            }
            else
            {
                FUser newUser = new FUser();
                newUser.Username = txtUser.Text;
                newUser.Passwrd = txtPassword.Password;
                newUser.UName = "User";
                newUser.UHeight = 160;
                newUser.UWeight = 60;
                newUser.UStatus = 0;
                newUser.Sex = 2;
                newUser.Age = 18;
                IsRegister = true;
                MainWindow.next_step(newUser);
                // close
            }
        }

        #region Validating

        private bool test_empty()
        {
            if (txtUser.Text == "" || txtPassword.Password == "" || txtrePassword.Password == "")
            {
                MessageBox.Show("Vui lòng không để trống các ô thông tin", "Thông báo");
                return false;
            }
            return true;
        }

        private bool test_password()
        {
            if (txtPassword.Password != txtrePassword.Password)
            {
                MessageBox.Show("Nhập lại mật khẩu không trùng khớp", "Thông báo");
                return false;
            }
            return true;
        }

        private bool test_length()
        {
            if (txtPassword.Password.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải từ 8 kí tự trở lên", "Thông báo");
                return false;
            }
            return true;
        }

        #endregion
    }
}
