namespace PrinterAccounter.Models;

internal class Device
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ConnectionType { get; set; }
    public string? MacAddress { get; set; }
}