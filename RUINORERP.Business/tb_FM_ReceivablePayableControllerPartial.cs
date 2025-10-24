
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:29
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
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.Business.CommService;
using AutoMapper;
using System.IO.IsolatedStorage;
using StackExchange.Redis;
using RUINORERP.Business.StatusManagerService;
using System.Drawing.Drawing2D;
using System.Collections;
using RUINORERP.Business.BizMapperService;
using ZXing;
using RUINORERP.Business.Processor;

namespace RUINORERP.Business
{
    /// <summary>
    /// 应收应付表
    /// </summary>
    public partial class tb_FM_ReceivablePayableController<T> : BaseController<T> where T : class
    {

        // 应收应付标记坏账
        //protected async Task MarkBadDebt()
        //{
        //    bool result = await Submit(ARAPStatus.坏账);
        //    if (result)
        //    {
        //        // 标记坏账后处理
        //    }
        //}
        /// <summary>
        /// 客户取消订单时，如果有订单，如果财务没有在他对应的收付单里审核前是可以反审的。否则只能通过红字机制处理。
        /// 应收应付 没有审核，没有生成付款单，也没有从预收付中抵扣时。即可 状态和 核销金额和 应收付金额一样时可以反审删除
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ReceivablePayable entity = ObjectEntity as tb_FM_ReceivablePayable;

            try
            {

                // 获取当前状态
                var statusProperty = typeof(ARAPStatus).Name;
                var currentStatus = (ARAPStatus)Enum.ToObject(
                    typeof(ARAPStatus),
                    entity.GetPropertyValue(statusProperty)
                );

                if (!FMPaymentStatusHelper.CanUnapprove(currentStatus, false))
                {
                    rmrs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单不可以反审";
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //如果是应收已经有收款记录，则生成反向收款，否则直接删除应收
                var Antiresult = await AntiApplyManualPaymentAllocation(entity, (ReceivePaymentType)entity.ReceivePaymentType, false, false);
                if (!Antiresult)
                {
                    _unitOfWorkManage.RollbackTran();
                    rmrs.ErrorMsg = $"应{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}款单反核销操作失败";
                    return rmrs;
                }

                //如果反抵扣预收付后 还有核销，则是由收款来的。则不能反审了。 
                #region 这里是以付款单为准，反审。暂时不用了

                var PaymentRecordlist = await _appContext.Db.Queryable<tb_FM_PaymentRecord>()
                            .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == entity.ARAPId))
                              .ToListAsync();
                if (PaymentRecordlist != null && PaymentRecordlist.Count > 0)
                {
                    //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                    if ((PaymentRecordlist.Any(c => c.PaymentStatus == (int)PaymentStatus.已支付)
                        && PaymentRecordlist.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"存在【已支付】的{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单，反审失败,请联系上级财务，或作退回处理。";
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

                entity.ARAPStatus = (int)ARAPStatus.待审核;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.ForeignPaidAmount,
                    it.LocalPaidAmount,
                    it.ForeignBalanceAmount,
                    it.LocalBalanceAmount,
                    it.ARAPStatus,
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
            tb_FM_ReceivablePayable entity = ObjectEntity as tb_FM_ReceivablePayable;
            try
            {
                if (entity == null)
                {
                    rmrs.ErrorMsg = "无效的应收应付单据";
                    return rmrs;
                }

                //只有待审核状态才能进行
                if (entity.ARAPStatus != (int)ARAPStatus.待审核)
                {
                    rmrs.ErrorMsg = "只有待审核状态的应收款单才可以审核";
                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    //自动审核时不检测
                    if (!entity.PayeeInfoID.HasValue && !IsAutoApprove)
                    {
                        rmrs.ErrorMsg = $"{entity.ARAPNo}付款时，对方的收款信息必填!";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }


                //检测往来单位要与来源业务要一致
                if (entity.SourceBillId.HasValue)
                {
                    if (entity.SourceBizType == (int)BizType.缴库单)
                    {
                        var PurEntry = await _appContext.Db.Queryable<tb_FinishedGoodsInv>()
                            .Where(c => c.FG_ID == entity.SourceBillId)
                           .SingleAsync();
                        if (!PurEntry.CustomerVendor_ID.Equals(entity.CustomerVendor_ID))
                        {
                            rmrs.ErrorMsg = "付款时必需与来源业务的往来单位相同!";
                            rmrs.Succeeded = false;
                            rmrs.ReturnObject = entity as T;
                            return rmrs;
                        }
                    }

                    if (entity.SourceBizType == (int)BizType.采购入库单)
                    {
                        var PurEntry = await _appContext.Db.Queryable<tb_PurEntry>()
                            .Where(c => c.PurEntryID == entity.SourceBillId)
                           .SingleAsync();
                        if (!PurEntry.CustomerVendor_ID.Equals(entity.CustomerVendor_ID))
                        {
                            rmrs.ErrorMsg = "付款时必需与来源业务的往来单位相同!";
                            rmrs.Succeeded = false;
                            rmrs.ReturnObject = entity as T;
                            return rmrs;
                        }
                    }
                    if (entity.SourceBizType == (int)BizType.采购退货单)
                    {
                        var PurEntryRe = await _appContext.Db.Queryable<tb_PurEntryRe>()
                    .Where(c => c.PurEntryRe_ID == entity.SourceBillId)
                   .SingleAsync();
                        if (!PurEntryRe.CustomerVendor_ID.Equals(entity.CustomerVendor_ID))
                        {
                            rmrs.ErrorMsg = "付款时必需与来源业务的往来单位相同!";
                            rmrs.Succeeded = false;
                            rmrs.ReturnObject = entity as T;
                            return rmrs;
                        }
                    }
                    if (entity.SourceBizType == (int)BizType.销售出库单)
                    {
                        var SaleOut = await _appContext.Db.Queryable<tb_SaleOut>()
                    .Where(c => c.SaleOut_MainID == entity.SourceBillId)
                   .SingleAsync(); if (!SaleOut.CustomerVendor_ID.Equals(entity.CustomerVendor_ID))
                        {
                            rmrs.ErrorMsg = "收款时必需与来源业务的往来单位相同!";
                            rmrs.Succeeded = false;
                            rmrs.ReturnObject = entity as T;
                            return rmrs;
                        }
                    }
                    if (entity.SourceBizType == (int)BizType.销售退回单)
                    {
                        var SaleOutRe = await _appContext.Db.Queryable<tb_SaleOutRe>()
                            .Where(c => c.SaleOutRe_ID == entity.SourceBillId)
                           .SingleAsync(); if (!SaleOutRe.CustomerVendor_ID.Equals(entity.CustomerVendor_ID))
                        {
                            rmrs.ErrorMsg = "收款时必需与来源业务的往来单位相同!";
                            rmrs.Succeeded = false;
                            rmrs.ReturnObject = entity as T;
                            return rmrs;
                        }
                    }
                }

                //得到当前实体对应的业务类型
                //var _mapper = _appContext.GetRequiredService<EnhancedBizTypeMapper>();
                //var bizType = _mapper.GetBizType(typeof(T), entity);
                //应收款中不能存在相同的来源的 正数金额的出库单的应收数据
                //一个出库不能多次应收。一个出库一个应收（负数除外）。一个应收可以多次收款来抵扣
                if (entity.SourceBizType.HasValue && entity.SourceBillId.HasValue)
                {
                    //审核时 要检测明细中对应的相同业务类型下不能有相同来源单号。除非有正负总金额为0对冲情况。或是两行数据?
                    var PendingApprovalReceivablePayable = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                        .Includes(c => c.tb_FM_ReceivablePayableDetails)
                    .Where(c => c.ARAPStatus >= (int)ARAPStatus.待支付)
                    .Where(c => c.SourceBizType == entity.SourceBizType)//相同业务来源的应收付款。是否有重复的检测
                    .ToListAsync();

                    //要把自己也算上。不能大于1,entity是等待审核。所以拼一起
                    PendingApprovalReceivablePayable.Add(entity);

                    if (!ValidatePaymentDetails(PendingApprovalReceivablePayable, rmrs))
                    {
                        //rmrs.ErrorMsg = "相同业务类型下不能有相同的来源单号!审核失败。";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }
                //出库入库时生成应收。。其它业务审核确认这个就生效。
                /*             
                1. 状态更新	将应收单状态从“未审核”改为“已审核”	应收单自身状态
                2. 财务凭证生成	根据应收单生成会计分录（如：借应收账款，贷销售收入）	总账模块、财务报表
                4. 预收款核销	若审核时触发预收款抵扣，需更新预收款单的核销状态及余额	预收款单、客户预收余额
                5. 业务单据回写	回写关联的销售出库单状态（如标记为“已生成应收并审核”）销售出库单状态跟踪
                6. 对账单生成	根据审核后的应收单生成客户对账单（可选）	客户对账流程
                8. 工作流触发	触发后续流程（如收款计划提醒、账期到期预警）	业务提醒与自动化
                 */

                // 开启事务，保证数据一致性

                entity.ARAPStatus = (int)ARAPStatus.待支付;

                //出库生成应付，应付审核时如果有预收付核销后应该是部分支付了。
                if (entity.LocalBalanceAmount == entity.TotalLocalPayableAmount)
                {
                    entity.ARAPStatus = (int)ARAPStatus.待支付;
                    entity.AllowAddToStatement = true;
                }
                else if (entity.TotalLocalPayableAmount == entity.LocalPaidAmount && entity.LocalBalanceAmount == 0)
                {
                    entity.ARAPStatus = (int)ARAPStatus.全部支付;
                    entity.AllowAddToStatement = false;
                }
                else
                {
                    entity.ARAPStatus = (int)ARAPStatus.部分支付;
                    entity.AllowAddToStatement = true;
                }


                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;

                entity.AllowAddToStatement = true;

                BusinessHelper.Instance.ApproverEntity(entity);

                _unitOfWorkManage.BeginTran();
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.AllowAddToStatement,
                    it.ForeignPaidAmount,
                    it.LocalPaidAmount,
                    it.ForeignBalanceAmount,
                    it.LocalBalanceAmount,
                    it.ARAPStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions,
                    it.PayeeInfoID,
                    it.Approver_at,
                    it.Approver_by
                }).ExecuteCommandAsync();
                if (result <= 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    rmrs.ErrorMsg = "更新结果为零，请确保数据完整。请检查当前单据数据是否存在。";
                    return rmrs;
                }

                if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款)
                {
                    if (_appContext.FMConfig.EnableAPAutoOffsetPrepay)
                    {
                        //去核销预收付表
                        bool rs = await ApplyAutoPaymentAllocation(entity, false);
                    }
                }
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    if (_appContext.FMConfig.EnableARAutoOffsetPreReceive)
                    {
                        //去核销预收付表
                        bool rs = await ApplyAutoPaymentAllocation(entity, false);
                    }
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
        /// 来源业务单等没有。是手工建的。则不进入这里检测
        /// </summary>
        /// <param name="ReceivablePayables"></param>
        /// <param name="returnResults"></param>
        /// <returns></returns>
        public static bool ValidatePaymentDetails(List<tb_FM_ReceivablePayable> ReceivablePayables, ReturnResults<T> returnResults = null)
        {
            // 按来源业务类型分组
            var groupedByBizType = ReceivablePayables
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

                    // 情况1：单条记录直接通过
                    if (items.Count == 1)
                        continue;

                    // 情况2：按交易方向分组（应收/应付）
                    var directionGroups = items
                        .GroupBy(d => d.ReceivePaymentType) // 应收/应付类型字段
                        .ToList();

                    bool hasError = false;
                    foreach (var directionGroup in directionGroups)
                    {
                        var dirItems = directionGroup.ToList();

                        // 子情况2.1：同一方向对冲（正负抵消）
                        if (dirItems.Count == 2)
                        {
                            decimal totalLocal = dirItems.Sum(i => i.LocalBalanceAmount);
                            decimal totalForeign = dirItems.Sum(i => i.ForeignBalanceAmount);
                            if (Math.Abs(totalLocal) < 0.001m && Math.Abs(totalForeign) < 0.001m)
                                continue; // 对冲成功
                        }
                        // 子情况2.2：单方向单条记录（允许不同方向共存）
                        else if (dirItems.Count == 1)
                        {
                            continue;
                        }

                        // 不满足上述条件则标记错误
                        hasError = true;
                        break;
                    }

                    // 情况3：混合方向（应收+应付）且各自合法
                    if (!hasError) continue;

                    // 错误处理
                    returnResults.ErrorMsg = $"业务来源：{(BizType)bizTypeGroup.Key}，单号:{billNoGroup.Key}\r\n"
                        + $"应{(ReceivePaymentType)ReceivablePayables[0].ReceivePaymentType}单已经存在，数据不能重复，请检查。";
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 检查选中的应收应付款单是否有未确认的预收付款单据
        /// </summary>
        /// <param name="receivablePayable">应收应付款单实体</param>
        /// <returns>包含检查结果和未确认单据编号列表的对象</returns>
        public async Task<BooleanWithDataListResult<string>> CheckUnconfirmedPrePaymentExists(tb_FM_ReceivablePayable receivablePayable)
        {
            BooleanWithDataListResult<string> result = new BooleanWithDataListResult<string>();
            result.DataList = new List<string>();

            try
            {
                if (receivablePayable == null)
                {
                    result.ErrorMsg = "无效的应收应付款单据";
                    return result;
                }

                // 查询与当前应收应付款单相关的未审核预收款/预付款单据
                var unconfirmedPrePayments = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                    .Where(p => p.CustomerVendor_ID == receivablePayable.CustomerVendor_ID) // 相同客户/供应商
                    .Where(p => p.Currency_ID == receivablePayable.Currency_ID) // 相同币种
                    .Where(p => p.ReceivePaymentType == receivablePayable.ReceivePaymentType) // 相同收付类型
                    .Where(p => p.ApprovalStatus != (int)ApprovalStatus.已审核) // 未审核状态
                    .Where(p => p.IsAvailable == true) // 可用状态
                    .OrderBy(p => p.PrePayDate) // 按时间排序
                    .ToListAsync();

                if (unconfirmedPrePayments.Any())
                {
                    // 如果存在未确认的预收付款单据，检查是否有业务关联
                    // 尝试通过来源单据建立关联
                    bool hasRelatedUnconfirmed = false;
                    List<string> unconfirmedBillNos = new List<string>();

                    foreach (var prePayment in unconfirmedPrePayments)
                    {
                        // 检查是否有业务关联：相同的来源单据或业务流程关联
                        bool isRelated = false;

                        // 1. 检查是否有直接关联的来源单据ID
                        if (receivablePayable.SourceBillId.HasValue && prePayment.SourceBillId.HasValue)
                        {
                            // 根据业务类型判断关联关系
                            if (receivablePayable.SourceBizType == (int)BizType.销售出库单 &&
                                prePayment.SourceBizType == (int)BizType.销售订单)
                            {
                                // 查询销售出库单对应的销售订单ID
                                var saleOut = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                                    .Where(s => s.SaleOut_MainID == receivablePayable.SourceBillId.Value)
                                    .FirstAsync();

                                if (saleOut != null && saleOut.SOrder_ID == prePayment.SourceBillId)
                                {
                                    isRelated = true;
                                }
                            }
                            else if (receivablePayable.SourceBizType == (int)BizType.采购入库单 &&
                                prePayment.SourceBizType == (int)BizType.采购订单)
                            {
                                // 查询采购入库单对应的采购订单ID
                                var purEntry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                                    .Where(p => p.PurEntryID == receivablePayable.SourceBillId.Value)
                                    .FirstAsync();

                                if (purEntry != null && purEntry.PurOrder_ID == prePayment.SourceBillId)
                                {
                                    isRelated = true;
                                }
                            }
                        }

                        //暂时一对一去核验
                        // 2. 如果没有直接的业务关联，但属于同一客户/供应商的未确认预收付款，也视为相关
                        //if (!isRelated && prePayment.LocalPrepaidAmount > 0)
                        //{
                        //    isRelated = true;
                        //}

                        if (isRelated)
                        {
                            hasRelatedUnconfirmed = true;
                            unconfirmedBillNos.Add("【"+prePayment.PreRPNO+"】");
                        }
                    }

                    if (hasRelatedUnconfirmed)
                    {
                        result.DataList = unconfirmedBillNos;
                        result.Succeeded = false;
                        result.ErrorMsg = $"存在未确认的{(receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款 ? "预收款" : "预付款")}单：{string.Join(", ", unconfirmedBillNos)}。请先确认这些预{(receivablePayable.ReceivePaymentType == (int)ReceivePaymentType.收款 ? "收款" : "付款")}单后再操作快捷收付款。";
                        return result;
                    }
                }

                // 没有未确认的相关预收付款单据
                result.Succeeded = true;
                result.DataList = new List<string>();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"检查未确认预收付款失败，应收应付款单ID: {receivablePayable.ARAPId}");
                result.Succeeded = false;
                result.DataList = new List<string>();
                result.ErrorMsg = "检查未确认预收付款时发生错误：" + ex.Message;
                return result;
            }
        }




        /// <summary>
        /// 维修工单中的维修物料
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_AS_RepairOrder entity)
        {
            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.SourceBillNo = entity.RepairOrderNo;
            payable.SourceBillId = entity.RepairOrderID;
            payable.SourceBizType = (int)BizType.维修工单;
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
            }
            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = MyCacheManager.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
                if (projectgroup != null && projectgroup.ToString() != "System.Object")
                {
                    if (projectgroup is tb_ProjectGroup pj)
                    {
                        entity.tb_projectgroup = pj;
                        payable.DepartmentID = pj.DepartmentID;
                    }
                }
                else
                {
                    //db查询
                    entity.tb_projectgroup = await _appContext.GetRequiredService<tb_ProjectGroupController<tb_ProjectGroup>>().BaseQueryByIdAsync(entity.ProjectGroup_ID);
                    payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
                }
            }

