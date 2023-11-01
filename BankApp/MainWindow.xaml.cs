using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
using BDDLL;

namespace BankApp
{

    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=DESKTOP-8P06H53;Initial Catalog=АфонькинУП4_2;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        Database db = null;
        public List<Predicate<Вклады>> depositFilter = new List<Predicate<Вклады>>();

        public enum Operation
        {
            Add, Update, Delete, Filter
        }

        string[] userIcons = new string[] { "defaultUser.png", "man.png", "man2.png", "woman.png", "woman2.png" };
        LoginData loginData;

        public MainWindow(LoginData loginData)
        {
            InitializeComponent();
            db = Database.GetInstance(connectionString);
            this.loginData = loginData;
            WelcomeLB.Content = "Login: " + loginData.Login + "\n\nRole: " + loginData.Role;
            UpdateImage();
            UpdateTables();

            if(loginData.Role == "Admin")
            {
                TabMyAccounts.Visibility = Visibility.Collapsed;
            }
            else if(loginData.Role == "Employee")
            {
                TabUsers.Visibility = Visibility.Collapsed;
                TabMyAccounts.Visibility = Visibility.Collapsed;
                TabHistory.Visibility = Visibility.Collapsed;
            }
            else if (loginData.Role == "Client")
            {
                TabUsers.Visibility = Visibility.Collapsed;
                TabClients.Visibility = Visibility.Collapsed;
                TabCurrencies.Visibility = Visibility.Collapsed;
                TabAccounts.Visibility = Visibility.Collapsed;
                TabHistory.Visibility = Visibility.Collapsed;
                ControlButtons.Visibility = Visibility.Collapsed;
                ControlButtons2.Visibility = Visibility.Collapsed;
            }
        }

        void UpdateImage()
        {
            WelcomeImage.Source = new BitmapImage(new Uri("/Resources/" + userIcons[loginData.Icon], UriKind.Relative));
        }

        private void WelcomeImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            loginData.Icon = loginData.Icon == 4 ? 0 : loginData.Icon + 1;
            string iconQuery = "UPDATE Users SET ImageID = @ImageID WHERE Login = @Login";
            db.Execute(iconQuery, new Parameter[] { new Parameter("Login", loginData.Login), new Parameter("ImageID", loginData.Icon) });
            UpdateImage();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new AuthorizationWindow().Show();
        }

        private void UpdateTables()
        {
            var Deposits = BankBaseEntities.GetContext().Вклады.ToList();
            foreach (var filter in depositFilter)
            {
                Deposits = Deposits.FindAll(filter);
            }
            DGridDeposits.ItemsSource = Deposits;

            DGridAccounts.ItemsSource = BankBaseEntities.GetContext().Счета.ToList();
            DGridClients.ItemsSource = BankBaseEntities.GetContext().Клиенты.ToList();
            DGridCurrencies.ItemsSource = BankBaseEntities.GetContext().Валюты.ToList();
            DGridUsers.ItemsSource = BankBaseEntities.GetContext().Users.ToList();
            DGridHistory.ItemsSource = BankBaseEntities.GetContext().Histories.ToList();
            DGridUserAccounts.ItemsSource = BankBaseEntities.GetContext().Счета.ToList().FindAll(p => p.Код_клиента == loginData.ClientID);
        }

        public void UpdateBD()
        {
            BankBaseEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            UpdateTables();
        }

        private void DepositAddBtn_Click(object sender, RoutedEventArgs e)
        {
            new DepositWindow(Operation.Add, this).Show();
        }

        public void DepositEditBtn_Click(object sender, RoutedEventArgs e)
        {
            new DepositWindow(Operation.Update, this, (sender as Button).DataContext as Вклады).Show();
        }

        public void DepositDelBtn_Click(object sender, RoutedEventArgs e)
        {
            var depositsToDelete = DGridDeposits.SelectedItems.Cast<Вклады>().ToList();

            if(MessageBox.Show($"Вы точно хотите удалить следующие {depositsToDelete.Count} элементов?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                try
                {
                    BankBaseEntities.GetContext().Вклады.RemoveRange(depositsToDelete);
                    BankBaseEntities.GetContext().SaveChanges();
                    UpdateBD();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DepositFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            new DepositWindow(Operation.Filter, this).Show();
        }

        private void DepositFilterDelBtn_Click(object sender, RoutedEventArgs e)
        {
            depositFilter.Clear();
            UpdateBD();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateBD();
        }

        private void UserAccountInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            new AccountInfo(this, (sender as Button).DataContext as Счета).Show();
        }

        private void CreateUserAccount(object sender, RoutedEventArgs e)
        {

        }
    }
}
