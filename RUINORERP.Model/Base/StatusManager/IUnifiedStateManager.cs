/**
 * 文件: IUnifiedStateManager.cs
 * 版本: V4 - 优化版统一状态管理器接口
 * 说明: 统一状态管理器接口 - 基于V4版本架构优化，简化方法定义，提升性能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本优化，简化接口定义
 */

using RUINORERP.Global;
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
        /// 获取实体的统一状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>统一状态</returns>
        EntityStatus GetUnifiedStatus(BaseEntity entity);

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
        object GetBusinessStatus(BaseEntity entity, Type statusType = null);

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
        /// 获取业务状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态类型</returns>
        Type GetBusinessStatusType(BaseEntity entity);

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
        /// 获取UI控件变更
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>UI控件变更</returns>
        Dictionary<string, bool> GetUIControlChanges(BaseEntity entity, MenuItemEnums action);

        /// <summary>
        /// 判断指定实体的业务状态是否为终态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否为终态</returns>
        bool IsFinalStatus<TEntity>(TEntity entity) where TEntity : BaseEntity;

        /// <summary>
        /// 判断指定实体是否可以修改
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以修改</returns>
        bool CanModify<TEntity>(TEntity entity) where TEntity : BaseEntity;
    }
}
