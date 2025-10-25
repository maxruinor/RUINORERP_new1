using FluentValidation;
using RUINORERP.Model.ConfigModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置验证服务接口
    /// 提供配置验证和错误处理功能
    /// 集成FluentValidation验证器以提供更强大的验证能力
    /// </summary>
    public interface IConfigValidationService
    {
        /// <summary>
        /// 验证配置对象（集成FluentValidation验证器）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>验证结果</returns>
        ConfigValidationResult ValidateConfig<T>(T config) where T : BaseConfig;

        /// <summary>
        /// 异步验证配置对象（集成FluentValidation验证器）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>验证结果</returns>
        Task<ConfigValidationResult> ValidateConfigAsync<T>(T config) where T : BaseConfig;

        /// <summary>
        /// 验证单个属性值
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>验证错误信息，如果没有错误则为空</returns>
        string ValidateProperty<T>(T config, string propertyName, object value) where T : BaseConfig;

        /// <summary>
        /// 获取配置的所有验证规则
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>验证规则列表</returns>
        List<ConfigValidationRule> GetValidationRules<T>() where T : BaseConfig;

        /// <summary>
        /// 注册自定义验证规则
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="rule">验证规则</param>
        void RegisterCustomRule<T>(ConfigValidationRule rule) where T : BaseConfig;

        /// <summary>
        /// 验证配置是否可应用（业务逻辑验证）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>验证结果</returns>
        ConfigValidationResult ValidateConfigApplication<T>(T config) where T : BaseConfig;
        
        /// <summary>
        /// 初始化服务器配置的验证规则
        /// </summary>
        void InitializeServerConfigValidationRules();
        
        /// <summary>
        /// 获取指定配置类型的FluentValidation验证器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>FluentValidation验证器实例</returns>
        IValidator<T> GetValidator<T>() where T : BaseConfig;
    }
}
