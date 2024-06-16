using Dapper;
using PrinterAccounter.Exceptions;
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

    public async Task<Employee> GetEmployeeById(int employeeId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                Id,
                Name,
                BranchId
            FROM
                Employees
            WHERE
                Id = @EmployeeId";

        var employee = await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { EmployeeId = employeeId });

        if (employee is null)
        {
            throw new NotFoundException($"Employee with id {employeeId} was not found.");
        }
        return employee;
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