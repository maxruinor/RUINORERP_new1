/**
 * 文件: StateManagerOptions.cs
 * 版本: V4 - 状态管理器配置选项
 * 说明: V4架构配置选项，简化版保留核心功能，支持一键初始化和零配置使用
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V4: 最终简化版配置，移除复杂配置项，保留核心功能
 * V4架构: 支持一键初始化和极低应用复杂度
 */

using System;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态管理器配置选项 - 简化版
    /// 移除了不必要的配置选项，保留核心功能
    /// </summary>
    public class StateManagerOptions
    {
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
        /// 创建默认配置
        /// </summary>
        /// <returns>默认配置选项</returns>
        public static StateManagerOptions CreateDefault()
        {
            return new StateManagerOptions();
        }
    }
}