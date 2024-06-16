using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs;
using PrinterAccounter.Services.InstallationServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstallationController : ControllerBase
{
    private readonly IInstallationService _installationService;

    public InstallationController(IInstallationService installationService)
    {
        _installationService = installationService;
    }

    [HttpPost]
    public async Task<IActionResult> AddInstallation([FromBody] InstallationDto installationDto)
    {
        var installation = await _installationService.AddInstallationAsync(installationDto);

        return CreatedAtAction(nameof(AddInstallation), new { id = installation.Id }, installation.Id);
    }

    [HttpGet]
    public async Task<IActionResult> GetInstallations([FromQuery] InstallationQueryParameters parameters)
    {
        var installations = await _installationService.GetInstallationsAsync(parameters);

        return Ok(installations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInstallationById(int id)
    {
        var installation = await _installationService.GetInstallationByIdAsync(id);

        return Ok(installation);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInstallation(int id)
    {
        await _installationService.DeleteInstallationAsync(id);

        return NoContent();
    }
}