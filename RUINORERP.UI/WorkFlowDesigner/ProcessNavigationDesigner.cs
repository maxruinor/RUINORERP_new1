using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.UI;
using RUINORERP.Model;
using RUINORERP.Services;
using RUINORERP.IServices;
using RUINORERP.UI.WorkFlowDesigner.Nodes;

namespace RUINORERP.UI.WorkFlowDesigner
{
    /// <summary>
    /// 流程导航图设计器
    /// 增强版设计器，支持业务节点模板拖拽和模块化设计
    /// </summary>
    public partial class ProcessNavigationDesigner : UserControl
    {
        #region Fields

        private GraphControl _graphControl;
        private SplitContainer _splitContainer;
        private TreeView _templateTreeView;
        private Panel _designPanel;
        private ToolStrip _toolStrip;
        private BusinessNodeTemplateManager _templateManager;
        private Itb_ProcessNavigationServices _processNavigationService;
        private Itb_MenuInfoServices _menuInfoService;
        private Itb_ModuleDefinitionServices _moduleDefinitionService;
        private tb_ProcessNavigation _currentNavigation;
        private ProcessNavigationMode _currentMode = ProcessNavigationMode.设计模式;

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
        /// 当前模式
        /// </summary>
        public ProcessNavigationMode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                _currentMode = value;
                OnModeChanged();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// 流程导航图改变事件
        /// </summary>
        public event EventHandler<EventArgs> NavigationChanged;

        /// <summary>
        /// 模式改变事件
        /// </summary>
        public event EventHandler<ProcessNavigationModeEventArgs> ModeChanged;

        #endregion

        #region Constructor

        public ProcessNavigationDesigner()
        {
            InitializeComponent();
            InitializeServices();
            InitializeTemplateManager();
            LoadTemplates();
        }

