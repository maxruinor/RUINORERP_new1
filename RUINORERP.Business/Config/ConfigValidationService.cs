using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.IO;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置验证服务实现
    /// 提供配置验证和错误处理功能
    /// </summary>
    public class ConfigValidationService : IConfigValidationService
    {
        private readonly ILogger<ConfigValidationService> _logger;
        private readonly Dictionary<string, List<ConfigValidationRule>> _customRules;
        private readonly IValidator<ServerConfig> _serverConfigValidator;
        private readonly IValidator<SystemGlobalconfig> _systemGlobalconfigValidator;
        private readonly IValidator<GlobalValidatorConfig> _globalValidatorConfigValidator;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="serverConfigValidator">服务器配置验证器</param>
        /// <param name="systemGlobalconfigValidator">系统全局配置验证器</param>
        /// <param name="globalValidatorConfigValidator">全局验证配置验证器</param>
        public ConfigValidationService(
            ILogger<ConfigValidationService> logger,
            IValidator<ServerConfig> serverConfigValidator,
            IValidator<SystemGlobalconfig> systemGlobalconfigValidator,
            IValidator<GlobalValidatorConfig> globalValidatorConfigValidator)
        {
            _logger = logger;
            _customRules = new Dictionary<string, List<ConfigValidationRule>>();
            _serverConfigValidator = serverConfigValidator;
            _systemGlobalconfigValidator = systemGlobalconfigValidator;
            _globalValidatorConfigValidator = globalValidatorConfigValidator;
            
            // 初始化服务器配置验证规则
            InitializeServerConfigValidationRules();
        }

        /// <summary>
        /// 验证配置对象（集成FluentValidation验证器）
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

                // 使用FluentValidation验证器进行验证
                var validator = GetValidator<T>();
                if (validator != null)
                {
                    var validationResult = validator.Validate(config);
                    
                    // 转换FluentValidation结果为ConfigValidationResult
                    foreach (var error in validationResult.Errors)
                    {
                        result.AddError(error.PropertyName, error.ErrorMessage);
                    }
                }
                else
                {
                    _logger.LogWarning("未找到类型 {ConfigType} 的验证器，使用默认验证逻辑", typeof(T).Name);
                    
                    // 如果没有对应的验证器，仍然使用原有的验证逻辑作为后备
                    ValidateLegacyRules(config, result);
                }
                
                // 执行额外的业务逻辑验证
                ValidateBusinessRules(config, result);
                
                if (!result.IsValid)
                {
                    _logger.LogWarning("配置验证失败: {ConfigType}\n{Errors}", 
                        typeof(T).Name, result.GetErrorMessage());
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
        /// 执行旧的验证规则（作为后备机制）
        /// </summary>
        private ConfigValidationResult ValidateLegacyRules<T>(T config, ConfigValidationResult result) where T : BaseConfig
        {
            // 获取配置类型
            Type configType = typeof(T);
            string typeKey = configType.FullName;
            
            // 获取所有属性
            PropertyInfo[] properties = configType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo property in properties)
            {
                // 获取属性值
                object value = property.GetValue(config);
            
                
                // 执行自定义规则验证
                ValidateCustomRules(typeKey, property.Name, value, result);
                
               
            }
            
            return result;
        }

        /// <summary>
        /// 异步验证配置对象（集成FluentValidation验证器）
        /// </summary>
        public async Task<ConfigValidationResult> ValidateConfigAsync<T>(T config) where T : BaseConfig
        {
            try
            {
                var result = new ConfigValidationResult();
                
                if (config == null)
                {
                    result.AddGlobalError("配置对象不能为空");
                    return await Task.FromResult(result);
                }

                // 使用FluentValidation验证器进行异步验证
                var validator = GetValidator<T>();
                if (validator != null)
                {
                    var validationResult = await validator.ValidateAsync(config);
                    
                    // 转换FluentValidation结果为ConfigValidationResult
                    foreach (var error in validationResult.Errors)
                    {
                        result.AddError(error.PropertyName, error.ErrorMessage);
                    }
                }
                else
                {
                    _logger.LogWarning("未找到类型 {ConfigType} 的验证器，使用默认验证逻辑", typeof(T).Name);
                    
                    // 如果没有对应的验证器，仍然使用原有的验证逻辑作为后备
                    ValidateLegacyRules(config, result);
                }
                
                // 执行额外的业务逻辑验证
                ValidateBusinessRules(config, result);
                
                if (!result.IsValid)
                {
                    _logger.LogWarning("配置验证失败: {ConfigType}\n{Errors}", 
                        typeof(T).Name, result.GetErrorMessage());
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "配置异步验证过程发生错误");
                var errorResult = new ConfigValidationResult();
                errorResult.AddGlobalError("配置异步验证过程发生错误: " + ex.Message);
                return errorResult;
            }
        }
        
        /// <summary>
        /// 获取指定配置类型的FluentValidation验证器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>FluentValidation验证器实例</returns>
        public IValidator<T> GetValidator<T>() where T : BaseConfig
        {
            Type configType = typeof(T);
            
            // 根据配置类型返回对应的验证器
            if (configType == typeof(ServerConfig))
            {
                return _serverConfigValidator as IValidator<T>;
            }
            else if (configType == typeof(SystemGlobalconfig))
            {
                return _systemGlobalconfigValidator as IValidator<T>;
            }
            else if (configType == typeof(GlobalValidatorConfig))
            {
                return _globalValidatorConfigValidator as IValidator<T>;
            }
            
            // 如果没有找到对应的验证器，返回null
            _logger.Debug("未找到类型 {ConfigType} 的验证器", configType.Name);
            return null;
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
               
                
                // 执行自定义规则验证
                string typeKey = typeof(T).FullName;
                ValidateCustomRules(typeKey, propertyName, value, result);
                
        
                
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
            _logger.Debug("注册自定义验证规则: {RuleName} 用于 {ConfigType}.{PropertyName}", 
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
            
            // 验证服务器配置中的文件存储路径
            if (config is ServerConfig serverConfig)
            {
                // 验证FileStoragePath是否为空
                if (string.IsNullOrEmpty(serverConfig.FileStoragePath))
                {
                    result.AddError("FileStoragePath", "文件存储路径(FileStoragePath)不能为空");
                }
                else
                {
                    try
                    {
                        // 解析环境变量
                        string resolvedPath = Environment.ExpandEnvironmentVariables(serverConfig.FileStoragePath);
                        
                        if (string.IsNullOrEmpty(resolvedPath))
                        {
                            result.AddError("FileStoragePath", $"无法解析环境变量路径: {serverConfig.FileStoragePath}");
                        }
                        
                        // 验证路径格式是否有效
                        if (!Path.IsPathRooted(resolvedPath))
                        {
                            result.AddError("FileStoragePath", $"文件存储路径必须是绝对路径: {resolvedPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        result.AddError("FileStoragePath", $"路径格式验证失败: {ex.Message}");
                    }
                }
                
              
            }
            
            // 其他业务逻辑验证...
            
            return result;
        }
        
        /// <summary>
        /// 验证文件分类路径格式
        /// </summary>
        private void ValidateCategoryPathFormat(string categoryPath, string propertyName, ConfigValidationResult result)
        {
            if (!string.IsNullOrEmpty(categoryPath))
            {
                // 分类路径应该是相对路径，不应该包含根路径
                if (System.IO.Path.IsPathRooted(categoryPath))
                {
                    result.AddError(propertyName, $"文件分类路径必须是相对路径: {categoryPath}");
                }
                
                // 验证路径中是否包含无效字符
                char[] invalidChars = System.IO.Path.GetInvalidPathChars();
                if (categoryPath.IndexOfAny(invalidChars) >= 0)
                {
                    result.AddError(propertyName, $"文件分类路径包含无效字符: {categoryPath}");
                }
            }
        }
        
        /// <summary>
        /// 验证文件存储路径
        /// </summary>
        private bool ValidateFileStoragePath(string path, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            if (string.IsNullOrEmpty(path))
            {
                errorMessage = "路径不能为空";
                return false;
            }
            
            try
            {
                // 解析环境变量
                string expandedPath = Environment.ExpandEnvironmentVariables(path);
                
                // 检查路径是否为绝对路径
                if (!System.IO.Path.IsPathRooted(expandedPath))
                {
                    errorMessage = "路径必须是绝对路径";
                    return false;
                }
                
                // 检查目录是否存在
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(expandedPath);
                if (!directoryInfo.Exists)
                {
                    try
                    {
                        // 尝试创建目录
                        directoryInfo.Create();
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"无法创建目录: {ex.Message}";
                        return false;
                    }
                }
                
                // 检查目录是否可写
                string testFilePath = System.IO.Path.Combine(expandedPath, "test_write_access.txt");
                try
                {
                    using (System.IO.FileStream stream = System.IO.File.Create(testFilePath))
                    {
                        stream.WriteByte(65); // 写入字符'A'
                    }
                    System.IO.File.Delete(testFilePath);
                }
                catch (Exception ex)
                {
                    errorMessage = $"目录不可写: {ex.Message}";
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"路径验证失败: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// 验证文件分类路径
        /// </summary>
        private bool ValidateFileCategoryPath(string path, string categoryName, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            if (string.IsNullOrEmpty(path))
            {
                errorMessage = $"{categoryName}路径不能为空";
                return false;
            }
            
            try
            {
                // 解析环境变量
                string expandedPath = Environment.ExpandEnvironmentVariables(path);
                
                // 检查路径是否为绝对路径
                if (!System.IO.Path.IsPathRooted(expandedPath))
                {
                    errorMessage = $"{categoryName}路径必须是绝对路径";
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"{categoryName}路径验证失败: {ex.Message}";
                return false;
            }
        }

        /// <summary>
        /// 初始化服务器配置的验证规则
        /// </summary>
        public void InitializeServerConfigValidationRules()
        {
            // 保留自定义规则注册功能，但主要验证逻辑已迁移到FluentValidation验证器
            // 这里主要是为了兼容可能使用自定义规则的现有代码
            RegisterCustomRule<ServerConfig>(new ConfigValidationRule
            {
                RuleName = "ServerPortRule",
                PropertyName = nameof(ServerConfig.ServerPort),
                MinValue = 1,
                MaxValue = 65535,
                ErrorMessage = "服务器端口必须在1-65535范围内"
            });
            
            RegisterCustomRule<ServerConfig>(new ConfigValidationRule
            {
                //RuleName = "DbConnectionStringRequiredRule",
                //PropertyName = nameof(ServerConfig.DbConnectionString),
                //IsRequired = true,
                //ErrorMessage = "数据库连接字符串不能为空"
            });
            
            RegisterCustomRule<GlobalValidatorConfig>(new ConfigValidationRule
            {
                RuleName = "ReworkTipDaysRule",
                PropertyName = nameof(GlobalValidatorConfig.ReworkTipDays),
                MinValue = 0,
                MaxValue = 365,
                ErrorMessage = "返工提醒天数必须在0-365范围内"
            });
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
                    
                    if (!string.IsNullOrEmpty(rule.RegexPattern) && !System.Text.RegularExpressions.Regex.IsMatch(strValue, rule.RegexPattern))
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
            
            // 验证数据库连接字符串格式（简单验证）
            //if (!string.IsNullOrEmpty(config.DbConnectionString) && 
            //    !config.DbConnectionString.Contains("Server=") && 
            //    !config.DbConnectionString.Contains("Data Source="))
            //{
            //    result.AddError(nameof(config.DbConnectionString), "数据库连接字符串格式不正确，缺少服务器地址");
            //}
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
