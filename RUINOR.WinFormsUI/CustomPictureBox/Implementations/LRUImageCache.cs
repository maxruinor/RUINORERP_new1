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
    public class LRUImageCache
    {
        // 最大缓存容量
        private int _maxCapacity;
        // LRU链表，用于维护访问顺序
        private LinkedList<string> _accessOrder;
        // 缓存字典，用于快速查找
        private Dictionary<string, Image> _cache;
        // 线程锁，保证线程安全
        private readonly object _lockObject = new object();

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
        public int Count => _cache.Count;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxCapacity">最大缓存容量</param>
        public LRUImageCache(int maxCapacity = 100)
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
        public void Add(string key, Image image)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "缓存键不能为空");

            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            lock (_lockObject)
            {
                // 如果键已存在，先移除旧条目
                if (_cache.ContainsKey(key))
                {
                    Remove(key);
                }

                // 如果缓存已满，移除最少使用的项
                if (_cache.Count >= _maxCapacity)
                {
                    Remove(_accessOrder.Last.Value);
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
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key), "缓存键不能为空");

            lock (_lockObject)
            {
                _accessOrder.Remove(key);
                return _cache.Remove(key);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                _accessOrder.Clear();
                _cache.Clear();
            }
        }

        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>生成的缓存键</returns>
        public string GenerateCacheKey(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input), "输入字符串不能为空");

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
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
                    Remove(_accessOrder.Last.Value);
                }
            }
        }
    }
}