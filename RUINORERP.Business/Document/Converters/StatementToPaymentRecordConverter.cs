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
    /// 对账单到收付款单转换器
    /// 负责将对账单及其明细转换为收付款单及其明细
    /// 复用业务层的核心转换逻辑，确保数据一致性
    /// </summary>
    public class StatementToPaymentRecordConverter : DocumentConverterBase<tb_FM_Statement, tb_FM_PaymentRecord>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<StatementToPaymentRecordConverter> _logger;
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
        public StatementToPaymentRecordConverter(
            ILogger<StatementToPaymentRecordConverter> logger,
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
        /// <param name="source">源单据：对账单</param>
        /// <param name="target">目标单据：收付款单</param>
        /// <returns>转换后的目标单据</returns>
        protected override async Task PerformConversionAsync(tb_FM_Statement source, tb_FM_PaymentRecord target)
        {
            try
            {
                if (source == null)
                {
                    throw new ArgumentException("对账单不能为空");
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

                // 设置支付日期
                target.PaymentDate = DateTime.Now;

                // 设置非佣金相关
                target.IsForCommission = false;

                // 设置客户信息
                target.CustomerVendor_ID = source.CustomerVendor_ID;
                target.PayeeInfoID = source.PayeeInfoID;
                target.Employee_ID = source.Employee_ID;
                target.PayeeAccountNo = source.PayeeAccountNo;

                // 设置为本位币
                target.Currency_ID = _appContext.BaseCurrency.Currency_ID;

                // 初始化明细集合
                if (target.tb_FM_PaymentRecordDetails == null)
                {
                    target.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();
                }

                // 转换明细 - 复用业务层核心逻辑
                await ConvertDetailsAsync(source, target);

                // 计算汇总字段
                RecalculateSummaryFields(target);

                // 根据对账单类型设置收付款类型
                target.ReceivePaymentType = source.ReceivePaymentType;

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
                _logger.LogError(ex, "对账单到收付款单转换失败，对账单号：{StatementNo}", 
                    source?.StatementNo ?? "未知");
                throw;
            }
        }

        /// <summary>
        /// 转换明细 - 复用业务层核心逻辑
        /// </summary>
        private async Task ConvertDetailsAsync(tb_FM_Statement source, tb_FM_PaymentRecord target)
        {
            var detail = _mapper.Map<tb_FM_PaymentRecordDetail>(source);
            var newDetails = new List<tb_FM_PaymentRecordDetail>();

            #region 明细转换
            tb_FM_PaymentRecordDetail paymentRecordDetail = detail;
            
            // 设置来源业务类型为对账单
            paymentRecordDetail.SourceBizType = (int)BizType.对账单;
            
            // 设置摘要
            paymentRecordDetail.Summary = $"本次生成的{Enum.GetName(typeof(ReceivePaymentType), source.ReceivePaymentType)}款金额：{Math.Abs(source.ClosingBalanceLocalAmount):F2},由应{Enum.GetName(typeof(ReceivePaymentType), source.ReceivePaymentType)}对账单的剩余未付金额自动生成。";
            
            // 设置币别
            paymentRecordDetail.Currency_ID = target.Currency_ID;

            // 处理对账单生成收付款单的金额逻辑
            // 根据对账单类型和余额进行处理：
            // 1. 付款对账单：确保金额为正数
            // 2. 收款对账单：确保金额为正数
            // 3. 余额对账模式：根据余额正负值处理
            if (source.ReceivePaymentType == (int)ReceivePaymentType.付款)
            {
                // 付款对账单，无论余额正负，付款金额都应为正数
                paymentRecordDetail.LocalAmount = Math.Abs(source.ClosingBalanceLocalAmount);
                paymentRecordDetail.LocalPayableAmount = Math.Abs(source.ClosingBalanceLocalAmount);
            }
            else if (source.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                // 收款对账单，收款金额应为正数
                paymentRecordDetail.LocalAmount = Math.Abs(source.ClosingBalanceLocalAmount);
                paymentRecordDetail.LocalPayableAmount = Math.Abs(source.ClosingBalanceLocalAmount);
            }
            
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
        /// <param name="source">源单据：对账单</param>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateConversionAsync(tb_FM_Statement source)
        {
            var result = new ValidationResult { CanConvert = true };

            try
            {
                // 检查源单据是否为空
                if (source == null)
                {
                    result.CanConvert = false;
                    result.ErrorMessage = "对账单不能为空";
                    return result;
                }

                // 检查对账单状态
                if (source.StatementStatus != (int)StatementStatus.已确认 && 
                    source.StatementStatus != (int)StatementStatus.部分结算 ||
                    source.ApprovalStatus != (int)ApprovalStatus.已审核 ||
                    !source.ApprovalResults.HasValue || 
                    !source.ApprovalResults.Value)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.CanConvert = false;
                    result.ErrorMessage = $"对账单{source.StatementNo}状态不符合转换条件，只能转换已审核且状态为[已确认]或[部分结算]的对账单";
                    return result;
                }

                // 添加关于金额计算的提示信息
                decimal absAmount = Math.Abs(source.ClosingBalanceLocalAmount);
                if (absAmount > 0)
                {
                    var paymentType = (ReceivePaymentType)source.ReceivePaymentType;
                    result.AddInfo($"转换金额为{absAmount:F2}，生成为{paymentType}单");
                }
                else
                {
                    result.AddInfo($"转换金额为0，仍将按对账单类型生成收付款单");
                }
                
                await Task.CompletedTask; // 满足异步方法签名要求
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "验证对账单转换条件时发生错误");
                result.CanConvert = false;
                result.ErrorMessage = $"验证转换条件时发生错误：{ex.Message}";
            }

            return result;
        }
    }
}