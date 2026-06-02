using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WpfBestandenOefenblad.Helpers;

namespace WpfBestandenOefenblad.Exercises;

[NavPage(Title = "Pad builder", Description = "Paden samenstellen uit basispad, map en bestandsnaam", Order = 1, IsVisible = true)]
public partial class PadBuilder : Page
{
    public PadBuilder()
    {
        InitializeComponent();
    }

    private void btnGenereerPad_Click(object sender, RoutedEventArgs e)
    {
        string folderPath = "";
        if (rdbDocumenten.IsChecked == true)
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        } else if (rdbDesktop.IsChecked == true)
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        } else if (rdbAfbeeldingen.IsChecked == true)
        {
            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        string subpath = txtPad.Text;
        string bestandNaam = txtBestandsnaam.Text;

        string filePath = System.IO.Path.Combine(folderPath.Trim(), subpath.Trim(), bestandNaam.Trim());

        txtResultaat.Text = filePath.Replace('\\', '/');

    }
}
