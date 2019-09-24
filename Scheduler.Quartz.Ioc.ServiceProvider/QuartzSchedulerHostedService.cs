using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz.Ioc.ServiceProvider
{
    public class QuartzSchedulerHostedService : IHostedService
    {
        private readonly IScheduleRunner _scheduler;

        public QuartzSchedulerHostedService(ISchedulerRunnerFactory schedulerRunnerFactory)
        {
            _scheduler = schedulerRunnerFactory.Create();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Stop();
        }
    }
}