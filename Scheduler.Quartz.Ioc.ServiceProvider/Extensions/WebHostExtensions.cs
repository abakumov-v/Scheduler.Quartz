using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz.Ioc.ServiceProvider.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost UseQuartz(this IWebHost host, Action<IScheduleRunner> scheduleJob = null)
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