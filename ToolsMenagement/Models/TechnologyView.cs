using System.Collections.ObjectModel;

namespace ToolsMenagement.Models;

public class TechnologyView
{
    public string? Opis { get; set; }
    public string IdNarzedzia { get; set; }
    public string Uzycie { get; set; }
    
    public string IDTechnologii { get; set; }
    public ObservableCollection<TechnologyView> Children { get; } = new();
}