namespace AspNetSamples.Services.Abstractions;

public interface ICacheService
{
    T Get<T>(string key);
    bool Set<T>(string key, T value, DateTimeOffset expirationTime);
    bool Remove(string key);
}