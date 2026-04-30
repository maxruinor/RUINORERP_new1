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


using RUINORERP.Global;
using RUINORERP.Model.Base;
using SqlSugar;
using RUINORERP.Business.Security;
using RUINORERP.Common.Extensions;
using System.Linq;
using AutoMapper;
using System.Numerics;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

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
                await _unitOfWorkManage.BeginTranAsync();

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
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
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
                await _unitOfWorkManage.CommitTranAsync();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {

                await _unitOfWorkManage.RollbackTranAsync();
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
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
                await _unitOfWorkManage.BeginTranAsync();

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
                        entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
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
                await _unitOfWorkManage.CommitTranAsync();

                return true;
            }
            catch (Exception ex)
            {

                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex);
                return false;
            }

        }

        /// <summary>
        /// 批量结案   
        /// 计划单结案，则相关的需求单，制令单， 都会结案 
        /// 添加判断：检查制令单下是否存在已发料但未缴库的情况
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_ProductionPlan> entitys = new List<tb_ProductionPlan>();
            entitys = NeedCloseCaseList as List<tb_ProductionPlan>;
            ReturnResults<bool> rs = new ReturnResults<bool>();
            
            long[] ids = entitys.Select(c => c.PPID).ToArray();

            // 【事务外】预加载计划单及其关联数据（包括需求分析、制令单）
            var Plans = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>()
                      .Includes(c => c.tb_ProductionDemands, d => d.tb_ManufacturingOrders)
                      .Where(d => ids.Contains(d.PPID)).ToListAsync();
            Plans.ForEach(c => c.CloseCaseOpinions = entitys[0].CloseCaseOpinions);

            List<tb_ProductionDemand> needupdateProductionDemands = new List<tb_ProductionDemand>();
            List<tb_ManufacturingOrder> needupdateManufacturingOrders = new List<tb_ManufacturingOrder>();

            #region 【事务外】结案前检查
            // 检查每个计划单下的制令单是否存在已发料但未缴库的情况
            foreach (var plan in Plans)
            {
                // 判断能结案的是确认审核过的
                if (plan.DataStatus != (int)DataStatus.确认 || !plan.ApprovalResults.HasValue)
                {
                    continue;
                }

                if (plan.tb_ProductionDemands != null)
                {
                    foreach (var demand in plan.tb_ProductionDemands)
                    {
                        if (demand.tb_ManufacturingOrders != null)
                        {
                            foreach (var mo in demand.tb_ManufacturingOrders)
                            {
                                // 加载制令单的完整数据（包括发料单和缴库单）
                                if (mo.DataStatus == (int)DataStatus.确认 && mo.ApprovalResults.HasValue && mo.ApprovalResults.Value)
                                {
                                    // 【事务外】查询制令单完整数据
                                    var moFullData = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                                        .Includes(m => m.tb_MaterialRequisitions, mr => mr.tb_MaterialRequisitionDetails)
                                        .Includes(m => m.tb_FinishedGoodsInvs, fg => fg.tb_FinishedGoodsInvDetails)
                                        .Where(m => m.MOID == mo.MOID)
                                        .FirstAsync();

                                    if (moFullData != null)
                                    {
                                        // 检查是否存在已审核的发料单
                                        if (moFullData.tb_MaterialRequisitions != null && moFullData.tb_MaterialRequisitions.Any())
                                        {
                                            var approvedMaterialRequisitions = moFullData.tb_MaterialRequisitions
                                                .Where(mr => mr.DataStatus == (int)DataStatus.确认 
                                                    && mr.ApprovalStatus.HasValue 
                                                    && mr.ApprovalStatus.Value == (int)ApprovalStatus.审核通过)
                                                .ToList();

                                            if (approvedMaterialRequisitions.Any())
                                            {
                                                // 计算已发料总数
                                                decimal totalMaterialSent = approvedMaterialRequisitions
                                                    .SelectMany(mr => mr.tb_MaterialRequisitionDetails)
                                                    .Sum(mrd => mrd.ActualSentQty);

                                                // 计算已缴库总数
                                                decimal totalFinishedGoods = 0;
                                                if (moFullData.tb_FinishedGoodsInvs != null)
                                                {
                                                    totalFinishedGoods = moFullData.tb_FinishedGoodsInvs
                                                        .Where(fg => fg.DataStatus == (int)DataStatus.确认 
                                                            && fg.ApprovalStatus.HasValue 
                                                            && fg.ApprovalStatus.Value == (int)ApprovalStatus.审核通过)
                                                        .SelectMany(fg => fg.tb_FinishedGoodsInvDetails)
                                                        .Sum(fgd => fgd.Qty);
                                                }

                                                // 如果存在发料但未缴库的情况，记录警告信息
                                                if (totalMaterialSent > 0 && totalFinishedGoods == 0)
                                                {
                                                    _logger.LogWarning($"计划单[{plan.PPNo}]下的制令单[{mo.MONO}]存在已发料({totalMaterialSent})但未缴库的情况，强制结案");
                                                    // 将警告信息添加到结案意见中
                                                    mo.CloseCaseOpinions = $"【警告】存在已发料({totalMaterialSent})但未缴库的情况，强制结案。" + (mo.CloseCaseOpinions ?? "");
                                                }
                                                else if (totalMaterialSent > totalFinishedGoods)
                                                {
                                                    _logger.LogWarning($"计划单[{plan.PPNo}]下的制令单[{mo.MONO}]存在已发料({totalMaterialSent})大于已缴库({totalFinishedGoods})的情况，强制结案");
                                                    // 将警告信息添加到结案意见中
                                                    mo.CloseCaseOpinions = $"【警告】已发料({totalMaterialSent})大于已缴库({totalFinishedGoods})，强制结案。" + (mo.CloseCaseOpinions ?? "");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            try
            {
                // 【事务开始】只包含更新操作，最小化事务区间
                await _unitOfWorkManage.BeginTranAsync();

                #region 结案
                //更新拟销售量  减少
                for (int m = 0; m < Plans.Count; m++)
                {
                    var plan = Plans[m];
                    //判断 能结案的 是确认审核过的。
                    if (plan.DataStatus != (int)DataStatus.确认 || !plan.ApprovalResults.HasValue)
                    {
                        //return false;
                        continue;
                    }

                    if (plan.tb_ProductionDemands != null)
                    {
                        plan.tb_ProductionDemands.ForEach(c => c.DataStatus = (int)DataStatus.完结);
                        plan.tb_ProductionDemands.ForEach(c => c.ApprovalOpinions += entitys[0].CloseCaseOpinions);

                        needupdateProductionDemands.AddRange(plan.tb_ProductionDemands);


                        plan.tb_ProductionDemands.ForEach(c =>
                        {
                            if (c.tb_ManufacturingOrders != null)
                            {
                                c.tb_ManufacturingOrders.ForEach(d => 
                                {
                                    d.CloseCaseOpinions = entitys[0].CloseCaseOpinions;
                                    d.DataStatus = (int)DataStatus.完结;
                                });
                                needupdateManufacturingOrders.AddRange(c.tb_ManufacturingOrders);
                            }

                        });
                    }

                    //这部分是否能提出到上一级公共部分？
                    plan.DataStatus = (int)DataStatus.完结;
                }

                var MORows = await _unitOfWorkManage.GetDbClient().Updateable(needupdateManufacturingOrders).UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions }).ExecuteCommandAsync();

                var DemandsaffectedRows = await _unitOfWorkManage.GetDbClient().Updateable(needupdateProductionDemands).UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions }).ExecuteCommandAsync();

                var PlansaffectedRows = await _unitOfWorkManage.GetDbClient().Updateable(Plans).UpdateColumns(it => new { it.DataStatus, it.CloseCaseOpinions }).ExecuteCommandAsync();

                #endregion
                // 注意信息的完整性
                await _unitOfWorkManage.CommitTranAsync();
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
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
                await _unitOfWorkManage.BeginTranAsync();

                tb_InventoryController<tb_Inventory> ctrinv = _appContext.GetRequiredService<tb_InventoryController<tb_Inventory>>();
                //更新拟销售量减少


                //判断是否能反审?
                if (entity.tb_ProductionDemands != null
                    && (entity.tb_ProductionDemands.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_ProductionDemands.Any(c => c.ApprovalStatus == (int)ApprovalStatus.审核通过)))
                {

                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的需求单，不能反审核  ";
                    await _unitOfWorkManage.RollbackTranAsync();
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                //判断是否能反审?
                if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
                {
                    rmrs.ErrorMsg = "计划单非确认或非完结，不能反审核  ";
                    await _unitOfWorkManage.RollbackTranAsync();
                    rmrs.Succeeded = false;
                    return rmrs;
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
                await _unitOfWorkManage.CommitTranAsync();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
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
