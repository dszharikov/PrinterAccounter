using PrinterAccounter.Models;

namespace PrinterAccounter.Services.EmployeeServices;

internal interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int employeeId);
}