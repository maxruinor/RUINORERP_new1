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
            
            try
            {
                var response = new GeneralResponse { Data = request.Data };
                
                // ✅ 添加超时保护，避免长时间阻塞
                using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    cts.CancelAfter(TimeSpan.FromSeconds(10)); // 10秒超时
                    
                    var systemGlobalConfig = Startup.GetFromFac<SystemGlobalConfig>();
                    var serverGlobalConfig = Startup.GetFromFac<ServerGlobalConfig>();
                    var globalValidatorConfig = Startup.GetFromFac<GlobalValidatorConfig>();
                    var performanceMonitorConfig = Startup.GetFromFac<Model.ConfigModel.PerformanceMonitorConfig>();
                    
                    List<BaseConfig> configList = new List<BaseConfig>
                    {
                        systemGlobalConfig,
                        serverGlobalConfig,
                        globalValidatorConfig,
                        performanceMonitorConfig
                    };

                    // 创建配置数据字典，同时放入Data和Metadata中确保兼容性
                    var configDataDict = new Dictionary<string, object>();
                    
                    // 序列化当前配置为JSON
                    int skippedCount = 0;
                    foreach (var config in configList)
                    {
                        // ✅ 跳过空配置实例（可能是未注册的配置）
                        if (config == null)
                        {
                            skippedCount++;
                            continue;
                        }
                        
                        string configData = JsonConvert.SerializeObject(config, Formatting.Indented);
                        
                        // 放入Data中，方便客户端直接访问
                        configDataDict[config.ConfigType] = configData;
                        
                        // 同时保留在Metadata中，保持兼容性
                        response.Metadata[config.ConfigType] = new Dictionary<string, object>
                        {
                            { config.ConfigType, configData }
                        };
                    }
                    
                    // ✅ 只在有跳过配置时记录一条Debug日志（避免频繁I/O）
                    if (skippedCount > 0)
                    {
                        _logger?.LogDebug("配置同步跳过{SkippedCount}个空配置实例", skippedCount);
                    }
                    
                    // 将配置数据字典作为响应的Data
                    response.Data = configDataDict;

                    // 通用数据传输处理
                    response.IsSuccess = true;
                    response.Message = $"数据传输成功，返回了{configDataDict.Count}个配置";
                    
                    // ✅ 详细日志记录（仅在Debug级别）
                    _logger?.LogDebug("[ConfigSync] 配置同步响应已准备 - ConfigCount: {ConfigCount}, ResponseType: {ResponseType}",
                        configDataDict.Count, typeof(GeneralResponse).Name);
                }
                
                return response;
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken.IsCancellationRequested == false)
            {
                // ✅ 内部超时取消
                _logger?.LogWarning(ex, "处理配置同步请求超时（10秒）");
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理配置同步请求超时");
            }
            catch (Exception ex) 
            { 
                _logger?.LogError(ex, "数据传输失败");
                var response = new GeneralResponse 
                { 
                    IsSuccess = false, 
                    ErrorMessage = ex.Message,
                    Data = request.Data
                };
                return response;
            } 
        } 
    }
}