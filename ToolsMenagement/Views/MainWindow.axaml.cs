using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
    
    /*private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }*/

    private void OnSubmitClicked(object sender, RoutedEventArgs e)
    {
        var newTool = new AddNewTool();
    }
    private void OnSubmit2Clicked(object sender, RoutedEventArgs e)
    {
        //string name = NameTextBox.Text;
        //string email = EmailTextBox.Text;
    }
}