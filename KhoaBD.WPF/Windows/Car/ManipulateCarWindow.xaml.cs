using LinqKit;
using PRN221.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KhoaBD.WPF.Windows.Car
{
    /// <summary>
    /// Interaction logic for ManipulateCarWindow.xaml
    /// </summary>
    public partial class ManipulateCarWindow : Window
    {
        private IEnumerable<Manufacturer> Manufacturers { get; set; }
        private IEnumerable<Supplier> Suppliers { get; set; }

        private readonly IManufacturerService _manufacturerService;
        private readonly ISupplierService _supplierService;
        private readonly ICarInformationService _carInformationService;

        public ManipulateCarWindow(IManufacturerService manufacturerService,
            ISupplierService supplierService,
            ICarInformationService carInformationService)
        {
            InitializeComponent();

            _manufacturerService = manufacturerService;
            _supplierService = supplierService;

            Suppliers = supplierService.GetAll();
            Manufacturers = manufacturerService.GetAll();
            _carInformationService = carInformationService;
        }

        public void LoadData4Create()
        {
            tbActionName.Text = "Create Car";
            btnAction.Content = "Create";

            cbManufacturer.Items.Clear();
            cbSupplier.Items.Clear();

            cbManufacturer.SelectedIndex = 0;
            cbSupplier.SelectedIndex = 0;

            Manufacturers.ForEach(m =>
            {
                cbManufacturer.Items.Add(new ComboBoxItem
                {
                    Content = m.ManufacturerName,
                });
            });
            Suppliers.ForEach(s => cbSupplier.Items.Add(new ComboBoxItem
            {
                Content = s.SupplierName
            }));
        }

        public void LoadData4Update()
        {
            tbActionName.Text = "Update Car";
            btnAction.Content = "Update";

            cbManufacturer.Items.Clear();
            cbSupplier.Items.Clear();

            var car = this.Tag as CarInformation;

            if (car != null)
            {
                tbCarName.Text = car.CarName;
                tbCarDescription.Text = car.CarDescription;
                tbNumerDoor.Text = car.NumberOfDoors.ToString();
                tbSeatingCapacity.Text = car.SeatingCapacity.ToString();
                tbFuelType.Text = car.FuelType;
                tbCarStatus.Text = car.CarStatus.ToString();
                tbYear.Text = car.Year.ToString();
                tbCarRentingPricePerDay.Text = car.CarRentingPricePerDay.ToString();

                var manufacturer = Manufacturers.FirstOrDefault(m => m.ManufacturerId == car.Manufacturer.ManufacturerId);
                var supplier = Suppliers.FirstOrDefault(s => s.SupplierId == car.Supplier.SupplierId);

                Manufacturers.ForEach(m =>
                {
                    cbManufacturer.Items.Add(new ComboBoxItem
                    {
                        Content = m.ManufacturerName,
                    });
                });
                Suppliers.ForEach(s => cbSupplier.Items.Add(new ComboBoxItem
                {
                    Content = s.SupplierName
                }));

                var matchingItem = cbManufacturer.Items.Cast<ComboBoxItem>().FirstOrDefault(m => m.Content == manufacturer.ManufacturerName);

                cbManufacturer.SelectedIndex = cbManufacturer.Items.IndexOf(matchingItem);

                var matchingItem1 = cbSupplier.Items.Cast<ComboBoxItem>().FirstOrDefault(m => m.Content == supplier.SupplierName);

                cbSupplier.SelectedIndex = cbSupplier.Items.IndexOf(matchingItem1);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbManufacturer.SelectedItem != null)
            {
                string selectedOption = ((ComboBoxItem)cbManufacturer.SelectedItem).Content.ToString();

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            var carName = tbCarName.Text;
            var carDescription = tbCarDescription.Text;
            var numberDoor = tbNumerDoor.Text;
            if (!Helpers.IsNumberPositive(numberDoor))
            {
                WindowsExtesions.ErrorMessageBox("Number door must be positive number");
                return;
            }
            var seatingCapacity = tbSeatingCapacity.Text;
            if (!Helpers.IsNumberPositive(seatingCapacity))
            {
                WindowsExtesions.ErrorMessageBox("Seating capacity must be positive number");
                return;
            }
            var fuelType = tbFuelType.Text;
            var carStatus = tbCarStatus.Text;
            if (!Helpers.IsByte(carStatus))
            {
                WindowsExtesions.ErrorMessageBox("CarStatus is invalid");
                return;
            }
            var year = tbYear.Text;
            if (!Helpers.IsYear(year))
            {
                WindowsExtesions.ErrorMessageBox("Year is invalid");
                return;
            }
            var carRentingPriceDay = tbCarRentingPricePerDay.Text;
            if (!Helpers.IsDecimal(carRentingPriceDay))
            {
                WindowsExtesions.ErrorMessageBox("Car renting price per day is invalid");
                return;
            }

            var manufacturerName = ((ComboBoxItem)cbManufacturer.SelectedItem).Content.ToString();
            var manufacturer = Manufacturers.FirstOrDefault(m => m.ManufacturerName == manufacturerName);
            var supplierName = ((ComboBoxItem)cbSupplier.SelectedItem).Content.ToString();
            var supplier = Suppliers.FirstOrDefault(s => s.SupplierName == supplierName);

            var car = new CarInformation
            {
                CarDescription = carDescription,
                CarName = carName,
                CarRentingPricePerDay = decimal.Parse(carRentingPriceDay),
                CarStatus = byte.Parse(carStatus),
                FuelType = fuelType,
                NumberOfDoors = int.Parse(numberDoor),
                SeatingCapacity = int.Parse(seatingCapacity),
                ManufacturerId = manufacturer.ManufacturerId,
                //Manufacturer = manufacturer,
                //Supplier = supplier,
                SupplierId = supplier.SupplierId,
                Year = int.Parse(year)
            };


            if (this.Tag != null)
            {// Update
                var confirm = WindowsExtesions.ConfirmMessageBox("Do you want update?");
                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }

                car.CarId = ((CarInformation)this.Tag).CarId;

                var (result, message) = _carInformationService.Update(car);
                if (result)
                {
                    this.DialogResult = true;
                    WindowsExtesions.SuccessMessageBox("Update successful");
                    return;
                }
                else
                {
                    WindowsExtesions.ErrorMessageBox(message);
                }

            }
            else
            {//Create
                var confirm = WindowsExtesions.ConfirmMessageBox("Do you want create?");
                if (confirm != MessageBoxResult.Yes)
                {
                    return;
                }
                var (result, message) = _carInformationService.Create(car);
                if (result)
                {
                    this.DialogResult = true;
                    WindowsExtesions.SuccessMessageBox("Create successful");
                    return;
                }
                else
                {
                    WindowsExtesions.ErrorMessageBox(message);
                }
            }
        }


    }
}
