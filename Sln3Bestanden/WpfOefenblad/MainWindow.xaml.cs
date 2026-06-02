using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WpfBestandenOefenblad.Helpers;

namespace WpfBestandenOefenblad;

public partial class MainWindow : Window
{
    private List<NavPageInfo> navItems = new List<NavPageInfo>();

    public MainWindow()
    {
        InitializeComponent();
        this.Loaded += this.MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        this.navItems = DiscoverNavPages();
        lstNav.ItemsSource = this.navItems;

        int lastOrder = Properties.Settings.Default.LastSelectedExercise;

        int index = this.navItems.FindIndex(x => x.Order == lastOrder);
        if (index < 0)
        {
            index = 0;
        }

        lstNav.SelectedIndex = index;
    }

    private static List<NavPageInfo> DiscoverNavPages()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        List<NavPageInfo> items =
            assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => typeof(Page).IsAssignableFrom(t))
                .Select(t => new
                {
                    Type = t,
                    Attr = t.GetCustomAttribute<NavPageAttribute>()
                })
                .Where(x => x.Attr != null)
                .Select(x => new NavPageInfo
                {
                    Title = x.Attr!.Title,
                    Description = x.Attr!.Description,
                    Order = x.Attr!.Order,
                    IsVisible = x.Attr!.IsVisible,
                    PageType = x.Type
                })
                .Where(x => x.IsVisible)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Title)
                .ToList();

        for (int i = 1; i < items.Count; i++)
        {
            items[i] = new NavPageInfo
            {
                Title = $"{i}. {items[i].Title}",
                Description = items[i].Description,
                Order = items[i].Order,
                IsVisible = items[i].IsVisible,
                PageType = items[i].PageType
            };
        }

        return items;
    }

    private void UpdateNavButtons()
    {
        btnPrev.IsEnabled = lstNav.SelectedIndex > 0;
        btnNext.IsEnabled = lstNav.SelectedIndex < lstNav.Items.Count - 1;
    }

    private void lstNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (lstNav.SelectedItem is not NavPageInfo navPageInfo)
        {
            return;
        }

        Page? page = (Page?)Activator.CreateInstance(navPageInfo.PageType);
        if (page != null)
        {
            fraMain.Navigate(page);
        }

        Properties.Settings.Default.LastSelectedExercise = navPageInfo.Order;
        Properties.Settings.Default.Save();

        UpdateNavButtons();
    }

    private void btnPrev_Click(object sender, RoutedEventArgs e)
    {
        if (lstNav.SelectedIndex > 0)
        {
            lstNav.SelectedIndex--;
        }
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
        if (lstNav.SelectedIndex < lstNav.Items.Count - 1)
        {
            lstNav.SelectedIndex++;
        }
    }

}
