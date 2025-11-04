using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.UI.SysConfig;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using RUINORERP.UI.SysConfig;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 配置命令处理器
    /// 负责处理与配置相关的命令，如配置同步等
    /// </summary>
    [ClientCommandHandler("ConfigCommandHandler", 60)]
    public class ConfigCommandHandler : BaseClientCommandHandler
    {
        private readonly OptionsMonitorConfigManager _optionsMonitorConfigManager;
        private readonly ILogger<ConfigCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="optionsMonitorConfigManager">配置管理器</param>
        /// <param name="logger">日志记录器</param>
        public ConfigCommandHandler(OptionsMonitorConfigManager optionsMonitorConfigManager, ILogger<ConfigCommandHandler> logger)
            : base(logger)
        {
            _optionsMonitorConfigManager = optionsMonitorConfigManager ?? throw new System.ArgumentNullException(nameof(optionsMonitorConfigManager));
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
        private Task HandleConfigSyncCommandAsync(PacketModel packet)
        {
            try
            {
                if (packet.Request is GeneralRequest generalRequest)
                {
                    // 尝试将数据解析为包含ConfigType和ConfigData的对象
                    // 匹配服务器端BroadcastConfigChange方法的数据结构
                    dynamic requestData = generalRequest.Data;
                    if (requestData == null)
                    {
                        _logger.LogWarning("配置同步命令数据为空");
                        return Task.CompletedTask;
                    }

                    string configType = null;
                    string configDataJson = null;
                    string version = null;
                    bool forceApply = false;

                    // 根据数据类型选择不同的解析方式
                    if (requestData is Dictionary<string, object> dictData)
                    {
                        // 处理Dictionary类型数据
                        _logger.LogDebug("处理Dictionary类型的配置数据");
                        ParseDictionaryConfigData(dictData, ref configType, ref configDataJson, ref version, ref forceApply);
                    }
                    else
                    {
                        // 尝试从动态对象中提取配置类型和数据
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
                            // 如果动态类型解析失败，尝试其他方式
                            TryAlternativeParsing(generalRequest, packet, ref configType, ref configDataJson);
                        }
                    }

                    if (string.IsNullOrEmpty(configType) || string.IsNullOrEmpty(configDataJson))
                    {
                        _logger.LogWarning("配置同步命令缺少必要的配置类型或配置数据");
                        return Task.CompletedTask;
                    }

                    _logger.LogInformation($"开始处理配置同步，配置类型: {configType}");
                    _logger.LogDebug($"配置版本: {version}, 强制应用: {forceApply}");

                    // 调用OptionsMonitorConfigManager处理配置同步（新的配置管理器）
                    _optionsMonitorConfigManager.HandleConfigSync(configType, configDataJson);
                    
                    // 同时调用ConfigManager的实例来处理配置（确保向后兼容）
                    ConfigManager.Instance.HandleConfigSync(configType, configDataJson);
                    
                    _logger.LogInformation($"配置同步已处理，配置类型: {configType}");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "处理配置同步命令时发生异常");
            }

            return Task.CompletedTask;
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