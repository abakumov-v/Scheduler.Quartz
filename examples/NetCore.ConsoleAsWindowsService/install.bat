:: Output current codepage:
FOR /F "tokens=* USEBACKQ" %%F IN (`chcp`) DO (
SET Encoding=%%F
)
ECHO "Current code page: %Encoding%"
:: Set service variables
SET ServiceName="Scheduler.Quartz_Example_NetCore.ConsoleAsWindowsService"
SET ApplicationPath="%CD%\NetCore.ConsoleAsWindowsService.exe --service"
:: Create Windows-service with autostart
sc create %ServiceName% binPath= %ApplicationPath% start= auto
:: Set UTF-8 current codepage for show Russian symbols with no problem in description of service
chcp 65001
SET Description="Example of Windows service for Quartz scheduler runner. See more: https://github.com/Valeriy1991/Scheduler.Quartz"
sc description %ServiceName% %Description%
sc start %ServiceName%