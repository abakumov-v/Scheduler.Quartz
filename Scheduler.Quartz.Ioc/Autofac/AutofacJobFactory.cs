using System;
using Autofac;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Scheduler.Quartz.Ioc.Autofac
{
    public class AutofacJobFactory : IJobFactory
    {
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger _logger;

        public AutofacJobFactory(ILifetimeScope lifetimeScope, ILogger<AutofacJobFactory> logger)
        {
            if (lifetimeScope == null)
                throw new ArgumentNullException(nameof(lifetimeScope));
            _lifetimeScope = lifetimeScope;

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }
        // source: http://tech.trailmax.info/2013/07/quartz-net-in-azure-with-autofac-smoothness/
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type jobType = null;
            try
            {
                var jobDetail = bundle.JobDetail;
                jobType = jobDetail.JobType;

                // resolve job object from DI - populate all services and repositories
                var job = _lifetimeScope.Resolve(jobType);

                return (IJob)job;
            }
            catch (Exception exception)
            {
                var message = $"Problem instantiating class {(jobType != null ? jobType.Name : "UNKNOWN")}";
                throw new SchedulerException(message, exception);
            }
        }

        public void ReturnJob(IJob job)
        {
            // source: http://tech.trailmax.info/2013/07/quartz-net-in-azure-with-autofac-smoothness/
            // do nothing here. Don't really care. Quartz.Net SimpleJobFactory does not have anything here.
        }
    }
}
