using PrinterAccounter.Data.Repositories.BranchRepositories;
using PrinterAccounter.Models;

namespace PrinterAccounter.Services.BranchServices;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;

    public BranchService(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;   
    }
    public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
    {
        return await _branchRepository.GetAllBranches();
    }

    public async Task<bool> ExistsAsync(int branchId)
    {
        var exists = await _branchRepository.ExistsAsync(branchId);

        return exists;
    }
}