
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
using AutoMapper;
using RUINORERP.Global.EnumExt;
using SharpYaml.Tokens;

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
                if (entity.tb_PurEntryDetails == null)
                {
                    //处理采购订单
                    entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                         .Includes(b => b.tb_PurEntryDetails)
                         .Where(c => c.PurEntryID == entity.PurEntryID)
                         .SingleAsync();
                }

                //采购入库总数量和明细求和检查
                if (entity.TotalQty.Equals(entity.tb_PurEntryDetails.Sum(c => c.Quantity)) == false)
                {
                    rs.ErrorMsg = $"采入入库数量与明细之和不相等!请检查数据后重试！";
                    rs.Succeeded = false;
                    return rs;
                }

                var ctrtb_BOM_SDetail = _appContext.GetRequiredService<tb_BOM_SDetailController<tb_BOM_SDetail>>();
                var ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

                if (entity.PurOrder_ID.HasValue && entity.PurOrder_ID.Value > 0)
                {
                    //处理采购订单
                    entity.tb_purorder = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                         .Includes(a => a.tb_PurEntries, b => b.tb_PurEntryDetails)
                         .Includes(t => t.tb_PurOrderDetails)
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_PurOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurOrder_ID == entity.PurOrder_ID)
                         .SingleAsync();

                    if (entity.tb_purorder == null)
                    {
                        rs.ErrorMsg = $"没有找到对应的采购订单!请检查数据后重试！";
                        rs.Succeeded = false;
                        return rs;
                    }
                    else
                    {
                        //如果采购订单的供应商和这里入库的供应商不相同，要提示
                        if (entity.CustomerVendor_ID != entity.tb_purorder.CustomerVendor_ID)
                        {
                            rs.Succeeded = false;
                            rs.ErrorMsg = $"入库供应商和采购订单供应商不同!请检查数据后重试！";
                            return rs;
                        }



                        // 检查采购订单状态是否为已确认且审核通过
                        bool isOrderConfirmed = entity.tb_purorder.DataStatus == (int)DataStatus.确认;
                        bool isApproved = entity.tb_purorder.ApprovalResults.HasValue &&
                                          entity.tb_purorder.ApprovalResults.Value;

                        if (!isOrderConfirmed || !isApproved)
                        {
                            rs.Succeeded = false;
                            rs.ErrorMsg = $"{entity.tb_purorder.PurOrderNo} 请确认采购订单状态为【确认】已审核，并且审核结果为已通过!请检查数据后重试！";
                            return rs;
                        }



                    }

                    //如果入库明细中的产品。不存在于订单中。审核失败。
                    foreach (var child in entity.tb_PurEntryDetails)
                    {
                        if (!entity.tb_purorder.tb_PurOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                        {
                            rs.Succeeded = false;

                            rs.ErrorMsg = $"入库明细中有产品不属于当前订单!请检查数据后重试！";
                            return rs;
                        }
                    }


                    #region  
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
                        string prodName = entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.SKU + "-" + entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purorder.tb_PurOrderDetails[i].tb_proddetail.tb_prod.Specifications;

                        //明细中有相同的产品或物品。
                        var aa = entity.tb_purorder.tb_PurOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID > 0)
                        {
                            #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_PurEntryDetails.Any(c => c.PurOrder_ChildID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加。";
                                rs.ErrorMsg = msg;
                                return rs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            //行数一致时，判断入库的数量是否大于订单数量。（费赠品）
                            && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID).Where(c => c.IsGift.HasValue && !c.IsGift.Value).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的{entity}【{prodName}】的入库数量不能大于订单中对应行数量\r\n" + $"或当前采购订单重复录入采购入库单。";
                                rs.ErrorMsg = msg;
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                                entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty -= RowQty;
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
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Where(c => c.IsGift.HasValue && !c.IsGift.Value).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {

                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行数量\r\n" + $"或当前采购订单重复录入了采购入库单。";
                                rs.ErrorMsg = msg;
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Sum(c => c.Quantity);
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity += RowQty;
                                entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty -= RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                                {
                                    throw new Exception($"入库单：{entity.PurEntryNo}审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，入库总数量不能大于订单数量！");
                                }
                            }
                        }
                    }


                    #endregion

                }



                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal PurQtySum, bool? IsGift,
                    decimal UntaxedUnitPrice, DateTime LatestStorageTime)>();

                // 遍历销售订单明细，聚合数据
                foreach (var child in entity.tb_PurEntryDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentEntryQty = child.Quantity;
                    DateTime currentStorageTime = DateTime.Now;

                    // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，

                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv == null)
                        {
                            inv = new tb_Inventory
                            {
                                ProdDetailID = key.ProdDetailID,
                                Location_ID = key.Location_ID,
                                Quantity = 0, // 初始数量
                                Inv_Cost = 0, // 假设成本价需从其他地方获取，需根据业务补充
                                Notes = "采购入库创建",
                                InitInventory = (int)inv.Quantity,
                                Sale_Qty = 0,
                                LatestStorageTime = DateTime.Now // 初始时间
                            };

                            BusinessHelper.Instance.InitEntity(inv); // 初始化公共字段
                        }
                        else
                        {
                            BusinessHelper.Instance.EditEntity(inv);
                        }

                        #endregion


                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            PurQtySum: currentEntryQty, // 首次累加
                            IsGift: child.IsGift,
                            UntaxedUnitPrice: child.UntaxedUnitPrice,
                            LatestStorageTime: currentStorageTime
                        );
                        inventoryGroups[key] = group;
                    }
                    else
                    {
                        // 累加已有分组的数值字段

                        group.IsGift = child.IsGift;
                        if (group.IsGift.HasValue && !group.IsGift.Value && group.UntaxedUnitPrice > 0)
                        {
                            group.UntaxedUnitPrice = ((currentEntryQty * child.UntaxedUnitPrice) + (group.UntaxedUnitPrice * group.PurQtySum)) / (group.PurQtySum + currentEntryQty);
                        }
                        else
                        {
                            group.UntaxedUnitPrice = child.UntaxedUnitPrice;
                        }
                        group.PurQtySum += currentEntryQty;

                        // 取最新出库时间（若当前时间更新，则覆盖）
                        group.LatestStorageTime = System.DateTime.Now;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                List<tb_BOM_SDetail> BOM_SDetails = new List<tb_BOM_SDetail>();
                List<tb_BOM_S> BOMs = new List<tb_BOM_S>();
                List<tb_PriceRecord> PriceRecords = new List<tb_PriceRecord>();

                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量 TODO 销售也会有这种情况
                if (entity.tb_purorder != null)
                {

                    entity.tb_purorder.TotalUndeliveredQty = entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.UndeliveredQty);
                    var OrderTotadeliveredQty = entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity);

                    if (entity.tb_purorder.DataStatus == (int)DataStatus.确认
                        && (entity.TotalQty == entity.tb_purorder.TotalQty || OrderTotadeliveredQty == entity.tb_purorder.TotalQty))
                    {
                        entity.tb_purorder.DataStatus = (int)DataStatus.完结;
                        entity.tb_purorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核入库单:" + entity.PurEntryNo + "结案。"; ;
                    }
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //更新未交数量
                int OrderCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder)
                    .UpdateColumns(it => new { it.TotalUndeliveredQty, it.DataStatus, it.CloseCaseOpinions, }).ExecuteCommandAsync();
                if (OrderCounter > 0)
                {

                }

                //更新已交数量
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder.tb_PurOrderDetails)
                    .UpdateColumns(it => new { it.DeliveredQuantity, it.UndeliveredQty }).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Debug(entity.PurEntryNo + "==>" + entity.PurOrder_NO + $"对应 的订单更新成功===重点代码 看已交数量是否正确");
                    }
                }



                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;

                    #region 计算成本
                    //直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。

                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。
                    if (group.Value.IsGift.HasValue && !group.Value.IsGift.Value && group.Value.UntaxedUnitPrice > 0)
                    {
                        //不含税的总金额+不含税运费
                        decimal UntaxedShippingCost = 0;
                        UntaxedShippingCost = entity.ShipCost;
                        if (entity.ShipCost > 0 && entity.TotalTaxAmount > 0)
                        {
                            decimal FreightTaxRate = entity.tb_PurEntryDetails.FirstOrDefault(c => c.TaxRate > 0).TaxRate;
                            UntaxedShippingCost = (entity.ShipCost / (1 + FreightTaxRate)); //计算列：不含税运费
                            UntaxedShippingCost = Math.Round(UntaxedShippingCost, 2);
                        }
                        CommService.CostCalculations.CostCalculation(_appContext, inv, group.Value.PurQtySum.ToInt(), group.Value.UntaxedUnitPrice, UntaxedShippingCost);

                        var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                        // 递归更新所有上级BOM的成本
                        await ctrbom.UpdateParentBOMsAsync(group.Key.ProdDetailID, inv.Inv_Cost);



                    }

                    #endregion

                    #region 更新采购价格记录表

                    //注意这里的人是指采购订单录入的人。不是采购入库的人。
                    tb_PriceRecordController<tb_PriceRecord> ctrPriceRecord = _appContext.GetRequiredService<tb_PriceRecordController<tb_PriceRecord>>();
                    tb_PriceRecord priceRecord = await _unitOfWorkManage.GetDbClient().Queryable<tb_PriceRecord>()
                    .Where(c => c.Employee_ID == entity.tb_purorder.Employee_ID && c.ProdDetailID == group.Key.ProdDetailID).FirstAsync();
                    //如果存在则更新，否则插入
                    if (priceRecord == null)
                    {
                        priceRecord = new tb_PriceRecord();
                        priceRecord.ProdDetailID = group.Key.ProdDetailID;
                    }
                    priceRecord.Employee_ID = entity.tb_purorder.Employee_ID;
                    if (group.Value.UntaxedUnitPrice != priceRecord.PurPrice)
                    {
                        priceRecord.PurPrice = group.Value.UntaxedUnitPrice;
                        priceRecord.PurDate = System.DateTime.Now;
                        PriceRecords.Add(priceRecord);
                    }


                    #endregion

                    // 累加数值字段
                    inv.On_the_way_Qty -= group.Value.PurQtySum.ToInt();
                    inv.Quantity += group.Value.PurQtySum.ToInt();
                    inv.LatestStorageTime = System.DateTime.Now;
                    // 计算衍生字段（如总成本）
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }

                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                if (invUpdateList.Count > 0 && Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("入库时，库存更新数据为0，更新失败！");
                }

                if (BOM_SDetails.Any())
                {
                    DbHelper<tb_BOM_SDetail> BOM_SDetaildbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_SDetail>>();
                    var BOM_SDetailCounter = await BOM_SDetaildbHelper.BaseDefaultAddElseUpdateAsync(BOM_SDetails);
                    if (BOM_SDetailCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，配方明细成本更新数据为0，更新失败！");
                    }
                }

                if (BOMs.Count > 0)
                {
                    DbHelper<tb_BOM_S> BOM_SdbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_S>>();
                    var BOMCounter = await BOM_SdbHelper.BaseDefaultAddElseUpdateAsync(BOMs);
                    if (BOMCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，配方主表成本更新数据为0，更新失败！");
                    }
                }

                if (PriceRecords.Count > 0)
                { 
                     
                    // 更新价格记录表  是不是 批量更新  或 批量插入？
                    DbHelper<tb_PriceRecord> PriceRecorddbHelper = _appContext.GetRequiredService<DbHelper<tb_PriceRecord>>();
                    var PriceRecordCounter = await PriceRecorddbHelper.BaseDefaultAddElseUpdateAsync(PriceRecords);
                    if (PriceRecordCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，采购价格历史记录更新数据为0，更新失败！");
                    }
                }



                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                int counter = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(
                    it => new
                    {
                        it.DataStatus,
                        it.ApprovalStatus,
                        it.ApprovalResults,
                        it.ApprovalOpinions
                    }
                    ).ExecuteCommandAsync();
                if (counter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        // _logger.Info(entity.PurEntryNo + "==>" + "状态更新成功");
                    }
                }


                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    try
                    {
                        #region 生成应付 ,应付从预付中抵扣 同时 核销
                        var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();
                        //出库时，全部生成应付，账期的。就加上到期日
                        //有付款过的。就去预收中抵扣，不够的金额及状态标识出来，生成应付后再用于生成对账单
                        tb_FM_ReceivablePayable Payable = await ctrpayable.BuildReceivablePayable(entity, false);
                        ReturnMainSubResults<tb_FM_ReceivablePayable> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ReceivablePayable>(Payable, false);
                        if (rmr.Succeeded)
                        {
                            //已经是等审核。 审核时会核销预收付款
                            rs.ReturnObjectAsOtherEntity = rmr.ReturnObject;
                        }
                        #endregion
                    }
                    catch (Exception)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，财务数据处理失败，更新失败！");
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
                _logger.Error(ex, $"事务回滚{entity.PurEntryNo}" + ex.Message);
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
                //判断是否能反审? 意思是。我这个入库单错了。但是你都当入库成功进行了后面的操作了，现在要反审，那肯定不行。所以，要判断，
                if (entity.tb_PurEntryRes != null
                    && (entity.tb_PurEntryRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_PurEntryRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rs.ErrorMsg = "存在已确认或已完结，或已审核的【采购入库退回单】，不能反审核  ";
                    return rs;
                }



                var ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                var bcf = _appContext.GetRequiredService<BillConverterFactory>();

                // 使用字典按 (ProdDetailID, LocationID) 分组，存储库存记录及累计数据
                var inventoryGroups = new Dictionary<(long ProdDetailID, long LocationID), (tb_Inventory Inventory, decimal PurQtySum, bool? IsGift,
                    decimal UntaxedUnitPrice, DateTime LatestStorageTime)>();

                // 遍历销售订单明细，聚合数据
                foreach (var child in entity.tb_PurEntryDetails)
                {
                    var key = (child.ProdDetailID, child.Location_ID);
                    decimal currentEntryQty = child.Quantity;
                    DateTime currentStorageTime = DateTime.Now;

                    // 若字典中不存在该产品，初始化记录
                    if (!inventoryGroups.TryGetValue(key, out var group))
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        //实际 期初已经有数据了，则要
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        BusinessHelper.Instance.EditEntity(inv);
                        #endregion

                        // 初始化分组数据
                        group = (
                            Inventory: inv,
                            PurQtySum: currentEntryQty, // 首次累加
                            IsGift: child.IsGift,
                            UntaxedUnitPrice: child.UntaxedUnitPrice,
                            LatestStorageTime: currentStorageTime
                        );
                        inventoryGroups[key] = group;
                    }
                    else
                    {
                        // 累加已有分组的数值字段

                        group.IsGift = child.IsGift;
                        if (group.IsGift.HasValue && !group.IsGift.Value && group.UntaxedUnitPrice > 0)
                        {
                            group.UntaxedUnitPrice = ((currentEntryQty * child.UntaxedUnitPrice) + (group.UntaxedUnitPrice * group.PurQtySum)) / (group.PurQtySum + currentEntryQty);
                        }
                        else
                        {
                            group.UntaxedUnitPrice = child.UntaxedUnitPrice;
                        }
                        group.PurQtySum += currentEntryQty;

                        // 取最新出库时间（若当前时间更新，则覆盖）
                        group.LatestStorageTime = System.DateTime.Now;
                        inventoryGroups[key] = group; // 更新分组数据
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                List<tb_BOM_SDetail> BOM_SDetails = new List<tb_BOM_SDetail>();
                List<tb_BOM_S> BOMs = new List<tb_BOM_S>();
                List<tb_PriceRecord> PriceRecords = new List<tb_PriceRecord>();

                // 处理分组数据，更新库存记录的各字段
                foreach (var group in inventoryGroups)
                {
                    var inv = group.Value.Inventory;

                    #region 计算成本
                    //直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。

                    //数据来源可以是多种多样的，例如：
                    //采购价格：从供应商处购买产品或物品时的价格。
                    //生产成本：自行生产产品时的成本，包括原材料、人工和间接费用等。
                    //市场价格：参考市场上类似产品或物品的价格。
                    if (group.Value.IsGift.HasValue && !group.Value.IsGift.Value && group.Value.UntaxedUnitPrice > 0)
                    {
                        CommService.CostCalculations.AntiCostCalculation(_appContext, inv, group.Value.PurQtySum.ToInt(), group.Value.UntaxedUnitPrice);

                        var ctrbom = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                        // 递归更新所有上级BOM的成本
                        await ctrbom.UpdateParentBOMsAsync(group.Key.ProdDetailID, inv.Inv_Cost);
                      
                    }

                    #endregion

                    #region 更新采购价格

                    //注意这里的人是指采购订单录入的人。不是采购入库的人。
                    tb_PriceRecordController<tb_PriceRecord> ctrPriceRecord = _appContext.GetRequiredService<tb_PriceRecordController<tb_PriceRecord>>();
                    tb_PriceRecord priceRecord = await _unitOfWorkManage.GetDbClient().Queryable<tb_PriceRecord>()
                    .Where(c => c.Employee_ID == entity.tb_purorder.Employee_ID && c.ProdDetailID == group.Key.ProdDetailID).FirstAsync();
                    //如果存在则更新，否则插入
                    if (priceRecord == null)
                    {
                        priceRecord = new tb_PriceRecord();
                        priceRecord.ProdDetailID = group.Key.ProdDetailID;
                    }
                    priceRecord.Employee_ID = entity.tb_purorder.Employee_ID;
                    if (group.Value.UntaxedUnitPrice != priceRecord.PurPrice)
                    {
                        priceRecord.PurPrice = group.Value.UntaxedUnitPrice;
                        priceRecord.PurDate = System.DateTime.Now;
                        PriceRecords.Add(priceRecord);
                    }


                    #endregion

                    // 累加数值字段
                    inv.On_the_way_Qty += group.Value.PurQtySum.ToInt();
                    inv.Quantity -= group.Value.PurQtySum.ToInt();
                    inv.LatestStorageTime = System.DateTime.Now;
                    // 计算衍生字段（如总成本）
                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity; // 需确保 Inv_Cost 有值
                    invUpdateList.Add(inv);
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (invUpdateList.Count > 0)
                {
                    DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                    var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                    if (Counter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，库存更新数据为0，更新失败！");
                    }
                }

                if (BOM_SDetails.Count > 0)
                {
                    DbHelper<tb_BOM_SDetail> BOM_SDetaildbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_SDetail>>();
                    var BOM_SDetailCounter = await BOM_SDetaildbHelper.BaseDefaultAddElseUpdateAsync(BOM_SDetails);
                    if (BOM_SDetailCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，配方明细成本更新数据为0，更新失败！");
                    }
                }

                if (BOMs.Count > 0)
                {
                    DbHelper<tb_BOM_S> BOM_SdbHelper = _appContext.GetRequiredService<DbHelper<tb_BOM_S>>();
                    var BOMCounter = await BOM_SdbHelper.BaseDefaultAddElseUpdateAsync(BOMs);
                    if (BOMCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，配方主表成本更新数据为0，更新失败！");
                    }
                }

                if (PriceRecords.Count > 0)
                {
                    DbHelper<tb_PriceRecord> PriceRecorddbHelper = _appContext.GetRequiredService<DbHelper<tb_PriceRecord>>();
                    var PriceRecordCounter = await PriceRecorddbHelper.BaseDefaultAddElseUpdateAsync(PriceRecords);
                    if (PriceRecordCounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception("入库时，采购价格历史记录更新数据为0，更新失败！");
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
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {
                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.PurOrder_ChildID == entity.tb_purorder.tb_PurOrderDetails[i].PurOrder_ChildID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty += RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purorder.tb_PurOrderDetails[i].Quantity)
                            {

                                string msg = $"采购订单:{entity.tb_purorder.PurOrderNo}的【{prodName}】的入库数量不能大于订单中对应行的数量。";
                                rs.ErrorMsg = msg;
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                //当前行累计到交付
                                var RowQty = entity.tb_PurEntryDetails.Where(c => c.ProdDetailID == entity.tb_purorder.tb_PurOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_purorder.tb_PurOrderDetails[i].Location_ID).Sum(c => c.Quantity);
                                entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity -= RowQty;
                                entity.tb_purorder.tb_PurOrderDetails[i].UndeliveredQty += RowQty;
                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purorder.tb_PurOrderDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    #endregion
                    entity.tb_purorder.TotalUndeliveredQty = entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.UndeliveredQty);
                    //更新未交数量
                    int OrderCounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder).UpdateColumns(c => c.TotalUndeliveredQty).ExecuteCommandAsync();
                    if (OrderCounter > 0)
                    {

                    }
                    //更新已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurOrderDetail>(entity.tb_purorder.tb_PurOrderDetails).ExecuteCommandAsync();
                    if (updatecounter == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        throw new Exception($"入库单：{entity.PurEntryNo}反审核时，对应的订单：{entity.tb_purorder.PurOrderNo}明细中的已交数更新出错！");
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

                int counter = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(
                   it => new
                   {
                       it.DataStatus,
                       it.ApprovalStatus,
                       it.ApprovalResults,
                       it.ApprovalOpinions
                   }
                   ).ExecuteCommandAsync();


                //采购入库单，如果来自于采购订单，则要把入库数量累加到订单中的已交数量
                if (entity.tb_purorder != null && entity.tb_purorder.TotalQty != entity.tb_purorder.tb_PurOrderDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_purorder.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }
                #region 财务反审
                AuthorizeController authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                if (authorizeController.EnableFinancialModule())
                {
                    #region 反审 反核销预付

                    var ctrpayable = _appContext.GetRequiredService<tb_FM_ReceivablePayableController<tb_FM_ReceivablePayable>>();

                    //出库时，全部生成应收，账期的。就加上到期日
                    //有付款过的。就去预收中抵扣，不够的金额及状态标识出来
                    //如果收款了，则不能反审,预收的可以
                    var ARAPList = await _appContext.Db.Queryable<tb_FM_ReceivablePayable>()
                                    .Includes(c => c.tb_FM_ReceivablePayableDetails)
                                   .Where(c => c.SourceBillId == entity.PurEntryID
                                   && c.TotalLocalPayableAmount > 0 //正向
                                   && c.SourceBizType == (int)BizType.采购入库单).ToListAsync();
                    if (ARAPList != null && ARAPList.Count > 0)
                    {
                        if (ARAPList.Count == 1)
                        {
                            var result = await ctrpayable.AntiApplyManualPaymentAllocation(ARAPList[0], ReceivePaymentType.付款, true, false);
                        }
                        else
                        {
                            //不会为多行。有错误
                            _unitOfWorkManage.RollbackTran();
                            rs.ErrorMsg = $"采购入库单{entity.PurEntryNo}有多张应付款单，数据重复，请检查数据正确性后，再操作。";
                            rs.Succeeded = false;
                            return rs;
                        }
                    }

                    #endregion

                }

                #endregion

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



