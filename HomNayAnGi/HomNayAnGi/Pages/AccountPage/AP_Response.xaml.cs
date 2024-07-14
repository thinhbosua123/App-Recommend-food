using Microsoft.Win32;
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
using Project.Pages;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Project.UserControlXAML.AccountPage
{
    /// <summary>
    /// Interaction logic for AP_Password.xaml
    /// </summary>
    public partial class AP_Response : UserControl
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

        string imagePath = "";

        ContentControl MainParent = null;

        public AP_Response(ContentControl mainParent)
        {
            InitializeComponent();
            DataContext = this;
            MainParent = mainParent;
        }

        #region Checking

        private bool test_empty()
        {
            if (Response.Text == "")
            {
                MessageBox.Show("Vui lòng không để trống textbox!", "Thông báo");
                return false;
            }
            return true;
        }

        private void close_tag()
        {
            MainParent.Content = new AP_Menu(MainParent);
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

        private void imageBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog image = new OpenFileDialog();
            image.Title = "Hãy chọn 1 tấm ảnh";
            image.Filter = "Image (*.jpeg;*.png;*.jpg;*.gif)|*.jpeg;*.png;*.jpg;*.gif";
            if (image.ShowDialog() == true)
            {
                ResponseImage.ImageSource = new BitmapImage(new Uri(image.FileName));
                imagePath = image.FileName;
            }
        }

        private void send_Click(object sender, RoutedEventArgs e)
        {
            if (test_empty())
            {
                SendResponse();
            }
        }

        private void deleteImage_Click(object sender, RoutedEventArgs e)
        {
            ResponseImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Assets/Account/clip.png"));
            imagePath = "";
        }
        #endregion

        #region Other
        // send response
        private void SendResponse()
        {
            string from, to, pass, messageBody;
            MailMessage message = new MailMessage();
            to = "mealfeedback@gmail.com";
            from = "todaywhateat008@gmail.com";
            pass = "plgyxsgchkhpbtqb";
            FUser user = Project.Pages.AccountPage.CurrentUser;

            messageBody = "Account: " + user.Username + "\n" +
                            "Mail: " + Mail_txt.Text + "\n" +
                            "ID: " + user.UserID + "\n" +
                            "Username: " + user.UName + "\n" +
                            "===============================================" + "\n" +
                            "Response: " + "\n" + Response.Text;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Subject = "User " + user.Username + " response";
            message.Body = messageBody;
            if (imagePath != "")
            {
                message.Attachments.Add(new Attachment(imagePath));
            }
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(message);
                if (MessageBox.Show("Gửi phản hồi thành công !", "Thông báo") == MessageBoxResult.OK)
                {
                    close_tag();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


    }
}
