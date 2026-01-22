

// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:33:16
// **************************************
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Config;
using RUINORERP.Business.EntityLoadService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Model;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IServices;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using IMapper = AutoMapper.IMapper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PaymentRecordController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 授权控制器，用于获取系统配置信息
        /// </summary>
        private IAuthorizeController _authorizeController;

        /// <summary>
        /// 初始化授权控制器（延迟加载，避免构造函数冲突）
        /// </summary>
        private void InitializeAuthorizeController()
        {
            if (_authorizeController == null)
            {
                _authorizeController = _appContext?.GetRequiredService<IAuthorizeController>();
            }
        }

        /// <summary>
        /// 收付款单反审核方法
        /// 注意：收付款单不支持反审核操作，必须通过红字冲销的方式处理
        /// </summary>
        /// <param name="ObjectEntity">要反审核的收付款单实体</param>
        /// <returns>返回错误信息，指示收付款单不能反审</returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            rmrs.ErrorMsg = "收付款单不支持反审核操作，请使用红字冲销方式处理";
            await Task.Delay(0);
            return rmrs;
        }

        /// <summary>
        /// 来源单据金额信息
        /// </summary>
        private class SourceBillAmountInfo
        {
            /// <summary>
            /// 来源单据总金额（本币）
            /// </summary>
            public decimal TotalAmount { get; set; }

            /// <summary>
            /// 历史已付款金额（本币）
            /// </summary>
            public decimal HistoricalPaidAmount { get; set; }

            /// <summary>
            /// 剩余可付款金额（本币）
            /// </summary>
            public decimal RemainingAmount => TotalAmount - HistoricalPaidAmount;
        }

        /// <summary>
        /// 获取来源单据的金额信息和历史付款情况
        /// </summary>
        /// <param name="sourceBillId">来源单据ID</param>
        /// <param name="sourceBizType">来源业务类型</param>
        /// <returns>金额信息结果</returns>
        private async Task<ReturnResults<SourceBillAmountInfo>> GetSourceBillAmountInfo(long sourceBillId, int sourceBizType)
        {
            var result = new ReturnResults<SourceBillAmountInfo>();

            try
            {
                decimal totalAmount = 0;
                decimal historicalPaidAmount = 0;

                // 根据业务类型获取来源单据的总金额
                switch ((BizType)sourceBizType)
                {
                    case BizType.对账单:
                        var statement = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Statement>()
                            .Where(s => s.StatementId == sourceBillId)
                            .FirstAsync();

                        if (statement == null)
                        {
                            result.ErrorMsg = $"未找到对账单ID: {sourceBillId}";
                            result.Succeeded = false;
                            _logger?.LogWarning(result.ErrorMsg);
                            return result;
                        }

                        // 对账单的总应付金额（本币）
                        totalAmount = statement.ReceivePaymentType == (int)ReceivePaymentType.收款 ?
                            statement.TotalReceivableLocalAmount :
                            statement.TotalPayableLocalAmount;
                        break;

                    case BizType.应收款单:
                    case BizType.应付款单:
                        var receivablePayable = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayable>()
                            .Where(rp => rp.ARAPId == sourceBillId)
                            .FirstAsync();

                        if (receivablePayable == null)
                        {
                            result.ErrorMsg = $"未找到应收/应付款单ID: {sourceBillId}";
                            result.Succeeded = false;
                            return result;
                        }

                        // 应收应付单的总金额（本币）
                        totalAmount = receivablePayable.TotalLocalPayableAmount;
                        break;

                    case BizType.预收款单:
                    case BizType.预付款单:
                        var preReceivedPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                            .Where(prp => prp.PreRPID == sourceBillId)
                            .FirstAsync();

                        if (preReceivedPayment == null)
                        {
                            result.ErrorMsg = $"未找到预收/付款单ID: {sourceBillId}";
                            result.Succeeded = false;
                            return result;
                        }

                        // 预收付款单的总金额（本币）
                        totalAmount = preReceivedPayment.LocalPrepaidAmount;
                        break;
                    case BizType.费用报销单:
                        var ExpenseClaimPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseClaim>()
                       .Where(prp => prp.ClaimMainID == sourceBillId)
                       .FirstAsync();

                        if (ExpenseClaimPayment == null)
                        {
                            result.ErrorMsg = $"未找到费用报销单ID: {sourceBillId}";
                            result.Succeeded = false;
                            return result;
                        }

                        // 费用报销单的总金额（本币）
                        totalAmount = ExpenseClaimPayment.ClaimAmount;
                        break;
                    default:
                        // 对于其他业务类型，暂时无法获取总金额，返回错误
                        result.ErrorMsg = $"不支持的来源业务类型: {(BizType)sourceBizType}";
                        result.Succeeded = false;
                        return result;
                }

                // 获取历史已付款金额（排除当前正在处理的付款单）
                historicalPaidAmount = await GetHistoricalPaidAmount(sourceBillId, sourceBizType);

                result.ReturnObject = new SourceBillAmountInfo
                {
                    TotalAmount = totalAmount,
                    HistoricalPaidAmount = historicalPaidAmount
                };

                result.Succeeded = true;
                return result;
            }
            catch (Exception ex)
            {
                result.ErrorMsg = $"获取来源单据金额信息时发生错误: {ex.Message}";
                result.Succeeded = false;
                return result;
            }
        }

        /// <summary>
        /// 获取来源单据的历史已付款金额
        /// </summary>
        /// <param name="sourceBillId">来源单据ID</param>
        /// <param name="sourceBizType">来源业务类型</param>
        /// <returns>历史已付款金额</returns>
        private async Task<decimal> GetHistoricalPaidAmount(long sourceBillId, int sourceBizType)
        {
            try
            {
                // 查询已审核的付款记录明细，计算历史已付款金额
                // 注意：这里只查询已审核通过的记录，当前正在审核的记录不应计入历史
                var paidAmount = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecordDetail>()
                    .Where(prd => prd.SourceBilllId == sourceBillId
                        && prd.SourceBizType == sourceBizType
                        && prd.tb_fm_paymentrecord.ApprovalStatus == (int)ApprovalStatus.审核通过
                        && prd.tb_fm_paymentrecord.PaymentStatus == (int)PaymentStatus.已支付
                        )
                    .SumAsync(prd => prd.LocalAmount);


                return paidAmount;  // SumAsync返回decimal类型，不会是null
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"获取历史已付款金额时发生错误，来源单据ID: {sourceBillId}, 业务类型: {(BizType)sourceBizType}");
                return 0; // 发生错误时返回0，避免阻塞业务流程
            }
        }

        /// <summary>
        /// 收付款单审核方法
        /// 功能说明：
        /// 1. 验证收付款单基本信息
        /// 2. 根据来源业务类型分组处理不同业务场景的核销逻辑
        /// 3. 对账单来源的收付款单采用FIFO（先进先出）方式核销
        /// 4. 更新相关单据状态并生成核销记录
        /// 5. 处理关联业务单据的收款状态更新
        /// 6. 如果付款单是对报销单进行付款，则更新报销单的付款状态。并且要求付款金额与报销金额要一致
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new();
            tb_FM_PaymentRecord entity = ObjectEntity as tb_FM_PaymentRecord;
            var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
            try
            {
                if (entity == null)
                {
                    rmrs.ErrorMsg = "付款单不能为空!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    // 非平台来源且没有收款信息时，返回错误
                    if (!entity.PayeeInfoID.HasValue)
                    {
                        var configManagerService = _appContext.GetRequiredService<IConfigManagerService>();
                        var _validatorConfig = configManagerService.GetConfig<GlobalValidatorConfig>();
                        if (_validatorConfig.收付款账户必填)
                        {
                            rmrs.ErrorMsg = $"{entity.PaymentNo}付款时，对方的收款信息必填!";
                            rmrs.Succeeded = false;
                            rmrs.ReturnObject = entity as T;
                            return rmrs;
                        }
                    }
                }


                //不能直接将实体entity重新查询赋值，否则反应到UI时不是相同对象了。
                if (entity.tb_FM_PaymentRecordDetails == null)
                {
                    entity.tb_FM_PaymentRecordDetails = await _appContext.Db.Queryable<tb_FM_PaymentRecordDetail>()
                                .Where(c => c.PaymentId == entity.PaymentId).ToListAsync();
                }


                //如果一个单位，正好正向500，负数-500 ，相抵消是正好为0，则可以为零。审核后要将应收应付核销掉。
                //只有明细中有负数才可能等于0
                if (entity.TotalLocalAmount == 0 && entity.TotalForeignAmount == 0 && entity.tb_FM_PaymentRecordDetails.Sum(c => c.LocalAmount) != 0)
                {
                    rmrs.ErrorMsg = "非正负红字时，付款金额不能为0!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }
                if (entity.TotalLocalAmount == 0 && !entity.tb_FM_PaymentRecordDetails.Any(c => c.LocalAmount < 0))
                {
                    rmrs.ErrorMsg = "非正负红字时，付款金额不能为0!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                foreach (var PaymentRecordDetail in entity.tb_FM_PaymentRecordDetails)
                {
                    //审核时 要检测明细中对应的相同业务类型下不能有相同来源单号。除非有正负总金额为0对冲情况。或是两行数据?
                    // 查询逻辑：获取所有已支付状态的记录（包括已审核和待审核的），用于验证冲突
                    var PendingApprovalDetails = await _appContext.Db.Queryable<tb_FM_PaymentRecordDetail>()
                        .Includes(c => c.tb_fm_paymentrecord)
                        .Where(c => c.SourceBilllId == PaymentRecordDetail.SourceBilllId && c.SourceBizType == PaymentRecordDetail.SourceBizType)
                        .Where(c => c.tb_fm_paymentrecord.ApprovalStatus == (int)ApprovalStatus.审核通过)
                        .Where(c => c.tb_fm_paymentrecord.PaymentStatus == (int)PaymentStatus.已支付)
                        // 排除当前付款单的记录，避免重复计算
                        .Where(c => c.PaymentId != entity.PaymentId)
                        .ToListAsync();

                    // 调用验证方法，仅传递当前付款单的明细和来源单据信息
                    // 注意：不需要添加历史记录，因为GetHistoricalPaidAmount方法会自动获取历史已付款金额
                    var currentPaymentDetails = new List<tb_FM_PaymentRecordDetail> { PaymentRecordDetail };
                    bool isValid = await ValidatePaymentDetails(currentPaymentDetails, entity, rmrs);
                    if (!isValid)
                    {
                        //rmrs.ErrorMsg = "相同业务类型下不能有相同的来源单号!审核失败。";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }


                #region 关联单据状态处理

                //相同客户，多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                //为了提高性能 将按业务类型分组后再找到对应的单据去处理
                //目前 所有业务都进应收应付 简化逻辑 
                entity.tb_FM_PaymentRecordDetails.GroupBy(c => c.SourceBizType).Select(c => new { SourceBizType = c.Key }).ToList();

                var details = entity.tb_FM_PaymentRecordDetails;
                Dictionary<int, List<long>> GroupResult = details
                    .GroupBy(d => d.SourceBizType)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(d => d.PaymentDetailId).ToList()
                    );
                //应收付
                List<tb_FM_ReceivablePayable> receivablePayableUpdateList = new List<tb_FM_ReceivablePayable>();

                //预收付
                List<tb_FM_PreReceivedPayment> preReceivedPaymentUpdateList = new List<tb_FM_PreReceivedPayment>();
                List<tb_SaleOut> saleOutUpdateList = new List<tb_SaleOut>();
                List<tb_SaleOrder> saleOrderUpdateList = new List<tb_SaleOrder>();

                List<tb_PurEntry> purEntryUpdateList = new List<tb_PurEntry>();
                List<tb_PurOrder> purOrderUpdateList = new List<tb_PurOrder>();

                List<tb_PurEntryRe> purEntryReUpdateList = new List<tb_PurEntryRe>();
                List<tb_SaleOutRe> SaleOutReUpdateList = new List<tb_SaleOutRe>();

                List<tb_FM_PriceAdjustment> priceAdjustmentUpdateList = new List<tb_FM_PriceAdjustment>();
                List<tb_FM_PaymentRecord> oldPaymentUpdateList = new List<tb_FM_PaymentRecord>();
                List<tb_FM_OtherExpense> otherExpenseUpdateList = new List<tb_FM_OtherExpense>();
                List<tb_FM_ExpenseClaim> expenseClaimUpdateList = new List<tb_FM_ExpenseClaim>();


                List<tb_AS_RepairOrder> RepairOrderUpdateList = new List<tb_AS_RepairOrder>();


                List<tb_FM_Statement> StatementUpdateList = new List<tb_FM_Statement>();
                List<tb_FM_StatementDetail> StatementDetailUpdateList = new List<tb_FM_StatementDetail>();
                List<tb_FinishedGoodsInv> FinishedGoodsInvUpdateList = new List<tb_FinishedGoodsInv>();


                List<tb_FM_ExpenseClaim> ExpenseClaimUpdateList = new List<tb_FM_ExpenseClaim>();


                //相同客户，多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //收款单 中的明细中的业务类型，对账单，应收应付， 预收付。 三种只会一种？ 暂时不处理混合情况
                foreach (var group in GroupResult)
                {
                    long[] sourcebillids = entity.tb_FM_PaymentRecordDetails.Where(c => group.Value.Contains(c.PaymentDetailId)).Select(c => c.SourceBilllId).ToArray();

                    #region 对账单来的收款单,支付部分支付，所以按 则按上到下核销,从早到近FIFO原则核销

                    //收款单审核时。除了保存核销记录，还要来源的 如 应收中的 余额这种更新，对账单有核销标识
                    if (group.Key == (int)BizType.对账单)
                    {

                        List<tb_FM_Statement> StatementList = await _appContext.Db.Queryable<tb_FM_Statement>()
                             .Includes(c => c.tb_FM_StatementDetails, d => d.tb_fm_receivablepayable, f => f.tb_FM_ReceivablePayableDetails)
                             .Where(c => sourcebillids.Contains(c.StatementId))
                             .OrderBy(c => c.Created_at)
                             .ToListAsync();
                        foreach (var statement in StatementList)
                        {
                            tb_FM_PaymentRecordDetail RecordDetail = entity.tb_FM_PaymentRecordDetails.FirstOrDefault(c => c.SourceBilllId == statement.StatementId);
                            #region 应收单余额更新

                            decimal StatementPaidAmount = RecordDetail.LocalAmount;  // 实际支付金额 用于核销对账单
                            decimal ARAPTotalPaidAmount = RecordDetail.LocalAmount;//用于按FIFO核销应收应付集合

                            // 记录原始支付金额，用于后续验证
                            decimal originalPaymentAmount = RecordDetail.LocalAmount;

                            // 重构FIFO核销逻辑，优化混合对冲场景的处理
                            // 1. 先分离应收款和应付款
                            var receivableList = statement.tb_FM_StatementDetails
                                .Where(c => c.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                .Select(c => c.tb_fm_receivablepayable)
                                .OrderBy(c => c.BusinessDate)
                                .ThenBy(c => c.Created_at)
                                .ToList();

                            var payableList = statement.tb_FM_StatementDetails
                                .Where(c => c.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                .Select(c => c.tb_fm_receivablepayable)
                                .OrderBy(c => c.BusinessDate)
                                .ThenBy(c => c.Created_at)
                                .ToList();

                            // 2. 根据对账单类型和混合场景决定核销顺序和逻辑
                            List<tb_FM_ReceivablePayable> receivablePayableList = new List<tb_FM_ReceivablePayable>();

                            // 初始化授权控制器
                            InitializeAuthorizeController();

                            // 检查是否存在混合对冲场景
                            bool hasMixedTypes = receivableList.Any() && payableList.Any();

                            // 计算对冲金额和实际支付金额
                            decimal offsetAmount = 0;

                            // 计算所有明细的总净额（应收应付的代数和）
                            // 这是所有应收应付单相互抵冲后的最终净额
                            // 例如：应收3102元 + 应付6120元 = 净支付金额3018元
                            // 其中间 应收应付 单项中：也是求负，如果有负数（余额模式）会对冲。如  付款时：负-100，+500，则是付400
                            decimal totalNetAmount = 0;
                            if (hasMixedTypes)
                            {
                                // 混合场景：分别计算应收和应付的总和，然后相加得到总净额
                                totalNetAmount = receivableList.Sum(r => r.LocalBalanceAmount) + payableList.Sum(p => p.LocalBalanceAmount);
                            }
                            else
                            {
                                // 单一场景：直接计算所有明细的代数和
                                totalNetAmount = statement.tb_FM_StatementDetails.Sum(sd => sd.tb_fm_receivablepayable.LocalBalanceAmount);
                            }

                            // 获取金额计算容差阈值
                            decimal tolerance = _authorizeController.GetAmountCalculationTolerance();

                            // 业务规则检查：确保实际支付金额不超过净支付金额
                            // 说明：所有明细相互抵冲后的净支付金额是最高可支付限额
                            // 如果实付金额超过这个净额，则属于预付款业务，不应在对账单中处理
                            if (Math.Abs(ARAPTotalPaidAmount) > Math.Abs(totalNetAmount) + tolerance)
                            {
                                // 提供专业的错误提示，说明业务规则违反情况
                                _unitOfWorkManage.RollbackTran();
                                rmrs.ErrorMsg = $"实付金额（{ARAPTotalPaidAmount}）超过了应收应付抵冲后的净支付金额（{totalNetAmount}）。\n超过部分应作为预付款/预收款处理，而非通过对账单核销。";
                                rmrs.Succeeded = false;
                                rmrs.ReturnObject = entity as T;
                                return rmrs;

                            }

                            // 计算实付金额与总净额的关系
                            // 1. 如果实付金额的绝对值等于总净额的绝对值，说明完全匹配，应全额核销所有明细
                            // 2. 否则，按FIFO顺序进行部分核销
                            bool isExactMatch = Math.Abs(ARAPTotalPaidAmount) >= Math.Abs(totalNetAmount) - tolerance &&
                                              Math.Abs(ARAPTotalPaidAmount) <= Math.Abs(totalNetAmount) + tolerance;

                            if (hasMixedTypes && statement.ReceivePaymentType == (int)ReceivePaymentType.付款)
                            {
                                // 对于付款对账单中的混合对冲场景
                                // 例如：应收3102元+应付6120元=付款3018元
                                // 使用绝对值计算对冲金额，确保正确处理正负金额
                                // 对冲金额 = 所有应收款单余额的绝对值总和
                                offsetAmount = Math.Abs(receivableList.Sum(r => r.LocalBalanceAmount));

                                // 先核销应收款（对冲项），再核销应付款（实际支付项）
                                // 按FIFO顺序排列对冲项内部的单据：先按业务日期排序，再按创建时间排序
                                receivableList = receivableList.OrderBy(c => c.BusinessDate).ThenBy(c => c.Created_at).ToList();
                                payableList = payableList.OrderBy(c => c.BusinessDate).ThenBy(c => c.Created_at).ToList();

                                // 构建完整的核销顺序：先对冲项，后实际支付项
                                receivablePayableList.AddRange(receivableList);
                                receivablePayableList.AddRange(payableList);
                            }
                            else if (hasMixedTypes && statement.ReceivePaymentType == (int)ReceivePaymentType.收款)
                            {
                                // 对于收款对账单中的混合对冲场景
                                // 例如：应付3018元+应收6120元=收款3102元
                                // 使用绝对值计算对冲金额，确保正确处理正负金额
                                // 对冲金额 = 所有应付款单余额的绝对值总和
                                offsetAmount = Math.Abs(payableList.Sum(p => p.LocalBalanceAmount));

                                // 先核销应付款（对冲项），再核销应收款（实际支付项）
                                // 按FIFO顺序排列对冲项内部的单据：先按业务日期排序，再按创建时间排序
                                payableList = payableList.OrderBy(c => c.BusinessDate).ThenBy(c => c.Created_at).ToList();
                                receivableList = receivableList.OrderBy(c => c.BusinessDate).ThenBy(c => c.Created_at).ToList();

                                // 构建完整的核销顺序：先对冲项，后实际支付项
                                receivablePayableList.AddRange(payableList);
                                receivablePayableList.AddRange(receivableList);
                            }
                            else
                            {
                                // 正常情况，按原始FIFO顺序
                                receivablePayableList = statement.tb_FM_StatementDetails
                                    .Select(c => c.tb_fm_receivablepayable)
                                    .OrderBy(c => c.BusinessDate)  // 按业务日期排序
                                    .ThenBy(c => c.Created_at)     // 再按创建时间排序
                                    .ToList();
                            }


                            // 遍历所有待核销的应收应付单，按预设顺序执行核销操作
                            foreach (var receivablePayable in receivablePayableList)
                            {
                                //核销过的。不重复处理
                                if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                                {
                                    continue;
                                }

                                #region  核销对账单
                                var StatementDetail = statement.tb_FM_StatementDetails.FirstOrDefault(c => c.ARAPId == receivablePayable.ARAPId);
                                if (StatementDetail == null)
                                    continue;
                                // 改进对账单明细核销逻辑，考虑正负金额的特殊处理
                                // 对于正负金额混合的场景（如16200元和-400元），我们需要确保正确核销
                                // 获取明细的剩余未核销金额的绝对值，用于比较计算
                                decimal absRemainingAmount = Math.Abs(StatementDetail.RemainingLocalAmount);

                                // 计算本次可核销金额
                                decimal statementAmountToWriteOff;

                                // 核销逻辑判断：
                                // 1. 如果isExactMatch为true，表示全额核销场景，直接核销所有剩余金额
                                // 2. 否则，按常规逻辑判断是否全额核销或部分核销
                                if (isExactMatch)
                                {
                                    // 全额核销场景：直接核销该明细的所有剩余金额
                                    statementAmountToWriteOff = StatementDetail.RemainingLocalAmount;
                                }
                                else if (absRemainingAmount <= Math.Abs(StatementPaidAmount))
                                {
                                    // 非全额核销场景下，如果剩余未核销金额的绝对值小于等于对账单剩余支付金额的绝对值
                                    // 则全额核销该明细
                                    statementAmountToWriteOff = StatementDetail.RemainingLocalAmount;
                                }
                                else
                                {
                                    // 部分核销：计算核销比例，确保正确处理正负金额
                                    // 比例 = 对账单剩余支付金额绝对值 / 明细剩余未核销金额绝对值
                                    decimal ratio = Math.Abs(StatementPaidAmount) / absRemainingAmount;
                                    // 核销金额 = 明细剩余未核销金额 * 比例（保持原符号方向）
                                    statementAmountToWriteOff = StatementDetail.RemainingLocalAmount * ratio;
                                }

                                // 更新核销金额和剩余金额
                                // 已核销金额：根据原单据类型和金额符号决定如何累加
                                // 对于应收单（收款类型）：核销金额应保持与原单据相同的符号方向
                                // 对于应付款单（付款类型）：核销金额应保持与原单据相同的符号方向
                                StatementDetail.WrittenOffLocalAmount += statementAmountToWriteOff;
                                // 2. 剩余未核销金额：减去本次核销金额（保留符号，确保正负方向正确）
                                StatementDetail.RemainingLocalAmount -= statementAmountToWriteOff;

                                // 确保金额计算精度，避免浮点运算误差导致的问题
                                // 保留两位小数，符合财务记账要求
                                StatementDetail.WrittenOffLocalAmount = Math.Round(StatementDetail.WrittenOffLocalAmount, 2);
                                StatementDetail.RemainingLocalAmount = Math.Round(StatementDetail.RemainingLocalAmount, 2);

                                // 对账明细的核销状态判断
                                // 考虑浮点精度问题，当剩余金额的绝对值小于0.01时视为已全额核销
                                if (Math.Abs(StatementDetail.RemainingLocalAmount) < 0.01m)
                                {
                                    // 显式设置剩余金额为0，确保数据一致性
                                    StatementDetail.RemainingLocalAmount = 0;
                                    StatementDetail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.全额核销;
                                }
                                else
                                {
                                    StatementDetail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.部分核销;
                                }

                                // 减少对账单剩余待核销金额
                                // 使用绝对值扣减，确保净额计算正确性
                                // 例如：核销+100元和-100元，都应减少相同的剩余支付金额
                                StatementPaidAmount -= Math.Abs(statementAmountToWriteOff);

                                #endregion

                                // 根据应收/应付类型分别处理核销逻辑
                                // 重点修复混合对冲场景：确保正确计算对冲和实际支付金额
                                decimal amountToWriteOff = 0; // 本次核销金额

                                // 处理混合对冲场景
                                if (hasMixedTypes)
                                {
                                    if (statement.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                    {
                                        if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款) // 应收款单作为对冲项
                                        {
                                            // 付款对账单中的应收款单作为对冲项，应全额核销
                                            // 对冲逻辑：应收款直接冲抵应付款，无需实际支付
                                            amountToWriteOff = receivablePayable.LocalBalanceAmount;

                                            // 记录混合对冲的特殊处理
                                            FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                            fMAuditLog.CreateAuditLog<tb_FM_ReceivablePayable>($"付款对账单对冲应收款：核销金额{amountToWriteOff}",
                                                receivablePayable);
                                        }
                                        else if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款) // 应付款单
                                        {
                                            // 付款对账单中的应付款单，核销金额处理逻辑
                                            // 1. 如果实付金额与总净额完全匹配(isExactMatch)，则全额核销应付款单
                                            // 2. 否则，按FIFO原则进行部分核销，同时支持正负金额混合场景
                                            if (isExactMatch)
                                            {
                                                // 场景：实付金额与总净额完全匹配（例如：应付6120元，应收3102元，实付3018元）
                                                // 全额核销应付款单，无论金额正负
                                                amountToWriteOff = receivablePayable.LocalBalanceAmount;
                                            }
                                            else
                                            {
                                                // 场景：实付金额小于总净额，需要按FIFO顺序部分核销
                                                // 针对正负金额混合的情况（如采购调价/退货等），使用绝对值进行核销计算
                                                decimal netPaymentAmount = ARAPTotalPaidAmount;
                                                decimal writeOffBase = Math.Min(Math.Abs(receivablePayable.LocalBalanceAmount), Math.Abs(netPaymentAmount));

                                                // 保持与应付款单相同的符号方向
                                                // 确保核销金额的符号与原单据保持一致（正数表示应付，负数表示退货/调价）
                                                amountToWriteOff = receivablePayable.LocalBalanceAmount > 0 ? writeOffBase : -writeOffBase;
                                            }
                                        }
                                    }
                                    else if (statement.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                    {
                                        if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款) // 应付款单作为对冲项
                                        {
                                            // 收款对账单中的应付款单作为对冲项，应全额核销
                                            // 对冲逻辑：应付款直接冲抵应收款，无需实际收取
                                            amountToWriteOff = receivablePayable.LocalBalanceAmount;

                                            // 记录混合对冲的特殊处理
                                            FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                            fMAuditLog.CreateAuditLog<tb_FM_ReceivablePayable>($"收款对账单对冲应付款：核销金额{amountToWriteOff}",
                                                receivablePayable);
                                        }
                                        else if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款) // 应收款单
                                        {
                                            // 收款对账单中的应收款单，核销金额处理逻辑
                                            // 1. 如果实付金额与总净额完全匹配(isExactMatch)，则全额核销应收款单
                                            // 2. 否则，按FIFO原则进行部分核销，同时支持正负金额混合场景
                                            if (isExactMatch)
                                            {
                                                // 场景：实付金额与总净额完全匹配（例如：应收6120元，应付3018元，实收3102元）
                                                // 全额核销应收款单，无论金额正负
                                                amountToWriteOff = receivablePayable.LocalBalanceAmount;
                                            }
                                            else
                                            {
                                                // 场景：实付金额小于总净额，需要按FIFO顺序部分核销
                                                // 针对正负金额混合的情况（如销售退货等），使用绝对值进行核销计算
                                                decimal netPaymentAmount = ARAPTotalPaidAmount;
                                                decimal writeOffBase = Math.Min(Math.Abs(receivablePayable.LocalBalanceAmount), Math.Abs(netPaymentAmount));

                                                // 保持与应收款单相同的符号方向
                                                // 确保核销金额的符号与原单据保持一致（正数表示应收，负数表示退货）
                                                amountToWriteOff = receivablePayable.LocalBalanceAmount > 0 ? writeOffBase : -writeOffBase;
                                            }
                                        }
                                    }
                                }
                                else // 非混合场景，使用优化后的核销逻辑
                                {
                                    if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款) // 应收款单
                                    {
                                        // 改进应收款单核销逻辑，考虑收款对账单中金额的正负方向和全额核销场景
                                        // 1. 如果实付金额与总净额完全匹配(isExactMatch)，则全额核销应收款单
                                        // 2. 否则，按传统逻辑进行部分核销
                                        if (isExactMatch)
                                        {
                                            // 实付金额与总净额完全匹配，全额核销应收款单
                                            // 业务场景：实际支付金额等于应收款单总净额，一次结清所有欠款
                                            amountToWriteOff = receivablePayable.LocalBalanceAmount;
                                        }
                                        else
                                        {
                                            // 获取对账单总支付净额
                                            decimal netPaymentAmount = ARAPTotalPaidAmount;

                                            if (statement.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                            {
                                                // 收款对账单场景 - 特别处理多行明细正负金额混合的情况
                                                // 例如：销售应收600元 + 销售退货-200元 = 实收400元
                                                // 无论应收款单是正数还是负数，都使用绝对值进行核销计算
                                                // 然后根据应收款单的实际方向调整最终的核销金额
                                                // 核心计算原则：确保金额比较的正确性，同时保留原始单据的符号方向
                                                decimal writeOffBase = Math.Min(Math.Abs(receivablePayable.LocalBalanceAmount), Math.Abs(netPaymentAmount));

                                                // 保持与应收款单相同的符号方向
                                                // 确保核销金额的符号与原单据保持一致（正数表示应收，负数表示退货）
                                                // 财务意义：保证账务处理的准确性，正确反映业务性质
                                                amountToWriteOff = receivablePayable.LocalBalanceAmount > 0 ? writeOffBase : -writeOffBase;
                                            }
                                            else
                                            {
                                                amountToWriteOff = Math.Min(receivablePayable.LocalBalanceAmount, ARAPTotalPaidAmount);
                                            }
                                        }
                                    }
                                    else if (receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款) // 应付款单
                                    {
                                        // 改进应付款单核销逻辑，考虑付款对账单中金额的正负方向和全额核销场景
                                        // 1. 如果实付金额与总净额完全匹配(isExactMatch)，则全额核销应付款单
                                        // 2. 否则，按传统逻辑进行部分核销
                                        if (isExactMatch)
                                        {
                                            // 实付金额与总净额完全匹配，全额核销应付款单
                                            // 业务场景：实际支付金额等于应付款单总净额，一次结清所有应付款
                                            amountToWriteOff = receivablePayable.LocalBalanceAmount;
                                        }
                                        else
                                        {
                                            // 对于付款对账单：
                                            // 1. 如果应付款单金额为正数（采购入库），正常核销
                                            // 2. 如果应付款单金额为负数（采购退货），需要特殊处理

                                            // 获取对账单总支付净额（使用ARAPTotalPaidAmount，它已经是考虑了正负的净额）
                                            decimal netPaymentAmount = ARAPTotalPaidAmount;

                                            // 根据付款单类型和应付款单方向调整核销逻辑
                                            if (statement.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                            {
                                                // 付款对账单场景 - 特别处理多行明细正负金额混合的情况
                                                // 例如：采购应付16200元 + 采购调价-400元 = 实付15800元
                                                // 无论应付款单是正数还是负数，都使用绝对值进行核销计算
                                                // 然后根据应付款单的实际方向调整最终的核销金额
                                                // 核心计算原则：确保金额比较的正确性，同时保留原始单据的符号方向
                                                decimal writeOffBase = Math.Min(Math.Abs(receivablePayable.LocalBalanceAmount), Math.Abs(netPaymentAmount));

                                                // 保持与应付款单相同的符号方向
                                                // 确保核销金额的符号与原单据保持一致（正数表示应付，负数表示退货）
                                                // 财务意义：保证账务处理的准确性，正确反映业务性质
                                                amountToWriteOff = receivablePayable.LocalBalanceAmount > 0 ? writeOffBase : -writeOffBase;
                                            }
                                            else
                                            {
                                                // 其他情况使用传统逻辑
                                                amountToWriteOff = Math.Min(receivablePayable.LocalBalanceAmount, ARAPTotalPaidAmount);
                                            }
                                        }
                                    }
                                }

                                // 执行核销操作
                                if (Math.Abs(amountToWriteOff) > 0)
                                {
                                    // 使用绝对值进行金额更新，保持符号一致性
                                    decimal absAmountToWriteOff = Math.Abs(amountToWriteOff);

                                    // 根据应收/应付款单的方向调整余额和已付金额
                                    if (receivablePayable.LocalBalanceAmount > 0)
                                    {
                                        // 正数单据（正常业务）
                                        receivablePayable.LocalPaidAmount += absAmountToWriteOff;
                                        receivablePayable.LocalBalanceAmount -= absAmountToWriteOff;
                                    }
                                    else if (receivablePayable.LocalBalanceAmount < 0)
                                    {
                                        // 负数单据（退货业务）
                                        receivablePayable.LocalPaidAmount -= absAmountToWriteOff; // 减少已付金额
                                        receivablePayable.LocalBalanceAmount += absAmountToWriteOff; // 增加余额（向零靠近）
                                    }

                                    // 确保精度，避免浮点误差导致的问题
                                    receivablePayable.LocalPaidAmount = Math.Round(receivablePayable.LocalPaidAmount, 2);
                                    receivablePayable.LocalBalanceAmount = Math.Round(receivablePayable.LocalBalanceAmount, 2);


                                    // 在混合对冲场景中，只有非对冲项才需要减少ARAPTotalPaidAmount
                                    // 说明：
                                    // 1. 混合模式：应收3102元 + 应付6120元 + 实付3018元的场景
                                    //    - 应收款单作为对冲项，全额核销3102元，不扣减ARAPTotalPaidAmount
                                    //    - 应付款单使用实付金额3018元核销，扣减ARAPTotalPaidAmount
                                    // 2. 单一模式：销售应收600元 + 销售退货-200元 = 实收400元
                                    //    - 按FIFO顺序核销，先核销600元的应收款，再核销-200元的退货
                                    //    - 每次核销都扣减ARAPTotalPaidAmount
                                    // 3. 单一模式：采购应付16200元 + 采购调价-400元 = 实付15800元
                                    //    - 按FIFO顺序核销，每次核销都扣减ARAPTotalPaidAmount

                                    // 合并全额核销和非全额核销的扣减逻辑，简化代码结构
                                    // 核心原则：在混合对冲模式下，只有非对冲项才扣减ARAPTotalPaidAmount
                                    // 无论是否为全额核销场景，此原则都适用
                                    if (!hasMixedTypes ||
                                        (statement.ReceivePaymentType == (int)ReceivePaymentType.付款 &&
                                         receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.付款) ||
                                        (statement.ReceivePaymentType == (int)ReceivePaymentType.收款 &&
                                         receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款))
                                    {
                                        // 使用绝对值扣减ARAPTotalPaidAmount，确保正确处理正负金额
                                        // 当实付金额小于应收应付总额时，通过扣减绝对值保证FIFO规则正确应用
                                        ARAPTotalPaidAmount -= absAmountToWriteOff;
                                    }
                                }

                                // 更新应收应付单状态
                                // 使用绝对值判断，确保浮点计算精度问题不影响状态判断
                                if (Math.Abs(receivablePayable.LocalBalanceAmount) < 0.01m)
                                {
                                    receivablePayable.LocalBalanceAmount = 0; // 显式设置为0，避免微小余额
                                    receivablePayable.ARAPStatus = (int)ARAPStatus.全部支付;
                                    receivablePayable.AllowAddToStatement = false;
                                }
                                else if (receivablePayable.LocalBalanceAmount > 0 && receivablePayable.LocalPaidAmount > 0)
                                {
                                    receivablePayable.ARAPStatus = (int)ARAPStatus.部分支付;
                                    receivablePayable.AllowAddToStatement = true;
                                }

                         
                                // 生成核销记录
                                await settlementController.GenerateSettlement(entity, RecordDetail, receivablePayable, amountToWriteOff);

                                await UpdateSourceDocumentStatus(receivablePayable, entity, saleOrderUpdateList, saleOutUpdateList, SaleOutReUpdateList, purOrderUpdateList, purEntryUpdateList, purEntryReUpdateList,
                                      priceAdjustmentUpdateList, otherExpenseUpdateList, expenseClaimUpdateList, RepairOrderUpdateList, FinishedGoodsInvUpdateList);


                                StatementDetailUpdateList.Add(StatementDetail);

                                // 如果已全额核销所有金额，跳出循环
                                // 使用绝对值判断，确保净额为0时能正确识别
                                // 适用于所有场景：
                                // 1. 混合模式：应收3102元 + 应付6120元 + 实付3018元
                                //    - 对冲3102元 + 支付3018元 = 应付6120元，此时ARAPTotalPaidAmount为0
                                // 2. 单一模式：销售应收600元 + 销售退货-200元 = 实收400元
                                //    - 全额核销后，ARAPTotalPaidAmount为0
                                // 3. 单一模式：采购应付16200元 + 采购调价-400元 = 实付15800元
                                //    - 全额核销后，ARAPTotalPaidAmount为0
                                // 4. 实付金额不足的情况：按FIFO顺序核销完可用金额后退出
                                if (Math.Abs(StatementPaidAmount) < 0.01m || Math.Abs(ARAPTotalPaidAmount) < 0.01m)
                                    break;
                            }

                            receivablePayableUpdateList.AddRange(receivablePayableList);

                            #endregion

                            // 修正TotalPaidLocalAmount和TotalReceivedLocalAmount计算
                            // 在混合对冲场景下，只计算实际支付部分，不包括对冲项
                            if (statement.ReceivePaymentType == (int)ReceivePaymentType.付款)
                            {
                                if (hasMixedTypes)
                                {
                                    // 混合对冲场景：只计算应付款单的核销金额（实际支付部分）
                                    // 先按类型过滤，然后计算代数和，最后取绝对值
                                    // 这样确保同一类型内的正负金额混合情况也能正确计算
                                    statement.TotalPaidLocalAmount = Math.Abs(statement.tb_FM_StatementDetails
                                        .Where(d => d.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                        .Sum(c => c.WrittenOffLocalAmount));
                                }
                                else
                                {
                                    // 单一模式：计算所有明细的核销金额的代数和的绝对值
                                    // 这确保了退款场景下负数金额会正确减少总支付金额
                                    statement.TotalPaidLocalAmount = Math.Abs(statement.tb_FM_StatementDetails.Sum(c => c.WrittenOffLocalAmount));
                                }
                            }
                            if (statement.ReceivePaymentType == (int)ReceivePaymentType.收款)
                            {
                                if (hasMixedTypes)
                                {
                                    // 混合对冲场景：只计算应收款单的核销金额（实际收取部分）
                                    // 先按类型过滤，然后计算代数和，最后取绝对值
                                    // 这样确保同一类型内的正负金额混合情况（如收款退款）也能正确计算
                                    statement.TotalReceivedLocalAmount = Math.Abs(statement.tb_FM_StatementDetails
                                        .Where(d => d.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                        .Sum(c => c.WrittenOffLocalAmount));
                                }
                                else
                                {
                                    // 单一模式：计算所有明细的核销金额的代数和的绝对值
                                    // 这确保了退款场景下负数金额会正确减少总收取金额
                                    statement.TotalReceivedLocalAmount = Math.Abs(statement.tb_FM_StatementDetails.Sum(c => c.WrittenOffLocalAmount));
                                }
                            }
                            // 改进对账单结清逻辑，正确处理混合对冲和单一退货场景
                            // 1. 计算所有明细的已核销金额和总应核销金额
                            decimal totalWrittenOff = statement.tb_FM_StatementDetails.Sum(c => c.WrittenOffLocalAmount);
                            decimal totalPayable = statement.tb_FM_StatementDetails.Sum(c => Math.Abs(c.IncludedLocalAmount));

                            // 2. 计算对账单的净金额（考虑正负金额的代数和）
                            decimal netAmount = statement.tb_FM_StatementDetails.Sum(c => c.IncludedLocalAmount);

                            // 3. 检查是否所有明细的余额都已清零（全额核销）
                            bool allDetailsFullyWrittenOff = statement.tb_FM_StatementDetails.All(d => Math.Abs(d.RemainingLocalAmount) < 0.01m);

                            // 4. 检查是否符合全额核销条件
                            bool isFullySettled = false;

                            // 场景判断：
                            // - 混合对冲场景：应收3102元+应付6120元=实付3018元
                            // - 单一模式退货：采购应付16200元+采购调价-400元=实付15800元
                            // - 单一模式退货：销售应收600元+销售退货-200元=实收400元
                            if (hasMixedTypes || statement.tb_FM_StatementDetails.Any(d => d.IncludedLocalAmount < 0))
                            {
                                // 对于混合对冲或包含退货的场景，检查以下条件：
                                // 1. 所有明细已全额核销
                                // 2. 实际支付金额与对账单净金额相匹配（考虑精度误差）
                                decimal actualPaymentAmount = Math.Abs(entity.tb_FM_PaymentRecordDetails?.Sum(d => d.LocalAmount) ?? 0);
                                isFullySettled = allDetailsFullyWrittenOff && Math.Abs(actualPaymentAmount - Math.Abs(netAmount)) < 0.01m;
                            }
                            else
                            {
                                // 常规场景：判断已核销金额是否等于总应核销金额
                                isFullySettled = Math.Abs(totalWrittenOff - totalPayable) < 0.01m;
                            }

                            // 5. 执行全额核销处理
                            if (isFullySettled)
                            {
                                // 确保所有明细都被标记为全额核销
                                foreach (var detail in statement.tb_FM_StatementDetails)
                                {
                                    detail.RemainingLocalAmount = 0; // 显式设置为0
                                    detail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.全额核销;

                                    // 同时更新对应的应收应付单状态为全部支付
                                    if (detail.tb_fm_receivablepayable != null)
                                    {
                                        detail.tb_fm_receivablepayable.LocalBalanceAmount = 0;
                                        detail.tb_fm_receivablepayable.ARAPStatus = (int)ARAPStatus.全部支付;
                                        detail.tb_fm_receivablepayable.AllowAddToStatement = false;
                                    }
                                }
                                statement.StatementStatus = (int)StatementStatus.全部结清;
                            }
                            else
                            {
                                statement.StatementStatus = (int)StatementStatus.部分结算;
                            }
                            // 修正期末余额计算逻辑，确保正确反映实际结算情况
                            if (statement.ReceivePaymentType == (int)ReceivePaymentType.收款)
                            {
                                // 应收账款逻辑：期末余额 = 期初余额 + 期间应收 - 期间收款
                                // 在混合对冲场景下，期间应付已通过核销逻辑冲抵了应收，不需要重复扣减
                                statement.ClosingBalanceLocalAmount = statement.OpeningBalanceLocalAmount
                                                                     + statement.TotalReceivableLocalAmount
                                                                     - statement.TotalReceivedLocalAmount;
                            }
                            else if (statement.ReceivePaymentType == (int)ReceivePaymentType.付款)
                            {
                                // 应付账款逻辑：期末余额 = 期初余额 + 期间应付 - 期间付款
                                // 在混合对冲场景下，期间应收已通过核销逻辑冲抵了应付，不需要重复扣减
                                statement.ClosingBalanceLocalAmount = statement.OpeningBalanceLocalAmount
                                                                     + statement.TotalPayableLocalAmount
                                                                     - statement.TotalPaidLocalAmount;
                            }

                            // 添加混合对冲场景的特殊处理
                            bool hasMixedTypesForAudit = statement.tb_FM_StatementDetails.Any(sd => sd.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.收款) &&
                                                statement.tb_FM_StatementDetails.Any(sd => sd.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.付款);

                            if (hasMixedTypesForAudit)
                            {
                                // 混合对冲场景：重新计算净核销金额和明细核销状态
                                var receivableDetails = statement.tb_FM_StatementDetails
                                    .Where(sd => sd.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                    .ToList();

                                var payableDetails = statement.tb_FM_StatementDetails
                                    .Where(sd => sd.tb_fm_receivablepayable.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                    .ToList();

                                decimal netReceivableAmount = receivableDetails.Sum(sd => sd.WrittenOffLocalAmount);
                                decimal netPayableAmount = payableDetails.Sum(sd => sd.WrittenOffLocalAmount);

                                // 强制将混合对冲场景下所有明细标记为全额核销
                                // 业务逻辑：当应收和应付相互对冲且实付金额等于净额时，所有相关单据都应视为全额核销
                                if (Math.Abs(StatementPaidAmount) < 0.01m) // 确认支付金额已全部使用
                                {
                                    foreach (var detail in statement.tb_FM_StatementDetails)
                                    {
                                        detail.RemainingLocalAmount = 0;
                                        detail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.全额核销;

                                        // 同时更新对应的应收应付单状态
                                        if (detail.tb_fm_receivablepayable != null)
                                        {
                                            detail.tb_fm_receivablepayable.LocalBalanceAmount = 0;
                                            detail.tb_fm_receivablepayable.ARAPStatus = (int)ARAPStatus.全部支付;
                                            detail.tb_fm_receivablepayable.AllowAddToStatement = false;
                                        }
                                    }
                                    statement.StatementStatus = (int)StatementStatus.全部结清;
                                }

                                // 记录混合对冲的日志信息
                                FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                fMAuditLog.CreateAuditLog<tb_FM_Statement>($"混合对冲场景：应收核销{netReceivableAmount}，应付核销{netPayableAmount}，净核销{(netPayableAmount - netReceivableAmount)}",
                                    statement as tb_FM_Statement);
                            }

                            StatementUpdateList.Add(statement);
                            if (StatementPaidAmount == 0)
                            {
                                break;  // 金额已全部核销完毕
                            }
                            // 如果还有剩余金额未核销，记录日志
                            if (StatementPaidAmount > 0)
                            {
                                FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                fMAuditLog.CreateAuditLog<tb_FM_Statement>($"收款单{entity.PaymentNo}核销后仍有剩余金额{StatementPaidAmount}未核销", statement as tb_FM_Statement);
                            }
                        }


                    }
                    #endregion


                    //收付款明细记录中，如果金额为负数则是 退款红字
                    //收款单审核时。除了保存核销记录，还要来源的 如 应收中的 余额这种更新
                    if (group.Key == (int)BizType.应收款单 || group.Key == (int)BizType.应付款单)
                    {
                        #region 应收单余额更新
                        List<tb_FM_ReceivablePayable> receivablePayableList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                             .Includes(c => c.tb_FM_ReceivablePayableDetails)
                             .Where(c => sourcebillids.Contains(c.ARAPId))
                             .OrderBy(r => r.DueDate).OrderBy(c => c.Created_at)      // 按到期日升序（最早优先）
                             .ToListAsync();
                        foreach (var receivablePayable in receivablePayableList)
                        {
                            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
                            //应收应付明细中可以相同的。负数对冲。最终收款时只会合并。
                            //这里目前假设的是
                            tb_FM_PaymentRecordDetail RecordDetail = entity.tb_FM_PaymentRecordDetails.FirstOrDefault(c => c.SourceBilllId == receivablePayable.ARAPId);
                            receivablePayable.ForeignPaidAmount += RecordDetail.ForeignAmount;
                            receivablePayable.LocalPaidAmount += RecordDetail.LocalAmount;
                            receivablePayable.ForeignBalanceAmount -= RecordDetail.ForeignAmount;
                            receivablePayable.LocalBalanceAmount -= RecordDetail.LocalAmount;
                            //应收应付 正反都 生成核销记录
                            await settlementController.GenerateSettlement(entity, RecordDetail, receivablePayable, RecordDetail.ForeignAmount);

                            //应收付的退款操作，对应收付款审核时。要找他对应的正向预收付单。修改状态。和退回金额。
                            if (RecordDetail.LocalAmount < 0 || RecordDetail.ForeignAmount < 0)
                            {
                                //应收款付款时，销售，采购的退货单的会是负数
                                #region 通过他的来源单据，找到对应的预收付单的收款单。标记为已关闭 !!!!!!!!!! 收款单 有是否反冲标记， 预收付中有退回金额
                                //要调式
                                //找到最原始的收款单 正数
                                tb_FM_PaymentRecord oldPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == RecordDetail.SourceBilllId)
                                && c.PaymentStatus == (int)PaymentStatus.已支付)
                                  .Where(c => c.tb_FM_PaymentRecordDetails.Any(b => b.LocalAmount > 0))
                                  .OrderByDescending(c => c.Created_at)  // 优先选择最近的正向付款
                                 .FirstAsync();
                                if (oldPayment != null)
                                {
                                    // 更新原始记录 指向[负数]冲销记录
                                    oldPayment.ReversedByPaymentId = entity.PaymentId;
                                    oldPayment.ReversedByPaymentNo = entity.PaymentNo;
                                    oldPaymentUpdateList.Add(oldPayment);
                                    // 指向原始记录
                                    entity.ReversedOriginalId = oldPayment.PaymentId;
                                    entity.IsReversed = true;
                                }


                                #endregion

                            }


                            // 更新应收应付单状态
                            if (receivablePayable.LocalBalanceAmount == 0)
                            {
                                receivablePayable.ARAPStatus = (int)ARAPStatus.全部支付;
                                receivablePayable.AllowAddToStatement = false;
                            }
                            else if (receivablePayable.LocalBalanceAmount > 0 && receivablePayable.LocalPaidAmount > 0)
                            {
                                receivablePayable.ARAPStatus = (int)ARAPStatus.部分支付;
                                receivablePayable.AllowAddToStatement = true;
                            }




                            //写回业务 原始单据的完结状态，销售出库。销售订单。
                            //通过的来源类型，来源单号，来源编号分组得到原始单据数据组后再根据类型分别处理更新状态
                            await UpdateSourceDocumentStatus(receivablePayable, entity, saleOrderUpdateList, saleOutUpdateList, SaleOutReUpdateList, purOrderUpdateList, purEntryUpdateList, purEntryReUpdateList,
                                     priceAdjustmentUpdateList, otherExpenseUpdateList, expenseClaimUpdateList, RepairOrderUpdateList, FinishedGoodsInvUpdateList);


                        }

                        receivablePayableUpdateList.AddRange(receivablePayableList);

                        #endregion
                    }

                    //单纯收款不用产生核销记录。核销要与业务相关联 这里只处理 应收，预收，对账单
                    //退款时写回上级预收付款单 状态为 已冲销.预先处理，不用核销.只是一个收款记录
                    //负数时
                    //这里收款审核，面对预先处理。只是一个记录，并且回写预收生效，待核销。不用生成核销记录。
                    if (group.Key == (int)BizType.预收款单 || group.Key == (int)BizType.预付款单)
                    {
                        //找到 付款单对应的预付款单
                        List<tb_FM_PreReceivedPayment> PreReceivablePayableList = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                           .Where(c => sourcebillids.Contains(c.PreRPID))
                           .ToListAsync();
                        foreach (var prePayment in PreReceivablePayableList)
                        {
                            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
                            tb_FM_PaymentRecordDetail RecordDetail = entity.tb_FM_PaymentRecordDetails.FirstOrDefault(c => c.SourceBilllId == prePayment.PreRPID);
                            if (prePayment != null)
                            {
                                //正数是地，是收预付款的款，所以是等待核销
                                prePayment.PrePaymentStatus = (int)PrePaymentStatus.待核销;
                                prePayment.PrePayDate = DateTime.Now;
                                prePayment.ForeignBalanceAmount += RecordDetail.ForeignAmount;
                                prePayment.LocalBalanceAmount += RecordDetail.LocalAmount;

                                //预收付的退款操作，对应收付款审核时。要找他对应的正向预收付单。修改状态。和退回金额。
                                if (RecordDetail.LocalAmount < 0 || RecordDetail.ForeignAmount < 0)
                                {
                                    prePayment.LocalRefundAmount += Math.Abs(RecordDetail.LocalAmount);
                                    prePayment.ForeignRefundAmount += Math.Abs(RecordDetail.ForeignAmount);
                                    prePayment.Remark += $"{System.DateTime.Now.ToString()}退款{Math.Abs(RecordDetail.LocalAmount)}";
                                    //预收的退款操作时。 应该是去找他相同的
                                    #region 通过他的来源单据，找到对应的预收付单
                                    //有退，有核销 则是  部分核销
                                    if ((prePayment.LocalPaidAmount > 0 && prePayment.LocalRefundAmount > 0) ||
                                        (prePayment.ForeignPaidAmount > 0 && prePayment.ForeignRefundAmount > 0))
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                                    }

                                    //有退部分,还没有核销，后面可能退，也可能核销掉 则是  部分核销
                                    if ((prePayment.LocalPaidAmount == 0 && prePayment.LocalRefundAmount > 0 && prePayment.LocalRefundAmount < prePayment.LocalPrepaidAmount) ||
                                        (prePayment.ForeignPaidAmount == 0 && prePayment.ForeignRefundAmount > 0 && prePayment.ForeignRefundAmount < prePayment.ForeignPrepaidAmount))
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                                    }
                                    //全退了则是 已冲销
                                    if (prePayment.LocalRefundAmount == prePayment.LocalPrepaidAmount || prePayment.ForeignRefundAmount == prePayment.ForeignPrepaidAmount)
                                    {
                                        //全退款
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.全额退款;
                                        prePayment.IsAvailable = false;
                                    }

                                    #endregion

                                    #region 通过他的来源单据，找到对应的预收付单的收款单。标记为已关闭 !!!!!!!!!! 收款单 有是否反冲标记， 预收付中有退回金额

                                    //负数时，他一定有一个正数的收款单。并且对应一个对应的预收付单。，预收则要转为已冲销。自己则为
                                    tb_FM_PaymentRecord oldPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                    .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == RecordDetail.SourceBilllId)
                                    && c.PaymentStatus == (int)PaymentStatus.已支付 && c.IsReversed == false
                                    && c.TotalLocalAmount > 0)
                                    .OrderByDescending(c => c.Created_at)  // 优先选择最近的正向付款
                                     .FirstAsync();
                                    if (oldPayment != null)
                                    {
                                        // 更新原始记录 指向[负数]冲销记录
                                        oldPayment.ReversedByPaymentId = entity.PaymentId;
                                        oldPaymentUpdateList.Add(oldPayment);
                                        // 指向原始记录
                                        entity.ReversedOriginalId = oldPayment.PaymentId;
                                        entity.IsReversed = true;
                                    }


                                    #endregion

                                    #region 对应的订单变为取消 不调用业务的取消是事务不能嵌套

                                    if (prePayment.SourceBizType == (int)BizType.销售订单 || prePayment.SourceBizType == (int)BizType.采购订单)
                                    {
                                        var ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                                        if (prePayment.SourceBizType == (int)BizType.销售订单)
                                        {
                                            var saleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                                                .Includes(c => c.tb_SaleOrderDetails)
                                            .Where(c => c.SOrder_ID == prePayment.SourceBillId)
                                            .Where(c => c.DataStatus == (int)DataStatus.确认)
                                            .SingleAsync();
                                            if (saleOrder != null)
                                            {
                                                //订金全退时 作废
                                                if (saleOrder.Deposit == Math.Abs(entity.TotalLocalAmount) || prePayment.LocalBalanceAmount == 0)
                                                {
                                                    if (saleOrder.Deposit == Math.Abs(entity.TotalLocalAmount))
                                                    {
                                                        saleOrder.CloseCaseOpinions += $"订金全退，订单取消作废";
                                                        saleOrder.DataStatus = (int)DataStatus.作废;
                                                    }
                                                    else
                                                    {
                                                        saleOrder.CloseCaseOpinions += $"部分出库，订金部分退款，订单结案";
                                                        saleOrder.DataStatus = (int)DataStatus.完结;
                                                    }

                                                    saleOrderUpdateList.Add(saleOrder);
                                                    #region 更新库存的拟销量

                                                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                                                    foreach (var child in saleOrder.tb_SaleOrderDetails)
                                                    {
                                                        #region 库存表的更新 ，
                                                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                                        int notoutqty = child.Quantity - child.TotalDeliveredQty;
                                                        if (notoutqty == 0)
                                                        {
                                                            continue;
                                                        }
                                                        //更新在途库存
                                                        inv.Sale_Qty = inv.Sale_Qty - notoutqty;
                                                        BusinessHelper.Instance.EditEntity(inv);
                                                        #endregion
                                                        invUpdateList.Add(inv);
                                                    }

                                                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                                                    var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                                                    #endregion
                                                }
                                            }

                                        }
                                        else if (prePayment.SourceBizType == (int)BizType.采购订单)
                                        {
                                            var purOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                                .Includes(c => c.tb_PurOrderDetails)
                                            .Where(c => c.PurOrder_ID == prePayment.SourceBillId)
                                             .Where(c => c.DataStatus == (int)DataStatus.确认)
                                            .SingleAsync();
                                            if (purOrder != null)
                                            { //订金全退时 作废

                                                if (purOrder.Deposit == Math.Abs(entity.TotalLocalAmount) || prePayment.LocalBalanceAmount == 0)
                                                {
                                                    if (purOrder.Deposit == Math.Abs(entity.TotalLocalAmount))
                                                    {
                                                        purOrder.CloseCaseOpinions += $"订金全退，订单取消作废";
                                                        purOrder.DataStatus = (int)DataStatus.作废;
                                                    }
                                                    else
                                                    {
                                                        purOrder.CloseCaseOpinions += $"部分入库，订金部分退款，订单结案";
                                                        purOrder.DataStatus = (int)DataStatus.完结;
                                                    }
                                                    purOrderUpdateList.Add(purOrder);
                                                    #region 更新库存的拟销量

                                                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                                                    foreach (var child in purOrder.tb_PurOrderDetails)
                                                    {
                                                        #region 库存表的更新 ，
                                                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                                        int notEntryQty = child.Quantity - child.DeliveredQuantity;
                                                        if (notEntryQty == 0)
                                                        {
                                                            continue;
                                                        }
                                                        inv.On_the_way_Qty = inv.On_the_way_Qty - notEntryQty;
                                                        BusinessHelper.Instance.EditEntity(inv);
                                                        #endregion
                                                        invUpdateList.Add(inv);
                                                    }

                                                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                                                    var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);

                                                    #endregion
                                                }
                                            }

                                        }
                                    }

                                    #endregion

                                }


                                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                {
                                    if (prePayment.SourceBizType == (int)BizType.销售订单)
                                    {
                                        //如果是多次预付，则合并订单中订金字段的预付金额
                                        //多次预付款时，则要找到这个订单名下的所有订金预付款的单。如果只有一行。则不用累计。如果是多行。要需要？

                                        #region 更新对应业务的单据状态和订金金额情况

                                        tb_SaleOrder saleOrder = await _appContext.Db.Queryable<tb_SaleOrder>()
                                             .Where(c => c.SOrder_ID == prePayment.SourceBillId).SingleAsync();
                                        if (saleOrder != null)
                                        {
                                            #region 找之前预付的
                                            //这里重新查询一次，因为上面查询的只是当前的部分数据
                                            List<tb_FM_PreReceivedPayment> SameOrderPrePayments = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                                                                .Where(c => c.SourceBillId == prePayment.SourceBillId)
                                                                .Where(c => c.SourceBizType == (int)BizType.销售订单)
                                                                .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.待核销)
                                                                .Where(c => c.PreRPID != prePayment.PreRPID)//排除当前的
                                                                .ToListAsync();
                                            if (SameOrderPrePayments != null)//为0说明是第一次。从未付款 到部分预付是正常的流程逻辑
                                            {
                                                //原来的。加上当前的
                                                saleOrder.Deposit = SameOrderPrePayments.Sum(c => c.LocalPrepaidAmount) + prePayment.LocalPrepaidAmount;
                                                //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                            }
                                            else
                                            {
                                                saleOrder.Deposit = prePayment.LocalPrepaidAmount;
                                            }
                                            #endregion

                                            if (saleOrder.Deposit == saleOrder.TotalAmount)
                                            {
                                                saleOrder.PayStatus = (int)PayStatus.全额预付;
                                                if (entity.Paytype_ID.HasValue)
                                                {
                                                    saleOrder.Paytype_ID = entity.Paytype_ID.Value;
                                                }
                                            }
                                            else
                                            {
                                                saleOrder.PayStatus = (int)PayStatus.部分预付;
                                                if (entity.Paytype_ID.HasValue)
                                                {
                                                    saleOrder.Paytype_ID = entity.Paytype_ID.Value;
                                                }
                                            }
                                            saleOrderUpdateList.Add(saleOrder);
                                        }

                                        #endregion

                                        #region 如果销售订单 有出库则核销出库时生成的应收款单
                                        //如果是[预收款]的确认到账，则会自动去核销[应收款]一一对应的单据[订单对应出库单]。
                                        //减少工作量，反过来。如果预收款等待核销。应收审核时也会自动
                                        if (_appContext.FMConfig.EnablePaymentAutoOffsetAR)
                                        {
                                            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                                            {
                                                if (prePayment.SourceBizType == (int)BizType.销售订单)
                                                {
                                                    var SaleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                                                        .Includes(c => c.tb_SaleOuts, b => b.tb_saleorder)
                                                    .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID && c.SOrder_ID == prePayment.SourceBillId)
                                                    .SingleAsync();

                                                    if (SaleOrder != null)
                                                    {
                                                        long[] SaleOutIds = SaleOrder.tb_SaleOuts.Select(c => c.SaleOut_MainID).ToArray();

                                                        //部分支付暂时不自动处理
                                                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                                        var receivablePayables = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                        .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                                                        .Where(c => c.ARAPStatus >= (int)ARAPStatus.待支付
                                                                        && c.CustomerVendor_ID == entity.CustomerVendor_ID
                                                                        && SaleOutIds.Contains(c.SourceBillId.Value))
                                                                        .ToListAsync();


                                                        //一切刚刚好时才能去核销
                                                        foreach (var receivablePayable in receivablePayables)
                                                        {
                                                            if (receivablePayable.TotalLocalPayableAmount == entity.TotalLocalAmount &&
                                                                receivablePayable.TotalLocalPayableAmount == prePayment.LocalBalanceAmount)
                                                            {
                                                                List<tb_FM_PreReceivedPayment> ProcessPreReceivablePayableList = new List<tb_FM_PreReceivedPayment>();
                                                                ProcessPreReceivablePayableList.Add(prePayment);
                                                                await ctrpayable.ApplyManualPaymentAllocation(receivablePayable, ProcessPreReceivablePayableList);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }

                                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                {
                                    if (prePayment.SourceBizType == (int)BizType.采购订单)
                                    {
                                        //如果是多次预付，则合并订单中订金字段的预付金额
                                        //多次预付款时，则要找到这个订单名下的所有订金预付款的单。如果只有一行。则不用累计。如果是多行。要需要？

                                        #region 更新对应业务的单据状态和订金金额情况


                                        tb_PurOrder purOrder = await _appContext.Db.Queryable<tb_PurOrder>()
                                             .Where(c => c.PurOrder_ID == prePayment.SourceBillId).SingleAsync();
                                        if (purOrder != null)
                                        {

                                            #region 找到前面已经预付过的
                                            List<tb_FM_PreReceivedPayment> SameOrderPrePayments = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                                                 .Where(c => c.SourceBillId == prePayment.SourceBillId)
                                                 .Where(c => c.SourceBizType == (int)BizType.采购订单)
                                                 .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.待核销)
                                                 .Where(c => c.PreRPID != prePayment.PreRPID)//排除当前的
                                                 .ToListAsync();
                                            if (SameOrderPrePayments != null)//为0说明是第一次。从未付款 到部分预付是正常的流程逻辑
                                            {
                                                //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                                                //原来的。加上当前的
                                                purOrder.Deposit = SameOrderPrePayments.Sum(c => c.LocalPrepaidAmount) + prePayment.LocalPrepaidAmount;
                                            }
                                            else
                                            {
                                                //加上当前的
                                                purOrder.Deposit = prePayment.LocalPrepaidAmount;
                                            }
                                            #endregion

                                            if (purOrder.Deposit == purOrder.TotalAmount)
                                            {
                                                purOrder.PayStatus = (int)PayStatus.全额预付;
                                                if (entity.Paytype_ID.HasValue)
                                                {
                                                    purOrder.Paytype_ID = entity.Paytype_ID.Value;
                                                }
                                            }
                                            else
                                            {
                                                purOrder.PayStatus = (int)PayStatus.部分预付;
                                                if (entity.Paytype_ID.HasValue)
                                                {
                                                    purOrder.Paytype_ID = entity.Paytype_ID.Value;
                                                }
                                            }
                                            purOrderUpdateList.Add(purOrder);
                                        }

                                        #endregion


                                        #region  如果采购订单有入库，则自动核销入库生成的应付款单

                                        //预付的确认时，可以核销应付   
                                        if (_appContext.FMConfig.EnablePaymentAutoOffsetAP)
                                        {
                                            if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                                            {
                                                if (prePayment.SourceBizType == (int)BizType.采购订单)
                                                {
                                                    var PurOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                                        .Includes(c => c.tb_PurEntries, b => b.tb_purorder)
                                                    .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID && c.PurOrder_ID == prePayment.SourceBillId)
                                                    .SingleAsync();

                                                    if (PurOrder != null)
                                                    {
                                                        long[] PurEntryIDs = PurOrder.tb_PurEntries.Select(c => c.PurEntryID).ToArray();

                                                        //部分支付暂时不自动处理
                                                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                                                        var receivablePayables = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                                                        .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                                                        .Where(c => c.ARAPStatus >= (int)ARAPStatus.待支付
                                                                        && c.CustomerVendor_ID == entity.CustomerVendor_ID
                                                                        && PurEntryIDs.Contains(c.SourceBillId.Value))
                                                                        .ToListAsync();



                                                        //一切刚刚好时才能去核销
                                                        foreach (var receivablePayable in receivablePayables)
                                                        {
                                                            if (receivablePayable.TotalLocalPayableAmount == entity.TotalLocalAmount &&
                                                                receivablePayable.TotalLocalPayableAmount == prePayment.LocalBalanceAmount)
                                                            {
                                                                List<tb_FM_PreReceivedPayment> ProcessPreReceivablePayableList = new List<tb_FM_PreReceivedPayment>();
                                                                ProcessPreReceivablePayableList.Add(prePayment);
                                                                await ctrpayable.ApplyManualPaymentAllocation(receivablePayable, ProcessPreReceivablePayableList);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                }



                            }

                        }

                        //收款单 确认时，会修改预收付款的状态。余额，退款金额等
                        preReceivedPaymentUpdateList.AddRange(PreReceivablePayableList);
                    }


                    #region 费用报销单回写状态
                    //要把费用报销单的状态更新了，
                    List<tb_FM_ExpenseClaim> ExpenseClaimList = await _appContext.Db.Queryable<tb_FM_ExpenseClaim>()
                        .Includes(c => c.tb_FM_ExpenseClaimDetails)
                       .Where(c => sourcebillids.Contains(c.ClaimMainID))
                       .ToListAsync();
                    foreach (var ExpenseClaim in ExpenseClaimList)
                    {
                        //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
                        tb_FM_PaymentRecordDetail RecordDetail = entity.tb_FM_PaymentRecordDetails.FirstOrDefault(c => c.SourceBilllId == ExpenseClaim.ClaimMainID);
                        if (ExpenseClaim != null)
                        {
                            if (RecordDetail.LocalAmount == ExpenseClaim.ClaimAmount)
                            {
                                ExpenseClaim.PayStatus = (int)PayStatus.全部付款;
                                ExpenseClaim.Paytype_ID = entity.Paytype_ID;
                                ExpenseClaim.DataStatus = (int)DataStatus.完结;
                            }
                            else
                            {
                                rmrs.ErrorMsg = "付款金额与报销金额不一致，审核失败";
                                rmrs.Succeeded = false;
                                rmrs.ReturnObject = entity as T;
                                return rmrs;
                            }
                            ExpenseClaimUpdateList.Add(ExpenseClaim);
                        }
                    }
                    #endregion
                }

                #region 更新数据库
                if (ExpenseClaimUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(ExpenseClaimUpdateList).UpdateColumns(t => new
                    {
                        t.PayStatus,
                        t.Paytype_ID,
                        t.DataStatus,
                    }).ExecuteCommandAsync();
                }

                if (FinishedGoodsInvUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(FinishedGoodsInvUpdateList).UpdateColumns(t => new
                    {
                        t.PayStatus,
                    }).ExecuteCommandAsync();
                }

                if (StatementUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(StatementUpdateList).UpdateColumns(t => new
                    {
                        t.StatementStatus,
                        t.ClosingBalanceForeignAmount,
                        t.ClosingBalanceLocalAmount,
                        t.TotalPaidLocalAmount,
                        t.TotalReceivedLocalAmount,
                    }).ExecuteCommandAsync();
                }

                if (StatementDetailUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(StatementDetailUpdateList).UpdateColumns(t => new
                    {
                        t.ARAPWriteOffStatus,
                        t.WrittenOffLocalAmount,
                        t.WrittenOffForeignAmount,
                        t.RemainingForeignAmount,
                        t.RemainingLocalAmount,
                    }).ExecuteCommandAsync();
                }


                if (oldPaymentUpdateList.Any())
                {
                    //更新原来的上一个预付记录
                    await _unitOfWorkManage.GetDbClient().Updateable(oldPaymentUpdateList).UpdateColumns(t => new
                    {
                        t.ReversedOriginalId,
                        t.IsReversed
                    }
                    ).ExecuteCommandAsync();
                }

                if (otherExpenseUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(otherExpenseUpdateList).UpdateColumns(t => new
                    {
                        t.DataStatus,
                        t.Paytype_ID,
                        t.PayStatus,
                    }).ExecuteCommandAsync();
                }

                if (priceAdjustmentUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(priceAdjustmentUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (purEntryReUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(purEntryReUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (SaleOutReUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutRe>(SaleOutReUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus,
                        t.RefundStatus,
                    }).ExecuteCommandAsync();
                }
                if (saleOutUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(saleOutUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus,
                        t.RefundStatus,
                    }).ExecuteCommandAsync();
                }

                if (RepairOrderUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable(RepairOrderUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (saleOrderUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(saleOrderUpdateList).UpdateColumns(t => new
                    {
                        t.Deposit,
                        t.ApprovalOpinions,
                        t.DataStatus,
                        t.PayStatus,
                        t.CloseCaseOpinions,
                        t.Paytype_ID,
                    }).ExecuteCommandAsync();

                }

                if (purEntryUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(purEntryUpdateList).UpdateColumns(t => new
                    {
                        t.Paytype_ID,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (purOrderUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(purOrderUpdateList).UpdateColumns(t => new
                    {
                        t.ApprovalOpinions,
                        t.DataStatus,
                        t.PayStatus
                    }).ExecuteCommandAsync();
                }

                if (receivablePayableUpdateList.Any())
                {
                    var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(receivablePayableUpdateList).UpdateColumns(it =>
                                new
                                {
                                    it.AllowAddToStatement,
                                    it.ARAPStatus,
                                    it.ForeignPaidAmount,
                                    it.LocalPaidAmount,
                                    it.LocalBalanceAmount,
                                    it.ForeignBalanceAmount,
                                }).ExecuteCommandAsync();
                }

                if (preReceivedPaymentUpdateList.Any())
                {
                    //更新
                    var preRs = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(preReceivedPaymentUpdateList).UpdateColumns(it =>
                                new
                                {
                                    it.PrePayDate,
                                    it.PrePaymentStatus,
                                    it.ForeignBalanceAmount,
                                    it.LocalBalanceAmount,
                                }).ExecuteCommandAsync();
                }

                //更新账户余额
                if (entity.tb_fm_account == null && entity.Account_id.HasValue)
                {
                    entity.tb_fm_account = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(c => c.Account_id == entity.Account_id).FirstAsync();
                    //和账户相同的币种才更新
                    if (entity.tb_fm_account.Currency_ID == entity.Currency_ID)
                    {
                        if (entity.tb_fm_account.Currency_ID == _appContext.BaseCurrency.Currency_ID)
                        {
                            entity.tb_fm_account.CurrentBalance += entity.TotalLocalAmount;
                        }
                        else
                        {
                            entity.tb_fm_account.CurrentBalance += entity.TotalForeignAmount;
                        }
                    }

                    await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_Account>(entity.tb_fm_account).UpdateColumns(it => new { it.CurrentBalance }).ExecuteCommandAsync();
                }

                #endregion

                #endregion

                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                entity.ApprovalResults = true;
                //等待真正支付
                entity.PaymentStatus = (int)PaymentStatus.已支付;

                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.PaymentStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                    it.IsReversed,
                    it.ReversedByPaymentId,
                    it.Paytype_ID
                }).ExecuteCommandAsync();


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }
        }



        // 更新源单据状态的方法
        private async Task UpdateSourceDocumentStatus(
            tb_FM_ReceivablePayable receivablePayable,
            tb_FM_PaymentRecord entity,
            List<tb_SaleOrder> saleOrderUpdateList,
            List<tb_SaleOut> saleOutUpdateList,
            List<tb_SaleOutRe> SaleOutReUpdateList,
            List<tb_PurOrder> purOrderUpdateList,
            List<tb_PurEntry> purEntryUpdateList,
            List<tb_PurEntryRe> purEntryReUpdateList,
            List<tb_FM_PriceAdjustment> priceAdjustmentUpdateList,
            List<tb_FM_OtherExpense> otherExpenseUpdateList,
            List<tb_FM_ExpenseClaim> expenseClaimUpdateList,
            List<tb_AS_RepairOrder> RepairOrderUpdateList,
            List<tb_FinishedGoodsInv> FinishedGoodsInvUpdateList
            )
        {
            // 根据业务类型更新不同的源单据状态
            switch (receivablePayable.SourceBizType)
            {
                case (int)BizType.销售出库单:
                    await UpdateSaleOutStatus(receivablePayable, entity, saleOrderUpdateList, saleOutUpdateList);
                    break;

                case (int)BizType.销售退回单:
                    await UpdateSaleOutReStatus(receivablePayable, entity, SaleOutReUpdateList, saleOutUpdateList);
                    break;

                case (int)BizType.采购入库单:
                    await UpdatePurEntryStatus(receivablePayable, entity, purOrderUpdateList, purEntryUpdateList);
                    break;

                case (int)BizType.采购退货单:
                    await UpdatePurEntryReStatus(receivablePayable, entity, purEntryReUpdateList);
                    break;

                case (int)BizType.销售价格调整单:
                case (int)BizType.采购价格调整单:
                    await UpdatePriceAdjustmentStatus(receivablePayable, entity, priceAdjustmentUpdateList);
                    break;

                case (int)BizType.其他费用收入:
                case (int)BizType.其他费用支出:
                    await UpdateOtherExpenseStatus(receivablePayable, entity, otherExpenseUpdateList);
                    break;

                case (int)BizType.费用报销单:
                    await UpdateExpenseClaimStatus(receivablePayable, entity, expenseClaimUpdateList);
                    break;

                case (int)BizType.维修工单:
                    await UpdateRepairOrderStatus(receivablePayable, entity, RepairOrderUpdateList);
                    break;
                case (int)BizType.缴库单:
                    await UpdateFinishedGoodsStatus(receivablePayable, entity, FinishedGoodsInvUpdateList);
                    break;
            }
        }

        private async Task UpdateFinishedGoodsStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_FinishedGoodsInv> FinishedGoodsInvUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.缴库单)
            {
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                {
                    #region 更新对应业务的单据状态和付款情况

                    tb_FinishedGoodsInv finishedGoodsInv = await _appContext.Db.Queryable<tb_FinishedGoodsInv>()
                      .Where(c => c.DataStatus >= (int)DataStatus.确认 && c.FG_ID == receivablePayable.SourceBillId).FirstAsync();
                    if (finishedGoodsInv != null)
                    {
                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == finishedGoodsInv.TotalManuFee)
                        {
                            //财务只管财务的状态
                            // saleOut.DataStatus = (int)DataStatus.完结;
                            finishedGoodsInv.PayStatus = (int)PayStatus.全部付款;
                        }
                        else
                        {
                            finishedGoodsInv.PayStatus = (int)PayStatus.部分付款;
                        }

                        FinishedGoodsInvUpdateList.Add(finishedGoodsInv);
                    }

                    #endregion
                }
            }
        }

        private async Task UpdateRepairOrderStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_AS_RepairOrder> repairOrderUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.维修工单)
            {
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                {
                    #region 更新对应业务的单据状态和付款情况

                    tb_AS_RepairOrder RepairOrder = await _appContext.Db.Queryable<tb_AS_RepairOrder>()
                      .Where(c => c.DataStatus >= (int)DataStatus.确认 && c.RepairOrderID == receivablePayable.SourceBillId).SingleAsync();
                    if (RepairOrder != null)
                    {
                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == RepairOrder.TotalAmount)
                        {
                            //财务只管财务的状态
                            // saleOut.DataStatus = (int)DataStatus.完结;
                            RepairOrder.PayStatus = (int)PayStatus.全部付款;
                            if (entity.Paytype_ID.HasValue)
                            {
                                RepairOrder.Paytype_ID = entity.Paytype_ID.Value;
                            }
                        }
                        else
                        {
                            RepairOrder.PayStatus = (int)PayStatus.部分付款;
                            if (entity.Paytype_ID.HasValue)
                            {
                                RepairOrder.Paytype_ID = entity.Paytype_ID.Value;
                            }
                        }

                        repairOrderUpdateList.Add(RepairOrder);
                    }

                    #endregion
                }
            }
        }

        private async Task UpdateExpenseClaimStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_FM_ExpenseClaim> expenseClaimUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.费用报销单)
            {
                #region 更新对应业务的单据状态和付款情况

                tb_FM_ExpenseClaim ExpenseClaim = await _appContext.Db.Queryable<tb_FM_ExpenseClaim>()
                    .Includes(c => c.tb_FM_ExpenseClaimDetails)
                  .Where(c => c.DataStatus >= (int)DataStatus.确认
                 && c.ClaimMainID == receivablePayable.SourceBillId).SingleAsync();
                if (ExpenseClaim != null)
                {
                    //根据支付状态更新费用报销单的付款状态
                    if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                    {
                        //应收结清，并且结清的金额等于费用报销金额，则修改费用报销单的状态
                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == ExpenseClaim.ClaimAmount)
                        {
                            ExpenseClaim.DataStatus = (int)DataStatus.完结;
                            ExpenseClaim.CloseCaseOpinions = "全部付款";
                            ExpenseClaim.PayStatus = (int)PayStatus.全部付款;
                            ExpenseClaim.Paytype_ID = entity.Paytype_ID;
                        }
                    }
                    else if (receivablePayable.ARAPStatus == (int)ARAPStatus.部分支付)
                    {
                        //部分支付时更新付款状态
                        ExpenseClaim.PayStatus = (int)PayStatus.部分付款;
                        ExpenseClaim.Paytype_ID = entity.Paytype_ID;
                    }
                    else if (receivablePayable.ARAPStatus == (int)ARAPStatus.待支付)
                    {
                        //未支付时重置付款状态
                        ExpenseClaim.PayStatus = (int)PayStatus.未付款;
                        ExpenseClaim.Paytype_ID = null;
                    }

                    expenseClaimUpdateList.Add(ExpenseClaim);
                }

                #endregion
            }
        }

        private async Task UpdateOtherExpenseStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_FM_OtherExpense> otherExpenseUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.其他费用收入
                             || receivablePayable.SourceBizType == (int)BizType.其他费用支出)
            {
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                {
                    #region 更新对应业务的单据状态和付款情况

                    tb_FM_OtherExpense OtherExpense = await _appContext.Db.Queryable<tb_FM_OtherExpense>()
                        .Includes(c => c.tb_FM_OtherExpenseDetails)
                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                     && c.ExpenseMainID == receivablePayable.SourceBillId).SingleAsync();
                    if (OtherExpense != null)
                    {
                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == OtherExpense.TotalAmount)
                        {
                            OtherExpense.DataStatus = (int)DataStatus.完结;
                            OtherExpense.PayStatus = (int)PayStatus.全部付款;
                            OtherExpense.Paytype_ID = entity.Paytype_ID;
                            otherExpenseUpdateList.Add(OtherExpense);
                        }
                    }

                    #endregion
                }
            }
        }

        private async Task UpdatePriceAdjustmentStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_FM_PriceAdjustment> priceAdjustmentUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.销售价格调整单 || receivablePayable.SourceBizType == (int)BizType.采购价格调整单)
            {
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                {
                    #region 更新对应业务的单据状态和付款情况

                    tb_FM_PriceAdjustment priceAdjustment = await _appContext.Db.Queryable<tb_FM_PriceAdjustment>()
                        .Includes(c => c.tb_FM_PriceAdjustmentDetails, b => b.tb_fm_priceadjustment)
                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                     && c.AdjustId == receivablePayable.SourceBillId).SingleAsync();
                    if (priceAdjustment != null)
                    {
                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == priceAdjustment.TotalLocalDiffAmount)
                        {
                            //priceAdjustment.DataStatus = (int)DataStatus.完结;
                            //价格调整单是不是也要加一个付款方式？区别账期？
                            priceAdjustment.PayStatus = (int)PayStatus.全部付款;
                            priceAdjustment.Paytype_ID = entity.Paytype_ID;
                            priceAdjustmentUpdateList.Add(priceAdjustment);
                        }
                    }

                    #endregion
                }
            }
        }

        private async Task UpdatePurEntryReStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_PurEntryRe> purEntryReUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.采购退货单)
            {
                //厂商退款 时才处理
                //退货单审核后生成红字应收单（负金额）
                //没有记录支付状态，只标记结案处理
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付 || receivablePayable.ARAPStatus == (int)ARAPStatus.部分支付)
                {
                    tb_PurEntryRe purEntryRe = await _appContext.Db.Queryable<tb_PurEntryRe>()
                        .Where(c => c.DataStatus >= (int)DataStatus.确认
                     && c.PurEntryRe_ID == receivablePayable.SourceBillId).SingleAsync();
                    if (purEntryRe != null)
                    {
                        if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                        {
                            purEntryRe.DataStatus = (int)DataStatus.完结;
                            purEntryRe.PayStatus = (int)PayStatus.全部付款;
                        }
                        else
                        {
                            purEntryRe.PayStatus = (int)PayStatus.部分付款;
                        }
                        purEntryRe.Paytype_ID = entity.Paytype_ID;
                        purEntryReUpdateList.Add(purEntryRe);
                    }
                }
            }
        }

        private async Task UpdatePurEntryStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_PurOrder> purOrderUpdateList, List<tb_PurEntry> purEntryUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.采购入库单)
            {
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                {
                    #region 更新对应业务的单据状态和付款情况

                    tb_PurEntry purEntiry = await _appContext.Db.Queryable<tb_PurEntry>()
                        .Includes(c => c.tb_purorder, b => b.tb_PurEntries)
                      .Where(c => c.DataStatus >= (int)DataStatus.确认
                     && c.PurEntryID == receivablePayable.SourceBillId).SingleAsync();
                    if (purEntiry != null)
                    {
                        //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                        if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == purEntiry.TotalAmount)
                        {
                            //财务只管财务的状态?
                            // purEntiry.DataStatus = (int)DataStatus.完结;
                            purEntiry.PayStatus = (int)PayStatus.全部付款;
                            purEntiry.Paytype_ID = entity.Paytype_ID;
                        }

                        if (purEntiry.tb_purorder.tb_PurEntries != null)
                        {
                            //如果这个出库单的上级 订单 是我次出库的。他出库的状态都是全部付款了。则这个订单就全部付款了。
                            //订单要保证全部出库了。才能这样算否则就先不管订单状态。只是部分付款
                            List<tb_PurEntry> otherPurEntrys = purEntiry.tb_purorder.tb_PurEntries
                            .Where(c => c.PurEntryID != purEntiry.PurEntryID && c.PayStatus == (int)PayStatus.全部付款).ToList();

                            if (receivablePayable.LocalPaidAmount == purEntiry.TotalAmount
                                && otherPurEntrys.Sum(c => c.TotalAmount) + purEntiry.TotalAmount == purEntiry.tb_purorder.TotalAmount)
                            {
                                purEntiry.tb_purorder.PayStatus = (int)PayStatus.全部付款;
                                purEntiry.tb_purorder.Paytype_ID = entity.Paytype_ID;
                            }
                            else
                            {
                                purEntiry.tb_purorder.PayStatus = (int)PayStatus.部分付款;
                                purEntiry.tb_purorder.Paytype_ID = entity.Paytype_ID;
                            }
                        }
                        purOrderUpdateList.Add(purEntiry.tb_purorder);
                        purEntryUpdateList.Add(purEntiry);
                    }

                    #endregion
                }
            }
        }

        private async Task UpdateSaleOutReStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_SaleOutRe> saleOutReUpdateList, List<tb_SaleOut> saleOutUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.销售退回单)
            {
                //退货单审核后生成红字应收单（负金额）
                //没有记录支付状态，只标记结案处理
                if (receivablePayable.ARAPStatus == (int)ARAPStatus.全部支付)
                {
                    tb_SaleOutRe saleOutRe = await _appContext.Db.Queryable<tb_SaleOutRe>()
                        .Includes(c => c.tb_saleout)
                        .Where(c => c.DataStatus >= (int)DataStatus.确认
                     && c.SaleOutRe_ID == receivablePayable.SourceBillId).SingleAsync();
                    if (saleOutRe != null)
                    {
                        saleOutRe.DataStatus = (int)DataStatus.完结;
                        saleOutRe.PayStatus = (int)PayStatus.全部付款;
                        saleOutRe.Paytype_ID = entity.Paytype_ID;

                        if (saleOutRe.RefundStatus == (int)RefundStatus.未退款已退货)
                        {
                            saleOutRe.RefundStatus = (int)RefundStatus.已退款已退货;
                            if (saleOutRe.TotalAmount == saleOutRe.tb_saleout.TotalAmount)
                            {
                                saleOutRe.tb_saleout.RefundStatus = (int)RefundStatus.已退款已退货;
                            }
                            else
                            {
                                saleOutRe.tb_saleout.RefundStatus = (int)RefundStatus.部分退款退货;
                            }
                            saleOutUpdateList.Add(saleOutRe.tb_saleout);
                        }

                        saleOutReUpdateList.Add(saleOutRe);
                    }

                }
            }
        }


        private async Task UpdateSaleOutStatus(tb_FM_ReceivablePayable receivablePayable, tb_FM_PaymentRecord entity, List<tb_SaleOrder> saleOrderUpdateList, List<tb_SaleOut> saleOutUpdateList)
        {
            if (receivablePayable.SourceBizType == (int)BizType.销售出库单)
            {

                #region 更新对应业务的单据状态和付款情况

                tb_SaleOut saleOut = await _appContext.Db.Queryable<tb_SaleOut>()
                    .Includes(c => c.tb_saleorder, b => b.tb_SaleOuts)
                  .Where(c => c.DataStatus >= (int)DataStatus.确认 && c.SaleOut_MainID == receivablePayable.SourceBillId).SingleAsync();
                if (saleOut != null)
                {
                    //应收结清，并且结清的金额等于销售出库金额，则修改出库单的状态。同时计算对应订单情况。也更新。
                    if (receivablePayable.LocalBalanceAmount == 0 && receivablePayable.LocalPaidAmount == saleOut.TotalAmount)
                    {
                        //财务只管财务的状态
                        // saleOut.DataStatus = (int)DataStatus.完结;
                        saleOut.PayStatus = (int)PayStatus.全部付款;
                        if (entity.Paytype_ID.HasValue)
                        {
                            saleOut.Paytype_ID = entity.Paytype_ID.Value;
                        }

                    }
                    else if (receivablePayable.LocalBalanceAmount > 0 && receivablePayable.LocalPaidAmount != saleOut.TotalAmount && receivablePayable.LocalPaidAmount > 0)
                    {
                        saleOut.PayStatus = (int)PayStatus.部分付款;
                        if (entity.Paytype_ID.HasValue)
                        {
                            saleOut.Paytype_ID = entity.Paytype_ID.Value;
                        }
                    }
                    if (saleOut.tb_saleorder.tb_SaleOuts != null)
                    {
                        //如果这个出库单的上级订单，的其它出库单的他出库的状态都是全部付款了。则这个订单就全部付款了。（排除自己）
                        //订单要保证全部出库了。才能这样算否则就先不管订单状态。只是部分付款
                        List<tb_SaleOut> otherSaleOuts = saleOut.tb_saleorder.tb_SaleOuts
                            .Where(c => c.SaleOut_MainID != saleOut.SaleOut_MainID && c.PayStatus == (int)PayStatus.全部付款).ToList();

                        if (receivablePayable.LocalPaidAmount == saleOut.TotalAmount
                            && otherSaleOuts.Sum(c => c.TotalAmount) + saleOut.TotalAmount == saleOut.tb_saleorder.TotalAmount)
                        {
                            saleOut.tb_saleorder.PayStatus = (int)PayStatus.全部付款;
                            if (entity.Paytype_ID.HasValue)
                            {
                                saleOut.tb_saleorder.Paytype_ID = entity.Paytype_ID.Value;
                            }
                        }
                        else
                        {
                            saleOut.tb_saleorder.PayStatus = (int)PayStatus.部分付款;
                            if (entity.Paytype_ID.HasValue)
                            {
                                saleOut.tb_saleorder.Paytype_ID = entity.Paytype_ID.Value;
                            }
                        }

                    }
                    saleOrderUpdateList.Add(saleOut.tb_saleorder);
                    saleOutUpdateList.Add(saleOut);
                }

                #endregion

            }
        }


        /// <summary>
        /// 验证付款明细。支持部分付款场景，确保累计付款金额不超过来源单据总金额
        /// 1
        /// </summary>
        /// <param name="paymentDetails">付款明细列表</param>
        /// <param name="returnResults">返回结果</param>
        /// <returns>验证是否通过</returns>
        public async Task<bool> ValidatePaymentDetails(List<tb_FM_PaymentRecordDetail> paymentDetails, tb_FM_PaymentRecord currentSelfRecord, ReturnResults<T> returnResults = null)
        {
            // 按来源业务类型分组
            var groupedByBizType = paymentDetails
                .GroupBy(d => d.SourceBizType)
                .ToList();

            foreach (var bizTypeGroup in groupedByBizType)
            {
                // 按来源单号分组
                var groupedByBillIDAndNo = bizTypeGroup
                    .GroupBy(d => new { d.SourceBilllId, d.SourceBillNo })
                    .ToList();

                foreach (var billNoGroup in groupedByBillIDAndNo)
                {
                    var items = billNoGroup.ToList();

                    // 关键修复：只保留当前付款单的明细，过滤掉其他付款单的明细
                    // currentSelfRecord.PaymentId 是当前正在审核的付款单ID
                    var currentBillItems = items.Where(item => item.PaymentId == currentSelfRecord.PaymentId).ToList();

                    // 如果过滤后没有明细，跳过
                    if (currentBillItems.Count == 0)
                    {
                        continue;
                    }

                    // 使用过滤后的当前付款单明细进行验证
                    items = currentBillItems;

                    // 如果只有一条记录，检查历史累计付款金额
                    if (items.Count == 1)
                    {
                        // 获取来源单据的总金额和历史已付款金额
                        var sourceAmountResult = await GetSourceBillAmountInfo(billNoGroup.Key.SourceBilllId, bizTypeGroup.Key);
                        if (!sourceAmountResult.Succeeded)
                        {
                            returnResults.ErrorMsg = sourceAmountResult.ErrorMsg;
                            return false;
                        }

                        var currentPaymentAmount = items[0].LocalAmount;
                        var totalPaidAfterPayment = sourceAmountResult.ReturnObject.HistoricalPaidAmount + currentPaymentAmount;

                        // 检查累计付款金额是否超过来源单据总金额1
                        if (totalPaidAfterPayment > sourceAmountResult.ReturnObject.TotalAmount)
                        {
                            StringBuilder errorBuilder = new StringBuilder();
                            errorBuilder.AppendLine($"来源单据 {(BizType)bizTypeGroup.Key}，单号：{billNoGroup.Key.SourceBillNo} 存在超额付款风险。");
                            errorBuilder.AppendLine("付款详情：");
                            errorBuilder.AppendLine($"  - 来源单据总金额: {sourceAmountResult.ReturnObject.TotalAmount}");
                            errorBuilder.AppendLine($"  - 历史已付款金额: {sourceAmountResult.ReturnObject.HistoricalPaidAmount}");
                            errorBuilder.AppendLine($"  - 当前付款金额: {currentPaymentAmount}");
                            errorBuilder.AppendLine($"  - 付款后累计金额: {totalPaidAfterPayment}");
                            errorBuilder.AppendLine($"  - 超额金额: {totalPaidAfterPayment - sourceAmountResult.ReturnObject.TotalAmount}");
                            errorBuilder.AppendLine("请调整付款金额或检查历史付款记录。");
                            returnResults.ErrorMsg = errorBuilder.ToString();
                            return false;
                        }

                        continue;
                    }

                    // 如果有两条记录，检查是否为对冲情况
                    bool hasNegativeAmount = items.Any(detail => detail.LocalAmount < 0);
                    if (items.Count == 2 && hasNegativeAmount)
                    {
                        // 计算本币金额总和  有对冲情况才计算总和
                        decimal totalLocalAmount = items.Sum(i => i.LocalAmount);
                        // 计算外币金额总和
                        decimal totalForeignAmount = items.Sum(i => i.ForeignAmount);

                        // 检查是否满足对冲条件（总和接近0，考虑浮点数精度问题）
                        if (Math.Abs(totalLocalAmount) < 0.001m && Math.Abs(totalForeignAmount) < 0.001m)
                            continue;

                        if (bizTypeGroup.Key == (int)BizType.预收款单 || bizTypeGroup.Key == (int)BizType.预付款单)
                        {
                            var PreReceivedPayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                  .Where(c => c.PreRPID == billNoGroup.Key.SourceBilllId)
                                  .FirstAsync();
                            if (PreReceivedPayment != null)
                            {
                                if (PreReceivedPayment.LocalPaidAmount == totalLocalAmount)
                                {
                                    //余额全部退款
                                    continue;
                                }
                                else if (PreReceivedPayment.LocalPaidAmount < totalLocalAmount)
                                {
                                    //部分退款
                                    continue;
                                }
                                else
                                {
                                    StringBuilder serrorBuilder = new StringBuilder();
                                    serrorBuilder.AppendLine($"可能存在超额退款的单据:{(BizType)groupedByBizType[0].Key}，单号：{billNoGroup.Key.SourceBillNo}");
                                    serrorBuilder.AppendLine("超额退款详情：");
                                    serrorBuilder.AppendLine($"  - 预收/付款单已支付金额: {PreReceivedPayment.LocalPaidAmount}");
                                    serrorBuilder.AppendLine($"  - 当前退款金额: {totalLocalAmount}");
                                    serrorBuilder.AppendLine("请检查退款金额是否正确。");
                                    returnResults.ErrorMsg = serrorBuilder.ToString();
                                    return false;
                                }
                            }
                        }
                    }

                    // 多条记录的情况（非对冲），需要检查历史累计付款
                    // 获取来源单据的总金额和历史已付款金额
                    var multiSourceAmountResult = await GetSourceBillAmountInfo(billNoGroup.Key.SourceBilllId, bizTypeGroup.Key);
                    if (!multiSourceAmountResult.Succeeded)
                    {
                        returnResults.ErrorMsg = multiSourceAmountResult.ErrorMsg;
                        return false;
                    }

                    var currentMultiPaymentAmount = items.Sum(i => i.LocalAmount);
                    var totalMultiPaidAfterPayment = multiSourceAmountResult.ReturnObject.HistoricalPaidAmount + currentMultiPaymentAmount;

                    // 检查累计付款金额是否超过来源单据总金额
                    if (totalMultiPaidAfterPayment > multiSourceAmountResult.ReturnObject.TotalAmount)
                    {
                        StringBuilder errorBuilder = new StringBuilder();
                        errorBuilder.AppendLine($"来源单据 {(BizType)bizTypeGroup.Key}，单号：{billNoGroup.Key.SourceBillNo} 存在超额付款风险。");
                        errorBuilder.AppendLine("当前付款明细：");
                        foreach (var item in items)
                        {
                            errorBuilder.AppendLine($"  - 付款金额: {item.LocalAmount}");
                        }
                        errorBuilder.AppendLine("累计付款详情：");
                        errorBuilder.AppendLine($"  - 来源单据总金额: {multiSourceAmountResult.ReturnObject.TotalAmount}");
                        errorBuilder.AppendLine($"  - 历史已付款金额: {multiSourceAmountResult.ReturnObject.HistoricalPaidAmount}");
                        errorBuilder.AppendLine($"  - 当前付款总金额: {currentMultiPaymentAmount}");
                        errorBuilder.AppendLine($"  - 付款后累计金额: {totalMultiPaidAfterPayment}");
                        errorBuilder.AppendLine($"  - 超额金额: {totalMultiPaidAfterPayment - multiSourceAmountResult.ReturnObject.TotalAmount}");
                        errorBuilder.AppendLine("请调整付款金额或检查历史付款记录。");
                        returnResults.ErrorMsg = errorBuilder.ToString();
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// 验证支付金额，如果支付金额不等于应付金额时。要判断只能一张单据部分支付。
        /// </summary>
        /// <param name="paymentRecord"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ValidatePaymentRecordDetails(tb_FM_PaymentRecord paymentRecord)
        {

            List<tb_FM_PaymentRecordDetail> details = paymentRecord.tb_FM_PaymentRecordDetails;
            // 按业务类型分组处理
            var groupedDetails = details.GroupBy(d => d.SourceBizType);

            foreach (var group in groupedDetails)
            {
                var bizType = group.Key;
                var detailList = group.ToList();

                // 计算总应付金额和总支付金额
                decimal totalLocalPayableAmount = detailList.Sum(d => d.LocalPayableAmount); // 假设LocalPayableAmount是原始应付金额
                decimal totalPaidAmount = detailList.Sum(d => d.LocalAmount); // LocalAmount是本次支付金额

                // 如果支付金额等于应付金额，所有单据都是全额支付，无需进一步验证
                if (totalPaidAmount == totalLocalPayableAmount)
                {
                    continue;
                }

                // 如果支付金额大于应付金额，这是错误情况
                if (totalPaidAmount > totalLocalPayableAmount)
                {
                    StringBuilder errorBuilder = new StringBuilder();
                    errorBuilder.AppendLine($"业务类型 {(BizType)bizType} 的总支付金额 {totalPaidAmount} 不能超过总应付金额 {totalLocalPayableAmount}。");
                    errorBuilder.AppendLine("付款金额明细：");

                    foreach (var detail in detailList)
                    {
                        errorBuilder.AppendLine($"  - 来源单号：{detail.SourceBillNo}，支付金额：{detail.LocalAmount}，应付金额：{detail.LocalPayableAmount}");
                    }

                    throw new Exception(errorBuilder.ToString());
                }

                // 如果支付金额小于应付金额，检查部分付款的单据数量
                List<tb_FM_PaymentRecordDetail> partialPaymentDetails = new List<tb_FM_PaymentRecordDetail>();

                foreach (var detail in detailList)
                {
                    // 如果支付金额小于应付金额，则是部分付款
                    if (detail.LocalAmount < detail.LocalPayableAmount)
                    {
                        partialPaymentDetails.Add(detail);

                        // 如果部分付款的单据超过一张，则抛出异常
                        if (partialPaymentDetails.Count > 1)
                        {
                            StringBuilder errorBuilder = new StringBuilder();
                            errorBuilder.AppendLine($"业务类型 {(BizType)bizType} 中，最多只能有一张单据进行部分付款。请调整支付金额或选择单据。");
                            errorBuilder.AppendLine("以下是部分付款的单据信息：");

                            foreach (var partialDetail in partialPaymentDetails)
                            {
                                errorBuilder.AppendLine($"  - 来源单号：{partialDetail.SourceBillNo}，支付金额：{partialDetail.LocalAmount}，应付金额：{partialDetail.LocalPayableAmount}");
                            }

                            throw new Exception(errorBuilder.ToString());
                        }
                    }
                    // 如果支付金额大于应付金额，也是错误情况
                    else if (detail.LocalAmount > detail.LocalPayableAmount)
                    {
                        StringBuilder errorBuilder = new StringBuilder();
                        errorBuilder.AppendLine($"业务类型 {(BizType)bizType} 中，单据 {detail.SourceBillNo} 的支付金额 {detail.LocalAmount} 不能超过应付金额 {detail.LocalPayableAmount}。");
                        errorBuilder.AppendLine("超额支付详情：");
                        errorBuilder.AppendLine($"  - 单据ID：{detail.SourceBillNo}");
                        errorBuilder.AppendLine($"  - 支付金额：{detail.LocalAmount}");
                        errorBuilder.AppendLine($"  - 应付金额：{detail.LocalPayableAmount}");
                        errorBuilder.AppendLine($"  - 超额金额：{detail.LocalAmount - detail.LocalPayableAmount}");
                        errorBuilder.AppendLine("请调整支付金额后重试。");
                        throw new Exception(errorBuilder.ToString());
                    }
                }

                // 如果没有部分付款的单据，但总支付金额小于总应付金额，这也是错误情况
                if (partialPaymentDetails.Count == 0 && totalPaidAmount < totalLocalPayableAmount)
                {
                    StringBuilder errorBuilder = new StringBuilder();
                    errorBuilder.AppendLine($"业务类型 {(BizType)bizType} 的总支付金额 {totalPaidAmount} 小于总应付金额 {totalLocalPayableAmount}，但没有单据被标记为部分付款。");
                    errorBuilder.AppendLine("提示：");
                    errorBuilder.AppendLine("1. 如要进行全额支付，请确保总支付金额等于总应付金额");
                    errorBuilder.AppendLine("2. 如需进行部分支付，请将其中一张单据的支付金额设为小于其应付金额");
                    errorBuilder.AppendLine("3. 系统限制每次只能对一张单据进行部分支付");
                    throw new Exception(errorBuilder.ToString());
                }
            }

            return true;
        }


        /// <summary>
        /// 自动分配支付金额的方法
        /// 根据明细顺序对支付金额进行自动分配，确保实际支付总金额等于各明细分配金额之和
        /// </summary>
        /// <param name="paymentRecord">付款单记录</param>
        /// <param name="details">付款明细列表</param>
        /// <returns>分配是否成功</returns>
        /// <exception cref="ArgumentNullException">当参数为空时抛出</exception>
        public bool AutoDistributePaymentAmount(tb_FM_PaymentRecord paymentRecord, List<tb_FM_PaymentRecordDetail> details)
        {
            try
            {
                // 参数验证
                if (paymentRecord == null)
                    throw new ArgumentNullException(nameof(paymentRecord), "付款单记录不能为空");
                if (details == null)
                    throw new ArgumentNullException(nameof(details), "付款明细列表不能为空");


                // 初始化所有明细的支付金额为0
                foreach (var detail in details)
                {
                    detail.LocalAmount = 0;
                }

                // 按业务类型分组处理
                var groupedDetails = details.GroupBy(d => d.SourceBizType);
                decimal totalDistributedAmount = 0;

                foreach (var group in groupedDetails)
                {
                    var detailList = group.ToList();
                    decimal totalLocalPayableAmount = detailList.Sum(d => d.LocalPayableAmount);

                    _logger?.LogDebug($"处理业务类型: {group.Key}，明细数量: {detailList.Count}，应付总金额: {totalLocalPayableAmount}");

                    // 计算该业务类型应分配的金额比例
                    decimal groupAllocationRatio = 0;
                    decimal groupTotalPayable = details.Where(d => d.SourceBizType == group.Key).Sum(d => d.LocalPayableAmount);
                    decimal overallTotalPayable = details.Sum(d => d.LocalPayableAmount);

                    if (overallTotalPayable > 0)
                    {
                        groupAllocationRatio = groupTotalPayable / overallTotalPayable;
                    }

                    // 计算该业务类型的分配金额
                    decimal groupAllocatedAmount = Math.Round(paymentRecord.TotalLocalAmount * groupAllocationRatio, 2);
                    decimal remainingGroupAmount = groupAllocatedAmount;

                    _logger?.LogDebug($"业务类型 {group.Key} 的分配比例: {groupAllocationRatio:P2}，分配金额: {groupAllocatedAmount}");

                    // 按明细顺序进行分配
                    for (int i = 0; i < detailList.Count; i++)
                    {
                        var detail = detailList[i];

                        // 边界情况检查
                        if (detail.LocalPayableAmount <= 0)
                        {
                            _logger?.LogWarning($"明细 {i + 1} 的应付金额小于等于0，跳过分配: {detail.LocalPayableAmount}");
                            continue;
                        }

                        // 计算当前明细可分配的金额
                        decimal allocableAmount;
                        if (remainingGroupAmount >= detail.LocalPayableAmount)
                        {
                            // 全额支付该明细
                            allocableAmount = detail.LocalPayableAmount;
                        }
                        else
                        {
                            // 部分支付该明细
                            allocableAmount = remainingGroupAmount;
                        }

                        // 更新明细支付金额
                        detail.LocalAmount = allocableAmount;
                        remainingGroupAmount -= allocableAmount;
                        totalDistributedAmount += allocableAmount;

                        _logger?.LogDebug($"明细 {i + 1} 分配金额: {allocableAmount}，剩余可分配金额: {remainingGroupAmount}");

                        // 如果没有剩余金额，结束分配
                        if (remainingGroupAmount <= 0)
                        {
                            break;
                        }
                    }

                    // 处理舍入误差，确保分配准确性
                    if (Math.Abs(remainingGroupAmount) > 0.01m)
                    {
                        _logger?.LogWarning($"业务类型 {group.Key} 存在分配误差: {remainingGroupAmount}");
                        // 尝试将误差分配给最后一个分配的明细
                        for (int i = detailList.Count - 1; i >= 0; i--)
                        {
                            if (detailList[i].LocalAmount > 0)
                            {
                                detailList[i].LocalAmount += remainingGroupAmount;
                                totalDistributedAmount += remainingGroupAmount;
                                _logger?.LogDebug($"已处理分配误差，调整明细 {i + 1} 的金额: {remainingGroupAmount}");
                                break;
                            }
                        }
                    }
                }

                // 最终验证：确保分配总额与支付总额一致
                decimal difference = Math.Abs(totalDistributedAmount - paymentRecord.TotalLocalAmount);
                if (difference > 0.01m)
                {
                    // 调整最后一个有支付金额的明细，处理舍入误差
                    var lastDetail = details.LastOrDefault(d => d.LocalAmount > 0);
                    if (lastDetail != null)
                    {
                        lastDetail.LocalAmount += (paymentRecord.TotalLocalAmount - totalDistributedAmount);
                        _logger?.LogDebug($"最终调整：修正分配差异 {paymentRecord.TotalLocalAmount - totalDistributedAmount}");
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "支付金额自动分配失败");
                // 添加用户友好的错误提示
                string errorMessage = $"支付金额自动分配过程中发生错误：{ex.Message}";
                if (_logger == null) // 如果没有日志记录器，显示错误消息
                {
                    MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }


        public async Task<tb_FM_PaymentRecord> BuildPaymentRecord(tb_FM_ExpenseClaim entity)
        {
            //报销单审核时 自动生成 收付款记录，同时注意报销金额要与付款金额一致

            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entity);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.IsFromPlatform = false;
            paymentRecord.ReceivePaymentType = (int)ReceivePaymentType.付款;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.费用报销单);

            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细 
            paymentRecordDetail.SourceBillNo = entity.ClaimNo;
            paymentRecordDetail.SourceBilllId = entity.ClaimMainID;
            paymentRecordDetail.Currency_ID = entity.Currency_ID;
            paymentRecordDetail.LocalAmount = entity.ClaimAmount;
            paymentRecordDetail.SourceBizType = (int)BizType.费用报销单;
            paymentRecordDetail.Summary = $"由费用报销单{entity.ClaimNo}转换自动生成。";


            paymentRecordDetail.LocalPayableAmount = entity.ClaimAmount;

            #endregion

            //一单就一行时才这样
            paymentRecord.TotalLocalAmount = paymentRecordDetail.LocalAmount;
            paymentRecord.TotalForeignAmount = paymentRecordDetail.ForeignAmount;
            paymentRecord.TotalForeignPayableAmount = paymentRecordDetail.ForeignPayableAmount;
            paymentRecord.TotalLocalPayableAmount = paymentRecordDetail.LocalPayableAmount;
            paymentRecord.LocalPamountInWords = paymentRecord.TotalLocalAmount.ToUpperAmount();

            paymentRecord.PaymentDate = entity.DocumentDate;

            paymentRecord.Reimburser = entity.Employee_ID;
            paymentRecord.CustomerVendor_ID = null;
            paymentRecord.PayeeInfoID = entity.PayeeInfoID;
            //paymentRecord.PaymentImagePath = entity.CloseCaseImagePath;
            if (entity.tb_fm_payeeinfo != null)
            {
                paymentRecord.PayeeAccountNo = entity.tb_fm_payeeinfo.Account_No;
            }


            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);

            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);
            //long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
            //if (id > 0)
            //{
            //    paymentRecordDetail.PaymentId = id;
            //    await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecordDetail>(paymentRecordDetail).ExecuteReturnSnowflakeIdAsync();
            //    paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            //}
            return paymentRecord;
        }

        public async Task<tb_FM_PaymentRecord> BuildPaymentRecord(tb_FM_OtherExpense entity)
        {
            //其它费用收入支出 审核时 自动生成 收付款记录
            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entity);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.IsFromPlatform = false;
            //0  支出  1为收入
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (entity.EXPOrINC == true)
            {
                paymentRecord.ReceivePaymentType = (int)ReceivePaymentType.收款;
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.收款单);
            }
            else
            {
                paymentRecord.ReceivePaymentType = (int)ReceivePaymentType.付款;
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.付款单);
            }
            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细 


            paymentRecordDetail.SourceBillNo = entity.ExpenseNo;
            paymentRecordDetail.SourceBilllId = entity.ExpenseMainID;
            if (entity.Currency_ID.HasValue)
            {
                paymentRecordDetail.Currency_ID = entity.Currency_ID.GetValueOrDefault();
            }
            else
            {
                paymentRecordDetail.Currency_ID = _appContext.BaseCurrency.Currency_ID;
            }

            if (paymentRecord.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecordDetail.SourceBizType = (int)BizType.其他费用收入;
                paymentRecordDetail.Summary = $"由其他费用收入单{entity.ExpenseNo}转换自动生成。";
            }
            else
            {

                paymentRecordDetail.SourceBizType = (int)BizType.其他费用支出;
                paymentRecordDetail.Summary = $"由其他费用支出单{entity.ExpenseNo}转换自动生成。";
            }
            //支出不用负数。后面会通过 ReceivePaymentType
            paymentRecordDetail.LocalAmount = entity.TotalAmount;
            paymentRecordDetail.LocalPayableAmount = entity.TotalAmount;

            #endregion

            //一单就一行时才这样
            paymentRecord.TotalLocalAmount = paymentRecordDetail.LocalAmount;
            paymentRecord.TotalForeignAmount = paymentRecordDetail.ForeignAmount;
            paymentRecord.TotalForeignPayableAmount = paymentRecordDetail.ForeignPayableAmount;
            paymentRecord.TotalLocalPayableAmount = paymentRecordDetail.LocalPayableAmount;
            paymentRecord.LocalPamountInWords = paymentRecord.TotalLocalAmount.ToUpperAmount();
            paymentRecord.PaymentDate = entity.DocumentDate;

            //paymentRecord.CustomerVendor_ID = entity.cus;
            //paymentRecord.PayeeInfoID = entity.PayeeInfoID;
            //paymentRecord.PaymentImagePath = entity.PaymentImagePath;
            //paymentRecord.PayeeAccountNo = entity.PayeeAccountNo;

            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
            paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);

            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);

            return paymentRecord;
        }

        /// <summary>
        /// 生成收付款记录表
        /// </summary>
        /// <param name="entity">预收付表</param>
        /// <param name="isRefund">true 如果是退款时 金额为负，SettlementType=退款红字</param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentRecord> BuildPaymentRecord(List<tb_FM_PreReceivedPayment> entities, bool isRefund)
        {
            if (entities.Count == 0)
            {
                throw new Exception("请选择要退款的预收付款单！");
            }

            //预收付款单 审核时 自动生成 收付款记录
            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            //转一些公共信息如往来单位，金额后面还会覆盖重置新值
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entities[0]);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.ReceivePaymentType = entities[0].ReceivePaymentType;
            paymentRecord.Employee_ID = entities[0].Employee_ID;
            paymentRecord.IsFromPlatform = entities[0].IsFromPlatform;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (entities[0].ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.收款单, CancellationToken.None);
                //如果合并生成则只能取到第一个，一般只是收款时才可能有对方的付款的水单图片
                paymentRecord.PaymentImagePath = entities[0].PaymentImagePath;
            }
            else
            {
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.付款单, CancellationToken.None);
            }

            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();
            for (int i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];

                #region 明细   一笔预收付款单只有一条明细
                tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    paymentRecordDetail.SourceBizType = (int)BizType.预收款单;
                }
                else
                {
                    paymentRecordDetail.SourceBizType = (int)BizType.预付款单;
                }
                paymentRecordDetail.DepartmentID = entities[0].DepartmentID;
                paymentRecordDetail.ProjectGroup_ID = entities[0].ProjectGroup_ID;
                paymentRecordDetail.IsFromPlatform = entity.IsFromPlatform;
                paymentRecordDetail.SourceBillNo = entity.PreRPNO;
                paymentRecordDetail.SourceBilllId = entity.PreRPID;
                paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
                paymentRecordDetail.Currency_ID = entity.Currency_ID;
                paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
                #endregion
                //只退余额
                if (isRefund)
                {
                    paymentRecordDetail.ForeignAmount = -(entity.ForeignBalanceAmount);
                    paymentRecordDetail.LocalAmount = -(entity.LocalBalanceAmount);


                }
                else
                {
                    paymentRecordDetail.LocalAmount = entity.LocalPrepaidAmount;
                    paymentRecordDetail.ForeignAmount = entity.ForeignPrepaidAmount;


                }
                paymentRecordDetail.Summary = $"来自预{(ReceivePaymentType)entity.ReceivePaymentType}{entity.PreRPNO}。";
                if (entity.PrePaymentReason.Contains("平台单号"))
                {
                    paymentRecordDetail.Summary += entity.PrePaymentReason;
                }
                paymentRecordDetail.LocalPayableAmount = paymentRecordDetail.LocalAmount;
                paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            }

            paymentRecord.TotalLocalAmount = paymentRecord.tb_FM_PaymentRecordDetails.Sum(c => c.LocalAmount);
            paymentRecord.TotalForeignAmount = paymentRecord.tb_FM_PaymentRecordDetails.Sum(c => c.ForeignAmount);
            paymentRecord.TotalLocalPayableAmount = paymentRecord.tb_FM_PaymentRecordDetails.Sum(c => c.LocalPayableAmount); paymentRecord.LocalPamountInWords = paymentRecord.TotalLocalAmount.ToUpperAmount();
            paymentRecord.PaymentDate = System.DateTime.Now;
            paymentRecord.Currency_ID = entities[0].Currency_ID;
            paymentRecord.CustomerVendor_ID = entities[0].CustomerVendor_ID;

            //默认取第一个的收款人信息
            paymentRecord.PayeeInfoID = entities[0].PayeeInfoID;
            paymentRecord.PayeeAccountNo = entities[0].PayeeAccountNo;


            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;

            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);

            return paymentRecord;
        }

        // 生成收付款记录表
        public async Task<tb_FM_PaymentRecord> BuildPaymentRecord(List<tb_FM_ReceivablePayable> entities, tb_FM_PaymentRecord OriginalPaymentRecord = null)
        {
            //通过应收 自动生成 收付款记录
            //如果应收付款单中，已经为部分付款，或可能是从预收付款单中核销了部分。所以这里生成时需要取未核销金额的应收付金额
            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            paymentRecord = mapper.Map<tb_FM_PaymentRecord>(entities[0]);
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.ReceivePaymentType = entities[0].ReceivePaymentType;
            paymentRecord.IsForCommission = entities[0].IsForCommission;
            paymentRecord.IsFromPlatform = entities[0].IsFromPlatform;
            
            List<tb_FM_PaymentRecordDetail> details = mapper.Map<List<tb_FM_PaymentRecordDetail>>(entities);
            List<tb_FM_PaymentRecordDetail> NewDetails = new List<tb_FM_PaymentRecordDetail>();

            for (int i = 0; i < details.Count; i++)
            {
                #region 明细 
                tb_FM_PaymentRecordDetail paymentRecordDetail = details[i];
                if (paymentRecord.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    paymentRecordDetail.SourceBizType = (int)BizType.应收款单;
                }
                else
                {
                    paymentRecordDetail.SourceBizType = (int)BizType.应付款单;
                }
                paymentRecordDetail.Summary = $"由应{((ReceivePaymentType)paymentRecord.ReceivePaymentType).ToString()}转换自动生成。";
                var entity = entities.FirstOrDefault(c => c.ARAPId == details[i].SourceBilllId);
                if (entity != null)
                {
                    paymentRecordDetail.Summary += entity.Remark;
                }

                paymentRecordDetail.LocalPayableAmount = details[i].LocalAmount;
                #endregion
                NewDetails.Add(paymentRecordDetail);
            }

            paymentRecord.PaymentDate = System.DateTime.Now;
            paymentRecord.Currency_ID = paymentRecord.Currency_ID;

            //应收的余额给到付款单。创建收款
            paymentRecord.TotalForeignAmount = NewDetails.Sum(c => c.ForeignAmount);
            paymentRecord.TotalLocalAmount = NewDetails.Sum(c => c.LocalAmount);
            paymentRecord.TotalLocalPayableAmount = NewDetails.Sum(c => c.LocalPayableAmount);
            if (paymentRecord.TotalForeignAmount < 0 || paymentRecord.TotalLocalAmount < 0)
            {
                paymentRecord.IsReversed = true;
                if (OriginalPaymentRecord != null)
                {
                    paymentRecord.ReversedOriginalId = OriginalPaymentRecord.PaymentId;
                    paymentRecord.ReversedOriginalNo = OriginalPaymentRecord.PaymentNo;
                }
            }
            paymentRecord.LocalPamountInWords = paymentRecord.TotalLocalAmount.ToUpperAmount();
            //默认给第一个
            paymentRecord.PayeeInfoID = entities[0].PayeeInfoID;
            paymentRecord.CustomerVendor_ID = entities[0].CustomerVendor_ID;
            paymentRecord.PayeeAccountNo = entities[0].PayeeAccountNo;
            paymentRecord.tb_FM_PaymentRecordDetails = NewDetails;
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
            if (entities[0].ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.收款单, CancellationToken.None);
                if (paymentRecord.tb_FM_PaymentRecordDetails.Where(c => c.IsFromPlatform.HasValue && c.IsFromPlatform == true).ToList().Count == paymentRecord.tb_FM_PaymentRecordDetails.Count)
                {
                    paymentRecord.IsFromPlatform = true;
                }
            }
            else
            {
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.付款单, CancellationToken.None);
            }
            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
            // 查找重复的单据（按业务类型和单据ID组合）
            var duplicates = paymentRecord.tb_FM_PaymentRecordDetails
                .GroupBy(c => new { c.SourceBizType, c.SourceBilllId })
                .Where(g => g.Count() > 1)
                .ToList();

            if (duplicates.Any())
            {
                StringBuilder errorBuilder = new StringBuilder();
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
                throw new Exception(errorBuilder.ToString());
            }
            //SourceBillNos的值来自于tb_FM_PaymentRecordDetails集合中的 SourceBillNo属性的值，用逗号隔开
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());

            BusinessHelper.Instance.InitEntity(paymentRecord);

            paymentRecord.PaymentStatus = (int)PaymentStatus.草稿;
            return paymentRecord;

        }


        /// <summary>
        /// 由对账单创建收付款单
        /// 功能说明：
        /// 1. 根据对账单信息生成收付款单
        /// 2. 处理明细数据，设置来源业务类型和摘要信息
        /// 3. 汇总金额（本币/外币）
        /// 4. 根据收付款类型生成对应编号
        /// 5. 检查重复单据（同一业务下同一张单据不能重复分次收款）
        /// 6. 初始化实体并返回草稿状态的收付款单
        /// 
        /// 注意事项：
        /// - 部分或多次收款/付款时，核销过程会按FIFO（先进先出）顺序分配金额
        /// - 生成的收付款单默认为草稿状态，需要后续审核才能完成实际核销
        /// </summary>
        /// <param name="entities">对账单列表</param>
        /// <returns>收付款单</returns>
        /// <exception cref="Exception">参数为空或验证失败时抛出</exception>
        public async Task<tb_FM_PaymentRecord> BuildPaymentRecord(List<tb_FM_Statement> entities)
        {
            #region 1. 验证输入参数
            if (entities == null || entities.Count == 0)
            {
                throw new Exception("请选择要转换的对账单！");
            }
            #endregion

            #region 2. 验证对账单状态
            var (isValid, validStatements, errorMessage) = ValidateStatements(entities);
            if (!isValid)
            {
                throw new Exception(errorMessage);
            }
            #endregion

            #region 3. 创建并初始化收付款单
            tb_FM_PaymentRecord paymentRecord = new tb_FM_PaymentRecord();
            InitializePaymentRecord(paymentRecord, validStatements[0]);
            #endregion

            #region 4. 转换对账单明细
            var newDetails = ConvertStatementDetails(validStatements, paymentRecord.Currency_ID);
            paymentRecord.tb_FM_PaymentRecordDetails = newDetails;
            #endregion

            #region 5. 计算汇总字段
            RecalculateSummaryFields(paymentRecord);
            #endregion

            #region 6. 验证重复单据
            ValidateNoDuplicates(paymentRecord);
            #endregion

            #region 7. 生成单据编号
            await GeneratePaymentNo(paymentRecord, validStatements[0]);
            #endregion

            #region 8. 设置来源单号并初始化实体
            paymentRecord.SourceBillNos = string.Join(",", paymentRecord.tb_FM_PaymentRecordDetails.Select(t => t.SourceBillNo).ToArray());
            BusinessHelper.Instance.InitEntity(paymentRecord);
            paymentRecord.PaymentStatus = (int)PaymentStatus.草稿;
            #endregion

            return paymentRecord;
        }

        #region 私有辅助方法

        /// <summary>
        /// 验证对账单列表状态
        /// </summary>
        /// <param name="entities">对账单列表</param>
        /// <returns>验证结果：(是否有效, 有效对账单列表, 错误信息)</returns>
        private (bool isValid, List<tb_FM_Statement> validStatements, string errorMessage) ValidateStatements(List<tb_FM_Statement> entities)
        {
            var validStatements = new List<tb_FM_Statement>();
            var errorMessages = new System.Text.StringBuilder();

            for (int i = 0; i < entities.Count; i++)
            {
                var statement = entities[i];
                bool canConvert = statement.StatementStatus == (int)StatementStatus.确认 &&
                                  statement.ApprovalStatus == (int)ApprovalStatus.审核通过 &&
                                  statement.ApprovalResults.HasValue &&
                                  statement.ApprovalResults.Value;

                if (canConvert || statement.StatementStatus == (int)StatementStatus.部分结算)
                {
                    validStatements.Add(statement);
                }
                else
                {
                    var paymentType = (ReceivePaymentType)statement.ReceivePaymentType;
                    errorMessages.AppendLine($"{i + 1}) {paymentType}对账单 {statement.StatementNo}状态为【{((StatementStatus)statement.StatementStatus.Value).ToString()}】无法生成{paymentType}单。");
                }
            }

            return (errorMessages.Length == 0, validStatements, errorMessages.ToString());
        }

        /// <summary>
        /// 初始化收付款单基本信息
        /// </summary>
        /// <param name="paymentRecord">收付款单</param>
        /// <param name="firstStatement">第一个对账单（用于获取基础信息）</param>
        private void InitializePaymentRecord(tb_FM_PaymentRecord paymentRecord, tb_FM_Statement firstStatement)
        {
            // 基础字段映射
            paymentRecord = mapper.Map(firstStatement, paymentRecord);

            #region 重置状态字段
            paymentRecord.ApprovalResults = null;
            paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
            paymentRecord.Approver_at = null;
            paymentRecord.Approver_by = null;
            paymentRecord.PrintStatus = 0;
            paymentRecord.ActionStatus = ActionStatus.新增;
            paymentRecord.ApprovalOpinions = "";
            paymentRecord.Modified_at = null;
            paymentRecord.Modified_by = null;
            paymentRecord.PrimaryKeyID = 0;
            #endregion

            #region 设置基本信息
            paymentRecord.IsForCommission = false;
            //对账单过来的无法确定是否为平台单 默认否
            paymentRecord.IsFromPlatform = false;
            paymentRecord.PayeeInfoID = firstStatement.PayeeInfoID;
            paymentRecord.CustomerVendor_ID = firstStatement.CustomerVendor_ID;
            paymentRecord.Employee_ID = firstStatement.Employee_ID;
            paymentRecord.Currency_ID = _appContext.BaseCurrency.Currency_ID;
            paymentRecord.PaymentDate = DateTime.Now;
            paymentRecord.PayeeAccountNo = firstStatement.PayeeAccountNo;
            paymentRecord.ReceivePaymentType = firstStatement.ReceivePaymentType;
            #endregion
        }

        /// <summary>
        /// 转换对账单明细
        /// </summary>
        /// <param name="statements">有效对账单列表</param>
        /// <param name="currencyId">币别ID</param>
        /// <returns>收付款单明细列表</returns>
        private List<tb_FM_PaymentRecordDetail> ConvertStatementDetails(List<tb_FM_Statement> statements, long currencyId)
        {
            var newDetails = new List<tb_FM_PaymentRecordDetail>();

            foreach (var statement in statements)
            {
                #region 明细转换
                tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();

                // 设置来源业务类型
                paymentRecordDetail.SourceBizType = (int)BizType.对账单;

                // 设置来源单据ID和单号
                paymentRecordDetail.SourceBilllId = statement.StatementId;
                paymentRecordDetail.SourceBillNo = statement.StatementNo;

                // 设置摘要信息
                paymentRecordDetail.Summary = $"本次生成的{Enum.GetName(typeof(ReceivePaymentType), statement.ReceivePaymentType)}款金额：{Math.Abs(statement.ClosingBalanceLocalAmount):F2},由应{Enum.GetName(typeof(ReceivePaymentType), statement.ReceivePaymentType)}对账单的剩余未付金额自动生成。";

                // 设置币别
                paymentRecordDetail.Currency_ID = currencyId;

                // 处理金额逻辑：确保金额为正数
                // 付款对账单和收款对账单，金额都应为正数
                paymentRecordDetail.LocalAmount = Math.Abs(statement.ClosingBalanceLocalAmount);
                paymentRecordDetail.LocalPayableAmount = Math.Abs(statement.ClosingBalanceLocalAmount);
                #endregion

                newDetails.Add(paymentRecordDetail);
            }

            return newDetails;
        }

        /// <summary>
        /// 重新计算汇总字段
        /// </summary>
        /// <param name="paymentRecord">收付款单</param>
        private void RecalculateSummaryFields(tb_FM_PaymentRecord paymentRecord)
        {
            if (paymentRecord.tb_FM_PaymentRecordDetails == null || !paymentRecord.tb_FM_PaymentRecordDetails.Any())
            {
                paymentRecord.TotalForeignAmount = 0;
                paymentRecord.TotalLocalAmount = 0;
                paymentRecord.TotalLocalPayableAmount = 0;
                return;
            }

            paymentRecord.TotalForeignAmount = paymentRecord.tb_FM_PaymentRecordDetails.Sum(c => c.ForeignAmount);
            paymentRecord.TotalLocalAmount = paymentRecord.tb_FM_PaymentRecordDetails.Sum(c => c.LocalAmount);
            paymentRecord.TotalLocalPayableAmount = paymentRecord.tb_FM_PaymentRecordDetails.Sum(c => c.LocalPayableAmount);
            paymentRecord.LocalPamountInWords = paymentRecord.TotalLocalAmount.ToUpperAmount();
        }

        /// <summary>
        /// 验证明细中是否有重复的单据
        /// </summary>
        /// <param name="paymentRecord">收付款单</param>
        /// <exception cref="Exception">存在重复单据时抛出</exception>
        private void ValidateNoDuplicates(tb_FM_PaymentRecord paymentRecord)
        {
            var duplicates = paymentRecord.tb_FM_PaymentRecordDetails
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
                throw new Exception(errorBuilder.ToString());
            }
        }

        /// <summary>
        /// 生成收付款单编号
        /// </summary>
        /// <param name="paymentRecord">收付款单</param>
        /// <param name="firstStatement">第一个对账单</param>
        private async Task GeneratePaymentNo(tb_FM_PaymentRecord paymentRecord, tb_FM_Statement firstStatement)
        {
            IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();

            if (firstStatement.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.收款单, CancellationToken.None);

                // 检查是否全部来自平台
                if (paymentRecord.tb_FM_PaymentRecordDetails.All(c => c.IsFromPlatform.HasValue && c.IsFromPlatform.Value))
                {
                    paymentRecord.IsFromPlatform = true;
                }
            }
            else
            {
                paymentRecord.PaymentNo = await bizCodeService.GenerateBizBillNoAsync(BizType.付款单, CancellationToken.None);
            }
        }

        #endregion


        public static bool ShowInvalidMessage(ValidationResult results)
        {
            bool validationSucceeded = results.IsValid;
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return results.IsValid;
        }
        public async Task<bool> BaseLogicDeleteAsync(tb_FM_PaymentRecord ObjectEntity)
        {
            //  ReturnResults<tb_FM_PaymentRecordController> rrs = new Business.ReturnResults<tb_FM_PaymentRecordController>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PaymentRecord>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // _cacheManager.DeleteEntityList<tb_FM_PaymentRecordController>(entity);
            }
            return false;
        }

        public async Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model, bool UseTran = false) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            //缓存当前编辑的对象。如果撤销就回原来的值
            T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
            try
            {

                tb_FM_PaymentRecord entity = model as tb_FM_PaymentRecord;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };

                if (UseTran)
                {
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                }


                if (entity.PaymentId > 0)
                {

                    rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PaymentRecord>(entity as tb_FM_PaymentRecord)
               .Include(m => m.tb_FM_PaymentRecordDetails)
           .ExecuteCommandAsync();
                }
                else
                {
                    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PaymentRecord>(entity as tb_FM_PaymentRecord)
            .Include(m => m.tb_FM_PaymentRecordDetails)

            .ExecuteCommandAsync();


                }

                if (UseTran)
                {
                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();
                }
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.PaymentId;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                if (UseTran)
                {
                    _unitOfWorkManage.RollbackTran();
                }
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
                _logger.Error(ex);
            }

            return rsms;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApproval(List<tb_FM_PaymentRecord> entitys, ApprovalEntity approvalEntity)
        {
            throw new Exception("收付款单，系统不支持批量审核！");
            return false;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                if (!approvalEntity.ApprovalResults)
                {
                    if (entitys == null)
                    {
                        return false;
                    }
                }
                else
                {
                    foreach (var entity in entitys)
                    {
                        //这部分是否能提出到上一级公共部分？
                        entity.PaymentStatus = (int)PaymentStatus.待审核;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                        BusinessHelper.Instance.ApproverEntity(entity);
                    }

                    //只更新指定列
                    var result = _unitOfWorkManage.GetDbClient().Updateable(entitys).UpdateColumns(it => new
                    {
                        it.PaymentStatus,
                        it.ApprovalOpinions,
                        it.ApprovalResults,
                        it.ApprovalStatus,
                        it.Approver_at,
                        it.Approver_by
                    }).ExecuteCommandHasChangeAsync();
                }
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(approvalEntity));
                return false;
            }

        }


        /// <summary>
        /// 这里如果查询得足够详细。就类似对账单了？
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PaymentRecord> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PaymentRecord>()
                .Where(m => m.PaymentId == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_customervendor)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_FM_PaymentRecordDetails)
                              .Includes(a => a.tb_FM_PaymentRecords)
                              .Includes(a => a.tb_FM_PaymentRecordsByReversedOriginal)
                              .Includes(a => a.tb_fm_paymentrecord)
                              .Includes(a => a.tb_fm_paymentrecordByReversedOriginal)
                            .ToListAsync();


            foreach (var item in list)
            {
                //为了查询效率。收款明细中，按业务类型查。虽然少。但是有这种情况。如 又有预付，又有应付
                //相同客户，多个应收可以合成一个收款 。所以明细中就是对应的应收单。
                //为了提高性能 将按业务类型分组后再找到对应的单据去处理
                //目前 所有业务都进应收应付 简化逻辑 
                var groupList = item.tb_FM_PaymentRecordDetails.GroupBy(c => c.SourceBizType).Select(c => new { SourceBizType = c.Key }).ToList();

                var details = item.tb_FM_PaymentRecordDetails;
                Dictionary<int, List<long>> GroupResult = details
                    .GroupBy(d => d.SourceBizType)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(d => d.PaymentDetailId).ToList()
                    );

                foreach (var group in GroupResult)
                {
                    long[] sourcebillids = item.tb_FM_PaymentRecordDetails.Where(c => group.Value.Contains(c.PaymentDetailId)).Select(c => c.SourceBilllId).ToArray();

                    if (group.Key == (int)BizType.应收款单 || group.Key == (int)BizType.应付款单)
                    {
                        #region  
                        List<tb_FM_ReceivablePayable> receivablePayableList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                             .Includes(c => c.tb_FM_ReceivablePayableDetails)
                             .Where(c => sourcebillids.Contains(c.ARAPId))
                             .ToListAsync();
                        foreach (var detail in item.tb_FM_PaymentRecordDetails)
                        {
                            detail.tb_FM_ReceivablePayables = receivablePayableList.Where(c => c.SourceBillId.Value == detail.SourceBilllId).ToList();
                        }

                        #endregion
                    }


                    if (group.Key == (int)BizType.预收款单 || group.Key == (int)BizType.预付款单)
                    {
                        List<tb_FM_PreReceivedPayment> PreReceivablePayableList = await _appContext.Db.Queryable<tb_FM_PreReceivedPayment>()
                           .Where(c => sourcebillids.Contains(c.PreRPID))
                           .ToListAsync();
                        foreach (var detail in item.tb_FM_PaymentRecordDetails)
                        {
                            detail.tb_FM_PreReceivedPayments = PreReceivablePayableList.Where(c => c.SourceBillId.Value == detail.SourceBilllId).ToList();
                        }

                    }
                }

            }

            return list as List<T>;
        }




    }

}



