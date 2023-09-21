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
        public ManipulateCarWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSupplier.SelectedItem != null)
            {
                string selectedOption = ((ComboBoxItem)cbSupplier.SelectedItem).Content.ToString();
                MessageBox.Show("You selected: " + selectedOption);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
