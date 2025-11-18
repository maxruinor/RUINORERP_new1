/**
 * 文件: StateManagerOptions.cs
 * 说明: 状态管理器配置选项 - 简化版
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
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