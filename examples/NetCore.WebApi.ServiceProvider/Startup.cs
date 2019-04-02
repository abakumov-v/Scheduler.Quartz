using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Jobs;
using Core.Models.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using Quartz;
using Scheduler.Quartz;
using Scheduler.Quartz.Ioc.ServiceProvider;

namespace NetCore.WebApi.ServiceProvider
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

            services.AddQuartzScheduler();
            // Use Scrutor for scan dependencies
            services.Scan(scan => scan
                // Register all own custom jobs
                .FromAssemblyOf<ExampleLogJob>()
                .AddClasses(classes => classes.AssignableTo<IJob>())
                .AsSelf()
                .WithTransientLifetime()
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
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
