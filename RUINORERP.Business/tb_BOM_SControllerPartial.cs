
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
using IMapper = AutoMapper.IMapper;
using System.Text;
using System.Windows.Forms;
using System.Runtime.ConstrainedExecution;
using RUINORERP.Business.EntityLoadService;


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
                await _unitOfWorkManage.BeginTranAsync();
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
                await _unitOfWorkManage.CommitTranAsync();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.BOM_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
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
            ReturnResults<T> rrs = new ReturnResults<T>();
            try
            {
                tb_BOM_S _bom = bom as tb_BOM_S;
                await _unitOfWorkManage.BeginTranAsync();
                var affected = await _unitOfWorkManage.GetDbClient().
                Updateable<tb_ProdDetail>()
                .SetColumns(it => it.BOM_ID == null)
                .Where(it => it.BOM_ID == _bom.BOM_ID).ExecuteCommandHasChangeAsync();

                bool affectedDetail = await BaseDeleteByNavAsync(bom);
                await _unitOfWorkManage.CommitTranAsync();
                rrs.Succeeded = true;
            }
            catch (Exception ex)
            {
                await _unitOfWorkManage.RollbackTranAsync();
                return rrs;
            }
            return rrs;
        }


        /// <summary>
        /// BOM审核，改变本身状态，修改对应母件的详情中的BOMID
        /// 同时修改对应母件的在其它配方中作为子件的成本价格。并且更新对应的总成本价格
        /// BOM明细预估成本(UnitCost)可以为0(新物料首次创建时未录入)，审核时会自动填充为自产成本
        /// 实时成本(RealTimeCost)始终更新，反映最新实际成本
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
                await _unitOfWorkManage.BeginTranAsync();

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
                entity.ApprovalStatus = (int)ApprovalStatus.审核通过;
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
                return rmrs;
            }

        }

        /// <summary>
        /// 递归更新所有上级BOM的成本信息
        /// ✅ 优化: 添加批处理和性能监控
        /// </summary>
        /// <param name="prodDetailId">当前BOM对应的产品详情ID</param>
        /// <param name="selfProductionCost">当前BOM的自产总成本</param>
        /// <param name="processedProdDetailIds">已处理的产品详情ID集合，用于检测循环引用</param>
        /// <param name="depth">当前递归深度(用于性能监控)</param>
        public async Task UpdateParentBOMsAsync(long prodDetailId, decimal selfProductionCost, HashSet<long> processedProdDetailIds = null, int depth = 0)
        {
            // ✅ 优化: 限制最大递归深度,防止异常情况下无限递归
            const int MAX_DEPTH = 10;
            if (depth > MAX_DEPTH)
            {
                _logger.LogWarning("BOM成本更新达到最大递归深度({MaxDepth}),ProdDetailID={ProdDetailID}", MAX_DEPTH, prodDetailId);
                return;
            }

            // 初始化已处理集合
            if (processedProdDetailIds == null)
            {
                processedProdDetailIds = new HashSet<long>();
            }

            // 检查是否存在循环引用
            if (processedProdDetailIds.Contains(prodDetailId))
            {
                _logger.LogWarning("检测到BOM循环引用: ProdDetailID = {0}", prodDetailId);
                return; // 存在循环引用，终止递归
            }

            // 将当前产品详情ID添加到已处理集合
            processedProdDetailIds.Add(prodDetailId);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
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

                _logger.LogDebug("BOM成本更新[深度={Depth}]: ProdDetailID={ProdDetailID}, 找到{Count}个上级BOM", 
                    depth, prodDetailId, parentBomList.Count);

                // ✅ 优化: 批量收集所有需要更新的明细和主表,减少数据库操作次数
                var allDetailsToUpdate = new List<tb_BOM_SDetail>();
                var allBomsToUpdate = new List<tb_BOM_S>();
                var nextLevelUpdates = new List<(long ProdDetailId, decimal Cost)>();

                // 处理当前层级的所有BOM
                foreach (var parentBom in parentBomList)
                {
                    bool hasChanges = false;

                    // 更新子件成本
                    foreach (var detail in parentBom.tb_BOM_SDetails)
                    {
                        if (detail.ProdDetailID == prodDetailId)
                        {
                            // 更新单位成本(兼容性保留)
                            // 如果预估成本为0或接近0,说明创建时未手工录入,则用自产成本填充
                            if (Math.Abs(detail.UnitCost) < 0.0001m)
                            {
                                detail.UnitCost = selfProductionCost;
                                _logger.LogDebug(
                                    "BOM明细[SubID={SubID}]预估成本为0,自动填充为{Cost}",
                                    detail.SubID, selfProductionCost
                                );
                            }
                            // 否则保持手工录入的预估成本不变


                            // 更新实时成本(始终更新,反映最新实际成本)
                            detail.RealTimeCost = selfProductionCost;

                            // 重新计算小计(优先使用RealTimeCost)
                            decimal costForCalculation = detail.RealTimeCost.HasValue && detail.RealTimeCost.Value > 0
                                ? detail.RealTimeCost.Value
                                : detail.UnitCost;

                            // 同时更新两个小计字段
                            detail.SubtotalUnitCost = detail.UnitCost * detail.UsedQty;           // 预估成本小计
                            detail.SubtotalRealTimeCost = detail.RealTimeCost * detail.UsedQty;   // 实时成本小计

                            hasChanges = true;
                        }
                    }

                    if (hasChanges)
                    {
                        // 重新计算BOM总成本
                        parentBom.TotalMaterialCost = parentBom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                        parentBom.OutProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalOutManuCost + parentBom.OutApportionedCost;
                        parentBom.SelfProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalSelfManuCost + parentBom.SelfApportionedCost;

                        // ✅ 优化: 收集到批量更新列表
                        allDetailsToUpdate.AddRange(parentBom.tb_BOM_SDetails);
                        allBomsToUpdate.Add(parentBom);

                        // 收集下一级更新
                        nextLevelUpdates.Add((parentBom.ProdDetailID, parentBom.SelfProductionAllCosts));
                    }
                }

                // ✅ 优化: 批量执行数据库更新
                if (allDetailsToUpdate.Count > 0)
                {
                    // 保存明细更新(包含RealTimeCost和SubtotalRealTimeCost)
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(allDetailsToUpdate)
                          .UpdateColumns(it => new { it.UnitCost, it.RealTimeCost, it.SubtotalUnitCost, it.SubtotalRealTimeCost })
                          .ExecuteCommandHasChangeAsync();

                    // 保存主表更新
                    await _unitOfWorkManage.GetDbClient().Updateable(allBomsToUpdate)
                          .UpdateColumns(it => new { it.TotalMaterialCost, it.OutProductionAllCosts, it.SelfProductionAllCosts })
                          .ExecuteCommandHasChangeAsync();

                    _logger.LogDebug("BOM成本更新[深度={Depth}]: 批量更新了{DetailCount}个明细,{BomCount}个主表", 
                        depth, allDetailsToUpdate.Count, allBomsToUpdate.Count);
                }

                // ✅ 优化: 递归处理上一级BOM(使用相同的processedProdDetailIds)
                foreach (var update in nextLevelUpdates)
                {
                    await UpdateParentBOMsAsync(update.ProdDetailId, update.Cost, processedProdDetailIds, depth + 1);
                }
            }
            finally
            {
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > 100) // 记录耗时较长的操作
                {
                    _logger.LogInformation("BOM成本更新[深度={Depth},ProdDetailID={ProdDetailID}]耗时:{ElapsedMs}ms", 
                        depth, prodDetailId, stopwatch.ElapsedMilliseconds);
                }

                // 从已处理集合中移除当前产品详情ID，允许在其他分支中再次处理
                processedProdDetailIds.Remove(prodDetailId);
            }
        }

        /// <summary>
        /// 反审核时回滚上级BOM的成本(将RealTimeCost重置为NULL)
        /// </summary>
        /// <param name="prodDetailId">当前BOM对应的产品详情ID</param>
        /// <param name="processedProdDetailIds">已处理的产品详情ID集合，用于检测循环引用</param>
        public async Task RollbackParentBOMsCostAsync(long prodDetailId, HashSet<long> processedProdDetailIds = null)
        {
            // 初始化已处理集合
            if (processedProdDetailIds == null)
            {
                processedProdDetailIds = new HashSet<long>();
            }

            // 检查是否存在循环引用
            if (processedProdDetailIds.Contains(prodDetailId))
            {
                _logger.LogWarning("检测到BOM循环引用: ProdDetailID = {0}", prodDetailId);
                return;
            }

            processedProdDetailIds.Add(prodDetailId);

            try
            {
                // 查询所有直接引用当前产品作为子件的上级BOM
                var parentBomList = await _appContext.Db.Queryable<tb_BOM_S>()
                                      .Includes(x => x.tb_BOM_SDetails)
                                      .Where(x => x.tb_BOM_SDetails.Any(z => z.ProdDetailID == prodDetailId))
                                      .ToListAsync();

                if (parentBomList == null || parentBomList.Count == 0)
                {
                    return;
                }

                foreach (var parentBom in parentBomList)
                {
                    bool hasChanges = false;

                    foreach (var detail in parentBom.tb_BOM_SDetails)
                    {
                        if (detail.ProdDetailID == prodDetailId)
                        {
                            // 将RealTimeCost重置为NULL(表示需要重新计算)
                            detail.RealTimeCost = null;
                            detail.SubtotalRealTimeCost = null;

                            // SubtotalUnitCost保持不变(基于UnitCost)
                            detail.SubtotalUnitCost = detail.UnitCost * detail.UsedQty;

                            hasChanges = true;
                        }
                    }

                    if (hasChanges)
                    {
                        // 重新计算BOM总成本(基于预估成本)
                        parentBom.TotalMaterialCost = parentBom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                        parentBom.OutProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalOutManuCost + parentBom.OutApportionedCost;
                        parentBom.SelfProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalSelfManuCost + parentBom.SelfApportionedCost;

                        // 保存明细更新
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(parentBom.tb_BOM_SDetails)
                              .UpdateColumns(it => new { it.RealTimeCost, it.SubtotalRealTimeCost, it.SubtotalUnitCost })
                              .ExecuteCommandHasChangeAsync();

                        // 保存主表更新
                        await _unitOfWorkManage.GetDbClient().Updateable(parentBom)
                              .UpdateColumns(it => new { it.TotalMaterialCost, it.OutProductionAllCosts, it.SelfProductionAllCosts })
                              .ExecuteCommandHasChangeAsync();

                        // 递归处理上一级BOM
                        await RollbackParentBOMsCostAsync(parentBom.ProdDetailID, processedProdDetailIds);
                    }
                }
            }
            finally
            {
                processedProdDetailIds.Remove(prodDetailId);
            }
        }

        /// <summary>
        /// 反审核时回滚上级BOM的成本(保留实时库存成本)
        /// ✅ 优化版本: 使用当前库存成本作为RealTimeCost,而非设为NULL
        /// </summary>
        /// <param name="prodDetailId">当前BOM对应的产品详情ID</param>
        /// <param name="inventoryCost">当前库存成本(反审核后的成本)</param>
        /// <param name="processedProdDetailIds">已处理的产品详情ID集合，用于检测循环引用</param>
        public async Task RollbackParentBOMsCostWithInventoryAsync(long prodDetailId, decimal inventoryCost, HashSet<long> processedProdDetailIds = null)
        {
            // 初始化已处理集合
            if (processedProdDetailIds == null)
            {
                processedProdDetailIds = new HashSet<long>();
            }

            // 检查是否存在循环引用
            if (processedProdDetailIds.Contains(prodDetailId))
            {
                _logger.LogWarning("检测到BOM循环引用: ProdDetailID = {0}", prodDetailId);
                return;
            }

            processedProdDetailIds.Add(prodDetailId);

            try
            {
                // 查询所有直接引用当前产品作为子件的上级BOM
                var parentBomList = await _appContext.Db.Queryable<tb_BOM_S>()
                                      .Includes(x => x.tb_BOM_SDetails)
                                      .Where(x => x.tb_BOM_SDetails.Any(z => z.ProdDetailID == prodDetailId))
                                      .ToListAsync();

                if (parentBomList == null || parentBomList.Count == 0)
                {
                    return;
                }

                foreach (var parentBom in parentBomList)
                {
                    bool hasChanges = false;

                    foreach (var detail in parentBom.tb_BOM_SDetails)
                    {
                        if (detail.ProdDetailID == prodDetailId)
                        {
                            // ✅ 优化: 使用当前库存成本作为RealTimeCost,而非设为NULL
                            // 这样即使反审核后,上级BOM仍然有一个有效的成本参考
                            if (inventoryCost > 0)
                            {
                                detail.RealTimeCost = inventoryCost;
                                detail.SubtotalRealTimeCost = inventoryCost * detail.UsedQty;
                                _logger.LogDebug("BOM明细[SubID={SubID}]反审核后使用库存成本:{Cost}", detail.SubID, inventoryCost);
                            }
                            else
                            {
                                // 如果库存成本无效,则回退到UnitCost
                                detail.RealTimeCost = detail.UnitCost;
                                detail.SubtotalRealTimeCost = detail.UnitCost * detail.UsedQty;
                                _logger.LogWarning("BOM明细[SubID={SubID}]反审核时库存成本无效({Cost}),使用UnitCost:{UnitCost}", 
                                    detail.SubID, inventoryCost, detail.UnitCost);
                            }

                            // SubtotalUnitCost保持不变(基于UnitCost)
                            detail.SubtotalUnitCost = detail.UnitCost * detail.UsedQty;

                            hasChanges = true;
                        }
                    }

                    if (hasChanges)
                    {
                        // 重新计算BOM总成本
                        // ✅ 优化: 优先使用RealTimeCost计算总成本
                        parentBom.TotalMaterialCost = parentBom.tb_BOM_SDetails.Sum(c => 
                            c.RealTimeCost.HasValue && c.RealTimeCost.Value > 0 
                            ? c.SubtotalRealTimeCost.Value 
                            : c.SubtotalUnitCost);
                        parentBom.OutProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalOutManuCost + parentBom.OutApportionedCost;
                        parentBom.SelfProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalSelfManuCost + parentBom.SelfApportionedCost;

                        // 保存明细更新
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(parentBom.tb_BOM_SDetails)
                              .UpdateColumns(it => new { it.RealTimeCost, it.SubtotalRealTimeCost, it.SubtotalUnitCost })
                              .ExecuteCommandHasChangeAsync();

                        // 保存主表更新
                        await _unitOfWorkManage.GetDbClient().Updateable(parentBom)
                              .UpdateColumns(it => new { it.TotalMaterialCost, it.OutProductionAllCosts, it.SelfProductionAllCosts })
                              .ExecuteCommandHasChangeAsync();

                        // 递归处理上一级BOM(使用当前BOM的自产成本)
                        await RollbackParentBOMsCostWithInventoryAsync(parentBom.ProdDetailID, parentBom.SelfProductionAllCosts, processedProdDetailIds);
                    }
                }
            }
            finally
            {
                processedProdDetailIds.Remove(prodDetailId);
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
                await _unitOfWorkManage.BeginTranAsync();
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

                // 反审核时需要回滚上级BOM的成本(将RealTimeCost重置为NULL或0)
                await RollbackParentBOMsCostAsync(entity.ProdDetailID);


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
                await _unitOfWorkManage.CommitTranAsync();
                rs.ReturnObject = entity as T;
                rs.Succeeded = true;
                return rs;
            }
            catch (Exception ex)
            {

                await _unitOfWorkManage.RollbackTranAsync();
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