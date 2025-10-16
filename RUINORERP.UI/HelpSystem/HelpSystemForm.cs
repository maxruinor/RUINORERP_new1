using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using Krypton.Toolkit;
using Krypton.Navigator;
using RUINORERP.UI.HelpSystem;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助系统主窗体
    /// </summary>
    public partial class HelpSystemForm : frmBase
    {
        public HelpSystemForm()
        {
            InitializeComponent();
            InitializeHelpSystem();
        }
        
        private void InitializeHelpSystem()
        {
            // 启用F1帮助功能
            this.EnableF1Help();
            
            // 为窗体设置帮助页面
            this.SetHelpPage("forms/help_system.html", "帮助系统");
            
            // 为控件设置帮助键
            kryptonTextBoxSearch.SetControlHelpKey("textbox_help_search");
            kryptonButtonSearch.SetControlHelpKey("button_help_search");
            kryptonListBoxSearchResults.SetControlHelpKey("listbox_help_search_results");
            kryptonListBoxRecommendations.SetControlHelpKey("listbox_help_recommendations");
            kryptonListBoxHistory.SetControlHelpKey("listbox_help_history");
            kryptonButtonClearHistory.SetControlHelpKey("button_help_clear_history");
            
            // 绑定事件处理程序
            kryptonButtonSearch.Click += BtnSearch_Click;
            kryptonListBoxSearchResults.DoubleClick += LstSearchResults_DoubleClick;
            kryptonListBoxRecommendations.DoubleClick += LstRecommendations_DoubleClick;
            kryptonListBoxHistory.DoubleClick += LstHistory_DoubleClick;
            kryptonButtonClearHistory.Click += BtnClearHistory_Click;
            webView2.NavigationCompleted += WebView2_NavigationCompleted;
            
            // 加载初始数据
            LoadRecommendations();
            LoadHistory();
            
            // 检查是否有请求的帮助页面
            this.Shown += HelpSystemForm_Shown;
        }
        
        private void HelpSystemForm_Shown(object sender, EventArgs e)
        {
            // 如果有请求的帮助页面，则显示它
            if (!string.IsNullOrEmpty(HelpManager.LastRequestedHelpPage))
            {
                ShowHelpPage(HelpManager.LastRequestedHelpPage);
                HelpManager.LastRequestedHelpPage = null;
            }
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }
        
        private void LstSearchResults_DoubleClick(object sender, EventArgs e)
        {
            if (kryptonListBoxSearchResults.SelectedItem is HelpSearchManager.SearchResultItem selectedItem)
            {
                ShowHelpPage(selectedItem.HelpPage);
            }
        }
        
        private void LstRecommendations_DoubleClick(object sender, EventArgs e)
        {
            if (kryptonListBoxRecommendations.SelectedItem is HelpRecommendationManager.RecommendationItem selectedItem)
            {
                ShowHelpPage(selectedItem.HelpPage);
            }
        }
        
        private void LstHistory_DoubleClick(object sender, EventArgs e)
        {
            if (kryptonListBoxHistory.SelectedItem is HelpHistoryManager.HelpHistoryItem selectedItem)
            {
                ShowHelpPage(selectedItem.HelpPage);
            }
        }
        
        private void BtnClearHistory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清除所有帮助查看历史记录吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                HelpHistoryManager.ClearHistory();
                LoadHistory();
                kryptonStatusLabel.Text = "历史记录已清除";
            }
        }
        
        private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // 获取页面标题并更新状态栏
            webView2.ExecuteScriptAsync("document.title").ContinueWith(task =>
            {
                if (task.Result != null)
                {
                    string title = task.Result.Trim('\"');
                    if (!string.IsNullOrEmpty(title))
                    {
                        Invoke(new Action(() => kryptonStatusLabel.Text = $"正在查看: {title}"));
                    }
                }
            });
        }
        
        private void PerformSearch()
        {
            string keywords = kryptonTextBoxSearch.Text.Trim();
            if (string.IsNullOrEmpty(keywords))
            {
                MessageBox.Show("请输入搜索关键词", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
                kryptonStatusLabel.Text = "正在搜索...";
                Application.DoEvents();
                
                var results = HelpSearchManager.Search(keywords);
                kryptonListBoxSearchResults.Items.Clear();
                
                foreach (var result in results)
                {
                    kryptonListBoxSearchResults.Items.Add(result);
                }
                
                kryptonStatusLabel.Text = $"找到 {results.Count} 个结果";
                
                // 切换到搜索结果标签页
                kryptonNavigatorTabs.SelectedPage = kryptonPageSearch;
            }
            catch (Exception ex)
            {
                kryptonStatusLabel.Text = "搜索出错";
                MessageBox.Show($"搜索时发生错误: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadRecommendations()
        {
            try
            {
                var recommendations = HelpRecommendationManager.GetRecommendations();
                kryptonListBoxRecommendations.Items.Clear();
                
                foreach (var recommendation in recommendations)
                {
                    kryptonListBoxRecommendations.Items.Add(recommendation);
                }
            }
            catch (Exception ex)
            {
                kryptonStatusLabel.Text = "加载推荐内容出错";
                System.Diagnostics.Debug.WriteLine($"加载推荐内容时出错: {ex.Message}");
            }
        }
        
        private void LoadHistory()
        {
            try
            {
                var history = HelpHistoryManager.GetRecentHistory(30);
                kryptonListBoxHistory.Items.Clear();
                
                foreach (var item in history)
                {
                    kryptonListBoxHistory.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                kryptonStatusLabel.Text = "加载历史记录出错";
                System.Diagnostics.Debug.WriteLine($"加载历史记录时出错: {ex.Message}");
            }
        }
        
        private async void ShowHelpPage(string helpPage)
        {
            if (string.IsNullOrEmpty(HelpManager.HelpFilePath))
                return;
                
            try
            {
                // 确保WebView2已经初始化
                if (webView2.CoreWebView2 == null)
                {
                    await webView2.EnsureCoreWebView2Async(null);
                }
                
                // 构造帮助页面URL
                string helpUrl = string.IsNullOrEmpty(helpPage) ? 
                    HelpManager.HelpFilePath : 
                    $"mk:@MSITStore:{HelpManager.HelpFilePath}::/{helpPage}";
                
                // 使用WebView2的Source属性设置URL
                webView2.Source = new Uri(helpUrl);
                kryptonStatusLabel.Text = $"正在加载: {helpPage ?? "默认页面"}";
            }
            catch (Exception ex)
            {
                kryptonStatusLabel.Text = "加载帮助页面出错";
                MessageBox.Show($"无法加载帮助页面: {ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}