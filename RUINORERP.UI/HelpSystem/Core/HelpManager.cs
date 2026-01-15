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
        private Form _activeHelpPanel;

        /// <summary>
        /// 智能提示气泡组件
        /// </summary>
        private HelpTooltip _helpTooltip;

        /// <summary>
        /// URL 路由管理器
        /// </summary>
        private HelpUrlRouter _urlRouter;

        /// <summary>
        /// 是否启用 WebView2
        /// </summary>
        private bool _useWebView2 = true;

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 帮助内容缓存
        /// </summary>
        private Dictionary<string, string> _helpCache;

        /// <summary>
        /// 缓存锁对象
        /// </summary>
        private readonly object _cacheLock = new object();

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
        public bool EnableSmartTooltip { get; set; } = false;

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

        /// <summary>
        /// 是否启用 WebView2
        /// </summary>
        public bool UseWebView2
        {
            get => _useWebView2;
            set => _useWebView2 = value;
        }

        /// <summary>
        /// 是否启用远程帮助
        /// </summary>
        public bool EnableRemoteHelp
        {
            get => _urlRouter?.EnableRemoteHelp ?? false;
            set
            {
                if (_urlRouter != null)
                {
                    _urlRouter.EnableRemoteHelp = value;
                }
            }
        }

        /// <summary>
        /// 远程帮助服务器 URL
        /// </summary>
        public string RemoteHelpUrl
        {
            get => _urlRouter?.RemoteHelpUrl;
            set
            {
                if (_urlRouter != null)
                {
                    _urlRouter.RemoteHelpUrl = value;
                }
            }
        }

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

                // 创建 URL 路由管理器
                _urlRouter = new HelpUrlRouter(HelpContentRootPath);

                // 初始化帮助内容监控器（静态类，无需实例化）
                HelpContentMonitor.LoadMonitoringData();

                // 创建智能提示组件
                _helpTooltip = new HelpTooltip();
                _helpTooltip.Timeout = TooltipDelay;

                // 注册全局帮助事件
                RegisterGlobalHelpEvents();

                // 记录初始化日志
                System.Diagnostics.Debug.WriteLine("HelpManager 初始化成功");
                System.Diagnostics.Debug.WriteLine($"帮助内容路径: {HelpContentRootPath}");
                System.Diagnostics.Debug.WriteLine($"帮助数量: {_helpProvider.HelpCount}");
                System.Diagnostics.Debug.WriteLine($"WebView2 启用: {_useWebView2}");
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
        /// 支持智能解析，使用智能帮助解析器自动匹配实体和字段
        /// </summary>
        /// <param name="control">要显示帮助的控件</param>
        public void ShowControlHelp(Control control)
        {
            if (control == null)
            {
                return;
            }

            // 使用智能帮助解析器获取多个候选帮助键
            var resolver = new SmartHelpResolver();
            List<string> helpKeys = resolver.ResolveHelpKeys(control);

            if (helpKeys.Count == 0)
            {
                return;
            }

            // 按优先级尝试获取帮助内容，找到第一个存在的就使用
            foreach (string helpKey in helpKeys)
            {
                var context = new HelpContext
                {
                    Level = HelpLevel.Control,
                    TargetControl = control,
                    ControlName = control.Name,
                    HelpKey = helpKey
                };

                string helpContent = GetHelpContent(context);
                if (!string.IsNullOrEmpty(helpContent))
                {
                    DisplayHelp(helpContent, context);
                    return; // 找到帮助，直接返回
                }
            }

            // 所有候选键都没找到帮助，显示未找到提示
            var contextNotFound = HelpContext.FromControl(control);
            ShowHelpNotFound(contextNotFound);
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
        /// 显示 URL 帮助
        /// </summary>
        /// <param name="url">帮助 URL</param>
        public async System.Threading.Tasks.Task ShowUrlHelpAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            try
            {
                // 解析 URL
                var result = _urlRouter.ResolveUrl(url);

                if (!result.IsSuccess)
                {
                    MessageBox.Show(
                        $"无法解析帮助 URL: {result.ErrorMessage}",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // 根据结果类型处理
                if (result.Type == HelpUrlType.LocalFile)
                {
                    // 加载本地文件
                    string content = File.ReadAllText(result.ResolvedPath);
                    var context = HelpContext.FromUrl(url);
                    ShowHelpPanel(content, context);
                }
                else if (result.Type == HelpUrlType.RemotePage || result.Type == HelpUrlType.RemoteApi)
                {
                    // 加载远程 URL（需要使用 WebView2）
                    if (_activeHelpPanel != null && !_activeHelpPanel.IsDisposed)
                    {
                        _activeHelpPanel.Close();
                    }

                    if (_useWebView2)
                    {
                        var webView2Panel = new WebView2HelpPanel("", HelpContext.FromUrl(url));
                        _activeHelpPanel = webView2Panel;
                        await webView2Panel.LoadUrlAsync(result.ResolvedPath);
                        _activeHelpPanel.Show();
                    }
                    else
                    {
                        MessageBox.Show(
                            "远程帮助需要启用 WebView2。\n" +
                            "请在应用程序设置中启用 WebView2。",
                            "提示",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示 URL 帮助失败: {ex.Message}");
                MessageBox.Show(
                    $"显示 URL 帮助失败: {ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 构建帮助 URL
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        /// <param name="level">帮助级别</param>
        /// <param name="useRemote">是否使用远程帮助</param>
        /// <returns>帮助 URL</returns>
        public string BuildHelpUrl(string helpKey, HelpLevel level, bool useRemote = false)
        {
            return _urlRouter.BuildHelpUrl(helpKey, level, useRemote);
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
                // 确保上下文包含当前焦点控件
                EnsureContextHasFocusControl(context);

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
                    // 显示总帮助而不是未找到提示
                    ShowMainHelp(context);
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

        /// <summary>
        /// 确保帮助上下文包含当前焦点控件
        /// </summary>
        /// <param name="context">帮助上下文</param>
        private void EnsureContextHasFocusControl(HelpContext context)
        {
            if (context == null) return;

            // 如果上下文已有目标控件,则不需要处理
            if (context.TargetControl != null)
            {
                return;
            }

            // 尝试获取当前活动窗口
            Form activeForm = Form.ActiveForm;
            if (activeForm == null)
            {
                return;
            }

            // 获取当前焦点控件
            Control focusedControl = GetFocusedControl(activeForm);
            if (focusedControl != null)
            {
                context.TargetControl = focusedControl;
                context.ControlName = focusedControl.Name;
                context.FormType = activeForm.GetType();
                context.Level = HelpLevel.Control;
            }
        }

        /// <summary>
        /// 获取当前焦点控件
        /// </summary>
        /// <param name="form">目标窗体</param>
        /// <returns>焦点控件</returns>
        private Control GetFocusedControl(Form form)
        {
            if (form.ActiveControl != null)
            {
                return form.ActiveControl;
            }

            // 如果没有活动控件,尝试遍历查找
            return FindFocusedControl(form);
        }

        /// <summary>
        /// 递归查找焦点控件
        /// </summary>
        /// <param name="control">起始控件</param>
        /// <returns>焦点控件</returns>
        private Control FindFocusedControl(Control control)
        {
            if (control.Focused)
            {
                return control;
            }

            foreach (Control child in control.Controls)
            {
                Control focused = FindFocusedControl(child);
                if (focused != null)
                {
                    return focused;
                }
            }

            return null;
        }

        /// <summary>
        /// 显示总帮助
        /// </summary>
        /// <param name="context">帮助上下文</param>
        private void ShowMainHelp(HelpContext context)
        {
            try
            {
                // 创建总帮助上下文
                var mainHelpContext = new HelpContext
                {
                    Level = HelpLevel.Module,
                    ModuleName = "MainHelp",
                    TargetControl = context.TargetControl,
                    FormType = context.FormType
                };

                // 获取总帮助内容
                string mainHelpContent = GetHelpContent(mainHelpContext);

                if (string.IsNullOrEmpty(mainHelpContent))
                {
                    // 如果没有找到总帮助,生成默认总帮助
                    mainHelpContent = GenerateDefaultMainHelp();
                }

                // 显示总帮助
                DisplayHelp(mainHelpContent, mainHelpContext);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示总帮助失败: {ex.Message}");
                // 如果总帮助也显示失败,显示帮助未找到提示
                ShowHelpNotFound(context);
            }
        }

        /// <summary>
        /// 生成默认总帮助内容
        /// </summary>
        /// <returns>默认总帮助内容</returns>
        private string GenerateDefaultMainHelp()
        {
            return "# 总帮助\n\n" +
                   "## 欢迎使用销售订单系统\n\n" +
                   "本系统用于管理销售订单的录入、审核、执行和查询。\n\n" +
                   "### 主要功能\n\n" +
                   "- 销售订单录入\n" +
                   "- 订单审核流程\n" +
                   "- 订单执行跟踪\n" +
                   "- 订单查询和统计\n\n" +
                   "### 使用指南\n\n" +
                   "1. 在订单录入界面填写相关信息\n" +
                   "2. 保存订单并提交审核\n" +
                   "3. 审核通过后执行订单\n" +
                   "4. 执行完成后关闭订单\n\n" +
                   "### 快捷键\n\n" +
                   "- F1: 显示当前控件帮助\n" +
                   "- Ctrl+S: 保存订单\n" +
                   "- Ctrl+N: 新建订单\n\n" +
                   "### 联系我们\n\n" +
                   "如有任何问题，请联系系统管理员。";
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
        /// <param name="menuInfo">菜单信息(用于智能解析)</param>
        public void EnableSmartTooltipForAll(Control parent, string helpKeyPrefix = null, object menuInfo = null)
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

                // 如果提供了菜单信息，保存到控件Tag供智能解析使用
                if (menuInfo != null)
                {
                    if (control.Tag == null)
                    {
                        control.Tag = new Dictionary<string, object>();
                    }
                    else if (control.Tag is string)
                    {
                        // 如果Tag是字符串(可能是HelpKey)，转换为字典
                        var existingTag = control.Tag as string;
                        control.Tag = new Dictionary<string, object>
                        {
                            { "HelpKey", existingTag }
                        };
                    }

                    if (control.Tag is Dictionary<string, object> tagDict)
                    {
                        tagDict["MenuInfo"] = menuInfo;
                    }
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

                // 调用重载方法
                return GetHelpContent(helpKey, context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取帮助内容失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取帮助内容（重载方法，直接使用helpKey）
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助内容</returns>
        private string GetHelpContent(string helpKey, HelpContext context)
        {
            try
            {
                // 如果启用缓存,尝试从缓存获取
                if (EnableCache)
                {
                    if (_helpCache.TryGetValue(helpKey, out string cachedContent))
                    {
                        return cachedContent;
                    }
                }

                // 创建新的上下文，使用指定的helpKey
                var helpContext = new HelpContext
                {
                    Level = context?.Level ?? HelpLevel.Control,
                    TargetControl = context?.TargetControl,
                    FormType = context?.FormType,
                    EntityType = context?.EntityType,
                    FieldName = context?.FieldName,
                    ControlName = context?.ControlName,
                    ModuleName = context?.ModuleName,
                    HelpKey = helpKey,
                    AdditionalInfo = context?.AdditionalInfo
                };

                // 从提供者获取
                string content = _helpProvider.GetHelpContent(helpContext);

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

                // 根据设置选择使用 WebView2 还是传统 WebBrowser
                Form helpPanel;
                
                if (_useWebView2)
                {
                    try
                    {
                        // 尝试使用 WebView2 帮助面板
                        helpPanel = new WebView2HelpPanel(content, context);
                        System.Diagnostics.Debug.WriteLine("使用 WebView2 帮助面板");
                    }
                    catch (Exception ex)
                    {
                        // WebView2 初始化失败,降级到传统方式
                        System.Diagnostics.Debug.WriteLine($"WebView2 初始化失败,降级到传统方式: {ex.Message}");
                        helpPanel = new HelpPanel(content, context);
                    }
                }
                else
                {
                    // 使用传统 WebBrowser
                    helpPanel = new HelpPanel(content, context);
                }

                _activeHelpPanel = helpPanel;
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
                // 记录缺失的帮助内容
                HelpContentMonitor.LogMissingHelp(
                    context.HelpKey,
                    context.Level,
                    context.ControlName,
                    context.EntityType);

                // 使用默认帮助内容生成器生成帮助内容
                string defaultHelp = GenerateDefaultHelpContent(context);

                if (EnableSmartTooltip && context.TargetControl != null)
                {
                    // 对于控件级别帮助,使用Tooltip显示
                    _helpTooltip.Show(defaultHelp, context.TargetControl);
                }
                else
                {
                    // 对于窗体级别或没有智能提示的情况,使用HelpPanel显示
                    DisplayHelp(defaultHelp, context);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示未找到帮助提示失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成默认帮助内容
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>默认帮助内容</returns>
        private string GenerateDefaultHelpContent(HelpContext context)
        {
            try
            {
                switch (context.Level)
                {
                    case HelpLevel.Control:
                        // 控件级别帮助
                        return DefaultHelpContentGenerator.GenerateDefaultControlHelp(
                            context.TargetControl,
                            context.EntityType);

                    case HelpLevel.Form:
                        // 窗体级别帮助
                        return DefaultHelpContentGenerator.GenerateDefaultFormHelp(
                            context.FormType?.GetType(),
                            context.EntityType);

                    case HelpLevel.Module:
                        // 实体级别帮助
                        if (context.EntityType != null)
                        {
                            return DefaultHelpContentGenerator.GenerateDefaultFormHelp(
                                null,
                                context.EntityType);
                        }
                        return DefaultHelpContentGenerator.GenerateGlobalHelp();

                    default:
                        // 默认返回全局帮助
                        return DefaultHelpContentGenerator.GenerateGlobalHelp();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"生成默认帮助内容失败: {ex.Message}");
                return DefaultHelpContentGenerator.GenerateMissingHelpPlaceholder(context.HelpKey);
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
