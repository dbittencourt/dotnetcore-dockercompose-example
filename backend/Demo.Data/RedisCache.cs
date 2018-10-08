using Demo.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Data
{
    public class RedisCache : ICache
    {
        public RedisCache(IConfiguration config)
        {
            _redisConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(config.GetConnectionString("CacheConnection"));
            });

            _cache = _redisConnection.Value.GetDatabase();
        }

        public void SetValue(string key, object obj)
        {
            SetValue(key, obj, DEFAULT_TTL);
        }

        public void SetValue(string key, object obj, int ttl)
        {
            var jsonValue = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            _cache.StringSet(key, jsonValue, TimeSpan.FromMinutes(ttl));
        }

        public T GetValue<T>(string key)
        {
            var obj = _cache.StringGet(key);

            if (!obj.IsNull)
                return JsonConvert.DeserializeObject<T>(obj);
            else
                return default(T);
        }

        public T GetValue<T>(string key, Func<T> defaultValueGetter)
        {
            var json = _cache.StringGet(key);
            if (json.IsNull)
            {
                var value = defaultValueGetter();
                SetValue(key, value);
                return value;
            }
            else
                return JsonConvert.DeserializeObject<T>(json);
        }

        public T GetValue<T>(string key, Func<T> defaultValueGetter, int ttl)
        {
            var json = _cache.StringGet(key);
            if (json.IsNull)
            {
                var value = defaultValueGetter();
                SetValue(key, value, ttl);
                return value;
            }
            else
                return JsonConvert.DeserializeObject<T>(json);
        }

        public IEnumerable<T> GetValues<T>(string keyPattern)
        {
            var objs = new List<T>();
            var endpoints = _redisConnection.Value.GetEndPoints();
            var server = _redisConnection.Value.GetServer(endpoints.First());

            if (server != null)
            {
                foreach (var key in server.Keys(pattern: $"{keyPattern}:*"))
                {
                    var json = _cache.StringGet(key);
                    objs.Add(JsonConvert.DeserializeObject<T>(json));
                }
            }

            return objs;
        }

        public void RemoveValue(string key)
        {
            _cache.KeyDelete(key);
        }

        public bool IsKeyPresent(string key)
        {
            var value = _cache.StringGet(key);
            return !value.IsNull;
        }

        public void Clear(string type)
        {
            var endpoints = _redisConnection.Value.GetEndPoints();
            var server = _redisConnection.Value.GetServer(endpoints.First());

            if (server != null)
            {
                foreach (var key in server.Keys(pattern: $"{type}:*"))
                {
                    _cache.KeyDelete(key);
                }
            }
        }

        private Lazy<ConnectionMultiplexer> _redisConnection;
        private IDatabase _cache;
        private int DEFAULT_TTL = 30;
    }
}
