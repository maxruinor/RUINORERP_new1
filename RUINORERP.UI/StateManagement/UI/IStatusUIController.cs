/**
 * 文件: IStatusUIController.cs
 * 说明: 状态UI控制器接口 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RUINORERP.Model.Base.StatusManager;

namespace RUINORERP.UI.StateManagement.UI
{
    /// <summary>
    /// 状态UI控制器接口 - v3版本
    /// 定义状态管理UI控制的基本行为
    /// </summary>
    public interface IStatusUIController
    {
        /// <summary>
        /// 根据状态上下文更新UI状态
        /// </summary>
        /// <param name="statusContext">状态上下文</param>
        /// <param name="controls">控件集合</param>
        void UpdateUIStatus(RUINORERP.Model.Base.StatusManager.Core.IStatusTransitionContext statusContext, IEnumerable<Control> controls);

        /// <summary>
        /// 根据状态更新单个控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        void UpdateControlStatus(Control control, Enum status, string statusType);

        /// <summary>
        /// 注册UI状态规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="rule">规则</param>
        void RegisterUIStatusRule(Enum status, IUIStatusRule rule);

        /// <summary>
        /// 应用状态规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        void ApplyRules(Enum status, IEnumerable<Control> controls);

        /// <summary>
        /// 检查指定操作在当前状态下是否可执行
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>是否可执行</returns>
        bool CanExecuteAction(Enum action, RUINORERP.Model.Base.StatusManager.Core.IStatusTransitionContext statusContext);

        /// <summary>
        /// 获取当前状态下可执行的操作列表
        /// </summary>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>可执行的操作列表</returns>
        IEnumerable<Enum> GetAvailableActions(RUINORERP.Model.Base.StatusManager.Core.IStatusTransitionContext statusContext);
    }

    /// <summary>
    /// UI状态规则接口
    /// </summary>
    public interface IUIStatusRule
    {
        /// <summary>
        /// 应用规则
        /// </summary>
        /// <param name="context">UI状态上下文</param>
        void Apply(IUIStatusContext context);
    }

    /// <summary>
    /// UI状态上下文接口
    /// </summary>
    public interface IUIStatusContext
    {
        /// <summary>
        /// 状态
        /// </summary>
        Enum Status { get; }

        /// <summary>
        /// 状态类型
        /// </summary>
        string StatusType { get; }

        /// <summary>
        /// 控件集合
        /// </summary>
        IEnumerable<Control> Controls { get; }

        /// <summary>
        /// 状态管理器
        /// </summary>
        RUINORERP.Model.Base.StatusManager.Core.IUnifiedStateManager StateManager { get; }

        /// <summary>
        /// 获取控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>控件状态</returns>
        ControlState GetControlState(string controlName);

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="state">控件状态</param>
        void SetControlState(string controlName, ControlState state);
    }

    /// <summary>
    /// 控件状态
    /// </summary>
    public class ControlState
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 是否只读
        /// </summary>
        public bool ReadOnly { get; set; } = false;

        /// <summary>
        /// 背景色
        /// </summary>
        public System.Drawing.Color BackColor { get; set; } = System.Drawing.Color.Empty;

        /// <summary>
        /// 前景色
        /// </summary>
        public System.Drawing.Color ForeColor { get; set; } = System.Drawing.Color.Empty;
    }
}