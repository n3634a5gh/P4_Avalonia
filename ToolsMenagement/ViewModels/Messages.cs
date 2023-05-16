using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Avalonia.Interactivity;
using Gat.Controls;
using ReactiveUI;

namespace ToolsMenagement.ViewModels;

public class Messages
{
    public async Task UniversalMessage(string message,Window location,string header, bool closing)
    {
        var messageBox = MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ContentHeader = header,
                ContentMessage = message,
                MinWidth = 400,
                CanResize = true,
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "OK", IsDefault  = true }
                    //new ButtonDefinition { Name = "Cancel", IsCancel = true }
                },
            });
        
        var result = await messageBox.ShowDialog(location);

        if ((result == "OK") & closing)
        {
            location.Close();
        }
    }
}