using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz
{
    public class QuartzScheduleRunner : IScheduleRunner
    {
        private readonly ILogger _logger;
        private readonly IScheduler _scheduler;
        private readonly Dictionary<IJobDetail, ITrigger> _jobsWithTriggers = new Dictionary<IJobDetail, ITrigger>();

        public QuartzScheduleRunner(ILogger<QuartzScheduleRunner> logger, IJobFactory jobFactory,
            NameValueCollection schedulerProps = null)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;

            _logger.LogDebug("Wait to create Quartz scheduler...");

            ISchedulerFactory factory = schedulerProps != null
                ? new StdSchedulerFactory(schedulerProps)
                : new StdSchedulerFactory();

            _scheduler = factory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();

            _logger.LogDebug("Quartz Scheduler was created successfully");
            SetJobFactory(jobFactory);
        }

        internal void SetJobFactory(IJobFactory jobFactory)
        {
            if (jobFactory != null && _scheduler != null)
            {
                _scheduler.JobFactory = jobFactory;
                _logger.LogDebug($"Set the \"{jobFactory.GetType()}\" job factory for scheduler");
            }
        }

        public async Task Start()
        {
            _logger.LogDebug("Scheduler will be started...");
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start().ConfigureAwait(false);
            }
        }
        
        public async Task Stop(bool needWaitForJobsToComplete = true)
        {
            _logger.LogDebug("Scheduler will be stopped");
            if (_scheduler.IsStarted)
            {
                await _scheduler.Shutdown(needWaitForJobsToComplete).ConfigureAwait(false);
            }
        }

        public async Task ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            _logger.LogDebug($"Scheduler will be start job \"{jobDetail.Key}\" with its trigger \"{trigger.Key}\"");
            await _scheduler.ScheduleJob(jobDetail, trigger).ConfigureAwait(false);
        }

        /*
        public Task AddJob(IJobDetail jobDetail, ITrigger trigger, bool isNeedReplace)
        {
            return Task.Run(() =>
            {
                if (_jobsWithTriggers.Any(e => e.Key.Key.Name.Equals(jobDetail.Key.Name) && isNeedReplace))
                {
                    _jobsWithTriggers[jobDetail] = trigger;
                }
                else
                {
                    _jobsWithTriggers.Add(jobDetail, trigger);
                }
            });
        }
        */

        internal void Config(IDictionary<IJobDetail, ITrigger> jobs)
        {
            foreach (var item in jobs)
            {
                _jobsWithTriggers.Add(item.Key, item.Value);
            }
        }

        /*
        public void RunJobs()
        {
            foreach (var item in _jobsWithTriggers)
            {
                var job = item.Key;
                var trigger = item.Value;
                _scheduler.ScheduleJob(job, trigger);
            }
        }
        */

        public async Task ScheduleRepeatableJob<T>(int intervalInSeconds, bool isRepeatForever = true,
            bool needStartNow = true) where T : IJob
        {
            var jobIdentity = typeof(T).Name;
            var jobDetail = JobBuilder.Create(typeof(T))
                .WithIdentity($"{jobIdentity}_Name", typeof(T).Name)
                .Build();

            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity($"{jobIdentity}_Trigger", typeof(T).Name)
                //.StartAt(startTime)
                .WithSimpleSchedule(t =>
                {
                    if (isRepeatForever)
                    {
                        t.WithIntervalInSeconds(intervalInSeconds).RepeatForever();
                    }
                    else
                        t.WithIntervalInSeconds(intervalInSeconds);
                });
            if (needStartNow)
            {
                triggerBuilder.StartNow();
            }

            var trigger = triggerBuilder.Build();

            _logger.LogInformation(
                $"Job \"{typeof(T).Name}\" will be scheduled with this settings: intervalInSeconds={intervalInSeconds}, isRepeatForever={isRepeatForever}, needStartNow={needStartNow}");
            await ScheduleJob(jobDetail, trigger).ConfigureAwait(false);
        }
    }
}
