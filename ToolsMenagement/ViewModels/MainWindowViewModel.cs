using System.Collections.ObjectModel;
using ToolsMenagement.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace ToolsMenagement.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    private ObservableCollection<Narzedzie> _narzedzia = new()
    {
        new Narzedzie { IdKategorii = 1, IdNarzedzia = 1,Srednica=1,Nazwa="zxc"}
    };

    public MainWindowViewModel()
    {

        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        /*context.Clients.Add(new Client()
        {
            Name = "Jan Kowalski",
            Adress = "Szeroka, Bielsko",
            Balance = 0
        });*/
        context.SaveChanges();

        var zxc = context.Narzedzies
            //.Where(narzedzie => narzedzie.IdKategorii == 1)
            .ToArray();

        var narzedziaCollection = new ObservableCollection<Narzedzie>(zxc);

        Source = new FlatTreeDataGridSource<Narzedzie>(narzedziaCollection)
        {
            Columns =
            {
                new TextColumn<Narzedzie, string>("Nazwa", x=> x.Nazwa),
                new TextColumn<Narzedzie, int>("Id Narzędzia", x => x.IdNarzedzia),
                new TextColumn<Narzedzie, int>("Id kategori", x => x.IdKategorii),
                new TextColumn<Narzedzie, int>("Średnica", x => x.Srednica),
            },
        };
    








    }

    //public FlatTreeDataGridSource<Narzedzie> Zxc { get; }

    public FlatTreeDataGridSource<Narzedzie> Source { get; }
}