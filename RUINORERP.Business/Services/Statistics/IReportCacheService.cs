using RUINORERP.Model.ChartFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.Services.Statistics
{
    /// <summary>
    /// 报表缓存服务接口
    /// </summary>
    public interface IReportCacheService
    {
        /// <summary>
        /// 尝试从缓存获取数据
        /// </summary>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// 移除缓存
        /// </summary>
        void Remove(string key);

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        void Clear();
    }
}
