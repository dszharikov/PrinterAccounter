using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.BranchRepositories;

internal interface IBranchRepository
{
    Task<IEnumerable<Branch>> GetAllBranches();
    Task<bool> ExistsAsync(int branchId);
}