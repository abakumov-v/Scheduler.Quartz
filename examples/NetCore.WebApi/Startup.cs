using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core.Ioc.Autofac;
using Core.Models.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using Scheduler.Quartz;

namespace NetCore.WebApi
{
    public class Startup
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _logger.Trace($"EnvironmentName is: {env.EnvironmentName}");
            _logger.Trace($"ContentRootPath is: {env.ContentRootPath}");
            
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterApplicationDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Take connection string for database from appsettings.json
            var logDb = Configuration.GetSection("ConnectionStrings:LogDb").Value;
            LogManager.Configuration.Variables["connectionString"] = logDb;

            try
            {
                app.StartQuartzScheduler();
            }
            catch (AggregateException ex)
            {
                var errorMessages = string.Join($"{Environment.NewLine}{Environment.NewLine}",
                    ex.InnerExceptions.Select(e => e.Message));
                var oneException = new Exception(errorMessages);
                _logger.Fatal(oneException, "An unexpected error occurred while starting the Quartz scheduler");
                throw oneException;
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "An unexpected error occurred while starting the Quartz scheduler");
                throw;
            }
        }
    }
}
