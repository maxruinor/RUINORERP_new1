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
using AutoMapper;

namespace RUINORERP.Business
{
    public partial class tb_ProductionPlanController<T>
    {


        /// <summary>
        /// 生产计划审核只是更新状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_ProductionPlan entity = ObjectEntity as tb_ProductionPlan;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity == null)
                {
                    return rmrs;
                }



                //更新在制数量 应该是在生产通知单时更新的，这里暂时不更新
                /*
                 * 
                 * 在 ERP 系统中，生产计划单的审核会影响多方面的数据，包括但不限于以下这些：
生产相关数据：
计划状态：生产计划单的状态会从待审核变为已审核，明确其当前阶段。
排程数据：审核通过后可能会影响生产排程，确定具体的生产时间和顺序。
物料数据：
物料需求：根据生产计划单确定的产品和数量，影响后续物料采购或调拨的需求数量计算。
物料库存：如果有配套物料，可能会锁定相应数量的物料库存。
成本数据：
预算分配：影响生产成本预算的具体分配。
人员数据：
人员安排：为相应生产任务安排合适的人力。
绩效数据：
绩效统计：与生产相关的绩效指标可能会受到影响，如生产任务完成率等。
订单数据：
关联订单进度：如果是为了满足特定订单的生产计划，会影响订单的生产进度更新。

 */

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions

                }).ExecuteCommandAsync();
                //await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entity).ExecuteCommandAsync();
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, "事务回滚" + ex.Message);
                rmrs.Succeeded = false;
                return rmrs;
            }

        }

        /// <summary>
        /// 库存中的拟销售量增加，同时检查数量和金额，总数量和总金额不能小于明细小计的和
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async virtual Task<bool> BatchApprovalAsync(List<tb_ProductionPlan> entitys, ApprovalEntity approvalEntity)
        {
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

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
                        //这部分是否能提出到上一级公共部分？
                        entity.DataStatus = (int)DataStatus.确认;
                        entity.ApprovalOpinions = approvalEntity.ApprovalOpinions;
                        //后面已经修改为
                        entity.ApprovalResults = approvalEntity.ApprovalResults;
                        entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                        BusinessHelper.Instance.ApproverEntity(entity);
                        //只更新指定列
                        var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entity).UpdateColumns(it => new
                        {
                            it.DataStatus,
                            it.ApprovalResults,
                            it.ApprovalStatus,
                            it.Approver_at,
                            it.Approver_by,
                            it.ApprovalOpinions

                        }).ExecuteCommandAsync();

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
                _logger.Error(ex, "事务回滚" + ex.Message);
                return false;
            }

        }

        /// <summary>
        /// 批量结案  销售订单标记结案，数据状态为8,可以更新销售订单付款状态， 如果还没有出库。但是结案的订单时。修正拟出库数量。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_ProductionPlan> entitys = new List<tb_ProductionPlan>();
            entitys = NeedCloseCaseList as List<tb_ProductionPlan>;

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
                    for (int c = 0; c < entitys[m].tb_ProductionPlanDetails.Count; c++)
                    {


                    }
                    
                    //这部分是否能提出到上一级公共部分？
                    entitys[m].DataStatus = (int)DataStatus.完结;
                    BusinessHelper.Instance.EditEntity(entitys[m]);

                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.Modified_by, it.Modified_at, it.Notes }).ExecuteCommandAsync();
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
        /// 反审核，生产计划反审时，不能有审核过的需求单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            tb_ProductionPlan entity = ObjectEntity as tb_ProductionPlan;
            ReturnResults<T> rmrs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                //判断是否能反审?
                if (entity.tb_ProductionDemands != null
                    && (entity.tb_ProductionDemands.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_ProductionDemands.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的需求单，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    rmrs.ErrorMsg = "计划单非确认或非完结，不能反审核  ";
                    _unitOfWorkManage.RollbackTran();
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列
                // var result = _unitOfWorkManage.GetDbClient().Updateable<tb_Stocktake>(entity).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommand();
                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entity).ExecuteCommandAsync();


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rmrs.ErrorMsg = ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }





        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {

            List<tb_ProductionPlan> list = await _appContext.Db.CopyNew().Queryable<tb_ProductionPlan>().Where(m => m.PPID == MainID)
                             .Includes(a => a.tb_employee)
                             .Includes(a => a.tb_department)
                             .Includes(a => a.tb_projectgroup)
                            .Includes(a => a.tb_saleorder)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProductionPlanDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }







    }
}
