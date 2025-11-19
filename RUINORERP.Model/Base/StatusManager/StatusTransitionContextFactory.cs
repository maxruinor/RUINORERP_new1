/**
 * 文件: StatusTransitionContextFactory.cs
 * 说明: 状态转换上下文工厂类 - 用于统一创建StatusTransitionContext实例
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换上下文工厂类
    /// 提供统一的方法来创建StatusTransitionContext实例，避免重复代码
    /// </summary>
    public static class StatusTransitionContextFactory
    {
        /// <summary>
        /// 创建数据状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateDataStatusContext(
            BaseEntity entity,
            DataStatus dataStatus,
            string reason = null,
            IServiceProvider serviceProvider = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                typeof(DataStatus),
                dataStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            return context;
        }

        /// <summary>
        /// 创建业务状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="businessStatus">业务状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateBusinessStatusContext<TBusinessStatus>(
            BaseEntity entity,
            TBusinessStatus businessStatus,
            string reason = null,
            IServiceProvider serviceProvider = null)
            where TBusinessStatus : struct, Enum
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                typeof(TBusinessStatus),
                businessStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            return context;
        }

        /// <summary>
        /// 创建操作状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="actionStatus">操作状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateActionStatusContext(
            BaseEntity entity,
            ActionStatus actionStatus,
            string reason = null,
            IServiceProvider serviceProvider = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                typeof(ActionStatus),
                actionStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            return context;
        }

        /// <summary>
        /// 创建UI状态更新上下文（用于UI状态更新，使用临时实体）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateUIUpdateContext(
            Type statusType,
            object currentStatus,
            string reason = null,
            IServiceProvider serviceProvider = null)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            // 创建临时实体用于UI更新
            var tempEntity = new BaseEntity();

            var context = new StatusTransitionContext(
                tempEntity,
                statusType,
                currentStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            return context;
        }

        /// <summary>
        /// 创建通用状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateContext(
            BaseEntity entity,
            Type statusType,
            object currentStatus,
            string reason = null,
            IServiceProvider serviceProvider = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                statusType,
                currentStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            return context;
        }
    }
}