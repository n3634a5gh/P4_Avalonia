using System;
using System.Linq;
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

        bool correctOrderId = false;
        
        foreach (var item in context.Zlecenies)
        {
            if (item.IdZlecenia == orderId)
            {
                if (item.Aktywne)
                {
                    correctOrderId = true;
                }
            }
        }

        if (correctOrderId)
        {
            return true;
        }
        else
        {
            string message = "Podany identyfikator zlecenia\nnie jest prawidłowy";
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

    private int[] GetValidToolsId(int order)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        int[] validToolId;

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

        int i = 0;

        validToolId = new int[technologyTools.Length];

        foreach (var item in technologyTools)
        {
            foreach (var item2 in technologystorageTools)
            {
                if (item.IdNarzedzia == item2.IdNarzedzia)
                {
                    validToolId[i]=item2.PozycjaMagazynowa;
                    i++;
                    break;
                }
            }
        }
        
        return validToolId;
    }

    public void UpdateWarehouse(int order)
    {
        int[] table = GetValidToolsId(order);
    }
}