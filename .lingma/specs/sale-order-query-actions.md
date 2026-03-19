# 销售单据查询列表提交审核功能实现方案

## 上下文

### 问题背景
当前销售单据编辑界面（`UCSaleOrder.cs`）已实现完整的提交、审核功能，但查询列表界面（`UCSaleOrderQuery.cs`）缺少这些操作能力。用户需要在查询列表中直接对选中的单据执行提交、审核操作，而无需打开编辑界面。

### 现状分析
- **编辑界面**：基于 `BaseBillEditGeneric<T,C>` 基类，通过 `DoButtonClick(MenuItemEnums)` 统一处理所有操作
- **查询列表**：基于 `BaseBillQueryMC<M,C>` 基类，已有部分审核功能（`Review`、`ReReview`），但缺少提交功能
- **架构特点**：系统采用模板方法模式，基类定义流程框架，虚方法供子类重写

### 核心需求
1. 在查询列表中实现提交、审核功能按钮
2. 复用编辑界面的业务逻辑和权限控制
3. 保持状态同步和错误处理机制一致
4. 考虑架构优化：将通用业务方法抽象为可跨基类复用的组件

---

## 实现方案

### 方案概述

**核心策略**：抽取通用业务逻辑到独立的服务接口，让 `BaseBillEditGeneric` 和 `BaseBillQueryMC` 都能复用同一套业务逻辑。

**架构设计**：
1. 创建 `IBillOperationService` 接口定义标准操作方法
2. 实现 `BillOperationService` 类封装通用业务逻辑
3. 两个基类通过依赖注入使用相同的服务
4. 特殊业务逻辑在子类中重写

### 关键文件

#### 需要修改/创建的文件
1. **新建** `RUINORERP.UI\BusinessService\IBillOperationService.cs` - 业务操作服务接口
2. **新建** `RUINORERP.UI\BusinessService\BillOperationService.cs` - 业务操作服务实现
3. `RUINORERP.UI\BaseForm\BaseBillQueryMC.cs` - 集成服务调用
4. `RUINORERP.UI\BaseForm\BaseBillEditGeneric.cs` - 重构为使用服务
5. `RUINORERP.UI\PSI\SAL\UCSaleOrderQuery.cs` - 重写 Submit/Review 等方法调用服务
6. `RUINORERP.UI\PSI\SAL\UCSaleOrder.cs` - 保持不变或微调

#### 辅助文件
7. `RUINORERP.Business\tb_SaleOrderController.cs` - 确认 Controller 层方法（只读）
8. `RUINORERP.CommonUI\frmApproval.cs` - 审核对话框（已存在，直接复用）

---

## 详细实现步骤

### 步骤 1：创建通用业务服务接口和实现

#### 1.1 定义 IBillOperationService 接口
新建文件 `RUINORERP.UI\BusinessService\IBillOperationService.cs`：

#### 1.1 定义 IBillOperationService 接口
新建文件 `RUINORERP.UI\BusinessService\IBillOperationService.cs`：

```csharp
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.UI.BusinessService
{
    /// <summary>
    /// 单据业务操作结果
    /// </summary>
    public class BillOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public object OriginalStatus { get; set; } // 用于失败时恢复
    }

    /// <summary>
    /// 单据业务操作服务接口
    /// 统一处理提交、审核、反审核、结案等操作
    /// </summary>
    public interface IBillOperationService
    {
        /// <summary>
        /// 提交单据
        /// </summary>
        Task<BillOperationResult> SubmitAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 批量提交单据
        /// </summary>
        Task<BillOperationResult> SubmitBatchAsync<T>(List<T> entities) where T : BaseEntity;

        /// <summary>
        /// 审核单据
        /// </summary>
        Task<BillOperationResult> ReviewAsync<T>(T entity, ApprovalEntity approvalInfo) where T : BaseEntity;

        /// <summary>
        /// 批量审核单据
        /// </summary>
        Task<BillOperationResult> ReviewBatchAsync<T>(List<T> entities, ApprovalEntity approvalInfo) where T : BaseEntity;

        /// <summary>
        /// 反审核单据
        /// </summary>
        Task<BillOperationResult> ReReviewAsync<T>(T entity, ApprovalEntity approvalInfo) where T : BaseEntity;

        /// <summary>
        /// 结案单据
        /// </summary>
        Task<BillOperationResult> CloseCaseAsync<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 批量结案单据
        /// </summary>
        Task<BillOperationResult> CloseCaseBatchAsync<T>(List<T> entities) where T : BaseEntity;

        /// <summary>
        /// 反结案单据
        /// </summary>
        Task<BillOperationResult> AntiCloseCaseAsync<T>(T entity) where T : BaseEntity;
    }
}
```

#### 1.2 实现 BillOperationService 类
新建文件 `RUINORERP.UI\BusinessService\BillOperationService.cs`：

