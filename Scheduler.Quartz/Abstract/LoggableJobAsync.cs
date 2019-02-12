using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Scheduler.Quartz.Abstract
{
    public abstract class LoggableJobAsync : IJob
    {
        protected ILogger Logger { get; }

        protected LoggableJobAsync(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            Logger = logger;
        }

        public abstract Task ExecuteJobAsync(IJobExecutionContext context);

        public async Task Execute(IJobExecutionContext context)
        {
            var jobId = GetJobId(context);
            var jobName = GetJobName(context);

            try
            {
                Logger.LogInformation($"[{jobId}] {jobName} - job started");

                await ExecuteJobAsync(context);

                Logger.LogInformation($"[{jobId}] {jobName} - job finished");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"[{jobId}] {jobName} - job finished with error");
            }
        }

        private string GetJobId(IJobExecutionContext context)
        {
            return context?.FireInstanceId;
        }
        private string GetJobName(IJobExecutionContext context)
        {
            return context?.JobDetail?.Key?.Name;
        }
    }
}