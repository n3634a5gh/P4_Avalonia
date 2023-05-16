using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using ToolsMenagement.ViewModels;

namespace ToolsMenagement.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        MyReferences.MainView = this;
        
    }

    private void OnSubmitClicked(object sender, RoutedEventArgs e)
    {
        if (!MyReferences.mwvm.IsDataValid)
        {
            
        }
        else
        {
            var newTool = new AddNewTool();
            CategoryComboBox.SelectedIndex = -1;
            MaterialComboBox.SelectedIndex = -1;
            PurposeComboBox.SelectedIndex = -1;
            DiameterTextBox.Text= "";
            LifetimeTextBox.Text = "";
        }
    }
    private void OnSubmit2Clicked(object sender, RoutedEventArgs e)
    {
        var restoretool = new RestoreTool().ExecuteRestoreTool();
        ToolTextBox1.Text = "";
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        TechnologyWindow techWindow = new TechnologyWindow();
       // techWindow.Width = 800;
        techWindow.Show();
    }

    private void OpenNewOrderWindow(object? sender, RoutedEventArgs e)
    {
        NewOrderWindow orderWindow = new NewOrderWindow();
        orderWindow.Width = 400;
        orderWindow.Show();
    }
}