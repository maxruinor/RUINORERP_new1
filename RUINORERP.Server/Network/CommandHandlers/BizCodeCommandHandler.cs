using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.IServices;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Services.BizCode;
using RUINORERP.PacketSpec.Models.BizCodeGenerate;
using RUINORERP.PacketSpec.Models.Common;


namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 业务编码生成命令处理器 - 处理各种业务单据编号、产品SKU码、员工编号等生成请求
    /// </summary>
    [CommandHandler("BizCodeCommandHandler", priority: 50)]
    public class BizCodeCommandHandler : BaseCommandHandler
    {
        private readonly ILogger<BizCodeCommandHandler> logger;
        private readonly ServerBizCodeGenerateService _bizCodeService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BizCodeCommandHandler(ILogger<BizCodeCommandHandler> logger, ServerBizCodeGenerateService bizCodeService) : base(logger)
        {
            this.logger = logger;
            _bizCodeService = bizCodeService;

            // 设置支持的命令
            SetSupportedCommands(
                BizCodeCommands.GenerateBizBillNo,
                BizCodeCommands.GenerateBaseInfoNo,
                BizCodeCommands.GenerateProductRelatedCode,
                BizCodeCommands.GenerateBarCode
            );
        }

        /// <summary>
        /// 核心命令处理方法
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            logger?.LogDebug("接收到业务编码生成命令: {CommandId}", cmd.Packet.CommandId);

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
                    else if (commandId == BizCodeCommands.GenerateProductRelatedCode)
                    {
                        return await HandleGenerateProductRelatedAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                    else if (commandId == BizCodeCommands.GenerateBarCode)
                    {
                        return await HandleGenerateBarCodeAsync(request, cmd.Packet.ExecutionContext, cancellationToken);
                    }
                }

                logger?.LogWarning("接收到不支持的业务编码命令: {CommandId}", commandId);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的业务编码命令: {commandId}");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理业务编码命令时出错，命令: {CommandId}", cmd.Packet.CommandId);
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
                string billNo = await _bizCodeService.GenerateBizBillNoAsync(request.BizType);

                logger?.LogDebug($"成功生成业务单据编号: {billNo}, 业务类型: {request.BizType}");

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
                logger?.LogError(ex, "生成业务单据编号失败");
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
                // 尝试将字符串转换为枚举类型

                string baseInfoNo;
                baseInfoNo = await _bizCodeService.GenerateBaseInfoNoAsync(request.BaseInfoType, request.ParaConst);
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
                logger?.LogError(ex, "生成基础信息编号失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "生成基础信息编号失败"
                };
            }
        }


        /// <summary>
        /// 处理生成基础信息编号请求
        /// </summary>
        private async Task<IResponse> HandleGenerateProductRelatedAsync(BizCodeRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 尝试将字符串转换为枚举类型
                if (request.BaseInfoType == BaseInfoType.SKU_No)
                {
                    string baseInfoNo = string.Empty;

                    // 使用常量参数生成编号
                    if (request.ProductParameter != null)
                    {
                        baseInfoNo = await _bizCodeService.GenerateProductSKUCodeAsync(request.BaseInfoType, request.ProductParameter.prod, request.ProductParameter.prodDetail);
                    }
                    // 返回成功响应
                    return new BizCodeResponse
                    {
                        IsSuccess = true,
                        GeneratedCode = baseInfoNo,
                        Message = "产品相关信息编号生成成功"
                    };
                }
                else
                {
                    string baseInfoNo = string.Empty;

                    // 使用常量参数生成编号
                    if (request.ProductParameter != null)
                    {
                        baseInfoNo = await _bizCodeService.GenerateProductRelatedCodeAsync(request.BaseInfoType, request.ProductParameter.prod, request.ParaConst);
                    }
                    // 返回成功响应
                    return new BizCodeResponse
                    {
                        IsSuccess = true,
                        GeneratedCode = baseInfoNo,
                        Message = "产品相关信息编号生成成功"
                    };
                }


            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "产品相关信息编号生成失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "产品相关信息编号生成失败"
                };
            }
        }


        /// <summary>
        /// 处理生成条码请求
        /// </summary>
        private async Task<IResponse> HandleGenerateBarCodeAsync(BizCodeRequest request, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 验证条码参数
                if (request.BarCodeParameter == null || string.IsNullOrEmpty(request.BarCodeParameter.OriginalCode))
                {
                    throw new ArgumentException("条码生成参数不完整，缺少原始编码");
                }

                // 生成条码
                string barcode = await _bizCodeService.GenerateBarCodeAsync(request.BarCodeParameter.OriginalCode,
                    request.BarCodeParameter.PaddingChar);


                // 返回成功响应
                return new BizCodeResponse
                {
                    IsSuccess = true,
                    GeneratedCode = barcode,
                    Message = "条码生成成功"
                };
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "生成条码失败");
                return new BizCodeResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                    Message = "生成条码失败"
                };
            }
        }

    }
}