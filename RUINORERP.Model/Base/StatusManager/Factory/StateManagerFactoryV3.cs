/**
 * 文件: StateManagerFactoryV3.cs
 * 说明: 简化版状态管理器工厂 - 提供基本的状态管理器和转换引擎创建功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager.Core;


namespace RUINORERP.Model.Base.StatusManager.Factory
{
    /// <summary>
    /// 简化版状态管理器工厂
    /// 提供基本的状态管理器和转换引擎创建功能
    /// 使用单例模式确保全局只有一个实例，避免重复初始化
    /// </summary>
    public class StateManagerFactoryV3 : IStateManagerFactoryV3, IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 单例实例
        /// </summary>
        private static StateManagerFactoryV3 _instance;

        /// <summary>
        /// 同步锁对象
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StateManagerFactoryV3> _logger;

        /// <summary>
        /// 默认状态管理器实例
        /// </summary>
        private readonly IUnifiedStateManager _defaultStateManager;

        /// <summary>
        /// 默认状态转换引擎实例
        /// </summary>
        private readonly IStatusTransitionEngine _defaultTransitionEngine;

        /// <summary>
        /// 自定义状态管理器注册表
        /// </summary>
        private readonly Dictionary<Type, Func<IUnifiedStateManager>> _customStateManagers;

        /// <summary>
        /// 自定义状态转换引擎注册表
        /// </summary>
        private readonly Dictionary<string, Func<IStatusTransitionEngine>> _customTransitionEngines;

        /// <summary>
        /// 是否已释放资源
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 状态转换规则是否已初始化
        /// </summary>
        private static bool _rulesInitialized = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <returns>工厂单例实例</returns>
        public static StateManagerFactoryV3 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new StateManagerFactoryV3();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 私有构造函数，防止外部实例化
        /// </summary>
        /// <param name="logger">日志记录器</param>
        private StateManagerFactoryV3(ILogger<StateManagerFactoryV3> logger = null)
        {
            _logger = logger;
            
            // 初始化注册表
            _customStateManagers = new Dictionary<Type, Func<IUnifiedStateManager>>();
            _customTransitionEngines = new Dictionary<string, Func<IStatusTransitionEngine>>();
            
            // 初始化默认实例
            var options = new StateManagerOptions();
            
            // 只初始化一次状态转换规则
            if (!_rulesInitialized)
            {
                StateTransitionRules.InitializeDefaultRules(options.TransitionRules);
                _rulesInitialized = true;
            }
            
            _defaultStateManager = new UnifiedStateManager(options);
            _defaultTransitionEngine = new StatusTransitionEngine();
            
            _logger?.LogInformation("状态管理器工厂已初始化");
        }

        #endregion

        #region 核心接口实现

        /// <summary>
        /// 获取统一状态管理器
        /// </summary>
        /// <returns>统一状态管理器实例</returns>
        public IUnifiedStateManager GetStateManager()
        {
            return _defaultStateManager;
        }

        /// <summary>
        /// 获取指定类型的统一状态管理器
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <returns>统一状态管理器实例</returns>
        public IUnifiedStateManager GetStateManager<T>() where T : Enum
        {
            return _defaultStateManager;
        }

        /// <summary>
        /// 获取状态转换引擎
        /// </summary>
        /// <returns>状态转换引擎实例</returns>
        public IStatusTransitionEngine GetTransitionEngine()
        {
            return _defaultTransitionEngine;
        }

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态转换上下文</returns>
        public IStatusTransitionContext CreateTransitionContext(object entity, Type statusType)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            return new StatusTransitionContext(
                (BaseEntity)entity,
                statusType,
                null,
                _defaultStateManager,
                _defaultTransitionEngine);
        }

        /// <summary>
        /// 创建状态转换上下文
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>状态转换上下文</returns>
        public IStatusTransitionContext CreateTransitionContext<T>(object entity) where T : Enum
        {
            return CreateTransitionContext(entity, typeof(T));
        }

        #endregion

        #region 注册方法实现

        /// <summary>
        /// 注册自定义状态管理器
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="factory">状态管理器工厂方法</param>
        public void RegisterStateManager(Type entityType, Func<IUnifiedStateManager> factory)
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _customStateManagers[entityType] = factory;
            _logger?.LogDebug("已注册自定义状态管理器: {EntityType}", entityType.Name);
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
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _customTransitionEngines["Default"] = factory;
            _logger?.LogDebug("已注册自定义状态转换引擎");
        }

        #endregion

        /// <summary>
        /// 获取默认状态转换引擎实例
        /// </summary>
        /// <returns>状态转换引擎实例</returns>
        public IStatusTransitionEngine GetDefaultTransitionEngine()
        {
            return _defaultTransitionEngine;
        }

   
        #region 兼容方法（简化版）

        /// <summary>
        /// 获取命名状态管理器实例（简化实现）
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>状态管理器实例</returns>
        public IUnifiedStateManager GetStateManager(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            // 简化实现：对于不同名称返回默认实例
            // 在实际应用中可以根据名称返回不同的预配置实例
            _logger?.LogDebug("获取状态管理器实例: {Name}", name);
            return _defaultStateManager;
        }

        /// <summary>
        /// 获取命名状态转换引擎实例（简化实现）
        /// </summary>
        /// <param name="name">实例名称</param>
        /// <returns>状态转换引擎实例</returns>
        public IStatusTransitionEngine GetTransitionEngine(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            // 简化实现：对于不同名称返回默认实例
            _logger?.LogDebug("获取状态转换引擎实例: {Name}", name);
            return _defaultTransitionEngine;
        }

        /// <summary>
        /// 创建状态转换上下文（完整参数版本，简化实现）
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
            object initialStatus = null,
            string stateManagerName = "Default",
            string transitionEngineName = "Default")
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            // 简化实现：忽略名称参数，使用默认实例
            return new StatusTransitionContext(
                entity,
                statusType,
                initialStatus,
                _defaultStateManager,
                _defaultTransitionEngine);
        }

        #endregion

        #region IDisposable接口实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 清理注册表
                    _customStateManagers?.Clear();
                    _customTransitionEngines?.Clear();
                    
                    _logger?.LogInformation("状态管理器工厂资源已释放");
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~StateManagerFactoryV3()
        {
            Dispose(false);
        }

        #endregion
    }
}