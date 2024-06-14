using PrinterAccounter.Models;

namespace PrinterAccounter.Services.EmployeeServices;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployeesAsync();
}