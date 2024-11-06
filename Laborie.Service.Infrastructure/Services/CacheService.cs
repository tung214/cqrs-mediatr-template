using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Application.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Laborie.Service.Infrastructure.Services
{
    public class CacheService(IDistributedCache redis) : ICacheService
    {
        public async Task<string?> GetString(string key)
        {
            return await redis.GetStringAsync(key);
        }

        public async Task SetString(string key, string value, int expire)
        {
            await redis.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expire)
            });
        }
    }
}