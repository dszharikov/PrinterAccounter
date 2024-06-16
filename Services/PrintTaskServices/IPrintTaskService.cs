using PrinterAccounter.DTOs;
using PrinterAccounter.Models;

namespace PrinterAccounter.Services.PrintTaskServices;

public interface IPrintTaskService
{
    Task<PrintTask> AddPrintTaskAsync(PrintTaskDto printTask);
}