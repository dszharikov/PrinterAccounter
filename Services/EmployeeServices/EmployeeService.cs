using PrinterAccounter.Data.Repositories.EmployeeRepositories;
using PrinterAccounter.Models;

namespace PrinterAccounter.Services.EmployeeServices;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
    {
        return await _employeeRepository.GetEmployeeById(employeeId);
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        return await _employeeRepository.GetEmployees();
    }
}