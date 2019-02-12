# Quartz scheduler - example

Console application for Quartz.NET scheduler that can started as Windows service.

## How to run

1. Clone
2. Build

### As Console application

1. Open cmd/Powershell
2. Run `NetCore.ConsoleAsWindowsService.exe` without any arguments
3. See help info
4. Run `NetCore.ConsoleAsWindowsService.exe` with `--start=true` argument
5. The `ExampleJob` was ran at 1 time

### As Windows service

1. Clone
2. Build and Publish
3. Go to publish folder
4. Open cmd/Powershell as Administrator
5. Run `install.bat`
6. Go to Control panel - Administration - Services
7. Find the `Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService` service in list
8. See logs in `C:\temp\nlog\Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService\` - the `ExampleJob` will be executed according to the schedule from `quartz_jobs.xml`