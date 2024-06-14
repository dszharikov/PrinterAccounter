using PrinterAccounter.Data.Repositories.BranchRepositories;
using PrinterAccounter.Data.Repositories.DeviceRepositories;
using PrinterAccounter.Data.Repositories.EmployeeRepositories;

namespace PrinterAccounter.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, DapperEmployeeRepository>();
        services.AddScoped<IDeviceRepository, DapperDeviceRepository>();
        services.AddScoped<IBranchRepository, DapperBranchRepository>();

        return services;
    }
}