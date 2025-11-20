# StatusDetector类移植实施步骤文档

## 1. 实施前准备

### 1.1 环境检查清单

- [ ] 确认V3状态管理服务已注册
- [ ] 确认依赖注入容器配置正确
- [ ] 确认状态规则配置中心可用
- [ ] 确认测试环境搭建完成
- [ ] 确认回退机制准备就绪

### 1.2 代码备份

```bash
# 备份当前BaseBillEditGeneric.cs文件
cp RUINORERP.UI/BaseForm/BaseBillEditGeneric.cs RUINORERP.UI/BaseForm/BaseBillEditGeneric.cs.backup

# 备份相关状态管理文件
cp RUINORERP.UI/StateManagement/ RUINORERP.UI/StateManagement.backup/ -r
```

### 1.3 分支创建

```bash
# 创建移植特性分支
git checkout -b feature/migrate-statusdetector-to-v3

# 推送分支到远程
git push origin feature/migrate-statusdetector-to-v3
```

## 2. 第一阶段：创建BillStatusManager

### 2.1 创建新文件

创建文件：`RUINORERP.UI/BaseForm/BillStatusManager.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.UI.StateManagement;
using RUINORERP.UI.Common;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 单据状态管理器 - V3状态管理体系适配器
    /// 用于替代StatusDetector类，提供更强大的状态管理功能
    /// </summary>
    public class BillStatusManager
    {
        private readonly IUnifiedStateManager _stateManager;
        private readonly IStatusUIController _uiController;
        private readonly ILogger<BillStatusManager> _logger;
        
        public BillStatusManager(
            IUnifiedStateManager stateManager,
            IStatusUIController uiController,
            ILogger<BillStatusManager> logger = null)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _uiController = uiController ?? throw new ArgumentNullException(nameof(uiController));
            _logger = logger;
        }
        
        /// <summary>
        /// 获取单据状态信息
        /// </summary>
        public BillStatusInfo GetBillStatus(BaseEntity entity)
        {
            if (entity == null)
            {
                _logger?.LogWarning("获取单据状态时实体为null");
                return new BillStatusInfo();
            }
                
            try
            {
                var statusInfo = new BillStatusInfo();
                
                // 获取数据状态
                var dataStatus = _stateManager.GetDataStatus(entity);
                statusInfo.DataStatus = dataStatus;
                
                // 获取业务状态
                var businessStatus = GetBusinessStatus(entity);
                statusInfo.BusinessStatus = businessStatus;
                statusInfo.BusinessStatusType = businessStatus?.GetType();
                
                // 计算权限 - 优先使用V3系统
                CalculatePermissions(entity, statusInfo);
                
                // 计算状态特定的权限
                CalculateStatusSpecificPermissions(entity, statusInfo, businessStatus);
                
                _logger?.LogDebug($"获取单据状态成功: 数据状态={dataStatus}, 业务状态={businessStatus}");
                return statusInfo;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取单据状态失败: 实体类型 {entity.GetType().Name}");
                return new BillStatusInfo();
            }
        }
        
        /// <summary>
        /// 更新UI控件状态
        /// </summary>
        public void UpdateUIStatus(BaseEntity entity, IEnumerable<Control> controls)
        {
            if (entity?.StatusContext == null || controls == null)
            {
                _logger?.LogWarning("更新UI状态时参数无效");
                return;
            }
                
            try
            {
                _logger?.LogDebug("开始更新UI控件状态");
                _uiController.UpdateUIStatus(entity.StatusContext, controls);
                _logger?.LogDebug("UI控件状态更新完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新UI状态失败");
                // 不抛出异常，避免影响主流程
            }
        }
        
        /// <summary>
        /// 验证状态转换
        /// </summary>
        public async Task<StateValidationResult> ValidateTransitionAsync(
            BaseEntity entity, 
            Enum targetStatus)
        {
            if (entity == null)
                return StateValidationResult.Failure("实体不能为空");
                
            if (targetStatus == null)
                return StateValidationResult.Failure("目标状态不能为空");
                
            try
            {
                _logger?.LogDebug($"验证状态转换: 目标状态={targetStatus}");
                
                var result = await _stateManager.ValidateBusinessStatusTransitionAsync(
                    entity, 
                    targetStatus.GetType(), 
                    targetStatus);
                    
                _logger?.LogDebug($"状态转换验证结果: {result.IsSuccessful}, {result.Message}");
                
                return new StateValidationResult
                {
                    IsValid = result.IsSuccessful,
                    ErrorMessage = result.Message
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证状态转换失败");
                return StateValidationResult.Failure($"验证失败: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 获取可用的状态转换列表
        /// </summary>
        public List<Enum> GetAvailableTransitions(BaseEntity entity, Type statusType)
        {
            if (entity == null || statusType == null)
                return new List<Enum>();
                
            try
            {
                var transitions = _stateManager.GetAvailableBusinessStatusTransitions(entity, statusType);
                return transitions.Cast<Enum>().ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可用状态转换列表失败");
                return new List<Enum>();
            }
        }
        
        private void CalculatePermissions(BaseEntity entity, BillStatusInfo statusInfo)
        {
            if (entity?.StatusContext == null)
            {
                _logger?.LogWarning("计算权限时StatusContext为null");
                return;
            }
                
            try
            {
                // 使用UI控制器计算权限
                statusInfo.IsEditable = _uiController.CanExecuteAction(
                    MenuItemEnums.修改, 
                    entity.StatusContext);
                    
                statusInfo.CanCancel = _uiController.CanExecuteAction(
                    MenuItemEnums.取消, 
                    entity.StatusContext);
                    
                statusInfo.CanSubmit = _uiController.CanExecuteAction(
                    MenuItemEnums.提交, 
                    entity.StatusContext);
                    
                statusInfo.CanReview = _uiController.CanExecuteAction(
                    MenuItemEnums.审核, 
                    entity.StatusContext);
                    
                statusInfo.CanReverseReview = _uiController.CanExecuteAction(
                    MenuItemEnums.反审, 
                    entity.StatusContext);
                    
                statusInfo.IsFinalStatus = !_uiController.CanExecuteAction(
                    MenuItemEnums.修改, 
                    entity.StatusContext);
                    
                _logger?.LogDebug($"权限计算完成: 可编辑={statusInfo.IsEditable}, 可提交={statusInfo.CanSubmit}, 可审核={statusInfo.CanReview}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "计算权限失败");
            }
        }
        
        private void CalculateStatusSpecificPermissions(
            BaseEntity entity, 
            BillStatusInfo statusInfo, 
            Enum businessStatus)
        {
            if (businessStatus == null)
                return;
                
            try
            {
                // 根据具体状态类型计算特定权限
                switch (businessStatus)
                {
                    case PrePaymentStatus pre:
                        statusInfo.CanSubmit = pre == PrePaymentStatus.草稿;
                        statusInfo.CanReview = pre == PrePaymentStatus.待审核;
                        statusInfo.CanReverseReview = pre == PrePaymentStatus.待核销 || pre == PrePaymentStatus.已生效;
                        statusInfo.CanClose = pre == PrePaymentStatus.待核销;
                        break;

                    case ARAPStatus arap:
                        statusInfo.CanSubmit = arap == ARAPStatus.草稿;
                        statusInfo.CanReview = arap == ARAPStatus.待审核;
                        statusInfo.CanReverseReview = arap == ARAPStatus.待支付;
                        break;

                    case PaymentStatus pay:
                        statusInfo.CanSubmit = pay == PaymentStatus.草稿;
                        statusInfo.CanReview = pay == PaymentStatus.待审核;
                        statusInfo.CanReverseReview = false;
                        statusInfo.CanClose = false;
                        break;
                        
                    case StatementStatus statementStatus:
                        statusInfo.CanSubmit = statementStatus == StatementStatus.草稿;
                        statusInfo.CanReview = statementStatus == StatementStatus.已发送;
                        statusInfo.CanReverseReview = statementStatus == StatementStatus.已确认;
                        statusInfo.CanClose = false;
                        break;
                        
                    case DataStatus dataStatus:
                        statusInfo.CanSubmit = dataStatus == DataStatus.草稿;
                        statusInfo.CanReview = dataStatus == DataStatus.新建;
                        statusInfo.CanReverseReview = dataStatus == DataStatus.确认;
                        statusInfo.CanClose = false;
                        break;
                }
                
                _logger?.LogDebug($"状态特定权限计算完成: 状态类型={businessStatus.GetType().Name}, 状态值={businessStatus}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "计算状态特定权限失败");
            }
        }
        
        private Enum GetBusinessStatus(BaseEntity entity)
        {
            try
            {
                // 检测业务状态类型
                if (entity.ContainsProperty(typeof(PrePaymentStatus).Name))
                    return _stateManager.GetBusinessStatus<PrePaymentStatus>(entity);
                    
                if (entity.ContainsProperty(typeof(ARAPStatus).Name))
                    return _stateManager.GetBusinessStatus<ARAPStatus>(entity);
                    
                if (entity.ContainsProperty(typeof(PaymentStatus).Name))
                    return _stateManager.GetBusinessStatus<PaymentStatus>(entity);
                    
                if (entity.ContainsProperty(typeof(StatementStatus).Name))
                    return _stateManager.GetBusinessStatus<StatementStatus>(entity);
                    
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取业务状态失败");
                return null;
            }
        }
    }
    
    /// <summary>
    /// 单据状态信息
    /// </summary>
    public class BillStatusInfo
    {
        public DataStatus? DataStatus { get; set; }
        public Enum BusinessStatus { get; set; }
        public Type BusinessStatusType { get; set; }
        
        // 权限属性
        public bool IsEditable { get; set; }
        public bool CanCancel { get; set; }
        public bool CanSubmit { get; set; }
        public bool CanReview { get; set; }
        public bool CanReverseReview { get; set; }
        public bool CanClose { get; set; }
        public bool IsFinalStatus { get; set; }
        
        public BillStatusInfo()
        {
            // 默认权限设置
            IsEditable = true;
            CanCancel = true;
            CanSubmit = true;
            CanReview = false;
            CanReverseReview = false;
            CanClose = false;
            IsFinalStatus = false;
        }
    }
    
    /// <summary>
    /// 状态验证结果
    /// </summary>
    public class StateValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        
        public static StateValidationResult Success()
        {
            return new StateValidationResult
            {
                IsValid = true,
                ErrorMessage = null
            };
        }
        
        public static StateValidationResult Failure(string message)
        {
            return new StateValidationResult
            {
                IsValid = false,
                ErrorMessage = message
            };
        }
    }
}
```

