using AutoMapper;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 销售出库单到销售退回单转换器
    /// 负责将销售出库单及其明细转换为销售退回单及其明细
    /// 复用业务层的核心转换逻辑（SaleOutToSaleOutRe），确保数据一致性
    /// </summary>
    public class SaleOutToSaleOutReConverter : DocumentConverterBase<tb_SaleOut, tb_SaleOutRe>
    {
        private readonly ILogger<SaleOutToSaleOutReConverter> _logger;
        private readonly tb_SaleOutController<tb_SaleOut> _saleOutController;
        private readonly IEntityCacheManager _cacheManager;
        
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="saleOutController">销售出库单控制器（用于调用核心转换逻辑）</param>
        /// <param name="cacheManager">实体缓存管理器</param>
        public SaleOutToSaleOutReConverter(
            ILogger<SaleOutToSaleOutReConverter> logger,
            tb_SaleOutController<tb_SaleOut> saleOutController,
            IEntityCacheManager cacheManager)
            : base(logger)
        {
            _saleOutController = saleOutController ?? throw new ArgumentNullException(nameof(saleOutController));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;
        
        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 SaleOutToSaleOutRe
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：销售出库单</param>
        /// <returns>转换后的销售退回单</returns>
        public override async Task<tb_SaleOutRe> ConvertAsync(tb_SaleOut source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 SaleOutToSaleOutRe 方法
            return await _saleOutController.SaleOutToSaleOutRe(source);
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：销售出库单</param>
        /// <param name="target">目标单据：销售退回单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_SaleOut source, tb_SaleOutRe target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：销售出库单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_SaleOut source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null || source.SaleOut_MainID <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售出库单不存在或无效";
                    return result;
                }

                // 检查出库单状态是否为已审核
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 || !source.ApprovalResults.GetValueOrDefault())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只有已审核通过的销售出库单才能生成销售退回单";
                    return result;
                }
                
                // 检查是否有出库明细
                if (source.tb_SaleOutDetails == null || !source.tb_SaleOutDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "该销售出库单没有明细，无法生成销售退回单";
                    return result;
                }
                
                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证销售出库单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
        
        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">销售出库单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_SaleOut source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_SaleOutDetails.Count;
                int returnableDetails = 0;
                int nonReturnableDetails = 0;
                decimal totalReturnableQty = 0;
                
                // 遍历所有出库明细，检查可退数量
                foreach (var detail in source.tb_SaleOutDetails)
                {
                    // 计算可退数量 = 出库数量 - 已退回数量
                    decimal returnableQty = detail.Quantity - detail.TotalReturnedQty;
                    
                    if (returnableQty > 0)
                    {
                        returnableDetails++;
                        totalReturnableQty += returnableQty;
                        
                        // 如果可退数量小于原出库数量，添加部分退回提示
                        if (returnableQty < detail.Quantity)
                        {
                            var prodInfo = _cacheManager.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已退回数量为{detail.TotalReturnedQty}，可退回数量为{returnableQty}，小于原出库数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonReturnableDetails++;
                        
                        // 添加完全退回提示
                        var prodInfo = _cacheManager.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部退回，可退回数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonReturnableDetails > 0)
                {
                    result.AddInfo($"共有{nonReturnableDetails}项产品已全部退回，将在转换时忽略");
                }
                
                if (returnableDetails > 0)
                {
                    result.AddInfo($"共有{returnableDetails}项产品可退回，总可退数量为{totalReturnableQty}");
                }
                
                // 如果所有明细都已退回，添加警告但仍允许转换（让用户知道）
                if (returnableDetails == 0)
                {
                    result.AddWarning("该出库单所有明细已全部退回，转换生成的退回单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证销售出库单明细数量时发生错误，出库单号：{SaleOutNo}", source?.SaleOutNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}