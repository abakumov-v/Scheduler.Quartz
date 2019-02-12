using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Scheduler.Quartz.Abstract;

namespace Core.Jobs
{
    public class ExampleJob : LoggableJob
    {
        public ExampleJob(ILogger<ExampleJob> logger) : base(logger)
        {

        }

        public override void ExecuteJob(IJobExecutionContext context)
        {
            Logger.LogInformation("Job will doing some work...");

            Task.Delay(2000).ConfigureAwait(false).GetAwaiter().GetResult();

            Logger.LogInformation("Job complete work successfully");
        }
    }
}
