# Quartz scheduler - example

ASP.NET Core Web API application for Quartz.NET scheduler.

## How to run

1. Clone
2. Build 
3. Change launch mode from `IIS Express` to Kestrel (`NetCore.WebApi`)
4. Run
9. See logs in Console output and also in `C:\temp\nlog\Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService\` - the `ExampleJob` will be executed according to the schedule from `quartz_jobs.xml`