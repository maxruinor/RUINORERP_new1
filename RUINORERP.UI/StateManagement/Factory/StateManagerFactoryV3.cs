/**
 * 文件: StateManagerFactoryV3.cs
 * 说明: 状态管理器工厂V3 - 用于创建和管理状态管理器实例
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.Base.StatusManager.Core;
using RUINORERP.UI.StateManagement.Core;
using RUINORERP.UI.StateManagement.Factory;

namespace RUINORERP.UI.StateManagement
{
    /// <summary>
    /// 状态管理器工厂V3 - 用于创建和管理状态管理器实例
    /// </summary>
    public class StateManagerFactoryV3 : IStateManagerFactoryV3
    {
        #region 私有字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StateManagerFactoryV3> _logger;

        /// <summary>
        /// 状态管理器实例缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, IUnifiedStateManager> _stateManagerCache;

        /// <summary>
        /// 状态转换引擎实例缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, IStatusTransitionEngine> _transitionEngineCache;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态管理器工厂
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public StateManagerFactoryV3(ILogger<StateManagerFactoryV3> logger = null)
        {
            _logger = logger;
            _stateManagerCache = new ConcurrentDictionary<string, IUnifiedStateManager>();
            _transitionEngineCache = new ConcurrentDictionary<string, IStatusTransitionEngine>();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取统一状态管理器
        /// </summary>
        /// <returns>统一状态管理器实例</returns>
        public IUnifiedStateManager GetStateManager()
        {
            return GetStateManager("Default");
        }

        /// <summary>
        /// 获取指定类型的统一状态管理器
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <returns>统一状态管理器实例</returns>
        public IUnifiedStateManager GetStateManager<T>() where T : Enum
        {
            return GetStateManager("Default");
        }

        /// <summary>
        /// 获取状态转换引擎
        /// </summary>
        /// <returns>状态转换引擎实例</returns>
        public IStatusTransitionEngine GetTransitionEngine()
        {
            return GetTransitionEngine("Default");
        }

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态转换上下文</returns>
        public IStatusTransitionContext CreateTransitionContext(object entity, Type statusType)
        {
            return CreateTransitionContext((BaseEntity)entity, statusType, null);
        }

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>状态转换上下文</returns>
        public IStatusTransitionContext CreateTransitionContext<T>(object entity) where T : Enum
        {
            return CreateTransitionContext((BaseEntity)entity, typeof(T), null);
        }

        /// <summary>
        /// 注册自定义状态管理器
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="factory">状态管理器工厂方法</param>
        public void RegisterStateManager(Type entityType, Func<IUnifiedStateManager> factory)
        {
            var name = entityType.Name;
            var stateManager = factory();
            RegisterStateManager(name, stateManager);
        }

        /// <summary>
        /// 注册自定义状态管理器
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="factory">状态管理器工厂方法</param>
        public void RegisterStateManager<TEntity>(Func<IUnifiedStateManager> factory)
        {
            RegisterStateManager(typeof(TEntity), factory);
        }

        /// <summary>
        /// 注册自定义状态转换引擎
        /// </summary>
        /// <param name="factory">状态转换引擎工厂方法</param>
        public void RegisterTransitionEngine(Func<IStatusTransitionEngine> factory)
        {
            var transitionEngine = factory();
            RegisterTransitionEngine("Custom", transitionEngine);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _stateManagerCache.Clear();
            _transitionEngineCache.Clear();
        }

        /// <summary>
        /// 获取命名状态管理器实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>状态管理器实例</returns>
        public IUnifiedStateManager GetStateManager(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return _stateManagerCache.GetOrAdd(name, CreateStateManager);
        }

        /// <summary>
        /// 获取默认状态转换引擎实例
        /// </summary>
        /// <returns>状态转换引擎实例</returns>
        public IStatusTransitionEngine GetDefaultTransitionEngine()
        {
            return GetTransitionEngine("Default");
        }

        /// <summary>
        /// 获取命名状态转换引擎实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>状态转换引擎实例</returns>
        public IStatusTransitionEngine GetTransitionEngine(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            return _transitionEngineCache.GetOrAdd(name, CreateTransitionEngine);
        }

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="initialStatus">初始状态</param>
        /// <param name="stateManagerName">状态管理器名称</param>
        /// <param name="transitionEngineName">转换引擎名称</param>
        /// <returns>状态转换上下文</returns>
        public IStatusTransitionContext CreateTransitionContext(
            BaseEntity entity,
            Type statusType,
            object initialStatus,
            string stateManagerName = "Default",
            string transitionEngineName = "Default")
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            var stateManager = GetStateManager(stateManagerName);
            var transitionEngine = GetTransitionEngine(transitionEngineName);

            return new StatusTransitionContext(
                entity,
                statusType,
                initialStatus,
                stateManager,
                transitionEngine);
        }

        /// <summary>
        /// 注册自定义状态管理器
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <param name="stateManager">状态管理器实例</param>
        public void RegisterStateManager(string name, IUnifiedStateManager stateManager)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (stateManager == null)
                throw new ArgumentNullException(nameof(stateManager));

            _stateManagerCache.AddOrUpdate(name, stateManager, (key, oldValue) => stateManager);
            _logger?.LogInformation("已注册状态管理器实例: {Name}", name);
        }

        /// <summary>
        /// 注册自定义状态转换引擎
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <param name="transitionEngine">状态转换引擎实例</param>
        public void RegisterTransitionEngine(string name, IStatusTransitionEngine transitionEngine)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (transitionEngine == null)
                throw new ArgumentNullException(nameof(transitionEngine));

            _transitionEngineCache.AddOrUpdate(name, transitionEngine, (key, oldValue) => transitionEngine);
            _logger?.LogInformation("已注册状态转换引擎实例: {Name}", name);
        }

        /// <summary>
        /// 移除状态管理器实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>是否成功移除</returns>
        public bool RemoveStateManager(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var result = _stateManagerCache.TryRemove(name, out _);
            if (result)
            {
                _logger?.LogInformation("已移除状态管理器实例: {Name}", name);
            }

            return result;
        }

        /// <summary>
        /// 移除状态转换引擎实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>是否成功移除</returns>
        public bool RemoveTransitionEngine(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            var result = _transitionEngineCache.TryRemove(name, out _);
            if (result)
            {
                _logger?.LogInformation("已移除状态转换引擎实例: {Name}", name);
            }

            return result;
        }

        /// <summary>
        /// 清除所有实例
        /// </summary>
        public void ClearAllInstances()
        {
            _stateManagerCache.Clear();
            _transitionEngineCache.Clear();
            _logger?.LogInformation("已清除所有状态管理器和转换引擎实例");
        }

        /// <summary>
        /// 获取所有状态管理器实例名称
        /// </summary>
        /// <returns>实例名称列表</returns>
        public IEnumerable<string> GetStateManagerNames()
        {
            return _stateManagerCache.Keys.ToList();
        }

        /// <summary>
        /// 获取所有状态转换引擎实例名称
        /// </summary>
        /// <returns>实例名称列表</returns>
        public IEnumerable<string> GetTransitionEngineNames()
        {
            return _transitionEngineCache.Keys.ToList();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建状态管理器实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>状态管理器实例</returns>
        private IUnifiedStateManager CreateStateManager(string name)
        {
            var options = new StateManagerOptions();
            
            // 根据名称配置不同的选项
            switch (name)
            {
                case "Default":
                    // 默认配置
                    break;
                case "Strict":
                    // 严格模式配置
                    options.EnableTransitionValidation = true;
                    options.EnableTransitionLogging = true;
                    break;
                case "Relaxed":
                    // 宽松模式配置
                    options.EnableTransitionValidation = false;
                    options.EnableTransitionLogging = false;
                    break;
                default:
                    // 自定义配置，可从配置文件加载
                    break;
            }

            // 使用Model项目中的UnifiedStateManager
            var stateManager = new UnifiedStateManager(options);
            _logger?.LogInformation("已创建状态管理器实例: {Name}", name);
            return stateManager;
        }

        /// <summary>
        /// 创建状态转换引擎实例
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>状态转换引擎实例</returns>
        private IStatusTransitionEngine CreateTransitionEngine(string name)
        {
            var transitionEngine = new StatusTransitionEngine();
            _logger?.LogInformation("已创建状态转换引擎实例: {Name}", name);
            return transitionEngine;
        }

        #endregion
    }
}