### 2.2 添加依赖注入

在`BaseBillEditGeneric.cs`中添加BillStatusManager字段和初始化：

```csharp
public partial class BaseBillEditGeneric<T> : BaseBillEdit<T> where T : BaseEntity
{
    private BillStatusManager _billStatusManager;
    
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        
        try
        {
            // 初始化状态管理器
            var stateManager = MainForm.Instance.AppContext.GetRequiredService<IUnifiedStateManager>();
            var uiController = MainForm.Instance.AppContext.GetRequiredService<IStatusUIController>();
            var logger = MainForm.Instance.AppContext.GetService<ILogger<BillStatusManager>>();
            
            _billStatusManager = new BillStatusManager(stateManager, uiController, logger);
            
            _logger?.LogInformation("BillStatusManager初始化成功");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "BillStatusManager初始化失败，将使用回退逻辑");
            _billStatusManager = null;
        }
    }
}
```

## 3. 第二阶段：创建V3状态更新方法

### 3.1 添加V3状态更新方法

在`BaseBillEditGeneric.cs`中添加：

```csharp
/// <summary>
/// 使用V3状态管理系统更新工具栏按钮状态
/// </summary>
private async Task UpdateToolbarStatusV3(BaseEntity entity)
{
    if (entity == null)
    {
        _logger?.LogWarning("V3状态更新时实体为null");
        return;
    }
        
    try
    {
        _logger?.LogDebug("开始V3状态更新");
        
        // 获取状态信息
        var statusInfo = _billStatusManager.GetBillStatus(entity);
        
        _logger?.LogDebug($"获取状态信息: 数据状态={statusInfo.DataStatus}, 业务状态={statusInfo.BusinessStatus}");
        
        // 更新按钮状态
        SetToolbarButtonState(btnSave, statusInfo.IsEditable);
        SetToolbarButtonState(btnSubmit, statusInfo.CanSubmit);
        SetToolbarButtonState(btnReview, statusInfo.CanReview);
        SetToolbarButtonState(btnReverseReview, statusInfo.CanReverseReview);
        SetToolbarButtonState(btnCancel, statusInfo.CanCancel);
        
        // 更新UI控件状态
        var controls = GetToolbarControls();
        _billStatusManager.UpdateUIStatus(entity, controls);
        
        _logger?.LogDebug("V3状态更新完成");
        
        // 触发状态变更事件
        OnStatusChanged(statusInfo);
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "V3状态更新失败，将回退到原始逻辑");
        throw; // 重新抛出异常，触发回退逻辑
    }
}

/// <summary>
/// 获取工具栏控件集合
/// </summary>
private IEnumerable<Control> GetToolbarControls()
{
    var controls = new List<Control>();
    
    // 添加主要按钮
    if (btnSave != null) controls.Add(btnSave);
    if (btnSubmit != null) controls.Add(btnSubmit);
    if (btnReview != null) controls.Add(btnReview);
    if (btnReverseReview != null) controls.Add(btnReverseReview);
    if (btnCancel != null) controls.Add(btnCancel);
    if (btnDelete != null) controls.Add(btnDelete);
    if (btnPrint != null) controls.Add(btnPrint);
    if (btnExport != null) controls.Add(btnExport);
    
    return controls;
}

/// <summary>
/// 设置工具栏按钮状态
/// </summary>
private void SetToolbarButtonState(ToolStripItem button, bool enabled)
{
    if (button != null)
    {
        button.Enabled = enabled;
        button.Visible = true;
        
        _logger?.LogDebug($"设置按钮状态: {button.Name} -> Enabled={enabled}");
    }
}

/// <summary>
/// 状态变更事件处理
/// </summary>
private void OnStatusChanged(BillStatusInfo statusInfo)
{
    try
    {
        // 触发RefreshToolbar事件，保持兼容性
        if (RefreshToolbar != null && statusInfo.BusinessStatus != null)
        {
            RefreshToolbar(ActionStatus.无操作, statusInfo.BusinessStatus);
            _logger?.LogDebug("触发RefreshToolbar事件");
        }
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "状态变更事件处理失败");
    }
}
```

