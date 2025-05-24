
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
using System.Windows.Forms;
using System.Text;

namespace RUINORERP.Business
{
    /// <summary>
    /// 应收应付表
    /// </summary>
    public partial class tb_FM_PriceAdjustmentController<T> : BaseController<T> where T : class
    {
        /// <summary>
        ///  
        ///  
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_FM_PriceAdjustment entity = ObjectEntity as tb_FM_PriceAdjustment;

            try
            {
                //只有生效状态的才允许反审，其它不能也不需要，有可能可删除。也可能只能红冲
                if (entity.DataStatus != (long)DataStatus.确认)
                {
                    rmrs.ErrorMsg = "只有【已确认】审核通过状态的价格调整单才可以反审";
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //应收应付 审核只是业务性确认，不会产生收会款单。通过对账单 确认，客户付款了才会产生付款单 应收右键生成收款单
                ////注意，反审是将只有收款单没有审核前，删除
                ////删除
                //if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                //{
                //    await _appContext.Db.Deleteable<tb_FM_PaymentRecord>().Where(c => c.SourceBilllID == entity.AdjustId && c.BizType == (int)BizType.应收单).ExecuteCommandAsync();
                //}
                //else
                //{
                //    await _appContext.Db.Deleteable<tb_FM_PaymentRecord>().Where(c => c.SourceBilllID == entity.AdjustId && c.BizType == (int)BizType.应付单).ExecuteCommandAsync();
                //}
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PriceAdjustment>(entity).ExecuteCommandAsync();
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
        /// 审核通过时生成关联的收款/付款草稿单 财务核对是否到账。
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> ApprovalResult = new ReturnResults<T>();
            tb_FM_PriceAdjustment entity = ObjectEntity as tb_FM_PriceAdjustment;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();


                var paymentController = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                //如果已经生成过的话，不能再次生成。
                string payTypeText = string.Empty;
                if (entity.ReceivePaymentType == (int)ReceivePaymentType.收款)
                {
                    payTypeText = "应收款单";
                }
                else
                {
                    payTypeText = "应付款单";
                }
                bool isExist = await paymentController.IsExistAsync(c => c.SourceBillId.Value == entity.AdjustId);
                if (isExist)
                {
                    ApprovalResult.ErrorMsg = $"当前价格调整单{entity.AdjustNo},已生成过{payTypeText}，不能再次生成";
                    return ApprovalResult;
                }
                else
                {
                    //生成应收应付，如果是采购后面生成付款单，则会影响入库成本哦。销售则影响利润哦。
                    tb_FM_ReceivablePayable payable = await paymentController.CreateReceivablePayable(entity);
                    ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(payable);
                    if (rmr.Succeeded)
                    {
                        var paybalbe = rmr.ReturnObject as tb_FM_ReceivablePayable;
                        paybalbe.ApprovalOpinions = $"价格调整单{entity.AdjustNo}审核时，自动创建并审核通过";
                        ReturnResults<tb_FM_ReceivablePayable> rr = await paymentController.ApprovalAsync(paybalbe);
                        if (rr.Succeeded)
                        {
                            rmr.Succeeded = false;
                        }
                        else
                        {
                            ApprovalResult.ErrorMsg = rr.ErrorMsg;
                            ApprovalResult.Succeeded = false;
                            throw new Exception("保存应收应付单失败");
                        }
                    }
                    else
                    {
                        ApprovalResult.ErrorMsg = rmr.ErrorMsg;
                        ApprovalResult.Succeeded = false;
                        throw new Exception("审核应收应付单失败");
                    }
                }

                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;

                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.FMPaymentStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PriceAdjustment>(entity).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                ApprovalResult.Succeeded = true;
                ApprovalResult.ReturnObject = entity as T;
                return ApprovalResult;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                ApprovalResult.ErrorMsg += ex.Message;
                return ApprovalResult;
            }
        }







        public async Task<bool> BaseLogicDeleteAsync(tb_FM_PriceAdjustment ObjectEntity)
        {
            //  ReturnResults<tb_FM_PriceAdjustmentController> rrs = new Business.ReturnResults<tb_FM_PriceAdjustmentController>();
            int count = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PriceAdjustment>(ObjectEntity).IsLogic().ExecuteCommandAsync();
            if (count > 0)
            {
                //rrs.Succeeded = true;
                return true;
                ////生成时暂时只考虑了一个主键的情况
                // MyCacheManager.Instance.DeleteEntityList<tb_FM_PriceAdjustmentController>(entity);
            }
            return false;
        }

