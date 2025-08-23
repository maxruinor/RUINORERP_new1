
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
using System.Collections;
using RUINORERP.Business.BizMapperService;

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
                            List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
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
                                invUpdateList.Add(inv);

                            }


                            DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                            var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                            if (Counter == 0)
                            {
                                _logger.LogInformation($"{entity.PurOrderNo}更新库存结果为0行，请检查数据！");
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



        //  采购预付款（预付）	- 生成预付单
        //- 收货后核销预付 → 冲抵应付	- 预收付表：减少 RemainAmount
        //- 应收应付表（应付）：减少 TotalAmount
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_PurOrder entity = ObjectEntity as tb_PurOrder;
            if (entity == null)
            {
                return rmrs;
            }
            try
            {
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //如果采购订单明细数据来自于请购单，则明细要回写状态为已采购
                //if (entity.RefBillID.HasValue && entity.RefBillID.Value > 0)
                //{
                //    if (entity.RefBizType == (int)BizType.请购单)
                //    {
                //        tb_BuyingRequisition buyingRequisition = _appContext.Db.Queryable<tb_BuyingRequisition>()
                //            .Includes(c => c.tb_BuyingRequisitionDetails)
                //            .Where(c => c.PuRequisition_ID == entity.RefBillID).Single();
                //        if (buyingRequisition != null)
                //        {
                //            foreach (var child in entity.tb_PurOrderDetails)
                //            {
                //                var buyItem = buyingRequisition.tb_BuyingRequisitionDetails
                //                    .FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                //                if (buyItem != null)//为空则是买的东西不在请购单明细中。
                //                {
                //                    buyItem.Purchased = true;
                //                    buyItem.HasChanged = true;
                //                }
                //            }
                //            await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisitionDetail>(buyingRequisition.tb_BuyingRequisitionDetails).ExecuteCommandAsync();
                //        }
                //    }
                //}

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OnTheWayQty)>();

                foreach (var child in entity.tb_PurOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentOnTheWayQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            //采购和销售都会提前处理。所以这里默认提供一行数据。成本和数量都可能为0
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0, // 初始数量
                                InitInventory = 0,
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充
                                Notes = "采购订单创建",
                                Sale_Qty = 0,
                            };
                            BusinessHelper.Instance.InitEntity(inv);
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            OnTheWayQty: currentOnTheWayQty // 首次累加
                                                            //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;

                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.OnTheWayQty += currentOnTheWayQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                // 处理分组数据，更新库存记录的各字段
                //循环inventoryGroups
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段 //更新在途库存
                    inv.On_the_way_Qty += group.Value.OnTheWayQty.ToInt();
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    rmrs.ErrorMsg = ("库存更新失败！");
                    rmrs.Succeeded = false;
                    return rmrs;
                }


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region 生成预付款单

                    // 获取付款方式信息
                    if (_appContext.PaymentMethodOfPeriod == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.Succeeded = false;
                        rmrs.ErrorMsg = $"请先配置付款方式信息！";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.LogInformation(rmrs.ErrorMsg);
                        }
                        return rmrs;
                    }


                    //如果是账期必须是未付款
                    if (entity.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        if (entity.PayStatus != (int)PayStatus.未付款)
                        {
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"付款方式为账期的订单必须是未付款。";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }

                    if (entity.PayStatus == (int)PayStatus.未付款)
                    {

                        if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                        {
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"未付款订单的付款方式必须是账期。";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rmrs.ErrorMsg);
                            }
                            return rmrs;
                        }
                    }
                    // 外币相关处理 正确是 外币时一定要有汇率
                    decimal exchangeRate = 1; // 获取销售订单的汇率
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        exchangeRate = entity.ExchangeRate; // 获取销售订单的汇率
                                                            // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                            // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
                    }

                    //销售订单审核时，非账期，即时收款时，生成预收款。 订金，部分收款
                    if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                    {

                        #region 生成预付款单

                        //正常来说。不能重复生成。即使退款也只会有一个对应订单的预收款单。 一个预收款单可以对应正负两个收款单。
                        // 生成预收款单前 检测
                        var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                        var PreReceivedPayment = ctrpay.BuildPreReceivedPayment(entity);
                        if (PreReceivedPayment.LocalPrepaidAmount > 0)
                        {
                            ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrpay.SaveOrUpdate(PreReceivedPayment);
                            if (!rmpay.Succeeded)
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
                            else
                            {
                                if (_appContext.FMConfig.AutoAuditPrePayment)
                                {
                                    ReturnResults<tb_FM_PreReceivedPayment> autoApproval = await ctrpay.ApprovalAsync(PreReceivedPayment);
                                    if (!autoApproval.Succeeded)
                                    {
                                        rmrs.Succeeded = false;
                                        _unitOfWorkManage.RollbackTran();
                                        rmrs.ErrorMsg = $"预付款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                        if (_appContext.SysConfig.ShowDebugInfo)
                                        {
                                            _logger.LogInformation(rmrs.ErrorMsg);
                                        }
                                        return rmrs;
                                    }
                                    else
                                    {
                                        FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                        fMAuditLog.CreateAuditLog<tb_FM_PreReceivedPayment>("预付单依系统设置自动审核", autoApproval.ReturnObject as tb_FM_PreReceivedPayment);
                                    }
                                }
                            }
                        }
                      

                        #endregion

                    }

                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列

                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                    .ExecuteCommandHasChangeAsync();
                _unitOfWorkManage.CommitTran();
                //_logger.Info(approvalEntity.bizName + "审核事务成功");
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
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

                ////如果采购订单明细数据来自于请购单，则明细要回写状态为已采购
                //if (entity.RefBillID.HasValue && entity.RefBillID.Value > 0)
                //{
                //    if (entity.RefBizType == (int)BizType.请购单)
                //    {
                //        tb_BuyingRequisition buyingRequisition = _appContext.Db.Queryable<tb_BuyingRequisition>()
                //            .Includes(c => c.tb_BuyingRequisitionDetails)
                //            .Where(c => c.PuRequisition_ID == entity.RefBillID).Single();
                //        if (buyingRequisition != null)
                //        {

                //            foreach (var child in entity.tb_PurOrderDetails)
                //            {
                //                var buyItem = buyingRequisition.tb_BuyingRequisitionDetails
                //                    .FirstOrDefault(c => c.ProdDetailID == child.ProdDetailID);
                //                buyItem.Purchased = false;
                //            }

                //            await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisitionDetail>(buyingRequisition.tb_BuyingRequisitionDetails).ExecuteCommandAsync();

                //        }
                //    }
                //}

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OnTheWayQty)>();

                foreach (var child in entity.tb_PurOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentOnTheWayQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0, // 初始数量
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充
                                Notes = "销售订单创建",
                                Sale_Qty = 0,
                                LatestOutboundTime = DateTime.MinValue // 初始时间
                            };
                            BusinessHelper.Instance.InitEntity(inv); // 初始化公共字段
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            OnTheWayQty: currentOnTheWayQty // 首次累加
                                                            //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.OnTheWayQty += currentOnTheWayQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                // 处理分组数据，更新库存记录的各字段
                //循环inventoryGroups
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段 //更新在途库存
                    inv.On_the_way_Qty -= group.Value.OnTheWayQty.ToInt();
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.LogInformation($"{entity.PurOrderNo}更新库存结果为0行，请检查数据！");
                }
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region  预付款单处理

                    var PrePaymentQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                        .Where(p => p.SourceBillId == entity.PurOrder_ID && p.SourceBizType == (int)BizType.采购订单 && p.Currency_ID == entity.Currency_ID);
                    var PrePaymentList = await PrePaymentQueryable.ToListAsync();
                    if (PrePaymentList != null && PrePaymentList.Count > 0)
                    {
                        var PrePayment = PrePaymentList[0];
                        //一个订单。只会有一个预收款单
                        if (PrePayment != null)
                        {
                            if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核)
                            {
                                await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                            }

                            #region  检测对应的收款单记录，如果没有支付也可以直接删除
                            //取消的话。则要退款。修改的话。则不需要退款？？？这个功能则要详细比对数据。将来可能来实现。现在统计
                            //如果没有出库，则生成红字单  ，已冲销  已取消，先用取消标记
                            //如果是要退款，则在预收款查询这，生成退款单。
                            //如果预收单审核了，生成收款单 在财务没有审核前。还是可以反审。这是为了保存系统的灵活性。
                            var PaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                  .Includes(a => a.tb_FM_PaymentRecordDetails)
                                 .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == PrePayment.PreRPID)).ToListAsync();
                            if (PaymentList != null && PaymentList.Count > 0)
                            {
                                if (PaymentList.Count > 1 && PaymentList.Sum(c => c.TotalLocalAmount) == 0 && PaymentList.Any(c => c.IsReversed))
                                {
                                    //退款冲销过
                                    _unitOfWorkManage.RollbackTran();
                                    rs.ErrorMsg = $"采购订单{PrePayment.SourceBillNo}的预付款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，不能反审,只能【取消】作废。";
                                    rs.Succeeded = false;
                                    return rs;
                                }
                                else
                                {
                                    tb_FM_PaymentRecord Payment = PaymentList[0];
                                    if (Payment.PaymentStatus == (int)PaymentStatus.草稿 || Payment.PaymentStatus == (int)PaymentStatus.待审核)
                                    {
                                        var PaymentCounter = await _unitOfWorkManage.GetDbClient().DeleteNav(Payment)
                                            .Include(c => c.tb_FM_PaymentRecordDetails)
                                            .ExecuteCommandAsync();
                                        if (PaymentCounter)
                                        {
                                            await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                                        }
                                    }
                                    else
                                    {
                                        _unitOfWorkManage.RollbackTran();
                                        rs.ErrorMsg = $"对应的预付款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，反审失败\r\n" +
                                            $"需将预付款单【退款】，对收款单{Payment.PaymentNo}进行冲销处理\r\n" +
                                            $"取消当前订单后，重新录入正确的采购订单。";
                                        rs.Succeeded = false;
                                        return rs;
                                    }
                                }

                            }
                            //else
                            //{
                            //    //预收单审核了。应该有收款单。正常不会到这步
                            //    await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                            //}
                            #endregion

                        }
                    }


                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity)
                     .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                    .ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rs;
            }

        }


        /// <summary>
        /// 手动生成预付款单，需要手动审核。因为除金额后，还可以添加 完善 付款方式等、备注、附件、等信息。
        /// </summary>
        /// <param name="PrepaidAmount">本次预付金额</param>
        /// <param name="entity">对应的订单</param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_FM_PreReceivedPayment>> ManualPrePayment(decimal PrepaidAmount, tb_PurOrder entity)
        {
            ReturnResults<tb_FM_PreReceivedPayment> rmrs = new ReturnResults<tb_FM_PreReceivedPayment>();
            AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
            if (authorizeController.EnableFinancialModule())
            {
                #region 生成预付款单 

                #region 生成预付款单条件判断检测
                // 获取付款方式信息
                if (_appContext.PaymentMethodOfPeriod == null)
                {
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    rmrs.ErrorMsg = $"请先配置付款方式信息！";
                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.LogInformation(rmrs.ErrorMsg);
                    }
                    return rmrs;
                }

                //如果是账期必须是未付款
                if (entity.Paytype_ID == _appContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    if (entity.PayStatus != (int)PayStatus.未付款)
                    {
                        rmrs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"付款方式为账期的订单必须是未付款。";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.LogInformation(rmrs.ErrorMsg);
                        }
                        return rmrs;
                    }
                }

                if (entity.PayStatus == (int)PayStatus.未付款)
                {
                    if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        rmrs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"未付款订单的付款方式必须是账期。";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.LogInformation(rmrs.ErrorMsg);
                        }
                        return rmrs;
                    }
                    else
                    {
                        //是账期。说明是未付过款时，则是第一次收订金。具体的渠道。由后面补上，这里暂时空置? 但是又不是空值类型。

                    }
                }


                #endregion


                // 外币相关处理 正确是 外币时一定要有汇率
                decimal exchangeRate = 1; // 获取销售订单的汇率
                if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                {
                    exchangeRate = entity.ExchangeRate; // 获取销售订单的汇率
                                                        // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                        // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
                }

                //正常来说。不能重复生成。即使退款也只会有一个对应订单的预付款单。 一个预付款单可以对应正负两个收款单。
                // 生成预付款单前 检测
                var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                var PreReceivedPayment = ctrpay.BuildPreReceivedPayment(entity, PrepaidAmount);
                if (PreReceivedPayment.LocalPrepaidAmount > 0)
                {
                    ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrpay.SaveOrUpdate(PreReceivedPayment);
                    if (!rmpay.Succeeded)
                    {

                        // 处理预付款单生成失败的情况
                        rmrs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"预付款单生成失败：{rmpay.ErrorMsg ?? "未知错误"}";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.LogInformation(rmrs.ErrorMsg);
                        }
                    }
                    else
                    {
                        rmrs.ReturnObject = rmpay.ReturnObject;
                        /*
                        if (_appContext.FMConfig.AutoAuditPrePayment)
                        {
                            #region 自动审核预付款
                            //销售订单审核时自动将预付款单设为"已生效"状态
                            PreReceivedPayment.ApprovalOpinions = "再次收到预付款，系统自动审核";
                            PreReceivedPayment.ApprovalStatus = (int)ApprovalStatus.已审核;
                            PreReceivedPayment.ApprovalResults = true;
                            ReturnResults<tb_FM_PreReceivedPayment> autoApproval = await ctrpay.ApprovalAsync(PreReceivedPayment);
                            if (!autoApproval.Succeeded)
                            {
                                rmrs.Succeeded = false;
                                _unitOfWorkManage.RollbackTran();
                                rmrs.ErrorMsg = $"预付款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(rmrs.ErrorMsg);
                                }
                            }
                            else
                            {
                                rmrs.ReturnObject = autoApproval.ReturnObject;
                                FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                fMAuditLog.CreateAuditLog<tb_FM_PreReceivedPayment>("预付款单自动审核成功", autoApproval.ReturnObject as tb_FM_PreReceivedPayment);
                            }
                            #endregion
                        }
                        */
                    }
                }

                #endregion
            }
            return rmrs;
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

                entity = mapper.Map<tb_PurEntry>(order);
                entity.PayStatus = order.PayStatus;
                entity.Paytype_ID = order.Paytype_ID;
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

                        string ProdName = string.Empty;
                        View_ProdDetail Prod = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                        {

                        }
                        else
                        {
                            Prod = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        }


                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = (details[i].UnitPrice + details[i].CustomizedCost) * details[i].Quantity;
                        details[i].SubtotalUntaxedAmount = (details[i].UntaxedUnitPrice + details[i].UntaxedCustomizedCost) * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{order.PurOrderNo}，{Prod.CNName + Prod.Specifications}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
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

                        string ProdName = string.Empty;
                        View_ProdDetail Prod = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                        {

                        }
                        else
                        {
                            Prod = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        }

                        details[i].Quantity = item.Quantity - item.DeliveredQuantity;// 已经交数量去掉
                        details[i].SubtotalAmount = details[i].UnitPrice * details[i].Quantity;
                        details[i].SubtotalUntaxedAmount = details[i].UntaxedUnitPrice * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            tipsMsg.Add($"订单{order.PurOrderNo}，{Prod.CNName}已入库数为{item.DeliveredQuantity}，可入库数为{details[i].Quantity}，当前行数据忽略！");
                        }
                        #endregion
                    }

                }


                entity.tb_PurEntryDetails = NewDetails;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = null;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.Created_by = null;
                entity.Created_at = null;
                entity.ActionStatus = ActionStatus.新增;

                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.TotalAmount = NewDetails.Sum(c => (c.UnitPrice + c.CustomizedCost) * c.Quantity);
                entity.TotalTaxAmount = NewDetails.Sum(c => c.TaxAmount);
                entity.TotalUntaxedAmount = NewDetails.Sum(c => (c.UntaxedUnitPrice + c.UntaxedCustomizedCost) * c.Quantity);

                entity.tb_PurEntryDetails = NewDetails;
                entity.PurOrder_ID = order.PurOrder_ID;
                entity.PurOrder_NO = order.PurOrderNo;
                entity.TotalAmount = entity.TotalAmount + entity.ShipCost;

                //要添加外币金额的运费
                entity.ForeignTotalAmount = entity.ForeignTotalAmount + entity.ForeignShipCost;

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



