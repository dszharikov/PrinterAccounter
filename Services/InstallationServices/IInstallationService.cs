using PrinterAccounter.DTOs;
using PrinterAccounter.Models;

namespace PrinterAccounter.Services.InstallationServices;

public interface IInstallationService
{
    Task<Installation> AddInstallationAsync(InstallationDto installation);
    Task<IEnumerable<Installation>> GetInstallationsAsync(InstallationQueryParameters parameters);
    Task<Installation> GetInstallationByIdAsync(int id);
    Task<bool> DeleteInstallationAsync(int id);
    Task<bool> ExistsAsync(int branchId, int serialNumber);
}