using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ToolsMenagement.ViewModels;

namespace ToolsMenagement.Views;

public partial class NewOrderWindow : MainWindow
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

    private void CheckOrder(object? sender, RoutedEventArgs e)
    {
        bool checktool = CheckOrderTool.CheckOrder(Convert.ToInt32(MyReferences.nowvm.TechnologyNumber));
        var dtb = this.FindControl<TextBox>("TechnologyId");
        dtb.Text = "";
        if(checktool)
        {
            var btn = this.FindControl<Button>("BtnCheck");
            btn.IsVisible = true;
        }
        
    }

    private void CreateOrder(object? sender, RoutedEventArgs e)
    {
        var new_order = new CheckOrderTool();
        new_order.CreateOrder(Convert.ToInt32(MyReferences.nowvm.TechnologyNumber));
    }
}