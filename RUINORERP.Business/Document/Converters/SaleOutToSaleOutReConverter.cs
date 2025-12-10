using RUINORERP.Model;
using RUINORERP.Business.Document;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using RUINORERP.Global;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Cache;
using RUINORERP.Business.AutoMapper;
using RUINORERP.IServices;
using RUINORERP.Model.Context;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 销售出库单到销售退回单转换器
    /// 负责将销售出库单及其明细转换为销售退回单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class SaleOutToSaleOutReConverter : DocumentConverterBase<tb_SaleOut, tb_SaleOutRe>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<SaleOutToSaleOutReConverter> _logger;
        private readonly IBizCodeGenerateService _bizCodeService;
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        public SaleOutToSaleOutReConverter(
            IMapper mapper,
            ILogger<SaleOutToSaleOutReConverter> logger,
            IBizCodeGenerateService bizCodeService,
            ApplicationContext appContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bizCodeService = bizCodeService ?? throw new ArgumentNullException(nameof(bizCodeService));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }

        /// <summary>
        /// 源单据类型名称
        /// </summary>
        public override string SourceDocumentType => "销售出库单";
        
        /// <summary>
        /// 目标单据类型名称
        /// </summary>
        public override string TargetDocumentType => "销售退回单";
        
        /// <summary>
        /// 转换器显示名称
        /// </summary>
        public override string DisplayName => "转为销售退回单";
        
        /// <summary>
        /// 执行具体的转换逻辑 - 复用业务层核心逻辑
        /// </summary>
        /// <param name="source">源单据：销售出库单</param>
        /// <param name="target">目标单据：销售退回单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_SaleOut source, tb_SaleOutRe target)
        {
            try
            {
                _logger.LogInformation("开始执行销售出库单到销售退回单转换，出库单号：{SaleOutNo}", source.SaleOutNo);

                // 使用AutoMapper进行基础映射
                _mapper.Map(source, target);

                // 重置状态字段 - 与业务层保持一致
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
                target.PayStatus = null;
                target.Paytype_ID = null;
                target.RefundStatus = (int)RefundStatus.未退款等待退货;

                // 生成退货单号
                target.ReturnNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.销售退回单);
                target.ReturnDate = DateTime.Now;
                target.Notes = $"由销售出库单{source.SaleOutNo}生成";

                // 设置关联信息
                if (target.SaleOut_MainID.HasValue && target.SaleOut_MainID > 0)
                {
                    target.CustomerVendor_ID = source.CustomerVendor_ID;
                    target.SaleOut_NO = source.SaleOutNo;
                    target.IsFromPlatform = source.IsFromPlatform;
                }

                // 初始化明细集合
                if (target.tb_SaleOutReDetails == null)
                {
                    target.tb_SaleOutReDetails = new List<tb_SaleOutReDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 处理运费逻辑 - 重复退库时运费为0
                ProcessFreightLogic(source, target);

                // 重新计算汇总字段
                RecalculateSummaryFields(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);

                _logger.LogInformation("销售出库单到销售退回单转换完成，生成退货单号：{ReturnNo}", target.ReturnNo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "销售出库单到销售退回单转换失败，出库单号：{SaleOutNo}", source.SaleOutNo);
                throw;
            }
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_SaleOut source, tb_SaleOutRe target)
        {
            var details = _mapper.Map<List<tb_SaleOutReDetail>>(source.tb_SaleOutDetails);
            var newDetails = new List<tb_SaleOutReDetail>();
            var tipsMsg = new List<string>();

            for (int i = 0; i < details.Count; i++)
            {
                await ProcessDetailItemAsync(source, details[i], newDetails, tipsMsg, i);
            }

            if (newDetails.Count == 0)
            {
                tipsMsg.Add($"出库单:{source.SaleOutNo}已全部退库，请检查是否正在重复退库！");
                _logger.LogWarning("销售出库单已全部退库，出库单号：{SaleOutNo}", source.SaleOutNo);
            }

            target.tb_SaleOutReDetails = newDetails;

            // 记录提示信息
            if (tipsMsg.Any())
            {
                _logger.LogInformation("转换过程中的提示信息：{Tips}", string.Join("; ", tipsMsg));
            }
        }

        /// <summary>
        /// 处理单个明细项 - 复用业务层核心逻辑
        /// </summary>
        private async Task ProcessDetailItemAsync(tb_SaleOut source, tb_SaleOutReDetail detail, List<tb_SaleOutReDetail> newDetails, List<string> tipsMsg, int index)
        {
            try
            {
                // 查找对应的出库明细
                var outDetail = FindCorrespondingOutDetail(source, detail);
                if (outDetail == null)
                {
                    _logger.LogWarning("找不到对应的出库明细，产品明细ID：{ProdDetailID}", detail.ProdDetailID);
                    return;
                }

                // 设置成本和自定义成本
                detail.Cost = outDetail.Cost;
                detail.CustomizedCost = outDetail.CustomizedCost;

                // 处理成本为0的情况 - 从库存获取成本
                if (detail.Cost == 0)
                {
                    await SetCostFromInventoryAsync(detail);
                }

                // 计算可退数量 = 出库数量 - 已退回数量
                detail.Quantity = outDetail.Quantity - outDetail.TotalReturnedQty;

                // 重新计算金额
                detail.SubtotalTransAmount = detail.TransactionPrice * detail.Quantity;
                detail.SubtotalCostAmount = (detail.Cost + detail.CustomizedCost) * detail.Quantity;

                // 检查是否可退
                if (detail.Quantity > 0)
                {
                    newDetails.Add(detail);
                }
                else
                {
                    AddQuantityWarning(tipsMsg, source, outDetail, detail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理明细项时发生错误，索引：{Index}", index);
                throw;
            }
        }

        /// <summary>
        /// 查找对应的出库明细 - 处理重复产品情况
        /// </summary>
        private tb_SaleOutDetail FindCorrespondingOutDetail(tb_SaleOut source, tb_SaleOutReDetail detail)
        {
            // 检查是否有重复产品
            var duplicateProdDetails = source.tb_SaleOutDetails
                .Where(x => x.ProdDetailID == detail.ProdDetailID)
                .ToList();

            if (duplicateProdDetails.Count > 1)
            {
                // 多行情况，需要更精确匹配
                return source.tb_SaleOutDetails.FirstOrDefault(c => 
                    c.ProdDetailID == detail.ProdDetailID &&
                    c.Location_ID == detail.Location_ID &&
                    c.SaleOutDetail_ID == detail.SaleOutDetail_ID);
            }
            else
            {
                // 单行情况
                return source.tb_SaleOutDetails.FirstOrDefault(c => 
                    c.ProdDetailID == detail.ProdDetailID &&
                    c.Location_ID == detail.Location_ID);
            }
        }

        /// <summary>
        /// 从库存获取成本
        /// </summary>
        private async Task SetCostFromInventoryAsync(tb_SaleOutReDetail detail)
        {
            try
            {
                var prodDetail = Cache.EntityCacheHelper.GetEntity<View_ProdDetail>(detail.ProdDetailID);
                if (prodDetail != null && prodDetail is View_ProdDetail viewProd)
                {
                    detail.Cost = viewProd.Inv_Cost ?? 0m;
                    _logger.LogInformation("从库存获取成本，产品明细ID：{ProdDetailID}，成本：{Cost}", 
                        detail.ProdDetailID, detail.Cost);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "从库存获取成本失败，产品明细ID：{ProdDetailID}", detail.ProdDetailID);
                detail.Cost = 0; // 确保成本有默认值
            }
        }

        /// <summary>
        /// 添加数量警告信息
        /// </summary>
        private void AddQuantityWarning(List<string> tipsMsg, tb_SaleOut source, tb_SaleOutDetail outDetail, tb_SaleOutReDetail detail)
        {
            var prodInfo = Cache.EntityCacheHelper.GetEntity<View_ProdInfo>(detail.ProdDetailID);
            if (prodInfo != null)
            {
                tipsMsg.Add($"销售出库单{source.SaleOutNo}，{prodInfo.CNName + prodInfo.Specifications}已退回数为{outDetail.TotalReturnedQty}，可退库数为{detail.Quantity}，当前行数据忽略！");
            }
            else
            {
                tipsMsg.Add($"销售出库单{source.SaleOutNo}，产品已退回数为{outDetail.TotalReturnedQty}，可退库数为{detail.Quantity}，当前行数据忽略！");
            }
        }

        /// <summary>
        /// 处理运费逻辑 - 重复退库时运费为0
        /// </summary>
        private void ProcessFreightLogic(tb_SaleOut source, tb_SaleOutRe target)
        {
            if (source.tb_SaleOutRes != null && source.tb_SaleOutRes.Count > 0)
            {
                if (source.FreightIncome > 0)
                {
                    _logger.LogInformation("当前出库单已经有退库记录，运费收入退回为零，出库单号：{SaleOutNo}", source.SaleOutNo);
                    target.FreightIncome = 0;
                }
                else
                {
                    _logger.LogInformation("当前出库单已经有退库记录，出库单号：{SaleOutNo}", source.SaleOutNo);
                }
            }
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        private void RecalculateSummaryFields(tb_SaleOutRe target)
        {
            if (target.tb_SaleOutReDetails != null && target.tb_SaleOutReDetails.Any())
            {
                target.TotalQty = target.tb_SaleOutReDetails.Sum(c => c.Quantity);
                target.TotalAmount = target.tb_SaleOutReDetails.Sum(c => c.TransactionPrice * c.Quantity);
                target.TotalAmount = target.TotalAmount + target.FreightIncome;

                _logger.LogInformation("重新计算汇总字段，总数量：{TotalQty}，总金额：{TotalAmount}", 
                    target.TotalQty, target.TotalAmount);
            }
        }
        
        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据对象</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_SaleOut source)
        {
            // 调用基类默认验证
            var result = await base.ValidateConversionAsync(source);
            
            if (result.CanConvert)
            {
                // 检查出库单状态是否为已审核
                if (source.ApprovalStatus != (int)ApprovalStatus.已审核 || !source.ApprovalResults.GetValueOrDefault())
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
            }
            
            return result;
        }
    }
}