using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToolsMenagement.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using ReactiveUI;

namespace ToolsMenagement.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<string>Categories { get; set; }
    public ObservableCollection<string>Purpose { get; set; }
    public ObservableCollection<string>Material { get; set; }
    
    
    private bool _isDataValid,_isDiameterValid,_isLifetimeValid;

    public bool IsDataValid
    {
        get => _isDataValid;
        set
        {
            if (_isDiameterValid & _isLifetimeValid)
            {
                this.RaiseAndSetIfChanged(ref _isDataValid, value);
            }
            
        }
    }
    
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

    public string Diameter
    {
        get => _tDiameter;
        set
        {
            double number;
            if(string.IsNullOrWhiteSpace(value))
            {
                IsDataValid = false;
                _isDiameterValid = false;
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (value.Contains('.'))
                {
                    IsDataValid = false;
                    _isDiameterValid = false;
                    throw new DataValidationException("Nieprawidłowy format liczby");
                }
                else
                {
                    if (!double.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        IsDataValid = false;
                        _isDiameterValid = false;
                        throw new DataValidationException("Wartość musi być dodatnia typu double");
                    }
                    else
                    {
                        this.RaiseAndSetIfChanged(ref _tDiameter, value);
                        _isDiameterValid = true;
                        IsDataValid = true;
                    }
                }
            }
        }
    }
    
    private string _lifetime;

    public string Lifetime
    {
        get => _lifetime;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                IsDataValid = false;
                _isLifetimeValid = false;
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (value.Contains('.'))
                {
                    IsDataValid = false;
                    _isLifetimeValid = false;
                    throw new DataValidationException("Nieprawidłowy format liczby");
                }
                else
                {
                    if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        IsDataValid = false;
                        _isLifetimeValid = false;
                        throw new DataValidationException("Wartość musi być dodatnia typu int");
                    }
                    else
                    {
                        this.RaiseAndSetIfChanged(ref _lifetime, value);
                        _isLifetimeValid = true;
                        IsDataValid = true;
                    }
                }
            }
        }
    }

    private string _afterRegeneration;

    public string AfterRegeneration
    {
        get => _afterRegeneration;
        set
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new DataValidationException("Pole nie może być puste");
            }
            else
            {
                if (!int.TryParse(value,out int number) || value.Contains('-') || value.Equals("0"))
                {
                    throw new DataValidationException("Numer jest liczbą naturalną większą od zera");
                }
                else
                {
                    this.RaiseAndSetIfChanged(ref _afterRegeneration, value);
                }
            }
        }
    }

    private string _warningtext;

    public string WarningText
    {
        get => _warningtext;
        set
        {
            this.RaiseAndSetIfChanged(ref _warningtext, value);
        }
    }

    public MainWindowViewModel()
    {
        
        MyReferences.mwvm = this;
        
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        context.SaveChanges();

       /* var narzedziaEXV = context.Kategoria
            .Select(kategorium => new ToolExv()
            {
                CKategorium = kategorium,
                Children = context.Narzedzies
                    .Where(narzedzie => narzedzie.IdKategorii==kategorium.IdKategorii)
                    .Select(narzedzie =>new ToolTx
                    {
                        CNarzedzie = narzedzie,
                        Children = context.Magazyns
                            .Where(magazyn => magazyn.IdNarzedzia==narzedzie.IdNarzedzia)
                            .ToList()
                    })
                    .ToList()
            })
            .ToArray();*/
        
        var nrz = context.Narzedzies
            //.Where(narzedzie => narzedzie.IdKategorii == 1)
            /*.GroupJoin(
                context.Magazyns,
                narzedzie =>narzedzie.IdNarzedzia,
                magazyn =>magazyn.IdNarzedzia,
                (narzedzie, magazyn) =>magazyn 
                )*/
            .ToList();

        List<Magazyn> narze = new List<Magazyn>();
        
        var Tools = context.Narzedzies
            .Join(
                context.Magazyns,
                narzedzie =>narzedzie.IdNarzedzia,
                magazyn =>magazyn.IdNarzedzia,
                (narzedzie, magazyn) =>magazyn 
            )
            .ToArray();
        foreach (var item in Tools)
        {
            narze.Add(item);
        }

        var narzedziaCollection = new ObservableCollection<Magazyn>(narze);

        var kategorie = context.Kategoria
            .ToArray();
        
        Source = new FlatTreeDataGridSource<Magazyn>(narzedziaCollection)
        {
            Columns =
            {
                new TextColumn<Magazyn, int>("PozycjaMagazynowa", x=> x.PozycjaMagazynowa),
                new TextColumn<Magazyn, int>("Id Narzędzia", x => x.IdNarzedzia),
                new TextColumn<Magazyn, int>("CyklRegeneracji", x => x.CyklRegeneracji),
                new TextColumn<Magazyn, double>("Trwalosc", x => x.Trwalosc),
            },
        };
        
        
        Categories = new ObservableCollection<string>(
            kategorie.Select(n => n.Opis.ToString()).Distinct());

        Purpose = new ObservableCollection<string>(
            kategorie.Select(n => n.Przeznaczenie.ToString()).Distinct());

        Material = new ObservableCollection<string>(
            kategorie.Select(n => n.MaterialWykonania.ToString()).Distinct());
    }
    
    
    public FlatTreeDataGridSource<Magazyn> Source { get; }
}