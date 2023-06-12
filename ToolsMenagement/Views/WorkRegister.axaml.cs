using System;
//using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DynamicData;
using ToolsMenagement.ViewModels;
using Brushes = Avalonia.Media.Brushes;

namespace ToolsMenagement.Views;

public partial class WorkRegister : MainWindow
{
    public WorkRegister()
    {
        InitializeComponent();
        DataContext = new WorkRegisterViewModel();
        MyReferences.registerview = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void YesRadioClick(object? sender, RoutedEventArgs e)
    {
        var newSaveOperation = new JobSaveService();
        if (!newSaveOperation.check_order(Convert.ToInt32(MyReferences.wrvm.OrderNumber)))
        {
            MyReferences.wrvm.Option1Checked = true;
        }
        else
        {
            var regenerate = new WorkRegisterViewModel();
            var ttb = this.FindControl<TextBox>("OrderTextBox");
            string tId = ttb.Text;
            regenerate.regenerate_source(Convert.ToInt32(ttb.Text));
            var tcb = this.FindControl<ComboBox>("ToolComboBox");
            tcb.Items = MyReferences.wrvm.DamagedTools;
        }
    }

    private void OnAddDTools(object? sender, RoutedEventArgs e)
    {
        var rdb = this.FindControl<RadioButton>("NotRadioButton");
        rdb.IsEnabled = false;
        
        var stackPanel = this.FindControl<StackPanel>("DToolsStackPanel");
        var tcb = this.FindControl<ComboBox>("ToolComboBox");
        var dcb = this.FindControl<ComboBox>("DamageComboBox");
        
        var tooltextblock = new TextBlock();
        var damagetypetextblock = new TextBlock();
        StackPanel newStackPanel = new StackPanel();
        var newhorizontalRectangle = new Rectangle();
        var newvericalRectangle = new Rectangle();
        newhorizontalRectangle.Height = 1;
        newvericalRectangle.Width = 1;
        newhorizontalRectangle.Fill = Brushes.DimGray;
        newvericalRectangle.Fill=Brushes.DimGray;
        newStackPanel.Orientation = Orientation.Horizontal;
        newStackPanel.Height = 32;
        newStackPanel.Background = Brushes.DarkGray;
        tooltextblock.Width = 299;
        damagetypetextblock.Width = 100;
        tooltextblock.TextAlignment = TextAlignment.Left;
        damagetypetextblock.TextAlignment = TextAlignment.Left;

        int damage_index = 0;
        
        tooltextblock.Text = tcb.SelectedItem.ToString();
        /*if (dcb.SelectedIndex == 0)
        {
            damagetypetextblock.Text = "Do regeneracji";
        }
        else if(dcb.SelectedIndex == 1)
        {
            damagetypetextblock.Text = "Trwałe";
        }*/
        //damagetypetextblock.Text = dcb.SelectedItem.ToString();
        //nawet nie chce dochodzić czemu ten sposób nie działa, ale inaczej 
        //do textblock zapisuje tylko ścieżkę lokalizacji, a nie wartość z combobox
        
        
        if (dcb.SelectedItem != null)
        {
            var selectedComboBoxItem = dcb.SelectedItem as ComboBoxItem;
            if (selectedComboBoxItem != null)
                damagetypetextblock.Text = selectedComboBoxItem.Content.ToString();
        }
        
        stackPanel.Children.Add(newStackPanel);
        stackPanel.Children.Add(newhorizontalRectangle);
        
        newStackPanel.Children.Add(tooltextblock);
        newStackPanel.Children.Add(newvericalRectangle);
        newStackPanel.Children.Add(damagetypetextblock);
        
    }

    private void SaveChanges(object? sender, RoutedEventArgs e)
    {
        var newSaveOperation = new JobSaveService();
        var orderID = this.FindControl<TextBox>("OrderTextBox");
        var quantity = this.FindControl<TextBox>("QuantityTextBox");
        var employee = this.FindControl<TextBox>("EmployeeTextBox");
        if (newSaveOperation.check_order(Convert.ToInt32(orderID.Text)))
        {
            newSaveOperation.UpdateRegisterTable(Convert.ToInt32(orderID.Text),
                Convert.ToInt32(quantity.Text),employee.Text);
        }
        
        newSaveOperation.UpdateWarehouse(Convert.ToInt32(orderID.Text));
        var stackPanel = this.FindControl<StackPanel>("DToolsStackPanel");
        int[] damageToolsIndexes;
        if (stackPanel.Children.Count > 0)
        {
            damageToolsIndexes = new int[stackPanel.Children.Count/2];
        }
    }
}