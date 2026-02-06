using System;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem.Core;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助启动器
    /// 提供统一的帮助调用接口，封装复杂的帮助系统初始化逻辑
    /// </summary>
    public static class HelpLauncher
    {
        #region 私有字段

        private static EnhancedCompositeHelpProvider _compositeProvider;
        private static bool _isInitialized = false;
        
        // 默认配置
        private static string _defaultBaseUrl = "http://localhost:8000";
        private static string _defaultCacheDirectory = @".\HelpContent";

        #endregion

        #region 公共属性

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public static bool IsInitialized => _isInitialized;

        /// <summary>
        /// 组合帮助提供者
        /// </summary>
        public static EnhancedCompositeHelpProvider Provider => _compositeProvider;

        /// <summary>
        /// 在线帮助基础URL
        /// </summary>
        public static string BaseUrl => _defaultBaseUrl;

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化帮助系统
        /// 应在应用程序启动时调用一次
        /// </summary>
        /// <param name="baseUrl">在线帮助网站URL</param>
        /// <param name="cacheDirectory">本地缓存目录</param>
        public static void Initialize(string baseUrl = null, string cacheDirectory = null)
        {
            if (_isInitialized)
                return;

            try
            {
                string url = baseUrl ?? _defaultBaseUrl;
                string cache = cacheDirectory ?? _defaultCacheDirectory;

                _compositeProvider = new EnhancedCompositeHelpProvider(url, cache);
                
                // 注册到HelpManager
                HelpManager.Instance.HelpProvider = _compositeProvider;
                
                _isInitialized = true;

                System.Diagnostics.Debug.WriteLine($"帮助系统初始化成功：{url}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"帮助系统初始化失败：{ex.Message}");
                _isInitialized = false;
            }
        }

        /// <summary>
        /// 使用现有提供者初始化
        /// </summary>
        public static void Initialize(EnhancedCompositeHelpProvider provider)
        {
            if (_isInitialized)
                return;

            _compositeProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            HelpManager.Instance.HelpProvider = provider;
            _isInitialized = true;
        }

        #endregion

        #region 显示帮助方法

        /// <summary>
        /// 显示帮助（智能模式）
        /// 自动选择在线或本地，这是主要的帮助调用方法
        /// </summary>
        /// <param name="helpKey">帮助键，如 "UCSaleOrder"</param>
        public static void ShowHelp(string helpKey)
        {
            EnsureInitialized();

            var context = new HelpContext
            {
                HelpKey = helpKey,
                Level = HelpLevel.Form
            };

            _compositeProvider.ShowHelp(context);
        }

        /// <summary>
        /// 显示帮助（从控件上下文）
        /// </summary>
        /// <param name="control">目标控件</param>
        public static void ShowHelp(System.Windows.Forms.Control control)
        {
            EnsureInitialized();

            var context = HelpContext.FromControl(control);
            if (context != null)
            {
                _compositeProvider.ShowHelp(context);
            }
        }

        /// <summary>
        /// 显示窗体帮助
        /// </summary>
        /// <param name="formType">窗体类型</param>
        public static void ShowFormHelp(Type formType)
        {
            EnsureInitialized();

            if (formType == null)
                return;

            var context = new HelpContext
            {
                HelpKey = formType.Name,
                Level = HelpLevel.Form,
                FormType = formType
            };

            _compositeProvider.ShowHelp(context);
        }

        /// <summary>
        /// 显示字段帮助
        /// </summary>
        public static void ShowFieldHelp(Type entityType, string fieldName)
        {
            EnsureInitialized();

            var context = HelpContext.FromField(entityType, fieldName);
            if (context != null)
            {
                _compositeProvider.ShowHelp(context);
            }
        }

        /// <summary>
        /// 强制使用在线帮助
        /// </summary>
        public static void ShowWebHelp(string helpKey)
        {
            EnsureInitialized();

            var context = new HelpContext
            {
                HelpKey = helpKey,
                Level = HelpLevel.Form
            };

            _compositeProvider.ShowWebHelpOnly(context);
        }

        /// <summary>
        /// 强制使用本地帮助（离线模式）
        /// </summary>
        public static void ShowLocalHelp(string helpKey)
        {
            EnsureInitialized();

            var context = new HelpContext
            {
                HelpKey = helpKey,
                Level = HelpLevel.Form
            };

            _compositeProvider.ShowLocalHelpOnly(context);
        }

        #endregion

        #region 搜索和查询

        /// <summary>
        /// 搜索帮助
        /// </summary>
        public static void SearchHelp(string keyword)
        {
            EnsureInitialized();

            var results = _compositeProvider.Search(keyword);
            
            // 显示搜索结果（可以改进为显示在专门的搜索窗口）
            if (results.Count > 0)
            {
                ShowHelp(results[0].HelpKey);
            }
            else
            {
                MessageBox.Show($"未找到包含\"{keyword}\"的帮助内容", 
                    "搜索", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 检查帮助是否存在
        /// </summary>
        public static bool HelpExists(string helpKey)
        {
            EnsureInitialized();

            var context = new HelpContext
            {
                HelpKey = helpKey,
                Level = HelpLevel.Form
            };

            return _compositeProvider.HelpExists(context);
        }

        #endregion

        #region 配置方法

        /// <summary>
        /// 设置在线帮助URL
        /// </summary>
        public static void SetBaseUrl(string baseUrl)
        {
            _defaultBaseUrl = baseUrl;
            
            // 如果已初始化，需要重新初始化
            if (_isInitialized)
            {
                Shutdown();
                Initialize(baseUrl);
            }
        }

        /// <summary>
        /// 设置本地缓存目录
        /// </summary>
        public static void SetCacheDirectory(string cacheDirectory)
        {
            _defaultCacheDirectory = cacheDirectory;
        }

        /// <summary>
        /// 设置是否优先使用在线帮助
        /// </summary>
        public static void SetPreferOnline(bool preferOnline)
        {
            if (_compositeProvider != null)
            {
                _compositeProvider.PreferOnline = preferOnline;
            }
        }

        /// <summary>
        /// 测试在线连接
        /// </summary>
        public static bool TestOnlineConnection()
        {
            if (_compositeProvider == null)
                return false;

            return _compositeProvider.TestOnlineConnection();
        }

        /// <summary>
        /// 获取状态信息
        /// </summary>
        public static string GetStatusInfo()
        {
            if (_compositeProvider == null)
                return "帮助系统未初始化";

            return _compositeProvider.GetStatusInfo();
        }

        #endregion

        #region 生命周期管理

        /// <summary>
        /// 关闭帮助系统
        /// </summary>
        public static void Shutdown()
        {
            _compositeProvider?.Dispose();
            _compositeProvider = null;
            _isInitialized = false;
        }

        /// <summary>
        /// 重新加载帮助索引
        /// </summary>
        public static void Reload()
        {
            _compositeProvider?.ReloadIndex();
        }

        #endregion

        #region 私有方法

        private static void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }

        #endregion
    }
}
