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
using RUINORERP.Model.ProductAttribute;
// 引入BNR架构命名空间
using RUINORERP.Business.BNR;
using System.Collections.Concurrent;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 业务编码服务类
    /// 提供业务单据编号、基础信息编号、产品编码和SKU编码的生成功能
    /// 采用与UserLoginService相似的设计模式，确保统一的网络通信和异常处理
    /// 同时提供静态方法以便兼容旧的调用模式
    /// </summary>
    public sealed class ClientBizCodeService : IBizCodeGenerateService, IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<ClientBizCodeService> _logger;
        
        // ✅ 移除细粒度锁 - 服务器端已有完善的并发控制,客户端加锁只会增加延迟
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
        /// 本地生成基础信息编号1
        /// 使用BNR架构体系的轻量级实现，不依赖数据库和Redis
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的基础信息编号</returns>
        public async Task<string> GenerateLocalBaseInfoNoAsync(BaseInfoType baseInfoType, string paraConst = null, CancellationToken ct = default)
        {
            try
            {
                // 使用BNR架构的轻量级工厂来生成序号
                LightweightBNRFactory bnrFactory = new LightweightBNRFactory();

                // 根据不同的基础信息类型，使用不同的生成规则
                string rule = GetBNRRuleByBaseInfoType(baseInfoType);

                // 生成编号
                string generatedCode = bnrFactory.Create(rule);

                return generatedCode;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "本地生成基础信息编号失败 - 类型: {BaseInfoType}", baseInfoType.ToString());
                throw new Exception($"本地生成基础信息编号失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根据基础信息类型获取 BNR 生成规则
        /// </summary>
        /// <param name="baseInfoType">基础信息类型</param>
        /// <returns>BNR 生成规则字符串</returns>
        /// <remarks>
        /// 客户端本地生成规则使用 "L" 前缀标识，与服务器端规则区分，避免重复
        /// 格式说明：{S:前缀}{D:日期格式}{N:序号键/序号格式}
        /// </remarks>
        private string GetBNRRuleByBaseInfoType(BaseInfoType baseInfoType)
        {
            // 客户端本地生成规则添加 "L" 前缀标识（L=Local），确保与服务器端编号体系隔离
            return baseInfoType switch
            {
                BaseInfoType.ModuleDefinition => "{S:L_MOD}{D:yyyyMMdd}{N:ModuleDefinition/0000}",
                BaseInfoType.Customer => "{S:L_CUST}{D:yyyyMMdd}{N:Customer/00000}",
                BaseInfoType.Supplier => "{S:L_SUPP}{D:yyyyMMdd}{N:Supplier/00000}",
                BaseInfoType.CVOther => "{S:L_CV}{D:yyyyMMdd}{N:CVOther/00000}",
                BaseInfoType.BusinessPartner => "{S:L_BP}{D:yyyyMMdd}{N:BusinessPartner/00000}",
                BaseInfoType.CRM_RegionCode => "{S:L_REG}{D:yyyyMM}{N:CRM_RegionCode/0000}",
                BaseInfoType.Department => "{S:L_DEPT}{D:yyyyMM}{N:Department/000}",
                BaseInfoType.Employee => "{S:L_EMP}{D:yyyyMM}{N:Employee/0000}",
                BaseInfoType.Location => "{S:L_LOC}{D:yyyyMM}{N:Location/0000}",
                BaseInfoType.ProjectGroupCode => "{S:L_PG}{D:yyyyMM}{N:ProjectGroup/0000}",
                BaseInfoType.StoreCode => "{S:L_STORE}{D:yyyyMM}{N:Store/0000}",
                BaseInfoType.FMSubject => "{S:L_FS}{D:yyyyMM}{N:FMSubject/0000}",
                _ => "{S:L_BASE}{D:yyyyMMdd}{N:Default/0000}"
            };
        }


        /// <summary>
        /// 生成产品相关编码（产品编号、SKU编号等）
        /// 支持SKU属性更新：
        /// - 编辑已有产品（ProdDetailID > 0）：SKU永远不变
        /// - 新增产品详情（ProdDetailID == 0）：
        ///   - 如果SKU已有值 → 使用 UpdateSKUAttributePart 更新（保持序号不变、只更新属性部分）
        ///   - 如果SKU为空 → 使用 GenerateProductRelatedCodeAsync 生成全新SKU
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="prod">产品实体</param>
        /// <param name="PrefixParaConst">参数常量（可选）</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的编码</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateProductRelatedCodeAsync(BaseInfoType baseInfoType, tb_Prod prod, string PrefixParaConst = null,
            int seqLength = 3, bool includeDate = false, CancellationToken ct = default)
        {
            // 将枚举转换为字符串发送到服务器
            var request = new BizCodeRequest { BaseInfoType = baseInfoType, ParaConst = PrefixParaConst };

            // 准备属性信息列表（用于SKU编码生成）
            if (baseInfoType == BaseInfoType.SKU_No && prod.tb_Prod_Attr_Relations != null && prod.tb_Prod_Attr_Relations.Count > 0)
            {
                // TODO: 处理产品属性关系，构建SKU属性部分
                // 当前版本暂未实现属性编码逻辑，预留扩展点
            }

            var ProductParameter = new ProdParameter
            {
                prod = prod,
                SeqLength = seqLength,
                IncludeDate = includeDate,
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
        /// 生成产品相关编码（产品编号、SKU编号等）
        /// 支持SKU属性更新：
        /// - 编辑已有产品（ProdDetailID > 0）：SKU永远不变
        /// - 新增产品详情（ProdDetailID == 0）：
        ///   - 如果SKU已有值 → 使用 UpdateSKUAttributePart 更新（保持序号不变、只更新属性部分）
        ///   - 如果SKU为空 → 使用 GenerateProductRelatedCodeAsync 生成全新SKU
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="prod">产品实体</param>
        /// <param name="PrefixParaConst">参数常量（可选）</param>
        /// <param name="seqLength">序号长度</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>生成的编码</returns>
        /// <exception cref="Exception">网络连接相关异常</exception>
        /// <exception cref="Exception">业务逻辑相关异常</exception>
        public async Task<string> GenerateProductSKUCodeAsync(BaseInfoType baseInfoType, tb_Prod prod,
            tb_ProdDetail prodDetail, 
            int seqLength = 3, bool includeDate = false, CancellationToken ct = default)
        {
            // 将枚举转换为字符串发送到服务器
            var request = new BizCodeRequest { BaseInfoType = baseInfoType };

            // 准备属性信息列表（用于SKU编码生成）
            if (baseInfoType == BaseInfoType.SKU_No && prodDetail.tb_Prod_Attr_Relations != null && prodDetail.tb_Prod_Attr_Relations.Count > 0)
            {
                // TODO: 处理产品详情属性关系，构建SKU属性部分
                // 当前版本暂未实现属性编码逻辑，预留扩展点
            }

            var ProductParameter = new ProdParameter
            {
                prod = prod,
                prodDetail = prodDetail,
                SeqLength = seqLength,
                IncludeDate = includeDate,
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
        /// ✅ 优化:移除客户端锁,让并发请求真正并行;减少重试次数降低延迟
        /// </summary>
        /// <typeparam name="TResponse">响应类型</typeparam>
        /// <param name="commandId">命令 ID</param>
        /// <param name="request">请求对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>响应对象</returns>
        private async Task<BizCodeResponse> SendBizCodeCommandAsync(CommandId commandId, BizCodeRequest request, CancellationToken ct = default)
        {
            // 创建带超时的取消令牌源
            using var timeoutCts = new CancellationTokenSource();
            // 设置默认超时时间为 5 秒（平衡网络延迟和用户体验）
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(5));
        
            // 创建组合取消令牌，任一令牌取消都会导致操作取消
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, timeoutCts.Token);
            var combinedCt = linkedCts.Token;
        
            // ✅ 删除锁相关代码 - 服务器端已有DatabaseSequenceService保证唯一性
            try
            {
                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _logger?.LogWarning("业务编码生成失败：未连接到服务器");
                    throw new Exception("未连接到服务器，请检查网络连接后重试");
                }

                // 发送命令(✅ 减少重试次数,从2次改为1次,避免延迟累积)
                BizCodeResponse response = null;
                int maxRetries = 1; // 从2改为1
                int retryCount = 0;

                while (retryCount <= maxRetries)
                {
                    try
                    {
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
                        // ✅ 固定延迟100ms,不使用指数退避(更可控)
                        await Task.Delay(100, combinedCt);
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
            // ✅ 删除finally中的锁释放
            // finally { operationLock.Release(); }
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源（标准Dispose模式）
        /// </summary>
        /// <param name="disposing">是否正在 disposing</param>
        private void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;
        
            if (disposing)
            {
                // 释放托管资源（当前无需释放特殊资源）
                System.Diagnostics.Debug.WriteLine("ClientBizCodeService 托管资源已释放");
            }
            
            _isDisposed = true;
        }

        #region 静态方法（兼容旧的调用模式）

        /// <summary>
        /// 本地获取基础信息编号（异步版本，推荐使用）
        /// </summary>
        /// <param name="baseInfoType">基础信息类型枚举</param>
        /// <param name="paraConst">参数常量（可选）</param>
        /// <returns>生成的基础信息编号</returns>
        /// <exception cref="Exception">生成失败时抛出异常</exception>
        /// <remarks>
        /// 兜底策略：只有确定本地生成失败时才回退到远程方式
        /// 排除超时等不确定异常，避免重复生成
        /// </remarks>
        public static async Task<string> GetLocalBaseInfoNoAsync(BaseInfoType baseInfoType, string paraConst = null)
        {
            try
            {
                // 从依赖注入容器中获取服务实例
                var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取 BizCodeService 实例");
                }

                return await bizCodeService.GenerateLocalBaseInfoNoAsync(baseInfoType, paraConst);
            }
            catch (Exception ex) when (IsDefiniteFailure(ex))
            {
                // 确定性失败，回退到远程方式
                var logger = Startup.GetFromFac<ILogger<ClientBizCodeService>>();
                logger?.LogWarning(ex, "本地生成确认失败，回退到远程生成方式 - 类型：{BaseInfoType}", baseInfoType.ToString());
                var bizCodeService = Startup.GetFromFac<ClientBizCodeService>();
                if (bizCodeService == null)
                {
                    throw new Exception("无法从容器中获取BizCodeService实例");
                }

                return await bizCodeService.GenerateBaseInfoNoAsync(baseInfoType, paraConst);
            }
        }
        
        /// <summary>
        /// 判断异常是否为确定性失败（应该回退到远程方式）
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>是否为确定性失败</returns>
        /// <remarks>
        /// 确定性失败包括：空指针、规则缺失等明确错误
        /// 不确定性失败包括：超时、网络中断（可能已成功但不确定）
        /// </remarks>
        private static bool IsDefiniteFailure(Exception ex)
        {
            // 明确失败场景：空指针、参数验证失败、规则缺失等
            if (ex is ArgumentNullException || 
                ex is ArgumentException ||
                ex.Message.Contains("规则不存在") ||
                ex.Message.Contains("无法识别"))
            {
                return true;
            }
                    
            // 不确定性失败：超时、网络问题（可能本地已生成成功）
            if (ex is TimeoutException ||
                ex is OperationCanceledException ||
                (ex.Message != null && (
                    ex.Message.IndexOf("timeout", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    ex.Message.IndexOf("取消", StringComparison.OrdinalIgnoreCase) >= 0)))
            {
                return false;
            }
                    
            // 其他异常默认按不确定性失败处理，避免盲目回退
            return false;
        }





        #endregion
    }
}