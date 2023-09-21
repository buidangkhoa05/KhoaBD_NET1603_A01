using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for ManipulateCustomer.xaml
    /// </summary>
    public partial class ManipulateCustomer : Window
    {
        private bool isUpdateWindow = false;
        private readonly ICustomerService _customerService;
        private int _customerId;

        public ManipulateCustomer(ICustomerService customerService)
        {
            InitializeComponent();

            _customerService = customerService;
        }

        public void LoadData4Create()
        {
            tbActionName.Text = "Create Customer";
            btnAction.Content = "Create";
        }

        public void LoadData4Update()
        {
            var customer = this.Tag as Customer;
            tbActionName.Text = "Update Customer";
            btnAction.Content = "Update";

            if (customer != null)
            {
                _customerId = customer.CustomerId;
                CustomerNameTextBox.Text = customer.CustomerName;
                TelephoneTextBox.Text = customer.Telephone;
                EmailTextBox.Text = customer.Email;
                BirthdayDatePicker.SelectedDate = customer.CustomerBirthday;
                Password.Text = customer.Password;
                CustomerStatus.Text = customer.CustomerStatus.ToString();
            }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsByte(CustomerStatus.Text))
            {
                Customer customer = new Customer
                {
                    CustomerName = CustomerNameTextBox.Text,
                    Telephone = TelephoneTextBox.Text,
                    Email = EmailTextBox.Text,
                    CustomerBirthday = BirthdayDatePicker.SelectedDate,
                    Password = Password.Text,
                    CustomerStatus = byte.Parse(CustomerStatus.Text)
                };

                bool result = false;
                string message = string.Empty;

                if (this.Tag != null)
                { // update
                    var confirmResult = WindowsExtesions.ConfirmMessageBox("Do you want update?");
                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        customer.CustomerId = _customerId;
                        (result, message) = _customerService.Update(customer);

                        if (result)
                        {
                            WindowsExtesions.SuccessMessageBox("Update successful");
                            this.DialogResult = true;
                        }
                        else
                        {
                            WindowsExtesions.ErrorMessageBox(message);
                        }
                    }
                }
                else
                { // create
                    var confirmResult = WindowsExtesions.ConfirmMessageBox("Do you want create?");
                    if (confirmResult == MessageBoxResult.Yes)
                    {
                        (result, message) = _customerService.Create(customer);

                        if (result)
                        {
                            WindowsExtesions.SuccessMessageBox("Create successful");
                            this.DialogResult = true;
                        }
                        else
                        {
                            WindowsExtesions.ErrorMessageBox(message);
                        }
                    }
                }
            }
            else
            {
                WindowsExtesions.ErrorMessageBox("CustomerStatus is invalid");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private bool IsByte(string text)
        {
            return byte.TryParse(text, out _);
        }
    }
}
