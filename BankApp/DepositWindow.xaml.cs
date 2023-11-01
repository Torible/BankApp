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
    /// <summary>
    /// Логика взаимодействия для DepositWindow.xaml
    /// </summary>
    public partial class DepositWindow : Window
    {
        MainWindow.Operation currentOperation;
        MainWindow mainWindow;
        

        Вклады currentDeposit = null;

        public DepositWindow(MainWindow.Operation currentOperation, MainWindow mainWindow, Вклады currentDeposit = null)
        {
            InitializeComponent();
            this.currentOperation = currentOperation;
            this.mainWindow = mainWindow;
            if (this.currentOperation == MainWindow.Operation.Filter)
            {
                SaveBtn.Visibility = Visibility.Collapsed;
                ID.Text = string.Empty;
            }
            else
            {
                FilterBtn.Visibility = Visibility.Collapsed;
                ID.IsEnabled = false;
            }

            if(currentDeposit == null)
            {
                this.currentDeposit = new Вклады();
            }
            else
            {
                this.currentDeposit = currentDeposit;
            }

            DataContext = this.currentDeposit;
            Currency.ItemsSource = BankBaseEntities.GetContext().Валюты.ToList();
            DepositType.ItemsSource = BankBaseEntities.GetContext().Типы_вкладов.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if(currentOperation == MainWindow.Operation.Add)
            {
                BankBaseEntities.GetContext().Вклады.Add(currentDeposit);
            }
            else if(currentOperation == MainWindow.Operation.Update)
            {
                BankBaseEntities.GetContext().Вклады.AddOrUpdate(currentDeposit);
            }

            try 
            {
                BankBaseEntities.GetContext().SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }

        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.depositFilter.Clear();
            if (!string.IsNullOrEmpty(ID.Text))
            {
                mainWindow.depositFilter.Add(p => p.Код == int.Parse(ID.Text));
            }
            if (!string.IsNullOrEmpty(Naming.Text))
            {
                mainWindow.depositFilter.Add(p => p.Наименование == Naming.Text);
            }
            if (!string.IsNullOrEmpty(Period.Text))
            {
                mainWindow.depositFilter.Add(p => p.Срок == int.Parse(Period.Text));
            }
            if (!string.IsNullOrEmpty(Percent.Text))
            {
                mainWindow.depositFilter.Add(p => p.Ставка == double.Parse(Percent.Text));
            }
            if (Currency.SelectedItem != null)
            {
                mainWindow.depositFilter.Add(p => p.Валюты.Код == ((Валюты)Currency.SelectedItem).Код);
            }
            if (DepositType.SelectedItem != null)
            {
                mainWindow.depositFilter.Add(p => p.Типы_вкладов.Код == ((Типы_вкладов)DepositType.SelectedItem).Код);
            }
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
