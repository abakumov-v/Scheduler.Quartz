using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Models.Infrastructure;
using NetCore.ConsoleAsWindowsService.Application;
using NLog;
using NLog.LayoutRenderers;
using NLog.Web;

namespace NetCore.ConsoleAsWindowsService
{
    class Program
    {
        private static Logger _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            try
            {
                _logger.Debug($"Init program with args: {string.Join("|", args)}");

                var argsModel = new ConsoleArgumentsModel(args);
                LayoutRenderer.Register("app-type", logEvent => argsModel.IsService ? "win-service" : "console");

                if (argsModel.IsShowHelp)
                {
                    Console.WriteLine(HelpTextGenerator.Generate());
                    return;
                }

                var app = new ConsoleApplication();
                await app.RunAsync(argsModel);
            }
            catch (AggregateException ex)
            {
                var errorMessages = string.Join($"{Environment.NewLine}{Environment.NewLine}",
                    ex.InnerExceptions.Select(e => e.Message));
                var oneException = new Exception(errorMessages);
                _logger.Fatal(oneException, "An unexpected error occurred while starting the application");
                throw oneException;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An unexpected error occurred while starting the application.");
                throw;
            }
            finally
            {
                _logger.Debug("Application shutdown");
                // Ensure to flush and stop internal timers/threads before application-exit
                // (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}
