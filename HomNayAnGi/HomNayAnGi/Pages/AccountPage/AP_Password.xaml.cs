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
using Project.Pages;
using Project.Model;

namespace Project.UserControlXAML.AccountPage
{
    /// <summary>
    /// Interaction logic for AP_Password.xaml
    /// </summary>
    public partial class AP_Password : UserControl
    {
        public string Username
        {
            get
            {
                if (Pages.AccountPage.CurrentUser == null)
                {
                    return "User";
                }
                else
                {
                    return Pages.AccountPage.CurrentUser.UName;
                }
            }
        }

        public string ShowAvatar
        {
            get
            {
                if (!AP_Profile.test_avatar_path(Project.Pages.AccountPage.AvatarLink))
                {
                    return Project.Pages.AccountPage.defaultAvatar;
                }
                return Project.Pages.AccountPage.AvatarLink;
            }
        }

        ContentControl MainParent = null;

        public AP_Password(ContentControl mainParent)
        {
            InitializeComponent();
            DataContext = this;
            MainParent = mainParent;
        }

        #region Checking

        private bool test_empty()
        {
            if (old_password.Password == "" || new_password1.Password == "" || new_password2.Password == "")
            {
                MessageBox.Show("Vui lòng không để trống các ô thông tin","Thông báo");
                return false;
            }
            return true;
        }

        private bool test_old_password()
        {
            if (old_password.Password != Pages.AccountPage.CurrentUser.Passwrd)
            {
                MessageBox.Show("Mật khẩu cũ không đúng", "Thông báo");
                return false;
            }
            return true;
        }

        private bool test_new_password()
        {
            if (new_password1.Password != new_password2.Password)
            {
                MessageBox.Show("Nhập lại mật khẩu mới không trùng khớp", "Thông báo");
                return false;
            }
            return true;
        }

        private bool test_old_new_password()
        {
            if (old_password.Password == new_password1.Password)
            {
                MessageBox.Show("Bạn đang nhập lại mật khẩu hiện tại.\nVui lòng nhập mật khẩu mới.", "Thông báo");
                return false;
            }
            return true;
        }

        private bool test_length()
        {
            if (new_password1.Password.Length < 8)
            {
                MessageBox.Show("Mật khẩu phải từ 8 kí tự trở lên", "Thông báo");
                return false;
            }
            return true;
        }

        private void close_tag()
        {
            MainParent.Content = new AP_Menu(MainParent);
        }

        private void change_password()
        {
            Pages.AccountPage.CurrentUser.Passwrd = new_password1.Password;
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Bạn đã đổi mật khẩu thành công!", "Thông báo");
            close_tag();
        }

        private void empty_Textbox()
        {
            old_password.Clear();
            new_password1.Clear();
            new_password2.Clear();
        }
        #endregion

        #region Event

        private void Back_MouseEnter(object sender, MouseEventArgs e)
        {
            button_back.Width = 56;
            button_back.Height = 56;
            button_back.Margin = new Thickness(6, 6, 0, 0);
        }

        private void Back_MouseLeave(object sender, MouseEventArgs e)
        {
            button_back.Width = 48;
            button_back.Height = 48;
            button_back.Margin = new Thickness(10, 10, 0, 0);
        }

        private void Back_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            close_tag();
        }

        private void changePass_Click(object sender, RoutedEventArgs e)
        {
            if (test_empty() && test_old_password() && test_new_password() && test_length() && test_old_new_password())
            {
                change_password();
            }
            else
            {
                empty_Textbox();
            }
        }
        #endregion


    }
}
