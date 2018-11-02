using Quartz;

namespace Scheduler.Quartz.Abstract
{
    public interface IConfigurableJob : IJob
    {
        IJobDetail ConfigJobDetail();
        ITrigger ConfigTrigger();
        JobBuilder ConfigJobBuilder();
        TriggerBuilder ConfigTriggerBuilder();
    }
}
