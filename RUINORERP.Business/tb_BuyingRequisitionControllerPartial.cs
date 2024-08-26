
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
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Extensions;

namespace RUINORERP.Business
{
    public partial class tb_BuyingRequisitionController<T> : BaseController<T> where T : class
    {

        /// <summary>
        /// 结案
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<ReturnResults<bool>> BatchCloseCaseAsync(List<tb_BuyingRequisition> entitys)
        {
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
                        foreach (var child in entity.tb_BuyingRequisitionDetails)
                        {
                            //明细已交？写回。?
                        }

                        entity.DataStatus = (int)DataStatus.完结;
                        entity.CloseCaseOpinions = "结案了";
                        BusinessHelper.Instance.EditEntity(entity);
                        //只更新指定列
                        var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisition>(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
                            it.CloseCaseOpinions,
                            it.Modified_by,
                            it.Modified_at
                        }).ExecuteCommandAsync();

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
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                rs.ErrorMsg = ex.Message;

                rs.Succeeded = false;
                return rs;
            }

        }


        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="approvalEntity"></param>
        /// <returns></returns>
        /// 
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_BuyingRequisition entity = ObjectEntity as tb_BuyingRequisition;
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;

                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisition>(entity).ExecuteCommandAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                rmrs.ErrorMsg = ex.Message;
                if (AuthorizeController.GetShowDebugInfoAuthorization(_appContext))
                {
                    _logger.Error("事务回滚" + ex.Message);
                }
                return rmrs;
            }

        }

        /// <summary>
        /// 反审
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_BuyingRequisition entity = ObjectEntity as tb_BuyingRequisition;
            try
            {
                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    return rs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();

                foreach (var child in entity.tb_BuyingRequisitionDetails)
                {

                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                await _unitOfWorkManage.GetDbClient().Updateable<tb_BuyingRequisition>(entity).ExecuteCommandAsync();
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
            List<tb_BuyingRequisition> list = await _appContext.Db.CopyNew().Queryable<tb_BuyingRequisition>().Where(m => m.PuRequisition_ID == MainID)
                             .Includes(a => a.tb_department)
                            .Includes(a => a.tb_employee)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_BuyingRequisitionDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }



    }

}



