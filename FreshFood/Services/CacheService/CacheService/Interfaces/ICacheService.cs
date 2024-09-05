namespace CacheService.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);

        bool Set<T>(string key, T value);

        void Remove(string key);

        void Refresh(string key, object value);
    }
}
