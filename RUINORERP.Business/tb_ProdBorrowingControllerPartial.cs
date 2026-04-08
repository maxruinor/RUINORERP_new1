
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:38
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

using RUINORERP.Model.Base;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.Business.EntityLoadService;


namespace RUINORERP.Business
{
    public partial class tb_ProdBorrowingController<T>
    {

        /// <summary>
        /// 审核借出 单据状态审核部分：负责验证单据数据合法性、权限检查及状态更新
        /// 不包含库存操作逻辑，库存操作由ConfirmExecution方法处理
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rsms = new ReturnResults<T>();
            tb_ProdBorrowing entity = ObjectEntity as tb_ProdBorrowing;

            try
            {
                // 验证单据数据合法性
                if (entity == null)
                {
                    rsms.ErrorMsg = "单据实体为空，无法审核";
                    rsms.Succeeded = false;
                    return rsms;
                }

                if (entity.tb_ProdBorrowingDetails == null || entity.tb_ProdBorrowingDetails.Count == 0)
                {
                    rsms.ErrorMsg = "借出单明细为空，无法审核";
                    rsms.Succeeded = false;
                    return rsms;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                // 更新单据状态为确认
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);

                // 只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                              .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                              .ExecuteCommandHasChangeAsync();
                _unitOfWorkManage.CommitTran();

                rsms.ReturnObject = entity as T;
                rsms.Succeeded = true;
                return rsms;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rsms.ErrorMsg = "审核事务回滚=>" + ex.Message;
                return rsms;
            }
        }

