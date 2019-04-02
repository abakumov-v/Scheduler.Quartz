using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Job;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;

namespace Scheduler.Quartz.Ioc.ServiceProvider
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services)
        {
            services
                .Scan(scan => scan
                    // Register all Scheduler.Quartz built-in jobs
                    .FromAssemblyOf<FileScanJob>()
                    .AddClasses(classes => classes.AssignableTo<IJob>())
                    .AsSelf()
                    .WithTransientLifetime()
                );

            return services
                .AddSingleton<IScheduleRunner, QuartzScheduleRunner>()
                .AddSingleton<IJobFactory, ServiceProviderQuartzJobFactory>();
        }
    }
}