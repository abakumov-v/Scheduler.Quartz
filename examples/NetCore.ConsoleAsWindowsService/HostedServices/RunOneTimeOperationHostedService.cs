using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Models.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetCore.ConsoleAsWindowsService.HostedServices
{
    /// <summary>
    /// Run some operation 1 time (on demand)
    /// </summary>
    public class RunOneTimeOperationHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly ConsoleSettings _consoleSettings;

        public RunOneTimeOperationHostedService(
            ILogger<RunOneTimeOperationHostedService> logger,
            ConsoleSettings consoleSettings
        )
        {
            _logger = logger;
            _consoleSettings = consoleSettings;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting");

            _logger.LogDebug("Do some useful work...");
            await Task.Delay(2000, cancellationToken);
            _logger.LogDebug("Useful work was completed successfully");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}