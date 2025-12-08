/**
 * 文件: IStatusTransitionContext.cs
 * 版本: V3 - 状态转换上下文接口（原始架构）
 * 说明: V3原始架构的状态转换上下文接口，提供状态转换过程中的上下文信息
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 原始复杂架构，8个接口之一，用于状态转换上下文管理
 * V3架构: 在V4中被简化合并，减少上下文复杂性
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换上下文接口 - v3版本
    /// 提供状态转换过程中的上下文信息
    /// </summary>
    [Obsolete("此接口已过时，请使用IUnifiedStateManager接口替代。此接口将在未来版本中移除。", false)]
    public interface IStatusTransitionContext
    {
        /// <summary>
        /// 状态变化事件
        /// </summary>
        event EventHandler<StateTransitionEventArgs> StatusChanged;
        /// <summary>
        /// 实体对象
        /// </summary>
        BaseEntity Entity { get; }

        /// <summary>
        /// 状态类型
        /// </summary>
        Type StatusType { get; }

        /// <summary>
        /// 当前状态
        /// </summary>
        object CurrentStatus { get; }

        /// <summary>
        /// 转换原因
        /// </summary>
        string Reason { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        string UserId { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        DateTime TransitionTime { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        Dictionary<string, object> AdditionalData { get; }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <returns>当前状态</returns>
        T GetCurrentStatus<T>() where T : struct, Enum;

        /// <summary>
        /// 获取数据性状态
        /// </summary>
        /// <returns>数据性状态</returns>
        DataStatus GetDataStatus();

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <returns>业务性状态</returns>
        object GetBusinessStatus(Type statusType);

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>业务性状态</returns>
        T GetBusinessStatus<T>() where T : struct, Enum;

        /// <summary>
        /// 获取操作状态
        /// </summary>
        /// <returns>操作状态</returns>
        ActionStatus GetActionStatus();

        /// <summary>
        /// 记录转换
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        void LogTransition(object fromStatus, object toStatus, string reason = null);

        /// <summary>
        /// 转换到指定状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        Task<StateTransitionResult> TransitionTo(object targetStatus, string reason = null);

        /// <summary>
        /// 获取可转换状态列表
        /// </summary>
        /// <returns>可转换状态列表</returns>
        IEnumerable<object> GetAvailableTransitions();

        /// <summary>
        /// 检查是否可以转换到指定状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        Task<bool> CanTransitionTo(object targetStatus);

        /// <summary>
        /// 检查是否可以转换到指定状态，并返回详细的提示消息
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>包含是否可以转换和提示消息的结果对象</returns>
        Task<StateTransitionResult> CanTransitionToWithMessage(object targetStatus);
    }
}