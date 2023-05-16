using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class AddNewTechnology
{
    public async Task<bool> AddNewTech(string technologyName, string [][] tools)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        int[] toolsIdArray = new int[tools.Length];

        for (int i = 0; i < tools.Length; i++)
        {
            foreach (var item in context.Kategoria)
            {
                if (item.Opis == tools[i][0])
                {
                    if (item.Przeznaczenie == tools[i][1])
                    {
                        if (item.MaterialWykonania == tools[i][2])
                        {
                            toolsIdArray[i] = item.IdKategorii;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < tools.Length; i++)
        {
            foreach (var item in context.Narzedzies)
            {
                if (item.IdKategorii == toolsIdArray[i])
                {
                    if (item.Srednica.ToString() == tools[i][3])
                    {
                        toolsIdArray[i] = item.IdNarzedzia;
                    }
                }
            }
        }

        var technology = new Technologium()
        {
            Opis = technologyName,
            DataUtworzenia = DateTime.Now,
        };
        
       for (int i = 0; i < tools.Length; i++)
        {
            technology.NarzedziaTechnologia.Add(new NarzedziaTechnologium()
            {
                IdNarzedzia = toolsIdArray[i],
                CzasPracy = Convert.ToInt32(tools[i][4])
            });
        }
       
        context.Add(technology);
        context.SaveChanges();
        
        int lastTechnologyId = 0;
        foreach (var item2 in context.NarzedziaTechnologia)
        {
            if (lastTechnologyId < item2.IdTechnologi)
            {
                lastTechnologyId = item2.IdTechnologi;
            }
        }
        
        string message=$"Utworzono technologię nr. {lastTechnologyId}";
        var newmessage = new Messages().UniversalMessage(message, MyReferences.techview,"",true);
        return true;
    }
}