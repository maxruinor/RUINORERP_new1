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

using RUINORERP.Global;
using RUINORERP.Model.Base;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using System.Linq;
using RUINORERP.Business.CommService;
using RUINOR.Core;
using RUINORERP.Common.Helper;

namespace RUINORERP.Business
{
    public partial class tb_SaleOrderController<T>
    {
        //public virtual async Task<List<tb_SaleOrder>> QueryAsync(tb_SaleOrderQueryDto QueryCriteria, Pagination pagination)
        //{
        //    List<tb_SaleOrder> list = await _tb_SaleOrderServices.QueryAsync(QueryCriteria, pagination);
        //    MyCacheManager.Instance.UpdateEntityList<tb_SaleOrder>(list);
        //    return list;
        //}


        public async override Task<ReturnResults<T>> AdvancedSave(T ObjectEntity)
        {
            ReturnResults<T> result = new ReturnResults<T>();
            await Task.Delay(0); // 模拟异步操作
            return result; // 或者根据实际情况返回值
        }

        /// <summary>
        /// 库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
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
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                //更新拟销售量

                foreach (var child in entity.tb_SaleOrderDetails)
                {
                    #region 库存表的更新 ，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        //采购和销售都会提前处理。所以这里默认提供一行数据。成本和数量都可能为0
                        inv = new tb_Inventory();
                        inv.ProdDetailID = child.ProdDetailID;
                        inv.Location_ID = child.Location_ID;

                        inv.Quantity = 0;

                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "销售订单创建";//后面修改数据库是不需要？
                        //inv.LatestStorageTime = System.DateTime.Now;
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    //更新在途库存
                    inv.Sale_Qty = inv.Sale_Qty + child.Quantity;
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).ExecuteCommandAsync();
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, "事务回滚");
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
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
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
                            ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                            if (rr.Succeeded)
                            {

                            }
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
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();

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
                        for (int c = 0; c < entitys[m].tb_SaleOrderDetails.Count; c++)
                        {

                            #region 库存表的更新 ，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entitys[m].tb_SaleOrderDetails[c].ProdDetailID);
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
                            ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                            if (rr.Succeeded)
                            {

                            }
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
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();

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
                        for (int c = 0; c < entitys[m].tb_SaleOrderDetails.Count; c++)
                        {

                            #region 库存表的更新 ，
                            tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entitys[m].tb_SaleOrderDetails[c].ProdDetailID);
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
                            ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                            if (rr.Succeeded)
                            {

                            }
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                //判断是否能反审?
                if (entity.tb_SaleOuts != null
                    && (entity.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_SaleOuts.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的销售出库单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }


                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {

                    rmrs.ErrorMsg = "只能反审核已确认,并且有审核结果的订单 ";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }
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
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {

                    }
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).ExecuteCommandAsync();



                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
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
        /// 更新订单，更新出库单的付款状态， 付款状态，付款类型。付款日期？付款凭证？
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
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity).UpdateColumns(it => new { it.PayStatus, it.Paytype_ID ,it.ProjectGroup_ID}).ExecuteCommandAsync();
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

        /*
        /// <summary>
        /// 没有删除。只是保留一点可以参考的代码写法
        /// </summary>
        /// <returns></returns>
        public  List<QueryParameter<T>> GetQueryParameters()
        {
            List<QueryParameter<tb_SaleOrder>> _Paras = new List<QueryParameter<tb_SaleOrder>>();
            var lambda = Expressionable.Create<tb_CustomerVendor>()
                       .And(t => t.isdeleted == false)
                       .And(t => t.Is_available == true)
                       .And(t => t.IsCustomer == true)
                       .And(t => t.Is_enabled == true)
                       .AndIF(AuthorizeController.GetSaleLimitedAuth(_appContext), t => t.Employee_ID == _appContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户,采购不限制
                       .ToExpression();//注意 这一句 不能少
            QueryParameter<tb_SaleOrder> parameter = new QueryParameter<tb_SaleOrder>(c => c.CustomerVendor_ID);
            parameter.SetFieldLimitCondition<tb_CustomerVendor>(lambda);
            // parameter.FieldLimitConditions<tb_CustomerVendor>(lambda);

            //设置次级查询条件
            var conlist = _appContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>().GetQueryParameters();
            parameter.SubQueryParameter = new List<string>(conlist.Select(t => t.QueryField).ToList()); ;
            //可以根据关联外键自动加载条件，条件用公共虚方法

            _Paras.Add(parameter);
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.SOrderNo));
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Employee_ID));
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.ProjectGroup_ID));
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Notes));
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.PlatformOrderNo));
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.ShippingAddress));
            _Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.SaleDate));

            QueryParameter<tb_SaleOrder> paraApprovalStatus = new QueryParameter<tb_SaleOrder>(c => c.ApprovalStatus);
            paraApprovalStatus.QueryFieldType = QueryFieldType.CmbEnum;

            QueryFieldEnumData<tb_SaleOrder> queryFieldData = new QueryFieldEnumData<tb_SaleOrder>();
            queryFieldData.EnumType = typeof(ApprovalStatus);
            queryFieldData.expEnumValueColName = c => c.ApprovalStatus;
            queryFieldData.AddSelectItem = true;

            //枚举过滤了一下

            ApprovalStatus enumdata = ApprovalStatus.已审核;
            List<string> listStr = new List<string>();
            List<EnumEntityMember> list = new List<EnumEntityMember>();
            list = enumdata.GetListByEnum(1);
            queryFieldData.BindDataSource = list;

            //是直接给类型还是给数据源呢？

            //InitDataToCmbByEnumDynamicGeneratedDataSource(typeof(CheckMode), cmbCheckMode);
            //DataBindingHelper.InitDataToCmbByEnumDynamicGeneratedDataSource<tb_Stocktake>(typeof(Adjust_Type), e => e.Adjust_Type, cmb调整类型, false);
            //EnumBindingHelper.InitDataToCmbByEnumOnWhere(list, "CheckMode", cmbCheckMode);
            paraApprovalStatus.QueryFieldDataPara = queryFieldData;
            _Paras.Add(paraApprovalStatus);


            QueryParameter<tb_SaleOrder> paraPayStatus = QueryParameterTool<tb_SaleOrder>.GetFieldEnumPara(QueryFieldType.CmbEnum,
                typeof(PayStatus), c => c.PayStatus, true);
            _Paras.Add(paraPayStatus);

            QueryParameter<tb_SaleOrder> paraDataStatus = QueryParameterTool<tb_SaleOrder>.GetFieldEnumPara(QueryFieldType.CmbEnum,
                typeof(DataStatus), c => c.DataStatus, true);
            _Paras.Add(paraDataStatus);

            QueryParameter<tb_SaleOrder> paraPrintStatus = QueryParameterTool<tb_SaleOrder>.GetFieldEnumPara(QueryFieldType.CmbEnum,
                typeof(PrintStatus), c => c.PrintStatus, true);
            _Paras.Add(paraPrintStatus);


            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.SOrderNo));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.SaleDate));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.CustomerVendor_ID));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.PayStatus));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.tb_paymentmethod));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.PlatformOrderNo));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.ShippingAddress));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.TrackNo));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Paytype_ID));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.ProjectGroup_ID));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Employee_ID));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Notes));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Created_at));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.Created_by));
            //_Paras.Add(new QueryParameter<tb_SaleOrder>(c => c.DeliveryDate));
            return _Paras as List<QueryParameter<T>>;
        }

        */


    }
}
