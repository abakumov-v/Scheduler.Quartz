using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Scheduler.Quartz.Ioc.ServiceProvider
{
    public class ServiceProviderQuartzJobFactory : IJobFactory
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderQuartzJobFactory(ILogger<ServiceProviderQuartzJobFactory> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        // source: http://tech.trailmax.info/2013/07/quartz-net-in-azure-with-autofac-smoothness/
        // Just instead Autofac we use .NET Core Ioc container
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Type jobType = null;
            try
            {
                var jobDetail = bundle.JobDetail;
                jobType = jobDetail.JobType;

                // resolve job object from DI - populate all services and repositories
                var job = _serviceProvider.GetRequiredService(jobType);

                return (IJob) job;
            }
            catch (Exception ex)
            {
                var message =
                    $"Problem instantiating class {(jobType != null ? jobType.Name : "UNKNOWN")} because: {ex.Message}";
                throw new SchedulerException(message, ex);
            }
        }

        public void ReturnJob(IJob job)
        {
            // source: http://tech.trailmax.info/2013/07/quartz-net-in-azure-with-autofac-smoothness/
            // do nothing here. Don't really care. Quartz.Net SimpleJobFactory does not have anything here.
        }
    }
}