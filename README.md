# Scheduler.Quartz
Quartz.NET runner for .NET Standard 2.0 with support of ASP.NET Core.

Source article: https://tech.trailmax.info/2013/07/quartz-net-in-azure-with-autofac-smoothness/

Package|Last version
-|-
Scheduler.Quartz|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Scheduler.Quartz.svg)](https://www.nuget.org/packages/Scheduler.Quartz/)
Scheduler.Quartz.Ioc.Autofac|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Scheduler.Quartz.Ioc.Autofac.svg)](https://www.nuget.org/packages/Scheduler.Quartz.Ioc.Autofac/)
Scheduler.Quartz.Ioc.ServiceProvider|[![NuGet Pre Release](https://img.shields.io/nuget/vpre/Scheduler.Quartz.Ioc.ServiceProvider.svg)](https://www.nuget.org/packages/Scheduler.Quartz.Ioc.ServiceProvider/)

## Builds

Branch|Build status
-|-
dev|[![Build status](https://ci.appveyor.com/api/projects/status/34jm9uvmxlnjx32n/branch/dev?svg=true)](https://ci.appveyor.com/project/Valeriy1991/scheduler-quartz/branch/dev)

AppVeyor Nuget project feed: 
https://ci.appveyor.com/nuget/scheduler-quartz-47b9607klagb


## Include packages:

1. `Scheduler.Quartz` - Quartz.NET scheduler;
2. `Scheduler.Quartz.Ioc.Autofac` - dependency registration components for Autofac;
2. `Scheduler.Quartz.Ioc.ServiceProvider` - dependency registration components for ServiceProvider (.NET Core built-in Ioc container).


## Features

1. Support .NET Standard 2.0 (and, of cource, .NET Core 2.x);
1. Has 2 types of Quartz scheduler:
    1. file configured;
    1. in-memory configured
1. Can adding via `ConfigureServices` in `Startup.cs`;
1. Can started via implementation of `IHostedService` of `IHost` (or `IWebHost`);
1. Has built-in:
    1. simple `QuartzSchedulerHostedService` hosted service which only starts the Quartz scheduler;
    1. Quartz jobs: `LoggableJob` and `LoggableJobAsync` which uses the `ILogger<T>`.

## Changelog

[changelog](./changelog.md)

## How to use it?

1. See `readme` inside [Scheduler.Quartz](Scheduler.Quartz) project.
2. More about using with `IServiceProvider` inside [Scheduler.Quartz.Ioc.ServiceProvider](Scheduler.Quartz.Ioc.ServiceProvider) project.
2. Need examples? See in [examples](examples) folder.