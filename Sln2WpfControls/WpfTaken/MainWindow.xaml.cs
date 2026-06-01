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

namespace WpfTaken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";

            if (String.IsNullOrEmpty(txtTaak.Text)) {
                errorMessage += "Gelieve een taak in te vullen" + Environment.NewLine;
            } if (cmbPrioriteit.SelectedIndex == 0)
            {
                errorMessage += "Gelieve een prioriteit te kiezen" + Environment.NewLine;
            } if (dpDeadline.SelectedDate == null)
            {
                errorMessage += "Gelieve een deadline te kiezen" + Environment.NewLine;
            } if (rbAdam.IsChecked == false && rbBilal.IsChecked == false && rbChelsey.IsChecked == false)
            {
                errorMessage += "Gelieve een uitvoerder te kiezen" + Environment.NewLine;
            }

            txtError.Text = errorMessage;
        }
    }
}
