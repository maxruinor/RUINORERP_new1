/*************************************************************
 * 文件名：ConfigValidator.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置验证系统，提供配置数据的完整性、有效性检查和验证规则管理
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config.Validation
{
    /// <summary>
    /// 配置验证规则类型
    /// </summary>
    public enum ValidationRuleType
    {
        /// <summary>
        /// 必需字段验证
        /// </summary>
        Required,
        /// <summary>
        /// 字符串长度验证
        /// </summary>
        StringLength,
        /// <summary>
        /// 范围验证
        /// </summary>
        Range,
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        RegularExpression,
        /// <summary>
        /// 自定义验证
        /// </summary>
        Custom
    }

    /// <summary>
    /// 配置验证结果
    /// 包含单个配置项的验证信息
    /// </summary>
    public class ConfigValidationResult
    {
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 验证失败的成员名称
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 验证错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public object CurrentValue { get; set; }

        /// <summary>
        /// 验证规则类型
        /// </summary>
        public ValidationRuleType RuleType { get; set; }

        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime ValidationTime { get; set; }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        public override string ToString()
        {
            return IsValid ? "验证通过" : $"{MemberName}: {ErrorMessage} (当前值: {CurrentValue})";
        }
    }

    /// <summary>
    /// 配置验证结果集
    /// 包含整个配置对象的验证信息
    /// </summary>
    public class ConfigValidationResultSet
    {
        /// <summary>
        /// 是否所有验证都通过
        /// </summary>
        public bool IsValid => ValidationResults.All(r => r.IsValid);

        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 单个验证结果列表
        /// </summary>
        public List<ConfigValidationResult> ValidationResults { get; set; } = new List<ConfigValidationResult>();

        /// <summary>
        /// 验证时间
        /// </summary>
        public DateTime ValidationTime { get; set; }

        /// <summary>
        /// 获取所有错误信息
        /// </summary>
        public List<string> GetErrorMessages()
        {
            return ValidationResults
                .Where(r => !r.IsValid)
                .Select(r => r.ToString())
                .ToList();
        }

        /// <summary>
        /// 获取验证摘要
        /// </summary>
        public string GetSummary()
        {
            if (IsValid)
                return $"配置 {ConfigType} 验证通过";

            return $"配置 {ConfigType} 验证失败，{ValidationResults.Count(r => !r.IsValid)} 个错误";
        }
    }

    /// <summary>
    /// 配置验证器接口
    /// 定义配置验证的核心功能
    /// </summary>
    public interface IConfigValidator
    {
        /// <summary>
        /// 验证配置对象
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>验证结果集</returns>
        Task<ConfigValidationResultSet> ValidateAsync<T>(T config) where T : class;

        /// <summary>
        /// 验证配置对象
        /// </summary>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型</param>
        /// <returns>验证结果集</returns>
        Task<ConfigValidationResultSet> ValidateAsync(object config, Type configType);

        /// <summary>
        /// 注册自定义验证规则
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="validationFunc">验证函数</param>
        /// <param name="errorMessage">错误消息</param>
        void RegisterCustomRule<T>(string propertyName, Func<T, bool> validationFunc, string errorMessage) where T : class;

        /// <summary>
        /// 清除指定类型的所有自定义验证规则
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        void ClearCustomRules<T>() where T : class;
    }

    /// <summary>
    /// 自定义配置验证特性
    /// 用于定义特定的配置验证规则
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class ConfigValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 配置特定的错误消息
        /// </summary>
        public string ConfigErrorMessage { get; set; }

        /// <summary>
        /// 验证规则类型
        /// </summary>
        public ValidationRuleType RuleType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigValidationAttribute() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        public ConfigValidationAttribute(string errorMessage) : base(errorMessage)
        {
            ConfigErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// 自定义验证规则
    /// </summary>
    public class CustomValidationRule
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 验证函数
        /// </summary>
        public object ValidationFunction { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// 默认配置验证器实现
    /// 基于DataAnnotations和自定义规则的配置验证
    /// </summary>
    public class DefaultConfigValidator : IConfigValidator
    {
        private readonly ILogger<DefaultConfigValidator> _logger;
        private readonly Dictionary<string, List<CustomValidationRule>> _customRules = new Dictionary<string, List<CustomValidationRule>>();
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public DefaultConfigValidator(ILogger<DefaultConfigValidator> logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 验证配置对象
        /// </summary>
        public async Task<ConfigValidationResultSet> ValidateAsync<T>(T config) where T : class
        {
            return await ValidateAsync(config, typeof(T));
        }

        /// <summary>
        /// 验证配置对象
        /// </summary>
        public async Task<ConfigValidationResultSet> ValidateAsync(object config, Type configType)
        {
            _logger?.LogInformation("开始验证配置: {ConfigType}", configType.Name);
            var resultSet = new ConfigValidationResultSet
            {
                ConfigType = configType.Name,
                ValidationTime = DateTime.Now
            };

            if (config == null)
            {
                resultSet.ValidationResults.Add(new ConfigValidationResult
                {
                    IsValid = false,
                    ConfigType = configType.Name,
                    MemberName = "Root",
                    ErrorMessage = "配置对象不能为空",
                    RuleType = ValidationRuleType.Required,
                    ValidationTime = DateTime.Now
                });
                _logger?.LogWarning("配置验证失败: 配置对象为空");
                return resultSet;
            }

            // 使用Task.Run确保验证在异步上下文中执行
            await Task.Run(() =>
            {
                // 1. 使用DataAnnotations进行验证
                ValidateWithDataAnnotations(config, configType, resultSet);

                // 2. 执行自定义验证规则
                ValidateWithCustomRules(config, configType, resultSet);

                // 3. 执行类型特定的验证方法（如果存在）
                ValidateWithTypeSpecificMethod(config, configType, resultSet);
            });

            _logger?.LogInformation("配置验证完成: {ConfigType}, 结果: {IsValid}", 
                configType.Name, resultSet.IsValid ? "通过" : "失败");
            return resultSet;
        }

        /// <summary>
        /// 使用DataAnnotations进行验证
        /// </summary>
        private void ValidateWithDataAnnotations(object config, Type configType, ConfigValidationResultSet resultSet)
        {
            try
            {
                // 获取所有属性
                PropertyInfo[] properties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    if (!property.CanRead)
                        continue;

                    object value = property.GetValue(config);
                    string propertyName = property.Name;

                    // 获取属性上的所有验证特性
                    var validationAttributes = property.GetCustomAttributes<ValidationAttribute>(true);

                    foreach (var attribute in validationAttributes)
                    {
                        bool isValid = attribute.IsValid(value);
                        if (!isValid)
                        {
                            // 确定规则类型
                            var ruleType = ValidationRuleType.Custom;
                            if (attribute is RequiredAttribute) ruleType = ValidationRuleType.Required;
                            else if (attribute is StringLengthAttribute) ruleType = ValidationRuleType.StringLength;
                            else if (attribute is RangeAttribute) ruleType = ValidationRuleType.Range;
                            else if (attribute is RegularExpressionAttribute) ruleType = ValidationRuleType.RegularExpression;
                            
                            resultSet.ValidationResults.Add(new ConfigValidationResult
                            {
                                IsValid = false,
                                ConfigType = configType.Name,
                                MemberName = propertyName,
                                ErrorMessage = attribute.FormatErrorMessage(propertyName),
                                CurrentValue = value,
                                RuleType = ruleType,
                                ValidationTime = DateTime.Now
                            });
                        }
                    }
                }

                // 验证类级别特性
                var classAttributes = configType.GetCustomAttributes<ValidationAttribute>(true);
                foreach (var attribute in classAttributes)
                {
                    bool isValid = attribute.IsValid(config);
                    if (!isValid)
                    {
                        resultSet.ValidationResults.Add(new ConfigValidationResult
                        {
                            IsValid = false,
                            ConfigType = configType.Name,
                            MemberName = "Class",
                            ErrorMessage = attribute.FormatErrorMessage(configType.Name),
                            CurrentValue = config,
                            RuleType = ValidationRuleType.Custom,
                            ValidationTime = DateTime.Now
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "使用DataAnnotations验证配置时出错");
                resultSet.ValidationResults.Add(new ConfigValidationResult
                {
                    IsValid = false,
                    ConfigType = configType.Name,
                    MemberName = "ValidationError",
                    ErrorMessage = $"验证过程中发生错误: {ex.Message}",
                    RuleType = ValidationRuleType.Custom,
                    ValidationTime = DateTime.Now
                });
            }
        }

        /// <summary>
        /// 使用自定义验证规则
        /// </summary>
        private void ValidateWithCustomRules(object config, Type configType, ConfigValidationResultSet resultSet)
        {
            try
            {
                string typeKey = configType.FullName;

                lock (_lock)
                {
                    if (_customRules.TryGetValue(typeKey, out List<CustomValidationRule> rules))
                    {
                        foreach (var rule in rules)
                        {
                            // 使用反射调用验证函数
                            bool isValid = false;
                            try
                            {
                                var funcType = rule.ValidationFunction.GetType();
                                isValid = (bool)funcType.InvokeMember("DynamicInvoke", 
                                    BindingFlags.InvokeMethod, null, 
                                    rule.ValidationFunction, new[] { config });
                            }
                            catch (Exception ex)
                            {
                                _logger?.LogError(ex, "执行自定义验证规则时出错: {PropertyName}", rule.PropertyName);
                                isValid = false;
                            }

                            if (!isValid)
                            {
                                resultSet.ValidationResults.Add(new ConfigValidationResult
                                {
                                    IsValid = false,
                                    ConfigType = configType.Name,
                                    MemberName = rule.PropertyName,
                                    ErrorMessage = rule.ErrorMessage,
                                    CurrentValue = configType.GetProperty(rule.PropertyName)?.GetValue(config),
                                    RuleType = ValidationRuleType.Custom,
                                    ValidationTime = DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "使用自定义规则验证配置时出错");
            }
        }

        /// <summary>
        /// 执行类型特定的验证方法
        /// 查找并执行配置类型中定义的Validate方法
        /// </summary>
        private void ValidateWithTypeSpecificMethod(object config, Type configType, ConfigValidationResultSet resultSet)
        {
            try
            {
                // 查找名为Validate的方法
                var validateMethod = configType.GetMethod("Validate", 
                    BindingFlags.Public | BindingFlags.Instance, 
                    null, 
                    new[] { typeof(List<ConfigValidationResult>) },
                    null);

                if (validateMethod != null)
                {
                    var validationResults = new List<ConfigValidationResult>();
                    validateMethod.Invoke(config, new object[] { validationResults });

                    foreach (var result in validationResults)
                    {
                        result.ConfigType = configType.Name;
                        result.ValidationTime = DateTime.Now;
                        resultSet.ValidationResults.Add(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行类型特定验证方法时出错");
            }
        }

        /// <summary>
        /// 注册自定义验证规则
        /// </summary>
        public void RegisterCustomRule<T>(string propertyName, Func<T, bool> validationFunc, string errorMessage) where T : class
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));
            
            if (validationFunc == null)
                throw new ArgumentNullException(nameof(validationFunc));

            string typeKey = typeof(T).FullName;

            lock (_lock)
            {
                if (!_customRules.ContainsKey(typeKey))
                {
                    _customRules[typeKey] = new List<CustomValidationRule>();
                }

                _customRules[typeKey].Add(new CustomValidationRule
                {
                    PropertyName = propertyName,
                    ValidationFunction = validationFunc,
                    ErrorMessage = errorMessage
                });
            }

            _logger?.LogInformation("已注册自定义验证规则: {Type}.{Property}", typeof(T).Name, propertyName);
        }

        /// <summary>
        /// 清除指定类型的所有自定义验证规则
        /// </summary>
        public void ClearCustomRules<T>() where T : class
        {
            string typeKey = typeof(T).FullName;

            lock (_lock)
            {
                if (_customRules.ContainsKey(typeKey))
                {
                    _customRules.Remove(typeKey);
                }
            }

            _logger?.LogInformation("已清除所有自定义验证规则: {Type}", typeof(T).Name);
        }
    }

    /// <summary>
    /// 配置验证扩展类
    /// 提供验证相关的扩展方法
    /// </summary>
    public static class ConfigValidationExtensions
    {
        /// <summary>
        /// 验证配置对象
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="validator">配置验证器</param>
        /// <param name="config">配置对象</param>
        /// <returns>验证是否通过</returns>
        public static async Task<bool> IsValidAsync<T>(this IConfigValidator validator, T config) where T : class
        {
            var result = await validator.ValidateAsync(config);
            return result.IsValid;
        }

        /// <summary>
        /// 验证配置并在失败时抛出异常
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="validator">配置验证器</param>
        /// <param name="config">配置对象</param>
        /// <exception cref="ConfigValidationException">当验证失败时抛出</exception>
        public static async Task ValidateAndThrowAsync<T>(this IConfigValidator validator, T config) where T : class
        {
            var result = await validator.ValidateAsync(config);
            if (!result.IsValid)
            {
                throw new ConfigValidationException(result);
            }
        }
    }

    /// <summary>
    /// 配置验证异常
    /// 当配置验证失败时抛出
    /// </summary>
    public class ConfigValidationException : Exception
    {
        /// <summary>
        /// 验证结果集
        /// </summary>
        public ConfigValidationResultSet ValidationResult { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">验证结果集</param>
        public ConfigValidationException(ConfigValidationResultSet result)
            : base(result.GetSummary())
        {
            ValidationResult = result;
        }
    }
}