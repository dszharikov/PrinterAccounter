using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs.Output;
using PrinterAccounter.Models;
using PrinterAccounter.Services.BranchServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesErrorResponseType(typeof(ErrorResponseDto))]
public class BranchController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    /// <summary>
    /// Retrieves all branches.
    /// </summary>
    /// <returns>A list of branches.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Branch>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await _branchService.GetAllBranchesAsync();
        if (branches == null || !branches.Any())
        {
            return NotFound(new ErrorResponseDto
            {
                Message = "No branches found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }
        return Ok(branches);
    }
}
