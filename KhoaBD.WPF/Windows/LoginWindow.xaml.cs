using PRN221.Application.Service.Interface;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KhoaBD.WPF.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAuthenService _authenService;
        private readonly AdminWindow _adminWindow;
        private readonly CustomerWindow _customerWindow;

        public LoginWindow(AdminWindow adminWindow,
            CustomerWindow customerWindow,
           IAuthenService authenService)
        {
            InitializeComponent();
            _adminWindow = adminWindow;
            _customerWindow = customerWindow;
            _authenService = authenService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var mail = txtMail.Text;
            var pass = txtPassword.Password;

            var (isValidAccount, message) = _authenService.Login(mail, pass, out bool isAdmin);

            if (isValidAccount)
            {
                if (isAdmin)
                {
                    _adminWindow.Show();
                    this.Close();
                }
                else
                {
                    _customerWindow.Tag = mail;
                    _customerWindow.LoadCustomer();
                    _customerWindow.Show();
                    this.Close();
                }
            }
            else
            {
                WindowsExtesions.ErrorMessageBox(message);
            }
        }


    }
}
