using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Plugin.AlibabaStoreManager.Core
{
    /// <summary>
    /// 浏览器控制器，负责WebView2浏览器控件的操作
    /// </summary>
    public class BrowserController : IDisposable
    {
        private WebView2 webView;
        private bool isInitialized = false;

        /// <summary>
        /// 页面加载完成事件
        /// </summary>
        public event EventHandler<string> PageLoaded;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="webView">WebView2控件实例</param>
        public BrowserController(WebView2 webView)
        {
            this.webView = webView ?? throw new ArgumentNullException(nameof(webView));
            InitializeAsync();
        }

        /// <summary>
        /// 异步初始化浏览器控件
        /// </summary>
        private async void InitializeAsync()
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
                isInitialized = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化浏览器控件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导航到指定URL
        /// </summary>
        /// <param name="url">目标URL</param>
        public async Task NavigateToAsync(string url)
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("浏览器控件尚未初始化完成");
            }

            try
            {
                webView.Source = new Uri(url);
                await WaitForPageLoadAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"导航到 {url} 时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 后退
        /// </summary>
        public void GoBack()
        {
            if (!isInitialized || webView.CoreWebView2 == null)
            {
                throw new InvalidOperationException("浏览器控件尚未初始化完成");
            }

            if (webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        /// <summary>
        /// 前进
        /// </summary>
        public void GoForward()
        {
            if (!isInitialized || webView.CoreWebView2 == null)
            {
                throw new InvalidOperationException("浏览器控件尚未初始化完成");
            }

            if (webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        /// <summary>
        /// 获取当前页面URL
        /// </summary>
        public string GetCurrentUrl()
        {
            if (!isInitialized || webView.Source == null)
            {
                return string.Empty;
            }
            return webView.Source.ToString();
        }

        /// <summary>
        /// 执行JavaScript代码
        /// </summary>
        /// <param name="script">要执行的JavaScript代码</param>
        /// <returns>执行结果</returns>
        public async Task<string> ExecuteScriptAsync(string script)
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("浏览器控件尚未初始化完成");
            }

            try
            {
                return await webView.CoreWebView2.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"执行JavaScript代码时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 等待页面加载完成
        /// </summary>
        private async Task WaitForPageLoadAsync()
        {
            // 简单的等待机制，实际项目中可能需要更复杂的等待逻辑
            await Task.Delay(3000);
        }

        /// <summary>
        /// 页面导航完成事件处理
        /// </summary>
        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                PageLoaded?.Invoke(this, webView.Source?.ToString() ?? "");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.NavigationCompleted -= OnNavigationCompleted;
            }
        }
    }
}