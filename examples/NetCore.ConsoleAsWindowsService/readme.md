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