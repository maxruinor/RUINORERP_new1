
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
using System.Collections;

namespace RUINORERP.Business
{
    public partial class tb_PurReturnEntryController<T>
    {

        /// <summary>
        /// 返回批量审核的结果
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            tb_PurReturnEntry entity = ObjectEntity as tb_PurReturnEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();
               

                //处理采购退货单
                entity.tb_purentryre = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryRe>()
                     .Includes(a => a.tb_PurReturnEntries, b => b.tb_PurReturnEntryDetails)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.PurEntryRe_ID == entity.PurEntryRe_ID)
                     .Single();

                if (entity.tb_purentryre == null)
                {
                    rs.ErrorMsg = $"没有找到对应的采购退货单!请检查数据后重试！";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                //如果入库明细中的产品。不存在于采购退货单中。审核失败。
                foreach (var child in entity.tb_PurReturnEntryDetails)
                {
                    if (!entity.tb_purentryre.tb_PurEntryReDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID==child.Location_ID))
                    {
                        rs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"入库明细中有产品不属于采购退货单!请检查数据后重试！";
                        return rs;
                    }
                }

                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();

                foreach (tb_PurReturnEntryDetail child in entity.tb_PurReturnEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，

                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                       _unitOfWorkManage.RollbackTran();
                       rs.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法进行采购退货。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                        rs.Succeeded = false;
                        return rs;
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

                    //采购退货单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty - child.Quantity;

                    //直接输入成本：在录入库存记录时，直接输入该产品或物品的成本价格。这种方式适用于成本价格相对稳定或容易确定的情况。
                    //平均成本法：通过计算一段时间内该产品或物品的平均成本来确定成本价格。这种方法适用于成本价格随时间波动的情况，可以更准确地反映实际成本。
                    //先进先出法（FIFO）：按照先入库的产品先出库的原则，计算库存成本。这种方法适用于库存流转速度较快，成本价格相对稳定的情况。适用范围：适用于存货的实物流转比较符合先进先出的假设，比如食品、药品等有保质期限制的商品，先购进的存货会先发出销售。

                    //采购退货入库暂时不影响成本
                    /*
                   if (child.IsGift.HasValue && child.IsGift == false && child.TransactionPrice > 0)
                   {
                       CommService.CostCalculations.CostCalculation(_appContext, inv, child.Quantity, child.TransactionPrice);
                       #region 更新BOM价格,当前产品存在哪些BOM中，则更新所有BOM的价格包含主子表数据的变化

                       tb_BOM_SDetailController<tb_BOM_SDetail> ctrtb_BOM_SDetail = _appContext.GetRequiredService<tb_BOM_SDetailController<tb_BOM_SDetail>>();
                       List<tb_BOM_SDetail> bomDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_SDetail>()
                       .Includes(b => b.tb_bom_s, d => d.tb_BOM_SDetails)
                       .Where(c => c.ProdDetailID == child.ProdDetailID).ToList();
                       foreach (tb_BOM_SDetail bomDetail in bomDetails)
                       {
                           //如果存在则更新 
                           bomDetail.UnitCost = inv.Inv_Cost;
                           bomDetail.SubtotalUnitCost = bomDetail.UnitCost * bomDetail.UsedQty;
                           if (bomDetail.tb_bom_s != null)
                           {
                               bomDetail.tb_bom_s.TotalMaterialCost = bomDetail.tb_bom_s.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                               bomDetail.tb_bom_s.OutProductionAllCosts = bomDetail.tb_bom_s.TotalMaterialCost + bomDetail.tb_bom_s.TotalOutManuCost + bomDetail.tb_bom_s.OutApportionedCost;
                               bomDetail.tb_bom_s.SelfProductionAllCosts = bomDetail.tb_bom_s.TotalMaterialCost + bomDetail.tb_bom_s.TotalSelfManuCost + bomDetail.tb_bom_s.SelfApportionedCost;
                               await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_S>(bomDetail.tb_bom_s).ExecuteCommandAsync();
                           }
                       }
                       await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(bomDetails).ExecuteCommandAsync();

                       #endregion
                   }
                    */

                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestStorageTime = System.DateTime.Now;
                    #endregion
                    invUpdateList.Add(inv);
                    
                }

                int InvUpdateCounter = await _unitOfWorkManage.GetDbClient().Updateable(invUpdateList).ExecuteCommandAsync();
                if (InvUpdateCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新失败！");
                }


