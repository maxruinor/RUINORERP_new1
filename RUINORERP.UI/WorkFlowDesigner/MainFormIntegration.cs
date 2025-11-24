using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.Services;
using RUINORERP.IServices;
using RUINORERP.UI.WorkFlowDesigner;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI
{
    /// <summary>
    /// 主窗体流程导航图集成类
    /// 负责在主窗体启动时加载和显示流程导航图
    /// </summary>
    public partial class MainForm
    {
        #region Fields

        private ProcessNavigationManager _processNavigationManager;
        private ProcessNavigationDesigner _processNavigationDesigner;
        private Panel _navigationPanel;
        private SplitContainer _navigationSplitContainer;
        private Itb_ProcessNavigationServices _processNavigationService;
        private Itb_ModuleDefinitionServices _moduleDefinitionService;
        private tb_ProcessNavigation _currentNavigation;
        private bool _navigationInitialized = false;

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

        #endregion

        #region Initialization Methods

        /// <summary>
        /// 初始化流程导航图功能
        /// </summary>
        private async Task InitializeProcessNavigationAsync()
        {
            try
            {
                if (_navigationInitialized)
                {
                    return;
                }

                // 初始化服务
                _processNavigationService = Startup.GetFromFac<Itb_ProcessNavigationServices>();
                _moduleDefinitionService = Startup.GetFromFac<Itb_ModuleDefinitionServices>();

                // 创建导航面板
                CreateNavigationPanel();

                // 加载默认流程导航图
                await LoadDefaultNavigationAsync();

                _navigationInitialized = true;

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
        /// 创建导航面板
        /// </summary>
        private void CreateNavigationPanel()
        {
            try
            {
                // 创建主分割容器
                _navigationSplitContainer = new SplitContainer();
                _navigationSplitContainer.Dock = DockStyle.Fill;
                _navigationSplitContainer.SplitterDistance = 300;
                _navigationSplitContainer.SplitterWidth = 5;
                _navigationSplitContainer.Orientation = Orientation.Horizontal;

                // 创建导航管理器（上方）
                _processNavigationManager = new ProcessNavigationManager();
                _processNavigationManager.Dock = DockStyle.Fill;
                _processNavigationManager.NodeClick += ProcessNavigationManager_NodeClick;
                _processNavigationManager.CurrentNavigationChanged += ProcessNavigationManager_CurrentNavigationChanged;

                // 创建导航设计器（下方，初始隐藏）
                _processNavigationDesigner = new ProcessNavigationDesigner();
                _processNavigationDesigner.Dock = DockStyle.Fill;
                _processNavigationDesigner.Visible = false;
                _processNavigationDesigner.ModeChanged += ProcessNavigationDesigner_ModeChanged;

                // 创建导航面板
                _navigationPanel = new Panel();
                _navigationPanel.Dock = DockStyle.Fill;
                _navigationPanel.BackColor = Color.White;

                // 设置分割容器面板
                _navigationSplitContainer.Panel1.Controls.Add(_processNavigationManager);
                _navigationSplitContainer.Panel2.Controls.Add(_processNavigationDesigner);

                _navigationPanel.Controls.Add(_navigationSplitContainer);

                // 将导航面板添加到主窗体的控制中心容器中
                // 这里假设主窗体有一个名为 controlCenterContainer 的容器
                // 实际使用时需要根据主窗体的具体结构进行调整
                AddNavigationToMainForm();

                logger?.LogInformation("导航面板创建完成");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "创建导航面板失败");
                throw;
            }
        }

        /// <summary>
        /// 将导航面板添加到主窗体
        /// </summary>
        private void AddNavigationToMainForm()
        {
            try
            {
                // 方法1：如果主窗体有控制中心容器
                var controlCenter = this.Controls.Find("controlCenterContainer", true).FirstOrDefault() as Control;
                if (controlCenter != null)
                {
                    controlCenter.Controls.Add(_navigationPanel);
                    return;
                }

                // 方法2：如果主窗体有工作区容器
                var workspace = this.Controls.Find("workspaceContainer", true).FirstOrDefault() as Control;
                if (workspace != null)
                {
                    workspace.Controls.Add(_navigationPanel);
                    return;
                }

                // 方法3：直接添加到主窗体
                this.Controls.Add(_navigationPanel);
                _navigationPanel.BringToFront();

                logger?.LogInformation("导航面板已添加到主窗体");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "添加导航面板到主窗体失败");
                throw;
            }
        }

        #endregion

        #region Navigation Loading

        /// <summary>
        /// 加载默认流程导航图
        /// </summary>
        private async Task LoadDefaultNavigationAsync()
        {
            try
            {
                // 1. 首先尝试加载系统总览图
                var allNavigations = await _processNavigationService.QueryAsync();
                var totalNavigation = allNavigations.FirstOrDefault(x => x.NavigationLevel == 0 && x.IsActive && x.IsDefault);

                if (totalNavigation != null)
                {
                    CurrentNavigation = totalNavigation;
                    _processNavigationManager.CurrentNavigation = totalNavigation;
                    logger?.LogInformation($"已加载系统总览图：{totalNavigation.ProcessNavName}");
                    return;
                }

                // 2. 如果没有默认总览图，加载第一个总览图
                var firstTotalNavigation = allNavigations
                    .Where(x => x.NavigationLevel == 0 && x.IsActive)
                    .OrderBy(x => x.CreateTime)
                    .First();

                if (firstTotalNavigation != null)
                {
                    CurrentNavigation = firstTotalNavigation;
                    _processNavigationManager.CurrentNavigation = firstTotalNavigation;
                    logger?.LogInformation($"已加载第一个总览图：{firstTotalNavigation.ProcessNavName}");
                    return;
                }

                // 3. 如果没有总览图，根据用户权限加载模块图
                await LoadModuleNavigationByUserPermissionAsync();

            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "加载默认流程导航图失败");
                MessageBox.Show($"加载默认流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 根据用户权限加载模块导航图
        /// </summary>
        private async Task LoadModuleNavigationByUserPermissionAsync()
        {
            try
            {
                // 获取用户可访问的模块
                var accessibleModules = await GetUserAccessibleModulesAsync();

                if (accessibleModules.Count == 0)
                {
                    logger?.LogWarning("用户没有可访问的模块");
                    return;
                }

                var allNavigations = await _processNavigationService.QueryAsync();

                // 加载第一个可访问模块的默认导航图
                var moduleNavigation = allNavigations
                    .FirstOrDefault(x => x.ModuleID.HasValue &&
                               accessibleModules.Contains(x.ModuleID.Value) &&
                               x.IsActive &&
                               x.IsDefault);

                if (moduleNavigation != null)
                {
                    CurrentNavigation = moduleNavigation;
                    _processNavigationManager.CurrentNavigation = moduleNavigation;
                    logger?.LogInformation($"已加载模块导航图：{moduleNavigation.ProcessNavName}");
                }
                else
                {
                    logger?.LogWarning("没有找到可用的模块导航图");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "根据用户权限加载模块导航图失败");
                throw;
            }
        }

        /// <summary>
        /// 获取用户可访问的模块列表
        /// </summary>
        /// <returns>模块ID列表</returns>
        private async Task<List<long>> GetUserAccessibleModulesAsync()
        {
            try
            {
                // 这里应该根据实际的权限系统来获取用户可访问的模块
                // 简化处理，返回所有可用模块
                var allModules = await _moduleDefinitionService.QueryAsync();
                var modules = allModules
                    .Where(x => x.Available && x.Visible)
                    .Select(x => x.ModuleID)
                    .ToList();

                return modules;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "获取用户可访问模块失败");
                return new List<long>();
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// 流程导航图节点点击事件
        /// </summary>
        private void ProcessNavigationManager_NodeClick(object sender, ProcessNodeClickEventArgs e)
        {
            try
            {
                var node = e.Node;

                switch (node.BusinessType)
                {
                    //case ProcessNavigationNodeBusinessType.菜单节点:
                    //    if (node.MenuID.HasValue)
                    //    {
                    //        // 通过MenuHelper打开菜单
                    //        MenuHelperExtensions.OpenMenu(node.MenuID.Value, logger);
                    //        logger?.LogInformation($"打开菜单：{node.ProcessName} (ID: {node.MenuID.Value})");
                    //    }
                    //    break;

                    //case ProcessNavigationNodeBusinessType.模块节点:
                    //    if (node.ModuleID.HasValue)
                    //    {
                    //        // 加载模块导航图
                    //        LoadModuleNavigationAsync(node.ModuleID.Value);
                    //        logger?.LogInformation($"加载模块导航图：{node.ProcessName} (ID: {node.ModuleID.Value})");
                    //    }
                    //    break;

                    //case ProcessNavigationNodeBusinessType.流程节点:
                    //    if (node.ChildNavigationID.HasValue)
                    //    {
                    //        // 加载子流程导航图
                    //        LoadChildNavigationAsync(node.ChildNavigationID.Value);
                    //        logger?.LogInformation($"加载子流程导航图：{node.ProcessName} (ID: {node.ChildNavigationID.Value})");
                    //    }
                    //    break;

                    default:
                        logger?.LogWarning($"不支持的节点类型：{node.BusinessType}");
                        break;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理流程导航图节点点击事件失败");
                MessageBox.Show($"处理节点点击失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 流程导航图改变事件
        /// </summary>
        private void ProcessNavigationManager_CurrentNavigationChanged(object sender, ProcessNavigationEventArgs e)
        {
            try
            {
                CurrentNavigation = e.Navigation;
                logger?.LogInformation($"当前流程导航图已切换为：{e.Navigation?.ProcessNavName}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理流程导航图改变事件失败");
            }
        }

        /// <summary>
        /// 导航设计器模式改变事件
        /// </summary>
        private void ProcessNavigationDesigner_ModeChanged(object sender, ProcessNavigationModeEventArgs e)
        {
            try
            {
                // 根据模式调整界面
                if (e.Mode == ProcessNavigationMode.设计模式)
                {
                    _navigationSplitContainer.SplitterDistance = 300;
                    _processNavigationDesigner.Visible = true;
                }
                else
                {
                    _navigationSplitContainer.SplitterDistance = _navigationSplitContainer.Height;
                    _processNavigationDesigner.Visible = false;
                }

                logger?.LogInformation($"导航设计器模式已切换为：{e.Mode}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理导航设计器模式改变事件失败");
            }
        }

        #endregion

        #region Navigation Operations

        /// <summary>
        /// 加载模块导航图
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        private async Task LoadModuleNavigationAsync(long moduleId)
        {
            try
            {
                var allNavigations = await _processNavigationService.QueryAsync();
                var navigation = allNavigations
                    .FirstOrDefault(x => x.ModuleID == moduleId && x.IsActive && x.IsDefault);

                if (navigation != null)
                {
                    CurrentNavigation = navigation;
                    _processNavigationManager.CurrentNavigation = navigation;
                }
                else
                {
                    // 如果没有默认导航图，加载第一个
                    var firstNavigation = allNavigations
                        .Where(x => x.ModuleID == moduleId && x.IsActive)
                        .OrderBy(x => x.CreateTime)
                        .FirstOrDefault();

                    if (firstNavigation != null)
                    {
                        CurrentNavigation = firstNavigation;
                        _processNavigationManager.CurrentNavigation = firstNavigation;
                    }
                    else
                    {
                        MessageBox.Show("该模块没有可用的导航图", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "加载模块导航图失败");
                MessageBox.Show($"加载模块导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载子流程导航图
        /// </summary>
        /// <param name="navigationId">导航图ID</param>
        private async Task LoadChildNavigationAsync(long navigationId)
        {
            try
            {
                var navigation = await _processNavigationService.QueryByIdAsync(navigationId);
                if (navigation != null && navigation.IsActive)
                {
                    CurrentNavigation = navigation;
                    _processNavigationManager.CurrentNavigation = navigation;
                }
                else
                {
                    MessageBox.Show("子流程导航图不存在或已禁用", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "加载子流程导航图失败");
                MessageBox.Show($"加载子流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新流程导航图
        /// </summary>
        public async Task RefreshProcessNavigationAsync()
        {
            try
            {
                if (_processNavigationManager != null)
                {
                    _processNavigationManager.Refresh();
                    await LoadDefaultNavigationAsync();
                    logger?.LogInformation("流程导航图已刷新");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "刷新流程导航图失败");
                MessageBox.Show($"刷新流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示/隐藏导航设计器
        /// </summary>
        public void ToggleNavigationDesigner()
        {
            try
            {
                if (_processNavigationDesigner != null)
                {
                    _processNavigationDesigner.Visible = !_processNavigationDesigner.Visible;

                    if (_processNavigationDesigner.Visible)
                    {
                        _navigationSplitContainer.SplitterDistance = 300;
                        _processNavigationDesigner.CurrentMode = ProcessNavigationMode.设计模式;
                    }
                    else
                    {
                        _navigationSplitContainer.SplitterDistance = _navigationSplitContainer.Height;
                    }

                    logger?.LogInformation($"导航设计器显示状态：{_processNavigationDesigner.Visible}");
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "切换导航设计器显示状态失败");
                MessageBox.Show($"切换导航设计器显示状态失败：{ex.Message}", "错误",
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
            try
            {
                // 更新窗体标题或其他UI元素
                if (CurrentNavigation != null)
                {
                    this.Text = $"RUINORERP - {CurrentNavigation.ProcessNavName}";
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理当前流程导航图改变事件失败");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 获取当前流程导航图信息
        /// </summary>
        /// <returns>流程导航图信息</returns>
        public tb_ProcessNavigation GetCurrentNavigation()
        {
            return CurrentNavigation;
        }

        /// <summary>
        /// 设置当前流程导航图
        /// </summary>
        /// <param name="navigationId">导航图ID</param>
        public async Task SetCurrentNavigationAsync(long navigationId)
        {
            try
            {
                var navigation = await _processNavigationService.QueryByIdAsync(navigationId);
                if (navigation != null && navigation.IsActive)
                {
                    CurrentNavigation = navigation;
                    _processNavigationManager.CurrentNavigation = navigation;
                }
                else
                {
                    MessageBox.Show("指定的流程导航图不存在或已禁用", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "设置当前流程导航图失败");
                MessageBox.Show($"设置当前流程导航图失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}