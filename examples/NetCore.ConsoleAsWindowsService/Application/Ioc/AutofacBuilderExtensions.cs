using System;
using System.Diagnostics.CodeAnalysis;
using Autofac;
using Core.Models.Options;
using Microsoft.Extensions.Options;

namespace NetCore.ConsoleAsWindowsService.Application.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class AutofacBuilderExtensions
    {
        public static void RegisterAppSettings(this ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var appSettingsOptions = context.Resolve<IOptions<AppSettings>>();
                if (appSettingsOptions?.Value == null)
                    throw new ArgumentNullException(nameof(appSettingsOptions));

                if (appSettingsOptions.Value.ConnectionStrings == null)
                    throw new ArgumentNullException(nameof(appSettingsOptions),
                        "Section \"ConnectionStrings\" in configuration file is null");

                return appSettingsOptions.Value;
            }).InstancePerLifetimeScope();
        }

        public static void RegisterConsoleSettings(this ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var consoleSettingsOptions = context.Resolve<IOptions<ConsoleSettings>>();
                if (consoleSettingsOptions?.Value == null)
                    throw new ArgumentNullException(nameof(consoleSettingsOptions));

                return consoleSettingsOptions.Value;
            }).InstancePerLifetimeScope();
        }

        public static void RegisterLoggers(this ContainerBuilder builder)
        {
            
        }

        public static void RegisterBusinessComponents(this ContainerBuilder builder)
        {
            
        }
    }
}