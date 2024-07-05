using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.PrintTaskRepositories;

internal interface IPrintTaskRepository
{
    Task<PrintTask> AddPrintTaskAsync(PrintTask printTask);    
    Task<PrintTask> GetPrintTaskByIdAsync(int printTaskId);
    Task<int> AddMultiplePrintTasksAsync(IEnumerable<PrintTask> printTasks);
}