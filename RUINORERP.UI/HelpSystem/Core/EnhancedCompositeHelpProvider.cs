using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 增强型组合帮助提供者
    /// 优先使用在线Web帮助，网络不可用时自动切换到本地缓存
    /// 提供统一的帮助调用接口
    /// </summary>
    public class EnhancedCompositeHelpProvider : IHelpProvider
    {
        #region 私有字段

        private readonly WebHelpProvider _webProvider;
        private readonly LocalCacheHelpProvider _localProvider;
        private bool _disposed = false;
        private bool _preferOnline = true;

        #endregion

        #region 公共属性

        public string ProviderName => "智能组合帮助提供者（在线优先）";
        
        public string HelpContentRootPath => _webProvider?.HelpContentRootPath ?? _localProvider?.HelpContentRootPath;
        
        public int HelpCount => (_webProvider?.HelpCount ?? 0) + (_localProvider?.HelpCount ?? 0);

        /// <summary>
        /// 是否优先使用在线帮助
        /// </summary>
        public bool PreferOnline
        {
            get => _preferOnline;
            set => _preferOnline = value;
        }

        /// <summary>
        /// Web帮助提供者
        /// </summary>
        public WebHelpProvider WebProvider => _webProvider;

        /// <summary>
        /// 本地缓存帮助提供者
        /// </summary>
        public LocalCacheHelpProvider LocalProvider => _localProvider;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="webProvider">Web帮助提供者</param>
        /// <param name="localProvider">本地缓存帮助提供者</param>
        public EnhancedCompositeHelpProvider(WebHelpProvider webProvider, LocalCacheHelpProvider localProvider)
        {
            _webProvider = webProvider ?? throw new ArgumentNullException(nameof(webProvider));
            _localProvider = localProvider ?? throw new ArgumentNullException(nameof(localProvider));
        }

        /// <summary>
        /// 便捷构造函数
        /// </summary>
        /// <param name="baseUrl">在线帮助网站URL</param>
        /// <param name="cacheDirectory">本地缓存目录</param>
        public EnhancedCompositeHelpProvider(string baseUrl, string cacheDirectory)
        {
            _webProvider = new WebHelpProvider(baseUrl);
            _localProvider = new LocalCacheHelpProvider(cacheDirectory);
        }

        #endregion

        #region IHelpProvider 实现

        public string GetHelpContent(HelpContext context)
        {
            // 优先尝试本地（因为本地返回HTML内容）
            if (_localProvider.HelpExists(context))
            {
                return _localProvider.GetHelpContent(context);
            }
            
            return null;
        }

        public List<HelpSearchResult> Search(string keyword, HelpContext context = null)
        {
            var results = new List<HelpSearchResult>();
            
            // 合并两个提供者的搜索结果
            try
            {
                var webResults = _webProvider.Search(keyword, context);
                if (webResults != null)
                    results.AddRange(webResults);
            }
            catch { }

            try
            {
                var localResults = _localProvider.Search(keyword, context);
                if (localResults != null)
                    results.AddRange(localResults);
            }
            catch { }

            // 去重并排序
            return results
                .GroupBy(r => r.HelpKey)
                .Select(g => g.First())
                .OrderByDescending(r => r.RelevanceScore)
                .ToList();
        }

        public bool HelpExists(HelpContext context)
        {
            // 任一提供者存在即可
            return _webProvider.HelpExists(context) || _localProvider.HelpExists(context);
        }

        public void ReloadIndex()
        {
            _webProvider.ReloadIndex();
            _localProvider.ReloadIndex();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _webProvider?.Dispose();
                _localProvider?.Dispose();
                _disposed = true;
            }
        }

        #endregion

        #region 智能显示帮助

        /// <summary>
        /// 智能显示帮助（自动选择在线或本地）
        /// 这是主要的帮助显示方法
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>是否成功显示</returns>
        public bool ShowHelp(HelpContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.HelpKey))
            {
                ShowHelpNotFound(context);
                return false;
            }

            // 检查网络状态
            bool isOnline = WebHelpProvider.IsNetworkAvailable() && _webProvider.TestConnection();

            if (isOnline && _preferOnline)
            {
                // 在线模式：优先使用Web帮助
                if (_webProvider.HelpExists(context))
                {
                    bool success = _webProvider.ShowWebHelp(context);
                    if (success)
                    {
                        // 异步同步到本地缓存（可选）
                        SyncToLocalCacheAsync(context);
                        return true;
                    }
                }
                
                // Web帮助失败，回退到本地
                System.Diagnostics.Debug.WriteLine("在线帮助不可用，切换到本地缓存");
            }

            // 离线模式或Web失败：使用本地缓存
            if (_localProvider.HelpExists(context))
            {
                return _localProvider.ShowLocalHelp(context);
            }

            // 都失败了，显示未找到
            ShowHelpNotFound(context);
            return false;
        }

        /// <summary>
        /// 强制使用本地帮助（离线模式）
        /// </summary>
        public bool ShowLocalHelpOnly(HelpContext context)
        {
            if (_localProvider.HelpExists(context))
            {
                return _localProvider.ShowLocalHelp(context);
            }
            
            ShowHelpNotFound(context);
            return false;
        }

        /// <summary>
        /// 强制使用在线帮助
        /// </summary>
        public bool ShowWebHelpOnly(HelpContext context)
        {
            if (_webProvider.HelpExists(context))
            {
                return _webProvider.ShowWebHelp(context);
            }
            
            // 在线帮助不存在，询问是否使用本地
            if (_localProvider.HelpExists(context))
            {
                var result = MessageBox.Show(
                    "在线帮助暂无此内容，是否查看本地缓存版本？",
                    "帮助",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    return _localProvider.ShowLocalHelp(context);
                }
            }
            
            ShowHelpNotFound(context);
            return false;
        }

        /// <summary>
        /// 显示帮助未找到提示
        /// </summary>
        private void ShowHelpNotFound(HelpContext context)
        {
            string message = $"抱歉，未找到\"{context?.HelpKey ?? "当前功能"}\"的帮助内容。\n\n" +
                           "可能原因：\n" +
                           "1. 帮助文档尚未编写\n" +
                           "2. 网络连接失败且本地无缓存\n\n" +
                           "建议：\n" +
                           "• 检查网络连接后重试\n" +
                           "• 联系系统管理员反馈问题";

            MessageBox.Show(message, "帮助", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region 缓存同步

        /// <summary>
        /// 同步在线内容到本地缓存（异步）
        /// </summary>
        private async void SyncToLocalCacheAsync(HelpContext context)
        {
            try
            {
                // 这里可以实现从Web获取Markdown内容并保存到本地
                // 目前简化处理，实际使用时可以通过API获取
                await System.Threading.Tasks.Task.Delay(100);
            }
            catch { /* 忽略同步错误 */ }
        }

        /// <summary>
        /// 手动同步所有在线内容到本地
        /// </summary>
        public void SyncAllToLocal()
        {
            // TODO: 实现批量同步
            MessageBox.Show("同步功能开发中...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 获取缓存统计
        /// </summary>
        public string GetCacheStatistics()
        {
            return _localProvider.GetCacheStatistics();
        }

        #endregion

        #region 便捷方法

        /// <summary>
        /// 测试在线连接
        /// </summary>
        public bool TestOnlineConnection()
        {
            return WebHelpProvider.IsNetworkAvailable() && _webProvider.TestConnection();
        }

        /// <summary>
        /// 获取当前状态信息
        /// </summary>
        public string GetStatusInfo()
        {
            bool isOnline = WebHelpProvider.IsNetworkAvailable();
            bool canConnect = isOnline && _webProvider.TestConnection();
            
            return $"网络状态: {(isOnline ? "在线" : "离线")}\n" +
                   "在线帮助: " + (canConnect ? "可用" : "不可用") + "\n" +
                   "本地缓存: " + GetCacheStatistics();
        }

        #endregion
    }
}
