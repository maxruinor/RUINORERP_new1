/**
 * 文件: StatusActionMapper.cs
 * 版本: V1.0 - 状态操作映射器
 * 说明: 将UI操作（如提交、审核、反审等）映射到具体的业务状态值
 * 创建日期: 2025-01-15
 * 作者: RUINOR ERP开发团队
 * 
 * 功能: 
 * 1. 定义所有支持的UI操作类型（提交、审核、反审等）
 * 2. 将UI操作映射到具体的状态值
 * 3. 支持自定义操作映射
 * 4. 提供统一的状态设置接口
 * 
 * 使用方法:
 * // 设置提交状态
 * var mapper = new StatusActionMapper();
 * var newStatus = mapper.MapActionToStatus(entity, StatusActionType.提交);
 * 
 * // 或者使用扩展方法
 * await stateManager.SetStatusByActionAsync(entity, StatusActionType.提交);
 */

using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Model.Base.StatusManager
{

    /// <summary>
    /// 状态操作处理委托
    /// 用于扩展自定义的操作处理逻辑
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="action">操作类型</param>
    /// <returns>目标状态值</returns>
    public delegate object StatusActionHandler(BaseEntity entity, MenuItemEnums action);

    /// <summary>
    /// 状态操作映射器
    /// 将UI操作（提交、审核、反审等）映射到具体的业务状态值
    /// </summary>
    public class StatusActionMapper
    {
        #region 字段和属性

        /// <summary>
        /// 自定义操作处理器字典
        /// Key: 状态类型 + 操作类型的组合
        /// Value: 处理器委托
        /// </summary>
        private static readonly Dictionary<string, StatusActionHandler> _customHandlers =
            new Dictionary<string, StatusActionHandler>();

        /// <summary>
        /// 线程同步锁
        /// </summary>
        private static readonly object _syncLock = new object();

        #endregion

        #region 公共方法

        /// <summary>
        /// 将UI操作映射到具体的状态值
        /// 基于实体类型和状态类型自动选择合适的目标状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>目标状态值</returns>
        public static object MapActionToStatus(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 获取实体的状态类型
            var stateManager = entity.StateManager;
            if (stateManager == null)
                throw new InvalidOperationException("无法获取状态管理器实例");

            var statusType = stateManager.GetStatusType(entity);

            // 获取当前状态
            var currentStatus = entity.GetPropertyValue(statusType.Name);

            // 尝试使用自定义处理器
            var customKey = $"{statusType.FullName}_{action}";
            lock (_syncLock)
            {
                if (_customHandlers.TryGetValue(customKey, out var handler))
                {
                    return handler(entity, action);
                }
            }

            // 使用默认映射逻辑
            return MapActionToStatusInternal(statusType, currentStatus, action);
        }

        /// <summary>
        /// 获取操作的描述文本
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <returns>操作描述</returns>
        public static string GetActionDescription(MenuItemEnums action)
        {
            if (action == MenuItemEnums.提交) return "提交单据";
            if (action == MenuItemEnums.审核) return "审核单据";
            if (action == MenuItemEnums.反审) return "反审单据";
            if (action == MenuItemEnums.结案) return "结案单据";
            if (action == MenuItemEnums.反结案) return "反结案单据";
            if (action == MenuItemEnums.保存) return "保存单据";
            return "未知操作";
        }

        /// <summary>
        /// 注册自定义的操作处理器
        /// 允许业务模块自定义特定状态类型的操作处理逻辑
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="action">操作类型</param>
        /// <param name="handler">处理器委托</param>
        public static void RegisterCustomHandler(Type statusType, MenuItemEnums action, StatusActionHandler handler)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var key = $"{statusType.FullName}_{action}";
            lock (_syncLock)
            {
                _customHandlers[key] = handler;
            }
        }

        /// <summary>
        /// 移除自定义的操作处理器
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="action">操作类型</param>
        public static void UnregisterCustomHandler(Type statusType, MenuItemEnums action)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            var key = $"{statusType.FullName}_{action}";
            lock (_syncLock)
            {
                _customHandlers.Remove(key);
            }
        }

        /// <summary>
        /// 清空所有自定义处理器
        /// 仅用于测试场景，请谨慎使用
        /// </summary>
        public static void ClearCustomHandlers()
        {
            lock (_syncLock)
            {
                _customHandlers.Clear();
            }
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 内部状态映射逻辑
        /// 根据状态类型和操作类型返回目标状态值
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态值</param>
        /// <param name="action">操作类型</param>
        /// <returns>目标状态值</returns>
        private static object MapActionToStatusInternal(Type statusType, object currentStatus, MenuItemEnums action)
        {
            // 如果是DataStatus类型，直接映射
            if (statusType == typeof(DataStatus))
            {
                if (action == MenuItemEnums.提交) return DataStatus.新建;
                if (action == MenuItemEnums.审核) return DataStatus.确认;
                if (action == MenuItemEnums.反审) return DataStatus.新建;
                if (action == MenuItemEnums.结案) return DataStatus.完结;
                if (action == MenuItemEnums.反结案) return DataStatus.新建;
                if (action == MenuItemEnums.保存) return DataStatus.草稿;
                return currentStatus;
            }
            
            // 如果是PrePaymentStatus类型，映射对应的状态
            if (statusType == typeof(PrePaymentStatus))
            {
                if (action == MenuItemEnums.提交) return PrePaymentStatus.待审核;
                if (action == MenuItemEnums.审核) return PrePaymentStatus.已生效;
                if (action == MenuItemEnums.反审) return PrePaymentStatus.待审核;
                if (action == MenuItemEnums.结案) return PrePaymentStatus.已结案;
                return currentStatus;
            }
            
            // 如果是ARAPStatus类型，映射对应的状态
            if (statusType == typeof(ARAPStatus))
            {
                if (action == MenuItemEnums.提交) return ARAPStatus.待审核;
                if (action == MenuItemEnums.审核) return ARAPStatus.待支付;
                if (action == MenuItemEnums.反审) return ARAPStatus.待审核;
                return currentStatus;
            }
            
            // 如果是PaymentStatus类型，映射对应的状态
            if (statusType == typeof(PaymentStatus))
            {
                if (action == MenuItemEnums.提交) return PaymentStatus.待审核;
                if (action == MenuItemEnums.审核) return PaymentStatus.已支付;
                if (action == MenuItemEnums.反审) return PaymentStatus.待审核;
                return currentStatus;
            }
            
            // 如果是StatementStatus类型，映射对应的状态
            if (statusType == typeof(StatementStatus))
            {
                if (action == MenuItemEnums.提交) return StatementStatus.新建;
                if (action == MenuItemEnums.审核) return StatementStatus.确认;
                if (action == MenuItemEnums.反审) return StatementStatus.新建;
                if (action == MenuItemEnums.结案) return StatementStatus.全部结清;
                if (action == MenuItemEnums.反结案) return StatementStatus.新建;
                return currentStatus;
            }

            // 其他业务状态类型的处理（需要子类或配置来指定）
            // 这里返回当前状态，实际使用时应该由业务模块进行自定义处理
            return currentStatus;
        }

        #endregion
    }

    /// <summary>
    /// 状态管理器扩展方法
    /// 提供通过UI操作直接设置状态的便捷方法
    /// 委托给IUnifiedStateManager的实现
    /// </summary>
    public static class StateManagerExtensions
    {
        /// <summary>
        /// 通过UI操作类型设置实体状态
        /// 这是一个通用方法，支持提交、审核、反审等所有预定义操作
        /// </summary>
        /// <param name="stateManager">状态管理器实例</param>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型（提交、审核、反审等）</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public static async System.Threading.Tasks.Task<StateTransitionResult> SetStatusByActionAsync(
            this IUnifiedStateManager stateManager,
            BaseEntity entity,
            MenuItemEnums action,
            string reason = null,
            string userId = null)
        {
            if (stateManager == null)
                return StateTransitionResult.Denied("状态管理器不能为空");

            // 委托给IUnifiedStateManager的实现
            // 这里实现已经在UnifiedStateManager类中的SetStatusByActionAsync方法中
            return await stateManager.SetStatusByActionAsync(entity, action, reason, userId);
        }
        
        /// <summary>
        /// 通过UI操作类型验证实体状态转换是否合法
        /// </summary>
        /// <param name="stateManager">状态管理器实例</param>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>验证结果</returns>
        public static async System.Threading.Tasks.Task<StateTransitionResult> ValidateActionTransitionAsync(
            this IUnifiedStateManager stateManager,
            BaseEntity entity,
            MenuItemEnums action)
        {
            if (stateManager == null)
                return StateTransitionResult.Denied("状态管理器不能为空");
                
            // 如果是UnifiedStateManager实例，直接调用其方法
            if (stateManager is UnifiedStateManager unifiedStateManager)
            {
                return await unifiedStateManager.ValidateActionTransitionAsync(entity, action);
            }
            
            // 对于其他实现，使用通用验证逻辑
            try
            {
                // 获取状态类型
                var statusType = stateManager.GetStatusType(entity);
                
                // 获取当前状态
                var currentStatus = stateManager.GetBusinessStatus(entity, statusType);
                
                // 获取目标状态
                var targetStatus = StatusActionMapper.MapActionToStatus(entity, action);
                if (targetStatus == null)
                    return StateTransitionResult.Denied($"无法映射操作类型 '{action}' 到状态值");
                
                // 验证状态转换
                return stateManager.ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus as Enum);
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Denied($"验证状态转换时发生错误: {ex.Message}");
            }
        }
    }
}
