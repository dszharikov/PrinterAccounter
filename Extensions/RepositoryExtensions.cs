using PrinterAccounter.Data.Repositories.BranchRepositories;
using PrinterAccounter.Data.Repositories.DeviceRepositories;
using PrinterAccounter.Data.Repositories.EmployeeRepositories;
using PrinterAccounter.Data.Repositories.InstallationRepositories;
using PrinterAccounter.Data.Repositories.PrintTaskRepositories;

namespace PrinterAccounter.Extensions;

public static class RepositoryExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeRepository, DapperEmployeeRepository>();
        services.AddScoped<IDeviceRepository, DapperDeviceRepository>();
        services.AddScoped<IBranchRepository, DapperBranchRepository>();
        services.AddScoped<IInstallationRepository, DapperInstallationRepository>();
        services.AddScoped<IPrintTaskRepository, DapperPrintTaskRepository>();

        return services;
    }
}