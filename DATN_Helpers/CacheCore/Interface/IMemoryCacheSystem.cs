using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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