```csharp
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business;
using RUINORERP.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BusinessService
{
    /// <summary>
    /// 单据业务操作服务实现
    /// 封装通用的业务逻辑，供编辑界面和查询列表共用
    /// </summary>
    public class BillOperationService : IBillOperationService
    {
        private readonly ILogger<BillOperationService> _logger;
        private readonly RepeatOperationGuardService _guardService;

        public BillOperationService(ILogger<BillOperationService> logger)
        {
            _logger = logger;
            _guardService = Startup.GetFromFac<RepeatOperationGuardService>();
        }

        /// <summary>
        /// 提交单据
        /// </summary>
        public async Task<BillOperationResult> SubmitAsync<T>(T entity) where T : BaseEntity
        {
            var result = new BillOperationResult();

            try
            {
                // 1. 防重复操作检查
                long entityId = entity?.PrimaryKeyID ?? 0;
                if (_guardService.ShouldBlockOperation(MenuItemEnums.提交，typeof(T).Name, entityId, showStatusMessage: true))
                {
                    result.Success = false;
                    result.Message = "操作正在进行中，请勿重复提交";
                    return result;
                }

                // 2. 获取状态管理器并验证权限
                var stateManager = GetStateManager();
                if (stateManager != null)
                {
                    var (canExecute, message) = stateManager.CanExecuteActionWithMessage(entity, MenuItemEnums.提交);
                    if (!canExecute)
                    {
                        result.Success = false;
                        result.Message = $"无法提交：{message}";
                        MainForm.Instance.uclog.AddLog(result.Message, UILogType.警告);
                        return result;
                    }
                }

                // 3. 获取 Controller 并调用提交方法
                var controller = GetController<T>();
                var submitResult = await controller.SubmitAsync(entity);

                if (submitResult.Succeeded)
                {
                    // 4. 更新状态
                    if (stateManager != null)
                    {
                        await stateManager.SetStatusByActionAsync(entity, MenuItemEnums.提交，"提交成功");
                    }

                    // 5. 同步任务状态
                    await SyncTodoStatusAsync(entity, "提交");

                    // 6. 记录操作
                    _guardService.RecordOperation(MenuItemEnums.提交，typeof(T).Name, entityId);

                    result.Success = true;
                    result.Message = $"{entity.BillNo}提交成功";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.信息);
                }
                else
                {
                    result.Success = false;
                    result.Message = $"提交失败：{submitResult.ErrorMsg}";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.错误);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交异常");
                result.Success = false;
                result.Message = $"提交异常：{ex.Message}";
                result.Exception = ex;
            }

            return result;
        }

        /// <summary>
        /// 批量提交单据
        /// </summary>
        public async Task<BillOperationResult> SubmitBatchAsync<T>(List<T> entities) where T : BaseEntity
        {
            var result = new BillOperationResult();
            int successCount = 0;
            int failCount = 0;
            var failureMessages = new System.Text.StringBuilder();

            foreach (var entity in entities)
            {
                var singleResult = await SubmitAsync(entity);
                if (singleResult.Success)
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                    failureMessages.AppendLine($"{entity.BillNo}: {singleResult.Message}");
                }
            }

            result.Success = successCount > 0;
            result.Message = $"批量提交完成：成功{successCount}条，失败{failCount}条";
            if (failCount > 0)
            {
                result.Message += $"\n{failureMessages}";
            }

            return result;
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        public async Task<BillOperationResult> ReviewAsync<T>(T entity, ApprovalEntity approvalInfo) where T : BaseEntity
        {
            var result = new BillOperationResult();

            try
            {
                // 1. 防重复操作检查
                long entityId = entity?.PrimaryKeyID ?? 0;
                if (_guardService.ShouldBlockOperation(MenuItemEnums.审核，typeof(T).Name, entityId, showStatusMessage: true))
                {
                    result.Success = false;
                    result.Message = "操作正在进行中，请勿重复提交";
                    return result;
                }

                // 2. 保存原始状态（用于失败时恢复）
                object originalStatus = null;
                var stateManager = GetStateManager();
                if (stateManager != null)
                {
                    originalStatus = stateManager.GetBusinessStatus(entity);
                    
                    // 二次验证
                    var (canExecute, message) = stateManager.CanExecuteActionWithMessage(entity, MenuItemEnums.审核);
                    if (!canExecute)
                    {
                        result.Success = false;
                        result.Message = $"审核失败：{message}";
                        MessageBox.Show(result.Message, "提示");
                        return result;
                    }
                }

                result.OriginalStatus = originalStatus;

                // 3. 显示审核对话框（如果需要）
                if (approvalInfo == null)
                {
                    approvalInfo = CreateDefaultApprovalInfo(entity);
                    CommonUI.frmApproval frm = new CommonUI.frmApproval();
                    frm.BindData(approvalInfo);
                    
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        result.Success = false;
                        result.Message = "用户取消审核";
                        return result;
                    }
                }

                // 4. 创建撤销命令
                RevertCommand command = new RevertCommand();
                T oldobj = CloneHelper.DeepCloneObject_maxnew<T>(entity);
                command.UndoOperation = delegate ()
                {
                    CloneHelper.SetValues<T>(entity, oldobj);
                    // 恢复原始状态
                    if (stateManager != null && originalStatus != null)
                    {
                        var statusType = stateManager.GetStatusType(entity);
                        ReflectionHelper.SetPropertyValue(entity, statusType.Name, originalStatus);
                    }
                };

                // 5. 调用 Controller 审核方法
                var controller = GetController<T>();
                
                if (approvalInfo.ApprovalResults == true)
                {
                    // 审核通过
                    ReturnResults<T> rmr = await controller.ApprovalAsync(entity);

                    if (rmr.Succeeded)
                    {
                        // 更新状态
                        if (stateManager != null)
                        {
                            await stateManager.SetStatusByActionAsync(entity, MenuItemEnums.审核，"审核通过");
                        }

                        // 同步任务状态
                        await SyncTodoStatusAsync(entity, "审核");

                        // 记录操作
                        _guardService.RecordOperation(MenuItemEnums.审核，typeof(T).Name, entityId);

                        result.Success = true;
                        result.Message = $"{entity.BillNo}审核成功";
                        MainForm.Instance.uclog.AddLog(result.Message, UILogType.信息);
                    }
                    else
                    {
                        // 审核失败，恢复状态
                        command.Undo();
                        result.Success = false;
                        result.Message = $"{entity.BillNo}审核失败：{rmr.ErrorMsg}";
                        MessageBox.Show(result.Message);
                    }
                }
                else
                {
                    // 审核驳回
                    if (stateManager != null)
                    {
                        await stateManager.HandleApprovalRejectAsync(entity, approvalInfo.ApprovalOpinions);
                    }
                    
                    // 更新审核意见和状态
                    entity.SetPropertyValue("ApprovalOpinions", approvalInfo.ApprovalOpinions);
                    entity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.审核驳回);
                    entity.SetPropertyValue("ApprovalResults", false);
                    BusinessHelper.Instance.ApproverEntity(entity);
                    
                    await controller.BaseSaveOrUpdate(entity);

                    result.Success = true;
                    result.Message = $"{entity.BillNo}已驳回";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.信息);
                }

                // 6. 创建审计日志
                await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", entity,
                    $"意见{approvalInfo.ApprovalOpinions}" + $"结果:{(approvalInfo.ApprovalResults ? "通过" : "拒绝")}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "审核异常");
                result.Success = false;
                result.Message = $"审核异常：{ex.Message}";
                result.Exception = ex;
            }

            return result;
        }

        /// <summary>
        /// 批量审核单据
        /// </summary>
        public async Task<BillOperationResult> ReviewBatchAsync<T>(List<T> entities, ApprovalEntity approvalInfo) where T : BaseEntity
        {
            var result = new BillOperationResult();
            int successCount = 0;
            int failCount = 0;
            var failureMessages = new System.Text.StringBuilder();

            foreach (var entity in entities)
            {
                var singleResult = await ReviewAsync(entity, approvalInfo);
                if (singleResult.Success)
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                    failureMessages.AppendLine($"{entity.BillNo}: {singleResult.Message}");
                }
            }

            result.Success = successCount > 0;
            result.Message = $"批量审核完成：成功{successCount}条，失败{failCount}条";
            if (failCount > 0)
            {
                result.Message += $"\n{failureMessages}";
            }

            return result;
        }

        /// <summary>
        /// 反审核单据
        /// </summary>
        public async Task<BillOperationResult> ReReviewAsync<T>(T entity, ApprovalEntity approvalInfo) where T : BaseEntity
        {
            var result = new BillOperationResult();

            try
            {
                // 1. 保存原始状态
                object originalStatus = null;
                var stateManager = GetStateManager();
                if (stateManager != null)
                {
                    originalStatus = stateManager.GetBusinessStatus(entity);
                    
                    // 验证权限
                    var (canExecute, message) = stateManager.CanExecuteActionWithMessage(entity, MenuItemEnums.反审);
                    if (!canExecute)
                    {
                        result.Success = false;
                        result.Message = $"反审核失败：{message}";
                        MessageBox.Show(result.Message, "提示");
                        return result;
                    }
                }

                result.OriginalStatus = originalStatus;

                // 2. 显示反审核对话框
                if (approvalInfo == null)
                {
                    approvalInfo = CreateDefaultApprovalInfo(entity);
                    CommonUI.frmReApproval frm = new CommonUI.frmReApproval();
                    frm.BindData(approvalInfo);
                    
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        result.Success = false;
                        result.Message = "用户取消反审核";
                        return result;
                    }
                }

                // 3. 创建撤销命令
                RevertCommand command = new RevertCommand();
                T oldobj = CloneHelper.DeepCloneObject_maxnew<T>(entity);
                command.UndoOperation = delegate ()
                {
                    CloneHelper.SetValues<T>(entity, oldobj);
                    if (stateManager != null && originalStatus != null)
                    {
                        var statusType = stateManager.GetStatusType(entity);
                        ReflectionHelper.SetPropertyValue(entity, statusType.Name, originalStatus);
                    }
                };

                // 4. 调用 Controller 反审核方法
                var controller = GetController<T>();
                ReturnResults<T> rmr = await controller.AntiApprovalAsync(entity);

                if (rmr.Succeeded)
                {
                    // 反审核成功
                    if (stateManager != null)
                    {
                        await stateManager.SetStatusByActionAsync(entity, MenuItemEnums.反审，"反审核通过");
                    }

                    result.Success = true;
                    result.Message = $"{entity.BillNo}反审核成功";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.信息);
                }
                else
                {
                    // 反审核失败，恢复状态
                    command.Undo();
                    result.Success = false;
                    result.Message = $"{entity.BillNo}反审核失败：{rmr.ErrorMsg}";
                    MessageBox.Show(result.Message);
                }

                // 5. 创建审计日志
                await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("反审", entity,
                    $"反审原因{approvalInfo.ApprovalOpinions}" + $"结果:{(approvalInfo.ApprovalResults ? "通过" : "拒绝")},{rmr.ErrorMsg}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "反审核异常");
                result.Success = false;
                result.Message = $"反审核异常：{ex.Message}";
                result.Exception = ex;
            }

            return result;
        }

        /// <summary>
        /// 结案单据
        /// </summary>
        public async Task<BillOperationResult> CloseCaseAsync<T>(T entity) where T : BaseEntity
        {
            // TODO: 实现结案逻辑
            return await Task.FromResult(new BillOperationResult { Success = false, Message = "暂未实现" });
        }

        /// <summary>
        /// 批量结案单据
        /// </summary>
        public async Task<BillOperationResult> CloseCaseBatchAsync<T>(List<T> entities) where T : BaseEntity
        {
            // TODO: 实现批量结案逻辑
            return await Task.FromResult(new BillOperationResult { Success = false, Message = "暂未实现" });
        }

        /// <summary>
        /// 反结案单据
        /// </summary>
        public async Task<BillOperationResult> AntiCloseCaseAsync<T>(T entity) where T : BaseEntity
        {
            // TODO: 实现反结案逻辑
            return await Task.FromResult(new BillOperationResult { Success = false, Message = "暂未实现" });
        }

        #region Helper Methods

        /// <summary>
        /// 获取状态管理器
        /// </summary>
        private IUnifiedStateManager GetStateManager()
        {
            return Startup.GetFromFac<IUnifiedStateManager>();
        }

        /// <summary>
        /// 获取 Controller
        /// </summary>
        private BaseController<T> GetController<T>() where T : BaseEntity
        {
            return Startup.GetFromFacByName<BaseController<T>>(typeof(T).Name + "Controller");
        }

        /// <summary>
        /// 创建默认审核信息
        /// </summary>
        private ApprovalEntity CreateDefaultApprovalInfo<T>(T entity) where T : BaseEntity
        {
            var approvalInfo = new ApprovalEntity();
            
            // 设置基本信息
            if (entity is BaseEntity baseEntity)
            {
                approvalInfo.bizName = baseEntity.BillNo;
                approvalInfo.BillNo = baseEntity.BillNo;
                approvalInfo.BillTypeID = baseEntity.GetPropertyValue("BillType_ID")?.ToString();
                approvalInfo.approvalLevelID = 1;
                approvalInfo.ApprovalResults = true;
                approvalInfo.ApprovalOpinions = "同意";
            }

            return approvalInfo;
        }

        /// <summary>
        /// 同步任务状态
        /// </summary>
        private async Task SyncTodoStatusAsync<T>(T entity, string action) where T : BaseEntity
        {
            try
            {
                var todoService = Startup.GetFromFac<ITodoUpdateService>();
                await todoService.SyncTodoStatusAsync(entity, action);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"同步任务状态失败：{action}");
                // 不阻断主流程
            }
        }

        #endregion
    }
}
```

