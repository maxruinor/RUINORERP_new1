
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
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.Business.CommService;

namespace RUINORERP.Business
{
    public partial class tb_PurEntryController<T>
    {

        /// <summary>
        /// 返回批量审核的结果
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurEntry entity = ObjectEntity as tb_PurEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();


                //处理采购订单
                entity.tb_purorder = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                     .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                     .Includes(t => t.tb_PurOrderDetails)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                     .Single();

                if (entity.tb_purorder == null)
                {
                    rs.ErrorMsg = $"没有找到对应的采购订单!请检查数据后重试！";
                    return rs;
                }

                //如果入库明细中的产品。不存在于订单中。审核失败。
                foreach (var child in entity.tb_PurEntryDetails)
                {
                    if (!entity.tb_purorder.tb_PurOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID))
                    {
                        rs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"入库明细中有产品不属性当前订单!请检查数据后重试！";
                        return rs;
                    }
                }



                foreach (tb_PurEntryDetail child in entity.tb_PurEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    bool Opening = false;
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        Opening = true;
                        inv = new tb_Inventory();
                        inv.Quantity = inv.Quantity + child.Quantity;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {
                        inv.Quantity = inv.Quantity + child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;

                    // 直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。
                    //后进先出法（LIFO）：按照后入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较慢，成本价格波动较大的情况。
                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。

                    CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);

                    inv.Inv_Cost = child.TransactionPrice;//这里需要计算，根据系统设置中的算法计算。
                    inv.CostFIFO = child.TransactionPrice;
                    inv.CostMonthlyWA = child.TransactionPrice;
                    inv.CostMovingWA = child.TransactionPrice;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion

                    #region 更新采购价格
                    tb_PriceRecordController<tb_PriceRecord> ctrPriceRecord = _appContext.GetRequiredService<tb_PriceRecordController<tb_PriceRecord>>();
                    tb_PriceRecord priceRecord = _unitOfWorkManage.GetDbClient().Queryable<tb_PriceRecord>()
                    .Where(c => c.Employee_ID == entity.Employee_ID.Value && c.ProdDetailID == child.ProdDetailID).First();
                    //如果存在则更新，否则插入
                    if (priceRecord == null)
                    {
                        priceRecord = new tb_PriceRecord();
                    }
                    priceRecord.Employee_ID = entity.Employee_ID.Value;
                    priceRecord.PurPrice = child.TransactionPrice;
                    priceRecord.PurDate = System.DateTime.Now;
                    priceRecord.ProdDetailID = child.ProdDetailID;
                    ReturnResults<tb_PriceRecord> rrpr = await ctrPriceRecord.SaveOrUpdate(priceRecord);

                    #endregion

                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(child.ProdDetailID + "==>" + child.property + "库存更新成功");
                        }

