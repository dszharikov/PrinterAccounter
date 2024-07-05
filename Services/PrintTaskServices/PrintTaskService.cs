using PrinterAccounter.Data.Repositories.PrintTaskRepositories;
using PrinterAccounter.DTOs.Input;
using PrinterAccounter.Exceptions;
using PrinterAccounter.Models;
using PrinterAccounter.Services.EmployeeServices;
using PrinterAccounter.Services.InstallationServices;
using PrinterAccounter.Services.PrintServices;

namespace PrinterAccounter.Services.PrintTaskServices;

public class PrintTaskService : IPrintTaskService
{
    private readonly IPrintTaskRepository _printTaskRepository;
    private readonly IEmployeeService _employeeService;
    private readonly IInstallationService _installationService;
    private readonly IPrintService _printJobService;

    public PrintTaskService(IPrintTaskRepository printTaskRepository,
                            IEmployeeService employeeService,
                            IInstallationService installationService,
                            IPrintService printJobService)
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

    public async Task<int> AddMultipleTasksAsync(IEnumerable<PrintTaskDto> printTaskDtos)
    {
        var printTaskDtoList = printTaskDtos.ToList();
        var printTasks = new List<PrintTask>();

        // Кэшированные результаты запросов
        var employeeCache = new Dictionary<int, Employee>();
        var installationCache = new Dictionary<(int BranchId, int SerialNumber), Installation>();
        var defaultInstallationCache = new Dictionary<int, Installation>();


        foreach (var printTaskDto in printTaskDtoList)
        {
            if (printTaskDto.PagesCount < 1)
            {
                continue;
            }

            // Получение сотрудника с кэшированием
            if (!employeeCache.TryGetValue(printTaskDto.EmployeeId, out var employee))
            {
                if (employee == null)
                {
                    try
                    {
                        employee = await _employeeService.GetEmployeeByIdAsync(printTaskDto.EmployeeId);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                employeeCache[printTaskDto.EmployeeId] = employee;
            }

            Installation installation;
            if (printTaskDto.SerialNumber is not null)
            {
                var key = (employee.BranchId, printTaskDto.SerialNumber.Value);

                // Получение инсталляции с кэшированием
                if (!installationCache.TryGetValue(key, out installation))
                {
                    if (installation == null)
                    {
                        try
                        {
                            installation = await _installationService.GetInstallationByBranchAndSerialNumberAsync(employee.BranchId, printTaskDto.SerialNumber.Value);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    if (installation.IsDefault)
                    {
                        defaultInstallationCache[employee.BranchId] = installation;
                    }
                    installationCache[key] = installation;
                }
            }
            else
            {
                // Получение стандартной инсталляции с кэшированием
                if (!defaultInstallationCache.TryGetValue(employee.BranchId, out installation))
                {
                    if (installation == null)
                    {
                        try
                        {
                            installation = await _installationService.GetDefaultInstallationByBranchAsync(employee.BranchId);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    installationCache[(installation.BranchId, installation.SerialNumber)] = installation;
                    defaultInstallationCache[employee.BranchId] = installation;
                }
            }

            var printTask = new PrintTask
            {
                EmployeeId = employee.Id,
                InstallationId = installation.Id,
                PagesCount = printTaskDto.PagesCount,
                Name = printTaskDto.Name,
            };

            var printSuccess = await _printJobService.PrintJobAsync(printTask);
            printTask.IsSuccess = printSuccess;

            printTasks.Add(printTask);
        }

        return await _printTaskRepository.AddMultiplePrintTasksAsync(printTasks);
    }

    public async Task<PrintTask> GetPrintTaskByIdAsync(int printTaskId)
    {
        var printTask = await _printTaskRepository.GetPrintTaskByIdAsync(printTaskId);

        return printTask;
    }
}