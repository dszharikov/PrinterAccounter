using PrinterAccounter.Data.Repositories.PrintTaskRepositories;
using PrinterAccounter.DTOs;
using PrinterAccounter.Exceptions;
using PrinterAccounter.Models;
using PrinterAccounter.Services.EmployeeServices;
using PrinterAccounter.Services.InstallationServices;
using PrinterAccounter.Services.PrintJobServices;

namespace PrinterAccounter.Services.PrintTaskServices;

public class PrintTaskService : IPrintTaskService
{
    private readonly IPrintTaskRepository _printTaskRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IInstallationService _installationService;
    private readonly IPrintJobService _printJobService;

    public PrintTaskService(IPrintTaskRepository printTaskRepository, 
                            IEmployeeService employeeService, 
                            IInstallationService installationService,
                            IPrintJobService printJobService)
    {
        _printTaskRepository = printTaskRepository;

        _employeeService = employeeService;
        _installationService = installationService;

        _printJobService = printJobService;
    }
    public async Task<PrintTask> AddPrintTaskAsync(PrintTaskDto printTaskDto)
    {
        if (printTaskDto.PagesCount < 1)
        {
            throw new ValidationException("Pages count must be greater than 0");
        }

        var employee = await _employeeService.GetEmployeeByIdAsync(printTaskDto.EmployeeId);

        var printTask = new PrintTask
        {
            EmployeeId = employee.Id,
            PagesCount = printTaskDto.PagesCount,
            Name = printTaskDto.Name,
        };

        if (printTaskDto.SerialNumber is not null)
        {
            var installation = await _installationService.GetInstallationByBranchAndSerialNumberAsync(
                branchId: employee.BranchId, 
                serialNumber: printTaskDto.SerialNumber.Value);
            
            printTask.InstallationId = installation.Id;
        }
        else
        {
            var installation = await _installationService.GetDefaultInstallationByBranchAsync(branchId: employee.BranchId);

            printTask.InstallationId = installation.Id;
        }

        var printSuccess = await _printJobService.PrintJobAsync(printTask);

        printTask.IsSuccess = printSuccess;

        return await _printTaskRepository.AddPrintTaskAsync(printTask);
    }
}