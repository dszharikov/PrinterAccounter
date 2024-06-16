using Dapper;
using PrinterAccounter.Models;
using PrinterAccounter.Services;

namespace PrinterAccounter.Data.Repositories.PrintTaskRepositories;

public class DapperPrintTaskRepository : IPrintTaskRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public DapperPrintTaskRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<PrintTask> AddPrintTaskAsync(PrintTask printTask)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = @"
                INSERT INTO PrintTasks (Name, EmployeeId, InstallationId, PagesCount, IsSuccess)
                OUTPUT INSERTED.Id
                VALUES (@Name, @EmployeeId, @InstallationId, @PagesCount, @IsSuccess)";
        
        var id = await connection.ExecuteScalarAsync<int>(sql, printTask);

        printTask.Id = id;

        return printTask;
    }

    public async Task<int> AddMultiplePrintTasksAsync(IEnumerable<PrintTask> printTasks)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = @"
                INSERT INTO PrintTasks (Name, EmployeeId, InstallationId, PagesCount, IsSuccess)
                VALUES (@Name, @EmployeeId, @InstallationId, @PagesCount, @IsSuccess)";

        var rowsAffected = await connection.ExecuteAsync(sql, printTasks);
        return rowsAffected;
    }
}