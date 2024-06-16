using PrinterAccounter.Services.BranchServices;
using PrinterAccounter.Services.DeviceServices;
using PrinterAccounter.Services.EmployeeServices;
using PrinterAccounter.Services.InstallationServices;
using PrinterAccounter.Services.PrintJobServices;
using PrinterAccounter.Services.PrintTaskServices;

namespace PrinterAccounter.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IInstallationService, InstallationService>();
        services.AddScoped<IPrintTaskService, PrintTaskService>();
        services.AddScoped<IPrintJobService, PrintJobSimulatorService>();

        return services;
    }
}