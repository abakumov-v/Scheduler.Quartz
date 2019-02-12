SET ServiceName="Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService"
sc stop %ServiceName%
sc delete %ServiceName%