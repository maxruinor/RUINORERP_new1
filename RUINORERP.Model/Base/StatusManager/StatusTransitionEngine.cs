/**
 * 文件: StatusTransitionEngine.cs
 * 说明: 简化版状态转换引擎 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 简化版状态转换引擎 - v3版本
    /// 负责处理状态转换的核心逻辑
    /// 移除了复杂的自定义验证器和执行器，简化了状态转换逻辑
    /// </summary>
    public class StatusTransitionEngine : IStatusTransitionEngine
    {
        #region 字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StatusTransitionEngine> _logger;

        /// <summary>
        /// 转换规则字典
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态转换引擎
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public StatusTransitionEngine(ILogger<StatusTransitionEngine> logger = null)
        {
            _logger = logger;
            _transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();

            // 初始化默认规则
            InitializeDefaultRules();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 执行状态转换（包含验证和执行）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>转换结果</returns>
        public async Task<StateTransitionResult> ExecuteTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : Enum
        {
            try
            {
                // 检查基本规则
                if (!IsTransitionAllowed(fromStatus, toStatus))
                {
                    return StateTransitionResult.Failure($"不允许从 {fromStatus} 转换到 {toStatus}");
                }

                // 默认执行逻辑
                return await ExecuteDefaultTransitionAsync(fromStatus, toStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行状态转换失败：从 {FromStatus} 转换到 {ToStatus}", fromStatus, toStatus);
                return StateTransitionResult.Failure($"执行状态转换失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证状态转换
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : Enum
        {
            try
            {
                // 检查基本规则
                if (!IsTransitionAllowed(fromStatus, toStatus))
                {
                    return StateTransitionResult.Failure($"不允许从 {fromStatus} 转换到 {toStatus}");
                }

                // 默认验证逻辑
                await Task.CompletedTask;
                return StateTransitionResult.Success(message: $"验证通过：可以从 {fromStatus} 转换到 {toStatus}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证状态转换失败：从 {FromStatus} 转换到 {ToStatus}", fromStatus, toStatus);
                return StateTransitionResult.Failure($"验证状态转换失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取可转换的状态列表
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<T> GetAvailableTransitions<T>(T currentStatus, IStatusTransitionContext context) where T : Enum
        {
            try
            {
                var statusType = typeof(T);
                if (!_transitionRules.ContainsKey(statusType) || !_transitionRules[statusType].ContainsKey(currentStatus))
                {
                    return Enumerable.Empty<T>();
                }

                return _transitionRules[statusType][currentStatus]
                    .Where(s => s is T)
                    .Cast<T>()
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换状态列表失败：当前状态 {CurrentStatus}", currentStatus);
                return Enumerable.Empty<T>();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化默认规则
        /// </summary>
        private void InitializeDefaultRules()
        {
            try
            {
                // 使用StateTransitionRules初始化默认规则
                StateTransitionRules.InitializeDefaultRules(_transitionRules);
                _logger?.LogInformation("状态转换规则初始化完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化状态转换规则失败");
                throw;
            }
        }

        /// <summary>
        /// 检查状态转换是否被允许
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否允许转换</returns>
        private bool IsTransitionAllowed<T>(T fromStatus, T toStatus) where T : Enum
        {
            return StateTransitionRules.IsTransitionAllowed(_transitionRules, fromStatus, toStatus);
        }

        /// <summary>
        /// 执行默认转换逻辑
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>转换结果</returns>
        private async Task<StateTransitionResult> ExecuteDefaultTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : Enum
        {
            // 默认转换逻辑
            await Task.CompletedTask;
            return StateTransitionResult.Success(message: $"成功从 {fromStatus} 转换到 {toStatus}");
        }

        #endregion
    }
}