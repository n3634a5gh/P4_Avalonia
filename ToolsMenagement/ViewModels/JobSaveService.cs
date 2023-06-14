using System;
using System.Linq;
using Avalonia.Animation.Easings;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class JobSaveService
{
    public bool check_order(int orderId)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        bool correctOrderId = false, _exist=false;
        string message = "";
        
        foreach (var item in context.Zlecenies)
        {
            if (item.IdZlecenia == orderId)
            {
                _exist = true;
                if (item.Aktywne)
                {
                    correctOrderId = true;
                }
            }
        }

        if (_exist)
        {
            if (!correctOrderId)
            {
                message = "Rejestracja na podanym zleceniu nie jest możliwa\n z " +
                          "powodu jego zakończenia";
            }
        }
        else
        {
            message = "Podany identyfikator zlecenia\nnie jest prawidłowy";
        }

        if (correctOrderId)
        {
            return true;
        }
        else
        {
            var newmessage = new Messages().UniversalMessage(message, MyReferences.registerview,"",false);
            return false;
        }
    }

    public void UpdateRegisterTable(int order,int quantity,string employee)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        var newPosition = new Rejestracja()
        {
            IdZlecenia = order,
            Sztuk = quantity,
            DataWykonania = DateTime.Now,
            Wykonal = employee
        };
        context.Add(newPosition);
        context.SaveChanges();
    }
    
    private bool InsteadTriggersOnDatabase(int [][]table)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        bool close_order = false;

        for (int i = 0; i < table[0].Length; i++)
        {
            foreach (var item in context.Magazyns)
            {
                if (table[0][i] == item.PozycjaMagazynowa)
                {
                    if (item.Uzycie >= item.Trwalosc)
                    {
                        close_order = true;
                        if (item.CyklRegeneracji >= 4)
                        {
                            item.Wycofany = true;
                        }
                        else
                        {
                            item.Regeneracja = true;
                        }
                    }
                    break;
                }
            }

            context.SaveChanges();
        }

        return close_order;
    }
    

    private int[][] GetValidToolsId(int order)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        int[][] validToolId;

        var technologystorageTools = context.Zlecenies
            .Where(zlecenie => zlecenie.IdZlecenia == order)
            .Join(
                context.Technologia,
                zlecenie => zlecenie.IdTechnologi,
                technologium => technologium.IdTechnologi,
                (zlecenie, technologium) => technologium
            )
            .Join(
                context.NarzedziaTechnologia,
                technologium =>technologium.IdTechnologi,
                technologium => technologium.IdTechnologi,
                (technologium, narzedziaTechnologium) => narzedziaTechnologium
                )
            .Join(
                context.Narzedzies,
                technologium =>technologium.IdNarzedzia,
                narzedzie =>narzedzie.IdNarzedzia,
                (technologium, narzedzie) =>narzedzie 
            )
            .Join(
                context.Magazyns,
                narzedzie =>narzedzie.IdNarzedzia,
                magazyn =>magazyn.IdNarzedzia,
                (narzedzie, magazyn) =>magazyn 
                )
            .Where(magazyn =>magazyn.Regeneracja==false )
            .Where(magazyn =>magazyn.Wycofany==false )
            .OrderByDescending(magazyn =>magazyn.Uzycie )
            .ToArray();

        var technologyTools = technologystorageTools
            .DistinctBy(magazyn => magazyn.IdNarzedzia)
            .ToArray();

        var timeUseTool = context.Zlecenies
            .Where(zlecenie => zlecenie.IdZlecenia == order)
            .Join(
                context.Technologia,
                zlecenie => zlecenie.IdTechnologi,
                technologium => technologium.IdTechnologi,
                (zlecenie, technologium) => technologium
            )
            .Join(
                context.NarzedziaTechnologia,
                technologium => technologium.IdTechnologi,
                technologium => technologium.IdTechnologi,
                (technologium, narzedziaTechnologium) => narzedziaTechnologium
            )
            .ToArray();

        int i = 0;

        validToolId = new int[2][];
        for (int j = 0; j < validToolId.Length; j++)
        {
            validToolId[j] = new int[technologyTools.Length];
        }
        
    //pobierz dane o pozycji magazynowej narzedzia oraz jego zużycia jednostkowego 
    //na 1 sztukę wyrobu gotowego
    
        foreach (var item in technologyTools)
        {
            foreach (var item2 in technologystorageTools)
            {
                if (item.IdNarzedzia == item2.IdNarzedzia)
                {
                    validToolId[0][i]=item2.PozycjaMagazynowa;
                    
                    foreach (var item3 in timeUseTool)
                    {
                        if (item3.IdNarzedzia == item2.IdNarzedzia)
                        {
                            validToolId[1][i] = item3.CzasPracy;
                        }
                    }
                    i++;
                    break;
                }
            }
        }

        return validToolId;
    }

    public bool UpdateWarehouse(int order,int quantity)
    {
        
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        bool order_close = false;
        
        int[][] storageToolsPositions = GetValidToolsId(order);
        
        //update użycia narzędzi w tabeli Magazyn
        for (int i = 0; i < storageToolsPositions[0].Length; i++)
        {
            foreach (var item in context.Magazyns)
            {
                if (storageToolsPositions[0][i] == item.PozycjaMagazynowa)
                {
                    item.Uzycie = item.Uzycie + (storageToolsPositions[1][i] * quantity);
                    break;
                }
            }
            context.SaveChanges();
        }

        if (InsteadTriggersOnDatabase(storageToolsPositions))
        {
            order_close = true;
            foreach (var item in context.Zlecenies)
            {
                if (item.IdZlecenia == order & item.Aktywne)
                {
                    item.Aktywne = false;
                    break;
                }
            }

            context.SaveChanges();

        }

        return order_close;
    }

    public void UpdateDamagedAndToRestoreTools(int order, string [][] tab)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        //znajdź id narzedzia
        int[] tools_id = new int[tab[0].Length];
        int positionsInTable=0;
        
        foreach (var item in context.Narzedzies)
        {
            if (item.Nazwa == tab[0][positionsInTable])
            {
                tools_id[positionsInTable] = item.IdNarzedzia;
                positionsInTable++;
            }
        }
        
        int[][] storageToolsPositions = GetValidToolsId(order);

        foreach (var item in context.Magazyns)
        {
            for (int i = 0; i < tab[0].Length; i++)
            {
                if (item.PozycjaMagazynowa == storageToolsPositions[0][i])
                {
                    for (int j = 0; j < tools_id.Length; j++)
                    {
                        if (item.IdNarzedzia == tools_id[j])
                        {
                            if (tab[1][j] == "Do regeneracji")
                            {
                                if (item.CyklRegeneracji >= 4)
                                {
                                    item.Regeneracja = true;
                                    item.Wycofany = true;
                                }
                                else
                                {
                                    item.Regeneracja = true;
                                }
                            }
                            else
                            {
                                item.Wycofany = true;
                            }
                        }
                    }
                }
            }
        }
        context.SaveChanges();
        
        foreach (var item in context.Zlecenies)
        {
            if (item.IdZlecenia == order & item.Aktywne)
            {
                item.Aktywne = false;
                break;
            }
        }

        context.SaveChanges();
    }
    
}