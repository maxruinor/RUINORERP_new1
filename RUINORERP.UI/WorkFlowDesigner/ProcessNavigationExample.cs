using System;
using System.Drawing;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.UI.WorkFlowDesigner;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 流程导航图使用示例
    /// 展示如何在MainForm中集成流程导航图功能
    /// </summary>
    [Common.MenuAttrAssemblyInfo("流程导航图示例", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.流程设计)]
    public partial class ProcessNavigationExample : KryptonForm
    {
        #region Fields

        private ProcessNavigationManager _navigationManager;
        private SplitContainer _splitContainer;
        private KryptonPanel _navigationPanel;
        private KryptonPanel _infoPanel;
        private KryptonLabel _lblTitle;
        private KryptonRichTextBox _txtInfo;

        #endregion

        #region Constructor

        public ProcessNavigationExample()
        {
            InitializeComponent();
            InitializeNavigationManager();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 窗体属性
            this.Text = "流程导航图示例";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
          //  this.Icon = Properties.Resources.SPP;

            // 主分割容器
            _splitContainer = new SplitContainer();
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.SplitterDistance = 800;
            _splitContainer.SplitterWidth = 5;

            // 流程导航图面板
            _navigationPanel = new KryptonPanel();
            _navigationPanel.Dock = DockStyle.Fill;
          //  _navigationPanel.StateCommon.Back.Color1 = Color.White;

            // 信息面板
            _infoPanel = new KryptonPanel();
            _infoPanel.Dock = DockStyle.Fill;
          //  _infoPanel.StateCommon.Back.Color1 = Color.FromArgb(240, 240, 240);

            // 标题标签
            _lblTitle = new KryptonLabel();
            _lblTitle.Text = "流程导航图使用说明";
            _lblTitle.Font = new Font("Microsoft YaHei", 12, FontStyle.Bold);
            _lblTitle.Location = new Point(20, 20);
            _lblTitle.Size = new Size(300, 30);

            // 信息文本框
            _txtInfo = new KryptonRichTextBox();
            _txtInfo.Location = new Point(20, 60);
            _txtInfo.Size = new Size(350, 680);
            _txtInfo.Font = new Font("Microsoft YaHei", 9);
            _txtInfo.ReadOnly = true;
            _txtInfo.Text = GetUsageInfo();

            // 添加控件到信息面板
            _infoPanel.Controls.Add(_lblTitle);
            _infoPanel.Controls.Add(_txtInfo);

            // 设置分割容器面板
            _splitContainer.Panel1.Controls.Add(_navigationPanel);
            _splitContainer.Panel2.Controls.Add(_infoPanel);

            // 添加到窗体
            this.Controls.Add(_splitContainer);

            this.ResumeLayout(false);
        }

        private void InitializeNavigationManager()
        {
            try
            {
                // 创建流程导航图管理器
                _navigationManager = new ProcessNavigationManager();
                _navigationManager.Dock = DockStyle.Fill;

                // 添加事件处理
                _navigationManager.NodeClick += NavigationManager_NodeClick;
                _navigationManager.CurrentNavigationChanged += NavigationManager_CurrentNavigationChanged;

                // 添加到导航面板
                _navigationPanel.Controls.Add(_navigationManager);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化流程导航图管理器失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        private void NavigationManager_NodeClick(object sender, ProcessNodeClickEventArgs e)
        {
            try
            {
                var node = e.Node;
                
                // 显示节点信息
                string message = $"点击了流程节点：\n\n" +
                               $"节点名称：{node.ProcessName}\n" +
                               $"节点描述：{node.Description}\n" +
                               $"关联菜单：{node.MenuID}\n" +
                               $"关联窗体：{node.FormName}";

                MessageBox.Show(message, "节点信息", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 如果节点关联了菜单，可以打开对应的业务单据
                if (!string.IsNullOrEmpty(node.MenuID))
                {
                    // 这里可以调用菜单打开逻辑
                    // 例如：menuHelper.ExecuteEvents(menuInfo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理节点点击事件失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NavigationManager_CurrentNavigationChanged(object sender, ProcessNavigationEventArgs e)
        {
            try
            {
                var navigation = e.Navigation;
                
                // 更新窗体标题
                this.Text = $"流程导航图示例 - {navigation?.Description ?? "未选择"}";

                // 可以在这里添加其他处理逻辑
                Console.WriteLine($"当前流程导航图已切换到：{navigation?.Description}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理流程导航图切换事件失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helper Methods

        private string GetUsageInfo()
        {
            return @"流程导航图使用说明

1. 功能概述
流程导航图是ERP系统中的一个重要功能，用于创建美观的业务流程示意图，帮助用户快速了解业务流程并直接访问相关功能。

2. 主要特性
• 可视化流程设计：通过拖拽方式创建业务流程图
• 节点关联业务：每个节点可以关联具体的业务单据或功能
• 交互式导航：点击节点可直接打开对应的业务窗体
• 灵活布局：支持多种布局方式和自定义样式
• 权限控制：根据用户权限显示相应的流程节点

3. 使用步骤
3.1 创建流程导航图
• 在工作流设计器中选择'流程导航图'菜单
• 点击'新建流程导航图'创建新的流程图
• 从工具箱拖拽'流程导航节点'到设计区域

3.2 配置节点属性
• 右键点击节点，选择'属性'
• 设置节点名称和描述
• 选择关联的业务菜单
• 自定义节点颜色和样式

3.3 连接节点
• 使用连接工具连接相关节点
• 设置连接线的样式和标签
• 调整整体布局使其美观

3.4 保存和发布
• 保存流程导航图到数据库
• 设置为默认流程图（可选）
• 在主系统中展示和使用

4. 节点类型说明
• 开始节点：流程的起始点
• 业务节点：关联具体业务功能
• 决策节点：表示流程分支
• 结束节点：流程的终止点

5. 最佳实践
• 保持流程图简洁明了
• 使用统一的颜色和样式
• 合理安排节点位置
• 添加清晰的描述信息
• 定期更新流程内容

6. 注意事项
• 确保关联的菜单存在且有权限
• 测试节点链接的有效性
• 考虑不同用户的权限差异
• 保持流程图与实际业务一致

7. 技术支持
如需技术支持或遇到问题，请联系系统管理员或开发团队。";
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///// 设置指定的流程导航图
        ///// </summary>
        ///// <param name="navigationId">流程导航图ID</param>
        //public void SetNavigation(string navigationId)
        //{
        //    try
        //    {
        //        _navigationManager?.SetCurrentNavigation(navigationId);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"设置流程导航图失败：{ex.Message}", "错误", 
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        /// <summary>
        /// 刷新流程导航图
        /// </summary>
        public void RefreshNavigation()
        {
            try
            {
                _navigationManager?.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新流程导航图失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}