using BDDLL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
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

namespace BankApp
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        Database db = null;
        string connectionString = "Data Source=DESKTOP-8P06H53;Initial Catalog=АфонькинУП4_2;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        public RegWindow()
        {
            InitializeComponent();
            db = Database.GetInstance(connectionString);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string clientQuery = "INSERT INTO Клиенты VALUES (@ФИО, @НомерПаспорта, @Адрес, @Телефон)";
            db.Execute(clientQuery, new Parameter[] { new Parameter("ФИО", FIO.Text), new Parameter("НомерПаспорта", Passport.Text), new Parameter("Адрес", Address.Text), new Parameter("Телефон", Phone.Text) });

            int clientID = (int)db.GetScalar("SELECT COUNT(Код) FROM Клиенты");
            string userQuery = "INSERT INTO Users VALUES (@Login, @Password, 3, 0, @ClientID)";
            db.Execute(userQuery, new Parameter[] { new Parameter("Login", Login.Text), new Parameter("Password", Password.Text), new Parameter("ClientID", clientID) });
            
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
