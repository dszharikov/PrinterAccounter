using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.DeviceRepositories;

internal interface IDeviceRepository
{
    Task<IEnumerable<Device>> GetAllDevices(string? connectionType = null);
    Task<bool> ExistsAsync(int deviceId);
}