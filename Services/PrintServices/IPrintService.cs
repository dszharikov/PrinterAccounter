using PrinterAccounter.Models;

namespace PrinterAccounter.Services.PrintServices;

public interface IPrintService
{
    Task<bool> PrintJobAsync(PrintTask printTask);
}