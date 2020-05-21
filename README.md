## AWSM HotSwap

A Dependency Injection extension library that enables hot-swapping injectable services.


### Round-Robin

Load-balancing can be achieved with:

```
services
    .AddScopedHotSwapService<IStringService>()
        .AddImplementation<ReverseStringService>()
        .AddImplementation<UppercaseStringService>()
        .AddImplementation<TruncatedStringService>()
            .WithRoundRobinSelection();
```

Where each service will be called in turn, spreading the load across different services.

### Auto Failure Resilience

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
            
Auto Failover can be used to swap to a new service when the first begins to fail.

For example, in the above scenario, when `ReverseStringService` starts to throw errors and hits over 100 in a 5 minute window, `UppercaseStringService` will be used from there on.

To publish the failure events, you will need to consume the `IFailoverMonitor<T>` service, like so:

```
public class ReverseStringService : IStringService
{
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
            failMon.RecordFailure<ReverseStringService>();
        }

        return "Something went wrong.";
    }
}
```

See the `Test` project for a full example.