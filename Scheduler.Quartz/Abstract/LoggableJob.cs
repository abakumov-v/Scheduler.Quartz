using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Scheduler.Quartz.Abstract
{
    public abstract class LoggableJob : IJob
    {
        protected ILogger Logger { get; }

        protected LoggableJob(ILogger logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            Logger = logger;
        }

        public abstract void ExecuteJob(IJobExecutionContext context);

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var jobId = GetJobId(context);
                var jobName = GetJobName(context);

                try
                {
                    Logger.LogInformation($"[{jobId}] {jobName} - job started");

                    ExecuteJob(context);

                    Logger.LogInformation($"[{jobId}] {jobName} - job finished");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"[{jobId}] {jobName} - job finished with error: ");
                }
            });
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