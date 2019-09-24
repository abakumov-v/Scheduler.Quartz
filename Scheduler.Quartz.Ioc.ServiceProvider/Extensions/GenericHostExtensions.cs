using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz.Ioc.ServiceProvider.Extensions
{
    public static class GenericHostExtensions
    {
        public static IHost UseQuartz(this IHost host, Action<IScheduleRunner> scheduleJob = null)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var schedulerRunnerFactory = serviceProvider.GetRequiredService<ISchedulerRunnerFactory>();
                var scheduler = schedulerRunnerFactory.Create();
                scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();

                scheduleJob?.Invoke(scheduler);
            }
            return host;
        }
    }
}