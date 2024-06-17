namespace PrinterAccounter.DTOs.Input;

public class PrintTaskDto
{
    public string Name { get; set; }
    public int EmployeeId { get; set; }
    public int? SerialNumber { get; set; }
    public int PagesCount { get; set; }
}