using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class AddNewTool
{
    public AddNewTool()
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();
        
        var kategoria = new Kategorium()
        {
            Opis = MyReferences.mwvm.SelectedCategory,
            Przeznaczenie = MyReferences.mwvm.SelectedPurpose
        };
        
        var narzedzie = new Narzedzie()
        {
            Material_wykonania = MyReferences.mwvm.SelectedMaterial,
            Srednica = Convert.ToInt32(MyReferences.mwvm.T_diameter),
            Ilosc_ostrzy = Convert.ToInt32(MyReferences.mwvm.CEdges),
            Nazwa = "zxc",
            Magazyns = new List<Magazyn>()
            {
                new Magazyn()
                {
                    Trwalosc = Convert.ToInt32(MyReferences.mwvm.Lifetime),
                    Uzycie = 0,
                    CyklRegeneracji = 0,
                    Wycofany = false
                }
            }
        };
        kategoria.Narzedzies.Add(narzedzie);
        context.Add(kategoria);
        context.SaveChanges();
        
    }
}