#### 1.2 增强审核方法
在 `BaseBillQueryMC.cs` 中已有的 `Review` 方法基础上，增加与编辑界面一致的处理逻辑：

```csharp
/// <summary>
/// 审核单个单据（增强版）
/// </summary>
protected async override Task<ReviewResult> ReviewSingle(M entity)
{
    ReviewResult reviewResult = new ReviewResult();
    
    // 1. 防重复检查
    if (_guardService == null)
        _guardService = Startup.GetFromFac<RepeatOperationGuardService>();
    
    long entityId = entity?.PrimaryKeyID ?? 0;
    if (_guardService.ShouldBlockOperation(MenuItemEnums.审核，this.GetType().Name, entityId, showStatusMessage: true))
        return reviewResult;
    
    // 2. 状态验证
    var (canExecute, message) = StateManager.CanExecuteActionWithMessage(entity, MenuItemEnums.审核);
    if (!canExecute)
    {
        MessageBox.Show($"审核失败：{message}");
        return reviewResult;
    }
    
    // 3. 显示审核对话框
    CommonUI.frmApproval frm = new CommonUI.frmApproval();
    frm.BindData(entity);
    
    if (frm.ShowDialog() == DialogResult.OK)
    {
        // 4. 创建撤销命令
        RevertCommand command = new RevertCommand();
        M oldobj = CloneHelper.DeepCloneObject_maxnew<M>(entity);
        command.UndoOperation = delegate () {
            CloneHelper.SetValues<M>(entity, oldobj);
        };
        
        try
        {
            // 5. 调用 Controller 审核方法
            var controller = GetController();
            ReturnResults<M> rmr = await controller.ApprovalAsync(entity);
            
            if (rmr.Succeeded)
            {
                // 6. 审核成功：更新状态
                await StateManager.SetBusinessStatusAsync<DataStatus>(entity, DataStatus.确认);
                
                // 7. 同步任务状态
                await SyncTodoStatusAsync(entity, "审核");
                
                // 8. 特殊业务处理（如销售出库单锁定订单）
                if (entity is tb_SaleOut saleOut)
                {
                    await LockBill(saleOut.SOrder_ID, ...);
                }
                
                // 9. 刷新列表
                RefreshData();
                
                reviewResult.Success = true;
                MainForm.Instance.uclog.AddLog($"{entity.BillNo}审核成功", UILogType.信息);
            }
            else
            {
                // 10. 审核失败：恢复状态
                command.Undo();
                MessageBox.Show($"{entity.BillNo}审核失败：{rmr.ErrorMsg}");
            }
        }
        catch (Exception ex)
        {
            command.Undo();
            logger.LogError(ex, "审核异常");
            MessageBox.Show($"审核异常：{ex.Message}");
        }
        finally
        {
            // 11. 记录操作
            _guardService.RecordOperation(MenuItemEnums.审核，this.GetType().Name, entityId);
        }
    }
    
    return reviewResult;
}
```

