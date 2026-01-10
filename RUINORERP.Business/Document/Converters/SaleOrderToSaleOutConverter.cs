// **********************************************************************
// 销售订单到销售出库单转换器
// 实现销售订单向销售出库单的转换逻辑
// **********************************************************************
using RUINORERP.Model;
using RUINORERP.Business.Document;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using RUINORERP.Model.Context;
using RUINORERP.IServices;
using System.Threading;
using RUINORERP.Business.Cache;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks; // 确保引入IUnitOfWorkManage所在的命名空间

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 销售订单到销售出库单转换器
    /// 负责将销售订单及其明细转换为销售出库单及其明细
    /// 复用业务层的核心转换逻辑（SaleOrderToSaleOut），确保数据一致性
    /// </summary>
    public class SaleOrderToSaleOutConverter : DocumentConverterBase<tb_SaleOrder, tb_SaleOut>
    {
        private readonly ILogger<SaleOrderToSaleOutConverter> _logger;
        private readonly tb_SaleOrderController<tb_SaleOrder> _saleOrderController;
        private readonly IEntityCacheManager _cacheManager;
        private readonly IUnitOfWorkManage _unitOfWorkManage; // 添加IUnitOfWorkManage依赖
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="saleOrderController">销售订单控制器（用于调用核心转换逻辑）</param>
        /// <param name="cacheManager">实体缓存管理器</param>
        /// <param name="unitOfWorkManage">工作单元管理器</param>
        public SaleOrderToSaleOutConverter(
            ILogger<SaleOrderToSaleOutConverter> logger,
            tb_SaleOrderController<tb_SaleOrder> saleOrderController,
            IEntityCacheManager cacheManager,
            IUnitOfWorkManage unitOfWorkManage)
            : base(logger)
        {
            _saleOrderController = saleOrderController ?? throw new ArgumentNullException(nameof(saleOrderController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager;
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;

        /// <summary>
        /// 执行单据转换 - 直接调用业务层核心逻辑 SaleOrderToSaleOut
        /// 重写基类方法，完全控制转换过程
        /// </summary>
        /// <param name="source">源单据：销售订单</param>
        /// <returns>转换后的销售出库单</returns>
        public override async Task<tb_SaleOut> ConvertAsync(tb_SaleOrder source)
        {
            // 验证转换条件
            var validationResult = await ValidateConversionAsync(source);
            if (!validationResult.CanConvert)
            {
                throw new InvalidOperationException(validationResult.ErrorMessage);
            }

            // 直接调用经过长期验证的 SaleOrderToSaleOut 方法
            return await _saleOrderController.SaleOrderToSaleOut(source);
        }
        
        /// <summary>
        /// 执行具体的转换逻辑 - 重写后不再使用基类的 target 参数模式
        /// </summary>
        /// <param name="source">源单据：销售订单</param>
        /// <param name="target">目标单据：销售出库单（不再使用）</param>
        /// <returns></returns>
        protected override async Task PerformConversionAsync(tb_SaleOrder source, tb_SaleOut target)
        {
            // 此方法不再使用，逻辑已移至 ConvertAsync
            // 保留此方法以满足抽象类要求
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：销售订单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_SaleOrder source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null || source.SOrder_ID <= 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单不存在或无效";
                    return result;
                }

                // 检查销售订单状态
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "仅已审核的销售订单可以生成销售出库单";
                    return result;
                }

                // 检查销售订单是否有明细
                if (source.tb_SaleOrderDetails == null || !source.tb_SaleOrderDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单没有明细，无法生成销售出库单";
                    return result;
                }

                // 检查当前销售订单是否已生成过销售出库单
                int existingCount = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                    .Where(c => c.SOrder_ID == source.SOrder_ID)
                    .CountAsync();

                if (existingCount > 0)
                {
                    // 添加警告信息，提示用户当前销售订单已生成过销售出库单
                    result.WarningMessages.Add($"当前销售订单已生成过{existingCount}张销售出库单，是否继续生成？");
                    result.InfoMessages.Add("系统支持同一销售订单的多次出库操作，但请注意避免重复生成相同的出库单。");
                }

                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);

                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证销售订单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
        
        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">销售订单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_SaleOrder source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_SaleOrderDetails.Count;
                int deliverableDetails = 0;
                int nonDeliverableDetails = 0;
                decimal totalDeliverableQty = 0;
                
                // 遍历所有订单明细，检查可出库数量
                foreach (var detail in source.tb_SaleOrderDetails)
                {
                    // 计算可出库数量 = 订单数量 - 已出库数量
                    decimal deliverableQty = detail.Quantity - detail.TotalDeliveredQty;
                    
                    if (deliverableQty > 0)
                    {
                        deliverableDetails++;
                        totalDeliverableQty += deliverableQty;
                        
                        // 如果可出库数量小于原订单数量，添加部分出库提示
                        if (deliverableQty < detail.Quantity)
                        {
                            var prodInfo = _cacheManager.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已出库数量为{detail.TotalDeliveredQty}，可出库数量为{deliverableQty}，小于原订单数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonDeliverableDetails++;
                        
                        // 添加完全出库提示
                        var prodInfo = _cacheManager.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部出库，可出库数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonDeliverableDetails > 0)
                {
                    result.AddInfo($"共有{nonDeliverableDetails}项产品已全部出库，将在转换时忽略");
                }
                
                if (deliverableDetails > 0)
                {
                    result.AddInfo($"共有{deliverableDetails}项产品可出库，总可出库数量为{totalDeliverableQty}");
                }
                
                // 如果所有明细都已出库，设置错误但仍允许转换（让用户知道）
                if (deliverableDetails == 0)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "销售订单所有明细已全部出库，无法再次生成出库单";
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证销售订单明细数量时发生错误，订单号：{SOrderNo}", source?.SOrderNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}