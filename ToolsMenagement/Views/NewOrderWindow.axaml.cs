using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ToolsMenagement.ViewModels;

namespace ToolsMenagement.Views;

public partial class NewOrderWindow : Window
{
    public NewOrderWindow()
    {
        InitializeComponent();
        DataContext = new NewOrderWindowViewModel();
        MyReferences.orderview = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}