
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/04/2025 18:27:23
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
using RUINORERP.Business.Security;
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using System.Windows.Forms;
using SqlSugar;

namespace RUINORERP.Business
{
    /// <summary>
    /// 返工退库
    /// </summary>
    public partial class tb_MRP_ReworkReturnController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 返回批量审核的结果
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_MRP_ReworkReturn entity = ObjectEntity as tb_MRP_ReworkReturn;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //采购入库总数量和明细求和检查
                if (entity.TotalQty.Equals(entity.tb_MRP_ReworkReturnDetails.Sum(c => c.Quantity)) == false)
                {
                    rs.ErrorMsg = $"采入入库数量与明细之和不相等!请检查数据后重试！";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                //如果引用了制令单。退库数量不能大于制令单已交数量
                if (entity.MOID.HasValue && entity.tb_manufacturingorder == null)
                {
                    entity.tb_manufacturingorder = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_MRP_ReworkReturns, b => b.tb_MRP_ReworkReturnDetails, c => c.tb_proddetail, d => d.tb_prod)
                         .Includes(a => a.tb_ManufacturingOrderDetails)
                         .Where(c => c.MOID == entity.MOID)
                         .Single();
                    //如果入库明细中的产品。不存在于订单中。审核失败。
                    foreach (var child in entity.tb_MRP_ReworkReturnDetails)
                    {
                        if (entity.tb_manufacturingorder.ProdDetailID == child.ProdDetailID)
                        {
                            rs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"入库明细中有产品不属于当前订单!请检查数据后重试！";
                            return rs;
                        }
                    }

                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                    List<tb_MRP_ReworkReturnDetail> detailList = new List<tb_MRP_ReworkReturnDetail>();
                    foreach (var item in entity.tb_manufacturingorder.tb_MRP_ReworkReturns)
                    {
                        detailList.AddRange(item.tb_MRP_ReworkReturnDetails);
                    }

                    //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                    string prodName = entity.tb_manufacturingorder.tb_proddetail.tb_prod.CNName +
                              entity.tb_manufacturingorder.tb_proddetail.tb_prod.Specifications;

                    //一对一时
                    var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_manufacturingorder.ProdDetailID).Sum(c => c.Quantity);
                    if (inQty > entity.tb_manufacturingorder.QuantityDelivered)
                    {

                        string msg = $"返工退库:{entity.ReworkReturnNo}的【{prodName}】的退库数量不能大于制令单中已交的数量\r\n" + $"或存在针对当前制令单重复录入了返工退库单，审核失败！";
                        MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _unitOfWorkManage.RollbackTran();
                        _logger.LogInformation(msg);
                        return rs;
                    }
                }

                foreach (tb_MRP_ReworkReturnDetail child in entity.tb_MRP_ReworkReturnDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        rs.ErrorMsg = $"必须要有库存初始数据!请检查数据后重试！";
                        _unitOfWorkManage.RollbackTran();
                        rs.Succeeded = false;
                        return rs;
                    }
                    else
                    {

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;
                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;

                  

                    inv.Quantity = inv.Quantity - child.Quantity;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion

                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(child.ProdDetailID + "==>" + child.property + "库存更新成功");
                        }
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //  entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //  entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                int counter = await _unitOfWorkManage.GetDbClient().Updateable<tb_MRP_ReworkReturn>(entity).ExecuteCommandAsync();
                if (counter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Info(entity.ReworkReturnNo + "==>" + "状态更新成功");
                    }
                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚");
                rs.Succeeded = false;
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }



        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_MRP_ReworkReturn entity = ObjectEntity as tb_MRP_ReworkReturn;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
              

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                foreach (var child in entity.tb_MRP_ReworkReturnDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要
               
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        rs.ErrorMsg = $"必须要有库存初始数据!请检查数据后重试！";
                        _unitOfWorkManage.RollbackTran();
                        rs.Succeeded = false;
                        return rs;
                    }
                    else
                    {

                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;
                  
                    inv.Quantity = inv.Quantity - child.Quantity;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;

                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                     
                    }
                }

                 
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_MRP_ReworkReturn>(entity).ExecuteCommandAsync();


                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_MRP_ReworkReturn> list = await _appContext.Db.CopyNew().Queryable<tb_MRP_ReworkReturn>().Where(m => m.ReworkReturnID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_MRP_ReworkReturnDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_MRP_ReworkReturnDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_MRP_ReworkReturnDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }


    }
}



