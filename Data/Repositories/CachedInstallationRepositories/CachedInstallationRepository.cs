using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Caching.Distributed;
using PrinterAccounter.Data.Repositories.InstallationRepositories;
using PrinterAccounter.DTOs.Input;
using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.CachedInstallationRepositories;

internal class CachedInstallationRepository : ICachedInstallationRepository
{
    private readonly IInstallationRepository _installationRepository;
    private readonly string _keyPrefix = $"{typeof(Installation).Name}:";
    private readonly IDistributedCache _cache;
    public CachedInstallationRepository(IInstallationRepository installationRepository,
                                        IDistributedCache cache)
    {
        _installationRepository = installationRepository;
        _cache = cache;
    }

    public async Task<Installation> AddInstallationAsync(InstallationDto installation)
    {
        var result = await _installationRepository.AddInstallationAsync(installation);

        var key = $"{_keyPrefix}:{result.Id}";

        await _cache.RemoveAsync(key);

        var allKey = $"{_keyPrefix}:All";
        await _cache.RemoveAsync(allKey);

        return result;
    }

    public async Task<bool> DeleteInstallationAsync(int id)
    {
        var result = await _installationRepository.DeleteInstallationAsync(id);

        var key = $"{_keyPrefix}:{id}";
        await _cache.RemoveAsync(key);

        var allKey = $"{_keyPrefix}:All";
        await _cache.RemoveAsync(allKey);

        return result;
    }

    public async Task<bool> ExistsAsync(int branchId, int serialNumber)
    {
        return await _installationRepository.ExistsAsync(branchId, serialNumber);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _installationRepository.ExistsAsync(id);
    }

    public async Task<Installation> GetDefaultInstallationByBranchAsync(int branchId)
    {
        var key = $"{_keyPrefix}:Default:{branchId}";

        var cachedValue = JsonSerializer.Deserialize<Installation>(await _cache.GetAsync(key));
        
        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await _installationRepository.GetDefaultInstallationByBranchAsync(branchId);

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(result));

        return result;
    }

    public async Task<Installation> GetInstallationByBranchAndSerialNumberAsync(int branchId, int serialNumber)
    {
        return await _installationRepository.GetInstallationByBranchAndSerialNumberAsync(branchId, serialNumber);
    }

    public async Task<Installation> GetInstallationByIdAsync(int id)
    {
        var key = $"{_keyPrefix}:{id}";

        var cachedValue = JsonSerializer.Deserialize<Installation>(await _cache.GetAsync(key));

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        var result = await _installationRepository.GetInstallationByIdAsync(id);

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(result));

        return result;
    }

    public async Task<IEnumerable<Installation>> GetInstallationsAsync(InstallationQueryParameters parameters)
    {
        return await _installationRepository.GetInstallationsAsync(parameters);
    }
}