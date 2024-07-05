using PrinterAccounter.Models;

namespace PrinterAccounter.Data.Repositories.EmployeeRepositories;

internal interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployees();
    Task<Employee> GetEmployeeById(int employeeId);
}