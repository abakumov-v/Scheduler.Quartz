using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Job;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;
using Scheduler.Quartz.Factories;

namespace Scheduler.Quartz.Ioc.ServiceProvider
{
    public static class ServiceProviderExtensions
    {
        [Obsolete("You must use the \"AddQuartz\" method instead of this", true)]
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

        /// <summary>
        /// Register basic dependencies:
        /// - IScheduleRunner
        /// - IJobFactory
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddQuartz(this IServiceCollection services,
            QuartzSchedulerOptions options = null)
        {
            services
                .AddSingleton<IScheduleRunner, QuartzScheduleRunner>()
                .AddSingleton<IJobFactory, ServiceProviderQuartzJobFactory>();

            if (options?.ConfigSource == ConfigSource.File)
            {
                services
                    .AddSingleton<ISchedulerRunnerFactory, FileSchedulerRunnerFactory>()
                    .Scan(scan => scan
                        // Register all Quartz built-in jobs
                        .FromAssemblyOf<FileScanJob>()
                        .AddClasses(classes => classes.AssignableTo<IJob>())
                        .AsSelf()
                        .WithTransientLifetime()
                    );
            }
            else if (options?.ConfigSource == ConfigSource.InMemory)
            {
                if (options.InMemorySchedulerFactoryType == null)
                    throw new ArgumentException(
                        $"Type of implementation of \"{typeof(BaseMemorySchedulerRunnerFactory).Name}\" must be specified.");

                services
                    .Scan(scan => scan
                        .FromAssembliesOf(options.InMemorySchedulerFactoryType)
                        .AddClasses(classes => classes.AssignableTo<BaseMemorySchedulerRunnerFactory>())
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime()
                    );
            }

            if (options?.OneOfCustomQuartzJobsType != null)
            {
                services
                    .Scan(scan => scan
                        // Register all custom Quartz jobs
                        .FromAssembliesOf(options.OneOfCustomQuartzJobsType)
                        .AddClasses(classes => classes.AssignableTo<IJob>())
                        .AsSelf()
                        .WithTransientLifetime()
                    );
            }

            return services;
        }
    }
}