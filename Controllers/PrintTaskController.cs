using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs;
using PrinterAccounter.Services.PrintTaskServices;
using PrinterAccounter.Utils;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrintTaskController : ControllerBase
{
    private readonly IPrintTaskService _printTaskService;
    private readonly PrinterCsvParser _csvParser;

    public PrintTaskController(IPrintTaskService printTaskService, 
                               PrinterCsvParser csvParser)
    {
        _printTaskService = printTaskService;

        _csvParser = csvParser;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrintTask([FromBody] PrintTaskDto printTaskDto)
    {
        var printTask = await _printTaskService.AddPrintTaskAsync(printTaskDto);

        return CreatedAtAction(nameof(AddPrintTask), new { id = printTask.Id }, printTask);
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportPrintTasks(IFormFile file)
    {
        if (file == null)
        {
            return BadRequest("No file was provided.");
        }

        // Проверка на расширение файла
        var fileExtension = Path.GetExtension(file.FileName);
        if (fileExtension?.ToLower() != ".csv")
        {
            return BadRequest("Invalid file format. Only .csv files are allowed.");
        }

        // Чтение файла в память
        byte[] fileContent;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileContent = memoryStream.ToArray();
        }

        var printTasksDto = _csvParser.ParseTasksCsv(fileContent);

        var rowsAffected = await _printTaskService.AddMultipleTasksAsync(printTasksDto);
        // var rowsAffected = await _printTaskService.AddPrintTasksAsync(printTasksDto);

        return CreatedAtAction(nameof(ImportPrintTasks), rowsAffected);
    }

}