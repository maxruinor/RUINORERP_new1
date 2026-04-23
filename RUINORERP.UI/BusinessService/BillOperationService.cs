using Microsoft.Extensions.Logging;
using RUINOR.Core; // 添加RevertCommand的命名空间
using RUINORERP.Business;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Extensions; // 添加SetPropertyValue扩展方法
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.UI.UserCenter.DataParts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.BusinessService
{
    #region 状态变更事件定义

    /// <summary>
    /// 单据状态变更事件参数
    /// </summary>
    public class BillStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 单据实体
        /// </summary>
        public BaseEntity Entity { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public MenuItemEnums OperationType { get; set; }

        /// <summary>
        /// 操作前状态
        /// </summary>
        public object PreviousStatus { get; set; }

        /// <summary>
        /// 操作后状态
        /// </summary>
        public object CurrentStatus { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }

        /// <summary>
        /// 操作用户ID
        /// </summary>
        public long OperatorId { get; set; }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperationDescription { get; set; }

        public BillStatusChangedEventArgs(BaseEntity entity, MenuItemEnums operationType, object previousStatus, object currentStatus, long operatorId, string operationDescription)
        {
            Entity = entity;
            OperationType = operationType;
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
            OperationTime = DateTime.Now;
            OperatorId = operatorId;
            OperationDescription = operationDescription;
        }
    }

    /// <summary>
    /// 单据操作完成事件参数
    /// </summary>
    public class BillOperationCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// 操作结果
        /// </summary>
        public BillOperationResult Result { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public MenuItemEnums OperationType { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperationTime { get; set; }
        
        /// <summary>
        /// 操作的单据实体(用于关联单据同步等场景)
        /// </summary>
        public BaseEntity Entity { get; set; }

        public BillOperationCompletedEventArgs(BillOperationResult result, MenuItemEnums operationType, BaseEntity entity = null)
        {
            Result = result;
            OperationType = operationType;
            OperationTime = DateTime.Now;
            Entity = entity;
        }
    }

    #endregion

    /// <summary>
    /// 单据业务操作服务实现
    /// 封装通用的业务逻辑，供编辑界面和查询列表共用
    /// </summary>
    public class BillOperationService : IBillOperationService
    {
        private readonly ILogger<BillOperationService> _logger;
        private readonly RepeatOperationGuardService _guardService;

        #region 状态变更事件

        /// <summary>
        /// 单据状态变更事件
        /// </summary>
        public event EventHandler<BillStatusChangedEventArgs> BillStatusChanged;

        /// <summary>
        /// 单据操作完成事件
        /// </summary>
        public event EventHandler<BillOperationCompletedEventArgs> BillOperationCompleted;

        /// <summary>
        /// 触发单据状态变更事件
        /// </summary>
        protected virtual void OnBillStatusChanged(RUINORERP.UI.BusinessService.BillStatusChangedEventArgs e)
        {
            BillStatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 触发单据操作完成事件
        /// </summary>
        protected virtual void OnBillOperationCompleted(RUINORERP.UI.BusinessService.BillOperationCompletedEventArgs e)
        {
            BillOperationCompleted?.Invoke(this, e);
        }

        #endregion

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
                if (_guardService.ShouldBlockOperation(MenuItemEnums.提交, typeof(T).Name, entityId, showStatusMessage: true))
                {
                    result.Success = false;
                    result.Message = "操作正在进行中, 请勿重复提交";
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
                        result.Message = $"无法提交: {message}";
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
                    object previousStatus = null;
                    object currentStatus = null;
                    
                    if (stateManager != null)
                    {
                        previousStatus = stateManager.GetBusinessStatus(entity);
                        await stateManager.SetStatusByActionAsync(entity, MenuItemEnums.提交, "提交成功");
                        currentStatus = stateManager.GetBusinessStatus(entity);
                    }

                    // 5. 同步任务状态（增强版）- 同时处理两种状态同步机制
                    await SyncTodoStatusAsync(entity, "提交", MenuItemEnums.提交, previousStatus, currentStatus);

                    // 6. 记录操作
                    _guardService.RecordOperation(MenuItemEnums.提交, typeof(T).Name, entityId);

                    result.Success = true;
                    result.Message = $"{entity.GetPropertyValue("BillNo")}提交成功";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.提示);

                    // 7. 触发状态变更事件（核心同步机制）
                    await TriggerStatusChangeEvent(entity, MenuItemEnums.提交, previousStatus, currentStatus, true, result.Message);
                }
                else
                {
                    result.Success = false;
                    result.Message = $"提交失败: {submitResult.ErrorMsg}";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.错误);

                    // 触发失败状态变更事件
                    await TriggerStatusChangeEvent(entity, MenuItemEnums.提交, null, null, false, result.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交异常");
                result.Success = false;
                result.Message = $"提交异常: {ex.Message}";
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
                    failureMessages.AppendLine($"{entity.GetPropertyValue("BillNo")}: {singleResult.Message}");
                }
            }

            result.Success = successCount > 0;
            result.Message = $"批量提交完成: 成功{successCount}条, 失败{failCount}条";
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
                if (_guardService.ShouldBlockOperation(MenuItemEnums.审核, typeof(T).Name, entityId, showStatusMessage: true))
                {
                    result.Success = false;
                    result.Message = "操作正在进行中, 请勿重复提交";
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
                        result.Message = $"审核失败: {message}";
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
                        object previousStatus = originalStatus;
                        object currentStatus = null;
                        
                        if (stateManager != null)
                        {
                            await stateManager.SetStatusByActionAsync(entity, MenuItemEnums.审核, "审核通过");
                            currentStatus = stateManager.GetBusinessStatus(entity);
                        }

                        // 同步任务状态（增强版）- 同时处理两种状态同步机制
                    await SyncTodoStatusAsync(entity, "审核", MenuItemEnums.审核, previousStatus, currentStatus);

                        // 记录操作
                        _guardService.RecordOperation(MenuItemEnums.审核, typeof(T).Name, entityId);

                        result.Success = true;
                        result.Message = $"{entity.GetPropertyValue("BillNo")}审核成功";
                        MainForm.Instance.uclog.AddLog(result.Message, UILogType.普通消息);

                        // 触发状态变更事件
                        await TriggerStatusChangeEvent(entity, MenuItemEnums.审核, previousStatus, currentStatus, true, result.Message);
                    }
                    else
                    {
                        // 审核失败，恢复状态
                        command.Undo();
                        result.Success = false;
                        result.Message = $"{entity.GetPropertyValue("BillNo")}审核失败: {rmr.ErrorMsg}";
                        MessageBox.Show(result.Message);

                        // 触发失败状态变更事件
                        await TriggerStatusChangeEvent(entity, MenuItemEnums.审核, originalStatus, originalStatus, false, result.Message);
                    }
                }
                else
                {
                    // 审核驳回
                    object previousStatus = originalStatus;
                    object currentStatus = null;
                    
                    if (stateManager != null)
                    {
                        await stateManager.HandleApprovalRejectAsync(entity, approvalInfo.ApprovalOpinions);
                        currentStatus = stateManager.GetBusinessStatus(entity);
                    }
                    
                    // 更新审核意见和状态
                    if (entity is BaseEntity baseEntity)
                    {
                        baseEntity.SetPropertyValue("ApprovalOpinions", approvalInfo.ApprovalOpinions);
                        baseEntity.SetPropertyValue("ApprovalStatus", (int)ApprovalStatus.审核驳回);
                        baseEntity.SetPropertyValue("ApprovalResults", false);
                    }
                    BusinessHelper.Instance.ApproverEntity(entity);
                    
                    await controller.BaseSaveOrUpdate(entity);

                    result.Success = true;
                    result.Message = $"{entity.GetPropertyValue("BillNo")}已驳回";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.普通消息);

                    // 触发状态变更事件
                    await TriggerStatusChangeEvent(entity, MenuItemEnums.审核, previousStatus, currentStatus, true, result.Message);
                }

                // 6. 创建审计日志
                await MainForm.Instance.AuditLogHelper.CreateAuditLog<T>("审核", entity,
                    $"意见{approvalInfo.ApprovalOpinions}" + $"结果:{(approvalInfo.ApprovalResults ? "通过" : "拒绝")}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "审核异常");
                result.Success = false;
                result.Message = $"审核异常: {ex.Message}";
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
                    failureMessages.AppendLine($"{entity.GetPropertyValue("BillNo")}: {singleResult.Message}");
                }
            }

            result.Success = successCount > 0;
            result.Message = $"批量审核完成: 成功{successCount}条, 失败{failCount}条";
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
                        result.Message = $"反审核失败: {message}";
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
                        await stateManager.SetStatusByActionAsync(entity, MenuItemEnums.反审, "反审核通过");
                    }

                    result.Success = true;
                    result.Message = $"{entity.GetPropertyValue("BillNo")}反审核成功";
                    MainForm.Instance.uclog.AddLog(result.Message, UILogType.普通消息);
                }
                else
                {
                    // 反审核失败，恢复状态
                    command.Undo();
                    result.Success = false;
                    result.Message = $"{entity.GetPropertyValue("BillNo")}反审核失败: {rmr.ErrorMsg}";
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
                result.Message = $"反审核异常: {ex.Message}";
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

        #region 状态同步核心方法

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
                approvalInfo.bizName = baseEntity.GetPropertyValue("BillNo")?.ToString();
                approvalInfo.BillNo = baseEntity.GetPropertyValue("BillNo")?.ToString();
                approvalInfo.ApprovalResults = true;
                approvalInfo.ApprovalOpinions = "同意";
            }

            return approvalInfo;
        }

        /// <summary>
        /// 同步任务状态（统一版本）- 整合三种同步机制，避免重复调用
        /// </summary>
        private async Task SyncTodoStatusAsync<T>(T entity, string action, MenuItemEnums operationType, object previousStatus, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 创建统一的 TodoUpdate 对象
                var todoUpdate = CreateTodoUpdate(entity, operationType, previousStatus, currentStatus);
                todoUpdate.OperationDescription = action;
                var todoUpdateList = new List<TodoUpdate> { todoUpdate };

                // 1. 同步到工作台待办事项 - 使用 TodoListManager
                await SyncToTodoListManagerAsync(todoUpdateList);

                // 2. 同步到 TodoSyncManager（事件驱动机制）- 内部已优化
                await SyncToTodoSyncManagerAsync(todoUpdate);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"同步任务状态失败：{action}");
            }
        }

        /// <summary>
        /// 同步到工作台待办事项管理器
        /// </summary>
        private async Task SyncToTodoListManagerAsync(List<TodoUpdate> updates)
        {
            try
            {
                var todoListManager = Startup.GetFromFac<TodoListManager>();
                if (todoListManager != null)
                {
                    todoListManager.ProcessUpdates(updates);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "工作台待办事项同步失败");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 同步到 TodoSyncManager（事件驱动机制）
        /// </summary>
        private async Task SyncToTodoSyncManagerAsync(TodoUpdate todoUpdate)
        {
            try
            {
                var todoSyncManager = Startup.GetFromFac<TodoSyncManager>();
                if (todoSyncManager != null)
                {
                    // 使用 PublishUpdate 进行事件驱动同步
                    todoSyncManager.PublishUpdate(todoUpdate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "事件驱动状态同步失败");
            }
        }

        /// <summary>
        /// 创建TodoUpdate对象
        /// 注意：不传递实体对象以避免序列化大对象导致 OOM
        /// </summary>
        private TodoUpdate CreateTodoUpdate<T>(T entity, MenuItemEnums operationType, object previousStatus, object currentStatus) where T : BaseEntity
        {
            // ✅ 修复：使用CreateSafe方法，确保Entity为null，避免OOM
            var todoUpdate = TodoUpdate.CreateSafe(
                GetTodoUpdateType(operationType),
                GetBusinessTypeFromEntity(entity),
                entity.PrimaryKeyID,
                entity.GetPropertyValue("BillNo")?.ToString(),
                typeof(DataStatus),
                currentStatus
            );
        
            todoUpdate.OperationDescription = GetOperationDescription(operationType);
            todoUpdate.InitiatorUserId = MainForm.Instance?.AppContext?.CurUserInfo?.UserInfo?.User_ID.ToString() ?? "0";
                    
            // 添加关键条件值，避免客户端重新查询
            if (previousStatus != null)
            {
                todoUpdate.ConditionValues["PreviousStatus"] = previousStatus;
            }
            if (currentStatus != null)
            {
                todoUpdate.ConditionValues["CurrentStatus"] = currentStatus;
            }
            todoUpdate.ConditionValues["OperationTime"] = DateTime.Now;
        
            return todoUpdate;
        }

        /// <summary>
        /// 根据实体类型获取业务类型
        /// </summary>
        private BizType GetBusinessTypeFromEntity<T>(T entity) where T : BaseEntity
        {
            try
            {
                var bizType = EntityMappingHelper.GetBizType(typeof(T));
                if (bizType == default(BizType))
                {
                    _logger.LogWarning("无法获取实体类型对应的BizType: {EntityType}, 使用默认值", typeof(T).Name);
                    return BizType.采购订单; // 降级为默认值
                }
                return bizType;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取BizType失败: {EntityType}", typeof(T).Name);
                return BizType.采购订单; // 异常时返回默认值
            }
        }

        /// <summary>
        /// 根据操作类型获取TodoUpdateType
        /// </summary>
        private TodoUpdateType GetTodoUpdateType(MenuItemEnums operationType)
        {
            switch (operationType)
            {
                case MenuItemEnums.提交:
                case MenuItemEnums.审核:
                case MenuItemEnums.反审:
                    return TodoUpdateType.StatusChanged;
                case MenuItemEnums.新增:
                    return TodoUpdateType.Created;
                case MenuItemEnums.删除:
                    return TodoUpdateType.Deleted;
                default:
                    return TodoUpdateType.StatusChanged;
            }
        }

        /// <summary>
        /// 获取操作描述
        /// </summary>
        private string GetOperationDescription(MenuItemEnums operationType)
        {
            return operationType switch
            {
                MenuItemEnums.提交 => "提交",
                MenuItemEnums.审核 => "审核",
                MenuItemEnums.反审 => "反审",
                MenuItemEnums.新增 => "新增",
                MenuItemEnums.删除 => "删除",
                _ => operationType.ToString()
            };
        }

        /// <summary>
        /// 触发状态变更事件（核心同步方法）- 增强版，协调两种状态机制
        /// </summary>
        private async Task TriggerStatusChangeEvent<T>(T entity, MenuItemEnums operationType, object previousStatus, object currentStatus, bool operationSuccess, string operationDescription) where T : BaseEntity
        {
            try
            {
                // 获取当前用户信息
                long operatorId = MainForm.Instance?.AppContext?.CurUserInfo?.UserInfo?.User_ID ?? 0;
                
                // 触发状态变更事件
                var statusEventArgs = new BillStatusChangedEventArgs(
                    entity, operationType, previousStatus, currentStatus, operatorId, operationDescription);
                OnBillStatusChanged(statusEventArgs);

                // 触发操作完成事件
                var operationResult = new BillOperationResult
                {
                    Success = operationSuccess,
                    Message = operationDescription,
                    OriginalStatus = previousStatus
                };
                var operationEventArgs = new BillOperationCompletedEventArgs(operationResult, operationType, entity);
                OnBillOperationCompleted(operationEventArgs);

                // 异步执行状态同步任务
                await Task.Run(async () =>
                {
                    try
                    {
                        // 1. 同步到查询列表
                        await SyncToQueryLists(entity, operationType, currentStatus);
                        
                        // 2. 同步到相关模块
                        await SyncToRelatedModules(entity, operationType, currentStatus);
                        
                        // 3. 记录状态变更审计日志
                        await LogStatusChangeAudit(entity, operationType, previousStatus, currentStatus, operatorId);
                        
                        // 4. 协调两种状态机制同步
                        await CoordinateTwoStateMechanisms(entity, operationType, previousStatus, currentStatus, operationSuccess);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "状态同步任务执行失败");
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "触发状态变更事件失败");
            }
        }

        /// <summary>
        /// 协调两种状态机制同步
        /// 确保单据窗体按钮状态和工作台待办状态协调工作
        /// </summary>
        private async Task CoordinateTwoStateMechanisms<T>(T entity, MenuItemEnums operationType, object previousStatus, object currentStatus, bool operationSuccess) where T : BaseEntity
        {
            try
            {
                if (!operationSuccess) return;

                // 1. 同步单据窗体按钮状态（机制1）
                await SyncFormButtonState(entity, currentStatus);
                
                // 2. 同步工作台待办状态（机制2）
                await SyncTodoListState(entity, operationType, previousStatus, currentStatus);
                
                // 3. 验证两种状态的一致性
                await ValidateStateConsistency(entity, operationType, previousStatus, currentStatus);
                
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "状态机制协调失败");
            }
        }

        /// <summary>
        /// 同步单据窗体按钮状态（机制1）
        /// </summary>
        private async Task SyncFormButtonState<T>(T entity, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取所有相关的编辑窗体实例
                var editForms = new List<Form>();
                foreach (Form form in Application.OpenForms)
                {
                    if (form.IsHandleCreated && !form.IsDisposed && form.GetType().Name.Contains("Edit"))
                    {
                        editForms.Add(form);
                    }
                }

                foreach (var editForm in editForms)
                {
                    try
                    {
                        // 使用反射调用UpdateAllUIStates方法
                        var updateMethod = editForm.GetType().GetMethod("UpdateAllUIStates");
                        if (updateMethod != null)
                        {
                            editForm.Invoke(new Action(() =>
                            {
                                updateMethod.Invoke(editForm, new object[] { entity });
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"同步窗体按钮状态失败: {editForm.Name}");
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步窗体按钮状态总体失败");
            }
        }

        /// <summary>
        /// 同步工作台待办状态（机制2）
        /// </summary>
        private async Task SyncTodoListState<T>(T entity, MenuItemEnums operationType, object previousStatus, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取所有工作台待办列表实例
                var todoLists = new List<Form>();
                foreach (Form form in Application.OpenForms)
                {
                    if (form.IsHandleCreated && !form.IsDisposed && form.GetType().Name.Contains("Todo"))
                    {
                        todoLists.Add(form);
                    }
                }

                foreach (var todoList in todoLists)
                {
                    try
                    {
                        // 创建工作台状态更新数据
                        var todoUpdate = CreateTodoUpdate(entity, operationType, previousStatus, currentStatus);
                        
                        // 使用反射调用RefreshDataNodes方法
                        var refreshMethod = todoList.GetType().GetMethod("RefreshDataNodes");
                        if (refreshMethod != null)
                        {
                            todoList.Invoke(new Action(() =>
                            {
                                refreshMethod.Invoke(todoList, new object[] { new List<TodoUpdate> { todoUpdate } });
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"同步工作台状态失败: {todoList.Name}");
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步工作台状态总体失败");
            }
        }

        /// <summary>
        /// 验证两种状态的一致性
        /// </summary>
        private async Task ValidateStateConsistency<T>(T entity, MenuItemEnums operationType, object previousStatus, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取状态管理器
                var stateManager = GetStateManager();
                if (stateManager == null) return;

                // 验证实体状态与状态管理器的一致性
                var managedStatus = stateManager.GetBusinessStatus(entity);
                if (managedStatus?.ToString() != currentStatus?.ToString())
                {
                    _logger.LogWarning($"状态不一致: 实体状态={currentStatus}, 管理器状态={managedStatus}");
                }

                // 验证工作台状态同步是否成功
                await ValidateTodoListSync(entity, operationType, currentStatus);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "状态一致性验证失败");
            }
        }

        /// <summary>
        /// 验证工作台状态同步是否成功
        /// </summary>
        private async Task ValidateTodoListSync<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 这里可以添加更复杂的工作台状态验证逻辑
                // 例如：检查工作台中是否确实反映了状态变更
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "工作台状态同步验证失败");
            }
        }

        /// <summary>
        /// 同步到查询列表
        /// </summary>
        private async Task SyncToQueryLists<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取所有相关的查询列表实例
                var queryLists = new List<Form>();
                foreach (Form form in Application.OpenForms)
                {
                    if (form.IsHandleCreated && !form.IsDisposed && form.GetType().Name.Contains("Query"))
                    {
                        queryLists.Add(form);
                    }
                }

                foreach (var queryList in queryLists)
                {
                    try
                    {
                        // 使用反射调用Query方法刷新数据
                        var queryMethod = queryList.GetType().GetMethod("Query");
                        var queryDtoProperty = queryList.GetType().GetProperty("QueryDtoProxy");
                        
                        if (queryMethod != null && queryDtoProperty != null)
                        {
                            queryList.Invoke(new Action(() =>
                            {
                                // 在UI线程中获取属性值
                                var queryDto = queryDtoProperty.GetValue(queryList, null);
                                queryMethod.Invoke(queryList, new[] { queryDto });
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"同步到查询列表失败: {queryList.Name}");
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步到查询列表总体失败");
            }
        }

        /// <summary>
        /// 同步到相关模块
        /// </summary>
        private async Task SyncToRelatedModules<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 1. 同步到工作流模块
                await SyncToWorkflowModule(entity, operationType, currentStatus);
                
                // 2. 同步到消息通知模块
                await SyncToNotificationModule(entity, operationType, currentStatus);
                
                // 3. 同步到缓存模块
                await SyncToCacheModule(entity, operationType, currentStatus);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步到相关模块失败");
            }
        }

        /// <summary>
        /// 同步到工作流模块
        /// </summary>
        private async Task SyncToWorkflowModule<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取工作流服务
                var workflowService = Startup.GetFromFac<IWorkflowService>();
                if (workflowService != null)
                {
                    await workflowService.OnBillStatusChanged(entity, operationType, currentStatus);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步到工作流模块失败");
            }
        }

        /// <summary>
        /// 同步到消息通知模块
        /// </summary>
        private async Task SyncToNotificationModule<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取消息通知服务
                var notificationService = Startup.GetFromFac<IMessageNotificationService>();
                if (notificationService != null)
                {
                    await notificationService.NotifyBillStatusChange(entity, operationType, currentStatus);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步到消息通知模块失败");
            }
        }

        /// <summary>
        /// 同步到缓存模块
        /// </summary>
        private async Task SyncToCacheModule<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity
        {
            try
            {
                // 获取缓存服务
                var cacheService = Startup.GetFromFac<IEntityCacheManager>();
                if (cacheService != null)
                {
                    // 更新缓存中的实体状态
                    await cacheService.UpdateEntityStatus(entity, currentStatus);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "同步到缓存模块失败");
            }
        }

        /// <summary>
        /// 记录状态变更审计日志
        /// </summary>
        private async Task LogStatusChangeAudit<T>(T entity, MenuItemEnums operationType, object previousStatus, object currentStatus, long operatorId) where T : BaseEntity
        {
            try
            {
                var auditLog = new
                {
                    EntityType = typeof(T).Name,
                    EntityId = entity.PrimaryKeyID,
                    OperationType = operationType.ToString(),
                    PreviousStatus = previousStatus?.ToString(),
                    CurrentStatus = currentStatus?.ToString(),
                    OperatorId = operatorId,
                    OperationTime = DateTime.Now,
                    BillNo = entity.GetPropertyValue("BillNo")
                };

                // 记录到审计日志
                _logger.LogInformation("状态变更审计: {@AuditLog}", auditLog);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "记录状态变更审计日志失败");
            }
        }

        #endregion

        #region 辅助接口定义（用于依赖注入）

        /// <summary>
        /// 工作流服务接口
        /// </summary>
        public interface IWorkflowService
        {
            Task OnBillStatusChanged<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity;
        }

        /// <summary>
        /// 消息通知服务接口
        /// </summary>
        public interface IMessageNotificationService
        {
            Task NotifyBillStatusChange<T>(T entity, MenuItemEnums operationType, object currentStatus) where T : BaseEntity;
        }

        /// <summary>
        /// 实体缓存管理器接口
        /// </summary>
        public interface IEntityCacheManager
        {
            Task UpdateEntityStatus<T>(T entity, object currentStatus) where T : BaseEntity;
        }

        #endregion
    }
}
