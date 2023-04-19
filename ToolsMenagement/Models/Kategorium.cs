using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class Kategorium
{
    public int IdKategorii { get; set; }

    public string Opis { get; set; } = null!;

    public virtual ICollection<Narzedzie> Narzedzies { get; set; } = new List<Narzedzie>();
}