            //销售就是收款
            payable.ReceivePaymentType = (int)ReceivePaymentType.收款;
            payable.BusinessDate = entity.RepairStartDate;
            payable.DocumentDate = entity.Created_at.Value;

            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收款单);

            payable.Currency_ID = _appContext.BaseCurrency.Currency_ID;
            payable.ExchangeRate = 1;
            //if (entity.tb_saleorder.tb_paymentmethod.Paytype_Name == DefaultPaymentMethod.账期.ToString())
            //{
            //    if (entity.tb_customervendor.CustomerCreditDays.HasValue)
            //    {
            //        // 从销售出库日期开始计算到期日
            //        payable.DueDate = entity.OutDate.Date.AddDays(entity.tb_customervendor.CustomerCreditDays.Value).AddDays(1).AddTicks(-1);
            //    }
            //}
            payable.ExchangeRate = 1;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_AS_RepairOrderMaterialDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                //这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                var olditem = entity.tb_AS_RepairOrderMaterialDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID && c.RepairMaterialDetailID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.SubtotalTaxAmount;
                    details[i].Quantity = olditem.ShouldSendQty;
                    details[i].UnitPrice = olditem.UnitPrice;
                    details[i].Summary = olditem.Summary;
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.SubtotalTaxAmount;
                    if (details[i].TaxLocalAmount > 0)
                    {
                        details[i].IncludeTax = true;
                    }

