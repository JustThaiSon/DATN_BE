namespace DATN_Helpers.CacheCore.Interface
{
    public interface IMemoryCacheSystem
    {
        void AddOrUpdate<T>(string key, T value);
        void AddOrUpdate<T>(string key, T value, TimeSpan expiration, TimeSpan? slidingExpiration = null);
        bool TryGetValue<T>(string key, out T value);
        bool Remove(string key);
        void Clear();
    }
}