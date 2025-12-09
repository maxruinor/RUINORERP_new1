/**
 * 文件: StatusTransitionEngine.cs
 * 版本: V3 - 状态转换引擎实现（原始架构）
 * 说明: V3原始架构的状态转换引擎实现，负责处理状态转换的核心逻辑
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 原始复杂架构，8个接口之一，工厂模式管理
 * V3架构: 在V4中被合并到UnifiedStateManager类中
 * 
 * 迁移指南：
 * 此类已过时，请使用UnifiedStateManager类替代。
 * 
 * 迁移步骤：
 * 1. 将IStatusTransitionEngine的依赖注入替换为IUnifiedStateManager
 * 2. 将ExecuteTransitionAsync调用替换为UnifiedStateManager的相应方法
 * 3. 将ValidateTransitionAsync调用替换为UnifiedStateManager的相应方法
 * 4. 将GetAvailableTransitions调用替换为UnifiedStateManager的相应方法
 * 
 * 示例代码：
 * // 旧代码
 * var result = await _transitionEngine.ExecuteTransitionAsync(fromStatus, toStatus, context);
 * 
 * // 新代码
 * var result = await _statusManager.ValidateDataStatusTransitionAsync(entity, fromStatus, toStatus);
 * if (result.Success)
 * {
 *     await _statusManager.SetDataStatusAsync(entity, toStatus);
 * }
 * 
 * 注意：此类计划在下个主要版本中移除，请尽快迁移到新的状态管理系统。
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 负责处理状态转换的核心逻辑 - V3增强版
    /// 借鉴V4验证器模式，集成轻量级规则配置中心，保持简洁高效
    /// </summary>
    [Obsolete("此类已过时，请使用UnifiedStateManager类替代。此类计划在下个主要版本中移除。请参考文件头部的迁移指南进行代码迁移。", false)]
    internal class StatusTransitionEngine : IStatusTransitionEngine
    {
        #region 字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StatusTransitionEngine> _logger;

        /// <summary>
        /// 转换规则字典 - 保持V3简洁架构
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;



        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态转换引擎 - V3增强版
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
        public async Task<StateTransitionResult> ExecuteTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : struct, Enum
        {
            try
            {
                return !IsTransitionAllowed(fromStatus, toStatus)
                    ? StateTransitionResult.Failure($"不允许从 {fromStatus} 转换到 {toStatus}")
                    : await ExecuteDefaultTransitionAsync(fromStatus, toStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行状态转换失败：从 {FromStatus} 转换到 {ToStatus}", fromStatus, toStatus);
                return StateTransitionResult.Failure($"执行状态转换失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证状态转换 - V3增强版：使用StateTransitionRules验证
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : struct, Enum
        {
            try
            {
                // 使用StateTransitionRules进行基础转换规则验证
                var allowed = IsTransitionAllowed(fromStatus, toStatus);
                if (allowed)
                {
                    _logger?.LogInformation("基础规则验证通过: 从 {FromStatus} 到 {ToStatus}", fromStatus, toStatus);
                }
                else
                {
                    _logger?.LogWarning("基础规则验证失败: 从 {FromStatus} 到 {ToStatus}", fromStatus, toStatus);
                }
                return allowed
                    ? StateTransitionResult.Success(message: $"验证通过：可以从 {fromStatus} 转换到 {toStatus}")
                    : StateTransitionResult.Failure($"不允许从 {fromStatus} 转换到 {toStatus}");
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
        public IEnumerable<T> GetAvailableTransitions<T>(T currentStatus, IStatusTransitionContext context) where T : struct, Enum
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
        private bool IsTransitionAllowed<T>(T fromStatus, T toStatus) where T : struct, Enum
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
        private async Task<StateTransitionResult> ExecuteDefaultTransitionAsync<T>(T fromStatus, T toStatus, IStatusTransitionContext context) where T : struct, Enum
        {
            // 默认转换逻辑
            await Task.CompletedTask;
            return StateTransitionResult.Success(message: $"成功从 {fromStatus} 转换到 {toStatus}");
        }

        #endregion
    }
}