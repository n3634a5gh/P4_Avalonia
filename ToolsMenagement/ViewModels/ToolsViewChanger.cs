using System;
using System.Linq;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels
{
    public class ToolsViewChanger
    {
        public ToolsView[] AllToolsView()
        {
            var context = new ToolsDatabase1Context();
            context.Database.EnsureCreated();
            context.Database.Migrate();

            context.SaveChanges();

            var toolsView = context.Kategoria
                .Join(
                    context.Narzedzies,
                    kategorium => kategorium.IdKategorii,
                    narzedzie => narzedzie.IdKategorii,
                    (kategorium, narzedzie) => new { Kategoria = kategorium, Narzedzie = narzedzie }
                )
                .Join(
                    context.Magazyns,
                    joinResult => joinResult.Narzedzie.IdNarzedzia,
                    magazyn => magazyn.IdNarzedzia,
                    (joinResult, magazyn) => new ToolsView 
                    {
                        Kategoria = joinResult.Kategoria,
                        Narzedzie = joinResult.Narzedzie,
                        Magazyn = magazyn
                    }
                )
                .ToArray();

            return toolsView;
        }
        
        public ToolsView[] ChangeToolsView(string[] filters)
        {
            var context = new ToolsDatabase1Context();
            context.Database.EnsureCreated();
            context.Database.Migrate();

            bool withoutfilters = true;
            ToolsView[] toolsResult;

            for (int i = 0; i < filters.Length; i++)
            {
                if (filters[i] != "")
                {
                    withoutfilters = false;
                    break;
                }
            }


            var toolsView = context.Kategoria
                .Join(
                    context.Narzedzies,
                    kategorium => kategorium.IdKategorii,
                    narzedzie => narzedzie.IdKategorii,
                    (kategorium, narzedzie) => new { Kategoria = kategorium, Narzedzie = narzedzie }
                )
                .Join(
                    context.Magazyns,
                    joinResult => joinResult.Narzedzie.IdNarzedzia,
                    magazyn => magazyn.IdNarzedzia,
                    (joinResult, magazyn) => new ToolsView 
                    {
                        Kategoria = joinResult.Kategoria,
                        Narzedzie = joinResult.Narzedzie,
                        Magazyn = magazyn
                    }
                )
                .ToArray();
            
            int counter = 0;
            int select_index = 0;

            for (int i = 0; i < filters.Length; i++)
            {
                if (!string.IsNullOrEmpty(filters[i]))
                {
                    select_index = i;
                    break;
                }
            }

            switch (select_index)
            {
                case 0:
                {
                    if (!string.IsNullOrEmpty(filters[0]))
                    {
                        foreach (var item in toolsView)
                        {
                            if (item.Narzedzie.Nazwa.IndexOf(filters[0], StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                counter++;
                            }
                        }
                    }
                    break;
                }
                case 1:
                {
                    if (!string.IsNullOrEmpty(filters[1]))
                    {
                        foreach (var item in toolsView)
                        {
                            if (item.Magazyn.PozycjaMagazynowa.ToString().IndexOf(filters[1], StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                counter++;
                            }
                        }
                    }
                    break;
                }
                case 2:
                {
                    if (!string.IsNullOrEmpty(filters[2]))
                    {
                        foreach (var item in toolsView)
                        {
                            if (item.Narzedzie.IdNarzedzia.ToString().IndexOf(filters[2], StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                counter++;
                            }
                        }
                    }
                    break;
                }
                case 3:
                {
                    if (!string.IsNullOrEmpty(filters[3]))
                    {
                        foreach (var item in toolsView)
                        {
                            if (item.Narzedzie.Srednica.ToString().IndexOf(filters[3], StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                counter++;
                            }
                        }
                    }
                    break;
                }
            }

            toolsResult = new ToolsView[counter];
            counter = 0;

            if (select_index == 0)
            {
                foreach (var item in toolsView)
                {
                    if (item.Narzedzie.Nazwa.IndexOf(filters[0], StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        toolsResult[counter] = item;
                        counter++;
                    }
                }
            }
            
            if (select_index == 1)
            {
                foreach (var item in toolsView)
                {
                    if (item.Magazyn.PozycjaMagazynowa.ToString().IndexOf(filters[1], StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        toolsResult[counter] = item;
                        counter++;
                    }
                }
            }
            
            if (select_index == 2)
            {
                foreach (var item in toolsView)
                {
                    if (item.Narzedzie.IdNarzedzia.ToString().IndexOf(filters[2], StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        toolsResult[counter] = item;
                        counter++;
                    }
                }
            }
            
            if (select_index == 3)
            {
                foreach (var item in toolsView)
                {
                    if (item.Narzedzie.Srednica.ToString().IndexOf(filters[3], StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        toolsResult[counter] = item;
                        counter++;
                    }
                }
            }

            if (withoutfilters)
            {
                return toolsView;
            }
            else
            {
                return toolsResult;
            }
        }
    }

    public class ToolsView
    {
        public Kategorium Kategoria { get; set; }
        public Narzedzie Narzedzie { get; set; }
        public Magazyn Magazyn { get; set; }
    }
}