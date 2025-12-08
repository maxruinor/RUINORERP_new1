/**
 * 文件: IStatusTransitionEngine.cs
 * 版本: V3 - 状态转换引擎接口（原始架构）
 * 说明: V3原始架构的状态转换引擎接口，负责处理状态转换的核心逻辑
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 原始复杂架构，8个接口之一，工厂模式管理
 * V3架构: 在V4中被合并到IStateManagerSimple接口中
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换引擎接口 - v3版本
    /// 负责处理状态转换的核心逻辑
    /// </summary>
    [Obsolete("此接口已过时，请使用IUnifiedStateManager接口替代。此接口将在未来版本中移除。", false)]
    public interface IStatusTransitionEngine
    {
        /// <summary>
        /// 执行状态转换（包含验证和执行）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>转换结果</returns>
        Task<StateTransitionResult> ExecuteTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : struct, Enum;

        /// <summary>
        /// 验证状态转换
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>验证结果</returns>
        Task<StateTransitionResult> ValidateTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : struct, Enum;

        /// <summary>
        /// 获取可转换的状态列表
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<T> GetAvailableTransitions<T>(T currentStatus, IStatusTransitionContext context) where T : struct, Enum;
    }
}