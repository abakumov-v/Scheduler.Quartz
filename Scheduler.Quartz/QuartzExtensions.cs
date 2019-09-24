using System;
using Scheduler.Quartz.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Scheduler.Quartz
{
    public static class QuartzExtensions
    {
        [Obsolete("You must start the scheduler via IHostedService instead of this. Will removed in future.", false)]
        public static void StartQuartzScheduler(this IApplicationBuilder app)
        {
            app.ApplicationServices.StartQuartzScheduler();
        }

        [Obsolete("You must start the scheduler via IHostedService instead of this. Will removed in future.", false)]
        public static void StartQuartzScheduler(this IServiceProvider serviceProvider)
        {
            // If IScheduleRunner was not registered, the GetRequiredService method throw an exception,
            // so we don't need null-checking 
            var schedulerFactory = serviceProvider.GetRequiredService<ISchedulerRunnerFactory>();
            // Waiting synchronously because it is called only in 1 place - Startup.cs
            var scheduler = schedulerFactory.Create();
            scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