        /// <summary>
        /// 确认执行借出单 库存操作执行部分：负责实际的库存变动逻辑实现
        /// 该方法从审核方法中分离出来，作为独立的库存操作入口
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ConfirmExecutionAsync(T ObjectEntity)
        {
            ReturnResults<T> rsms = new ReturnResults<T>();
            tb_ProdBorrowing entity = ObjectEntity as tb_ProdBorrowing;

            try
            {
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys = new List<(long ProdDetailID, long Location_ID)>();
                if (entity.tb_ProdBorrowingDetails != null)
                {
                    foreach (var detail in entity.tb_ProdBorrowingDetails)
                    {
                        allKeys.Add((detail.ProdDetailID, detail.Location_ID));
                    }
                }

                var invDict1 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                if (allKeys.Count > 0)
                {
                    var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict1 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                // 创建库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();

                foreach (var child in entity.tb_ProdBorrowingDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    // ✅ 从预加载字典获取（死锁优化）
                    var key = (child.ProdDetailID, child.Location_ID);
                    invDict1.TryGetValue(key, out var inv);
                    if (inv != null)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Qty) < 0)
                        {
                            rsms.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rsms.Succeeded = false;
                            return rsms;
                        }
                        //更新库存
                        inv.Quantity = inv.Quantity - child.Qty;

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据,请联系管理员");
                    }

                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        // 实时获取当前库存成本
                        decimal realtimeCost = inv.Inv_Cost;

                        // 更新借出明细的成本为实时成本
                        child.Cost = realtimeCost;
                        child.SubtotalCostAmount = realtimeCost * child.Qty;

                        // 创建库存流水记录
                        tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                        transaction.ProdDetailID = inv.ProdDetailID;
                        transaction.Location_ID = inv.Location_ID;
                        transaction.BizType = (int)BizType.借出单;
                        transaction.ReferenceId = entity.BorrowID;
                        transaction.ReferenceNo = entity.BorrowNo;
                        transaction.BeforeQuantity = inv.Quantity + child.Qty; // 变动前的库存数量
                        transaction.QuantityChange = -child.Qty; // 借出减少库存
                        transaction.AfterQuantity = inv.Quantity;
                        transaction.UnitCost = realtimeCost; // 使用实时成本
                        transaction.TransactionTime = DateTime.Now;
                        transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                        transaction.Notes = $"借出单确认执行：{entity.BorrowNo}，产品：{inv.tb_proddetail?.tb_prod?.CNName}";

                        transactionList.Add(transaction);
                    }
                }

                // 记录库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);

                // 更新借出明细的成本信息
                await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_ProdBorrowingDetails)
                    .UpdateColumns(it => new { it.Cost, it.SubtotalCostAmount })
                    .ExecuteCommandHasChangeAsync();

                entity.DataStatus = (int)DataStatus.完结;
                entity.CloseCaseOpinions = "确认执行";
                // 更新借出单的状态
                await _unitOfWorkManage.GetDbClient().Updateable(entity)
                    .UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions })
                    .ExecuteCommandHasChangeAsync();


                _unitOfWorkManage.CommitTran();

                rsms.ReturnObject = entity as T;
                rsms.Succeeded = true;
                return rsms;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rsms.ErrorMsg = "确认执行事务回滚=>" + ex.Message;
                return rsms;
            }
        }


        /// <summary>
        /// 反审核（仅状态回退）：将单据状态从确认回退到新建
        /// 不包含库存操作，库存操作由 AntiConfirmExecutionAsync 处理
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rsms = new ReturnResults<T>();
            tb_ProdBorrowing entity = ObjectEntity as tb_ProdBorrowing;
        
            try
            {
                // 验证单据数据合法性
                if (entity == null)
                {
                    rsms.ErrorMsg = "单据实体为空，无法反审核";
                    rsms.Succeeded = false;
                    return rsms;
                }
        
                // 判断是否能反审？必须是已审核但未执行的状态
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    rsms.ErrorMsg = "只有已审核的单据才能反审核";
                    rsms.Succeeded = false;
                    return rsms;
                }
        
                // 检查是否已执行（完结状态），已执行的单据不能直接反审核
                if (entity.DataStatus == (int)DataStatus.完结)
                {
                    rsms.ErrorMsg = "此单据已执行，无法直接反审核！\n\n请先执行【反执行】操作回滚库存，然后再反审核。";
                    rsms.Succeeded = false;
                    return rsms;
                }
        
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
        
                // 更新单据状态为新建（仅状态变更）
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
        
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                         .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                         .ExecuteCommandHasChangeAsync();
        
                _unitOfWorkManage.CommitTran();
        
                rsms.ReturnObject = entity as T;
                rsms.Succeeded = true;
                return rsms;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rsms.ErrorMsg = "反审核事务回滚=>" + ex.Message;
                return rsms;
            }
        }

        /// <summary>
        /// 反执行（库存回滚）：回滚借出单的库存操作
        /// 将库存反向变动（增加），并回退状态到确认
        /// 注意：此方法仅处理已执行的单据（DataStatus=完结）
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiConfirmExecutionAsync(T ObjectEntity)
        {
            ReturnResults<T> rsms = new ReturnResults<T>();
            tb_ProdBorrowing entity = ObjectEntity as tb_ProdBorrowing;
        
            try
            {
                // 验证单据数据合法性
                if (entity == null)
                {
                    rsms.ErrorMsg = "单据实体为空，无法反执行";
                    rsms.Succeeded = false;
                    return rsms;
                }
        
                // 只有完结状态的单据才能反执行
                if (entity.DataStatus != (int)DataStatus.完结)
                {
                    rsms.ErrorMsg = "只有已执行的单据才能反执行！";
                    rsms.Succeeded = false;
                    return rsms;
                }
        
                #region 【死锁优化】预处理阶段（事务外批量预加载库存）
                var allKeys2 = new List<(long ProdDetailID, long Location_ID)>();
                if (entity.tb_ProdBorrowingDetails != null)
                {
                    foreach (var detail in entity.tb_ProdBorrowingDetails)
                    {
                        allKeys2.Add((detail.ProdDetailID, detail.Location_ID));
                    }
                }
        
                var invDict2 = new Dictionary<(long ProdDetailID, long Location_ID), tb_Inventory>();
                if (allKeys2.Count > 0)
                {
                    var requiredKeys = allKeys2.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
                    var inventoryList = await _unitOfWorkManage.GetDbClient()
                        .Queryable<tb_Inventory>()
                        .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
                        .ToListAsync();
                    invDict2 = inventoryList.ToDictionary(i => (i.ProdDetailID, i.Location_ID));
                }
                #endregion
        
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
        
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
        
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                // 创建反向库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
        
                foreach (var child in entity.tb_ProdBorrowingDetails)
                {
                    #region 库存表的更新（反向操作：增加库存）
                    // ✅ 从预加载字典获取（死锁优化）
                    var key = (child.ProdDetailID, child.Location_ID);
                    invDict2.TryGetValue(key, out var inv);
                    if (inv == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rsms.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法借出。请使用【期初盘点】【采购入库】【生产缴库】的方式进行盘点后，再操作。";
                        rsms.Succeeded = false;
                        return rsms;
        
                    }
                    //更新在途库存
                    //反执行：出库的要加回来
                    inv.Quantity = inv.Quantity + child.Qty;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    invUpdateList.Add(inv);
        
                    // 实时获取当前库存成本
                    decimal realtimeCost = inv.Inv_Cost;
        
                    // 创建反向库存流水记录
                    tb_InventoryTransaction transaction = new tb_InventoryTransaction();
                    transaction.ProdDetailID = inv.ProdDetailID;
                    transaction.Location_ID = inv.Location_ID;
                    transaction.BizType = (int)BizType.借出单;
                    transaction.ReferenceId = entity.BorrowID;
                    transaction.ReferenceNo = entity.BorrowNo;
                    transaction.BeforeQuantity = inv.Quantity - child.Qty; // 变动前的库存数量
                    transaction.QuantityChange = child.Qty; // 反执行增加库存
                    transaction.AfterQuantity = inv.Quantity;
                    transaction.UnitCost = realtimeCost; // 使用实时成本
                    transaction.TransactionTime = DateTime.Now;
                    transaction.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transaction.Notes = $"借出单反确认执行：{entity.BorrowNo}，产品：{inv.tb_proddetail?.tb_prod?.CNName}";
        
                    transactionList.Add(transaction);
                }
                        
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvMainCounter == 0)
                {
                    _logger.Debug($"{entity.BorrowNo}更新库存结果为 0 行，请检查数据！");
                }
        
                // 记录反向库存流水（带死锁重试机制）
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactionsWithRetry(transactionList);
        
                // 回退单据状态到确认
                entity.DataStatus = (int)DataStatus.确认;
                entity.CloseCaseOpinions = "反执行";
                        
                await _unitOfWorkManage.GetDbClient().Updateable(entity)
                    .UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions })
                    .ExecuteCommandHasChangeAsync();
        
                _unitOfWorkManage.CommitTran();
        
                rsms.ReturnObject = entity as T;
                rsms.Succeeded = true;
                return rsms;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rsms.ErrorMsg = "反确认执行事务回滚=>" + ex.Message;
                return rsms;
            }
        }

        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_ProdBorrowing> list = await _appContext.Db.CopyNew().Queryable<tb_ProdBorrowing>().Where(m => m.BorrowID == ID)
                            .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProdBorrowingDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }


    }


}



