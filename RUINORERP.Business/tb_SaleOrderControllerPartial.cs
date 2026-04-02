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


using RUINORERP.Global;
using RUINORERP.Model.Base;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using System.Linq;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using System.Security.Policy;
using AutoMapper;
using System.Windows.Forms;
using RUINORERP.Global.EnumExt;
using System.Collections;
using RUINORERP.Business.BizMapperService;
using System.Threading;
using RUINORERP.Business.EntityLoadService;
using RUINOR.Core;

namespace RUINORERP.Business
{
    public partial class tb_SaleOrderController<T>
    {

        public async override Task<ReturnResults<T>> AdvancedSave(T ObjectEntity)
        {
            ReturnResults<T> result = new ReturnResults<T>();
            await Task.Delay(0); // 模拟异步操作
            return result; // 或者根据实际情况返回值
        }

        /// <summary>
        /// 保存订单及明细（带事务保护）
        /// </summary>
        public async Task<ReturnResults<tb_SaleOrder>> SaveWithDetailsAsync(tb_SaleOrder order, List<tb_SaleOrderDetail> details, bool isUpdate)
        {
            ReturnResults<tb_SaleOrder> result = new ReturnResults<tb_SaleOrder>();
            
            var db = _unitOfWorkManage.GetDbClient();
            
            try
            {
                // 开启事务
                _unitOfWorkManage.BeginTran();
                
                try
                {
                    // 1. 保存主表
                    if (isUpdate)
                    {
                        await db.Updateable(order).ExecuteCommandAsync();
                    }
                    else
                    {
                        await db.Insertable(order).ExecuteReturnSnowflakeIdAsync();
                    }
                    
                    // 2. 处理明细
                    if (order.SOrder_ID > 0 && details != null && details.Any())
                    {
                        // 2.1 查询已有明细
                        var existingDetails = await db.Queryable<tb_SaleOrderDetail>()
                            .Where(fk => fk.SOrder_ID == order.SOrder_ID)
                            .ToListAsync();
                        
                        //// 获取主键
                        //string pkName = "SaleOrderDetail_ID";
                        
                        // 2.2 分类处理
                        var existingIds = existingDetails.Select(d => d.SaleOrderDetail_ID).ToHashSet();
                        var currentIds = details.Where(d => d.SaleOrderDetail_ID > 0)
                                               .Select(d => d.SaleOrderDetail_ID).ToHashSet();
                        
                        // 删除已移除的明细
                        var idsToDelete = existingIds.Except(currentIds).ToList();
                        if (idsToDelete.Any())
                        {
                            await db.Deleteable<tb_SaleOrderDetail>()
                                .In(idsToDelete.ToArray())
                                .ExecuteCommandAsync();
                        }
                        
                        // 更新现有明细
                        var detailsToUpdate = details.Where(d => d.SaleOrderDetail_ID > 0 && existingIds.Contains(d.SaleOrderDetail_ID)).ToList();
                        if (detailsToUpdate.Any())
                        {
                            await db.Updateable(detailsToUpdate).ExecuteCommandAsync();
                        }
                        
                        // 新增新明细
                        var detailsToAdd = details.Where(d => d.SaleOrderDetail_ID == 0).ToList();
                        if (detailsToAdd.Any())
                        {
                            foreach (var detail in detailsToAdd)
                            {
                                detail.SOrder_ID = order.SOrder_ID;
                            }
                            await db.Insertable(detailsToAdd).ExecuteCommandAsync();
                        }
                    }
                    
                    // 提交事务
                    _unitOfWorkManage.CommitTran();
                    
                    result.Succeeded = true;
                    result.ReturnObject = order;
                }
                catch (Exception ex)
                {
                    _unitOfWorkManage.RollbackTran();
                    result.Succeeded = false;
                    result.ErrorMsg = ex.Message;
                    throw;
                }
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.ErrorMsg = ex.Message;
            }
            
            return result;
        }

        /// <summary>
        /// 库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// 财务业务模板：如果账期，则是在销售出库时生成应收。
        /// 销售订单审核时
        /// 部分付款叫订金。 有订金才生成预收款，意思是有金额交易才生成
        /// 全部付款生成应收
        /// 账期就要等出库审核时生成应收款。
        /// 
        /// 销售订金（预收）	- 预收定金生成预收单--》预收审核时 生成收款单 收款单审核代表完成支付。
        //- 后续订单核销预收 → 自动冲抵应收	- 预收付表：减少 RemainAmount
        //- 应收应付表：减少 TotalAmount1

        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_SaleOrder entity = ObjectEntity as tb_SaleOrder;
            
            // 【事务优化】保存关键数据到方法级变量，用于财务独立事务处理
            bool needProcessFinance = false;
            long? orderId = null;
            string orderNo = "";
            bool isFromPlatform = false;
            long? paytypeId = null;
            decimal totalAmount = 0;
            
            try
            {
                // 【事务优化】第一步：预处理阶段（无事务）- 验证和计算
                #region 预处理阶段
                
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long Location_ID), (tb_Inventory Inventory, int SaleQtySum)>();

                long[] childProdDetailIds = entity.tb_SaleOrderDetails.Select(c => c.ProdDetailID).ToList().ToArray();

                List<tb_Inventory> invExistEntityList = new List<tb_Inventory>();
                invExistEntityList = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                    .Where(c => childProdDetailIds.Contains(c.ProdDetailID))
                    .ToListAsync();

