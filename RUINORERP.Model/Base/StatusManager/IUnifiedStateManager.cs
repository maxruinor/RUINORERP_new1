/**
 * 文件: IUnifiedStateManager.cs
 * 版本: V4 - 优化版统一状态管理器接口
 * 说明: 统一状态管理器接口 - 基于V4版本架构优化，简化方法定义，提升性能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本优化，简化接口定义
 */

using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态管理器接口
    /// 基于V4版本架构优化，简化日志记录，提升性能
    /// 支持MenuItemEnums，实现操作权限检查和UI控件影响
    /// </summary>
    public interface IUnifiedStateManager : IDisposable
    {
        /// <summary>
        /// 获取实体的状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>状态类型</returns>
        Type GetStatusType(BaseEntity entity);

        /// <summary>
        /// 获取实体的业务状态（整合版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型（可选）</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型（可选）</param>
        /// <returns>业务状态值</returns>
        object GetBusinessStatus<T>(BaseEntity entity, Type statusType = null) where T : struct, Enum;

        /// <summary>
        /// 获取实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型（可选）</param>
        /// <returns>业务状态值</returns>
        Enum GetBusinessStatus(BaseEntity entity, Type statusType = null);

        /// <summary>
        /// 验证数据状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        StateTransitionResult ValidateBusinessStatusTransitionAsync(Enum fromStatus, Enum toStatus);

        /// <summary>
        /// 验证业务状态转换是否合法（泛型版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        StateTransitionResult ValidateBusinessStatusTransitionAsync<T>(T? fromStatus, T? toStatus) where T : struct, Enum;

        /// <summary>
        /// 验证操作状态转换
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        Task<StateTransitionResult> ValidateActionStatusTransitionAsync(ActionStatus fromStatus, ActionStatus? toStatus);

        /// <summary>
        /// 设置实体的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">业务状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        Task<StateTransitionResult> SetBusinessStatusAsync<T>(BaseEntity entity, T? status, string reason = null, string userId = null) where T : struct, Enum;

        /// <summary>
        /// 设置实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态值</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        Task<StateTransitionResult> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null, string userId = null);

        /// <summary>
        /// 设置实体的操作状态（ActionStatus）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">目标操作状态</param>
        /// <param name="reason">状态变更原因</param>
        /// <param name="userId">执行操作的用户ID</param>
        /// <returns>状态转换结果对象</returns>
        Task<StateTransitionResult> SetActionStatusAsync(BaseEntity entity, ActionStatus? status, string reason = null, string userId = null);

        /// <summary>
        /// 检查是否可以执行指定操作，并返回详细消息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>操作结果</returns>
        (bool CanExecute, string Message) CanExecuteActionWithMessage(BaseEntity entity, MenuItemEnums action);

        /// <summary>
        /// 获取实体当前状态对应的UI控件状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>UI控件状态字典</returns>
        Dictionary<string,  bool > GetUIControlStates(BaseEntity entity);

        /// <summary>
        /// 获取特定操作按钮的状态Enabled
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮状态</returns>
        bool GetButtonState(BaseEntity entity, string buttonName);

        /// <summary>
        /// 状态转换事件
        /// </summary>
        event EventHandler<StateTransitionEventArgs> StatusChanged;

 
        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        void TriggerStatusChangedEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null);

        /// <summary>
        /// 判断指定实体是否可以修改，并返回详细消息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以修改及详细消息</returns>
        (bool CanModify, string Message) CanModifyWithMessage<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        /// 判断指定实体的业务状态是否为终态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否为终态</returns>
        bool IsFinalStatus<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        /// 通过UI操作类型设置实体状态（通用方法）
        /// 支持提交、审核、反审等所有预定义的UI操作
        /// 这是一个简化的通用接口，将UI操作自动映射到具体的状态值
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型（提交、审核、反审等）</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        Task<StateTransitionResult> SetStatusByActionAsync(BaseEntity entity, MenuItemEnums action, string reason = null, string userId = null);

        /// <summary>
        /// 通过UI操作类型验证实体状态转换是否合法
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateActionTransitionAsync(BaseEntity entity, MenuItemEnums action);

        /// <summary>
        /// 检查是否需要关键操作二次确认
        /// 用于对关键操作（如删除已审核单据、作废单据等）进行二次确认
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">当前状态</param>
        /// <param name="operationType">操作类型（如"delete", "cancel", "reverseReview"等）</param>
        /// <returns>是否需要二次确认</returns>
        bool NeedConfirmationForCriticalOperation<T>(T status, string operationType) where T : struct;

        /// <summary>
        /// 获取关键操作确认提示信息
        /// 用于UI层调用以显示适当的二次确认对话框
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">当前状态</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>确认提示信息文本</returns>
        string GetCriticalOperationConfirmationMessage<T>(T status, string operationType) where T : struct;

        /// <summary>
        /// 记录状态变更操作日志
        /// 用于记录所有关键状态变更，便于审计追踪
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="entityId">实体ID</param>
        /// <param name="entityType">实体类型名称</param>
        /// <param name="fromStatus">原始状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="operatorId">操作用户ID</param>
        /// <param name="operatorName">操作用户名称</param>
        /// <param name="remarks">备注信息</param>
        void LogStatusChangeOperation<T>(long entityId, string entityType, T fromStatus, T toStatus, long operatorId, string operatorName, string remarks = "") where T : struct;

        /// <summary>
        /// 记录关键操作
        /// 用于记录非状态变更的关键操作，如删除、打印等
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <param name="entityType">实体类型名称</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="operatorId">操作用户ID</param>
        /// <param name="operatorName">操作用户名称</param>
        /// <param name="remarks">备注信息</param>
        void LogCriticalOperation(long entityId, string entityType, string operationType, long operatorId, string operatorName, string remarks = "");

        /// <summary>
        /// 获取状态类型的描述信息
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态类型描述</returns>
        string GetStatusTypeDescription(Type statusType);

        /// <summary>
        /// 检查提交后是否允许修改
        /// 根据全局模式设置和状态判断
        /// </summary>
        /// <param name="isSubmittedStatus">是否为已提交状态</param>
        /// <returns>是否允许修改</returns>
        bool AllowModifyAfterSubmit(bool isSubmittedStatus);


    }
}


