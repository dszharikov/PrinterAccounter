using Dapper;
using PrinterAccounter.Models;
using PrinterAccounter.Services;

namespace PrinterAccounter.Data.Repositories.DeviceRepositories;

public class DapperDeviceRepository : IDeviceRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public DapperDeviceRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<IEnumerable<Device>> GetAllDevices(string? connectionType = null)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        string sql = @"
            SELECT
                Id,
                Name,
                ConnectionType,
                MacAddress
            FROM
                Devices";

        if (!string.IsNullOrEmpty(connectionType))
        {
            sql += " WHERE ConnectionType = @ConnectionType";
            return await connection.QueryAsync<Device>(sql, new { ConnectionType = connectionType });
        }

        return await connection.QueryAsync<Device>(sql);
    }
}