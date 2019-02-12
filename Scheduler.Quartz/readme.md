# Quartz scheduler

Quartz.NET scheduler

## How to use

**Important**: currenty only `Autofac` as IoC-container supported.

### 1. Create project with Quartz jobs

1. Create class library project which will contains all needed Quartz jobs
2. Install package `Scheduler.Quartz`
3. Create Quartz Job - for example, `ExampleJob` 
4. Inherit from abstract class `LoggableJob` (from `Scheduler.Quartz.Abstract` namespace) and implement it

### 2. ASP.NET Core

1. Install Autofac
2. Install packages:
    1. `Quartz.Plugings`
    2. `Scheduler.Quartz`
3. In your `Startup` class:
    1. Add using for `Scheduler.Quartz`:
    2. Then add this method to `Configure` method in `Startup` class:
        ```csharp
        app.StartQuartzScheduler();
        ```
    3. That is all.

P.S. For Quartz.NET you may need file `quartz.config` and `*.xml` file with 
jobs settings - you can see `quartz_example.config` and 
`quartz_jobs_example.xml`.

### 3. Console .NET Core as Windows Service

1. Install Autofac
2. Make your .NET Core 2.x application as Windows Service:
    1. Steve Gordon: "[Running a .NET Core Generic Host App as a Windows Service](https://www.stevejgordon.co.uk/running-net-core-generic-host-applications-as-a-windows-service)"
    2. [GenericHost](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.1)
3. For using scheduled jobs create class that implements `IHostedService` and inject `IScheduleRunner` instance to constructor of this class - for example:
   ```csharp
    public class RunScheduledOperationHostedService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IScheduleRunner _scheduleRunner;

        public RunScheduledOperationHostedService(
            ILogger<RunScheduledOperationHostedService> logger,
            IScheduleRunner scheduleRunner
            )
        {
            _logger = logger;
            _scheduleRunner = scheduleRunner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting scheduler");
            await _scheduleRunner.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping scheduler...");
            await _scheduleRunner.Stop(true);
            _logger.LogDebug("Stopping service");
        }

        public void Dispose()
        {
            
        }
    }
   ```
6. For more info see [examples](../examples)

## Notes

Method `StartQuartzScheduler` will immediatelly launch Quartz.NET scheduler.

For catch any exceptions when trying launch Quartz.NET scheduler wrap your
code in `Configure` method by `try-catch`:
```csharp
public void Configure(IApplicationBuilder app)
{
    try
    {
        // ...
        
        app.StartQuartzScheduler();

        // ...
    }
    catch (Exception ex)
    {
        _logger.LogCritical(ex, "An unexpected error has occurred");
        throw;
    }
}
```