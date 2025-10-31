using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.IServices;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Services.BizCode;


namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 业务编码生成命令处理器 - 处理各种业务单据编号、产品SKU码、员工编号等生成请求
    /// </summary>
    [CommandHandler("BizCodeCommandHandler", priority: 50)]
    public class BizCodeCommandHandler : BaseCommandHandler
    {
        private readonly ILogger<BizCodeCommandHandler> _logger;
        private readonly BizCodeService _bizCodeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BizCodeCommandHandler(ILogger<BizCodeCommandHandler> logger, BizCodeService bizCodeService) : base(logger)
        {
            _logger = logger;
            _bizCodeService = bizCodeService;

            // 设置支持的命令
            SetSupportedCommands(
                BizCodeCommands.GenerateBizBillNo,
                BizCodeCommands.GenerateBaseInfoNo,
                BizCodeCommands.GenerateProductNo,
                BizCodeCommands.GenerateProductSKUNo
            );
        }

        /// <summary>
        /// 核心命令处理方法
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            _logger?.LogDebug("接收到业务编码生成命令: {CommandId}", cmd.Packet.CommandId);

            try
            {
                var commandId = cmd.Packet.CommandId;

                if (cmd.Packet.Request is BizCodeRequest request)
                {
                    if (commandId == BizCodeCommands.GenerateBizBillNo)
                    {
                        return await HandleGenerateBizBillNoAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == BizCodeCommands.GenerateBaseInfoNo)
                    {
                        return await HandleGenerateBaseInfoNoAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == BizCodeCommands.GenerateProductNo)
                    {
                        return await HandleGenerateProductNoAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == BizCodeCommands.GenerateProductSKUNo)
                    {
                        return await HandleGenerateProductSKUNoAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                }

                _logger?.LogWarning("接收到不支持的业务编码命令: {CommandId}", commandId);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的业务编码命令: {commandId}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理业务编码命令时出错，命令: {CommandId}", cmd.Packet.CommandId);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理业务编码命令 {cmd.Packet.CommandId} 时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理生成业务单据编号请求
        /// </summary>
        private async Task<IResponse> HandleGenerateBizBillNoAsync(BizCodeRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 使用业务编码服务生成编号
                string billNo = _bizCodeService.GenerateBizBillNo(request.BizType, request.BizCodePara);
                
                _logger?.LogInformation($"成功生成业务单据编号: {billNo}, 业务类型: {request.BizType}");
                
                // 返回成功响应
                return new BizCodeResponse
                {
                    IsSuccess = true,
                    GeneratedCode = billNo,
                    Message = "业务单据编号生成成功"
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "生成业务单据编号失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "生成业务单据编号失败"
                };
            }
        }

        /// <summary>
        /// 处理生成基础信息编号请求
        /// </summary>
        private async Task<IResponse> HandleGenerateBaseInfoNoAsync(BizCodeRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                string baseInfoNo;
                if (!string.IsNullOrEmpty(request.ParaConst))
                {
                    // 使用常量参数生成编号
                    baseInfoNo = _bizCodeService.GenerateBaseInfoNo(request.BaseInfoType, request.ParaConst);
                }
                else
                {
                    // 使用默认方式生成编号
                    baseInfoNo = _bizCodeService.GenerateBaseInfoNo(request.BaseInfoType);
                }
                
                _logger?.LogInformation($"成功生成基础信息编号: {baseInfoNo}, 信息类型: {request.BaseInfoType}");
                
                // 返回成功响应
                return new BizCodeResponse
                {
                    IsSuccess = true,
                    GeneratedCode = baseInfoNo,
                    Message = "基础信息编号生成成功"
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "生成基础信息编号失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "生成基础信息编号失败"
                };
            }
        }

        /// <summary>
        /// 处理生成产品编码请求
        /// </summary>
        private async Task<IResponse> HandleGenerateProductNoAsync(BizCodeRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 生成产品编码
                string productNo = _bizCodeService.GenerateProductNo();
                
                _logger?.LogInformation($"成功生成产品编码: {productNo}");
                
                // 返回成功响应
                return new BizCodeResponse
                {
                    IsSuccess = true,
                    GeneratedCode = productNo,
                    Message = "产品编码生成成功"
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "生成产品编码失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "生成产品编码失败"
                };
            }
        }

        /// <summary>
        /// 处理生成产品SKU编码请求
        /// </summary>
        private async Task<IResponse> HandleGenerateProductSKUNoAsync(BizCodeRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 生成产品SKU编码
                string productSkuNo = _bizCodeService.GenerateProductSKUNo();
                
                _logger?.LogInformation($"成功生成产品SKU编码: {productSkuNo}");
                
                // 返回成功响应
                return new BizCodeResponse
                {
                    IsSuccess = true,
                    GeneratedCode = productSkuNo,
                    Message = "产品SKU编码生成成功"
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "生成产品SKU编码失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "生成产品SKU编码失败"
                };
            }
        }
       
     
    }
}