using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class Zlecenie
{
    public int IdZlecenia { get; set; }

    public int IdTechnologi { get; set; }

    public int Sztuk { get; set; }

    public DateOnly DataWykonania { get; set; }

    public string Wykonal { get; set; } = null!;

    public virtual Technologium IdTechnologiNavigation { get; set; } = null!;
}
