using System.Collections.Generic;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace Scheduler.Quartz.Abstract
{
    public interface IScheduleRunner
    {
        //void SetJobFactory(IJobFactory jobFactory);
        Task Start();
        Task Stop(bool needWaitForJobsToComplete = true);
        Task ScheduleJob(IJobDetail jobDetail, ITrigger trigger);
        //void Config(IDictionary<IJobDetail, ITrigger> jobs);
        Task ScheduleRepeatableJob<T>(int intervalInSeconds, bool isNeedRepeatForever = true,
            bool needStartNow = true) where T : IJob;
    }
}
