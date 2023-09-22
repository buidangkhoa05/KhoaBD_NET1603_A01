using PRN221.Application.Common;
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

namespace KhoaBD.WPF.Windows.RentingTrans
{
    /// <summary>
    /// Interaction logic for DetailTransactionWindow.xaml
    /// </summary>
    public partial class DetailTransactionWindow : Window
    {
        private ICollection<RentingDetail> _rentingDetails = new List<RentingDetail>();
        private RentingTransaction _rentingTransaction;

        private readonly IRentinTransactionService _rentinTransactionService;
        private readonly ICarInformationService _carInformationService;

        public DetailTransactionWindow(IRentinTransactionService rentinTransactionService,
            ICarInformationService carInformationService)
        {
            InitializeComponent();
            _rentinTransactionService = rentinTransactionService;
            _carInformationService = carInformationService;
            LoadData();
        }

        public void LoadData()
        {
            _rentingTransaction = this.Tag as RentingTransaction;
            if (_rentingTransaction != null)
            {
                rentingDetailDataGrid.ItemsSource = _rentingDetails;
                _rentinTransactionService.GetAllFullField(_rentingTransaction.RentingTransationId)
                    .FirstOrDefault().RentingDetails
                    .ToList()
                    .ForEach(r => _rentingDetails.Add(r));

                this.tbRentingTransID.Text = _rentingTransaction.RentingTransationId.ToString();
                this.dpRentingDate.SelectedDate = _rentingTransaction.RentingDate.GetValueOrDefault();
                this.tbCustomerID.Text = _rentingTransaction.CustomerId.ToString();
                this.tbTotalPrice.Text = _rentingTransaction.TotalPrice.ToString();
                this.tbRentingStatus.Text = _rentingTransaction.RentingStatus.ToString();
            }
        }

        public RentingTransaction Bindingdata()
        {
            if (_rentingTransaction == null)
            {
                _rentingTransaction = new RentingTransaction();
            }
            if (this.dpRentingDate.SelectedDate == null)
            {
                WindowsExtesions.ErrorMessageBox("Please select renting date");
                return null;
            }
            if (!Helpers.IsDecimal(tbTotalPrice.Text))
            {
                WindowsExtesions.ErrorMessageBox("Price is invalid");
                return null;
            }
            if (!Helpers.IsByte(tbRentingStatus.Text))
            {
                WindowsExtesions.ErrorMessageBox("Renting Status is invalid");
                return null;
            }
            var cars = _carInformationService.GetAll();
            var carsIdValidate = _rentingDetails.ToList().Select(c => c.CarId).All(carId => cars.Any(i => i.CarId == carId));
            if (carsIdValidate == false)
            {
                WindowsExtesions.ErrorMessageBox("CarId is invalid");
                return null;
            }
            var dateValidate = _rentingDetails.ToList().Select(c => new
            {
                StartDate = c.StartDate,
                EndDate = c.EndDate
            }).All(date => date.StartDate < date.EndDate);
            if (dateValidate == false)
            {
                WindowsExtesions.ErrorMessageBox("StarDate and EndDate is invalid");
                return null;
            }

            _rentingTransaction = new RentingTransaction()
            {
                CustomerId = int.Parse(this.tbCustomerID.Text),
                RentingDate = this.dpRentingDate.SelectedDate,
                RentingDetails = _rentingDetails.ToList(),
                RentingStatus = byte.Parse(this.tbRentingStatus.Text),
                TotalPrice = decimal.Parse(this.tbTotalPrice.Text),
                RentingTransationId = int.Parse(this.tbRentingTransID.Text)
            };

            var newrentingTransactionnew = new RentingTransaction()
            {
                CustomerId = int.Parse(this.tbCustomerID.Text),
                RentingDate = this.dpRentingDate.SelectedDate,
                RentingDetails = _rentingDetails.ToList(),
                RentingStatus = byte.Parse(this.tbRentingStatus.Text),
                TotalPrice = decimal.Parse(this.tbTotalPrice.Text),
                RentingTransationId = int.Parse(this.tbRentingTransID.Text)
            };
            return newrentingTransactionnew;
        }

        void Reload()
        {
            _rentingTransaction = _rentinTransactionService.GetAllFullField(_rentingTransaction.RentingTransationId, true).FirstOrDefault();
                var rentingDetails = _rentingTransaction.RentingDetails;

            if (_rentingDetails.Any())
            {
                _rentingDetails = new ObservableCollection<RentingDetail>();
            }
            rentingDetails.ToList().ForEach(r => _rentingDetails.Add(r));
            rentingDetailDataGrid.ItemsSource = _rentingDetails;



            this.tbRentingTransID.Text = _rentingTransaction.RentingTransationId.ToString();
            this.dpRentingDate.SelectedDate = _rentingTransaction.RentingDate.GetValueOrDefault();
            this.tbCustomerID.Text = _rentingTransaction.CustomerId.ToString();
            this.tbTotalPrice.Text = _rentingTransaction.TotalPrice.ToString();
            this.tbRentingStatus.Text = _rentingTransaction.RentingStatus.ToString();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void UpdateTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            var rentingTransaction = Bindingdata();

            if (rentingTransaction != null)
            {
                var (result, message) = _rentinTransactionService.Update(rentingTransaction);

                if (result)
                {
                    WindowsExtesions.SuccessMessageBox("Update successful");
                    Reload();
                }
                else
                {
                    WindowsExtesions.ErrorMessageBox(message);
                }
            }
            Reload();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var rentingTransactioon = rentingDetailDataGrid.SelectedItem as RentingDetail;
           var (result, message) = _rentinTransactionService.DeleteTransDetail(rentingTransactioon.RentingTransactionId, rentingTransactioon.CarId);

            if (result)
            {
                WindowsExtesions.SuccessMessageBox("Delete successful");
                Reload();
            }
            else
            {
                WindowsExtesions.SuccessMessageBox(message);
            }
        }
    }
}
