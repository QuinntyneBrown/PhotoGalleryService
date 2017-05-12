﻿using System;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace PhotoGalleryService.Features.Core
{
    public class RedisCache : Cache
    {
        private static volatile PhotoGalleryService.Features.Core.RedisCache _current = null;
        private IDatabase _database { get; set; }
        private ConnectionMultiplexer _connection { get; set; }
        
        public RedisCache()
        {
            _connection = ConnectionMultiplexer.Connect(RedisCacheConfiguration.Config.ConnectionString);
            _database = _connection.GetDatabase();            
        }
       
        public static RedisCache Current
        {
            get
            {
                if (_current == null)
                    _current = new RedisCache();
                return _current;
            }
        }

        public override void Add(object objectToCache, string key)
        {            
            _database.StringSet(key, JsonConvert.SerializeObject(objectToCache));
        }

        public override void Add<T>(object objectToCache, string key) => Add(objectToCache, key);


        public override void Add<T>(object objectToCache, string key, double cacheDuration)
        {
            _database.StringSet(key, JsonConvert.SerializeObject(objectToCache), TimeSpan.FromMinutes(cacheDuration));
        }

        public override void ClearAll()
        {
            foreach (var endpoint in _connection.GetEndPoints())
            {                
                var server = _connection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }

        public override bool Exists(string key) => _database.KeyExists(key);

        public override T Get<T>(string key)
        {
            RedisValue redisValue = _database.StringGet(key);

            if (redisValue.IsNull)
                return default(T);

            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public override object Get(string key)
        {
            RedisValue redisValue = _database.StringGet(key);

            if (redisValue.IsNull)
                return null;

            return JsonConvert.DeserializeObject(redisValue);
        }

        public override void Remove(string key)
        {
            Add(null, key);
        }
    }
}
