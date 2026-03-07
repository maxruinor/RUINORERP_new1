using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// LRU图片缓存实现
    /// 使用最近最少使用策略管理图片缓存
    /// </summary>
    public class LRUImageCache : IDisposable
    {
        /// <summary>
        /// 最大缓存容量
        /// </summary>
        private int _maxCapacity;

        /// <summary>
        /// LRU链表，用于维护访问顺序
        /// </summary>
        private LinkedList<string> _accessOrder;

        /// <summary>
        /// 缓存字典，用于快速查找
        /// </summary>
        private Dictionary<string, Image> _cache;

        /// <summary>
        /// 线程锁，保证线程安全
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// 静态SHA256实例，提升性能
        /// </summary>
        private static readonly SHA256 _sha256 = SHA256.Create();

        /// <summary>
        /// 是否已释放
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 最大缓存容量
        /// </summary>
        public int MaxCapacity
        {
            get => _maxCapacity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("缓存容量必须大于0");

                _maxCapacity = value;
                // 调整容量后，清理超出部分
                Cleanup();
            }
        }

        /// <summary>
        /// 当前缓存项数量
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lockObject)
                {
                    return _cache.Count;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxCapacity">最大缓存容量</param>
        /// <exception cref="ArgumentException">当maxCapacity小于等于0时抛出</exception>
        public LRUImageCache(int maxCapacity = ImageProcessingConstants.DefaultMaxCacheCapacity)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException("缓存容量必须大于0");

            _maxCapacity = maxCapacity;
            _accessOrder = new LinkedList<string>();
            _cache = new Dictionary<string, Image>();
        }

        /// <summary>
        /// 添加图片到缓存
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="image">要缓存的图片</param>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public void Add(string key, Image image)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "缓存键不能为空");

            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            lock (_lockObject)
            {
                // 如果键已存在，先移除旧条目并释放资源
                if (_cache.ContainsKey(key))
                {
                    SafeRemove(key);
                }

                // 如果缓存已满，移除最少使用的项
                if (_cache.Count >= _maxCapacity)
                {
                    SafeRemove(_accessOrder.Last.Value);
                }

                // 添加新项到缓存
                _cache.Add(key, image);
                _accessOrder.AddFirst(key);
            }
        }

        /// <summary>
        /// 从缓存获取图片
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存的图片，如果不存在则返回null</returns>
        public Image Get(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "缓存键不能为空");

            lock (_lockObject)
            {
                if (_cache.TryGetValue(key, out Image image))
                {
                    // 更新访问顺序
                    _accessOrder.Remove(key);
                    _accessOrder.AddFirst(key);
                    return image;
                }
                return null;
            }
        }

        /// <summary>
        /// 检查缓存中是否存在指定键
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否存在</returns>
        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "缓存键不能为空");

            lock (_lockObject)
            {
                return _cache.ContainsKey(key);
            }
        }

        /// <summary>
        /// 从缓存中移除指定项
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        /// <exception cref="ArgumentNullException">当key为null或空时抛出</exception>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "缓存键不能为空");

            return SafeRemove(key);
        }

        /// <summary>
        /// 安全移除缓存项并释放图片资源
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>是否移除成功</returns>
        private bool SafeRemove(string key)
        {
            _accessOrder.Remove(key);

            if (_cache.TryGetValue(key, out Image image))
            {
                _cache.Remove(key);
                // 释放图片资源
                if (image != null)
                {
                    try
                    {
                        image.Dispose();
                    }
                    catch
                    {
                        // 忽略释放异常
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清空缓存并释放所有图片资源
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                // 释放所有图片资源
                foreach (var image in _cache.Values)
                {
                    try
                    {
                        image?.Dispose();
                    }
                    catch
                    {
                        // 忽略释放异常
                    }
                }

                _accessOrder.Clear();
                _cache.Clear();
            }
        }

        /// <summary>
        /// 生成缓存键（使用静态SHA256实例提升性能）
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>生成的缓存键</returns>
        /// <exception cref="ArgumentNullException">当input为null或空时抛出</exception>
        public string GenerateCacheKey(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input), "输入字符串不能为空");

            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = _sha256.ComputeHash(bytes);

            // 使用Convert.ToHexString提升性能（.NET 5+）
            // 兼容旧版本：使用BitConverter
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

        /// <summary>
        /// 清理超出容量的缓存项
        /// </summary>
        private void Cleanup()
        {
            lock (_lockObject)
            {
                while (_cache.Count > _maxCapacity)
                {
                    SafeRemove(_accessOrder.Last.Value);
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    Clear();
                    _accessOrder = null;
                    _cache = null;
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~LRUImageCache()
        {
            Dispose(false);
        }
    }
}