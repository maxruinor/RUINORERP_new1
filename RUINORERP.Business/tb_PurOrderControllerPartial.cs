
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:35
// **************************************
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.EntityLoadService;
using RUINORERP.Business.Security;
using RUINORERP.Common.DB;
using RUINORERP.Common.Extensions;
using RUINORERP.Extensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IServices;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public partial class tb_PurOrderController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 批量结案  采购订单标记结案，数据状态为结案
        /// 如果还没有入库。但是结案的订单时。修正在途库存数量,将数量减掉。不需再进货了。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案是仓库和采购确定这个订单不再执行的一个确认过程。
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
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long LocationID)>();
                var orderStatusMap = new Dictionary<tb_PurOrder, (decimal DeliveredQty, decimal OrderQty)>();
                var ordersToProcess = new List<tb_PurOrder>();

                foreach (var entity in entitys)
                {
                    if (entity.DataStatus == (int)DataStatus.确认 && (entity.ApprovalStatus.HasValue && entity.ApprovalStatus.Value == (int)ApprovalStatus.审核通过 && entity.ApprovalResults.Value))
                    {
                        ordersToProcess.Add(entity);
                        decimal deliveredQty = entity.tb_PurOrderDetails.Select(c => c.DeliveredQuantity).Sum();
                        decimal orderQty = entity.tb_PurOrderDetails.Select(c => c.Quantity).Sum();
                        orderStatusMap[entity] = (deliveredQty, orderQty);
                        if (deliveredQty < orderQty)
                        {
                            foreach (var child in entity.tb_PurOrderDetails)
                            {
                                allKeys.Add((child.ProdDetailID, child.Location_ID));
                            }
                        }
                    }
                }

                var invDict = new Dictionary<(long ProdDetailID, long LocationID), tb_Inventory>();
                if (allKeys.Count > 0)
                {
                    var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.LocationID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.LocationID == i.Location_ID))
                        .ToListAsync();
                    invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                #region 处理库存更新（批量操作）
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var entity in ordersToProcess)
                {
                    if (orderStatusMap.TryGetValue(entity, out var status) && status.DeliveredQty < status.OrderQty)
                    {
                        foreach (var child in entity.tb_PurOrderDetails)
                        {
                            var key = (child.ProdDetailID, child.Location_ID);
                            if (!invDict.TryGetValue(key, out var inv) || inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = child.ProdDetailID;
                                inv.Location_ID = child.Location_ID;
                                inv.Quantity = 0;
                                inv.InitInventory = 0;
                                inv.Notes = "采购订单创建";
                                BusinessHelper.Instance.InitEntity(inv);
                                invDict[key] = inv;
                            }
                            //更新在途库存
                            inv.On_the_way_Qty -= (child.Quantity - child.DeliveredQuantity);
                            BusinessHelper.Instance.EditEntity(inv);
                            invUpdateList.Add(inv);
                        }
                    }
                }

                // 批量更新库存（事务内）
                if (invUpdateList.Any())
                {
                    _unitOfWorkManage.BeginTran();
                    try
                    {
                        DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                        var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (Counter == 0)
                        {
                            _logger.Debug($"更新库存结果为0行，请检查数据！");
                        }
                        _unitOfWorkManage.CommitTran();
                    }
                    catch (Exception ex)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.Succeeded = false;
                        rs.ErrorMsg = ex.Message;
                        return rs;
                    }
                }
                #endregion

                #region 处理预付款单（异步处理）
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    // 异步处理财务相关操作，不影响主流程
                    await Task.Run(async () =>
                    {
                        foreach (var entity in ordersToProcess)
                        {
                            try
                            {
                                var PrePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                                    .Where(p => p.SourceBillId == entity.PurOrder_ID && p.SourceBizType == (int)BizType.采购订单)
                                    .FirstAsync();

                                if (PrePayment != null)
                                {
                                    // 定义容差值，用于浮点数比较
                                    const decimal tolerance = 0.01m;

                                    if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 ||
                                        PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核)
                                    {
                                        // 开启独立事务处理预付款单
                                        _unitOfWorkManage.BeginTran();
                                        try
                                        {
                                            //没有付款记录的，直接删除关闭
                                            await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();

                                            // 检测对应的付款单记录，如果没有支付也可以直接删除
                                            var PaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                                  .Includes(a => a.tb_FM_PaymentRecordDetails)
                                                 .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == PrePayment.PreRPID)).ToListAsync();
                                            if (PaymentList != null && PaymentList.Count > 0)
                                            {
                                                if (PaymentList.Count > 1 && PaymentList.Sum(c => c.TotalLocalAmount) == 0 && PaymentList.Any(c => c.IsReversed))
                                                {
                                                    _unitOfWorkManage.RollbackTran();
                                                    _logger.LogWarning($"采购订单{PrePayment.SourceBillNo}的预付款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，不能作废结案，只能【预付款退款】作废。");
                                                }
                                                else
                                                {
                                                    tb_FM_PaymentRecord Payment = PaymentList[0];
                                                    if (Payment.PaymentStatus == (int)PaymentStatus.草稿 || Payment.PaymentStatus == (int)PaymentStatus.待审核)
                                                    {
                                                        await _unitOfWorkManage.GetDbClient().DeleteNav(Payment)
                                                            .Include(c => c.tb_FM_PaymentRecordDetails)
                                                            .ExecuteCommandAsync();
                                                    }
                                                    else
                                                    {
                                                        _unitOfWorkManage.RollbackTran();
                                                        _logger.LogWarning($"对应的预付款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，作废结案失败\r\n" +
                                                            $"需将预付款单【退款】，对付款单{Payment.PaymentNo}进行冲销处理\r\n" +
                                                            $"当前订单【作废结案】后，重新录入正确的采购订单。");
                                                    }
                                                }
                                            }
                                            _unitOfWorkManage.CommitTran();
                                        }
                                        catch (Exception ex)
                                        {
                                            _unitOfWorkManage.RollbackTran();
                                            _logger.LogError(ex, $"处理采购订单{entity.PurOrderNo}的预付款单时出错");
                                        }
                                    }
                                    else if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.已生效)
                                    {
                                        //已生效表示审核通过，可以预收付，但还未实际收款
                                        //检查是否已经有实际收款（余额=预定金额表示未收款，余额<预定金额表示已收款）
                                        if (Math.Abs(PrePayment.LocalBalanceAmount - PrePayment.LocalPrepaidAmount) <= tolerance)
                                        {
                                            // 余额等于预定金额，表示还未实际收款，可以直接删除
                                            await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                                        }
                                        else
                                        {
                                            // 已经有实际收款，需要退款后才能结案
                                            _unitOfWorkManage.RollbackTran();
                                            _logger.LogWarning($"采购订单存在预付款单{PrePayment.PreRPNO}，且已实际收款{PrePayment.LocalPrepaidAmount - PrePayment.LocalBalanceAmount}元，不能直接作废结案,请先完成【预付款退款】处理。");
                                        }
                                    }
                                    else if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待核销
                                        || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.处理中)
                                    {
                                        //待核销表示已经收款，等待核销；处理中表示部分核销或部分退款
                                        //这两种状态都需要先退款才能结案
                                        if (Math.Abs(PrePayment.LocalBalanceAmount) > tolerance)
                                        {
                                            // 有余额，需要先退款
                                            _unitOfWorkManage.RollbackTran();
                                            _logger.LogWarning($"存在预付款单{PrePayment.PreRPNO}，状态为{(PrePaymentStatus)PrePayment.PrePaymentStatus}，本币余额{PrePayment.LocalBalanceAmount}元，不能直接作废结案,请进行【退款】处理。");
                                        }
                                        // 余额为0，理论上不应该出现在这两个状态，但为了容错允许结案
                                    }
                                    else if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.全额核销
                                        || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.混合结清
                                        || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.全额退款
                                        || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.结案)
                                    {
                                        //这些状态表示预付款已经处理完毕，如果余额为0，可以直接结案
                                        if (Math.Abs(PrePayment.LocalBalanceAmount) > tolerance)
                                        {
                                            // 理论上不应该出现这种情况，但为了安全还是检查一下
                                            _unitOfWorkManage.RollbackTran();
                                            _logger.LogWarning($"存在预付款单，状态为{(PrePaymentStatus)PrePayment.PrePaymentStatus}，但本币余额{PrePayment.LocalBalanceAmount}元不为0，数据异常，请联系管理员。");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"处理采购订单{entity.PurOrderNo}的财务操作时出错");
                            }
                        }
                    });
                }
                #endregion

                #region 更新订单状态（批量操作）
                _unitOfWorkManage.BeginTran();
                try
                {
                    foreach (var entity in ordersToProcess)
                    {
                        // 更新订单状态为结案
                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
                            it.CloseCaseOpinions,
                            it.Paytype_ID,
                            it.Modified_at,
                            it.Modified_by
                        }).ExecuteCommandAsync();
                    }
                    _unitOfWorkManage.CommitTran();
                }
                catch (Exception ex)
                {
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    rs.ErrorMsg = ex.Message;
                    return rs;
                }
                #endregion

                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                rs.Succeeded = false;
                rs.ErrorMsg = ex.Message;
                _logger.Error(ex, "批量结案操作异常");
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

            // 【事务优化】保存关键数据到方法级变量，用于财务独立事务处理
            bool needProcessFinance = false;
            long? orderId = null;
            string orderNo = "";
            long? paytypeId = null;
            decimal totalAmount = 0;

            try
            {
                // ========== 第一阶段: 预处理验证(无事务) ==========

                // 1.1 基础验证 - 检查重复审核
                var existingEntity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                    .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                    .Select(c => new { c.DataStatus, c.ApprovalStatus, c.ApprovalResults })
                    .FirstAsync();

                if (existingEntity != null &&
                    existingEntity.DataStatus == (int)DataStatus.确认 &&
                    existingEntity.ApprovalStatus == (int)ApprovalStatus.审核通过)
                {
                    rmrs.ErrorMsg = "采购订单已经审核通过，不能重复审核！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 1.2 加载依赖数据
                if (entity.tb_PurOrderDetails == null || entity.tb_PurOrderDetails.Count == 0)
                {
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                        .Includes(c => c.tb_PurOrderDetails)
                        .Where(d => d.PurOrder_ID == entity.PurOrder_ID)
                        .FirstAsync();
                }

                // 1.3 基础业务规则验证
                if (entity.TotalQty != entity.tb_PurOrderDetails.Sum(c => c.Quantity))
                {
                    rmrs.ErrorMsg = $"采购订单数量与明细之和不相等!请检查数据后重试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 1.4 付款状态验证(在事务外执行)
                var validationError = ValidateOrderPaymentStatus((object)entity.Paytype_ID, (object)entity.PayStatus);
                if (validationError != null)
                {
                    rmrs.Succeeded = false;
                    rmrs.ErrorMsg = validationError;
                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.Debug(rmrs.ErrorMsg);
                    }
                    return rmrs;
                }

                // 【事务优化】保存关键数据，用于后续财务独立处理
                orderId = entity.PurOrder_ID;
                orderNo = entity.PurOrderNo;
                paytypeId = entity.Paytype_ID;
                totalAmount = entity.TotalAmount;

                // 【事务优化】检查是否需要财务独立处理
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule() && entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    needProcessFinance = true;
                }

                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var requiredKeys = entity.tb_PurOrderDetails
                    .Select(c => new { c.ProdDetailID, c.Location_ID })
                    .Distinct()
                    .ToList();

                var invDict = new Dictionary<(long ProdDetailID, long LocationID), tb_Inventory>();
                if (requiredKeys.Count > 0)
                {
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                // 1.5 准备库存更新数据
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal OnTheWayQty)>();

                foreach (var child in entity.tb_PurOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentOnTheWayQty = child.Quantity;

                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新
                        // ✅ 从预加载字典获取（死锁优化）
                        if (!invDict.TryGetValue(key, out var inv) || inv == null)
                        {
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0,
                                InitInventory = 0,
                                Inv_Cost = 0,
                                Notes = "采购订单创建",
                                Sale_Qty = 0,
                            };
                            BusinessHelper.Instance.InitEntity(inv);
                            invDict[key] = inv;
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        group = (Inventory: inv, OnTheWayQty: currentOnTheWayQty);
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        group.OnTheWayQty += currentOnTheWayQty;
                        inventoryGroups[key] = group;
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    inv.On_the_way_Qty += group.Value.OnTheWayQty.ToInt();
                    invUpdateList.Add(inv);
                }

                // ========== 第二阶段: 事务内执行核心业务 ==========
                _unitOfWorkManage.BeginTran();

                try
                {
                    // 2.1 更新库存(带死锁优化)
                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (InvUpdateCounter == 0)
                    {
                        throw new Exception("库存更新失败！");
                    }

                    // 2.2 更新订单状态
                    entity.DataStatus = (int)DataStatus.确认;
                    entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                    entity.ApprovalResults = true;
                    BusinessHelper.Instance.ApproverEntity(entity);

                    var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                        .UpdateColumns(it => new
                        {
                            it.DataStatus,
                            it.ApprovalOpinions,
                            it.ApprovalResults,
                            it.ApprovalStatus,
                            it.Approver_at,
                            it.Approver_by
                        })
                        .ExecuteCommandHasChangeAsync();

                    // 提交主事务(快速提交,~50ms)
                    _unitOfWorkManage.CommitTran();
                    _logger.LogInformation($"采购订单{entity.PurOrderNo}审核：主事务提交成功");

                    // ========== 第三阶段: 后置处理(独立事务) ==========

                    // 3.1 财务处理(独立事务)
                    if (needProcessFinance)
                    {
                        var financeResult = await ProcessFinanceAfterPurOrderApprovalAsync(orderId, orderNo, paytypeId, totalAmount);
                        if (!financeResult.Succeeded)
                        {
                            _logger.LogWarning($"采购订单{entity.PurOrderNo}审核：主事务成功，但财务处理失败 - {financeResult.ErrorMsg}");
                        }
                    }

                    rmrs.ReturnObject = entity as T;
                    rmrs.Succeeded = true;
                    return rmrs;
                }
                catch (Exception ex)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw;
                }
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
                    && (entity.tb_PurEntries.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntries.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购入库单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }



                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var requiredKeys3 = entity.tb_PurOrderDetails
                    .Select(c => new { c.ProdDetailID, c.Location_ID })
                    .Distinct()
                    .ToList();

                var invDict3 = new Dictionary<(long ProdDetailID, long LocationID), tb_Inventory>();
                if (requiredKeys3.Count > 0)
                {
                    var inventoryList3 = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys3.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict3 = inventoryList3.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

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
                        // ✅ 从预加载字典获取（死锁优化）
                        if (!invDict3.TryGetValue(key, out var inv) || inv == null)
                        {
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0, // 初始数量
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充
                                Notes = "采购订单反审核",
                                Sale_Qty = 0,
                                LatestOutboundTime = DateTime.MinValue // 初始时间
                            };
                            BusinessHelper.Instance.InitEntity(inv); // 初始化公共字段
                            invDict3[key] = inv;
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
                    _logger.Debug($"{entity.PurOrderNo}更新库存结果为0行，请检查数据！");
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
                        _logger.Debug(rmrs.ErrorMsg);
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
                            _logger.Debug(rmrs.ErrorMsg);
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
                            _logger.Debug(rmrs.ErrorMsg);
                        }
                        return rmrs;
                    }
                    else
                    {
                        //是账期。说明是未付过款时，则是第一次收订金。具体的渠道。由后面补上，这里暂时空置? 但是又不是空值类型。

                    }
                }
                #endregion

                if (entity.PayStatus == (int)PayStatus.未付款 || entity.PayStatus == (int)PayStatus.部分预付)
                {
                    // 验证预付金额是否超过订单总金额
                    var paymentCheck = await CheckOrderPaymentWithMessage(PrepaidAmount, entity);
                    if (!paymentCheck.CanDo)
                    {
                        rmrs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = paymentCheck.Message;
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.Debug(rmrs.ErrorMsg);
                        }
                        return rmrs;
                    }

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
                    var PreReceivedPayment = await ctrpay.BuildPreReceivedPaymentAsync(entity, PrepaidAmount);
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
                                _logger.Debug(rmrs.ErrorMsg);
                            }
                        }
                        else
                        {
                            rmrs.ReturnObject = rmpay.ReturnObject;
                            rmrs.Succeeded = true;
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
                                        _logger.Debug(rmrs.ErrorMsg);
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
                else
                {
                    rmrs.Succeeded = false;
                    _unitOfWorkManage.RollbackTran();
                    rmrs.ErrorMsg = "只有未付款或部分预付的订单才能生成预付款单";
                    if (_appContext.SysConfig.ShowDebugInfo)
                    {
                        _logger.Debug(rmrs.ErrorMsg);
                    }
                    return rmrs;
                }
            }
            return rmrs;
        }

        /// <summary>
        /// 检查对应的订单的预付金额是否有超过订单金额的情况
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<(bool CanDo, string Message)> CheckOrderPaymentWithMessage(decimal PrepaidAmount, tb_PurOrder entity)
        {
            List<tb_FM_PreReceivedPayment> prePaidList = await _appContext.Db.CopyNew().Queryable<tb_FM_PreReceivedPayment>()
                .Where(m => m.SourceBizType == (int)BizType.采购订单 && m.SourceBillId == entity.PurOrder_ID)
                .Where(c => c.PrePaymentStatus >= (int)PrePaymentStatus.已生效)
                .ToListAsync();
            decimal totalPrePaid = prePaidList.Sum(c => c.LocalPrepaidAmount);
            if (totalPrePaid + PrepaidAmount > entity.TotalAmount)
            {
                return (false, $"采购订单【{entity.PurOrderNo}】：已生效预付款金额【{totalPrePaid:F2}】+ 本次预付金额【{PrepaidAmount:F2}】= 【{(totalPrePaid + PrepaidAmount):F2}】，已超过订单总金额【{entity.TotalAmount:F2}】，请检查！");
            }

            return (true, $"");
        }


        /// <summary>
        /// 转换为采购入库单,注意一个订单可以多次转成入库单。
        /// </summary>
        /// <param name="order"></param>
        public async Task<tb_PurEntry> BuildPurEntryFromPurOrder(tb_PurOrder order)
        {
            AuthorizeController authorizeController = null;
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
                IEntityCacheManager _cacheManager = _appContext.GetRequiredService<IEntityCacheManager>();
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



                        View_ProdDetail Prod = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                        {

                        }
                        else
                        {
                            Prod = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                        View_ProdDetail Prod = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        if (Prod != null && Prod.GetType().Name != "Object" && Prod is View_ProdDetail prodDetail)
                        {

                        }
                        else
                        {
                            Prod = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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

                #region 分摊成本计算

                entity.TotalQty = NewDetails.Sum(c => c.Quantity);

                //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                if (entity.ShipCost > 0)
                {

                    //根据系统设置中的分摊规则来分配运费收入到明细。

                    if (_appContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                    {
                        // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                        foreach (var item in NewDetails)
                        {
                            item.AllocatedFreightCost = entity.ShipCost * (item.Quantity / entity.TotalQty);
                            item.AllocatedFreightCost = item.AllocatedFreightCost.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                            item.FreightAllocationRules = _appContext.SysConfig.FreightAllocationRules;
                        }
                    }
                }


                #endregion


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
                entity.EntryDate = System.DateTime.Now;
                entity.PrintStatus = 0;
                BusinessHelper.Instance.InitEntity(entity);

                if (entity.PurOrder_ID.HasValue && entity.PurOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = order.CustomerVendor_ID;
                    entity.PurOrder_NO = order.PurOrderNo;
                }
                // 使用带重试机制的编号生成（自动审核关键流程）
                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                ILogger logger = _appContext.GetRequiredService<ILogger<tb_PurOrderController<T>>>();
                entity.PurEntryNo = await RUINORERP.Business.Helpers.BizCodeHelper.GenerateBizBillNoWithRetryAsync(
                    bizCodeService,
                    BizType.采购入库单,
                    maxRetries: 3,
                    initialDelayMs: 500,
                    logger: logger);
                //保存到数据库
                BusinessHelper.Instance.InitEntity(entity);
            }
            return entity;
        }
        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_PurOrder> list = await _appContext.Db.CopyNew().Queryable<tb_PurOrder>().Where(m => m.PurOrder_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

        /// <summary>
        /// 【事务优化】采购订单审核后的财务独立事务处理
        /// 将预付款单生成、审核等操作从主事务中分离，减少主事务持有时间
        /// 包含补偿机制：当后续步骤失败时，回滚已创建的预付款单
        /// </summary>
        private async Task<ReturnResults<bool>> ProcessFinanceAfterPurOrderApprovalAsync(long? orderId, string orderNo, long? paytypeId, decimal totalAmount)
        {
            ReturnResults<bool> result = new ReturnResults<bool>();
            tb_FM_PreReceivedPayment savedPrePayment = null;
            bool prePaymentSaved = false;

            try
            {
                _logger.LogInformation($"采购订单{orderNo}审核：开始处理财务独立事务...");

                var ctrPreReceivedPayment = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();

                // 重新加载订单实体
                var order = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                    .Where(c => c.PurOrder_ID == orderId)
                    .FirstAsync();

                if (order == null)
                {
                    _logger.LogError($"采购订单{orderNo}审核：无法重新加载订单实体");
                    result.ErrorMsg = "无法重新加载订单实体";
                    return result;
                }

                // 外币相关处理
                decimal exchangeRate = 1;
                if (_appContext.BaseCurrency.Currency_ID != order.Currency_ID)
                {
                    exchangeRate = order.ExchangeRate;
                }

                // 生成预付款单
                var PreReceivedPayment = await ctrPreReceivedPayment.BuildPreReceivedPaymentAsync(order);
                if (PreReceivedPayment.LocalPrepaidAmount > 0)
                {
                    ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrPreReceivedPayment.SaveOrUpdate(PreReceivedPayment);
                    if (!rmpay.Succeeded)
                    {
                        _logger.LogError($"采购订单{orderNo}审核：预付款单生成失败 - {rmpay.ErrorMsg}");
                        result.ErrorMsg = $"预付款单生成失败 - {rmpay.ErrorMsg}";
                        return result;
                    }

                    // 记录已保存的预付款单，用于后续补偿
                    savedPrePayment = rmpay.ReturnObject;
                    prePaymentSaved = true;
                    _logger.LogInformation($"采购订单{orderNo}审核：预付款单已保存，ID={savedPrePayment?.PreRPID}");

                    // 自动审核预收款
                    if (_appContext.FMConfig.AutoAuditPrePayment)
                    {
                        PreReceivedPayment.ApprovalOpinions = "系统自动审核";
                        PreReceivedPayment.ApprovalStatus = (int)ApprovalStatus.审核通过;
                        PreReceivedPayment.ApprovalResults = true;

                        var autoApproval = await ctrPreReceivedPayment.ApprovalAsync(PreReceivedPayment);
                        if (!autoApproval.Succeeded)
                        {
                            _logger.LogError($"采购订单{orderNo}审核：预付款单自动审核失败 - {autoApproval.ErrorMsg}");
                            result.ErrorMsg = $"预付款单自动审核失败 - {autoApproval.ErrorMsg}";
                            // 触发补偿：删除已保存的预付款单
                            await CompensatePrePaymentAsync(savedPrePayment?.PreRPID, orderNo);
                            return result;
                        }

                        if (autoApproval.ReturnObject != null)
                        {
                            FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                            fMAuditLog.CreateAuditLog<tb_FM_PreReceivedPayment>("预付款单自动审核成功", autoApproval.ReturnObject as tb_FM_PreReceivedPayment);
                        }
                        else
                        {
                            _logger.LogWarning($"采购订单{orderNo}审核：预付款单审核返回对象为空，跳过审计日志记录");
                        }
                    }
                }

                _logger.LogInformation($"采购订单{orderNo}审核：财务独立事务处理完成");
                result.Succeeded = true;
                result.ReturnObject = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"采购订单{orderNo}审核：财务独立事务处理失败 - {ex.Message}");
                result.ErrorMsg = $"财务独立事务处理失败 - {ex.Message}";

                // 触发补偿：回滚已保存的数据
                if (prePaymentSaved)
                {
                    await CompensatePrePaymentAsync(savedPrePayment?.PreRPID, orderNo);
                }

                return result;
            }
        }

        /// <summary>
        /// 补偿机制：删除已创建的预付款单
        /// </summary>
        private async Task CompensatePrePaymentAsync(long? preRPID, string orderNo)
        {
            if (!preRPID.HasValue)
            {
                return;
            }

            try
            {
                _logger.LogWarning($"采购订单{orderNo}审核：触发补偿机制，删除预付款单 {preRPID}");

                // 检查预付款单是否存在且未被核销
                var prePayment = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                    .Where(c => c.PreRPID == preRPID)
                    .FirstAsync();

                if (prePayment == null)
                {
                    _logger.LogInformation($"采购订单{orderNo}审核：预付款单 {preRPID} 不存在，无需补偿");
                    return;
                }

                // 检查是否已被核销（如果已核销则不能删除）
                if (prePayment.LocalPaidAmount > 0 || prePayment.ForeignPaidAmount > 0)
                {
                    _logger.LogError($"采购订单{orderNo}审核：预付款单 {preRPID} 已被核销，无法补偿删除");
                    return;
                }

                // 删除预付款单
                var deleteResult = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PreReceivedPayment>()
                    .Where(c => c.PreRPID == preRPID)
                    .ExecuteCommandAsync();

                if (deleteResult > 0)
                {
                    _logger.LogInformation($"采购订单{orderNo}审核：预付款单 {preRPID} 补偿删除成功");
                }
                else
                {
                    _logger.LogWarning($"采购订单{orderNo}审核：预付款单 {preRPID} 补偿删除未找到记录");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"采购订单{orderNo}审核：预付款单 {preRPID} 补偿删除失败");
            }
        }




    }

}



