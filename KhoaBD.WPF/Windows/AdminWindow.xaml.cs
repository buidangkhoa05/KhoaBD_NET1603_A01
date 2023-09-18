using PRN221.Application.Service.Implement;
using PRN221.Application.Service.Interface;
using PRN221.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace KhoaBD.WPF.Windows
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();

        private readonly ICustomerService _customerService;

        public AdminWindow(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
            LoadCustomer();
        }

        public void LoadCustomer()
        {
            customerDataGrid.ItemsSource = customers;
            _customerService.GetAll().ToList().ForEach(x => customers.Add(x));
        }

        public void ReloadCustomer()
        {
            if (customers.Any())
            {
                customers = new ObservableCollection<Customer>();
            }
            _customerService.GetAll().ToList().ForEach(x => customers.Add(x));
            customerDataGrid.ItemsSource = customers;
        }

        private void EditCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = customerDataGrid.SelectedItem as Customer;

            var confirmResult = WindowsExtesions.ConfirmMessageBox("Do you want update?");

            if (confirmResult == MessageBoxResult.Yes)
            {
                var (result, message) = _customerService.Update(customer);

                if (result)
                {
                    WindowsExtesions.SuccessMessageBox("Update successful");
                    ReloadCustomer();
                }
                else
                {
                    WindowsExtesions.ErrorMessageBox(message);
                }
            }
        }

        private void LoadCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadCustomer();
        }
    }
}