                        if (Opening)
                        {
                            #region 处理期初  
                            //库存都没有。期初也会没有 ,并且期初只会新增，并且数据只是期初盘点时才有，不会修改。
                            tb_OpeningInventory oinv = new tb_OpeningInventory();
                            oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                            oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                            oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                            oinv.InitInvDate = entity.EntryDate;
                            oinv.RefBillID = entity.PurEntryID;
                            oinv.RefNO = entity.PurEntryNo;
                            oinv.InitQty = 0;
                            oinv.InitInvDate = System.DateTime.Now;
                            CommBillData cbd = bcf.GetBillData<tb_PurEntry>(entity);
                            //oinv.RefBizType = cbd.BizType;
                            //TODO 还要完善引用数据
                            await ctrOPinv.AddReEntityAsync(oinv);
                            #endregion
                        }
                    }
                }

                //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
                foreach (var item in entity.tb_purorder.tb_PurEntries)
                {
                    detailList.AddRange(item.tb_PurEntryDetails);
                }

                //分两种情况处理。
                for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
                {
                    //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较


                    string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                    //明细中有相同的产品或物品。
                    var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                    {
                        #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                        if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                        {
                            //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        #endregion

                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                        {
                            string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量\r\n" + $"                                    或存在针对当前采购订单重复录入了采购入库单，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID).Sum(c => c.Quantity);
                            //算出交付的数量
                            entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，入库总数量不能大于订单数量！");
                            }
                        }
                    }
                    else
                    {
                        //一对一时
                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                        {

                            string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量\r\n" + $"                                    或存在针对当前采购订单重复录入了采购入库单，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付
                            var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID).Sum(c => c.Quantity);
                            entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，入库总数量不能大于订单数量！");
                            }
                        }
                    }
                }

                //更新已交数量
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Info(entity.PurEntryNo + "==>" + entity.PurOrder_NO + $"对应 的订单更新成功===重点代码 看已交数量是否正确");
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
                int counter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(entity).ExecuteCommandAsync();
                if (counter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Info(entity.PurEntryNo + "==>" + "状态更新成功");
                    }
                }

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量 TODO 销售也会有这种情况
                if (entity.tb_purorder != null && (entity.TotalQty == entity.tb_purorder.TotalQty || entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity) == entity.tb_purorder.TotalQty))
                {
                    entity.tb_purorder.DataStatus = (int)DataStatus.完结;
                    entity.tb_purorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核入库单:" + entity.PurEntryNo + "结案。"; ;
                    int poendcounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder)
                        .UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    if (poendcounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(entity.tb_purorder.PurOrderNo + "==>" + "结案状态更新成功");
                        }
                    }
                }


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();

                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);

                if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                {
                    _logger.Error("事务回滚" + ex.Message);
                }
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
            tb_PurEntry entity = ObjectEntity as tb_PurEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {
                //判断是否能反审?
                if (entity.tb_PurEntryRes != null
                    && (entity.tb_PurEntryRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntryRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的采购入库退回单，不能反审核  ";
                    rs.Succeeded = false;
                }


                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                foreach (var child in entity.tb_PurEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要
                    bool Opening = false;
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        Opening = true;
                        inv = new tb_Inventory();
                        inv.Quantity = inv.Quantity - child.Quantity;
                        inv.InitInventory = (int)inv.Quantity;
                        inv.Notes = "";//后面修改数据库是不需要？
                        BusinessHelper.Instance.InitEntity(inv);
                    }
                    else
                    {
                        inv.Quantity = inv.Quantity - child.Quantity;
                        BusinessHelper.Instance.EditEntity(inv);
                    }
                    inv.ProdDetailID = child.ProdDetailID;
                    inv.Location_ID = child.Location_ID;
                    inv.Notes = "";//后面修改数据库是不需要？
                    inv.LatestStorageTime = System.DateTime.Now;

                    //采购订单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;
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
                    CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);

                    inv.Inv_Cost = child.TransactionPrice;//这里需要计算，根据系统设置中的算法计算。
                    inv.CostFIFO = child.TransactionPrice;
                    inv.CostMonthlyWA = child.TransactionPrice;
                    inv.CostMovingWA = child.TransactionPrice;
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;

                    #endregion
                    ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    if (rr.Succeeded)
                    {
                        if (Opening)
                        {
                            #region 处理期初
                            //库存都没有。期初也会没有 ,并且期初只会新增，不会修改。
                            tb_OpeningInventory oinv = new tb_OpeningInventory();
                            oinv.Inventory_ID = rr.ReturnObject.Inventory_ID;
                            oinv.Cost_price = rr.ReturnObject.Inv_Cost;
                            oinv.Subtotal_Cost_Price = oinv.Cost_price * oinv.InitQty;
                            oinv.InitInvDate = entity.EntryDate;
                            oinv.RefBillID = entity.PurEntryID;
                            oinv.RefNO = entity.PurEntryNo;
                            oinv.InitQty = 0;
                            oinv.InitInvDate = System.DateTime.Now;
                            CommBillData cbd = bcf.GetBillData<tb_PurEntry>(entity);
                            //oinv.RefBizType = cbd.BizType;
                            //TODO 还要完善引用数据
                            await ctrOPinv.AddReEntityAsync(oinv);
                            #endregion
                        }
                    }
                }

                if (entity.tb_purorder != null)
                {
                    #region  反审检测写回 退回

                    //处理采购订单
                    entity.tb_purorder = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                         .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                         .Includes(t => t.tb_PurOrderDetails)
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                         .Single();


                    //先找到所有入库明细,再找按订单明细去循环比较。如果入库总数量大于订单数量，则不允许入库。
                    List<tb_PurEntryDetail> detailList = new List<tb_PurEntryDetail>();
                    foreach (var item in entity.tb_purorder.tb_PurEntries)
                    {
                        detailList.AddRange(item.tb_PurEntryDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_purorder.tb_PurOrderDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较


                        string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                        {
                            #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！"); throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，入库总数量不能大于订单数量！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {

                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID).Sum(c => c.Quantity);
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    #endregion

                    //更新已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                    if (updatecounter == 0)
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
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntry>(entity).ExecuteCommandAsync();

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量
                if (entity.tb_purorder != null && entity.tb_purorder.TotalQty != entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_purorder.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }


                _unitOfWorkManage.CommitTran();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                rs.ErrorMsg = ex.Message;
                return rs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_PurEntry> list = await _appContext.Db.CopyNew().Queryable<tb_PurEntry>().Where(m => m.PurEntryID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_PurEntryDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_PurEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }





    }
}



