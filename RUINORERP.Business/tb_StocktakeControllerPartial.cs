
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
using Microsoft.Extensions.Hosting;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business
{
    public partial class tb_StocktakeController<T>
    {
        /// <summary>
        /// 审核盘点单 注意盘点单明细中的差数，在提交或录入时就自动计算。到审核时数据都准备好了。只是确认审核会影响其他数据以及审核状态等
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_Stocktake entity = ObjectEntity as tb_Stocktake;

            try
            {
                // 提前开启事务，确保所有数据库操作都在事务内执行
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                if (entity == null)
                {
                    _unitOfWorkManage.RollbackTran();
                    return rmrs;
                }

                //!!!child.DiffQty 是否有正负数？如果有正数
                CheckMode cm = (CheckMode)entity.CheckMode;
                //将盘点到的数据，根据处理调整类型去修改库存表，期初还需要保存到期初表中
                //最后要用事务操作
                //保存库存，增加 通过产品明细ID去找
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_StocktakeDetails)
                {
                    //先看库存表中是否存在记录。
                    #region 库存表的更新
                    //标记是否有期初

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID);
                    if (inv == null)
                    {
                        if (CheckMode.期初盘点 != cm)
                        {
                            View_ProdDetail view_Prod = await _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>().Where(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == entity.Location_ID).FirstAsync();

                            if (view_Prod == null)
                            {
                                view_Prod = _cacheManager.GetEntity<View_ProdDetail>(child.ProdDetailID);
                            }
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"{view_Prod.SKU}=> {view_Prod.CNName}\r\n当前盘点产品在当前仓库中，无库存数据。请使用【期初盘点】方式盘点。";
                            return rmrs;
                        }

                        inv = new tb_Inventory();
                        if (child.UntaxedCost == 0)
                        {
                            View_ProdDetail view_Prod = await _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>().Where(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == entity.Location_ID).FirstAsync();
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"{view_Prod.SKU}=> {view_Prod.CNName}\r\n【期初盘点】时，必须输入正确的未税成本价格。";
                            return rmrs;
                        }
                        else
                        {
                            inv.Inv_Cost = child.UntaxedCost;
                            inv.Inv_AdvCost = child.UntaxedCost;
                            inv.CostMovingWA = child.UntaxedCost;
                            inv.CostFIFO = child.UntaxedCost;
                        }
                        inv.Location_ID = entity.Location_ID;
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.InitInventory = 0;
                        inv.InitInvCost = 0;

                        inv.Notes = "期初盘点";
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {
                        inv.LastInventoryDate = System.DateTime.Now;
                        BusinessHelper.Instance.EditEntity(inv);
                    }

                    //盘点模式 三个含义是:期初时可以录入成本,另两个不可以,由库存表中带出来.
                    //并且其实盘点时只有数量大于0时才计算成本
                    if (CheckMode.期初盘点 == cm && child.DiffQty > 0)
                    {
                        //设置期初数量和成本
                        inv.InitInventory = child.DiffQty;
                        inv.InitInvCost = child.UntaxedCost;
                        inv.Inv_Cost = child.UntaxedCost;
                        inv.Inv_AdvCost = child.UntaxedCost;
                        inv.CostMovingWA = child.UntaxedCost;
                        inv.CostFIFO = child.UntaxedCost;
                        //既然是期初始录入成本。那就直接用期初成本作为库存成本
                        //CommService.CostCalculations.CostCalculation(_appContext, inv, child.DiffQty, child.UntaxedCost);
                    }
                    //更新库存
                    if (entity.Adjust_Type == (int)Adjust_Type.全部)
                    {
                        inv.Quantity = inv.Quantity + child.DiffQty;
                    }
                    if (entity.Adjust_Type == (int)Adjust_Type.减少 && child.DiffQty < 0)
                    {
                        inv.Quantity = inv.Quantity + child.DiffQty;
                    }
                    if (entity.Adjust_Type == (int)Adjust_Type.增加 && child.DiffQty > 0)
                    {
                        inv.Quantity = inv.Quantity + child.DiffQty;
                    }

                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Rack_ID = child.Rack_ID;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    //!!!child.DiffQty 是否有正负数？如果有正数
                    if (child.DiffQty > 0)
                    {
                        inv.LatestStorageTime = System.DateTime.Now;
                    }
                    if (child.DiffQty < 0)
                    {
                        inv.LatestOutboundTime = System.DateTime.Now;
                    }

                    #endregion
                    invUpdateList.Add(inv);
                }

                // 使用LINQ查询
                var CheckNewInvList = invUpdateList
                    .GroupBy(i => new { i.ProdDetailID, i.Location_ID })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key.ProdDetailID)
                    .ToList();

                if (CheckNewInvList.Count > 0)
                {
                    //新增库存中有重复的商品，操作失败。请联系管理员。
                    rmrs.ErrorMsg = "新增库存中有重复的商品，操作失败。";
                    rmrs.Succeeded = false;
                    _logger.LogError(rmrs.ErrorMsg + "详细信息：" + string.Join(",", CheckNewInvList));
                    return rmrs;
                }

                // 事务已经在方法开始处开启

                DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.Debug($"{entity.CheckNo}审核时，更新库存结果为0行，请检查数据！");
                }

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    try
                    {
                        #region 生成费用单
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ProfitLossController<tb_FM_ProfitLoss>>();
                        List<tb_FM_ProfitLoss> profitLossList = await ctrpayable.BuildProfitLoss(entity);
                        foreach (var profitLoss in profitLossList)
                        {
                            ReturnMainSubResults<tb_FM_ProfitLoss> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ProfitLoss>(profitLoss);
                        }

                        #endregion
                    }
                    catch (Exception)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("盘点单审核时，财务费用单数据保存失败！");
                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                // entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

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
                return rmrs;
            }

        }


        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_Stocktake entity = ObjectEntity as tb_Stocktake;
            ReturnResults<T> rmsr = new ReturnResults<T>();

            try
            {


                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                CheckMode cm = (CheckMode)entity.CheckMode;
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_StocktakeDetails)
                {
                    //先看库存表中是否存在记录。
                    #region 库存表的更新
                    //标记是否有期初
                    bool Opening;
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID);
                    if (inv != null)
                    {
                        Opening = false;
                        //更新库存
                        if (entity.Adjust_Type == (int)Adjust_Type.全部)
                        {
                            inv.Quantity = inv.Quantity - child.DiffQty;
                        }
                        if (entity.Adjust_Type == (int)Adjust_Type.减少 && child.DiffQty < 0)
                        {
                            inv.Quantity = inv.Quantity - child.DiffQty;
                        }
                        if (entity.Adjust_Type == (int)Adjust_Type.增加 && child.DiffQty > 0)
                        {
                            inv.Quantity = inv.Quantity - child.DiffQty;
                        }
                        inv.LastInventoryDate = null;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    else
                    {
                        Opening = true;
                        inv = new tb_Inventory();
                        inv.Location_ID = entity.Location_ID;
                        inv.Quantity = child.DiffQty;
                        inv.InitInventory = 0;
                        inv.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inv);
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

                    //如果数量没有变化。成本不参数反计算
                    if (CheckMode.期初盘点 == cm && child.DiffQty != 0)
                    {
                        CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.DiffQty, child.UntaxedCost);
                    }

                    //inv.Inv_Cost = child.UntaxedCost;//这里需要计算，根据系统设置中的算法计算。
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Rack_ID = child.Rack_ID;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    //!!!child.DiffQty 是否有正负数？如果有正数(这时是反审。要相反？）
                    if (child.DiffQty < 0)
                    {
                        inv.LatestStorageTime = System.DateTime.Now;
                    }
                    if (child.DiffQty > 0)
                    {
                        inv.LatestOutboundTime = System.DateTime.Now;
                    }

                    #endregion
                    invUpdateList.Add(inv);
                }

                // 使用LINQ查询
                var CheckNewInvList = invUpdateList.Where(c => c.Inventory_ID == 0)
                    .GroupBy(i => new { i.ProdDetailID, i.Location_ID })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key.ProdDetailID)
                    .ToList();

                if (CheckNewInvList.Count > 0)
                {
                    //新增库存中有重复的商品，操作失败。请联系管理员。
                    rmsr.ErrorMsg = "新增库存中有重复的商品，操作失败。";
                    rmsr.Succeeded = false;
                    _logger.LogError(rmsr.ErrorMsg + "详细信息：" + string.Join(",", CheckNewInvList));
                    return rmsr;

                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                DbHelper<tb_Inventory> InvdbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await InvdbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (Counter == 0)
                {
                    _logger.Debug($"{entity.CheckNo}反审核时，更新库存结果为0行，请检查数据！");
                }

                //盘点模式 三个含义是:期初时可以录入成本,另两个不可以,由库存表中带出来.
                if (CheckMode.期初盘点 == cm)
                {

                }

                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    try
                    {
                        #region  删除 生成费用单，如果没有审核则可以删除。如果审核了。则不能反审核
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ProfitLossController<tb_FM_ProfitLoss>>();
                        List<tb_FM_ProfitLoss> profitLoss = await ctrpayable.QueryByNavAsync(c => c.SourceBillNo == entity.CheckNo);
                        if (profitLoss.Where(c => c.DataStatus.Value > (int)DataStatus.新建).Any())
                        {
                            rmsr.ErrorMsg = "有盘点单生成的费用单已经审核，操作失败。";
                            rmsr.Succeeded = false;
                            _logger.LogError(rmsr.ErrorMsg);
                            return rmsr;
                        }
                        else
                        {
                            await ctrpayable.BaseDeleteAsync(profitLoss);
                        }


                        #endregion
                    }
                    catch (Exception)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("盘点单审核时，财务数据处理失败，更新失败！");
                    }
                }

                //这部分是否能提出到上一级公共部分？
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
                rmsr.ErrorMsg = ex.Message;
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
                //  _logger.Error(approvalEntity.bizName +);
                return rmsr;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long ID)
        {
            List<tb_Stocktake> list = await _appContext.Db.CopyNew().Queryable<tb_Stocktake>().Where(m => m.MainID == ID)
                             .Includes(a => a.tb_location)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_StocktakeDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

    }

}