        ///// <summary>
        ///// 要生成收付单 没完成
        ///// </summary>
        ///// <param name="entitys"></param>
        ///// <returns></returns>
        //public async virtual Task<bool> BatchApproval(List<tb_FM_PriceAdjustment> entitys, ApprovalEntity approvalEntity)
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
        //                await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_PriceAdjustment>(entity).ExecuteCommandAsync();
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
            List<tb_FM_PriceAdjustment> entitys = new List<tb_FM_PriceAdjustment>();
            entitys = NeedCloseCaseList as List<tb_FM_PriceAdjustment>;

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
                        .Updateable<tb_FM_PriceAdjustment>(entitys[m])
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


        public async Task<tb_FM_PriceAdjustment> CreatePriceAdjustment(ReceivePaymentType PaymentType, long sourceBillID, string NewBillNo = "")
        {
            tb_FM_PriceAdjustment priceAdjustment = new tb_FM_PriceAdjustment();

            if (PaymentType == ReceivePaymentType.收款)
            {
                #region
                tb_SaleOut SourceBill = await _appContext.Db.Queryable<tb_SaleOut>().Where(c => c.SaleOut_MainID == sourceBillID)
                     .Includes(a => a.tb_SaleOutDetails, b => b.tb_proddetail, c => c.tb_prod)
                    .SingleAsync();

                //新增时才可以转单
                if (SourceBill != null)
                {
                    priceAdjustment = mapper.Map<tb_FM_PriceAdjustment>(SourceBill);
                    List<tb_FM_PriceAdjustmentDetail> details = mapper.Map<List<tb_FM_PriceAdjustmentDetail>>(SourceBill.tb_SaleOutDetails);
                    priceAdjustment.SourceBillId = sourceBillID;
                    priceAdjustment.SourceBillNo = SourceBill.SaleOutNo;
                    priceAdjustment.SourceBizType = (int)BizType.销售出库单;
                    if (!string.IsNullOrEmpty(NewBillNo))
                    {
                        priceAdjustment.AdjustNo = NewBillNo;
                    }
                    else
                    {
                        priceAdjustment.AdjustNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售价格调整单);
                    }
                    priceAdjustment.AdjustDate = System.DateTime.Now;
                    priceAdjustment.ApprovalOpinions = "";
                    priceAdjustment.ApprovalResults = null;
                    priceAdjustment.DataStatus = (int)DataStatus.草稿;
                    priceAdjustment.ApprovalStatus = (int)ApprovalStatus.未审核;
                    priceAdjustment.Approver_at = null;
                    priceAdjustment.Approver_by = null;
                    priceAdjustment.PrintStatus = 0;
                    priceAdjustment.ActionStatus = ActionStatus.新增;
                    priceAdjustment.ApprovalOpinions = "";
                    priceAdjustment.Modified_at = null;
                    priceAdjustment.Modified_by = null;
                    List<tb_FM_PriceAdjustmentDetail> NewDetails = new List<tb_FM_PriceAdjustmentDetail>();
                    List<string> tipsMsg = new List<string>();
                    for (global::System.Int32 i = 0; i < details.Count; i++)
                    {
                        var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0)
                        {
                            //来源单据明细中有相同产品多行出库时，请合并调整价格。
                            tipsMsg.Add($"来源单据明细中有相同产品多行出库时，请合并调整价格");
                        }
                        else
                        {
                            #region 每行产品ID唯一
                            tb_SaleOutDetail item = SourceBill.tb_SaleOutDetails
                                .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                            details[i].OriginalUnitPrice = item.UnitPrice;
                            NewDetails.Add(details[i]);
                            //tb_SaleOutDetail item = SourceBill.tb_SaleOutDetails
                            //    .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                            //details[i].Quantity = item.Quantity;// 已经交数量去掉
                            //details[i].SubtotalAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;
                            //if (details[i].Quantity > 0)
                            //{
                            //    NewDetails.Add(details[i]);
                            //}
                            //else
                            //{
                            //    tipsMsg.Add($"订单{purorder.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                            //}
                            #endregion
                        }

                    }

                    //if (NewDetails.Count == 0)
                    //{
                    //    tipsMsg.Add($"订单:{priceAdjustment.PurOrder_NO}已全部入库，请检查是否正在重复入库！");
                    //}

                    StringBuilder msg = new StringBuilder();
                    foreach (var item in tipsMsg)
                    {
                        msg.Append(item).Append("\r\n");

                    }
                    if (tipsMsg.Count > 0)
                    {
                        MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    priceAdjustment.tb_FM_PriceAdjustmentDetails = NewDetails;

                }
                #endregion
            }
            else
            {
                #region
                tb_PurEntry SourceBill = await _appContext.Db.Queryable<tb_PurEntry>().Where(c => c.PurEntryID == sourceBillID)
                     .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod)
                    .SingleAsync();

