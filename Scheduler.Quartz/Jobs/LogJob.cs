using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Scheduler.Quartz.Jobs
{
    public class LogJob : IJob //IConfigurableJob
    {
        private readonly ILogger _logger;

        public LogJob(ILogger<LogJob> logger)
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

        /*
        public IJobDetail ConfigJobDetail()
        {
            return ConfigJobBuilder().Build();
        }

        public ITrigger ConfigTrigger()
        {
            return ConfigTriggerBuilder().Build();
        }

        public JobBuilder ConfigJobBuilder()
        {
            return JobBuilder.Create<LogJob>();
        }

        public TriggerBuilder ConfigTriggerBuilder()
        {
            return TriggerBuilder.Create()
                .WithIdentity($"{nameof(LogJob)}Trigger")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever());
        }
        */
    }
}