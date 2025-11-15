
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:27
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
using RUINOR.Core;
using RUINORERP.Common.Helper;
using RUINORERP.Business.CommService;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.BizMapperService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 售后交付单
    /// </summary>
    public partial class tb_AS_AfterSaleDeliveryController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 将售后商品转回到维修仓库。 变为没有交付审核前的状态
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_AfterSaleDelivery entity = ObjectEntity as tb_AS_AfterSaleDelivery;
            try
            {

                if ((entity.TotalDeliveryQty == 0 || entity.tb_AS_AfterSaleDeliveryDetails.Sum(c => c.Quantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总复核数量{entity.TotalDeliveryQty}和明细复核数量之和{entity.tb_AS_AfterSaleDeliveryDetails.Sum(c => c.Quantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //当第一次审核失败后，Undo值是没有把tb_SaleOutDetails保存回去。导致 为null  暂时先查一下。后面优化尝试复制，恢复
                if (entity.tb_AS_AfterSaleDeliveryDetails == null)
                {
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleDelivery>()
                        .Includes(c => c.tb_AS_AfterSaleDeliveryDetails)
                        .Where(d => d.ASDeliveryID == entity.ASDeliveryID).FirstAsync();
                }


                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal Qty)>();

                //更新
                foreach (var child in entity.tb_AS_AfterSaleDeliveryDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentOutQty = child.Quantity;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            throw new Exception($"售后交付单反审核时，库存不存在，ProdDetailID：{child.ProdDetailID}，LocationID：{child.Location_ID}");
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            Qty: currentOutQty // 首次累加
                                               //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.Qty += currentOutQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段
                    inv.Quantity += group.Value.Qty.ToInt();
                    invList.Add(inv);
                }

                _unitOfWorkManage.BeginTran();
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("售后交付单，库存更新数据为0，更新失败！");
                }
                #region 回写售后交付数量
                if (entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0)
                {
                    entity.tb_as_aftersaleapply = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApply>()
                         .Includes(c => c.tb_AS_AfterSaleApplyDetails)
                        .Where(c => c.ASApplyID == entity.ASApplyID).FirstAsync();

                    List<tb_AS_AfterSaleDeliveryDetail> AfterSaleDeliveryDetails = new List<tb_AS_AfterSaleDeliveryDetail>();
                    AfterSaleDeliveryDetails.AddRange(entity.tb_AS_AfterSaleDeliveryDetails);

                    for (int i = 0; i < entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails.Count; i++)
                    {
                        tb_AS_AfterSaleApplyDetail AfterSaleApplyDetail = entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails[i];
                        var totalDeliveryQty = AfterSaleDeliveryDetails.Where(c => c.ProdDetailID == AfterSaleApplyDetail.ProdDetailID
                        && c.Location_ID == AfterSaleApplyDetail.Location_ID).ToList().Sum(c => c.Quantity);
                        //没有交付
                        if (totalDeliveryQty == 0)
                        {
                            continue;
                        }

                        AfterSaleApplyDetail.DeliveredQty -= totalDeliveryQty;
                    }
                    entity.tb_as_aftersaleapply.TotalDeliveredQty = entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails.Sum(c => c.DeliveredQty);
                    //更新交付数量
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails)
                        .UpdateColumns(t => new { t.DeliveredQty }).ExecuteCommandAsync();

                    //更新交付数量
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_aftersaleapply)
                        .UpdateColumns(t => new { t.TotalDeliveredQty }).ExecuteCommandAsync();


                    if (entity.tb_as_aftersaleapply.TotalConfirmedQuantity != entity.tb_as_aftersaleapply.TotalDeliveredQty)
                    {
                        entity.tb_as_aftersaleapply.DataStatus = (int)DataStatus.确认;
                        entity.tb_as_aftersaleapply.ASProcessStatus = (int)ASProcessStatus.待交付;
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_aftersaleapply).UpdateColumns(t => new { t.DataStatus,t.ASProcessStatus }).ExecuteCommandAsync();
                    }
                }
                #endregion

                //这部分是否能提出到上一级公共部分
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalResults = false;
                entity.ApprovalOpinions = "";
                entity.Approver_at = null;
                entity.Approver_by = null;

                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleDelivery>(entity).UpdateColumns(it => new
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
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }


        /// <summary>
        /// 将售后商品转出
        /// </summary>
        /// <param name="ObjectEntity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_AS_AfterSaleDelivery entity = ObjectEntity as tb_AS_AfterSaleDelivery;
            try
            {

                if ((entity.TotalDeliveryQty == 0 || entity.tb_AS_AfterSaleDeliveryDetails.Sum(c => c.Quantity) == 0))
                {
                    rmrs.ErrorMsg = $"单据总复核数量{entity.TotalDeliveryQty}和明细复核数量之和{entity.tb_AS_AfterSaleDeliveryDetails.Sum(c => c.Quantity)},其中有数据为零，请检查后再试！";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //当第一次审核失败后，Undo值是没有把tb_SaleOutDetails保存回去。导致 为null  暂时先查一下。后面优化尝试复制，恢复
                if (entity.tb_AS_AfterSaleDeliveryDetails == null)
                {
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleDelivery>()
                        .Includes(c => c.tb_AS_AfterSaleDeliveryDetails)
                        .Where(d => d.ASDeliveryID == entity.ASDeliveryID).FirstAsync();
                }


                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal Qty)>();

                //更新
                foreach (var child in entity.tb_AS_AfterSaleDeliveryDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentOutQty = child.Quantity;
                    DateTime currentOutboundTime = DateTime.Now; // 每次出库更新时间
                                                                 // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            throw new Exception($"售后交付单审核时，库存不存在，ProdDetailID：{child.ProdDetailID}，LocationID：{child.Location_ID}");
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            Qty: currentOutQty // 首次累加
                                               //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.Qty += currentOutQty;
                        if (!_appContext.SysConfig.CheckNegativeInventory && (group.Inventory.Quantity - group.Qty) < 0)
                        {
                            // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                            rmrs.ErrorMsg = $"sku:{group.Inventory.tb_proddetail.SKU}库存为：{group.Inventory.Quantity}，要交付的数量为：{group.Qty}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据";
                            _unitOfWorkManage.RollbackTran();
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段
                    inv.Quantity -= group.Value.Qty.ToInt();
                    inv.LatestOutboundTime = System.DateTime.Now;
                    invList.Add(inv);
                }

                _unitOfWorkManage.BeginTran();
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("售后交付单,库存更新数据为0，更新失败！");
                }
                #region 回写售后交付数量
                if (entity.ASApplyID.HasValue && entity.ASApplyID.Value > 0)
                {
                    entity.tb_as_aftersaleapply = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApply>()
                         .Includes(c => c.tb_AS_AfterSaleApplyDetails)
                        .Where(c => c.ASApplyID == entity.ASApplyID).FirstAsync();

                    List<tb_AS_AfterSaleDeliveryDetail> AfterSaleDeliveryDetails = new List<tb_AS_AfterSaleDeliveryDetail>();
                    AfterSaleDeliveryDetails.AddRange(entity.tb_AS_AfterSaleDeliveryDetails);

                    for (int i = 0; i < entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails.Count; i++)
                    {
                        tb_AS_AfterSaleApplyDetail AfterSaleApplyDetail = entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails[i];
                        var totalDeliveryQty = AfterSaleDeliveryDetails.Where(c => c.ProdDetailID == AfterSaleApplyDetail.ProdDetailID
                        && c.Location_ID == AfterSaleApplyDetail.Location_ID).ToList().Sum(c => c.Quantity);
                        //没有交付
                        if (totalDeliveryQty == 0)
                        {
                            continue;
                        }

                        AfterSaleApplyDetail.DeliveredQty += totalDeliveryQty;
                        if (AfterSaleApplyDetail.DeliveredQty > AfterSaleApplyDetail.ConfirmedQuantity)
                        {
                            throw new Exception($"售后交付单中,交付数量{AfterSaleApplyDetail.DeliveredQty}不能大于申请单时复核的数量{AfterSaleApplyDetail.ConfirmedQuantity}，更新失败！");
                        }
                    }

                    entity.tb_as_aftersaleapply.TotalDeliveredQty = entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails.Sum(c => c.DeliveredQty);
                    //更新交付数量
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_aftersaleapply.tb_AS_AfterSaleApplyDetails)
                        .UpdateColumns(t => new { t.DeliveredQty }).ExecuteCommandAsync();

                    //更新交付数量
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_aftersaleapply)
                        .UpdateColumns(t => new { t.TotalDeliveredQty }).ExecuteCommandAsync();


                    if (entity.tb_as_aftersaleapply.TotalConfirmedQuantity == entity.tb_as_aftersaleapply.TotalDeliveredQty)
                    {
                        entity.tb_as_aftersaleapply.DataStatus = (int)DataStatus.完结;
                        entity.tb_as_aftersaleapply.ASProcessStatus = (int)ASProcessStatus.已完成;
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_as_aftersaleapply).UpdateColumns(t => new { t.DataStatus, t.ASProcessStatus }).ExecuteCommandAsync();
                    }
                }
                #endregion

                //这部分是否能提出到上一级公共部分
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleDelivery>(entity).UpdateColumns(it => new
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
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_AS_AfterSaleDelivery> list = await _appContext.Db.CopyNew().Queryable<tb_AS_AfterSaleDelivery>().Where(m => m.ASDeliveryID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_projectgroup)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_AS_AfterSaleDeliveryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }
    }
}



