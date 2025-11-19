/**
 * 文件: StateTransitionResult.cs
 * 版本: 公共 - 状态转换结果类（V3/V4共用）
 * 说明: 状态转换结果类，提供状态转换结果处理功能，V3和V4架构共用
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * 公共代码: V3和V4架构共用的结果类
 * 通用性: 状态转换结果处理，两个版本保持一致
 */

using RUINORERP.Global;
using System;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换结果
    /// </summary>
    public class StateTransitionResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StateTransitionResult()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isValid">转换是否有效</param>
        /// <param name="message">消息</param>
        public StateTransitionResult(bool isValid = false, string message = null)
        {
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// 转换是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public DataStatus? OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public DataStatus? NewStatus { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; set; }

        /// <summary>
        /// 转换原因
        /// </summary>
        public string TransitionReason { get; set; }

        /// <summary>
        /// 转换是否有效（兼容StatusManager.Core版本）
        /// </summary>
        public bool IsValid 
        { 
            get => IsSuccess; 
            set => IsSuccess = value; 
        }

        /// <summary>
        /// 消息（兼容StatusManager.Core版本）
        /// </summary>
        public string Message 
        { 
            get => ErrorMessage; 
            set => ErrorMessage = value; 
        }

        /// <summary>
        /// 异常信息（来自StatusManager.Core版本）
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Success(string message = null)
        {
            return new StateTransitionResult(true, message);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Failure(string message, Exception exception = null)
        {
            return new StateTransitionResult(false, message)
            {
                Exception = exception
            };
        }
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
        public new T? OldStatus
        {
            get
            {
                if (base.OldStatus.HasValue)
                {
                    return (T)Enum.Parse(typeof(T), base.OldStatus.Value.ToString());
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    base.OldStatus = (DataStatus)Enum.Parse(typeof(DataStatus), value.Value.ToString());
                }
                else
                {
                    base.OldStatus = null;
                }
            }
        }

        /// <summary>
        /// 转换后的状态（泛型类型）
        /// </summary>
        public new T? NewStatus
        {
            get
            {
                if (base.NewStatus.HasValue)
                {
                    return (T)Enum.Parse(typeof(T), base.NewStatus.Value.ToString());
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    base.NewStatus = (DataStatus)Enum.Parse(typeof(DataStatus), value.Value.ToString());
                }
                else
                {
                    base.NewStatus = null;
                }
            }
        }

        /// <summary>
        /// 创建泛型成功结果
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="message">消息</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult<T> Success(T? oldStatus, T? newStatus, string message = null)
        {
            return new StateTransitionResult<T>
            {
                IsSuccess = true,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ErrorMessage = message,
                TransitionTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建泛型失败结果
        /// </summary>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="message">错误消息</param>
        /// <param name="exception">异常</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult<T> Failure(T? oldStatus, T? newStatus, string message, Exception exception = null)
        {
            return new StateTransitionResult<T>
            {
                IsSuccess = false,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ErrorMessage = message,
                Exception = exception,
                TransitionTime = DateTime.Now
            };
        }
    }
}