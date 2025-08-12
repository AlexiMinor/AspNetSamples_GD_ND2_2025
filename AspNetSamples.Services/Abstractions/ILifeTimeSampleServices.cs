namespace AspNetSamples.Services.Abstractions;

public interface ITransientService
{
    public int DoSomething();
}

public interface IScopedService
{
    public int DoSomething();

}

public interface ISingletonService
{
    public int DoSomething();

}

public interface ILifeTimeSampleService
{
    public (int, int, int) Test();
}