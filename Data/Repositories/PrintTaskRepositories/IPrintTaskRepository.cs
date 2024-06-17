using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.PrintTaskRepositories;

public interface IPrintTaskRepository
{
    Task<PrintTask> AddPrintTaskAsync(PrintTask printTask);    
    Task<int> AddMultiplePrintTasksAsync(IEnumerable<PrintTask> printTasks);
}