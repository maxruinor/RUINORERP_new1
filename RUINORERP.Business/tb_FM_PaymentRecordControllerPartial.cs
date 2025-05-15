
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
using RUINORERP.Business.FMService;
using MapsterMapper;
using IMapper = AutoMapper.IMapper;
using System.Text;
using System.Windows.Forms;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PaymentRecordController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 审核了就不能反审了。只能冲销
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        [Obsolete]
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            rmrs.ErrorMsg = "付款记录，已审核后不能反审。只能红冲";
            return rmrs;

            tb_FM_PaymentRecord entity = ObjectEntity as tb_FM_PaymentRecord;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //entity.FMPaymentStatus = (int)FMPaymentStatus.草稿;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
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
        /// 付款单审核通过时，更新对应的业务单据的收款状态。更新余额
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PaymentRecord entity = ObjectEntity as tb_FM_PaymentRecord;
            try
            {
                if (entity.TotalLocalAmount == 0 && entity.TotalForeignAmount == 0)
                {
                    rmrs.ErrorMsg = "付款金额不能为0,审核失败!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                #region
                if (entity.tb_FM_PaymentRecordDetails != null)
                {
                    foreach (var RecordDetail in entity.tb_FM_PaymentRecordDetails)
                    {
                        //收付款记录中，如果金额为负数则是 退款红冲
                        //收款单审核时。除了保存核销记录，还要来源的 如 应收中的 余额这种更新
                        if (RecordDetail.SourceBizType == (int)BizType.应收单 || RecordDetail.SourceBizType == (int)BizType.应付单)
                        {
                            #region 应收单余额更新
                            tb_FM_ReceivablePayable receivablePayable = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                .Where(c => c.ARAPId == RecordDetail.SourceBilllId).FirstAsync();
                            if (receivablePayable != null)
                            {
                                receivablePayable.ForeignPaidAmount += RecordDetail.ForeignAmount;
                                receivablePayable.LocalPaidAmount += RecordDetail.LocalAmount;

                                receivablePayable.ForeignBalanceAmount -= RecordDetail.ForeignAmount;
                                receivablePayable.LocalBalanceAmount -= RecordDetail.LocalAmount;

                                if (receivablePayable.ForeignBalanceAmount == 0 || receivablePayable.LocalBalanceAmount == 0)
                                {
                                    receivablePayable.ARAPStatus = (long)ARAPStatus.已结清;
                                }
                                //付过，没付结清
                                if ((receivablePayable.ForeignBalanceAmount > 0 && receivablePayable.ForeignPaidAmount > 0)
                                    || (receivablePayable.LocalBalanceAmount > 0 && receivablePayable.LocalPaidAmount > 0))
                                {
                                    receivablePayable.ARAPStatus = (long)ARAPStatus.部分支付;
                                }

                                var r = await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(receivablePayable).UpdateColumns(it => new
                                {
                                    it.ARAPStatus,
                                    it.ForeignPaidAmount,
                                    it.LocalPaidAmount,
                                    it.ForeignBalanceAmount,
                                    it.LocalBalanceAmount
                                }).ExecuteCommandAsync();
                                if (r > 0)
                                {
                                    //收到，付了钱。审核就会生成一笔核销记录  收款抵扣应收
                                    var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
                                    await settlementController.GenerateSettlement(entity);
                                }

                                //写回 原始单据的支付状态
                                if (receivablePayable.tb_FM_ReceivablePayableDetails != null)
                                {
                                    //通过明细中的来源类型，来源单号，来源编号分组得到原始单据数据组后再根据类型分别处理更新状态

                                    var sourceList = receivablePayable.tb_FM_ReceivablePayableDetails
                                        .GroupBy(c => new { c.SourceBizType, c.SourceBillNo, c.SourceBillId })
                                        .ToList();

                                    foreach (var PayableDetail in receivablePayable.tb_FM_ReceivablePayableDetails)
                                    {
                                        if (PayableDetail.SourceBizType == (long)BizType.销售出库单)
                                        {
                                            if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                            {

                                            }
                                        }
                                        if (PayableDetail.SourceBizType == (long)BizType.采购入库单)
                                        {
                                            if (receivablePayable.ARAPStatus == (long)ARAPStatus.已结清)
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                            #endregion



                        }


                        //单纯收款不用产生核销记录。核销要与业务相关联 
                        //退款时写回上级预收付款单 状态为 已冲销
                        //负数时
                        if (RecordDetail.SourceBizType == (int)BizType.预收款单 || RecordDetail.SourceBizType == (int)BizType.预付款单)
                        {
                            var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                .Where(c => c.PreRPID == RecordDetail.SourceBilllId).FirstAsync();
                            if (prePayment != null)
                            {
                                prePayment.PrePaymentStatus = (long)PrePaymentStatus.待核销;
                                prePayment.ForeignBalanceAmount += RecordDetail.ForeignAmount;
                                prePayment.LocalBalanceAmount += RecordDetail.LocalAmount;

                                //抵扣时 更新核销金额
                                //prePayment.ForeignPaidAmount+=entity.ForeignPaidAmount;
                                //prePayment.LocalPaidAmount += entity.LocalPaidAmount;

                                _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).UpdateColumns(it =>
                                new
                                {
                                    it.PrePaymentStatus,
                                    it.ForeignBalanceAmount,
                                    it.LocalBalanceAmount,
                                }).ExecuteCommand();
                            }

                            var result = _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>()
                                   .SetColumns(it => it.PrePaymentStatus == (long)PrePaymentStatus.已冲销)//SetColumns是可以叠加的 写2个就2个字段赋值
                                   .SetColumns(it => it.ForeignBalanceAmount == (it.ForeignBalanceAmount + RecordDetail.ForeignAmount))// 正负为0.全部冲销时
                                   .SetColumns(it => it.LocalBalanceAmount == (it.LocalBalanceAmount + RecordDetail.LocalAmount))
                                   .Where(it => it.PreRPID == RecordDetail.SourceBilllId)
                                   .ExecuteCommand();
                            if (result > 0)
                            {
                                //生成核销记录
                                //退款或红冲时。审核就会生成一笔核销记录  收款抵扣应收
                                var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
                                await settlementController.GenerateSettlement(entity);
                            }
                            entity.PaymentStatus = (long)PaymentStatus.已冲销;
                        }

                    }
                }


                //等待真正支付
                entity.PaymentStatus = (long)PaymentStatus.已支付;

                //更新账户余额
                if (entity.tb_fm_account == null && entity.Account_id.HasValue)
                {
                    entity.tb_fm_account = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Account>().Where(c => c.Account_id == entity.Account_id).FirstAsync();
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

                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);


                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).ExecuteCommandAsync();
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


        // 生成收付款记录表
        /// <summary>
        /// 生成收付款记录表
        /// </summary>
        /// <param name="entity">预收付表</param>
        /// <param name="isRefund">true 如果是退款时 金额为负，SettlementType=退款红冲</param>
        /// <returns></returns>
        public async Task<tb_FM_PaymentRecord> CreatePaymentRecord(tb_FM_PreReceivedPayment entity, bool isRefund)
        {
            //预收付款单 审核时 自动生成 收付款记录
            
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
            paymentRecord.ReceivePaymentType = entity.ReceivePaymentType;
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
            }
            else
            {
                paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
            }
            tb_FM_PaymentRecordDetail paymentRecordDetail = new tb_FM_PaymentRecordDetail();
            #region 明细 

            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                paymentRecordDetail.SourceBizType = (int)BizType.预收款单;
            }
            else
            {
                paymentRecordDetail.SourceBizType = (int)BizType.预付款单;
            }
            paymentRecordDetail.SourceBillNo = entity.PreRPNO;
            paymentRecordDetail.SourceBilllId = entity.PreRPID;
            paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
            paymentRecordDetail.Currency_ID = entity.Currency_ID;
            paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
            #endregion
            if (isRefund)
            {
                paymentRecordDetail.ForeignAmount = -entity.ForeignPrepaidAmount;
                paymentRecordDetail.LocalAmount = -entity.LocalPrepaidAmount;
            }
            else
            {
                paymentRecordDetail.LocalAmount = entity.LocalPrepaidAmount;
                paymentRecordDetail.ForeignAmount = entity.ForeignPrepaidAmount;
            }

            paymentRecord.TotalLocalAmount = paymentRecordDetail.LocalAmount;
            paymentRecord.TotalForeignAmount = paymentRecordDetail.ForeignAmount;

            paymentRecord.PaymentDate = entity.PrePayDate;
            paymentRecord.Currency_ID = entity.Currency_ID;
            paymentRecord.CustomerVendor_ID = entity.CustomerVendor_ID;
            paymentRecord.PayeeInfoID = entity.PayeeInfoID;
            paymentRecord.PaymentImagePath = entity.PaymentImagePath;
            paymentRecord.PayeeAccountNo = entity.PayeeAccountNo;
            paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

            // paymentRecord.ReferenceNo=entity.no
            //自动提交 审核，等待确认收款 或支付 【实际核对收款情况到账】
            paymentRecord.PaymentStatus = (long)PaymentStatus.待审核;
            BusinessHelper.Instance.InitEntity(paymentRecord);
            long id = await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecord>(paymentRecord).ExecuteReturnSnowflakeIdAsync();
            if (id > 0)
            {
                paymentRecordDetail.PaymentId = id;
                await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_PaymentRecordDetail>(paymentRecordDetail).ExecuteReturnSnowflakeIdAsync();
                paymentRecord.tb_FM_PaymentRecordDetails.Add(paymentRecordDetail);
            }
            return paymentRecord;
        }

        // 生成收付款记录表
        public async Task<List<tb_FM_PaymentRecord>> CreatePaymentRecord(List<tb_FM_ReceivablePayable> entities, bool SaveToDb = false, tb_FM_PaymentRecord OriginalPaymentRecord = null)
        {
            //通过应收 自动生成 收付款记录

            List<tb_FM_PaymentRecord> PaymentRecords = new List<tb_FM_PaymentRecord>();
            foreach (var entity in entities)
            {
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
                paymentRecord.ReceivePaymentType = entity.ReceivePaymentType;
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款单);
                }
                else
                {
                    paymentRecord.PaymentNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款单);
                }
                List<tb_FM_PaymentRecordDetail> details = mapper.Map<List<tb_FM_PaymentRecordDetail>>(entity.tb_FM_ReceivablePayableDetails);
                List<tb_FM_PaymentRecordDetail> NewDetails = new List<tb_FM_PaymentRecordDetail>();
                for (int i = 0; i < details.Count; i++)
                {
                    #region 明细 
                    tb_FM_PaymentRecordDetail paymentRecordDetail = details[i];
                    if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        paymentRecordDetail.SourceBizType = (int)BizType.应收单;
                    }
                    else
                    {
                        paymentRecordDetail.SourceBizType = (int)BizType.应付单;
                    }

                    paymentRecordDetail.SourceBillNo = entity.ARAPNo;
                    paymentRecordDetail.SourceBilllId = entity.ARAPId;
                    paymentRecordDetail.Currency_ID = entity.Currency_ID;
                    paymentRecordDetail.ExchangeRate = entity.ExchangeRate;
                    paymentRecordDetail.ForeignAmount = entity.ForeignBalanceAmount;
                    paymentRecordDetail.LocalAmount = entity.LocalBalanceAmount;
                    #endregion
                    NewDetails.Add(paymentRecordDetail);
                }


                paymentRecord.PaymentDate = System.DateTime.Now;
                paymentRecord.Currency_ID = paymentRecord.Currency_ID;

                //应收的余额给到付款单。创建收款
                paymentRecord.TotalForeignAmount = entity.ForeignBalanceAmount;
                paymentRecord.TotalLocalAmount = entity.LocalBalanceAmount;

                if (paymentRecord.TotalForeignAmount < 0 || paymentRecord.TotalLocalAmount < 0)
                {
                    //
                    paymentRecord.IsReversed = true;
                    if (OriginalPaymentRecord != null)
                    {
                        paymentRecord.ReversedPaymentId = OriginalPaymentRecord.PaymentId;
                        paymentRecord.ReversedPaymentNo = OriginalPaymentRecord.PaymentNo;
                    }
                }

                paymentRecord.PayeeInfoID = entity.PayeeInfoID;
                paymentRecord.CustomerVendor_ID = entity.CustomerVendor_ID;
                paymentRecord.PayeeAccountNo = entity.PayeeAccountNo;
                paymentRecord.tb_FM_PaymentRecordDetails = new List<tb_FM_PaymentRecordDetail>();

                BusinessHelper.Instance.InitEntity(paymentRecord);
                //            paymentRecord.ReferenceNo=entity.no 流水
                paymentRecord.tb_FM_PaymentRecordDetails = NewDetails;
                if (SaveToDb)
                {
                    //自动提交
                    paymentRecord.PaymentStatus = (long)PaymentStatus.待审核;
                    var ctr = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    //比方 暂时没有供应商  又是外键，则是如何处理的？
                    bool vb = ShowInvalidMessage(ctr.Validator(paymentRecord));
                    if (!vb)
                    {
                        return new List<tb_FM_PaymentRecord>() ;
                    }

                    var paymentRecordController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                    ReturnMainSubResults<tb_FM_PaymentRecord> rsms = await paymentRecordController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord);
                    if (rsms.Succeeded)
                    {
                        paymentRecord.tb_FM_PaymentRecordDetails.ForEach(c => c.PaymentId = rsms.ReturnObject.PaymentId);
                    }
                    else
                    {
                        //记录错误日志
                        _logger.LogError(rsms.ErrorMsg);
                    }

                }
                else
                {
                    paymentRecord.PaymentStatus = (long)PaymentStatus.草稿;
                }

                PaymentRecords.Add(paymentRecord);
            }
            return PaymentRecords;
        }

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
                // MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentRecordController>(entity);
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApproval(List<tb_FM_PaymentRecord> entitys, ApprovalEntity approvalEntity)
        {
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
                        entity.PaymentStatus = (long)PaymentStatus.待审核;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PaymentRecord>(entity).ExecuteCommandAsync();
                    }
                }
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();

                //_logger.Info(approvalEntity.bizName + "审核事务成功");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                _logger.Error(approvalEntity.bizName + "事务回滚");
                return false;
            }

        }
        /// <summary>
        /// 批量结案  销售订单标记结案，数据状态为8, 
        /// 如果还没有出库。但是结案的订单时。修正拟出库数量。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_FM_PaymentRecord> entitys = new List<tb_FM_PaymentRecord>();
            entitys = NeedCloseCaseList as List<tb_FM_PaymentRecord>;

            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案
                //更新拟销售量  减少
                for (int m = 0; m < entitys.Count; m++)
                {
                    //判断 能结案的 是关闭的意思。就是没有收到款 作废
                    // 检查预付款取消
                    var preStatus = PrePaymentStatus.已生效 | PrePaymentStatus.部分核销;
                    bool hasRelated = true; // 存在核销单
                    bool canCancel = preStatus.CanCancel(hasRelated); // 返回false



                    if (entitys[m].PaymentStatus == (long)PaymentStatus.已冲销 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }

                    entitys[m].PaymentStatus = (long)PaymentStatus.已取消;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient()
                        .Updateable<tb_FM_PaymentRecord>(entitys[m])
                        .UpdateColumns(it => new
                        {
                            it.PaymentStatus,
                            it.ApprovalStatus,
                            it.ApprovalResults,
                            it.ApprovalOpinions,
                            it.PaymentImagePath,
                            it.Modified_by,
                            it.Modified_at,
                            it.Remark
                        }).ExecuteCommandAsync();
                }
                #endregion
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }

        }

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PaymentRecord> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PaymentRecord>()
                .Where(m => m.PaymentId == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .ToListAsync();
            return list as List<T>;
        }




    }

}



