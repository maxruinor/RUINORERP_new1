using System;
using System.Collections.Generic;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 配置验证结果
    /// </summary>
    public class ConfigValidationResult
    {
        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsValid { get; set; } = true;

        /// <summary>
        /// 验证错误列表，键为属性名称，值为错误信息
        /// </summary>
        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 全局错误信息（不针对特定属性）
        /// </summary>
        public List<string> GlobalErrors { get; set; } = new List<string>();

        /// <summary>
        /// 添加属性错误
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="errorMessage">错误信息</param>
        public void AddError(string propertyName, string errorMessage)
        {
            IsValid = false;
            if (Errors.ContainsKey(propertyName))
            {
                Errors[propertyName] = errorMessage;
            }
            else
            {
                Errors.Add(propertyName, errorMessage);
            }
        }

        /// <summary>
        /// 添加全局错误
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public void AddGlobalError(string errorMessage)
        {
            IsValid = false;
            GlobalErrors.Add(errorMessage);
        }

        /// <summary>
        /// 获取所有错误信息的组合字符串
        /// </summary>
        public string GetErrorMessage()
        {
            var messages = new List<string>();
            
            foreach (var error in Errors)
            {
                messages.Add($"{error.Key}: {error.Value}");
            }
            
            messages.AddRange(GlobalErrors);
            
            return string.Join("\n", messages);
        }
    }

    /// <summary>
    /// 配置验证规则
    /// </summary>
    public class ConfigValidationRule
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 验证函数
        /// </summary>
        public Func<object, string> Validate { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// 最小长度（针对字符串）
        /// </summary>
        public int MinLength { get; set; } = -1;

        /// <summary>
        /// 最大长度（针对字符串）
        /// </summary>
        public int MaxLength { get; set; } = -1;

        /// <summary>
        /// 最小值（针对数值类型）
        /// </summary>
        public decimal MinValue { get; set; } = decimal.MinValue;

        /// <summary>
        /// 最大值（针对数值类型）
        /// </summary>
        public decimal MaxValue { get; set; } = decimal.MaxValue;

        /// <summary>
        /// 正则表达式模式（针对字符串格式验证）
        /// </summary>
        public string RegexPattern { get; set; }
    }

    /// <summary>
    /// 配置错误码
    /// </summary>
    public enum ConfigErrorCode
    {
        /// <summary>
        /// 无错误
        /// </summary>
        None = 0,
        
        /// <summary>
        /// 配置文件不存在
        /// </summary>
        FileNotFound = 1,
        
        /// <summary>
        /// 配置文件格式错误
        /// </summary>
        FormatError = 2,
        
        /// <summary>
        /// 配置验证失败
        /// </summary>
        ValidationFailed = 3,
        
        /// <summary>
        /// 加密/解密失败
        /// </summary>
        EncryptionError = 4,
        
        /// <summary>
        /// 权限不足
        /// </summary>
        PermissionDenied = 5,
        
        /// <summary>
        /// 配置冲突
        /// </summary>
        Conflict = 6,
        
        /// <summary>
        /// 内部错误
        /// </summary>
        InternalError = 99
    }

    /// <summary>
    /// 配置异常类
    /// </summary>
    public class ConfigException : System.Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public ConfigErrorCode ErrorCode { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="errorCode">错误码</param>
        public ConfigException(string message, ConfigErrorCode errorCode = ConfigErrorCode.InternalError)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        /// <param name="errorCode">错误码</param>
        public ConfigException(string message, System.Exception innerException, ConfigErrorCode errorCode = ConfigErrorCode.InternalError)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}