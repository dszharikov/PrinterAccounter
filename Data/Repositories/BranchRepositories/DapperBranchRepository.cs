using Dapper;
using Microsoft.Data.SqlClient;
using PrinterAccounter.Exceptions;
using PrinterAccounter.Models;
using PrinterAccounter.Services;

namespace PrinterAccounter.Data.Repositories.BranchRepositories;

internal class DapperBranchRepository : IBranchRepository
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

    public async Task<bool> ExistsAsync(int branchId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<bool>(
            "SELECT COUNT(1) FROM Branches WHERE Id = @BranchId",
            new { BranchId = branchId });

    }
}