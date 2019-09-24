CHANGELOG:

### 1.0.2

1. new: method `UseQuartz()` as `IWebHost` extension;
2. new: method `UseQuartz()` as `IHost` extension;
2. new: method `AddQuartz()` as `IServiceCollection` extension;
2. new: `QuartzSchedulerOptions` as options for new `AddQuartz()` method;
2. **beraking change (!)**: mark `AddQuartzScheduler` extension method for `IServiceCollection` as **obsolete** - use the new `AddQuartz()` method (with new `QuartzSchedulerOptions`) instead;
2. new: add support of InMemory-configuration for Quartz built-in `IScheduler`.