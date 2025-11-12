using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using RUINORERP.Business.Config;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 配置命令处理器
    /// 负责处理与配置相关的命令，如配置同步等
    /// </summary>
    [ClientCommandHandler("ConfigCommandHandler", 60)]
    public class ConfigCommandHandler : BaseClientCommandHandler
    {
        private readonly IConfigManagerService _configManagerService;
        private readonly ILogger<ConfigCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optionsMonitorConfigManager">配置管理器</param>
        /// <param name="logger">日志记录器</param>
        public ConfigCommandHandler(IConfigManagerService configManagerService, ILogger<ConfigCommandHandler> logger)
            : base(logger)
        {
            _configManagerService = configManagerService ?? throw new System.ArgumentNullException(nameof(configManagerService));
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));

            // 保留通过SetSupportedCommands方法设置命令的方式
            SetSupportedCommands(GeneralCommands.ConfigSync);
        }



        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public override async Task<bool> InitializeAsync()
        {
            bool initialized = await base.InitializeAsync();
            if (initialized)
            {
                _logger.LogDebug("配置命令处理器初始化成功");
            }
            return initialized;
        }

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        public override async Task HandleAsync(PacketModel packet)
        {
            if (packet == null || packet.CommandId == null)
            {
                _logger.LogError("收到无效的数据包");
                return;
            }

            // 根据命令ID处理不同的配置命令
            // 使用if-else替代switch，因为FullCode是计算属性而非常量
            if (packet.CommandId == GeneralCommands.ConfigSync)
            {
                await HandleConfigSyncCommandAsync(packet);
            }
            else
            {
                _logger.LogWarning($"未处理的配置命令ID: {packet.CommandId.FullCode}");
            }
        }

        /// <summary>
        /// 处理配置同步命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        private async Task HandleConfigSyncCommandAsync(PacketModel packet)
        {
            try
            {
                if (packet.Request is GeneralRequest generalRequest)
                {
                    string configType = null;
                    string configDataJson = null;
                    bool forceApply = false;
                    string version = null;

                    // 处理不同格式的请求数据
                    if (generalRequest.Data is Dictionary<string, object> commandData)
                    {
                        // 从主命令数据中提取配置信息
                        if (!TryExtractConfigData(commandData, out configType, out configDataJson))
                        {
                            // 如果主命令数据中没有找到，尝试从Parameters中提取
                            if (commandData.TryGetValue("Parameters", out object parametersObj) &&
                                parametersObj is Dictionary<string, object> parametersDict)
                            {
                                if (!TryExtractConfigData(parametersDict, out configType, out configDataJson))
                                {
                                    _logger.LogWarning("配置同步命令缺少必要参数 ConfigType 或 ConfigData");
                                    return;
                                }
                                _logger.LogDebug($"从Parameters获取配置同步数据: {configType}");
                            }
                            else
                            {
                                _logger.LogWarning("配置同步命令缺少必要参数 ConfigType 或 ConfigData");
                                return;
                            }
                        }
                        else
                        {
                            _logger.LogDebug($"收到配置同步命令: {configType}");
                        }

                        // 检查是否需要强制应用配置
                        if (commandData.TryGetValue("ForceApply", out object forceApplyObj))
                        {
                            bool.TryParse(forceApplyObj?.ToString(), out forceApply);
                        }

                        // 检查配置版本信息
                        if (commandData.TryGetValue("Version", out object versionObj))
                        {
                            version = versionObj.ToString();
                            _logger.LogDebug($"配置版本: {version}");
                        }
                    }
                    else
                    {
                        // 尝试使用原有的解析方式作为备用
                        dynamic requestData = generalRequest.Data;
                        if (requestData != null)
                        {
                            try
                            {
                                configType = requestData.ConfigType?.ToString();
                                configDataJson = requestData.ConfigData?.ToString();
                                version = requestData.Version?.ToString();
                                forceApply = requestData.ForceApply ?? false;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning($"解析配置数据结构失败: {ex.Message}");
                                TryAlternativeParsing(generalRequest, packet, ref configType, ref configDataJson);
                            }

                            if (string.IsNullOrEmpty(configType) || string.IsNullOrEmpty(configDataJson))
                            {
                                _logger.LogWarning("配置同步命令缺少必要的配置类型或配置数据");
                                return;
                            }
                        }
                    }

                    _logger.LogDebug($"开始处理配置同步，配置类型: {configType}");
                    _logger.LogDebug($"配置版本: {version}, 强制应用: {forceApply}");

                    // 完全使用新的配置管理服务处理配置同步，移除对过时的UIConfigManager的依赖
                    try
                    {
                        _logger.LogDebug("使用新的配置管理服务处理配置同步，配置类型: {ConfigType}", configType);

                        // 根据配置类型使用相应的泛型方法
                        switch (configType)
                        {
                            case "SystemGlobalConfig":
                                var systemConfig = await _configManagerService.LoadConfigFromJsonAsync<SystemGlobalConfig>(configDataJson);
                                // 持久化配置到文件系统
                                bool systemConfigSaved = await _configManagerService.SaveConfigAsync(systemConfig);
                                if (systemConfigSaved)
                                {
                                    _logger.LogDebug("系统全局配置同步并持久化成功");
                                    // 刷新配置，确保所有依赖该配置的服务获取到最新值
                                    await _configManagerService.RefreshConfigAsync<SystemGlobalConfig>();
                                }
                                else
                                {
                                    _logger.LogError("系统全局配置同步失败，无法持久化");
                                }
                                break;
                            case "GlobalValidatorConfig":
                                var validatorConfig = await _configManagerService.LoadConfigFromJsonAsync<GlobalValidatorConfig>(configDataJson);
                                // 持久化配置到文件系统
                                bool validatorConfigSaved = await _configManagerService.SaveConfigAsync(validatorConfig);
                                if (validatorConfigSaved)
                                {
                                    _logger.LogDebug("验证配置同步并持久化成功");
                                    // 刷新配置，确保所有依赖该配置的服务获取到最新值
                                    GlobalValidatorConfig globalValidatorConfig = await _configManagerService.RefreshConfigAsync<GlobalValidatorConfig>();
                                    // 获取容器中的GlobalValidatorConfig实例并更新其属性
                                    var containerGlobalConfig = Startup.GetFromFac<GlobalValidatorConfig>();
                                    if (containerGlobalConfig != null)
                                    {
                                        // 使用反射或映射将新配置的值复制到容器中的实例
                                        foreach (var property in typeof(GlobalValidatorConfig).GetProperties())
                                        {
                                            if (property.CanRead && property.CanWrite)
                                            {
                                                var value = property.GetValue(globalValidatorConfig);
                                                property.SetValue(containerGlobalConfig, value);
                                            }
                                        }
                                    }
                                    _logger.LogDebug("GlobalValidatorConfig已在容器中更新");
                                }
                                else
                                {
                                    _logger.LogError("验证配置同步失败，无法持久化");
                                }

                            
                                break;
                            case "Database":
                                // 数据库配置特殊处理，这里可以根据实际需求实现
                                _logger.LogDebug("数据库配置同步请求已接收");
                                break;
                            default:
                                _logger.LogWarning("未知的配置类型: {ConfigType}", configType);

                                // 尝试作为自定义配置处理
                                try
                                {
                                    // 对于自定义配置，我们可以记录日志并提供扩展点
                                    _logger.LogDebug("尝试处理自定义配置类型: {ConfigType}", configType);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, "处理自定义配置类型失败: {ConfigType}", configType);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "使用配置管理服务处理配置同步时发生异常，配置类型: {ConfigType}", configType);
                    }

                    _logger.LogDebug($"配置同步已处理，配置类型: {configType}");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "处理配置同步命令时发生异常");
            }
        }

        /// <summary>
        /// 尝试从字典中提取配置类型和数据
        /// </summary>
        /// <param name="dataDict">数据字典</param>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON字符串</param>
        /// <returns>是否成功提取</returns>
        private bool TryExtractConfigData(Dictionary<string, object> dataDict, out string configType, out string configDataJson)
        {
            configType = null;
            configDataJson = null;

            try
            {
                // 提取配置类型
                if (!dataDict.TryGetValue("ConfigType", out object configTypeObj))
                {
                    return false;
                }

                configType = configTypeObj.ToString();

                // 提取配置数据
                if (!dataDict.TryGetValue("ConfigData", out object configDataObj))
                {
                    return false;
                }

                // 使用Newtonsoft.Json序列化
                if (configDataObj is Dictionary<string, object> configDataDict)
                {
                    configDataJson = JsonConvert.SerializeObject(configDataDict);
                }
                else if (configDataObj is string)
                {
                    // 如果已经是JSON字符串，直接使用
                    configDataJson = configDataObj.ToString();
                }
                else
                {
                    // 其他类型对象直接序列化
                    configDataJson = JsonConvert.SerializeObject(configDataObj);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提取配置数据失败");
                return false;
            }
        }

        /// <summary>
        /// 从Dictionary中解析配置数据
        /// </summary>
        /// <param name="dictData">Dictionary数据</param>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON字符串</param>
        /// <param name="version">版本信息</param>
        /// <param name="forceApply">是否强制应用</param>
        private void ParseDictionaryConfigData(Dictionary<string, object> dictData, ref string configType, ref string configDataJson, ref string version, ref bool forceApply)
        {
            // 从Dictionary中提取配置类型
            if (dictData.TryGetValue("ConfigType", out var typeValue))
            {
                configType = typeValue?.ToString();
            }

            // 从Dictionary中提取配置数据
            if (dictData.TryGetValue("ConfigData", out var dataValue))
            {
                // 检查数据类型，如果是对象则序列化为JSON
                if (dataValue is Newtonsoft.Json.Linq.JObject || dataValue is ExpandoObject || dataValue is IDictionary<string, object>)
                {
                    configDataJson = JsonConvert.SerializeObject(dataValue);
                }
                else
                {
                    configDataJson = dataValue?.ToString();
                }
            }

            // 从Dictionary中提取版本信息
            if (dictData.TryGetValue("Version", out var versionValue))
            {
                version = versionValue?.ToString();
            }

            // 从Dictionary中提取强制应用标志
            if (dictData.TryGetValue("ForceApply", out var forceValue))
            {
                bool.TryParse(forceValue?.ToString(), out forceApply);
            }
        }

        /// <summary>
        /// 尝试备选的解析方式
        /// </summary>
        /// <param name="generalRequest">通用请求</param>
        /// <param name="packet">数据包</param>
        /// <param name="configType">配置类型</param>
        /// <param name="configDataJson">配置数据JSON字符串</param>
        private void TryAlternativeParsing(GeneralRequest generalRequest, PacketModel packet, ref string configType, ref string configDataJson)
        {
            _logger.LogWarning("无法从请求数据中提取配置信息，尝试备用解析方式");

            // 尝试从Parameters中获取配置数据
            if (packet.Request is RequestBase requestBase)
            {
                if (requestBase.Parameters?.TryGetValue("ConfigData", out var attachConfigData) ?? false)
                {
                    configDataJson = attachConfigData.ToString();
                }

                // 尝试将整个Data作为配置类型
                if (string.IsNullOrEmpty(configType))
                {
                    configType = generalRequest.Data.ToString();
                }
            }
        }
    }
}