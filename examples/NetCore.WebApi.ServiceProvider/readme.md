# Quartz scheduler - example

ASP.NET Core Web API application for Quartz.NET scheduler which registered in ServiceProvider.

## How to run

1. Clone
2. Build 
3. Change launch mode from `IIS Express` to Kestrel (`NetCore.WebApi.ServiceProvider`)
4. Run
9. See logs in Console output and also in `C:\temp\nlog\Scheduler.Quartz_Example_NetCore.WebApi.ServiceProvider\` - the `ExampleLogJob`, `ExampleLogJobAsync`, `LongRunningJob` and `LongRunningJobAsync` will be executed according to schedule settings from `quartz_jobs.xml`