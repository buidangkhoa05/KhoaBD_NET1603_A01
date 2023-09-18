using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhoaBD.WPF
{
    public static class WindowsExtesions
    {
        public static MessageBoxResult SuccessMessageBox(string succesMessage)
        {
            return MessageBox.Show(succesMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ErrorMessageBox(string succesMessage)
        {
            return MessageBox.Show(succesMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static MessageBoxResult ConfirmMessageBox(string succesMessage)
        {
            return MessageBox.Show(succesMessage, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }
    }
}
