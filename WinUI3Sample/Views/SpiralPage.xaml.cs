using System.Diagnostics;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using WinUI3Sample.ViewModels;

namespace WinUI3Sample.Views;

public sealed partial class SpiralPage : Page
{
    public SpiralViewModel ViewModel
    {
        get;
    }

    public SpiralPage()
    {
        ViewModel = App.GetService<SpiralViewModel>();
        InitializeComponent();
    }
}
