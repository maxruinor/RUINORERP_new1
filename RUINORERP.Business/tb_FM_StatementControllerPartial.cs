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
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.StatementId && d.SourceBizType == (int)BizType.对账单))
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

                //反审核了。对应的收款单，还能生成对账单。正常要删除才行。所以在审核时要判断重复性
                //对账单明细中的应收款单据 标识回写为[能再加入对账单]，并回退已对账金额
                foreach (var detail in entity.tb_FM_StatementDetails)
                {
                    var arap = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayable>()
                        .Where(a => a.ARAPId == detail.ARAPId)
                        .FirstAsync();

                    // 回退已对账金额
                    arap.LocalReconciledAmount -= detail.IncludedLocalAmount;
                    arap.ForeignReconciledAmount -= detail.IncludedForeignAmount;
                    arap.AllowAddToStatement = true;
                    
                    // 确保金额不会变为负数
                    arap.LocalReconciledAmount = Math.Max(0, arap.LocalReconciledAmount);
                    arap.ForeignReconciledAmount = Math.Max(0, arap.ForeignReconciledAmount);
                    
                    await _unitOfWorkManage.GetDbClient().Updateable(arap).ExecuteCommandAsync();
                }


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


                long[] arapids = entity.tb_FM_StatementDetails.Select(m => m.ARAPId).ToArray();
                //审核时，如果应收付款单中的状态不是【AllowAddToStatement==true】，则不能审核。重复了。
                var CheckDuplicateAddList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayable>()
                               .Where(it => it.AllowAddToStatement == false)
                               .Where(it => arapids.Contains(it.ARAPId))
                               .ToListAsync();

                if (CheckDuplicateAddList.Count > 0)
                {
                    var NoList = CheckDuplicateAddList.Select(m => m.ARAPNo).ToArray();
                    string ARAPNos = string.Join(",", NoList);
                    rmrs.ErrorMsg = $"审核失败，{entity.StatementNo}对账单中的{ARAPNos}存在重复对账!请检查数据后再试！";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }
                //生成时也检查了。内存处理。多处理一次也可以
                var duplicateArapIds = entity.tb_FM_StatementDetails
                .GroupBy(c => c.ARAPId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
                if (duplicateArapIds.Any())
                {
                    throw new Exception($"对账单明细中，以下应收付款单存在重复对账：{string.Join(",", duplicateArapIds)}");
                }



                if (entity.tb_FM_StatementDetails == null)
                {
                    entity.tb_FM_StatementDetails = new List<tb_FM_StatementDetail>();
                    entity.tb_FM_StatementDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_StatementDetail>().Where(m => m.StatementId == entity.StatementId).ToListAsync();
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //对账单明细中的应收款单据 标识回写为不能再加入对账单，并更新已对账金额
                foreach (var detail in entity.tb_FM_StatementDetails)
                {
                    var arap = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayable>()
                        .Where(a => a.ARAPId == detail.ARAPId)
                        .FirstAsync();

                    // 更新已对账金额
                    arap.LocalReconciledAmount += detail.IncludedLocalAmount;
                    arap.ForeignReconciledAmount += detail.IncludedForeignAmount;
                    arap.AllowAddToStatement = false;
                    
                    // 验证已对账金额不超过未核销金额
                    if (arap.LocalReconciledAmount > arap.LocalBalanceAmount || 
                        arap.ForeignReconciledAmount > arap.ForeignBalanceAmount)
                    {
                        throw new Exception($"应收付款单{arap.ARAPNo}的已对账金额超过未核销金额，请检查数据");
                    }
                    
                    await _unitOfWorkManage.GetDbClient().Updateable(arap).ExecuteCommandAsync();
                }


                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                entity.StatementStatus = (int)StatementStatus.已确认;


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



        /// <summary>
        /// 生成对账单
        /// </summary>
        /// <param name="entities">应收应付单列表</param>
        /// <param name="paymentType">付款类型</param>
        /// <returns>生成的对账单</returns>
        /// <exception cref="Exception"></exception>
        public async Task<tb_FM_Statement> BuildStatement(List<tb_FM_ReceivablePayable> entities, ReceivePaymentType paymentType)
        {
            // 验证是否为同一客户/供应商
            var customerVendorIds = entities.Select(e => e.CustomerVendor_ID).Distinct().ToList();
            if (customerVendorIds.Count > 1)
            {
                throw new Exception("对账单只能针对同一客户/供应商生成");
            }

            long customerVendorId = customerVendorIds.First();

            // 创建对账单基本信息
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
            statement.ReceivePaymentType = (int)paymentType;
            statement.CustomerVendor_ID = customerVendorId;

            // 生成对账单明细，考虑已对账金额
            List<tb_FM_StatementDetail> details = new List<tb_FM_StatementDetail>();

            foreach (var entity in entities)
            {
                var detail = mapper.Map<tb_FM_StatementDetail>(entity);
                detail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.待核销;

                // 使用未对账的余额（总余额减去已对账金额）
                detail.IncludedLocalAmount = entity.LocalBalanceAmount - entity.LocalReconciledAmount;
                detail.IncludedForeignAmount = entity.ForeignBalanceAmount - entity.ForeignReconciledAmount;

                // 验证金额是否合法
                if (detail.IncludedLocalAmount < 0 || detail.IncludedForeignAmount < 0)
                {
                    throw new Exception($"应收付款单{entity.ARAPNo}的已对账金额大于未核销金额，请检查数据");
                }

                detail.RemainingForeignAmount = detail.IncludedForeignAmount;
                detail.RemainingLocalAmount = detail.IncludedLocalAmount;
                detail.Summary = entity.Remark;
                details.Add(detail);
            }

            // 获取期初余额（按客户供应商和日期范围）
            DateTime startDate = entities.Min(c => c.BusinessDate).Value;
            statement.OpeningBalanceLocalAmount = await GetOpeningBalance(customerVendorIds.ToArray(), startDate);
            statement.OpeningBalanceForeignAmount = 0; // 可根据需要扩展外币逻辑

            // 计算期间收款和付款总额
            statement.TotalReceivableLocalAmount = details
                .Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.收款)
                .Sum(c => c.IncludedLocalAmount);
            
            statement.TotalPayableLocalAmount = details
                .Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款)
                .Sum(c => c.IncludedLocalAmount);
            
            // 期间收款和付款暂时设为0，实际应根据对账期间内的收付款记录计算
            statement.TotalReceivedLocalAmount = 0;
            statement.TotalPaidLocalAmount = 0;
            statement.TotalReceivableForeignAmount = 0;
            statement.TotalPayableForeignAmount = 0;
            statement.TotalReceivedForeignAmount = 0;
            statement.TotalPaidForeignAmount = 0;

            // 正确计算期末余额
            // 期末余额 = 期初余额 + 期间应收 - 期间应付 + 期间收款 - 期间付款
            statement.ClosingBalanceLocalAmount = statement.OpeningBalanceLocalAmount + 
                                                 statement.TotalReceivableLocalAmount - 
                                                 statement.TotalPayableLocalAmount +
                                                 statement.TotalReceivedLocalAmount - 
                                                 statement.TotalPaidLocalAmount;
            statement.ClosingBalanceForeignAmount = 0; // 可根据需要扩展外币逻辑

            // 计算净额并确定对账单类型
            decimal netAmount = statement.ClosingBalanceLocalAmount;
            if (netAmount > 0)
            {
                statement.Summary = ($"最终需要付款给供应商，金额：{netAmount}（本币）");
                statement.ReceivePaymentType = (int)ReceivePaymentType.付款;
            }
            else if (netAmount < 0)
            {
                statement.Summary = ($"供应商需要付款给你方（应收），金额：{Math.Abs(netAmount)}（本币）");
                statement.ReceivePaymentType = (int)ReceivePaymentType.收款;
            }
            else
            {
                statement.Summary = ("双方无欠款，金额已平。");
            }

            // 设置对账单基本信息
            statement.StatementNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.对账单);
            statement.StartDate = startDate;
            statement.EndDate = entities.Max(c => c.BusinessDate).Value;
            
            // 设置收款信息（如果有）
            if (entities.Count > 0 && entities[0].PayeeInfoID.HasValue)
            {
                statement.PayeeInfoID = entities[0].PayeeInfoID;
                statement.PayeeAccountNo = entities[0].PayeeAccountNo;
            }
            
            statement.tb_FM_StatementDetails = details;

            // 检查是否有重复的应收付款单
            var duplicateArapIds = statement.tb_FM_StatementDetails
                .GroupBy(c => c.ARAPId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            
            if (duplicateArapIds.Any())
            {
                throw new Exception($"对账单明细中，以下应收付款单存在重复对账：{string.Join(",", duplicateArapIds)}");
            }

            statement.ARAPNos = string.Join(",", entities.Select(c => c.ARAPNo).ToArray());
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
        /// <summary>
        /// 获取客户供应商的期初余额
        /// </summary>
        /// <param name="CustomerVendorIds">客户供应商Id</param>
        /// <param name="asOfDate">截至日期（默认为当前日期）</param>
        /// <returns>期初余额</returns>
        public async Task<decimal> GetOpeningBalance(long[] CustomerVendorIds, DateTime? asOfDate = null)
        {
            DateTime cutoffDate = asOfDate ?? DateTime.Now;
            
            //获取所有已审核未结的对账单，并且结束日期在截至日期之前
            List<tb_FM_Statement> list = await _appContext.Db.CopyNew().Queryable<tb_FM_Statement>()
                .Where(m => CustomerVendorIds.Contains(m.CustomerVendor_ID))
                .Where(m => m.StatementStatus == (int)StatementStatus.部分结算 || m.StatementStatus == (int)StatementStatus.已确认)
                .Where(m => m.EndDate < cutoffDate) // 只考虑截至日期之前的对账单
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
            
            //二次验证
            var ClosingBalance = OpeningBalanceLocalAmount + TotalReceivableLocalAmount - TotalPayableLocalAmount + TotalReceivedLocalAmount - TotalPaidLocalAmount;

            if (Math.Abs(ClosingBalance - ClosingBalanceLocalAmount) > 0.0001m) // 考虑小数精度问题
            {
                throw new Exception($"结存金额不一致！请检查数据。计算值:{ClosingBalance},验证值:{ClosingBalanceLocalAmount}");
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



