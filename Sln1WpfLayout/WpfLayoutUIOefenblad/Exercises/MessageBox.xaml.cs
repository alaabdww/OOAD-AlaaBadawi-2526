using System.Windows;
using System.Windows.Controls;
using WpfLayoutUIOefenblad.Helpers;

namespace WpfLayoutUIOefenblad.Exercises;

[NavPage(title: "MessageBox", description: "Dialoogvensters", order: 10)]
public partial class MessageBoxDialog : Page
{
    public MessageBoxDialog()
    {
        InitializeComponent();
    }

    private void btnOpslaan_Click(object sender, RoutedEventArgs e)
    {
        
    }
}