### 3.2 修改现有的ToolBarEnabledControl方法

```csharp
/// <summary>
/// 控制工具栏按钮的启用/禁用状态 - V3版本
/// </summary>
private async void ToolBarEnabledControl(T entity)
{
    if (entity == null)
    {
        _logger?.LogWarning("工具栏状态控制时实体为null");
        return;
    }
        
    try
    {
        _logger?.LogDebug($"开始工具栏状态控制: 实体类型={typeof(T).Name}");
        
        // 优先使用V3状态管理系统
        if (_billStatusManager != null && entity.StatusContext != null)
        {
            _logger?.LogDebug("使用V3状态管理系统");
            await UpdateToolbarStatusV3(entity);
        }
        else
        {
            _logger?.LogDebug("V3系统不可用，使用回退逻辑");
            // 回退到原始逻辑
            await UpdateToolbarStatusLegacy(entity);
        }
        
        // 处理锁定状态（保持不变）
        await HandleLockStatus(entity);
        
        _logger?.LogDebug("工具栏状态控制完成");
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "工具栏状态控制失败");
        // 异常时保持按钮可用状态，避免用户无法操作
        SetToolbarButtonsEnabled(true);
    }
}

/// <summary>
/// 回退到原始StatusDetector逻辑
/// </summary>
private async Task UpdateToolbarStatusLegacy(BaseEntity entity)
{
    try
    {
        _logger?.LogDebug("使用原始StatusDetector逻辑");
        
        // 保留原始的StatusDetector逻辑
        var statusDetector = new StatusDetector(entity, UIController);
        
        // 原有的按钮状态设置逻辑
        btnSave.Enabled = statusDetector.IsEditable;
        btnSubmit.Enabled = statusDetector.CanSubmit;
        btnReview.Enabled = statusDetector.CanReview;
        btnReverseReview.Enabled = statusDetector.CanReverseReview;
        btnCancel.Enabled = statusDetector.CanCancel;
        
        // 触发事件
        if (statusDetector.RefreshToolbar != null)
        {
            statusDetector.RefreshToolbar(ActionStatus.无操作, null);
        }
        
        _logger?.LogDebug("原始逻辑执行完成");
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "原始逻辑执行失败");
        throw;
    }
}

/// <summary>
/// 设置所有工具栏按钮为可用状态
/// </summary>
private void SetToolbarButtonsEnabled(bool enabled)
{
    try
    {
        SetToolbarButtonState(btnSave, enabled);
        SetToolbarButtonState(btnSubmit, enabled);
        SetToolbarButtonState(btnReview, enabled);
        SetToolbarButtonState(btnReverseReview, enabled);
        SetToolbarButtonState(btnCancel, enabled);
        SetToolbarButtonState(btnDelete, enabled);
        SetToolbarButtonState(btnPrint, enabled);
        SetToolbarButtonState(btnExport, enabled);
        
        _logger?.LogWarning($"所有工具栏按钮设置为可用状态: Enabled={enabled}");
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "设置工具栏按钮状态失败");
    }
}
```

