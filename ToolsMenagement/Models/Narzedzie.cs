using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class Narzedzie
{
    public int IdNarzedzia { get; set; }

    public int IdKategorii { get; set; }

    public string Nazwa { get; set; } = null!;

    public double Srednica { get; set; }

    public virtual Kategorium IdKategoriiNavigation { get; set; } = null!;

    public virtual ICollection<Magazyn> Magazyns { get; set; } = new List<Magazyn>();

    public virtual ICollection<NarzedziaTechnologium> NarzedziaTechnologia { get; set; } = new List<NarzedziaTechnologium>();
}
