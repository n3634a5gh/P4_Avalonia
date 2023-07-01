using System.Collections.Generic;
using System.Collections.ObjectModel;
using ToolsMenagement.Models;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using ReactiveUI;

namespace ToolsMenagement.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<string>Categories { get; set; }
    public ObservableCollection<string>Purpose { get; set; }
    public ObservableCollection<string>Material { get; set; }


    private ObservableCollection<ToolsDBView> _toolsDbViews = new ObservableCollection<ToolsDBView>();
    private ObservableCollection<Zlecenie> _zlecenies;
    private ObservableCollection<TechnologyView> _technologyViews= new ObservableCollection<TechnologyView>();

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

    public void change_view_tools_results(string[] filterTable)
    { 
        _toolsDbViews = new ObservableCollection<ToolsDBView>();
        bool empty_filters = true;

        for (int i = 0; i < filterTable.Length; i++)
        {
            if (filterTable[i] != "")
            {
                empty_filters = false;
                break;
            }
        }

        ToolsView[] toolsView; 
        if (empty_filters)
        {
            toolsView = new ToolsViewChanger().AllToolsView();
        }
        else
        {
           toolsView=new ToolsViewChanger().ChangeToolsView(filterTable);
        }
        
        
        
        foreach (var item in toolsView)
        {
            _toolsDbViews.Add(new ToolsDBView()
            {
                PozycjaMagazynowa = item.Magazyn.PozycjaMagazynowa.ToString(),
                IDNarzedzia = item.Narzedzie.IdNarzedzia.ToString(),
                Nazwa = item.Narzedzie.Nazwa,
                Srednica = item.Narzedzie.Srednica,
                Trwalosc = item.Magazyn.Trwalosc,
                Uzycie = item.Magazyn.Uzycie,
                CyklRegeneracji = item.Magazyn.CyklRegeneracji,
                Wycofany = item.Magazyn.Wycofany,
                Regeneracja = item.Magazyn.Regeneracja
            });
        }
        var toolsCollection = new ObservableCollection<ToolsDBView>(_toolsDbViews);
        
        Source2 = new FlatTreeDataGridSource<ToolsDBView>(toolsCollection)
        {
            Columns =
            {
                new TextColumn<ToolsDBView, string>("Pozycja", x=> x.PozycjaMagazynowa),
                new TextColumn<ToolsDBView, string>("Narzędzie", x=> x.IDNarzedzia),
                new TextColumn<ToolsDBView, string>("Nazwa", x=> x.Nazwa),
                new TextColumn<ToolsDBView, double>("Średnica", x => x.Srednica),
                new TextColumn<ToolsDBView, int>("Trwałość", x => x.Trwalosc),
                new TextColumn<ToolsDBView, int>("Użycie", x => x.Uzycie),
                new TextColumn<ToolsDBView, int>("CyklRegeneracji", x => x.CyklRegeneracji),
                new TextColumn<ToolsDBView, bool>("Wycofany", x => x.Wycofany),
                new TextColumn<ToolsDBView, bool>("W regeneracji", x => x.Regeneracja),
            },
        };
    }

    public void refresh_source()
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        context.SaveChanges();

        var orders = context.Zlecenies
            .ToArray();

        var alltechnologies = context.Technologia;

        var technologyView = context.Technologia
            .Join(
                context.NarzedziaTechnologia,
                technologium => technologium.IdTechnologi,
                technologium => technologium.IdTechnologi,
                (technologium, narzedziaTechnologium) => new { Technologium=technologium,NarzedziaTechnologium=narzedziaTechnologium}
            )
            .Join(
                context.Narzedzies,
                joinResult => joinResult.NarzedziaTechnologium.IdNarzedzia,
                narzedzie => narzedzie.IdNarzedzia,
                (joinResult,narzedzie)=>new {joinResult.Technologium,joinResult.NarzedziaTechnologium,Narzedzie=narzedzie}
            )
            .OrderBy(technologium=>technologium.Technologium.IdTechnologi)
            .ToArray();
        
        foreach (var item in alltechnologies)
        {
            var parentTechnology = new TechnologyView()
            {
                Opis = item.Opis,
                IdNarzedzia = "",
                Uzycie = ""
            };

            foreach (var item1 in technologyView)
            {
                if (item1.NarzedziaTechnologium.IdTechnologi==item.IdTechnologi)
                {
                    parentTechnology.Children.Add(new TechnologyView()
                    {
                        IdNarzedzia = item1.NarzedziaTechnologium.IdNarzedzia.ToString(),
                        Uzycie = item1.NarzedziaTechnologium.CzasPracy.ToString(),
                        IDTechnologii = item.IdTechnologi.ToString()
                    });
                }
            }

            _technologyViews.Add(parentTechnology);
        }
        var technologiesCollection = new ObservableCollection<TechnologyView>(_technologyViews);
        
        var kategorie = context.Kategoria
            .ToArray();
        
        Source = new FlatTreeDataGridSource<Zlecenie>(orders)
        {
            Columns =
            {
                new TextColumn<Zlecenie, int>("IdZlecenia", x=> x.IdZlecenia),
                new TextColumn<Zlecenie, int>("IdTechnologi", x => x.IdTechnologi),
                new TextColumn<Zlecenie, bool>("Aktywne", x => x.Aktywne),
            },
        };
        
        Source3 = new HierarchicalTreeDataGridSource<TechnologyView>(technologiesCollection)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<TechnologyView>(
                    new TextColumn<TechnologyView,string>("Opis",x=>x.Opis),
                    x=>x.Children),
                new TextColumn<TechnologyView,string>("ID Technologii",x=>x.IDTechnologii.ToString()),
                new TextColumn<TechnologyView,string>("Narzędzie",x=>x.IdNarzedzia.ToString()),
                new TextColumn<TechnologyView,string>("Użycie",x=>x.Uzycie.ToString())
            }
        };
    }

    public MainWindowViewModel()
    {
        
        MyReferences.mwvm = this;
        
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        context.SaveChanges();
        
        _toolsDbViews = new ObservableCollection<ToolsDBView>();
        
        var toolsView=new ToolsViewChanger().AllToolsView();
        
        var orders = context.Zlecenies
            .ToArray();

        var alltechnologies = context.Technologia;

        var technologyView = context.Technologia
            .Join(
                context.NarzedziaTechnologia,
                technologium => technologium.IdTechnologi,
                technologium => technologium.IdTechnologi,
                (technologium, narzedziaTechnologium) => new { Technologium=technologium,NarzedziaTechnologium=narzedziaTechnologium}
                )
            .Join(
                context.Narzedzies,
                joinResult => joinResult.NarzedziaTechnologium.IdNarzedzia,
                narzedzie => narzedzie.IdNarzedzia,
                (joinResult,narzedzie)=>new {joinResult.Technologium,joinResult.NarzedziaTechnologium,Narzedzie=narzedzie}
            )
            .OrderBy(technologium=>technologium.Technologium.IdTechnologi)
            .ToArray();

        foreach (var item in toolsView)
        {
            _toolsDbViews.Add(new ToolsDBView()
            {
                PozycjaMagazynowa = item.Magazyn.PozycjaMagazynowa.ToString(),
                IDNarzedzia = item.Narzedzie.IdNarzedzia.ToString(),
                Nazwa = item.Narzedzie.Nazwa,
                Srednica = item.Narzedzie.Srednica,
                Trwalosc = item.Magazyn.Trwalosc,
                Uzycie = item.Magazyn.Uzycie,
                CyklRegeneracji = item.Magazyn.CyklRegeneracji,
                Wycofany = item.Magazyn.Wycofany,
                Regeneracja = item.Magazyn.Regeneracja
            });
        }

        foreach (var item in alltechnologies)
        {
            var parentTechnology = new TechnologyView()
            {
                Opis = item.Opis,
                IdNarzedzia = "",
                Uzycie = ""
            };

            foreach (var item1 in technologyView)
            {
                if (item1.NarzedziaTechnologium.IdTechnologi==item.IdTechnologi)
                {
                    parentTechnology.Children.Add(new TechnologyView()
                    {
                        IdNarzedzia = item1.NarzedziaTechnologium.IdNarzedzia.ToString(),
                        Uzycie = item1.NarzedziaTechnologium.CzasPracy.ToString(),
                        IDTechnologii = item.IdTechnologi.ToString()
                    });
                }
            }

            _technologyViews.Add(parentTechnology);
        }


        var toolsCollection = new ObservableCollection<ToolsDBView>(_toolsDbViews);
        //var ordersCollection = new ObservableCollection<Zlecenie>(orders);
        var technologiesCollection = new ObservableCollection<TechnologyView>(_technologyViews);
        
        var kategorie = context.Kategoria
            .ToArray();
        
        Source = new FlatTreeDataGridSource<Zlecenie>(orders)
        {
            Columns =
            {
                new TextColumn<Zlecenie, int>("IdZlecenia", x=> x.IdZlecenia),
                new TextColumn<Zlecenie, int>("IdTechnologi", x => x.IdTechnologi),
                new TextColumn<Zlecenie, bool>("Aktywne", x => x.Aktywne),
            },
        };
        
        Source2 = new FlatTreeDataGridSource<ToolsDBView>(toolsCollection)
        {
            Columns =
            {
                new TextColumn<ToolsDBView, string>("Pozycja", x=> x.PozycjaMagazynowa),
                new TextColumn<ToolsDBView, string>("Narzędzie", x=> x.IDNarzedzia),
                new TextColumn<ToolsDBView, string>("Nazwa", x=> x.Nazwa),
                new TextColumn<ToolsDBView, double>("Średnica", x => x.Srednica),
                new TextColumn<ToolsDBView, int>("Trwałość", x => x.Trwalosc),
                new TextColumn<ToolsDBView, int>("Użycie", x => x.Uzycie),
                new TextColumn<ToolsDBView, int>("CyklRegeneracji", x => x.CyklRegeneracji),
                new TextColumn<ToolsDBView, bool>("Wycofany", x => x.Wycofany),
                new TextColumn<ToolsDBView, bool>("W regeneracji", x => x.Regeneracja),
            },
        };

        Source3 = new HierarchicalTreeDataGridSource<TechnologyView>(technologiesCollection)
        {
            Columns =
            {
                new HierarchicalExpanderColumn<TechnologyView>(
                    new TextColumn<TechnologyView,string>("Opis",x=>x.Opis),
                    x=>x.Children),
                new TextColumn<TechnologyView,string>("ID Technologii",x=>x.IDTechnologii.ToString()),
                new TextColumn<TechnologyView,string>("Narzędzie",x=>x.IdNarzedzia.ToString()),
                new TextColumn<TechnologyView,string>("Użycie",x=>x.Uzycie.ToString())
            }
        };
        
        Categories = new ObservableCollection<string>(
            kategorie.Select(n => n.Opis.ToString()).Distinct());

        Purpose = new ObservableCollection<string>(
            kategorie.Select(n => n.Przeznaczenie.ToString()).Distinct());

        Material = new ObservableCollection<string>(
            kategorie.Select(n => n.MaterialWykonania.ToString()).Distinct());
    }
    
    
    public FlatTreeDataGridSource<Zlecenie> Source { get; set; }
    public  FlatTreeDataGridSource<ToolsDBView> Source2 { get; set; }

    public HierarchicalTreeDataGridSource<TechnologyView> Source3 { get; set; }
}