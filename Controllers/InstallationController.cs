using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs.Input;
using PrinterAccounter.DTOs.Output;
using PrinterAccounter.Services.InstallationServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesErrorResponseType(typeof(ErrorResponseDto))]
public class InstallationController : ControllerBase
{
    private readonly IInstallationService _installationService;

    public InstallationController(IInstallationService installationService)
    {
        _installationService = installationService;
    }

    /// <summary>
    /// Adds a new installation.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /api/Installation
    ///     {
    ///         "name": "Installation 1",
    ///         "branchId": 3,
    ///         "deviceId": 2,
    ///         "serialNumber": 1,
    ///         "isDefault": true
    ///     }
    /// 
    /// SerialNumber can be null.
    /// </remarks>
    /// <param name="installationDto">The installation to add.</param>
    /// <returns>The ID of the added installation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AddModelResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddInstallation([FromBody] InstallationDto installationDto)
    {
        if (installationDto == null)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Installation data is required.",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        var installation = await _installationService.AddInstallationAsync(installationDto);
        var resultDto = new AddModelResultDto { Id = installation.Id };

        return CreatedAtAction(nameof(GetInstallationById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Searches for installations by query parameters.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/Installation?branchId=1
    /// 
    /// The query parameters are optional.
    /// </remarks>
    /// <param name="parameters">Query parameters for searching installations.</param>
    /// <returns>A list of installations.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InstallationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInstallations([FromQuery] InstallationQueryParameters parameters)
    {
        var installations = await _installationService.GetInstallationsAsync(parameters);
        if (installations == null || !installations.Any())
        {
            return NotFound(new ErrorResponseDto
            {
                Message = "No installations found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }
        return Ok(installations);
    }

    /// <summary>
    /// Retrieves an installation by ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/Installation/1
    /// </remarks>
    /// <param name="id">The ID of the installation to retrieve.</param>
    /// <returns>The installation with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(InstallationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInstallationById(int id)
    {
        var installation = await _installationService.GetInstallationByIdAsync(id);
        if (installation == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Message = $"Installation with ID {id} not found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }
        return Ok(installation);
    }

    /// <summary>
    /// Deletes an installation by ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /api/Installation/1
    /// </remarks>
    /// <param name="id">The ID of the installation to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInstallation(int id)
    {
        var existing = await _installationService.GetInstallationByIdAsync(id);
        if (existing == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Message = $"Installation with ID {id} not found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }

        await _installationService.DeleteInstallationAsync(id);
        return NoContent();
    }
}
