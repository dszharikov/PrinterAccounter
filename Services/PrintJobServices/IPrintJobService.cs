using PrinterAccounter.Models;

namespace PrinterAccounter.Services.PrintJobServices;

public interface IPrintJobService
{
    Task<bool> PrintJobAsync(PrintTask printTask);
}