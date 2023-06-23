using System.Collections.ObjectModel;

namespace ToolsMenagement.Models;

public class TechnologyView
{
    public string? Opis { get; set; }
    public int IdNarzedzia { get; set; }
    public int Uzycie { get; set; }
    public ObservableCollection<TechnologyView> Children { get; } = new();
}