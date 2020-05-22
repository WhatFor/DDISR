## AWSM HotSwap

A Dependency Injection extension library that enables hot-swapping injectable services.

### Levels

Services can be registered at any DI level, i.e. `Scoped`, `Transient` or `Singleton`.

```cs
// Scoped
services
    .AddScopedHotSwapService<IStringService>()
        .AddImplementation<ReverseStringService>()
        .AddImplementation<UppercaseStringService>();

// Transient
services
    .AddTransientHotSwapService<IStringService>()
        .AddImplementation<ReverseStringService>()
        .AddImplementation<UppercaseStringService>();

// Singleton
services
    .AddSingletonHotSwapService<IStringService>()
        .AddImplementation<ReverseStringService>()
        .AddImplementation<UppercaseStringService>();
```

### Round-Robin

Load-balancing can be achieved with:

```cs
services
    .AddScopedHotSwapService<IStringService>()
        .AddImplementation<ReverseStringService>()
        .AddImplementation<UppercaseStringService>()
        .AddImplementation<TruncatedStringService>()
            .WithRoundRobinSelection();
```

Where each service will be called in turn, spreading the load across different services.

### Auto Failure Resilience

```cs
services
    .AddScopedHotSwapService<IStringService>()
        .AddImplementation<ReverseStringService>()
        .AddImplementation<UppercaseStringService>()
        .AddImplementation<TruncatedStringService>()
            .WithAutoRecovery(o =>
            {
                o.ErrorCount = 100;
                o.ErrorWindow = TimeSpan.FromMinutes(5);
            });
```
            
Auto Failover can be used to swap to a new service when the current begins to fail.

For example, in the above scenario, when `ReverseStringService` starts to throw errors and hits over 100 in a 5 minute window, `UppercaseStringService` will be used from there on.

To publish the failure events, you will need to consume the `IFailoverMonitor<T>` service, like so:

```cs
public class ReverseStringService : IStringService
{
    // IFailoverMonitor<TInterface> where TInterface is your interface
    private readonly IFailoverMonitor<IStringService> failMon;

    public ReverseStringService(IFailoverMonitor<IStringService> failMon)
    {
        this.failMon = failMon;
    }

    public string FormatString(string input)
    {
        try
        {
            throw new Exception();
        }
        catch
        {
            // RecordFailure<TImpl> where TImpl is the implementation to throw the error
            // TImpl will extend from the above TInterface,
            // and align with the configuration from Startup.cs
            failMon.RecordFailure<ReverseStringService>();
        }

        return "Something went wrong.";
    }
}
```

### Manual Management API [Currently WIP]

In scenarios where manual intervention is needed, an API is available through middleware.

This can be useful for binding the different services to an admin dashboard for example, where an admin user may wish to manually override the current service.

Register the API like so:

```cs
app.UseHotSwapServices(o =>
{
    // Default: /api/hot-swap
    o.Endpoint = "/my-services-endpoint";
});
```

#### Service Summary

Calling the `Endpoint` configured will return a summary of the services and their current state. [WIP]

#### Manual Switching

Passing an ID parameter to the service can allow you to switch services. The Summary will return unique GUID Ids for each service. For example, `/api/hot-swap/d131bc24-a7d6-430e-8def-1b22fce4cbf7`. [WIP]



See the `Examples` project for a full example, or the `Tests` project for some registration examples.