## 4. 第三阶段：状态事件集成

### 4.1 添加状态变更监听器

```csharp
/// <summary>
/// 注册状态变更监听器
/// </summary>
private void RegisterStatusChangeListeners(BaseEntity entity)
{
    if (entity == null)
    {
        _logger?.LogWarning("注册状态监听器时实体为null");
        return;
    }
        
    try
    {
        _logger?.LogDebug("注册状态变更监听器");
        
        // 监听属性变更事件
        entity.PropertyChanged -= OnEntityPropertyChanged; // 先移除，避免重复注册
        entity.PropertyChanged += OnEntityPropertyChanged;
        
        // 监听V3状态变更事件
        if (entity.StatusContext != null)
        {
            entity.StatusContext.StatusChanged -= OnStatusContextChanged;
            entity.StatusContext.StatusChanged += OnStatusContextChanged;
            
            _logger?.LogDebug("V3状态上下文监听器注册完成");
        }
        
        _logger?.LogDebug("状态变更监听器注册完成");
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "注册状态监听器失败");
    }
}

/// <summary>
/// 实体属性变更处理
/// </summary>
private async void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs e)
{
    try
    {
        if (e.PropertyName.EndsWith("Status"))
        {
            _logger?.LogDebug($"实体状态属性变更: {e.PropertyName}");
            
            // 状态属性变更时刷新工具栏
            if (EditEntity is T entity)
            {
                await ToolBarEnabledControl(entity);
            }
        }
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "实体属性变更处理失败");
    }
}

/// <summary>
/// 状态上下文变更处理
/// </summary>
private async void OnStatusContextChanged(object sender, StatusChangedEventArgs e)
{
    try
    {
        _logger?.LogDebug($"V3状态上下文变更: {e.OldStatus} -> {e.NewStatus}");
        
        // V3状态变更时刷新工具栏
        if (EditEntity is T entity)
        {
            await ToolBarEnabledControl(entity);
        }
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "状态上下文变更处理失败");
    }
}
```

