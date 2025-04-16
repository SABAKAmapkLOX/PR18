using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PR18
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();
        }
        DispatcherTimer _timer;
        int _countLogin = 1;

        void GetCaptch()
        {
            string masChar = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            string captcha = "";
            Random rnd = new Random();
            for (int i = 0; i <= 6; i++)
            {
                captcha = captcha + masChar[rnd.Next(0, masChar.Length)];
            }
            grid.Visibility = Visibility.Visible;
            txtCaptcha.Text = captcha;
            tbCaptcha.Text = null;
            txtCaptcha.LayoutTransform = new RotateTransform(rnd.Next(-15, 15));
            line.X1 = 10;
            line.Y1 = rnd.Next(10, 40);
            line.X2 = 10;
            line.Y2 = rnd.Next(10, 40);
        }
        public void Window_Activated(object sender, EventArgs e)
        {
            tbLogin.Focus();
            Data.Login = false;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 10);
            _timer.Tick += new EventHandler(timer_Tick);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            stackPanel.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            switch (s.Content)
            {
                case "Войти":Login(); break;
                case "Выйти":Close(); break;
                case "Зайти как гость": LoginGuest(); break;
            }
        }
        public void LoginGuest()
        {
            Data.Login = true;
            Data.UserName = "";
            Data.UserSurname = "Гость";
            Data.UserPartonymic = "";
            Data.Right = "Клиент";
            Close();
        }
        public void Login()
        {
            using (OptSalesContext _db = new OptSalesContext())
            {
                var user = _db.Users.Where(user => user.UserLogin == tbLogin.Text && user.UserPassword == pbPassword.Password);
                if (user.Count() == 1 && txtCaptcha.Text == tbCaptcha.Text)
                {
                    Data.Login = true;
                    Data.UserSurname = user.First().UserSurname;
                    Data.UserName = user.First().UserName;
                    Data.UserPartonymic = user.First().UserPatronymic;
                    _db.Roles.Load();
                    Data.Right = user.First().UserRoleNavigation.RoleName;
                    Close();
                }
                else
                {
                    if (user.Count() == 1)
                    {
                        MessageBox.Show("Капча фигня переделывай");
                    }
                    else
                    {
                        MessageBox.Show("Логин или пароль неверный");
                    }
                    GetCaptch();
                    if (_countLogin >= 2)
                    {
                        stackPanel.IsEnabled = false;
                        _timer.Start();
                    }
                    _countLogin++;
                    tbLogin.Focus();
                }
            }
        }
    }
}
