using LinqKit;
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

        public ManipulateCarWindow(IManufacturerService manufacturerService,
            ISupplierService supplierService)
        {
            InitializeComponent();

            _manufacturerService = manufacturerService;
            _supplierService = supplierService;

            Suppliers = supplierService.GetAll();
            Manufacturers = manufacturerService.GetAll();
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
            if (this.Tag != null)
            {// Update
                
            }
            else
            {

            }
        }
    }
}
