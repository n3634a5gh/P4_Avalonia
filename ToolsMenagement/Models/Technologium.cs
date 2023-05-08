using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class Technologium
{
    public int IdTechnologi { get; set; }

    public string Opis { get; set; } = null!;
    
    public DateTime DataUtworzenia { get; set; }

    public virtual ICollection<NarzedziaTechnologium> NarzedziaTechnologia { get; set; } = new List<NarzedziaTechnologium>();

    public virtual ICollection<Zlecenie> Zlecenies { get; set; } = new List<Zlecenie>();
}
