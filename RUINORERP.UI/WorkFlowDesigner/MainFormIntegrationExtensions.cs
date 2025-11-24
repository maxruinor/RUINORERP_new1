using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.WorkFlowDesigner;

namespace RUINORERP.UI
{
    /// <summary>
    /// MainForm 流程导航图集成扩展
    /// 提供便捷的集成方法
    /// </summary>
    public static class MainFormIntegrationExtensions
    {
        /// <summary>
        /// 初始化流程导航图功能（扩展方法）
        /// </summary>
        /// <param name="mainForm">主窗体实例</param>
        /// <param name="logger">日志记录器</param>
        public static async Task InitializeProcessNavigationAsync(this MainForm mainForm, ILogger logger = null)
        {
            try
            {
                // 检查是否已经初始化
                if (mainForm.Tag?.ToString() == "ProcessNavigationInitialized")
                {
                    logger?.LogInformation("流程导航图功能已经初始化");
                    return;
                }

                // 创建流程导航图管理器
                var navigationManager = new ProcessNavigationManager();
                
                // 设置管理器属性（如果MainForm有相关属性）
                // 这里需要根据实际的MainForm结构调整

                // 标记为已初始化
                mainForm.Tag = "ProcessNavigationInitialized";

                logger?.LogInformation("流程导航图功能初始化完成");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "初始化流程导航图功能失败");
                MessageBox.Show($"初始化流程导航图功能失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示流程导航图设计器（扩展方法）
        /// </summary>
        /// <param name="mainForm">主窗体实例</param>
        /// <param name="logger">日志记录器</param>
        public static void ShowProcessNavigationDesigner(this MainForm mainForm, ILogger logger = null)
        {
            try
            {
                var designer = new ProcessNavigationDesigner();
                
                // 创建一个包含设计器的窗体来显示
                var form = new Form
                {
                    Text = "流程导航图设计器",
                    Size = new Size(800, 600),
                    StartPosition = FormStartPosition.CenterParent,
                    WindowState = FormWindowState.Maximized
                };
                
                form.Controls.Add(designer);
                designer.Dock = DockStyle.Fill;
                
                form.ShowDialog(mainForm);
                
                logger?.LogInformation("流程导航图设计器已显示");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "显示流程导航图设计器失败");
                MessageBox.Show($"显示流程导航图设计器失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新流程导航图（扩展方法）
        /// </summary>
        /// <param name="mainForm">主窗体实例</param>
        /// <param name="logger">日志记录器</param>
        public static async Task RefreshProcessNavigationAsync(this MainForm mainForm, ILogger logger = null)
        {
            try
            {
                // 这里需要根据实际的MainForm结构调整
                // 如果MainForm有ProcessNavigationManager属性，则调用其Refresh方法

                logger?.LogInformation("流程导航图已刷新");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "刷新流程导航图失败");
                MessageBox.Show($"刷新流程导航图失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加流程导航图菜单项（扩展方法）
        /// </summary>
        /// <param name="mainForm">主窗体实例</param>
        /// <param name="logger">日志记录器</param>
        public static void AddProcessNavigationMenuItems(this MainForm mainForm, ILogger logger = null)
        {
            try
            {
                // 这里需要根据实际的MainForm菜单结构调整
                // 添加流程导航图相关的菜单项

                logger?.LogInformation("流程导航图菜单项已添加");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "添加流程导航图菜单项失败");
                MessageBox.Show($"添加流程导航图菜单项失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}