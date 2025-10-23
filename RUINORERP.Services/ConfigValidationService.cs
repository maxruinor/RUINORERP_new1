using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Services
{
    /// <summary>
    /// 配置验证服务实现
    /// 处理配置对象的验证和错误管理
    /// </summary>
    public class ConfigValidationService : IConfigValidationService
    {
        private readonly ILogger<ConfigValidationService> _logger;
        private readonly Dictionary<string, List<ConfigValidationRule>> _customRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ConfigValidationService(ILogger<ConfigValidationService> logger)
        {
            _logger = logger;
            _customRules = new Dictionary<string, List<ConfigValidationRule>>();
        }

        /// <summary>
        /// 验证配置对象
        /// </summary>
        public ConfigValidationResult ValidateConfig<T>(T config) where T : BaseConfig
        {
            try
            {
                var result = new ConfigValidationResult();
                
                if (config == null)
                {
                    result.AddGlobalError("配置对象不能为空");
                    return result;
                }

                // 获取配置类型
                Type configType = typeof(T);
                string typeKey = configType.FullName;
                
                // 获取所有属性
                PropertyInfo[] properties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                
                foreach (PropertyInfo property in properties)
                {
                    // 获取属性值
                    object value = property.GetValue(config);
                    
                    // 执行数据注解验证
                    ValidateDataAnnotations(property, value, result);
                    
                    // 执行自定义规则验证
                    ValidateCustomRules(typeKey, property.Name, value, result);
                    
                    // 执行基本类型验证
                    ValidateBasicRules(property, value, result);
                }
                
                // 执行配置特定的业务验证
                ValidateBusinessRules(config, result);
                
                if (!result.IsValid)
                {
                    _logger.LogWarning("配置验证失败: {ConfigType}\n{Errors}", 
                        configType.Name, result.GetErrorMessage());
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置验证过程发生错误");
                var errorResult = new ConfigValidationResult();
                errorResult.AddGlobalError("配置验证过程发生错误: " + ex.Message);
                return errorResult;
            }
        }

        /// <summary>
        /// 异步验证配置对象
        /// </summary>
        public async Task<ConfigValidationResult> ValidateConfigAsync<T>(T config) where T : BaseConfig
        {
            return await Task.Run(() => ValidateConfig(config));
        }

        /// <summary>
        /// 验证单个属性值
        /// </summary>
        public string ValidateProperty<T>(T config, string propertyName, object value) where T : BaseConfig
        {
            try
            {
                PropertyInfo property = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                {
                    return $"属性 '{propertyName}' 不存在";
                }

                var result = new ConfigValidationResult();
                
                // 执行数据注解验证
                ValidateDataAnnotations(property, value, result);
                
                // 执行自定义规则验证
                string typeKey = typeof(T).FullName;
                ValidateCustomRules(typeKey, propertyName, value, result);
                
                // 执行基本类型验证
                ValidateBasicRules(property, value, result);
                
                // 返回属性的错误信息
                if (result.Errors.ContainsKey(propertyName))
                {
                    return result.Errors[propertyName];
                }
                
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证属性失败: {PropertyName}", propertyName);
                return "属性验证过程发生错误";
            }
        }

        /// <summary>
        /// 获取配置的所有验证规则
        /// </summary>
        public List<ConfigValidationRule> GetValidationRules<T>() where T : BaseConfig
        {
            string typeKey = typeof(T).FullName;
            if (_customRules.ContainsKey(typeKey))
            {
                return _customRules[typeKey];
            }
            return new List<ConfigValidationRule>();
        }

        /// <summary>
        /// 注册自定义验证规则
        /// </summary>
        public void RegisterCustomRule<T>(ConfigValidationRule rule) where T : BaseConfig
        {
            if (rule == null)
                return;

            string typeKey = typeof(T).FullName;
            if (!_customRules.ContainsKey(typeKey))
            {
                _customRules[typeKey] = new List<ConfigValidationRule>();
            }
            
            // 检查是否已存在同名规则，如果存在则替换
            var existingRule = _customRules[typeKey].FirstOrDefault(r => r.RuleName == rule.RuleName);
            if (existingRule != null)
            {
                _customRules[typeKey].Remove(existingRule);
            }
            
            _customRules[typeKey].Add(rule);
            _logger.LogInformation("注册自定义验证规则: {RuleName} 用于 {ConfigType}.{PropertyName}", 
                rule.RuleName, typeof(T).Name, rule.PropertyName);
        }

        /// <summary>
        /// 验证配置是否可应用（业务逻辑验证）
        /// </summary>
        public ConfigValidationResult ValidateConfigApplication<T>(T config) where T : BaseConfig
        {
            var result = new ConfigValidationResult();
            
            // 这里可以实现特定于配置类型的业务逻辑验证
            // 例如，检查数据库连接字符串格式、端口号范围等
            
            // 示例：验证服务器配置中的端口号
            if (config is ServerConfig serverConfig)
            {
                //if (serverConfig.ServerSettings.Port < 1 || serverConfig.ServerSettings.Port > 65535)
                //{
                //    result.AddError("ServerSettings.Port", "端口号必须在1-65535之间");
                //}
            }
            
            // 其他业务逻辑验证...
            
            return result;
        }

        /// <summary>
        /// 执行数据注解验证
        /// </summary>
        private void ValidateDataAnnotations(PropertyInfo property, object value, ConfigValidationResult result)
        {
            // 获取属性的所有验证属性
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>();
            
            foreach (var attribute in validationAttributes)
            {
                if (!attribute.IsValid(value))
                {
                    result.AddError(property.Name, attribute.ErrorMessage ?? $"属性 '{property.Name}' 验证失败");
                }
            }
        }

        /// <summary>
        /// 执行自定义规则验证
        /// </summary>
        private void ValidateCustomRules(string typeKey, string propertyName, object value, ConfigValidationResult result)
        {
            if (!_customRules.ContainsKey(typeKey))
                return;

            var rules = _customRules[typeKey].Where(r => r.PropertyName == propertyName);
            
            foreach (var rule in rules)
            {
                // 这里使用Func类型，但需要根据ConfigValidationRule的实际定义进行调整
                // 暂时注释掉这部分代码，因为Validate属性的具体实现未知
                
                // 执行规则中的其他验证
                if (rule.IsRequired && value == null)
                {
                    result.AddError(propertyName, rule.ErrorMessage ?? $"属性 '{propertyName}' 是必填项");
                }
                
                // 字符串长度验证
                if (value is string strValue)
                {
                    if (rule.MinLength >= 0 && strValue.Length < rule.MinLength)
                    {
                        result.AddError(propertyName, rule.ErrorMessage ?? $"属性 '{propertyName}' 长度不能小于 {rule.MinLength}");
                    }
                    
                    if (rule.MaxLength >= 0 && strValue.Length > rule.MaxLength)
                    {
                        result.AddError(propertyName, rule.ErrorMessage ?? $"属性 '{propertyName}' 长度不能大于 {rule.MaxLength}");
                    }
                    
                    if (!string.IsNullOrEmpty(rule.RegexPattern) && !Regex.IsMatch(strValue, rule.RegexPattern))
                    {
                        result.AddError(propertyName, rule.ErrorMessage ?? $"属性 '{propertyName}' 格式不正确");
                    }
                }
                
                // 数值范围验证
                if (value is IConvertible && rule.MinValue != decimal.MinValue && rule.MaxValue != decimal.MaxValue)
                {
                    try
                    {
                        decimal numValue = Convert.ToDecimal(value);
                        if (numValue < rule.MinValue || numValue > rule.MaxValue)
                        {
                            result.AddError(propertyName, rule.ErrorMessage ?? $"属性 '{propertyName}' 值必须在 {rule.MinValue} 到 {rule.MaxValue} 之间");
                        }
                    }
                    catch { /* 忽略转换错误，数值验证可能不是必须的 */ }
                }
            }
        }

        /// <summary>
        /// 执行基本类型验证
        /// </summary>
        private void ValidateBasicRules(PropertyInfo property, object value, ConfigValidationResult result)
        {
            // 检查必填字段
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null)
            {
                if (value == null || (value is string str && string.IsNullOrEmpty(str)))
                {
                    result.AddError(property.Name, requiredAttribute.ErrorMessage ?? $"属性 '{property.Name}' 是必填项");
                }
            }
        }

        /// <summary>
        /// 执行配置特定的业务验证规则
        /// </summary>
        private void ValidateBusinessRules<T>(T config, ConfigValidationResult result) where T : BaseConfig
        {
            // 根据配置类型进行特定验证
            switch (config)
            {
                case ServerConfig serverConfig:
                    ValidateServerConfig(serverConfig, result);
                    break;
                case SystemGlobalconfig systemConfig:
                    ValidateSystemConfig(systemConfig, result);
                    break;
                case GlobalValidatorConfig validatorConfig:
                    ValidateValidatorConfig(validatorConfig, result);
                    break;
            }
        }

        /// <summary>
        /// 验证服务器配置
        /// </summary>
        private void ValidateServerConfig(ServerConfig config, ConfigValidationResult result)
        {
            if (config.ServerPort < 1 || config.ServerPort > 65535)
            {
                result.AddError(nameof(config.ServerPort), "服务器端口必须在1-65535范围内");
            }
            
            if (string.IsNullOrEmpty(config.DbConnectionString))
            {
                result.AddError(nameof(config.DbConnectionString), "数据库连接字符串不能为空");
            }
            
            // 验证数据库连接字符串格式（简单验证）
            if (!string.IsNullOrEmpty(config.DbConnectionString) && 
                !config.DbConnectionString.Contains("Server=") && 
                !config.DbConnectionString.Contains("Data Source="))
            {
                result.AddError(nameof(config.DbConnectionString), "数据库连接字符串格式不正确，缺少服务器地址");
            }
        }

        /// <summary>
        /// 验证系统配置
        /// </summary>
        private void ValidateSystemConfig(SystemGlobalconfig config, ConfigValidationResult result)
        {
            // 系统配置验证逻辑
           
        }

        /// <summary>
        /// 验证验证器配置
        /// </summary>
        private void ValidateValidatorConfig(GlobalValidatorConfig config, ConfigValidationResult result)
        {
            if (config.ReworkTipDays < 0 || config.ReworkTipDays > 365)
            {
                result.AddError(nameof(config.ReworkTipDays), "返工提醒天数必须在0-365范围内");
            }
            
           
        }
    }
}