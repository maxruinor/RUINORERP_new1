using System.Drawing;
using Microsoft.Extensions.Caching.Memory;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 图片缓存
    /// </summary>
    public class ImageCache
    {
        private static readonly ImageCache _instance = new ImageCache();
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 1024 * 1024 * 1024, // 1GB缓存
            ExpirationScanFrequency = System.TimeSpan.FromMinutes(5)
        });
        private readonly object _lock = new object();
        
        /// <summary>
        /// 单例实例
        /// </summary>
        public static ImageCache Instance => _instance;

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片对象</returns>
        public Image GetImage(long fileId)
        {
            lock (_lock)
            {
                if (_cache.TryGetValue(fileId.ToString(), out Image image))
                {
                    return image;
                }
                return null;
            }
        }
        
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <param name="image">图片对象</param>
        public void AddImage(long fileId, Image image)
        {
            lock (_lock)
            {
                if (fileId > 0 && image != null)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(image.Size.Width * image.Size.Height * 4) // 估算内存占用
                        .SetSlidingExpiration(System.TimeSpan.FromMinutes(30))
                        .SetAbsoluteExpiration(System.TimeSpan.FromHours(2));
                    
                    _cache.Set(fileId.ToString(), image, cacheEntryOptions);
                }
            }
        }
        
        /// <summary>
        /// 移除图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        public void RemoveImage(long fileId)
        {
            lock (_lock)
            {
                if (fileId > 0)
                {
                    _cache.Remove(fileId.ToString());
                }
            }
        }
        
        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_lock)
            {
                _cache.Clear();
            }
        }
        
        /// <summary>
        /// 获取缓存大小
        /// </summary>
        /// <returns>缓存大小（字节）</returns>
        public long GetCacheSize()
        {
            // 这里可以实现缓存大小的计算
            return 0;
        }
    }
}