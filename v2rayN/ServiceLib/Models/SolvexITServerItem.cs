using System.Collections.Generic;

namespace ServiceLib.Models;

public class SolvexITServerItem
{
    public string Id { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public int Port { get; set; }
    public string VrtexID { get; set; } = string.Empty;
    public long Sort { get; set; }
}

public class SolvexITConfig
{
    public List<SolvexITServerItem> Servers { get; set; } = new();
    public string DraftRemarks { get; set; } = string.Empty;
    public string DraftDomainPort { get; set; } = string.Empty;
    public string DraftVrtexID { get; set; } = string.Empty;
}
