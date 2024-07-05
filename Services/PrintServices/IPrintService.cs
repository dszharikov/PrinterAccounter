using PrinterAccounter.Models;

namespace PrinterAccounter.Services.PrintServices;

internal interface IPrintService
{
    Task<bool> PrintJobAsync(PrintTask printTask);
}