using PrinterAccounter.DTOs.Input;
using PrinterAccounter.Models;

namespace PrinterAccounter.Services.PrintTaskServices;

public interface IPrintTaskService
{
    Task<PrintTask> AddPrintTaskAsync(PrintTaskDto printTask);
    Task<PrintTask> GetPrintTaskByIdAsync(int printTaskId);
    Task<int> AddMultipleTasksAsync(IEnumerable<PrintTaskDto> printTasks);
}