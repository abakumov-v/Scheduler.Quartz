# Quartz scheduler

Quartz.NET scheduler

## How to use

In your `Startup` class:
1. Install package and add using:
    ```csharp
    using Scheduler.Quartz;
    ```
2. Then add this method to `Configure` method in `Startup` class:
    ```csharp
    app.StartQuartzScheduler();
    ```
2. That is all!

P.S. For Quartz.NET you may need file `quartz.config` and `*.xml` file with 
jobs settings - you can see `quartz_example.config` and 
`quartz_jobs_example.xml`.

## Short description

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