/**
 * 文件: IUnifiedStateManager.cs
 * 版本: V4 - 简化版统一状态管理器接口
 * 说明: 简化版统一状态管理器接口，提供核心的状态管理功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V4: 简化版架构，移除冗余方法和复杂逻辑
 * V4架构: 核心状态管理功能，提高可维护性
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 简化版统一状态管理器接口
    /// </summary>
    public interface IUnifiedStateManager
    {
        /// <summary>
        /// 获取实体的状态信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体状态信息</returns>
        EntityStatus GetEntityStatus(BaseEntity entity);

        /// <summary>
        /// 获取当前数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前数据状态</returns>
        DataStatus GetDataStatus(BaseEntity entity);

        /// <summary>
        /// 设置数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetDataStatusAsync(BaseEntity entity, DataStatus status, string reason = null);

        /// <summary>
        /// 设置实体数据状态（同步版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        void SetEntityDataStatus(BaseEntity entity, DataStatus status);

        /// <summary>
        /// 获取业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态</returns>
        T GetBusinessStatus<T>(BaseEntity entity) where T : struct, Enum;

        /// <summary>
        /// 获取业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态</returns>
        object GetBusinessStatus(BaseEntity entity);

        /// <summary>
        /// 设置业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetBusinessStatusAsync<T>(BaseEntity entity, T status, string reason = null) where T : struct, Enum;

        /// <summary>
        /// 设置实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null);

        /// <summary>
        /// 获取当前操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前操作状态</returns>
        ActionStatus GetActionStatus(BaseEntity entity);

        /// <summary>
        /// 设置操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetActionStatusAsync(BaseEntity entity, ActionStatus status, string reason = null);

        /// <summary>
        /// 验证数据状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateDataStatusTransitionAsync(BaseEntity entity, DataStatus targetStatus);

        /// <summary>
        /// 验证业务状态转换
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : struct, Enum;

        /// <summary>
        /// 验证业务状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(BaseEntity entity, Type statusType, object targetStatus);

        /// <summary>
        /// 验证操作状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateActionStatusTransitionAsync(BaseEntity entity, ActionStatus targetStatus);

        /// <summary>
        /// 获取可转换的数据状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<DataStatus> GetAvailableDataStatusTransitions(BaseEntity entity);

        /// <summary>
        /// 获取可转换的业务状态列表
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<T> GetAvailableBusinessStatusTransitions<T>(BaseEntity entity) where T : struct, Enum;

        /// <summary>
        /// 获取可转换的业务状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<object> GetAvailableBusinessStatusTransitions(BaseEntity entity, Type statusType);

        /// <summary>
        /// 获取可转换的操作状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<ActionStatus> GetAvailableActionStatusTransitions(BaseEntity entity);

        /// <summary>
        /// 检查是否可以转换到目标数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionToDataStatus(BaseEntity entity, DataStatus targetStatus);

        /// <summary>
        /// 检查是否可以转换到目标业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : struct, Enum;

        /// <summary>
        /// 检查是否可以转换到目标操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionToActionStatus(BaseEntity entity, ActionStatus targetStatus);

        /// <summary>
        /// 状态变更事件
        /// </summary>
        event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 创建数据状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateDataStatusContext(object entity, DataStatus currentStatus, IServiceProvider serviceProvider, string reason = null);

        /// <summary>
        /// 创建业务状态转换上下文
        /// </summary>
        /// <typeparam name="TBusinessStatus">业务状态类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateBusinessStatusContext<TBusinessStatus>(object entity, TBusinessStatus currentStatus, IServiceProvider serviceProvider, string reason = null);

        /// <summary>
        /// 创建操作状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateActionStatusContext(object entity, ActionStatus currentStatus, IServiceProvider serviceProvider, string reason = null);

        /// <summary>
        /// 创建UI更新状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateUIUpdateContext(object entity, DataStatus currentStatus, IServiceProvider serviceProvider, string reason = null);

        /// <summary>
        /// 创建通用状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        IStatusTransitionContext CreateContext(object entity, Type statusType, object currentStatus, IServiceProvider serviceProvider, string reason = null);

        /// <summary>
        /// 清理状态缓存
        /// </summary>
        void ClearCache();

        /// <summary>
        /// 检查是否可以执行指定操作
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <returns>是否可以执行操作</returns>
        bool CanExecuteAction(MenuItemEnums action, BaseEntity entity, Type statusType, object status);

        /// <summary>
        /// 检查是否可以执行指定操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否可以执行</returns>
        bool CanExecuteAction<TEntity>(TEntity entity, MenuItemEnums action) where TEntity : class;

        /// <summary>
        /// 检查是否可以执行指定操作（带详细消息）
        /// </summary>
        /// <param name="stateManager">状态管理器</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>包含操作权限和友好提示消息的结果</returns>
        StateTransitionResult CanExecuteActionWithMessage(IUnifiedStateManager stateManager, BaseEntity entity,MenuItemEnums action);

        /// <summary>
        /// 获取可用的操作列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <returns>可执行的操作列表</returns>
        IEnumerable<MenuItemEnums> GetAvailableActions(BaseEntity entity, Type statusType, object status);

        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        void TriggerStatusChangedEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null);
    }
}