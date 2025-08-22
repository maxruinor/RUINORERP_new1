
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:33:16
// **************************************
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Model;
using FluentValidation.Results;
using RUINORERP.Services;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt;
using AutoMapper;
using RUINORERP.Business.StatusManagerService;
using OfficeOpenXml.Export.ToDataTable;
using Fireasy.Common.Extensions;
using RUINORERP.Business.CommService;
using System.Windows.Forms;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentController<T> : BaseController<T> where T : class
    {
        //protected async Task SettlePrePayment()
        //{
        //    var targetStatus = IsFullSettlement ?
        //        PrePaymentStatus.全额核销 :
        //        PrePaymentStatus.部分核销;

        //    bool result = await Submit(targetStatus);
        //    if (result)
        //    {
        //        // 核销成功后处理
        //    }
        //}
        /// <summary>
        /// 客户取消订单时，如果有订单，如果财务没有在他对应的收付单里审核前是可以反审的。否则只能通过红冲机制处理。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;

            try
            {
                //只有生效状态的才允许反审，其它不能也不需要，有可能可删除。也可能只能红冲
                // 获取当前状态
                var statusProperty = typeof(PrePaymentStatus).Name;
                var currentStatus = (PrePaymentStatus)Enum.ToObject(
                    typeof(PrePaymentStatus),
                    entity.GetPropertyValue(statusProperty)
                );

                if (!FMPaymentStatusHelper.CanReReview(currentStatus, false))
                {
                    rmrs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单不可以反审";
                    return rmrs;
                }

                var paymentRecordController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //注意，反审 将对应的预付生成的收款单，只有收款单没有审核前，可以删除
                //不能直接删除上级。要让对应的人员自己删除。不然不清楚。逻辑也不对。只能通过判断
                var PaymentRecordlist = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                            // .Where(c => (c.PaymentStatus == (int)PaymentStatus.草稿 || c.PaymentStatus == (int)PaymentStatus.待审核))
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.PreRPID))
                              .ToListAsync();
                if (PaymentRecordlist != null && PaymentRecordlist.Count > 0)
                {
                    //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                    if ((PaymentRecordlist.Any(c => c.PaymentStatus == (int)PaymentStatus.已支付)
                        && PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"存在【已支付】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单，不能反审预款单,请联系上级财务，或作退回处理。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else
                    {
                        foreach (var item in PaymentRecordlist)
                        {
                            //删除对应的由预收款生成的收款单
                            await _appContext.Db.DeleteNav<tb_FM_PaymentRecord>(item)
                                .Include(c => c.tb_FM_PaymentRecordDetails)
                                .ExecuteCommandAsync();
                        }

                    }

                }

                entity.PrePaymentStatus = (int)PrePaymentStatus.待审核;
                entity.ApprovalResults = null;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it => new
                {
                    it.PrePaymentStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
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
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

        }



        /// <summary>
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付，这财务审核收款单前，还是可以反审的
        /// 审核通过时
        /// 预收款单本身是「收款」的一种业务类型，销售订单审核时已经生成了预收款单 ，通过 BizType 标记其业务属性为预收款。
        /// 这里审核生成收款单
        /// tb_FM_PaymentSettlement 不需要立即生成，但需在后续触发核销时生成（抵扣时生成）。
        /// 销售订单审核时，则自动审核掉预收款单
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;
            try
            {
                if (entity.PrePaymentStatus != (int)PrePaymentStatus.草稿 && entity.PrePaymentStatus != (int)PrePaymentStatus.待审核)
                {
                    rmrs.Succeeded = false;
                    rmrs.ErrorMsg = $"预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单{entity.PreRPNO}，状态为【{((PrePaymentStatus)entity.PrePaymentStatus).ToString()}】\r\n请确认状态为【草稿】或【待审核】才可以审核。";

                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.LogInformation(rmrs.ErrorMsg);
                    }

                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    if (!entity.PayeeInfoID.HasValue)
                    {
                        rmrs.ErrorMsg = $"{entity.PreRPNO}付款时，对方的收款信息必填!";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }

                var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();

                var records = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecordDetail>()
                    .Includes(c => c.tb_fm_paymentrecord)
                    .Where(c => c.SourceBizType == (int)BizType.预收款单 && c.SourceBilllId == entity.PreRPID)
                    .Where(c => c.tb_fm_paymentrecord.ApprovalStatus == (int)ApprovalStatus.已审核
                    && c.tb_fm_paymentrecord.ApprovalResults == true
                    && c.tb_fm_paymentrecord.PaymentStatus == (int)PaymentStatus.已支付)
                    .ToListAsync();
                if (records.Count > 0)
                {
                    //一个预收款单可以生成两份收款单，仅仅是在退款要冲销时。即收款金额要为负数
                    rmrs.Succeeded = false;
                    rmrs.ErrorMsg = $" 预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单{entity.PreRPNO},已生成收款单,系统不支持重复生成收付款。";
                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.LogInformation(rmrs.ErrorMsg);
                    }
                    return rmrs;
                }


                List<tb_FM_PreReceivedPayment> PendingApprovalDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                    .Where(c => c.ReceivePaymentType == entity.ReceivePaymentType)
                    .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.待审核)
                    .Where(c => c.SourceBillId == entity.SourceBillId)
                    .ToListAsync();


                //加上自己
                //要把自己也算上。不能大于1 ，entity是等待审核。所以拼一起
                //PendingApprovalDetails.AddRange(entity);

                //验证 如果相同收款方向 下，相同业务类型下的相同来源单号，比方一个销售订单 多次收款，一个采购订单 多次付款时
                //则要计算累计收款金额，如果累计金额大于等于收款金额，则不能再收款。如果超过收款金额，则进一步提示才能继续收款。
                decimal TotalOrderAmount = 0;
                if (entity.SourceBizType.HasValue)
                {
                    if (entity.SourceBizType.Value == (int)BizType.销售订单)
                    {
                        var saleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                        .Where(c => c.SOrder_ID == entity.SourceBillId)
                        .SingleAsync();
                        if (saleOrder != null)
                        {
                            TotalOrderAmount = saleOrder.TotalAmount;
                        }
                    }
                    if (entity.SourceBizType.Value == (int)BizType.采购订单)
                    {
                        var purOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                        .Where(c => c.PurOrder_ID == entity.SourceBillId)
                        .SingleAsync();
                        if (purOrder != null)
                        {
                            TotalOrderAmount = purOrder.TotalAmount;
                        }
                    }
                }

                if (!ValidatePaymentDetails(PendingApprovalDetails, entity, TotalOrderAmount, rmrs))
                {
                    //rmrs.ErrorMsg = "相同业务类型下不能有相同的来源单号!审核失败。";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }


                entity.PrePaymentStatus = (int)PrePaymentStatus.已生效;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;

                //下面的自动审核会修改PrePaymentStatus状态。所以已经生效生赋值。后面 可能是审核后变为等待核销
                tb_FM_PaymentRecord paymentRecord = paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { entity }, false);
                var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                if (rrs.Succeeded)
                {
                    rmrs.ReturnObjectAsOtherEntity = paymentRecord;
                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                BusinessHelper.Instance.ApproverEntity(entity);


                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it => new
                {
                    it.PrePaymentStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions,
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                try
                {
                    //按配置自动审核收款单
                    if (_appContext.FMConfig.AutoAuditReceivePaymentRecordByPlatform && entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        if (entity.IsFromPlatform)
                        {
                            //自动审核收款单
                            paymentRecord.ApprovalOpinions = "系统自动审核";
                            paymentRecord.ApprovalStatus = (int)ApprovalStatus.已审核;
                            paymentRecord.ApprovalResults = true;
                            var ctrPaymentRecord = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                            ReturnResults<tb_FM_PaymentRecord> rr = await ctrPaymentRecord.ApprovalAsync(paymentRecord);
                            if (!rr.Succeeded)
                            {
                                rmrs.ErrorMsg = $"预收款单{entity.PreRPNO}审核成功，系统自动审核收款单{paymentRecord.PaymentNo}时失败!";
                                rmrs.Succeeded = false;
                                rmrs.ReturnObject = entity as T;
                                return rmrs;
                            }
                            else
                            {
                                FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                fMAuditLog.CreateAuditLog<tb_FM_PaymentRecord>("收款单自动审核成功", rr.ReturnObject as tb_FM_PaymentRecord);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }
        }



        public static bool ValidatePaymentDetails(List<tb_FM_PreReceivedPayment> paymentDetails, tb_FM_PreReceivedPayment currentPrePayment, decimal totalOrderAmount, ReturnResults<T> returnResults = null)
        {
            if (paymentDetails.Count == 0)
            {
                return true;
            }
            var PaymentType = (ReceivePaymentType)paymentDetails[0].ReceivePaymentType;


            // 按来源业务类型分组
            var groupedByBizType = paymentDetails
                .GroupBy(d => d.SourceBizType)
                .ToList();

            foreach (var bizTypeGroup in groupedByBizType)
            {
                // 按来源单号分组
                var groupedByBillNo = bizTypeGroup
                    .GroupBy(d => d.SourceBillNo)
                    .ToList();

                foreach (var billNoGroup in groupedByBillNo)
                {
                    var items = billNoGroup.ToList();

                    // 如果只有一条记录，直接通过
                    if (items.Count == 1)
                        continue;

                    // 如果有两条记录，检查是否为对冲情况
                    if (items.Count >= 2)
                    {
                        // 计算本币金额总和
                        decimal totalLocalAmount = items.Sum(i => i.LocalPrepaidAmount);
                        // 计算外币金额总和
                        decimal totalForeignAmount = items.Sum(i => i.ForeignPrepaidAmount);

                        // 检查是否满足对冲条件（总和接近0，考虑浮点数精度问题）
                        if (Math.Abs(totalLocalAmount) < 0.001m && Math.Abs(totalForeignAmount) < 0.001m)
                            continue;

                        if (totalLocalAmount > totalOrderAmount)
                        {
                            if (MessageBox.Show($"预{PaymentType}单总金额{totalLocalAmount}(包含当前金额{currentPrePayment.LocalPrepaidAmount})，超过了订单总金额{totalOrderAmount}，确定要超额预{PaymentType}吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, defaultButton: MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                continue;
                            }
                        }
                    }
                    returnResults.ErrorMsg = $"不能存在相同业务来源的数据:{(BizType)groupedByBizType[0].Key}，来源单号为:{groupedByBillNo[0].Key}";
                    returnResults.ErrorMsg += $"\r\n通常是生成了重复的预{PaymentType}单。请仔细核对！";
                    // 其他情况均视为不合法
                    return false;
                }
            }

            return true;
        }


        public async Task<bool> BaseLogicDeleteAsync(tb_FM_PreReceivedPayment ObjectEntity)
        {
            //  ReturnResults<tb_FM_PreReceivedPaymentController> rrs = new Business.ReturnResults<tb_FM_PreReceivedPaymentController>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PreReceivedPayment>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // MyCacheManager.Instance.DeleteEntityList<tb_FM_PreReceivedPaymentController>(entity);
            }
            return false;
        }

        /// <summary>
        /// 通过销售订单生成预收款单
        /// </summary>
        /// <param name="entity"></param>
        ///<param name="PrepaidAmount">手动再次预付款时的金额，</param>
        /// <returns></returns>
        public tb_FM_PreReceivedPayment BuildPreReceivedPayment(tb_SaleOrder entity, decimal PrepaidAmount = 0)
        {

            // 外币相关处理 正确是 外币时一定要有汇率
            decimal exchangeRate = 1; // 获取销售订单的汇率
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                exchangeRate = entity.ExchangeRate; // 获取销售订单的汇率
                                                    // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                    // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
            }

            #region 生成预收款

            tb_FM_PreReceivedPayment payable = new tb_FM_PreReceivedPayment();
            payable = mapper.Map<tb_FM_PreReceivedPayment>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.IsAvailable = true;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            if (entity.tb_projectgroup != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }
            //销售就是收款
            payable.ReceivePaymentType = (int)ReceivePaymentType.收款;
            payable.PreRPNO = BizCodeGenerator.Instance.GetBizBillNo(BizType.预收款单);
            payable.SourceBizType = (int)BizType.销售订单;
            payable.SourceBillNo = entity.SOrderNo;
            payable.SourceBillId = entity.SOrder_ID;
            payable.Currency_ID = entity.Currency_ID;
            payable.PrePayDate = entity.SaleDate;
            payable.ExchangeRate = exchangeRate;

            payable.LocalPrepaidAmountInWords = string.Empty;
            payable.Account_id = entity.Account_id;

            if (PrepaidAmount == 0)
            {        //如果是外币时，则由外币算出本币
                if (entity.PayStatus == (int)PayStatus.全额预付)
                {
                    //外币时 全部付款，则外币金额=本币金额/汇率 在UI中显示出来。
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignTotalAmount;
                        //payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    //本币时
                    payable.LocalPrepaidAmount = entity.TotalAmount;

                }
                else            //来自于订金
                if (entity.PayStatus == (int)PayStatus.部分预付)
                {
                    //外币时
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignDeposit;
                        // payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    else
                    {
                        payable.LocalPrepaidAmount = entity.Deposit;
                    }
                }
            }
            else
            {
                payable.LocalPrepaidAmount = PrepaidAmount;
            }

            payable.LocalPrepaidAmountInWords = payable.LocalPrepaidAmount.ToUpper();
            payable.IsAvailable = true;//默认可用
            payable.PrePaymentReason = $"销售订单{entity.SOrderNo}的预收款。";
            if (!string.IsNullOrEmpty(entity.PlatformOrderNo) && entity.PlatformOrderNo.Trim().Length > 3)
            {
                payable.PrePaymentReason += $"平台单号：{entity.PlatformOrderNo}";
            }
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.PrePaymentStatus = (int)PrePaymentStatus.待审核;

            #endregion
            return payable;
        }


        /// <summary>
        /// 通过销售订单生成预收款单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="SaveToDb"></param>
        /// <returns></returns>
        public tb_FM_PreReceivedPayment BuildPreReceivedPayment(tb_PurOrder entity, decimal PrepaidAmount = 0)
        {

            // 外币相关处理 正确是 外币时一定要有汇率
            decimal exchangeRate = 1; // 获取销售订单的汇率
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                exchangeRate = entity.ExchangeRate; // 获取销售订单的汇率
                                                    // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                    // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
            }

            #region 生成预付款

            tb_FM_PreReceivedPayment payable = new tb_FM_PreReceivedPayment();
            payable = mapper.Map<tb_FM_PreReceivedPayment>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.IsAvailable = true;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            if (entity.tb_projectgroup != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }
            //采购就是付款
            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;

            payable.PreRPNO = BizCodeGenerator.Instance.GetBizBillNo(BizType.预付款单);
            payable.SourceBizType = (int)BizType.采购订单;
            payable.SourceBillNo = entity.PurOrderNo;
            payable.SourceBillId = entity.PurOrder_ID;
            payable.Currency_ID = entity.Currency_ID;
            payable.PrePayDate = entity.PurDate;
            payable.ExchangeRate = exchangeRate;
            payable.LocalPrepaidAmountInWords = string.Empty;
            if (PrepaidAmount == 0)
            {

                //payable.Account_id = entity.Account_id;//付款账户信息 在采购订单时 不用填写。由财务决定 
                //如果是外币时，则由外币算出本币
                if (entity.PayStatus == (int)PayStatus.全额预付)
                {
                    //外币时 全部付款，则外币金额=本币金额/汇率 在UI中显示出来。
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignTotalAmount;
                        //payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    //本币时
                    payable.LocalPrepaidAmount = entity.TotalAmount;

                }
                else            //来自于订金
                if (entity.PayStatus == (int)PayStatus.部分预付)
                {
                    //外币时
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        payable.ForeignPrepaidAmount = entity.ForeignDeposit;
                        // payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                    }
                    else
                    {
                        payable.LocalPrepaidAmount = entity.Deposit;
                    }
                }
            }
            else
            {
                payable.LocalPrepaidAmount = PrepaidAmount;
            }


            //payable.LocalPrepaidAmountInWords = payable.LocalPrepaidAmount.ToString("C");
            payable.LocalPrepaidAmountInWords = payable.LocalPrepaidAmount.ToUpper();
            payable.IsAvailable = true;//默认可用
            payable.PrePaymentReason = $"采购订单{entity.PurOrderNo}的预付款";
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.PrePaymentStatus = (int)PrePaymentStatus.待审核;

            #endregion
            return payable;
        }


        ///// <summary>
        ///// 要生成收付单 没完成
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns></returns>
        //public async virtual Task<bool> BatchApproval(List<tb_FM_PreReceivedPayment> entitys, ApprovalEntity approvalEntity)
        //{
        //    try
        //    {
        //        // 开启事务，保证数据一致性
        //        _unitOfWorkManage.BeginTran();
        //        if (!approvalEntity.ApprovalResults)
        //        {
        //            if (entitys == null)
        //            {
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            foreach (var entity in entitys)
        //            {
        //                //这部分是否能提出到上一级公共部分？
        //                entity.PrePaymentStatus = (int)PrePaymentStatus.已生效;
        //                entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
        //                //后面已经修改为
        //                entity.ApprovalResults = approvalEntity.ApprovalResults;
        //                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
        //                BusinessHelper.Instance.ApproverEntity(entity);
        //                //只更新指定列
        //                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
        //                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
        //            }
        //        }
        //        // 注意信息的完整性
        //        _unitOfWorkManage.CommitTran();

        //        //_logger.Info(approvalEntity.bizName + "审核事务成功");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex);
        //        _unitOfWorkManage.RollbackTran();
        //        _logger.Error(approvalEntity.bizName + "事务回滚");
        //        return false;
        //    }

        //}


        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PreReceivedPayment> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PreReceivedPayment>()
                .Where(m => m.PreRPID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .ToListAsync();


            foreach (var item in list)
            {
                if (item.SourceBizType.HasValue && item.SourceBizType.Value == (int)BizType.采购订单)
                {
                    item.tb_purorder = await _appContext.Db.CopyNew().Queryable<tb_PurOrder>()
                        .Includes(a=>a.tb_PurOrderDetails,b=>b.tb_proddetail,c=>c.tb_prod)
                        .Where(c => c.PurOrder_ID == item.SourceBillId).FirstAsync();
                }

                if (item.SourceBizType.HasValue && item.SourceBizType.Value == (int)BizType.销售订单)
                {
                    item.tb_saleorder = await _appContext.Db.CopyNew().Queryable<tb_SaleOrder>()
                        .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.SOrder_ID == item.SourceBillId).FirstAsync();
                }
            }

            return list as List<T>;
        }
    }
}



