namespace PrinterAccounter.Models;

public class Installation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BranchId { get; set; }
    public int SerialNumber { get; set; }
    public bool IsDefault { get; set; }
    public int DeviceId { get; set; }
}