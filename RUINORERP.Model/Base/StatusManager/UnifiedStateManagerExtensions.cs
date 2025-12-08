using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 文件: UnifiedStateManagerExtensions.cs
    /// 版本: V4简化版 - 统一状态管理器扩展
    /// 说明: 为统一状态管理器添加业务性状态支持的简化扩展方法
    /// 创建日期: 2024年
    /// 作者: RUINOR ERP开发团队
    /// 
    /// 版本标识：
    /// V4简化版: 最小化扩展，仅添加必要的业务状态支持，避免复杂依赖
    /// 功能: 提供业务性状态与数据状态、审批状态的统一管理
    /// </summary>

    /// <summary>
    /// 统一状态管理器扩展类
    /// 为统一状态管理器添加业务性状态支持的简化扩展方法
    /// </summary>
    public static class UnifiedStateManagerExtensions
    {
        #region 业务性状态集成

        /// <summary>
        /// 注册业务性状态类型到统一状态管理器
        /// </summary>
        /// <param name="stateManager">统一状态管理器</param>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="description">描述</param>
        public static void RegisterBusinessStatusType(
            this UnifiedStateManager stateManager, 
            Type statusType, 
            string displayName = null,
            string description = null)
        {
            try
            {
                // 验证参数
                if (stateManager == null)
                    throw new ArgumentNullException(nameof(stateManager));
                
                if (statusType == null)
                    throw new ArgumentNullException(nameof(statusType));

                // 简化版实现，直接使用状态管理器的现有功能
                // 这里仅做日志记录，实际注册逻辑在状态管理器内部
                var logger = GetLogger(stateManager);
                logger?.LogInformation("注册业务状态类型: {StatusType}", statusType.Name);
            }
            catch (Exception ex)
            {
                var logger = GetLogger(stateManager);
                logger?.LogError(ex, "注册业务状态类型失败: {StatusType}", statusType?.Name);
                throw;
            }
        }

        /// <summary>
        /// 验证业务性状态转换是否允许
        /// </summary>
        /// <param name="stateManager">统一状态管理器</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public static async Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(
            this UnifiedStateManager stateManager, 
            BaseEntity entity,
            Type statusType, 
            object targetStatus)
        {
            try
            {
                // 验证参数
                if (stateManager == null)
                    throw new ArgumentNullException(nameof(stateManager));
                
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                
                if (statusType == null)
                    throw new ArgumentNullException(nameof(statusType));

                // 使用统一状态管理器的现有验证方法
                return await stateManager.ValidateBusinessStatusTransitionAsync(entity, statusType, targetStatus);
            }
            catch (Exception ex)
            {
                var logger = GetLogger(stateManager);
                logger?.LogError(ex, "验证业务状态转换失败: {StatusType}", statusType?.Name);
                
                return new StateTransitionResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 获取实体的所有业务状态信息
        /// </summary>
        /// <param name="stateManager">统一状态管理器</param>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态信息列表</returns>
        public static List<EntityStatusInfo> GetEntityBusinessStatuses(
            this UnifiedStateManager stateManager, 
            BaseEntity entity)
        {
            var result = new List<EntityStatusInfo>();
            
            if (entity == null)
                return result;

            try
            {
                var entityType = entity.GetType();
                var properties = entityType.GetProperties();

                foreach (var property in properties)
                {
                    var propertyType = property.PropertyType;
                    
                    // 检查是否为枚举类型（业务状态通常是枚举）
                    if (propertyType.IsEnum)
                    {
                        var statusValue = property.GetValue(entity);
                        if (statusValue != null)
                        {
                            var statusInfo = new EntityStatusInfo
                            {
                                PropertyName = property.Name,
                                StatusType = propertyType,
                                StatusValue = statusValue,
                                IsBusinessStatus = true,
                                IsDataStatus = false,
                                IsApprovalStatus = false,
                                DisplayInfo = new BusinessStatusDisplayInfoExt
                                {
                                    StatusName = statusValue.ToString(),
                                    DisplayText = statusValue.ToString(),
                                    Description = $"{property.Name}: {statusValue}"
                                }
                            };

                            result.Add(statusInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = GetLogger(stateManager);
                logger?.LogError(ex, "获取实体业务状态失败: {EntityType}", entity.GetType().Name);
            }

            return result;
        }

        /// <summary>
        /// 获取实体在当前状态下的可用操作
        /// </summary>
        /// <param name="stateManager">统一状态管理器</param>
        /// <param name="entity">实体对象</param>
        /// <param name="context">上下文信息</param>
        /// <returns>可用操作列表</returns>
        public static List<EntityActionInfo> GetEntityAvailableActions(
            this UnifiedStateManager stateManager, 
            BaseEntity entity,
            Dictionary<string, object> context = null)
        {
            var result = new List<EntityActionInfo>();
            
            if (entity == null)
                return result;

            try
            {
                var entityType = entity.GetType();
                var properties = entityType.GetProperties();

                foreach (var property in properties)
                {
                    var propertyType = property.PropertyType;
                    
                    // 检查是否为枚举类型（业务状态通常是枚举）
                    if (propertyType.IsEnum)
                    {
                        var statusValue = property.GetValue(entity);
                        if (statusValue != null)
                        {
                            // 根据状态类型和当前状态值，确定可用操作
                            var actions = GetAvailableActionsForStatus(propertyType, statusValue, context);
                            
                            foreach (var action in actions)
                            {
                                var actionInfo = new EntityActionInfo
                                {
                                    ActionName = action,
                                    StatusType = propertyType,
                                    StatusValue = statusValue,
                                    PropertyName = property.Name,
                                    IsAllowed = true // 简化版默认允许所有操作
                                };

                                result.Add(actionInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = GetLogger(stateManager);
                logger?.LogError(ex, "获取实体可用操作失败: {EntityType}", entity.GetType().Name);
            }

            return result;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取日志记录器
        /// </summary>
        /// <param name="stateManager">统一状态管理器</param>
        /// <returns>日志记录器</returns>
        private static ILogger GetLogger(UnifiedStateManager stateManager)
        {
            try
            {
                // 通过反射获取日志记录器
                var loggerField = stateManager.GetType().GetField("_logger", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                return loggerField?.GetValue(stateManager) as ILogger;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据状态类型和当前状态值，获取可用操作
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="statusValue">当前状态值</param>
        /// <param name="context">上下文信息</param>
        /// <returns>可用操作列表</returns>
        private static List<string> GetAvailableActionsForStatus(
            Type statusType, 
            object statusValue, 
            Dictionary<string, object> context)
        {
            var actions = new List<string>();
            
            // 简化版实现，根据常见业务状态返回基本操作
            var statusName = statusValue?.ToString();
            
            // 根据状态名称添加通用操作
            switch (statusName)
            {
                case "草稿":
                case "Draft":
                    actions.AddRange(new[] { "提交", "保存", "删除" });
                    break;
                case "待审核":
                case "Pending":
                    actions.AddRange(new[] { "审核", "驳回", "撤回" });
                    break;
                case "已审核":
                case "Approved":
                    actions.AddRange(new[] { "执行", "取消" });
                    break;
                case "已完成":
                case "Completed":
                    actions.AddRange(new[] { "查看", "打印" });
                    break;
                default:
                    actions.AddRange(new[] { "查看", "编辑" });
                    break;
            }
            
            return actions;
        }

        #endregion
    }

    /// <summary>
    /// 实体状态信息
    /// </summary>
    public class EntityStatusInfo
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public object StatusValue { get; set; }

        /// <summary>
        /// 是否为业务状态
        /// </summary>
        public bool IsBusinessStatus { get; set; }

        /// <summary>
        /// 是否为数据状态
        /// </summary>
        public bool IsDataStatus { get; set; }

        /// <summary>
        /// 是否为审批状态
        /// </summary>
        public bool IsApprovalStatus { get; set; }

        /// <summary>
        /// 显示信息
        /// </summary>
        public BusinessStatusDisplayInfoExt DisplayInfo { get; set; }
    }

    /// <summary>
    /// 业务状态显示信息扩展
    /// </summary>
    public class BusinessStatusDisplayInfoExt
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 实体操作信息
    /// </summary>
    public class EntityActionInfo
    {
        /// <summary>
        /// 操作名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public object StatusValue { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 是否允许操作
        /// </summary>
        public bool IsAllowed { get; set; }
    }
}