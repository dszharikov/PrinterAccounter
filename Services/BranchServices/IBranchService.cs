using PrinterAccounter.Models;

namespace PrinterAccounter.Services.BranchServices;

internal interface IBranchService
{
    Task<IEnumerable<Branch>> GetAllBranchesAsync();
    Task<bool> ExistsAsync(int branchId);
}