# Quartz scheduler - example

Console application for Quartz.NET scheduler that can started as Windows service.

## How to run

### As Console application

1. Clone
2. Build
3. Open cmd/Powershell
4. Run `NetCore.ConsoleAsWindowsService.exe` without any arguments
5. See help info
6. Run `NetCore.ConsoleAsWindowsService.exe` with `--start=true` argument
7. The `ExampleJob` was ran at 1 time

### As Windows service

1. Clone
2. Build 
3. Publish with `FolderProfile`
4. Go to publish folder
5. Open cmd/Powershell as Administrator
6. Run `install.bat`
7. Go to Control panel - Administration - Services
8. Find the `Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService` service in list
9. See logs in `C:\temp\nlog\Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService\` - the `ExampleJob` will be executed according to the schedule from `quartz_jobs.xml`

## Logs example output:

### Console

```
||2019-02-12 12:48:31.3724||DEBUG|NetCore.ConsoleAsWindowsService.Program|Init program with args: --start=true 
|console|2019-02-12 12:48:32.6404||DEBUG|NetCore.ConsoleAsWindowsService.Application.ConsoleApplication|pathToContentRoot: D:\Projects\Git\GitHub\My\Scheduler.Quartz\examples\NetCore.ConsoleAsWindowsService\bin\Debug\netcoreapp2.2\win7-x64\ 
|console|2019-02-12 12:48:34.6674||DEBUG|NetCore.ConsoleAsWindowsService.Application.ConsoleApplication|EnvironmentName: Development 
|console|2019-02-12 12:48:34.6804||DEBUG|NetCore.ConsoleAsWindowsService.Application.ConsoleApplication|ContentRootPath: D:\Projects\Git\GitHub\My\Scheduler.Quartz\examples\NetCore.ConsoleAsWindowsService\bin\Debug\netcoreapp2.2\win7-x64\ 
|console|2019-02-12 12:48:37.2354||DEBUG|NetCore.ConsoleAsWindowsService.HostedServices.RunOneTimeOperationHostedService|Starting 
|console|2019-02-12 12:48:37.2534||DEBUG|NetCore.ConsoleAsWindowsService.HostedServices.RunOneTimeOperationHostedService|Do some useful work... 
|console|2019-02-12 12:48:39.2684||DEBUG|NetCore.ConsoleAsWindowsService.HostedServices.RunOneTimeOperationHostedService|Useful work was completed successfully 
|console|2019-02-12 12:48:40.7304||DEBUG|NetCore.ConsoleAsWindowsService.HostedServices.RunOneTimeOperationHostedService|Stopping 
|console|2019-02-12 12:48:47.5394||DEBUG|NetCore.ConsoleAsWindowsService.Program|Application shutdown 
```

### Windows service

```
||2019-02-12 12:46:21.6544||DEBUG|NetCore.ConsoleAsWindowsService.Program|Init program with args: --service 
|win-service|2019-02-12 12:46:21.8654||DEBUG|NetCore.ConsoleAsWindowsService.Application.ConsoleApplication|pathToContentRoot: D:\Projects\Git\GitHub\My\Scheduler.Quartz\examples\NetCore.ConsoleAsWindowsService\bin\Release\netcoreapp2.2\win7-x64\ 
|win-service|2019-02-12 12:46:21.8874||DEBUG|NetCore.ConsoleAsWindowsService.Application.ConsoleApplication|EnvironmentName: Development 
|win-service|2019-02-12 12:46:21.8934||DEBUG|NetCore.ConsoleAsWindowsService.Application.ConsoleApplication|ContentRootPath: D:\Projects\Git\GitHub\My\Scheduler.Quartz\examples\NetCore.ConsoleAsWindowsService\bin\Release\netcoreapp2.2\win7-x64\ 
|win-service|2019-02-12 12:46:22.5464||DEBUG|NetCore.ConsoleAsWindowsService.HostedServices.RunScheduledOperationHostedService|Starting scheduler 
|win-service|2019-02-12 12:46:22.8424||INFO|Core.Jobs.ExampleJob|[636855615827954670] ExampleJob - job started 
|win-service|2019-02-12 12:46:22.8424||INFO|Core.Jobs.ExampleJob|Job will doing some work... 
|win-service|2019-02-12 12:46:24.8564||INFO|Core.Jobs.ExampleJob|Job complete work successfully 
|win-service|2019-02-12 12:46:24.8564||INFO|Core.Jobs.ExampleJob|[636855615827954670] ExampleJob - job finished 
```