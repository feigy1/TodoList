using System;
using System.Collections.Generic;

namespace API;

public partial class Session
{
    public int Number { get; set; }

    public int User_id { get; set; }

    public DateTime? Date { get; set; }

    public virtual Users User { get; set; } = null!;
}