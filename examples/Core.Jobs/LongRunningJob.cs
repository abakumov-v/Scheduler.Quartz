using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Scheduler.Quartz.Abstract;

namespace Core.Jobs
{
    public class LongRunningJob : LoggableJob
    {
        private readonly ILogger _logger;

        public LongRunningJob(ILogger<LongRunningJob> logger) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public override void ExecuteJob(IJobExecutionContext context)
        {
            _logger.LogInformation("Sync long-running job starting...");
            Task.Delay(2000).GetAwaiter().GetResult();
            _logger.LogInformation("Sync long-running job was completed.");
        }
    }
}