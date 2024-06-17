using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs.Output;
using PrinterAccounter.Models;
using PrinterAccounter.Services.EmployeeServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesErrorResponseType(typeof(ErrorResponseDto))]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Retrieves all employees.
    /// </summary>
    /// <returns>A list of employees.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _employeeService.GetEmployeesAsync();
        if (employees == null || !employees.Any())
        {
            return NotFound(new ErrorResponseDto
            {
                Message = "No employees found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }
        return Ok(employees);
    }
}
