using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RUINORERP.Model.BusinessImage
{
    /// <summary>
    /// ✅ 通用图片缓存服务基类 - 使用object存储,不依赖具体实体类型
    /// 适用于客户端和服务器端,减少数据库查询压力
    /// 调用方自行转换类型,保持缓存服务的通用性
    /// </summary>
    public abstract class ImageCacheServiceBase : IDisposable
    {
        protected readonly IMemoryCache _memoryCache;
        protected MemoryCacheEntryOptions _cacheOptions;
        private bool _disposed = false;

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        protected const string CacheKeyPrefix = "Image_";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memoryCache">内存缓存实例</param>
        /// <param name="slidingExpirationMinutes">滑动过期时间(分钟),默认240分钟(4小时)</param>
        /// <param name="absoluteExpirationHours">绝对过期时间(小时),默认8小时</param>
        /// <param name="priority">缓存优先级,默认High</param>
        protected ImageCacheServiceBase(
            IMemoryCache memoryCache,
            int slidingExpirationMinutes = 240,
            int absoluteExpirationHours = 8,
            CacheItemPriority priority = CacheItemPriority.High)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpirationMinutes))
                .SetAbsoluteExpiration(TimeSpan.FromHours(absoluteExpirationHours))
                .SetSize(1024)
                .SetPriority(priority);
        }

        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>缓存键</returns>
        protected virtual string GenerateCacheKey(long fileId)
        {
            return $"{CacheKeyPrefix}{fileId}";
        }

        #region 基础缓存操作

        /// <summary>
        /// ✅ 从缓存获取对象(返回object,调用方自行转换)
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>缓存对象,不存在返回null</returns>
        public virtual object GetImageInfo(long fileId)
        {
            if (fileId <= 0)
                return null;

            var cacheKey = GenerateCacheKey(fileId);
            _memoryCache.TryGetValue(cacheKey, out object imageInfo);
            return imageInfo;
        }

        /// <summary>
        /// ✅ 将对象添加到缓存(接受object,通用性强)
        /// </summary>
        /// <param name="imageInfo">图片信息对象(tb_FS_FileStorageInfo或其他)</param>
        public virtual void AddImageInfo(tb_FS_FileStorageInfo imageInfo)
        {
            if (imageInfo == null)
                return;
            var cacheKey = GenerateCacheKey(imageInfo.FileId);
            _memoryCache.Set(cacheKey, imageInfo, _cacheOptions);
        }

        /// <summary>
        /// 从缓存移除图片信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        public virtual void RemoveImageInfo(long fileId)
        {
            if (fileId <= 0)
                return;

            var cacheKey = GenerateCacheKey(fileId);
            _memoryCache.Remove(cacheKey);
        }

        /// <summary>
        /// ✅ 批量添加对象到缓存
        /// </summary>
        /// <param name="imageInfos">图片信息列表(object类型)</param>
        public virtual void AddImageInfos(IEnumerable<tb_FS_FileStorageInfo> imageInfos)
        {
            if (imageInfos == null)
                return;

            foreach (var imageInfo in imageInfos)
            {
                AddImageInfo(imageInfo);
            }
        }

        /// <summary>
        /// 批量清除指定文件ID的缓存
        /// </summary>
        /// <param name="fileIds">要清除的文件ID列表</param>
        public virtual void RemoveImageInfos(IEnumerable<long> fileIds)
        {
            if (fileIds == null)
                return;

            foreach (var fileId in fileIds)
            {
                RemoveImageInfo(fileId);
            }
        }

        /// <summary>
        /// 清除所有图片缓存
        /// </summary>
        public virtual void ClearCache()
        {
            if (_memoryCache is MemoryCache memoryCache)
            {
                memoryCache.Clear();
            }
        }

        #endregion

        #region IDisposable实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            OnDisposing();
        }

        /// <summary>
        /// 派生类可以重写此方法以执行自定义的清理操作
        /// </summary>
        protected virtual void OnDisposing()
        {
            // 派生类可以在此处添加额外的清理逻辑
        }

        #endregion
    }
}
