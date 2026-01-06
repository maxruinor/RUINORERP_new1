using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 采购订单到采购入库单转换器
    /// 负责将采购订单及其明细转换为采购入库单及其明细
    /// 复用业务层的核心转换逻辑（BuildPurEntryFromPurOrder），确保数据一致性
    /// </summary>
    public class PurOrderToPurEntryConverter : DocumentConverterBase<tb_PurOrder, tb_PurEntry>
    {
        private readonly ILogger<PurOrderToPurEntryConverter> _logger;
        private readonly tb_PurOrderController<tb_PurOrder> _purOrderController;
        private readonly IEntityCacheManager _cacheManager;
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="purOrderController">采购订单控制器（用于调用核心转换逻辑）</param>
        public PurOrderToPurEntryConverter(
            ILogger<PurOrderToPurEntryConverter> logger,
            tb_PurOrderController<tb_PurOrder> purOrderController,
            IEntityCacheManager cacheManager)
            : base(logger)
        {
            _purOrderController = purOrderController ?? throw new ArgumentNullException(nameof(purOrderController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager;

        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 BuildPurEntryFromPurOrder
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：采购订单</param>
        /// <returns>转换后的采购入库单</returns>
        public override async Task<tb_PurEntry> ConvertAsync(tb_PurOrder source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 BuildPurEntryFromPurOrder 方法
            return await _purOrderController.BuildPurEntryFromPurOrder(source);
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：采购订单</param>
        /// <param name="target">目标单据：采购入库单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_PurOrder source, tb_PurEntry target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：采购订单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_PurOrder source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "采购订单不能为空";
                    return result;
                }

                // 检查采购订单状态
                if (source.DataStatus != (int)DataStatus.确认 || source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只能转换已确认且已审核的采购订单";
                    return result;
                }

                // 检查采购订单是否有明细
                if (source.tb_PurOrderDetails == null || !source.tb_PurOrderDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "采购订单没有明细，无法转换";
                    return result;
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
                
                // 检查是否有可入库的明细
                var hasEntryableDetails = source.tb_PurOrderDetails.Any(d => d.Quantity > d.DeliveredQuantity);
                if (!hasEntryableDetails)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "采购订单所有明细已全部入库，无需转换";
                    return result;
                }

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证采购订单转换条件时发生错误，采购订单号：{PurOrderNo}", source?.PurOrderNo);
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
        
        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">采购订单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_PurOrder source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_PurOrderDetails.Count;
                int entryableDetails = 0;
                int nonEntryableDetails = 0;
                decimal totalEntryableQty = 0;
                
                // 遍历所有订单明细，检查可入库数量
                foreach (var detail in source.tb_PurOrderDetails)
                {
                    // 计算可入库数量 = 订单数量 - 已入库数量
                    decimal entryableQty = detail.Quantity - detail.DeliveredQuantity;
                    
                    if (entryableQty > 0)
                    {
                        entryableDetails++;
                        totalEntryableQty += entryableQty;
                        
                        // 如果可入库数量小于原订单数量，添加部分入库提示
                        if (entryableQty < detail.Quantity)
                        {
                            var prodInfo = _cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已入库数量为{detail.DeliveredQuantity}，可入库数量为{entryableQty}，小于原订单数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonEntryableDetails++;
                        
                        // 添加完全入库提示
                        var prodInfo = _cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部入库，可入库数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonEntryableDetails > 0)
                {
                    result.AddInfo($"共有{nonEntryableDetails}项产品已全部入库，将在转换时忽略");
                }
                
                if (entryableDetails > 0)
                {
                    result.AddInfo($"共有{entryableDetails}项产品可入库，总可入库数量为{totalEntryableQty}");
                }
                
                // 如果所有明细都已入库，添加警告但仍允许转换（让用户知道）
                if (entryableDetails == 0)
                {
                    result.AddWarning("该采购订单所有明细已全部入库，转换生成的入库单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证采购订单明细数量时发生错误，采购订单号：{PurOrderNo}", source?.PurOrderNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}