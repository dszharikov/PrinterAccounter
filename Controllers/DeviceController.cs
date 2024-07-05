using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs.Output;
using PrinterAccounter.Models;
using PrinterAccounter.Services.DeviceServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesErrorResponseType(typeof(ErrorResponseDto))]
public class DeviceController : ControllerBase
{
    private readonly IDeviceService _deviceService;

    public DeviceController(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    /// <summary>
    /// Searches for devices by connection type.
    /// </summary>
    /// <remarks>
    /// You can search for devices using the optional connection type parameter.
    /// 
    /// Sample request:
    /// 
    ///     GET /api/Devices?connectionType=Network
    /// 
    /// If no parameter is provided, all devices are returned.
    /// </remarks>
    /// <param name="connectionType">The type of connection (e.g., Network).</param>
    /// <returns>A list of devices.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Device>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDevices([FromQuery] string? connectionType)
    {
        var devices = await _deviceService.GetAllDevicesAsync(connectionType);
        if (devices == null || !devices.Any())
        {
            return NotFound(new ErrorResponseDto
            {
                Message = "No devices found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }
        return Ok(devices);
    }
}