#### 1.3 添加辅助方法

```csharp
/// <summary>
/// 获取对应的 Controller
/// </summary>
protected virtual IControllerBase<M> GetController()
{
    // 根据泛型类型 M 自动获取对应的 Controller
    return Startup.GetFromFac<IControllerBase<M>>();
}

/// <summary>
/// 同步任务状态
/// </summary>
protected async Task SyncTodoStatusAsync(M entity, string action)
{
    try
    {
        var todoService = Startup.GetFromFac<ITodoUpdateService>();
        await todoService.SyncTodoStatusAsync(entity, action);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, $"同步任务状态失败：{action}");
        // 不阻断主流程
    }
}
```

---

### 步骤 2：重构 BaseBillEditGeneric 使用服务

在 `BaseBillEditGeneric.cs` 中，将现有的提交、审核等方法改为调用 `IBillOperationService`：

#### 2.1 添加服务依赖
在类中添加字段：
```csharp
/// <summary>
/// 单据业务操作服务
/// </summary>
protected readonly IBillOperationService BillOperationService;

// 在构造函数中初始化
public BaseBillEditGeneric()
{
    InitializeComponent();
    BillOperationService = Startup.GetFromFac<IBillOperationService>();
    // ... 其他初始化代码 ...
}
```

#### 2.2 重构 Submit 方法
修改现有的 Submit 方法（约第 5125 行）：
```csharp
protected async override Task<bool> Submit()
{
    if (EditEntity == null)
    {
        MessageBox.Show("没有可提交的单据");
        return false;
    }

    // 调用业务服务
    var result = await BillOperationService.SubmitAsync(EditEntity);
    
    if (result.Success)
    {
        // 更新 UI 状态
        UpdateAllUIStates(EditEntity);
        
        // 同步图片（如果需要）
        await SyncImagesIfNeeded();
        
        return true;
    }
    else
    {
        MessageBox.Show(result.Message);
        return false;
    }
}
```

