using Microsoft.AspNetCore.Mvc;
using PrinterAccounter.DTOs;
using PrinterAccounter.Services.PrintTaskServices;

namespace PrinterAccounter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrintTaskController : ControllerBase
{
    private readonly IPrintTaskService _printTaskService;

    public PrintTaskController(IPrintTaskService printTaskService)
    {
        _printTaskService = printTaskService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrintTask([FromBody] PrintTaskDto printTaskDto)
    {
        var printTask = await _printTaskService.AddPrintTaskAsync(printTaskDto);

        return CreatedAtAction(nameof(AddPrintTask), new { id = printTask.Id }, printTask);
    }
}