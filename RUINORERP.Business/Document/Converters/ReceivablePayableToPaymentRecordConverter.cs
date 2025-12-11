using AutoMapper;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Document;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business.Document.Converters
{
    /// <summary>
    /// 应收应付款单到收付款单转换器
    /// 负责将应收应付款单及其明细转换为收付款单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class ReceivablePayableToPaymentRecordConverter : DocumentConverterBase<tb_FM_ReceivablePayable, tb_FM_PaymentRecord>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ReceivablePayableToPaymentRecordConverter> _logger;
        private readonly IBizCodeGenerateService _bizCodeService;
        private readonly ApplicationContext _appContext;
        private readonly AuthorizeController _authorizeController;
        /// <summary>
        /// 构造函数 - 依赖注入
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="mapper">AutoMapper映射器</param>
        /// <param name="bizCodeService">业务编码生成服务</param>
        /// <param name="appContext">应用程序上下文</param>
        /// <param name="authorizeController">权限控制器</param>
        public ReceivablePayableToPaymentRecordConverter(
            ILogger<ReceivablePayableToPaymentRecordConverter> logger,
            IMapper mapper,
            IBizCodeGenerateService bizCodeService,
            ApplicationContext appContext,
            AuthorizeController authorizeController)
            : base(logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bizCodeService = bizCodeService ?? throw new ArgumentNullException(nameof(bizCodeService));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
            _authorizeController = authorizeController ?? throw new ArgumentNullException(nameof(authorizeController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 转换器显示名称
        /// 使用基类实现，从Description特性获取
        /// </summary>
        public override string DisplayName => base.DisplayName;
        
        /// <summary>
        /// 执行具体的转换逻辑 - 复用业务层核心逻辑
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <param name="target">目标单据：收付款单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_FM_ReceivablePayable source, tb_FM_PaymentRecord target)
        {
            try
            {
                if (source == null )
                {
                    throw new ArgumentException("应收应付款单不能为空");
                }

                // 使用AutoMapper进行基础映射
                _mapper.Map(source, target);

                // 重置状态字段 - 与业务层保持一致
                target.ApprovalResults = null;
                target.ApprovalStatus = (int)ApprovalStatus.未审核;
                target.Approver_at = null;
                target.Approver_by = null;
                target.PrintStatus = 0;
                target.ActionStatus = ActionStatus.新增;
                target.ApprovalOpinions = "";
                target.Modified_at = null;
                target.Modified_by = null;
                target.Created_by = null;
                target.Created_at = null;

                // 设置收付类型和佣金标识
                target.ReceivePaymentType = source.ReceivePaymentType;
                target.IsForCommission = source.IsForCommission;

                // 设置支付日期
                target.PaymentDate = DateTime.Now;

                // 设置客户信息
                target.CustomerVendor_ID = source.CustomerVendor_ID;
                target.PayeeInfoID = source.PayeeInfoID;
                target.PayeeAccountNo = source.PayeeAccountNo;

                // 初始化明细集合
                if (target.tb_FM_PaymentRecordDetails == null)
                {
                    target.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 计算汇总字段
                RecalculateSummaryFields(target);

                // 检查是否为红字单据
                if (target.TotalForeignAmount < 0 || target.TotalLocalAmount < 0)
                {
                    target.IsReversed = true;
                }

                // 生成收付款单号
                if (source.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    target.PaymentNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.收款单, CancellationToken.None);
                    // 检查是否全部来自平台
                    if (target.tb_FM_PaymentRecordDetails.All(c => c.IsFromPlatform.HasValue && c.IsFromPlatform.Value))
                    {
                        target.IsFromPlatform = true;
                    }
                }
                else
                {
                    target.PaymentNo = await _bizCodeService.GenerateBizBillNoAsync(BizType.付款单, CancellationToken.None);
                }

                // 设置来源单号
                target.SourceBillNos = string.Join(",", target.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

                // 验证明细中是否有重复的单据
                ValidateDuplicateDetails(target);

                // 初始化实体
                BusinessHelper.Instance.InitEntity(target);
                target.PaymentStatus = (int)PaymentStatus.草稿;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "应收应付款单到收付款单转换失败，应收应付款单号：{ARAPNo}", 
                    source?.ARAPNo ?? "未知");
                throw;
            }
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_FM_ReceivablePayable source, tb_FM_PaymentRecord target)
        {
            var details = _mapper.Map<tb_FM_PaymentRecordDetail>(source);
            var newDetails = new List<tb_FM_PaymentRecordDetail>();

            #region 明细转换
            tb_FM_PaymentRecordDetail paymentRecordDetail = details;
            
            // 设置来源业务类型
            if (target.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecordDetail.SourceBizType = (int)BizType.应收款单;
            }
            else
            {
                paymentRecordDetail.SourceBizType = (int)BizType.应付款单;
            }
            
            // 设置摘要
            paymentRecordDetail.Summary = $"由应{((ReceivePaymentType)target.ReceivePaymentType).ToString()}转换自动生成。";
            
            // 添加源单据的备注信息
            if (!string.IsNullOrEmpty(source.Remark))
            {
                paymentRecordDetail.Summary += source.Remark;
            }

            // 设置应付金额
            paymentRecordDetail.LocalPayableAmount = details.LocalAmount;
            
            // 关联到收付款单
            paymentRecordDetail.PaymentId = target.PaymentId;
            
            #endregion
            newDetails.Add(paymentRecordDetail);

            target.tb_FM_PaymentRecordDetails = newDetails;

            await Task.CompletedTask; // 满足异步方法签名要求
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        /// <param name="target">收付款单</param>
        private void RecalculateSummaryFields(tb_FM_PaymentRecord target)
        {
            if (target.tb_FM_PaymentRecordDetails == null || !target.tb_FM_PaymentRecordDetails.Any())
            {
                target.TotalForeignAmount = 0;
                target.TotalLocalAmount = 0;
                target.TotalLocalPayableAmount = 0;
                return;
            }

            // 计算外币总金额
            target.TotalForeignAmount = target.tb_FM_PaymentRecordDetails.Sum(c => c.ForeignAmount);
            
            // 计算本币总金额
            target.TotalLocalAmount = target.tb_FM_PaymentRecordDetails.Sum(c => c.LocalAmount);
            
            // 计算应付总金额
            target.TotalLocalPayableAmount = target.tb_FM_PaymentRecordDetails.Sum(c => c.LocalPayableAmount);
        }

        /// <summary>
        /// 验证明细中是否有重复的单据
        /// </summary>
        /// <param name="target">收付款单</param>
        private void ValidateDuplicateDetails(tb_FM_PaymentRecord target)
        {
            // 查找重复的单据（按业务类型和单据ID组合）
            var duplicates = target.tb_FM_PaymentRecordDetails
                .GroupBy(c => new { c.SourceBizType, c.SourceBilllId })
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicates.Any())
            {
                var errorBuilder = new System.Text.StringBuilder();
                errorBuilder.AppendLine("收付款单明细中，同一业务下同一张单据不能重复分次收款。");
                errorBuilder.AppendLine("重复单据详情：");

                foreach (var duplicateGroup in duplicates)
                {
                    errorBuilder.AppendLine($"  - 业务类型: {(BizType)duplicateGroup.Key.SourceBizType}，单据ID: {duplicateGroup.Key.SourceBilllId}");
                    foreach (var item in duplicateGroup)
                    {
                        errorBuilder.AppendLine($"    - 明细来源: {item.SourceBillNo}，金额: {item.LocalAmount}");
                    }
                }

                errorBuilder.AppendLine("\r\n相同业务下的单据必须合并为一行。");
                throw new InvalidOperationException(errorBuilder.ToString());
            }
        }

        /// <summary>
        /// 验证转换条件
        /// </summary>
        /// <param name="source">源单据：应收应付款单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_FM_ReceivablePayable source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "应收应付款单不能为空";
                    return result;
                }

                // 检查应收应付款单状态
                if (source.ARAPStatus != (int)ARAPStatus.待支付 && 
                    source.ARAPStatus != (int)ARAPStatus.部分支付 ||
                    source.ApprovalStatus != (int)ApprovalStatus.已审核 ||
                    !source.ApprovalResults.HasValue || 
                    !source.ApprovalResults.Value)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = $"应收应付款单{source.ARAPNo}状态不符合转换条件，只能转换已审核且状态为[待支付]或[部分支付]的应收应付款单";
                    return result;
                }

                // 检查是否有可抵扣的预收付款单
                await ValidateAvailableAdvancesAsync(source, result);
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证应收应付款单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
        
        /// <summary>
        /// 验证是否有可抵扣的预收付款单
        /// </summary>
        /// <param name="source">应收应付款单</param>
        /// <param name="result">验证结果对象</param>
        private async Task ValidateAvailableAdvancesAsync(tb_FM_ReceivablePayable source, ValidationResult result)
        {
            try
            {
                // 创建应收应付款单控制器
                var receivablePayableController = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                
                // 查找可抵扣的预收付款单
                var sourceList = new List<tb_FM_ReceivablePayable> { source };
                var availableAdvances = await receivablePayableController.FindAvailableAdvances(sourceList);
                
                if (availableAdvances.Any())
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.AddWarning($"有可抵扣的预{paymentType}单，请先进行抵扣操作！");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证可抵扣预收付款单时发生错误");
                result.AddWarning("验证可抵扣预收付款单时发生错误，请检查数据完整性");
            }
        }
    }
}