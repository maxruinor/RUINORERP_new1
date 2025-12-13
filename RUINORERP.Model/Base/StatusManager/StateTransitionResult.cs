/**
 * 文件: StateTransitionResult.cs
 * 版本: V4 - 合并版状态转换结果类
 * 说明: 合并了状态转换检查结果和转换结果，通过ResultType属性区分用途
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - 合并StateTransitionCheckResult和StateTransitionResult
 * 
 * 版本标识：
 * V4: 合并状态转换检查结果和转换结果，通过ResultType属性区分用途
 * 公共代码: 状态转换结果处理，V3和V4架构共用
 * 通用性: 状态转换结果处理，支持检查和实际转换两种场景
 */

using RUINORERP.Global;
using System;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换结果类型枚举
    /// 用于区分结果是用于检查还是实际转换
    /// </summary>
    public enum StateTransitionResultType
    {
        /// <summary>
        /// 检查结果 - 仅用于验证状态转换是否被允许
        /// </summary>
        Check,
        
        /// <summary>
        /// 转换结果 - 实际执行状态转换后的结果
        /// </summary>
        Transition
    }

    /// <summary>
    /// 状态转换结果
    /// 合并了状态转换检查结果和转换结果，通过ResultType属性区分用途
    /// </summary>
    public class StateTransitionResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StateTransitionResult()
        {
            TransitionTime = DateTime.Now;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resultType">结果类型</param>
        /// <param name="isSuccess">转换是否成功或允许</param>
        /// <param name="errorMessage">错误消息</param>
        public StateTransitionResult(StateTransitionResultType resultType, bool isSuccess = false, string errorMessage = null)
        {
            ResultType = resultType;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            TransitionTime = DateTime.Now;
        }

        #region 属性

        /// <summary>
        /// 结果类型 - 区分是检查结果还是转换结果
        /// </summary>
        public StateTransitionResultType ResultType { get; set; }

        /// <summary>
        /// 是否成功或允许
        /// 对于检查结果，表示是否允许转换
        /// 对于转换结果，表示转换是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public object OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public object NewStatus { get; set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; set; }

        /// <summary>
        /// 转换原因
        /// </summary>
        public string TransitionReason { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        #endregion

        #region 检查结果相关方法

        /// <summary>
        /// 创建允许转换的检查结果
        /// </summary>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Allowed()
        {
            return new StateTransitionResult(StateTransitionResultType.Check, true);
        }

        /// <summary>
        /// 创建拒绝转换的检查结果
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Denied(string errorMessage)
        {
            return new StateTransitionResult(StateTransitionResultType.Check, false, errorMessage);
        }

        #endregion

        #region 转换结果相关方法

        /// <summary>
        /// 创建成功转换结果
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="message">消息</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Success(object oldStatus = null, object newStatus = null, Type statusType = null, string message = null)
        {
            return new StateTransitionResult(StateTransitionResultType.Transition, true, message)
            {
                OldStatus = oldStatus,
                NewStatus = newStatus,
                StatusType = statusType
            };
        }

        /// <summary>
        /// 创建失败转换结果
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="message">错误消息</param>
        /// <param name="exception">异常</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Failure(object oldStatus = null, object newStatus = null, Type statusType = null, string message = null, Exception exception = null)
        {
            return new StateTransitionResult(StateTransitionResultType.Transition, false, message)
            {
                OldStatus = oldStatus,
                NewStatus = newStatus,
                StatusType = statusType,
                Exception = exception
            };
        }

        #endregion

        #region 通用方法

        /// <summary>
        /// 获取结果描述
        /// </summary>
        /// <returns>结果描述</returns>
        public string GetDescription()
        {
            var prefix = ResultType == StateTransitionResultType.Check ? "状态转换检查" : "状态转换";
            var status = IsSuccess ? "成功" : "失败";
            
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                return $"{prefix}{status}：{ErrorMessage}";
            }
            
            return $"{prefix}{status}";
        }

        #endregion
    }

    /// <summary>
    /// 泛型状态转换结果
    /// 提供类型安全的状态转换结果
    /// </summary>
    /// <typeparam name="T">状态枚举类型</typeparam>
    public class StateTransitionResult<T> : StateTransitionResult where T : struct, Enum
    {
        /// <summary>
        /// 转换前的状态（泛型类型）
        /// </summary>
        public T? OldTypedStatus { get; set; }

        /// <summary>
        /// 转换后的状态（泛型类型）
        /// </summary>
        public T? NewTypedStatus { get; set; }

        #region 检查结果相关方法

        /// <summary>
        /// 创建允许转换的泛型检查结果
        /// </summary>
        /// <returns>状态转换结果</returns>
        public static new StateTransitionResult<T> Allowed()
        {
            return new StateTransitionResult<T>
            {
                ResultType = StateTransitionResultType.Check,
                IsSuccess = true
            };
        }

        /// <summary>
        /// 创建拒绝转换的泛型检查结果
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>状态转换结果</returns>
        public static new StateTransitionResult<T> Denied(string errorMessage)
        {
            return new StateTransitionResult<T>
            {
                ResultType = StateTransitionResultType.Check,
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }

        #endregion

        #region 转换结果相关方法

        /// <summary>
        /// 创建泛型成功转换结果
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="message">消息</param>
        /// <returns>状态转换结果</returns>
        public static new StateTransitionResult<T> Success(T? oldStatus, T? newStatus, string message = null)
        {
            return new StateTransitionResult<T>
            {
                ResultType = StateTransitionResultType.Transition,
                IsSuccess = true,
                OldTypedStatus = oldStatus,
                NewTypedStatus = newStatus,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                StatusType = typeof(T),
                ErrorMessage = message,
                TransitionTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建泛型失败转换结果
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="message">错误消息</param>
        /// <param name="exception">异常</param>
        /// <returns>状态转换结果</returns>
        public static new StateTransitionResult<T> Failure(T? oldStatus, T? newStatus, string message, Exception exception = null)
        {
            return new StateTransitionResult<T>
            {
                ResultType = StateTransitionResultType.Transition,
                IsSuccess = false,
                OldTypedStatus = oldStatus,
                NewTypedStatus = newStatus,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                StatusType = typeof(T),
                ErrorMessage = message,
                Exception = exception,
                TransitionTime = DateTime.Now
            };
        }

        #endregion
    }
}