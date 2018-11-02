using System;
using Scheduler.Quartz.Abstract;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Scheduler.Quartz
{
    public static class QuartzExtensions
    {
        public static void StartQuartzScheduler(this IApplicationBuilder app)
        {
            app.ApplicationServices.StartQuartzScheduler();
        }

        public static void StartQuartzScheduler(this IServiceProvider serviceProvider)
        {
            // If IScheduleRunner was not registered, the GetRequiredService method throw an exception,
            // so we don't need null-checking 
            var scheduler = serviceProvider.GetRequiredService<IScheduleRunner>();
            scheduler.Start().Wait();
        }
    }
}