### 4.2 在适当位置注册监听器

在`LoadDataToUI`方法或其他适当位置添加：

```csharp
/// <summary>
/// 加载数据到UI - 增强版
/// </summary>
protected override void LoadDataToUI(T entity)
{
    base.LoadDataToUI(entity);
    
    if (entity != null)
    {
        // 注册状态变更监听器
        RegisterStatusChangeListeners(entity);
        
        // 更新工具栏状态
        ToolBarEnabledControl(entity);
    }
}
```

## 5. 第四阶段：测试验证

### 5.1 创建单元测试

创建文件：`RUINORERP.UI.Tests/BaseForm/BillStatusManagerTests.cs`

```csharp
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RUINORERP.Model;
using RUINORERP.UI.StateManagement;
using RUINORERP.UI.BaseForm;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Tests.BaseForm
{
    [TestClass]
    public class BillStatusManagerTests
    {
        private Mock<IUnifiedStateManager> _mockStateManager;
        private Mock<IStatusUIController> _mockUIController;
        private Mock<ILogger<BillStatusManager>> _mockLogger;
        private BillStatusManager _billStatusManager;
        
        [TestInitialize]
        public void Setup()
        {
            _mockStateManager = new Mock<IUnifiedStateManager>();
            _mockUIController = new Mock<IStatusUIController>();
            _mockLogger = new Mock<ILogger<BillStatusManager>>();
            
            _billStatusManager = new BillStatusManager(
                _mockStateManager.Object,
                _mockUIController.Object,
                _mockLogger.Object
            );
        }
        
        [TestMethod]
        public void GetBillStatus_WithNullEntity_ReturnsDefaultStatusInfo()
        {
            // Act
            var result = _billStatusManager.GetBillStatus(null);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsEditable);
            Assert.IsFalse(result.CanSubmit);
        }
        
        [TestMethod]
        public void GetBillStatus_WithValidEntity_ReturnsCorrectStatusInfo()
        {
            // Arrange
            var entity = new TestEntity();
            var dataStatus = DataStatus.草稿;
            
            _mockStateManager.Setup(x => x.GetDataStatus(entity))
                .Returns(dataStatus);
                
            _mockUIController.Setup(x => x.CanExecuteAction(MenuItemEnums.修改, It.IsAny<IStatusTransitionContext>()))
                .Returns(true);
                
            _mockUIController.Setup(x => x.CanExecuteAction(MenuItemEnums.提交, It.IsAny<IStatusTransitionContext>()))
                .Returns(true);
                
            // Act
            var result = _billStatusManager.GetBillStatus(entity);
            
            // Assert
            Assert.AreEqual(dataStatus, result.DataStatus);
            Assert.IsTrue(result.IsEditable);
            Assert.IsTrue(result.CanSubmit);
            Assert.IsFalse(result.IsFinalStatus);
        }
        
        [TestMethod]
        public async Task ValidateTransitionAsync_WithValidTransition_ReturnsSuccess()
        {
            // Arrange
            var entity = new TestEntity();
            var targetStatus = DataStatus.新建;
            
            _mockStateManager.Setup(x => x.ValidateBusinessStatusTransitionAsync(
                    entity, typeof(DataStatus), targetStatus))
                .ReturnsAsync(StateTransitionResult.Success());
                
            // Act
            var result = await _billStatusManager.ValidateTransitionAsync(entity, targetStatus);
            
            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorMessage);
        }
        
        [TestMethod]
        public async Task ValidateTransitionAsync_WithInvalidTransition_ReturnsFailure()
        {
            // Arrange
            var entity = new TestEntity();
            var targetStatus = DataStatus.新建;
            var errorMessage = "状态转换不允许";
            
            _mockStateManager.Setup(x => x.ValidateBusinessStatusTransitionAsync(
                    entity, typeof(DataStatus), targetStatus))
                .ReturnsAsync(StateTransitionResult.Failure(errorMessage));
                
            // Act
            var result = await _billStatusManager.ValidateTransitionAsync(entity, targetStatus);
            
            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }
    }
    
    /// <summary>
    /// 测试用实体类
    /// </summary>
    public class TestEntity : BaseEntity
    {
        public TestEntity()
        {
            // 初始化测试数据
        }
    }
}
```

