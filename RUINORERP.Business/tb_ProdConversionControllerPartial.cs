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
using System.Collections;

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
                        invTo.InitInventory = (int)invTo.Quantity;
                        BusinessHelper.Instance.InitEntity(invTo);
                    }
                    else
                    {
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

                }
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var InvMainCounter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (InvMainCounter.ToInt() != invList.Count)
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("库存更新失败！");
                }




                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_ProdConversion>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdConversion>(entity).ExecuteCommandAsync();

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
                        invForm.InitInventory += (int)invForm.Quantity;
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
                        invTo.InitInventory -= (int)invTo.Quantity;
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
                    invTo.Quantity -=  TransferQty;
                    if (TransferQty < 0)
                    {
                        invTo.LatestOutboundTime = System.DateTime.Now;
                    }
                    #endregion
                    invList.Add(invTo);
                }
                DbHelper<tb_Inventory> dbHelper = _appContext.GetRequiredService<DbHelper<tb_Inventory>>();
                var Counter = await dbHelper.BaseDefaultAddElseUpdateAsync(invList);
                if (Counter != invList.Count)
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
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdConversion>(entity).ExecuteCommandAsync();

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
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_ProdConversion)).ToString() + "事务回滚=>" + ex.Message;
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
