using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助 URL 路由管理器
    /// 负责处理帮助内容的 URL 路由，支持本地文件和远程 URL
    /// 为未来的远程帮助系统提供基础架构
    /// </summary>
    public class HelpUrlRouter
    {
        #region 私有字段

        /// <summary>
        /// URL 路由规则
        /// 键: URL 模式, 值: 处理器
        /// </summary>
        private Dictionary<string, HelpUrlHandler> _routes;

        /// <summary>
        /// 基础 URL（用于相对路径）
        /// </summary>
        private string _baseUrl;

        /// <summary>
        /// 本地帮助文件根目录
        /// </summary>
        private string _localHelpPath;

        /// <summary>
        /// 远程帮助服务器 URL
        /// </summary>
        private string _remoteHelpUrl;

        /// <summary>
        /// 是否启用远程帮助
        /// </summary>
        private bool _enableRemoteHelp;

        #endregion

        #region 公共属性

        /// <summary>
        /// 基础 URL
        /// </summary>
        public string BaseUrl
        {
            get => _baseUrl;
            set
            {
                _baseUrl = value;
                NormalizeBaseUrl();
            }
        }

        /// <summary>
        /// 本地帮助文件根目录
        /// </summary>
        public string LocalHelpPath
        {
            get => _localHelpPath;
            set
            {
                _localHelpPath = value;
                if (!Directory.Exists(_localHelpPath))
                {
                    Directory.CreateDirectory(_localHelpPath);
                }
            }
        }

        /// <summary>
        /// 远程帮助服务器 URL
        /// </summary>
        public string RemoteHelpUrl
        {
            get => _remoteHelpUrl;
            set
            {
                _remoteHelpUrl = value;
                NormalizeRemoteUrl();
            }
        }

        /// <summary>
        /// 是否启用远程帮助
        /// </summary>
        public bool EnableRemoteHelp
        {
            get => _enableRemoteHelp;
            set => _enableRemoteHelp = value;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public HelpUrlRouter()
        {
            _routes = new Dictionary<string, HelpUrlHandler>(StringComparer.OrdinalIgnoreCase);
            InitializeDefaultRoutes();
        }

        /// <summary>
        /// 构造函数（带参数）
        /// </summary>
        /// <param name="localHelpPath">本地帮助文件根目录</param>
        /// <param name="remoteHelpUrl">远程帮助服务器 URL</param>
        public HelpUrlRouter(string localHelpPath, string remoteHelpUrl = null)
            : this()
        {
            LocalHelpPath = localHelpPath;
            RemoteHelpUrl = remoteHelpUrl;
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化默认路由规则
        /// </summary>
        private void InitializeDefaultRoutes()
        {
            // 本地文件路由
            // help://local/form/{formName}
            RegisterRoute(@"^help://local/form/(.+)$", ResolveLocalFormHelp);
            
            // help://local/control/{formName}/{controlName}
            RegisterRoute(@"^help://local/control/(.+)/(.+)$", ResolveLocalControlHelp);
            
            // help://local/field/{entityName}/{fieldName}
            RegisterRoute(@"^help://local/field/(.+)/(.+)$", ResolveLocalFieldHelp);
            
            // help://local/module/{moduleName}
            RegisterRoute(@"^help://local/module/(.+)$", ResolveLocalModuleHelp);
            
            // 本地文件路由（直接文件路径）
            // help://file/{path}
            RegisterRoute(@"^help://file/(.+)$", ResolveLocalFileHelp);
            
            // 远程帮助路由
            // help://remote/api/help/{helpKey}
            RegisterRoute(@"^help://remote/api/help/(.+)$", ResolveRemoteHelp);
            
            // help://remote/page/{pagePath}
            RegisterRoute(@"^help://remote/page/(.+)$", ResolveRemotePage);
            
            // HTTP/HTTPS 远程链接
            RegisterRoute(@"^https?://.+\.example\.com/help/(.+)$", ResolveRemoteHttpHelp);
            
            // GitHub Pages 帮助
            RegisterRoute(@"^https?://.+\.github\.io/ruinorerp-help/(.+)$", ResolveGitHubPagesHelp);
        }

        /// <summary>
        /// 规范化基础 URL
        /// </summary>
        private void NormalizeBaseUrl()
        {
            if (!string.IsNullOrEmpty(_baseUrl))
            {
                if (!_baseUrl.EndsWith("/"))
                {
                    _baseUrl += "/";
                }
            }
        }

        /// <summary>
        /// 规范化远程 URL
        /// </summary>
        private void NormalizeRemoteUrl()
        {
            if (!string.IsNullOrEmpty(_remoteHelpUrl))
            {
                if (!_remoteHelpUrl.EndsWith("/"))
                {
                    _remoteHelpUrl += "/";
                }
                
                // 确保 URL 以 http:// 或 https:// 开头
                if (!_remoteHelpUrl.StartsWith("http://") && !_remoteHelpUrl.StartsWith("https://"))
                {
                    _remoteHelpUrl = "https://" + _remoteHelpUrl;
                }
            }
        }

        #endregion

        #region 路由管理

        /// <summary>
        /// 注册路由规则
        /// </summary>
        /// <param name="pattern">URL 模式（正则表达式）</param>
        /// <param name="handler">处理器</param>
        public void RegisterRoute(string pattern, HelpUrlHandler handler)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _routes[pattern] = handler;
        }

        /// <summary>
        /// 注销路由规则
        /// </summary>
        /// <param name="pattern">URL 模式</param>
        public bool UnregisterRoute(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            return _routes.Remove(pattern);
        }

        /// <summary>
        /// 清除所有路由规则
        /// </summary>
        public void ClearRoutes()
        {
            _routes.Clear();
            InitializeDefaultRoutes();
        }

        #endregion

        #region URL 解析

        /// <summary>
        /// 解析 URL 并返回帮助内容路径
        /// </summary>
        /// <param name="url">要解析的 URL</param>
        /// <returns>解析结果</returns>
        public HelpUrlResolutionResult ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return HelpUrlResolutionResult.Failure("URL 为空");
            }

            try
            {
                // 遍历所有路由规则
                foreach (var route in _routes)
                {
                    var match = Regex.Match(url, route.Key, RegexOptions.IgnoreCase);
                    
                    if (match.Success)
                    {
                        try
                        {
                            // 调用处理器
                            return route.Value(url, match);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"路由处理器执行失败: {ex.Message}");
                            continue;
                        }
                    }
                }

                // 没有匹配的路由
                return HelpUrlResolutionResult.Failure($"未找到匹配的路由: {url}");
            }
            catch (Exception ex)
            {
                return HelpUrlResolutionResult.Failure($"URL 解析失败: {ex.Message}");
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
            try
            {
                if (useRemote && EnableRemoteHelp && !string.IsNullOrEmpty(_remoteHelpUrl))
                {
                    // 构建远程帮助 URL
                    switch (level)
                    {
                        case HelpLevel.Form:
                            return $"{_remoteHelpUrl}api/help/form/{helpKey}";
                        
                        case HelpLevel.Control:
                            return $"{_remoteHelpUrl}api/help/control/{helpKey}";
                        
                        case HelpLevel.Field:
                            return $"{_remoteHelpUrl}api/help/field/{helpKey}";
                        
                        case HelpLevel.Module:
                            return $"{_remoteHelpUrl}api/help/module/{helpKey}";
                        
                        default:
                            return $"{_remoteHelpUrl}api/help/{helpKey}";
                    }
                }
                else
                {
                    // 构建本地帮助 URL
                    switch (level)
                    {
                        case HelpLevel.Form:
                            return $"help://local/form/{helpKey}";
                        
                        case HelpLevel.Control:
                            return $"help://local/control/{helpKey}";
                        
                        case HelpLevel.Field:
                            return $"help://local/field/{helpKey}";
                        
                        case HelpLevel.Module:
                            return $"help://local/module/{helpKey}";
                        
                        default:
                            return $"help://local/{helpKey}";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"构建帮助 URL 失败: {ex.Message}");
                return $"help://local/{helpKey}";
            }
        }

        /// <summary>
        /// 构建本地文件 URL
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>本地文件 URL</returns>
        public string BuildFileUrl(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return null;
            }

            // 确保路径使用正斜杠
            string normalizedPath = relativePath.Replace("\\", "/");
            
            return $"help://file/{normalizedPath}";
        }

        #endregion

        #region 路由处理器

        /// <summary>
        /// 解析本地窗体帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveLocalFormHelp(string url, Match match)
        {
            string formName = match.Groups[1].Value;
            string filePath = Path.Combine(LocalHelpPath, "forms", $"{formName}.md");
            
            if (File.Exists(filePath))
            {
                return HelpUrlResolutionResult.Success(filePath, HelpUrlType.LocalFile);
            }
            
            // 尝试 HTML 文件
            filePath = Path.Combine(LocalHelpPath, "forms", $"{formName}.html");
            if (File.Exists(filePath))
            {
                return HelpUrlResolutionResult.Success(filePath, HelpUrlType.LocalFile);
            }
            
            return HelpUrlResolutionResult.Failure($"未找到窗体帮助文件: {formName}");
        }

        /// <summary>
        /// 解析本地控件帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveLocalControlHelp(string url, Match match)
        {
            string formName = match.Groups[1].Value;
            string controlName = match.Groups[2].Value;
            string filePath = Path.Combine(LocalHelpPath, "controls", formName, $"{controlName}.md");
            
            if (File.Exists(filePath))
            {
                return HelpUrlResolutionResult.Success(filePath, HelpUrlType.LocalFile);
            }
            
            return HelpUrlResolutionResult.Failure($"未找到控件帮助文件: {formName}/{controlName}");
        }

        /// <summary>
        /// 解析本地字段帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveLocalFieldHelp(string url, Match match)
        {
            string entityName = match.Groups[1].Value;
            string fieldName = match.Groups[2].Value;
            string filePath = Path.Combine(LocalHelpPath, "fields", entityName, $"{fieldName}.md");
            
            if (File.Exists(filePath))
            {
                return HelpUrlResolutionResult.Success(filePath, HelpUrlType.LocalFile);
            }
            
            return HelpUrlResolutionResult.Failure($"未找到字段帮助文件: {entityName}/{fieldName}");
        }

        /// <summary>
        /// 解析本地模块帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveLocalModuleHelp(string url, Match match)
        {
            string moduleName = match.Groups[1].Value;
            string filePath = Path.Combine(LocalHelpPath, "modules", $"{moduleName}.md");
            
            if (File.Exists(filePath))
            {
                return HelpUrlResolutionResult.Success(filePath, HelpUrlType.LocalFile);
            }
            
            return HelpUrlResolutionResult.Failure($"未找到模块帮助文件: {moduleName}");
        }

        /// <summary>
        /// 解析本地文件帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveLocalFileHelp(string url, Match match)
        {
            string relativePath = match.Groups[1].Value;
            string filePath = Path.Combine(LocalHelpPath, relativePath);
            
            if (File.Exists(filePath))
            {
                return HelpUrlResolutionResult.Success(filePath, HelpUrlType.LocalFile);
            }
            
            return HelpUrlResolutionResult.Failure($"未找到文件: {relativePath}");
        }

        /// <summary>
        /// 解析远程帮助 API
        /// </summary>
        private HelpUrlResolutionResult ResolveRemoteHelp(string url, Match match)
        {
            if (!EnableRemoteHelp || string.IsNullOrEmpty(_remoteHelpUrl))
            {
                return HelpUrlResolutionResult.Failure("远程帮助未启用");
            }
            
            string helpKey = match.Groups[1].Value;
            string remoteUrl = $"{_remoteHelpUrl}api/help/{helpKey}";
            
            return HelpUrlResolutionResult.Success(remoteUrl, HelpUrlType.RemoteApi);
        }

        /// <summary>
        /// 解析远程页面
        /// </summary>
        private HelpUrlResolutionResult ResolveRemotePage(string url, Match match)
        {
            if (!EnableRemoteHelp || string.IsNullOrEmpty(_remoteHelpUrl))
            {
                return HelpUrlResolutionResult.Failure("远程帮助未启用");
            }
            
            string pagePath = match.Groups[1].Value;
            string remoteUrl = $"{_remoteHelpUrl}{pagePath}";
            
            return HelpUrlResolutionResult.Success(remoteUrl, HelpUrlType.RemotePage);
        }

        /// <summary>
        /// 解析远程 HTTP 帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveRemoteHttpHelp(string url, Match match)
        {
            if (!EnableRemoteHelp)
            {
                return HelpUrlResolutionResult.Failure("远程帮助未启用");
            }
            
            return HelpUrlResolutionResult.Success(url, HelpUrlType.RemotePage);
        }

        /// <summary>
        /// 解析 GitHub Pages 帮助
        /// </summary>
        private HelpUrlResolutionResult ResolveGitHubPagesHelp(string url, Match match)
        {
            if (!EnableRemoteHelp)
            {
                return HelpUrlResolutionResult.Failure("远程帮助未启用");
            }
            
            return HelpUrlResolutionResult.Success(url, HelpUrlType.RemotePage);
        }

        #endregion

        #region 帮助方法

        /// <summary>
        /// 判断 URL 是否为本地帮助 URL
        /// </summary>
        public static bool IsLocalHelpUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && url.StartsWith("help://local/");
        }

        /// <summary>
        /// 判断 URL 是否为远程帮助 URL
        /// </summary>
        public static bool IsRemoteHelpUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && url.StartsWith("help://remote/");
        }

        /// <summary>
        /// 判断 URL 是否为 HTTP(S) URL
        /// </summary>
        public static bool IsHttpUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && 
                   (url.StartsWith("http://") || url.StartsWith("https://"));
        }

        /// <summary>
        /// 从 URL 中提取帮助键
        /// </summary>
        public static string ExtractHelpKey(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            // 匹配 help://local/{type}/{key} 格式
            var match = Regex.Match(url, @"help://local/\w+/(.+)$");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // 匹配 help://remote/api/help/{key} 格式
            match = Regex.Match(url, @"help://remote/api/help/(.+)$");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return null;
        }

        #endregion
    }

    #region 委托和枚举

    /// <summary>
    /// URL 处理器委托
    /// </summary>
    /// <param name="url">原始 URL</param>
    /// <param name="match">正则匹配结果</param>
    /// <returns>解析结果</returns>
    public delegate HelpUrlResolutionResult HelpUrlHandler(string url, Match match);

    /// <summary>
    /// 帮助 URL 类型
    /// </summary>
    public enum HelpUrlType
    {
        /// <summary>
        /// 本地文件
        /// </summary>
        LocalFile,

        /// <summary>
        /// 远程 API
        /// </summary>
        RemoteApi,

        /// <summary>
        /// 远程页面
        /// </summary>
        RemotePage
    }

    /// <summary>
    /// 帮助 URL 解析结果
    /// </summary>
    public class HelpUrlResolutionResult
    {
        #region 私有字段

        private bool _success;
        private string _resolvedPath;
        private HelpUrlType _type;
        private string _errorMessage;

        #endregion

        #region 公共属性

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => _success;

        /// <summary>
        /// 解析后的路径或 URL
        /// </summary>
        public string ResolvedPath => _resolvedPath;

        /// <summary>
        /// URL 类型
        /// </summary>
        public HelpUrlType Type => _type;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage => _errorMessage;

        #endregion

        #region 构造函数

 

        /// <summary>
        /// 公共无参构造函数（用于依赖注入）
        /// </summary>
        public HelpUrlResolutionResult()
        {
            _success = false;
            _resolvedPath = null;
            _type = HelpUrlType.LocalFile;
            _errorMessage = null;
        }

        #endregion

        #region 工厂方法

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static HelpUrlResolutionResult Success(string resolvedPath, HelpUrlType type)
        {
            return new HelpUrlResolutionResult
            {
                _success = true,
                _resolvedPath = resolvedPath,
                _type = type,
                _errorMessage = null
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static HelpUrlResolutionResult Failure(string errorMessage)
        {
            return new HelpUrlResolutionResult
            {
                _success = false,
                _resolvedPath = null,
                _type = HelpUrlType.LocalFile,
                _errorMessage = errorMessage
            };
        }

        #endregion
    }

    #endregion
}
