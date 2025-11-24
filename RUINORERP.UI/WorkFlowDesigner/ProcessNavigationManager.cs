using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.UI;
using RUINORERP.Model;
using RUINORERP.Services;
using RUINORERP.IServices;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using SqlSugar;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 流程导航图管理器
    /// 负责在ERP主系统中展示和管理流程导航图
    /// </summary>
    public class ProcessNavigationManager : UserControl
    {
        #region Fields

        private GraphControl _graphControl;
        private Panel _mainPanel;
        private ToolStrip _toolStrip;
        private ToolStripButton _btnRefresh;
        private ToolStripComboBox _cmbNavigationList;
        private ToolStripLabel _lblStatus;
        private Itb_ProcessNavigationServices _processNavigationService;
        private Itb_ProcessNavigationNodeServices _processNavigationNodeService;
        private List<tb_ProcessNavigation> _navigationList;
        private tb_ProcessNavigation _currentNavigation;

        #endregion

        #region Properties

        /// <summary>
        /// 当前流程导航图
        /// </summary>
        public tb_ProcessNavigation CurrentNavigation
        {
            get { return _currentNavigation; }
            set
            {
                _currentNavigation = value;
                OnCurrentNavigationChanged();
            }
        }

        /// <summary>
        /// 流程导航图列表
        /// </summary>
        public List<tb_ProcessNavigation> NavigationList
        {
            get { return _navigationList; }
            set
            {
                _navigationList = value;
                UpdateNavigationComboBox();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 当前流程导航图改变事件
        /// </summary>
        public event EventHandler<ProcessNavigationEventArgs> CurrentNavigationChanged;

        /// <summary>
        /// 节点点击事件
        /// </summary>
        public event EventHandler<ProcessNodeClickEventArgs> NodeClick;

        #endregion

        #region Constructor

        public ProcessNavigationManager()
        {
            InitializeComponent();
            InitializeGraphControl();
            InitializeServices();
            LoadNavigationList();
        }

        private void InitializeServices()
        {
            try
            {
                _processNavigationService = Startup.GetFromFac<Itb_ProcessNavigationServices>();
                _processNavigationNodeService = Startup.GetFromFac<Itb_ProcessNavigationNodeServices>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化服务失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 主面板
            _mainPanel = new Panel();
            _mainPanel.Dock = DockStyle.Fill;
            _mainPanel.BackColor = Color.White;

            // 工具栏
            _toolStrip = new ToolStrip();
            _toolStrip.Dock = DockStyle.Top;
            _toolStrip.GripStyle = ToolStripGripStyle.Hidden;

            // 刷新按钮
            _btnRefresh = new ToolStripButton();
            _btnRefresh.Text = "刷新";
            _btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnRefresh.Click += BtnRefresh_Click;

            // 流程导航图下拉框
            _cmbNavigationList = new ToolStripComboBox();
            _cmbNavigationList.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbNavigationList.Width = 200;
            _cmbNavigationList.SelectedIndexChanged += CmbNavigationList_SelectedIndexChanged;

            // 状态标签
            _lblStatus = new ToolStripLabel();
            _lblStatus.TextAlign = ContentAlignment.MiddleRight;

            // 添加工具栏项
            _toolStrip.Items.AddRange(new ToolStripItem[] {
                _btnRefresh,
                new ToolStripSeparator(),
                new ToolStripLabel("流程导航图:"),
                _cmbNavigationList,
                new ToolStripSeparator(),
                _lblStatus
            });

            // 添加控件
            this.Controls.Add(_mainPanel);
            this.Controls.Add(_toolStrip);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void InitializeGraphControl()
        {
            try
            {
                // 创建图形控件
                _graphControl = new GraphControl();
                _graphControl.Dock = DockStyle.Fill;
                _graphControl.AllowAddConnection = false;
                _graphControl.AllowAddShape = false;
                _graphControl.AllowDeleteShape = false;
                _graphControl.AllowMoveShape = false;
                // Note: AllowResize and AllowRotate properties may not exist
                _graphControl.ShowGrid = false;
                _graphControl.BackColor = Color.White;

                // 注册节点类型 - Note: RegisterShape method may not exist in this version
                // _graphControl.RegisterShape(typeof(ProcessNavigationNode));

                // 添加事件处理 - Note: OnShapeClick event may not exist, using OnContextMenu instead
                _graphControl.OnContextMenu += GraphControl_OnContextMenu;

                // 添加到主面板
                _mainPanel.Controls.Add(_graphControl);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化图形控件失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Data Loading

        /// <summary>
        /// 加载流程导航图列表
        /// </summary>
        private async void LoadNavigationList()
        {
            try
            {
                // 获取流程导航图列表
                NavigationList = await _processNavigationService.QueryAsync();

                // 如果有默认流程导航图，自动加载
                var defaultNavigation = NavigationList.FirstOrDefault(n => n.IsDefault);
                if (defaultNavigation != null)
                {
                    CurrentNavigation = defaultNavigation;
                }
                else if (NavigationList.Count > 0)
                {
                    CurrentNavigation = NavigationList[0];
                }

                UpdateStatus($"已加载 {NavigationList.Count} 个流程导航图");
            }
            catch (Exception ex)
            {
                UpdateStatus($"加载流程导航图列表失败：{ex.Message}");
                MessageBox.Show($"加载流程导航图列表失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载流程导航图内容
        /// </summary>
        /// <param name="navigation">流程导航图定义</param>
        private async void LoadNavigationContent(tb_ProcessNavigation navigation)
        {
            try
            {
                if (navigation == null || string.IsNullOrEmpty(navigation.GraphXml))
                {
                    _graphControl.Shapes.Clear();
                    return;
                }

                // 从XML加载图形
                using (var stringReader = new StringReader(navigation.GraphXml))
                {
                    // TODO: Implement XML loading to graph control
                    // _graphControl.Load(stringReader); // Method may not exist
                }

                // 自动布局
                AutoLayout();

                UpdateStatus($"已加载流程导航图：{navigation.ProcessNavName}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"加载流程导航图内容失败：{ex.Message}");
                MessageBox.Show($"加载流程导航图内容失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region UI Updates

        /// <summary>
        /// 更新流程导航图下拉框
        /// </summary>
        private void UpdateNavigationComboBox()
        {
            try
            {
                _cmbNavigationList.Items.Clear();
                
                if (NavigationList != null)
                {
                    foreach (var navigation in NavigationList)
                    {
                        _cmbNavigationList.Items.Add(navigation.ProcessNavName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新流程导航图下拉框失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新状态信息
        /// </summary>
        /// <param name="message">状态消息</param>
        private void UpdateStatus(string message)
        {
            try
            {
                if (_lblStatus != null)
                {
                    _lblStatus.Text = message;
                }
            }
            catch (Exception ex)
            {
                // 忽略状态更新错误
            }
        }

        #endregion

        #region Event Handlers

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                 LoadNavigationList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新失败：{ex.Message}", "错误",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbNavigationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (_cmbNavigationList.SelectedIndex >= 0 && NavigationList != null)
                {
                    CurrentNavigation = NavigationList[_cmbNavigationList.SelectedIndex];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择流程导航图失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 

        private void GraphControl_OnContextMenu(object sender, MouseEventArgs e)
        {
            try
            {
                // TODO: Implement shape hit detection
                // var hitShape = GetShapeAtPoint(e.Location);
                // if (hitShape is ProcessNavigationNode node)
                // {
                //     // 触发节点点击事件
                //     OnNodeClick(new ProcessNodeClickEventArgs(node));
                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理节点点击事件失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Triggers

        protected virtual void OnCurrentNavigationChanged()
        {
            try
            {
                LoadNavigationContent(CurrentNavigation);
                CurrentNavigationChanged?.Invoke(this, new ProcessNavigationEventArgs(CurrentNavigation));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理当前流程导航图改变事件失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void OnNodeClick(ProcessNodeClickEventArgs e)
        {
            try
            {
                NodeClick?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理节点点击事件失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Layout

        /// <summary>
        /// 自动布局
        /// </summary>
        private void AutoLayout()
        {
            try
            {
                // 简单的网格布局
                var shapes = _graphControl.Shapes.OfType<ProcessNavigationNode>().ToList();
                if (shapes.Count == 0) return;

                float x = 50;
                float y = 50;
                float maxWidth = 0;
                int row = 0;
                int maxPerRow = 4;

                for (int i = 0; i < shapes.Count; i++)
                {
                    var shape = shapes[i];
                    shape.Rectangle = new RectangleF(x, y, shape.Rectangle.Width, shape.Rectangle.Height);

                    x += shape.Rectangle.Width + 30;
                    maxWidth = Math.Max(maxWidth, x);

                    if ((i + 1) % maxPerRow == 0)
                    {
                        x = 50;
                        y += shape.Rectangle.Height + 30;
                        row++;
                    }
                }

                // 调整图形控件大小
                _graphControl.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"自动布局失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 刷新流程导航图
        /// </summary>
        public void Refresh()
        {
            LoadNavigationList();
        }

        /// <summary>
        /// 设置当前流程导航图
        /// </summary>
        /// <param name="navigationId">流程导航图ID</param>
        public void SetCurrentNavigation(long navigationId)
        {
            try
            {
                var navigation = NavigationList?.FirstOrDefault(n => n.ProcessNavID == navigationId);
                if (navigation != null)
                {
                    CurrentNavigation = navigation;
                    
                    // 更新下拉框选择
                    int index = NavigationList.FindIndex(n => n.ProcessNavID == navigation.ProcessNavID);
                    if (index >= 0)
                    {
                        _cmbNavigationList.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置当前流程导航图失败：{ex.Message}", "错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }

    #region Event Args Classes

    /// <summary>
    /// 流程导航图事件参数
    /// </summary>
    public class ProcessNavigationEventArgs : EventArgs
    {
        public tb_ProcessNavigation Navigation { get; }

        public ProcessNavigationEventArgs(tb_ProcessNavigation navigation)
        {
            Navigation = navigation;
        }
    }

    /// <summary>
    /// 流程节点点击事件参数
    /// </summary>
    public class ProcessNodeClickEventArgs : EventArgs
    {
        public ProcessNavigationNode Node { get; }

        public ProcessNodeClickEventArgs(ProcessNavigationNode node)
        {
            Node = node;
        }
    }

    #endregion
}