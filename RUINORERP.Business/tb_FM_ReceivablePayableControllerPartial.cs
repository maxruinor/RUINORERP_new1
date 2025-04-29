
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 应收应付表
    /// </summary>
    public partial class tb_FM_ReceivablePayableController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 客户取消订单时，如果有订单，如果财务没有在他对应的收付单里审核前是可以反审的。否则只能通过红冲机制处理。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ReceivablePayable entity = ObjectEntity as tb_FM_ReceivablePayable;

            try
            {
                //只有生效状态的才允许反审，其它不能也不需要，有可能可删除。也可能只能红冲
                if (entity.ARAPStatus != (long)ARAPStatus.已生效)
                {
                    if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                    {
                        rmrs.ErrorMsg = "只有【已生效】状态的预收款单才可以反审";
                    }
                    else
                    {
                        rmrs.ErrorMsg = "只有【已生效】状态的预付款单才可以反审";
                    }

                    return rmrs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //注意，反审是将只有收款单没有审核前，删除
                //删除
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    await _appContext.Db.Deleteable<tb_FM_PaymentRecord>().Where(c => c.SourceBilllID == entity.PreRPID && c.BizType == (int)BizType.应收单).ExecuteCommandAsync();
                }
                else
                {
                    await _appContext.Db.Deleteable<tb_FM_PaymentRecord>().Where(c => c.SourceBilllID == entity.PreRPID && c.BizType == (int)BizType.应付单).ExecuteCommandAsync();
                }
                entity.ARAPStatus = (long)ARAPStatus.草稿;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(entity).ExecuteCommandAsync();
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
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付，这财务审核收款单前，还是可以反审的
        /// 审核通过时
        /// 预收款单本身是「收款」的一种业务类型，要生成收款单，通过 BizType 标记其业务属性为预收款。
        /// tb_FM_PaymentSettlement 不需要立即生成，但需在后续触发核销时生成（抵扣时生成）。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_ReceivablePayable entity = ObjectEntity as tb_FM_ReceivablePayable;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_FM_PaymentRecordController<tb_FM_PaymentRecord> settlementController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                tb_FM_PaymentRecord paymentRecord = await settlementController.CreatePaymentRecord(entity, false);
                //确认收到款  应该是收款审核时 反写回来 成 【待核销】
                //if (paymentRecord.PaymentId > 0)
                //{
                //    entity.ForeignBalanceAmount = entity.ForeignPrepaidAmount;
                //    entity.LocalBalanceAmount = entity.LocalPrepaidAmount;
                //}

                entity.ARAPStatus = (long)ARAPStatus.已生效;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayable>(entity).ExecuteCommandAsync();
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
        /// 创建为红字冲销 应收款单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReturnMainSubResults<tb_FM_ReceivablePayable>> CreateReceivablePayable(tb_SaleOutRe entity)
        {

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
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
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
            }
            //销售就是收款
            payable.ReceivePaymentType = (int)ReceivePaymentType.收款;

            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收单);
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

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_SaleOutReDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                details[i].SourceBillNO = entity.ReturnNo;
                details[i].SourceBill_ID = entity.SaleOutRe_ID;
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;
                details[i].BizType = (int)BizType.销售退回单;
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
                payable.ForeignBalanceAmount = entity.ForeignTotalAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = entity.ForeignTotalAmount;
            }
            else
            {
                //本币时
                payable.LocalBalanceAmount = entity.TotalAmount;
                payable.LocalPaidAmount = 0;
                payable.TotalLocalPayableAmount = entity.TotalAmount;
            }

            payable.Remark = $"销售出库单：{entity.SaleOut_NO}对应的销售退回单{entity.ReturnNo}的应退款";

            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (long)ARAPStatus.待审核;
            BaseController<tb_FM_ReceivablePayable> ctrpay = _appContext.GetRequiredServiceByName<BaseController<tb_FM_ReceivablePayable>>(typeof(T).Name + "Controller");
            ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable);
            return rmr;
        }

        /// <summary>
        /// 创建应收款单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isRefund">true为红字冲销</param>
        /// <returns></returns>
        public async Task<ReturnMainSubResults<tb_FM_ReceivablePayable>> CreateReceivablePayable(tb_SaleOut entity, bool isRefund)
        {

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
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
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
            }
            //销售就是收款
            payable.ReceivePaymentType = (int)ReceivePaymentType.收款;

            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应收单);
            if (entity.Currency_ID.HasValue)
            {
                payable.Currency_ID = entity.Currency_ID.Value;
            }
            if (entity.tb_saleorder.tb_paymentmethod.Paytype_Name == DefaultPaymentMethod.账期.ToString())
            {
                if (entity.tb_customervendor.CustomerCreditDays.HasValue)
                {
                    // 从销售出库日期开始计算到期日
                    payable.DueDate = entity.OutDate.Date.AddDays(entity.tb_customervendor.CustomerCreditDays.Value).AddDays(1).AddTicks(-1);
                }
            }
            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_SaleOutDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                details[i].SourceBillNO = entity.SaleOutNo;
                details[i].SourceBill_ID = entity.SaleOut_MainID;
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;
                details[i].BizType = (int)BizType.销售出库单;
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
            if (isRefund)
            {
                //为负数
                entity.ForeignTotalAmount = -entity.ForeignTotalAmount;
                entity.TotalAmount = -entity.TotalAmount;
            }
            //外币时
            if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
            {
                payable.ForeignBalanceAmount = entity.ForeignTotalAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = entity.ForeignTotalAmount;
            }
            else
            {
                //本币时
                payable.LocalBalanceAmount = entity.TotalAmount;
                payable.LocalPaidAmount = 0;
                payable.TotalLocalPayableAmount = entity.TotalAmount;
            }

            payable.Remark = $"销售出库单：{entity.SaleOutNo}的应收款";

            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (long)ARAPStatus.待审核;
            BaseController<tb_FM_ReceivablePayable> ctrpay = _appContext.GetRequiredServiceByName<BaseController<tb_FM_ReceivablePayable>>(typeof(T).Name + "Controller");
            ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable);
            return rmr;
        }

        /// <summary>
        /// 创建应付款单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isRefund">true为红字冲销</param>
        /// <returns></returns>
        public async Task<ReturnMainSubResults<tb_FM_ReceivablePayable>> CreateReceivablePayable(tb_PurEntry entity, bool isRefund)
        {
            //入库时，全部生成应付，账期的。就加上到期日
            //有付款过的。就去预付中抵扣，不够的金额及状态标识出来生成对账单

            tb_FM_ReceivablePayable payable = new tb_FM_ReceivablePayable();
            IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
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
            if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
            {
                payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
            }
            //采购就是付款
            payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
            payable.ARAPNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.应付单);
            if (entity.Currency_ID.HasValue)
            {
                payable.Currency_ID = entity.Currency_ID.Value;
            }
            if (entity.tb_purorder.tb_paymentmethod.Paytype_Name == DefaultPaymentMethod.账期.ToString())
            {
                if (entity.tb_customervendor.SupplierCreditDays.HasValue)
                {
                    // 从销售出库日期开始计算到期日
                    payable.DueDate = entity.EntryDate.Date.AddDays(entity.tb_customervendor.SupplierCreditDays.Value).AddDays(1).AddTicks(-1);
                }
            }
            payable.ExchangeRate = entity.ExchangeRate;

            List<tb_FM_ReceivablePayableDetail> details = mapper.Map<List<tb_FM_ReceivablePayableDetail>>(entity.tb_PurEntryDetails);

            for (global::System.Int32 i = 0; i < details.Count; i++)
            {
                details[i].SourceBillNO = entity.PurEntryNo;
                details[i].SourceBill_ID = entity.PurEntryID;
                details[i].ExchangeRate = entity.ExchangeRate;
                details[i].ActionStatus = ActionStatus.新增;
                details[i].BizType = (int)BizType.采购入库单;
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
            if (isRefund)
            {
                //为负数
                entity.ForeignTotalAmount = -entity.ForeignTotalAmount;
                entity.TotalAmount = -entity.TotalAmount;
            }
            //外币时
            if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
            {
                payable.ForeignBalanceAmount = entity.ForeignTotalAmount;
                payable.ForeignPaidAmount = 0;
                payable.TotalForeignPayableAmount = entity.ForeignTotalAmount;
            }
            else
            {
                //本币时
                payable.LocalBalanceAmount = entity.TotalAmount;
                payable.LocalPaidAmount = 0;
                payable.TotalLocalPayableAmount = entity.TotalAmount;
            }

            payable.Remark = $"采购入库单：{entity.PurEntryNo}的应付款";

            Business.BusinessHelper.Instance.InitEntity(payable);
            payable.ARAPStatus = (long)ARAPStatus.待审核;

            BaseController<tb_FM_ReceivablePayable> ctrpay = _appContext.GetRequiredServiceByName<BaseController<tb_FM_ReceivablePayable>>(typeof(T).Name + "Controller");
            ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpay.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable);
            return rmr;
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
        //                entity.PrePaymentStatus = (long)PrePaymentStatus.已生效;
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
                    if (entitys[m].PrePaymentStatus != (long)PrePaymentStatus.已冲销 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].PrePaymentStatus = (long)PrePaymentStatus.已冲销;
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
                .Where(m => m.PreRPID == ID)
                            .Includes(a => a.tb_fm_account)
                            .Includes(a => a.tb_fm_payeeinfo)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_FM_ReceivablePayableDetails)
                            .ToListAsync();
            return list as List<T>;
        }




    }
}



