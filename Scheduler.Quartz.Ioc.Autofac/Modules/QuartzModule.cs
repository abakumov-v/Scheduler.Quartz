using System;
using Autofac;
using Quartz;
using Quartz.Spi;
using Scheduler.Quartz.Abstract;
using QuartzJobs = Quartz.Job;

namespace Scheduler.Quartz.Ioc.Autofac.Modules
{
    /// <inheritdoc />
    /// <summary>
    /// Autofac dependecy registering module for Quartz schedule runner
    /// </summary>
    public class QuartzModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<QuartzScheduleRunner>()
                .As<IScheduleRunner>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AutofacJobFactory>()
                .As<IJobFactory>()
                .InstancePerLifetimeScope();

            // Register all Scheduler.Quartz built-in jobs
            builder.RegisterTypes(GetTypes(typeof(QuartzScheduleRunner)))
                .Where(t => t != typeof(IJob) && typeof(IJob).IsAssignableFrom(t))
                .AsSelf()
                .InstancePerLifetimeScope();

            // Register all Quartz built-in jobs
            // Note: since release 3.0.0-beta1 all ***Job were moved to other separate 
            // nuget package - Quartz.Jobs, but the namespace of them remains the same
            builder.RegisterTypes(GetTypes(typeof(QuartzJobs.FileScanJob)))
                .Where(t => t != typeof(IJob) && typeof(IJob).IsAssignableFrom(t))
                .AsSelf()
                .InstancePerLifetimeScope();
        }

        protected Type[] GetTypes(Type targetType)
        {
            return targetType.Assembly.GetTypes();
        }
    }
}
