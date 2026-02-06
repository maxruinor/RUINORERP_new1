using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 在线Web帮助提供者
    /// 通过WebView2或系统浏览器显示在线帮助网站
    /// 支持URL参数传递帮助上下文
    /// </summary>
    public class WebHelpProvider : IHelpProvider
    {
        #region 私有字段

        private readonly string _baseUrl;
        private readonly Dictionary<string, string> _urlMapping;
        private WebView2 _webView;
        private bool _disposed = false;
        private bool _useWebView2 = true;

        #endregion

        #region 公共属性

        public string ProviderName => "在线Web帮助提供者";
        
        public string HelpContentRootPath => _baseUrl;
        
        public int HelpCount => _urlMapping.Count;

        /// <summary>
        /// 是否使用WebView2（false则使用系统浏览器）
        /// </summary>
        public bool UseWebView2
        {
            get => _useWebView2;
            set => _useWebView2 = value;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseUrl">帮助网站基础URL，如 http://localhost:8000</param>
        public WebHelpProvider(string baseUrl)
        {
            _baseUrl = baseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(baseUrl));
            
            // 初始化帮助键到URL的映射
            _urlMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                // 快速入门
                ["QuickStart"] = "/quickstart/",
                ["Login"] = "/quickstart/login/",
                ["Interface"] = "/quickstart/interface/",
                ["BasicOperations"] = "/quickstart/basic-operations/",
                ["Shortcuts"] = "/quickstart/shortcuts/",
                
                // 窗体级帮助 - 销售管理
                ["UCSaleOrder"] = "/forms/UCSaleOrder/",
                ["UCSaleOut"] = "/forms/UCSaleOut/",
                ["UCSaleReturn"] = "/forms/UCSaleReturn/",
                
                // 窗体级帮助 - 采购管理
                ["UCPurchaseOrder"] = "/forms/UCPurchaseOrder/",
                ["UCStockIn"] = "/forms/UCStockIn/",
                
                // 窗体级帮助 - 库存管理
                ["UCStockQuery"] = "/forms/UCStockQuery/",
                ["UCStockCheck"] = "/forms/UCStockCheck/",
                ["UCStockTransfer"] = "/forms/UCStockTransfer/",
                
                // 模块级帮助
                ["SalesManagement"] = "/modules/sales/",
                ["PurchaseManagement"] = "/modules/purchase/",
                ["InventoryManagement"] = "/modules/inventory/",
                ["FinanceManagement"] = "/modules/finance/",
                
                // 字段级帮助 - 销售订单
                ["Fields.tb_SaleOrder.CustomerVendor_ID"] = "/fields/tb_SaleOrder/CustomerVendor_ID/",
                ["Fields.tb_SaleOrder.Employee_ID"] = "/fields/tb_SaleOrder/Employee_ID/",
                ["Fields.tb_SaleOrder.OrderNo"] = "/fields/tb_SaleOrder/OrderNo/",
                ["Fields.tb_SaleOrder.SaleDate"] = "/fields/tb_SaleOrder/SaleDate/",
                ["Fields.tb_SaleOrder.PreDeliveryDate"] = "/fields/tb_SaleOrder/PreDeliveryDate/",
                ["Fields.tb_SaleOrder.Paytype_ID"] = "/fields/tb_SaleOrder/Paytype_ID/",
                ["Fields.tb_SaleOrder.PayStatus"] = "/fields/tb_SaleOrder/PayStatus/",
                ["Fields.tb_SaleOrder.TotalCost"] = "/fields/tb_SaleOrder/TotalCost/",
            };
        }

        #endregion

        #region IHelpProvider 实现

        public string GetHelpContent(HelpContext context)
        {
            // Web提供者不返回内容，而是直接显示网页
            // 返回null让调用者知道需要调用ShowWebHelp
            return null;
        }

        public List<HelpSearchResult> Search(string keyword, HelpContext context = null)
        {
            var results = new List<HelpSearchResult>();
            
            foreach (var mapping in _urlMapping)
            {
                if (mapping.Key.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    mapping.Value.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(new HelpSearchResult
                    {
                        Title = mapping.Key,
                        Content = $"{_baseUrl}{mapping.Value}",
                        HelpKey = mapping.Key,
                        RelevanceScore = CalculateRelevance(mapping.Key, keyword)
                    });
                }
            }
            
            return results.OrderByDescending(r => r.RelevanceScore).ToList();
        }

        public bool HelpExists(HelpContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.HelpKey))
                return false;

            // 检查网络连接
            if (!IsNetworkAvailable())
                return false;

            // 检查是否有映射
            return _urlMapping.ContainsKey(context.HelpKey) ||
                   _urlMapping.Keys.Any(k => context.HelpKey.StartsWith(k, StringComparison.OrdinalIgnoreCase));
        }

        public void ReloadIndex()
        {
            // Web帮助是动态加载的，不需要重新加载索引
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _webView?.Dispose();
                _disposed = true;
            }
        }

        #endregion

        #region Web帮助专用方法

        /// <summary>
        /// 显示Web帮助
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>是否成功显示</returns>
        public bool ShowWebHelp(HelpContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.HelpKey))
                return false;

            string url = BuildHelpUrl(context);
            if (string.IsNullOrEmpty(url))
            {
                // 没有精确匹配，显示首页
                url = _baseUrl;
            }

            try
            {
                if (_useWebView2)
                {
                    // 使用WebView2显示
                    ShowWebView2(url, context);
                }
                else
                {
                    // 使用系统浏览器
                    OpenSystemBrowser(url);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示Web帮助失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 构建帮助URL
        /// </summary>
        private string BuildHelpUrl(HelpContext context)
        {
            string helpKey = context.HelpKey;
            
            // 精确匹配
            if (_urlMapping.TryGetValue(helpKey, out string urlPath))
            {
                return $"{_baseUrl}{urlPath}";
            }

            // 尝试部分匹配
            foreach (var mapping in _urlMapping)
            {
                if (helpKey.StartsWith(mapping.Key, StringComparison.OrdinalIgnoreCase) ||
                    mapping.Key.StartsWith(helpKey, StringComparison.OrdinalIgnoreCase))
                {
                    return $"{_baseUrl}{mapping.Value}";
                }
            }

            // 回退到基于HelpKey构建URL
            // 例如：Forms.UCSaleOrder -> /forms/UCSaleOrder/
            if (helpKey.StartsWith("Forms."))
            {
                string formName = helpKey.Substring(6);
                return $"{_baseUrl}/forms/{formName}/";
            }

            return null;
        }

        /// <summary>
        /// 使用WebView2显示帮助
        /// </summary>
        private void ShowWebView2(string url, HelpContext context)
        {
            // 创建帮助窗口
            var helpForm = new Form
            {
                Text = "RUINOR ERP 帮助中心",
                Width = 1200,
                Height = 800,
                StartPosition = FormStartPosition.CenterScreen,
                Icon = System.Drawing.SystemIcons.Question
            };

            // 创建WebView2控件
            _webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            helpForm.Controls.Add(_webView);

            // 异步初始化并导航
            InitializeWebViewAsync(_webView, url);

            // 显示窗口
            helpForm.Show();
        }

        /// <summary>
        /// 异步初始化WebView2
        /// </summary>
        private async void InitializeWebViewAsync(WebView2 webView, string url)
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.Navigate(url);
            }
            catch (Exception ex)
            {
                // WebView2初始化失败，回退到系统浏览器
                System.Diagnostics.Debug.WriteLine($"WebView2初始化失败: {ex.Message}");
                OpenSystemBrowser(url);
            }
        }

        /// <summary>
        /// 使用系统浏览器打开
        /// </summary>
        private void OpenSystemBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开浏览器: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 检查网络是否可用
        /// </summary>
        public static bool IsNetworkAvailable()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://www.baidu.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 测试帮助网站是否可访问
        /// </summary>
        public bool TestConnection()
        {
            try
            {
                using (var client = new WebClientWithTimeout())
                {
                    client.Timeout = 5000; // 5秒超时
                    using (client.OpenRead(_baseUrl))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 添加URL映射（用于动态扩展）
        /// </summary>
        public void AddUrlMapping(string helpKey, string urlPath)
        {
            _urlMapping[helpKey] = urlPath;
        }

        #endregion

        #region 私有方法

        private double CalculateRelevance(string key, string keyword)
        {
            if (key.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                return 1.0;
            
            if (key.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                return 0.8;
            
            return 0.5;
        }

        #endregion
    }

    /// <summary>
    /// WebClient扩展，支持超时设置
    /// </summary>
    public class WebClientWithTimeout : WebClient
    {
        public int Timeout { get; set; } = 30000;

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest request = base.GetWebRequest(uri);
            request.Timeout = Timeout;
            return request;
        }
    }
}
