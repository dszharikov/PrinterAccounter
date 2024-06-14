using PrinterAccounter.Data.Repositories.DeviceRepositories;
using PrinterAccounter.Models;

namespace PrinterAccounter.Services.DeviceServices;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;

    public DeviceService(IDeviceRepository deviceRepository)
    {
        _deviceRepository = deviceRepository;   
    }
    public async Task<IEnumerable<Device>> GetAllDevicesAsync(string? connectionType = null)
    {
        return await _deviceRepository.GetAllDevices(connectionType);
    }
}