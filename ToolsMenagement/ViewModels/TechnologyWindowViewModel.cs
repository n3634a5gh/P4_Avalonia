﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Data;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class TechnologyWindowViewModel:ViewModelBase
{
    
    public ObservableCollection<string>Categories { get; set; }
    public ObservableCollection<string>Purpose { get; set; }
    public ObservableCollection<string>Material { get; set; }

    private bool _isenable1, _isenable2, _isenable3,_isenable4;

    public bool IsEnable1
    {
        get => _isenable1;
        set
        {
            this.RaiseAndSetIfChanged(ref _isenable1, value);
        }
    }
    
    public bool IsEnable2
    {
        get => _isenable2;
        set
        {
            this.RaiseAndSetIfChanged(ref _isenable2, value);
        }
    }
    
    public bool IsEnable3
    {
        get => _isenable3;
        set
        {
            this.RaiseAndSetIfChanged(ref _isenable3, value);
        }
    }
    
    public bool IsEnable4
    {
        get => _isenable4;
        set
        {
            this.RaiseAndSetIfChanged(ref _isenable4, value);
        }
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
                    IsEnable4 = false;
                    throw new DataValidationException("Pole nie może być puste");
                }
                else
                {
                    if (value.Contains('.'))
                    {
                        IsEnable4 = false;
                        throw new DataValidationException("Nieprawidłowy format liczby");
                    }
                    else
                    {
                        if (!double.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                        {
                            IsEnable4 = false;
                            throw new DataValidationException("Wartość musi być dodatnia typu double");
                        }
                        else
                        {
                            IsEnable4 = true;
                            this.RaiseAndSetIfChanged(ref _tDiameter, value);
                        }
                    }
                }
            }
        }

    private string _selectedCategory;
    
    public string SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedCategory, value);
            IsEnable1 = true;
        }
        
    }

    private string _selectedPurpose;

    public string SelectedPurpose
    {
        get => _selectedPurpose;
        set
        {
            IsEnable2 = true;
            this.RaiseAndSetIfChanged(ref _selectedPurpose, value);
        }
    }
    
    private string _selectedMaterial;

    public string SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            IsEnable3 = true;
            this.RaiseAndSetIfChanged(ref _selectedMaterial, value);
        }
    }

    public TechnologyWindowViewModel()
    {
        MyReferences.twvm = this;
        
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        context.SaveChanges();
        
        var kategorie = context.Kategoria
            .ToArray();
        
        Categories = new ObservableCollection<string>(
            kategorie.Select(n => n.Opis.ToString()).Distinct());

        Purpose = new ObservableCollection<string>(
            kategorie.Select(n => n.Przeznaczenie.ToString()).Distinct());

        Material = new ObservableCollection<string>(
            kategorie.Select(n => n.MaterialWykonania.ToString()).Distinct());

    }
}