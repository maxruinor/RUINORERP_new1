using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CacheManager.Core;


namespace RUINORERP.Business.Cache
{



    /// <summary>
    /// 缓存管理器工厂接口
    /// </summary>
    public interface ICacheManagerFactory
    {
        /// <summary>
        /// 获取指定名称的缓存管理器
        /// </summary>
        /// <param name="name">缓存名称</param>
        /// <returns>缓存管理器实例</returns>
        ICacheManager<object> GetCacheManager(string name);

        /// <summary>
        /// 获取业务缓存实例
        /// </summary>
        /// <typeparam name="TCache">业务缓存接口类型</typeparam>
        /// <returns>业务缓存实例</returns>
        TCache GetBusinessCache<TCache>() where TCache : ICache;
    }

    


}