#### 2.3 重构 Review 方法
修改现有的 Review 方法（约第 3718 行）：
```csharp
protected async override Task<ReviewResult> Review()
{
    var reviewResult = new ReviewResult();
    
    if (EditEntity == null)
    {
        MessageBox.Show("没有可审核的单据");
        return reviewResult;
    }

    // 调用业务服务
    var result = await BillOperationService.ReviewAsync(EditEntity, null);
    
    if (result.Success)
    {
        reviewResult.Success = true;
        
        // 更新 UI 状态
        UpdateAllUIStates(EditEntity);
        
        // 特殊业务处理（如销售出库单锁定订单）
        await HandleSpecialBusinessLogicAfterReview();
    }
    
    return reviewResult;
}

/// <summary>
/// 子类可重写此方法处理特殊业务逻辑
/// </summary>
protected virtual Task HandleSpecialBusinessLogicAfterReview()
{
    return Task.CompletedTask;
}
```

---

### 步骤 3：扩展 BaseBillQueryMC 使用服务

在 `BaseBillQueryMC.cs` 中集成服务调用：

#### 3.1 添加服务依赖
在类中添加字段（与 BaseBillEditGeneric 一致）：
```csharp
/// <summary>
/// 单据业务操作服务
/// </summary>
protected readonly IBillOperationService BillOperationService;

// 在构造函数中初始化
public BaseBillQueryMC()
{
    InitializeComponent();
    BillOperationService = Startup.GetFromFac<IBillOperationService>();
    // ... 其他初始化代码 ...
}
```

#### 3.2 实现 Submit 方法
修改现有的 Submit 方法（约第 1123 行）：
```csharp
public async override Task<bool> Submit()
{
    if (selectlist.Count == 0)
    {
        MessageBox.Show("请选择要提交的单据");
        return false;
    }

    // 根据选中数量决定调用单个还是批量
    if (selectlist.Count == 1)
    {
        var result = await BillOperationService.SubmitAsync(selectlist[0]);
        if (result.Success)
        {
            RefreshData(); // 刷新列表
            return true;
        }
        else
        {
            MessageBox.Show(result.Message);
            return false;
        }
    }
    else
    {
        // 批量提交
        var result = await BillOperationService.SubmitBatchAsync(selectlist);
        if (result.Success)
        {
            RefreshData(); // 刷新列表
            return true;
        }
        else
        {
            MessageBox.Show(result.Message);
            return false;
        }
    }
}
```

