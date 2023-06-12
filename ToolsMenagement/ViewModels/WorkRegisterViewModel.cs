using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Data;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class WorkRegisterViewModel:ViewModelBase
{
    
    public ObservableCollection<string>DamagedTools { get; set; }
    public ObservableCollection<string> DamageType { get; set; }

    private bool _isenable1,_isenable2,_enable3,_enable2,_enable1,_contentenable;
    
    
    public bool ContentEnable
    {
        get => _contentenable;
        set
        {
            this.RaiseAndSetIfChanged(ref _contentenable, value);
        }
    }
    public bool Enable1
    {
        get => _enable1;
        set
        {
            this.RaiseAndSetIfChanged(ref _enable1, value);
            if (Enable2 & Enable3 & Enable1)
            {
                ContentEnable = true;
            }
            else
            {
                ContentEnable = false;
            }
        }
    }
    public bool Enable2
    {
        get => _enable2;
        set
        {
            this.RaiseAndSetIfChanged(ref _enable2, value);
            if (Enable1 & Enable3 & Enable2)
            {
                ContentEnable = true;
            }
            else
            {
                ContentEnable = false;
            }
        }
    }
    public bool Enable3
    {
        get => _enable3;
        set
        {
            this.RaiseAndSetIfChanged(ref _enable3, value);
            if (Enable2 & Enable1 & Enable3)
            {
                ContentEnable = true;
            }
            else
            {
                ContentEnable = false;
            }
        }
    }
    
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
    
    private string _selectedTool;
    
    public string SelectedTool
    {
        get => _selectedTool;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTool, value);
            IsEnable1 = true;
        }
    }
    
    private int _selectedDamage;
    
    public int SelectedDamage
    {
        get => _selectedDamage;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDamage, value);
            IsEnable2 = true;
        }
    }
    
    private string _eployeeName;

    public string EmployeeName
    {
        get => _eployeeName;
        set
        {
            int number;
            if (string.IsNullOrWhiteSpace(value))
            {
                Enable1 = false;
                throw new DataValidationException("");

            }
            else
            {
                if (value.Length > 64)
                {
                    Enable1 = false;
                    throw new DataValidationException("");
                }
                else
                {
                    Enable1 = true;
                    this.RaiseAndSetIfChanged(ref _eployeeName, value);
                }
            }
        }
    }
    
    private string _orderNumber;

    public string OrderNumber
    {
        get => _orderNumber;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                Enable2 = false;
                throw new DataValidationException("");
            }
            else
            {
                if (value.Contains('.'))
                {
                    Enable2 = false;
                    throw new DataValidationException("");
                }
                else
                {
                    if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        Enable2 = false;
                        throw new DataValidationException("");
                    }
                    else
                    {
                        Enable2 = true;
                        this.RaiseAndSetIfChanged(ref _orderNumber, value);
                    }
                }
            }
        }
    }
    
    private string _quantity;

    public string Quantity
    {
        get => _quantity;
        set
        {
            int number;
            if(string.IsNullOrWhiteSpace(value))
            {
                Enable3 = false;
                throw new DataValidationException("");
            }
            else
            {
                if (value.Contains('.'))
                {
                    Enable3 = false;
                    throw new DataValidationException("");
                }
                else
                {
                    if (!int.TryParse(value, out number) || value.Contains('-') || value.Equals("0"))
                    {
                        Enable3 = false;
                        throw new DataValidationException("");
                    }
                    else
                    {
                        Enable3 = true;
                        this.RaiseAndSetIfChanged(ref _quantity, value);
                    }
                }
            }
        }
    }
    

    
    private bool _option1Checked;

    public bool Option1Checked
    {
        get => _option1Checked;
        set
        {
            int number;
            this.RaiseAndSetIfChanged(ref _option1Checked, value);
        }
    }
    
    private bool _option2Checked;

    public bool Option2Checked
    {
        get => _option2Checked;
        set
        {
            int number;
            this.RaiseAndSetIfChanged(ref _option2Checked, value);
        }
    }

    public void regenerate_source(int orderNr)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        var narzedzia = context.Zlecenies
            .Where(zlecenie => zlecenie.IdZlecenia==Convert.ToInt32(orderNr))
            .Join(
                context.Technologia,
                zlecenie =>zlecenie.IdTechnologi,
                technologium => technologium.IdTechnologi,
                ((zlecenie, technologium) => technologium)
            )
            .Join(
                context.NarzedziaTechnologia,
                technologium => technologium.IdTechnologi,
                technologium => technologium.IdTechnologi,
                ((technologium, narzedziaTechnologium) => narzedziaTechnologium)
            )
            .Join(
                context.Narzedzies,
                technologium => technologium.IdNarzedzia,
                narzedzie => narzedzie.IdNarzedzia,
                ((technologium, narzedzie) => narzedzie)
            )
            .ToArray();

        DamagedTools = new ObservableCollection<string>(
            narzedzia.Select(n => n.Nazwa.ToString()).Distinct());
    }

    public WorkRegisterViewModel()
    {
        MyReferences.wrvm = this;
        Option1Checked = true;
    }
}