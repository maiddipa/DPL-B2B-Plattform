namespace Dpl.B2b.Contracts.Options
{
    public class CacheOptions
    {
        public const string Cache = "Cache";
        public DistributedCacheOptions DistributedCache { get; set; }
        public QueryCacheOptions QueryCache { get; set; }
    }
    
    public class DistributedCacheOptions
    {
        public const string DistributedCache = "DistributedCache";
        public int AbsoluteExpirationInMinutes { get; set; }
        public int SlidingExpirationInMinutes { get; set; }
    }
    
    public class QueryCacheOptions
    {
        public const string QueryCache = "QueryCache";
        public int AbsoluteExpirationInMinutes { get; set; }
        public int SlidingExpirationInMinutes { get; set; }
    }
}