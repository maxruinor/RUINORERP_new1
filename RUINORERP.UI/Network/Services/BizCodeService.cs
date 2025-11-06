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
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 业务编码服务类
    /// 提供业务单据编号、基础信息编号、产品编码和SKU编码的生成功能
    /// 采用与UserLoginService相似的设计模式，确保统一的网络通信和异常处理
    /// 同时提供静态方法以便兼容旧的调用模式
    /// </summary>
    public sealed class BizCodeService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<BizCodeService> _logger;
        private readonly SemaphoreSlim _operationLock = new SemaphoreSlim(1, 1); // 防止并发操作请求
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当必要参数为空时抛出</exception>
        public BizCodeService(
            ClientCommunicationService communicationService,
            ILogger<BizCodeService> logger = null)
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

            throw new Exception(response.ErrorMessage ?? "生成业务单据编号失败");
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
            string baseInfoTypeStr = baseInfoType.ToString();
            var request = new BizCodeRequest { BaseInfoType = baseInfoTypeStr, ParaConst = paraConst };
            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateBaseInfoNo, request, ct);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }

            throw new Exception(response.ErrorMessage ?? "生成基础信息编号失败");
        }
        
        /// <summary>
        /// 生成基础信息编号（字符串版本，保持向后兼容）
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateBaseInfoNoAsync(string baseInfoType, string paraConst = null, CancellationToken ct = default)
        {
            // 尝试将字符串转换为枚举以确保类型安全
            if (!Enum.TryParse<BaseInfoType>(baseInfoType, true, out var _))
            {
                _logger?.LogWarning("使用了非枚举类型的基础信息类型字符串: {BaseInfoType}", baseInfoType);
            }
            
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
        /// 生成产品编码
        /// </summary>
        /// <param name="categoryId">产品类别ID</param>
        /// <param name="customPrefix">自定义前缀（可选）</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品编码</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateProductNoAsync(long categoryId, string customPrefix = null, bool includeDate = false, CancellationToken ct = default)
        {
            var request = new BizCodeRequest
            {
                ProductParameter = new ProductCodeParameter
                {
                    CategoryId = categoryId,
                    CustomPrefix = customPrefix,
                    IncludeDate = includeDate
                }
            };

            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateProductNo, request, ct);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }

            throw new Exception(response.ErrorMessage ?? "生成产品编码失败");
        }

        /// <summary>
        /// 生成产品SKU编码
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的产品SKU编码</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateProductSKUNoAsync(long productId, string productCode, string attributes = null, int seqLength = 3, CancellationToken ct = default)
        {
            var request = new BizCodeRequest
            {
                SKUParameter = new SKUCodeParameter
                {
                    ProductId = productId,
                    ProductCode = productCode,
                    Attributes = attributes,
                    SeqLength = seqLength
                }
            };

            var response = await SendBizCodeCommandAsync(
                BizCodeCommands.GenerateProductSKUNo, request, ct);

            if (response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
            {
                return response.GeneratedCode;
            }

            throw new Exception(response.ErrorMessage ?? "生成产品SKU编码失败");
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
        /// 发送业务编码生成命令的通用方法
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="request">请求对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>响应对象</returns>
        private async Task<BizCodeResponse> SendBizCodeCommandAsync(CommandId commandId, BizCodeRequest request, CancellationToken ct = default)
        {
            // 使用信号量确保同一时间只有一个请求
            await _operationLock.WaitAsync(ct);
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

                while (retryCount <= maxRetries)
                {
                    try
                    {
                        // 发送命令并获取响应
                        response = await _communicationService.SendCommandWithResponseAsync<BizCodeResponse>(
                            commandId, request, ct);
                        break; // 成功则跳出重试循环
                    }
                    catch (Exception ex) when (IsRetryableException(ex) && retryCount < maxRetries)
                    {
                        retryCount++;
                        _logger?.LogWarning(ex, "业务编码生成请求失败，正在重试 ({RetryCount}/{MaxRetries}) - 命令ID: {CommandId}",
                            retryCount, maxRetries, commandId.ToString());
                        // 指数退避策略
                        await Task.Delay(100 * (int)Math.Pow(2, retryCount), ct);
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
            catch (OperationCanceledException ex)
            {
                _logger?.LogDebug(ex, "业务编码生成操作已被取消");
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "业务编码生成过程中发生未预期的异常");
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
                var bizCodeService = Startup.GetFromFac<BizCodeService>();
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
                var bizCodeService = Startup.GetFromFac<BizCodeService>();
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
                var bizCodeService = Startup.GetFromFac<BizCodeService>();
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

        /// <summary>
        /// 获取基础信息编号（静态方法，兼容旧的调用模式，使用字符串参数）
        /// 内部使用同步方式调用异步方法
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <returns>生成的基础信息编号</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        public static string GetBaseInfoNo(string baseInfoType, string paraConst = null)
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<BizCodeService>();
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

        /// <summary>
        /// 获取产品编码（静态方法，兼容旧的调用模式）
        /// 内部使用同步方式调用异步方法
        /// </summary>
        /// <param name="categoryId">产品类别ID</param>
        /// <param name="customPrefix">自定义前缀（可选）</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <returns>生成的产品编码</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        public static string GetProductNo(long categoryId, string customPrefix = null, bool includeDate = false)
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<BizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取BizCodeService实例");
                }

                // 同步调用异步方法
                return Task.Run(async () => await bizCodeService.GenerateProductNoAsync(categoryId, customPrefix, includeDate)).Result;
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
        /// 获取产品SKU编码（静态方法，兼容旧的调用模式）
        /// 内部使用同步方式调用异步方法
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <returns>生成的产品SKU编码</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        public static string GetProductSKUNo(long productId, string productCode, string attributes = null, int seqLength = 3)
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<BizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取BizCodeService实例");
                }

                // 同步调用异步方法
                return Task.Run(async () => await bizCodeService.GenerateProductSKUNoAsync(productId, productCode, attributes, seqLength)).Result;
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