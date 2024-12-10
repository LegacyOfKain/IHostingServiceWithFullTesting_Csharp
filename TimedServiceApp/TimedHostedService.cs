namespace TimedServiceApp;

using Microsoft.Extensions.Hosting;
using NLog;

public class TimedHostedService : IHostedService
{
    private readonly ITimePrinter _timePrinter;
    private readonly ILogger _logger;
    private Timer _timer;

    public TimedHostedService(ITimePrinter timePrinter, ILogger logger)
    {
        _timePrinter = timePrinter;
        _logger = logger ;
        _timer = null!;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.Debug("Timed Hosted Service is starting.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    public void DoWork(object? state)
    {
        try
        {
            _timePrinter.PrintCurrentTime();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An error occurred while printing the time.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.Debug("Timed Hosted Service is stopping.");

        _timer.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}