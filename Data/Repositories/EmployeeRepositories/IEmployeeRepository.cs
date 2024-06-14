using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.EmployeeRepositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployees();
}