namespace ToolsMenagement.Models;

public class ToolsDBView
{
    public string Nazwa { get; set; } = null!;

    public double Srednica { get; set; }
    
    public int Trwalosc { get; set; }

    public int Uzycie { get; set; }

    public int CyklRegeneracji { get; set; }

    public bool Wycofany { get; set; }
    
    public bool Regeneracja { get; set; }
}