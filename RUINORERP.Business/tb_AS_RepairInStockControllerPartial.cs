﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:53:32
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
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;

namespace RUINORERP.Business
{
    /// <summary>
    /// 维修入库单
    /// </summary>
    public partial class tb_AS_RepairInStockController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 转为维修入库单
        /// </summary>
        /// <param name="RepairOrder"></param>
        public tb_AS_RepairInStock ToRepairInStock(tb_AS_RepairOrder RepairOrder)
        {
            tb_AS_RepairInStock entity = new tb_AS_RepairInStock();
            //转单
            if (RepairOrder != null)
            {
                entity = mapper.Map<tb_AS_RepairInStock>(RepairOrder);
                entity.ApprovalOpinions = "";
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
                //entity.RepairStatus = (int)RepairStatus.待维修;
                List<string> tipsMsg = new List<string>();
                List<tb_AS_RepairInStockDetail> details = mapper.Map<List<tb_AS_RepairInStockDetail>>(RepairOrder.tb_AS_RepairOrderDetails);
                List<tb_AS_RepairInStockDetail> NewDetails = new List<tb_AS_RepairInStockDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {

                    #region 每行产品ID唯一
                    var item = RepairOrder.tb_AS_RepairOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                      && c.Location_ID == details[i].Location_ID);
                    //details[i].Quantity = item.ConfirmedQuantity - item.DeliveredQty;// 减掉已经出库的数量
                    details[i].Quantity = item.Quantity;// 确认的数量全部修，但是可以根据实际来手工修改
                                                        //评估后 材料表中的成本传过来。成本才生效
                                                        // details[i].SubtotalCostAmount = (details[i].SubtotalCost + details[i].CustomizedCost) * details[i].Quantity;
                    if (details[i].Quantity > 0)
                    {
                        NewDetails.Add(details[i]);
                    }
                    //else
                    //{
                    //    tipsMsg.Add($"当前【售后申请单】的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                    //}
                    #endregion


                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"【维修工单】:{entity.RepairOrderNo}已全部入库，请检查是否正在重复操作！");
                }

                entity.tb_AS_RepairInStockDetails = NewDetails;
                entity.EntryDate = System.DateTime.Now;
                BusinessHelper.Instance.InitEntity(entity);

                entity.RepairOrderNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.维修工单);
                entity.tb_as_repairorder = RepairOrder;
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);


                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }


        /// <summary>
        /// 数量入到维修仓库
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairInStock entity = ObjectEntity as tb_AS_RepairInStock;
            try
            {
                if ((entity.TotalQty == 0 || entity.tb_AS_RepairInStockDetails.Sum(c => c.Quantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总数量{entity.TotalQty}和明细数量之和{entity.tb_AS_RepairInStockDetails.Sum(c => c.Quantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal RepairQty)>();

                //更新  维修仓的数量增加
                foreach (var child in entity.tb_AS_RepairInStockDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentRepairQty = child.Quantity;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
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
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充 。 售后维修仓  不处理成本 所以是不是在仓库设置中设置一个参数是否处理成本
                                Notes = "维修入库单创建",
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
                            RepairQty: currentRepairQty // 首次累加
                                                           //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.RepairQty += currentRepairQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    inv.Quantity += group.Value.RepairQty.ToInt();
                    invList.Add(inv);
                }

                _unitOfWorkManage.BeginTran();
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新数据为0，更新失败！");
                }
               
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairInStock>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "维修入库审核时，事务回滚" + ex.Message);
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }

        /// <summary>
        /// 维修仓库 出来 返回维修状态
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_RepairInStock entity = ObjectEntity as tb_AS_RepairInStock;
            try
            {
                if ((entity.TotalQty == 0 || entity.tb_AS_RepairInStockDetails.Sum(c => c.Quantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总数量{entity.TotalQty}和明细数量之和{entity.tb_AS_RepairInStockDetails.Sum(c => c.Quantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal RepairQty)>();

                //更新  维修仓的数量增加
                foreach (var child in entity.tb_AS_RepairInStockDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentRepairQty = child.Quantity;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
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
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充 。 售后维修仓  不处理成本 所以是不是在仓库设置中设置一个参数是否处理成本
                                Notes = "维修入库单创建",
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
                            RepairQty: currentRepairQty // 首次累加
                                                        //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.RepairQty += currentRepairQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    inv.Quantity -= group.Value.RepairQty.ToInt();
                    invList.Add(inv);
                }

                _unitOfWorkManage.BeginTran();
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新数据为0，更新失败！");
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairInStock>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions
                }).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "维修入库审核时，事务回滚" + ex.Message);
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }
    }
}



