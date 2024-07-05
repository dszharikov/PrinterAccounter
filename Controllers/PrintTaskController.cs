using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs.Input;
using PrinterAccounter.DTOs.Output;
using PrinterAccounter.Services.PrintTaskServices;
using PrinterAccounter.Utils;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
[ProducesErrorResponseType(typeof(ErrorResponseDto))]
internal class PrintTaskController : ControllerBase
{
    private readonly IPrintTaskService _printTaskService;
    private readonly PrinterCsvParser _csvParser;

    public PrintTaskController(IPrintTaskService printTaskService, PrinterCsvParser csvParser)
    {
        _printTaskService = printTaskService;
        _csvParser = csvParser;
    }

    /// <summary>
    /// Adds a new print task.
    /// </summary>
    /// <remarks>
    /// Example request:
    /// 
    ///     POST /api/PrintTask
    ///     {
    ///         "name": "Task name",
    ///         "employeeId": 2,
    ///         "serialNumber": 0,
    ///         "pagesCount": 10
    ///     }
    /// 
    /// SerialNumber can be null. PagesCount must be greater than 0.
    /// </remarks>
    /// <param name="printTaskDto">Details of the print task to add.</param>
    /// <returns>The ID of the added print task.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AddModelResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPrintTask([FromBody] PrintTaskDto printTaskDto)
    {
        if (printTaskDto == null || printTaskDto.PagesCount <= 0)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Print task data is invalid or pages count is zero.",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        var printTask = await _printTaskService.AddPrintTaskAsync(printTaskDto);
        var resultDto = new AddModelResultDto { Id = printTask.Id };

        return CreatedAtAction(nameof(GetPrintTaskById), new { id = resultDto.Id }, resultDto);
    }

    /// <summary>
    /// Imports print tasks from a CSV file.
    /// </summary>
    /// <remarks>
    /// The file must be in CSV format with fields separated by semicolons (";").
    /// 
    /// Columns must be in the following order: Task name;EmployeeId;SerialNumber;PagesCount
    /// SerialNumber can be empty. The CSV file must be UTF-8 encoded.
    /// 
    /// Example of a valid CSV line:
    /// 
    ///     Task 2;4;2;10
    ///     Task 1;4;;5
    /// </remarks>
    /// <param name="file">The CSV file containing print tasks.</param>
    /// <returns>The number of rows affected.</returns>
    [HttpPost("import")]
    [ProducesResponseType(typeof(ImportPrintTasksResultDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ImportPrintTasks(IFormFile file)
    {
        if (file == null)
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "No file was provided.",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        var fileExtension = Path.GetExtension(file.FileName);
        if (fileExtension?.ToLower() != ".csv")
        {
            return BadRequest(new ErrorResponseDto
            {
                Message = "Invalid file format. Only .csv files are allowed.",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }

        byte[] fileContent;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileContent = memoryStream.ToArray();
        }

        var printTasksDto = _csvParser.ParseTasksCsv(fileContent);
        if (printTasksDto == null || !printTasksDto.Any())
        {
            return UnprocessableEntity(new ErrorResponseDto
            {
                Message = "CSV file content is invalid or empty.",
                StatusCode = StatusCodes.Status422UnprocessableEntity
            });
        }

        var rowsAffected = await _printTaskService.AddMultipleTasksAsync(printTasksDto);
        var importResultDto = new ImportPrintTasksResultDto { RowsAffected = rowsAffected };

        return CreatedAtAction(nameof(ImportPrintTasks), importResultDto);
    }
    
    /// <summary>
    /// Retrieves a print task by ID.
    /// </summary>
    /// <remarks>
    /// Example request:
    /// 
    ///     GET /api/PrintTask/1
    /// </remarks>
    /// <param name="id">The ID of the print task to retrieve.</param>
    /// <returns>The print task with the specified ID.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PrintTaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPrintTaskById(int id)
    {
        var printTask = await _printTaskService.GetPrintTaskByIdAsync(id);
        if (printTask == null)
        {
            return NotFound(new ErrorResponseDto
            {
                Message = $"Print task with ID {id} not found.",
                StatusCode = StatusCodes.Status404NotFound
            });
        }
        return Ok(printTask);
    }
}
