using PrinterAccounter.Services.BranchServices;
using PrinterAccounter.Services.DeviceServices;
using PrinterAccounter.Services.EmployeeServices;
using PrinterAccounter.Services.InstallationServices;
using PrinterAccounter.Services.PrintServices;
using PrinterAccounter.Services.PrintTaskServices;
using PrinterAccounter.Utils;

namespace PrinterAccounter.Extensions;

internal static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IInstallationService, InstallationService>();
        services.AddScoped<IPrintTaskService, PrintTaskService>();
        services.AddScoped<IPrintService, PrintSimulatorService>();

        services.AddScoped<PrinterCsvParser>();

        return services;
    }
}