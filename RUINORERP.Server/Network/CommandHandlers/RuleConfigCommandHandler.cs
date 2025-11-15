using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Services.BizCode;
using RUINORERP.Model;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using System.Threading;
using RUINORERP.PacketSpec.Models.BizCodeGenerate;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 规则配置命令处理器
    /// 处理与规则配置相关的命令请求 本地想生成时要请求规则？
    /// </summary>
    public class RuleConfigCommandHandler : BaseCommandHandler
    {
        private readonly IBizCodeGenerateService _bizCodeService;
        private readonly ILogger<RuleConfigCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bizCodeService">业务编码服务</param>
        /// <param name="logger">日志记录器</param>
        public RuleConfigCommandHandler(
            IBizCodeGenerateService bizCodeService,
            ILogger<RuleConfigCommandHandler> logger)
        {
            _bizCodeService = bizCodeService;
            _logger = logger;
        }




        /// <summary>
        /// 处理命令请求
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求对象</param>
        /// <returns>响应对象</returns>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {

            try
            {
                var commandId = cmd.Packet.CommandId;

                var bizCodeRequest = cmd.Packet.Request as BizCodeRequest;
                if (bizCodeRequest == null)
                {
                    return CreateErrorResponse("无效的请求参数");
                }

                // 根据命令ID处理不同的请求
                switch (commandId)
                {
                    case var _ when commandId == BizCodeCommands.GetAllRuleConfigs:
                        return await HandleGetAllRuleConfigsAsync(bizCodeRequest);

                    case var _ when commandId == BizCodeCommands.SaveRuleConfig:
                        return await HandleSaveRuleConfigAsync(bizCodeRequest);

                    case var _ when commandId == BizCodeCommands.DeleteRuleConfig:
                        return await HandleDeleteRuleConfigAsync(bizCodeRequest);

                    default:
                        return CreateErrorResponse($"不支持的命令: {commandId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理规则配置命令时发生异常");
                return CreateErrorResponse($"处理命令时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理获取所有规则配置请求
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>响应对象</returns>
        private async Task<BizCodeResponse> HandleGetAllRuleConfigsAsync(BizCodeRequest request)
        {
            try
            {
                var configs = await _bizCodeService.GetAllRuleConfigsAsync();

                return new BizCodeResponse
                {
                    IsSuccess = true,
                    RuleConfigs = configs,
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取规则配置列表失败");
                return CreateErrorResponse($"获取规则配置列表失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理保存规则配置请求
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>响应对象</returns>
        private async Task<BizCodeResponse> HandleSaveRuleConfigAsync(BizCodeRequest request)
        {
            try
            {
                if (request.RuleConfig == null)
                {
                    return CreateErrorResponse("规则配置不能为空");
                }

                await _bizCodeService.SaveRuleConfigAsync(request.RuleConfig);

                return new BizCodeResponse
                {
                    IsSuccess = true,
                    Message = "规则配置保存成功",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存规则配置失败");
                return CreateErrorResponse($"保存规则配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理删除规则配置请求
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>响应对象</returns>
        private async Task<BizCodeResponse> HandleDeleteRuleConfigAsync(BizCodeRequest request)
        {
            try
            {
                if (request.RuleConfigId <= 0)
                {
                    return CreateErrorResponse("无效的规则配置ID");
                }

                await _bizCodeService.DeleteRuleConfigAsync(request.RuleConfigId);

                return new BizCodeResponse
                {
                    IsSuccess = true,
                    Message = "规则配置删除成功",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除规则配置失败");
                return CreateErrorResponse($"删除规则配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 创建错误响应
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>错误响应对象</returns>
        private BizCodeResponse CreateErrorResponse(string errorMessage)
        {
            return new BizCodeResponse
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }
}