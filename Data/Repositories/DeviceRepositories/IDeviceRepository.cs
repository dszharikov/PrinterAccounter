using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.DeviceRepositories;

public interface IDeviceRepository
{
    Task<IEnumerable<Device>> GetAllDevices(string? connectionType = null);
}