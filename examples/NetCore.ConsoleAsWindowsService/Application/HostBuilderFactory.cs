using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Models.Infrastructure;
using Core.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetCore.ConsoleAsWindowsService.Application.Ioc;
using NetCore.ConsoleAsWindowsService.HostedServices;
using NLog;
using NLog.Extensions.Logging;

namespace NetCore.ConsoleAsWindowsService.Application
{
    [ExcludeFromCodeCoverage]
    public class HostBuilderFactory
    {
        private readonly Logger _logger;

        public HostBuilderFactory(Logger logger)
        {
            _logger = logger;
        }

        public IHostBuilder Create(ConsoleArgumentsModel argsModel)
        {
            var isService = argsModel.IsService;

            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var pathToContentRoot = AppDomain.CurrentDomain.BaseDirectory;
            _logger.Debug($"pathToContentRoot: {pathToContentRoot}");
            if (isService)
            {
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            return new HostBuilder()
                    .UseEnvironment(aspNetCoreEnvironment)
                    .UseContentRoot(pathToContentRoot)
                    .ConfigureAppConfiguration((hostContext, configApp) =>
                    {
                        _logger.Debug($"EnvironmentName: {hostContext.HostingEnvironment.EnvironmentName}");
                        _logger.Debug($"ContentRootPath: {hostContext.HostingEnvironment.ContentRootPath}");

                        configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        configApp.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json", optional: false,
                            reloadOnChange: true);
                        configApp.AddEnvironmentVariables();
                        configApp.AddCommandLine(argsModel.Args);
                    })
                    .ConfigureLogging((hostContext, configLogging) =>
                    {
                        configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                        configLogging.AddConsole();
                        configLogging.AddDebug();
                        configLogging.AddNLog();

                        // Read connection string to database from appsettings.json
                        var logDb = hostContext.Configuration.GetSection("ConnectionStrings:LogDb").Value;
                        LogManager.Configuration.Variables["connectionString"] = logDb;
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddLogging();
                        services.Configure<AppSettings>(hostContext.Configuration);
                        services.Configure<ConsoleSettings>(hostContext.Configuration);

                        if (isService)
                        {
                            services.AddSingleton<IHostedService, RunScheduledOperationHostedService>();
                        }
                        else
                        {
                            services.AddSingleton<IHostedService, RunOneTimeOperationHostedService>();
                        }
                    })
                    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                    .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
                    {
                        builder.RegisterApplicationDependencies();
                    })
                // This remove any other logging services (like Console, Debug, etc.) and add only NLogLogger
                //.UseNLog()
                ;
        }
    }
}