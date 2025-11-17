/**
 * 文件: IStatusTransitionContext.cs
 * 说明: 状态转换上下文接口 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换上下文接口 - v3版本
    /// 提供状态转换过程中的上下文信息
    /// </summary>
    public interface IStatusTransitionContext
    {
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
        /// 状态变更事件
        /// </summary>
        event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 获取数据性状态
        /// </summary>
        /// <returns>数据性状态</returns>
        DataStatus GetDataStatus();

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>业务性状态</returns>
        T GetBusinessStatus<T>() where T : Enum;

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <returns>业务性状态</returns>
        object GetBusinessStatus(Type statusType);

        /// <summary>
        /// 获取操作状态
        /// </summary>
        /// <returns>操作状态</returns>
        ActionStatus GetActionStatus();

        /// <summary>
        /// 设置数据性状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetDataStatusAsync(DataStatus status, string reason = null);

        /// <summary>
        /// 设置业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetBusinessStatusAsync<T>(T status, string reason = null) where T : Enum;

        /// <summary>
        /// 设置操作状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        Task<bool> SetActionStatusAsync(ActionStatus status, string reason = null);

        /// <summary>
        /// 获取转换历史
        /// </summary>
        /// <returns>转换历史记录</returns>
        IEnumerable<IStatusTransitionRecord> GetTransitionHistory();

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
    }

    /// <summary>
    /// 状态转换记录接口
    /// </summary>
    public interface IStatusTransitionRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 实体类型
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// 实体ID
        /// </summary>
        long EntityId { get; }

        /// <summary>
        /// 状态类型
        /// </summary>
        Type StatusType { get; }

        /// <summary>
        /// 源状态
        /// </summary>
        object FromStatus { get; }

        /// <summary>
        /// 目标状态
        /// </summary>
        object ToStatus { get; }

        /// <summary>
        /// 转换时间
        /// </summary>
        DateTime TransitionTime { get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 转换原因
        /// </summary>
        string Reason { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        Dictionary<string, object> AdditionalData { get; }
    }
}