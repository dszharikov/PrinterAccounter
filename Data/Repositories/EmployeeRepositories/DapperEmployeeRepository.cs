using Dapper;
using PrinterAccounter.Models;
using PrinterAccounter.Services;

namespace PrinterAccounter.Data.Repositories.EmployeeRepositories;

public class DapperEmployeeRepository : IEmployeeRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public DapperEmployeeRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                Id,
                Name,
                BranchId
            FROM
                Employees";

        return await connection.QueryAsync<Employee>(sql);
    }
}