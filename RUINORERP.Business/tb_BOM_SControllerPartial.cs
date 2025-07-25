﻿
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Business.Security;
using RUINORERP.Business.CommService;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common.Extensions;
using RUINORERP.IServices.BASE;
using RUINORERP.Model.Context;
using System.Linq;
using AutoMapper;
using RUINORERP.Business.FMService;
using MapsterMapper;
using IMapper = AutoMapper.IMapper;
using System.Text;
using System.Windows.Forms;


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



        /// <summary>
        /// BOM审核，改变本身状态，修改对应母件的详情中的BOMID
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
                    return rmrs;
                }

                foreach (tb_BOM_SDetail detailEntity in entity.tb_BOM_SDetails)
                {
                    if (detailEntity.UnitCost == 0)
                    {
                        _unitOfWorkManage.RollbackTran();
                        rmrs.ErrorMsg = $"{detailEntity.SKU}的单位成本不能为0。";
                        rmrs.Succeeded = false;
                        return rmrs;
                    }
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
                    detail.BOM_ID = entity.BOM_ID;
                    detail.DataStatus = (int)DataStatus.确认;
                    await ctrDetail.UpdateAsync(detail);
                }

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
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(bOM_S.tb_BOM_SDetails).ExecuteCommandAsync();
                    }

                    await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_S>(PreviouslevelBomList).ExecuteCommandAsync();
                }

                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                // entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                //只更新指定列
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

                _logger.Error(ex, "事务回滚" + ex.Message);

                return rmrs;
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
                BillConverterFactory bcf = _appContext.GetRequiredService<BillConverterFactory>();

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
                                        .Includes(a => a.tb_BOM_SDetails, b => b.view_ProdDetail)
                                        //.Includes(a => a.tb_BOM_SDetailSecondaries)
                                        .Includes(a => a.tb_files)
                                        .Includes(a => a.view_ProdDetail)
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