using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.BizCodeGenerate;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 业务编码服务类
    /// 提供业务单据编号、基础信息编号、产品编码和SKU编码的生成功能
    /// 采用与UserLoginService相似的设计模式，确保统一的网络通信和异常处理
    /// 同时提供静态方法以便兼容旧的调用模式
    /// </summary>
    public sealed class ClientBizCodeService : IBizCodeGenerateService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<ClientBizCodeService> _logger;
        private readonly SemaphoreSlim _operationLock = new SemaphoreSlim(1, 1); // 防止并发操作请求
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当必要参数为空时抛出</exception>
        public ClientBizCodeService(
            ClientCommunicationService communicationService,
            ILogger<ClientBizCodeService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;
        }

        /// <summary>
        /// 生成业务单据编号
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的业务单据编号</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateBizBillNoAsync(BizType bizType, CancellationToken ct = default)
        {
            var request = new BizCodeRequest { BizType = bizType };
            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateBizBillNo, request, ct);
            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }
            throw new Exception(response.ErrorMessage ?? "生成业务单据编号失败,请重试。");
        }


        /// <summary>
        /// 生成基础信息编号（枚举版本）
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateBaseInfoNoAsync(BaseInfoType baseInfoType, string paraConst = null, CancellationToken ct = default)
        {
            // 将枚举转换为字符串发送到服务器
            var request = new BizCodeRequest { BaseInfoType = baseInfoType, ParaConst = paraConst };
            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateBaseInfoNo, request, ct);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }

            throw new Exception(response.ErrorMessage ?? "生成基础信息编号失败");
        }


        /// <summary>
        /// 生成基础信息编号（枚举版本）
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="PrefixParaConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateProductRelatedCodeAsync(BaseInfoType baseInfoType, tb_Prod prod, string PrefixParaConst = null, int seqLength = 3, bool includeDate = false, CancellationToken ct = default)
        {
            // 将枚举转换为字符串发送到服务器
            var request = new BizCodeRequest { BaseInfoType = baseInfoType, ParaConst = PrefixParaConst };

            var ProductParameter = new ProdParameter
            {
                prod = prod,
                PrefixParaConst = PrefixParaConst,
                SeqLength = seqLength,
                IncludeDate = includeDate
            };

            request.ProductParameter = ProductParameter;

            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateProductRelatedCode, request, ct);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }

            throw new Exception(response.ErrorMessage ?? "生成基础信息编号失败");
        }




        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的条码</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateBarCodeAsync(string originalCode, char paddingChar = '0', CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(originalCode))
            {
                throw new ArgumentNullException(nameof(originalCode), "原始编码不能为空");
            }

            var request = new BizCodeRequest
            {
                BarCodeParameter = new BarCodeParameter
                {
                    OriginalCode = originalCode,
                    PaddingChar = paddingChar
                }
            };

            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateBarCode, request, ct);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }

            throw new Exception(response.ErrorMessage ?? "生成条码失败");
        }

        /// <summary>
        /// 获取所有规则配置
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>规则配置列表</returns>
        public async Task<List<tb_sys_BillNoRule>> GetAllRuleConfigsAsync(CancellationToken ct = default)
        {
            var request = new BizCodeRequest();
            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GetAllRuleConfigs, request, ct);

            if (response.IsSuccess && response.RuleConfigs != null)
            {
                return response.RuleConfigs;
            }

            throw new Exception(response.ErrorMessage ?? "获取规则配置失败");
        }

        /// <summary>
        /// 保存规则配置
        /// </summary>
        /// <param name="config">规则配置</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        public async Task SaveRuleConfigAsync(tb_sys_BillNoRule config, CancellationToken ct = default)
        {
            var request = new BizCodeRequest
            {
                RuleConfig = config
            };

            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.SaveRuleConfig, request, ct);

            if (!response.IsSuccess)
            {
                throw new Exception(response.ErrorMessage ?? "保存规则配置失败");
            }
        }

        /// <summary>
        /// 删除规则配置
        /// </summary>
        /// <param name="id">规则配置ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>任务</returns>
        public async Task DeleteRuleConfigAsync(long id, CancellationToken ct = default)
        {
            var request = new BizCodeRequest
            {
                RuleConfigId = id
            };

            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.DeleteRuleConfig, request, ct);

            if (!response.IsSuccess)
            {
                throw new Exception(response.ErrorMessage ?? "删除规则配置失败");
            }
        }

        /// <summary>
        /// 发送业务编码生成命令的通用方法
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>响应对象</returns>
        private async Task<BizCodeResponse> SendBizCodeCommandAsync(CommandId commandId, BizCodeRequest request, CancellationToken ct = default)
        {
            // 创建带超时的取消令牌源
            using var timeoutCts = new CancellationTokenSource();
            // 设置默认超时时间为10秒
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(10));

            // 创建组合取消令牌，任一令牌取消都会导致操作取消
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);
            var combinedCt = linkedCts.Token;

            // 使用信号量确保同一时间只有一个请求
            await _operationLock.WaitAsync(combinedCt);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
                {
                    _logger?.LogWarning("业务编码生成失败：未连接到服务器");
                    throw new Exception("未连接到服务器，请检查网络连接后重试");
                }

                // 使用重试机制发送请求
                BizCodeResponse response = null;
                int maxRetries = 2;
                int retryCount = 0;
                // 记录开始时间，用于总超时控制
                var startTime = DateTime.UtcNow;
                // 最大允许的总重试时间为10秒
                var maxTotalRetryTime = TimeSpan.FromSeconds(10);

                while (retryCount <= maxRetries)
                {
                    try
                    {
                        // 检查总重试时间是否超过限制
                        if (DateTime.UtcNow - startTime > maxTotalRetryTime)
                        {
                            _logger?.LogWarning("业务编码生成请求总重试时间超过限制 - 命令ID: {CommandId}", commandId.ToString());
                            throw new TimeoutException("请求重试时间过长，请稍后重试");
                        }

                        // 发送命令并获取响应
                        response = await _communicationService.SendCommandWithResponseAsync<BizCodeResponse>(
                            commandId, request, combinedCt);
                        break; // 成功则跳出重试循环
                    }
                    catch (Exception ex) when (IsRetryableException(ex) && retryCount < maxRetries)
                    {
                        retryCount++;
                        _logger?.LogWarning(ex, "业务编码生成请求失败，正在重试 ({RetryCount}/{MaxRetries}) - 命令ID: {CommandId}",
                            retryCount, maxRetries, commandId.ToString());
                        // 指数退避策略，增加随机因子避免重试风暴
                        int delayMs = (int)(100 * Math.Pow(2, retryCount)) + new Random().Next(0, 100);
                        await Task.Delay(delayMs, combinedCt);
                    }
                }

                // 检查响应数据是否为空
                if (response == null)
                {
                    _logger?.LogError("业务编码生成失败：服务器返回了空的响应数据");
                    throw new Exception("服务器返回了空的响应数据，请联系系统管理员");
                }

                return response;
            }
            catch (OperationCanceledException ex) when (ct.IsCancellationRequested)
            {
                _logger?.LogDebug(ex, "业务编码生成操作已被用户取消");
                throw new OperationCanceledException("操作已被用户取消", ex);
            }
            catch (OperationCanceledException ex) when (timeoutCts.Token.IsCancellationRequested)
            {
                _logger?.LogWarning(ex, "业务编码生成操作超时");
                throw new TimeoutException("业务编码生成请求超时，请检查网络连接后重试", ex);
            }
            catch (TimeoutException ex)
            {
                _logger?.LogError(ex, "业务编码生成过程中发生超时异常");
                throw;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                _logger?.LogError(ex, "业务编码生成过程中发生网络连接异常 (SocketError: {SocketError})");
                throw new Exception("网络连接错误，请检查网络后重试", ex);
            }
            catch (System.IO.IOException ex)
            {
                _logger?.LogError(ex, "业务编码生成过程中发生IO异常");
                throw new Exception("网络传输错误，请稍后重试", ex);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "业务编码生成过程中发生未预期的异常 - 命令ID: {CommandId}", commandId.ToString());
                throw new Exception("生成业务编码时发生错误，请稍后重试", ex);
            }
            finally
            {
                _operationLock.Release();
            }
        }

        /// <summary>
        /// 判断异常是否可重试
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>是否可重试</returns>
        private bool IsRetryableException(Exception ex)
        {
            return ex is TimeoutException ||
                   ex is System.Net.Sockets.SocketException ||
                   ex is System.IO.IOException ||
                   (ex is AggregateException && ex.InnerException != null && IsRetryableException(ex.InnerException)) ||
                   (ex.Message != null && (
                       ex.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       ex.Message.IndexOf("connection", StringComparison.OrdinalIgnoreCase) >= 0 ||
                       ex.Message.IndexOf("网络", StringComparison.OrdinalIgnoreCase) >= 0));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            try
            {
                _operationLock.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放BizCodeService资源时发生异常");
            }
        }

        #region 静态方法（兼容旧的调用模式）

        /// <summary>
        /// 获取条码（静态方法，兼容旧的调用模式）
        /// 内部使用同步方式调用异步方法
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <returns>生成的条码</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        public static string GetBarCode(string originalCode, char paddingChar = '0')
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取BizCodeService实例");
                }

                // 同步调用异步方法
                return Task.Run(async () => await bizCodeService.GenerateBarCodeAsync(originalCode, paddingChar)).Result;
            }
            catch (AggregateException ex)
            {
                // 解包AggregateException，获取内部异常
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }

        /// <summary>
        /// 获取业务单据编号（静态方法，兼容旧的调用模式）
        /// 内部使用同步方式调用异步方法
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <returns>生成的业务单据编号</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        public static string GetBizBillNo(BizType bizType)
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取BizCodeService实例");
                }

                // 同步调用异步方法
                // 使用Task.Run以避免可能的死锁
                return Task.Run(async () => await bizCodeService.GenerateBizBillNoAsync(bizType)).Result;
            }
            catch (AggregateException ex)
            {
                // 解包AggregateException，获取内部异常
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }

        /// <summary>
        /// 获取基础信息编号（静态方法，兼容旧的调用模式，使用枚举参数）
        /// 内部使用同步方式调用异步方法
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <returns>生成的基础信息编号</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        public static string GetBaseInfoNo(BaseInfoType baseInfoType, string paraConst = null)
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取BizCodeService实例");
                }

                // 同步调用异步方法
                return Task.Run(async () => await bizCodeService.GenerateBaseInfoNoAsync(baseInfoType, paraConst)).Result;
            }
            catch (AggregateException ex)
            {
                // 解包AggregateException，获取内部异常
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw;
            }
        }





        #endregion
    }
}