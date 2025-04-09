
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
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    public partial class tb_PurEntryReController<T>
    {

        /// <summary>
        /// 采购入库退回，会影响到原始采购单，UI上如果勾选影响原始采购单，则会影响到原始采购单的状态和数量，如果不勾选，则只退货。不要退了。。退货款。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurEntryRe entity = ObjectEntity as tb_PurEntryRe;
            ReturnResults<T> rs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                if (entity == null)
                {
                    return rs;
                }

                foreach (var child in entity.tb_PurEntryReDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv != null)
                    {
                        //更新库存
                        inv.Quantity = inv.Quantity - child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    /*
                  直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                 平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                 先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                 后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                 数据来源可以是多种多样的，例如：
                 采购价格：从供应商处购买产品或物品时的价格。
                 生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                 市场价格：参考市场上类似产品或物品的价格。
                  */
                    // inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //入库退回 写回入库单明细,审核用加，
                if (entity.tb_purentry != null)
                {
                    if (entity.tb_purentry.tb_PurEntryDetails == null)
                    {
                        entity.tb_purentry.tb_PurEntryDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(c => c.PurEntryID == entity.tb_purentry.PurEntryID).ToList();
                    }

                    foreach (var child in entity.tb_purentry.tb_PurEntryDetails)
                    {
                        tb_PurEntryReDetail entryDetail = entity.tb_PurEntryReDetails.Where(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID==child.Location_ID).FirstOrDefault();
                        if (entryDetail == null)
                        {
                            continue;
                        }
                        child.ReturnedQty += entryDetail.Quantity;
                        //如果已退数量大于订单数量 给出警告实际操作中 使用其他方式出库
                        if (child.ReturnedQty > entity.tb_purentry.TotalQty)
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception("退回数量不能大于对应入库数量！");
                        }
                    }
                    //更新已退回数量
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryDetail>(entity.tb_purentry.tb_PurEntryDetails).ExecuteCommandAsync();
                }


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;

                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).ExecuteCommandAsync();
                //rmr = await ctr.BaseSaveOrUpdate(EditEntity);
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
            tb_PurEntryRe entity = ObjectEntity as tb_PurEntryRe;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                //判断是否能反审? 意思是。我这个入库单错了。但是你都当入库成功进行了后面的操作了，现在要反审，那肯定不行。所以，要判断，
                if (entity.tb_PurReturnEntries != null
                    && (entity.tb_PurReturnEntries.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurReturnEntries.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购退回入库单，不能反审采购退回单。 ";
                    return rs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                foreach (var child in entity.tb_PurEntryReDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv != null)
                    {
                       
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        //不应该为空
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception(child.ProdDetailID + "期初库存不应该为空.");
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    /*
                  直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                 平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                 先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                 后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                 数据来源可以是多种多样的，例如：
                 采购价格：从供应商处购买产品或物品时的价格。
                 生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                 市场价格：参考市场上类似产品或物品的价格。
                  */
                    CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Quantity, child.TransactionPrice);
                    inv.Quantity = inv.Quantity + child.Quantity;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }
                //入库退回 写回入库单明细，
                if (entity.tb_purentry != null)
                {
                    if (entity.tb_purentry.tb_PurEntryDetails == null)
                    {
                        entity.tb_purentry.tb_PurEntryDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(c => c.PurEntryID == entity.tb_purentry.PurEntryID).ToList();
                    }

                    foreach (var child in entity.tb_purentry.tb_PurEntryDetails)
                    {
                        tb_PurEntryReDetail entryDetail = entity.tb_PurEntryReDetails
                            .Where(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID==child.Location_ID).FirstOrDefault();
                        if (entryDetail == null)
                        {
                            continue;
                        }
                        child.ReturnedQty -= entryDetail.Quantity;
                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (child.ReturnedQty < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception("已退回数量不能小于0！");
                        }
                    }
                    //更新已退回数量
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryDetail>(entity.tb_purentry.tb_PurEntryDetails).ExecuteCommandAsync();
                }
                //===============
                //也写回采购订单明细
                //退回流程不算入采购订单的已交数量

                /*
                if (entity.tb_purentry.tb_purorder == null)
                {
                    if (entity.tb_purentry.tb_purorder.tb_PurOrderDetails == null)
                    {
                        entity.tb_purentry.tb_purorder.tb_PurOrderDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderDetail>().Where(c => c.PurOrder_ID == entity.tb_purorder.PurOrder_ID).ToList();
                    }

                    foreach (var child in entity.tb_purorder.tb_PurOrderDetails)
                    {
                        tb_PurEntryDetail entryDetail = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == child.ProdDetailID).FirstOrDefault();
                        if (entryDetail == null)
                        {
                            continue;
                        }
                        child.DeliveredQuantity -= entryDetail.Quantity;
                        //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                        if (child.DeliveredQuantity < 0)
                        {
                            throw new Exception("已入库数量不能小于0！");
                        }
                    }
                    //更新已交数量
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                }
                */



                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalOpinions = "被反审核";
                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalOpinions,
                    it.Modified_at,
                    it.Modified_by,
                    it.ApprovalResults,
                    it.ApprovalStatus
                }).ExecuteCommandAsync();
                // await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).ExecuteCommandAsync();

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


        /// <summary>
        /// 批量结案  销售出库标记结案，数据状态为8,可以修改付款状态，同时检测销售订单的付款状态，也可以更新销售订单付款状态
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程。 如：返厂后不用退回的。扣货款的。则可以直接结案。
        /// 结案时，如果引用了入库单，则要更新入库和他对应的采购单为结案状态。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_PurEntryRe> entitys = new List<tb_PurEntryRe>();
            entitys = NeedCloseCaseList as List<tb_PurEntryRe>;
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
                        if (entity.PurEntryID.HasValue)
                        {
                            if (entity.tb_purentry == null)
                            {
                                entity.tb_purentry = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                                    .Includes(a => a.tb_purorder)
                                    .Where(c => c.PurEntryID == entity.PurEntryID.Value)
                                    .SingleAsync();
                            }
                            if (entity.tb_purentry.DataStatus == (int)DataStatus.确认)
                            {
                                entity.tb_purentry.DataStatus = (int)DataStatus.完结;
                            }
                         
                            BusinessHelper.Instance.EditEntity(entity);
                            if (entity.tb_purentry.tb_purorder == null && entity.tb_purentry.PurOrder_ID.HasValue)
                            {
                                entity.tb_purentry.tb_purorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                   .Where(c => c.PurOrder_ID == entity.tb_purentry.PurOrder_ID.Value)
                                   .SingleAsync();
                            }
                            if (entity.tb_purentry.tb_purorder.DataStatus == (int)DataStatus.确认)
                            {
                                entity.tb_purentry.tb_purorder.DataStatus = (int)DataStatus.完结;
                            }
                            entity.tb_purentry.tb_purorder.CloseCaseOpinions = "由采购退货单关联式结案";
                            BusinessHelper.Instance.EditEntity(entity);
                            var affectedPORows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrder>(entity).UpdateColumns(it => new
                            {
                                it.DataStatus,
                                it.CloseCaseOpinions,
                                it.Modified_by,
                                it.Modified_at
                            }).ExecuteCommandAsync();

                            var affectedPERows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(entity).UpdateColumns(it => new
                            {
                                it.DataStatus,
                                it.Modified_by,
                                it.Modified_at
                            }).ExecuteCommandAsync();

                        }

                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryRe>(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
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




        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            //var queryable = _appContext.Db.Queryable<tb_SaleOrderDetail>();
            //var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();
            List<tb_PurEntryRe> list = await _appContext.Db.CopyNew().Queryable<tb_PurEntryRe>().Where(m => m.PurEntryRe_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }


    }
}



