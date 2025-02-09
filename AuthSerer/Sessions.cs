using System;
using System.Collections.Generic;

namespace TodoList;

public partial class Session
{
    public int Number { get; set; }

    public int UserId { get; set; }

    public DateTime? Date { get; set; }

    public virtual User User { get; set; } = null!;
}