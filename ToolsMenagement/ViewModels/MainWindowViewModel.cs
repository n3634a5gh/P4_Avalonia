using System.Collections.ObjectModel;
using ToolsMenagement.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;

namespace ToolsMenagement.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ObservableCollection<Person> _people = new()
    {
        new Person 
        { 
            FirstName = "Eleanor", 
            LastName = "Pope", 
            Age = 32,
            Children =
            {
                new Person { FirstName = "Marcel", LastName = "Gutierrez", Age = 4 },
            }
        },
        new Person 
        { 
            FirstName = "Jeremy",
            LastName = "Navarro",
            Age = 74,
            Children =
            {
                new Person 
                { 
                    FirstName = "Jane",
                    LastName = "Navarro",
                    Age = 42 ,
                    Children =
                    {
                        new Person { FirstName = "Lailah ", LastName = "Velazquez", Age = 16 }
                    }
                },
            }
        },
        new Person { FirstName = "Jazmine", LastName = "Schroeder", Age = 52 },
    };
    
    public MainWindowViewModel()
    {
        Source = new HierarchicalTreeDataGridSource<Person>(_people)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<Person>(
                    new TextColumn<Person, string>("First Name", x => x.FirstName),
                    x => x.Children),
                new TextColumn<Person, string>("Last Name", x => x.LastName),
                new TextColumn<Person, int>("Age", x => x.Age),
            },
        };
    }

    public HierarchicalTreeDataGridSource<Person> Source { get; }
}