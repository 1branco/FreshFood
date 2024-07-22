using Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cache.Services
{
    public class CacheService : ICacheService
    {
        private readonly DistributedCacheEntryOptions _cacheOptions;
        private readonly IDatabase _db;
        private readonly IConfiguration _config;

        public CacheService(IConfiguration config)
        {
            _cacheOptions = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                AbsoluteExpiration = DateTime.Now.AddMinutes(30)
            };   
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,password=J_p#A.b1997");
            _db = redis.GetDatabase();
        }

        public T Get<T>(string key)
        {
            var result = _db.StringGet(key);
            if(!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default;
        }

        public void Refresh(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            if (_db.KeyExists(key))
            {
                _db.KeyDelete(key);
            }
        }

        public bool Set<T>(string key, T value)
        {
            return _db.StringSet(key, JsonConvert.SerializeObject(value), _cacheOptions.AbsoluteExpirationRelativeToNow);            
        }
    }
}
