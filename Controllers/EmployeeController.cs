using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.Models;
using PrinterAccounter.Services.EmployeeServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<IEnumerable<Employee>> GetEmployees()
    {
        return await _employeeService.GetEmployeesAsync();
    }
}