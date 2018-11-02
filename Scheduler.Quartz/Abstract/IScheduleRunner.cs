using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace Scheduler.Quartz.Abstract
{
    public interface IScheduleRunner
    {
        void SetJobFactory(IJobFactory jobFactory);
        Task Start();
        Task Stop();
        Task Stop(bool isNeedWaitForJobsToComplete);
        Task StartJob(IJobDetail jobDetail, ITrigger trigger);
        void Config(IDictionary<IJobDetail, ITrigger> jobs);
        Task RunJob<T>(int intervalInSeconds, bool isNeedRepeatForever = true) where T : IConfigurableJob;
    }
}
