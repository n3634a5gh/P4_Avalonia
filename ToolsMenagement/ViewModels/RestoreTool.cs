using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class RestoreTool
{
    public async Task ExecuteRestoreTool()
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        var message = "";
        int toolPosition = 0;
        bool canToolRestore = false;

        foreach (var item in context.Magazyns)
        {
            if (item.PozycjaMagazynowa == Convert.ToInt32(MyReferences.mwvm.AfterRegeneration))
            {
                if (!item.Wycofany & item.Regeneracja)
                {
                    toolPosition = item.PozycjaMagazynowa;
                    canToolRestore = true;
                }
                else
                {
                    if (!item.Regeneracja & !item.Wycofany)
                    {
                        message = "Narzędzie nie zostało poddane regeneracji.\n" +
                                  "Nie jest możliwe jego przywrócenie.";
                        break;
                    }
                    else
                    {
                        message = "Narzędzie zostało wycofane z eksploatacji.\n" +
                                  "Nie jest możliwe jego przywrócenie.";
                        break;
                    }
                }
            }
            else
            {
                message = "Nie odnaleziono narzędzia w bazie.\n" +
                          "Podaj inny numer narzędzia.";
            }
        }
        
        if (!canToolRestore)
        {
            var newmessage = new Messages().UniversalMessage(message, MyReferences.MainView,"Błąd",false);
        }

        if (toolPosition != 0)
        {
            foreach (var item in context.Magazyns)
            {
               if (item.PozycjaMagazynowa == toolPosition)
               {
                   item.Regeneracja = false;
                   item.CyklRegeneracji ++;
                   item.Trwalosc=(int) (item.Trwalosc * 0.9);
                   item.Uzycie = 0;
               } 
            }
            
            context.SaveChanges();
            
            message = "Stan narzędzia został zaktualizowany";
            var newmessage2 = new Messages().UniversalMessage(message, MyReferences.MainView,"",false);
        }
    }
    
}