#### 3.3 增强 Review 方法
修改现有的 Review 方法（约第 1155 行），保持方法签名不变，内部调用服务：
```csharp
protected async override Task<ApprovalEntity> Review(List<M> EditEntities, int delayMs)
{
    if (EditEntities.Count == 0)
    {
        MessageBox.Show("请选择要审核的单据");
        return null;
    }

    // 1. 弹出审核对话框获取用户审核意见
    ApprovalEntity batchApprovalInfo = new ApprovalEntity();
    // ... 初始化审核信息 ...
    
    CommonUI.frmApproval frm = new CommonUI.frmApproval();
    frm.BindData(batchApprovalInfo);
    
    if (frm.ShowDialog() != DialogResult.OK)
    {
        return null; // 用户取消审核
    }

    // 2. 调用业务服务进行批量审核
    var result = await BillOperationService.ReviewBatchAsync(EditEntities, batchApprovalInfo);
    
    if (result.Success)
    {
        // 刷新列表
        RefreshData();
    }
    
    MessageBox.Show(result.Message);
    return batchApprovalInfo;
}
```

#### 3.4 实现 ReReview 方法
使用服务调用（约第 1446 行）：
```csharp
public async override Task<ApprovalEntity> ReReview(M EditEntity)
{
    if (EditEntity == null)
    {
        return null;
    }

    ApprovalEntity ae = new ApprovalEntity();
    
    // 显示反审核对话框
    CommonUI.frmReApproval frm = new CommonUI.frmReApproval();
    frm.BindData(ae);
    
    if (frm.ShowDialog() != DialogResult.OK)
    {
        return ae;
    }

    // 调用业务服务
    var result = await BillOperationService.ReReviewAsync(EditEntity, ae);
    
    if (result.Success)
    {
        // 刷新列表
        RefreshData();
    }
    else
    {
        MessageBox.Show(result.Message);
    }
    
    return ae;
}
```

---

### 步骤 4：在 UCSaleOrderQuery 中重写特定方法

`UCSaleOrderQuery.cs` 需要根据销售订单的特殊需求重写某些方法：

#### 4.1 重写 Submit 方法（如需要）
如果销售订单有特殊的提交验证逻辑：
```csharp
public async override Task<bool> Submit()
{
    if (selectlist.Count == 0)
    {
        MessageBox.Show("请选择要提交的单据");
        return false;
    }

    // 销售订单特有的提交前验证
    foreach (var order in selectlist)
    {
        if (!ValidateSaleOrderBeforeSubmit(order))
        {
            return false;
        }
    }

    // 调用基类方法（会使用 BillOperationService）
    return await base.Submit();
}

/// <summary>
/// 销售订单提交前验证
/// </summary>
private bool ValidateSaleOrderBeforeSubmit(tb_SaleOrder order)
{
    // 例如：检查必填字段、业务规则等
    if (string.IsNullOrEmpty(order.SOrderNo))
    {
        MessageBox.Show($"订单 {order.BillNo} 缺少订单编号");
        return false;
    }
    
    // 其他验证...
    return true;
}
```

#### 4.2 重写 CloseCase 方法
根据销售订单的结案逻辑：
```csharp
public async override Task<bool> CloseCase(List<tb_SaleOrder> entities)
{
    // TODO: 实现销售订单特有的结案逻辑
    // 可以调用 BillOperationService.CloseCaseBatchAsync(entities)
    return await Task.FromResult(true);
}
```

#### 4.3 重写 AntiCloseCase 方法（如需要）
```csharp
public async override Task<bool> AntiCloseCase(List<tb_SaleOrder> entities)
{
    // TODO: 实现销售订单特有的反结案逻辑
    return await Task.FromResult(true);
}
```

---

### 步骤 6：状态同步机制

#### 6.1 操作成功后刷新列表
在查询列表中，操作成功后调用 `RefreshData()` 方法刷新数据：

```csharp
// 提交成功后
var result = await BillOperationService.SubmitAsync(entity);
if (result.Success)
{
    RefreshData(); // 刷新列表显示最新状态
}

// 审核成功后
var reviewResult = await BillOperationService.ReviewAsync(entity, approvalInfo);
if (reviewResult.Success)
{
    RefreshData(); // 刷新列表显示最新状态
}
```

#### 6.2 实体状态变化事件订阅（可选）
如果需要实时更新（其他用户操作导致的状态变化），可以在查询列表中订阅实体的状态变化事件：

```csharp
// 在 QueryConditionBuilder 或 BindData 方法中
private void SubscribeEntityStatusChanges()
{
    foreach (var entity in Datas)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.PropertyChanged += (sender, e) => {
                if (e.PropertyName == "BusinessStatus" || 
                    e.PropertyName == "ApprovalStatus" ||
                    e.PropertyName == "ApprovalResults")
                {
                    this.InvokeIfNeeded(() => {
                        // 局部更新该行
                        dgvList.Refresh();
                    });
                }
            };
        }
    }
}
```

---

### 步骤 7：权限控制集成

#### 7.1 菜单权限
通过基类已有的 `UIHelper.ControlButton` 自动应用菜单权限，无需额外代码。

#### 7.2 行级权限
保持 `UCSaleOrderQuery.QueryConditionBuilder()` 中已有的实现：

