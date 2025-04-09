
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/01/2023 18:04:38
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
using System.Windows.Forms;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using RUINORERP.Business.Security;
using RUINORERP.Global.EnumExt.CRM;
using System.Runtime.InteropServices.ComTypes;


namespace RUINORERP.Business
{
    public partial class tb_SaleOutController<T>
    {

        /// <summary>
        /// 批量结案  销售出库标记结案，数据状态为8,可以修改付款状态，同时检测销售订单的付款状态，也可以更新销售订单付款状态
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案就是一个财务确认过程
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_SaleOut> entitys = new List<tb_SaleOut>();
            entitys = NeedCloseCaseList as List<tb_SaleOut>;


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
                        //只修改订单的付款状态
                        if (entity.tb_saleorder.TotalQty == entity.tb_SaleOutDetails.Sum(c => c.Quantity))
                        {
                            entity.tb_saleorder.PayStatus = entity.PayStatus.Value;
                        }
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrder>(entity.tb_saleorder).ExecuteCommandAsync();

                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.完结;
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).UpdateColumns(it => new { it.DataStatus, it.PayStatus, it.Paytype_ID, it.Modified_by, it.Modified_at }).ExecuteCommandAsync();
                        //.Where(d => d.ProdBaseID == info.ProdBaseID).ExecuteCommandAsync();
                        // return affectedRows > 0;
                        //var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                        //await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).ExecuteCommandAsync();
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



        /// <summary>
        /// 审核其他出库单 注意逻辑是减少库存，并且更新单据本身状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rrs = new ReturnResults<T>();
            tb_SaleOut entity = ObjectEntity as tb_SaleOut;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                if (entity == null)
                {
                    return rrs;
                }
                //采购入库总数量和明细求和检查
                if (entity.TotalQty.Equals(entity.tb_SaleOutDetails.Sum(c => c.Quantity)) == false)
                {
                    rrs.ErrorMsg = $"销售出库数量与明细之和不相等!请检查数据后重试！";
                    _unitOfWorkManage.RollbackTran();
                    rrs.Succeeded = false;
                    return rrs;
                }

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                // entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //  entity.ApprovalResults = approvalEntity.ApprovalResults;

