using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace RUINORERP.Plugin.AlibabaStoreManager
{
    public partial class ValidationForm : Form
    {
        private WebView2 webView;
        private bool isWebViewInitialized = false;

        public ValidationForm()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private void InitializeWebView()
        {
            try
            {
                // 创建WebView2控件
                webView = new WebView2();
                webView.Dock = DockStyle.Fill;
                
                // 添加到窗体面板中
                panelWebView.Controls.Add(webView);
                
                // 初始化WebView2
                InitializeWebViewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化WebView2时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void InitializeWebViewAsync()
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);
                isWebViewInitialized = true;
                
                // 订阅导航完成事件
                webView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;
                
                // 加载1688登录页面
                NavigateToUrl("https://login.1688.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化WebView2时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                UpdateStatus($"页面加载成功: {webView.Source}");
                btnTestElements.Enabled = true;
            }
            else
            {
                UpdateStatus($"页面加载失败: {e.WebErrorStatus}");
            }
        }

        private void NavigateToUrl(string url)
        {
            try
            {
                if (isWebViewInitialized)
                {
                    webView.Source = new Uri(url);
                    UpdateStatus($"正在加载: {url}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航到 {url} 时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnTestElements_Click(object sender, EventArgs e)
        {
            if (!isWebViewInitialized)
            {
                MessageBox.Show("WebView2尚未初始化完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                UpdateStatus("正在测试元素定位...");
                
                // 测试用户名输入框定位
                string usernameScript = @"
                    (function() {
                        var usernameInput = document.querySelector('#username');
                        if (usernameInput) {
                            usernameInput.value = 'test_username';
                            return '用户名输入框定位成功';
                        } else {
                            return '未找到用户名输入框';
                        }
                    })();
                ";
                
                string result = await webView.CoreWebView2.ExecuteScriptAsync(usernameScript);
                UpdateStatus($"用户名测试结果: {result}");
                
                // 测试密码输入框定位
                string passwordScript = @"
                    (function() {
                        var passwordInput = document.querySelector('#password');
                        if (passwordInput) {
                            passwordInput.value = 'test_password';
                            return '密码输入框定位成功';
                        } else {
                            return '未找到密码输入框';
                        }
                    })();
                ";
                
                result = await webView.CoreWebView2.ExecuteScriptAsync(passwordScript);
                UpdateStatus($"密码测试结果: {result}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试元素定位时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatus), message);
            }
            else
            {
                lblStatus.Text = message;
                txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\r\n");
            }
        }

        private void btnNavigate_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();
            if (!string.IsNullOrEmpty(url))
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }
                NavigateToUrl(url);
            }
        }

        private void ValidationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.NavigationCompleted -= OnNavigationCompleted;
            }
        }
    }
}