using System;
using System.Collections.Generic;

namespace CWSServerList.Models;

public partial class CwsserverPurpose
{
    public string TypeCode { get; set; } = null!;

    public string ServerTypeName { get; set; } = null!;

    public string? SortKey { get; set; }
}
