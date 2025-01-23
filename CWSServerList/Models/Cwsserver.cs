using System;
using System.Collections.Generic;

namespace CWSServerList.Models;

public partial class Cwsserver
{
    public int ServerId { get; set; }

    public string ServerName { get; set; } = null!;

    public string EnvironmentCode { get; set; } = null!;

    public string TypeCode { get; set; } = null!;

    public bool IsActive { get; set; }

    public string? Comment { get; set; }

    public string? Cwsurl { get; set; }

    public string? ServiceAccount { get; set; }

    public string? Ipaddress { get; set; }

    public string? Gateway { get; set; }

    public int VersionId { get; set; }

    public string? Prefix { get; set; }

    public int Vmgroup { get; set; }
}
