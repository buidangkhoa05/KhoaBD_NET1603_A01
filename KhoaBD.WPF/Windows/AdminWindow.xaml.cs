
using KhoaBD.WPF.Windows.Car;
using KhoaBD.WPF.Windows.RentingTrans;
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
        private ObservableCollection<RentingTransaction> rentingTrans = new ObservableCollection<RentingTransaction>();


        private readonly ICustomerService _customerService;
        private readonly ICarInformationService _carService;
        private readonly IRentinTransactionService _rentinTransactionService;

        private ManipulateCustomer _manipulateCustomerWindow;
        private ManipulateCarWindow _manipulateCarWindow;
        private DetailTransactionWindow _detailTransactionWindow;

        public AdminWindow(IServiceProvider serviceProvider,
            ICustomerService customerService,
            ICarInformationService carService,
            IRentinTransactionService rentinTransactionService
        )
        {
            InitializeComponent();

            _customerService = customerService;
            _serviceProvider = serviceProvider;
            _carService = carService;
            _rentinTransactionService = rentinTransactionService;

            LoadCustomer();
            LoadCar();
            LoadRentTrans();
        }

        #region  Customer
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
        #endregion  Customer

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

        #endregion Car Information

        #region Renting Transaction
        private void LoadRentTrans()
        {
            rentingDataGrid.ItemsSource = rentingTrans;
            _rentinTransactionService.GetAll().ToList().ForEach(r => rentingTrans.Add(r));
        }
        public void ReloadRentTrans()
        {
            if (rentingTrans.Any())
            {
                rentingTrans = new ObservableCollection<RentingTransaction>();
            }
            _rentinTransactionService.GetAll().ToList().ForEach(r => rentingTrans.Add(r));
            rentingDataGrid.ItemsSource = rentingTrans;
        }
        private void ViewDetailButton_Click(object sender, RoutedEventArgs e)
        {
            var rentingTrans = rentingDataGrid.SelectedItem as RentingTransaction;
            if (rentingTrans == null)
            {
                WindowsExtesions.ErrorMessageBox("Vui lòng chọn giao dịch để xem chi tiết");
                return;
            }
            _detailTransactionWindow = _serviceProvider.GetRequiredService<DetailTransactionWindow>();
            _detailTransactionWindow.Tag = rentingTrans;
            _detailTransactionWindow.LoadData();
            var result = _detailTransactionWindow.ShowDialog();

        }

        #endregion Renting Transaction

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rentingTrans = rentingDataGrid.SelectedItem as RentingTransaction;
            var (result, message) = _rentinTransactionService.DeleteTrans(rentingTrans.RentingTransationId);
            if (result)
            {
                WindowsExtesions.SuccessMessageBox("Delete successful");
                ReloadRentTrans();
            }
            else
            {
                WindowsExtesions.ErrorMessageBox(message);
            }
        }
    }
}
