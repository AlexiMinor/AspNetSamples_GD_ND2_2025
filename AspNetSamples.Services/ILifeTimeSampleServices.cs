using AspNetSamples.Services.Abstractions;

namespace AspNetSamples.Services;

public class TransientService : ITransientService
{
    private int x = 0;
    private readonly Guid Id = Guid.NewGuid();
    public int DoSomething()
    {
        x++;
        return x;
    }
}

public class ScopedService : IScopedService
{
    private int x = 0;
    private readonly Guid Id = Guid.NewGuid();

    public int DoSomething()
    {
        x++;
        return x;
    }

}

public class SingletonService : ISingletonService
{
    private int x = 0;
    private readonly Guid Id = Guid.NewGuid();

    public int DoSomething()
    {
        x++;
        return x;
    }

}

public class LifetimeService: ILifeTimeSampleService
{
    private readonly ITransientService _transientService;
    private readonly IScopedService _scopedService;
    private readonly ISingletonService _singletonService;

    public LifetimeService(ITransientService transientService, 
        IScopedService scopedService, 
        ISingletonService singletonService)
    {
        _transientService = transientService;
        _scopedService = scopedService;
        _singletonService = singletonService;
    }

    public (int, int, int) Test()
    {
        var val1 = _transientService.DoSomething();
        var val2 = _scopedService.DoSomething();
        var val3 = _singletonService.DoSomething();
        
        return (val1, val2, val3);
    }
}