        private void InitializeServices()
        {
            try
            {
                _processNavigationService = Startup.GetFromFac<Itb_ProcessNavigationServices>();
                _menuInfoService = Startup.GetFromFac<Itb_MenuInfoServices>();
                _moduleDefinitionService = Startup.GetFromFac<Itb_ModuleDefinitionServices>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化服务失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeTemplateManager()
        {
            _templateManager = new BusinessNodeTemplateManager();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 主分割容器
            _splitContainer = new SplitContainer();
            _splitContainer.Dock = DockStyle.Fill;
            _splitContainer.SplitterDistance = 250;
            _splitContainer.SplitterWidth = 5;

            // 左侧模板面板
            var leftPanel = new Panel();
            leftPanel.Dock = DockStyle.Fill;
            leftPanel.Padding = new Padding(5);

            // 模板树形控件
            _templateTreeView = new TreeView();
            _templateTreeView.Dock = DockStyle.Fill;
            _templateTreeView.LabelEdit = false;
            _templateTreeView.ShowPlusMinus = true;
            _templateTreeView.ShowLines = true;
            _templateTreeView.ItemDrag += TemplateTreeView_ItemDrag;
            _templateTreeView.AfterSelect += TemplateTreeView_AfterSelect;

            leftPanel.Controls.Add(_templateTreeView);

            // 右侧设计面板
            _designPanel = new Panel();
            _designPanel.Dock = DockStyle.Fill;
            _designPanel.Padding = new Padding(5);

            // 图形控件
            _graphControl = new GraphControl();
            _graphControl.Dock = DockStyle.Fill;
            _graphControl.AllowAddConnection = true;
            _graphControl.AllowAddShape = true;
            _graphControl.AllowDeleteShape = true;
            _graphControl.AllowMoveShape = true;
            // Note: AllowResize property may not exist, check if needed
            _graphControl.ShowGrid = true;
            _graphControl.BackColor = Color.White;
            _graphControl.DragDrop += GraphControl_DragDrop;
            _graphControl.DragEnter += GraphControl_DragEnter;
            // Note: OnShapeClick event may not exist, shapes handle their own mouse events
            // _graphControl.OnShapeClick += GraphControl_OnShapeClick;

            // 注册节点类型 - Note: RegisterShape method may not exist in this version
            // _graphControl.RegisterShape(typeof(ProcessNavigationNode));

            _designPanel.Controls.Add(_graphControl);

            // 工具栏
            _toolStrip = new ToolStrip();
            _toolStrip.Dock = DockStyle.Top;
            _toolStrip.GripStyle = ToolStripGripStyle.Hidden;

            // 工具栏按钮
            var btnNew = new ToolStripButton("新建");
            btnNew.Click += BtnNew_Click;

            var btnOpen = new ToolStripButton("打开");
            btnOpen.Click += BtnOpen_Click;

            var btnSave = new ToolStripButton("保存");
            btnSave.Click += BtnSave_Click;

            var btnMode = new ToolStripButton("切换模式");
            btnMode.Click += BtnMode_Click;

            var separator1 = new ToolStripSeparator();
            var separator2 = new ToolStripSeparator();

            var lblMode = new ToolStripLabel();
            lblMode.Text = "当前模式：设计模式";
            lblMode.Name = "lblMode";

            _toolStrip.Items.AddRange(new ToolStripItem[] {
                btnNew, btnOpen, btnSave, separator1, btnMode, separator2, lblMode
            });

            // 设置分割容器面板
            _splitContainer.Panel1.Controls.Add(leftPanel);
            _splitContainer.Panel2.Controls.Add(_designPanel);

            // 添加控件
            this.Controls.Add(_toolStrip);
            this.Controls.Add(_splitContainer);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Template Loading

        /// <summary>
        /// 加载模板
        /// </summary>
        private void LoadTemplates()
        {
            try
            {
                _templateTreeView.Nodes.Clear();

                // 添加业务模块节点
                var moduleNode = _templateTreeView.Nodes.Add("业务模块", "业务模块");
                moduleNode.Tag = "Modules";

                foreach (ERPBusinessModule module in Enum.GetValues(typeof(ERPBusinessModule)))
                {
                    if (module == ERPBusinessModule.未分类) continue;

                    var moduleChildNode = moduleNode.Nodes.Add(module.ToString(), module.ToString());
                    moduleChildNode.Tag = module;

                    // 添加该模块的节点模板
                    var templates = _templateManager.GetModuleTemplates(module);
                    foreach (var template in templates)
                    {
                        var templateNode = moduleChildNode.Nodes.Add(template.Name, template.Name);
                        templateNode.Tag = template;
                        templateNode.ForeColor = template.DefaultColor;
                    }
                }

                // 添加菜单节点
                var menuNode = _templateTreeView.Nodes.Add("菜单节点", "菜单节点");
                menuNode.Tag = "Menus";

                // 动态加载菜单（这里简化处理）
                LoadMenuNodes(menuNode);

                // 添加通用节点
                var commonNode = _templateTreeView.Nodes.Add("通用节点", "通用节点");
                commonNode.Tag = "Common";

                var startTemplate = new BusinessNodeTemplate
                {
                    Name = "开始节点",
                    Description = "流程开始节点",
                    BusinessType = ProcessNavigationNodeBusinessType.通用节点,
                    NodeType = ProcessNavigationNodeType.开始节点,
                    DefaultColor = Color.LightGreen,
                    Icon = "🚀",
                    Category = "通用节点"
                };

                var endTemplate = new BusinessNodeTemplate
                {
                    Name = "结束节点",
                    Description = "流程结束节点",
                    BusinessType = ProcessNavigationNodeBusinessType.通用节点,
                    NodeType = ProcessNavigationNodeType.结束节点,
                    DefaultColor = Color.LightCoral,
                    Icon = "🏁",
                    Category = "通用节点"
                };

                var startNode = commonNode.Nodes.Add(startTemplate.Name, startTemplate.Name);
                startNode.Tag = startTemplate;
                startNode.ForeColor = startTemplate.DefaultColor;

                var endNode = commonNode.Nodes.Add(endTemplate.Name, endTemplate.Name);
                endNode.Tag = endTemplate;
                endNode.ForeColor = endTemplate.DefaultColor;

                // 展开第一个节点
                if (_templateTreeView.Nodes.Count > 0)
                {
                    _templateTreeView.Nodes[0].Expand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载模板失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载菜单节点
        /// </summary>
        /// <param name="parentNode">父节点</param>
        private async void LoadMenuNodes(TreeNode parentNode)
        {
            try
            {
                // 这里应该从数据库加载菜单，简化处理
                var commonMenus = new List<string>
                {
                    "系统设置", "用户管理", "权限管理", "数据字典",
                    "日志查询", "备份恢复", "系统监控"
                };

                foreach (var menuName in commonMenus)
                {
                    var menuTemplate = new BusinessNodeTemplate
                    {
                        Name = menuName,
                        Description = $"菜单：{menuName}",
                        BusinessType = ProcessNavigationNodeBusinessType.菜单节点,
                        NodeType = ProcessNavigationNodeType.流程导航节点,
                        DefaultColor = Color.LightBlue,
                        Icon = "📋",
                        Category = "系统菜单"
                    };

                    var menuNode = parentNode.Nodes.Add(menuTemplate.Name, menuTemplate.Name);
                    menuNode.Tag = menuTemplate;
                    menuNode.ForeColor = menuTemplate.DefaultColor;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载菜单节点失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 模板树拖拽开始
        /// </summary>
        private void TemplateTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item is TreeNode node && node.Tag is BusinessNodeTemplate)
            {
                DoDragDrop(node.Tag, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 模板树选择改变
        /// </summary>
        private void TemplateTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 可以在这里显示模板的详细信息
        }

        /// <summary>
        /// 图形控件拖拽进入
        /// </summary>
        private void GraphControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(BusinessNodeTemplate)) != null)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 图形控件拖拽放下
        /// </summary>
        private void GraphControl_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(typeof(BusinessNodeTemplate)) is BusinessNodeTemplate template)
                {
                    // 计算节点位置
                    Point clientPoint = new Point(e.X, e.Y);
                    PointF position = new PointF(clientPoint.X, clientPoint.Y);

                    // 创建节点
                    var node = _templateManager.CreateProcessNavigationNode(template, position);

                    // 添加到图形控件
                    _graphControl.AddShape(node);

                    // 刷新显示
                    _graphControl.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加节点失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 图形控件节点点击
        /// </summary>
        private void GraphControl_OnShapeClick(object sender, Shape shape)
        {
            // 在设计模式下，确保被点击的节点移到顶层以解决覆盖问题
            if (CurrentMode == ProcessNavigationMode.设计模式)
            {
                // 将当前节点设置为最高Z-order
                BringShapeToFront(shape);
            }
            
            // 在预览模式下执行相应操作
            if (CurrentMode == ProcessNavigationMode.预览模式 && shape is ProcessNavigationNode node)
            {
                ExecuteNodeAction(node);
            }
        }
        
        /// <summary>
        /// 将图形移到最顶层
        /// </summary>
        /// <param name="shape">要移动的图形</param>
        private void BringShapeToFront(Shape shape)
        {
            if (shape == null || _graphControl == null || _graphControl.Shapes == null)
                return;
            
            try
            {
                // 找到当前最大的Z-order
                int maxZOrder = 0;
                foreach (Shape s in _graphControl.Shapes)
                {
                    if (s.ZOrder > maxZOrder)
                        maxZOrder = s.ZOrder;
                }
                
                // 将选中的图形设置为新的最高Z-order
                shape.ZOrder = maxZOrder + 1;
                
                // 选中该图形
                shape.IsSelected = true;
                
                // 刷新图形控件
                _graphControl.Refresh();
            }
            catch (Exception ex)
            {
                // 记录错误但不中断操作
                System.Diagnostics.Debug.WriteLine($"BringShapeToFront 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 新建按钮点击
        /// </summary>
        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("是否保存当前流程导航图？", "提示",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    SaveNavigation();
                }

                // 创建新的流程导航图
                CurrentNavigation = new tb_ProcessNavigation
                {
                    ProcessNavName = "新建流程导航图",
                    Description = "新建的流程导航图",
                    Version = 1,
                    NavigationLevel = (int)ProcessNavigationLevel.业务图,
                    IsActive = true,
                    IsDefault = false
                };

                // 使用时间更新帮助类设置时间
                CurrentNavigation.SetTimeBeforeSave();

                // 清空图形控件
                _graphControl.Shapes.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新建流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 打开按钮点击
        /// </summary>
        private async void BtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                // 这里应该打开一个选择对话框，简化处理
                var navigations = await _processNavigationService.QueryAsync()
                    .ContinueWith(t => t.Result.Where(x => x.IsActive).ToList());

                if (navigations.Count == 0)
                {
                    MessageBox.Show("没有可用的流程导航图", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 选择第一个（实际应该让用户选择）
                CurrentNavigation = navigations[0];
                LoadNavigationContent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存按钮点击
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveNavigation();
        }

        /// <summary>
        /// 切换模式按钮点击
        /// </summary>
        private void BtnMode_Click(object sender, EventArgs e)
        {
            CurrentMode = CurrentMode == ProcessNavigationMode.设计模式
                ? ProcessNavigationMode.预览模式
                : ProcessNavigationMode.设计模式;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 保存流程导航图
        /// </summary>
        private async void SaveNavigation()
        {
            try
            {
                if (CurrentNavigation == null)
                {
                    MessageBox.Show("没有可保存的流程导航图", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 生成XML
                var xml = GenerateGraphXml();

                // 保存到数据库
                CurrentNavigation.GraphXml = xml;
                CurrentNavigation.SetTimeBeforeSave();

                bool result;
                if (CurrentNavigation.ProcessNavID == 0)
                {
                    result = await _processNavigationService.Add(CurrentNavigation) > 0;
                }
                else
                {
                    result = await _processNavigationService.Update(CurrentNavigation);
                }

                if (result)
                {
                    MessageBox.Show("保存成功", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("保存失败", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载流程导航图内容
        /// </summary>
        private void LoadNavigationContent()
        {
            try
            {
                if (CurrentNavigation == null || string.IsNullOrEmpty(CurrentNavigation.GraphXml))
                {
                    _graphControl.Shapes.Clear();
                    return;
                }

                // 从XML加载图形
                using (var stringReader = new System.IO.StringReader(CurrentNavigation.GraphXml))
                {
                    // TODO: Implement XML loading to graph control
                    // _graphControl.Load(stringReader); // Method may not exist
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载流程导航图内容失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成图形XML
        /// </summary>
        /// <returns>XML字符串</returns>
        private string GenerateGraphXml()
        {
            try
            {
                using (var stringWriter = new System.IO.StringWriter())
                {
                    // TODO: Implement XML saving from graph control
                    // _graphControl.Save(stringWriter); // Method may not exist
                    return CurrentNavigation.GraphXml; // Return existing XML for now
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"生成图形XML失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 执行节点操作
        /// </summary>
        /// <param name="node">节点</param>
        private void ExecuteNodeAction(ProcessNavigationNode node)
        {
            try
            {
                // 安全转换业务类型
                if (Enum.IsDefined(typeof(ProcessNavigationNodeBusinessType), node.BusinessType))
                {
                    switch ((ProcessNavigationNodeBusinessType)node.BusinessType)
                    {
                    case ProcessNavigationNodeBusinessType.菜单节点:
                        if (!string.IsNullOrEmpty(node.MenuID))
                        {
                            // 打开菜单
                           // MenuHelperExtensions.OpenMenu(node.MenuID);
                        }
                        break;

                    case ProcessNavigationNodeBusinessType.模块节点:
                        if (node.ModuleID.HasValue)
                        {
                            // 打开模块导航图
                            OpenModuleNavigation(node.ModuleID.Value);
                        }
                        break;

                    case ProcessNavigationNodeBusinessType.流程节点:
                        //if (node.ChildNavigationID.HasValue)
                        //{
                        //    // 打开子流程导航图
                        //    OpenChildNavigation(node.ChildNavigationID.Value);
                        //}
                        break;
                    default:
                        MessageBox.Show($"节点类型 {node.BusinessType} 暂不支持操作", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                }
                else
                {
                    MessageBox.Show($"未知的节点类型值: {node.BusinessType}", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行节点操作失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 打开模块导航图
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        private async void OpenModuleNavigation(long moduleId)
        {
            try
            {
                var allNavigations = await _processNavigationService.QueryAsync();
                var navigations = allNavigations.Where(x => x.ModuleID == moduleId && x.IsActive).ToList();

                if (navigations.Count > 0)
                {
                    CurrentNavigation = navigations[0];
                    LoadNavigationContent();
                }
                else
                {
                    MessageBox.Show("该模块没有可用的导航图", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开模块导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 打开子流程导航图
        /// </summary>
        /// <param name="navigationId">导航图ID</param>
        private async void OpenChildNavigation(long navigationId)
        {
            try
            {
                var navigation = await _processNavigationService.QueryByIdAsync(navigationId);
                if (navigation != null)
                {
                    CurrentNavigation = navigation;
                    LoadNavigationContent();
                }
                else
                {
                    MessageBox.Show("子流程导航图不存在", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开子流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Event Triggers

        /// <summary>
        /// 当前流程导航图改变
        /// </summary>
        protected virtual void OnCurrentNavigationChanged()
        {
            LoadNavigationContent();
            NavigationChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 模式改变
        /// </summary>
        protected virtual void OnModeChanged()
        {
            // 更新工具栏显示
            var lblMode = _toolStrip.Items.Find("lblMode", false).FirstOrDefault() as ToolStripLabel;
            if (lblMode != null)
            {
                lblMode.Text = $"当前模式：{CurrentMode}";
            }

            // 根据模式设置图形控件属性
            if (CurrentMode == ProcessNavigationMode.设计模式)
            {
                _graphControl.AllowAddConnection = true;
                _graphControl.AllowAddShape = true;
                _graphControl.AllowDeleteShape = true;
                _graphControl.AllowMoveShape = true;
                // Note: AllowResize property may not exist
                _graphControl.ShowGrid = true;
            }
            else
            {
                _graphControl.AllowAddConnection = false;
                _graphControl.AllowAddShape = false;
                _graphControl.AllowDeleteShape = false;
                _graphControl.AllowMoveShape = false;
                // Note: AllowResize property may not exist
                _graphControl.ShowGrid = false;
            }

            ModeChanged?.Invoke(this, new ProcessNavigationModeEventArgs(CurrentMode));
        }

        #endregion
    }

    /// <summary>
    /// 流程导航模式事件参数
    /// </summary>
    public class ProcessNavigationModeEventArgs : EventArgs
    {
        public ProcessNavigationMode Mode { get; }

        public ProcessNavigationModeEventArgs(ProcessNavigationMode mode)
        {
            Mode = mode;
        }
    }
}