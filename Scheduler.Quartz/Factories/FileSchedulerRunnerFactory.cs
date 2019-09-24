using Microsoft.Extensions.Logging;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz.Factories
{
    public class FileSchedulerRunnerFactory : ISchedulerRunnerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IJobFactory _jobFactory;

        public FileSchedulerRunnerFactory(ILoggerFactory loggerFactory, IJobFactory jobFactory)
        {
            _loggerFactory = loggerFactory;
            _jobFactory = jobFactory;
        }

        public IScheduleRunner Create()
        {
            var logger = _loggerFactory.CreateLogger<QuartzScheduleRunner>();
            return new QuartzScheduleRunner(logger, _jobFactory);
        }
    }
}