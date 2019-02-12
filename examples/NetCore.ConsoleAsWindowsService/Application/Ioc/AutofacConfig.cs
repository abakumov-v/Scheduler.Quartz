using System.Diagnostics.CodeAnalysis;
using Autofac;

namespace NetCore.ConsoleAsWindowsService.Application.Ioc
{
    [ExcludeFromCodeCoverage]
    public static class AutofacConfig
    {
        public static void RegisterApplicationDependencies(this ContainerBuilder builder)
        {
            builder.RegisterModule(new QuartzAutofacModule());
            builder.RegisterAppSettings();
            builder.RegisterConsoleSettings();
            builder.RegisterLoggers();
            builder.RegisterBusinessComponents();
        }
    }
}