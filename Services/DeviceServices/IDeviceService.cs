using PrinterAccounter.Models;

namespace PrinterAccounter.Services.DeviceServices;

public interface IDeviceService
{
    Task<IEnumerable<Device>> GetAllDevicesAsync(string? connectionType = null);
    Task<bool> ExistsAsync(int deviceId);
}