                    details[i].LocalPayableAmount = olditem.SubtotalTransAmount;
                }
                details[i].ExchangeRate = 1;
                details[i].ActionStatus = ActionStatus.新增;


                details[i].Quantity = details[i].Quantity;
                details[i].TaxLocalAmount = details[i].TaxLocalAmount;
                details[i].LocalPayableAmount = details[i].UnitPrice.Value * details[i].Quantity.Value;
            }

            #region 单独加一行运算到明细中 默认退货不退运费,要的话,自己手动添加

            // 添加人工成本（关键部分）
            if (entity.LaborCost > 0)
            {
                details.Add(new tb_FM_ReceivablePayableDetail
                {
                    ProdDetailID = null,
                    //property = "人工费",
                    ExchangeRate = 1,
                    ExpenseDescription = "人工费",
                    UnitPrice = entity.LaborCost,
                    Quantity = 1,
                    TaxRate = 0.0m, // 假设运费税率为9%
                    TaxLocalAmount = 0,
                    Summary = "人工费",
                    LocalPayableAmount = entity.LaborCost
                });
            }

            #endregion

            payable.tb_FM_ReceivablePayableDetails = details;

            //本币时
            payable.LocalBalanceAmount = entity.TotalAmount;
            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = entity.TotalAmount;
            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }
            payable.Remark = $"售后申请单：{entity.ASApplyNo}对应的维修工单{entity.RepairOrderNo}的应付款";
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;
            return payable;

        }


        /// <summary>
        /// 创建为红字冲销 应收款单
        /// 即使相同客户的，也不能合并，一个销售出库，退货对应一个应收单。收付单和付款单才可以合并。
        /// 暂时不提供批量。要检查数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_SaleOutRe entity)
        {
            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.SourceBillNo = entity.ReturnNo;
            payable.SourceBillId = entity.SaleOutRe_ID;
            payable.SourceBizType = (int)BizType.销售退回单;
            payable.BusinessDate = entity.ReturnDate.Value;
            payable.DocumentDate = entity.Created_at.Value;
            payable.IsExpenseType = false;
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
            }
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = MyCacheManager.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
                if (projectgroup != null && projectgroup.ToString() != "System.Object")
                {
                    if (projectgroup is tb_ProjectGroup pj)
                    {
                        entity.tb_projectgroup = pj;
                        payable.DepartmentID = pj.DepartmentID;
                    }
                }
                else
                {
                    entity.tb_projectgroup = await _appContext.GetRequiredService<tb_ProjectGroupController<tb_ProjectGroup>>().BaseQueryByIdAsync(entity.ProjectGroup_ID);
                    payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
                }
            }

            payable.ReceivePaymentType = (int)ReceivePaymentType.收款;
            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收款单);
            payable.Currency_ID = entity.Currency_ID;
            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_SaleOutReDetails);

            foreach (var d in details)
            {
                var src = entity.tb_SaleOutReDetails.First(c => c.ProdDetailID == d.ProdDetailID);

                d.TaxRate = src.TaxRate;
                d.UnitPrice = Math.Abs(src.TransactionPrice);    // 保证正数
                d.Quantity = -Math.Abs(src.Quantity);            // 保证负数
                d.TaxLocalAmount = -Math.Abs(src.SubtotalTaxAmount);   // 保证负数
                d.LocalPayableAmount = d.UnitPrice.Value * d.Quantity.Value;         // 负金额

                d.ExchangeRate = entity.ExchangeRate;
                d.ActionStatus = ActionStatus.新增;
            }

            #region 单独处理运费
            if (entity.FreightIncome > 0)
            {
                payable.ShippingFee = -entity.FreightIncome;   // 运费应收，同样为负
            }
            #endregion

            payable.tb_FM_ReceivablePayableDetails = details;

            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                payable.ForeignBalanceAmount = -Math.Abs(entity.ForeignTotalAmount);
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = payable.ForeignBalanceAmount;
            }
            else
            {
                payable.LocalBalanceAmount = -Math.Abs(entity.TotalAmount);
                payable.LocalPaidAmount = 0;
                payable.TotalLocalPayableAmount = payable.LocalBalanceAmount;
            }
            //业务经办人
            if (entity.Employee_ID.HasValue)
            {
                payable.Employee_ID = entity.Employee_ID.Value;
            }

            payable.Remark = $"销售出库单：{entity.SaleOut_NO}对应的销售退回单{entity.ReturnNo}的红字应付款";
            if (payable.IsFromPlatform)
            {
                payable.Remark += " 平台单号:" + entity.PlatformOrderNo;
            }
            if (entity.OfflineRefund.HasValue && entity.OfflineRefund.Value)
            {
                payable.Remark += " 线下退款";
            }
            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;
            return payable;
        }

        /// <summary>
        ///  生成应收应付
        /// </summary>
        /// <param name="entity">返回应收付款单给UI进一步检查</param>
        /// <returns></returns>
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_FM_PriceAdjustment entity)
        {

            #region 创建应收款单

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ARAPStatus = (int)ARAPStatus.草稿;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.SourceBillNo = entity.AdjustNo;
            payable.SourceBillId = entity.AdjustId;
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
            {
                payable.SourceBizType = (int)BizType.销售价格调整单;
                payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收款单);
                payable.Remark = $"销售出库单后【价格调整单】{entity.SourceBillNo} 的应收款";
            }
            else
            {
                payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
                payable.SourceBizType = (int)BizType.采购价格调整单;
                payable.Remark = $"采购入库后【价格调整单】{entity.SourceBillNo} 的应付款";
            }

            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = MyCacheManager.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
                if (projectgroup != null && projectgroup.ToString() != "System.Object")
                {
                    if (projectgroup is tb_ProjectGroup pj)
                    {
                        entity.tb_projectgroup = pj;
                        payable.DepartmentID = pj.DepartmentID;
                    }
                }
                else
                {
                    //db查询
                    entity.tb_projectgroup = await _appContext.GetRequiredService<tb_ProjectGroupController<tb_ProjectGroup>>().BaseQueryByIdAsync(entity.ProjectGroup_ID);
                    payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
                }
            }
            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = MyCacheManager.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
                if (projectgroup != null && projectgroup.ToString() != "System.Object")
                {
                    if (projectgroup is tb_ProjectGroup pj)
                    {
                        entity.tb_projectgroup = pj;
                        payable.DepartmentID = pj.DepartmentID;
                    }
                }
                else
                {
                    //db查询
                    entity.tb_projectgroup = await _appContext.GetRequiredService<tb_ProjectGroupController<tb_ProjectGroup>>().BaseQueryByIdAsync(entity.ProjectGroup_ID);
                    payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
                }
            }
            payable.BusinessDate = entity.AdjustDate;
            payable.DocumentDate = entity.Created_at.Value;
            payable.Currency_ID = entity.Currency_ID;
            var obj = MyCacheManager.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
            if (obj != null && obj.ToString() != "System.Object")
            {
                if (obj is tb_CustomerVendor cv)
                {
                    entity.tb_customervendor = cv;
                }
            }
            else
            {
                //db查询
                entity.tb_customervendor = await _appContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>().BaseQueryByIdAsync(entity.CustomerVendor_ID);
            }

            if (entity.tb_customervendor.CustomerCreditDays.HasValue)
            {
                // 从调整日期开始计算到期日
                //应收账期 的到期时间
                payable.DueDate = entity.AdjustDate.AddDays(entity.tb_customervendor.CustomerCreditDays.Value).AddDays(1).AddTicks(-1);
            }


            payable.ExchangeRate = 1;


            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_FM_PriceAdjustmentDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                //这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                var olditem = entity.tb_FM_PriceAdjustmentDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID && c.AdjustDetailID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.Correct_TaxRate;
                    details[i].TaxLocalAmount = olditem.TaxAmount_Diff;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.UnitPrice_WithTax_Diff;
                    details[i].LocalPayableAmount = olditem.UnitPrice_WithTax_Diff * olditem.Quantity;
                }
                details[i].Summary = entity.AdjustReason;
                details[i].ActionStatus = ActionStatus.新增;


            }

            payable.tb_FM_ReceivablePayableDetails = details;


            //本币时
            payable.LocalBalanceAmount = entity.TotalLocalDiffAmount;
            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = entity.TotalLocalDiffAmount;

            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }


            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;

            #endregion

            return payable;
        }


        /// <summary>
        /// 执行坏账处理
        /// </summary>
        /// <param name="reason">坏账原因</param>
        /// <param name="approverId">审批人ID</param>
        public async Task<ReturnResults<tb_FM_ReceivablePayable>> WriteOffBadDebt(tb_FM_ReceivablePayable receivablePayable, string reason)
        {

            ReturnResults<tb_FM_ReceivablePayable> returnResults = new Business.ReturnResults<tb_FM_ReceivablePayable>();

            // 0. 验证
            if (receivablePayable == null)
                throw new Exception("无可处理数据");

            // 1. 验证未核销金额
            if (receivablePayable.ForeignBalanceAmount == 0 && receivablePayable.LocalBalanceAmount == 0)
                throw new Exception("无未核销金额，无法进行坏账处理");
            try
            {
                // 2. 更新状态信息
                receivablePayable.Remark = $"[坏账处理]原因：{reason};处理时间：{DateTime.Now}";
                BusinessHelper.Instance.EditEntity(receivablePayable);
                receivablePayable.ARAPStatus = (int)ARAPStatus.坏账;

                // 3. 核销记录
                receivablePayable.ForeignBalanceAmount = 0;
                receivablePayable.LocalBalanceAmount = 0;
                // 3. 生成核销记录
                var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                await settlementController.GenerateSettlement(receivablePayable, receivablePayable.LocalBalanceAmount, receivablePayable.ForeignBalanceAmount);

                //自动添加到损失费用单
                var ctrProfitloss = _appContext.GetRequiredService<tb_FM_ProfitLossController<tb_FM_ProfitLoss>>();
                tb_FM_ProfitLoss profitLoss = ctrProfitloss.BuildProfitLoss(receivablePayable);
                var rmr = await ctrProfitloss.BaseSaveOrUpdateWithChild<tb_FM_ProfitLoss>(profitLoss);
                if (rmr.Succeeded)
                {
                    var Auditresults = await ctrProfitloss.ApprovalAsync(rmr.ReturnObject);
                    if (!Auditresults.Succeeded)
                    {
                        _logger.LogError($"损失费用单审核失败{Auditresults.ErrorMsg}");
                    }
                }

                // 4. 核销金额
                receivablePayable.ForeignPaidAmount += receivablePayable.ForeignBalanceAmount;
                receivablePayable.LocalPaidAmount += receivablePayable.LocalBalanceAmount;
                receivablePayable.ForeignBalanceAmount = 0;
                receivablePayable.LocalBalanceAmount = 0;

                //只更新指定列
                var updateResult = await _unitOfWorkManage.GetDbClient().Updateable(receivablePayable).UpdateColumns(it => new
                {
                    it.ForeignPaidAmount,
                    it.LocalPaidAmount,
                    it.ForeignBalanceAmount,
                    it.LocalBalanceAmount,
                    it.ARAPStatus,
                    it.Remark,
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                returnResults.Succeeded = true;
                returnResults.ReturnObject = receivablePayable as tb_FM_ReceivablePayable;
                return returnResults;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(receivablePayable));
                returnResults.ErrorMsg = ex.Message;
                return returnResults;
            }

        }


        #region 从预收付款中正向抵扣 [反审]

        /// <summary>
        /// 预收付款抵扣-的反操作
        /// 这个反审有一个类型是 来自销售出库，采购入库的反操作，因为都是自动生成。他们的反操作时要删除。不然会重复生成。
        /// 如果这反审是应收应付的财务反审，则不需要删除。会再次修改审核，所以这里需要一个开关
        /// 这个反操作，就是撤销预付的抵扣,如果指定的撤销对象可以不用一一对应，否则要一一对应
        /// </summary>
        /// <param name="AutoAntiApproval">如果自动审核为真时，则会删除应收付款单据，否则保存用于人工修改</param>
        /// <param name="preReceivedPayments">撤销时，指定了预收付时则不用一一对应，否则要一一对应</param>
        /// <param name="UseTransaction">外层有事务时这里传false</param>
        public async Task<bool> AntiApplyManualPaymentAllocation(tb_FM_ReceivablePayable payable, ReceivePaymentType paymentType,
           bool AutoAntiApproval, bool UseTransaction = true)
        {
            bool result = false;
            try
            {
                decimal remainingLocal = payable.LocalBalanceAmount; // 本币要支付的金额
                decimal remainingForeign = payable.ForeignBalanceAmount; // 外币要支付的金额
                if (UseTransaction)
                {
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                }

                #region 反写预收付款

                List<tb_FM_ReceivablePayable> arapupdatelist = new List<tb_FM_ReceivablePayable>();
                List<tb_FM_PreReceivedPayment> prePaymentsUpdateList = new List<tb_FM_PreReceivedPayment>();
                //产生两个相同出库单号的应付，只有退款 红字 一正一负

                if (payable.ARAPStatus == (int)ARAPStatus.草稿
                    || payable.ARAPStatus == (int)ARAPStatus.待审核)
                {
                    if (AutoAntiApproval)
                    {
                        await _appContext.Db.DeleteNav(payable).Include(c => c.tb_FM_ReceivablePayableDetails).ExecuteCommandAsync();
                    }
                    //这里还要判断 是否有付款单，还是只有预收付的核销。有付款单则用红字?
                }
                else
                {
                    //根据不同情况处理： 如果没有收款记录，
                    #region 如果有核销过，或部分支付
                    //通过核销记录来处理
                    #region 通过核销记录来处理 如果核销记录中是来自预收付的 则恢复，如果是来自收款单的则无法直接反审出库单。

                    //反核销  再修改应收付本身金额及状态  以核销记录为标准，因为预收付可以弄一半。核销是整单处理
                    var Settlements = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentSettlement>()
                         .Where(x => (x.TargetBillId == payable.ARAPId || x.SourceBillId == payable.ARAPId))
                         .Where(x => x.ReceivePaymentType == payable.ReceivePaymentType)
                         .Where(x => x.IsReversed == false && x.isdeleted == false)//正向的
                            .ToListAsync();
                    foreach (var Settlement in Settlements)
                    {
                        if (paymentType == ReceivePaymentType.收款)
                        {
                            //预收转应收时
                            if (Settlement.SourceBizType == (int)BizType.预收款单 && Settlement.TargetBizType == (int)BizType.应收款单)
                            {
                                // 要把抵扣掉的预付的 恢复回去，这里恢复是反操作。查询条件是按主键来的。只会一条数据
                                var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                         .Where(x => x.PreRPID == Settlement.SourceBillId).SingleAsync();
                                //    .Where(x => (x.PrePaymentStatus == (int)PrePaymentStatus.全额核销 || x.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                                //&& x.CustomerVendor_ID == entity.CustomerVendor_ID
                                //&& x.Currency_ID == entity.Currency_ID
                                //&& x.ReceivePaymentType == payable.ReceivePaymentType
                                //&& x.PreRPID == Settlement.SourceBillId
                                //).ToListAsync();
                                if (prePayment != null)
                                {
                                    prePayment.ForeignBalanceAmount += Settlement.SettledForeignAmount;
                                    prePayment.LocalBalanceAmount += Settlement.SettledLocalAmount;
                                    prePayment.LocalPaidAmount -= Settlement.SettledLocalAmount;
                                    prePayment.ForeignPaidAmount -= Settlement.SettledForeignAmount;
                                    if (prePayment.LocalPaidAmount == 0)
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.待核销;
                                    }
                                    else if (prePayment.LocalPaidAmount < prePayment.LocalBalanceAmount)
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                                    }
                                    payable.LocalBalanceAmount += Settlement.SettledLocalAmount;
                                    payable.ForeignBalanceAmount += Settlement.SettledForeignAmount;
                                    payable.ForeignPaidAmount -= Settlement.SettledForeignAmount;
                                    payable.LocalPaidAmount -= Settlement.SettledLocalAmount;
                                    prePaymentsUpdateList.Add(prePayment);

                                    //Settlement 逻辑删除
                                    Settlement.isdeleted = true;
                                    await _unitOfWorkManage.GetDbClient().Updateable(Settlement).UpdateColumns(it => new
                                    {
                                        it.isdeleted
                                    }).ExecuteCommandAsync();
                                }
                            }
                        }

                    }

                    if (payable.LocalPaidAmount == 0)
                    {
                        payable.ARAPStatus = (int)ARAPStatus.待审核;
                        payable.AllowAddToStatement = false;
                    }
                    else if (payable.LocalPaidAmount < payable.LocalBalanceAmount)
                    {
                        payable.ARAPStatus = (int)ARAPStatus.部分支付;
                        payable.AllowAddToStatement = true;
                    }

                    #endregion

                    //要根据他核销的是预收的。还是收款单的 分别处理
                    switch (payable.ARAPStatus.Value)
                    {
                        case (int)ARAPStatus.全部支付:
                        case (int)ARAPStatus.部分支付:
                        case (int)ARAPStatus.坏账:
                        case (int)ARAPStatus.已冲销:
                            if (UseTransaction)
                            {
                                _unitOfWorkManage.RollbackTran();
                            }
                            string msg = $"应{(ReceivePaymentType)payable.ReceivePaymentType}单状态为【{((ARAPStatus)payable.ARAPStatus.Value).ToString()}】，无法反核销。请联系管理员";
                            _logger.Debug(msg);
                            break;
                        case (int)ARAPStatus.草稿:
                        case (int)ARAPStatus.待审核:
                        case (int)ARAPStatus.待支付:
                            //如果有全額预付则这里可能是全部支付也会来自预收款了。
                            //这种可能预收核销过。或没有支付过
                            //先把核销的记录恢复，再删除核销记录
                            if (payable.LocalBalanceAmount == payable.TotalLocalPayableAmount)
                            {
                                //没有核销过时。直接删除
                                if (AutoAntiApproval)
                                {
                                    await _appContext.Db.DeleteNav(payable).Include(c => c.tb_FM_ReceivablePayableDetails).ExecuteCommandAsync();
                                }

                            }
                            else
                            {
                                //正常不会到这里的。
                                if (UseTransaction)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                }
                                string errormsg = $"应{(ReceivePaymentType)payable.ReceivePaymentType}单状态为【{payable.ARAPStatus}】未核销金额为{payable.LocalBalanceAmount}，核销金额为{payable.LocalPaidAmount}，无法反核销";

                                _logger.Debug(errormsg);
                            }
                            break;
                    }

                    //回写原始业务。因为全额预付会修改业务单据为全部付款


                    #endregion

                }
                arapupdatelist.Add(payable);

                if (prePaymentsUpdateList.Any())
                {
                    var resultprePayment = await _unitOfWorkManage.GetDbClient().Updateable(prePaymentsUpdateList).UpdateColumns(it => new
                    {
                        it.ForeignPaidAmount,
                        it.LocalPaidAmount,
                        it.ForeignBalanceAmount,
                        it.LocalBalanceAmount,
                        it.PrePaymentStatus,
                    }).ExecuteCommandAsync();
                }
                if (arapupdatelist.Any())
                {
                    var resultprePayment = await _unitOfWorkManage.GetDbClient().Updateable(arapupdatelist).UpdateColumns(it => new
                    {
                        it.ForeignPaidAmount,
                        it.LocalPaidAmount,
                        it.ForeignBalanceAmount,
                        it.LocalBalanceAmount,
                        it.ARAPStatus,
                    }).ExecuteCommandAsync();
                }
                #endregion

                #region 更新原来业务单据
                switch (payable.SourceBizType)
                {
                    case (int)BizType.销售出库单:

                        var saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                            .Includes(c => c.tb_saleorder)
                            .Includes(c => c.tb_SaleOutDetails)
                         .Where(x => (x.SaleOut_MainID == payable.SourceBillId))
                         .SingleAsync();


                        if (saleout.tb_saleorder.Deposit > 0 && saleout.PayStatus != (int)PayStatus.部分预付)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.部分预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.部分付款)
                            {
                                saleout.PayStatus = (int)PayStatus.部分预付;
                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            }
                        }

                        //要验证  感觉有问题
                        if (saleout.tb_saleorder.Deposit == 0 && saleout.PayStatus != (int)PayStatus.全额预付)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.全额预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.全部付款)
                            {
                                saleout.PayStatus = (int)PayStatus.全额预付;
                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            }
                        }
                        //要验证 感觉有问题
                        if (saleout.tb_saleorder.Deposit == 0 && saleout.PayStatus != (int)PayStatus.未付款)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.未付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.未付款)
                            {
                                saleout.PayStatus = (int)PayStatus.未付款;
                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            }
                        }

                        break;
                    case (int)BizType.采购入库单:

                        var purentry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_purorder)
                            .Includes(c => c.tb_PurEntryDetails)
                         .Where(x => (x.PurEntryID == payable.SourceBillId))
                         .SingleAsync();

                        if (payable.LocalPaidAmount < purentry.TotalAmount && purentry.PayStatus != (int)PayStatus.部分预付)
                        {
                            purentry.PayStatus = (int)PayStatus.部分预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (payable.LocalPaidAmount < purentry.TotalAmount && purentry.TotalAmount < purentry.tb_purorder.TotalAmount)
                        {
                            purentry.tb_purorder.PayStatus = (int)PayStatus.部分预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry.tb_purorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        if (payable.LocalPaidAmount == purentry.TotalAmount && purentry.PayStatus != (int)PayStatus.全额预付)
                        {
                            purentry.PayStatus = (int)PayStatus.全额预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (payable.LocalPaidAmount == purentry.TotalAmount && purentry.TotalAmount == purentry.tb_purorder.TotalAmount)
                        {
                            purentry.tb_purorder.PayStatus = (int)PayStatus.全额预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry.tb_purorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        break;
                }
                #endregion

                //只更新指定列
                var updateResult = await _unitOfWorkManage.GetDbClient().Updateable(payable).UpdateColumns(it => new
                {
                    it.ForeignPaidAmount,
                    it.LocalPaidAmount,
                    it.ForeignBalanceAmount,
                    it.LocalBalanceAmount,
                    it.ARAPStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                if (UseTransaction)
                {
                    _unitOfWorkManage.CommitTran();
                }
                result = true;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    _unitOfWorkManage.RollbackTran();
                    _logger.Error(ex, EntityDataExtractor.ExtractDataContent(payable));
                }
                result = false;
            }
            return result;
        }





        #endregion


        #region 撤销 从预收付款中正向的抵扣金额

        /// <summary>
        /// 这个反审有一个类型是 来自销售出库，采购入库的反操作，因为都是自动生成。他们的反操作时要删除。不然会重复生成。
        /// 如果这反审是应收应付的财务反审，则不需要删除。会再次修改审核，所以这里需要一个开关
        /// 这个反操作，就是撤销预付的抵扣,如果指定的撤销对象可以不用一一对应，否则要一一对应
        /// </summary>
        /// <param name="PrePayments">撤销时，指定了预收付时则不用一一对应，否则要一一对应</param>
        /// <param name="UseTransaction">外层有事务时这里传false</param>
        public async Task<bool> RevokeApplyManualPaymentAllocation(tb_FM_ReceivablePayable payable,
        List<KeyValuePair<tb_FM_PreReceivedPayment, tb_FM_PaymentSettlement>> PrePaySettlements, bool UseTransaction = true)
        {
            //应收付状态降级
            //预收付状态变化
            //金额变化
            bool result = false;
            try
            {
                decimal remainingLocal = payable.LocalBalanceAmount; // 本币要支付的金额
                decimal remainingForeign = payable.ForeignBalanceAmount; // 外币要支付的金额
                if (UseTransaction)
                {
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                }

                #region 反写预收付款

                List<tb_FM_ReceivablePayable> arapupdatelist = new List<tb_FM_ReceivablePayable>();
                List<tb_FM_PreReceivedPayment> prePaymentsUpdateList = new List<tb_FM_PreReceivedPayment>();
                //产生两个相同出库单号的应付，只有退款 红字 一正一负

                if (payable.ARAPStatus == (int)ARAPStatus.草稿
                    || payable.ARAPStatus == (int)ARAPStatus.待审核)
                {

                    throw new Exception("应收款状态错误，不能进行撤销抵扣");
                }
                else
                {
                    //根据不同情况处理： 如果没有收款记录，
                    #region 如果有核销过，或部分支付
                    //通过核销记录来处理
                    #region 通过核销记录来处理 如果核销记录中是来自预收付的 则恢复，如果是来自收款单的则无法直接反审出库单。

                    foreach (var item in PrePaySettlements)
                    {
                        if (payable.ReceivePaymentType == (int)ReceivePaymentType.收款)
                        {
                            //预收转应收时
                            if (payable.SourceBizType == (int)BizType.销售出库单)
                            {
                                // 要把抵扣掉的预付的 恢复回去，这里恢复是反操作。查询条件是按主键来的。只会一条数据
                                var prePayment = item.Key as tb_FM_PreReceivedPayment;
                                var Settlement = item.Value as tb_FM_PaymentSettlement;
                                if (prePayment != null)
                                {
                                    prePayment.ForeignBalanceAmount += Settlement.SettledForeignAmount;
                                    prePayment.LocalBalanceAmount += Settlement.SettledLocalAmount;
                                    prePayment.LocalPaidAmount -= Settlement.SettledLocalAmount;
                                    prePayment.ForeignPaidAmount -= Settlement.SettledForeignAmount;
                                    if (prePayment.LocalPaidAmount == 0)
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.待核销;
                                    }
                                    else if (prePayment.LocalPaidAmount < prePayment.LocalPrepaidAmount)
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                                    }
                                    payable.LocalBalanceAmount += Settlement.SettledLocalAmount;
                                    payable.ForeignBalanceAmount += Settlement.SettledForeignAmount;
                                    payable.ForeignPaidAmount -= Settlement.SettledForeignAmount;
                                    payable.LocalPaidAmount -= Settlement.SettledLocalAmount;
                                    prePaymentsUpdateList.Add(prePayment);

                                    //Settlement 逻辑删除
                                    Settlement.isdeleted = true;
                                    await _unitOfWorkManage.GetDbClient().Updateable(Settlement).UpdateColumns(it => new
                                    {
                                        it.isdeleted
                                    }).ExecuteCommandAsync();
                                }

                                //更新预收款对应的业务单据：销售订单
                                if (prePayment.SourceBizType == (int)BizType.销售订单)
                                {
                                    var SaleOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                                           .Includes(c => c.tb_SaleOuts)
                                        .Where(x => (x.SOrder_ID == prePayment.SourceBillId))
                                        .SingleAsync();

                                    if (SaleOrder.Deposit > 0 && SaleOrder.PayStatus != (int)PayStatus.部分预付)
                                    {
                                        SaleOrder.PayStatus = (int)PayStatus.部分预付;
                                        await _unitOfWorkManage.GetDbClient().Updateable(SaleOrder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                                        foreach (var saleout in SaleOrder.tb_SaleOuts)
                                        {
                                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.部分付款)
                                            {
                                                saleout.PayStatus = (int)PayStatus.部分预付;
                                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                                            }
                                        }

                                    }

                                    //要验证
                                    if (SaleOrder.Deposit == 0 && SaleOrder.PayStatus != (int)PayStatus.全额预付)
                                    {
                                        SaleOrder.PayStatus = (int)PayStatus.全额预付;
                                        await _unitOfWorkManage.GetDbClient().Updateable(SaleOrder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();

                                        foreach (var saleout in SaleOrder.tb_SaleOuts)
                                        {
                                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.全部付款)
                                            {
                                                saleout.PayStatus = (int)PayStatus.全额预付;
                                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (payable.ReceivePaymentType == (int)ReceivePaymentType.付款)
                        {
                            //预收转应收时
                            if (payable.SourceBizType == (int)BizType.采购入库单)
                            {
                                // 要把抵扣掉的预付的 恢复回去，这里恢复是反操作。查询条件是按主键来的。只会一条数据
                                var prePayment = item.Key as tb_FM_PreReceivedPayment;
                                var Settlement = item.Value as tb_FM_PaymentSettlement;
                                if (prePayment != null)
                                {
                                    prePayment.ForeignBalanceAmount += Settlement.SettledForeignAmount;
                                    prePayment.LocalBalanceAmount += Settlement.SettledLocalAmount;
                                    prePayment.LocalPaidAmount -= Settlement.SettledLocalAmount;
                                    prePayment.ForeignPaidAmount -= Settlement.SettledForeignAmount;
                                    if (prePayment.LocalPaidAmount == 0)
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.待核销;
                                    }
                                    else if (prePayment.LocalPaidAmount < prePayment.LocalPrepaidAmount)
                                    {
                                        prePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                                    }
                                    payable.LocalBalanceAmount += Settlement.SettledLocalAmount;
                                    payable.ForeignBalanceAmount += Settlement.SettledForeignAmount;
                                    payable.ForeignPaidAmount -= Settlement.SettledForeignAmount;
                                    payable.LocalPaidAmount -= Settlement.SettledLocalAmount;
                                    prePaymentsUpdateList.Add(prePayment);

                                    //Settlement 逻辑删除
                                    Settlement.isdeleted = true;
                                    await _unitOfWorkManage.GetDbClient().Updateable(Settlement).UpdateColumns(it => new
                                    {
                                        it.isdeleted
                                    }).ExecuteCommandAsync();
                                }

                                //更新预收款对应的业务单据：销售订单
                                if (prePayment.SourceBizType == (int)BizType.采购订单)
                                {
                                    var PurOrder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                           .Includes(c => c.tb_PurEntries)
                                        .Where(x => (x.PurOrder_ID == prePayment.SourceBillId))
                                        .SingleAsync();

                                    if (PurOrder.Deposit > 0 && PurOrder.PayStatus != (int)PayStatus.部分预付)
                                    {
                                        PurOrder.PayStatus = (int)PayStatus.部分预付;
                                        await _unitOfWorkManage.GetDbClient().Updateable(PurOrder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                                        foreach (var purEntry in PurOrder.tb_PurEntries)
                                        {
                                            if (purEntry.TotalAmount == purEntry.TotalAmount && purEntry.PayStatus == (int)PayStatus.部分付款)
                                            {
                                                purEntry.PayStatus = (int)PayStatus.部分预付;
                                                await _unitOfWorkManage.GetDbClient().Updateable(purEntry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                                            }
                                        }

                                    }

                                    //要验证
                                    if (PurOrder.Deposit == 0 && PurOrder.PayStatus != (int)PayStatus.未付款)
                                    {
                                        PurOrder.PayStatus = (int)PayStatus.全额预付;
                                        await _unitOfWorkManage.GetDbClient().Updateable(PurOrder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();

                                        foreach (var purEntry in PurOrder.tb_PurEntries)
                                        {
                                            if (purEntry.TotalAmount == purEntry.TotalAmount && purEntry.PayStatus == (int)PayStatus.全部付款)
                                            {
                                                purEntry.PayStatus = (int)PayStatus.全额预付;
                                                await _unitOfWorkManage.GetDbClient().Updateable(purEntry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }

                    if (payable.LocalPaidAmount == 0)
                    {
                        payable.ARAPStatus = (int)ARAPStatus.待支付;
                        payable.AllowAddToStatement = true;
                    }
                    else if (payable.LocalPaidAmount < payable.TotalLocalPayableAmount)
                    {
                        payable.ARAPStatus = (int)ARAPStatus.部分支付;
                        payable.AllowAddToStatement = true;
                    }
                    else if (payable.TotalLocalPayableAmount > payable.LocalBalanceAmount)//应付的金额永远大于已经付的
                    {
                        payable.ARAPStatus = (int)ARAPStatus.部分支付;
                        payable.AllowAddToStatement = true;
                    }

                    #endregion

                    #endregion

                }
                arapupdatelist.Add(payable);

                if (prePaymentsUpdateList.Any())
                {
                    var resultprePayment = await _unitOfWorkManage.GetDbClient().Updateable(prePaymentsUpdateList).UpdateColumns(it => new
                    {
                        it.ForeignPaidAmount,
                        it.LocalPaidAmount,
                        it.ForeignBalanceAmount,
                        it.LocalBalanceAmount,
                        it.PrePaymentStatus,
                    }).ExecuteCommandAsync();
                }
                if (arapupdatelist.Any())
                {
                    var resultprePayment = await _unitOfWorkManage.GetDbClient().Updateable(arapupdatelist).UpdateColumns(it => new
                    {
                        it.ForeignPaidAmount,
                        it.LocalPaidAmount,
                        it.ForeignBalanceAmount,
                        it.LocalBalanceAmount,
                        it.ARAPStatus,
                    }).ExecuteCommandAsync();
                }
                #endregion

                #region 恢复更新原来业务单据
                switch (payable.SourceBizType)
                {
                    case (int)BizType.销售出库单:

                        var saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                            .Includes(c => c.tb_saleorder)
                            .Includes(c => c.tb_SaleOutDetails)
                         .Where(x => (x.SaleOut_MainID == payable.SourceBillId))
                         .SingleAsync();


                        if (saleout.tb_saleorder.Deposit > 0 && saleout.PayStatus != (int)PayStatus.部分预付)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.部分预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.部分付款)
                            {
                                saleout.PayStatus = (int)PayStatus.部分预付;
                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            }
                        }

                        //要验证
                        if (saleout.tb_saleorder.Deposit == 0 && saleout.PayStatus != (int)PayStatus.全额预付)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.全额预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            if (saleout.TotalAmount == saleout.TotalAmount && saleout.PayStatus == (int)PayStatus.全部付款)
                            {
                                saleout.PayStatus = (int)PayStatus.全额预付;
                                await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                            }
                        }


                        break;
                    case (int)BizType.采购入库单:

                        var purentry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_purorder)
                            .Includes(c => c.tb_PurEntryDetails)
                         .Where(x => (x.PurEntryID == payable.SourceBillId))
                         .SingleAsync();

                        if (payable.LocalPaidAmount < purentry.TotalAmount && purentry.PayStatus != (int)PayStatus.部分预付)
                        {
                            purentry.PayStatus = (int)PayStatus.部分预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (payable.LocalPaidAmount < purentry.TotalAmount && purentry.TotalAmount < purentry.tb_purorder.TotalAmount)
                        {
                            purentry.tb_purorder.PayStatus = (int)PayStatus.部分预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry.tb_purorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        if (payable.LocalPaidAmount == purentry.TotalAmount && purentry.PayStatus != (int)PayStatus.全额预付)
                        {
                            purentry.PayStatus = (int)PayStatus.全额预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (payable.LocalPaidAmount == purentry.TotalAmount && purentry.TotalAmount == purentry.tb_purorder.TotalAmount)
                        {
                            purentry.tb_purorder.PayStatus = (int)PayStatus.全额预付;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry.tb_purorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        break;
                }
                #endregion

                //只更新指定列
                var updateResult = await _unitOfWorkManage.GetDbClient().Updateable(payable).UpdateColumns(it => new
                {
                    it.ForeignPaidAmount,
                    it.LocalPaidAmount,
                    it.ForeignBalanceAmount,
                    it.LocalBalanceAmount,
                    it.ARAPStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                if (UseTransaction)
                {
                    _unitOfWorkManage.CommitTran();
                }
                result = true;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    _unitOfWorkManage.RollbackTran();
                }
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(payable));
                result = false;
            }
            return result;
        }





        #endregion

        #region 从预收付款中正向抵扣  [审核]
        /// <summary>
        /// 预收付款抵扣
        /// 自动调用手动
        /// </summary>
        public async Task<bool> ApplyManualPaymentAllocation(tb_FM_ReceivablePayable entity, List<tb_FM_PreReceivedPayment> prePayments, bool UseTransaction = true)
        {
            bool result = false;
            try
            {
                decimal remainingLocal = entity.LocalBalanceAmount; // 本币要支付的金额
                decimal remainingForeign = entity.ForeignBalanceAmount; // 外币要支付的金额
                if (UseTransaction)
                {
                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();
                }
                var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();
                foreach (var prePayment in prePayments)
                {
                    // 应收款已全部抵扣，退出循环
                    if (remainingLocal <= 0 && remainingForeign <= 0) break;

                    // 可抵扣金额：预收款余额与剩余待抵扣金额的较小值
                    // 计算可抵扣金额（本币优先）
                    decimal localDeduct = Math.Min(prePayment.LocalBalanceAmount, remainingLocal);
                    decimal foreignDeduct = Math.Min(prePayment.ForeignBalanceAmount, remainingForeign);

                    if (localDeduct > 0 || foreignDeduct > 0)
                    {
                        // 预收款余额减少
                        // 已抵扣金额增加
                        // 更新预收款单余额
                        prePayment.LocalBalanceAmount -= localDeduct;
                        prePayment.ForeignBalanceAmount -= foreignDeduct;
                        prePayment.LocalPaidAmount += localDeduct;
                        prePayment.ForeignPaidAmount += foreignDeduct;

                        // 已收金额增加
                        // 剩余待抵扣金额减少
                        // 更新应收款单已收金额
                        entity.LocalPaidAmount += localDeduct;
                        entity.ForeignPaidAmount += foreignDeduct;
                        entity.LocalBalanceAmount -= localDeduct;
                        entity.ForeignBalanceAmount -= foreignDeduct;

                        remainingLocal -= localDeduct;
                        remainingForeign -= foreignDeduct;

                        // 生成会计分录（需根据实际财务科目调整）
                        /*

                        //var financialVoucher = new tb_FM_FinancialVoucher
                        //{
                        //    VoucherNo = GenerateVoucherNo(),
                        //    VoucherDate = DateTime.Now,
                        //    Subject = $"应收款审核：{entity.ARAPNo}",
                        //    DebitAmount = entity.LocalBalanceAmount, // 借：应收账款
                        //    CreditAmount = entity.LocalRevenueAmount, // 贷：销售收入
                        //    RelatedBillID = entity.ARAPID
                        //};
                      */
                        // 3. 生成核销记录
                        await settlementController.GenerateSettlement(prePayment, entity, localDeduct, foreignDeduct);

                        // 更新预收款状态
                        if (prePayment.LocalBalanceAmount == 0)
                        {
                            prePayment.PrePaymentStatus = (int)PrePaymentStatus.全额核销;
                        }
                        else
                        {
                            prePayment.PrePaymentStatus = (int)PrePaymentStatus.部分核销;
                        }
                    }
                }



                // 处理后若仍有剩余金额，应收款状态为部分支付；否则为已结清
                entity.LocalBalanceAmount = remainingLocal;
                entity.ForeignBalanceAmount = remainingForeign;
                #region 更新原来业务单据
                switch (entity.SourceBizType)
                {
                    case (int)BizType.销售出库单:

                        var saleout = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                            .Includes(c => c.tb_saleorder)
                            .Includes(c => c.tb_SaleOutDetails)
                         .Where(x => (x.SaleOut_MainID == entity.SourceBillId))
                         .SingleAsync();

                        if (entity.LocalPaidAmount < saleout.TotalAmount)
                        {
                            saleout.PayStatus = (int)PayStatus.部分付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (entity.LocalPaidAmount == saleout.TotalAmount)
                        {
                            saleout.PayStatus = (int)PayStatus.全部付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        if (entity.LocalPaidAmount < saleout.TotalAmount && saleout.TotalAmount < saleout.tb_saleorder.TotalAmount)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.部分付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        if (entity.LocalPaidAmount == saleout.TotalAmount && saleout.TotalAmount == saleout.tb_saleorder.TotalAmount)
                        {
                            saleout.tb_saleorder.PayStatus = (int)PayStatus.全部付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(saleout.tb_saleorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }


                        break;
                    case (int)BizType.采购入库单:

                        var purentry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                            .Includes(c => c.tb_purorder)
                            .Includes(c => c.tb_PurEntryDetails)
                         .Where(x => (x.PurEntryID == entity.SourceBillId))
                         .SingleAsync();

                        if (entity.LocalPaidAmount < purentry.TotalAmount)
                        {
                            purentry.PayStatus = (int)PayStatus.部分付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (entity.LocalPaidAmount < purentry.TotalAmount && purentry.TotalAmount < purentry.tb_purorder.TotalAmount)
                        {
                            purentry.tb_purorder.PayStatus = (int)PayStatus.部分付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry.tb_purorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }


                        if (entity.LocalPaidAmount == purentry.TotalAmount)
                        {
                            purentry.PayStatus = (int)PayStatus.全部付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }
                        if (entity.LocalPaidAmount == purentry.TotalAmount && purentry.TotalAmount == purentry.tb_purorder.TotalAmount)
                        {
                            purentry.tb_purorder.PayStatus = (int)PayStatus.全部付款;
                            await _unitOfWorkManage.GetDbClient().Updateable(purentry.tb_purorder).UpdateColumns(it => new { it.PayStatus, }).ExecuteCommandAsync();
                        }

                        break;
                }
                #endregion

                if (remainingLocal == 0)
                {
                    //当预收款中的金额大于等于（超付）收款中的金额时才可能到这里
                    entity.ARAPStatus = (int)ARAPStatus.全部支付;
                    entity.AllowAddToStatement = false;
                }
                else if (entity.LocalBalanceAmount == entity.TotalLocalPayableAmount)
                {
                    entity.ARAPStatus = (int)ARAPStatus.待支付;
                    entity.AllowAddToStatement = true;
                }
                else
                {
                    entity.ARAPStatus = (int)ARAPStatus.部分支付;
                    entity.AllowAddToStatement = true;
                }


                if (prePayments.Any())
                {
                    await _unitOfWorkManage.GetDbClient().Updateable(prePayments).UpdateColumns(
                    it => new
                    {
                        it.LocalBalanceAmount,
                        it.ForeignBalanceAmount,
                        it.LocalPaidAmount,
                        it.ForeignPaidAmount,
                        it.PrePaymentStatus,
                    }
                    ).ExecuteCommandAsync();
                }

                //只更新指定列
                var updateResult = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.ForeignPaidAmount,
                    it.LocalPaidAmount,
                    it.ForeignBalanceAmount,
                    it.LocalBalanceAmount,
                    it.ARAPStatus,
                    it.ApprovalStatus,
                    it.ApprovalResults,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                if (UseTransaction)
                {
                    _unitOfWorkManage.CommitTran();
                }
                result = true;
            }
            catch (Exception ex)
            {
                if (UseTransaction)
                {
                    _unitOfWorkManage.RollbackTran();
                    _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                }
                result = false;

            }
            return result;
        }

        // 自动核销 - 按时间顺序
        /// <summary>
        /// 自动调用手动核销
        /// 适用于销售出库的应收款自动去核销对应的销售订单进来的预付款。  通过销售订单和出库单对应的情况才去核销
        /// 其它情况暂时手动
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="UseTransaction"></param>
        /// <returns></returns>
        public async Task<bool> ApplyAutoPaymentAllocation(tb_FM_ReceivablePayable entity, bool UseTransaction = true)
        {
            bool result = false;
            List<tb_FM_PreReceivedPayment> prePayments = new List<tb_FM_PreReceivedPayment>();
            // 更新预收款单核销状态
            // 找相同客户相同币种 是否有预收付余额
            var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();

            ////一切刚刚好时才能去核销
            if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款 && entity.SourceBizType == (int)BizType.销售出库单)
            {
                var SaleOut = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                                        .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID && c.SaleOut_MainID == entity.SourceBillId)
                                        .SingleAsync();
                if (SaleOut.SOrder_ID.HasValue)
                {

                    prePayments = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                       .Where(x => (x.PrePaymentStatus == (int)PrePaymentStatus.待核销 || x.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                   && x.CustomerVendor_ID == entity.CustomerVendor_ID
                   && x.IsAvailable == true
                   && x.SourceBizType == (int)BizType.销售订单
                   && x.Currency_ID == entity.Currency_ID
                   && x.ReceivePaymentType == entity.ReceivePaymentType
                   && x.SourceBillId == SaleOut.SOrder_ID
                   ).OrderBy(x => x.PrePayDate) // 按创建时间排序
                       .ToListAsync();
                }
            }

            if (entity.ReceivePaymentType == (int)ReceivePaymentType.付款 && entity.SourceBizType == (int)BizType.采购入库单)
            {
                var PurEntry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                                        .Where(c => c.CustomerVendor_ID == entity.CustomerVendor_ID && c.PurEntryID == entity.SourceBillId)
                                        .SingleAsync();
                if (PurEntry.PurOrder_ID.HasValue)
                {
                    prePayments = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                        .Where(x => (x.PrePaymentStatus == (int)PrePaymentStatus.待核销 || x.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                      && x.CustomerVendor_ID == entity.CustomerVendor_ID
                      && x.IsAvailable == true
                      && x.SourceBizType == (int)BizType.采购订单
                      && x.Currency_ID == entity.Currency_ID
                      && x.ReceivePaymentType == entity.ReceivePaymentType
                      && x.SourceBillId == PurEntry.PurOrder_ID
                      ).OrderBy(x => x.PrePayDate) // 按创建时间排序
                          .ToListAsync();
                }

            }


            if (prePayments.Count > 0)
            {
                //去预收中抵扣 的预收付款，生成收款单，并且生成核销记录
                result = await ApplyManualPaymentAllocation(entity, prePayments, UseTransaction);
            }
            return result;



            /*  比方 一个客户的最大限制金额为50W。设置在客户资料中，只会是在检测应收时。去比较。警告提醒。并不会修改这个数据。所以这里不需要这样处理。
            tb_CustomerVendor customerCredit = new tb_CustomerVendor();
            if (prePayments.Count > 0)
            {
                #region  扣除客户信用额度（需根据业务规则实现）订单时可以检测这个值。或提示
                customerCredit = _unitOfWorkManage.GetDbClient()
                  .Queryable<tb_CustomerVendor>()
                  .Where(x => x.CustomerVendor_ID == entity.CustomerVendor_ID)
                  .First();
                #endregion

                if (customerCredit != null)
                {
                    if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        customerCredit.CustomerCreditLimit -= entity.LocalBalanceAmount;
                    }
                }
            }
            */
        }



        #endregion

        /// <summary>
        /// 查找可抵扣的预收付款单
        /// 批量处理
        /// </summary>
        /// <param name="receivablePayable"></param>
        /// <returns></returns>
        public async Task<List<KeyValuePair<tb_FM_ReceivablePayable, tb_FM_PreReceivedPayment>>> FindAvailableAdvances(List<tb_FM_ReceivablePayable> receivablePayables)
        {
            List<KeyValuePair<tb_FM_ReceivablePayable, tb_FM_PreReceivedPayment>> keyValuePairs = new List<KeyValuePair<tb_FM_ReceivablePayable, tb_FM_PreReceivedPayment>>();

            foreach (var receivablePayable in receivablePayables)
            {
                if (receivablePayable.SourceBizType == (int)BizType.销售出库单)
                {
                    var SaleOut = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                    .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID && c.SaleOut_MainID == receivablePayable.SourceBillId)
                    .FirstAsync();//single的话 如果修改过客户。会查不到。所以用first。
                    if (SaleOut == null)
                    {
                        throw new Exception($"销售出库单：{receivablePayable.SourceBillNo}的应收款单来源数据出错，请检查往来单位是否对应。");
                    }
                    if (SaleOut.SOrder_ID.HasValue)
                    {
                        var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                       .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID && c.IsAvailable == true)
                                       .Where(c => c.PrePaymentStatus == (int)PrePaymentStatus.待核销 || c.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                                       .Where(c => c.Currency_ID == receivablePayable.Currency_ID)
                                       .Where(c => c.ReceivePaymentType == receivablePayable.ReceivePaymentType
                                       && c.ReceivePaymentType == receivablePayable.ReceivePaymentType
                                       && c.SourceBillId == SaleOut.SOrder_ID
                                       )
                                       .ToListAsync();
                        if (prePayment.Count > 0)
                        {

                            if (receivablePayable.TotalLocalPayableAmount == prePayment[0].LocalPrepaidAmount &&
                                                       receivablePayable.TotalLocalPayableAmount == prePayment[0].LocalBalanceAmount)
                            {
                                keyValuePairs.Add(new KeyValuePair<tb_FM_ReceivablePayable, tb_FM_PreReceivedPayment>(receivablePayable, prePayment[0]));
                            }
                        }
                    }
                }

                if (receivablePayable.SourceBizType == (int)BizType.采购入库单)
                {
                    var PurEntry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                    .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID && c.PurEntryID == receivablePayable.SourceBillId)
                   .FirstAsync();
                    if (PurEntry == null)
                    {
                        throw new Exception($"采购入库单：{receivablePayable.SourceBillNo}的应付款单来源数据出错，请检查往来单位是否对应。");
                    }
                    if (PurEntry.PurOrder_ID.HasValue)
                    {
                        var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                       .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID && c.IsAvailable == true)
                                       .Where(c => c.PrePaymentStatus == (int)PrePaymentStatus.待核销 || c.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                                       .Where(c => c.Currency_ID == receivablePayable.Currency_ID)
                                       .Where(c => c.ReceivePaymentType == receivablePayable.ReceivePaymentType
                                       && c.ReceivePaymentType == receivablePayable.ReceivePaymentType
                                       && c.SourceBillId == PurEntry.PurOrder_ID
                                       )
                                       .ToListAsync();
                        if (prePayment.Count > 0)
                        {

                            if (receivablePayable.TotalLocalPayableAmount == prePayment[0].LocalPrepaidAmount &&
                                                       receivablePayable.TotalLocalPayableAmount == prePayment[0].LocalBalanceAmount)
                            {
                                keyValuePairs.Add(new KeyValuePair<tb_FM_ReceivablePayable, tb_FM_PreReceivedPayment>(receivablePayable, prePayment[0]));
                            }
                        }
                    }
                }
            }

            return keyValuePairs;
        }



        /// <summary>
        /// 查找可撤销抵扣的预收付款单
        /// </summary>
        /// <param name="receivablePayable"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PreReceivedPayment>> FindAvailableAntiOffset(tb_FM_ReceivablePayable receivablePayable, List<tb_FM_PaymentSettlement> settlements)
        {

            long[] statementIds = settlements.Where(c => c.SourceBillId.HasValue).ToList().Select(c => c.SourceBillId.Value).ToArray();

            var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                         .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID && c.IsAvailable == true)
                         .Where(c => c.PrePaymentStatus == (int)PrePaymentStatus.全额核销 || c.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                         .Where(c => c.Currency_ID == receivablePayable.Currency_ID)
                         .Where(c => c.ReceivePaymentType == receivablePayable.ReceivePaymentType
                          && statementIds.Contains(c.PreRPID)
                         )
                         .OrderBy(c => c.PrePayDate)
                         .ToListAsync();
            return prePayment;
        }

        /// <summary>
        /// 查找可抵扣的预收付款单
        /// </summary>
        /// <param name="receivablePayable"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PreReceivedPayment>> FindAvailableAdvances(tb_FM_ReceivablePayable receivablePayable)
        {
            var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                         .Where(c => c.CustomerVendor_ID == receivablePayable.CustomerVendor_ID && c.IsAvailable == true)
                         .Where(c => c.PrePaymentStatus == (int)PrePaymentStatus.待核销 || c.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                         .Where(c => c.Currency_ID == receivablePayable.Currency_ID)
                         .Where(c => c.ReceivePaymentType == receivablePayable.ReceivePaymentType)
                         //.Where(p => p.LocalBalanceAmount > 0)
                         .OrderBy(c => c.PrePayDate)
                         .ToListAsync();
            return prePayment;
        }

        /// <summary>
        /// 创建应收款单，并且自动审核，因为后面还会自动去冲预收款单
        /// 如果有订金，会先去预收检测
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isRefund">反审核时用，true为红字冲销</param>
        /// <returns></returns>
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_SaleOut entity, bool IsProcessCommission = false)
        {
            #region 创建应收款单

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.IsForCommission = IsProcessCommission;
            if (entity.tb_saleorder != null)
            {
                payable.Account_id = entity.tb_saleorder.Account_id;
            }
            payable.BusinessDate = entity.OutDate;
            payable.DocumentDate = entity.Created_at.Value;
            payable.SourceBillNo = entity.SaleOutNo;
            payable.SourceBillId = entity.SaleOut_MainID;
            payable.SourceBizType = (int)BizType.销售出库单;
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.DepartmentID.HasValue)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }
            payable.IsExpenseType = false;
            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = MyCacheManager.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
                if (projectgroup != null && projectgroup.ToString() != "System.Object")
                {
                    if (projectgroup is tb_ProjectGroup pj)
                    {
                        entity.tb_projectgroup = pj;
                        payable.DepartmentID = pj.DepartmentID;
                    }
                }
                else
                {
                    //db查询
                    entity.tb_projectgroup = await _appContext.GetRequiredService<tb_ProjectGroupController<tb_ProjectGroup>>().BaseQueryByIdAsync(entity.ProjectGroup_ID);
                    payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
                }
            }
            //佣金生成应付款单
            if (IsProcessCommission)
            {
                payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
                payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
                payable.DueDate = null;
            }
            else
            {
                //销售就是收款
                payable.ReceivePaymentType = (int)ReceivePaymentType.收款;
                payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收款单);
                if (entity.tb_saleorder.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    var obj = MyCacheManager.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                    if (obj != null && obj.ToString() != "System.Object")
                    {
                        if (obj is tb_CustomerVendor cv)
                        {
                            entity.tb_customervendor = cv;
                        }
                    }
                    else
                    {
                        //db查询
                        entity.tb_customervendor = await _appContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>().BaseQueryByIdAsync(entity.CustomerVendor_ID);
                    }

                    if (entity.tb_customervendor.CustomerCreditDays.HasValue)
                    {
                        // 从销售出库日期开始计算到期日
                        //应收账期 的到期时间
                        payable.DueDate = entity.OutDate.Date.AddDays(entity.tb_customervendor.CustomerCreditDays.Value).AddDays(1).AddTicks(-1);
                    }
                }
            }

            payable.Currency_ID = entity.Currency_ID;

            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_SaleOutDetails);
            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                ////这个写法，如果原来明细中  相同产品ID 多行录入。就会出错。混乱。
                ///如果来源和目录字段值一致。mapper应该完成映射。目前为了保险，在目标明细实体中添加一个 原始的主键ID
                var olditem = entity.tb_SaleOutDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID
                && c.SaleOutDetail_ID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    if (IsProcessCommission)
                    {
                        //佣金应付
                        details[i].TaxRate = olditem.TaxRate;
                        details[i].TaxLocalAmount = olditem.SubtotalTaxAmount;
                        details[i].Quantity = olditem.Quantity;
                        details[i].UnitPrice = olditem.UnitCommissionAmount;
                        details[i].LocalPayableAmount = olditem.CommissionAmount;
                        //下面有专门的一行运费。这里加上去不明了，会导致误会。如果生成对账单的话。
                        //details[i].LocalPayableAmount+= olditem.AllocatedFreightIncome;
                    }
                    else
                    {
                        //正常应收
                        details[i].TaxRate = olditem.TaxRate;
                        details[i].TaxLocalAmount = olditem.SubtotalTaxAmount;
                        details[i].Quantity = olditem.Quantity;
                        details[i].UnitPrice = olditem.TransactionPrice;
                        details[i].LocalPayableAmount = olditem.TransactionPrice * olditem.Quantity;
                        //下面有专门的一行运费。这里加上去不明了，会导致误会。如果生成对账单的话。
                        //details[i].LocalPayableAmount+= olditem.AllocatedFreightIncome;
                    }

                }

                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;


            }

            #region 单独处理运费 ,这里是应收。意思是收取客户的运费。应该以运费成本为标准。佣金不需要
            // 添加运费行（关键部分）
            if (entity.FreightCost > 0 && !IsProcessCommission)
            {
                payable.ShippingFee = entity.FreightCost;
            }

            #endregion

            payable.tb_FM_ReceivablePayableDetails = details;
            payable.TotalLocalPayableAmount = payable.tb_FM_ReceivablePayableDetails.Sum(c => c.LocalPayableAmount) + payable.ShippingFee;
            payable.TaxTotalAmount = payable.tb_FM_ReceivablePayableDetails.Sum(c => c.TaxLocalAmount);
            payable.UntaxedTotalAmont = payable.TotalLocalPayableAmount - payable.TaxTotalAmount;
            //本币时
            payable.LocalBalanceAmount = payable.TotalLocalPayableAmount;
            payable.LocalPaidAmount = 0;

            //这里要重点思考 是本币一定有。！！！！！！！！！！！！！！！！！ TODO by watson
            //外币时  只是换算。本币不能少。
            //暂时外布没有严格的逻辑核对
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                payable.TotalForeignPayableAmount = payable.TotalLocalPayableAmount * payable.ExchangeRate;
                payable.ForeignBalanceAmount = payable.TotalForeignPayableAmount;
                payable.ForeignPaidAmount = 0;
            }
            //业务经办人
            if (entity.tb_saleorder != null)
            {
                payable.Employee_ID = entity.tb_saleorder.Employee_ID;
            }
            if (IsProcessCommission)
            {
                payable.Remark = $"由销售出库单：{entity.SaleOutNo}【佣金】生成的应付款单";
            }
            else
            {
                payable.Remark = $"由销售出库单：{entity.SaleOutNo}【货款】生成的应收款单";
            }

            if (payable.IsFromPlatform)
            {
                payable.Remark += " 平台单号:" + entity.PlatformOrderNo;
            }
            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }

            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;

            #endregion
            return payable;
        }


        /// <summary>
        /// 创建应付款单，审核时如果有预付，会先核销
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isRefund">true为红字冲销</param>
        /// <returns></returns>
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_PurEntry entity, bool isRefund)
        {
            //入库时，全部生成应付，账期的。就加上到期日
            //有付款过的。就去预付中抵扣，不够的金额及状态标识出来生成对账单

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.SourceBillNo = entity.PurEntryNo;
            payable.SourceBillId = entity.PurEntryID;
            payable.SourceBizType = (int)BizType.采购入库单;
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }

            #region 如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话 
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = MyCacheManager.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
                if (projectgroup != null && projectgroup.ToString() != "System.Object")
                {
                    if (projectgroup is tb_ProjectGroup pj)
                    {
                        entity.tb_projectgroup = pj;
                        payable.DepartmentID = pj.DepartmentID;
                    }
                }
                else
                {
                    //db查询
                    entity.tb_projectgroup = await _appContext.GetRequiredService<tb_ProjectGroupController<tb_ProjectGroup>>().BaseQueryByIdAsync(entity.ProjectGroup_ID);
                    payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
                }
            }

            #endregion

            //采购就是付款
            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
            if (entity.Currency_ID.HasValue)
            {
                payable.Currency_ID = entity.Currency_ID.Value;
            }
            else
            {
                payable.Currency_ID = _appContext.BaseCurrency.Currency_ID;
            }
            payable.BusinessDate = entity.EntryDate;
            payable.DocumentDate = entity.Created_at.Value;
            //如果销售订单中付款方式不为空，并且是账期时
            if (entity.tb_purorder.Paytype_ID.HasValue && entity.tb_purorder.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
            {
                var obj = MyCacheManager.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
                if (obj != null && obj.ToString() != "System.Object")
                {
                    if (obj is tb_CustomerVendor cv)
                    {
                        entity.tb_customervendor = cv;
                    }
                }
                else
                {
                    //db查询
                    entity.tb_customervendor = await _appContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>().BaseQueryByIdAsync(entity.CustomerVendor_ID);
                }

                if (entity.tb_customervendor.SupplierCreditDays.HasValue)
                {
                    // 从入库日期开始计算到期日
                    payable.DueDate = entity.EntryDate.Date.AddDays(entity.tb_customervendor.SupplierCreditDays.Value).AddDays(1).AddTicks(-1);
                }
            }
            payable.ExchangeRate = entity.ExchangeRate;

            //日期类型 业务意义    财务价值 使用场景
            //业务发生日期 真实交易时间  确定收入成本期间 对账核心依据
            //单据日期 债权债务确认  账期起始点 账龄计算
            //到期日 资金计划依据  现金流管理 催款付款
            //创建时间 系统操作时间  操作审计 内部控制
            //审核时间 财务确认时间  审批流程 责任追溯





            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_PurEntryDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID && c.PurEntryDetail_ID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.TaxAmount;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.UnitPrice;
                    details[i].LocalPayableAmount = olditem.UnitPrice * olditem.Quantity;
                    //下面有专门的一行运费。这里加上去不明了，会导致误会。如果生成对账单的话。
                    //details[i].LocalPayableAmount+= olditem.AllocatedFreightIncome;
                }
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;

            }

            payable.tb_FM_ReceivablePayableDetails = details;
            //如果是外币时，则由外币算出本币
            if (isRefund)
            {
                //为负数
                entity.ForeignTotalAmount = -entity.ForeignTotalAmount;
                entity.TotalAmount = -entity.TotalAmount;
            }
            //外币时
            //这里要重点思考 是本币一定有。！！！！！！！！！！！！！！！！！ TODO by watson
            //外币时  只是换算。本币不能少。
            if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
            {
                payable.ForeignBalanceAmount = entity.ForeignTotalAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = entity.ForeignTotalAmount;
            }
            #region 单独处理运费 ,这里是应收。意思是收取客户的运费。应该以运费成本为标准。
            // 添加运费行（关键部分）
            if (entity.ShipCost > 0)
            {
                payable.ShippingFee = entity.ShipCost;
            }
            #endregion
            //本币时 一定会有值。
            payable.LocalBalanceAmount = entity.TotalAmount;
            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = entity.TotalAmount;
            if (entity.tb_purorder != null && !payable.PayeeInfoID.HasValue)
            {
                //通过订单添加付款信息
                payable.PayeeInfoID = entity.tb_purorder.PayeeInfoID;
            }

            //业务经办人
            if (entity.tb_purorder != null)
            {
                payable.Employee_ID = entity.tb_purorder.Employee_ID;
            }

            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }
            payable.Remark = $"采购入库单：{entity.PurEntryNo}的应付款";
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;
            return payable;
        }

        /// <summary>
        /// 创建应付款单，审核时如果有预付，会先核销
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isRefund">true为红字冲销</param>
        /// <returns></returns>
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_FinishedGoodsInv entity, bool isRefund)
        {
            if (!entity.CustomerVendor_ID.HasValue)
            {
                throw new Exception("请选择正确的加工单位！");
            }
            //入库时，全部生成应付，账期的。就加上到期日
            //有付款过的。就去预付中抵扣，不够的金额及状态标识出来生成对账单

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.SourceBillNo = entity.DeliveryBillNo;
            payable.SourceBillId = entity.FG_ID;
            payable.SourceBizType = (int)BizType.缴库单;
            if (entity.tb_manufacturingorder != null)
            {
                payable.DepartmentID = entity.tb_manufacturingorder.DepartmentID;
            }

            payable.CustomerVendor_ID = entity.CustomerVendor_ID.Value;

            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);

            payable.Currency_ID = _appContext.BaseCurrency.Currency_ID;

            payable.BusinessDate = entity.DeliveryDate;
            payable.DocumentDate = entity.Created_at.Value;
            //如果销售订单中付款方式不为空，并且是账期时

            var obj = MyCacheManager.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
            if (obj != null && obj.ToString() != "System.Object")
            {
                if (obj is tb_CustomerVendor cv)
                {
                    entity.tb_customervendor = cv;
                }
            }
            else
            {
                //db查询
                entity.tb_customervendor = await _appContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>().BaseQueryByIdAsync(entity.CustomerVendor_ID);
            }

            if (entity.tb_customervendor.SupplierCreditDays.HasValue)
            {
                // 从入库日期开始计算到期日
                payable.DueDate = entity.DeliveryDate.Date.AddDays(entity.tb_customervendor.SupplierCreditDays.Value).AddDays(1).AddTicks(-1);
            }

            payable.ExchangeRate = 1;

            //日期类型 业务意义    财务价值 使用场景
            //业务发生日期 真实交易时间  确定收入成本期间 对账核心依据
            //单据日期 债权债务确认  账期起始点 账龄计算
            //到期日 资金计划依据  现金流管理 催款付款
            //创建时间 系统操作时间  操作审计 内部控制
            //审核时间 财务确认时间  审批流程 责任追溯

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_FinishedGoodsInvDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_FinishedGoodsInvDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID && c.Sub_ID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = 0;
                    details[i].TaxLocalAmount = 0;
                    if (isRefund)
                    {
                        details[i].Quantity = -olditem.Qty;
                    }
                    //制造费   
                    details[i].UnitPrice = olditem.ManuFee;
                    details[i].LocalPayableAmount = olditem.ManuFee * details[i].Quantity.Value;
                }
                details[i].ExchangeRate = 1;
                details[i].ActionStatus = ActionStatus.新增;

            }

            payable.tb_FM_ReceivablePayableDetails = details;

            #region 单独处理运费 ,这里是应收。意思是收取客户的运费。应该以运费成本为标准。

            payable.ShippingFee = 0;

            #endregion
            //本币时 一定会有值。

            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = details.Sum(c => c.LocalPayableAmount);
            payable.LocalBalanceAmount = payable.TotalLocalPayableAmount;

            //业务经办人
            payable.Employee_ID = entity.Employee_ID;

            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }
            payable.Remark = $"缴库单：{entity.DeliveryBillNo}制作费的应付款";
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;
            return payable;
        }


        /// <summary>
        /// 由采购退货入为生成蓝字应付款单，这个单一般是用来冲红采购退货单对应的红字应付款单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public tb_FM_ReceivablePayable BuildReceivablePayable(tb_PurReturnEntry entity)
        {
            //入库时，全部生成应付，账期的。就加上到期日
            //有付款过的。就去预付中抵扣，不够的金额及状态标识出来生成对账单

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.SourceBillNo = entity.PurReEntryNo;
            payable.SourceBillId = entity.PurReEntry_ID;
            payable.SourceBizType = (int)BizType.采购退货入库;
            payable.BusinessDate = entity.BillDate;
            payable.DocumentDate = entity.Created_at.Value;
            payable.DepartmentID = entity.DepartmentID;

            //采购就是付款
            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);

            payable.Currency_ID = entity.Currency_ID;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_PurReturnEntryDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_PurReturnEntryDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID && c.PurReEntry_CID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.TaxAmount;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.UnitPrice;
                    details[i].LocalPayableAmount = olditem.UnitPrice * olditem.Quantity;
                }
                details[i].ActionStatus = ActionStatus.新增;
            }

            payable.tb_FM_ReceivablePayableDetails = details;

            #region 单独处理运费 ,这里是应收。意思是收取客户的运费。应该以运费成本为标准。
            // 添加运费行（关键部分）
            //  payable.ShippingFee = entity.ShipCost;

            #endregion
            //本币时 一定会有值。
            payable.LocalBalanceAmount = entity.TotalAmount;
            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = entity.TotalAmount;

            //通过订单添加付款信息
            // payable.PayeeInfoID = entity.PayeeInfoID;


            //业务经办人
            payable.Employee_ID = entity.Employee_ID;
            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }
            payable.IsExpenseType = false;
            payable.Remark = $"采购退货入库单：{entity.PurReEntryNo}的应付款";
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;
            return payable;
        }


        public async Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model, bool UseTran = false) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            //缓存当前编辑的对象。如果撤销就回原来的值
            T oldobj = CloneHelper.DeepCloneObject_maxnew<T>((T)model);

            tb_FM_ReceivablePayable entity = model as tb_FM_ReceivablePayable;
            command.UndoOperation = delegate ()
            {
                //Undo操作会执行到的代码
                CloneHelper.SetValues<T>(entity, oldobj);
            };

            if (entity.ARAPId > 0)
            {

                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_ReceivablePayable>(entity as tb_FM_ReceivablePayable)
           .Include(m => m.tb_FM_StatementDetails)
       .Include(m => m.tb_FM_ReceivablePayableDetails)
       .ExecuteCommandAsync();
            }
            else
            {
                rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_ReceivablePayable>(entity as tb_FM_ReceivablePayable)
        .Include(m => m.tb_FM_StatementDetails)
        .Include(m => m.tb_FM_ReceivablePayableDetails)
        .ExecuteCommandAsync();

            }

            rsms.ReturnObject = entity as T;
            entity.PrimaryKeyID = entity.ARAPId;
            rsms.Succeeded = rs;
            return rsms;
        }


        /// <summary>
        /// 创建为红字冲销 应收款单
        /// 退回用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public tb_FM_ReceivablePayable BuildReceivablePayable(tb_PurEntryRe entity)
        {
            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            payable = mapper.Map<tb_FM_ReceivablePayable>(entity);
            payable.ApprovalResults = null;
            payable.ApprovalStatus = (int)ApprovalStatus.未审核;
            payable.Approver_at = null;
            payable.Approver_by = null;
            payable.PrintStatus = 0;
            payable.ActionStatus = ActionStatus.新增;
            payable.ApprovalOpinions = "";
            payable.Modified_at = null;
            payable.Modified_by = null;
            payable.DepartmentID = entity.DepartmentID;
            payable.SourceBillNo = entity.PurEntryReNo;
            payable.SourceBillId = entity.PurEntryRe_ID;
            payable.SourceBizType = (int)BizType.采购退货单;
            //销售就是收款
            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
            payable.BusinessDate = entity.ReturnDate;
            payable.DocumentDate = entity.Created_at.Value;
            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
            if (entity.Currency_ID.HasValue)
            {
                payable.Currency_ID = entity.Currency_ID.Value;
            }
            else
            {
                payable.Currency_ID = _appContext.BaseCurrency.Currency_ID;
            }

            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_PurEntryReDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_PurEntryReDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID && c.PurEntryRe_CID == details[i].SourceItemRowID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.TaxAmount;
                    details[i].Quantity = -olditem.Quantity;//注意是负数方向
                    details[i].UnitPrice = olditem.UnitPrice;
                    details[i].LocalPayableAmount = details[i].UnitPrice.Value * details[i].Quantity.Value;
                }
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;

            }
            payable.tb_FM_ReceivablePayableDetails = details;
            //如果是外币时，则由外币算出本币

            //外币时
            if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
            {
                payable.ForeignBalanceAmount = -entity.ForeignTotalAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = -entity.ForeignTotalAmount;
            }
            else
            {
                //本币时
                payable.LocalBalanceAmount = -entity.TotalAmount;
                payable.LocalPaidAmount = 0;
                payable.TotalLocalPayableAmount = -entity.TotalAmount;
            }
            //业务经办人
            payable.Employee_ID = entity.Employee_ID;
            payable.IsExpenseType = false;
            payable.Remark = $"采购入库单：{entity.PurEntryNo}对应的采购退货单{entity.PurEntryReNo}的红字应付款";
            //否则会关联性SQL出错，外键
            if (payable.Account_id <= 0)
            {
                payable.Account_id = null;
            }
            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;

            return payable;
        }


        public async Task<bool> BaseLogicDeleteAsync(tb_FM_ReceivablePayable ObjectEntity)
        {
            //  ReturnResults<tb_FM_ReceivablePayableController> rrs = new Business.ReturnResults<tb_FM_ReceivablePayableController>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_ReceivablePayable>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // MyCacheManager.Instance.DeleteEntityList<tb_FM_ReceivablePayableController>(entity);
            }
            return false;
        }

        ///// <summary>
        ///// 要生成收付单 没完成
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns></returns>
        //public async virtual Task<bool> BatchApproval(List<tb_FM_ReceivablePayable> entitys, ApprovalEntity approvalEntity)
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
        //                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(entity).ExecuteCommandAsync();
        //            }
        //        }
        //        // 注意信息的完整性
        //        _unitOfWorkManage.CommitTran();

        //        //_logger.Info(approvalEntity.bizName + "审核事务成功");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
        //        _unitOfWorkManage.RollbackTran();
        //        _logger.Error(approvalEntity.bizName + "事务回滚");
        //        return false;
        //    }

        //}


        /*
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
            List<tb_FM_ReceivablePayable> entitys = new List<tb_FM_ReceivablePayable>();
            entitys = NeedCloseCaseList as List<tb_FM_ReceivablePayable>;

            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案
                //更新拟销售量  减少
                for (int m = 0; m < entitys.Count; m++)
                {
                    //DOTO 没有完成
                    //判断 能结案的 是关闭的意思。就是没有收到款 作废
                    // 检查预付款取消
                    var preStatus = PrePaymentStatus.已生效 | PrePaymentStatus.部分核销;
                    bool hasRelated = false; // 存在核销单
                    bool canCancel = preStatus.CanCancel(hasRelated); // 返回false



                    //判断 能结案的 是确认审核过的。
                    if (entitys[m].PrePaymentStatus != (int)PrePaymentStatus.已冲销 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].PrePaymentStatus = (int)PrePaymentStatus.已冲销;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient()
                        .Updateable<tb_FM_ReceivablePayable>(entitys[m])
                        .UpdateColumns(it => new
                        {
                            it.PrePaymentStatus,
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
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }
        }
        */

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_ReceivablePayable> list = await _appContext.Db.CopyNew().Queryable<tb_FM_ReceivablePayable>()
                .Where(m => m.ARAPId == ID)
                            .Includes(a => a.tb_fm_account)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_FM_ReceivablePayableDetails, b => b.tb_fm_expensetype)
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_FM_ReceivablePayableDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                            .ToListAsync();
            return list as List<T>;
        }




    }
}



