using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using Quartz.Impl;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz.Factories
{
    public abstract class BaseMemorySchedulerRunnerFactory : ISchedulerRunnerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IJobFactory _jobFactory;
        protected NameValueCollection SchedulerSettings { get; private set; }

        protected BaseMemorySchedulerRunnerFactory(ILoggerFactory loggerFactory, IJobFactory jobFactory)
        {
            _loggerFactory = loggerFactory;
            _jobFactory = jobFactory;

            SchedulerSettings = new NameValueCollection();
        }

        public virtual IScheduleRunner Create()
        {
            var logger = _loggerFactory.CreateLogger<QuartzScheduleRunner>();
            return new QuartzScheduleRunner(logger, _jobFactory, SchedulerSettings);
        }
    }
}