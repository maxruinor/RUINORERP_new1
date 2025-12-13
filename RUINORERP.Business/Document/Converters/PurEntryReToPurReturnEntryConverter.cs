// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/11/2025 15:30:00
// **************************************
using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
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
    /// 采购退货单到采购退货入库单转换器
    /// 用于采购退回供应商的货物修复或换新后再次入库的场景
    /// </summary>
    public class PurEntryReToPurReturnEntryConverter : DocumentConverterBase<tb_PurEntryRe, tb_PurReturnEntry>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PurEntryReToPurReturnEntryConverter> _logger;
        private readonly IBizCodeGenerateService _bizCodeService;
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 初始化采购退货单到采购退货入库单转换器
        /// </summary>
        /// <param name="mapper">对象映射器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="bizCodeService">业务编码生成服务</param>
        /// <param name="appContext">应用程序上下文</param>
        public PurEntryReToPurReturnEntryConverter(
            IMapper mapper,
            ILogger<PurEntryReToPurReturnEntryConverter> logger,
            IBizCodeGenerateService bizCodeService,
            ApplicationContext appContext) : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bizCodeService = bizCodeService ?? throw new ArgumentNullException(nameof(bizCodeService));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }

        /// <summary>
        /// 执行从采购退货单到采购退货入库单的转换
        /// </summary>
        /// <param name="source">源单据：采购退货单</param>
        /// <param name="target">目标单据：采购退货入库单</param>
        /// <returns>转换操作任务</returns>
        protected override async Task PerformConversionAsync(tb_PurEntryRe source, tb_PurReturnEntry target)
        {
            try
            {
                _logger.LogInformation($"开始转换采购退货单 {source.PurEntryReNo} 到采购退货入库单");

                // 使用AutoMapper进行基础属性映射
                _mapper.Map(source, target);

                // 重置状态相关字段 - 与业务层保持一致
                target.ApprovalOpinions = "快捷转单";
                target.ApprovalResults = null;
                target.DataStatus = (int)DataStatus.草稿;
                target.ApprovalStatus = (int)ApprovalStatus.未审核;
                target.Approver_at = null;
                target.Approver_by = null;
                target.PrintStatus = 0;
                target.ActionStatus = ActionStatus.新增;
                target.ApprovalOpinions = "";
                target.Modified_at = null;
                target.Modified_by = null;
                target.Paytype_ID = null;
                target.PayStatus = (int)PayStatus.未付款;

                // 生成新的采购退货入库单号
                target.PurReEntryNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.采购退货入库);
                target.BillDate = DateTime.Now;
                target.Notes = $"由采购退货单{source.PurEntryReNo}生成";

                // 设置关联信息
                if (source.PurEntryRe_ID > 0)
                {
                    target.PurEntryRe_ID = source.PurEntryRe_ID;
                    target.CustomerVendor_ID = source.CustomerVendor_ID;
                    target.DepartmentID = source.DepartmentID;
                    target.Paytype_ID = source.Paytype_ID;
                    target.PurEntryReNo = source.PurEntryReNo;
                    //target.IsCompleted = source.IsCustomizedOrder;
                }

                // 初始化明细集合
                if (target.tb_PurReturnEntryDetails == null)
                {
                    target.tb_PurReturnEntryDetails = new List<tb_PurReturnEntryDetail>();
                }

                // 转换明细数据
                await ConvertDetailsAsync(source, target);

                // 处理运费逻辑 - 重复入库时运费为0
                ProcessFreightLogic(source, target);

                // 重新计算汇总字段
                RecalculateSummaryFields(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);

                _logger.LogInformation($"成功转换采购退货单 {source.PurEntryReNo} 到采购退货入库单 {target.PurReEntryNo}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"转换采购退货单 {source.PurEntryReNo} 到采购退货入库单时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 转换明细数据
        /// </summary>
        /// <param name="source">源单据：采购退货单</param>
        /// <param name="target">目标单据：采购退货入库单</param>
        /// <returns>转换操作任务</returns>
        private async Task ConvertDetailsAsync(tb_PurEntryRe source, tb_PurReturnEntry target)
        {
            var details = _mapper.Map<List<tb_PurReturnEntryDetail>>(source.tb_PurEntryReDetails);
            var newDetails = new List<tb_PurReturnEntryDetail>();
            var tipsMsg = new List<string>();

            for (int i = 0; i < details.Count; i++)
            {
                await ProcessDetailItemAsync(source, details[i], newDetails, tipsMsg, i);
            }

            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"采购退货单:{source.PurEntryReNo}已全部入回仓库，请检查是否正在重复操作！");
            }

            target.tb_PurReturnEntryDetails = newDetails;

            // 保留重要提示，移除简单日志记录
            if (tipsMsg.Any())
            {
                _logger.LogInformation("转换过程中的提示信息：{Tips}", string.Join("; ", tipsMsg));
            }
        }

        /// <summary>
        /// 处理单个明细项
        /// </summary>
        private async Task ProcessDetailItemAsync(tb_PurEntryRe source, tb_PurReturnEntryDetail detail, List<tb_PurReturnEntryDetail> newDetails, List<string> tipsMsg, int index)
        {
            try
            {
                // 查找对应的退货明细
                var reDetail = FindCorrespondingReDetail(source, detail);
                if (reDetail == null)
                {
                    _logger.LogWarning("找不到对应的退货明细，产品明细ID：{ProdDetailID}", detail.ProdDetailID);
                    return;
                }

                // 设置成本和自定义成本
        
                detail.UnitPrice = reDetail.UnitPrice + reDetail.CustomizedCost; 

                // 处理成本为0的情况 - 从库存获取成本
                if (detail.UnitPrice == 0)
                {
                    await SetCostFromInventoryAsync(detail);
                }

                // 计算可入库数量 = 退货数量 - 已交回数量
                detail.Quantity = reDetail.Quantity - reDetail.DeliveredQuantity;

                // 重新计算金额
                detail.SubtotalTrPriceAmount = detail.UnitPrice * detail.Quantity;
                detail.TaxAmount = detail.SubtotalTrPriceAmount / (1 + detail.TaxRate) * detail.TaxRate;
                // 检查是否可入库
                if (detail.Quantity > 0)
                {
                    newDetails.Add(detail);
                }
                else
                {
                    AddQuantityWarning(tipsMsg, source, reDetail, detail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理明细项时发生错误，索引：{Index}", index);
                throw;
            }
        }

        /// <summary>
        /// 查找对应的退货明细 - 处理重复产品情况
        /// </summary>
        private tb_PurEntryReDetail FindCorrespondingReDetail(tb_PurEntryRe source, tb_PurReturnEntryDetail detail)
        {
            // 检查是否有重复产品
            var duplicateProdDetails = source.tb_PurEntryReDetails
                .Where(x => x.ProdDetailID == detail.ProdDetailID)
                .ToList();

            if (duplicateProdDetails.Count > 1)
            {
                // 多行情况，需要更精确匹配
                return source.tb_PurEntryReDetails.FirstOrDefault(c => 
                    c.ProdDetailID == detail.ProdDetailID &&
                    c.Location_ID == detail.Location_ID &&
                    c.PurEntryRe_CID == detail.PurEntryRe_CID);
            }
            else
            {
                // 单行情况
                return source.tb_PurEntryReDetails.FirstOrDefault(c => 
                    c.ProdDetailID == detail.ProdDetailID &&
                    c.Location_ID == detail.Location_ID);
            }
        }

        /// <summary>
        /// 从库存获取成本
        /// </summary>
        private async Task SetCostFromInventoryAsync(tb_PurReturnEntryDetail detail)
        {
            try
            {
                var prodDetail = Cache.EntityCacheHelper.GetEntity<View_ProdDetail>(detail.ProdDetailID);
                if (prodDetail != null && prodDetail is View_ProdDetail viewProd)
                {
                    detail.UnitPrice = viewProd.Inv_Cost ?? 0m;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "从库存获取成本失败，产品明细ID：{ProdDetailID}", detail.ProdDetailID);
                detail.UnitPrice = 0; // 确保成本有默认值
            }
        }

        /// <summary>
        /// 添加数量警告信息
        /// </summary>
        private void AddQuantityWarning(List<string> tipsMsg, tb_PurEntryRe source, tb_PurEntryReDetail reDetail, tb_PurReturnEntryDetail detail)
        {
            var prodInfo = Cache.EntityCacheHelper.GetEntity<View_ProdInfo>(detail.ProdDetailID);
            if (prodInfo != null)
            {
                tipsMsg.Add($"采购退货单{source.PurEntryReNo}，{prodInfo.CNName + prodInfo.Specifications}已交回数为{reDetail.DeliveredQuantity}，可入库数为{detail.Quantity}，当前行数据忽略！");
            }
            else
            {
                tipsMsg.Add($"采购退货单{source.PurEntryReNo}，产品已交回数为{reDetail.DeliveredQuantity}，可入库数为{detail.Quantity}，当前行数据忽略！");
            }
        }

        /// <summary>
        /// 处理运费逻辑 - 重复入库时运费为0
        /// </summary>
        private void ProcessFreightLogic(tb_PurEntryRe source, tb_PurReturnEntry target)
        {
            //可能字段要完善调整。
            //if (source.ActualAmount != null && source.tb_PurReturnEntry.Count > 0)
            //{
            //    if (source. > 0)
            //    {
            //        _logger.LogInformation("当前退货单已经有入库记录，运费退回为零，退货单号：{PurEntryReNo}", source.PurEntryReNo);
            //        target.Freight = 0;
            //    }
            //    else
            //    {
            //        _logger.LogInformation("当前退货单已经有入库记录，退货单号：{PurEntryReNo}", source.PurEntryReNo);
            //    }
            //}
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        private void RecalculateSummaryFields(tb_PurReturnEntry target)
        {
            if (target.tb_PurReturnEntryDetails != null && target.tb_PurReturnEntryDetails.Any())
            {
                target.TotalQty = target.tb_PurReturnEntryDetails.Sum(c => c.Quantity);
                target.TotalAmount = target.tb_PurReturnEntryDetails.Sum(c => c.UnitPrice * c.Quantity);
                target.TotalTaxAmount= target.tb_PurReturnEntryDetails.Sum(c => c.TaxAmount);
                target.TotalAmount = target.TotalAmount;// +target.shipcost;
            }
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_PurEntryRe source)
        {
            // 调用基类默认验证
            var result = await base.ValidateConversionAsync(source);
            
            if (result.CanConvert)
            {
                // 检查退货单状态是否为已审核
                if (source.ApprovalStatus != (int)ApprovalStatus.审核通过 || !source.ApprovalResults.GetValueOrDefault())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "只有已审核通过的采购退货单才能生成采购退货入库单";
                    return result;
                }
                
                // 检查是否有退货明细
                if (source.tb_PurEntryReDetails == null || !source.tb_PurEntryReDetails.Any())
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "该采购退货单没有明细，无法生成采购退货入库单";
                    return result;
                }
                
                // 添加明细数量业务验证
                await ValidateDetailQuantitiesAsync(source, result);
            }
            
            return result;
        }

        /// <summary>
        /// 验证明细数量的业务逻辑
        /// </summary>
        /// <param name="source">采购退货单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateDetailQuantitiesAsync(tb_PurEntryRe source, ValidationResult result)
        {
            try
            {
                int totalDetails = source.tb_PurEntryReDetails.Count;
                int returnableDetails = 0;
                int nonReturnableDetails = 0;
                decimal totalReturnableQty = 0;
                var cacheManager = _appContext?.GetRequiredService<IEntityCacheManager>();
                
                // 遍历所有退货明细，检查可入库数量
                foreach (var detail in source.tb_PurEntryReDetails)
                {
                    // 计算可入库数量 = 退货数量 - 已交回数量
                    decimal returnableQty = detail.Quantity - detail.DeliveredQuantity;
                    
                    if (returnableQty > 0)
                    {
                        returnableDetails++;
                        totalReturnableQty += returnableQty;
                        
                        // 如果可入库数量小于原退货数量，添加部分交回提示
                        if (returnableQty < detail.Quantity)
                        {
                            var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                            string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                            
                            result.AddWarning($"产品【{prodName}】已交回数量为{detail.DeliveredQuantity}，可入库数量为{returnableQty}，小于原退货数量{detail.Quantity}");
                        }
                    }
                    else
                    {
                        nonReturnableDetails++;
                        
                        // 添加完全交回提示
                        var prodInfo = cacheManager?.GetEntity<View_ProdInfo>(detail.ProdDetailID);
                        string prodName = prodInfo?.CNName ?? $"产品ID:{detail.ProdDetailID}";
                        
                        result.AddWarning($"产品【{prodName}】已全部交回，可入库数量为0，将忽略此明细");
                    }
                }
                
                // 添加汇总提示信息
                if (nonReturnableDetails > 0)
                {
                    result.AddInfo($"共有{nonReturnableDetails}项产品已全部交回，将在转换时忽略");
                }
                
                if (returnableDetails > 0)
                {
                    result.AddInfo($"共有{returnableDetails}项产品可入库，总可入库数量为{totalReturnableQty}");
                }
                
                // 如果所有明细都已交回，添加警告但仍允许转换（让用户知道）
                if (returnableDetails == 0)
                {
                    result.AddWarning("该退货单所有明细已全部交回，转换生成的入库单将没有明细数据");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证采购退货单明细数量时发生错误，退货单号：{PurEntryReNo}", source?.PurEntryReNo);
                result.AddWarning("验证明细数量时发生错误，请检查数据完整性");
            }
        }
    }
}