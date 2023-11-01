using BDDLL;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
using System.Windows.Forms;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using MessageBox = System.Windows.MessageBox;

namespace BankApp
{
    public partial class AuthorizationWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-8P06H53;Initial Catalog=АфонькинУП4_2;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        private string captcha = String.Empty;
        bool isCaptchaActive = false;
        Database db = null;
        Timer captchaTimer;
        bool isCaptchaDelayed = true;
        int captchaTimerInterval = 0;
        int captchaCurrentTime = 0;
        public AuthorizationWindow()
        {
            InitializeComponent();
            db = Database.GetInstance(connectionString);
            captchaTimer = new Timer();
            captchaTimer.Interval = 1000;
            captchaTimer.Tick += CaptchaTimer_Tick;
        }

        private void CaptchaTimer_Tick(object sender, EventArgs e)
        {
            if (captchaCurrentTime > 0)
            {
                captchaTimerLB.Content = "Seconds for next try: " + captchaCurrentTime;
                captchaCurrentTime--;
                captchaTimerLB.Visibility = Visibility.Visible;
            }
            else
            {
                captchaTimer.Stop();
                captchaTimerLB.Visibility = Visibility.Hidden;
                isCaptchaDelayed = true;
            }
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (!isCaptchaDelayed)
                return;

            string query = "SELECT Role FROM Users, Roles WHERE Login = @Login AND Password = @Password AND Users.RoleID = Roles.RoleID";
            string role = (string)db.GetScalar(query, new Parameter[] { new Parameter("Login", loginTB.Text), new Parameter("Password", passwordTB.Password) });
            string query2 = "SELECT ClientID FROM Users WHERE Login = @Login";
            object clientID = db.GetScalar(query2, new Parameter[] { new Parameter("Login", loginTB.Text)});
            string iconQuery = "SELECT ImageID FROM Users WHERE Login = @Login AND Password = @Password";
            object iconObj = db.GetScalar(iconQuery, new Parameter[] { new Parameter("Login", loginTB.Text), new Parameter("Password", passwordTB.Password) });
            int icon = iconObj != null ? (int)iconObj : 0;
            int? tempClientID = clientID != null ? clientID.GetType() == typeof(DBNull) ? null : (int?)clientID : null;
            if (isCaptchaActive && captchaTB.Text != captcha)
            {
                MessageBox.Show("Неверно введена captcha");
                captchaImage.Source = BitmapToImageSource(CreateImage(100, 50));
                captchaTB.Text = string.Empty;
                
                if (captchaTimerInterval < 60)
                    captchaTimerInterval += 2;
                captchaCurrentTime = captchaTimerInterval;
                isCaptchaDelayed = false;
                captchaTimer.Start();
                return;
            }
            if (role != null)
            {
                string historyQuery = "INSERT INTO History VALUES (@Login, @DateTime, @Succsess)";
                db.Execute(historyQuery, new Parameter[] { new Parameter("Login", loginTB.Text), new Parameter("DateTime", DateTime.Now.ToString()), new Parameter("Succsess", true) });

                new MainWindow(new LoginData(loginTB.Text, role, icon, (int?)tempClientID)).Show();
                Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
                captchaImage.Source = BitmapToImageSource(CreateImage(100, 50));
                captchaImage.Visibility = Visibility.Visible;
                captchaTB.Visibility = Visibility.Visible;
                captchaButton.Visibility = Visibility.Visible;
                isCaptchaActive = true;
                captchaTB.Text = string.Empty;

                string isLoginCorrectQuery = "SELECT * FROM Users WHERE Login = @Login";
                int isLoginCorrect = db.Execute(isLoginCorrectQuery, new Parameter[] { new Parameter("Login", loginTB.Text)});
                if(isLoginCorrect != 0)
                {
                    string historyQuery = "INSERT INTO History VALUES (@Login, @DateTime, @Succsess)";
                    db.Execute(historyQuery, new Parameter[] { new Parameter("Login", loginTB.Text), new Parameter("DateTime", DateTime.Now.ToString()), new Parameter("Succsess", false) });
                }
            }
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);

            //Добавим различные цвета
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Сгенерируем текст
            captcha = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                captcha += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(captcha,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            //Добавим немного помех
            /////Линии из углов
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            ////Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, System.Drawing.Color.White);

            return result;
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void captchaButton_Click(object sender, RoutedEventArgs e)
        {
            captchaImage.Source = BitmapToImageSource(CreateImage(100, 50));
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            new RegWindow().Show();
        }
    }
}