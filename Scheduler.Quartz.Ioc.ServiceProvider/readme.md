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
2. In your `Startup.cs` add Quartz schduler:
    ```csharp
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
		// ...
        services.AddQuartzScheduler();
    }
    ```