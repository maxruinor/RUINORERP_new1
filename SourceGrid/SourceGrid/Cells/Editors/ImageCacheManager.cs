using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片缓存管理器
    /// 提供图片懒加载、内存缓存和磁盘缓存功能
    /// </summary>
    public class ImageCacheManager : IDisposable
    {
        #region 单例模式

        private static readonly Lazy<ImageCacheManager> _instance = new Lazy<ImageCacheManager>(() => new ImageCacheManager());
        public static ImageCacheManager Instance => _instance.Value;

        private ImageCacheManager()
        {
            InitializeCache();
        }

        #endregion

        #region 配置属性

        /// <summary>
        /// 内存缓存最大数量（默认50张图片）
        /// </summary>
        public int MaxMemoryCacheSize { get; set; } = 50;

        /// <summary>
        /// 单个图片最大缓存大小（默认5MB）
        /// </summary>
        public long MaxImageCacheSize { get; set; } = 5 * 1024 * 1024;

        /// <summary>
        /// 磁盘缓存目录
        /// </summary>
        public string DiskCacheDirectory { get; set; }

        /// <summary>
        /// 磁盘缓存过期时间（默认7天）
        /// </summary>
        public TimeSpan DiskCacheExpiry { get; set; } = TimeSpan.FromDays(7);

        /// <summary>
        /// 是否启用磁盘缓存
        /// </summary>
        public bool EnableDiskCache { get; set; } = true;

        #endregion

        #region 缓存存储

        /// <summary>
        /// 内存缓存：文件ID -> 图片对象
        /// </summary>
        private readonly ConcurrentDictionary<string, CachedImage> _memoryCache = new ConcurrentDictionary<string, CachedImage>();

        /// <summary>
        /// 加载任务缓存：文件ID -> 任务
        /// </summary>
        private readonly ConcurrentDictionary<string, Task<System.Drawing.Image>> _loadingTasks = new ConcurrentDictionary<string, Task<System.Drawing.Image>>();

        /// <summary>
        /// 访问记录（用于LRU清理）
        /// </summary>
        private readonly LinkedList<string> _accessOrder = new LinkedList<string>();

        /// <summary>
        /// 访问记录的映射：文件ID -> 链表节点
        /// </summary>
        private readonly ConcurrentDictionary<string, LinkedListNode<string>> _accessNodes = new ConcurrentDictionary<string, LinkedListNode<string>>();

        /// <summary>
        /// 缓存锁
        /// </summary>
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化缓存
        /// </summary>
        private void InitializeCache()
        {
            // 设置默认磁盘缓存目录
            if (string.IsNullOrEmpty(DiskCacheDirectory))
            {
                DiskCacheDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "SourceGrid",
                    "ImageCache"
                );
            }

            // 确保缓存目录存在
            if (EnableDiskCache && !Directory.Exists(DiskCacheDirectory))
            {
                Directory.CreateDirectory(DiskCacheDirectory);
            }

            // 启动定期清理任务
            Task.Run(async () =>
            {
                while (!_disposed)
                {
                    await Task.Delay(TimeSpan.FromHours(1));
                    CleanupExpiredCache();
                }
            });
        }

        #endregion

        #region 公共接口

        /// <summary>
        /// 异步获取图片（带缓存）
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="imageLoader">图片加载器</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>图片对象</returns>
        public async Task<System.Drawing.Image> GetImageAsync(string fileId, Func<string, Task<byte[]>> imageLoader, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentException("文件ID不能为空", nameof(fileId));

            if (imageLoader == null)
                throw new ArgumentNullException(nameof(imageLoader));

            // 1. 检查内存缓存
            if (TryGetFromMemoryCache(fileId, out var cachedImage))
            {
                return cachedImage.Image;
            }

            // 2. 检查是否已在加载中
            var loadingTask = _loadingTasks.GetOrAdd(fileId, async (id) =>
            {
                try
                {
                    return await LoadImageInternal(id, imageLoader, cancellationToken);
                }
                finally
                {
                    _loadingTasks.TryRemove(id, out _);
                }
            });

            return await loadingTask;
        }

        /// <summary>
        /// 同步获取图片（带缓存）
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="imageLoader">图片加载器</param>
        /// <returns>图片对象</returns>
        public System.Drawing.Image GetImage(string fileId, Func<string, byte[]> imageLoader)
        {
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentException("文件ID不能为空", nameof(fileId));

            if (imageLoader == null)
                throw new ArgumentNullException(nameof(imageLoader));

            // 检查内存缓存
            if (TryGetFromMemoryCache(fileId, out var cachedImage))
            {
                return cachedImage.Image;
            }

            // 检查磁盘缓存
            if (TryGetFromDiskCache(fileId, out byte[] diskData))
            {
                var image = ImageProcessor.ByteArrayToImage(diskData);
                AddToMemoryCache(fileId, image);
                return image;
            }

            // 直接加载
            byte[] imageData = imageLoader(fileId);
            var loadedImage = ImageProcessor.ByteArrayToImage(imageData);

            // 添加到缓存
            AddToMemoryCache(fileId, loadedImage);
            if (EnableDiskCache)
            {
                SaveToDiskCache(fileId, imageData);
            }

            return loadedImage;
        }

        /// <summary>
        /// 预加载图片
        /// </summary>
        /// <param name="fileIds">要预加载的文件ID列表</param>
        /// <param name="imageLoader">图片加载器</param>
        public async Task PreloadImagesAsync(IEnumerable<string> fileIds, Func<string, Task<byte[]>> imageLoader)
        {
            if (fileIds == null || imageLoader == null)
                return;

            var preloadTasks = fileIds
                .Where(fileId => !string.IsNullOrEmpty(fileId) && !IsInMemoryCache(fileId))
                .Select(fileId => GetImageAsync(fileId, imageLoader))
                .ToList();

            await Task.WhenAll(preloadTasks);
        }

        /// <summary>
        /// 清除指定图片的缓存
        /// </summary>
        /// <param name="fileId">文件ID</param>
        public void ClearCache(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
                return;

            // 清除内存缓存
            if (_memoryCache.TryRemove(fileId, out var cachedImage))
            {
                cachedImage.Image?.Dispose();
            }

            // 更新访问记录
            UpdateAccessOrder(fileId, remove: true);

            // 清除磁盘缓存
            if (EnableDiskCache)
            {
                try
                {
                    var diskPath = GetDiskCachePath(fileId);
                    if (File.Exists(diskPath))
                    {
                        File.Delete(diskPath);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"删除磁盘缓存失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            // 清除内存缓存
            _cacheLock.EnterWriteLock();
            try
            {
                foreach (var kvp in _memoryCache)
                {
                    kvp.Value.Image?.Dispose();
                }
                _memoryCache.Clear();
                _accessOrder.Clear();
                _accessNodes.Clear();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            // 清除磁盘缓存
            if (EnableDiskCache && Directory.Exists(DiskCacheDirectory))
            {
                try
                {
                    var cacheFiles = Directory.GetFiles(DiskCacheDirectory, "*.cache");
                    foreach (var file in cacheFiles)
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"清除磁盘缓存失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public CacheStatistics GetCacheStatistics()
        {
            _cacheLock.EnterReadLock();
            try
            {
                return new CacheStatistics
                {
                    MemoryCacheCount = _memoryCache.Count,
                    LoadingTaskCount = _loadingTasks.Count,
                    TotalMemorySize = _memoryCache.Values.Sum(x => x.ImageSize),
                    MaxMemoryCacheSize = MaxMemoryCacheSize
                };
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 从内存缓存获取图片
        /// </summary>
        private bool TryGetFromMemoryCache(string fileId, out CachedImage cachedImage)
        {
            _cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (_memoryCache.TryGetValue(fileId, out cachedImage))
                {
                    // 更新访问顺序
                    UpdateAccessOrder(fileId);
                    return true;
                }
                return false;
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 从磁盘缓存获取图片
        /// </summary>
        private bool TryGetFromDiskCache(string fileId, out byte[] imageData)
        {
            imageData = null;

            if (!EnableDiskCache)
                return false;

            try
            {
                var diskPath = GetDiskCachePath(fileId);
                if (File.Exists(diskPath))
                {
                    var fileInfo = new FileInfo(diskPath);
                    if (DateTime.Now - fileInfo.LastWriteTime < DiskCacheExpiry)
                    {
                        imageData = File.ReadAllBytes(diskPath);
                        return true;
                    }
                    else
                    {
                        // 过期文件，删除
                        File.Delete(diskPath);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取磁盘缓存失败: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// 内部图片加载方法
        /// </summary>
        private async Task<System.Drawing.Image> LoadImageInternal(string fileId, Func<string, Task<byte[]>> imageLoader, CancellationToken cancellationToken)
        {
            // 检查磁盘缓存
            if (TryGetFromDiskCache(fileId, out byte[] diskData))
            {
                var image = ImageProcessor.ByteArrayToImage(diskData);
                AddToMemoryCache(fileId, image);
                return image;
            }

            // 从外部加载
            byte[] imageData = await imageLoader(fileId);
            if (imageData == null || imageData.Length == 0)
                throw new InvalidOperationException("图片数据为空");

            var loadedImage = ImageProcessor.ByteArrayToImage(imageData);

            // 添加到缓存
            AddToMemoryCache(fileId, loadedImage);
            if (EnableDiskCache)
            {
                SaveToDiskCache(fileId, imageData);
            }

            return loadedImage;
        }

        /// <summary>
        /// 添加到内存缓存
        /// </summary>
        private void AddToMemoryCache(string fileId, System.Drawing.Image image)
        {
            if (image == null)
                return;

            var cachedImage = new CachedImage
            {
                Image = image,
                LastAccess = DateTime.Now,
                ImageSize = EstimateImageSize(image)
            };

            _cacheLock.EnterWriteLock();
            try
            {
                // 检查缓存大小限制
                while (_memoryCache.Count >= MaxMemoryCacheSize)
                {
                    RemoveOldestCache();
                }

                // 添加到缓存
                _memoryCache.AddOrUpdate(fileId, cachedImage, (key, oldValue) =>
                {
                    oldValue.Image?.Dispose();
                    return cachedImage;
                });

                // 更新访问顺序
                UpdateAccessOrder(fileId);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 保存到磁盘缓存
        /// </summary>
        private void SaveToDiskCache(string fileId, byte[] imageData)
        {
            if (!EnableDiskCache || imageData == null)
                return;

            try
            {
                var diskPath = GetDiskCachePath(fileId);
                ImageProcessor.SaveBytesAsImage(imageData, diskPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存磁盘缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取磁盘缓存路径
        /// </summary>
        private string GetDiskCachePath(string fileId)
        {
            // 将文件ID中的斜杠替换为下划线
            var safeFileName = fileId.Replace('/', '_');
            return Path.Combine(DiskCacheDirectory, $"{safeFileName}.cache");
        }

        /// <summary>
        /// 更新访问顺序
        /// </summary>
        private void UpdateAccessOrder(string fileId, bool remove = false)
        {
            if (_accessNodes.TryRemove(fileId, out var node))
            {
                _accessOrder.Remove(node);
            }

            if (!remove)
            {
                var newNode = _accessOrder.AddLast(fileId);
                _accessNodes.TryAdd(fileId, newNode);
            }
        }

        /// <summary>
        /// 移除最旧的缓存
        /// </summary>
        private void RemoveOldestCache()
        {
            if (_accessOrder.First == null)
                return;

            var oldestFileId = _accessOrder.First.Value;
            _accessOrder.RemoveFirst();
            _accessNodes.TryRemove(oldestFileId, out _);

            if (_memoryCache.TryRemove(oldestFileId, out var oldestImage))
            {
                oldestImage.Image?.Dispose();
            }
        }

        /// <summary>
        /// 检查是否在内存缓存中
        /// </summary>
        private bool IsInMemoryCache(string fileId)
        {
            return _memoryCache.ContainsKey(fileId);
        }

        /// <summary>
        /// 清理过期缓存
        /// </summary>
        private void CleanupExpiredCache()
        {
            if (!EnableDiskCache || !Directory.Exists(DiskCacheDirectory))
                return;

            try
            {
                var cacheFiles = Directory.GetFiles(DiskCacheDirectory, "*.cache");
                foreach (var file in cacheFiles)
                {
                    var fileInfo = new FileInfo(file);
                    if (DateTime.Now - fileInfo.LastWriteTime > DiskCacheExpiry)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清理过期缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 估算图片大小
        /// </summary>
        private long EstimateImageSize(System.Drawing.Image image)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return ms.Length;
                }
            }
            catch
            {
                // 如果无法估算，使用默认大小
                return 1024 * 1024; // 1MB
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _cacheLock.EnterWriteLock();
                try
                {
                    foreach (var kvp in _memoryCache)
                    {
                        kvp.Value.Image?.Dispose();
                    }
                    _memoryCache.Clear();
                    _accessOrder.Clear();
                    _accessNodes.Clear();
                    _disposed = true;
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                    _cacheLock?.Dispose();
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 缓存的图片信息
    /// </summary>
    internal class CachedImage
    {
        public System.Drawing.Image Image { get; set; }
        public DateTime LastAccess { get; set; }
        public long ImageSize { get; set; }
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        public int MemoryCacheCount { get; set; }
        public int LoadingTaskCount { get; set; }
        public long TotalMemorySize { get; set; }
        public int MaxMemoryCacheSize { get; set; }
    }
}