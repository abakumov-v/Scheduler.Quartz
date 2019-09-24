CHANGELOG:

### 1.0.2

Changes:

1. mark `AddQuartzScheduler` extension method for `IServiceCollection` as **obsolete** - use the new `AddQuartz()` with new `QuartzSchedulerOptions` inside `ConfigureServices()` in `Startup.cs`;
2. mark extension methods `StartQuartzScheduler()` of `IApplicationBuilder` and `IServiceProvider` as **obsolete** - you must start the scheduler via `IHostedService` ([example](../examples/NetCore.WebApi.ServiceProvider.InMemoryConfig/Extensions/QuartzSchedulerHostedService.cs)). This methods will removed in future;
2. in `IScheduleRunner`:
    1. union both methods `Stop()` and `Stop(bool isNeedWaitForJobsToComplete)` in one method with default agrument;
    1. remove public method `SetJobFactory` because it is unecessary;

New:

1. method `UseQuartz()` as `IWebHost` extension;
2. method `UseQuartz()` as `IHost` extension;
2. method `AddQuartz()` as `IServiceCollection` extension;
2. `QuartzSchedulerOptions` as options for new `AddQuartz()` method;
2. add support of InMemory-configuration for Quartz built-in `IScheduler`;
2. `IScheduleRunner` - method `ScheduleRepeatableJob()`;
2. `ISchedulerRunnerFactory` - factory for scheduler with 2 implementations:
    1. `FileSchedulerRunnerFactory` for file cofigured scheduler;
    1. `BaseMemorySchedulerRunnerFactory` for in-memory configured scheduler. You need to create your custom memory scheduler runner factory that inherits from `BaseMemorySchedulerRunnerFactory` and has some Quartz properties in ctor, for example:
    ```csharp
    public class MemorySchedulerFactory : BaseMemorySchedulerRunnerFactory
    {
        public MemorySchedulerFactory(ILoggerFactory loggerFactory, IJobFactory jobFactory)
            : base(loggerFactory, jobFactory)
        {
            SchedulerSettings.Add("quartz.scheduler.instanceName", "ExampleQuartzScheduler");
            SchedulerSettings.Add("quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz");
            SchedulerSettings.Add("quartz.threadPool.threadCount", "3");
        }
    }
    ```