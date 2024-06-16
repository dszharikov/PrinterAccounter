using PrinterAccounter.DTOs;
using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.InstallationRepositories;

public interface IInstallationRepository
{
    Task<Installation> AddInstallationAsync(InstallationDto installation);
    Task<IEnumerable<Installation>> GetInstallationsAsync(InstallationQueryParameters parameters);
    Task<Installation> GetInstallationByIdAsync(int id);
    Task<bool> DeleteInstallationAsync(int id);
    Task<bool> ExistsAsync(int branchId, int serialNumber);
    Task<bool> ExistsAsync(int id);
}