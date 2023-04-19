using System;
using System.Collections.Generic;

namespace ToolsMenagement.Models;

public partial class NarzedziaTechnologium
{
    public int IdNarzedzia { get; set; }

    public int IdTechnologi { get; set; }

    public int CzasPracy { get; set; }

    public virtual Narzedzie IdNarzedziaNavigation { get; set; } = null!;

    public virtual Technologium IdTechnologiNavigation { get; set; } = null!;
}
