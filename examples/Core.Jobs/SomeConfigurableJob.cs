using System.Threading.Tasks;
using Quartz;
using Scheduler.Quartz.Abstract;

namespace Core.Jobs
{
    public class SomeConfigurableJob : IConfigurableJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        public IJobDetail CreateJobDetail()
        {
            throw new System.NotImplementedException();
        }

        public ITrigger CreateTrigger()
        {
            throw new System.NotImplementedException();
        }

        public JobBuilder CreateJobBuilder()
        {
            throw new System.NotImplementedException();
        }

        public TriggerBuilder CreateTriggerBuilder()
        {
            throw new System.NotImplementedException();
        }
    }
}