/**
 * 文件: StatusTransitionEngine.cs
 * 说明: 状态转换引擎实现 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换引擎实现 - v3版本
    /// 负责处理状态转换的核心逻辑
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

        /// <summary>
        /// 自定义验证器字典
        /// </summary>
        private readonly Dictionary<string, Func<IStatusTransitionContext, Task<bool>>> _customValidators;

        /// <summary>
        /// 自定义执行器字典
        /// </summary>
        private readonly Dictionary<string, Func<IStatusTransitionContext, Task<StateTransitionResult>>> _customExecutors;

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
            _customValidators = new Dictionary<string, Func<IStatusTransitionContext, Task<bool>>>();
            _customExecutors = new Dictionary<string, Func<IStatusTransitionContext, Task<StateTransitionResult>>>();

            // 初始化默认规则
            InitializeDefaultRules();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 验证状态转换是否有效
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

                // 检查自定义验证器
                var validatorKey = $"{typeof(T).Name}_{fromStatus}_{toStatus}";
                if (_customValidators.ContainsKey(validatorKey))
                {
                    var validator = _customValidators[validatorKey];
                    var isValid = await validator(context);
                    if (!isValid)
                    {
                        return StateTransitionResult.Failure($"自定义验证失败：从 {fromStatus} 转换到 {toStatus}");
                    }
                }

                return StateTransitionResult.Success(message: $"验证通过：从 {fromStatus} 转换到 {toStatus}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证状态转换失败：从 {FromStatus} 转换到 {ToStatus}", fromStatus, toStatus);
                return StateTransitionResult.Failure($"验证状态转换失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 执行状态转换
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
                // 先验证转换
                var validationResult = await ValidateTransitionAsync(fromStatus, toStatus, context);
                if (!validationResult.IsValid)
                {
                    return validationResult;
                }

                // 检查自定义执行器
                var executorKey = $"{typeof(T).Name}_{fromStatus}_{toStatus}";
                if (_customExecutors.ContainsKey(executorKey))
                {
                    var executor = _customExecutors[executorKey];
                    return await executor(context);
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

                var availableStatuses = _transitionRules[statusType][currentStatus]
                    .Where(s => s is T)
                    .Cast<T>()
                    .ToList();

                // 同步过滤自定义验证器允许的状态
                var filteredStatuses = new List<T>();
                foreach (var status in availableStatuses)
                {
                    var validatorKey = $"{statusType.Name}_{currentStatus}_{status}";
                    if (_customValidators.ContainsKey(validatorKey))
                    {
                        // 对于同步版本，我们只能检查基本规则，不能执行异步验证
                        // 如果有自定义验证器，则跳过该状态
                        continue;
                    }
                    else
                    {
                        filteredStatuses.Add(status);
                    }
                }

                return filteredStatuses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换状态列表失败：当前状态 {CurrentStatus}", currentStatus);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 获取可转换的状态列表（异步版本）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="context">状态转换上下文</param>
        /// <returns>可转换的状态列表</returns>
        public async Task<IEnumerable<T>> GetAvailableTransitionsAsync<T>(T currentStatus, IStatusTransitionContext context) where T : Enum
        {
            try
            {
                var statusType = typeof(T);
                if (!_transitionRules.ContainsKey(statusType) || !_transitionRules[statusType].ContainsKey(currentStatus))
                {
                    return Enumerable.Empty<T>();
                }

                var availableStatuses = _transitionRules[statusType][currentStatus]
                    .Where(s => s is T)
                    .Cast<T>()
                    .ToList();

                // 异步过滤自定义验证器允许的状态
                var filteredStatuses = new List<T>();
                foreach (var status in availableStatuses)
                {
                    var validatorKey = $"{statusType.Name}_{currentStatus}_{status}";
                    if (_customValidators.ContainsKey(validatorKey))
                    {
                        var validator = _customValidators[validatorKey];
                        if (await validator(context))
                        {
                            filteredStatuses.Add(status);
                        }
                    }
                    else
                    {
                        filteredStatuses.Add(status);
                    }
                }

                return filteredStatuses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换状态列表失败：当前状态 {CurrentStatus}", currentStatus);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 注册自定义转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="validator">验证器</param>
        public void RegisterTransitionRule<T>(T fromStatus, T toStatus, Func<IStatusTransitionContext, Task<bool>> validator) where T : Enum
        {
            var validatorKey = $"{typeof(T).Name}_{fromStatus}_{toStatus}";
            _customValidators[validatorKey] = validator;
        }

        /// <summary>
        /// 注册转换执行器
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="executor">执行器</param>
        public void RegisterTransitionExecutor<T>(T fromStatus, T toStatus, Func<IStatusTransitionContext, Task<StateTransitionResult>> executor) where T : Enum
        {
            var executorKey = $"{typeof(T).Name}_{fromStatus}_{toStatus}";
            _customExecutors[executorKey] = executor;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化默认规则
        /// </summary>
        private void InitializeDefaultRules()
        {
            StateTransitionRules.InitializeDefaultRules(_transitionRules);
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