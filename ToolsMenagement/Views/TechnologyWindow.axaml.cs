using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ToolsMenagement.ViewModels;

namespace ToolsMenagement.Views;

public partial class TechnologyWindow : MainWindow
{
    public TechnologyWindow()
    {
        InitializeComponent();
        DataContext = new TechnologyWindowViewModel();
        MyReferences.techview = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AddToolPosition()
    {
        StackPanel newStackPanel = new StackPanel();
        Rectangle newRectangle = new Rectangle();
        newStackPanel.Orientation = Orientation.Horizontal;
        newStackPanel.Height = 32;
        newStackPanel.Background = Brushes.DarkGray;
        newRectangle.Fill = Brushes.Gray;
        newRectangle.Height = 5;
        var stackPanel = this.FindControl<StackPanel>("StackPanel");
        stackPanel.Children.Add(newRectangle);
        stackPanel.Children.Add(newStackPanel);
        int position_counter=(int)(stackPanel.Children.Count/2);

        var positiontextblock = new TextBlock();
        var categorytextblock = new TextBlock();
        var purposetextblock = new TextBlock();
        var materialtextblock = new TextBlock();
        var diametertextblock = new TextBlock();
        var tusetextblock = new TextBlock();
        var borderrect = new Rectangle();

        var ccb = this.FindControl<ComboBox>("CategoryComboBox");
        var pcb = this.FindControl<ComboBox>("PurposeComboBox");
        var mcb = this.FindControl<ComboBox>("MaterialComboBox");
        var dtb = this.FindControl<TextBox>("DiameterTextBox");
        var utb = this.FindControl<TextBox>("TUseTextBox");

        categorytextblock.Text = ccb.SelectedItem.ToString();
        categorytextblock.MinWidth = 255;
        categorytextblock.MinHeight = 32;
        categorytextblock.TextAlignment = TextAlignment.Center;
        categorytextblock.VerticalAlignment = VerticalAlignment.Center; ;
        purposetextblock.Text = pcb.SelectedItem.ToString();
        purposetextblock.MinWidth = 175;
        purposetextblock.TextAlignment = TextAlignment.Center;
        materialtextblock.Text = mcb.SelectedItem.ToString();
        materialtextblock.MinWidth = 125;
        materialtextblock.TextAlignment = TextAlignment.Center;
        diametertextblock.Text = dtb.Text;
        diametertextblock.Width = 125;
        diametertextblock.TextAlignment = TextAlignment.Center;
        diametertextblock.TextAlignment = TextAlignment.Center;
        tusetextblock.Text = utb.Text;
        tusetextblock.Width = 125;
        tusetextblock.TextAlignment = TextAlignment.Center;
        positiontextblock.Text = position_counter.ToString();
        positiontextblock.Width = 20;
        borderrect.Width = 3;
        borderrect.Fill=Brushes.Gray;

        newStackPanel.Children.Add(positiontextblock);
        newStackPanel.Children.Add(categorytextblock);
        newStackPanel.Children.Add(purposetextblock);
        newStackPanel.Children.Add(materialtextblock);
        //newStackPanel.Children.Add(borderrect);
        newStackPanel.Children.Add(diametertextblock);
        //newStackPanel.Children.Add(borderrect);
        newStackPanel.Children.Add(tusetextblock);
        
        var sstackPanel = this.FindControl<StackPanel>("StackPanel");
        int count = stackPanel.Children.Count;
                
        if (count > 0)
        {
            MyReferences.twvm.EnableAddTech2 = true;
        }
        else
        {
            MyReferences.twvm.EnableAddTech2 = false;
        }
        

    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        var stackPanel = this.FindControl<StackPanel>("StackPanel");
        int count = stackPanel.Children.Count;
        string[][] tab1 = new string[(int)(count/2)][];
        for (int i = 0; i < tab1.Length; i++)
        {
            tab1[i] = new string[5];
        }

        int stackp_counter = 0;
        for (int i = 0; i < count; i++)
        {
            if (stackPanel.Children[i] is StackPanel)
            {
                StackPanel childStackPanel = stackPanel.Children[i] as StackPanel;
                int childCount = childStackPanel.Children.Count;
                int k = 0;
                for (int j = 0; j < childCount; j++)
                {
                    if (childStackPanel.Children[j] is TextBlock)
                    {
                        TextBlock textBlock = childStackPanel.Children[j] as TextBlock;
                        if (j!= 0)
                        {
                            tab1[stackp_counter][k] = textBlock.Text;
                            k++;
                        }
                        
                        //textBlock.Text = (j).ToString();

                    }
                }

                stackp_counter++;
            }
        }
        async void addech()
        {
            var addtechnology = new AddNewTechnology();
            var ifadd = await addtechnology.AddNewTech(MyReferences.twvm.TechnologyName, tab1);
            this.IsEnabled = false;
        }
        
        addech();

    }

    private void AddTTool(object? sender, RoutedEventArgs e)
    {
        async void F1()
        {
            var toolExist = new ToolExist();
            var checkToolExist = await toolExist.ExecuteToolExist();

            if (checkToolExist)
            {
                AddToolPosition();
                
                var ccb = this.FindControl<ComboBox>("CategoryComboBox");
                ccb.SelectedIndex = -1;
                var pcb = this.FindControl<ComboBox>("PurposeComboBox");
                pcb.SelectedIndex = -1;
                var mcb = this.FindControl<ComboBox>("MaterialComboBox");
                mcb.SelectedIndex = -1;
                var dtb = this.FindControl<TextBox>("DiameterTextBox");
                dtb.Text = "";
                var utb = this.FindControl<TextBox>("TUseTextBox");
                utb.Text = "";

                MyReferences.twvm.IsEnable1 = false;
                MyReferences.twvm.IsEnable2 = false;
                MyReferences.twvm.IsEnable3 = false;
                MyReferences.twvm.IsEnable4 = false;
                MyReferences.twvm.IsEnable5 = false;
                
            }
        }

        F1();
    }
}