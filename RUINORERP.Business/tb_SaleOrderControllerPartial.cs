﻿using System;
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

using RUINORERP.Global;
using RUINORERP.Model.Base;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using System.Linq;
using RUINORERP.Business.CommService;
using RUINOR.Core;
using RUINORERP.Common.Helper;
using System.Security.Policy;
using AutoMapper;
using System.Windows.Forms;
using RUINORERP.Global.EnumExt;
using Fireasy.Common.Extensions;
using System.Collections;

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
        /// 库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// 财务业务模板：如果账期，则是在销售出库时生成应收。
        /// 销售订单审核时
        /// 部分付款叫订金。 有订金才生成预收款，意思是有金额交易才生成
        /// 全部付款生成应收
        /// 账期就要等出库审核时生成应收款。
        /// 
        /// 销售订金（预收）	- 预收定金生成预收单
        //- 后续订单核销预收 → 自动冲抵应收	- 预收付表：减少 RemainAmount
        //- 应收应付表：减少 TotalAmount

        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_SaleOrder entity = ObjectEntity as tb_SaleOrder;
            try
            {

                // 开启事务，保证数据一致性
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                List<tb_Inventory> invList = new List<tb_Inventory>();

                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                //var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal SaleQtySum, decimal QtySum)>();
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal SaleQtySum)>();

                //更新拟销售量
                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentSaleQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
                    //decimal currentQty = child.Quantity; // 假设 Qty 与 Sale_Qty 相同，可根据实际业务调整
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
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充
                                Notes = "销售订单创建",
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
                            SaleQtySum: currentSaleQty // 首次累加
                                                       //QtySum: currentQty
                        );
                        inventoryGroups[key] = group;
                        #endregion
                    }
                    else
                    {
                        // 累加已有分组的数值字段
                        group.SaleQtySum += currentSaleQty;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                // 处理分组数据，更新库存记录的各字段
                //循环inventoryGroups
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;
                    // 累加数值字段
                    inv.Sale_Qty += group.Value.SaleQtySum.ToInt();
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


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region 生成预收款单 

                    #region 生成预收款单条件判断检测
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


                    #endregion


                    // 外币相关处理 正确是 外币时一定要有汇率
                    decimal exchangeRate = 1; // 获取销售订单的汇率
                    if (_appContext.BaseCurrency.Currency_ID != entity.Currency_ID)
                    {
                        exchangeRate = entity.ExchangeRate; // 获取销售订单的汇率
                                                            // 这里可以考虑获取最新的汇率，而不是直接使用销售订单的汇率
                                                            // exchangeRate = GetLatestExchangeRate(entity.Currency_ID.Value, _appContext.BaseCurrency.Currency_ID);
                    }

                    //销售订单审核时，非账期，即时收款时
                    //订金，部分收款 生成预收款。
                    //全款生成应收 转收款单
                    var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID
                        && entity.PayStatus == (int)PayStatus.部分付款
                        )
                    {
                        var PreReceivedPayment = ctrpay.CreatePreReceivedPayment(entity, true);
                    }

                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result =await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).UpdateColumns(it => new
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
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

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
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                            if (inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = child.ProdDetailID;
                                inv.Location_ID = child.Location_ID;

                                inv.Quantity = 0;


                                inv.InitInventory = (int)inv.Quantity;
                                inv.Notes = "";//后面修改数据库是不需要？
                                               //inv.LatestStorageTime = System.DateTime.Now;
                                BusinessHelper.Instance.InitEntity(inv);
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
                            _logger.LogInformation($"{entity.SOrderNo}批量审核时，更新库存结果为0行，请检查数据！");
                        }


                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.确认;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).ExecuteCommandAsync();

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
                    if (entitys[m].tb_SaleOrderDetails.Select(c => c.TotalDeliveredQty).Sum() < entitys[m].tb_SaleOrderDetails.Select(c => c.Quantity).Sum())
                    {
                        tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                        List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                        for (int c = 0; c < entitys[m].tb_SaleOrderDetails.Count; c++)
                        {

                            #region 库存表的更新 ，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entitys[m].tb_SaleOrderDetails[c].ProdDetailID
                            && i.Location_ID == entitys[m].tb_SaleOrderDetails[c].Location_ID
                            );
                            if (inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = entitys[m].tb_SaleOrderDetails[c].ProdDetailID;
                                inv.Location_ID = entitys[m].tb_SaleOrderDetails[c].Location_ID;
                                inv.Quantity = 0;
                                inv.InitInventory = (int)inv.Quantity;
                                inv.Notes = "";//后面修改数据库是不需要？
                                               //inv.LatestStorageTime = System.DateTime.Now;
                                BusinessHelper.Instance.InitEntity(inv);
                            }
                            //更新在途库存 ,如果订单明细中，在途=在途-(订单数量-已出库数)
                            inv.Sale_Qty -= (entitys[m].tb_SaleOrderDetails[c].Quantity - entitys[m].tb_SaleOrderDetails[c].TotalDeliveredQty);
                            BusinessHelper.Instance.EditEntity(inv);
                            #endregion
                            invUpdateList.Add(inv);
                        }

                        DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                        var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (InvUpdateCounter == 0)
                        {
                            _logger.LogInformation($"{entitys[m].SOrderNo},批量关闭时，更新库存结果为0行，请检查数据！");
                        }


                    }
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].DataStatus = (int)DataStatus.完结;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //后面是不是要做一个审核历史记录表？

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
                    if (entitys[m].tb_SaleOrderDetails.Select(c => c.TotalDeliveredQty).Sum() < entitys[m].tb_SaleOrderDetails.Select(c => c.Quantity).Sum())
                    {
                        tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                        List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                        for (int c = 0; c < entitys[m].tb_SaleOrderDetails.Count; c++)
                        {

                            #region 库存表的更新 ，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entitys[m].tb_SaleOrderDetails[c].ProdDetailID
                            && i.Location_ID == entitys[m].tb_SaleOrderDetails[c].Location_ID
                            );
                            if (inv == null)
                            {
                                inv = new tb_Inventory();
                                inv.ProdDetailID = entitys[m].tb_SaleOrderDetails[c].ProdDetailID;
                                inv.Location_ID = entitys[m].tb_SaleOrderDetails[c].Location_ID;
                                inv.Quantity = 0;
                                inv.InitInventory = (int)inv.Quantity;
                                inv.Notes = "";//后面修改数据库是不需要？
                                               //inv.LatestStorageTime = System.DateTime.Now;
                                BusinessHelper.Instance.InitEntity(inv);
                            }
                            //更新在途库存 ,如果订单明细中，在途=在途+(订单数量-已出库数)
                            inv.Sale_Qty += (entitys[m].tb_SaleOrderDetails[c].Quantity - entitys[m].tb_SaleOrderDetails[c].TotalDeliveredQty);
                            BusinessHelper.Instance.EditEntity(inv);
                            #endregion
                            invUpdateList.Add(inv);
                        }
                        DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                        var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                        if (InvUpdateCounter == 0)
                        {
                            _logger.LogInformation($"{entitys[m].SOrderNo}反审核，更新库存结果为0行，请检查数据！");
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


                //判断是否能反审?
                if (entity.tb_SaleOuts != null
                    && (entity.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结)
                    && entity.tb_SaleOuts.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的销售出库单，不能反审核,请退回处理。";
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal SaleQtySum)>();


                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentSaleQty = child.Quantity; // 假设 Sale_Qty 对应明细中的 Quantity
                                                             // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
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
                    inv.Sale_Qty -= group.Value.SaleQtySum.ToInt();
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (InvUpdateCounter == 0)
                {
                    _logger.LogInformation($"{entity.SOrderNo}反审核，更新库存结果为0行，请检查数据！");
                }



                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region  预收款单处理

                    tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment> ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    var pay = await ctrpay.IsExistEntityAsync(p => p.SourceBillId == entity.SOrder_ID);
                    if (pay != null)
                    {
                        if (pay.PrePaymentStatus == (long)PrePaymentStatus.草稿 || pay.PrePaymentStatus == (long)PrePaymentStatus.待审核)
                        {
                            await ctrpay.DeleteAsync(pay);
                        }
                        else
                        {
                            //订单反审核  只是用来修改，还是真实取消订单。取消的话。则要退款。修改的话。则不需要退款。
                            //如果没有出库，则生成红冲单  ，已冲销  已取消，先用取消标记
                            //如果是要退款，则在预收款查询这，生成退款单。

                            //rmrs.ErrorMsg = $"销售订单{pay.SourceBillNo}已经生成预收款单{pay.PreRPNO}，已经确认收款，请不能反审核。";
                            //_unitOfWorkManage.RollbackTran();
                            //rmrs.Succeeded = false;
                            //return rmrs;
                        }

                    }

                    #endregion
                }
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalOpinions+="【被反审】";
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
                    BizTypeMapper mapper = new BizTypeMapper();
                    rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=> 保存出错";
                    rmrs.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                BizTypeMapper mapper = new BizTypeMapper();
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
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
                BizTypeMapper mapper = new BizTypeMapper();
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
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
                BizTypeMapper mapper = new BizTypeMapper();
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
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


        /// <summary>
        /// 转换为销售出库单
        /// </summary>
        /// <param name="saleorder"></param>
        public tb_SaleOut SaleOrderToSaleOut(tb_SaleOrder saleorder)
        {
            tb_SaleOut entity = new tb_SaleOut();
            //转单
            if (saleorder != null)
            {
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
                List<string> tipsMsg = new List<string>();
                List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
                List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();

                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].SaleOrderDetail_ID > 0)
                    {
                        #region 产品ID可能大于1行，共用料号情况
                        tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                        details[i].Cost = item.Cost;
                        details[i].CustomizedCost = item.CustomizedCost;
                        //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                        if (details[i].Cost == 0)
                        {
                            View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                            tipsMsg.Add($"销售订单{saleorder.SOrderNo}，{item.tb_proddetail.tb_prod.CNName + item.tb_proddetail.tb_prod.Specifications}已出库数为{item.TotalDeliveredQty}，可出库数为{details[i].Quantity}，当前行数据忽略！");
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
                            View_ProdDetail obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                            tipsMsg.Add($"当前订单的SKU:{item.tb_proddetail.SKU}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                        }
                        #endregion
                    }

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{entity.SaleOrderNo}已全部出库，请检查是否正在重复出库！");
                }

                entity.tb_SaleOutDetails = NewDetails;


                //如果这个订单已经有出库单 则第二次运费为0
                if (saleorder.tb_SaleOuts != null && saleorder.tb_SaleOuts.Count > 0)
                {
                    if (saleorder.FreightIncome > 0)
                    {
                        tipsMsg.Add($"当前订单已经有出库记录，运费收入已经计入前面出库单，当前出库运费收入为零！");
                        entity.FreightIncome = 0;
                    }
                    else
                    {
                        tipsMsg.Add($"当前订单已经有出库记录！");
                    }
                }


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
                entity.SaleOutNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.销售出库单);
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
                entity.TotalQty = NewDetails.Sum(c => c.Quantity);
                entity.TotalCost = NewDetails.Sum(c => c.Cost * c.Quantity);
                entity.TotalCost = entity.TotalCost + entity.FreightCost;

                entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();

                entity.TotalTaxAmount = entity.TotalTaxAmount.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());

                entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.FreightIncome;


                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }

        //应该就是反审 直接删除。或逻辑删除。不用再做一个重复的方法来实现。
        public async Task<ReturnResults<tb_SaleOrder>> CancelOrder(tb_SaleOrder ObjectEntity)
        {
            ReturnResults<tb_SaleOrder> rmrs = new ReturnResults<tb_SaleOrder>();
            tb_SaleOrder entity = ObjectEntity as tb_SaleOrder;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                //判断是否能反审?
                if (entity.tb_SaleOuts != null
                    && (entity.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_SaleOuts.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
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
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        inv = new tb_Inventory();
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;
                        inv.Quantity = 0;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                        //inv.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.InitEntity(inv);
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
                    _logger.LogInformation($"{entity.SOrderNo}取消时，更新库存结果为0行，请检查数据！");
                }


                #region  预收款单处理

                tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment> ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                var pay = await ctrpay.IsExistEntityAsync(p => p.SourceBillId == entity.SOrder_ID && p.PrePaymentStatus == (long)PrePaymentStatus.待核销);
                if (pay != null)
                {
                    if (pay.PrePaymentStatus == (long)PrePaymentStatus.待核销)
                    {
                        //预收款未核销：全额退款，生成退款单。  让财务退款
                        if (pay.ForeignBalanceAmount > 0 || pay.LocalBalanceAmount > 0)
                        {
                            tb_FM_PaymentRecordController<tb_FM_PaymentRecord> paymentController = _appContext.GetRequiredService<tb_FM_PaymentRecordController<tb_FM_PaymentRecord>>();
                            bool isRefund = true;
                            tb_FM_PaymentRecord paymentRecord = await paymentController.CreatePaymentRecord(pay, isRefund);
                        }
                        //推送到财务 ，告诉要退款 TODO

                    }
                    else if (pay.PrePaymentStatus == (long)PrePaymentStatus.部分核销)
                    {
                        //预收款已核销：冲销原核销记录，释放应收单金额，再退款
                        rmrs.ErrorMsg = $"部分核销的预收款单不能直接取消订单。";
                        _unitOfWorkManage.RollbackTran();
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else if (pay.PrePaymentStatus == (long)PrePaymentStatus.全额核销)
                    {
                        //预收款已核销：冲销原核销记录，释放应收单金额，再退款

                        rmrs.ErrorMsg = $"全额核销的预收款单不能直接取消订单。";
                        _unitOfWorkManage.RollbackTran();
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else if (pay.PrePaymentStatus == (long)PrePaymentStatus.已冲销)
                    {
                        //预收款已核销：冲销原核销记录，释放应收单金额，再退款
                        rmrs.ErrorMsg = $"已冲销的预收款单不能直接取消订单。";
                        _unitOfWorkManage.RollbackTran();
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else
                    {
                        await ctrpay.DeleteAsync(pay);
                    }
                }

                #endregion
                entity.DataStatus = (int)DataStatus.已取消;
                BusinessHelper.Instance.EditEntity(entity);

                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).UpdateColumns(it => new { it.DataStatus }).ExecuteCommandAsync();

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
                    BizTypeMapper mapper = new BizTypeMapper();
                    rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=> 订单取消失败";
                    rmrs.Succeeded = false;
                }
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                BizTypeMapper mapper = new BizTypeMapper();
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_SaleOrder)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
            }
            return rmrs;
        }





    }
}
