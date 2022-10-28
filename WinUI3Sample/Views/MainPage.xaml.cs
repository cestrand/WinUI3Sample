using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using WinUI3Sample.ViewModels;

namespace WinUI3Sample.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

    private async void OpenUri_WinUI3_Documentation(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        // Open WinUI3 documentation
        Uri uri = new Uri("https://learn.microsoft.com/en-us/windows/apps/winui/");
        Windows.System.Launcher.LaunchUriAsync(uri);
    }
}
