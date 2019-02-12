using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Core.Jobs
{
    public class ExampleLogJob : IJob
    {
        private readonly ILogger _logger;

        public ExampleLogJob(ILogger<ExampleLogJob> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var jobId = context.FireInstanceId;
                var jobName = context.JobDetail.Key.Name;

                _logger.LogInformation($"[{jobId}] {jobName} - job is work");
            });
        }
    }
}