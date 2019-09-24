namespace Scheduler.Quartz.Abstract
{
    public interface ISchedulerRunnerFactory
    {
        IScheduleRunner Create();
    }
}