```csharp
public override void QueryConditionBuilder()
{
    base.QueryConditionBuilder();
    
    // 业务员只能查看自己的订单
    var lambda = Expressionable.Create<tb_SaleOrder>()
        .AndIF(
            AuthorizeController.GetSaleLimitedAuth(MainForm.Instance.AppContext) 
            && !MainForm.Instance.AppContext.IsSuperUser, 
            t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID
        )
        .ToExpression();
    
    QueryConditionFilter.FilterLimitExpressions.Add(lambda);
}
```

#### 7.3 状态权限
通过 `StateManager.CanExecuteActionWithMessage` 在服务层自动验证，已在 `BillOperationService` 中实现。

---

## 验证方案

### 编译检查

```bash
# 编译项目，检查是否有编译错误
dotnet build RUINORERP.UI/RUINORERP.UI.csproj --no-incremental
```

### 功能测试清单

#### 提交功能
- [ ] **单张提交**：选中草稿状态的单据，点击提交按钮，状态变为"已提交"
- [ ] **批量提交**：选中多张草稿状态单据提交，观察日志输出成功/失败数量
- [ ] **状态验证**：选中已提交状态的单据，点击提交按钮，提示"当前状态下无法提交"
- [ ] **空选择处理**：未选择单据时点击提交，提示"请选择要提交的单据"
- [ ] **异常处理**：模拟网络异常，显示错误提示且数据不丢失
- [ ] **防重复**：快速连续点击提交按钮，第二次操作被阻止

#### 审核功能
- [ ] **单张审核**：选中已提交状态的单据，点击审核，弹出审核对话框
- [ ] **批量审核**：选中多张已提交状态单据，批量审核
- [ ] **审核通过**：审核通过后，状态变为"确认"，列表实时刷新
- [ ] **审核驳回**：审核驳回，状态变为"审核驳回"，显示驳回意见
- [ ] **反审核**：对确认状态的单据执行反审核，状态恢复为"已提交"
- [ ] **撤销恢复**：审核失败时，数据能恢复到原始状态

#### 权限控制
- [ ] **菜单权限**：无提交权限的用户，提交按钮隐藏或禁用
- [ ] **菜单权限**：无审核权限的用户，审核按钮隐藏或禁用
- [ ] **行级权限**：业务员只能看到自己的订单
- [ ] **状态权限**：草稿状态允许提交，已提交状态不允许提交
- [ ] **状态权限**：已提交状态允许审核，草稿状态不允许审核

#### 状态同步
- [ ] **提交后刷新**：提交成功后，列表立即显示最新状态
- [ ] **审核后刷新**：审核成功后，列表立即显示最新状态
- [ ] **局部更新**：只刷新状态列，不重新加载整个列表（如实现）

#### 错误处理
- [ ] **重复提交**：防重复机制生效，显示友好提示
- [ ] **网络异常**：显示错误日志，记录到 uclog
- [ ] **审核失败**：使用 RevertCommand 恢复原始状态
- [ ] **异常日志**：所有异常都记录到日志系统

### 手动测试流程

1. **登录系统** → 销售管理 → 销售订单 → 查询列表

2. **测试提交**：
   - 找到一张草稿状态的销售订单
   - 选中该订单，点击工具栏"提交"按钮
   - 验证：状态变为"已提交"，列表刷新
   
3. **测试批量提交**：
   - 按住 Ctrl 键选中多张草稿状态订单
   - 点击"提交"按钮
   - 验证：显示成功/失败数量，列表刷新

4. **测试审核**：
   - 选中一张已提交状态的订单
   - 点击"审核"按钮
   - 填写审核意见，点击确定
   - 验证：状态变为"确认"，列表刷新

5. **测试批量审核**：
   - 选中多张已提交状态订单
   - 点击"审核"按钮
   - 验证：批量审核完成，显示结果

6. **测试反审核**：
   - 选中一张确认状态的订单
   - 点击"反审"按钮（如果工具栏有）
   - 验证：状态恢复为"已提交"

7. **测试权限**：
   - 使用不同角色的账号登录
   - 验证按钮的可见性和启用状态

8. **测试异常**：
   - 断开网络连接
   - 尝试提交或审核
   - 验证：显示友好的错误提示

### 自动化测试建议（如有单元测试框架）

```csharp
[TestClass]
public class BillOperationServiceTests
{
    private IBillOperationService _service;
    private Mock<BaseController<tb_SaleOrder>> _mockController;
    private Mock<IUnifiedStateManager> _mockStateManager;

    [TestInitialize]
    public void Setup()
    {
        _mockController = new Mock<BaseController<tb_SaleOrder>>();
        _mockStateManager = new Mock<IUnifiedStateManager>();
        _service = new BillOperationService(/* 注入 mock 对象 */);
    }

    [TestMethod]
    public async Task SubmitAsync_ShouldReturnSuccess_WhenControllerReturnsSuccess()
    {
        // Arrange
        var entity = new tb_SaleOrder { BillNo = "SO20260318001" };
        _mockController.Setup(c => c.SubmitAsync(entity))
            .ReturnsAsync(new ReturnResults<tb_SaleOrder> { Succeeded = true });

        // Act
        var result = await _service.SubmitAsync(entity);

        // Assert
        Assert.IsTrue(result.Success);
        Assert.AreEqual("SO20260318001 提交成功", result.Message);
    }

    [TestMethod]
    public async Task SubmitAsync_ShouldBlockDuplicateOperation()
    {
        // Arrange
        var entity = new tb_SaleOrder { BillNo = "SO20260318001" };
        // 模拟防重复服务阻止操作

        // Act
        var result = await _service.SubmitAsync(entity);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsTrue(result.Message.Contains("请勿重复提交"));
    }
}
```

