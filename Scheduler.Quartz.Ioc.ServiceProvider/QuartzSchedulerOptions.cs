using System;
using Scheduler.Quartz.Factories;

namespace Scheduler.Quartz.Ioc.ServiceProvider
{
    public class QuartzSchedulerOptions
    {
        /// <summary>
        /// Source for Quartz jobs configurations. Default value - InMemory
        /// </summary>
        public ConfigSource ConfigSource { get; set; } = ConfigSource.InMemory;
        /// <summary>
        /// Type of implementation of <see cref="BaseMemorySchedulerRunnerFactory"/> memory scheduler factory.
        /// Should be specified, if <see cref="ConfigSource"/> is InMemory.
        /// </summary>
        public Type InMemorySchedulerFactoryType { get; set; }
        /// <summary>
        /// Type of Quartz job from assembly which contains all quartz jobs for registering in ServiceProvider.
        /// If not specified then you MUST register all your Quartz jobs manually
        /// </summary>
        public Type OneOfCustomQuartzJobsType { get; set; }
    }
}