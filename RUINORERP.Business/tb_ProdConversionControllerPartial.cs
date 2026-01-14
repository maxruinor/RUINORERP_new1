using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using RUINOR.Core;
using RUINORERP.Business.CommService;
using RUINORERP.Business.EntityLoadService;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace RUINORERP.Business
{
    public partial class tb_ProdConversionController<T>
    {

        /// <summary>
        /// 库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_ProdConversion entity = ObjectEntity as tb_ProdConversion;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新库存  from 8 to   from-qty=to+qty;

                List<tb_Inventory> invList = new List<tb_Inventory>();
                // 创建库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                foreach (var child in entity.tb_ProdConversionDetails)
                {
                    int TransferQty = child.ConversionQty;
                    #region 来源库存的更新 ，
                    tb_Inventory invForm = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID_from && i.Location_ID == entity.Location_ID);
                    if (invForm == null)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"来源产品必须通过【采购入库】，【期初盘点】或【缴库记录】产生过库存记录。转换失败。" +
                            $"\r\n可以尝用【期初盘点】数量为零的方式或开启【手工录入】初始成本。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
                    else
                    {
                        BusinessHelper.Instance.EditEntity(invForm);
                    }

                    if (TransferQty > 0)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (invForm.Quantity - TransferQty) < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"来源产品库存为：{invForm.Quantity}，拟转换数量为：{TransferQty}\r\n 系统设置不允许负库存， 请检查要转换出库数量的情况。";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        invForm.LatestOutboundTime = System.DateTime.Now;
                    }
                    invForm.Quantity = invForm.Quantity - TransferQty;
                    if (TransferQty < 0)
                    {
                        invForm.LatestStorageTime = System.DateTime.Now;
                    }
                    #endregion

                    #region  目标库存更新 ，
                    tb_Inventory invTo = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID_to && i.Location_ID == entity.Location_ID);
                    if (invTo == null)
                    {
                        if (child.TargetInitCost == 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"来源产品必须通过【采购入库】，【期初盘点】或【缴库记录】产生过库存记录。转换失败。" +
                                 $"\r\n可以尝用【期初盘点】数量为零的方式或开启【手工录入】初始成本。";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        invTo = new tb_Inventory();
                        invTo.ProdDetailID = child.ProdDetailID_to;
                        invTo.Location_ID = entity.Location_ID;
                        invTo.Quantity = 0;
                        invTo.Inv_Cost = child.TargetInitCost;
                        invForm.Notes = $"由转换单{entity.ConversionNo}初始化";
                        invTo.InitInventory =0;
                        BusinessHelper.Instance.InitEntity(invTo);
                    }
                    else
                    {
                        if (invTo.Inv_Cost == 0 && child.TargetInitCost > 0)
                        {
                            invTo.Inv_Cost = child.TargetInitCost;
                        }
                        BusinessHelper.Instance.EditEntity(invTo);
                    }

                    if (TransferQty > 0)
                    {
                        invTo.LatestStorageTime = System.DateTime.Now;
                    }
                    if (TransferQty < 0)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (invTo.Quantity + TransferQty) < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"目标产品库存为：{invTo.Quantity}，拟转换数量为：{TransferQty}\r\n 系统设置不允许负库存， 请检查要转换出库数量的情况。";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        invForm.LatestOutboundTime = System.DateTime.Now;
                    }
                    invTo.Quantity = invTo.Quantity + TransferQty;
                    #endregion

                    invList.Add(invForm);
                    invList.Add(invTo);
                    
                    // 实时获取当前库存成本
                    decimal realtimeCostFrom = invForm.Inv_Cost;
                    decimal realtimeCostTo = invTo.Inv_Cost;
                    
                    // 转换单明细没有成本相关属性，不需要更新
                    
                    // 创建源产品的库存流水记录（减少库存）
                    tb_InventoryTransaction transactionFrom = new tb_InventoryTransaction();
                    transactionFrom.ProdDetailID = invForm.ProdDetailID;
                    transactionFrom.Location_ID = invForm.Location_ID;
                    transactionFrom.BizType = (int)BizType.产品转换单;
                    transactionFrom.ReferenceId = entity.ConversionID;
                    transactionFrom.ReferenceNo = entity.ConversionNo;
                    transactionFrom.QuantityChange = -TransferQty; // 源产品减少库存
                    transactionFrom.AfterQuantity = invForm.Quantity;
                    transactionFrom.UnitCost = realtimeCostFrom; // 使用实时成本
                    transactionFrom.TransactionTime = DateTime.Now;
                    transactionFrom.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionFrom.Notes = $"转换单审核：{entity.ConversionNo}，源产品出库：{invForm.tb_proddetail?.tb_prod?.CNName}，转换为：{invTo.tb_proddetail?.tb_prod?.CNName}";
                    transactionList.Add(transactionFrom);
                    
                    // 创建目标产品的库存流水记录（增加库存）
                    tb_InventoryTransaction transactionTo = new tb_InventoryTransaction();
                    transactionTo.ProdDetailID = invTo.ProdDetailID;
                    transactionTo.Location_ID = invTo.Location_ID;
                    transactionTo.BizType = (int)BizType.产品转换单;
                    transactionTo.ReferenceId = entity.ConversionID;
                    transactionTo.ReferenceNo = entity.ConversionNo;
                    transactionTo.QuantityChange = TransferQty; // 目标产品增加库存
                    transactionTo.AfterQuantity = invTo.Quantity;
                    transactionTo.UnitCost = realtimeCostTo; // 使用实时成本
                    transactionTo.TransactionTime = DateTime.Now;
                    transactionTo.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionTo.Notes = $"转换单审核：{entity.ConversionNo}，目标产品入库：{invTo.tb_proddetail?.tb_prod?.CNName}，来自：{invForm.tb_proddetail?.tb_prod?.CNName}";
                    transactionList.Add(transactionTo);
                }
                
                // 记录库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactions(transactionList);
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (InvMainCounter.ToInt() == 0)
                {
                    _logger.Debug($"{entity.ConversionNo}更新库存结果为0行，请检查数据！");
                }



                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                            .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                            .ExecuteCommandHasChangeAsync();

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
                rmrs.Succeeded = false;
                return rmrs;
            }
        }


        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_ProdConversion> entitys = new List<tb_ProdConversion>();
            entitys = NeedCloseCaseList as List<tb_ProdConversion>;


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

                    entitys[m].DataStatus = (int)DataStatus.完结;
                    BusinessHelper.Instance.EditEntity(entitys[m]);
                    //后面是不是要做一个审核历史记录表？

                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdConversion>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.Modified_by, it.Modified_at }).ExecuteCommandAsync();
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
        /// 反审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_ProdConversion entity = ObjectEntity as tb_ProdConversion;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    rmrs.ErrorMsg = "只能反审核已确认,并且有审核结果的订单 ";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }
                //反审核时 更新库存  from 8 to   from-qty=to+qty;===》  from+qty=to-qty
                List<tb_Inventory> invList = new List<tb_Inventory>();

                //创建反向库存流水记录列表
                List<tb_InventoryTransaction> transactionList = new List<tb_InventoryTransaction>();
                
                //将from的 数量 减少，to 数量增加  但是如果为负数。则实际相反
                foreach (var child in entity.tb_ProdConversionDetails)
                {
                    int TransferQty = child.ConversionQty;
                    #region 来源库存的更新 ，
                    tb_Inventory invForm = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID_from && i.Location_ID == entity.Location_ID);
                    if (invForm == null)
                    {
                        invForm = new tb_Inventory();
                        invForm.ProdDetailID = child.ProdDetailID_from;
                        invForm.Location_ID = entity.Location_ID;
                        invForm.Quantity = 0;
                        invForm.InitInventory=0;
                        BusinessHelper.Instance.InitEntity(invForm);
                    }
                    else
                    {
                        BusinessHelper.Instance.EditEntity(invForm);
                    }

                    if (TransferQty > 0)
                    {
                        invForm.LatestOutboundTime = System.DateTime.Now;
                    }
                    if (TransferQty < 0)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (invForm.Quantity + TransferQty) < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"反审转换单时，来源产品库存为：{invForm.Quantity}，拟转换数量为：{TransferQty}\r\n 系统设置不允许负库存， 请检查要转换出库数量的情况。";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        invForm.LatestStorageTime = System.DateTime.Now;
                    }
                    invForm.Quantity += TransferQty;

                    invList.Add(invForm);
                    #endregion

                    #region  目标库存更新 ，
                    tb_Inventory invTo = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID_to && i.Location_ID == entity.Location_ID);
                    if (invTo == null)
                    {
                        invTo = new tb_Inventory();
                        invTo.ProdDetailID = child.ProdDetailID_to;
                        invTo.Location_ID = entity.Location_ID;
                        invTo.Quantity = 0;
                        invTo.InitInventory=0;
                        BusinessHelper.Instance.InitEntity(invTo);
                    }
                    else
                    {
                        BusinessHelper.Instance.EditEntity(invTo);
                    }

                    if (TransferQty > 0)
                    {
                        if (!_appContext.SysConfig.CheckNegativeInventory && (invTo.Quantity - TransferQty) < 0)
                        {
                            _unitOfWorkManage.RollbackTran();
                            rmrs.ErrorMsg = $"反审转换单时，目标产品库存为：{invTo.Quantity}，拟转换数量为：{TransferQty}\r\n 系统设置不允许负库存， 请检查要转换出库数量的情况。";
                            rmrs.Succeeded = false;
                            return rmrs;
                        }
                        invTo.LatestStorageTime = System.DateTime.Now;
                    }
                    invTo.Quantity -= TransferQty;
                    if (TransferQty < 0)
                    {
                        invTo.LatestOutboundTime = System.DateTime.Now;
                    }
                    #endregion
                    invList.Add(invTo);
                    
                    // 实时获取当前库存成本
                    decimal realtimeCostFrom = invForm.Inv_Cost;
                    decimal realtimeCostTo = invTo.Inv_Cost;
                    
                    // 创建源产品的反向库存流水记录（反审核时源产品增加库存）
                    tb_InventoryTransaction transactionFrom = new tb_InventoryTransaction();
                    transactionFrom.ProdDetailID = invForm.ProdDetailID;
                    transactionFrom.Location_ID = invForm.Location_ID;
                    transactionFrom.BizType = (int)BizType.产品转换单;
                    transactionFrom.ReferenceId = entity.ConversionID;
                    transactionFrom.ReferenceNo = entity.ConversionNo;
                    transactionFrom.QuantityChange = TransferQty; // 反审核时源产品增加库存
                    transactionFrom.AfterQuantity = invForm.Quantity;
                    transactionFrom.UnitCost = realtimeCostFrom; // 使用实时成本
                    transactionFrom.TransactionTime = DateTime.Now;
                    transactionFrom.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionFrom.Notes = $"转换单反审核：{entity.ConversionNo}，源产品反向调整：{invForm.tb_proddetail?.tb_prod?.CNName}，恢复数量：{TransferQty}";
                    transactionList.Add(transactionFrom);
                    
                    // 创建目标产品的反向库存流水记录（反审核时目标产品减少库存）
                    tb_InventoryTransaction transactionTo = new tb_InventoryTransaction();
                    transactionTo.ProdDetailID = invTo.ProdDetailID;
                    transactionTo.Location_ID = invTo.Location_ID;
                    transactionTo.BizType = (int)BizType.产品转换单;
                    transactionTo.ReferenceId = entity.ConversionID;
                    transactionTo.ReferenceNo = entity.ConversionNo;
                    transactionTo.QuantityChange = -TransferQty; // 反审核时目标产品减少库存
                    transactionTo.AfterQuantity = invTo.Quantity;
                    transactionTo.UnitCost = realtimeCostTo; // 使用实时成本
                    transactionTo.TransactionTime = DateTime.Now;
                    transactionTo.OperatorId = _appContext.CurUserInfo.UserInfo.User_ID;
                    transactionTo.Notes = $"转换单反审核：{entity.ConversionNo}，目标产品反向调整：{invTo.tb_proddetail?.tb_prod?.CNName}，减少数量：{TransferQty}";
                    transactionList.Add(transactionTo);
                }
                
                // 记录反向库存流水
                tb_InventoryTransactionController<tb_InventoryTransaction> tranController = _appContext.GetRequiredService<tb_InventoryTransactionController<tb_InventoryTransaction>>();
                await tranController.BatchRecordTransactions(transactionList);
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter == 0)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新数据为0，更新失败！");
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                             .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                             .ExecuteCommandHasChangeAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
               
                rmrs.ErrorMsg = BizMapperService.EntityMappingHelper.GetBizType(typeof(tb_ProdConversion)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
            }
            return rmrs;
        }

        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {
            List<tb_ProdConversion> list = await _appContext.Db.CopyNew().Queryable<tb_ProdConversion>().Where(m => m.ConversionID == MainID)
                              .Includes(a => a.tb_ProdConversionDetails)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }

    }
}
