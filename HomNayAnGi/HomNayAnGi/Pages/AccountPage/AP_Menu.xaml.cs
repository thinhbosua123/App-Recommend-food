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

namespace Project.UserControlXAML.AccountPage
{
    /// <summary>
    /// Interaction logic for AP_Menu.xaml
    /// </summary>
    public partial class AP_Menu : UserControl
    {
        public string Username 
        {
            get
            {
                if (Project.Pages.AccountPage.CurrentUser == null)
                {
                    return "User";
                }
                else
                {
                    return Project.Pages.AccountPage.CurrentUser.UName;
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

        public AP_Menu(ContentControl mainParent)
        {
            InitializeComponent();
            DataContext = this;
            MainParent = mainParent;
        }

        private void profile_MouseEnter(object sender, MouseEventArgs e)
        {
            button_profile.Margin = new Thickness(0, 15, 0, 5);
        }

        private void profile_MouseLeave(object sender, MouseEventArgs e)
        {
            button_profile.Margin = new Thickness(0, 20, 0, 10);
        }

        private void password_MouseEnter(object sender, MouseEventArgs e)
        {
            button_password.Margin = new Thickness(0, 15, 0, 5);
        }

        private void password_MouseLeave(object sender, MouseEventArgs e)
        {
            button_password.Margin = new Thickness(0, 20, 0, 10);
        }

        private void response_MouseEnter(object sender, MouseEventArgs e)
        {
            button_response.Margin = new Thickness(0, 15, 0, 5);
        }

        private void response_MouseLeave(object sender, MouseEventArgs e)
        {
            button_response.Margin = new Thickness(0, 20, 0, 10);
        }

        private void password_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainParent.Content = new AP_Password(MainParent);
        }

        private void profile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainParent.Content = new AP_Profile(MainParent);
        }

        private void response_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainParent.Content = new AP_Response(MainParent);
        }
    }
}
