// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:13
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
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.StatusManagerService;
using RUINORERP.Global;
using RUINORERP.Business.BizMapperService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 对账单
    /// </summary>
    public partial class tb_FM_StatementController<T> : BaseController<T> where T : class
    {
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_Statement entity = ObjectEntity as tb_FM_Statement;

            try
            {

                // 获取当前状态
                var statusProperty = typeof(StatementStatus).Name;
                var currentStatus = (StatementStatus)Enum.ToObject(
                    typeof(StatementStatus),
                    entity.GetPropertyValue(statusProperty)
                );

                if (!FMPaymentStatusHelper.CanUnapprove(currentStatus, false))
                {
                    rmrs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}对账单不可以反审";
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //如果是应收已经有收款记录，则生成反向收款，否则直接删除应收
                //var Antiresult = await AntiApplyManualPaymentAllocation(entity, (ReceivePaymentType)entity.ReceivePaymentType, false, false);
                //if (!Antiresult)
                //{
                //    _unitOfWorkManage.RollbackTran();
                //    rmrs.ErrorMsg = $"应{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}款单反核销操作失败";
                //    return rmrs;
                //}

                //如果反抵扣预收付后 还有核销，则是由收款来的。则不能反审了。 
                #region 这里是以付款单为准，反审。暂时不用了

                var PaymentRecordlist = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.StatementId))
                              .ToListAsync();
                if (PaymentRecordlist != null && PaymentRecordlist.Count > 0)
                {
                    //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                    if ((PaymentRecordlist.Any(c => c.PaymentStatus == (int)PaymentStatus.已支付)
                        && PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"存在【已支付】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单，反审失败。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else
                    {
                        foreach (var item in PaymentRecordlist)
                        {
                            //删除对应的由应收付款单生成的收款单
                            await _appContext.Db.DeleteNav<tb_FM_PaymentRecord>(item)
                                .Include(c => c.tb_FM_PaymentRecordDetails)
                                .ExecuteCommandAsync();
                        }
                    }
                }
                #endregion

                entity.StatementStatus = (int)StatementStatus.已发送;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.StatementStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions
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



        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            return await ApprovalAsync(ObjectEntity, false);
        }

        /// <summary>
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付，这财务审核收款单前，还是可以反审的
        /// 审核通过时生成关联的收款/付款草稿单 财务核对是否到账。
        /// 核销记录：预收款抵扣应收款（正向核销）
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity, bool IsAutoApprove = false)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_Statement entity = ObjectEntity as tb_FM_Statement;
            try
            {
                if (entity == null)
                {
                    rmrs.ErrorMsg = "无效的对账单单据";
                    return rmrs;
                }

                //只有待审核状态才能进行
                if (entity.StatementStatus != (int)StatementStatus.已发送)
                {
                    rmrs.ErrorMsg = "只有【已发送】待审核状态的对账单才可以审核";
                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    //自动审核时不检测
                    if (!entity.PayeeInfoID.HasValue && !IsAutoApprove)
                    {
                        rmrs.ErrorMsg = $"{entity.StatementNo}付款对账单时，对方的收款信息必填!";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }


                // 开启事务，保证数据一致性

                _unitOfWorkManage.BeginTran();


                entity.StatementStatus = (int)StatementStatus.已确认;
                if (entity.tb_FM_StatementDetails == null)
                {
                    entity.tb_FM_StatementDetails = new List<tb_FM_StatementDetail>();
                    entity.tb_FM_StatementDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_StatementDetail>().Where(m => m.StatementId == entity.StatementId).ToListAsync();
                }

                //对账单明细中的应收款单据 标识回写为不能再加入对账单
                long[] arapids = entity.tb_FM_StatementDetails.Select(m => m.ARAPId).ToArray();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>()
                              .SetColumns(it => it.AllowAddToStatement == false)
                              .Where(it => arapids.Contains(it.ARAPId))
                              .ExecuteCommandAsync();


                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;


                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.StatementStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions,
                    it.PayeeInfoID,
                }).ExecuteCommandAsync();
                if (result <= 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    rmrs.ErrorMsg = "更新结果为零，请确保数据完整。请检查当前单据数据是否存在。";
                    return rmrs;
                }



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




        public async Task<tb_FM_Statement> BuildStatement(List<tb_FM_ReceivablePayable> entities, ReceivePaymentType receivePaymentType)
        {
            //通过应收 自动生成 对账单
            //如果应收付款单中，已经为部分付款，或可能是从预收付款单中核销了部分。所以这里生成时需要取未核销金额的应收付金额
            tb_FM_Statement statement = new tb_FM_Statement();
            statement.ApprovalResults = null;
            statement.ApprovalStatus = (int)ApprovalStatus.未审核;
            statement.Approver_at = null;
            statement.Approver_by = null;
            statement.PrintStatus = 0;
            statement.ActionStatus = ActionStatus.新增;
            statement.ApprovalOpinions = "";
            statement.Modified_at = null;
            statement.Modified_by = null;
            statement.ReceivePaymentType = (int)receivePaymentType;

            List<tb_FM_StatementDetail> details = mapper.Map<List<tb_FM_StatementDetail>>(entities);


            if (receivePaymentType == ReceivePaymentType.收款)
            {
                statement.StatementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.收款对账单);
            }
            else
            {
                statement.StatementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.付款对账单);
            }

            //entities明细中的创建时间的最大值，最小值来决定账单的开始结束日期
            statement.StartDate = entities.Min(c => c.Created_at).Value;
            statement.EndDate = entities.Max(c => c.Created_at).Value;
            //statement.EndDate = System.DateTime.Now;
            // statement.Currency_ID = statement.Currency_ID;

            //默认给第一个
            statement.PayeeInfoID = entities[0].PayeeInfoID;
            statement.CustomerVendor_ID = entities[0].CustomerVendor_ID;
            statement.PayeeAccountNo = entities[0].PayeeAccountNo;
            statement.tb_FM_StatementDetails = details;

            //应收的余额给到付款单。创建收款
            statement.OpeningBalanceForeignAmount = 0;
            statement.OpeningBalanceLocalAmount = await GetOpeningBalance(entities.Select(c => c.CustomerVendor_ID).ToArray());

            statement.TotalPayableForeignAmount = 0;
            statement.TotalPayableLocalAmount = details.Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款).Sum(c => c.IncludedLocalAmount);

            statement.TotalReceivableForeignAmount = 0;
            statement.TotalReceivableLocalAmount = details.Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.收款).Sum(c => c.IncludedLocalAmount);

            statement.ClosingBalanceForeignAmount = 0;
            statement.ClosingBalanceLocalAmount = statement.OpeningBalanceLocalAmount + statement.TotalReceivableLocalAmount - statement.TotalPayableLocalAmount;

            //在收款单明细中，不可以存在：一种应付下有两同的两个应收单。 否则这里会出错。
            var duplicateArapIds = statement.tb_FM_StatementDetails
            .GroupBy(c => c.ARAPId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
            if (duplicateArapIds.Any())
            {
                throw new Exception($"对账单明细中，以下应收付款单存在重复对账：{string.Join(",", duplicateArapIds)}");
            }
            statement.ARAPNos = string.Join(",", statement.tb_FM_StatementDetails.Select(t => t.ARAPId).ToArray());

            BusinessHelper.Instance.InitEntity(statement);

            statement.StatementStatus = (int)StatementStatus.草稿;
            return statement;

        }


        /// <summary>
        /// 获取客户供应商的历史未结账单
        /// 暂时只算本币
        /// </summary>
        /// <param name="CustomerVendorIds"></param>
        /// <param name="receivePaymentType"></param>
        /// <returns></returns>
        public async Task<decimal> GetOpeningBalance(long[] CustomerVendorIds)
        {
            List<tb_FM_Statement> list = await _appContext.Db.CopyNew().Queryable<tb_FM_Statement>()
                .Where(m => CustomerVendorIds.Contains(m.CustomerVendor_ID))
                .Where(m => m.StatementStatus == (int)StatementStatus.部分结算 || m.StatementStatus == (int)StatementStatus.已确认)
                            .Includes(a => a.tb_FM_StatementDetails, b => b.tb_fm_receivablepayable, c => c.tb_FM_ReceivablePayableDetails)
                            .ToListAsync();

            //期初
            var OpeningBalanceLocalAmount = list.Sum(c => c.OpeningBalanceLocalAmount);

            //应收
            var TotalReceivableLocalAmount = list.Sum(c => c.TotalReceivableLocalAmount);

            //应付
            var TotalPayableLocalAmount = list.Sum(c => c.TotalPayableLocalAmount);

            //收款
            var TotalReceivedLocalAmount = list.Sum(c => c.TotalReceivedLocalAmount);

            //付款
            var TotalPaidLocalAmount = list.Sum(c => c.TotalPaidLocalAmount);

            //结余
            var ClosingBalanceLocalAmount = list.Sum(c => c.ClosingBalanceLocalAmount);
            //期末余额=开期结存+期初结存-期初结存-期末结存

            //二次验证
            var ClosingBalance = OpeningBalanceLocalAmount + TotalReceivableLocalAmount - TotalPayableLocalAmount + TotalReceivedLocalAmount - TotalPaidLocalAmount;

            if (ClosingBalance != ClosingBalanceLocalAmount)
            {
                throw new Exception("结存金额不一致！请检查数据");
            }

            return ClosingBalance;
        }




        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_Statement> list = await _appContext.Db.CopyNew().Queryable<tb_FM_Statement>()
                .Where(m => m.StatementId == ID)
                            .Includes(a => a.tb_fm_account)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_FM_StatementDetails, b => b.tb_fm_receivablepayable, c => c.tb_FM_ReceivablePayableDetails)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                             .Includes(a => a.tb_FM_StatementDetails, b => b.tb_fm_receivablepayable, c => c.tb_FM_ReceivablePayableDetails, d => d.tb_proddetail, e => e.tb_prod, f => f.tb_unit)
                            .ToListAsync();
            return list as List<T>;
        }







    }
}



