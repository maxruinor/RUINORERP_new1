using Microsoft.Extensions.Logging;
using RUINORERP.Business.Network;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using System;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 服务器通信服务实现类
    /// 在UI层实现，用于将UI层的通信能力传递给业务层
    /// 避免业务层直接依赖UI层的通信组件
    /// </summary>
    public class ServerCommunicationServiceImpl : IServerCommunicationService
    {
        private readonly ILogger<ServerCommunicationServiceImpl> _logger;
        private readonly ClientCommunicationService _clientCommunicationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="clientCommunicationService">客户端通信服务</param>
        public ServerCommunicationServiceImpl(
            ILogger<ServerCommunicationServiceImpl> logger,
            ClientCommunicationService clientCommunicationService)
        {
            _logger = logger;
            _clientCommunicationService = clientCommunicationService ?? throw new ArgumentNullException(nameof(clientCommunicationService));
        }

        /// <summary>
        /// 获取当前连接状态
        /// </summary>
        public bool IsConnected => _clientCommunicationService != null && _clientCommunicationService.IsConnected;

        /// <summary>
        /// 生成业务单据编号
        /// 通过UI层的BizCodeService生成，确保分布式环境下的唯一性
        /// </summary>
        /// <param name="bizType">业务类型枚举</param>
        /// <param name="parameter">业务编码参数</param>
        /// <returns>生成的单据编号</returns>
        public string GenerateBizBillNo(BizType bizType, BizCodeParameter parameter = null)
        {
            try
            {
                _logger?.LogDebug("直接通过通信服务生成业务单据编号: {BizType}", bizType);
                
                // 避免循环引用：直接创建请求并发送到服务器，而不是调用BizCodeService.GetBizBillNo()
                var request = new BizCodeRequest { BizType = bizType };
                
                // 使用同步方式发送请求并获取响应
                // 注意：这里需要确保通信服务能够处理同步请求，或者使用适当的异步转同步机制
                var response = _clientCommunicationService.SendCommandWithResponse<BizCodeResponse>(
                    BizCodeCommands.GenerateBizBillNo, request);
                
                if (response != null && response.IsSuccess && !string.IsNullOrEmpty(response.GeneratedCode))
                {
                    return response.GeneratedCode;
                }
                
                throw new Exception(response?.ErrorMessage ?? "生成业务单据编号失败");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "生成业务单据编号失败: {BizType}", bizType);
                throw;
            }
        }

        /// <summary>
        /// 生成基础信息编号
        /// 通过UI层的BizCodeService生成，确保分布式环境下的唯一性
        /// </summary>
        /// <param name="infoType">信息类型</param>
        /// <param name="paraConst">常量参数</param>
        /// <returns>生成的信息编号</returns>
        public string GenerateBaseInfoNo(string infoType, string paraConst = null)
        {
            try
            {
                _logger?.LogDebug("通过UI层BizCodeService生成基础信息编号: {InfoType}, 参数: {ParaConst}", infoType, paraConst);
                // 调用UI层BizCodeService的静态方法，保持与现有代码的一致性
                return BizCodeService.GetBaseInfoNo(infoType, paraConst);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "通过UI层BizCodeService生成基础信息编号失败: {InfoType}", infoType);
                throw;
            }
        }

        /// <summary>
        /// 生成产品编码
        /// 通过UI层的BizCodeService生成，确保分布式环境下的唯一性
        /// </summary>
        /// <param name="categoryId">产品类别ID</param>
        /// <param name="customPrefix">自定义前缀</param>
        /// <param name="includeDate">是否包含日期</param>
        /// <returns>生成的产品编码</returns>
        public string GenerateProductNo(long categoryId, string customPrefix = null, bool includeDate = false)
        {
            try
            {
                _logger?.LogDebug("通过UI层BizCodeService生成产品编码: 类别ID={CategoryId}, 前缀={CustomPrefix}, 包含日期={IncludeDate}", 
                    categoryId, customPrefix, includeDate);
                // 调用UI层BizCodeService的静态方法，保持与现有代码的一致性
                return BizCodeService.GetProductNo(categoryId, customPrefix, includeDate);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "通过UI层BizCodeService生成产品编码失败: 类别ID={CategoryId}", categoryId);
                throw;
            }
        }

        /// <summary>
        /// 生成产品SKU编码
        /// 通过UI层的BizCodeService生成，确保分布式环境下的唯一性
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="attributes">属性组合信息</param>
        /// <param name="seqLength">序号长度</param>
        /// <returns>生成的产品SKU编码</returns>
        public string GenerateProductSKUNo(long productId, string productCode, string attributes = null, int seqLength = 3)
        {
            try
            {
                _logger?.LogDebug("通过UI层BizCodeService生成产品SKU编码: 产品ID={ProductId}, 产品编码={ProductCode}", 
                    productId, productCode);
                // 调用UI层BizCodeService的静态方法，保持与现有代码的一致性
                return BizCodeService.GetProductSKUNo(productId, productCode, attributes, seqLength);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "通过UI层BizCodeService生成产品SKU编码失败: 产品ID={ProductId}", productId);
                throw;
            }
        }

        /// <summary>
        /// 生成条码
        /// 通过UI层的BizCodeService生成
        /// </summary>
        /// <param name="originalCode">原始编码</param>
        /// <param name="paddingChar">补位字符</param>
        /// <returns>生成的条码</returns>
        public string GenerateBarCode(string originalCode, char paddingChar = '0')
        {
            try
            {
                _logger?.LogDebug("通过UI层BizCodeService生成条码: 原始编码={OriginalCode}", originalCode);
                // 调用UI层BizCodeService的静态方法，保持与现有代码的一致性
                return BizCodeService.GetBarCode(originalCode, paddingChar);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "通过UI层BizCodeService生成条码失败: 原始编码={OriginalCode}", originalCode);
                throw;
            }
        }
    }
}