                //因为可以多次分批入库，所以需要判断当前入库数量是否大于退货数量
                //先找到所有采购退货入库明细,再找按采购退款明细去循环比较。如果入库总数量大于退货数量，则不允许入库。
                List<tb_PurReturnEntryDetail> detailList = new List<tb_PurReturnEntryDetail>();
                foreach (var item in entity.tb_purentryre.tb_PurReturnEntries)
                {
                    detailList.AddRange(item.tb_PurReturnEntryDetails);
                }

                //分两种情况处理。
                for (int i = 0; i < entity.tb_purentryre.tb_PurEntryReDetails.Count; i++)
                {
                    //如果当前采购退款明细行，不存在于采购退货入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                    string prodName = entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.Specifications;
                    //明细中有相同的产品或物品。
                    var aa = entity.tb_purentryre.tb_PurEntryReDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID > 0)
                    {
                        #region 如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                        if (entity.tb_PurReturnEntryDetails.Any(c => c.PurReEntry_CID == 0))
                        {
                            //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】在明细中拥有多行记录，必须使用引用的方式添加，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        #endregion

                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                        && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                        {
                            string msg = $"【{prodName}】的采购退货入库数量不能大于退货单中对应行的数量\r\n" + $"或存在针对当前采购退货单重复录入了采购退货入库单，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            var RowQty = entity.tb_PurReturnEntryDetails
                                .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID 
                                && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID
                                 && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                            //算出交付的数量
                            entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"采购退货入库：{entity.PurReEntryNo}审核时，对应的采购退货单：{entity.tb_purentryre.PurEntryReNo}中，入库总数量不能大于退货数量！");
                            }
                        }
                    }
                    else
                    {
                        //一对一时
                        var inQty = detailList
                            .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                         && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                        ).Sum(c => c.Quantity);
                        if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                        {

                            string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】的入库数量不能大于退货单中对应行的数量\r\n" + $"                                    或存在针对当前采购退货单重复录入了采购入库单，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付
                            var RowQty = entity.tb_PurReturnEntryDetails
                                .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                && c.Location_ID==entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                            entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity += RowQty;
                            //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"入库单：{entity.PurReEntryNo}审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，入库总数量不能大于退货单数量！");
                            }
                        }
                    }
                }

                //更新已交数量
                int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryReDetail>(entity.tb_purentryre.tb_PurEntryReDetails).ExecuteCommandAsync();
                if (poCounter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Debug(entity.PurReEntryNo + "==>" + entity.PurEntryReNo + $"对应 的退货单更新成功===重点代码 看已交数量是否正确");
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
                int counter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurReturnEntry>(entity).ExecuteCommandAsync();
                if (counter > 0)
                {
                    if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                    {
                        _logger.Info(entity.PurReEntryNo + "==>" + "状态更新成功");
                    }
                }

                //采购入库单，如果来自于采购退货单，则要把入库数量累加到退货单中的已交数量 TODO 销售也会有这种情况
                if (entity.tb_purentryre != null && entity.tb_purentryre.DataStatus == (int)DataStatus.确认 && (entity.TotalQty == entity.tb_purentryre.TotalQty || entity.tb_purentryre.tb_PurEntryReDetails.Sum(c => c.DeliveredQuantity) == entity.tb_purentryre.TotalQty))
                {
                    entity.tb_purentryre.DataStatus = (int)DataStatus.完结;
                    // entity.tb_purentryre.ApprovalOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核入库单:" + entity.PurReEntryNo + "结案。"; ;
                    int poendcounter = await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purentryre)
                        .UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                    if (poendcounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            _logger.Info(entity.tb_purentryre.PurEntryReNo + "==>" + "结案状态更新成功");
                        }
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
                _logger.Error(ex, "事务回滚" + ex.Message);
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
            tb_PurReturnEntry entity = ObjectEntity as tb_PurReturnEntry;
            ReturnResults<T> rs = new ReturnResults<T>();
            rs.Succeeded = false;
            try
            {

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();
                List<tb_Inventory> invUpdateList = new List<tb_Inventory>();
                foreach (var child in entity.tb_PurReturnEntryDetails)
                {
                    #region 库存表的更新 这里应该是必需有库存的数据，
                    //实际 期初已经有数据了，则要
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
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

                    //采购退货单时添加 。这里减掉在路上的数量
                    inv.On_the_way_Qty = inv.On_the_way_Qty + child.Quantity;

                    //这个业务暂时不处理成本

                    inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                    inv.LatestOutboundTime = System.DateTime.Now;

                    #endregion
                    invUpdateList.Add(inv);
                   
                }
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvUpdateCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invUpdateList);
                 if (InvUpdateCounter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新失败！");
                }


                if (entity.tb_purentryre != null)
                {
                    #region  反审检测写回 退回

                    //处理采购退货单
                    entity.tb_purentryre = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryRe>()
                         .Includes(a => a.tb_PurReturnEntries, b => b.tb_PurReturnEntryDetails)
                         .Includes(t => t.tb_PurEntryReDetails)
                         .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                         .Includes(a => a.tb_PurEntryReDetails, b => b.tb_proddetail, c => c.tb_prod)
                         .Where(c => c.PurEntryRe_ID == entity.PurEntryRe_ID)
                         .Single();


                    //先找到所有入库明细,再找按退货单明细去循环比较。如果入库总数量大于退货单数量，则不允许入库。
                    List<tb_PurReturnEntryDetail> detailList = new List<tb_PurReturnEntryDetail>();
                    foreach (var item in entity.tb_purentryre.tb_PurReturnEntries)
                    {
                        detailList.AddRange(item.tb_PurReturnEntryDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_purentryre.tb_PurEntryReDetails.Count; i++)
                    {
                        //如果当前退货单明细行，不存在于入库明细行。直接跳过。这种就是多行多品被删除时。不需要比较


                        string prodName = entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_purentryre.tb_PurEntryReDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        var aa = entity.tb_purentryre.tb_PurEntryReDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID > 0)
                        {
                            #region //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            if (entity.tb_PurReturnEntryDetails.Any(c => c.PurEntryRe_CID == 0))
                            {
                                //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                                string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】在退货单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            #endregion

                            var inQty = detailList
                                .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                             && c.Location_ID == entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                            {
                                string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】的入库数量不能大于退货单中对应行的数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(msg);
                                }
                                return rs;
                            }
                            else
                            {
                                var RowQty = entity.tb_PurReturnEntryDetails
                                    .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID 
                                    && c.PurEntryRe_CID == entity.tb_purentryre.tb_PurEntryReDetails[i].PurEntryRe_CID
                                    && c.Location_ID== entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                    
                                    ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurReEntryNo}反审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，{prodName}的明细不能为负数！"); throw new Exception($"入库单：{entity.PurReEntryNo}审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，入库总数量不能大于退货单数量！");
                                }
                            }
                        }
                        else
                        {
                            //一对一时
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID).Sum(c => c.Quantity);
                            if (inQty > entity.tb_purentryre.tb_PurEntryReDetails[i].Quantity)
                            {

                                string msg = $"采购退货单:{entity.tb_purentryre.PurEntryReNo}的【{prodName}】的入库数量不能大于退货单中对应行的数量，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                var RowQty = entity.tb_PurReturnEntryDetails
                                    .Where(c => c.ProdDetailID == entity.tb_purentryre.tb_PurEntryReDetails[i].ProdDetailID
                                    && c.Location_ID==entity.tb_purentryre.tb_PurEntryReDetails[i].Location_ID
                                    ).Sum(c => c.Quantity);
                                entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity -= RowQty;
                                //如果已交数据大于 退货单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_purentryre.tb_PurEntryReDetails[i].DeliveredQuantity < 0)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    throw new Exception($"入库单：{entity.PurReEntryNo}反审核时，对应的退货单：{entity.tb_purentryre.PurEntryReNo}，{prodName}的明细不能为负数！");
                                }
                            }
                        }
                    }


                    #endregion

                    //更新已交数量
                    int updatecounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryReDetail>(entity.tb_purentryre.tb_PurEntryReDetails).ExecuteCommandAsync();
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
                await _unitOfWorkManage.GetDbClient().Updateable<tb_PurReturnEntry>(entity).ExecuteCommandAsync();

                //采购入库单，如果来自于采购退货单，则要把入库数量累加到退货单中的已交数量
                if (entity.tb_purentryre != null && entity.tb_purentryre.TotalQty != entity.tb_purentryre.tb_PurEntryReDetails.Sum(c => c.DeliveredQuantity))
                {
                    entity.tb_purentryre.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_purentryre).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }


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
            List<tb_PurReturnEntry> list = await _appContext.Db.CopyNew().Queryable<tb_PurReturnEntry>().Where(m => m.PurReEntry_ID == MainID)
                             .Includes(a => a.tb_customervendor)
                                .Includes(a => a.tb_employee)
                            .Includes(a => a.tb_purentryre)
                          .Includes(a => a.tb_department)
                           .Includes(a => a.tb_PurReturnEntryDetails, c => c.tb_location)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_PurReturnEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                    .Includes(a => a.tb_PurReturnEntryDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                 .ToListAsync();
            return list as List<T>;
        }





    }
}



