# Quartz scheduler

Extensions for IOC-containers.

Supporting:

* Autofac
* ... 

---

## How to use

### Autofac

1. Package already have Autofac-module named `QuartzModule` that register all required
dependecies (such as `IScheduleRunner`, `IJobFactory` and `IJob`).
2. If you need a register some custom your jobs, create your own Autofac `Module`
and inherit it from `QuartzModule`, for example:
    ```csharp
    /// <inheritdoc />
    /// <summary>
    /// Dependency registration module for Quartz jobs
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class QuartzAutofacModule : QuartzModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // Register all our custom Quartz jobs
            builder.RegisterTypes(GetTypes(typeof(MyCustomJob)))
                .Where(t => t != typeof(IJob) && typeof(IJob).IsAssignableFrom(t))
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
    ```
3. Call method `RegisterModule` (of your `ContainerBuilder` instance) with 
new instance of `QuartzAutofacModule` class:
    ```csharp
    [ExcludeFromCodeCoverage]
    public static class AutofacConfig
    {
        public static void RegisterApplicationDependencies(this ContainerBuilder builder)
        {
            // Register our custom module:
            builder.RegisterModule(new QuartzAutofacModule());
            // ... register other dependencies
        }
    }
    ```
4. That is all.