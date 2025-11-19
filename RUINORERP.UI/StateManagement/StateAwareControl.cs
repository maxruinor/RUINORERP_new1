/**
 * 文件: StateAwareControl.cs
 * 说明: 状态感知控件基类 - v3版本（简化版）
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 优化: 简化状态管理逻辑，移除冗余方法，保留核心功能
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.UI.StateManagement.UI;
using RUINORERP.Global;
using RUINORERP.UI.StateManagement.Core;


namespace RUINORERP.UI.StateManagement
{
    /// <summary>
    /// 状态感知控件基类 - v3版本（简化版）
    /// 提供统一的状态管理支持，使用v3状态管理系统
    /// 所有需要状态管理的用户控件都应继承此类
    /// </summary>
    public partial class StateAwareControl : UserControl
    {
        #region 字段

        /// <summary>
        /// v3统一状态管理器
        /// </summary>
        public IUnifiedStateManager _stateManager;

        /// <summary>
        /// v3状态上下文
        /// </summary>
        private IStatusTransitionContext _statusContext;

        /// <summary>
        /// UI状态控制器
        /// </summary>
        private IStatusUIController _uiController;

        #endregion

        #region 事件

        /// <summary>
        /// 状态变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        #endregion

        #region 构造函数

        public StateAwareControl()
        {
            InitializeComponent();
            InitializeStateManagement();
        }

        #endregion

        #region 属性

        /// <summary>
        /// v3统一状态管理器
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IUnifiedStateManager StateManager
        {
            get => _stateManager;
            set
            {
                if (_stateManager != value)
                {
                    _stateManager = value;
                    _statusContext = null;
                    _uiController = null;
                }
            }
        }

        /// <summary>
        /// v3状态上下文
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IStatusTransitionContext StatusContext
        {
            get => _statusContext;
            set
            {
                if (_statusContext != value)
                {
                    _statusContext = value;
                    OnStatusContextChanged();
                }
            }
        }

        /// <summary>
        /// UI状态控制器
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IStatusUIController UIController
        {
            get => _uiController;
            set
            {
                _uiController = value;
                OnUIControllerChanged();
            }
        }

        /// <summary>
        /// 当前数据状态
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataStatus CurrentDataStatus =>
            StatusContext?.CurrentStatus switch
            {
                DataStatus dataStatus => dataStatus,
                _ when StatusContext != null => StatusContext.GetCurrentStatus<DataStatus>(),
                _ => DataStatus.草稿
            };

        /// <summary>
        /// 绑定的实体对象
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseEntity BoundEntity { get; set; }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化状态管理
        /// </summary>
        protected virtual void InitializeStateManagement()
        {
            try
            {
                // 从服务容器获取状态管理器（现在服务已注册）
                if (Startup.ServiceProvider != null)
                {
                    _stateManager = Startup.ServiceProvider.GetService<IUnifiedStateManager>();
                    _uiController = Startup.ServiceProvider.GetService<IStatusUIController>();
                }

                // 如果仍然获取失败，记录错误信息
                if (_stateManager == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法从DI容器获取IUnifiedStateManager服务，请确保在Startup.cs中调用了builder.AddStateManager()");
                }

                if (_uiController == null)
                {
                    System.Diagnostics.Debug.WriteLine("无法从DI容器获取IStatusUIController服务，请确保在Startup.cs中调用了builder.AddStateManager()");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化状态管理失败: {ex.Message}");
            }
        }

        #endregion

        #region 状态管理

        /// <summary>
        /// 绑定实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        public virtual void BindEntity(BaseEntity entity)
        {
            if (entity == null)
            {
                UnbindEntity();
                return;
            }

            BoundEntity = entity;
            InitializeStatusContext(entity);
            ApplyCurrentStatusToUI();
        }

        /// <summary>
        /// 初始化状态上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void InitializeStatusContext(BaseEntity entity)
        {
            try
            {
                var factory = Startup.ServiceProvider?.GetService<IUnifiedStateManager>();
                // 使用单例工厂获取状态管理器，避免重复初始化

                //StatusContext = factory?.CreateTransitionContext<DataStatus>(entity);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取实体状态上下文失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 解绑实体对象
        /// </summary>
        public virtual void UnbindEntity()
        {
            BoundEntity = null;
            StatusContext = null;
        }

        /// <summary>
        /// 应用当前状态到UI
        /// </summary>
        protected virtual void ApplyCurrentStatusToUI()
        {
            if (_uiController == null || StatusContext == null)
                return;

            try
            {
                var controls = GetAllControls(this);
                _uiController.UpdateUIStatus(StatusContext, controls);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"应用状态到UI失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取控件及其所有子控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <returns>控件列表</returns>
        protected virtual IEnumerable<Control> GetAllControls(Control parent) =>
            parent.Controls.Cast<Control>()
                .SelectMany(control => new[] { control }.Concat(GetAllControls(control)));

        /// <summary>
        /// 刷新状态
        /// </summary>
        public virtual void RefreshStatus()
        {
            try
            {
                if (StatusContext != null)
                {
                    ApplyCurrentStatusToUI();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"刷新状态失败: {ex.Message}");
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 状态上下文变更事件
        /// </summary>
        protected virtual void OnStatusContextChanged()
        {
            // 触发StatusChanged事件
            StatusChanged?.Invoke(this, new StateTransitionEventArgs(
                BoundEntity,
                StatusContext?.CurrentStatus?.GetType() ?? typeof(object),
                null, // 旧状态
                StatusContext?.CurrentStatus, // 新状态
                "状态上下文变更"));
        }

        /// <summary>
        /// UI控制器变更事件
        /// </summary>
        protected virtual void OnUIControllerChanged()
        {
            // 子类可以重写此方法来处理UI控制器变更
        }

        #endregion

        #region 状态转换

        /// <summary>
        /// 转换到新的数据状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public virtual async Task<StateTransitionResult> TransitionToDataStatusAsync(DataStatus targetStatus, string reason = "")
        {
            if (StatusContext == null)
                return StateTransitionResult.Failure("状态上下文未初始化");

            try
            {
                var result = await StatusContext.TransitionTo(targetStatus, reason);
                
                if (result.IsValid)
                {
                    ApplyCurrentStatusToUI();
                    return StateTransitionResult.Success();
                }
                
                return StateTransitionResult.Failure($"转换到数据状态失败: {targetStatus}");
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否可以转换到指定的数据状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可转换</returns>
        public virtual async Task<bool> CanTransitionToDataStatus(DataStatus targetStatus) =>
            StatusContext != null && await StatusContext.CanTransitionTo(targetStatus);

        /// <summary>
        /// 获取可用的数据状态转换列表
        /// </summary>
        /// <returns>可转换的状态列表</returns>
        public virtual IEnumerable<DataStatus> GetAvailableDataStatusTransitions() =>
            StatusContext?.GetAvailableTransitions()
                .OfType<DataStatus>() ?? Enumerable.Empty<DataStatus>();

        #endregion
    }
}