using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Plugin.AlibabaStoreManager.Core;
using RUINORERP.Plugin.AlibabaStoreManager.Models;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;

namespace RUINORERP.Plugin.AlibabaStoreManager
{
    public partial class MainForm : Form
    {
        private WebView2 webView;
        private BrowserController browserController;
        private DataExtractor dataExtractor;
        private FormOperator formOperator;
        private PluginConfig pluginConfig;
        private List<string> favorites;
        private string favoritesFilePath;
        private string configFilePath;

        public MainForm()
        {
            InitializeComponent();
            InitializeWebView();
            InitializeConfig();
            InitializeFavorites();
        }

        /// <summary>
        /// 初始化WebView控件
        /// </summary>
        private void InitializeWebView()
        {
            try
            {
                webView = new WebView2();
                webView.Dock = DockStyle.Fill;
                this.panelMain.Controls.Add(webView);
                
                browserController = new BrowserController(webView);
                dataExtractor = new DataExtractor(browserController);
                formOperator = new FormOperator(browserController);
                
                // 订阅页面加载事件
                browserController.PageLoaded += OnPageLoaded;
                
                // 初始化完成后导航到默认URL
                _ = NavigateToInitialUrlAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化WebView控件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private void InitializeConfig()
        {
            configFilePath = System.IO.Path.Combine(Application.StartupPath, "plugin_config.json");
            LoadPluginConfig();
        }

        /// <summary>
        /// 初始化收藏夹
        /// </summary>
        private void InitializeFavorites()
        {
            favorites = new List<string>();
            favoritesFilePath = System.IO.Path.Combine(Application.StartupPath, "favorites.json");
            
            // 加载收藏夹
            LoadFavorites();
        }

        /// <summary>
        /// 加载收藏夹
        /// </summary>
        private void LoadFavorites()
        {
            try
            {
                if (System.IO.File.Exists(favoritesFilePath))
                {
                    string json = System.IO.File.ReadAllText(favoritesFilePath);
                    favorites = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载收藏夹时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存收藏夹
        /// </summary>
        private void SaveFavorites()
        {
            try
            {
                string json = JsonConvert.SerializeObject(favorites, Formatting.Indented);
                System.IO.File.WriteAllText(favoritesFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存收藏夹时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导航到初始URL
        /// </summary>
        private async Task NavigateToInitialUrlAsync()
        {
            try
            {
                // 等待控件初始化完成
                await Task.Delay(1000);
                
                // 如果启用了自动登录并且有保存的cookies，则先恢复cookies
                if (pluginConfig.AutoLogin && pluginConfig.SavedCookies != null && pluginConfig.SavedCookies.Count > 0)
                {
                    await RestoreCookiesAsync();
                }
                
                await browserController.NavigateToAsync("https://work.1688.com/home/seller.htm");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到初始URL时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 恢复保存的cookies
        /// </summary>
        private async Task RestoreCookiesAsync()
        {
            try
            {
                if (webView.CoreWebView2 != null)
                {
                    foreach (var cookieData in pluginConfig.SavedCookies)
                    {
                        // 创建cookie
                        var cookie = webView.CoreWebView2.CookieManager.CreateCookie(
                            cookieData.Name,
                            cookieData.Value,
                            cookieData.Domain,
                            cookieData.Path);
                        
                        // 设置过期时间
                        if (cookieData.Expires != null && cookieData.Expires > DateTime.MinValue && cookieData.Expires != DateTime.MaxValue)
                        {
                            try
                            {
                                // 将DateTime转换为Unix时间戳
                                DateTimeOffset dto = new DateTimeOffset(cookieData.Expires);
                                cookie.Expires = dto.ToUnixTimeSeconds();
                            }
                            catch
                            {
                                // 如果转换失败，不设置过期时间
                            }
                        }
                        
                        cookie.IsHttpOnly = cookieData.HttpOnly;
                        cookie.IsSecure = cookieData.Secure;
                        
                        // 添加cookie到管理器
                        webView.CoreWebView2.CookieManager.AddOrUpdateCookie(cookie);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"恢复cookies时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存当前cookies
        /// </summary>
        private async Task SaveCookiesAsync()
        {
            try
            {
                if (webView.CoreWebView2 != null)
                {
                    var cookies = await webView.CoreWebView2.CookieManager.GetCookiesAsync("https://1688.com");
                    pluginConfig.SavedCookies.Clear();
                    
                    foreach (var cookie in cookies)
                    {
                        // 检查cookie.Expires的类型并正确处理
                        DateTime expires = DateTime.MaxValue;
                        if (cookie.Expires > 0)
                        {
                            try
                            {
                                expires = DateTimeOffset.FromUnixTimeSeconds((long)cookie.Expires).DateTime;
                            }
                            catch
                            {
                                expires = DateTime.MaxValue;
                            }
                        }
                        
                        var cookieData = new CookieData
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain,
                            Path = cookie.Path,
                            Expires = expires,
                            HttpOnly = cookie.IsHttpOnly,
                            Secure = cookie.IsSecure
                        };
                        
                        pluginConfig.SavedCookies.Add(cookieData);
                    }
                    
                    SavePluginConfig();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存cookies时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导航到指定URL
        /// </summary>
        /// <param name="url">目标URL</param>
        public async Task NavigateToUrlAsync(string url)
        {
            try
            {
                // 验证URL格式
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }
                
                txtUrl.Text = url;
                await browserController.NavigateToAsync(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到 {url} 时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 页面加载完成事件处理
        /// </summary>
        private void OnPageLoaded(object sender, string url)
        {
            // 更新UI状态
            this.Invoke(new Action(() =>
            {
                txtUrl.Text = url;
                // 这里可以更新界面状态，例如启用某些按钮等
            }));
        }

        /// <summary>
        /// 后退按钮点击事件
        /// </summary>
        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (webView != null && webView.CoreWebView2 != null)
                {
                    if (webView.CoreWebView2.CanGoBack)
                    {
                        webView.CoreWebView2.GoBack();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"后退时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 前进按钮点击事件
        /// </summary>
        private void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (webView != null && webView.CoreWebView2 != null)
                {
                    if (webView.CoreWebView2.CanGoForward)
                    {
                        webView.CoreWebView2.GoForward();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"前进时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 导航按钮点击事件
        /// </summary>
        private void btnNavigate_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (!string.IsNullOrEmpty(url))
            {
                _ = NavigateToUrlAsync(url);
            }
        }

        /// <summary>
        /// 收藏按钮点击事件
        /// </summary>
        private void btnFavorite_Click(object sender, EventArgs e)
        {
            // 显示收藏夹窗体
            using (FavoritesForm favoritesForm = new FavoritesForm(favorites, (url) => {
                _ = NavigateToUrlAsync(url);
            }))
            {
                // 显示为模式对话框
                favoritesForm.ShowDialog(this);
                
                // 保存可能已修改的收藏夹
                SaveFavorites();
            }
        }

        /// <summary>
        /// 登录设置按钮点击事件
        /// </summary>
        private void btnLoginSettings_Click(object sender, EventArgs e)
        {
            // 显示登录设置窗体
            using (LoginForm loginForm = new LoginForm(pluginConfig, (config) => {
                pluginConfig = config;
                SavePluginConfig();
            }))
            {
                // 显示为模式对话框
                loginForm.ShowDialog(this);
            }
        }

        /// <summary>
        /// URL文本框按键事件
        /// </summary>
        private void txtUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 按回车键时导航到指定URL
            if (e.KeyChar == (char)Keys.Enter)
            {
                string url = txtUrl.Text.Trim();
                if (!string.IsNullOrEmpty(url))
                {
                    _ = NavigateToUrlAsync(url);
                    e.Handled = true; // 标记事件已处理，防止系统响应该按键
                }
            }
        }

        /// <summary>
        /// 加载插件配置
        /// </summary>
        private void LoadPluginConfig()
        {
            try
            {
                if (System.IO.File.Exists(configFilePath))
                {
                    string json = System.IO.File.ReadAllText(configFilePath);
                    pluginConfig = JsonConvert.DeserializeObject<PluginConfig>(json);
                }
                
                if (pluginConfig == null)
                {
                    pluginConfig = new PluginConfig();
                }
                
                // 应用配置
                ApplyConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载插件配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pluginConfig = new PluginConfig();
            }
        }

        /// <summary>
        /// 应用配置
        /// </summary>
        private void ApplyConfig()
        {
            // 这里可以应用配置到界面控件
        }

        /// <summary>
        /// 保存插件配置
        /// </summary>
        private void SavePluginConfig()
        {
            try
            {
                string json = JsonConvert.SerializeObject(pluginConfig, Formatting.Indented);
                System.IO.File.WriteAllText(configFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存插件配置时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // 保存cookies
            _ = SaveCookiesAsync();
            
            browserController?.Dispose();
            base.OnFormClosed(e);
        }
    }
}