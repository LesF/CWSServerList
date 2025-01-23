using System;
using System.Collections.Generic;

namespace CWSServerList.Models;

public partial class Cwsenvironment
{
    public string EnvironmentCode { get; set; } = null!;

    public string Environment { get; set; } = null!;

    public string? Nlburl { get; set; }

    public string? Version { get; set; }
}
