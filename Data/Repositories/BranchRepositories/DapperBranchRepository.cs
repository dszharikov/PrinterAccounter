using Dapper;
using PrinterAccounter.Models;
using PrinterAccounter.Services;

namespace PrinterAccounter.Data.Repositories.BranchRepositories;

public class DapperBranchRepository : IBranchRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public DapperBranchRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<IEnumerable<Branch>> GetAllBranches()
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                Id,
                Name,
                Location
            FROM
                Branches";
        
        return await connection.QueryAsync<Branch>(sql);
    }
}