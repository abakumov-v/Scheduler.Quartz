using System;
using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Scheduler.Quartz.Ioc.ServiceProvider
{
    public class ServiceProviderQuartzJobFactory : IJobFactory, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;

        internal ConcurrentDictionary<object, JobTrackingInfo> RunningJobs { get; } =
            new ConcurrentDictionary<object, JobTrackingInfo>();

        public ServiceProviderQuartzJobFactory(
            ILogger<ServiceProviderQuartzJobFactory> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        // source: https://github.com/alphacloud/Autofac.Extras.Quartz/blob/v2.1.1/src/Autofac.Extras.Quartz/AutofacJobFactory.cs
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {

            if (bundle == null) throw new ArgumentNullException(nameof(bundle));
            if (scheduler == null) throw new ArgumentNullException(nameof(scheduler));

            var jobType = bundle.JobDetail.JobType;

            var scope = _serviceProvider.CreateScope();

            IJob newJob;
            try
            {
                newJob = (IJob)scope.ServiceProvider.GetRequiredService(jobType);
                var jobTrackingInfo = new JobTrackingInfo(scope);
                RunningJobs[newJob] = jobTrackingInfo;

                scope = null;
            }
            catch (Exception ex)
            {
                scope?.Dispose();

                throw new SchedulerConfigException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to instantiate Job '{0}' of type '{1}'",
                    bundle.JobDetail.Key, bundle.JobDetail.JobType), ex);
            }

            return newJob;
        }

        public void Dispose()
        {
            RunningJobs.Clear();
        }

        public void ReturnJob(IJob job)
        {
            if (job == null)
                return;

            if (!RunningJobs.TryRemove(job, out var trackingInfo))
            {
                _logger.LogWarning("Tracking info for job {HashCode:x} not found", job.GetHashCode());
                // ReSharper disable once SuspiciousTypeConversion.Global
                var disposableJob = job as IDisposable;
                disposableJob?.Dispose();
            }
            else
            {
                trackingInfo.Scope?.Dispose();
            }
        }

        #region Job data

        internal sealed class JobTrackingInfo
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
            /// </summary>
            public JobTrackingInfo(IServiceScope scope)
            {
                Scope = scope;
            }

            public IServiceScope Scope { get; }
        }

        #endregion Job data
    }
}