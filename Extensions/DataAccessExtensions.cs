using PrinterAccounter.Services;

namespace PrinterAccounter.Extensions;

internal static class DataAccessExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(serviceProvider =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException("Connection string is missing");
            return new SqlConnectionFactory(connectionString);
        });

        // добавление кэширования
        services.AddStackExchangeRedisCache(options =>
        {
            string redisConnection = configuration.GetConnectionString("RedisConnection") ??
                                     throw new InvalidOperationException("Redis connection string is missing");
            options.Configuration = redisConnection;
        });

        return services;
    }
}