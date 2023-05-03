using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DynamicData;
using ReactiveUI;
using ToolsMenagement.ViewModels;

namespace ToolsMenagement.Views;

public partial class TechnologyWindow : Window
{
    public TechnologyWindow()
    {
        InitializeComponent();
        MyReferences.techview = this;
        
        DataContext = new TechnologyWindowViewModel();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        //ButtonAdd.IsEnabled = false; 
        /*Button button = (Button)sender;
        button.IsEnabled = false;*/
        
        TextBox newTextBox = new TextBox();
        StackPanel newStackPanel = new StackPanel();
        newStackPanel.Orientation = Orientation.Horizontal;
        var stackPanel = this.FindControl<StackPanel>("StackPanel");
        stackPanel.Children.Add(newStackPanel);
        
        var binding = new Binding(nameof(ComboBox.SelectedItem))
        {
            Source = null,
            Mode = BindingMode.OneWay
        };

        for (int i = 0; i <= 0; i++)
        {
            var comboBox1 = new ComboBox();
            var comboBox2 = new ComboBox();
            var comboBox3 = new ComboBox();
            var textbox1 = new TextBox();

            //MyReferences.techview.TextStackPanel.IsVisible = true;
            
            
            comboBox1.Items = MyReferences.mwvm.Categories;
            comboBox1.MinWidth = 275;
            comboBox2.Items = MyReferences.mwvm.Purpose;
            comboBox2.MinWidth = 175;
            comboBox2.IsEnabled = false;
            comboBox3.Items = MyReferences.mwvm.Material;
            comboBox3.MinWidth = 125;
            comboBox3.IsEnabled = false;
            textbox1.Width = 125;
            textbox1.IsEnabled = false;
            textbox1.TextAlignment = TextAlignment.Center;
            
            binding.Source = MyReferences.twvm;
            binding.Path = nameof(MyReferences.twvm.SelectedCategory);
            
            comboBox1.SelectionChanged += (s, args) =>
            {
                //newTextBox.Text = comboBox1.SelectedItem?.ToString() ?? "";
                MyReferences.twvm.SelectedCategory = comboBox1.SelectedItem?.ToString() ?? "";
                //newTextBox.Text = MyReferences.twvm.SelectedCategory;
                comboBox2.IsEnabled = true;
            };
            
            binding.Source = MyReferences.twvm;
            binding.Path = nameof(MyReferences.twvm.SelectedPurpose);
            
            comboBox2.SelectionChanged += (s, args) =>
            {
                MyReferences.twvm.SelectedPurpose = comboBox2.SelectedItem?.ToString() ?? "";
                //newTextBox.Text = MyReferences.twvm.SelectedPurpose;
                comboBox3.IsEnabled = true;
            };

            binding.Source = MyReferences.twvm;
            binding.Path = nameof(MyReferences.twvm.SelectedMaterial);

            comboBox3.SelectionChanged += (s, args) =>
            {
                MyReferences.twvm.SelectedMaterial = comboBox3.SelectedItem?.ToString() ?? "";
                //newTextBox.Text = MyReferences.twvm.SelectedMaterial;
                textbox1.IsEnabled = true;
            };
            
            binding.Source = MyReferences.twvm;
             binding.Path = nameof(MyReferences.twvm.Diameter);
             binding.Mode = BindingMode.TwoWay;
             textbox1.Bind(TextBox.TextProperty, binding);

             newStackPanel.Children.Add(comboBox1);
            newStackPanel.Children.Add(comboBox2);
            newStackPanel.Children.Add(comboBox3);
            newStackPanel.Children.Add(textbox1);
        }
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        var stackPanel = this.FindControl<StackPanel>("StackPanel");
        int count = stackPanel.Children.Count;
        for (int i = 0; i < count; i++)
        {
            if (stackPanel.Children[i] is StackPanel)
            {
                StackPanel childStackPanel = stackPanel.Children[i] as StackPanel;
                int childCount = childStackPanel.Children.Count;
                for (int j = 0; j < childCount; j++)
                {
                    if (childStackPanel.Children[j] is TextBox)
                    {
                        TextBox textBox = childStackPanel.Children[j] as TextBox;
                        textBox.Text = (i).ToString();
                    }
                }
            }
        }
    }
}