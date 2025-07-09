
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
using RUINORERP.Business.FMService;
using System.Drawing.Drawing2D;
using System.Collections;

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
        /// 客户取消订单时，如果有订单，如果财务没有在他对应的收付单里审核前是可以反审的。否则只能通过红冲机制处理。
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

                if (!FMPaymentStatusHelper.CanReReview(currentStatus, false))
                {
                    rmrs.ErrorMsg = $"状态为【{currentStatus.ToString()}】的预{((ReceivePaymentType)entity.ReceivePaymentType).ToString()}单不可以反审";
                    return rmrs;
                }


                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //如果是应收已经有收款记录，则生成反向收款，否则直接删除应收

                var Antiresult = await AntiApplyManualPaymentAllocation(entity, (ReceivePaymentType)entity.ReceivePaymentType, false);
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

                #region

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
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }

        }


        /// <summary>
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付，这财务审核收款单前，还是可以反审的
        /// 审核通过时生成关联的收款/付款草稿单 财务核对是否到账。
        /// 核销记录：预收款抵扣应收款（正向核销）
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
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
                    if (!entity.PayeeInfoID.HasValue)
                    {
                        rmrs.ErrorMsg = "付款时，对方的收款信息必填!";
                        rmrs.Succeeded = false;
                        rmrs.ReturnObject = entity as T;
                        return rmrs;
                    }
                }

                //应收款中不能存在相同的来源的 正数金额的出库单的应收数据
                //一个出库不能多次应收。一个出库一个应收（负数除外）。一个应收可以多次收款来抵扣

                //审核时 要检测明细中对应的相同业务类型下不能有相同来源单号。除非有正负总金额为0对冲情况。或是两行数据?
                var PendingApprovalReceivablePayable = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                    .Includes(c => c.tb_FM_ReceivablePayableDetails)
                .Where(c => c.ARAPStatus >= (int)ARAPStatus.待支付)
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
                _unitOfWorkManage.BeginTran();

                //去核销预收付表
                bool rs = await ApplyAutoPaymentAllocation(entity, false);

                //出库生成应付，应付审核时如果有预收付核销后应该是部分支付了。
                if (entity.LocalBalanceAmount == entity.TotalLocalPayableAmount)
                {
                    entity.ARAPStatus = (int)ARAPStatus.待支付;
                }
                else if (entity.TotalLocalPayableAmount == entity.LocalPaidAmount && entity.LocalBalanceAmount == 0)
                {
                    entity.ARAPStatus = (int)ARAPStatus.全部支付;
                }
                else
                {
                    entity.ARAPStatus = (int)ARAPStatus.部分支付;
                }
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
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
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = ex.Message;
                return rmrs;
            }
        }


        /// <summary>
        /// 审核及之后的状态的应收付款中，不能有相同业务单号的数据存在。除非正负红冲情况
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

                    // 如果只有一条记录，直接通过
                    if (items.Count == 1)
                        continue;

                    // 如果有两条记录，检查是否为对冲情况
                    if (items.Count == 2)
                    {
                        // 计算本币金额总和
                        decimal totalLocalAmount = items.Sum(i => i.LocalBalanceAmount);
                        // 计算外币金额总和
                        decimal totalForeignAmount = items.Sum(i => i.ForeignBalanceAmount);

                        // 检查是否满足对冲条件（总和接近0，考虑浮点数精度问题）
                        if (Math.Abs(totalLocalAmount) < 0.001m && Math.Abs(totalForeignAmount) < 0.001m)
                            continue;
                    }
                    returnResults.ErrorMsg = $"业务来源：{(BizType)groupedByBizType[0].Key}，单号:{groupedByBillNo[0].Key}\r\n应{(ReceivePaymentType)ReceivablePayables[0].ReceivePaymentType}单已经存在，数据不能重复，请检查。";
                    // 其他情况均视为不合法
                    return false;
                }
            }

            return true;
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
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
            }
            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = BizCacheHelper.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
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

            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收款单);

            payable.Currency_ID = entity.Currency_ID;

            //if (entity.tb_saleorder.tb_paymentmethod.Paytype_Name == DefaultPaymentMethod.账期.ToString())
            //{
            //    if (entity.tb_customervendor.CustomerCreditDays.HasValue)
            //    {
            //        // 从销售出库日期开始计算到期日
            //        payable.DueDate = entity.OutDate.Date.AddDays(entity.tb_customervendor.CustomerCreditDays.Value).AddDays(1).AddTicks(-1);
            //    }
            //}
            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_SaleOutReDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_SaleOutReDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.SubtotalTaxAmount;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.TransactionPrice;
                    details[i].LocalPayableAmount = olditem.TransactionPrice * olditem.Quantity;
                }
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;

                View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                {
                    if (prodDetail != null && obj.Unit_ID != null && obj.Unit_ID.HasValue)
                    {
                        details[i].Unit_ID = obj.Unit_ID.Value;
                    }
                }
                //数量标负数，单价保持正数是标准做法
                details[i].Quantity = -details[i].Quantity;
                details[i].TaxLocalAmount = -details[i].TaxLocalAmount;
                details[i].LocalPayableAmount = details[i].UnitPrice.Value * details[i].Quantity.Value;
            }

            #region 单独加一行运算到明细中 默认退货不退运费,要的话,自己手动添加

            // 添加运费行（关键部分）
            //if (entity.ShipCost > 0)
            //{
            //    details.Add(new tb_FM_ReceivablePayableDetail
            //    {
            //        ProdDetailID = null,
            //        property = "运费",
            //        SourceBillNo = entity.ReturnNo,
            //        SourceBillId = entity.SaleOutRe_ID,
            //        SourceBizType = (int)BizType.销售退回单,
            //        Specifications = "",
            //        ExchangeRate = 1,
            //        Description = "发货运费",
            //        UnitPrice = entity.ShipCost,
            //        Quantity = 1,
            //        TaxRate = 0.09m, // 假设运费税率为9%
            //        TaxLocalAmount = 1,
            //        Summary = "",
            //        LocalPayableAmount = entity.ShipCost
            //    });
            //}

            #endregion

            payable.tb_FM_ReceivablePayableDetails = details;
            //如果是外币时，则由外币算出本币
            //外币时
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
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

            payable.Remark = $"销售出库单：{entity.SaleOut_NO}对应的销售退回单{entity.ReturnNo}的应付款";
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
                payable.Remark = $"销售出库单调整后：{entity.SourceBillNo} 的应收款";
            }
            else
            {
                payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
                payable.SourceBizType = (int)BizType.采购价格调整单;
                payable.Remark = $"采购入库单调整后：{entity.SourceBillNo} 的应付款";
            }

            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = BizCacheHelper.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
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
                var projectgroup = BizCacheHelper.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
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

            payable.Currency_ID = entity.Currency_ID;
            var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
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



            payable.ExchangeRate = entity.ExchangeRate;


            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_FM_PriceAdjustmentDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_FM_PriceAdjustmentDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.TaxDiffLocalAmount;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.DiffUnitPrice;
                    details[i].LocalPayableAmount = olditem.DiffUnitPrice * olditem.Quantity;
                }
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;

                View_ProdDetail Prodobj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (Prodobj != null && Prodobj.GetType().Name != "Object" && Prodobj is View_ProdDetail prodDetail)
                {
                    if (prodDetail != null && Prodobj.Unit_ID != null && Prodobj.Unit_ID.HasValue)
                    {
                        details[i].Unit_ID = Prodobj.Unit_ID.Value;
                    }
                }
            }

            payable.tb_FM_ReceivablePayableDetails = details;

            //外币时
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                payable.ForeignBalanceAmount = entity.TotalForeignDiffAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = entity.TotalForeignDiffAmount;
            }
            else
            {
                //本币时
                payable.LocalBalanceAmount = entity.TotalLocalDiffAmount;
                payable.LocalPaidAmount = 0;
                payable.TotalLocalPayableAmount = entity.TotalLocalDiffAmount;
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
            try
            {
                // 0. 验证
                if (receivablePayable == null)
                    throw new Exception("无可处理数据");

                // 1. 验证未核销金额
                if (receivablePayable.ForeignBalanceAmount <= 0 || receivablePayable.LocalBalanceAmount <= 0)
                    throw new Exception("无未核销金额，无法进行坏账处理");

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
                _logger.Error(ex, "事务回滚" + ex.Message);
                returnResults.ErrorMsg = ex.Message;
                return returnResults;
            }

        }

        #region 从预收付款中正向抵扣 [反审]

        /// <summary>
        /// 预收付款抵扣-的反操作
        /// </summary>
        /// <param name="UseTransaction">外层有事务时这里传false</param>
        public async Task<bool> AntiApplyManualPaymentAllocation(tb_FM_ReceivablePayable payable, ReceivePaymentType paymentType, bool UseTransaction = true)
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
                //产生两个相同出库单号的应付，只有退款 红冲 一正一负

                if (payable.ARAPStatus == (int)ARAPStatus.草稿
                    || payable.ARAPStatus == (int)ARAPStatus.待审核)
                {
                    await _appContext.Db.DeleteNav(payable).Include(c => c.tb_FM_ReceivablePayableDetails).ExecuteCommandAsync();
                    //这里还要判断 是否有付款单，还是只有预收付的核销。有付款单则用红冲?
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
                    }
                    else if (payable.LocalPaidAmount < payable.LocalBalanceAmount)
                    {
                        payable.ARAPStatus = (int)ARAPStatus.部分支付;
                    }

                    #endregion

                    //要根据他核销的是预收的。还是收款单的 分别处理
                    switch (payable.ARAPStatus)
                    {
                        case (int)ARAPStatus.全部支付:
                        case (int)ARAPStatus.部分支付:
                        case (int)ARAPStatus.坏账:
                        case (int)ARAPStatus.已冲销:
                            if (UseTransaction)
                            {
                                _unitOfWorkManage.RollbackTran();
                            }
                            _unitOfWorkManage.RollbackTran();
                            string msg = $"应{(ReceivePaymentType)payable.ReceivePaymentType}单状态为【{payable.ARAPStatus}】，无法反核销。请联系管理员";
                            _logger.LogInformation(msg);
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
                                await _appContext.Db.DeleteNav(payable).Include(c => c.tb_FM_ReceivablePayableDetails).ExecuteCommandAsync();
                            }
                            else
                            {
                                //正常不会到这里的。
                                if (UseTransaction)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                }
                                string errormsg = $"应{(ReceivePaymentType)payable.ReceivePaymentType}单状态为【{payable.ARAPStatus}】未核销金额为{payable.LocalBalanceAmount}，核销金额为{payable.LocalPaidAmount}，无法反核销";

                                _logger.LogInformation(errormsg);
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
                    _logger.Error(ex, "事务回滚" + ex.Message);
                }
                result = false;
            }
            return result;
        }



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="UseTransaction"></param>
        ///// <returns></returns>
        //public async Task<bool> AntiApplyAutoPaymentAllocation(tb_FM_ReceivablePayable entity, bool UseTransaction = true)
        //{
        //    bool result = false;

        //    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

        //    //出库时，全部生成应收，账期的。就加上到期日
        //    //有付款过的。就去预收中抵扣，不够的金额及状态标识出来
        //    //反操作上面的逻辑

        //    //如果收款了，则不能反审,预收的可以
        //    List<tb_FM_ReceivablePayable> arapupdatelist = new List<tb_FM_ReceivablePayable>();
        //    List<tb_FM_PreReceivedPayment> prePaymentsUpdateList = new List<tb_FM_PreReceivedPayment>();
        //    var ARAPList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
        //                    .Includes(c => c.tb_FM_ReceivablePayableDetails)
        //                   .Where(c => c.SourceBillId == entity.SaleOut_MainID && c.SourceBizType == (int)BizType.销售出库单).ToListAsync();

        //    if (ARAPList != null)
        //    {

        //    }

        //    result = await AntiApplyManualPaymentAllocation(entity, UseTransaction);
        //    return result;

        //}


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
                }
                else if (entity.LocalBalanceAmount == entity.TotalLocalPayableAmount)
                {
                    entity.ARAPStatus = (int)ARAPStatus.待支付;
                }
                else
                {
                    entity.ARAPStatus = (int)ARAPStatus.部分支付;
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
                    _logger.Error(ex, "事务回滚" + ex.Message);
                }
                result = false;

            }
            return result;
        }

        // 自动核销 - 按时间顺序
        /// <summary>
        /// 自动调用手动核销
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="UseTransaction"></param>
        /// <returns></returns>
        public async Task<bool> ApplyAutoPaymentAllocation(tb_FM_ReceivablePayable entity, bool UseTransaction = true)
        {
            bool result = false;

            // 更新预收款单核销状态
            // 找相同客户相同币种 是否有预收付余额
            var settlementController = _appContext.GetRequiredService<tb_FM_PaymentSettlementController<tb_FM_PaymentSettlement>>();

            //去预收中抵扣相同币种的情况下的预收款，生成收款单，并且生成核销记录
            var prePayments = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                .Where(x => (x.PrePaymentStatus == (int)PrePaymentStatus.待核销 || x.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
            && x.CustomerVendor_ID == entity.CustomerVendor_ID
            && x.IsAvailable == true
            && x.Currency_ID == entity.Currency_ID
            && x.ReceivePaymentType == entity.ReceivePaymentType
            ).OrderBy(x => x.PrePayDate) // 按创建时间排序
                .ToListAsync();

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

            result = await ApplyManualPaymentAllocation(entity, prePayments, UseTransaction);
            return result;
        }



        #endregion


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
            payable.SourceBillNo = entity.SaleOutNo;
            payable.SourceBillId = entity.SaleOut_MainID;
            payable.SourceBizType = (int)BizType.销售出库单;
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.DepartmentID.HasValue)
            {
                payable.DepartmentID = entity.tb_projectgroup.DepartmentID;
            }



            //如果部门还是没有值 则从缓存中加载,如果项目有所属部门的话
            if (payable.ProjectGroup_ID.HasValue && !payable.DepartmentID.HasValue)
            {
                var projectgroup = BizCacheHelper.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
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
                    var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
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
                var olditem = entity.tb_SaleOutDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.SubtotalTaxAmount;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.TransactionPrice;
                    details[i].LocalPayableAmount = olditem.TransactionPrice * olditem.Quantity;
                    //下面有专门的一行运费。这里加上去不明了，会导致误会。如果生成对账单的话。
                    //details[i].LocalPayableAmount+= olditem.AllocatedFreightIncome;
                }

                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;

                View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                {
                    if (prodDetail != null && obj.Unit_ID != null && obj.Unit_ID.HasValue)
                    {
                        details[i].Unit_ID = obj.Unit_ID.Value;
                    }
                }
            }

            #region 单独加一行运算到明细中 ,这里是应收。意思是收取客户的运费。应该以运费成本为标准。佣金不需要
            // 添加运费行（关键部分）
            if (entity.FreightCost > 0 && !IsProcessCommission)
            {
                details.Add(new tb_FM_ReceivablePayableDetail
                {
                    ProdDetailID = null,
                    property = "运费",
                    Specifications = "",
                    ExchangeRate = 1,
                    Description = "发货运费",
                    UnitPrice = entity.FreightCost,
                    Quantity = 1,
                    TaxRate = 1.0m, // 假设运费税率为9%
                    TaxLocalAmount = 0,
                    Summary = "",
                    LocalPayableAmount = entity.FreightCost
                });
            }

            #endregion

            payable.tb_FM_ReceivablePayableDetails = details;
            ////如果是外币时，则由外币算出本币
            //if (isRefund)
            //{
            //    //为负数，退款时设置为负数。退货，出库反审？
            //    entity.ForeignTotalAmount = -entity.ForeignTotalAmount;
            //    entity.TotalAmount = -entity.TotalAmount;
            //}

            //这里要重点思考 是本币一定有。！！！！！！！！！！！！！！！！！ TODO by watson
            //外币时  只是换算。本币不能少。
            if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
            {
                payable.ForeignBalanceAmount = entity.ForeignTotalAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = entity.ForeignTotalAmount;
            }

            //本币时
            payable.LocalBalanceAmount = entity.TotalAmount;
            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = entity.TotalAmount;
            if (IsProcessCommission)
            {
                payable.Remark = $"由销售出库单：{entity.SaleOutNo}【佣金】生成的应收款单";
            }
            else
            {
                payable.Remark = $"由销售出库单：{entity.SaleOutNo}【货款】生成的应收款单";
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
                var projectgroup = BizCacheHelper.Instance.GetEntity<tb_ProjectGroup>(entity.ProjectGroup_ID);
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

            //如果销售订单中付款方式不为空，并且是账期时
            if (entity.tb_purorder.Paytype_ID.HasValue && entity.tb_purorder.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
            {
                var obj = BizCacheHelper.Instance.GetEntity<tb_CustomerVendor>(entity.CustomerVendor_ID);
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


            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_PurEntryDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID).FirstOrDefault();
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
                View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                {
                    if (prodDetail != null && obj.Unit_ID != null && obj.Unit_ID.HasValue)
                    {
                        details[i].Unit_ID = obj.Unit_ID.Value;
                    }
                }
            }
            #region 单独加一行运算到明细中 ,这里是应收。意思是收取客户的运费。应该以运费成本为标准。
            // 添加运费行（关键部分）
            if (entity.ShipCost > 0)
            {
                details.Add(new tb_FM_ReceivablePayableDetail
                {
                    ProdDetailID = null,
                    property = "运费",
                    Specifications = "",
                    ExchangeRate = 1,
                    Description = "发货运费",
                    UnitPrice = entity.ShipCost,
                    Quantity = 1,
                    TaxRate = 1.0m, // 假设运费税率为9%
                    TaxLocalAmount = 0,
                    Summary = "",
                    LocalPayableAmount = entity.ShipCost
                });
            }
            #endregion
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

            //本币时 一定会有值。
            payable.LocalBalanceAmount = entity.TotalAmount;
            payable.LocalPaidAmount = 0;
            payable.TotalLocalPayableAmount = entity.TotalAmount;


            payable.Remark = $"采购入库单：{entity.PurEntryNo}的应付款";

            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (int)ARAPStatus.待审核;

            //var ctrpay = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
            //ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable, false);
            return payable;
        }


        public async Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model, bool UseTran = false) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            //缓存当前编辑的对象。如果撤销就回原来的值
            T oldobj = CloneHelper.DeepCloneObject<T>((T)model);

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
        public async Task<tb_FM_ReceivablePayable> BuildReceivablePayable(tb_PurEntryRe entity)
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

            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付款单);
            if (entity.Currency_ID.HasValue)
            {
                payable.Currency_ID = entity.Currency_ID.Value;
            }
            //if (entity.tb_saleorder.tb_paymentmethod.Paytype_Name == DefaultPaymentMethod.账期.ToString())
            //{
            //    if (entity.tb_customervendor.CustomerCreditDays.HasValue)
            //    {
            //        // 从销售出库日期开始计算到期日
            //        payable.DueDate = entity.OutDate.Date.AddDays(entity.tb_customervendor.CustomerCreditDays.Value).AddDays(1).AddTicks(-1);
            //    }
            //}
            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_PurEntryReDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                var olditem = entity.tb_PurEntryReDetails.Where(c => c.ProdDetailID == details[i].ProdDetailID).FirstOrDefault();
                if (olditem != null)
                {
                    details[i].TaxRate = olditem.TaxRate;
                    details[i].TaxLocalAmount = olditem.TaxAmount;
                    details[i].Quantity = olditem.Quantity;
                    details[i].UnitPrice = olditem.UnitPrice;
                    details[i].LocalPayableAmount = olditem.UnitPrice * olditem.Quantity;
                }
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;

                View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                {
                    if (prodDetail != null && obj.Unit_ID != null && obj.Unit_ID.HasValue)
                    {
                        details[i].Unit_ID = obj.Unit_ID.Value;
                    }
                }
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

            payable.Remark = $"采购入库单：{entity.PurEntryNo}对应的采购退回单{entity.PurEntryReNo}的应付款";

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
        //        _logger.Error(ex);
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
                _logger.Error(ex);
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
                             .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_FM_ReceivablePayableDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                            .ToListAsync();
            return list as List<T>;
        }




    }
}



