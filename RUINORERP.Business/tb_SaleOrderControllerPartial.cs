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
using System.Security.Policy;
using AutoMapper;
using System.Windows.Forms;
using RUINORERP.Global.EnumExt;
using Fireasy.Common.Extensions;
using System.Collections;
using Mapster;

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
        /// 销售订金（预收）	- 预收定金生成预收单--》预收审核时 生成收款单 收款单审核代表完成支付。
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
                    //订金，部分收款和全部收款都要生成预收款。（因为财务必须有一个审核收款行为）
                    //销售出库时，生成应收去核销预收。 如果客户超付，（预收，收款，实际已经支付。 出库时 只要生成应收，和核销记录，并且回写
                    if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                    {
                        //正常来说。不能重复生成。即使退款也只会有一个对应订单的预收款单。 一个预收款单可以对应正负两个收款单。
                        // 生成预收款单前 检测
                        var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                        var PreReceivedPayment = ctrpay.BuildPreReceivedPayment(entity);
                        if (PreReceivedPayment.LocalPrepaidAmount > 0)
                        {
                            ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrpay.SaveOrUpdate(PreReceivedPayment);
                            if (!rmpay.Succeeded)
                            {
                                // 处理预收款单生成失败的情况
                                rmrs.Succeeded = false;
                                _unitOfWorkManage.RollbackTran();
                                rmrs.ErrorMsg = $"预收款单生成失败：{rmpay.ErrorMsg ?? "未知错误"}";
                                if (_appContext.SysConfig.ShowDebugInfo)
                                {
                                    _logger.LogInformation(rmrs.ErrorMsg);
                                }
                            }
                            else
                            {
                                if (_appContext.FMConfig.AutoAuditPreReceive)
                                {
                                    #region 自动审核预收款
                                    //销售订单审核时自动将预付款单设为"已生效"状态
                                    PreReceivedPayment.ApprovalOpinions = "系统自动审核";
                                    PreReceivedPayment.ApprovalStatus = (int)ApprovalStatus.已审核;
                                    PreReceivedPayment.ApprovalResults = true;
                                    ReturnResults<tb_FM_PreReceivedPayment> autoApproval = await ctrpay.ApprovalAsync(PreReceivedPayment);
                                    if (!autoApproval.Succeeded)
                                    {
                                        rmrs.Succeeded = false;
                                        _unitOfWorkManage.RollbackTran();
                                        rmrs.ErrorMsg = $"预收款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                        if (_appContext.SysConfig.ShowDebugInfo)
                                        {
                                            _logger.LogInformation(rmrs.ErrorMsg);
                                        }
                                    }
                                    else
                                    {
                                        FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                        fMAuditLog.CreateAuditLog<tb_FM_PreReceivedPayment>("预收款单自动审核成功", autoApproval.ReturnObject as tb_FM_PreReceivedPayment);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                entity.ApprovalResults = true;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity).UpdateColumns(it => new
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
                _logger.Error(ex, "销售订单审核时，事务回滚" + ex.Message);
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }


        /// <summary>
        /// 手动生成预付款单
        /// </summary>
        /// <param name="PrepaidAmount">本次预付金额</param>
        /// <param name="entity">对应的订单</param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_FM_PreReceivedPayment>> ManualPrePayment(decimal PrepaidAmount, tb_SaleOrder entity)
        {
            ReturnResults<tb_FM_PreReceivedPayment> rmrs = new ReturnResults<tb_FM_PreReceivedPayment>();
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
                //订金，部分收款和全部收款都要生成预收款。（因为财务必须有一个审核收款行为）
                //销售出库时，生成应收去核销预收。 如果客户超付，（预收，收款，实际已经支付。 出库时 只要生成应收，和核销记录，并且回写
                if (entity.Paytype_ID != _appContext.PaymentMethodOfPeriod.Paytype_ID)
                {
                    //正常来说。不能重复生成。即使退款也只会有一个对应订单的预收款单。 一个预收款单可以对应正负两个收款单。
                    // 生成预收款单前 检测
                    var ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                    var PreReceivedPayment = ctrpay.BuildPreReceivedPayment(entity, PrepaidAmount);
                    if (PreReceivedPayment.LocalPrepaidAmount > 0)
                    {
                        ReturnResults<tb_FM_PreReceivedPayment> rmpay = await ctrpay.SaveOrUpdate(PreReceivedPayment);
                        if (!rmpay.Succeeded)
                        {
                       
                            // 处理预收款单生成失败的情况
                            rmrs.Succeeded = false;
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"预收款单生成失败：{rmpay.ErrorMsg ?? "未知错误"}";
                            if (_appContext.SysConfig.ShowDebugInfo)
                            {
                                _logger.LogInformation(rmrs.ErrorMsg);
                            }
                        }
                        else
                        {
                            rmrs.ReturnObject = rmpay.ReturnObject;
                            if (_appContext.FMConfig.AutoAuditPreReceive)
                            {
                                #region 自动审核预收款
                                //销售订单审核时自动将预付款单设为"已生效"状态
                                PreReceivedPayment.ApprovalOpinions = "再次收到预付款，系统自动审核";
                                PreReceivedPayment.ApprovalStatus = (int)ApprovalStatus.已审核;
                                PreReceivedPayment.ApprovalResults = true;
                                ReturnResults<tb_FM_PreReceivedPayment> autoApproval = await ctrpay.ApprovalAsync(PreReceivedPayment);
                                if (!autoApproval.Succeeded)
                                {
                                    rmrs.Succeeded = false;
                                    _unitOfWorkManage.RollbackTran();
                                    rmrs.ErrorMsg = $"预收款单自动审核失败：{autoApproval.ErrorMsg ?? "未知错误"}";
                                    if (_appContext.SysConfig.ShowDebugInfo)
                                    {
                                        _logger.LogInformation(rmrs.ErrorMsg);
                                    }
                                }
                                else
                                {
                                    rmrs.ReturnObject = autoApproval.ReturnObject;
                                    FMAuditLogHelper fMAuditLog = _appContext.GetRequiredService<FMAuditLogHelper>();
                                    fMAuditLog.CreateAuditLog<tb_FM_PreReceivedPayment>("预收款单自动审核成功", autoApproval.ReturnObject as tb_FM_PreReceivedPayment);
                                }
                                #endregion
                            }
                        }
                    }
                }
                #endregion
            }
            return rmrs;
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
                        var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();
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

                //判断是否能反审? 如果出库是草稿，订单反审 修改后。出库再提交 审核。所以 出库审核要核对订单数据。
                if (entity.tb_SaleOuts != null
                    && (entity.tb_SaleOuts.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结)
                    && entity.tb_SaleOuts.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {
                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的销售出库单，不能反审核,请联系管理员，或作退回处理。";
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
                    //var PrePaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(p => p.SourceBillId.HasValue && p.SourceBillId.Value == entity.SOrder_ID).ToListAsync();
                    //var PrePaymentList1 = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>().Where(p => p.SourceBillId.HasValue && p.SourceBillId.Value == entity.SOrder_ID).ToListAsync();
                    var PrePaymentQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PreReceivedPayment>()
                        .Where(p => p.SourceBillId == entity.SOrder_ID && p.SourceBizType == (int)BizType.销售订单 && p.Currency_ID == entity.Currency_ID);
                    var PrePaymentList = await PrePaymentQueryable.ToListAsync();
                    if (PrePaymentList != null && PrePaymentList.Count > 0)
                    {
                        var PrePayment = PrePaymentList[0];
                        //一个订单。只会有一个预收款单
                        if (PrePayment != null)
                        {
                            if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 || PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核)
                            {
                                await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                            }

                            #region  检测对应的收款单记录，如果没有支付也可以直接删除
                            //订单反审核  只是用来修改，还是真实取消订单。取消的话。则要退款。修改的话。则不需要退款。
                            //如果没有出库，则生成红冲单  ，已冲销  已取消，先用取消标记
                            //如果是要退款，则在预收款查询这，生成退款单。
                            //如果预收单审核了，生成收款单 在财务没有审核前。还是可以反审。这是为了保存系统的灵活性。
                            var PaymentList = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentRecord>()
                                  .Includes(a => a.tb_FM_PaymentRecordDetails)
                                 .Where(c => c.tb_FM_PaymentRecordDetails.Any(d => d.SourceBilllId == PrePayment.PreRPID)).ToListAsync();
                            if (PaymentList != null && PaymentList.Count > 0)
                            {
                                if (PaymentList.Count > 1 && PaymentList.Sum(c => c.TotalLocalAmount) == 0 && PaymentList.Any(c => c.IsReversed))
                                {
                                    //退款冲销过
                                    _unitOfWorkManage.RollbackTran();
                                    rmrs.ErrorMsg = $"销售订单{PrePayment.SourceBillNo}的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，不能反审,只能【取消】作废。";
                                    rmrs.Succeeded = false;
                                    return rmrs;
                                }
                                else
                                {
                                    tb_FM_PaymentRecord Payment = PaymentList[0];
                                    if (Payment.PaymentStatus == (int)PaymentStatus.草稿 || Payment.PaymentStatus == (int)PaymentStatus.待审核)
                                    {
                                        var PaymentCounter = await _unitOfWorkManage.GetDbClient().DeleteNav(Payment)
                                            .Include(c => c.tb_FM_PaymentRecordDetails)
                                            .ExecuteCommandAsync();
                                        if (PaymentCounter)
                                        {
                                            await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                                        }
                                    }
                                    else
                                    {
                                        _unitOfWorkManage.RollbackTran();
                                        rmrs.ErrorMsg = $"对应的预收款单{PrePayment.PreRPNO}状态为【{(PrePaymentStatus)PrePayment.PrePaymentStatus}】，反审失败\r\n" +
                                            $"需将预收款单【退款】，对收款单{Payment.PaymentNo}进行冲销处理\r\n" +
                                            $"当前订单【作废】后，重新录入正确的销售订单。";
                                        rmrs.Succeeded = false;
                                        return rmrs;
                                    }
                                }

                            }
                            //else
                            //{
                            //    //预收单审核了。应该有收款单。正常不会到这步
                            //    await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                            //}
                            #endregion

                        }
                    }


                    #endregion
                }
                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                entity.ApprovalOpinions += "【被反审】";
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
        ///  没有完成！！！！！！！！！！！！！
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

        AuthorizeController authorizeController = null;

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
                if (authorizeController == null)
                {
                    authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                }

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
                entity.PayStatus = saleorder.PayStatus;
                entity.Paytype_ID = saleorder.Paytype_ID;
                List<string> tipsMsg = new List<string>();
                List<tb_SaleOutDetail> details = mapper.Map<List<tb_SaleOutDetail>>(saleorder.tb_SaleOrderDetails);
                List<tb_SaleOutDetail> NewDetails = new List<tb_SaleOutDetail>();


                //多行相同产品时 可能还在仔细优化核对
                for (global::System.Int32 i = 0; i < details.Count; i++)
                {
                    View_ProdDetail obj = null;
                    var aa = details.Select(c => c.ProdDetailID).ToList().GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                    if (aa.Count > 0 && details[i].SaleOrderDetail_ID > 0)
                    {
                        obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                        #region 产品ID可能大于1行，共用料号情况
                        tb_SaleOrderDetail item = saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == details[i].ProdDetailID
                        && c.Location_ID == details[i].Location_ID
                        && c.SaleOrderDetail_ID == details[i].SaleOrderDetail_ID);
                        details[i].Cost = item.Cost;
                        details[i].CustomizedCost = item.CustomizedCost;
                        //这时有一种情况就是订单时没有成本。没有产品。出库前有类似采购入库确定的成本
                        if (details[i].Cost == 0)
                        {
                            obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                            if (obj == null)
                            {
                                obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            }
                            tipsMsg.Add($"销售订单{saleorder.SOrderNo}，{obj.CNName + obj.Specifications}已出库数为{item.TotalDeliveredQty}，可出库数为{details[i].Quantity}，当前行数据忽略！");
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
                            obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
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
                            if (obj == null)
                            {
                                obj = BizCacheHelper.Instance.GetEntity<View_ProdDetail>(details[i].ProdDetailID);
                            }
                            tipsMsg.Add($"当前订单的SKU:{obj.SKU}{obj.CNName}已出库数量为{details[i].Quantity}，当前行数据将不会加载到明细！");
                        }
                        #endregion
                    }

                }

                if (NewDetails.Count == 0)
                {
                    tipsMsg.Add($"订单:{entity.SaleOrderNo}已全部出库，请检查是否正在重复出库！");
                }

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

                entity.TotalQty = NewDetails.Sum(c => c.Quantity);

                //默认认为 订单中的运费收入 就是实际发货的运费成本， 可以手动修改覆盖
                if (entity.FreightIncome > 0)
                {
                    entity.FreightCost = entity.FreightIncome;
                    //根据系统设置中的分摊规则来分配运费收入到明细。

                    if (_appContext.SysConfig.FreightAllocationRules == (int)FreightAllocationRules.产品数量占比)
                    {
                        // 单个产品分摊运费 = 整单运费 ×（该产品数量 ÷ 总产品数量） 
                        foreach (var item in NewDetails)
                        {
                            item.AllocatedFreightIncome = entity.FreightIncome * (item.Quantity.ToDecimal() / saleorder.TotalQty.ToDecimal());
                            item.AllocatedFreightIncome = item.AllocatedFreightIncome.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                            item.FreightAllocationRules = _appContext.SysConfig.FreightAllocationRules;
                        }
                    }
                }


                entity.tb_SaleOutDetails = NewDetails;
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

                entity.TotalCost = NewDetails.Sum(c => (c.Cost + c.CustomizedCost) * c.Quantity);
                entity.TotalCost = entity.TotalCost + entity.FreightCost;
                entity.TotalTaxAmount = NewDetails.Sum(c => c.SubtotalTaxAmount);
                entity.TotalTaxAmount = entity.TotalTaxAmount.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());

                entity.TotalAmount = NewDetails.Sum(c => c.TransactionPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.FreightIncome;


                BusinessHelper.Instance.InitEntity(entity);
                //保存到数据库

            }
            return entity;
        }


        /// <summary>
        /// 销售转换为采购订单
        /// </summary>
        /// <param name="saleorder"></param>
        public tb_PurOrder SaleOrderToPurOrder(tb_SaleOrder saleorder)
        {
            tb_PurOrder entity = new tb_PurOrder();
            //转单
            if (saleorder != null)
            {
                if (authorizeController == null)
                {
                    authorizeController = _appContext.GetRequiredService<AuthorizeController>();
                }

                entity = mapper.Map<tb_PurOrder>(saleorder);
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
                entity.CustomerVendor_ID = 0;//销售订单是客户，这里是厂商。默认为空。验证时要填写
                entity.PayStatus = 0;
                entity.Paytype_ID = null;
                List<string> tipsMsg = new List<string>();
                List<tb_PurOrderDetail> details = mapper.Map<List<tb_PurOrderDetail>>(saleorder.tb_SaleOrderDetails);
                entity.TotalQty = details.Sum(c => c.Quantity);

                entity.tb_PurOrderDetails = details;
                entity.PurDate = System.DateTime.Now;
                entity.PreDeliveryDate = saleorder.PreDeliveryDate;
                BusinessHelper.Instance.InitEntity(entity);
                if (entity.SOrder_ID.HasValue && entity.SOrder_ID > 0)
                {
                    entity.SOrderNo = saleorder.SOrderNo;//销售订单号.
                    entity.Notes = saleorder.Notes;
                    entity.tb_saleorder = saleorder;
                    entity.ShipCost = saleorder.FreightIncome;
                }

                //销售订单单号 转为 采购订单单号
                entity.PurOrderNo = saleorder.SOrderNo.Replace("SO", "PO");
                //entity.PurOrderNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.采购订单);

                entity.tb_saleorder = saleorder;
                entity.TotalTaxAmount = details.Sum(c => c.TaxAmount);
                entity.TotalTaxAmount = entity.TotalTaxAmount.ToRoundDecimalPlaces(authorizeController.GetMoneyDataPrecision());
                entity.TotalAmount = details.Sum(c => c.UnitPrice * c.Quantity);
                entity.TotalAmount = entity.TotalAmount + entity.ShipCost;
                BusinessHelper.Instance.InitEntity(entity);
            }
            return entity;
        }




        public async Task<ReturnResults<tb_SaleOrder>> CancelOrder(tb_SaleOrder ObjectEntity)
        {
            ReturnResults<tb_SaleOrder> rmrs = new ReturnResults<tb_SaleOrder>();
            tb_SaleOrder entity = ObjectEntity as tb_SaleOrder;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                #region  预收款单处理
                tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment> ctrpay = _appContext.GetRequiredService<tb_FM_PreReceivedPaymentController<tb_FM_PreReceivedPayment>>();
                var PrePayment = await ctrpay.IsExistEntityAsync(p => p.SourceBillId == entity.SOrder_ID && p.SourceBizType == (int)BizType.销售订单);
                if (PrePayment != null)
                {
                    if (PrePayment.PrePaymentStatus == (int)PrePaymentStatus.草稿 ||
                        PrePayment.PrePaymentStatus == (int)PrePaymentStatus.待审核 ||
                        PrePayment.PrePaymentStatus == (int)PrePaymentStatus.已生效)
                    {
                        //没有付款记录的，直接删除关闭
                        await _unitOfWorkManage.GetDbClient().Deleteable(PrePayment).ExecuteCommandAsync();
                    }
                    else
                    {
                        //订单取消后，预收款，可以退款可以下一个订单，应收来处理。由财务决定。
                        //这里仅提醒，订金已支付
                    }

                }

                #endregion

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

                entity.DataStatus = (int)DataStatus.作废;
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
