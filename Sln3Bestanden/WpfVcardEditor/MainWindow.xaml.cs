using Microsoft.Win32;
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
using System.IO;

namespace WpfVcardEditor
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      private string currentFilePath = null;
      private bool hasChanges = false;
      private string photoBase64 = null; // Met AI gedaan 


      public MainWindow()
      {
         InitializeComponent();
      }

      // opent het about venster
      private void MnuAbout_Click(object sender, RoutedEventArgs e)
      {
         new AboutWindow().Show();
      }

      // vraagt bevestiging en sluit de applicatie
      private void mnuExit_Click(object sender, RoutedEventArgs e)
      {
         MessageBoxResult result = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?", "Toepassing sluiten", MessageBoxButton.OKCancel);

         if (result == MessageBoxResult.OK)
         {
            this.Close();
         }
      }

      // leest een .vcf bestand en vult alle velden in
      private void ReadVCard(string filePath)
      {
         try
         {
            string[] lines = File.ReadAllLines(filePath);
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtPrivateEmail.Text = "";
            txtPrivatePhone.Text = "";
            datBirthday.SelectedDate = null;
            rdnMale.IsChecked = false;
            rdnFemale.IsChecked = false;
            rdnUnknown.IsChecked = false;
            photoBase64 = null;
            imgPreview.Source = null;
            tblPhotoName.Text = "(geen geselecteerd)";

            foreach (string line in lines)
            {
               if (line.Contains("N;") || line.Contains("N:"))
               {
                  string value = line.Substring(line.IndexOf(':') + 1);
                  string[] parts = value.Split(';');
                  if (parts.Length > 1)
                  {
                     txtLastname.Text = parts[0];
                     txtFirstname.Text = parts[1];
                  }
               }
               if (line.Contains("BDAY:"))
               {
                  string value = line.Substring(line.IndexOf(':') + 1);
                  int year = int.Parse(value.Substring(0, 4));
                  int month = int.Parse(value.Substring(4, 2));
                  int day = int.Parse(value.Substring(6, 2));
                  datBirthday.SelectedDate = new DateTime(year, month, day);
               }
               if (line.Contains("GENDER:"))
               {
                  string value = line.Substring(line.IndexOf(':') + 1);
                  if (value == "M") rdnMale.IsChecked = true;
                  else if (value == "F") rdnFemale.IsChecked = true;
                  else rdnUnknown.IsChecked = true;
               }
               if (line.Contains("type=HOME,INTERNET:"))
                  txtPrivateEmail.Text = line.Substring(line.IndexOf(':') + 1);
               if (line.Contains("TYPE=HOME,VOICE:"))
                  txtPrivatePhone.Text = line.Substring(line.IndexOf(':') + 1);

               if (line.Contains("PHOTO;ENCODING=BASE64")) // Door AI laten genereren
               {
                  photoBase64 = line.Substring(line.IndexOf(':') + 1);
                  ShowPhoto(photoBase64);
                  tblPhotoName.Text = "(foto geladen uit bestand)";
               }
            }
            tblCurrentCard.Text = "huidige kaart: " + System.IO.Path.GetFileNameWithoutExtension(filePath);
            currentFilePath = filePath;
            mnuSaveAs.IsEnabled = true;
            hasChanges = false;
            UpdateSaveButton();
            UpdatePercentage();
         }
         catch (FileNotFoundException ex)
         {
            MessageBox.Show("Kan bestand " + filePath + " niet lezen\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
         catch (UnauthorizedAccessException ex)
         {
            MessageBox.Show("Geen toegang tot bestand " + filePath + "\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
         catch (Exception ex)
         {
            MessageBox.Show("Onverwachte fout bij lezen\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }


      // opent een OpenFileDialog en laadt de gekozen .vcf
      private void mnuOpen_Click(object sender, RoutedEventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
         openFileDialog.Filter = "vCard files (*.vcf)|*.vcf";

         bool? result = openFileDialog.ShowDialog();
         if (result == true)
         {
            ReadVCard(openFileDialog.FileName);
         }
      }

      // schrijft alle ingevulde velden weg naar een .vcf bestand
      private void WriteVCard(string filePath)
      {
         try
         {
            string content = "BEGIN:VCARD" + Environment.NewLine;
            content += "VERSION:3.0" + Environment.NewLine;
            if (!string.IsNullOrEmpty(txtLastname.Text) || !string.IsNullOrEmpty(txtFirstname.Text))
               content += "N;CHARSET=UTF-8:" + txtLastname.Text + ";" + txtFirstname.Text + ";;;" + Environment.NewLine;
            if (rdnMale.IsChecked == true) content += "GENDER:M" + Environment.NewLine;
            else if (rdnFemale.IsChecked == true) content += "GENDER:F" + Environment.NewLine;
            if (datBirthday.SelectedDate != null)
            {
               DateTime bday = datBirthday.SelectedDate.Value;
               content += "BDAY:" + bday.Year.ToString() + bday.Month.ToString("D2") + bday.Day.ToString("D2") + Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(txtPrivateEmail.Text))
               content += "EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:" + txtPrivateEmail.Text + Environment.NewLine;
            if (!string.IsNullOrEmpty(txtPrivatePhone.Text))
               content += "TEL;TYPE=HOME,VOICE:" + txtPrivatePhone.Text + Environment.NewLine;
            if (photoBase64 != null) // Door AI laten genereren
               content += "PHOTO;ENCODING=BASE64;TYPE=image/jpeg:" + photoBase64 + Environment.NewLine;

            content += "END:VCARD";
            File.WriteAllText(filePath, content);
         }
         catch (UnauthorizedAccessException ex)
         {
            MessageBox.Show("Geen toegang tot bestand " + filePath + "\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
         catch (IOException ex)
         {
            MessageBox.Show("Fout bij wegschrijven naar " + filePath + "\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
         catch (Exception ex)
         {
            MessageBox.Show("Onverwachte fout bij opslaan\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }

      // activeert of deactiveert mnuSave op basis van ingevulde velden
      private void UpdateSaveButton()
      {
         if (currentFilePath != null &&
             !string.IsNullOrEmpty(txtFirstname.Text) &&
             !string.IsNullOrEmpty(txtLastname.Text) &&
             !string.IsNullOrEmpty(txtPrivateEmail.Text) &&
             !string.IsNullOrEmpty(txtPrivatePhone.Text) &&
             datBirthday.SelectedDate != null &&
             (rdnMale.IsChecked == true || rdnFemale.IsChecked == true || rdnUnknown.IsChecked == true))
            mnuSave.IsEnabled = true;
         else
            mnuSave.IsEnabled = false;
      }

      // wordt opgeroepen bij elke veldwijziging, zet hasChanges op true
      private void Card_Changed(object sender, EventArgs e)
      {
         hasChanges = true;
      }

      // wordt opgeroepen bij elke veldwijziging, update save knop en percentage
      private void Field_Changed(object sender, RoutedEventArgs e)
      {
         UpdateSaveButton();
         Card_Changed(sender, e);
         UpdatePercentage();
      }

      // slaat de huidige kaart op en toont een bevestiging
      private void mnuSave_Click(object sender, RoutedEventArgs e)
      {
         WriteVCard(currentFilePath);
         MessageBox.Show("Bestand opgeslagen!", "Opslaan", MessageBoxButton.OK);
      }

      // opent een SaveFileDialog en slaat op naar gekozen locatie
      private void mnuSaveAs_Click(object sender, RoutedEventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
         saveFileDialog.Filter = "vCard files (*.vcf)|*.vcf";
         bool? result = saveFileDialog.ShowDialog();
         if (result == true)
         {
            currentFilePath = saveFileDialog.FileName;
            WriteVCard(currentFilePath);
            tblCurrentCard.Text = "huidige kaart: " + System.IO.Path.GetFileNameWithoutExtension(currentFilePath);
            UpdateSaveButton();
         }
      }

      // vraagt bevestiging bij wijzigingen en maakt een nieuwe lege kaart
      private void mnuNew_Click(object sender, RoutedEventArgs e)
      {
         if (hasChanges)
         {
            MessageBoxResult result = MessageBox.Show("Er zijn onopgeslagen wijzigingen. Verdergaan?", "Nieuwe kaart", MessageBoxButton.OKCancel);
            if (result != MessageBoxResult.OK) return;
         }
         txtFirstname.Text = "";
         txtLastname.Text = "";
         txtPrivateEmail.Text = "";
         txtPrivatePhone.Text = "";
         datBirthday.SelectedDate = null;
         rdnMale.IsChecked = false;
         rdnFemale.IsChecked = false;
         rdnUnknown.IsChecked = false;
         photoBase64 = null;
         imgPreview.Source = null;
         tblPhotoName.Text = "(geen geselecteerd)";
         currentFilePath = null;
         tblCurrentCard.Text = "huidige kaart: (geen)";
         hasChanges = false;
         mnuSaveAs.IsEnabled = false;
         UpdateSaveButton();
         UpdatePercentage();
      }

      // toont een base64 afbeelding in het imgPreview control
      private void ShowPhoto(string base64) //Methode door AI laten generern 
      {
         try
         {
            byte[] bytes = Convert.FromBase64String(base64);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();
            imgPreview.Source = bitmap;
         }
         catch (Exception ex)
         {
            MessageBox.Show("Fout bij laden foto\n" + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
         }
      }

      // opent een OpenFileDialog en laadt de gekozen afbeelding als foto
      private void btnSelectPhoto_Click(object sender, RoutedEventArgs e) //Methode door AI laten genereren
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.Filter = "Image files (*.jpg;*.jpeg)|*.jpg;*.jpeg";
         bool? result = openFileDialog.ShowDialog();
         if (result == true)
         {
            tblPhotoName.Text = openFileDialog.FileName;
            byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);
            photoBase64 = Convert.ToBase64String(bytes);
            ShowPhoto(photoBase64);
            UpdatePercentage();
            hasChanges = true;
         }
      }

      // herberekent het percentage ingevulde velden in de statusbar
      private void UpdatePercentage()
      {
         int total = 7;
         int filled = 0;
         if (!string.IsNullOrEmpty(txtFirstname.Text)) { filled++; }
         if (!string.IsNullOrEmpty(txtLastname.Text)) { filled++; }
         if (!string.IsNullOrEmpty(txtPrivateEmail.Text)) { filled++; }
         if (!string.IsNullOrEmpty(txtPrivatePhone.Text)) { filled++; }
         if (datBirthday.SelectedDate != null) { filled++; }
         if (rdnMale.IsChecked == true || rdnFemale.IsChecked == true || rdnUnknown.IsChecked == true) { filled++; }
         if (photoBase64 != null) { filled++; }
         tblPercentage.Text = "percentage ingevuld: " + (filled * 100 / total) + "%";
      }

   }
}
