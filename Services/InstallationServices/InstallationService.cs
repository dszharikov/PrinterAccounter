using PrinterAccounter.Data.Repositories.InstallationRepositories;
using PrinterAccounter.DTOs.Input;
using PrinterAccounter.Exceptions;
using PrinterAccounter.Models;
using PrinterAccounter.Services.BranchServices;
using PrinterAccounter.Services.DeviceServices;

namespace PrinterAccounter.Services.InstallationServices;

public class InstallationService : IInstallationService
{
    private readonly IInstallationRepository _installationRepository;
    private readonly IBranchService _branchService;
    private readonly IDeviceService _deviceService;

    public InstallationService(
        IInstallationRepository installationRepository,
        IBranchService branchService,
        IDeviceService deviceService)
    {
        _installationRepository = installationRepository;
        _branchService = branchService;
        _deviceService = deviceService;
    }

    public async Task<Installation> AddInstallationAsync(InstallationDto installationDto)
    {
        // Checking the completeness of the data
        if (string.IsNullOrEmpty(installationDto.Name)
            || installationDto.BranchId == 0
            || installationDto.DeviceId == 0)
        {
            throw new ValidationException("Fill in all the required fields");
        }

        // Checking the existence of the branch
        var branchExists = await _branchService.ExistsAsync(installationDto.BranchId);
        if (!branchExists)
        {
            throw new NotFoundException("Branch was not found");
        }

        // Checking the existence of the device
        var deviceExists = await _deviceService.ExistsAsync(installationDto.DeviceId);
        if (!deviceExists)
        {
            throw new NotFoundException("Device was not found");
        }

        if (installationDto.SerialNumber is not null)
        {
            // Checking the existence of the installation
            var exists = await _installationRepository.ExistsAsync(installationDto.BranchId, installationDto.SerialNumber.Value);

            if (exists)
            {
                throw new ValidationException("Installation already exists");
            }
        }

        var installation = await _installationRepository.AddInstallationAsync(installationDto);
        return installation;
    }

    public async Task<bool> ExistsAsync(int branchId, int serialNumber)
    {
        var exists = await _installationRepository.ExistsAsync(branchId, serialNumber);
        return exists;
    }

    public async Task<IEnumerable<Installation>> GetInstallationsAsync(InstallationQueryParameters parameters)
    {
        if (parameters.BranchId is not null)
        {
            var branchExists = await _branchService.ExistsAsync((int)parameters.BranchId!);
            if (!branchExists)
            {
                throw new NotFoundException("Branch was not found");
            }
        }

        var installations = await _installationRepository.GetInstallationsAsync(parameters);
        return installations;
    }

    public async Task<Installation> GetInstallationByIdAsync(int id)
    {
        var installation = await _installationRepository.GetInstallationByIdAsync(id);
        
        return installation;
    }

    public async Task<bool> DeleteInstallationAsync(int id)
    {
        bool exists = await _installationRepository.ExistsAsync(id);

        if (!exists)
        {
            throw new NotFoundException("Installation with specified id was not found");
        }

        var result = await _installationRepository.DeleteInstallationAsync(id);
        return result;
    }

    public async Task<Installation> GetDefaultInstallationByBranchAsync(int branchId)
    {
        var installation = await _installationRepository.GetDefaultInstallationByBranchAsync(branchId);

        return installation;
    }

    public async Task<Installation> GetInstallationByBranchAndSerialNumberAsync(int branchId, int serialNumber)
    {
        var installation = await _installationRepository.GetInstallationByBranchAndSerialNumberAsync(branchId, serialNumber);

        return installation;
    }
}
