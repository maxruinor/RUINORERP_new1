/**
 * 文件: StateManagerOptions.cs
 * 说明: 状态管理器配置选项
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态管理器配置选项
    /// 用于配置状态管理器的行为和特性
    /// </summary>
    public class StateManagerOptions
    {
        /// <summary>
        /// 是否启用状态转换日志
        /// </summary>
        public bool EnableTransitionLogging { get; set; } = false;

        /// <summary>
        /// 是否启用状态转换验证
        /// </summary>
        public bool EnableTransitionValidation { get; set; } = true;

        /// <summary>
        /// 是否启用状态变更事件
        /// </summary>
        public bool EnableStatusChangedEvents { get; set; } = true;

        /// <summary>
        /// 是否启用UI状态同步
        /// 当实体状态变更时，是否自动同步更新UI控件状态
        /// </summary>
        public bool EnableUIStateSync { get; set; } = true;
        
        /// <summary>
        /// 是否启用严格模式
        /// 严格模式下，只要有一个状态转换验证失败，就不执行任何状态设置
        /// </summary>
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 状态转换规则实例
        /// </summary>
        public Dictionary<Type, Dictionary<object, List<object>>> TransitionRules { get; set; } = 
            new Dictionary<Type, Dictionary<object, List<object>>>();

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>默认配置选项</returns>
        public static StateManagerOptions CreateDefault(Type entityType = null)
        {
            return new StateManagerOptions
            {
                EntityType = entityType,
                EnableTransitionLogging = false,
                EnableTransitionValidation = true,
                EnableStatusChangedEvents = true,
                EnableUIStateSync = true
            };
        }

        /// <summary>
        /// 创建严格验证配置
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>严格验证配置选项</returns>
        public static StateManagerOptions CreateStrict(Type entityType = null)
        {
            return new StateManagerOptions
            {
                EntityType = entityType,
                EnableTransitionLogging = true,
                EnableTransitionValidation = true,
                EnableStatusChangedEvents = true,
                EnableUIStateSync = true
      
            };
        }

        /// <summary>
        /// 创建宽松配置
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>宽松配置选项</returns>
        public static StateManagerOptions CreateRelaxed(Type entityType = null)
        {
            return new StateManagerOptions
            {
                EntityType = entityType,
                EnableTransitionLogging = false,
                EnableTransitionValidation = false,
                EnableStatusChangedEvents = true,
                EnableUIStateSync = false
            };
        }
    }
}