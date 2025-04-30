
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:35
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
using RUINORERP.Global;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Extensions;
using AutoMapper;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using Fireasy.Common.Extensions;

namespace RUINORERP.Business
{
    public partial class tb_PurOrderController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 批量结案  销售出库标记结案，数据状态为8,可以修改付款状态，同时检测销售订单的付款状态，也可以更新销售订单付款状态
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_PurOrder> entitys = new List<tb_PurOrder>();
            entitys = NeedCloseCaseList as List<tb_PurOrder>;


            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案
                foreach (var entity in entitys)
                {
                    //结案的出库单。先要是审核成功通过的
                    if (entity.DataStatus == (int)DataStatus.确认 && (entity.ApprovalStatus.HasValue && entity.ApprovalStatus.Value == (int)ApprovalStatus.已审核 && entity.ApprovalResults.Value))
                    {

                        //更新在途库存
                        //如果采购明细中的入库数量小于订单中数量，则在途数量要减去这个差值,比方说采购入库只入了一半，那么在途库存就要减去这个差值，另一半可能不要了。
                        if (entity.tb_PurOrderDetails.Select(c => c.DeliveredQuantity).Sum() < entity.tb_PurOrderDetails.Select(c => c.Quantity).Sum())
                        {
                            tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                #region 库存表的更新 这里应该是必需有库存的数据，
                                tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                                if (inv == null)
                                {
                                    inv = new tb_Inventory();
                                    inv.ProdDetailID = child.ProdDetailID;
                                    inv.Location_ID = child.Location_ID;
                                    inv.Quantity = 0;
                                    inv.InitInventory = (int)inv.Quantity;
                                    inv.Notes = "采购订单创建";//后面修改数据库是不需要？
                                                         //inv.LatestStorageTime = System.DateTime.Now;
                                    BusinessHelper.Instance.InitEntity(inv);
                                }
                                //更新在途库存
                                inv.On_the_way_Qty -= (child.Quantity - child.DeliveredQuantity);
                                BusinessHelper.Instance.EditEntity(inv);
                                #endregion
                                ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                                if (rr.Succeeded)
                                {

                                }
                            }
                        }


                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
                            it.CloseCaseOpinions,
                            it.Paytype_ID,
                            it.Modified_by,
                            it.Modified_at
                        }).ExecuteCommandAsync();

                    }
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




        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_PurOrder entity = ObjectEntity as tb_PurOrder;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                if (entity == null)
                {
                    return rmrs;
                }


                //如果采购订单明细数据来自于请购单，则明细要回写状态为已采购
                if (entity.RefBillID.HasValue && entity.RefBillID.Value > 0)
                {
                    if (entity.RefBizType == (int)BizType.请购单)
                    {
                        tb_BuyingRequisition buyingRequisition = _appContext.Db.Queryable<tb_BuyingRequisition>()
                            .Includes(c => c.tb_BuyingRequisitionDetails)
                            .Where(c => c.PuRequisition_ID == entity.RefBillID).Single();
                        if (buyingRequisition != null)
                        {

                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                var buyItem = buyingRequisition.tb_BuyingRequisitionDetails
                                    .FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                                if (buyItem != null)//为空则是买的东西不在请购单明细中。
                                {
                                    buyItem.Purchased = true;
                                    buyItem.HasChanged = true;
                                }
                            }
                            await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisitionDetail>(buyingRequisition.tb_BuyingRequisitionDetails).ExecuteCommandAsync();
                        }
                    }
                }
                foreach (var child in entity.tb_PurOrderDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        //采购和销售都会提前处理。所以这里默认提供一行数据。成本和数量都可能为0
                        inv = new tb_Inventory();
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;
                        inv.Quantity = 0;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "采购订单创建";//后面修改数据库是不需要？
                                             //inv.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    //更新在途库存
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }

                    //
                }


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region 生成预付款单

                    if (entity.tb_paymentmethod == null)
                    {
                        var obj = BizCacheHelper.Instance.GetEntity<tb_PaymentMethod>(entity.Paytype_ID.Value);
                        if (obj != null && obj.ToString() != "System.Object")
                        {
                            entity.tb_paymentmethod = obj as tb_PaymentMethod;
                        }
                        if (entity.tb_paymentmethod == null)
                        {
                            entity.tb_paymentmethod = await _appContext.Db.Queryable<tb_PaymentMethod>().Where(c => c.Paytype_ID == entity.Paytype_ID).FirstAsync();
                        }
                    }

                    //如果是账期必须是未付款
                    if (entity.tb_paymentmethod.Paytype_Name == DefaultPaymentMethod.账期.ToString())
                    {
                        if (entity.PayStatus != (int)PayStatus.未付款)
                        {
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"付款方式为账期的订单必须是未付款！审核失败。";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }

                    if (entity.PayStatus == (int)PayStatus.未付款)
                    {
                        if (entity.tb_paymentmethod.Paytype_Name != DefaultPaymentMethod.账期.ToString())
                        {
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"未付款订单的付款方式必须是账期！审核失败。";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }
                    // 外币相关处理 正确是 外币时一定要有汇率
                    decimal exchangeRate = 1; // 获取销售订单的汇率
                    if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
                    {
                        exchangeRate = entity.ExchangeRate.Value; // 获取销售订单的汇率
                                                                  // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                                  // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
                    }

                    //销售订单审核时，非账期，即时收款时，生成预收款。 订金，部分收款
                    if (entity.tb_paymentmethod.Paytype_Name != DefaultPaymentMethod.账期.ToString())
                    {
                        tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment> ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                        tb_FM_PreReceivedPayment payable = new tb_FM_PreReceivedPayment();
                        IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
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
                        if (entity.tb_projectgroup != null && entity.tb_projectgroup.tb_department != null)
                        {
                            payable.DepartmentID = entity.tb_projectgroup.tb_department.DepartmentID;
                        }
                        //采购就是付款
                        payable.ReceivePaymentType = (int)ReceivePaymentType.付款;
                        payable.PreRPNO = BizCodeGenerator.Instance.GetBizBillNo(BizType.预付款单);
                        payable.BizType = (int)BizType.采购订单;
                        payable.SourceBillNO = entity.PurOrderNo;
                        payable.SourceBill_ID = entity.PurOrder_ID;
                        payable.Currency_ID = entity.Currency_ID;
                        payable.PrePayDate = entity.PurDate;
                        payable.ExchangeRate = exchangeRate;
                        payable.LocalPrepaidAmountInWords = string.Empty;
                        //payable.Account_id = entity.Account_id; 付款账户信息 在采购订单时 不用填写。由财务决定 
                        //如果是外币时，则由外币算出本币
                        if (entity.PayStatus == (int)PayStatus.全部付款)
                        {
                            //外币时
                            if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
                            {
                                payable.ForeignPrepaidAmount = entity.ForeignTotalAmount;
                                payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                            }
                            else
                            {
                                //本币时
                                payable.LocalPrepaidAmount = entity.TotalAmount;
                            }
                        }
                        //来自于订金
                        if (entity.PayStatus == (int)PayStatus.部分付款)
                        {
                            //外币时
                            if (entity.Currency_ID.HasValue && _appContext.BaseCurrency.Currency_ID != entity.Currency_ID.Value)
                            {
                                payable.ForeignPrepaidAmount = entity.ForeignDeposit;
                                payable.LocalPrepaidAmount = payable.ForeignPrepaidAmount * exchangeRate;
                            }
                            else
                            {
                                payable.LocalPrepaidAmount = entity.Deposit;
                            }
                        }

                        payable.PrePaymentReason = $"采购订单{entity.PurOrderNo}的预付款";
                        Business.BusinessHelper.Instance.InitEntity(payable);
                        payable.PrePaymentStatus = (long)PrePaymentStatus.待审核;
                        ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrpay.SaveOrUpdate(payable);
                        if (rmpay.Succeeded)
                        {
                            // 预付款单生成成功后的处理逻辑
                        }
                        else
                        {
                            // 处理预收款单生成失败的情况
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"预付款单生成失败：{rmpay.ErrorMsg ?? "未知错误"}";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }

                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //   entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //   entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                //_logger.Info(approvalEntity.bizName + "审核事务成功");
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;

                return rmrs;
            }

        }


        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_PurOrder entity = ObjectEntity as tb_PurOrder;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {

                //判断是否能反审?
                if (entity.tb_PurEntries != null
                    && (entity.tb_PurEntries.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntries.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购入库单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }



                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //如果采购订单明细数据来自于请购单，则明细要回写状态为已采购
                if (entity.RefBillID.HasValue && entity.RefBillID.Value > 0)
                {
                    if (entity.RefBizType == (int)BizType.请购单)
                    {
                        tb_BuyingRequisition buyingRequisition = _appContext.Db.Queryable<tb_BuyingRequisition>()
                            .Includes(c => c.tb_BuyingRequisitionDetails)
                            .Where(c => c.PuRequisition_ID == entity.RefBillID).Single();
                        if (buyingRequisition != null)
                        {

                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                var buyItem = buyingRequisition.tb_BuyingRequisitionDetails
                                    .FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                                buyItem.Purchased = false;
                            }

                            await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisitionDetail>(buyingRequisition.tb_BuyingRequisitionDetails).ExecuteCommandAsync();

                        }
                    }
                }

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                foreach (var child in entity.tb_PurOrderDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;
                        inv.Quantity = 0;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                                       //inv.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    //更新在途库存
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }


        /// <summary>
        /// 转换为采购入库单,注意一个订单可以多次转成入库单。
        /// </summary>
        /// <param name="order"></param>
        public tb_PurEntry PurOrderTotb_PurEntry(tb_PurOrder order)
        {
            tb_PurEntry entity = new tb_PurEntry();
            //转单
            if (order != null)
            {
                IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();

                entity = mapper.Map<tb_PurEntry>(order);
                List<tb_PurEntryDetail> details = mapper.Map<List<tb_PurEntryDetail>>(order.tb_PurOrderDetails);
                //转单要TODO
                //转换时，默认认为订单出库数量就等于这次出库数量，是否多个订单累计？，如果是UI录单。则只是默认这个数量。也可以手工修改
                List<tb_PurEntryDetail> NewDetails = new List<tb_PurEntryDetail>();

                List<string> tipsMsg = new List<string>();
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].PurOrder_ChildID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_PurOrderDetail item = order.tb_PurOrderDetails
                            .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                            && c.Location_ID == details[i].Location_ID
                            && c.PurOrder_ChildID == details[i].PurOrder_ChildID);
                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = details[i].TransactionPrice * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{order.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一

                        tb_PurOrderDetail item = order.tb_PurOrderDetails
                            .FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                            && c.Location_ID == details[i].Location_ID
                            );
                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = details[i].TransactionPrice * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{order.PurOrderNo}，{item.tb_proddetail.tb_prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }
                        #endregion
                    }

                }

                /*
                foreach (tb_PurEntryDetail item in details)
                {
                    tb_PurOrderDetail orderDetail = new tb_PurOrderDetail();
                    orderDetail = order.tb_PurOrderDetails.FirstOrDefault<tb_PurOrderDetail>(c => c.ProdDetailID == item.ProdDetailID);
                    if (orderDetail != null)
                    {
                        //已经入库数量等于已经入库数量则认为这项全入库了，不再出
                        if (orderDetail.DeliveredQuantity == item.Quantity)
                        {
                            continue;
                        }
                        else
                        {
                            item.Quantity = item.Quantity - orderDetail.DeliveredQuantity;
                            NewDetails.Add(item);
                        }
                    }

                }
                */



                entity.tb_PurEntryDetails = NewDetails;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.ActionStatus = ActionStatus.新增;

                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.TotalTaxAmount = NewDetails.Sum(c => c.TaxAmount);
                entity.ActualAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.tb_PurEntryDetails = NewDetails;
                entity.PurOrder_ID = order.PurOrder_ID;
                entity.PurOrder_NO = order.PurOrderNo;
                entity.TotalAmount = entity.TotalAmount + entity.ShippingCost;
                entity.ActualAmount = entity.ShippingCost + entity.TotalAmount;

                //if (order.Arrival_date.HasValue)
                //{
                //    entity.EntryDate = order.Arrival_date.Value;
                //}
                //else
                //{
                entity.EntryDate = System.DateTime.Now;
                //}

                entity.PrintStatus = 0;
                BusinessHelper.Instance.InitEntity(entity);

                if (entity.PurOrder_ID.HasValue && entity.PurOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = order.CustomerVendor_ID;
                    entity.PurOrder_NO = order.PurOrderNo;
                }
                entity.PurEntryNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购入库单);
                //保存到数据库
                BusinessHelper.Instance.InitEntity(entity);
            }
            return entity;
        }
        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            //var queryable = _appContext.Db.Queryable<tb_SaleOrderDetail>();
            //var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();
            List<tb_PurOrder> list = await _appContext.Db.CopyNew().Queryable<tb_PurOrder>().Where(m => m.PurOrder_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }




    }

}



