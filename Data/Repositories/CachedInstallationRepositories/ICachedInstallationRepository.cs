using PrinterAccounter.DTOs.Input;
using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.CachedInstallationRepositories;

public interface ICachedInstallationRepository
{
    Task<Installation> AddInstallationAsync(InstallationDto installation);
    Task<IEnumerable<Installation>> GetInstallationsAsync(InstallationQueryParameters parameters);
    Task<Installation> GetInstallationByIdAsync(int id);
    Task<Installation> GetDefaultInstallationByBranchAsync(int branchId);
    Task<Installation> GetInstallationByBranchAndSerialNumberAsync(int branchId, int serialNumber);
    Task<bool> DeleteInstallationAsync(int id);
    Task<bool> ExistsAsync(int branchId, int serialNumber);
    Task<bool> ExistsAsync(int id);
}