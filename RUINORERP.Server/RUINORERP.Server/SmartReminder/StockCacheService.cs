using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.SmartReminder
{
    public interface IStockCacheService
    {
        public Task<tb_Inventory> GetStockAsync(long productId);
    }

    public class StockCacheService : IStockCacheService
    {
        private readonly IMemoryCache _cache;
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        private readonly ILogger<StockCacheService> _logger;

        public StockCacheService(
            IMemoryCache cache,
         ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage,
            ILogger<StockCacheService> logger)
        {
            _cache = cache;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
        }

        public async Task<tb_Inventory> GetStockAsync(long productId)
        {
            return await _cache.GetOrCreateAsync($"stock_{productId}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                try
                {
                    return await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                        .FirstAsync(p => p.ProdDetailID == productId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "获取库存缓存失败 ProductID={ProductId}", productId);
                    return null;
                }
            });
        }
    }
}
