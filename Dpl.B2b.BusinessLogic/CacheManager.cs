using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Dpl.B2b.BusinessLogic
{
    
    /// <summary>
    /// Utility Klasse für das Speichern und laden in die Redis Database
    /// Einfaches Konstrukt zum verwenden der Azure Redis Database -> StackExchange.Redis
    /// Dient als Ausgangspunkt wenn nicht über den DistributedCache Klasse gearbeitet wird
    /// </summary>
    /// <remarks>https://docs.microsoft.com/de-de/azure/azure-cache-for-redis/cache-dotnet-core-quickstart</remarks>
    public class CacheManager : ICacheManager
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection = null;
        private static IDatabase _database;
        private static ConnectionMultiplexer Connection => _lazyConnection.Value;
        public CacheManager(IOptions<AzureRedisConfig> config)
        {
            _config = config.Value;
        }

        public AzureRedisConfig _config { get; set; }

        public class AzureRedisConfig
        {
            public string ConnectionString { get; set; }
        }

        public void SetUpCacheManager()
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            { 
                string connectionString = _config.ConnectionString;
                var redisConfig = ConfigurationOptions.Parse(connectionString);
                redisConfig.AsyncTimeout = 15000;
                return ConnectionMultiplexer.Connect(redisConfig);
            });

            _database = Connection.GetDatabase();
        }

        public async Task<bool> Store<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(nameof(key));
            key = $"user_{key}";
            string val = JsonConvert.SerializeObject(value);
            return await _database.StringSetAsync(key, JsonConvert.SerializeObject(value), new TimeSpan(0, 12, 0, 0, 0));
        }
        
        public async Task<T> Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(nameof(key));
            key = $"user_{key}";
            try
            {
                var result = await _database.StringGetAsync(key);
                if (!result.HasValue)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch(Exception e)
            {
                return default(T);
            }
        }
    }

    public interface ICacheManager
    {
        public void SetUpCacheManager();
        public Task<bool> Store<T>(string key, T value);
        public Task<T> Get<T>(string key);
    }
}