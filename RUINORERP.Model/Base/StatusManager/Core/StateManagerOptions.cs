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
        /// 自定义状态转换规则
        /// </summary>
        public Dictionary<Type, Dictionary<object, List<object>>> CustomTransitionRules { get; set; } = 
            new Dictionary<Type, Dictionary<object, List<object>>>();
    }
}