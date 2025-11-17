/**
 * 文件: StateAwareControl.cs
 * 说明: 状态感知控件基类 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
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
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.UI.StateManagement.UI;
using RUINORERP.Model.Base.StatusManager.Core;
using RUINORERP.Global;
using RUINORERP.UI.StateManagement.Core;
using RUINORERP.UI.StateManagement.Factory;

namespace RUINORERP.UI.StateManagement
{
    /// <summary>
    /// 状态感知控件基类 - v3版本
    /// 提供统一的状态管理支持，使用v3状态管理系统
    /// 所有需要状态管理的用户控件都应继承此类
    /// </summary>
    public partial class StateAwareControl : UserControl
    {
        #region 字段

        /// <summary>
        /// v3统一状态管理器
        /// </summary>
        private IUnifiedStateManager _stateManager;

        /// <summary>
        /// v3状态上下文
        /// </summary>
        private IStatusTransitionContext _statusContext;

        /// <summary>
        /// UI状态控制器
        /// </summary>
        private IStatusUIController _uiController;

        /// <summary>
        /// 控件状态缓存
        /// </summary>
        private Dictionary<string, ControlState> _controlStates;

        /// <summary>
        /// 状态变更事件处理程序
        /// </summary>
        private EventHandler<StateTransitionEventArgs> _statusChangedHandler;

        #endregion

        #region 事件

        /// <summary>
        /// 状态上下文变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusContextChanged;

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
                    UnsubscribeFromStatusContext();
                    _stateManager = value;
                    _statusContext = null;
                    _uiController = null;
                    SubscribeToStatusContext();
                    OnStateManagerChanged();
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
                    UnsubscribeFromStatusContext();
                    _statusContext = value;
                    SubscribeToStatusContext();
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
        public DataStatus CurrentDataStatus
        {
            get
            {
                if (StatusContext != null && StatusContext.CurrentStatus != null)
                {
                    // 尝试将当前状态转换为DataStatus枚举
                    if (StatusContext.CurrentStatus is DataStatus dataStatus)
                    {
                        return dataStatus;
                    }
                    // 如果当前状态不是DataStatus类型，尝试从状态上下文获取
                    try
                    {
                        return StatusContext.GetDataStatus();
                    }
                    catch
                    {
                        // 如果获取失败，返回默认值
                        return DataStatus.草稿;
                    }
                }
                return DataStatus.草稿; // 默认状态
            }
            protected set
            {
                if (StatusContext != null)
                {
                    try
                    {
                        // 使用状态上下文的异步方法设置状态
                        _ = StatusContext.SetDataStatusAsync(value, "UI状态变更");
                        OnDataStatusChanged(CurrentDataStatus, value);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"设置数据状态失败: {ex.Message}");
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取实体的所有状态
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EntityStatus EntityStatus
        {
            get
            {
                try
                {
                    if (BoundEntity != null && StatusContext != null)
                    {
                        // 创建实体状态对象并填充各种状态
                        var entityStatus = new EntityStatus
                        {
                            dataStatus = StatusContext.GetDataStatus(),
                            actionStatus = StatusContext.GetActionStatus()
                        };
                        
                        // 从状态上下文获取所有业务状态
                        // 注意：这里需要根据实际业务状态类型进行调整
                        // 可以通过反射或其他方式获取所有已注册的业务状态类型
                        
                        return entityStatus;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"获取实体状态失败: {ex.Message}");
                }
                
                return new EntityStatus();
            }
        }

        /// <summary>
        /// 是否启用状态管理
        /// </summary>
        [Category("状态管理")]
        [Description("是否启用状态管理功能")]
        [DefaultValue(true)]
        public bool EnableStateManagement { get; set; } = true;

        /// <summary>
        /// 是否自动应用状态规则
        /// </summary>
        [Category("状态管理")]
        [Description("是否自动应用状态规则")]
        [DefaultValue(true)]
        public bool AutoApplyStateRules { get; set; } = true;

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
            if (!EnableStateManagement)
                return;

            try
            {
                // 尝试从服务容器获取状态管理器
                if (Startup.ServiceProvider != null)
                {
                    _stateManager = Startup.ServiceProvider.GetService<IUnifiedStateManager>();
                    _uiController = Startup.ServiceProvider.GetService<IStatusUIController>();
                }

                // 如果获取失败，创建默认实例
                if (_stateManager == null)
                {
                    _stateManager = new UnifiedStateManager();
                }

                if (_uiController == null)
                {
                    _uiController = new UnifiedStatusUIControllerV3(_stateManager);
                }

                // 初始化控件状态缓存
                _controlStates = new Dictionary<string, ControlState>();

                // 创建状态变更事件处理程序
                _statusChangedHandler = OnStatusContextChanged;

                // 订阅状态变更事件
                SubscribeToStatusContext();
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，避免影响控件初始化
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

            // 检查实体是否已有状态管理器
            // 注意：这里假设实体类已经扩展了状态管理功能
            // 如果没有，则需要通过状态管理器工厂创建状态上下文
            
            // 尝试从状态管理器获取或创建实体的状态上下文
            if (_stateManager != null)
            {
                // 使用状态管理器获取实体的状态上下文
                // 注意：这里需要根据实际的状态管理器API进行调整
                try
                {
                    // 使用状态管理器工厂创建状态上下文
                    var factory = Startup.ServiceProvider?.GetService<IStateManagerFactoryV3>();
                    if (factory != null)
                    {
                        StatusContext = factory.CreateTransitionContext<DataStatus>(entity);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"获取实体状态上下文失败: {ex.Message}");
                }
            }

            // 应用当前状态到UI
            if (AutoApplyStateRules)
            {
                ApplyCurrentStatusToUI();
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
        /// 订阅状态上下文变更事件
        /// </summary>
        protected virtual void SubscribeToStatusContext()
        {
            try
            {
                if (_statusContext != null && _statusChangedHandler != null)
                {
                    // 订阅状态上下文变更事件
                    _statusContext.StatusChanged += _statusChangedHandler;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"订阅状态上下文事件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 取消订阅状态上下文变更事件
        /// </summary>
        protected virtual void UnsubscribeFromStatusContext()
        {
            try
            {
                if (_statusContext != null && _statusChangedHandler != null)
                {
                    // 取消订阅状态上下文变更事件
                    _statusContext.StatusChanged -= _statusChangedHandler;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"取消订阅状态上下文事件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用当前状态到UI
        /// </summary>
        protected virtual void ApplyCurrentStatusToUI()
        {
            if (_uiController == null || StatusContext == null)
                return;

            // 获取所有子控件
            var controls = GetAllControls(this);

            try
            {
                // 使用v3状态管理系统的UI控制器更新UI状态
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
        protected virtual IEnumerable<Control> GetAllControls(Control parent)
        {
            var controls = new List<Control>();
            foreach (Control control in parent.Controls)
            {
                controls.Add(control);
                controls.AddRange(GetAllControls(control));
            }
            return controls;
        }

        /// <summary>
        /// 保存控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="state">控件状态</param>
        protected virtual void SaveControlState(string controlName, ControlState state)
        {
            if (_controlStates == null)
                _controlStates = new Dictionary<string, ControlState>();

            _controlStates[controlName] = state;
        }

        /// <summary>
        /// 获取保存的控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>控件状态</returns>
        protected virtual ControlState GetSavedControlState(string controlName)
        {
            if (_controlStates != null && _controlStates.ContainsKey(controlName))
                return _controlStates[controlName];

            return null;
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        public virtual void RefreshStatus()
        {
            try
            {
                if (StatusContext != null)
                {
                    // 从状态上下文重新获取当前状态
                    var currentDataStatus = StatusContext.GetDataStatus();
                    var currentActionStatus = StatusContext.GetActionStatus();
                    
                    // 触发状态变更事件
                    OnDataStatusChanged(CurrentDataStatus, currentDataStatus);
                    
                    // 更新UI状态
                    ApplyCurrentStatusToUI();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"刷新状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 将实体数据加载到UI控件
        /// </summary>
        /// <param name="entity">要加载的实体对象</param>
        public virtual void LoadDataToUI(BaseEntity entity)
        {
            if (entity == null)
            {
                System.Diagnostics.Debug.WriteLine("LoadDataToUI: 实体对象为空");
                return;
            }

            try
            {
                // 绑定实体到状态管理
                BindEntity(entity);
                
                // 获取所有子控件
                var controls = GetAllControls(this);
                
                // 遍历所有控件，将实体数据绑定到对应的UI控件
                foreach (var control in controls)
                {
                    // 跳过容器控件
                    if (control is ContainerControl || control is ToolStrip || control is MenuStrip || control is StatusStrip)
                        continue;
                        
                    // 根据控件类型和数据绑定设置值
                    SetControlValue(control, entity);
                }
                
                // 应用当前状态到UI
                if (AutoApplyStateRules)
                {
                    ApplyCurrentStatusToUI();
                }
                
                System.Diagnostics.Debug.WriteLine($"成功将实体 {entity.GetType().Name} 数据加载到UI控件");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载数据到UI失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 设置控件值
        /// </summary>
        /// <param name="control">UI控件</param>
        /// <param name="entity">实体对象</param>
        protected virtual void SetControlValue(Control control, BaseEntity entity)
        {
            if (control == null || entity == null)
                return;
                
            try
            {
                // 检查控件是否有数据绑定
                if (control.DataBindings.Count > 0)
                {
                    // 如果有数据绑定，则由绑定机制处理
                    return;
                }
                
                // 根据控件名称匹配实体属性
                string controlName = control.Name;
                if (string.IsNullOrEmpty(controlName))
                    return;
                    
                // 尝试获取实体属性
                var property = entity.GetType().GetProperty(controlName);
                if (property == null)
                {
                    // 尝试去掉前缀匹配（如txtName匹配Name属性）
                    if (controlName.StartsWith("txt") || controlName.StartsWith("lbl") || 
                        controlName.StartsWith("btn") || controlName.StartsWith("chk") ||
                        controlName.StartsWith("cmb") || controlName.StartsWith("dtp"))
                    {
                        string propertyName = controlName.Substring(3);
                        property = entity.GetType().GetProperty(propertyName);
                    }
                    
                    if (property == null)
                        return;
                }
                
                // 获取属性值
                var value = property.GetValue(entity);
                if (value == null)
                    return;
                    
                // 根据控件类型设置值
                switch (control)
                {
                    case TextBox textBox:
                        textBox.Text = value.ToString();
                        break;
                        
                    case Label label:
                        label.Text = value.ToString();
                        break;
                        
                    case CheckBox checkBox:
                        if (value is bool boolValue)
                            checkBox.Checked = boolValue;
                        else if (bool.TryParse(value.ToString(), out bool parsedBool))
                            checkBox.Checked = parsedBool;
                        break;
                        
                    case ComboBox comboBox:
                        comboBox.SelectedItem = value;
                        break;
                        
                    case DateTimePicker dateTimePicker:
                        if (value is DateTime dateTimeValue)
                            dateTimePicker.Value = dateTimeValue;
                        else if (DateTime.TryParse(value.ToString(), out DateTime parsedDateTime))
                            dateTimePicker.Value = parsedDateTime;
                        break;
                        
                    case NumericUpDown numericUpDown:
                        if (decimal.TryParse(value.ToString(), out decimal decimalValue))
                            numericUpDown.Value = decimalValue;
                        break;
                        
                    case RadioButton radioButton:
                        if (value is bool radioBoolValue)
                            radioButton.Checked = radioBoolValue;
                        else if (bool.TryParse(value.ToString(), out bool parsedRadioBool))
                            radioButton.Checked = parsedRadioBool;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"设置控件 {control.Name} 的值失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从UI控件获取数据到实体
        /// </summary>
        /// <param name="entity">目标实体对象</param>
        public virtual void GetDataFromUI(BaseEntity entity)
        {
            if (entity == null)
            {
                System.Diagnostics.Debug.WriteLine("GetDataFromUI: 实体对象为空");
                return;
            }

            try
            {
                // 获取所有子控件
                var controls = GetAllControls(this);
                
                // 遍历所有控件，将UI控件的值更新到对应的实体属性
                foreach (var control in controls)
                {
                    // 跳过容器控件
                    if (control is ContainerControl || control is ToolStrip || control is MenuStrip || control is StatusStrip)
                        continue;
                        
                    // 根据控件类型和数据绑定获取值
                    GetEntityValue(control, entity);
                }
                
                System.Diagnostics.Debug.WriteLine($"成功从UI控件获取数据到实体 {entity.GetType().Name}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从UI获取数据失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 从控件获取值并更新到实体
        /// </summary>
        /// <param name="control">UI控件</param>
        /// <param name="entity">实体对象</param>
        protected virtual void GetEntityValue(Control control, BaseEntity entity)
        {
            if (control == null || entity == null)
                return;
                
            try
            {
                // 检查控件是否有数据绑定
                if (control.DataBindings.Count > 0)
                {
                    // 如果有数据绑定，则由绑定机制处理
                    return;
                }
                
                // 根据控件名称匹配实体属性
                string controlName = control.Name;
                if (string.IsNullOrEmpty(controlName))
                    return;
                    
                // 尝试获取实体属性
                var property = entity.GetType().GetProperty(controlName);
                if (property == null)
                {
                    // 尝试去掉前缀匹配（如txtName匹配Name属性）
                    if (controlName.StartsWith("txt") || controlName.StartsWith("lbl") || 
                        controlName.StartsWith("btn") || controlName.StartsWith("chk") ||
                        controlName.StartsWith("cmb") || controlName.StartsWith("dtp"))
                    {
                        string propertyName = controlName.Substring(3);
                        property = entity.GetType().GetProperty(propertyName);
                    }
                    
                    if (property == null)
                        return;
                }
                
                // 根据控件类型获取值
                object value = null;
                switch (control)
                {
                    case TextBox textBox:
                        value = textBox.Text;
                        break;
                        
                    case CheckBox checkBox:
                        value = checkBox.Checked;
                        break;
                        
                    case ComboBox comboBox:
                        value = comboBox.SelectedItem;
                        break;
                        
                    case DateTimePicker dateTimePicker:
                        value = dateTimePicker.Value;
                        break;
                        
                    case NumericUpDown numericUpDown:
                        value = numericUpDown.Value;
                        break;
                        
                    case RadioButton radioButton:
                        value = radioButton.Checked;
                        break;
                }
                
                if (value != null)
                {
                    // 尝试转换值类型
                    if (property.PropertyType != value.GetType())
                    {
                        try
                        {
                            value = Convert.ChangeType(value, property.PropertyType);
                        }
                        catch
                        {
                            // 转换失败，跳过
                            return;
                        }
                    }
                    
                    // 设置属性值
                    property.SetValue(entity, value);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从控件 {control.Name} 获取值失败: {ex.Message}");
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 状态管理器变更事件
        /// </summary>
        protected virtual void OnStateManagerChanged()
        {
            // 子类可以重写此方法来处理状态管理器变更
        }

        /// <summary>
        /// 状态上下文变更事件
        /// </summary>
        protected virtual void OnStatusContextChanged()
        {
            // 应用当前状态到UI
            if (AutoApplyStateRules)
            {
                ApplyCurrentStatusToUI();
            }

            // 触发StatusContextChanged事件
            StatusContextChanged?.Invoke(this, new StateTransitionEventArgs(
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
            // 应用当前状态到UI
            if (AutoApplyStateRules)
            {
                ApplyCurrentStatusToUI();
            }
        }

        /// <summary>
        /// 数据状态变更事件处理
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        protected virtual void OnDataStatusChanged(DataStatus oldStatus, DataStatus newStatus)
        {
            // 应用新状态到UI
            if (AutoApplyStateRules)
            {
                ApplyCurrentStatusToUI();
            }

            // 子类可以重写此方法来处理状态变更
        }

        /// <summary>
        /// 状态上下文状态变更事件处理
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        protected virtual void OnStatusContextChanged(object sender, StateTransitionEventArgs e)
        {
            // 应用新状态到UI
            if (AutoApplyStateRules)
            {
                ApplyCurrentStatusToUI();
            }

            // 触发StatusContextChanged事件
            StatusContextChanged?.Invoke(this, e);

            // 子类可以重写此方法来处理状态变更
        }

        #endregion

        #region 控件状态管理

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        /// <param name="readOnly">是否只读</param>
        public virtual void SetControlState(string controlName, bool enabled, bool visible, bool readOnly = false)
        {
            var control = FindControl(controlName);
            if (control != null)
            {
                // 保存原始状态
                if (!_controlStates.ContainsKey(controlName))
                {
                    _controlStates[controlName] = new ControlState
                    {
                        Enabled = control.Enabled,
                        Visible = control.Visible,
                        ReadOnly = GetControlReadOnly(control),
                        BackColor = control.BackColor,
                        ForeColor = control.ForeColor
                    };
                }

                // 应用新状态
                control.Enabled = enabled;
                control.Visible = visible;
                SetControlReadOnly(control, readOnly);
            }
        }

        /// <summary>
        /// 恢复控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        public virtual void RestoreControlState(string controlName)
        {
            if (!_controlStates.ContainsKey(controlName))
                return;

            var state = _controlStates[controlName];
            var control = FindControl(controlName);
            if (control != null)
            {
                control.Enabled = state.Enabled;
                control.Visible = state.Visible;
                SetControlReadOnly(control, state.ReadOnly);
                control.BackColor = state.BackColor;
                control.ForeColor = state.ForeColor;
            }
        }

        /// <summary>
        /// 查找控件
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>控件</returns>
        protected virtual Control FindControl(string controlName)
        {
            if (string.IsNullOrEmpty(controlName))
                return null;

            return GetAllControls(this).FirstOrDefault(c => c.Name == controlName);
        }

        /// <summary>
        /// 获取控件只读状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>是否只读</returns>
        protected virtual bool GetControlReadOnly(Control control)
        {
            if (control is TextBox textBox)
                return textBox.ReadOnly;
            else if (control is ComboBox comboBox)
                return !comboBox.Enabled;
            else if (control is NumericUpDown numericUpDown)
                return numericUpDown.ReadOnly;
            else
                return false;
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="readOnly">是否只读</param>
        protected virtual void SetControlReadOnly(Control control, bool readOnly)
        {
            if (control is TextBox textBox)
                textBox.ReadOnly = readOnly;
            else if (control is ComboBox comboBox)
                comboBox.Enabled = !readOnly;
            else if (control is NumericUpDown numericUpDown)
                numericUpDown.ReadOnly = readOnly;
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
            {
                return StateTransitionResult.Failure("状态上下文未初始化");
            }

            try
            {
                // 使用状态上下文执行转换
                var result = await StatusContext.SetDataStatusAsync(targetStatus, reason);
                
                // 如果转换成功，更新UI状态
                if (result)
                {
                    ApplyCurrentStatusToUI();
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"转换到数据状态失败: {targetStatus}");
                }
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
        public virtual async Task<bool> CanTransitionToDataStatus(DataStatus targetStatus)
        {
            if (StatusContext == null)
                return false;

            try
            {
                // 使用状态上下文验证转换
                return await StatusContext.CanTransitionTo(targetStatus);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取可用的数据状态转换列表
        /// </summary>
        /// <returns>可转换的状态列表</returns>
        public virtual IEnumerable<DataStatus> GetAvailableDataStatusTransitions()
        {
            if (StatusContext == null)
                return Enumerable.Empty<DataStatus>();

            // 使用非泛型方法获取所有可用转换
            var transitions = StatusContext.GetAvailableTransitions();
            
            // 过滤出DataStatus类型的转换
            return transitions.Where(t => t is DataStatus).Cast<DataStatus>();
        }

        /// <summary>
        /// 转换到指定的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public virtual async Task<StateTransitionResult> TransitionToBusinessStatusAsync<T>(T targetStatus, string reason = "") where T : Enum
        {
            if (StatusContext == null)
            {
                return StateTransitionResult.Failure("状态上下文未初始化");
            }

            try
            {
                // 使用状态上下文执行转换
                var result = await StatusContext.SetBusinessStatusAsync(targetStatus, reason);
                
                // 如果转换成功，更新UI状态
                if (result)
                {
                    ApplyCurrentStatusToUI();
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"转换到业务状态失败: {targetStatus}");
                }
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 转换到指定的操作状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public virtual async Task<StateTransitionResult> TransitionToActionStatusAsync(ActionStatus targetStatus, string reason = "")
        {
            if (StatusContext == null)
            {
                return StateTransitionResult.Failure("状态上下文未初始化");
            }

            try
            {
                // 使用状态上下文执行转换
                var result = await StatusContext.SetActionStatusAsync(targetStatus, reason);
                
                // 如果转换成功，更新UI状态
                if (result)
                {
                    ApplyCurrentStatusToUI();
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"转换到操作状态失败: {targetStatus}");
                }
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
            }
        }

        #endregion

        #region 资源释放

        /// <summary>
        /// 释放状态管理资源
        /// </summary>
        /// <param name="disposing">是否正在释放</param>
        protected virtual void DisposeStateManagement(bool disposing)
        {
            if (disposing)
            {
                // 取消订阅事件
                UnsubscribeFromStatusContext();

                // 释放状态上下文
                if (_statusContext is IDisposable disposableContext)
                {
                    disposableContext.Dispose();
                }

                // 清理控件状态缓存
                _controlStates?.Clear();
            }
        }

       

        #endregion
    }

    /// <summary>
    /// 控件状态
    /// </summary>
    public class ControlState
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForeColor { get; set; }
    }
}