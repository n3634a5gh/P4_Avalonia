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
        bool orderclose = false,notvalid=false;
        
        var damagetoolsstackPanel = this.FindControl<StackPanel>("DToolsStackPanel");
        string[][] damageTools;


        if (newSaveOperation.check_order(Convert.ToInt32(orderID.Text)))
        {
            
            //zapis to dabeli rejestracja
            newSaveOperation.UpdateRegisterTable(Convert.ToInt32(orderID.Text),
                Convert.ToInt32(quantity.Text),employee.Text);
            
            //zapis to dabeli magazyn
            orderclose=newSaveOperation.UpdateWarehouse(Convert.ToInt32(orderID.Text)
                ,Convert.ToInt32(quantity.Text));
            
            //obsługa zapisu uszkodzonych narzędzi
            if (damagetoolsstackPanel.Children.Count > 0)
            {
                orderclose = true;
                damageTools = new string[2][];
                for (int t = 0; t < damageTools.Length; t++)
                {
                    damageTools[t] = new string[damagetoolsstackPanel.Children.Count / 2];
                }

                int item_counter = 0;

                for (int i = 0; i < damagetoolsstackPanel.Children.Count;i++)
                {
                    if (damagetoolsstackPanel.Children[i] is StackPanel)
                    {
                        StackPanel childStackPanel = damagetoolsstackPanel.Children[i] as StackPanel;
                        int zxc = childStackPanel.Children.Count;
                        
                        TextBlock nameTextBlock = childStackPanel.Children[0] as TextBlock;
                        TextBlock damageTextBlock = childStackPanel.Children[2] as TextBlock;

                        damageTools[0][item_counter] = nameTextBlock.Text;
                        damageTools[1][item_counter] = damageTextBlock.Text;

                        item_counter++;
                    }
                }
                
                newSaveOperation.UpdateDamagedAndToRestoreTools(Convert.ToInt32(orderID.Text),damageTools);
            }
            
        }
        else
        {
            notvalid = true;
        }

        var message = "";

        if (!notvalid)
        {
            if (orderclose)
            {
                message = "Zmiany zostały zapisane\n Zlecenie zostało zakończone.";
            }
            else
            {
                message = "Zmiany zostały zapisane";
            }
            var newmessage2 = new Messages().UniversalMessage(message, MyReferences.registerview,"",true);
        }
    }
}