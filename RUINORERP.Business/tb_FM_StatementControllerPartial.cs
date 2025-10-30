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
using System.Runtime.InteropServices.ComTypes;

namespace RUINORERP.Business
{
    /// <summary>
    /// 对账单
    /// </summary>
    public partial class tb_FM_StatementController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 对账单反审核方法
        /// 1. 检查状态是否允许反审核
        /// 2. 检查是否存在已支付的收付款单，如有则不允许反审
        /// 3. 删除相关的未审核收付款单
        /// 4. 回退应收付款单的已对账金额并允许其重新加入对账单
        /// 5. 更新对账单状态为已发送
        /// </summary>
        /// <param name="ObjectEntity">对账单实体</param>
        /// <returns>操作结果</returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_Statement entity = ObjectEntity as tb_FM_Statement;

            try
            {
                // 获取当前状态并验证是否允许反审核
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

                // 查找与当前对账单关联的收付款单
                var paymentRecordList = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.StatementId && d.SourceBizType == (int)BizType.对账单))
                            .ToListAsync();

                // 检查是否存在已支付的收付款单，如有则不允许反审
                if (paymentRecordList != null && paymentRecordList.Count > 0)
                {
                    if (paymentRecordList.Any(c => c.PaymentStatus == (int)PaymentStatus.已支付 && c.ApprovalStatus == (int)ApprovalStatus.已审核))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"存在【已支付】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单，反审失败。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else
                    {
                        // 删除未审核的相关收付款单
                        foreach (var item in paymentRecordList)
                        {
                            await _appContext.Db.DeleteNav<tb_FM_PaymentRecord>(item)
                                .Include(c => c.tb_FM_PaymentRecordDetails)
                                .ExecuteCommandAsync();
                        }
                    }
                }

                // 回退应收付款单的已对账金额并允许其重新加入对账单
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

                // 更新对账单状态为已发送
                entity.StatementStatus = (int)StatementStatus.已发送;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                // 更新对账单
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.StatementStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();

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
        /// 对账单审核方法
        /// 1. 验证对账单状态是否为已发送
        /// 2. 验证付款对账单是否有收款信息
        /// 3. 检查是否存在重复对账的应收付款单
        /// 4. 更新应收付款单的已对账金额并标记为不允许再加入对账单
        /// 5. 验证已对账金额不超过未核销金额
        /// 6. 更新对账单状态为已确认
        /// 注：此审核由业务人员执行，后续财务人员将审核收款单确认实际收付情况
        /// </summary>
        /// <param name="ObjectEntity">对账单实体</param>
        /// <param name="IsAutoApprove">是否自动审核</param>
        /// <returns>操作结果</returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity, bool IsAutoApprove = false)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_Statement entity = ObjectEntity as tb_FM_Statement;

            try
            {
                // 基础验证
                if (entity == null)
                {
                    rmrs.ErrorMsg = "无效的对账单单据";
                    return rmrs;
                }

                // 验证状态是否为已发送
                if (entity.StatementStatus != (int)StatementStatus.已发送)
                {
                    rmrs.ErrorMsg = "只有【已发送】待审核状态的对账单才可以审核";
                    return rmrs;
                }

                // 验证付款对账单是否有收款信息
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款 && !entity.PayeeInfoID.HasValue && !IsAutoApprove)
                {
                    rmrs.ErrorMsg = $"{entity.StatementNo}付款对账单时，对方的收款信息必填!";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                // 获取对账单明细中的应收付款单ID
                long[] arapIds = entity.tb_FM_StatementDetails.Select(m => m.ARAPId).ToArray();

                // 检查是否存在重复对账的应收付款单（已被标记为不允许再次加入对账单）
                var duplicateAddList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayable>()
                               .Where(it => it.AllowAddToStatement == false)
                               .Where(it => arapIds.Contains(it.ARAPId))
                               .ToListAsync();

                if (duplicateAddList.Count > 0)
                {
                    var noList = duplicateAddList.Select(m => m.ARAPNo).ToArray();
                    string arapNos = string.Join(",", noList);
                    rmrs.ErrorMsg = $"审核失败，{entity.StatementNo}对账单中的{arapNos}存在重复对账!请检查数据后再试！";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                // 检查内存中的对账单明细是否有重复的应收付款单
                var duplicateArapIds = entity.tb_FM_StatementDetails
                    .GroupBy(c => c.ARAPId)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateArapIds.Any())
                {
                    rmrs.ErrorMsg = $"对账单明细中，以下应收付款单存在重复对账：{string.Join(",", duplicateArapIds)}";
                    rmrs.Succeeded = false;
                    rmrs.ReturnObject = entity as T;
                    return rmrs;
                }

                // 确保对账单明细已加载
                if (entity.tb_FM_StatementDetails == null)
                {
                    entity.tb_FM_StatementDetails = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_StatementDetail>()
                        .Where(m => m.StatementId == entity.StatementId).ToListAsync();
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                // 更新应收付款单的已对账金额并标记为不允许再加入对账单
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

                // 更新对账单状态为已确认
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                entity.StatementStatus = (int)StatementStatus.已确认;

                BusinessHelper.Instance.ApproverEntity(entity);

                // 更新对账单
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
        /// <param name="paymentType">付款类型（收款/付款）</param>
        /// <param name="statementType">对账模式（余额对账/收款对账/付款对账）</param>
        /// <returns>生成的对账单结果</returns>
        public async Task<ReturnResults<tb_FM_Statement>> BuildStatement(List<tb_FM_ReceivablePayable> entities, ReceivePaymentType receivePaymentType, StatementType statementType)
        {
            ReturnResults<tb_FM_Statement> rmrs = new ReturnResults<tb_FM_Statement>();

            try
            {
                // 验证是否为同一客户/供应商
                var customerVendorIds = entities.Select(e => e.CustomerVendor_ID).Distinct().ToList();
                if (customerVendorIds.Count > 1)
                {
                    rmrs.ErrorMsg = "对账单只能针对同一客户/供应商生成";
                    rmrs.Succeeded = false;
                    return rmrs;
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
                statement.ReceivePaymentType = (int)receivePaymentType;
                statement.StatementType = (int)statementType;
                statement.CustomerVendor_ID = customerVendorId;
                statement.Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID.Value;
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
                    // 考虑到可能存在负数金额的情况（如销售退货、采购退货）
                    // 这里只检查计算逻辑是否正确，不直接限制金额必须为正数
                    if ((entity.LocalBalanceAmount - entity.LocalReconciledAmount) != detail.IncludedLocalAmount ||
                        (entity.ForeignBalanceAmount - entity.ForeignReconciledAmount) != detail.IncludedForeignAmount)
                    {
                        rmrs.ErrorMsg = $"应收付款单{entity.ARAPNo}的对账金额计算错误，请检查数据";
                        rmrs.Succeeded = false;
                        return rmrs;
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
                                                           // 期间收款总额：生成对账单时设置为零，后续通过收付款确认业务来更新
                statement.TotalReceivedLocalAmount = 0;
                // 期间付款总额：生成对账单时设置为零，后续通过收付款确认业务来更新
                statement.TotalPaidLocalAmount = 0;
                // 期间收款外币总额：生成对账单时设置为零，后续通过收付款确认业务来更新
                statement.TotalReceivedForeignAmount = 0;

                // 期间付款外币总额：生成对账单时设置为零，后续通过收付款确认业务来更新
                statement.TotalPaidForeignAmount = 0;
                //计算总金额
                CalculateTotalAmount(statement, details, receivePaymentType, statementType);

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
                    rmrs.ErrorMsg = $"对账单明细中，以下应收付款单存在重复对账：{string.Join(",", duplicateArapIds)}";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                statement.ARAPNos = string.Join(",", entities.Select(c => c.ARAPNo).ToArray());
                BusinessHelper.Instance.InitEntity(statement);
                statement.StatementStatus = (int)StatementStatus.草稿;

                rmrs.Succeeded = true;
                rmrs.ReturnObject = statement;
                return rmrs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "生成对账单失败");
                rmrs.ErrorMsg = ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }
        }


        /// <summary>
        /// 期末，期间金额进行计算
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="details"></param>
        /// <param name="receivePaymentType"></param>
        /// <param name="statementType"></param>
        /// <exception cref="Exception"></exception>
        public void CalculateTotalAmount(tb_FM_Statement statement, List<tb_FM_StatementDetail> details, ReceivePaymentType receivePaymentType, StatementType statementType)
        {
            // 计算期间收款和付款总额
            // 严格按照使用说明书的业务规则：
            // - 收款类型的数据保持正数
            // - 付款类型的数据直接使用原始值汇总，不使用绝对值
            statement.TotalReceivableLocalAmount = details
                .Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.收款)
                .Sum(c => c.IncludedLocalAmount);

            // 付款类型的数据直接使用原始值汇总，不使用绝对值
            // 确保正确处理采购退货等场景下的负数付款金额
            statement.TotalPayableLocalAmount = details
                .Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款)
                .Sum(c => c.IncludedLocalAmount);


            // 设置外币金额
            statement.TotalReceivableForeignAmount = details
                .Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.收款)
                .Sum(c => c.IncludedForeignAmount);

            statement.TotalPayableForeignAmount = details
                .Where(c => c.ReceivePaymentType == (int)ReceivePaymentType.付款)
                .Sum(c => c.IncludedForeignAmount);


            // 正确计算期末余额
            // 假如：不是余额对账时是单向的，则全部累加就是期末余额，下面会针对余额对账再次计算覆盖
            statement.ClosingBalanceLocalAmount = (statement.OpeningBalanceLocalAmount + statement.TotalReceivableLocalAmount + statement.TotalPayableLocalAmount);


            if (statementType == StatementType.余额对账)
            {
                #region  余额对账要特殊处理
                if (receivePaymentType == ReceivePaymentType.付款)
                {
                    //付款时，收进来算减去的金额
                    statement.TotalReceivableLocalAmount = -statement.TotalReceivableLocalAmount;
                    statement.ClosingBalanceLocalAmount = (statement.OpeningBalanceLocalAmount - statement.TotalReceivableLocalAmount +
                                                  statement.TotalPayableLocalAmount);

                    statement.Summary = ($"最终需要付款给供应商，金额：{statement.ClosingBalanceLocalAmount}（本币）");
                    statement.ReceivePaymentType = (int)ReceivePaymentType.付款;

                }
                else if (receivePaymentType == ReceivePaymentType.收款)
                {
                    statement.TotalPayableLocalAmount = -statement.TotalPayableLocalAmount;
                    //收款时，付出去的算减去的金额
                    statement.ClosingBalanceLocalAmount = (statement.OpeningBalanceLocalAmount + statement.TotalReceivableLocalAmount -
                                                  statement.TotalPayableLocalAmount);

                    statement.Summary = ($"客户需要付款给你方（应收），金额：{statement.ClosingBalanceLocalAmount}（本币）");
                    statement.ReceivePaymentType = (int)ReceivePaymentType.收款;
                }
                #endregion
            }

            statement.ClosingBalanceForeignAmount = 0; // 可根据需要扩展外币逻辑
            decimal netForeignAmount = statement.ClosingBalanceForeignAmount;

            if (netForeignAmount < 0 && statementType == StatementType.余额对账)
            {
                throw new Exception("计算错误。余额对账时，结果不会为负数。");
            }

            // 根据对账模式调整对账单类型和提示信息
            if (statementType == StatementType.收款对账)
            {
                // 收款对账模式：总是生成收款对账单
                // 调整期末余额为正数，确保与收款类型匹配
                statement.ClosingBalanceForeignAmount = Math.Abs(netForeignAmount);
                statement.Summary = ($"客户需要付款给你方（应收），金额：{statement.ClosingBalanceLocalAmount}（本币）");
                statement.ReceivePaymentType = (int)ReceivePaymentType.收款;
            }
            else if (statementType == StatementType.付款对账)
            {
                // 付款对账模式：总是生成付款对账单
                // 调整期末余额为正数，确保与付款类型匹配
                statement.ClosingBalanceForeignAmount = Math.Abs(netForeignAmount);
                statement.Summary = ($"最终需要付款给供应商，金额：{statement.ClosingBalanceLocalAmount}（本币）");
                statement.ReceivePaymentType = (int)ReceivePaymentType.付款;
            }

            // 当净额为0时的特殊提示
            if (statement.ClosingBalanceLocalAmount == 0)
            {
                statement.ClosingBalanceLocalAmount = 0; // 确保余额为0
                statement.ClosingBalanceForeignAmount = 0; // 确保外币余额为0
                statement.Summary = ("双方无欠款，金额已平。");
            }
        }


        /// <summary>
        /// 获取客户供应商的期初余额
        /// 计算逻辑：
        /// 1. 查询所有已审核未结的对账单（状态为部分结算或已确认）
        /// 2. 计算各金额字段的总和
        /// 3. 二次验证计算结果的准确性
        /// </summary>
        /// <param name="CustomerVendorIds">客户供应商Id数组</param>
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



