using System;
using System.Collections.Generic;
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

        public QuartzScheduleRunner(ILogger<QuartzScheduleRunner> logger, IJobFactory jobFactory)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            _logger = logger;

            _logger.LogTrace("Wait to create Quartz scheduler...");
            var factory = new StdSchedulerFactory();
            _scheduler = factory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();

            _logger.LogTrace("Quartz Scheduler was created successfully");
            SetJobFactory(jobFactory);
        }

        public void SetJobFactory(IJobFactory jobFactory)
        {
            if (jobFactory != null && _scheduler != null)
            {
                _scheduler.JobFactory = jobFactory;
                _logger.LogTrace($"Set the \"{jobFactory.GetType()}\" job factory for scheduler");
            }
        }

        public async Task Start()
        {
            _logger.LogTrace($"Scheduler will be started...");
            if (!_scheduler.IsStarted)
            {
                await _scheduler.Start().ConfigureAwait(false);
            }
        }

        public async Task Stop()
        {
            _logger.LogTrace($"Scheduler will be stopped");
            await Stop(true).ConfigureAwait(false);
        }

        public async Task Stop(bool isNeedWaitForJobsToComplete)
        {
            if (_scheduler.IsStarted)
            {
                await _scheduler.Shutdown(isNeedWaitForJobsToComplete).ConfigureAwait(false);
            }
        }

        public async Task StartJob(IJobDetail jobDetail, ITrigger trigger)
        {
            _logger.LogTrace($"Scheduler will be start job \"{jobDetail.Key}\" with its trigger \"{trigger.Key}\"");
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

        public void Config(IDictionary<IJobDetail, ITrigger> jobs)
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

        public async Task RunJob<T>(int intervalInSeconds, bool isNeedRepeatForever = true) where T : IConfigurableJob
        {
            var jobIdentity = typeof(T).Name;
            var jobDetail = JobBuilder.Create(typeof(T))
                .WithIdentity($"{jobIdentity}_Name", typeof(T).Name)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{jobIdentity}_Trigger", typeof(T).Name)
                //.StartAt(startTime)
                .StartNow()
                .WithSimpleSchedule(t =>
                {
                    if (isNeedRepeatForever)
                    {
                        t.WithIntervalInSeconds(intervalInSeconds).RepeatForever();
                    }
                    else
                        t.WithIntervalInSeconds(intervalInSeconds);
                })
                .Build();

            await StartJob(jobDetail, trigger).ConfigureAwait(false);
        }
    }
}
