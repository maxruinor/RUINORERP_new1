
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
using OfficeOpenXml.Export.ToDataTable;

namespace RUINORERP.Business
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    public partial class tb_FM_PreReceivedPaymentController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 客户取消订单时
        /// 生成退款单到 tb_FM_PaymentRecord
        /// 客户取消订单时，需要在 tb_FM_PaymentRecord 生成退款单并生成 tb_FM_PaymentSettlement
        /// 预收款单（BizType=84）和预付款单（BizType=85）审核后同样代表资金已实际收付（预收款已到账、预付款已支付），不可直接反审核，需通过红冲机制处理。
        /// 所以这里由销售订单取消时调用。UI上没有反审核了。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        [Obsolete]
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //审核就是收款 或 付款了 ，则生成退款单  （负数的收款单）
                bool isRefund = true;
                tb_FM_PaymentRecordController<tb_FM_PaymentRecord> settlementController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                tb_FM_PaymentRecord paymentRecord = await settlementController.CreatePaymentRecord(entity, isRefund);
                //这个状态要退款单审核后回写
                //entity.FMPaymentStatus = (int)FMPaymentStatus.已冲销;//退款  余额有多少退多少。

                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
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
        /// 这个审核可以由业务来审。后面还会有财务来定是否真实收付
        /// 审核通过时
        /// 预收款单本身是「收款」的一种业务类型，要生成收款单，通过 BizType 标记其业务属性为预收款。
        /// tb_FM_PaymentSettlement 不需要立即生成，但需在后续触发核销时生成（抵扣时生成）。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PreReceivedPayment entity = ObjectEntity as tb_FM_PreReceivedPayment;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();


                tb_FM_PaymentRecordController<tb_FM_PaymentRecord> settlementController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();

                tb_FM_PaymentRecord paymentRecord = await settlementController.CreatePaymentRecord(entity, false);
                //确认收到款  应该是收款审核时 反写回来
                //if (paymentRecord.PaymentId > 0)
                //{
                //    entity.ForeignBalanceAmount = entity.ForeignPrepaidAmount;
                //    entity.LocalBalanceAmount = entity.LocalPrepaidAmount;
                //}

                entity.PrePaymentStatus = (long)PrePaymentStatus.已生效;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PreReceivedPayment>(entity).ExecuteCommandAsync();
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
        //                entity.PrePaymentStatus = (long)PrePaymentStatus.已生效;
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
            List<tb_FM_PreReceivedPayment> entitys = new List<tb_FM_PreReceivedPayment>();
            entitys = NeedCloseCaseList as List<tb_FM_PreReceivedPayment>;

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
                        .Updateable<tb_FM_PreReceivedPayment>(entitys[m])
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

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PreReceivedPayment> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PreReceivedPayment>()
                .Where(m => m.PreRPID == ID)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_paymentmethod)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_fm_account)
                            .ToListAsync();
            return list as List<T>;
        }




    }

}



