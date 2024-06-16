using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.Models;
using PrinterAccounter.Services.DeviceServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDevices([FromQuery] string? connectionType)
    {
        var devices = await _deviceService.GetAllDevicesAsync(connectionType);

        return Ok(devices);
    }
}