### 5.2 创建集成测试

创建文件：`RUINORERP.UI.Tests/BaseForm/BaseBillEditGenericV3Tests.cs`

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RUINORERP.Model;
using RUINORERP.UI.StateManagement;

namespace RUINORERP.UI.Tests.BaseForm
{
    [TestClass]
    public class BaseBillEditGenericV3Tests
    {
        private Mock<IUnifiedStateManager> _mockStateManager;
        private Mock<IStatusUIController> _mockUIController;
        private TestBillEditForm _form;
        
        [TestInitialize]
        public void Setup()
        {
            _mockStateManager = new Mock<IUnifiedStateManager>();
            _mockUIController = new Mock<IStatusUIController>();
            
            // 这里需要创建测试用的表单实例
            // _form = new TestBillEditForm();
        }
        
        [TestMethod]
        public async Task ToolBarEnabledControl_V3SystemAvailable_UsesV3Logic()
        {
            // 测试V3系统可用时使用V3逻辑
            // 需要模拟完整的UI环境
        }
        
        [TestMethod]
        public async Task ToolBarEnabledControl_V3SystemFails_FallbackToLegacy()
        {
            // 测试V3系统失败时回退到原始逻辑
        }
        
        [TestMethod]
        public void StatusChange_TriggersToolbarRefresh()
        {
            // 测试状态变更时触发工具栏刷新
        }
    }
    
