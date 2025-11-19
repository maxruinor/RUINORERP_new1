/**
 * 文件: StateTransitionCheckResult.cs
 * 说明: 状态转换检查结果类 - 提供状态转换验证功能
 * 创建日期: 2025年
 * 作者: RUINOR ERP开发团队
 */

using System;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换检查结果
    /// 用于检查状态转换是否被允许的结果封装
    /// </summary>
    public class StateTransitionCheckResult
    {
        /// <summary>
        /// 是否允许转换
        /// </summary>
        public bool IsAllowed { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StateTransitionCheckResult()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isAllowed">是否允许转换</param>
        /// <param name="errorMessage">错误消息</param>
        public StateTransitionCheckResult(bool isAllowed, string errorMessage = null)
        {
            IsAllowed = isAllowed;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 创建允许转换的结果
        /// </summary>
        /// <returns>状态转换检查结果</returns>
        public static StateTransitionCheckResult Allowed()
        {
            return new StateTransitionCheckResult(true);
        }

        /// <summary>
        /// 创建拒绝转换的结果
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>状态转换检查结果</returns>
        public static StateTransitionCheckResult Denied(string errorMessage)
        {
            return new StateTransitionCheckResult(false, errorMessage);
        }
    }
}