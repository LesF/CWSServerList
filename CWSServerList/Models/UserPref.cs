using System;
using System.Collections.Generic;

namespace CWSServerList.Models;

public partial class UserPref
{
    public string Username { get; set; } = null!;

    public string? EnvironmentCode { get; set; }

    public string? TypeCode { get; set; }

    public bool? IncludeInactive { get; set; }

    public string? QuickLinks { get; set; }

    public bool? CanEdit { get; set; }

    public int? Permissions { get; set; }

    public string? VersionId { get; set; }
}