---

## 风险评估与回滚方案

### 风险评估

| 风险项 | 风险等级 | 影响范围 | 缓解措施 |
|--------|---------|---------|---------|
| 服务注册失败 | 低 | 新功能无法使用 | 在 Startup 中添加 try-catch，启动时检测 |
| 现有功能受影响 | 中 | 编辑界面提交/审核 | 充分测试，保持向后兼容 |
| 状态管理不一致 | 中 | 按钮状态显示错误 | 确保 StateManager 正确初始化 |
| 性能问题 | 低 | 批量操作缓慢 | 使用异步方法，添加进度提示 |

### 回滚方案

如发现问题，可通过 Git 快速回滚：

```bash
# 查看修改的文件
git status

# 回滚特定文件
git checkout -- RUINORERP.UI/BusinessService/IBillOperationService.cs
git checkout -- RUINORERP.UI/BusinessService/BillOperationService.cs
git checkout -- RUINORERP.UI/BaseForm/BaseBillQueryMC.cs
git checkout -- RUINORERP.UI/BaseForm/BaseBillEditGeneric.cs

# 或者回滚整个提交
git reset --hard HEAD~1
```

### 应急预案

1. **服务注册失败**：在 `BillOperationService` 构造函数中添加 fallback 逻辑
2. **批量操作卡死**：添加超时机制和取消按钮
3. **状态不同步**：提供手动刷新按钮

---

## 总结

本方案通过创建通用的 `IBillOperationService` 业务操作服务接口和 `BillOperationService` 实现类，实现了以下目标：

1. **代码复用**：提交、审核、反审核等业务逻辑在两个基类间完全复用
2. **一致性保证**：编辑界面和查询列表使用同一套业务逻辑，行为完全一致
3. **易于维护**：业务规则变更只需修改一处
4. **可测试性**：独立服务便于单元测试和 Mock
5. **扩展性**：新增操作类型（如结案、反结案）只需在接口中添加方法

关键改动点：
- 新建 2 个文件：`IBillOperationService.cs`、`BillOperationService.cs`
- 修改 2 个基类：`BaseBillEditGeneric.cs`、`BaseBillQueryMC.cs`
- 子类（如 `UCSaleOrderQuery`）按需重写特定方法

此方案基于现有架构体系，不涉及底层重构，风险可控。

---

## 总结

本方案通过创建通用的 `IBillOperationService` 业务操作服务接口和 `BillOperationService` 实现类，实现了以下目标：

1. **代码复用**：提交、审核、反审核等业务逻辑在两个基类间完全复用
2. **一致性保证**：编辑界面和查询列表使用同一套业务逻辑，行为完全一致
3. **易于维护**：业务规则变更只需修改一处
4. **可测试性**：独立服务便于单元测试和 Mock
5. **扩展性**：新增操作类型（如结案、反结案）只需在接口中添加方法

### 关键改动点

- **新建 2 个文件**：
  - `RUINORERP.UI\BusinessService\IBillOperationService.cs` - 业务操作服务接口
  - `RUINORERP.UI\BusinessService\BillOperationService.cs` - 业务操作服务实现

- **修改 2 个基类**：
  - `RUINORERP.UI\BaseForm\BaseBillEditGeneric.cs` - 重构为使用服务
  - `RUINORERP.UI\BaseForm\BaseBillQueryMC.cs` - 集成服务调用

- **子类按需调整**：
  - `RUINORERP.UI\PSI\SAL\UCSaleOrderQuery.cs` - 按需重写特定方法
  - `RUINORERP.UI\PSI\SAL\UCSaleOrder.cs` - 保持不变或微调

### 架构优势

```
┌─────────────────────────────────────────────────┐
│           IBillOperationService                 │
│  (统一业务逻辑接口)                              │
└─────────────────────────────────────────────────┘
                       ↑
                       │ 依赖注入
                       │
        ┌──────────────┴──────────────┐
        │                             │
┌───────▼────────┐          ┌────────▼────────┐
│BaseBillEdit    │          │BaseBillQueryMC  │
│Generic<T,C>    │          │<M,C>            │
│(编辑界面基类)   │          │(查询列表基类)    │
└────────────────┘          └─────────────────┘
        ↑                             ↑
        │ 继承                        │ 继承
        │                             │
┌───────┴────────┐          ┌────────┴────────┐
│ UCSaleOrder    │          │UCSaleOrderQuery │
│(销售订单编辑)  │          │(销售订单查询)    │
└────────────────┘          └─────────────────┘
```

此方案基于现有架构体系，不涉及底层重构，风险可控。通过服务层封装，实现了编辑界面和查询列表的业务逻辑统一，为未来的功能扩展奠定了良好基础。
