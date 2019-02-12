namespace NetCore.ConsoleAsWindowsService.Application
{
    public class HelpTextGenerator
    {
        public static string Generate()
        {
            return $@"
Console application as Windows Service for Quartz scheduler runner.

Available command line arguments:
    --service     - Run as Windows-service.

    --start=true  - Run as Console application.
";
        }
    }
}