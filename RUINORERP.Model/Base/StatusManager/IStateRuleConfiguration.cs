/**
 * 文件: IStateRuleConfiguration.cs
 * 版本: V3增强版 - 轻量级规则配置中心接口
 * 说明: 借鉴V4优点，为V3添加轻量级规则配置中心，提供状态规则统一管理
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3增强版: 借鉴V4规则配置中心优点，保持V3简洁架构
 * 功能: 提供轻量级状态规则配置，避免V4的过度复杂性
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 轻量级状态规则配置接口 - 借鉴V4优点
    /// 提供统一的状态规则管理，保持V3架构简洁性
    /// </summary>
    public interface IStateRuleConfiguration
    {
        /// <summary>
        /// 注册状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="validator">验证函数</param>
        void RegisterTransitionRule<T>(T fromStatus, T toStatus, Func<object, bool> validator = null) where T : Enum;

        /// <summary>
        /// 获取状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <returns>允许转换到的状态列表</returns>
        IEnumerable<T> GetTransitionRules<T>(T fromStatus) where T : Enum;

        /// <summary>
        /// 验证状态转换是否允许
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">验证上下文</param>
        /// <returns>验证结果</returns>
        bool ValidateTransition<T>(T fromStatus, T toStatus, object context = null) where T : Enum;

        /// <summary>
        /// 注册UI控件状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">数据状态</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        void RegisterUIControlRule<T>(T status, string controlName, bool enabled, bool visible = true) where T : Enum;

        /// <summary>
        /// 获取UI控件状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">数据状态</param>
        /// <returns>控件状态配置字典</returns>
        Dictionary<string, (bool Enabled, bool Visible)> GetUIControlRules<T>(T status) where T : Enum;

        /// <summary>
        /// 注册自定义验证规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="validator">验证函数</param>
        void RegisterCustomValidationRule(string ruleName, Func<object, bool> validator);

        /// <summary>
        /// 执行自定义验证规则
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="context">验证上下文</param>
        /// <returns>验证结果</returns>
        bool ExecuteCustomValidationRule(string ruleName, object context = null);

        /// <summary>
        /// 清除所有规则
        /// </summary>
        void ClearAllRules();

        /// <summary>
        /// 获取可用操作列表
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>可用操作列表</returns>
        List<string> GetAvailableActions<T>(T currentStatus) where T : Enum;

        /// <summary>
        /// 获取允许的操作列表（用于UI控制器）
        /// </summary>
        /// <param name="status">状态枚举</param>
        /// <returns>允许的操作枚举列表</returns>
        IEnumerable<Enum> GetAllowedActions(Enum status);

        /// <summary>
        /// 注册操作规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="canExecute">是否可以执行</param>
        void RegisterActionRule<T>(T status, string actionName, bool canExecute) where T : Enum;

        /// <summary>
        /// 检查操作是否可以执行
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">当前状态</param>
        /// <param name="actionName">操作名称</param>
        /// <returns>是否可以执行</returns>
        bool CanExecuteAction<T>(T status, string actionName) where T : Enum;

        /// <summary>
        /// 检查操作是否允许（用于UI控制器）
        /// </summary>
        /// <param name="status">状态枚举</param>
        /// <param name="action">操作名称</param>
        /// <returns>是否允许</returns>
        bool IsActionAllowed(Enum status, string action);

        /// <summary>
        /// 获取业务状态规则
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="status">业务状态</param>
        /// <returns>业务状态规则</returns>
        BusinessStatusRule GetBusinessStatusRule<T>(T status) where T : Enum;

        /// <summary>
        /// 获取业务状态规则（通过类型和值）
        /// </summary>
        /// <param name="businessStatusType">业务状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>业务状态规则</returns>
        BusinessStatusRule GetBusinessStatusRule(Type businessStatusType, object status);
    }
}