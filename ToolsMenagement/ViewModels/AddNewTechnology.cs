using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class AddNewTechnology
{
    public AddNewTechnology(string technologyName, string [][] tools)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        var technology = new Technologium()
        {
            Opis = technologyName,
            DataUtworzenia = DateTime.Now,
            
            /*NarzedziaTechnologia = new List<NarzedziaTechnologium>()
            {
                new NarzedziaTechnologium()
                {
                    
                }
            }*/
        };
        
        technology.NarzedziaTechnologia.Add(new NarzedziaTechnologium()
        {
            
        });
        
        for(int i=0;i<=)
        
        context.Add(technology);
        context.SaveChanges();

        /*int select_category = 0;
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
        }*/
    }
}