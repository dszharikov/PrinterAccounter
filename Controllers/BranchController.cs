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
    public async Task<IEnumerable<Branch>> GetBranches()
    {
        return await _branchService.GetAllBranchesAsync();
    }
}