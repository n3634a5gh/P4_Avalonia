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

        int select_category = 0;
        foreach (var item in context.Kategoria)
        {
            if (item.Opis == MyReferences.mwvm.SelectedCategory)
            {
                if (item.Przeznaczenie == MyReferences.mwvm.SelectedPurpose)
                {
                    if (item.MaterialWykonania == MyReferences.mwvm.SelectedMaterial)
                    {
                        select_category = item.IdKategorii;
                    }
                }
            }
        }
        
        string temp = MyReferences.mwvm.Diameter;
        double zxc = Convert.ToDouble(temp);
        
        int existing_tool = 0;
        
        foreach (var item in context.Narzedzies)
        {
            if (item.IdKategorii == select_category)
            {
                if(item.Srednica==zxc)
                {
                    existing_tool = item.IdNarzedzia;
                }
            }
        }

        if (existing_tool == 0)
        {
            var narzedzie = new Narzedzie()
            {
                IdKategorii = select_category,
                Srednica = zxc,
                Nazwa = Create_name.Tool_Name(MyReferences.mwvm.SelectedCategory,
                    zxc,
                    MyReferences.mwvm.SelectedMaterial,
                    MyReferences.mwvm.SelectedPurpose),
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
        
            context.Add(narzedzie);
            context.SaveChanges();

            int newtoolid = 0;
            foreach (var item2 in context.Narzedzies)
            {
                if (item2.IdKategorii == select_category)
                {
                    if (item2.Srednica == zxc)
                    {
                        newtoolid = item2.IdNarzedzia;
                    }
                }
            }
            
            string message=$"Dodano nowe narzędzie nr. {newtoolid}";
            var newmessage = new Messages().UniversalMessage(message, MyReferences.MainView,"",false);
        }
        else
        {
            var magazyn = new Magazyn()
            {
                IdNarzedzia = existing_tool,
                Trwalosc = Convert.ToInt32(MyReferences.mwvm.Lifetime),
                Uzycie = 0,
                CyklRegeneracji = 0,
                Wycofany = false
            };
            context.Add(magazyn);
            context.SaveChanges();
            
            string message=$"Dodano nową pozycję magazynową narzędzia {existing_tool}";
            var newmessage = new Messages().UniversalMessage(message, MyReferences.MainView,"",false);
        }
    }
}