/**
 * 文件: IUnifiedStateManager.cs
 * 版本: V6 - 完全基于实现的接口定义
 * 说明: 完全基于UnifiedStateManager实现类的接口定义，仅包含已实现的方法
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V6: 完全基于UnifiedStateManager实现类的接口定义，仅包含已实现的方法
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态管理器接口
    /// </summary>
    public interface IUnifiedStateManager
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

        void TriggerStatusChangedEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null);

        /// <summary>
        /// 获取实体的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态值</returns>
        object GetBusinessStatus<T>(BaseEntity entity) where T : struct, Enum;

        /// <summary>
        /// 获取实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>业务状态值</returns>
        object GetBusinessStatus(BaseEntity entity, Type statusType);

        /// <summary>
        /// 获取实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态值</returns>
        object GetBusinessStatus(BaseEntity entity);

        /// <summary>
        /// 验证数据状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        StateTransitionResult ValidateBusinessStatusTransitionAsync(Enum fromStatus, Enum toStatus);

        /// <summary>
        /// 验证业务状态转换是否合法
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        StateTransitionResult ValidateBusinessStatusTransitionAsync<T>(T? fromStatus, T? toStatus) where T : struct, Enum;

        /// <summary>
        /// 验证操作状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        StateTransitionResult ValidateActionStatusTransitionAsync(ActionStatus? fromStatus, ActionStatus? toStatus);

        /// <summary>
        /// 检查是否可以转换到指定的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        bool CanTransitionToBusinessStatus(BaseEntity entity, DataStatus targetStatus);

        /// <summary>
        /// 检查是否可以转换到指定的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        bool CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : struct, Enum;

        /// <summary>
        /// 检查是否可以转换到指定的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        bool CanTransitionToActionStatus(BaseEntity entity, ActionStatus targetStatus);

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
        /// <param name="status">业务状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        Task<StateTransitionResult> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null, string userId = null);

        /// <summary>
        /// 设置实体的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
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
        Dictionary<string, (bool Enabled, bool Visible)> GetUIControlStates(BaseEntity entity);

        /// <summary>
        /// 获取特定操作按钮的状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮状态</returns>
        (bool Enabled, bool Visible) GetButtonState(BaseEntity entity, string buttonName);

        /// <summary>
        /// 检查操作是否会影响UI控件状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>UI控件变化信息</returns>
        (bool WillChange, Dictionary<string, (bool OldEnabled, bool OldVisible, bool NewEnabled, bool NewVisible)> Changes)
            GetUIControlChanges(BaseEntity entity, MenuItemEnums action, object targetStatus = null);

        /// <summary>
        /// 获取实体状态类型
        /// 根据实体包含的属性判断使用哪种状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>状态类型</returns>
        Type GetBusinessStatusType(BaseEntity entity);

        /// <summary>
        /// 状态转换事件
        /// </summary>
        event EventHandler<StateTransitionEventArgs> StatusChanged;
    }
}