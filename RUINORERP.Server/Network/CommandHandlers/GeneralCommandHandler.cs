using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model.ConfigModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests; 
using RUINORERP.PacketSpec.Models.Responses; 
using RUINORERP.Server.Network.Models; 
using System;
using System.Collections.Generic;
using System.Threading; 
using System.Threading.Tasks; 

namespace RUINORERP.Server.Network.CommandHandlers 
{ 
    /// <summary> 
    /// 通用命令处理器 - 处理所有通用相关的网络命令 
    /// 这是一个通讯模块，用于处理简单的数据传输，具体业务逻辑由其他模块实现
    /// </summary> 
    [CommandHandler("GeneralCommandHandler", priority: 100)]
    public class GeneralCommandHandler : BaseCommandHandler 
    { 
        private readonly ILogger<GeneralCommandHandler> _logger; 

        /// <summary> 
        /// 构造函数 
        /// </summary> 
        public GeneralCommandHandler(ILogger<GeneralCommandHandler> logger = null) 
            : base(logger) 
        { 
            _logger = logger; 
            
            // 设置支持的命令 
            SetSupportedCommands(
                GeneralCommands.ConfigSync
            );
        } 

        /// <summary> 
        /// 核心命令处理方法 
        /// </summary> 
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken) 
        { 
            _logger?.LogDebug("接收到通用命令: {CommandId}", cmd.Packet.CommandId);
            
            try 
            { 
                var commandId = cmd.Packet.CommandId;
                
                if (cmd.Packet.Request is GeneralRequest request && commandId == GeneralCommands.ConfigSync)
                {
                    return await HandleDataTransferAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else
                {
                    _logger?.LogWarning("接收到不支持的通用命令: {CommandId}", commandId);
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的通用命令: {commandId}");
                }
            } 
            catch (Exception ex) 
            { 
                _logger?.LogError(ex, "处理通用命令时出错，命令: {CommandId}", cmd.Packet.CommandId);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理通用命令 {cmd.Packet.CommandId} 时出错: {ex.Message}");
            } 
        } 

        /// <summary> 
        /// 处理数据传输请求 
        /// </summary> 
        private async Task<IResponse> HandleDataTransferAsync(GeneralRequest request, CommandContext executionContext, CancellationToken cancellationToken) 
        { 
            _logger?.LogDebug("开始处理数据传输请求");
            var response = new GeneralResponse { Data = request.Data };
            var systemGlobalConfig = Startup.GetFromFac<SystemGlobalConfig>();
            var serverGlobalConfig = Startup.GetFromFac<ServerGlobalConfig>();
            var globalValidatorConfig = Startup.GetFromFac<GlobalValidatorConfig>();
            List<BaseConfig> configList = new List<BaseConfig>
            {
                systemGlobalConfig,
                serverGlobalConfig,
                globalValidatorConfig
            };

            // 序列化当前配置为JSON
            foreach (var config in configList)
            {
                string configData = JsonConvert.SerializeObject(config, Formatting.Indented);
                response.Metadata[config.ConfigType] = new Dictionary<string, object>
                {
                    { config.ConfigType, configData }
                };
            }

            try 
            { 
                // 通用数据传输处理
                // 这里只是简单地返回成功响应，具体业务逻辑由其他模块实现
                response.IsSuccess = true;
                response.Message = "数据传输成功";
                _logger?.LogInformation($"数据传输成功"); 
            } 
            catch (Exception ex) 
            { 
                response.IsSuccess = false; 
                response.ErrorMessage = ex.Message; 
                _logger?.LogError(ex, "数据传输失败"); 
            } 
            
            return response; 
        } 
    }
}