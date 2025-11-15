
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/22/2023 17:06:12
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
using SqlSugar;
using RUINORERP.Global;
using RUINOR.Core;
using RUINORERP.Common.Helper;

using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using AutoMapper;
using RUINORERP.Business.StatusManagerService;
using IMapper = AutoMapper.IMapper;
using System.Text;
using System.Windows.Forms;
using System.Runtime.ConstrainedExecution;


namespace RUINORERP.Business
{
    /// <summary>
    /// 客户厂商表
    /// </summary>
    public partial class tb_BOM_SController<T>
    {

        /// <summary>
        /// 原始思路，主表 单独处理  新增或修改
        /// 子表明细  分修改的，和 新增的两组
        /// 后暂时改为框架方法
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnMainSubResults<T>> SaveOrUpdateWithChild<C>(T model) where C : class
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
                tb_BOM_S entity = model as tb_BOM_S;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };


                //rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BOM_S>(entity as tb_BOM_S, new UpdateNavRootOptions() { IsInsertRoot = true })//IsInsertRoot=true表示不存在插入主表1
                //        .Include(b => b.tb_BOM_SDetails).ThenInclude(e => e.tb_BOM_SDetailSubstituteMaterials)
                //        .Include(m => m.tb_BOM_SDetailSecondaries)
                //        .ExecuteCommandAsync();

                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BOM_S>(entity as tb_BOM_S,
                     new UpdateNavRootOptions() { IsInsertRoot = true })
                        .Include(b => b.tb_BOM_SDetails, new UpdateNavOptions()
                        {
                            OneToManyInsertOrUpdate = true
                        }).ThenInclude(e => e.tb_BOM_SDetailSubstituteMaterials)
                        .Include(m => m.tb_BOM_SDetailSecondaries)
                        .ExecuteCommandAsync();



