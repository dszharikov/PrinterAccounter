using PrinterAccounter.Models;

namespace PrinterAccounter.Services.PrintServices;

internal class PrintSimulatorService : IPrintService
{
    private readonly Random _random;
    public PrintSimulatorService()
    {
        _random = new Random();
    }
    public async Task<bool> PrintJobAsync(PrintTask printTask)
    {
        int delay = _random.Next(1000, 4000);

        await Task.Delay(delay);

        // 90 percent probability of success
        return _random.NextDouble() > 0.1;
    }
}