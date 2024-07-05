namespace PrinterAccounter.DTOs.Input;

internal class InstallationDto
{
    public string Name { get; set; }
    public int BranchId { get; set; }
    public int DeviceId { get; set; }
    public int? SerialNumber { get; set; }
    public bool IsDefault { get; set; }
}