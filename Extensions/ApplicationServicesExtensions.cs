using PrinterAccounter.Services.BranchServices;
using PrinterAccounter.Services.DeviceServices;
using PrinterAccounter.Services.EmployeeServices;

namespace PrinterAccounter.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IBranchService, BranchService>();

        return services;
    }
}