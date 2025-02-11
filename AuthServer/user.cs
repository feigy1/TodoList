using System;
using System.Collections.Generic;

namespace TodoList;

public partial class Users
{
    public int id { get; set; } 

    public string username { get; set; }  = null!;

    public string password { get; set; } = null!;
}
