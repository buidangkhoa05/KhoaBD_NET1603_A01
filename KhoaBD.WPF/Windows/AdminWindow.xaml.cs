
using KhoaBD.WPF.Windows.Car;
using Microsoft.EntityFrameworkCore.Query;
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
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<Customer> customers = new ObservableCollection<Customer>();
        private ObservableCollection<CarInformation> cars = new ObservableCollection<CarInformation>();

        private readonly ICustomerService _customerService;
        private readonly ICarInformationService _carService;
        private ManipulateCustomer _manipulateCustomerWindow;
        private ManipulateCarWindow _manipulateCarWindow;

        public AdminWindow(IServiceProvider serviceProvider,
            ICustomerService customerService,
            ICarInformationService carService
        )
        {
            InitializeComponent();

            _customerService = customerService;
            _serviceProvider = serviceProvider;
            _carService = carService;

            LoadCustomer();
            LoadCar();
        }

        #region Load Customer
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
            _manipulateCustomerWindow = _serviceProvider.GetRequiredService<ManipulateCustomer>();

            var customer = customerDataGrid.SelectedItem as Customer;

            if (customer == null)
            {
                WindowsExtesions.ErrorMessageBox("Please select a customer");
                return;
            }

            _manipulateCustomerWindow.Tag = customer;

            _manipulateCustomerWindow.LoadData4Update();
            var isUpdateSuccess = _manipulateCustomerWindow.ShowDialog() ?? false;

            if (isUpdateSuccess)
            {
                ReloadCustomer();
            }

            _manipulateCustomerWindow = null;
        }

        private void LoadCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadCustomer();
        }

        private void CreateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            _manipulateCustomerWindow = _serviceProvider.GetRequiredService<ManipulateCustomer>();
            _manipulateCustomerWindow.LoadData4Create();
            var isCreateSucces = _manipulateCustomerWindow.ShowDialog() ?? false;

            if (isCreateSucces)
            {
                ReloadCustomer();
            }
            _manipulateCustomerWindow = null;
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var customer = customerDataGrid.SelectedItem as Customer;

            if (customer != null)
            {
                var confirmResult = WindowsExtesions.ConfirmMessageBox($"Do you want delete Customer:{customer.CustomerId} ?");

                if (confirmResult == MessageBoxResult.Yes)
                {
                    var (result, message) = _customerService.Delete(customer.CustomerId);

                    if (result)
                    {
                        WindowsExtesions.SuccessMessageBox("Delete successful");
                        ReloadCustomer();
                    }
                    else
                    {
                        WindowsExtesions.ErrorMessageBox(message);
                    }
                }
            }

        }
        #endregion Load Customer

        #region Car Information 
        void LoadCar()
        {
            carDataGrid.ItemsSource = cars;
            _carService.GetAll().ToList().ForEach(x => cars.Add(x));
        }

        public void ReloadCar()
        {
            if (cars.Any())
            {
                cars = new ObservableCollection<CarInformation>();
            }
            _carService.GetAll().ToList().ForEach(x => cars.Add(x));
            carDataGrid.ItemsSource = cars;
        }
        #endregion Car Information

        private void LoadCarButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadCar();
        }

        private void CreateCarButton_Click(object sender, RoutedEventArgs e)
        {
            _manipulateCarWindow = _serviceProvider.GetRequiredService<ManipulateCarWindow>();
            _manipulateCarWindow.LoadData4Create();
            var isCreateSucces = _manipulateCarWindow.ShowDialog() ?? false;

            if (isCreateSucces)
            {
                ReloadCar();
            }
        }

        private void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            var confirm = WindowsExtesions.ConfirmMessageBox("Do you want delete this car?");
            if (confirm == MessageBoxResult.Yes)
            {
                var car = carDataGrid.SelectedItem as CarInformation;
                var (result, message) = _carService.Delete(car.CarId);

                if (result)
                {
                    WindowsExtesions.SuccessMessageBox("Delete successful");
                    ReloadCar();
                }
                else
                {
                    WindowsExtesions.ErrorMessageBox(message);

                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _manipulateCarWindow = _serviceProvider.GetRequiredService<ManipulateCarWindow>();
            _manipulateCarWindow.Tag = carDataGrid.SelectedItem as CarInformation;
            _manipulateCarWindow.LoadData4Update();
            var isUpdateSucces = _manipulateCarWindow.ShowDialog() ?? false;

            if (isUpdateSucces)
            {
                ReloadCar();
            }
        }
    }
}
