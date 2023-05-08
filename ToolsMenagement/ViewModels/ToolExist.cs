using System;
using System.Linq;
using System.Threading.Tasks;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;

namespace ToolsMenagement.ViewModels;

public class ToolExist
{
    public async Task<bool> ExecuteToolExist()
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        var message = "";
        
        int categoryID = 0;
        bool tool_found = false;

        foreach (var item in context.Kategoria)
        {
            if (item.Opis == MyReferences.twvm.SelectedCategory)
            {
                if (item.Przeznaczenie == MyReferences.twvm.SelectedPurpose)
                {
                    if (item.MaterialWykonania == MyReferences.twvm.SelectedMaterial)
                    {
                        categoryID = item.IdKategorii;
                    }
                }
            }
        }

        foreach (var item in context.Narzedzies)
        {
            if (categoryID > 0)
            {
                if (item.IdKategorii == categoryID)
                {
                    if (item.Srednica == Convert.ToDouble(MyReferences.twvm.Diameter))
                    {
                        tool_found = true;
                    }
                }
            }
            else
            {
                break;
            }
        }
        
        message = "Nie odnaleziono narzędzia \n" +
                  "o podanej średnicy.";

        if (!tool_found)
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
            await messageBox.ShowDialog(MyReferences.techview);
        }
        
        
        /*if (toolPosition != 0)
        {
            var pozycja_magazyn = context.Magazyns.First(a => a.PozycjaMagazynowa == toolPosition);
            pozycja_magazyn.CyklRegeneracji = pozycja_magazyn.CyklRegeneracji + 1;
            pozycja_magazyn.Trwalosc = (int) (pozycja_magazyn.Trwalosc * 0.9);
            pozycja_magazyn.Uzycie = 0;
            context.SaveChanges();
        }*/
        return tool_found;
    }
}