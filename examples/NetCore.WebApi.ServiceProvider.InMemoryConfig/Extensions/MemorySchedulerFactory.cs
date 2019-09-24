using Core.Jobs;
using Microsoft.Extensions.Logging;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;
using Scheduler.Quartz.Factories;

namespace NetCore.WebApi.ServiceProvider.InMemoryConfig.Extensions
{
    public class MemorySchedulerFactory : BaseMemorySchedulerRunnerFactory
    {
        public MemorySchedulerFactory(ILoggerFactory loggerFactory, IJobFactory jobFactory)
            : base(loggerFactory, jobFactory)
        {
            SchedulerSettings.Add("quartz.scheduler.instanceName", "ExampleQuartzScheduler");
            SchedulerSettings.Add("quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz");
            SchedulerSettings.Add("quartz.threadPool.threadCount", "3");
        }
    }
}