    /// <summary>
    /// 测试用的单据编辑表单
    /// </summary>
    public class TestBillEditForm : BaseBillEditGeneric<TestEntity>
    {
        // 测试实现
    }
}
```

## 6. 第五阶段：部署验证

### 6.1 功能开关配置

在配置文件`appsettings.json`中添加：

```json
{
  "FeatureFlags": {
    "UseV3StateManagement": true,
    "ForceFallbackToStatusDetector": false,
    "EnableStateManagementLogging": true,
    "StateManagementLogLevel": "Debug"
  }
}
```

### 6.2 创建功能开关类

```csharp
/// <summary>
/// 状态管理功能开关
/// </summary>
public static class StateManagementFeatureFlags
{
    private static IConfiguration _configuration;
    
    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// 使用V3状态管理系统
    /// </summary>
    public static bool UseV3StateManagement => 
        _configuration?.GetValue<bool>("FeatureFlags:UseV3StateManagement") ?? true;
        
    /// <summary>
    /// 强制回退到StatusDetector
    /// </summary>
    public static bool ForceFallbackToStatusDetector => 
        _configuration?.GetValue<bool>("FeatureFlags:ForceFallbackToStatusDetector") ?? false;
        
    /// <summary>
    /// 启用状态管理日志
    /// </summary>
    public static bool EnableStateManagementLogging => 
        _configuration?.GetValue<bool>("FeatureFlags:EnableStateManagementLogging") ?? false;
}
```

### 6.3 修改回退逻辑

```csharp
private async void ToolBarEnabledControl(T entity)
{
    if (entity == null)
        return;
        
    try
    {
        // 功能开关检查
        if (StateManagementFeatureFlags.ForceFallbackToStatusDetector)
        {
            _logger?.LogWarning("功能开关强制使用原始逻辑");
            await UpdateToolbarStatusLegacy(entity);
            return;
        }
        
        // 优先使用V3状态管理系统
        if (StateManagementFeatureFlags.UseV3StateManagement && 
            _billStatusManager != null && 
            entity.StatusContext != null)
        {
            await UpdateToolbarStatusV3(entity);
        }
        else
        {
            _logger?.LogDebug("使用回退逻辑");
            await UpdateToolbarStatusLegacy(entity);
        }
        
        // 处理锁定状态
        await HandleLockStatus(entity);
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "工具栏状态控制失败");
        SetToolbarButtonsEnabled(true);
    }
}
```

### 6.4 部署检查清单

部署前检查：

- [ ] 所有单元测试通过
- [ ] 集成测试通过
- [ ] 代码审查完成
- [ ] 性能测试通过
- [ ] 配置项正确设置
- [ ] 回退机制验证
- [ ] 监控指标配置
- [ ] 用户通知发送

部署步骤：

1. **预发布环境验证**（1天）
2. **灰度发布**（10%用户，1天）
3. **逐步扩大**（50%用户，1天）
4. **全量发布**（100%用户）

### 6.5 监控指标

关键指标监控：

```csharp
public class StateManagementMetrics
{
    private readonly ILogger<StateManagementMetrics> _logger;
    
