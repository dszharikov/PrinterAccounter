using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Caching.Distributed;
using PrinterAccounter.DTOs;
using PrinterAccounter.Exceptions;
using PrinterAccounter.Models;
using PrinterAccounter.Services;

namespace PrinterAccounter.Data.Repositories.InstallationRepositories;

public class DapperInstallationRepository : IInstallationRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;
    private readonly IDistributedCache _cache;
    private const string _installationCacheKey = "Installation";
    private const double _cacheExpirationMinutes = 105;

    public DapperInstallationRepository(SqlConnectionFactory sqlConnectionFactory, IDistributedCache cache)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _cache = cache;
    }

    public async Task<Installation> AddInstallationAsync(InstallationDto installation)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var newInstallation = await connection.QuerySingleAsync<Installation>(
            "AddInstallation",
            installation,
            commandType: CommandType.StoredProcedure);

        if (newInstallation is not null)
        {
            var cachedInstallations = await GetInstallationsFromCache();
            if (cachedInstallations is not null)
            {
                cachedInstallations.Add(newInstallation);
                await SaveInstallationsToCache(cachedInstallations);
            }

            return newInstallation;
        }

        throw new DatabaseException("Error adding installation");
    }


    public async Task<bool> ExistsAsync(int branchId, int serialNumber)
    {
        var cachedInstallations = await GetInstallationsFromCache();
        if (cachedInstallations is not null)
        {
            var cachedExists = cachedInstallations.Any(i => i.BranchId == branchId && i.SerialNumber == serialNumber);
            return cachedExists;
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        string sql = @"
            SELECT
                COUNT(1)
            FROM
                Installations
            WHERE
                BranchId = @BranchId
                AND SerialNumber = @SerialNumber";

        var dbExists = await connection.ExecuteScalarAsync<bool>(sql,
            new { BranchId = branchId, SerialNumber = serialNumber });
        return dbExists;
    }

    public async Task<Installation> GetInstallationByIdAsync(int id)
    {
        var cachedInstallations = await GetInstallationsFromCache();
        if (cachedInstallations is not null)
        {
            var cachedInstallation = cachedInstallations.FirstOrDefault(i => i.Id == id);
            if (cachedInstallation is not null)
            {
                return cachedInstallation;
            }
            else
            {
                throw new NotFoundException("Installation with specified id was not found");
            }
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = @"
                SELECT * 
                FROM 
                    Installations 
                WHERE
                    Id = @Id";

        var installation = await connection.QuerySingleAsync<Installation>(sql, new { Id = id });
        return installation;
    }

    public async Task<IEnumerable<Installation>> GetInstallationsAsync(InstallationQueryParameters parameters)
    {
        var cachedInstallations = await GetInstallationsFromCache();

        var sql = new StringBuilder("SELECT * FROM Installations WHERE 1=1");

        if (parameters.BranchId.HasValue)
        {
            if (cachedInstallations is not null)
            {
                var cachedInstallationsByBranch = cachedInstallations.Where(i => i.BranchId == parameters.BranchId);
                return cachedInstallationsByBranch;
            }

            sql.Append(" AND BranchId = @BranchId");
        }

        if (cachedInstallations is not null)
        {
            return cachedInstallations;
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        var installations = await connection.QueryAsync<Installation>(sql.ToString(), parameters);

        await SaveInstallationsToCache(installations.ToList());

        return installations;
    }
    public async Task<bool> DeleteInstallationAsync(int id)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var response = await connection.ExecuteAsync(
            "DeleteInstallation",
            new { Id = id },
            commandType: CommandType.StoredProcedure);


        var cachedInstallations = await GetInstallationsFromCache();
        if (cachedInstallations is not null)
        {
            var installationToRemove = cachedInstallations.FirstOrDefault(i => i.Id == id);
            if (installationToRemove is not null)
            {
                cachedInstallations.Remove(installationToRemove);
                await SaveInstallationsToCache(cachedInstallations);
            }
        }

        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        var cachedInstallations = await GetInstallationsFromCache();
        if (cachedInstallations is not null)
        {
            return cachedInstallations.Any(i => i.Id == id);
        }

        using var connection = _sqlConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(1) FROM Installations WHERE Id = @Id",
            new { Id = id });   
    }

    private async Task<List<Installation>?> GetInstallationsFromCache()
    {
        var cachedInstallations = await _cache.GetStringAsync(_installationCacheKey);

        if (cachedInstallations is not null)
        {
            return JsonSerializer.Deserialize<List<Installation>>(cachedInstallations);
        }

        return null;
    }

    private async Task SaveInstallationsToCache(List<Installation> installations)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheExpirationMinutes)
        };

        var serializedInstallations = JsonSerializer.Serialize(installations);
        await _cache.SetStringAsync(_installationCacheKey, serializedInstallations, options);
    }
}