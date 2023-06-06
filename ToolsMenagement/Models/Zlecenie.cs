using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class Zlecenie
{
    public int IdZlecenia { get; set; }

    public int IdTechnologi { get; set; }

    public bool Aktywne { get; set; }

    public virtual Technologium IdTechnologiNavigation { get; set; } = null!;

    public virtual ICollection<Rejestracja> Rejestracjas { get; set; } = new List<Rejestracja>();
}
