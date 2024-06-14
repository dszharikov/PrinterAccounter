using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.BranchRepositories;

public interface IBranchRepository
{
    Task<IEnumerable<Branch>> GetAllBranches();
}