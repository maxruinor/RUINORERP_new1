/**
 * 文件: UnifiedStateManager.cs
 * 版本: V4 - 优化版统一状态管理器
 * 说明: 统一状态管理器 - 基于V4版本架构优化，去掉重复冗余代码
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本优化，使用EntityStatus和StateTransitionResult
 * 
 * 版本标识：
 * V4: 基于EntityStatus和StateTransitionResult的优化实现，去掉重复冗余代码
 * V3: 支持数据状态、操作状态和业务状态的统一管理
 * 公共代码: 状态管理核心逻辑，所有版本通用
 */

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using SqlSugar.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态管理器 - V4优化版
    /// 基于V4版本架构优化，去掉重复冗余代码，使用EntityStatus和StateTransitionResult
    /// 支持MenuItemEnums，实现操作权限检查和UI控件影响
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager, IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<UnifiedStateManager> _logger;

        /// <summary>
        /// 状态转换规则字典
        /// </summary>
        private Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;


        /// <summary>
        /// 状态转换事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;


        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="cache">缓存管理器</param>
        public UnifiedStateManager(ILogger<UnifiedStateManager> logger, IMemoryCache cache = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>(GlobalStateRulesManager.Instance.StateTransitionRules.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(innerKvp => innerKvp.Key, innerKvp => innerKvp.Value))); // 使用全局共享的规则实例
        }

        #endregion

        #region 状态获取方法
        /// <summary>
        /// 获取实体的统一状态（同步版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>统一状态</returns>
        public EntityStatus GetUnifiedStatus(BaseEntity entity)
        {
            if (entity == null)
                return null;

            // 通过中间值获取EntityStatus
            var entityStatus = new EntityStatus();
            var statusType = entityStatus.GetStatusType(entity);
            if (statusType != null)
            {
                // 优先返回DataStatus
                //    if (statusType == typeof(DataStatus))
                //{
                //    var dataStatus = entity.GetPropertyValue(nameof(DataStatus));
                //    entityStatus.SetBusinessStatus((DataStatus)dataStatus);
                //}
                entityStatus.SetBusinessStatus(statusType, entity.GetPropertyValue(statusType.Name));
            }
            entityStatus.actionStatus = entity.ActionStatus;
            if (entity.ContainsProperty("ApprovalResults"))
            {
                entityStatus.ApprovalResults = entity.GetPropertyValue("ApprovalResults").ObjToBool();
            }
            //已经审核的并且通过的情况才能结案
            if (entity.ContainsProperty("ApprovalStatus"))
            {
                entityStatus.ApprovalStatus= entity.GetPropertyValue("ApprovalStatus").ObjToInt();
            }
            return entityStatus; // 默认状态
        }

        /// <summary>
        /// 获取实体的状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>状态类型</returns>
        public Type GetStatusType(BaseEntity entity)
        {
            if (entity == null)
                return null;

            // 通过中间值获取EntityStatus
            var entityStatus = new EntityStatus();
            return entityStatus.GetStatusType(entity);
        }



        /// <summary>
        /// 获取实体的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态值</returns>
        public object GetBusinessStatus<T>(BaseEntity entity) where T : struct, Enum
        {
            if (entity == null)
                return default;

            return GetBusinessStatus(entity, typeof(T));
        }

        /// <summary>
        /// 获取实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>业务状态值</returns>
        public object GetBusinessStatus(BaseEntity entity, Type statusType)
        {
            if (entity == null || statusType == null)
                return null;

            // 通过中间值获取EntityStatus
            var entityStatus = new EntityStatus();
            var currentStatusType = entityStatus.GetStatusType(entity);

            // 如果是请求的状态类型，直接返回
            if (currentStatusType == statusType)
            {
                return entity.GetPropertyValue(statusType.Name);
            }

            return null;
        }

        /// <summary>
        /// 获取实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>业务状态值</returns>
        public object GetBusinessStatus(BaseEntity entity)
        {
            if (entity == null)
                return null;

            // 通过中间值获取EntityStatus
            var entityStatus = new EntityStatus();
            var currentStatusType = GetBusinessStatusType(entity);

            // 如果是请求的状态类型，直接返回
            //if (currentStatusType == )
            //{
            //    return entity.GetPropertyValue(statusType.Name);
            //}

            // 获取状态类型和值

            if (currentStatusType != null)
            {
                // 动态获取状态值
                dynamic status = entity.GetPropertyValue(currentStatusType.Name);
                int statusValue = (int)status;
                dynamic statusEnum = Enum.ToObject(currentStatusType, statusValue);
                //是一个类型
                //statusEnum

                var dataStatus = (DataStatus)(entity.GetPropertyValue(typeof(DataStatus).Name).ObjToInt());
                if (dataStatus == DataStatus.新建 || dataStatus == DataStatus.草稿)
                {

                }
                return statusValue;
            }

            return null;
        }

        #endregion

        #region 状态验证方法

        /// <summary>
        /// 验证数据状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public StateTransitionResult ValidateBusinessStatusTransitionAsync(Enum fromStatus, Enum toStatus)
        {
            // 参数校验
            if (fromStatus == null)
                return StateTransitionResult.Allowed();
                
            if (toStatus == null)
                return StateTransitionResult.Denied("目标状态不能为空");
            
            // 验证两个枚举类型是否一致
            if (fromStatus.GetType() != toStatus.GetType())
                return StateTransitionResult.Denied("源状态和目标状态类型必须一致");
        
            // 动态获取实际状态类型进行验证
            bool isAllowed = GlobalStateRulesManager.Instance.IsTransitionAllowed(fromStatus.GetType(), fromStatus, toStatus);

            if (isAllowed)
            {
                return StateTransitionResult.Allowed();
            }
            else
            {
                return StateTransitionResult.Denied($"不允许从{fromStatus}转换到{toStatus}");
            }
        }

        /// <summary>
        /// 验证业务状态转换是否合法
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public StateTransitionResult ValidateBusinessStatusTransitionAsync<T>(T? fromStatus, T? toStatus) where T : struct, Enum
        {
            // 如果源状态为空，允许设置任何状态
            if (!fromStatus.HasValue)
                return StateTransitionResult.Allowed();

            // 如果目标状态为空，不允许转换
            if (!toStatus.HasValue)
                return StateTransitionResult.Denied("目标状态不能为空");

            // 使用状态转换规则验证
            bool isAllowed = GlobalStateRulesManager.Instance.IsTransitionAllowed(typeof(T), fromStatus.Value, toStatus.Value);

            if (isAllowed)
            {
                return StateTransitionResult.Allowed();
            }
            else
            {
                return StateTransitionResult.Denied($"不允许从{fromStatus.Value}转换到{toStatus.Value}");
            }
        }

        /// <summary>
        /// 验证操作状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public StateTransitionResult ValidateActionStatusTransitionAsync(ActionStatus? fromStatus, ActionStatus? toStatus)
        {
            // 如果源状态为空，允许设置任何状态
            if (!fromStatus.HasValue)
                return StateTransitionResult.Allowed();

            // 如果目标状态为空，不允许转换
            if (!toStatus.HasValue)
                return StateTransitionResult.Denied("目标状态不能为空");

            // 使用状态转换规则验证
            bool isAllowed = GlobalStateRulesManager.Instance.IsTransitionAllowed(typeof(ActionStatus), fromStatus.Value, toStatus.Value);

            if (isAllowed)
            {
                return StateTransitionResult.Allowed();
            }
            else
            {
                return StateTransitionResult.Denied($"不允许从{fromStatus.Value}转换到{toStatus.Value}");
            }
        }

        #endregion

        #region 状态转换检查方法

        /// <summary>
        /// 检查是否可以转换到指定的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public bool CanTransitionToBusinessStatus(BaseEntity entity, DataStatus targetStatus)
        {
            if (entity == null)
                return false;
            var currentStatus = GetBusinessStatus(entity);
            var result = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus);


            return result.IsSuccess;
        }

        /// <summary>
        /// 检查是否可以转换到指定的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public bool CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : struct, Enum
        {
            if (entity == null)
                return false;

            var currentStatus = GetBusinessStatus<T>(entity);
            var result = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus);


            return result.IsSuccess;
        }

        /// <summary>
        /// 检查是否可以转换到指定的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public bool CanTransitionToActionStatus(BaseEntity entity, ActionStatus targetStatus)
        {
            if (entity == null)
                return false;

            var currentStatus = entity.ActionStatus;
            var result = ValidateActionStatusTransitionAsync(currentStatus, targetStatus);

            return result.IsSuccess;
        }

        #endregion



        #region 状态更新和事件方法

        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        private StateTransitionResult UpdateBusinessStatus(BaseEntity entity, Type statusType, object newStatus, string reason = null, string userId = null)
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, newStatus, statusType, "实体不能为空");

            if (statusType == null)
                return StateTransitionResult.Failure(null, newStatus, statusType, "状态类型不能为空");

            // 获取旧状态
            object oldStatus = null;

            if (statusType == typeof(DataStatus))
            {
                oldStatus = GetBusinessStatus(entity);
            }
            else
            {
                // 业务状态
                oldStatus = GetBusinessStatus(entity, statusType);
            }

            // 检查状态是否实际发生了变更
            if (Equals(oldStatus, newStatus))
            {
                return StateTransitionResult.Success(oldStatus, newStatus, statusType, "状态未发生变更");
            }

            try
            {
                // 更新状态 - 直接设置实体的状态属性，而不是调用SetBusinessStatusAsync（避免循环调用）
                var statusProperty = entity.GetType().GetProperty(statusType.Name);
                if (statusProperty != null && statusProperty.CanWrite)
                {
                    statusProperty.SetValue(entity, newStatus);
                }
                else if (statusType == typeof(DataStatus))
                {
                    // 对于DataStatus特殊处理
                    var dataStatusProperty = entity.GetType().GetProperty("DataStatus");
                    if (dataStatusProperty != null && dataStatusProperty.CanWrite)
                    {
                        dataStatusProperty.SetValue(entity, newStatus);
                    }
                }

                // 触发状态变更事件
                TriggerStatusChangedEvent(entity, statusType, oldStatus, newStatus, reason, userId);

                return StateTransitionResult.Success(oldStatus, newStatus, statusType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新实体状态时发生错误: {ErrorMessage}", ex.Message);
                return StateTransitionResult.Failure(oldStatus, newStatus, statusType, ex.Message, ex);
            }
        }


        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        private StateTransitionResult UpdateActionStatus(BaseEntity entity, ActionStatus newStatus, string reason = null, string userId = null)
        {
            // 获取旧状态
            var oldStatus = entity.ActionStatus;

            // 检查状态是否实际发生了变更
            if (Equals(oldStatus, newStatus))
            {
                return StateTransitionResult.Success(oldStatus, newStatus, typeof(ActionStatus), "状态未发生变更");
            }

            try
            {
                // 更新状态
                entity.ActionStatus = newStatus;
                // 触发状态变更事件
                TriggerStatusChangedEvent(entity, typeof(ActionStatus), oldStatus, newStatus, reason, userId);

                return StateTransitionResult.Success(oldStatus, newStatus, typeof(ActionStatus));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新实体状态时发生错误: {ErrorMessage}", ex.Message);
                return StateTransitionResult.Failure(oldStatus, newStatus, typeof(ActionStatus), ex.Message, ex);
            }
        }



        /// <summary>
        /// 触发状态变更事件（Winform桌面程序优化版）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        public void TriggerStatusChangedEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null)
        {
            // 快速检查是否有订阅者，避免不必要的对象创建
            if (StatusChanged == null) return;
            
            try
            {
                // 创建事件参数，简化构造函数调用（适配我们修改后的StateTransitionEventArgs）
                var eventArgs = new StateTransitionEventArgs(
                    entity,
                    statusType,
                    oldStatus,
                    newStatus,
                    reason,
                    userId,
                    null,
                    null); // 使用正确的8参数构造函数
                
                // 获取事件委托副本，避免多线程情况下的空引用问题
                var statusChangedEvent = StatusChanged;
                
                // 触发事件
                statusChangedEvent?.Invoke(this, eventArgs);
            }
            catch (Exception ex)
            {
                // 详细记录错误信息，但不中断程序流程
                _logger.LogError(ex, "触发状态变更事件时发生错误: {ErrorMessage}，实体类型: {EntityType}，状态类型: {StatusType}", 
                    ex.Message, entity?.GetType().Name ?? "未知", statusType?.Name ?? "未知");
            }
        }


        #endregion

        #region 状态设置方法





        /// <summary>
        /// 设置实体的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">业务状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> SetBusinessStatusAsync<T>(BaseEntity entity, T? status, string reason = null, string userId = null) where T : struct, Enum
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, status, typeof(T), "实体不能为空");

            // 验证状态转换
            var currentStatus = GetBusinessStatus<T>(entity);
            var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, status);
            if (!validationResult.IsSuccess)
            {
                // 触发状态检查事件
                return validationResult;
            }

            // 更新状态
            var result = UpdateBusinessStatus(entity, typeof(T), status, reason, userId);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 设置实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null, string userId = null)
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, status, statusType, "实体不能为空");

            if (statusType == null)
                return StateTransitionResult.Failure(null, status, statusType, "状态类型不能为空");

            // 验证状态转换
            var currentStatus = GetBusinessStatus(entity, statusType);
            var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, status as Enum);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // 更新状态
            var result = UpdateBusinessStatus(entity, statusType, status, reason, userId);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 设置实体的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> SetActionStatusAsync(BaseEntity entity, ActionStatus? status, string reason = null, string userId = null)
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, status, typeof(ActionStatus), "实体不能为空");

            // 验证状态转换
            var currentStatus = entity.ActionStatus;
            var validationResult = ValidateActionStatusTransitionAsync(currentStatus, status);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            // 更新状态
            var result = UpdateActionStatus(entity, status.Value, reason, userId);

            return await Task.FromResult(result);
        }

        #endregion

        #region 操作权限检查方法

        /// <summary>
        /// 检查是否可以执行指定操作，并返回详细消息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>操作结果</returns>
        public (bool CanExecute, string Message) CanExecuteActionWithMessage(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return (false, "实体不能为空");

            // 使用增强规则检查操作权限
            var canExecute = CanExecuteAction(entity, action);

            // 根据操作类型和当前状态返回相应的消息
            if (canExecute)
            {
                return (true, GetSuccessMessage(action));
            }
            else
            {
                return (false, GetFailureMessage(entity, action));
            }
        }

        /// <summary>
        /// 使用增强规则检查操作权限
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否可以执行</returns>
        private bool CanExecuteAction(BaseEntity entity, MenuItemEnums action)
        {
            // 获取实体的统一状态
            var entityStatus = GetUnifiedStatus(entity);

            // 如果没有状态信息，默认允许
            if (entityStatus == null)
                return true;

            // 获取当前状态类型和值
            var statusType = GetStatusType(entity);
            var statusValue = entityStatus;

            // 获取操作权限规则
            var allowedActions = GetActionPermissionRules(statusType, statusValue);

            // 检查当前状态是否允许执行指定操作
            return allowedActions.Contains(action);
        }

        /// <summary>
        /// 获取操作权限规则
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="statusValue">状态值</param>
        /// <returns>操作权限列表</returns>
        private List<MenuItemEnums> GetActionPermissionRules(Type statusType, object statusValue)
        {
            // 使用GlobalStateRulesManager中定义的规则
            return GlobalStateRulesManager.Instance.GetActionPermissionRules(statusType, statusValue);
        }

        /// <summary>
        /// 获取操作权限规则（基于DataStatus）
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <returns>操作权限列表</returns>
        private List<MenuItemEnums> GetActionPermissionRules(DataStatus status)
        {
            // 使用GlobalStateRulesManager中定义的规则
            return GlobalStateRulesManager.Instance.GetActionPermissionRules(typeof(DataStatus), status);
        }

        /// <summary>
        /// 获取操作成功消息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <returns>成功消息</returns>
        private string GetSuccessMessage(MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                    return "可以修改当前记录";
                case MenuItemEnums.删除:
                    return "可以删除当前记录";
                case MenuItemEnums.保存:
                    return "可以保存当前记录";
                case MenuItemEnums.提交:
                    return "可以提交当前记录";
                case MenuItemEnums.审核:
                    return "可以审核当前记录";
                case MenuItemEnums.反审:
                    return "可以取消审核当前记录";
                case MenuItemEnums.查询:
                    return "可以查看当前记录";
                case MenuItemEnums.打印:
                    return "可以打印当前记录";
                case MenuItemEnums.导出:
                    return "可以导出当前记录";
                default:
                    return "可以执行当前操作";
            }
        }

        /// <summary>
        /// 获取操作失败消息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetFailureMessage(BaseEntity entity, MenuItemEnums action)
        {
            // 获取实体的统一状态
            var entityStatus = GetUnifiedStatus(entity);

            // 如果没有状态信息，返回通用消息
            if (entityStatus == null)
                return $"当前状态下不允许执行{action}操作";

            // 获取当前状态类型和值
            var statusType = GetStatusType(entity);
            var statusValue = entityStatus;

            // 根据状态类型和操作类型返回相应的失败消息
            if (statusType == typeof(DataStatus))
            {
                var dataStatus = entityStatus.dataStatus.Value;
                return GetDataStatusFailureMessage(dataStatus, action);
            }
            else if (statusType == typeof(PaymentStatus))
            {
                var paymentStatus = (PaymentStatus)Enum.ToObject(typeof(PaymentStatus), entityStatus.CurrentStatus);
                return GetPaymentStatusFailureMessage(paymentStatus, action);
            }
            else if (statusType == typeof(PrePaymentStatus))
            {
                var prePaymentStatus = (PrePaymentStatus)Enum.ToObject(typeof(PrePaymentStatus), entityStatus.CurrentStatus);
                return GetPrePaymentStatusFailureMessage(prePaymentStatus, action);
            }
            else if (statusType == typeof(ARAPStatus))
            {
                var arapStatus = (ARAPStatus)Enum.ToObject(typeof(ARAPStatus), entityStatus.CurrentStatus);
                return GetARAPStatusFailureMessage(arapStatus, action);
            }
            else if (statusType == typeof(StatementStatus))
            {
                var statementStatus = (StatementStatus)Enum.ToObject(typeof(StatementStatus), entityStatus.CurrentStatus);
                return GetStatementStatusFailureMessage(statementStatus, action);
            }

            // 默认返回通用消息
            return $"当前状态下不允许执行{action}操作";
        }

        /// <summary>
        /// 获取数据状态操作失败消息
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetDataStatusFailureMessage(DataStatus status, MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                    if (status == DataStatus.完结 || status == DataStatus.作废)
                        return status == DataStatus.完结 ? "已完结的记录不能修改" : "已作废的记录不能修改";
                    break;
                case MenuItemEnums.删除:
                    if (status == DataStatus.确认 || status == DataStatus.完结 || status == DataStatus.作废)
                        return status == DataStatus.确认 ? "已确认的记录不能删除" :
                               status == DataStatus.完结 ? "已完结的记录不能删除" : "已作废的记录不能删除";
                    break;
                case MenuItemEnums.保存:
                    if (status == DataStatus.完结 || status == DataStatus.作废)
                        return status == DataStatus.完结 ? "已完结的记录不能保存" : "已作废的记录不能保存";
                    break;
                case MenuItemEnums.提交:
                    if (status == DataStatus.完结 || status == DataStatus.作废)
                        return status == DataStatus.完结 ? "已完结的记录不能提交" : "已作废的记录不能提交";
                    break;
                case MenuItemEnums.审核:
                    if (status == DataStatus.草稿 || status == DataStatus.新建)
                        return "草稿或新建状态的记录不能审核";
                    break;
                case MenuItemEnums.反审:
                    if (status == DataStatus.草稿 || status == DataStatus.新建)
                        return "草稿或新建状态的记录不能取消审核";
                    if (status == DataStatus.完结 || status == DataStatus.作废)
                        return status == DataStatus.完结 ? "已完结的记录不能取消审核" : "已作废的记录不能取消审核";
                    break;
            }

            return $"当前状态下不允许执行{action}操作";
        }

        /// <summary>
        /// 获取付款状态操作失败消息
        /// </summary>
        /// <param name="status">付款状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetPaymentStatusFailureMessage(PaymentStatus status, MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                case MenuItemEnums.删除:
                case MenuItemEnums.保存:
                case MenuItemEnums.提交:
                    if (status == PaymentStatus.已支付)
                        return "已支付的记录不能执行此操作";
                    break;
                case MenuItemEnums.审核:
                    if (status == PaymentStatus.草稿)
                        return "草稿状态的记录不能审核";
                    if (status == PaymentStatus.已支付)
                        return "已支付的记录不能审核";
                    break;
            }

            return $"当前状态下不允许执行{action}操作";
        }

        /// <summary>
        /// 获取预付款状态操作失败消息
        /// </summary>
        /// <param name="status">预付款状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetPrePaymentStatusFailureMessage(PrePaymentStatus status, MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                case MenuItemEnums.删除:
                case MenuItemEnums.保存:
                case MenuItemEnums.提交:
                    if (status == PrePaymentStatus.已生效 || status == PrePaymentStatus.全额核销 || status == PrePaymentStatus.已结案)
                        return "已生效或已核销的记录不能执行此操作";
                    break;
                case MenuItemEnums.审核:
                    if (status == PrePaymentStatus.草稿)
                        return "草稿状态的记录不能审核";
                    if (status == PrePaymentStatus.已生效 || status == PrePaymentStatus.全额核销 || status == PrePaymentStatus.已结案)
                        return "已生效或已核销的记录不能审核";
                    break;
            }

            return $"当前状态下不允许执行{action}操作";
        }

        /// <summary>
        /// 获取应收应付状态操作失败消息
        /// </summary>
        /// <param name="status">应收应付状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetARAPStatusFailureMessage(ARAPStatus status, MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                case MenuItemEnums.删除:
                case MenuItemEnums.保存:
                case MenuItemEnums.提交:
                    if (status == ARAPStatus.全部支付 || status == ARAPStatus.坏账 || status == ARAPStatus.已冲销)
                        return "已支付或已冲销的记录不能执行此操作";
                    break;
                case MenuItemEnums.审核:
                    if (status == ARAPStatus.草稿)
                        return "草稿状态的记录不能审核";
                    if (status == ARAPStatus.全部支付 || status == ARAPStatus.坏账 || status == ARAPStatus.已冲销)
                        return "已支付或已冲销的记录不能审核";
                    break;
            }

            return $"当前状态下不允许执行{action}操作";
        }

        /// <summary>
        /// 获取对账单状态操作失败消息
        /// </summary>
        /// <param name="status">对账单状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetStatementStatusFailureMessage(StatementStatus status, MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                case MenuItemEnums.删除:
                case MenuItemEnums.保存:
                case MenuItemEnums.提交:
                    if (status == StatementStatus.已确认 || status == StatementStatus.已作废 || status == StatementStatus.已结清
                        || status == StatementStatus.部分结算)
                        return "已确认或已结案的记录不能执行此操作";
                    break;
                case MenuItemEnums.审核:
                    if (status == StatementStatus.已发送)
                        return "未确认状态的记录不能审核";
                    if (status == StatementStatus.已结清)
                        return "已结案的记录不能审核";
                    break;
            }

            return $"当前状态下不允许执行{action}操作";
        }

        /// <summary>
        /// 获取修改操作失败消息
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>失败消息</returns>
        private string GetModifyFailureMessage(DataStatus? currentStatus)
        {
            return GetDataStatusFailureMessage(currentStatus.Value, MenuItemEnums.修改);
        }

        /// <summary>
        /// 获取删除操作失败消息
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>失败消息</returns>
        private string GetDeleteFailureMessage(DataStatus? currentStatus)
        {
            return GetDataStatusFailureMessage(currentStatus.Value, MenuItemEnums.删除);
        }

        /// <summary>
        /// 获取保存操作失败消息
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>失败消息</returns>
        private string GetSaveFailureMessage(DataStatus? currentStatus)
        {
            return GetDataStatusFailureMessage(currentStatus.Value, MenuItemEnums.保存);
        }

        /// <summary>
        /// 获取提交操作失败消息
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>失败消息</returns>
        private string GetSubmitFailureMessage(DataStatus? currentStatus)
        {
            return GetDataStatusFailureMessage(currentStatus.Value, MenuItemEnums.提交);
        }

        /// <summary>
        /// 获取审核操作失败消息
        /// </summary>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>失败消息</returns>
        private string GetApproveFailureMessage(DataStatus? currentStatus)
        {
            return GetDataStatusFailureMessage(currentStatus.Value, MenuItemEnums.审核);
        }



        /// <summary>
        /// 检查业务特定规则
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否满足业务规则</returns>
        private bool CheckBusinessSpecificRules(BaseEntity entity, MenuItemEnums action)
        {
            // 这里可以根据不同的业务实体类型实现特定的业务规则
            // 默认返回true，表示满足业务规则
            return true;
        }

        #endregion

        #region UI控件影响方法

        /// <summary>
        /// 获取实体当前状态对应的UI控件状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>UI控件状态字典</returns>
        public Dictionary<string, (bool Enabled, bool Visible)> GetUIControlStates(BaseEntity entity)
        {
            if (entity == null)
                return new Dictionary<string, (bool Enabled, bool Visible)>();

            // 获取实体的数据状态和操作状态
            var dataStatus = GetBusinessStatus(entity);
            var statusType = GetStatusType(entity);
            var actionStatus = entity.ActionStatus;

            // 创建临时EntityStatus对象用于UI控件规则判断
            var tempEntityStatus = new EntityStatus();

            tempEntityStatus.SetBusinessStatus(statusType, dataStatus);

            tempEntityStatus.actionStatus = actionStatus;

            return GlobalStateRulesManager.Instance.GetButtonRules(tempEntityStatus);
        }

        /// <summary>
        /// 获取特定操作按钮的状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮状态</returns>
        public (bool Enabled, bool Visible) GetButtonState(BaseEntity entity, string buttonName)
        {
            var uiStates = GetUIControlStates(entity);

            if (uiStates.TryGetValue(buttonName, out var state))
            {
                return state;
            }

            // 默认状态：启用且可见
            return (true, true);
        }





        /// <summary>
        /// 检查操作是否会影响UI控件状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>UI控件变化信息</returns>
        public (bool WillChange, Dictionary<string, (bool OldEnabled, bool OldVisible, bool NewEnabled, bool NewVisible)> Changes)
            GetUIControlChanges(BaseEntity entity, MenuItemEnums action, object targetStatus = null)
        {
            if (entity == null)
                return (false, new Dictionary<string, (bool, bool, bool, bool)>());

            // 获取当前UI控件状态
            var currentStates = GetUIControlStates(entity);

            BaseEntity tempEntity = entity.Clone() as BaseEntity;
            // 如果有额外的目标状态，也应用它
            if (targetStatus != null)
            {
                // 处理其他枚举类型状态
                SetBusinessStatusAsync(tempEntity, targetStatus.GetType(), targetStatus).Wait();
            }

            // 获取变化后的UI控件状态
            var newStates = GetUIControlStates(tempEntity);

            // 比较变化
            var changes = new Dictionary<string, (bool OldEnabled, bool OldVisible, bool NewEnabled, bool NewVisible)>();
            var hasChanges = false;

            // 检查所有当前状态的按钮
            foreach (var kvp in currentStates)
            {
                var buttonName = kvp.Key;
                var oldState = kvp.Value;

                if (newStates.TryGetValue(buttonName, out var newState))
                {
                    if (oldState.Enabled != newState.Enabled || oldState.Visible != newState.Visible)
                    {
                        changes[buttonName] = (oldState.Enabled, oldState.Visible, newState.Enabled, newState.Visible);
                        hasChanges = true;
                    }
                }
            }

            // 检查新增的按钮
            foreach (var kvp in newStates)
            {
                if (!currentStates.ContainsKey(kvp.Key))
                {
                    changes[kvp.Key] = (true, true, kvp.Value.Enabled, kvp.Value.Visible);
                    hasChanges = true;
                }
            }

            return (hasChanges, changes);
        }

        #endregion

        #region 辅助方法



        /// <summary>
        /// 获取实体状态类型
        /// 根据实体包含的属性判断使用哪种状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>状态类型</returns>
        public Type GetBusinessStatusType(BaseEntity entity)
        {
            // 检查实体是否包含DataStatus属性
            if (entity.ContainsProperty(typeof(DataStatus).Name))
                return typeof(DataStatus);

            // 检查实体是否包含PrePaymentStatus属性
            if (entity.ContainsProperty(typeof(PrePaymentStatus).Name))
                return typeof(PrePaymentStatus);

            // 检查实体是否包含ARAPStatus属性
            if (entity.ContainsProperty(typeof(ARAPStatus).Name))
                return typeof(ARAPStatus);

            // 检查实体是否包含PaymentStatus属性
            if (entity.ContainsProperty(typeof(PaymentStatus).Name))
                return typeof(PaymentStatus);

            // 检查实体是否包含StatementStatus属性
            if (entity.ContainsProperty(typeof(StatementStatus).Name))
                return typeof(StatementStatus);

            return null;
        }
        #endregion

        #region IDisposable实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 清除事件订阅
            StatusChanged = null;

            // 清除缓存
        }

        #endregion
    }
}