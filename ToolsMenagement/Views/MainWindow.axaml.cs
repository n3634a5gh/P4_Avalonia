using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using ToolsMenagement.ViewModels;

namespace ToolsMenagement.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        MyReferences.MainView = this;
        
        FToolName.Text = "";
        FPositionId.Text = "";
        FToolId.Text = "";
        FDiameter.Text = "";

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
        techWindow.Show();
    }

    private void OpenNewOrderWindow(object? sender, RoutedEventArgs e)
    {
        NewOrderWindow orderWindow = new NewOrderWindow();
        orderWindow.Width = 400;
        orderWindow.Show();
    }

    private void WorkRegistration(object? sender, RoutedEventArgs e)
    {
        WorkRegister workRegister = new WorkRegister();
        workRegister.Width = 800;
        workRegister.MinHeight = 600;
        workRegister.Show();
    }

    private void FResults(object? sender, RoutedEventArgs e)
    {
        string[] filterTable = new string[4];

        filterTable[0] = FToolName.Text;
        filterTable[1] = FPositionId.Text;
        filterTable[2] = FToolId.Text;
        filterTable[3] = FDiameter.Text;

        MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
        mainWindowViewModel.change_view_tools_results(filterTable);
        
        TreeDataGrid newTreeDataGrid = new TreeDataGrid();

        newTreeDataGrid.Margin = new Thickness(0, 20, 0, 0);
        newTreeDataGrid.Source = MyReferences.mwvm.Source2;

        var tdg= this.FindControl<Grid>("ToolViewGrid");
        if (tdg.Children.Count > 0)
        {
            tdg.Children.Clear();
        }
        tdg.Children.Add(newTreeDataGrid);
    }
}