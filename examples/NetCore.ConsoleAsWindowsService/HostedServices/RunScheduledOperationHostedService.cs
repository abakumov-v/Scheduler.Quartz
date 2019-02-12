using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scheduler.Quartz.Abstract;

namespace NetCore.ConsoleAsWindowsService.HostedServices
{
    /// <summary>
    /// Run some operation (on schedule)
    /// </summary>
    public class RunScheduledOperationHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IScheduleRunner _scheduleRunner;

        public RunScheduledOperationHostedService(
            ILogger<RunScheduledOperationHostedService> logger,
            IScheduleRunner scheduleRunner
            )
        {
            _logger = logger;
            _scheduleRunner = scheduleRunner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting scheduler");
            await _scheduleRunner.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping scheduler...");
            await _scheduleRunner.Stop(true);
            _logger.LogDebug("Stopping service");
        }

        public void Dispose()
        {
            
        }
    }
}