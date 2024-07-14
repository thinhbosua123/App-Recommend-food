using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Interaction logic for ForgetPassPage.xaml
    /// </summary>
    public partial class ForgetPassPage : Page
    {
        public string Text { get; set; }
        public LoginWindow Main;
        string randomCode;
        public static string to;
        private FUser user;
        public ForgetPassPage(LoginWindow login)
        {
            InitializeComponent();
            this.DataContext = this;
            user = new FUser();
            Text = null;
            Main = login;
        }

        private void GetPass_Click(object sender, RoutedEventArgs e)
        {
            if (Text == null) return;
            string from, pass, messageBody , NewPass;
            bool UserExist = true, UserMailExist = true;
            Random rand = new Random();
            randomCode = (rand.Next(999999)).ToString();
            NewPass = "pa$$" + randomCode.ToString();
            MailMessage message = new MailMessage();
            to = (Mail_txt.Text).ToString();
            from = "todaywhateat008@gmail.com";
            pass = "plgyxsgchkhpbtqb";
            messageBody = "Mật khẩu mới của bạn là : " + NewPass;
            try
            {
                message.To.Add(to);
            }
            catch
            {
                UserMailExist = false;
            }
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Mật khẩu mới";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            user = DataProvider.Ins.DB.FUser.SingleOrDefault(p => p.Username == Acc_txt.Text);
            if (user != null)
            {
                user.Passwrd = NewPass;
                DataProvider.Ins.DB.SaveChanges();
            }
            else UserExist = false;

            if(!UserExist || !UserMailExist)
            {
                MessageBox.Show("Tài khoản của bạn không tồn tại hoặc mail của bạn không đúng !", "Thông báo" , MessageBoxButton.OK , MessageBoxImage.Error);
                return;
            }
            try
            {
                smtp.Send(message);
                if (MessageBox.Show("Mật khẩu mới đã được gửi đến mail của bạn !", "Thông báo") == MessageBoxResult.OK)
                {
                    Main.turn_back();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Main.turn_back();
        }
    }
}
