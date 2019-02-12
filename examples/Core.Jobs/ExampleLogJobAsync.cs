using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Scheduler.Quartz.Abstract;

namespace Core.Jobs
{
    public class ExampleLogJobAsync : LoggableJobAsync
    {
        private readonly ILogger _logger;

        public ExampleLogJobAsync(ILogger<ExampleLogJobAsync> logger) : base(logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public override Task ExecuteJobAsync(IJobExecutionContext context)
        {
            _logger.LogInformation("Example job was completed");
            return Task.CompletedTask;
        }
    }
}