using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Cache
{
    public class GenericRedisService<T> : IGenericRedisService<T> where T : class
    {
        private readonly IConfiguration configuration;
        private readonly IDistributedCache distributedCache;
        private readonly Lazy<ConnectionMultiplexer> redisConn;

        public GenericRedisService(IConfiguration configuration, IDistributedCache distributedCache)
        {
            this.configuration = configuration;
            this.distributedCache = distributedCache;

            var redisConnectionString = configuration.GetSection("Redis").Value;
            redisConn = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisConnectionString));
        }

        private ConnectionMultiplexer Connection => redisConn.Value;

        public async Task<T> GetByIdAsync(Guid key, CancellationToken cancellationToken)
        {
            var keyData = $"{typeof(T).Name.ToLower()}:{key}";
            var data = await distributedCache.GetStringAsync(keyData);

            if (data is not null)
            {
                T? entity = JsonSerializer.Deserialize<T>(data);
                return entity;
            }
                
            return null;
        }

        public async Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            bool control = await EntityExistsAsync(key, cancellationToken);
            if (!control)
                throw new Exception($"{typeof(T).Name} not found in cache!");

            var keyData = $"{typeof(T).Name.ToLower()}:{key}";
            await distributedCache.RemoveAsync(keyData);
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            IDatabase db = Connection.GetDatabase();
            var server = Connection.GetServer(Connection.GetEndPoints().First());

            RedisKey[] keys = server.Keys(pattern: $"{typeof(T).Name.ToLower()}:*").ToArray();
            List<T> entities = new List<T>();

            foreach (RedisKey keyData in keys)
            {
                var data = await db.StringGetAsync(keyData);
                if (data.HasValue)
                {
                    T? entity = JsonSerializer.Deserialize<T>(data);
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public async Task SetAsync(T entity, Guid key, CancellationToken cancellationToken)
        {
            bool control = await EntityExistsAsync(key, cancellationToken);
            if (control)
                throw new Exception($"{typeof(T).Name} found in cache!");

            var keyData = $"{typeof(T).Name.ToLower()}:{key}";
            var serializedData = JsonSerializer.Serialize(entity);
            await distributedCache.SetStringAsync(keyData, serializedData);
        }

        public async Task UpdateAsync(T entity, Guid key, CancellationToken cancellationToken)
        {
            var control = await EntityExistsAsync(key, cancellationToken);
            if (!control)
                throw new Exception($"{typeof(T).Name} not found in cache!");

            var keyData = $"{typeof(T).Name.ToLower()}:{key}";
            await distributedCache.RemoveAsync(keyData);

            var serializedData = JsonSerializer.Serialize(entity);
            await distributedCache.SetStringAsync(keyData, serializedData);
        }

        public async Task<bool> EntityExistsAsync(Guid key, CancellationToken cancellationToken)
        {
            var keyData = $"{typeof(T).Name.ToLower()}:{key}";
            var data = await distributedCache.GetStringAsync(keyData);
            return data != null;
        }
    }
}
