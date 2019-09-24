# Quartz scheduler

Quartz.NET scheduler

## How to use

### 1. Create project with Quartz jobs

1. Create class library project which will contains all needed Quartz jobs.
2. Install package `Scheduler.Quartz`.
2. Create your custom Quartz jobs (for example, `ExampleJob`).
2. Inherit this job:
    1. from abstract class `LoggableJob` or `LoggableJobAsync` from `Scheduler.Quartz.Abstract` namespace;
    1. or just from `IJob` interface of `Quartz` namespace.
2. Implement logic of job.

### 2. Add Quartz scheduler to .NET Core app (GenericHost/WebHost)

1. Install appropriate IOC-container - all possible IOC-container are listed [here](../readme.md)
2. Install packages:
    1. `Scheduler.Quartz`
    2. `Quartz.Plugings` - only if your Quartz configured from *.xml file ("file configured scheduler")
2. In your `Startup` class add the Quartz scheduler:
    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        // ...

        services.AddQuartz(new QuartzSchedulerOptions()
        {
            // For file configured scheduler:
            UseFileConfig = true,
            
            // For in memory configured Quartz scheduler specify type of implementation of ISchedulerRunnerFactory:
            // InMemorySchedulerFactoryType = typeof(MemorySchedulerFactory),

            // Specify type of your 1 custom Qartz job for automatically
            // registering all jobs from type's assembly. 
            // Otherwise you need register your custom jobs manually.
            OneOfCustomQuartzJobsType = typeof(ExampleLogJob)
        });
    }
    ```
2. Create implementation of `IHostedService` (read more about hosted services on [MS docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host)) and inject the `ISchedulerRunnerFactory` to constructor - for example:
    ```csharp
    public class QuartzSchedulerHostedService : IHostedService
    {
        private readonly IScheduleRunner _scheduler;

        public QuartzSchedulerHostedService(ISchedulerRunnerFactory schedulerRunnerFactory)
        {
            _scheduler = schedulerRunnerFactory.Create();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // You can configure Quartz memory-jobs here:
            // await _scheduler.ScheduleRepeatableJob<ExampleLogJobAsync>(60);

            await _scheduler.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Stop();
        }
    }
    ```
2. Configure this hosted service in your `GenericHost` (or `WebHost`) in `Program.cs`:
    ```csharp
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            // some configurations of .NET Core Host
            .ConfigureServices((context, services) =>
            {            
                services.AddSingleton<IHostedService, QuartzSchedulerHostedService>();
            })
            // some other configurations
            ;
    ```
2. That is all - when the .NET Core host will be start then all implementations of `IHostedService` will be start too.

P.S. For configuring Quartz via config files you may need next files:
* `quartz.config` with settings of Quartz scheduler;
* `*.xml` file with jobs settings - you can see `quartz_example.config` and `quartz_jobs_example.xml`.

### Examples

See inside [examples](../examples) folder.