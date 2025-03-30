using Microsoft.Extensions.Caching.Memory;
using RUINORERP.UI.ChartFramework.Data.Interfaces;
using RUINORERP.UI.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartDataSource
{
    // 添加缓存装饰器
    public class CachedDataSource : IChartDataSource
    {
        private readonly IChartDataSource _inner;
        private readonly IMemoryCache _cache;

        public async Task<ChartDataSet> GetDataAsync(ChartRequest request)
        {
            var cacheKey = $"Chart_{request.GetHashCode()}";
            return await _cache.GetOrCreateAsync(cacheKey,
                async entry => {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                    return await _inner.GetDataAsync(request);
                });
        }
    }
}
