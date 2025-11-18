/**
 * 文件: IStatusTransitionEngine.cs
 * 说明: 状态转换引擎接口 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换引擎接口 - v3版本
    /// 负责处理状态转换的核心逻辑
    /// </summary>
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
        Task<StateTransitionResult> ExecuteTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : Enum;

        /// <summary>
        /// 获取可转换的状态列表
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>可转换的状态列表</returns>
        IEnumerable<T> GetAvailableTransitions<T>(T currentStatus, IStatusTransitionContext context) where T : Enum;

        /// <summary>
        /// 获取可转换的状态列表（异步版本）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>可转换的状态列表</returns>
        Task<IEnumerable<T>> GetAvailableTransitionsAsync<T>(T currentStatus, IStatusTransitionContext context) where T : Enum;

        /// <summary>
        /// 注册自定义转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="validator">验证器</param>
        void RegisterTransitionRule<T>(T fromStatus, T toStatus, Func<IStatusTransitionContext, Task<bool>> validator) where T : Enum;

        /// <summary>
        /// 注册转换执行器
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="executor">执行器</param>
        void RegisterTransitionExecutor<T>(T fromStatus, T toStatus, Func<IStatusTransitionContext, Task<StateTransitionResult>> executor) where T : Enum;
    }
}