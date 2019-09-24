# Quartz scheduler

Extensions for .NET Core built-in IOC-container (ServiceProvider) for registering Scheduler.Quartz dependencies.

---

## How to use

1. Install this package:
	1. Package Manager Console
	```powershell
	Install-Package Scheduler.Quartz.Ioc.ServiceProvider
	```
	2. .NET Core CLI
	```powershell
	dotnet add package Scheduler.Quartz.Ioc.ServiceProvider
	```
2. Start Quartz scheduler by one of next ways:
   1. [**prefer way, because you can configure the IHostedService as you want**] When you configure your Host (WebHost or GenericHost), you need register the `QuartzSchedulerHostedService` like this:   
    ```csharp
    // Program.cs
    public static IWebHostBuilder CreateWebHostBuilder(string[] args) => 
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IHostedService, QuartzSchedulerHostedService>();
                // Or you can write another IHostedService with your custom logic and register it
                services.AddSingleton<IHostedService, MyCustomQuartzSchedulerHostedService>();
            })
            // ... other configurations ...
            ;
    ```
    `QuartzSchedulerHostedService.cs` is very simple:
    ```csharp
    public class QuartzSchedulerHostedService : IHostedService
    {
        private readonly IScheduleRunner _scheduler;

        public QuartzSchedulerHostedService(ISchedulerRunnerFactory schedulerRunnerFactory)
        {
            _scheduler = schedulerRunnerFactory.Create();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Configure here your repeatable jobs (don't forget add the async keyword)
            // await _scheduler.ScheduleRepeatableJob<ExampleLogJobAsync>(60);

            return _scheduler.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _scheduler.Stop();
        }
    }
    ```
    Read more about `IHostedService` on [.NET Core 2.2 MS docs](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.2#introduction).

    2. [**may be, it is not very nice...**] Use the extension method `UseQuartz()` of `IWebHost`/`IHost`:
    ```csharp
    var host = CreateWebHostBuilder(args).Build();
    host.UseQuartz(runner =>
    {
        // Start some your repeatable jobs
        runner.ScheduleRepeatableJob<ExampleLogJobAsync>(60)
            .ConfigureAwait(false).GetAwaiter().GetResult();
        // or start any other jobs: 
    });
    host.Run();
    ```