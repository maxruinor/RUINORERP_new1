/**
 * 文件: DefaultUIStatusRule.cs
 * 说明: 默认UI状态规则实现 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Model.Base.StatusManager;

namespace RUINORERP.UI.StateManagement.UI
{
    /// <summary>
    /// 默认UI状态规则实现 - v3版本
    /// 提供基础的UI状态控制规则
    /// </summary>
    public class DefaultUIStatusRule : IUIStatusRule
    {
        #region 字段

        /// <summary>
        /// 规则名称
        /// </summary>
        private readonly string _ruleName;

        /// <summary>
        /// 规则描述
        /// </summary>
        private readonly string _description;

        /// <summary>
        /// 控件状态配置
        /// </summary>
        private readonly Dictionary<string, ControlState> _controlStates;

        #endregion

        #region 属性

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName => _ruleName;

        /// <summary>
        /// 规则描述
        /// </summary>
        public string Description => _description;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化默认UI状态规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="description">规则描述</param>
        public DefaultUIStatusRule(string ruleName, string description = null)
        {
            _ruleName = ruleName ?? throw new ArgumentNullException(nameof(ruleName));
            _description = description ?? $"默认UI状态规则: {ruleName}";
            _controlStates = new Dictionary<string, ControlState>();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 应用规则
        /// </summary>
        /// <param name="context">UI状态上下文</param>
        public void Apply(IUIStatusContext context)
        {
            if (context == null)
                return;

            try
            {
                // 根据状态类型和状态值应用规则
                switch (context.StatusType)
                {
                    case "DataStatus":
                        ApplyDataStatusRule(context);
                        break;
                    case "BusinessStatus":
                        ApplyBusinessStatusRule(context);
                        break;
                    case "ActionStatus":
                        ApplyActionStatusRule(context);
                        break;
                    default:
                        ApplyDefaultRule(context);
                        break;
                }
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，避免影响UI更新
                System.Diagnostics.Debug.WriteLine($"应用UI状态规则失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        /// <param name="readOnly">是否只读</param>
        /// <param name="backColor">背景色</param>
        /// <param name="foreColor">前景色</param>
        /// <returns>当前规则实例，支持链式调用</returns>
        public DefaultUIStatusRule SetControlState(
            string controlName, 
            bool enabled = true, 
            bool visible = true, 
            bool readOnly = false,
            System.Drawing.Color? backColor = null,
            System.Drawing.Color? foreColor = null)
        {
            if (!string.IsNullOrEmpty(controlName))
            {
                _controlStates[controlName] = new ControlState
                {
                    Enabled = enabled,
                    Visible = visible,
                    ReadOnly = readOnly,
                    BackColor = backColor ?? System.Drawing.Color.Empty,
                    ForeColor = foreColor ?? System.Drawing.Color.Empty
                };
            }

            return this;
        }

        /// <summary>
        /// 清除控件状态配置
        /// </summary>
        /// <param name="controlName">控件名称，如果为空则清除所有</param>
        /// <returns>当前规则实例，支持链式调用</returns>
        public DefaultUIStatusRule ClearControlState(string controlName = null)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                _controlStates.Clear();
            }
            else if (_controlStates.ContainsKey(controlName))
            {
                _controlStates.Remove(controlName);
            }

            return this;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 应用数据状态规则
        /// </summary>
        /// <param name="context">UI状态上下文</param>
        private void ApplyDataStatusRule(IUIStatusContext context)
        {
            var status = context.Status.ToString();
            var isEditable = true;
            var isVisible = true;

            // 根据数据状态确定控件是否可编辑
            if (status.Contains("确认") || status.Contains("完结") || status.Contains("审核"))
            {
                isEditable = false;
            }

            // 应用规则到所有控件
            ApplyControlStates(context.Controls, isEditable, isVisible);
        }

        /// <summary>
        /// 应用业务状态规则
        /// </summary>
        /// <param name="context">UI状态上下文</param>
        private void ApplyBusinessStatusRule(IUIStatusContext context)
        {
            var status = context.Status.ToString();
            var isEditable = true;
            var isVisible = true;

            // 根据业务状态确定控件是否可编辑
            if (status.Contains("已关闭") || status.Contains("已完成"))
            {
                isEditable = false;
            }

            // 应用规则到所有控件
            ApplyControlStates(context.Controls, isEditable, isVisible);
        }

        /// <summary>
        /// 应用操作状态规则
        /// </summary>
        /// <param name="context">UI状态上下文</param>
        private void ApplyActionStatusRule(IUIStatusContext context)
        {
            var status = context.Status.ToString();
            var isEditable = true;
            var isVisible = true;

            // 根据操作状态确定控件是否可见
            if (status.Equals("删除"))
            {
                isVisible = false;
            }

            // 应用规则到所有控件
            ApplyControlStates(context.Controls, isEditable, isVisible);
        }

        /// <summary>
        /// 应用默认规则
        /// </summary>
        /// <param name="context">UI状态上下文</param>
        private void ApplyDefaultRule(IUIStatusContext context)
        {
            // 默认规则：所有控件可编辑和可见
            ApplyControlStates(context.Controls, true, true);
        }

        /// <summary>
        /// 应用控件状态
        /// </summary>
        /// <param name="controls">控件集合</param>
        /// <param name="isEditable">是否可编辑</param>
        /// <param name="isVisible">是否可见</param>
        private void ApplyControlStates(IEnumerable<Control> controls, bool isEditable, bool isVisible)
        {
            foreach (var control in controls)
            {
                // 检查是否有特定控件的状态配置
                if (_controlStates.TryGetValue(control.Name, out var state))
                {
                    ApplyControlState(control, state);
                }
                else
                {
                    // 应用默认状态
                    var defaultState = new ControlState
                    {
                        Enabled = isEditable,
                        Visible = isVisible,
                        ReadOnly = !isEditable
                    };
                    ApplyControlState(control, defaultState);
                }
            }
        }

        /// <summary>
        /// 应用单个控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="state">控件状态</param>
        private void ApplyControlState(Control control, ControlState state)
        {
            if (control == null || state == null)
                return;

            // 应用基本属性
            control.Enabled = state.Enabled;
            control.Visible = state.Visible;

            // 应用只读属性
            SetControlReadOnly(control, state.ReadOnly);

            // 应用颜色属性
            if (state.BackColor != System.Drawing.Color.Empty)
            {
                control.BackColor = state.BackColor;
            }

            if (state.ForeColor != System.Drawing.Color.Empty)
            {
                control.ForeColor = state.ForeColor;
            }
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="readOnly">是否只读</param>
        private void SetControlReadOnly(Control control, bool readOnly)
        {
            switch (control)
            {
                case TextBox:
                    ((TextBox)control).ReadOnly = readOnly;
                    break;
                case ComboBox:
                    ((ComboBox)control).Enabled = !readOnly;
                    break;
                case NumericUpDown:
                    ((NumericUpDown)control).ReadOnly = readOnly;
                    break;
                case DataGridView:
                    ((DataGridView)control).ReadOnly = readOnly;
                    break;
                case CheckBox:
                    ((CheckBox)control).Enabled = !readOnly;
                    break;
                case RadioButton:
                    ((RadioButton)control).Enabled = !readOnly;
                    break;
            }
        }

        #endregion
    }

    /// <summary>
    /// UI状态规则构建器
    /// 用于简化UI状态规则的创建
    /// </summary>
    public static class UIStatusRuleBuilder
    {
        /// <summary>
        /// 创建数据状态规则
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="ruleName">规则名称</param>
        /// <returns>规则构建器</returns>
        public static DefaultUIStatusRule CreateDataStatusRule(Enum status, string ruleName = null)
        {
            var name = ruleName ?? $"DataStatus_{status}";
            return new DefaultUIStatusRule(name, $"数据状态规则: {status}");
        }

        /// <summary>
        /// 创建业务状态规则
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="ruleName">规则名称</param>
        /// <returns>规则构建器</returns>
        public static DefaultUIStatusRule CreateBusinessStatusRule(Enum status, string ruleName = null)
        {
            var name = ruleName ?? $"BusinessStatus_{status}";
            return new DefaultUIStatusRule(name, $"业务状态规则: {status}");
        }

        /// <summary>
        /// 创建操作状态规则
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="ruleName">规则名称</param>
        /// <returns>规则构建器</returns>
        public static DefaultUIStatusRule CreateActionStatusRule(Enum status, string ruleName = null)
        {
            var name = ruleName ?? $"ActionStatus_{status}";
            return new DefaultUIStatusRule(name, $"操作状态规则: {status}");
        }

        /// <summary>
        /// 创建自定义规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="description">规则描述</param>
        /// <returns>规则构建器</returns>
        public static DefaultUIStatusRule CreateCustomRule(string ruleName, string description = null)
        {
            return new DefaultUIStatusRule(ruleName, description);
        }
    }
}