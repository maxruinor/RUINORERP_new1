using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem.Components;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助管理器 - 协调整个帮助系统的核心类
    /// 采用单例模式,全局唯一实例
    /// 职责: 帮助上下文识别、帮助内容加载、帮助显示触发、智能提示管理
    /// </summary>
    public class HelpManager : IDisposable
    {
        #region 单例模式

        /// <summary>
        /// 单例实例
        /// </summary>
        private static readonly Lazy<HelpManager> _instance = 
            new Lazy<HelpManager>(() => new HelpManager());

        /// <summary>
        /// 获取帮助管理器单例
        /// </summary>
        public static HelpManager Instance => _instance.Value;

        #endregion

        #region 私有字段

        /// <summary>
        /// 帮助提供者接口
        /// </summary>
        private IHelpProvider _helpProvider;

        /// <summary>
        /// 当前显示的帮助面板
        /// </summary>
        private HelpPanel _activeHelpPanel;

        /// <summary>
        /// 智能提示气泡组件
        /// </summary>
        private HelpTooltip _helpTooltip;

        /// <summary>
        /// 帮助内容缓存
        /// 键: 帮助键, 值: 帮助内容
        /// </summary>
        private Dictionary<string, string> _helpCache;

        /// <summary>
        /// 缓存锁对象
        /// </summary>
        private readonly object _cacheLock = new object();

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region 公共属性

        /// <summary>
        /// 帮助内容根路径
        /// </summary>
        public string HelpContentRootPath { get; private set; }

        /// <summary>
        /// 当前帮助提供者
        /// </summary>
        public IHelpProvider HelpProvider
        {
            get => _helpProvider;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                // 释放旧的提供者
                _helpProvider?.Dispose();

                // 设置新的提供者
                _helpProvider = value;
            }
        }

        /// <summary>
        /// 是否启用智能提示
        /// </summary>
        public bool EnableSmartTooltip { get; set; } = true;

        /// <summary>
        /// 智能提示延迟时间(毫秒)
        /// </summary>
        public int TooltipDelay { get; set; } = 500;

        /// <summary>
        /// 是否启用帮助缓存
        /// </summary>
        public bool EnableCache { get; set; } = true;

        /// <summary>
        /// 缓存最大容量
        /// </summary>
        public int CacheMaxSize { get; set; } = 100;

        #endregion

        #region 构造函数

        /// <summary>
        /// 私有构造函数(单例模式)
        /// </summary>
        private HelpManager()
        {
            Initialize();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化帮助管理器
        /// </summary>
        private void Initialize()
        {
            try
            {
                // 设置帮助内容根路径
                HelpContentRootPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    "HelpContent");

                // 如果目录不存在,创建它
                if (!Directory.Exists(HelpContentRootPath))
                {
                    Directory.CreateDirectory(HelpContentRootPath);
                }

                // 初始化缓存
                _helpCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                // 创建帮助提供者(默认使用本地帮助提供者)
                _helpProvider = new LocalHelpProvider(HelpContentRootPath);

                // 创建智能提示组件
                _helpTooltip = new HelpTooltip();
                _helpTooltip.Timeout = TooltipDelay;

                // 注册全局帮助事件
                RegisterGlobalHelpEvents();

                // 记录初始化日志
                System.Diagnostics.Debug.WriteLine("HelpManager 初始化成功");
                System.Diagnostics.Debug.WriteLine($"帮助内容路径: {HelpContentRootPath}");
                System.Diagnostics.Debug.WriteLine($"帮助数量: {_helpProvider.HelpCount}");
            }
            catch (Exception ex)
            {
                // 记录初始化错误
                System.Diagnostics.Debug.WriteLine($"HelpManager 初始化失败: {ex.Message}");
                
                // 尝试记录到主窗体日志
                try
                {
                    System.Diagnostics.Debug.WriteLine($"HelpManager 初始化失败: {ex.Message}");
                }
                catch
                {
                    // 忽略日志记录错误
                }
            }
        }

        /// <summary>
        /// 注册全局帮助事件
        /// </summary>
        private void RegisterGlobalHelpEvents()
        {
            // 这里可以注册应用程序级别的事件
            // 例如: Application.Idle += Application_Idle;
            // 当前版本暂不需要
        }

        #endregion

        #region 公共方法 - 帮助显示

        /// <summary>
        /// 显示窗体级别的帮助
        /// </summary>
        /// <param name="form">要显示帮助的窗体</param>
        public void ShowFormHelp(Form form)
        {
            if (form == null)
            {
                return;
            }

            var context = HelpContext.FromForm(form);
            ShowHelp(context);
        }

        /// <summary>
        /// 显示控件级别的帮助
        /// </summary>
        /// <param name="control">要显示帮助的控件</param>
        public void ShowControlHelp(Control control)
        {
            if (control == null)
            {
                return;
            }

            var context = HelpContext.FromControl(control);
            ShowHelp(context);
        }

        /// <summary>
        /// 显示字段级别的帮助
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="fieldName">字段名称</param>
        public void ShowFieldHelp(Type entityType, string fieldName)
        {
            if (entityType == null || string.IsNullOrEmpty(fieldName))
            {
                return;
            }

            var context = HelpContext.FromField(entityType, fieldName);
            ShowHelp(context);
        }

        /// <summary>
        /// 显示模块级别的帮助
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        public void ShowModuleHelp(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return;
            }

            var context = HelpContext.FromModule(moduleName);
            ShowHelp(context);
        }

        /// <summary>
        /// 根据帮助上下文显示帮助
        /// 这是核心的显示方法,所有其他显示方法都调用此方法
        /// </summary>
        /// <param name="context">帮助上下文</param>
        public void ShowHelp(HelpContext context)
        {
            if (context == null || _disposed)
            {
                return;
            }

            try
            {
                // 获取帮助内容
                string helpContent = GetHelpContent(context);

                // 如果没有找到精确匹配,尝试智能搜索
                if (string.IsNullOrEmpty(helpContent))
                {
                    helpContent = SmartSearch(context);
                }

                // 显示帮助
                if (!string.IsNullOrEmpty(helpContent))
                {
                    DisplayHelp(helpContent, context);
                }
                else
                {
                    // 显示未找到帮助的提示
                    ShowHelpNotFound(context);
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                System.Diagnostics.Debug.WriteLine($"显示帮助时发生错误: {ex.Message}");
                
                try
                {
                    // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"显示帮助时发生错误: {ex.Message}");
                }
                catch
                {
                    // 忽略日志记录错误
                }

                // 显示错误提示
                MessageBox.Show(
                    $"显示帮助时发生错误: {ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 公共方法 - 智能提示

        /// <summary>
        /// 为控件启用智能提示
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="helpKey">帮助键(可选)</param>
        public void EnableSmartTooltipForControl(Control control, string helpKey = null)
        {
            if (control == null || !EnableSmartTooltip)
            {
                return;
            }

            try
            {
                // 移除已存在的事件处理器
                control.MouseHover -= Control_MouseHover;
                control.MouseLeave -= Control_MouseLeave;

                // 添加事件处理器
                control.MouseHover += Control_MouseHover;
                control.MouseLeave += Control_MouseLeave;

                // 如果指定了帮助键,保存到Tag
                if (!string.IsNullOrEmpty(helpKey))
                {
                    control.Tag = $"HelpKey:{helpKey}";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"启用智能提示失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 为所有子控件启用智能提示
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="helpKeyPrefix">帮助键前缀(可选)</param>
        public void EnableSmartTooltipForAll(Control parent, string helpKeyPrefix = null)
        {
            if (parent == null)
            {
                return;
            }

            foreach (Control control in GetAllControls(parent))
            {
                string helpKey = null;

                if (!string.IsNullOrEmpty(helpKeyPrefix))
                {
                    helpKey = $"{helpKeyPrefix}.{control.Name}";
                }

                EnableSmartTooltipForControl(control, helpKey);
            }
        }

        #endregion

        #region 公共方法 - 帮助搜索

        /// <summary>
        /// 搜索帮助内容
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="context">当前上下文(用于优先排序)</param>
        /// <returns>搜索结果列表</returns>
        public List<HelpSearchResult> SearchHelp(string keyword, HelpContext context = null)
        {
            if (string.IsNullOrEmpty(keyword) || _disposed)
            {
                return new List<HelpSearchResult>();
            }

            try
            {
                return _helpProvider.Search(keyword, context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"搜索帮助失败: {ex.Message}");
                return new List<HelpSearchResult>();
            }
        }

        #endregion

        #region 私有方法 - 帮助内容获取

        /// <summary>
        /// 获取帮助内容
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助内容</returns>
        private string GetHelpContent(HelpContext context)
        {
            try
            {
                // 生成帮助键
                string helpKey = GenerateHelpKey(context);

                // 如果启用缓存,尝试从缓存获取
                if (EnableCache)
                {
                    if (_helpCache.TryGetValue(helpKey, out string cachedContent))
                    {
                        return cachedContent;
                    }
                }

                // 从提供者获取
                string content = _helpProvider.GetHelpContent(context);

                // 如果启用缓存且内容有效,缓存内容
                if (EnableCache && !string.IsNullOrEmpty(content))
                {
                    lock (_cacheLock)
                    {
                        // 如果缓存已满,移除最旧的项
                        if (_helpCache.Count >= CacheMaxSize)
                        {
                            string oldestKey = _helpCache.Keys.First();
                            _helpCache.Remove(oldestKey);
                        }

                        // 添加新的缓存项
                        if (!_helpCache.ContainsKey(helpKey))
                        {
                            _helpCache[helpKey] = content;
                        }
                    }
                }

                return content;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取帮助内容失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 生成帮助键
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助键</returns>
        private string GenerateHelpKey(HelpContext context)
        {
            switch (context.Level)
            {
                case HelpLevel.Field:
                    return $"field.{context.EntityType?.Name}.{context.FieldName}";

                case HelpLevel.Control:
                    return $"control.{context.FormType?.Name}.{context.ControlName}";

                case HelpLevel.Form:
                    return $"form.{context.FormType?.Name}";

                case HelpLevel.Module:
                    return $"module.{context.ModuleName}";

                default:
                    return context.HelpKey ?? "default";
            }
        }

        /// <summary>
        /// 智能搜索
        /// 当精确匹配失败时,尝试从实体层或搜索相关帮助
        /// </summary>
        /// <param name="context">原始上下文</param>
        /// <returns>帮助内容</returns>
        private string SmartSearch(HelpContext context)
        {
            try
            {
                // 如果是字段级别帮助,尝试在实体中查找
                if (context.Level == HelpLevel.Field && context.EntityType != null)
                {
                    try
                    {
                        var entity = Startup.GetFromFacByName<RUINORERP.Model.BaseEntity>(context.EntityType.Name);
                        if (entity?.HelpInfos != null && 
                            entity.HelpInfos.ContainsKey(context.FieldName))
                        {
                            string fieldName = entity.FieldNameList.ContainsKey(context.FieldName) 
                                ? entity.FieldNameList[context.FieldName].Trim() 
                                : context.FieldName;
                            
                            // 格式化帮助内容
                            string helpContent = $"<h1>{fieldName}</h1>\n" +
                                             "<h2>说明</h2>\n" +
                                             $"<p>{entity.HelpInfos[context.FieldName]}</p>";

                            return helpContent;
                        }
                    }
                    catch
                    {
                        // 忽略异常,继续其他搜索策略
                    }
                }

                // 尝试搜索相关的帮助
                var searchResults = _helpProvider.Search(context.HelpKey ?? "", context);
                if (searchResults.Any())
                {
                    return searchResults.First().Content;
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"智能搜索失败: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region 私有方法 - 帮助显示

        /// <summary>
        /// 显示帮助内容
        /// </summary>
        /// <param name="content">帮助内容</param>
        /// <param name="context">帮助上下文</param>
        private void DisplayHelp(string content, HelpContext context)
        {
            try
            {
                // 判断内容长度,决定显示方式
                bool isShortContent = content.Length < 300 && content.Split('\n').Length < 8;

                // 如果内容简短且启用智能提示,显示为工具提示
                if (isShortContent && EnableSmartTooltip && context.TargetControl != null)
                {
                    _helpTooltip.Show(content, context.TargetControl);
                }
                else
                {
                    // 显示完整的帮助面板
                    ShowHelpPanel(content, context);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示帮助内容失败: {ex.Message}");
                MessageBox.Show(
                    $"显示帮助内容失败: {ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示帮助面板
        /// </summary>
        /// <param name="content">帮助内容</param>
        /// <param name="context">帮助上下文</param>
        private void ShowHelpPanel(string content, HelpContext context)
        {
            try
            {
                // 如果已有帮助面板在显示,先关闭它
                if (_activeHelpPanel != null && !_activeHelpPanel.IsDisposed)
                {
                    _activeHelpPanel.Close();
                }

                // 创建新的帮助面板
                _activeHelpPanel = new HelpPanel(content, context);
                _activeHelpPanel.Show();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示帮助面板失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 显示未找到帮助的提示
        /// </summary>
        /// <param name="context">帮助上下文</param>
        private void ShowHelpNotFound(HelpContext context)
        {
            try
            {
                string message = $"未找到相关帮助信息。\n\n" +
                               $"类型: {context.Level}\n" +
                               $"键: {context.HelpKey}";

                if (EnableSmartTooltip && context.TargetControl != null)
                {
                    _helpTooltip.Show(message, context.TargetControl);
                }
                else
                {
                    MessageBox.Show(
                        message, 
                        "提示", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示未找到帮助提示失败: {ex.Message}");
            }
        }

        #endregion

        #region 私有方法 - 控件遍历

        /// <summary>
        /// 获取所有控件(包括子控件)
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <returns>所有控件列表</returns>
        private IEnumerable<Control> GetAllControls(Control parent)
        {
            var controls = new List<Control> { parent };

            foreach (Control child in parent.Controls)
            {
                controls.AddRange(GetAllControls(child));
            }

            return controls;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 控件悬停事件处理
        /// </summary>
        private void Control_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (!(sender is Control control))
                {
                    return;
                }

                var context = HelpContext.FromControl(control);
                ShowHelp(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"控件悬停事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 控件鼠标离开事件处理
        /// </summary>
        private void Control_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                _helpTooltip?.Hide();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"控件鼠标离开事件处理失败: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法 - 缓存管理

        /// <summary>
        /// 清除帮助缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_cacheLock)
            {
                _helpCache.Clear();
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存项数量</returns>
        public int GetCacheCount()
        {
            lock (_cacheLock)
            {
                return _helpCache.Count;
            }
        }

        #endregion

        #region 公共方法 - 提供者管理

        /// <summary>
        /// 重新加载帮助索引
        /// </summary>
        public void ReloadHelpIndex()
        {
            try
            {
                _helpProvider?.ReloadIndex();
                ClearCache(); // 清除缓存以加载新内容
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"重新加载帮助索引失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取帮助统计信息
        /// </summary>
        /// <returns>帮助数量</returns>
        public int GetHelpCount()
        {
            try
            {
                return _helpProvider?.HelpCount ?? 0;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的实际实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    _helpProvider?.Dispose();
                    _helpTooltip?.Dispose();
                    _activeHelpPanel?.Dispose();
                    _helpCache?.Clear();
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~HelpManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
