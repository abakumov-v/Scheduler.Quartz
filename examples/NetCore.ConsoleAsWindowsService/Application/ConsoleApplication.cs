using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Core.Models.Infrastructure;
using Microsoft.Extensions.Hosting;
using NetCore.ConsoleAsWindowsService.Application.AsWindowsService;
using NLog;
using NLog.Web;

namespace NetCore.ConsoleAsWindowsService.Application
{
    [ExcludeFromCodeCoverage]
    public class ConsoleApplication
    {
        private readonly Logger _logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
        
        public async Task RunAsync(ConsoleArgumentsModel argsModel)
        {
            var hostBuilderFactory = new HostBuilderFactory(_logger);
            var hostBuilder = hostBuilderFactory.Create(argsModel);
            if (argsModel.IsService)
            {
                await hostBuilder.RunAsServiceAsync();
            }
            else
            {
                using (var host = hostBuilder.Build())
                {
                    host.Start();
                    await host.StopAsync();
                }
            }
        }
    }
}