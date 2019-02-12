using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Scheduler.Quartz.Abstract;

namespace Core.Jobs
{
    public class LongRunningJobAsync : LoggableJobAsync
    {
        private readonly ILogger _logger;

        public LongRunningJobAsync(ILogger<LongRunningJobAsync> logger) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }
        
        public override async Task ExecuteJobAsync(IJobExecutionContext context)
        {
            _logger.LogInformation("Async long-running job starting...");
            await Task.Delay(2000);
            _logger.LogInformation("Async long-running job was completed.");
        }
    }
}