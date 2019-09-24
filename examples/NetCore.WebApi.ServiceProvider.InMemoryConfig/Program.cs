using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Jobs;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Quartz;
using Scheduler.Quartz.Ioc.ServiceProvider;
using Scheduler.Quartz.Ioc.ServiceProvider.Extensions;
using QuartzSchedulerHostedService = NetCore.WebApi.ServiceProvider.InMemoryConfig.Extensions.QuartzSchedulerHostedService;

namespace NetCore.WebApi.ServiceProvider.InMemoryConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("Init main");

                var host = CreateWebHostBuilder(args).Build();
                // Example 1 - extension method
                //host.UseQuartz(runner =>
                //{
                //    runner.ScheduleRepeatableJob<ExampleLogJobAsync>(60)
                //        .ConfigureAwait(false).GetAwaiter().GetResult();
                //});

                host.Run();
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices((context, services) =>
                {
                    // Example 2 - use HostedService (prefer)
                    services.AddSingleton<IHostedService, QuartzSchedulerHostedService>();
                })
                .UseNLog();
    }
}
