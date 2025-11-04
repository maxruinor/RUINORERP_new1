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

                    // 从动态对象中提取配置类型和数据
                    try
                    {
                        configType = requestData.ConfigType?.ToString();
                        configDataJson = requestData.ConfigData?.ToString();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"解析配置数据结构失败: {ex.Message}");
                    }

                    // 如果解析失败，尝试使用旧格式作为备选
                    if (string.IsNullOrEmpty(configType) || string.IsNullOrEmpty(configDataJson))
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

                    if (string.IsNullOrEmpty(configType) || string.IsNullOrEmpty(configDataJson))
                    {
                        _logger.LogWarning("配置同步命令缺少必要的配置类型或配置数据");
                        return Task.CompletedTask;
                    }

                    _logger.LogInformation($"开始处理配置同步，配置类型: {configType}");

                    // 调用OptionsMonitorConfigManager处理配置同步
                    // 注意：HandleConfigSync是同步方法，不需要await
                    _optionsMonitorConfigManager.HandleConfigSync(configType, configDataJson);
                    _logger.LogInformation($"配置同步已处理，配置类型: {configType}");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "处理配置同步命令时发生异常");
            }

            return Task.CompletedTask;
        }
    }
}