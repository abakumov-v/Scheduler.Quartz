using System.Threading;
using System.Threading.Tasks;
using Core.Jobs;
using Microsoft.Extensions.Hosting;
using Scheduler.Quartz.Abstract;

namespace NetCore.WebApi.ServiceProvider.InMemoryConfig.Extensions
{
    public class QuartzSchedulerHostedService : IHostedService
    {
        private readonly IScheduleRunner _scheduler;

        public QuartzSchedulerHostedService(ISchedulerRunnerFactory schedulerRunnerFactory)
        {
            _scheduler = schedulerRunnerFactory.Create();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _scheduler.ScheduleRepeatableJob<ExampleLogJobAsync>(60);
            await _scheduler.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Stop();
        }
    }
}