using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class Magazyn
{
    public int PozycjaMagazynowa { get; set; }

    public int IdNarzedzia { get; set; }

    public int Trwalosc { get; set; }

    public int Uzycie { get; set; }

    public int CyklRegeneracji { get; set; }

    public bool Wycofany { get; set; }
    
    public bool Regeneracja { get; set; }

    public virtual Narzedzie IdNarzedziaNavigation { get; set; } = null!;
}