                //if (entity.MainID > 0)
                //{
                //    rs = await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_S>(entity as tb_BOM_S).ExecuteCommandAsync();
                //}
                //else
                //{
                //    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BOM_S>(entity as tb_BOM_S)
                //        .Include(m => m.tb_ProdDetails)
                //        .Include(m => m.tb_BOM_SDetails)
                //        .Include(m => m.tb_BOM_SDetailSecondaries)
                //        .Include(m => m.tb_ProduceGoodsRecommendDetails)
                //        .Include(m => m.tb_ProductionDemandDetails)
                //                .ExecuteCommandAsync();
                //}

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.BOM_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                rsms.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }


        public List<tb_BOM_S> QueryByBOMIDAsync(long bomid)
        {
            var tree = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>().ToTree(it => it.tb_BOM_SDetails, it => it.BOM_ID, bomid);
            return tree;
        }

        //删除指定配方主表和明细，同时清空产品明细中的对应关系，要使用事务处理
        public async Task<ReturnResults<T>> DeleteBOM_SDetail_Clear_ProdDetailMapping(T bom)
        {
            ReturnResults<T> rrs = new Business.ReturnResults<T>();
            try
            {
                tb_BOM_S _bom = bom as tb_BOM_S;
                _unitOfWorkManage.BeginTran();
                var affected = await _unitOfWorkManage.GetDbClient().
                Updateable<tb_ProdDetail>()
                .SetColumns(it => it.BOM_ID == null)
                .Where(it => it.BOM_ID == _bom.BOM_ID).ExecuteCommandHasChangeAsync();

                bool affectedDetail = await BaseDeleteByNavAsync(bom);
                _unitOfWorkManage.CommitTran();
                rrs.Succeeded = true;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                return rrs;
            }
            return rrs;
        }


        /// <summary>
        /// BOM审核，改变本身状态，修改对应母件的详情中的BOMID
        /// 同时修改对应母件的在其它配方中作为子件的成本价格。并且更新对应的总成本价格
        /// BOM明细成本不能为0，如果是新物料，后面会采购入库时覆盖这个物料成本变为最新成本
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_BOM_S entity = ObjectEntity as tb_BOM_S;

            try
            {

                tb_ProdDetailController<tb_ProdDetail> ctrDetail = _appContext.GetRequiredService<tb_ProdDetailController<tb_ProdDetail>>();
                tb_BOM_SController<tb_BOM_S> ctrinv = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
                if (entity == null)
                {
                    rmrs.ErrorMsg = "无效的BOM数据";
                    rmrs.Succeeded = false;
                    return rmrs;
                }

                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //更新产品表回写他的配方号
                //entity.tb_proddetail.DataStatus = (int)DataStatus.完结;
                //entity.tb_proddetail.MainID = entity.MainID;
                //await _appContext.Db.Updateable(entity.tb_proddetail).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();

                tb_ProdDetail detail = await ctrDetail.BaseQueryByIdAsync(entity.ProdDetailID);
                if (detail != null)
                {
                    detail.AcceptChanges();
                    detail.BOM_ID = entity.BOM_ID;
                    detail.DataStatus = (int)DataStatus.确认;
                    await ctrDetail.UpdateAsync(detail);
                }

                // 递归更新所有上级BOM的成本
                await UpdateParentBOMsAsync(entity.ProdDetailID, entity.SelfProductionAllCosts);
                /*

                //如果这个配方的母件，属性其它的配方子件。则同时要更新他对应的子件成本及配件总成本。
                var PreviouslevelBomList = await _appContext.Db.Queryable<tb_BOM_S>()
                                       .Includes(x => x.tb_BOM_SDetails)
                                          .WhereIF(1 == 1, x => x.tb_BOM_SDetails.Any(z => z.ProdDetailID == entity.ProdDetailID))
                                          .ToListAsync();
                if (PreviouslevelBomList != null && PreviouslevelBomList.Count > 0)
                {
                    //更新配方明细的成本后，再汇总到主表并更新
                    foreach (var bOM_S in PreviouslevelBomList)
                    {
                        foreach (var item in bOM_S.tb_BOM_SDetails)
                        {
                            if (item.ProdDetailID == entity.ProdDetailID)
                            {
                                //默认选择自产成本作为上一级的配方明细的单位成本
                                item.UnitCost = entity.SelfProductionAllCosts;
                                item.SubtotalUnitCost = item.UnitCost * item.UsedQty;
                            }
                        }
                        bOM_S.TotalMaterialCost = bOM_S.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                        bOM_S.OutProductionAllCosts = bOM_S.TotalMaterialCost + bOM_S.TotalOutManuCost + bOM_S.OutApportionedCost;
                        bOM_S.SelfProductionAllCosts = bOM_S.TotalMaterialCost + bOM_S.TotalSelfManuCost + bOM_S.SelfApportionedCost;
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(bOM_S.tb_BOM_SDetails)
                              .UpdateColumns(it => new { it.UnitCost, it.SubtotalUnitCost })
                             .ExecuteCommandHasChangeAsync();
                    }

                    await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_S>(PreviouslevelBomList)
                                    .UpdateColumns(it => new { it.TotalMaterialCost, it.OutProductionAllCosts, it.SelfProductionAllCosts })
                                    .ExecuteCommandHasChangeAsync();
                }
                */


                entity.DataStatus = (int)DataStatus.确认;
                entity.ApprovalResults = true;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.ReturnObject = entity as T;
                rmrs.Succeeded = true;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex, RUINORERP.Business.BizMapperService.EntityDataExtractor.ExtractDataContent(entity));
                return rmrs;
            }

        }

        /// <summary>
        /// 递归更新所有上级BOM的成本信息
        /// </summary>
        /// <param name="prodDetailId">当前BOM对应的产品详情ID</param>
        /// <param name="selfProductionCost">当前BOM的自产总成本</param>
        public async Task UpdateParentBOMsAsync(long prodDetailId, decimal selfProductionCost)
        {
            // 查询所有直接引用当前产品作为子件的上级BOM
            var parentBomList = await _appContext.Db.Queryable<tb_BOM_S>()
                                  .Includes(x => x.tb_BOM_SDetails)
                                  .Where(x => x.tb_BOM_SDetails.Any(z => z.ProdDetailID == prodDetailId))
                                  .ToListAsync();

            if (parentBomList == null || parentBomList.Count == 0)
            {
                return; // 没有上级BOM，递归终止
            }

            // 处理当前层级的所有BOM
            foreach (var parentBom in parentBomList)
            {
                bool hasChanges = false;

                // 更新子件成本
                foreach (var detail in parentBom.tb_BOM_SDetails)
                {
                    if (detail.ProdDetailID == prodDetailId)
                    {
                        // 更新单位成本和小计
                        detail.UnitCost = selfProductionCost;
                        detail.SubtotalUnitCost = detail.UnitCost * detail.UsedQty;
                        hasChanges = true;
                    }
                }

                if (hasChanges)
                {
                    // 重新计算BOM总成本
                    parentBom.TotalMaterialCost = parentBom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                    parentBom.OutProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalOutManuCost + parentBom.OutApportionedCost;
                    parentBom.SelfProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalSelfManuCost + parentBom.SelfApportionedCost;

                    // 保存明细更新
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(parentBom.tb_BOM_SDetails)
                          .UpdateColumns(it => new { it.UnitCost, it.SubtotalUnitCost })
                          .ExecuteCommandHasChangeAsync();

                    // 保存主表更新
                    await _unitOfWorkManage.GetDbClient().Updateable(parentBom)
                          .UpdateColumns(it => new { it.TotalMaterialCost, it.OutProductionAllCosts, it.SelfProductionAllCosts })
                          .ExecuteCommandHasChangeAsync();

                    // 递归处理上一级BOM
                    await UpdateParentBOMsAsync(parentBom.ProdDetailID, parentBom.SelfProductionAllCosts);
                }
            }
        }


        /// <summary>
        /// 反审核  并且要修改写到产品表中的数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>> AntiApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rs = new ReturnResults<T>();
            tb_BOM_S entity = ObjectEntity as tb_BOM_S;

            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                tb_ProdDetailController<tb_ProdDetail> ctrDetail = _appContext.GetRequiredService<tb_ProdDetailController<tb_ProdDetail>>();
                tb_BOM_SController<tb_BOM_S> ctrinv = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
             
                //更新产品表回写他的配方号
                //entity.tb_proddetail.DataStatus = (int)DataStatus.完结;
                //entity.tb_proddetail.MainID = entity.MainID;
                //await _appContext.Db.Updateable(entity.tb_proddetail).UpdateColumns(t => new { t.DataStatus }).ExecuteCommandAsync();

                tb_ProdDetail detail = await ctrDetail.BaseQueryByIdAsync(entity.ProdDetailID);
                if (detail != null)
                {
                    detail.BOM_ID = null;
                    detail.DataStatus = (int)DataStatus.新建;
                    await ctrDetail.UpdateAsync(detail);
                }


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;

                //后面已经修改为
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable(entity)
                                    .UpdateColumns(it => new { it.DataStatus, it.ApprovalOpinions, it.ApprovalResults, it.ApprovalStatus, it.Approver_at, it.Approver_by })
                                    .ExecuteCommandHasChangeAsync();

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
                rs.Succeeded = false;
                return rs;
            }

        }

        public async override Task<List<T>> GetPrintDataSource(long mainid)
        {
            List<tb_BOM_S> list = await _appContext.Db.CopyNew().Queryable<tb_BOM_S>().Where(m => m.BOM_ID == mainid)
                                        .Includes(a => a.tb_proddetail, b => b.tb_prod, c => c.tb_producttype)
                                        .Includes(a => a.tb_bomconfighistory)
                                        //.Includes(a => a.tb_BOM_SDetailSecondaries)暂时不用
                                        .Includes(a => a.tb_department)
                                        .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdInfo)
                                        //.Includes(a => a.tb_BOM_SDetailSecondaries)
                                        .Includes(a => a.tb_files)
                                        .Includes(a => a.view_ProdInfo)
                                        .Includes(a => a.tb_ProductionDemandDetails)
                                        .Includes(a => a.tb_ProduceGoodsRecommendDetails)
                                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_producttype)
                                        .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                        .ToListAsync();

            return list as List<T>;
        }


    }
}