                //新增时才可以转单
                if (SourceBill != null)
                {
                    priceAdjustment = mapper.Map<tb_FM_PriceAdjustment>(SourceBill);
                    List<tb_FM_PriceAdjustmentDetail> details = mapper.Map<List<tb_FM_PriceAdjustmentDetail>>(SourceBill.tb_PurEntryDetails);
                    priceAdjustment.SourceBillId = sourceBillID;
                    priceAdjustment.SourceBillNo = SourceBill.PurEntryNo;
                    priceAdjustment.SourceBizType = (int)BizType.采购入库单;
                    if (!string.IsNullOrEmpty(NewBillNo))
                    {
                        priceAdjustment.AdjustNo = NewBillNo;
                    }
                    else
                    {
                        priceAdjustment.AdjustNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购价格调整单);
                    }
                    priceAdjustment.AdjustDate = System.DateTime.Now;
                    priceAdjustment.ApprovalOpinions = "";
                    priceAdjustment.ApprovalResults = null;
                    priceAdjustment.DataStatus = (int)DataStatus.草稿;
                    priceAdjustment.ApprovalStatus = (int)ApprovalStatus.未审核;
                    priceAdjustment.Approver_at = null;
                    priceAdjustment.Approver_by = null;
                    priceAdjustment.PrintStatus = 0;
                    priceAdjustment.ActionStatus = ActionStatus.新增;
                    priceAdjustment.ApprovalOpinions = "";
                    priceAdjustment.Modified_at = null;
                    priceAdjustment.Modified_by = null;
                    List<tb_FM_PriceAdjustmentDetail> NewDetails = new List<tb_FM_PriceAdjustmentDetail>();
                    List<string> tipsMsg = new List<string>();
                    for (global::System.Int32 i = 0; i < details.Count; i++)
                    {
                        var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0)
                        {
                            //来源单据明细中有相同产品多行出库时，请合并调整价格。
                            tipsMsg.Add($"来源单据明细中有相同产品多行出库时，请合并调整价格");
                        }
                        else
                        {
                            #region 每行产品ID唯一
                            tb_PurEntryDetail item = SourceBill.tb_PurEntryDetails
                                .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                            details[i].OriginalUnitPrice = item.UnitPrice;
                            NewDetails.Add(details[i]);
                            //tb_PurEntryDetail item = SourceBill.tb_PurEntryDetails
                            //    .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID);
                            //details[i].Quantity = item.Quantity;// 已经交数量去掉
                            //details[i].SubtotalAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;
                            //if (details[i].Quantity > 0)
                            //{
                            //    NewDetails.Add(details[i]);
                            //}
                            //else
                            //{
                            //    tipsMsg.Add($"订单{purorder.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                            //}
                            #endregion
                        }

                    }

                    //if (NewDetails.Count == 0)
                    //{
                    //    tipsMsg.Add($"订单:{priceAdjustment.PurOrder_NO}已全部入库，请检查是否正在重复入库！");
                    //}

                    StringBuilder msg = new StringBuilder();
                    foreach (var item in tipsMsg)
                    {
                        msg.Append(item).Append("\r\n");

                    }
                    if (tipsMsg.Count > 0)
                    {
                        MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    priceAdjustment.tb_FM_PriceAdjustmentDetails = NewDetails;

                }
                #endregion
            }
            return priceAdjustment;
        }
        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_FM_PriceAdjustment> list = await _appContext.Db.CopyNew().Queryable<tb_FM_PriceAdjustment>()
                .Where(m => m.AdjustId == ID)
                            .Includes(a => a.tb_currency)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_department)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_FM_PriceAdjustmentDetails)
                            .ToListAsync();
            return list as List<T>;
        }




    }
}



