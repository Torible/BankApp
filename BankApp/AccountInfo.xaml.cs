using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    public partial class AccountInfo : Window
    {
        public AccountInfo()
        {
            InitializeComponent();
        }

        MainWindow.Operation currentOperation;
        MainWindow mainWindow;


        Счета currentAccount = null;

        public AccountInfo(MainWindow mainWindow, Счета currentAccount)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            this.currentAccount = currentAccount;
            DataContext = this.currentAccount;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BankBaseEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.UpdateBD();
        }
    }
}