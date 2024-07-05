using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.PrintTaskRepositories;

public interface IPrintTaskRepository
{
    Task<PrintTask> AddPrintTaskAsync(PrintTask printTask);    
    Task<PrintTask> GetPrintTaskByIdAsync(int printTaskId);
    Task<int> AddMultiplePrintTasksAsync(IEnumerable<PrintTask> printTasks);
}