                #region 审核 通过时
                if (entity.ApprovalResults.Value)
                {

                    entity.tb_saleorder = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                        .Includes(a => a.tb_SaleOuts, b => b.tb_SaleOutDetails)
                        .Includes(a => a.tb_customervendor, b => b.tb_crm_customer, c => c.tb_crm_leads)
                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                      .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                        .Where(c => c.SOrder_ID == entity.SOrder_ID).Single();

                    //如果销售订单不是审核状态。则不能出库审核。
                    if (entity.tb_saleorder.DataStatus != (int)DataStatus.确认)
                    {
                        rrs.Succeeded = false;
                        _unitOfWorkManage.RollbackTran();
                        rrs.ErrorMsg = $"出库时，对应销售订单不是审核状态!请检查数据后重试！";
                        if (_appContext.SysConfig.ShowDebugInfo)
                        {
                            _logger.LogInformation(rrs.ErrorMsg);
                        }
                        return rrs;
                    }

                    //如果明细中的产品。不存在于订单中。审核失败。
                    foreach (var child in entity.tb_SaleOutDetails)
                    {
                        if (!entity.tb_saleorder.tb_SaleOrderDetails.Any(c => c.ProdDetailID == child.ProdDetailID && c.Location_ID == child.Location_ID))
                        {
                            rrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rrs.ErrorMsg = $"出库明细中有产品不属于当前销售订单!请检查数据后重试！";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rrs.ErrorMsg);
                            }
                            return rrs;
                        }
                    }

                    // 如果成本为零时则会实时检测库存成本，以库存成本为基准。这种情况解决未知成本，提前销售的情况。
                    //这里设置一个集合 用于保存特殊情况，后面统一更新
                    List<tb_SaleOutDetail> UpdateSaleOutCostlist = new List<tb_SaleOutDetail>();
                    List<tb_SaleOrderDetail> UpdateSaleOrderCostlist = new List<tb_SaleOrderDetail>();
                    foreach (var child in entity.tb_SaleOutDetails)
                    {
                        #region 库存表的更新 这里应该是必需有库存的数据，
                        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                        if (inv != null)
                        {
                            if (!_appContext.SysConfig.CheckNegativeInventory && (inv.Quantity - child.Quantity) < 0)
                            {

                                // rrs.ErrorMsg = "系统设置不允许负库存，请检查物料出库数量与库存相关数据";
                                rrs.ErrorMsg = $"库存为：{inv.Quantity}，拟销售量为：{child.Quantity}\r\n 系统设置不允许负库存， 请检查出库数量与库存相关数据";
                                _unitOfWorkManage.RollbackTran();
                                rrs.Succeeded = false;
                                return rrs;
                            }

                            //更新库存
                            inv.Quantity = inv.Quantity - child.Quantity;
                            inv.Sale_Qty = inv.Sale_Qty - child.Quantity;
                            BusinessHelper.Instance.EditEntity(inv);
                        }
                        else
                        {
                            _unitOfWorkManage.RollbackTran();
                            throw new Exception($"当前仓库{child.Location_ID}无产品{child.ProdDetailID}的库存数据,请联系管理员");
                        }
                        // CommService.CostCalculations.CostCalculation(_appContext, inv, child.TransactionPrice);
                        //inv.Inv_Cost = 0;//这里需要计算，根据系统设置中的算法计算。
                        inv.Inv_SubtotalCostMoney = inv.Inv_Cost * inv.Quantity;
                        inv.LatestOutboundTime = System.DateTime.Now;
                        #endregion

                        #region 更新销售价格历史记录
                        //注意这里的人应该是业务员的ID。销售订单的录入人，不是审核人。也不是出库单的人。
                        // tb_PriceRecordController<tb_PriceRecord> ctrPriceRecord = _appContext.GetRequiredService<tb_PriceRecordController<tb_PriceRecord>>();
                        tb_PriceRecord priceRecord = _unitOfWorkManage.GetDbClient().Queryable<tb_PriceRecord>()
                        .Where(c => c.Employee_ID == entity.tb_saleorder.Employee_ID.Value && c.ProdDetailID == child.ProdDetailID

                        ).First();
                        //如果存在则更新，否则插入
                        if (priceRecord == null)
                        {
                            priceRecord = new tb_PriceRecord();
                        }
                        priceRecord.Employee_ID = entity.tb_saleorder.Employee_ID.Value;
                        if (priceRecord.SalePrice != child.TransactionPrice)
                        {
                            priceRecord.SalePrice = child.TransactionPrice;
                            priceRecord.SaleDate = System.DateTime.Now;
                            priceRecord.ProdDetailID = child.ProdDetailID;

                            await _unitOfWorkManage.GetDbClient().Storageable(priceRecord).DefaultAddElseUpdate().ExecuteReturnEntityAsync();
                            //ReturnResults<tb_PriceRecord> rrpr = await ctrPriceRecord.SaveOrUpdate(priceRecord);
                        }

                        #endregion
                        await _unitOfWorkManage.GetDbClient().Storageable(inv).DefaultAddElseUpdate().ExecuteReturnEntityAsync();
                        //ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                        //if (rr.Succeeded)
                        //{
                        //}
                        //else
                        //{
                        //    return rrs;
                        //}
                        #region 如果成本为零时则会实时检测库存成本，以库存成本为基准。这种情况解决未知成本，提前销售的情况。
                        if (child.Cost == 0 && inv.Inv_Cost > 0)
                        {
                            child.Cost = inv.Inv_Cost;
                            UpdateSaleOutCostlist.Add(child);
                        }
                        #endregion

                    }


                    int UpdateSaleOutCostCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutDetail>(UpdateSaleOutCostlist).ExecuteCommandAsync();
                    if (UpdateSaleOutCostCounter > 0)
                    {

                    }

                    #region 回写订单状态及明细数据

                    //先找到所有出库明细,再找按订单明细去循环比较。如果出库总数量大于订单数量，则不允许出库。
                    List<tb_SaleOutDetail> detailList = new List<tb_SaleOutDetail>();
                    foreach (var item in entity.tb_saleorder.tb_SaleOuts)
                    {
                        detailList.AddRange(item.tb_SaleOutDetails);
                    }

                    //分两种情况处理。
                    for (int i = 0; i < entity.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                    {
                        //如果当前订单明细行，不存在于出库明细行。直接跳过。这种就是多行多品被删除时。不需要比较
                        decimal saleOutDetailCost = 0;
                        var saleOutDetail = detailList
                            .Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                        ).FirstOrDefault();
                        if (saleOutDetail != null)
                        {
                            saleOutDetailCost = saleOutDetail.Cost;
                        }

                        string prodName = entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.CNName +
                                  entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                        //明细中有相同的产品或物品。
                        //2024-4-29 思路更新:如果订单中有相同的产品的多行情况。出库明细冗余了订单明细的行号ID，就容易分清具体行的数据
                        var aa = entity.tb_saleorder.tb_SaleOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                        if (aa.Count > 0 && entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID > 0)
                        {
                            #region 如果存在不是引用的明细,则不允许出库。这样不支持手动添加的情况。
                            if (entity.tb_saleorder.tb_SaleOrderDetails.Any(c => c.SaleOrderDetail_ID == 0))
                            {
                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rrs;
                            }
                            #endregion

                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                            && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                              && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);

                            if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                            {
                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，\r\n\" " +
                                    $"或存在当前销售订单重复录入了销售出库单，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rrs;
                            }
                            else
                            {
                                var RowQty = entity.tb_SaleOutDetails
                                    .Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                    && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                                    && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                    ).Sum(c => c.Quantity);
                                //算出交付的数量
                                entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty += RowQty;

                                //如果是业务员在没有成本数据（新品口头上问了采购）就录入了订单。后面入库后销售出库时成本是正确时。要更新回去
                                if (entity.tb_saleorder.tb_SaleOrderDetails[i].Cost == 0 && saleOutDetailCost > 0)
                                {
                                    entity.tb_saleorder.tb_SaleOrderDetails[i].Cost = saleOutDetailCost;
                                    entity.tb_saleorder.tb_SaleOrderDetails[i].SubtotalCostAmount = saleOutDetailCost * entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity;
                                }

                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    string msg = $"销售出库单：{entity.SaleOutNo}审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，入库总数量不能大于订单数量！";
                                    return rrs;

                                }
                            }
                        }
                        else
                        {
                            //一对一时，找到所有的出库明细数量总和
                            var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                            && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                            {

                                string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，\r\n\" " +
                                    $"或存在当前销售订单重复录入了销售出库单，审核失败！";
                                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _unitOfWorkManage.RollbackTran();
                                _logger.LogInformation(msg);
                                return rrs;
                            }
                            else
                            {
                                //当前行累计到交付，只能是当前行所以重新找到当前出库单明细的的数量
                                var RowQty = entity.tb_SaleOutDetails.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                                entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty += RowQty;

                                //如果是业务员在没有成本数据（新品口头上问了采购）就录入了订单。后面入库后销售出库时成本是正确时。要更新回去
                                if (entity.tb_saleorder.tb_SaleOrderDetails[i].Cost == 0 && saleOutDetailCost > 0)
                                {
                                    entity.tb_saleorder.tb_SaleOrderDetails[i].Cost = saleOutDetailCost;
                                    entity.tb_saleorder.tb_SaleOrderDetails[i].SubtotalCostAmount = saleOutDetailCost * entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity;
                                }

                                //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                                if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                                {
                                    _unitOfWorkManage.RollbackTran();
                                    string msg = $"销售出库单：{entity.SaleOutNo}审核时，【{prodName}】的出库总数量不能大于订单数量！";
                                    return rrs;

                                }
                            }
                        }
                    }

                    //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                    if (entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) > entity.tb_saleorder.TotalQty)
                    {
                        _unitOfWorkManage.RollbackTran();
                        string msg = $"销售订单：{entity.tb_saleorder.SOrderNo}中，出库总交付数量不能大于订单数量！";
                        return rrs;
                    }

                    //更新已交数量
                    int poCounter = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleorder.tb_SaleOrderDetails).ExecuteCommandAsync();
                    if (poCounter > 0)
                    {
                        if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                        {
                            // _logger.Debug(entity.SaleOutNo + "==>" + entity.tb_saleorder.SOrderNo + $"对应的订单更新成功===重点代码 看已交数量是否正确");
                        }
                    }

                    #endregion


                    //如果出库金额为0，但是订单中不是为0，则不允许出库,如果明细中注明了。都是赠品是可以的。
                    if (entity.TotalAmount == 0 && entity.tb_saleorder.TotalAmount != 0 && !entity.tb_SaleOutDetails.Any(c => c.Gift))
                    {
                        string msg = $"出库单:{entity.tb_saleorder.SOrderNo}的总金额为0， 但是订单中不为0，审核失败！请检查数据!";
                        _unitOfWorkManage.RollbackTran();
                        _logger.LogInformation(msg);
                        return rrs;
                    }

                    #region CRM

                    //如果是新客户或潜在客户。则转换为首单客户
                    if (entity.tb_saleorder.tb_customervendor.tb_crm_customer != null)
                    {
                        var crm_customer = entity.tb_saleorder.tb_customervendor.tb_crm_customer;
                        if (crm_customer.CustomerStatus == (int)CustomerStatus.潜在客户 ||
                            crm_customer.CustomerStatus == (int)CustomerStatus.新增客户)
                        {
                            crm_customer.CustomerStatus = (int)CustomerStatus.首单客户;
                            crm_customer.FirstPurchaseDate = entity.OutDate;
                        }

                        //更新采购金额  结案时才算次数。
                        if (crm_customer.PurchaseCount == null)
                        {
                            crm_customer.PurchaseCount = 0;
                        }

                        if (crm_customer.TotalPurchaseAmount == null)
                        {
                            crm_customer.TotalPurchaseAmount = 0;
                        }
                        crm_customer.TotalPurchaseAmount += entity.tb_SaleOutDetails.Sum(c => c.Quantity * c.TransactionPrice);
                        crm_customer.LastPurchaseDate = entity.OutDate;
                        if (crm_customer.FirstPurchaseDate.HasValue)
                        {
                            TimeSpan duration = crm_customer.LastPurchaseDate.Value - crm_customer.FirstPurchaseDate.Value;
                            int days = duration.Days;
                            crm_customer.DaysSinceLastPurchase = days; //这个可以反审时倒算出来。
                        }
                        else
                        {
                            crm_customer.FirstPurchaseDate = entity.OutDate;
                        }

                        //这个是结案时才算次数
                        if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                        {
                            crm_customer.PurchaseCount = crm_customer.PurchaseCount + 1;
                        }


                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder.tb_customervendor.tb_crm_customer).UpdateColumns(t => new { t.CustomerStatus, t.PurchaseCount, t.TotalPurchaseAmount, t.LastPurchaseDate, t.DaysSinceLastPurchase, t.FirstPurchaseDate }).ExecuteCommandAsync();

                        //如果这个客户是由线索转过来的则线索转化成功
                        if (entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads != null)
                        {
                            entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads.LeadsStatus = (int)LeadsStatus.已转化;
                            await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder.tb_customervendor.tb_crm_customer.tb_crm_leads).UpdateColumns(t => new { t.LeadsStatus }).ExecuteCommandAsync();
                        }
                    }

                    #endregion

                    //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                    if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) == entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity)
                        && entity.tb_saleorder.DataStatus == (int)DataStatus.确认)
                    {
                        entity.tb_saleorder.DataStatus = (int)DataStatus.完结;
                        entity.tb_saleorder.CloseCaseOpinions = "【系统自动结案】==》" + System.DateTime.Now.ToString() + _appContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "审核销售库单时:" + entity.SaleOutNo + "结案。"; ;
                        entity.tb_saleorder.TotalCost = entity.tb_SaleOutDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);
                        await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder).UpdateColumns(t => new { t.DataStatus, t.CloseCaseOpinions, entity.tb_saleorder.TotalCost }).ExecuteCommandAsync();
                    }

                    #endregion
                    //运费检测  如果一个订单有运费。多次出库时。运费也默认加到了每次的出库单。
                    //这里审核时检测之前的是不是已加过（多次出库第一次加上运费。后面财务如何处理？再说） 第二次起都是为0.
                    if (entity.tb_saleorder.tb_SaleOuts.Count > 1)
                    {
                        entity.ShipCost = 0;
                    }
                    entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                    BusinessHelper.Instance.ApproverEntity(entity);
                    //只更新指定列
                    int last = await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).ExecuteCommandAsync();
                    if (last > 0)
                    {

                    }
                    else
                    {
                        _logger.LogInformation("审核销售出库单失败" + entity.SaleOutNo);
                        _unitOfWorkManage.RollbackTran();
                        rrs.Succeeded = false;
                        return rrs;
                    }
                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rrs.ReturnObject = entity as T;
                rrs.Succeeded = true;
                return rrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                rrs.Succeeded = false;
                rrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, "事务回滚");
                return rrs;
            }

        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_SaleOut entity = ObjectEntity as tb_SaleOut;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                //  tb_OpeningInventoryController<tb_OpeningInventory> ctrOPinv = _appContext.GetRequiredService<tb_OpeningInventoryController<tb_OpeningInventory>>();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少

                //判断是否能反审?
                if (entity.tb_SaleOutRes != null
                    && (entity.tb_SaleOutRes.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_SaleOutRes.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rs.ErrorMsg = "存在已确认或已完结，或已审核的销售退回单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    //return false;
                    rs.ErrorMsg = "有结案的单据，已经跳过反审";
                    _unitOfWorkManage.RollbackTran();
                    rs.Succeeded = false;
                    return rs;
                }
                foreach (var child in entity.tb_SaleOutDetails)
                {
                    #region 库存表的更新 ，
                    tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == child.Location_ID);
                    if (inv == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rs.ErrorMsg = $"{child.ProdDetailID}当前产品无库存数据，无法借出。请使用【期初盘点】【采购入库】】【生产缴库】的方式进行盘点后，再操作。";
                        rs.Succeeded = false;
                        return rs;
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
                    //反审，出库的要加回来，要卖的也要加回来
                    inv.Quantity = inv.Quantity + child.Quantity;
                    inv.Sale_Qty = inv.Sale_Qty + child.Quantity;
                    //最后出库时间要改回来，这里没有处理
                    //inv.LatestStorageTime
                    BusinessHelper.Instance.EditEntity(inv);
                    #endregion

                    await _unitOfWorkManage.GetDbClient().Storageable(inv).ExecuteReturnEntityAsync();
                    // ReturnResults<tb_Inventory> rr = await ctrinv.SaveOrUpdate(inv);
                    // if (rr.Succeeded)
                    // {

                    // }
                }

                #region  反审检测写回 退回
                //处理销售订单
                entity.tb_saleorder = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrder>()
                     .Includes(a => a.tb_SaleOuts, b => b.tb_SaleOutDetails)
                     .Includes(a => a.tb_customervendor, b => b.tb_crm_customer, c => c.tb_crm_leads)
                     .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                     .Includes(a => a.tb_SaleOrderDetails, b => b.tb_proddetail, c => c.tb_prod)
                     .Where(c => c.SOrder_ID == entity.SOrder_ID)
                     .Single();

                //先找到所有出库明细,再找按订单明细去循环比较。如果出库总数量大于订单数量，则不允许出库。
                List<tb_SaleOutDetail> detailList = new List<tb_SaleOutDetail>();
                foreach (var item in entity.tb_saleorder.tb_SaleOuts)
                {
                    detailList.AddRange(item.tb_SaleOutDetails);
                }

                //分两种情况处理。
                for (int i = 0; i < entity.tb_saleorder.tb_SaleOrderDetails.Count; i++)
                {
                    //如果当前订单明细行，不存在于出库明细行。直接跳过。这种就是多行多品被删除时。不需要比较

                    string prodName = entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.CNName +
                              entity.tb_saleorder.tb_SaleOrderDetails[i].tb_proddetail.tb_prod.Specifications;
                    //明细中有相同的产品或物品。
                    var aa = entity.tb_saleorder.tb_SaleOrderDetails.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID > 0)
                    {
                        #region 如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                        if (entity.tb_SaleOutDetails.Any(c => c.SaleOrderDetail_ID == 0))
                        {
                            //如果存在不是引用的明细,则不允许入库。这样不支持手动添加的情况。
                            string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】在订单明细中拥有多行记录，必须使用引用的方式添加，反审失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        #endregion

                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                        && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                         && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                        ).Sum(c => c.Quantity);
                        if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                        {
                            string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            var RowQty = entity.tb_SaleOutDetails.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                            && c.SaleOrderDetail_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].SaleOrderDetail_ID
                              && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                            ).Sum(c => c.Quantity);
                            //算出交付的数量
                            entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty -= RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty < 0)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"销售出库单：{entity.SaleOutNo}反审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，{prodName}的明细不能为负数！");
                            }
                        }
                    }
                    else
                    {
                        //一对一时
                        var inQty = detailList.Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                        && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                        ).Sum(c => c.Quantity);
                        if (inQty > entity.tb_saleorder.tb_SaleOrderDetails[i].Quantity)
                        {

                            string msg = $"销售订单:{entity.tb_saleorder.SOrderNo}的【{prodName}】的出库数量不能大于订单中对应行的数量，审核失败！";
                            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _unitOfWorkManage.RollbackTran();
                            _logger.LogInformation(msg);
                            return rs;
                        }
                        else
                        {
                            //当前行累计到交付
                            var RowQty = entity.tb_SaleOutDetails
                                .Where(c => c.ProdDetailID == entity.tb_saleorder.tb_SaleOrderDetails[i].ProdDetailID
                                && c.Location_ID == entity.tb_saleorder.tb_SaleOrderDetails[i].Location_ID
                                ).Sum(c => c.Quantity);
                            entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty -= RowQty;
                            //如果已交数据大于 订单数量 给出警告实际操作中 使用其他方式将备品入库
                            if (entity.tb_saleorder.tb_SaleOrderDetails[i].TotalDeliveredQty < 0)
                            {
                                _unitOfWorkManage.RollbackTran();
                                throw new Exception($"入库单：{entity.SaleOutNo}反审核时，对应的订单：{entity.tb_saleorder.SOrderNo}，{prodName}的明细不能为负数！");
                            }
                        }
                    }
                }
                #endregion

                //更新已交数量
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity.tb_saleorder.tb_SaleOrderDetails).ExecuteCommandAsync();
                //销售出库单，如果来自于销售订单，则要把出库数量累加到订单中的已交数量 并且如果数量够则自动结案
                if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) != entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                {
                    entity.tb_saleorder.DataStatus = (int)DataStatus.确认;
                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();
                }

                //如果是新客户或潜在客户。则转换为首单客户
                //降级 退回
                if (entity.tb_saleorder.tb_customervendor.tb_crm_customer != null)
                {
                    var crm_customer = entity.tb_saleorder.tb_customervendor.tb_crm_customer;
                    if (crm_customer.CustomerStatus == (int)CustomerStatus.首单客户)
                    {
                        crm_customer.CustomerStatus = (int)CustomerStatus.潜在客户;
                        crm_customer.LastPurchaseDate = entity.OutDate;
                    }

                    //更新采购金额
                    if (crm_customer.TotalPurchaseAmount == null)
                    {
                        crm_customer.TotalPurchaseAmount = 0;
                    }
                    if (crm_customer.PurchaseCount == null)
                    {
                        crm_customer.PurchaseCount = 0;
                    }

                    crm_customer.TotalPurchaseAmount -= entity.tb_SaleOutDetails.Sum(c => c.Quantity * c.TransactionPrice);
                    if (crm_customer.DaysSinceLastPurchase.HasValue)
                    {
                        crm_customer.LastPurchaseDate = crm_customer.LastPurchaseDate.Value.AddDays(-crm_customer.DaysSinceLastPurchase.Value); //todo ??// 这个如何退回？
                    }
                    else
                    {
                        crm_customer.LastPurchaseDate = null; //没有采购过
                    }

                    //撤回完结时次数也减少1
                    if (entity.tb_saleorder != null && entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.TotalDeliveredQty) != entity.tb_saleorder.tb_SaleOrderDetails.Sum(c => c.Quantity))
                    {
                        crm_customer.PurchaseCount = crm_customer.PurchaseCount - 1;
                    }

                    await _unitOfWorkManage.GetDbClient().Updateable(entity.tb_saleorder.tb_customervendor.tb_crm_customer).UpdateColumns(t => new { t.CustomerStatus, t.PurchaseCount, t.TotalPurchaseAmount, t.LastPurchaseDate }).ExecuteCommandAsync();
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOut>(entity).ExecuteCommandAsync();

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
                rs.ErrorMsg = ex.Message;
                rs.Succeeded = false;
                return rs;
            }

        }






        public async override Task<List<T>> GetPrintDataSource(long SaleOut_MainID)
        {
            // var queryable = _appContext.Db.Queryable<tb_SaleOutDetail>();
            // var list = _appContext.Db.Queryable(queryable).LeftJoin<View_ProdDetail>((o, d) => o.ProdDetailID == d.ProdDetailID).Select(o => o).ToList();

            List<tb_SaleOut> list = await _appContext.Db.CopyNew().Queryable<tb_SaleOut>().Where(m => m.SaleOut_MainID == SaleOut_MainID)
                             .Includes(a => a.tb_customervendor)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_SaleOutDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }






    }

}



