using System.Collections.Generic;

namespace ToolsMenagement.Models;

public class ToolExv
{
    public Kategorium CKategorium{ get; set; }
    public List<ToolTx> Children { get; set; }
}

public class ToolTx
{
    public Narzedzie CNarzedzie { get; set; }
    public List<Magazyn> Children { get; set; }
}