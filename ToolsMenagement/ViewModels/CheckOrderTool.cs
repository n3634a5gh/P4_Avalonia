using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToolsMenagement.Models;
using ToolsMenagement.Views;

namespace ToolsMenagement.ViewModels;

public class CheckOrderTool
{
    //public ObservableCollection<ItemCollection> Cater { get; set; }
    public static bool CheckOrder(int technologyId)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();
        
        //sprawdź czy istnieje podana technologia1

        var tech_exist = context.Technologia
            .Count(technologium => technologium.IdTechnologi == technologyId);
        if (tech_exist == 0)
        {
            string message=$"Nie odnaleziono technologii.";
            var newmessage = new Messages().UniversalMessage(message, MyReferences.orderview,"",false);
            return false;
        }
        
        //zapis do available_tools wszystkich narzędzi w magazynie, które mogą zostać użyte w technologii
        //nie w regeneracji i nie wycofane
        
        var available_tools = context.NarzedziaTechnologia
            .Where(technologium => technologium.IdTechnologi == technologyId)
            .Join(
                context.Narzedzies,
                technologium => technologium.IdNarzedzia,
                tool => tool.IdNarzedzia,
                (technologium, tool) => tool
            )
            .Join(
                context.Magazyns,
                narzedzie => narzedzie.IdNarzedzia,
                magazyn =>magazyn.IdNarzedzia,
                ((narzedzie, magazyn) => magazyn )
                )
            .Where(magazyn => magazyn.CyklRegeneracji<=5)
            .Where(magazyn => magazyn.Wycofany==false)
            .Where(magazyn => magazyn.Regeneracja==false)
            .Where(magazyn => magazyn.Uzycie<magazyn.Trwalosc)
            .ToArray();

        var technology_tools = context.NarzedziaTechnologia
            .Where(technologium => technologium.IdTechnologi == technologyId)
            .ToArray();

        int number_of_available = 0;
        
        
        //sprawdź czy każde narzędzie wymienione w technologi można "pobrać " z magazynu
        
        foreach (var item2 in technology_tools)
        {
            if (available_tools.Any(item => item2.IdNarzedzia == item.IdNarzedzia))
            {
                number_of_available++;
            }
        }

        if (number_of_available >= technology_tools.Length)
        {
            return true;
        }
        else
        {
            string message=$"Brak narzędzi koniecznych do utworzenia zlecenia";
            var newmessage = new Messages().UniversalMessage(message, MyReferences.orderview,"",true);
            return false;
        }
        //jezeli jakies narzędziwe nie jest dostępne wyświetl komunikat, że nie można
        //utworzyć zlecenia, jeśli można utworzyć zlecenie, wyświetl liste narzedzi w 
        //oknie i wyświetl przycisk utwórz zlecenie, który je doda do bazy

        //dodać też numer technologi do obsługi funkcji(z textboxa)
    }

    public void CreateOrder(int technologyId)
    {
        var context = new ToolsDatabase1Context();
        context.Database.EnsureCreated();
        context.Database.Migrate();
        
        //Sprawdź czy otwarte jest zlecenie
        //inne, o takim samym idtechnologi, jesli nie, dodaj zlecenie
        //i wyświetl jego id

        bool tech_is_open = false;

        tech_is_open = context.Zlecenies
            .Where(zlecenie => zlecenie.Aktywne==true)
            .Any(zlecenie => zlecenie.IdTechnologi == technologyId);

        if (tech_is_open)
        {
            string message=$"Nie można utworzyć nowego zlecenia \n" +
                           $"ponieważ poprzednie nie zostało zakończone.";
            var newmessage = new Messages().UniversalMessage(message, MyReferences.orderview,"",true);
        }
        else
        {
            var neworder = new Zlecenie()
            {
                IdTechnologi = technologyId,
                Aktywne = true
            };
            context.Add(neworder);
            context.SaveChanges();

            var orders = context.Zlecenies
                .Where(zlecenie => zlecenie.Aktywne == true)
                .Where(zlecenie => zlecenie.IdTechnologi == technologyId)
                .OrderByDescending(zlecenie => zlecenie.IdTechnologi)
                .FirstOrDefault();
            
            string message=$"Dodano nowe zlecenie nr. {orders.IdZlecenia}";
            var newmessage = new Messages().UniversalMessage(message, MyReferences.orderview,"",true);
        
        }
    }
}
