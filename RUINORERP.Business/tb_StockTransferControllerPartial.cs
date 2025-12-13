
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/12/2023 14:45:18
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
using System.Linq;
using SqlSugar;
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    public partial class tb_StockTransferController<T>
    {


        /// <summary>
        /// 调拨单审核，
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmsr = new ReturnResults<T>();
            tb_StockTransfer entity = ObjectEntity as tb_StockTransfer;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

              var   ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                if (entity == null)
                {
                    return rmsr;
                }
                List<tb_Inventory> invUpdateListFrom = new List<tb_Inventory>();
                List<tb_Inventory> invUpdateListTo = new List<tb_Inventory>();
                foreach (var child in entity.tb_StockTransferDetails)
                {
                    //先看库存表中是否存在记录。  

                    #region 库存表的更新 调出
                    //标记是否有期初

                    tb_Inventory invFrom = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID_from);
                    if (invFrom != null)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (invFrom.Quantity - child.Qty) < 0)
                        {
                            rmsr.ErrorMsg = "系统设置不允许负库存，请检查调出数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rmsr.Succeeded = false;
                            return rmsr;
                        }

                        //更新库存
                        invFrom.Quantity = invFrom.Quantity - child.Qty;
                        invFrom.Inv_SubtotalCostMoney = invFrom.Inv_Cost * invFrom.Quantity;
                        invFrom.LatestOutboundTime = System.DateTime.Now;
                        BusinessHelper.Instance.EditEntity(invFrom);

                    }
                    else
                    {
                        rmsr.ErrorMsg = "调出仓库中不存在这个产品的库存，出库产品必须存在于仓库中。";
                        _unitOfWorkManage.RollbackTran();
                        rmsr.Succeeded = false;
                        return rmsr;
                    }
                    invUpdateListFrom.Add(invFrom);

                    #endregion
#warning 将来要更新。增加单据调拨费用，如运费杂费 入仓费，调拨的额外成本，如运费、装卸费， 出仓不管成本。入库类似采购一样算成本

                    #region 库存表的更新  调入时不考虑成本价格,如果初次入库时则使用调出时的成本。
                    //标记是否有期初

                    tb_Inventory invTo = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID_to);
                    if (invTo != null)
                    {
                        //更新库存
                        invTo.Quantity = invTo.Quantity + child.Qty;
                        invTo.LatestStorageTime = System.DateTime.Now;
                        invTo.Inv_SubtotalCostMoney = invTo.Inv_Cost * invTo.Quantity;
                        BusinessHelper.Instance.EditEntity(invTo);
                    }
                    else
                    {

                        invTo = new tb_Inventory();
                        invTo.Location_ID = entity.Location_ID_to;
                        invTo.ProdDetailID = child.ProdDetailID;
                        invTo.Quantity = invTo.Quantity + child.Qty;
                        invTo.InitInventory = (int)invTo.Quantity;
                        invTo.CostFIFO = invFrom.CostFIFO;
                        invTo.CostMonthlyWA = invFrom.CostMonthlyWA;
                        invTo.CostMovingWA = invFrom.CostMovingWA;
                        invTo.Inv_AdvCost = invFrom.Inv_AdvCost;
                        invTo.Inv_Cost = invFrom.Inv_Cost;
                        invTo.Inv_SubtotalCostMoney = invTo.Inv_Cost * invTo.Quantity;
                        invTo.LatestStorageTime = System.DateTime.Now;
                        invTo.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(invTo);
                    }
                    invUpdateListTo.Add(invTo);
                    #endregion


                }

                 

                int InvUpdateCounterFrom = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateListFrom)
                    .UpdateColumns(it => new { it.Quantity, it.Inv_SubtotalCostMoney, it.LatestOutboundTime })
                    .ExecuteCommandAsync();
                if (InvUpdateCounterFrom == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("来源库存更新失败！");
                }

                List<tb_Inventory> invInsertList = invUpdateListTo.Where(c => c.Inventory_ID == 0).ToList();
                if (invInsertList.Any())
                {
                    var InvInsertCounterTo = await _unitOfWorkManage.GetDbClient().Insertable(invInsertList).ExecuteReturnSnowflakeIdListAsync();
                    if (InvInsertCounterTo.Count == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("目标库存保存失败！");
                    }
                }
               
                List<tb_Inventory> invUpdateList = invUpdateListTo.Where(c => c.Inventory_ID > 0).ToList();
                int InvUpdateCounterTo = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList)
                    .UpdateColumns(it => new { it.Quantity, it.Inv_SubtotalCostMoney, it.LatestStorageTime }).ExecuteCommandAsync();
                if (InvUpdateCounterTo == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("目标库存更新失败！");
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmsr.ReturnObject = entity as T;
                rmsr.Succeeded = true;
                return rmsr;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, BizTypeText + "事务回滚");
                rmsr.ErrorMsg = BizTypeText + ex.Message;
                return rmsr;
            }

        }


        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_StockTransfer entity = ObjectEntity as tb_StockTransfer;
            ReturnResults<T> rmsr = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                foreach (var child in entity.tb_StockTransferDetails)
                {
                    //先看库存表中是否存在记录。  

                    #region 库存表的更新 反审时，调出的要加回来。
                    //标记是否有期初

                    tb_Inventory invFrom = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID_from);
                    if (invFrom != null)
                    {
                        //更新库存
                        invFrom.Quantity = invFrom.Quantity + child.Qty;
                        invFrom.LatestStorageTime = System.DateTime.Now;
                        invFrom.Inv_SubtotalCostMoney = invFrom.Inv_Cost * invFrom.Quantity;
                        BusinessHelper.Instance.EditEntity(invFrom);
                    }
                    else
                    {
                        rmsr.ErrorMsg = "调出仓库中不存在这个产品的库存，出库产品必须存在于仓库中。";
                        _unitOfWorkManage.RollbackTran();
                        rmsr.Succeeded = false;
                        return rmsr;
                    }
                    ReturnResults<tb_Inventory> rrfrom = await ctrinv.SaveOrUpdate(invFrom);

                    #endregion

                    #region 库存表的更新  调入时不考虑成本价格,如果初次入库时则使用调出时的成本。
                    //标记是否有期初

                    tb_Inventory invTo = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID_to);
                    if (invTo != null)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (invTo.Quantity - child.Qty) < 0)
                        {
                            rmsr.ErrorMsg = "系统设置不允许负库存，请检查调出数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rmsr.Succeeded = false;
                            return rmsr;
                        }

                        //更新库存
                        invTo.Quantity = invTo.Quantity - child.Qty;
                        invTo.LatestOutboundTime = System.DateTime.Now;
                        invTo.Inv_SubtotalCostMoney = invTo.Inv_Cost * invTo.Quantity;
                        BusinessHelper.Instance.EditEntity(invTo);
                    }
                    else
                    {
                        //正常逻辑不会执行到这里
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("调入仓库中不存在这个产品的库存，出库产品必须存在于仓库中。");
                    }

                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(invTo);

                }

                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "反审";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmsr.Succeeded = true;
                rmsr.ReturnObject = entity as T;
                return rmsr;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                rmsr.ErrorMsg = BizTypeText + "反审失败，" + ex.Message;
                _logger.Error(ex, BizTypeText + "事务回滚");
                return rmsr;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_StockTransfer> list = await _appContext.Db.CopyNew().Queryable<tb_StockTransfer>().Where(m => m.StockTransferID == ID)
                             .Includes(a => a.tb_location)
                             .Includes(a => a.tb_location_locationIdTo)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_StockTransferDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

    }

}