                //更新拟销售量
                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    int currentSaleQty = child.Quantity;
                    DateTime currentOutboundTime = DateTime.Now;
                    
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        tb_Inventory inv = invExistEntityList.Find(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0,
                                InitInventory = 0,
                                Inv_Cost = 0,
                                Notes = "销售订单创建",
                                Sale_Qty = 0,
                            };
                            BusinessHelper.Instance.InitEntity(inv);
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        group = (Inventory: inv, SaleQtySum: currentSaleQty);
                        inventoryGroups[key] = group;
                    }
                    else
                    {
                        group.SaleQtySum += currentSaleQty;
                        inventoryGroups[key] = group;
                    }
                }

                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    inv.Sale_Qty += group.Value.SaleQtySum;
                    invList.Add(inv);
                }
                
                #endregion
                
                // 付款状态验证（在事务外执行）
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
                orderId = entity.SOrder_ID;
                orderNo = entity.SOrderNo;
                isFromPlatform = entity.IsFromPlatform;
                paytypeId = entity.Paytype_ID;
                totalAmount = entity.TotalAmount;
                
                // 【事务优化】检查是否需要财务独立处理
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule() && entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    needProcessFinance = true;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                // 更新库存拟销售量
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新数据为0，更新失败！");
                }

                // 更新订单状态
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                
                _unitOfWorkManage.CommitTran();
                _logger.LogInformation($"销售订单{entity.SOrderNo}审核：主事务提交成功");
                
                // 【事务优化】财务独立事务处理（主事务提交后执行）
                if (needProcessFinance)
                {
                    await ProcessFinanceOrderApprovalAsync(orderId, orderNo, isFromPlatform, paytypeId, totalAmount);
                }
                
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                // 检查事务状态并回滚
                try
                {
                    var transactionState = _unitOfWorkManage.GetTransactionState();
                    if (transactionState.IsActive)
                    {
                        _unitOfWorkManage.RollbackTran();
                        _logger.LogInformation($"销售订单审核：事务已回滚");
                    }
                }
                catch (Exception rollbackEx)
                {
                    _logger.LogCritical(rollbackEx, $"销售订单审核：事务回滚失败");
                }
                
                rmrs.Succeeded = false;
                rmrs.ErrorMsg = $"审核失败：{ex.Message}";
                _logger.Error(ex, "销售订单审核异常");
                return rmrs;
            }
        }
        
        /// <summary>
        /// 【事务优化】销售订单审核后的财务独立事务处理
        /// 将预收款单生成、审核等操作从主事务中分离，减少主事务持有时间
        /// </summary>
        private async Task ProcessFinanceOrderApprovalAsync(long? orderId, string orderNo, bool isFromPlatform, long? paytypeId, decimal totalAmount)
        {
            try
            {
                _logger.LogInformation($"销售订单{orderNo}审核：开始处理财务独立事务...");
                
                var ctrPreReceivedPayment = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                
                // 重新加载订单实体
                var order = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                    .Where(c => c.SOrder_ID == orderId)
                    .FirstAsync();
                
                if (order == null)
                {
                    _logger.LogError($"销售订单{orderNo}审核：无法重新加载订单实体");
                    return;
                }
                
                // 生成预收款单
                var PreReceivedPayment = await ctrPreReceivedPayment.BuildPreReceivedPaymentAsync(order);
                if (PreReceivedPayment.LocalPrepaidAmount > 0)
                {
                    ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrPreReceivedPayment.SaveOrUpdate(PreReceivedPayment);
                    if (!rmpay.Succeeded)
                    {
                        _logger.LogError($"销售订单{orderNo}审核：预收款单生成失败 - {rmpay.ErrorMsg}");
                        return;
                    }
                    
                    // 自动审核预收款
                    if (_appContext.FMConfig.AutoAuditPreReceive)
                    {
                        PreReceivedPayment.ApprovalOpinions = "系统自动审核";
                        PreReceivedPayment.ApprovalStatus = (int)ApprovalStatus.审核通过;
                        PreReceivedPayment.ApprovalResults = true;
                        
                        var autoApproval = await ctrPreReceivedPayment.ApprovalAsync(PreReceivedPayment);
                        if (!autoApproval.Succeeded)
                        {
                            _logger.LogError($"销售订单{orderNo}审核：预收款单自动审核失败 - {autoApproval.ErrorMsg}");
                            return;
                        }
                        
                        if (autoApproval.ReturnObject != null)
                        {
                            FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                            fMAuditLog.CreateAuditLog<tb_FM_PreReceivedPayment>("预收款单自动审核成功", autoApproval.ReturnObject as tb_FM_PreReceivedPayment);
                        }
                        else
                        {
                            _logger.LogWarning($"销售订单{orderNo}审核：预收款单审核返回对象为空，跳过审计日志记录");
                        }
                        
                        // 平台订单自动生成并审核收款单
                        if (isFromPlatform && _appContext.FMConfig.AutoAuditReceivePaymentRecordByPlatform)
                        {
                            var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                            tb_FM_PreReceivedPayment preReceivedPayment = autoApproval.ReturnObject;
                            
                            tb_FM_PaymentRecord paymentRecord = await paymentController.BuildPaymentRecord(
                                new List<tb_FM_PreReceivedPayment> { preReceivedPayment }, false);
                            
                            var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                            if (rrs.Succeeded)
                            {
                                paymentRecord.ApprovalOpinions = "平台订单，预收款单自动审核成功后，系统自动审核收款单";
                                paymentRecord.ApprovalStatus = (int)ApprovalStatus.审核通过;
                                paymentRecord.ApprovalResults = true;
                                
                                var ctrPaymentRecord = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                var rr = await ctrPaymentRecord.ApprovalAsync(paymentRecord);
                                if (!rr.Succeeded)
                                {
                                    _logger.LogError($"销售订单{orderNo}审核：收款单自动审核失败 - {rr.ErrorMsg}");
                                }
                            }
                        }
                        else
                        {
                            // 非平台订单，生成收款单但不自动审核
                            //暂时注释掉，因为自动化的操作表越多。事务死锁死锁的可能性越大，后可提供批量生成功能
                            /*
                            try
                            {
                                var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                tb_FM_PreReceivedPayment preReceivedPayment = autoApproval.ReturnObject;
                                
                                if (preReceivedPayment != null)
                                {
                                    tb_FM_PaymentRecord paymentRecord = await paymentController.BuildPaymentRecord(
                                        new List<tb_FM_PreReceivedPayment> { preReceivedPayment }, false);
                                    paymentRecord.ApprovalStatus = (int)ApprovalStatus.未审核;
                                    paymentRecord.PaymentStatus = (int)PaymentStatus.待审核;
                                    
                                    var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                                    if (rrs.Succeeded)
                                    {
                                        _logger.LogInformation($"销售订单{orderNo}审核：收款单生成成功(待审核)");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "非平台订单生成收款单失败");
                            }
                            */
                        }
                    }
                }
                
                _logger.LogInformation($"销售订单{orderNo}审核：财务独立事务处理完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"销售订单{orderNo}审核：财务独立事务处理失败 - {ex.Message}");
            }
        }
           


        /// <summary>
        /// 手动生成预付款单，需要手工审核
        /// 注意: 此方法不开启事务,让SaveOrUpdate方法统一管理事务
        /// </summary>
        /// <param name="PrepaidAmount">本次预付金额</param>
        /// <param name="entity">对应的订单</param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_FM_PreReceivedPayment>> ManualPrePayment(decimal PrepaidAmount, tb_SaleOrder entity)
        {
            ReturnResults<tb_FM_PreReceivedPayment> rmrs = new ReturnResults<tb_FM_PreReceivedPayment>();
            AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
            if (authorizeController.EnableFinancialModule())
            {
                #region 生成预收款单

                #region 生成预收款单条件判断检测
                // 获取付款方式信息
                if (_appContext.PaymentMethodOfPeriod == null)
                {
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


                // 外币相关处理 正确是 外币时一定要有汇率
                decimal exchangeRate = 1; // 获取销售订单的汇率
                if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                {
                    exchangeRate = entity.ExchangeRate; // 获取销售订单的汇率
                                                        // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                        // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
                }

                //正常来说。不能重复生成。即使退款也只会有一个对应订单的预收款单。 一个预收款单可以对应正负两个收款单。
                // 生成预收款单前 检测
                var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                var PreReceivedPayment = await ctrpay.BuildPreReceivedPaymentAsync(entity, PrepaidAmount);
                rmrs.ReturnObject = PreReceivedPayment;
                #endregion
            }
            return rmrs;
        }



        /// <summary>
        /// 库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApprovalAsync(List<tb_SaleOrder> entitys, ApprovalEntity approvalEntity)
        {
            try
            {
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long Location_ID)>();
                if (approvalEntity.ApprovalResults && entitys != null)
                {
                    foreach (var entity in entitys)
                    {
                        foreach (var child in entity.tb_SaleOrderDetails)
                        {
                            allKeys.Add((child.ProdDetailID, child.Location_ID));
                        }
                    }
                }


                var invDict = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                if (allKeys.Count > 0)
                {
                    var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (!approvalEntity.ApprovalResults)
                {
                    if (entitys == null)
                    {
                        return false;
                    }

                }
                else
                {
                    //更新拟销售量
                    #region 审核
                    foreach (var entity in entitys)
                    {
                        List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                        foreach (var child in entity.tb_SaleOrderDetails)
                        {
                            #region 库存表的更新 ，
                            // ✅ 从预加载字典获取（死锁优化）
                            var key = (child.ProdDetailID, child.Location_ID);
                            if (!invDict.TryGetValue(key, out var inv) || inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = child.ProdDetailID;
                                inv.Location_ID = child.Location_ID;

                                inv.Quantity = 0;
                                inv.InitInventory = 0;
                                inv.InitInvCost = 0;
                                inv.Notes = "销售订单初始化";
                                BusinessHelper.Instance.InitEntity(inv);
                                invDict[key] = inv;
                            }
                            //更新在途库存
                            inv.Sale_Qty = inv.Sale_Qty + child.Quantity;
                            BusinessHelper.Instance.EditEntity(inv);
                            #endregion
                            invUpdateList.Add(inv);
                        }

                        DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                        var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (Counter == 0)
                        {
                            _logger.Debug($"{entity.SOrderNo}批量审核时，更新库存结果为0行，请检查数据！");
                        }


                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.确认;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();
                    }
                    #endregion

                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();

                return true;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, approvalEntity.bizName + "事务回滚");
                return false;
            }

        }


        /// <summary>
        /// 批量结案  销售订单标记结案，数据状态为8, 
        /// 如果还没有出库。但是结案的订单时。修正拟出库数量,将数量减掉。不需再出货了。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// 审核状态时取消订单式结案 有预收付款则同时要退款
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_SaleOrder> entitys = new List<tb_SaleOrder>();
            entitys = NeedCloseCaseList as List<tb_SaleOrder>;

            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long Location_ID)>();

                // 【性能优化】预计算每个订单的已出库数量和订单数量，避免循环内重复计算
                var entityStatusMap = new Dictionary<tb_SaleOrder, (decimal DeliveredQty, decimal OrderQty)>();
                foreach (var entity in entitys)
                {
                    if (entity.DataStatus == (int)DataStatus.确认 && entity.ApprovalResults.HasValue)
                    {
                        var deliveredQty = entity.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty);
                        var orderQty = entity.tb_SaleOrderDetails.Sum(c => c.Quantity);
                        entityStatusMap[entity] = (deliveredQty, orderQty);
                    }
                }

                foreach (var entity in entitys)
                {
                    // 使用预计算的值进行比较
                    if (entityStatusMap.TryGetValue(entity, out var status) && status.DeliveredQty < status.OrderQty)
                    {
                        foreach (var child in entity.tb_SaleOrderDetails)
                        {
                            allKeys.Add((child.ProdDetailID, child.Location_ID));
                        }
                    }
                }

                var invDict2 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                if (allKeys.Count > 0)
                {
                    var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict2 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 结案


                //更新拟销售量  减少
                for (int m = 0; m < entitys.Count; m++)
                {
                    //判断 能结案的 是确认审核过的。
                    if (entitys[m].DataStatus != (int)DataStatus.确认 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }

                    //更新拟销售量
                    //如果销售订单明细中的出库数量小于订单中数量，则拟销售量要减去这个差值，因为这种情况是强制结案，意思是可能出库只出一半。就不会自动结案。
                    // 【性能优化】使用预计算的值进行比较
                    if (entityStatusMap.TryGetValue(entitys[m], out var status) && status.DeliveredQty < status.OrderQty)
                    {
                        tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                        List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                        for (int c = 0; c < entitys[m].tb_SaleOrderDetails.Count; c++)
                        {

                            #region 库存表的更新 ，
                            // ✅ 从预加载字典获取（死锁优化）
                            var key = (entitys[m].tb_SaleOrderDetails[c].ProdDetailID, entitys[m].tb_SaleOrderDetails[c].Location_ID);
                            if (!invDict2.TryGetValue(key, out var inv) || inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = entitys[m].tb_SaleOrderDetails[c].ProdDetailID;
                                inv.Location_ID = entitys[m].tb_SaleOrderDetails[c].Location_ID;
                                inv.Quantity = 0;
                                inv.InitInventory = 0;
                                inv.InitInvCost = 0;
                                inv.Notes = "销售订单初始化";
                                BusinessHelper.Instance.InitEntity(inv);
                                invDict2[key] = inv;
                            }
                            //更新在途库存 - 只更新一次
                            inv.Sale_Qty = inv.Sale_Qty - (entitys[m].tb_SaleOrderDetails[c].Quantity - entitys[m].tb_SaleOrderDetails[c].TotalDeliveredQty);
                            BusinessHelper.Instance.EditEntity(inv);
                            #endregion
                            invUpdateList.Add(inv);
                        }

                        DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                        var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (InvUpdateCounter == 0)
                        {
                            _logger.Debug($"{entitys[m].SOrderNo},批量关闭时，更新库存结果为0行，请检查数据！");
                        }
                    }
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].DataStatus = (int)DataStatus.完结;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //后面是不是要做一个审核历史记录表？

                    #region  预收款单处理（结案时检查）
                    AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                    if (authorizeController.EnableFinancialModule())
                    {
                        var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                        var PrePayment = await ctrpay.IsExistEntityAsync(p => p.SourceBillId == entitys[m].SOrder_ID && p.SourceBizType == (int)BizType.销售订单);
                        if (PrePayment != null)
                        {
                            if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 ||
                                PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核 ||
                                PrePayment.PrePaymentStatus == (int)PrePaymentStatus.已生效)
                            {
                                //没有付款记录的，直接删除关闭
                                await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();

                                #region  检测对应的收款单记录，如果没有支付也可以直接删除
                                var PaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                      .Includes(a => a.tb_FM_PaymentRecordDetails)
                                     .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == PrePayment.PreRPID)).ToListAsync();
                                if (PaymentList != null && PaymentList.Count > 0)
                                {
                                    if (PaymentList.Count > 1 && PaymentList.Sum(c => c.TotalLocalAmount) == 0 && PaymentList.Any(c => c.IsReversed))
                                    {
                                        if (_appContext.FMConfig.EnableAutoRefundOnOrderCancel && entitys[m].IsFromPlatform)
                                        {
                                            //自动退款处理
                                        }
                                        else
                                        {
                                            _unitOfWorkManage.RollbackTran();
                                            rs.ErrorMsg = $"销售订单{PrePayment.SourceBillNo}的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，不能结案，只能【预收款退款】作废。";
                                            rs.Succeeded = false;
                                            return rs;
                                        }
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
                                            if (_appContext.FMConfig.EnableAutoRefundOnOrderCancel && entitys[m].IsFromPlatform)
                                            {
                                                var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                                bool isRefund = true;
                                                tb_FM_PaymentRecord paymentRecord = await paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { PrePayment }, isRefund);
                                                paymentRecord.Remark += "（结案）";
                                                var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                                                if (rrs.Succeeded)
                                                {
                                                    await paymentController.ApprovalAsync(paymentRecord, true);
                                                }
                                            }
                                            else
                                            {
                                                _unitOfWorkManage.RollbackTran();
                                                rs.ErrorMsg = $"对应的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，结案失败\r\n" +
                                                    $"需将预收款单【退款】，对收款单{Payment.PaymentNo}进行冲销处理\r\n" +
                                                    $"当前订单【结案】后，重新录入正确的销售订单。";
                                                rs.Succeeded = false;
                                                return rs;
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                //订单结案后，预收款，可以退款可以下一个订单，应收来处理。由财务决定。
                                //这里仅提醒，订金已支付
                                if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.全额核销
                                    || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    rs.ErrorMsg = $"存在预收款单，且状态为{(PrePaymentStatus)PrePayment.PrePaymentStatus},不能直接结案,请撤销核销，再退款处理,或部分退款";
                                    rs.Succeeded = false;
                                    return rs;
                                }

                                if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待核销)
                                {
                                    if (_appContext.FMConfig.EnableAutoRefundOnOrderCancel && entitys[m].IsFromPlatform)
                                    {
                                        var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                        bool isRefund = true;
                                        tb_FM_PaymentRecord paymentRecord = await paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { PrePayment }, isRefund);
                                        paymentRecord.Remark += "（结案）";
                                        var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                                        if (rrs.Succeeded)
                                        {
                                            await paymentController.ApprovalAsync(paymentRecord, true);
                                        }
                                    }
                                    else
                                    {
                                        _unitOfWorkManage.RollbackTran();
                                        rs.ErrorMsg = $"存在预收款单，且状态为{(PrePaymentStatus)PrePayment.PrePaymentStatus},不能直接结案,请进行【退款】处理。";
                                        rs.Succeeded = false;
                                        return rs;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions, it.Modified_by, it.Modified_at, it.Notes }).ExecuteCommandAsync();
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

        /// <summary>
        /// 销售订单标记为结案前的状态，数据状态为4, 
        /// 如果还没有出库。但是反结案的订单时。修正拟出库数量将数量加回去。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> AntiBatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_SaleOrder> entitys = new List<tb_SaleOrder>();
            entitys = NeedCloseCaseList as List<tb_SaleOrder>;

            ReturnResults<bool> rs = new ReturnResults<bool>();
            try
            {
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys3 = new List<(long ProdDetailID, long Location_ID)>();

                // 【性能优化】预计算每个订单的已出库数量和订单数量，避免循环内重复计算
                var entityStatusMap3 = new Dictionary<tb_SaleOrder, (decimal DeliveredQty, decimal OrderQty)>();
                foreach (var entity in entitys)
                {
                    if (entity.DataStatus == (int)DataStatus.完结 && entity.ApprovalResults.HasValue)
                    {
                        var deliveredQty = entity.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty);
                        var orderQty = entity.tb_SaleOrderDetails.Sum(c => c.Quantity);
                        entityStatusMap3[entity] = (deliveredQty, orderQty);
                    }
                }

                foreach (var entity in entitys)
                {
                    // 使用预计算的值进行比较
                    if (entityStatusMap3.TryGetValue(entity, out var status) && status.DeliveredQty < status.OrderQty)
                    {
                        foreach (var child in entity.tb_SaleOrderDetails)
                        {
                            allKeys3.Add((child.ProdDetailID, child.Location_ID));
                        }
                    }
                }

                var invDict3 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                if (allKeys3.Count > 0)
                {
                    var requiredKeys = allKeys3.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict3 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region 反结案


                //更新拟销售量  加回去
                for (int m = 0; m < entitys.Count; m++)
                {
                    //判断 能结案的 是确认审核过的。
                    if (entitys[m].DataStatus != (int)DataStatus.完结 || !entitys[m].ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }

                    //更新拟销售量
                    //如果销售订单明细中的出库数量小于订单中数量，则拟销售量要减去这个差值，因为这种情况是强制结案，意思是可能出库只出一半。就不会自动结案。
                    // 【性能优化】使用预计算的值进行比较
                    if (entityStatusMap3.TryGetValue(entitys[m], out var status) && status.DeliveredQty < status.OrderQty)
                    {
                        tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                        List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                        for (int c = 0; c < entitys[m].tb_SaleOrderDetails.Count; c++)
                        {

                            #region 库存表的更新 ，
                            // ✅ 从预加载字典获取（死锁优化）
                            var key = (entitys[m].tb_SaleOrderDetails[c].ProdDetailID, entitys[m].tb_SaleOrderDetails[c].Location_ID);
                            if (!invDict3.TryGetValue(key, out var inv) || inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = entitys[m].tb_SaleOrderDetails[c].ProdDetailID;
                                inv.Location_ID = entitys[m].tb_SaleOrderDetails[c].Location_ID;
                                inv.Quantity = 0;
                                inv.InitInventory = 0;
                                inv.InitInvCost = 0;
                                inv.Notes = "销售订单初始化";
                                BusinessHelper.Instance.InitEntity(inv);
                                invDict3[key] = inv;
                            }
                            //更新在途库存 - 只更新一次
                            inv.Sale_Qty = inv.Sale_Qty + (entitys[m].tb_SaleOrderDetails[c].Quantity - entitys[m].tb_SaleOrderDetails[c].TotalDeliveredQty);
                            BusinessHelper.Instance.EditEntity(inv);
                            #endregion
                            invUpdateList.Add(inv);
                        }
                        DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                        var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (InvUpdateCounter == 0)
                        {
                            _logger.Debug($"{entitys[m].SOrderNo}反审核，更新库存结果为0行，请检查数据！");
                        }

                    }

                    entitys[m].DataStatus = (int)DataStatus.确认;
                    BusinessHelper.Instance.EditEntity(entitys[m]);

                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions, it.Modified_by, it.Modified_at, it.Notes }).ExecuteCommandAsync();
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


        public async override Task<ReturnMainSubResults<T>> BaseUpdateWithChild(T model)
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_SaleOrder entity = model as tb_SaleOrder;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };

                if (entity.SOrder_ID > 0)
                {
                    rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_SaleOrder>(entity as tb_SaleOrder)
                        .Include(m => m.tb_PurOrders)
                        .Include(x => x.tb_ProductionPlans, new SqlSugar.UpdateNavOptions()
                        {
                            OneToManyInsertOrUpdate = true,//配置启用 插入、更新或删除模式
                        })
                        .Include(m => m.tb_SaleOuts)
                        .Include(m => m.tb_OrderPackings)
                        .Include(x => x.tb_SaleOrderDetails, new SqlSugar.UpdateNavOptions()
                        {
                            OneToManyInsertOrUpdate = true,//配置启用 插入、更新或删除模式
                        })
                        .ExecuteCommandAsync();

                }
                else
                {
                    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_SaleOrder>(entity as tb_SaleOrder)
                        .Include(m => m.tb_PurOrders)
                        .Include(m => m.tb_ProductionPlans)
                        .Include(m => m.tb_SaleOrderDetails)
                        .Include(m => m.tb_SaleOuts)
                        .Include(m => m.tb_OrderPackings)
                                .ExecuteCommandAsync();
                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.SOrder_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                //_logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                // rr.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }

        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public T ExistFieldValueWithReturn(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>()
                .Where(exp)
                .First();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_SaleOrder entity = ObjectEntity as tb_SaleOrder;
            try
            {

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                // ✅ 死锁优化：事务外批量预加载库存
                var allKeysCancel = new List<(long ProdDetailID, long Location_ID)>();
                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    allKeysCancel.Add((child.ProdDetailID, child.Location_ID));
                }

                var invDict5 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                if (allKeysCancel.Count > 0)
                {
                    var requiredKeys = allKeysCancel.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict5 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }

                //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                if (entity.tb_SaleOuts != null
                    && (entity.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结)
                    && entity.tb_SaleOuts.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的销售出库单，不能反审核,请联系管理员，或作退回处理。";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {

                    rmrs.ErrorMsg = "只能反审核已确认,并且有审核结果的订单 ";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var requiredKeys4 = entity.tb_SaleOrderDetails
                    .Select(c => new { c.ProdDetailID, c.Location_ID })
                    .Distinct()
                    .ToHashSet();

                var invDict4 = new Dictionary<(long ProdDetailID, long LocationID), tb_Inventory>();
                if (requiredKeys4.Count > 0)
                {
                    var prodDetailIds = entity.tb_SaleOrderDetails.Select(c => c.ProdDetailID).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => prodDetailIds.Contains(i.ProdDetailID))
                        .ToListAsync();
                    inventoryList = inventoryList.Where(i => requiredKeys4.Contains(new { i.ProdDetailID, i.Location_ID })).ToList();
                    invDict4 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, int SaleQtySum)>();


                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    int currentSaleQty = child.Quantity;
                                                             // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        // ✅ 从预加载字典获取（死锁优化）
                        if (!invDict4.TryGetValue(key, out var inv) || inv == null)
                        {
                            //实际不会出现这个情况。因为审核时创建了。
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception("库存数据不存在,反审失败！");
                        }

                        BusinessHelper.Instance.EditEntity(inv);
                        #endregion
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            SaleQtySum: currentSaleQty // 首次累加
                        );
                        inventoryGroups[key] = group;
                    }
                    else
                    {
                        // 累加分组的数值字段 反审也是累加。下面才可能是减少
                        group.SaleQtySum += currentSaleQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    //反审 要用减
                    inv.Sale_Qty -= group.Value.SaleQtySum;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _logger.Debug($"{entity.SOrderNo}反审核，更新库存结果为0行，请检查数据！");
                }

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region  预收款单处理
                    //var PrePaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(p => p.SourceBillId.HasValue && p.SourceBillId.Value == entity.SOrder_ID).ToListAsync();
                    //var PrePaymentList1 = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(p => p.SourceBillId.HasValue && p.SourceBillId.Value == entity.SOrder_ID).ToListAsync();
                    var PrePaymentQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                        .Where(p => p.SourceBillId == entity.SOrder_ID && p.SourceBizType == (int)BizType.销售订单 && p.Currency_ID == entity.Currency_ID);
                    var PrePaymentList = await PrePaymentQueryable.ToListAsync();
                    if (PrePaymentList != null && PrePaymentList.Count > 0)
                    {
                        var PrePayment = PrePaymentList[0];
                        //一个订单。只会有一个预收款单
                        if (PrePayment != null)
                        {
                            if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核 || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.已生效)
                            {
                                await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                            }

                            #region  检测对应的收款单记录，如果没有支付也可以直接删除
                            //订单反审核  只是用来修改，还是真实取消订单。取消的话。则要退款。修改的话。则不需要退款。
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
                                    rmrs.ErrorMsg = $"销售订单{PrePayment.SourceBillNo}的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，不能反审,只能【取消】作废。";
                                    rmrs.Succeeded = false;
                                    return rmrs;
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
                                        rmrs.ErrorMsg = $"对应的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，反审失败\r\n" +
                                            $"需将预收款单【退款】，对收款单{Payment.PaymentNo}进行冲销处理\r\n" +
                                            $"当前订单【作废】后，重新录入正确的销售订单。";
                                        rmrs.Succeeded = false;
                                        return rmrs;
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
                if (_appContext != null && _appContext.CurUserInfo != null)
                {
                    entity.ApprovalOpinions += $"【被{_appContext.CurUserInfo.tb_Employee.Employee_Name}反审】";
                }
                else
                {
                    entity.ApprovalOpinions += $"【被反审】";
                }
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).UpdateColumns(it => new
                {
                    it.ApprovalStatus,
                    it.DataStatus,
                    it.ApprovalResults,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                if (result > 0)
                {
                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();
                    rmrs.ReturnObject = entity as T;
                    rmrs.Succeeded = true;
                }
                else
                {
                    _unitOfWorkManage.RollbackTran();

                    rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=> 保存出错";
                    rmrs.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);

                rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
            }
            return rmrs;
        }


        /// <summary>
        /// 更新付款状态，并且一次只能更新一个单据
        /// 更新订单，更新出库单的付款状态， 付款状态，付款方式。付款日期？付款凭证？
        ///  没有完成！！！！！！！！！！！！！
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> UpdateCustomizedCost(tb_SaleOrder entity)
        {
            ReturnResults<bool> rmrs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //判断是否需要财务更新
                if (!entity.ApprovalResults.HasValue || (entity.DataStatus == (int)DataStatus.草稿 || entity.DataStatus == (int)DataStatus.新建) || entity.ApprovalResults.Value == false)
                {

                    rmrs.ErrorMsg = "只能更新已审核且通过的订单";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                if (entity.tb_SaleOuts == null)
                {
                    entity.tb_SaleOuts = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(m => m.SOrder_ID == entity.SOrder_ID).ToListAsync();
                }

                entity.tb_SaleOuts.ForEach(c => c.PayStatus = entity.PayStatus);
                entity.tb_SaleOuts.ForEach(c => c.ProjectGroup_ID = entity.ProjectGroup_ID);
                entity.tb_SaleOuts.ForEach(c => c.Paytype_ID = entity.Paytype_ID);


                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).UpdateColumns(it => new { it.PayStatus, it.Paytype_ID, it.ProjectGroup_ID }).ExecuteCommandAsync();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity.tb_SaleOuts).UpdateColumns(it => new { it.PayStatus, it.Paytype_ID, it.ProjectGroup_ID }).ExecuteCommandAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();

                rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
                _logger.Error(ex);
                rmrs.Succeeded = false;
            }
            return rmrs;
        }

        /// <summary>
        /// 更新付款状态，并且一次只能更新一个单据
        /// 更新订单，更新出库单的付款状态， 付款状态，付款方式。付款日期？付款凭证？
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> UpdatePaymentStatus(tb_SaleOrder entity)
        {
            ReturnResults<bool> rmrs = new ReturnResults<bool>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //判断是否需要财务更新
                if (!entity.ApprovalResults.HasValue || (entity.DataStatus == (int)DataStatus.草稿 || entity.DataStatus == (int)DataStatus.新建) || entity.ApprovalResults.Value == false)
                {

                    rmrs.ErrorMsg = "只能更新已审核且通过的订单";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                if (entity.tb_SaleOuts == null)
                {
                    entity.tb_SaleOuts = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(m => m.SOrder_ID == entity.SOrder_ID).ToListAsync();
                }

                entity.tb_SaleOuts.ForEach(c => c.PayStatus = entity.PayStatus);
                entity.tb_SaleOuts.ForEach(c => c.ProjectGroup_ID = entity.ProjectGroup_ID);
                entity.tb_SaleOuts.ForEach(c => c.Paytype_ID = entity.Paytype_ID);


                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).UpdateColumns(it => new { it.PayStatus, it.Paytype_ID, it.ProjectGroup_ID }).ExecuteCommandAsync();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity.tb_SaleOuts).UpdateColumns(it => new { it.PayStatus, it.Paytype_ID, it.ProjectGroup_ID }).ExecuteCommandAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();

                rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
                _logger.Error(ex);
                rmrs.Succeeded = false;
            }
            return rmrs;
        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            //var queryable = _appContext.Db.Queryable<tb_SaleOrderDetail>();
            //var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();
            List<tb_SaleOrder> list = await _appContext.Db.CopyNew().Queryable<tb_SaleOrder>().Where(m => m.SOrder_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_projectgroup)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

        AuthorizeController authorizeController = null;

        /// <summary>
        /// 转换为销售出库单
        /// </summary>
        /// <param name="saleorder"></param>
        public async Task<tb_SaleOut> SaleOrderToSaleOut(tb_SaleOrder saleorder)
        {
            tb_SaleOut entity = new tb_SaleOut();
            //转单
            if (saleorder != null)
            {
                if (authorizeController == null)
                {
                    authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                }

                entity = mapper.Map<tb_SaleOut>(saleorder);
                //注意转过来的实体  各种状态要重新赋值不然逻辑有问题，保存就是已经审核
                entity.ApprovalOpinions = "快捷转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.PayStatus = saleorder.PayStatus;
                entity.Paytype_ID = saleorder.Paytype_ID;
                entity.PrimaryKeyID = 0;
                List<string> tipsMsg = new List<string>();
                List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
                List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();


                // 【性能优化】在循环外预先计算重复的ProdDetailID，避免每次迭代都执行GroupBy
                var duplicateProdDetailIds = details.Select(c => c.ProdDetailID).ToList()
                    .GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

                //多行相同产品时 可能还在仔细优化核对
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    View_ProdDetail obj = null;
                    // 使用预计算的结果进行比较
                    if (duplicateProdDetailIds.Count > 0 && details[i].SaleOrderDetail_ID > 0)
                    {
                        obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        #region 产品ID可能大于1行，共用料号情况
                        tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                        details[i].Cost = item.Cost;
                        details[i].CustomizedCost = item.CustomizedCost;
                        //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                        if (details[i].Cost == 0)
                        {
                            obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                            {
                                details[i].Cost = obj.Inv_Cost.Value;
                            }
                        }
                        details[i].Quantity = item.Quantity - item.TotalDeliveredQty;// 已经出数量去掉
                        details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                        details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;
                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            if (obj == null)
                            {
                                obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            }
                            tipsMsg.Add($"销售订单{saleorder.SOrderNo}，{obj.CNName + obj.Specifications}已出库数为{item.TotalDeliveredQty}，可出库数为{details[i].Quantity}，当前行数据忽略！");
                        }

                        #endregion
                    }
                    else
                    {
                        #region 每行产品ID唯一
                        tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                          && c.Location_ID == details[i].Location_ID
                        && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                        details[i].Cost = item.Cost;
                        details[i].CustomizedCost = item.CustomizedCost;
                        //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                        if (details[i].Cost == 0)
                        {
                            obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            if (obj != null && obj.GetType().Name != "Object" && obj is View_ProdDetail prodDetail)
                            {
                                if (obj.Inv_Cost == null)
                                {
                                    obj.Inv_Cost = 0;
                                }
                                details[i].Cost = obj.Inv_Cost.Value;
                            }
                        }
                        details[i].Quantity = details[i].Quantity - item.TotalDeliveredQty;// 减掉已经出库的数量
                        details[i].SubtotalTransAmount = details[i].TransactionPrice * details[i].Quantity;
                        details[i].SubtotalCostAmount = (details[i].Cost + details[i].CustomizedCost) * details[i].Quantity;

                        if (details[i].Quantity > 0)
                        {
                            NewDetails.Add(details[i]);
                        }
                        else
                        {
                            if (obj == null)
                            {
                                obj = _cacheManager.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            }
                            tipsMsg.Add($"当前订单的SKU:{obj.SKU}{obj.CNName}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                        }
                        #endregion
                    }
                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{entity.SaleOrderNo}已全部出库，请检查是否正在重复出库！");
                }

                //关键修复：如果这个订单已经有【已审核】的出库单，则当前出库单的运费收入应该为 0
                //只有已审核的出库单才代表运费已经确认收入，草稿状态的出库单不应该影响运费计算
                if (saleorder.tb_SaleOuts != null && saleorder.tb_SaleOuts.Count > 0)
                {
                    // 检查是否存在已审核的出库单（排除草稿和未审核的）
                    var auditedSaleOuts = saleorder.tb_SaleOuts
                        .Where(o => o.DataStatus >= (int)DataStatus.确认 
                                 && o.ApprovalStatus == (int)ApprovalStatus.审核通过)
                        .ToList();
                                    
                    if (auditedSaleOuts.Count > 0)
                    {
                        if (saleorder.FreightIncome > 0)
                        {
                            tipsMsg.Add($"当前订单已有已审核的出库记录，运费收入已经计入前面出库单，当前出库运费收入为零！");
                            entity.FreightIncome = 0;
                        }
                        else
                        {
                            tipsMsg.Add($"当前订单已有已审核的出库记录！");
                        }
                    }
                    else
                    {
                        // 没有已审核的出库单，保留运费收入
                        if (saleorder.FreightIncome > 0)
                        {
                            tipsMsg.Add($"当前订单是第一次出库（或以前出库单未审核），保留运费收入：{saleorder.FreightIncome}");
                        }
                    }
                }

                entity.TotalQty = NewDetails.Sum(c => c.Quantity);

                //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                if (entity.FreightIncome > 0)
                {
                    entity.FreightCost = entity.FreightIncome;
                    //根据系统设置中的分摊规则来分配运费收入到明细。

                    if (_appContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                    {
                        // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                        foreach (var item in NewDetails)
                        {
                            item.AllocatedFreightIncome = entity.FreightIncome * (item.Quantity.ToDecimal() / saleorder.TotalQty.ToDecimal());
                            item.AllocatedFreightIncome = item.AllocatedFreightIncome.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                            item.FreightAllocationRules = _appContext.SysConfig.FreightAllocationRules;
                        }
                    }
                }


                entity.tb_SaleOutDetails = NewDetails;
                entity.OutDate = System.DateTime.Now;
                entity.DeliveryDate = System.DateTime.Now;

                BusinessHelper.Instance.InitEntity(entity);

                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.CustomerVendor_ID = saleorder.CustomerVendor_ID;
                    entity.SaleOrderNo = saleorder.SOrderNo;
                    entity.PlatformOrderNo = saleorder.PlatformOrderNo;
                    entity.IsFromPlatform = saleorder.IsFromPlatform;
                }
                IBizCodeGenerateService bizCodeService = _appContext.GetRequiredService<IBizCodeGenerateService>();
                entity.SaleOutNo = await bizCodeService.GenerateBizBillNoAsync(BizType.销售出库单, CancellationToken.None);
                //if (NewDetails.Count != details.Count)
                //{
                //    //已经出库过，第二次不包括 运费
                //    entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                //    entity.TotalCost = NewDetails.Sum(c => c.Cost * c.Quantity);
                //    entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                //    entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                //    entity.TotalUntaxedAmount = NewDetails.Sum(c => c.SubtotalUntaxedAmount);
                //}
                entity.tb_saleorder = saleorder;

                entity.TotalCost = NewDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);
                entity.TotalCost = entity.TotalCost + entity.FreightCost;
                entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                entity.TotalTaxAmount = entity.TotalTaxAmount.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());

                entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.FreightIncome;


                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }


        /// <summary>
        /// 销售转换为采购订单
        /// </summary>
        /// <param name="saleorder"></param>
        public tb_PurOrder SaleOrderToPurOrder(tb_SaleOrder saleorder)
        {
            tb_PurOrder entity = new tb_PurOrder();
            //转单
            if (saleorder != null)
            {
                if (authorizeController == null)
                {
                    authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                }

                entity = mapper.Map<tb_PurOrder>(saleorder);
                //注意转过来的实体  各种状态要重新赋值不然逻辑有问题，保存就是已经审核
                entity.ApprovalOpinions = "快捷转单";
                entity.ApprovalResults = null;
                entity.DataStatus = (int)DataStatus.草稿;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.Approver_at = null;
                entity.Approver_by = null;
                entity.PrintStatus = 0;
                entity.ActionStatus = ActionStatus.新增;
                entity.ApprovalOpinions = "";
                entity.Modified_at = null;
                entity.Modified_by = null;
                entity.CustomerVendor_ID = 0;//销售订单是客户，这里是厂商。默认为空。验证时要填写
                entity.PayStatus = 0;
                entity.Paytype_ID = null;
                List<string> tipsMsg = new List<string>();
                List<tb_PurOrderDetail> details = mapper.Map<List<tb_PurOrderDetail>>(saleorder.tb_SaleOrderDetails);
                entity.TotalQty = details.Sum(c => c.Quantity);

                entity.tb_PurOrderDetails = details;
                entity.PurDate = System.DateTime.Now;
                entity.PreDeliveryDate = saleorder.PreDeliveryDate;
                BusinessHelper.Instance.InitEntity(entity);
                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.SOrderNo = saleorder.SOrderNo;//销售订单号.
                    entity.Notes = saleorder.Notes;
                    entity.tb_saleorder = saleorder;
                    entity.ShipCost = saleorder.FreightIncome;
                    //销售订单单号 转为 采购订单单号
                    entity.PurOrderNo = saleorder.SOrderNo.Replace("SO", "PO");
                }
                entity.tb_saleorder = saleorder;
                entity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
                entity.TotalTaxAmount = entity.TotalTaxAmount.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                entity.TotalAmount = details.Sum(c => c.UnitPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.ShipCost;
                BusinessHelper.Instance.InitEntity(entity);
            }
            return entity;
        }



        /// <summary>
        /// 订单取消作废
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_SaleOrder>> CancelOrder(tb_SaleOrder entity, string CancelReason)
        {
            ReturnResults<tb_SaleOrder> rmrs = new ReturnResults<tb_SaleOrder>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                var authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region  预收款单处理
                    tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment> ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    var PrePayment = await ctrpay.IsExistEntityAsync(p => p.SourceBillId == entity.SOrder_ID && p.SourceBizType == (int)BizType.销售订单);
                    if (PrePayment != null)
                    {
                        if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 ||
                            PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核 ||
                            PrePayment.PrePaymentStatus == (int)PrePaymentStatus.已生效)
                        {
                            //没有付款记录的，直接删除关闭
                            await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                            #region  检测对应的收款单记录，如果没有支付也可以直接删除

                            var PaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                  .Includes(a => a.tb_FM_PaymentRecordDetails)
                                 .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == PrePayment.PreRPID)).ToListAsync();
                            if (PaymentList != null && PaymentList.Count > 0)
                            {
                                if (PaymentList.Count > 1 && PaymentList.Sum(c => c.TotalLocalAmount) == 0 && PaymentList.Any(c => c.IsReversed))
                                {

                                    if (_appContext.FMConfig.EnableAutoRefundOnOrderCancel && entity.IsFromPlatform)
                                    {

                                    }
                                    else
                                    {
                                        //退款冲销过
                                        _unitOfWorkManage.RollbackTran();
                                        rmrs.ErrorMsg = $"销售订单{PrePayment.SourceBillNo}的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，不能直接取消订单,只能【预收款退款】作废。";
                                        rmrs.Succeeded = false;
                                        return rmrs;
                                    }

                                }
                                else
                                {
                                    tb_FM_PaymentRecord Payment = PaymentList[0];
                                    if (Payment.PaymentStatus == (int)PaymentStatus.草稿 || Payment.PaymentStatus == (int)PaymentStatus.待审核)
                                    {
                                        var PaymentCounter = await _unitOfWorkManage.GetDbClient().DeleteNav(Payment)
                                            .Include(c => c.tb_FM_PaymentRecordDetails)
                                            .ExecuteCommandAsync();
                                        //if (PaymentCounter)
                                        //{
                                        //    await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                                        //}
                                    }
                                    else
                                    {
                                        if (_appContext.FMConfig.EnableAutoRefundOnOrderCancel && entity.IsFromPlatform)
                                        {
                                            var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                            bool isRefund = true;
                                            tb_FM_PaymentRecord paymentRecord = await paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { PrePayment }, isRefund);
                                            paymentRecord.Remark += "（作废）" + CancelReason;
                                            var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                                            if (rrs.Succeeded)
                                            {
                                                await paymentController.ApprovalAsync(paymentRecord, true);
                                            }
                                        }
                                        else
                                        {
                                            _unitOfWorkManage.RollbackTran();
                                            rmrs.ErrorMsg = $"对应的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，取消失败\r\n" +
                                                $"需将预收款单【退款】，对收款单{Payment.PaymentNo}进行冲销处理\r\n" +
                                                $"当前订单【作废】后，重新录入正确的销售订单。";
                                            rmrs.Succeeded = false;
                                            return rmrs;
                                        }

                                    }
                                }

                            }

                            #endregion
                        }
                        else
                        {
                            //订单取消后，预收款，可以退款可以下一个订单，应收来处理。由财务决定。
                            //这里仅提醒，订金已支付
                            if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.全额核销
                                || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.部分核销)
                            {
                                rmrs.ErrorMsg = $"存在预收款单，且状态为{(PrePaymentStatus)PrePayment.PrePaymentStatus},不能直接取消订单,请撤销核销，再退款处理,或部分退款";
                                _unitOfWorkManage.RollbackTran();
                                rmrs.Succeeded = false;
                                return rmrs;
                            }

                            if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待核销)
                            {
                                if (_appContext.FMConfig.EnableAutoRefundOnOrderCancel && entity.IsFromPlatform)
                                {
                                    var paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                                    bool isRefund = true;
                                    tb_FM_PaymentRecord paymentRecord = await paymentController.BuildPaymentRecord(new List<tb_FM_PreReceivedPayment> { PrePayment }, isRefund);
                                    paymentRecord.Remark += "（作废）" + CancelReason;
                                    var rrs = await paymentController.BaseSaveOrUpdateWithChild<tb_FM_PaymentRecord>(paymentRecord, false);
                                    if (rrs.Succeeded)
                                    {
                                        await paymentController.ApprovalAsync(paymentRecord, true);
                                    }
                                }
                                else
                                {
                                    rmrs.ErrorMsg = $"存在预收款单，且状态为{(PrePaymentStatus)PrePayment.PrePaymentStatus},不能直接取消订单,请进行【退款】处理。";
                                    _unitOfWorkManage.RollbackTran();
                                    rmrs.Succeeded = false;
                                    return rmrs;
                                }

                            }
                        }

                    }

                    #endregion
                }
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                // ✅ 死锁优化：事务外批量预加载库存
                var allKeysCancelOrder = new List<(long ProdDetailID, long LocationID)>();
                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    allKeysCancelOrder.Add((child.ProdDetailID, child.Location_ID));
                }

                var invDict5 = new Dictionary<(long ProdDetailID, long LocationID), tb_Inventory>();
                if (allKeysCancelOrder.Count > 0)
                {
                    var requiredKeys = allKeysCancelOrder.Select(k => new { k.ProdDetailID, k.LocationID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.LocationID == i.Location_ID))
                        .ToListAsync();
                    invDict5 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }

                //判断是否能反审?
                if (entity.tb_SaleOuts != null
                    && (entity.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_SaleOuts.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的销售出库单，不能直接取消订单,请进行退货退款处理。";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    #region 库存表的更新 ，
                    // ✅ 从预加载字典获取（死锁优化）
                    var key = (child.ProdDetailID, child.Location_ID);
                    if (!invDict5.TryGetValue(key, out var inv) || inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;
                        inv.Quantity = 0;
                        inv.InitInventory = 0;
                        inv.Notes = "";//后面修改数据库是不需要？
                        //inv.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.InitEntity(inv);
                        invDict5[key] = inv;
                    }
                    //更新在途库存
                    inv.Sale_Qty = inv.Sale_Qty - child.Quantity;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _logger.Debug($"{entity.SOrderNo}取消时，更新库存结果为0行，请检查数据！");
                }
                entity.DataStatus = (int)DataStatus.作废;
                entity.CloseCaseOpinions += CancelReason;
                entity.Notes += $"取消原因：{CancelReason}";
                BusinessHelper.Instance.EditEntity(entity);

                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity)
                    .UpdateColumns(it => new { it.DataStatus, entity.Notes, entity.CloseCaseOpinions })
                    .ExecuteCommandAsync();

                if (result > 0)
                {
                    // 注意信息的完整性
                    _unitOfWorkManage.CommitTran();
                    rmrs.ReturnObject = entity;
                    rmrs.Succeeded = true;
                }
                else
                {
                    _unitOfWorkManage.RollbackTran();

                    rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=> 订单取消失败";
                    rmrs.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);

                rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
            }
            return rmrs;
        }





    }
}
