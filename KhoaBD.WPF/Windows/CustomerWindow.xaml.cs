using PRN221.Domain.Models;
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
using System.Windows.Shapes;

namespace KhoaBD.WPF.Windows
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {

        private readonly ICustomerService _customerService;
        public CustomerWindow(ICustomerService customerService)
        {
            InitializeComponent();

            _customerService = customerService;
        }

        public void LoadCustomer()
        {
            var customerMail = this.Tag as string;

            var customer = _customerService.GetAll().FirstOrDefault(x => x.Email == customerMail);

            tbCustomerID.Text = customer.CustomerId.ToString();
            tbCustomerName.Text = customer.CustomerName;
            tbPhone.Text = customer.Telephone;
            tbMail.Text = customer.Email;
            dbBirdDate.SelectedDate = customer.CustomerBirthday;
            tbStatus.Text = customer.CustomerStatus.ToString();
            tbPass.Text = customer.Password;

            LoadData();
        }

        private void ViewDetailButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var confirm = WindowsExtesions.ConfirmMessageBox("Are you sure to update this customer?");

            if (!(confirm == MessageBoxResult.Yes))
            {
                return;
            }

            var customer = new Customer()
            {
                CustomerId = int.Parse(tbCustomerID.Text),
                CustomerName = tbCustomerName.Text,
                Email = tbMail.Text,
                Telephone = tbPhone.Text,
                CustomerBirthday = dbBirdDate.SelectedDate,
                CustomerStatus = byte.Parse(tbStatus.Text),
                Password = tbPass.Text
            };

            var (result, message) = _customerService.Update(customer);

            if (result)
            {
                WindowsExtesions.SuccessMessageBox("Update succesful");
                LoadCustomer();
            }
            else
            {
                WindowsExtesions.ErrorMessageBox(message);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomer();
        }


        public void LoadData()
        {
            var rentingTransaction = _customerService.GetById(int.Parse(tbCustomerID.Text)).RentingTransactions;
            if (rentingTransaction != null)
            {
                rentingDataGrid.ItemsSource = rentingTransaction;
            }
        }
    }
}