    public StateManagementMetrics(ILogger<StateManagementMetrics> logger)
    {
        _logger = logger;
    }
    
    public void RecordV3Usage()
    {
        _logger.LogInformation("[Metric] V3状态管理系统使用");
    }
    
    public void RecordLegacyFallback()
    {
        _logger.LogWarning("[Metric] 回退到原始StatusDetector");
    }
    
    public void RecordStatusQueryTime(long milliseconds)
    {
        if (milliseconds > 100)
        {
            _logger.LogWarning($"[Metric] 状态查询耗时过长: {milliseconds}ms");
        }
    }
    
    public void RecordError(string operation, Exception ex)
    {
        _logger.LogError(ex, $"[Metric] 状态管理错误: {operation}");
    }
}
```

## 7. 第六阶段：清理和优化

### 7.1 移除StatusDetector类

在确认V3系统稳定运行1个月后，可以移除StatusDetector类：

```bash
# 删除StatusDetector类
# 删除相关的回退逻辑
# 清理不再使用的FMPaymentStatusHelper引用
```

### 7.2 性能优化

```csharp
/// <summary>
/// 缓存优化的状态管理器
/// </summary>
public class CachedBillStatusManager : BillStatusManager
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    
    public CachedBillStatusManager(
        IUnifiedStateManager stateManager,
        IStatusUIController uiController,
        IMemoryCache cache,
        ILogger<CachedBillStatusManager> logger = null) 
        : base(stateManager, uiController, logger)
    {
        _cache = cache;
    }
    
    public override BillStatusInfo GetBillStatus(BaseEntity entity)
    {
        if (entity == null)
            return new BillStatusInfo();
            
        var cacheKey = $"BillStatus_{entity.GetType().Name}_{entity.GetId()}";
        
        if (_cache.TryGetValue<BillStatusInfo>(cacheKey, out var cachedInfo))
        {
            return cachedInfo;
        }
        
        var statusInfo = base.GetBillStatus(entity);
        _cache.Set(cacheKey, statusInfo, CacheDuration);
        
        return statusInfo;
    }
    
    public void InvalidateCache(BaseEntity entity)
    {
        var cacheKey = $"BillStatus_{entity.GetType().Name}_{entity.GetId()}";
        _cache.Remove(cacheKey);
    }
}
```

### 7.3 文档更新

更新相关文档：

- 开发文档
- API文档
- 用户手册
- 故障排查指南

## 8. 总结

通过这六个阶段的实施，可以安全、稳定地将StatusDetector类移植到V3状态管理体系：

1. **第一阶段**：创建BillStatusManager，提供V3系统适配
2. **第二阶段**：实现V3状态更新逻辑，保持兼容性
3. **第三阶段**：集成状态事件监听，实现自动刷新
4. **第四阶段**：全面测试验证，确保功能正确
5. **第五阶段**：灰度部署，监控验证
6. **第六阶段**：清理优化，性能提升

整个移植过程遵循"渐进式、可回退"的原则，确保系统稳定性和业务连续性。