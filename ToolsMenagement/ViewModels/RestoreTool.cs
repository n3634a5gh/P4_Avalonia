using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Styling;
using Gat.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
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
                if (!item.Wycofany & item.CyklRegeneracji < 5)
                {
                    toolPosition = item.PozycjaMagazynowa;
                    canToolRestore = true;
                }
                else
                {
                    message = "Narzędzie zostało wycofane z eksploatacji.\n" +
                              "Nie jest możliwe jego przywrócenie.";
                    break;
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
            var messageBox = MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ContentHeader = "Błąd",
                    ContentMessage = message,
                    MinWidth = 400,
                    CanResize = true,
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "OK", IsCancel = true }
                    },
                });
            await messageBox.ShowDialog(MyReferences.MainView);
        }

        if (toolPosition != 0)
        {
            var pozycja_magazyn = context.Magazyns.First(a => a.PozycjaMagazynowa == toolPosition);
            pozycja_magazyn.CyklRegeneracji = pozycja_magazyn.CyklRegeneracji + 1;
            pozycja_magazyn.Trwalosc = (int) (pozycja_magazyn.Trwalosc * 0.9);
            pozycja_magazyn.Uzycie = 0;
            context.SaveChanges();
        }
    }
    
}