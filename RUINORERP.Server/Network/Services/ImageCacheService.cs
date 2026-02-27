using Microsoft.Extensions.Caching.Memory;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 图片缓存服务
    /// 提供基于内存的图片信息缓存，减少数据库查询压力
    /// </summary>
    public class ImageCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        /// <summary>
        /// 缓存键前缀
        /// </summary>
        private const string CacheKeyPrefix = "Image_";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memoryCache">内存缓存</param>
        public ImageCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            
            // 设置缓存选项：2小时过期，滑动过期
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(2))
                .SetAbsoluteExpiration(TimeSpan.FromHours(4))
                .SetSize(1024); // 设置缓存项大小
        }

        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>缓存键</returns>
        private string GenerateCacheKey(long fileId)
        {
            return $"{CacheKeyPrefix}{fileId}";
        }

        /// <summary>
        /// 从缓存获取图片信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>图片信息，不存在返回null</returns>
        public tb_FS_FileStorageInfo GetImageInfo(long fileId)
        {
            var cacheKey = GenerateCacheKey(fileId);
            _memoryCache.TryGetValue(cacheKey, out tb_FS_FileStorageInfo imageInfo);
            return imageInfo;
        }

        /// <summary>
        /// 将图片信息添加到缓存
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        public void AddImageInfo(tb_FS_FileStorageInfo imageInfo)
        {
            if (imageInfo == null || imageInfo.FileId <= 0)
                return;

            var cacheKey = GenerateCacheKey(imageInfo.FileId);
            _memoryCache.Set(cacheKey, imageInfo, _cacheOptions);
        }

        /// <summary>
        /// 从缓存移除图片信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        public void RemoveImageInfo(long fileId)
        {
            var cacheKey = GenerateCacheKey(fileId);
            _memoryCache.Remove(cacheKey);
        }

        /// <summary>
        /// 批量添加图片信息到缓存
        /// </summary>
        /// <param name="imageInfos">图片信息列表</param>
        public void AddImageInfos(IEnumerable<tb_FS_FileStorageInfo> imageInfos)
        {
            if (imageInfos == null)
                return;

            foreach (var imageInfo in imageInfos)
            {
                AddImageInfo(imageInfo);
            }
        }

        /// <summary>
        /// 清除所有图片缓存
        /// </summary>
        public void ClearCache()
        {
            // 注意：MemoryCache 没有直接的 Clear 方法
            // 这里可以通过创建新的缓存实例或使用其他策略
            // 暂时留空，实际项目中可以根据需要实现
        }
    }
}
