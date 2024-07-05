using PrinterAccounter.Models;

namespace PrinterAccounter.Services.BranchServices;

public interface IBranchService
{
    Task<IEnumerable<Branch>> GetAllBranchesAsync();
    Task<bool> ExistsAsync(int branchId);
}