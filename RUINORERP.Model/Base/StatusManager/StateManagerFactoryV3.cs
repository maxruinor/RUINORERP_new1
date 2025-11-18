/**
 * 文件: StateManagerFactoryV3.cs
 * 说明: 简化版状态管理器工厂 - 提供基本的状态管理器和转换引擎创建功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using Microsoft.Extensions.Logging;


namespace RUINORERP.Model.Base.StatusManager
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
        /// 是否已释放资源
        /// </summary>
        private bool _disposed = false;

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
                    _instance = new StateManagerFactoryV3();
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

            // 初始化默认实例
            var options = new StateManagerOptions();

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
                    _logger?.LogInformation("状态管理器工厂资源已释放");
                }

                _disposed = true;
            }
        }

        #endregion
    }
}