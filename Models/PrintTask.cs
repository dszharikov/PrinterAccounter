namespace PrinterAccounter.Models;

public class PrintTask
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int EmployeeId { get; set; }
    public int InstallationId { get; set; }
    public int PagesCount { get; set; }
    public bool IsSuccess { get; set; }
}