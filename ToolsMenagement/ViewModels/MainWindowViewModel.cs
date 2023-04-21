using System;
using System.Collections.ObjectModel;
using ToolsMenagement.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Avalonia.Data;
using Avalonia.Interactivity;
using ReactiveUI;

namespace ToolsMenagement.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<string>Categories { get; set; }
    public ObservableCollection<string>Purpose { get; set; }
    public ObservableCollection<string>Material { get; set; }
    
    private string _selectedCategory;
    public string SelectedCategory
    {
        get => _selectedCategory;
        set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
    }

    private string _selectedPurpose;

    public string SelectedPurpose
    {
        get => _selectedPurpose;
        set => this.RaiseAndSetIfChanged(ref _selectedPurpose, value);
    }
    
    private string _selectedMaterial;

    public string SelectedMaterial
    {
        get => _selectedMaterial;
        set => this.RaiseAndSetIfChanged(ref _selectedMaterial, value);
    }

    private string _tDiameter;

    public string T_diameter
    {
        get => _tDiameter;
        set
        {
            double number;
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (value.Contains('.'))
                {
                    throw new DataValidationException("Nieprawidłowy format liczby");
                }
                else
                {
                    if (!double.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        throw new DataValidationException("Wartość musi być dodatnia typu double");
                    }
                    else
                    {
                        this.RaiseAndSetIfChanged(ref _tDiameter, value);
                    }
                }
            }
        }
    }

    private string c_edges;
    
    public string CEdges
    {
        get => c_edges;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                {
                    throw new DataValidationException("Wymagana liczba całkowita dodatnia");
                }
                else
                {
                    this.RaiseAndSetIfChanged(ref _tDiameter, value);
                }
            }
        }
    }
    
    private string lifetime;

    public string Lifetime
    {
        get => lifetime;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (value.Contains('.'))
                {
                    throw new DataValidationException("Nieprawidłowy format liczby");
                }
                else
                {
                    if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        throw new DataValidationException("Wartość musi być dodatnia typu double");
                    }
                    else
                    {
                        this.RaiseAndSetIfChanged(ref lifetime, value);
                    }
                }
            }
        }
    }
    public MainWindowViewModel()
    {
        
        MyReferences.mwvm = this;
        
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

        var nrz = context.Narzedzies
            //.Where(narzedzie => narzedzie.IdKategorii == 1)
            .ToArray();

        var narzedziaCollection = new ObservableCollection<Narzedzie>(nrz);

        var kategorie = context.Kategoria
            .ToArray();

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
        
        Categories = new ObservableCollection<string>(
            kategorie.Select(n => n.Opis.ToString()).Distinct());

        Purpose = new ObservableCollection<string>(
            kategorie.Select(n => n.Przeznaczenie.ToString()).Distinct());

        Material = new ObservableCollection<string>(
            narzedziaCollection.Select(n => n.Material_wykonania.ToString()).Distinct());
    }
    
    
    public FlatTreeDataGridSource<Narzedzie> Source { get; }
}