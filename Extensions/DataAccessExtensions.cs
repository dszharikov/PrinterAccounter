using PrinterAccounter.Services;

namespace PrinterAccounter.Extensions;

public static class DataAccessExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(serviceProvider =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException("Connection string is missing");
            return new SqlConnectionFactory(connectionString);
        });

        return services;
    }
}