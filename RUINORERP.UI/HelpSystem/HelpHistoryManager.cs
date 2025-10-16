using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助历史记录管理器
    /// </summary>
    public static class HelpHistoryManager
    {
        /// <summary>
        /// 帮助历史记录项
        /// </summary>
        [Serializable]
        public class HelpHistoryItem
        {
            /// <summary>
            /// 帮助页面路径
            /// </summary>
            public string HelpPage { get; set; }

            /// <summary>
            /// 帮助页面标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 查看时间
            /// </summary>
            public DateTime ViewTime { get; set; }

            /// <summary>
            /// 查看次数
            /// </summary>
            public int ViewCount { get; set; }
        }

        /// <summary>
        /// 历史记录列表
        /// </summary>
        private static readonly List<HelpHistoryItem> _history = new List<HelpHistoryItem>();

        /// <summary>
        /// 配置
        /// </summary>
        private static readonly HelpSystemConfig _config = HelpSystemConfig.Load();

        /// <summary>
        /// 历史记录文件路径
        /// </summary>
        private static readonly string HistoryFilePath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "HelpHistory.xml");

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static HelpHistoryManager()
        {
            LoadHistory();
        }

        /// <summary>
        /// 记录帮助页面查看历史
        /// </summary>
        /// <param name="helpPage">帮助页面路径</param>
        /// <param name="title">帮助页面标题</param>
        public static void RecordView(string helpPage, string title = null)
        {
            if (!_config.IsHistoryTrackingEnabled || string.IsNullOrEmpty(helpPage))
                return;

            try
            {
                // 查找是否已存在该页面的记录
                var existingItem = _history.FirstOrDefault(item => item.HelpPage == helpPage);
                if (existingItem != null)
                {
                    // 更新现有记录
                    existingItem.ViewTime = DateTime.Now;
                    existingItem.ViewCount++;
                    if (!string.IsNullOrEmpty(title))
                        existingItem.Title = title;
                }
                else
                {
                    // 添加新记录
                    var newItem = new HelpHistoryItem
                    {
                        HelpPage = helpPage,
                        Title = title ?? helpPage,
                        ViewTime = DateTime.Now,
                        ViewCount = 1
                    };
                    _history.Add(newItem);
                }

                // 保持历史记录数量在限制范围内
                if (_history.Count > _config.MaxHistoryCount)
                {
                    // 移除最旧的记录
                    var itemsToRemove = _history
                        .OrderBy(item => item.ViewTime)
                        .Take(_history.Count - _config.MaxHistoryCount)
                        .ToList();
                    
                    foreach (var item in itemsToRemove)
                    {
                        _history.Remove(item);
                    }
                }

                // 保存历史记录
                SaveHistory();
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"记录帮助历史时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取最近查看的帮助页面
        /// </summary>
        /// <param name="count">返回记录数量</param>
        /// <returns>帮助历史记录列表</returns>
        public static List<HelpHistoryItem> GetRecentHistory(int count = 10)
        {
            if (!_config.IsHistoryTrackingEnabled)
                return new List<HelpHistoryItem>();

            return _history
                .OrderByDescending(item => item.ViewTime)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// 获取最常查看的帮助页面
        /// </summary>
        /// <param name="count">返回记录数量</param>
        /// <returns>帮助历史记录列表</returns>
        public static List<HelpHistoryItem> GetMostViewed(int count = 10)
        {
            if (!_config.IsHistoryTrackingEnabled)
                return new List<HelpHistoryItem>();

            return _history
                .OrderByDescending(item => item.ViewCount)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// 清除历史记录
        /// </summary>
        public static void ClearHistory()
        {
            _history.Clear();
            SaveHistory();
        }

        /// <summary>
        /// 加载历史记录
        /// </summary>
        private static void LoadHistory()
        {
            try
            {
                if (File.Exists(HistoryFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<HelpHistoryItem>));
                    using (FileStream fs = new FileStream(HistoryFilePath, FileMode.Open))
                    {
                        var loadedHistory = (List<HelpHistoryItem>)serializer.Deserialize(fs);
                        _history.Clear();
                        _history.AddRange(loadedHistory);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"加载帮助历史记录时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 保存历史记录
        /// </summary>
        private static void SaveHistory()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<HelpHistoryItem>));
                using (FileStream fs = new FileStream(HistoryFilePath, FileMode.Create))
                {
                    serializer.Serialize(fs, _history);
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"保存帮助历史记录时出错: {ex.Message}");
            }
        }
    }
}