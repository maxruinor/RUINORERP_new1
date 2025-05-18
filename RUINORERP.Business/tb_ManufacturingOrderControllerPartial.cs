
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:35
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
using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.Security;
using AutoMapper;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    /// <summary>
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960
    /// </summary>
    public partial class tb_ManufacturingOrderController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 批量结案
        /// 结案时 如果没有完全完成的。则未发量这些数量要减去。结案后不需要计算未发量了
        /// 如果是结案了。上层业务据是不是 将这级视为通过?加入自动结案？还是全部要手动结果。目前是没有加入。要手动。
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_ManufacturingOrder> entitys = new List<tb_ManufacturingOrder>();
            entitys = NeedCloseCaseList as List<tb_ManufacturingOrder>;
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

                    //更新 未发数量？
                    //如果领料明细中的出库数量小于制令单中应发数量，则未发数要减去这个差值
                    decimal 已发数 = entitys[m].tb_ManufacturingOrderDetails.Select(c => c.ActualSentQty).Sum();
                    decimal 应发数 = entitys[m].tb_ManufacturingOrderDetails.Select(c => c.ShouldSendQty).Sum();

                    tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                    List<tb_Inventory> invUpdateList = new List<tb_Inventory>();


                    for (int c = 0; c < entitys[m].tb_ManufacturingOrderDetails.Count; c++)
                    {
                        #region 库存表的更新 ，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entitys[m].tb_ManufacturingOrderDetails[c].ProdDetailID
                        && i.Location_ID == entitys[m].tb_ManufacturingOrderDetails[c].Location_ID);
                        if (inv == null)
                        {
                            //应该不会到这里面来了。
                            //View_ProdDetail view_Prod = await _unitOfWorkManage.GetDbClient().Queryable<View_ProdDetail>()
                            //    .Where(c => c.ProdDetailID == entitys[m].ProdDetailID && c.Location_ID==entitys[m].tb_ManufacturingOrderDetails[c].Location_ID).FirstAsync();
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"{entitys[m].tb_ManufacturingOrderDetails[c].ProdDetailID}库存中没有当前的产品。请使用【期初盘点】的方式进行盘点后，再操作。";
                            rs.Succeeded = false;
                            return rs;
                        }
                        //更新未发数,这种情况是少发领料，强制结案时。
                        if (已发数 < 应发数)
                        {
                            decimal diffqty = 应发数 - 已发数;
                            inv.NotOutQty = inv.NotOutQty - diffqty.ToInt();
                        }

                        //这个情况时。领料时候，已经发完了。超发也不会负数。 notoutqty=0  相对于这个品这个单而言
                        if (已发数 >= 应发数)
                        {
                            //inv.NotOutQty -= (entitys[m].tb_ManufacturingOrderDetails[c].ShouldSendQty - entitys[m].tb_ManufacturingOrderDetails[c].ActualSentQty);
                        }
                        BusinessHelper.Instance.EditEntity(inv);
                        #endregion
                        invUpdateList.Add(inv);
                    }

                    int InvUpdateCounter = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList).ExecuteCommandAsync();
                     if (InvUpdateCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("库存更新失败！");
                    }

                    //这部分是否能提出到上一级公共部分？
                    entitys[m].DataStatus = (int)DataStatus.完结;
                    if (string.IsNullOrEmpty(entitys[m].CloseCaseOpinions))
                    {
                        entitys[m].CloseCaseOpinions = "强制结案";
                    }
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //后面是不是要做一个审核历史记录表？

                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions, it.Modified_by, it.Modified_at, it.Notes }).ExecuteCommandAsync();
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



        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_ManufacturingOrder entity = ObjectEntity as tb_ManufacturingOrder;


            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                if (entity.tb_productiondemand == null)
                {
                    entity.tb_productiondemand = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                                                 .Includes(e => e.tb_ProduceGoodsRecommendDetails)
                                                .Where(c => c.PDID == entity.PDID).Single();
                }
                else
                {
                    //判断是否能反审?
                    if (entity.tb_productiondemand.tb_ProduceGoodsRecommendDetails == null)
                    {
                        entity.tb_productiondemand.tb_ProduceGoodsRecommendDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>()
                            .Where(c => c.PDID == entity.tb_productiondemand.PDID).ToList();
                    }
                }


                if (entity.tb_MaterialRequisitions != null
                    && (entity.tb_MaterialRequisitions.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_MaterialRequisitions.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的领料单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }


                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    rs.ErrorMsg = "计划单非确认或非完结，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                #region 并且更新需求分析时 自制品建议的生成单号等信息
                tb_ProduceGoodsRecommendDetail pgrd = entity.tb_productiondemand.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ProdDetailID == entity.ProdDetailID
               && c.Location_ID == entity.Location_ID
               && c.PDCID == entity.PDCID//生成时以建议成品明细中的主键PDCID来关联到制令单的一行。因为相同产品也可以多次生成制令单
               );
                if (pgrd != null)
                {
                    pgrd.RefBillNO = string.Empty;
                    pgrd.RefBillID = null;
                    pgrd.RefBillType = null;
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProduceGoodsRecommendDetail>(pgrd).ExecuteCommandAsync();
                }
                #endregion

                #region 更新在制数量，这个是针对主表目标的更新

                tb_Inventory invMain = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entity.ProdDetailID && i.Location_ID == entity.Location_ID);
                if (invMain == null)
                {
                    invMain = new tb_Inventory();
                    invMain.ProdDetailID = entity.ProdDetailID;
                    invMain.Location_ID = entity.Location_ID;
                    invMain.Quantity = 0;
                    invMain.InitInventory = (int)invMain.Quantity;
                    invMain.Notes = "";//后面修改数据库是不需要？
                                       //inv.LatestStorageTime = System.DateTime.Now;
                    BusinessHelper.Instance.InitEntity(invMain);
                }
                invMain.MakingQty -= entity.ManufacturingQty.ToInt();
                //更新未发数量
                BusinessHelper.Instance.EditEntity(invMain);


                //下面的写法可以做到批量的  插入更新。雪花ID
                //var x = _unitOfWorkManage.GetDbClient().Storageable(units).ToStorage();
                //x.AsInsertable.ExecuteReturnSnowflakeIdList();//不存在插入
                //return await x.AsUpdateable.ExecuteCommandAsync();//存在更新
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invMain);
                if (InvMainCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("在制产品库存更新失败！");
                }


                #endregion

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (tb_ManufacturingOrderDetail item in entity.tb_ManufacturingOrderDetails)
                {
                    #region 更新未发数量 是要在明细中体现的
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == item.ProdDetailID && i.Location_ID == item.Location_ID);
                    if (invMain != null)
                    {
                        inv.NotOutQty -= item.ShouldSendQty.ToInt();
                    }
                    else
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"ProdDetailID:{inv.ProdDetailID}不存在库存信息");
                    }
                    BusinessHelper.Instance.EditEntity(inv);
                    invUpdateList.Add(inv);
                    #endregion
                }
                int InvUpdateCounter = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList).ExecuteCommandAsync();
                if (InvUpdateCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新失败！");
                }
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(entity).ExecuteCommandAsync();



                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rs.Succeeded = false;
                BizTypeMapper mapper = new BizTypeMapper();
                rs.ErrorMsg = mapper.GetBizType(typeof(tb_ManufacturingOrder)).ToString() + "事务回滚=>" + ex.Message;
                rs.ErrorMsg = ex.Message;
                //  _logger.Error(approvalEntity.bizName + "事务回滚");
                return rs;
            }
        }

        /// <summary>
        /// 批量审核 未发量,在制量的更新，并且更新需求分析时 自制品建议的生成单号等信息
        /// 制作令明细中是可以存在相同产品的并且数量不同，会在领料单中合并
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="approvalEntity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_ManufacturingOrder entity = ObjectEntity as tb_ManufacturingOrder;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //更新未发料量
                #region 审核


                #region 并且更新需求分析时 自制品建议的生成单号等信息
                if (entity.tb_productiondemand == null)
                {
                    entity.tb_productiondemand = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                                                 .Includes(e => e.tb_ProduceGoodsRecommendDetails)
                                                .Where(c => c.PDID == entity.PDID).Single();
                }
                else
                {
                    //判断是否能反审?
                    if (entity.tb_productiondemand.tb_ProduceGoodsRecommendDetails == null)
                    {
                        entity.tb_productiondemand.tb_ProduceGoodsRecommendDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>()
                            .Where(c => c.PDID == entity.tb_productiondemand.PDID).ToList();
                    }
                }


                tb_ProduceGoodsRecommendDetail pgrd = entity.tb_productiondemand.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ProdDetailID == entity.ProdDetailID
                && c.Location_ID == entity.Location_ID
                && c.PDCID == entity.PDCID//生成时以建议成品明细中的主键PDCID来关联到制令单的一行。因为相同产品也可以多次生成制令单
                );
                if (pgrd != null)
                {
                    pgrd.RefBillNO = entity.MONO;
                    pgrd.RefBillID = entity.MOID;
                    pgrd.RefBillType = (int)BizType.制令单;
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProduceGoodsRecommendDetail>(pgrd).ExecuteCommandAsync();
                }
                #endregion

                #region 更新在制数量，这个是针对主表目标的更新

                tb_Inventory invMain = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == entity.ProdDetailID && i.Location_ID == entity.Location_ID);
                if (invMain == null)
                {
                    invMain = new tb_Inventory();
                    invMain.ProdDetailID = entity.ProdDetailID;
                    invMain.Location_ID = entity.Location_ID;
                    invMain.Quantity = 0;
                    invMain.InitInventory = 0;
                    invMain.Notes = "制令单审核时，自动生成库存信息";
                    BusinessHelper.Instance.InitEntity(invMain);
                }
                invMain.MakingQty += entity.ManufacturingQty.ToInt();

                #endregion

                BusinessHelper.Instance.EditEntity(invMain);
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invMain);

                if (InvMainCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("在制产品库存更新失败！");
                }

                List<tb_Inventory> invInsertList = new List<tb_Inventory>();
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (tb_ManufacturingOrderDetail item in entity.tb_ManufacturingOrderDetails)
                {
                    #region 更新未发数量 是要在明细中体现的

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == item.ProdDetailID && i.Location_ID == item.Location_ID);
                    if (inv != null)
                    {
                        inv.NotOutQty += item.ShouldSendQty.ToInt();
                        BusinessHelper.Instance.EditEntity(inv);
                        invUpdateList.Add(inv);
                    }
                    else
                    {

                        inv = new tb_Inventory();
                        inv.ProdDetailID = item.ProdDetailID;
                        inv.Location_ID = item.Location_ID;
                        inv.Quantity = 0;
                        inv.InitInventory = 0;
                        inv.Notes = "制令单审核时，自动生成库存信息";
                        BusinessHelper.Instance.InitEntity(inv);
                        invInsertList.Add(inv);
                    }

                    #endregion
                }
                var InvInsertCounter = await _unitOfWorkManage.GetDbClient().Insertable(invInsertList).ExecuteReturnSnowflakeIdListAsync();
                if (InvInsertCounter.Count != invInsertList.Count)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存保存失败！");
                }

                int InvUpdateCounter = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList).ExecuteCommandAsync();
                //因为目前库存是整数  纸箱这种用了小数所以更新失败。暂时不管理 后面统一为decimal(18, 4)
                if (InvUpdateCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新失败！");
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ManufacturingOrder>(entity).ExecuteCommandAsync();




                #endregion


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                //_logger.Info("审核事务成功");
                return rs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                rs.ErrorMsg = ex.Message;
                _logger.Error(ex, "事务回滚" + ex.Message);

                return rs;
            }
        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_ManufacturingOrder> list = await _appContext.Db.CopyNew().Queryable<tb_ManufacturingOrder>().Where(m => m.MOID == MainID)
                             .Includes(a => a.tb_customervendor)
                             .Includes(a => a.tb_customervendor_out)
                             .Includes(a => a.tb_employee)
                              .Includes(a => a.tb_department)
                              .Includes(a => a.tb_location)
                              .Includes(a => a.tb_producttype)
                              .Includes(a => a.tb_unit)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ManufacturingOrderDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                              .Includes(a => a.tb_ManufacturingOrderDetails, b => b.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .ToListAsync();
            return list as List<T>;
        }

    }
}



