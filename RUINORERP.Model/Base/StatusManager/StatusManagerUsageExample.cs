/**
 * 文件: StatusManagerUsageExample.cs
 * 版本: V4 - 优化版状态管理器使用示例
 * 说明: 展示如何使用UnifiedStateManager和GlobalStateRulesManager的新增方法
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - 添加终态判断和修改权限检查示例
 */

using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.Base;
using System;
using System.Diagnostics;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态管理器使用示例类
    /// 展示如何使用新增的终态判断和修改权限检查方法
    /// </summary>
    public static class StatusManagerUsageExample
    {
        #region 终态判断示例

        /// <summary>
        /// 检查实体是否处于终态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="stateManager">统一状态管理器</param>
        /// <returns>是否为终态</returns>
        public static bool CheckEntityFinalStatus<TEntity>(TEntity entity, UnifiedStateManager stateManager) where TEntity : BaseEntity
        {
            try
            {
                if (entity == null || stateManager == null)
                    return false;

                // 使用新增的IsFinalStatus方法检查实体状态
                bool isFinal = stateManager.IsFinalStatus(entity);
                
                // 获取实体类型和当前状态用于日志记录
                string entityType = typeof(TEntity).Name;
                DataStatus dataStatus = entity.DataStatus;
                
                // 记录状态检查结果
                Debug.WriteLine($"实体 {entityType} (ID: {entity.ID}) 的状态检查: 数据状态={dataStatus}, 是终态={isFinal}");
                
                return isFinal;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"检查实体终态时发生异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 直接检查特定状态是否为终态
        /// </summary>
        /// <param name="paymentStatus">付款状态</param>
        /// <returns>是否为终态</returns>
        public static bool CheckPaymentFinalStatus(PaymentStatus paymentStatus)
        {
            // 直接使用GlobalStateRulesManager的IsFinalStatus方法
            return GlobalStateRulesManager.Instance.IsFinalStatus(paymentStatus);
        }

        #endregion

        #region 修改权限检查示例

        /// <summary>
        /// 检查实体是否可以修改
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="stateManager">统一状态管理器</param>
        /// <returns>是否可以修改</returns>
        public static bool CheckCanModifyEntity<TEntity>(TEntity entity, UnifiedStateManager stateManager) where TEntity : BaseEntity
        {
            try
            {
                if (entity == null || stateManager == null)
                    return false;

                // 使用新增的CanModify方法检查修改权限
                bool canModify = stateManager.CanModify(entity);
                
                // 获取实体类型和当前状态用于日志记录
                string entityType = typeof(TEntity).Name;
                DataStatus dataStatus = entity.DataStatus;
                
                // 记录修改权限检查结果
                Debug.WriteLine($"实体 {entityType} (ID: {entity.ID}) 的修改权限检查: 数据状态={dataStatus}, 可以修改={canModify}");
                
                if (!canModify)
                {
                    // 分析无法修改的原因
                    if (stateManager.IsFinalStatus(entity))
                    {
                        Debug.WriteLine($"无法修改原因: 实体已处于终态");
                    }
                    else if (dataStatus == DataStatus.完结 || dataStatus == DataStatus.作废)
                    {
                        Debug.WriteLine($"无法修改原因: 数据状态不允许修改");
                    }
                    else if (dataStatus == DataStatus.确认 && !GlobalStateRulesManager.Instance.AllowModifyAfterSubmit(entity))
                    {
                        Debug.WriteLine($"无法修改原因: 提交后不允许修改");
                    }
                }
                
                return canModify;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"检查实体修改权限时发生异常: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region 实际业务场景示例

        /// <summary>
        /// 销售订单保存前的状态验证
        /// </summary>
        /// <typeparam name="TSalesOrder">销售订单实体类型</typeparam>
        /// <param name="order">销售订单实体</param>
        /// <param name="stateManager">统一状态管理器</param>
        /// <returns>验证结果</returns>
        public static bool ValidateSalesOrderBeforeSave<TSalesOrder>(TSalesOrder order, UnifiedStateManager stateManager) where TSalesOrder : BaseEntity
        {
            // 检查是否可以修改订单
            bool canModify = CheckCanModifyEntity(order, stateManager);
            
            if (!canModify)
            {
                // 订单处于终态或不允许修改，可以返回友好提示
                return false;
            }
            
            // 其他业务验证逻辑...
            return true;
        }

        /// <summary>
        /// 付款处理前的状态检查
        /// </summary>
        /// <typeparam name="TPaymentEntity">付款相关实体类型</typeparam>
        /// <param name="paymentEntity">付款相关实体</param>
        /// <param name="stateManager">统一状态管理器</param>
        /// <returns>是否可以处理付款</returns>
        public static bool CanProcessPayment<TPaymentEntity>(TPaymentEntity paymentEntity, UnifiedStateManager stateManager) where TPaymentEntity : BaseEntity
        {
            // 检查实体是否处于终态
            if (CheckEntityFinalStatus(paymentEntity, stateManager))
            {
                // 已处于终态，不允许再次处理付款
                return false;
            }
            
            // 检查是否可以修改
            if (!CheckCanModifyEntity(paymentEntity, stateManager))
            {
                return false;
            }
            
            // 可以继续付款处理
            return true;
        }

        #endregion

        #region 错误处理和日志记录示例

        /// <summary>
        /// 安全地检查实体状态并记录日志
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="stateManager">统一状态管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>安全的状态检查结果</returns>
        public static SafeStatusCheckResult SafeCheckEntityStatus<TEntity>(
            TEntity entity, 
            UnifiedStateManager stateManager, 
            ILogger logger = null) where TEntity : BaseEntity
        {
            try
            {
                if (entity == null)
                {
                    logger?.LogWarning("实体对象为空，无法进行状态检查");
                    return new SafeStatusCheckResult { IsSuccess = false, Message = "实体对象为空" };
                }
                
                if (stateManager == null)
                {
                    logger?.LogWarning("状态管理器为空，无法进行状态检查");
                    return new SafeStatusCheckResult { IsSuccess = false, Message = "状态管理器为空" };
                }

                // 获取状态信息
                bool isFinalStatus = stateManager.IsFinalStatus(entity);
                bool canModify = stateManager.CanModify(entity);
                DataStatus dataStatus = entity.DataStatus;
                
                // 记录状态信息
                logger?.LogInformation(
                    "实体状态检查结果 - 类型: {EntityType}, ID: {EntityId}, 数据状态: {DataStatus}, 终态: {IsFinalStatus}, 可修改: {CanModify}",
                    typeof(TEntity).Name, entity.ID, dataStatus, isFinalStatus, canModify);
                
                return new SafeStatusCheckResult
                {
                    IsSuccess = true,
                    IsFinalStatus = isFinalStatus,
                    CanModify = canModify,
                    DataStatus = dataStatus
                };
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "实体状态检查失败");
                return new SafeStatusCheckResult 
                {
                    IsSuccess = false, 
                    Message = "状态检查过程中发生异常: " + ex.Message 
                };
            }
        }

        /// <summary>
        /// 安全状态检查结果类
        /// </summary>
        public class SafeStatusCheckResult
        {
            /// <summary>
            /// 检查是否成功
            /// </summary>
            public bool IsSuccess { get; set; }
            
            /// <summary>
            /// 是否为终态
            /// </summary>
            public bool IsFinalStatus { get; set; }
            
            /// <summary>
            /// 是否可以修改
            /// </summary>
            public bool CanModify { get; set; }
            
            /// <summary>
            /// 数据状态
            /// </summary>
            public DataStatus DataStatus { get; set; }
            
            /// <summary>
            /// 错误或提示信息
            /// </summary>
            public string Message { get; set; }
        }

        #endregion
    }
}
