using Loyalto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Data
{
    public class LookupDataRetriever : ILookupDataRetriever
    {
        private readonly string KeyName = "OnCacheLookupData";
        private IMemoryCache memoryCache;
        private readonly DbContextOptions<LoyaltoContext> dbContextOptions;
        //private static OnMemoryLookupData onMemoryLookupData;
        public LookupDataRetriever(IMemoryCache memoryCache, DbContextOptions<LoyaltoContext> dbContextOptions)
        {
            //clear cache
            //memoryCache.Remove(KeyName);
            this.dbContextOptions = dbContextOptions;
            this.memoryCache = memoryCache;
            //onMemoryLookupData = new OnMemoryLookupData(dbContextOptions);
        }

        public OnMemoryLookupData GetLookupData()
        {
            // Look for cache key.
            if (!memoryCache.TryGetValue(KeyName, out OnMemoryLookupData onMemoryLookupData))
            {
                // Key not in cache, so get data.
                onMemoryLookupData = Refresh();
            }
            return onMemoryLookupData;
        }
        public OnMemoryLookupData Refresh()
        {
            // Create a new fresh one
            OnMemoryLookupData onMemoryLookupData = new OnMemoryLookupData(dbContextOptions);
            // Save data in cache.
            memoryCache.Set(KeyName, onMemoryLookupData, DateTimeOffset.MaxValue);
            return onMemoryLookupData;
        }

    }
}
