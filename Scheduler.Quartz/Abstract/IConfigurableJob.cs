using Quartz;

namespace Scheduler.Quartz.Abstract
{
    public interface IConfigurableJob : IJob
    {
        IJobDetail CreateJobDetail();
        ITrigger CreateTrigger();
        JobBuilder CreateJobBuilder();
        TriggerBuilder CreateTriggerBuilder();
    }
}
