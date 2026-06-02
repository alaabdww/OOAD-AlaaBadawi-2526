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

      private Stack<ListBoxItem> verwijderdeItems = new Stack<ListBoxItem>();



      private string CheckForm(string taak, int prioriteitIndex, DateTime? datum, string selectedPerson)
      {
         string errorMessage = "";

         if (String.IsNullOrEmpty(taak))
         {
            errorMessage += "Gelieve een taak in te vullen" + Environment.NewLine;
         }

         if (prioriteitIndex == 0)
         {
            errorMessage += "Gelieve een prioriteit te kiezen" + Environment.NewLine;
         }

         if (datum == null)
         {
            errorMessage += "Gelieve een deadline te kiezen" + Environment.NewLine;
         }

         if (selectedPerson == "")
         {
            errorMessage += "Gelieve een uitvoerder te kiezen" + Environment.NewLine;
         }

         return errorMessage;
      }

      private void btnToevoegen_Click(object sender, RoutedEventArgs e)
      {
         string taak = txtTaak.Text;
         int prioriteitIndex = cmbPrioriteit.SelectedIndex;
         DateTime? datum = dpDeadline.SelectedDate;
         string selectedPerson = "";

         if (rbAdam.IsChecked == true)
         {
            selectedPerson = "Adam";
         }
         else if (rbBilal.IsChecked == true)
         {
            selectedPerson = "Bilal";
         }
         else if (rbChelsey.IsChecked == true)
         {
            selectedPerson = "Chelsey";
         }

         string errorMessage = CheckForm(taak, prioriteitIndex, datum, selectedPerson);
         txtError.Text = errorMessage;

         if (errorMessage != "")
         {
            return;
         }

         ListBoxItem item = new ListBoxItem();
         item.Content = $"{taak} (deadline: {datum.Value.ToShortDateString()}; door: {selectedPerson})";

         if (prioriteitIndex == 1)
         {
            item.Background = Brushes.LightGreen;
         }
         else if (prioriteitIndex == 2)
         {
            item.Background = Brushes.LightYellow;
         }
         else if (prioriteitIndex == 3)
         {
            item.Background = Brushes.LightCoral;
         }

         lstTaken.Items.Add(item);
      }

      private void lstTaken_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (lstTaken.SelectedItem != null)
         {
            btnVerwijderen.IsEnabled = true;
         }
         else
         {
            btnVerwijderen.IsEnabled = false;
         }
      }

      private void btnVerwijderen_Click(object sender, RoutedEventArgs e)
      {
         if (lstTaken.SelectedItem != null)
         {
            ListBoxItem geslecteerdeItem = (ListBoxItem)lstTaken.SelectedItem;
            verwijderdeItems.Push(geslecteerdeItem);
            lstTaken.Items.Remove(geslecteerdeItem);
         }
         btnVerwijderen.IsEnabled = false;
         btnTerugzetten.IsEnabled = true;
      }

      private void btnTerugzetten_Click(object sender, RoutedEventArgs e)
      {
         ListBoxItem item = verwijderdeItems.Pop();
         lstTaken.Items.Add(item);
         btnTerugzetten.IsEnabled = false;

      }
   }
}
