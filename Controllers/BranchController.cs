using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.Models;
using PrinterAccounter.Services.BranchServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("[controller]")]
public class BranchController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await _branchService.GetAllBranchesAsync();
        return Ok(branches);
    }
}