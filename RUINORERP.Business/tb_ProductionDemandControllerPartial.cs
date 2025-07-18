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
using RUINORERP.Common.DB;
using System.Collections;
using RUINORERP.Business.AutoMapper;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using RUINORERP.Business.CommService;
using System.Windows.Forms;

namespace RUINORERP.Business
{

    /// <summary>
    /// 需求分析审核
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class tb_ProductionDemandController<T>
    {


        /// <summary>
        /// 更新了计划单中明细的分析数量及主单的分析状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// 

        public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
        {
            ReturnResults<T> rmrs = new ReturnResults<T>();
            tb_ProductionDemand entity = ObjectEntity as tb_ProductionDemand;
            try
            {
                if (entity == null)
                {
                    return rmrs;
                }
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();


                //更新计划单中已分析字段,并且要具体到目标产品。计划可能多个成品。但是只能一个一个分析
                if (entity.tb_productionplan != null)
                {

                    if (entity.tb_productionplan.tb_ProductionPlanDetails == null)
                    {
                        entity.tb_productionplan.tb_ProductionPlanDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlanDetail>()
                            .Where(c => c.PPID == entity.tb_productionplan.PPID).ToList();
                    }


                    foreach (tb_ProductionDemandTargetDetail tag in entity.tb_ProductionDemandTargetDetails)
                    {
                        var planDetail = entity.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == tag.ProdDetailID && c.Location_ID == tag.Location_ID);
                        if (planDetail != null)
                        {
                            planDetail.IsAnalyzed = true;
                            planDetail.AnalyzedQuantity += tag.NeedQuantity;
                        }
                    }
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlanDetail>(entity.tb_productionplan.tb_ProductionPlanDetails).ExecuteCommandAsync();
                    //如果全部分析过，则更新计划单中的分析字段
                    if (entity.tb_productionplan.tb_ProductionPlanDetails.All(c => c.IsAnalyzed.HasValue && c.IsAnalyzed.Value))
                    {
                        entity.tb_productionplan.Analyzed = true;
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entity.tb_productionplan).ExecuteCommandAsync();
                    }
                }


                //这部分是否能提出到                上一级公共部分？
                entity.DataStatus = (int)DataStatus.确认;
                //entity.ApprovalOpinions = approvalEntity.ApprovalComments;
                //后面已经修改为
                //entity.ApprovalResults = approvalEntity.ApprovalResults;
                entity.ApprovalStatus = (int)ApprovalStatus.已审核;
                BusinessHelper.Instance.ApproverEntity(entity);
                //只更新指定列
                var result = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionDemand>(entity).UpdateColumns(it => new
                {
                    it.DataStatus,
                    it.ApprovalResults,
                    it.ApprovalStatus,
                    it.Approver_at,
                    it.Approver_by,
                    it.ApprovalOpinions

                }).ExecuteCommandAsync();

                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {

                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                rmrs.ErrorMsg = "事务回滚=>" + ex.Message;
                return rmrs;
            }

        }


        /// <summary>
        /// 批量结案  销售订单标记结案，数据状态为8,可以更新销售订单付款状态， 如果还没有出库。但是结案的订单时。修正拟出库数量。
        /// 目前暂时是这个逻辑。后面再处理凭证财务相关的
        /// 目前认为结案 是仓库和业务确定这个订单不再执行的一个确认过程。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
        {
            List<tb_ProductionDemand> entitys = new List<tb_ProductionDemand>();
            entitys = NeedCloseCaseList as List<tb_ProductionDemand>;


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

                    //这部分是否能提出到上一级公共部分？
                    entitys[m].DataStatus = (int)DataStatus.完结;
                    BusinessHelper.Instance.EditEntity(entitys[m]);

                    //只更新指定列
                    var affectedRows = await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionDemand>(entitys[m]).UpdateColumns(it => new { it.DataStatus, it.Modified_by, it.Modified_at, it.Notes }).ExecuteCommandAsync();
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
        /// 是否被制令单引用,是的话，不可以删除
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public bool IsReferencedCanDelete(tb_ProductionDemand entity)
        {
            bool rs = false;
            try
            {
                bool a = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>().Any(it => it.PDID == entity.PDID);

                bool b = _unitOfWorkManage.GetDbClient().Queryable<tb_BuyingRequisition>().Any(it => it.RefBillID == entity.PDID);

                rs = !a && !b;
            }
            catch (Exception ex)
            {

            }
            return rs;
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
            tb_ProductionDemand entity = ObjectEntity as tb_ProductionDemand;

            ReturnResults<T> rmrs = new ReturnResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                //判断是否能反审?
                if (entity.tb_ManufacturingOrders != null
                    && (entity.tb_ManufacturingOrders.Any(c => c.DataStatus == (int)DataStatus.确认 || c.DataStatus == (int)DataStatus.完结) && entity.tb_ManufacturingOrders.Any(c => c.ApprovalStatus == (int)ApprovalStatus.已审核)))
                {

                    rmrs.ErrorMsg = "存在已确认或已完结，或已审核的制令单，不能反审核  ";
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


                //更新在制数量 应该是在生产通知单时更新的，这里暂时不更新
                //更新计划单中已分析字段,并且要具体到目标产品。计划可能多个成品。但是只能一个一个分析
                if (entity.tb_productionplan != null)
                {
                    if (entity.tb_productionplan.tb_ProductionPlanDetails == null)
                    {
                        entity.tb_productionplan.tb_ProductionPlanDetails = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlanDetail>()
                            .Where(c => c.PPID == entity.tb_productionplan.PPID).ToList();
                    }

                    foreach (tb_ProductionDemandTargetDetail tag in entity.tb_ProductionDemandTargetDetails)
                    {
                        var planDetail = entity.tb_productionplan.tb_ProductionPlanDetails.FirstOrDefault(c => c.ProdDetailID == tag.ProdDetailID && c.Location_ID == tag.Location_ID);
                        if (planDetail != null)
                        {
                            planDetail.IsAnalyzed = false;
                            planDetail.AnalyzedQuantity -= tag.NeedQuantity;
                        }
                    }
                    await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlanDetail>(entity.tb_productionplan.tb_ProductionPlanDetails).ExecuteCommandAsync();
                    //如果全部分析过，则更新计划单中的分析字段,都 有值，但是有一个为假，则为假。
                    if (entity.tb_productionplan.tb_ProductionPlanDetails.All(c => c.IsAnalyzed.HasValue)
                        && entity.tb_productionplan.tb_ProductionPlanDetails.Any(c => c.IsAnalyzed == false))
                    {
                        entity.tb_productionplan.Analyzed = false;
                        await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionPlan>(entity.tb_productionplan).ExecuteCommandAsync();
                    }
                }


                //这部分是否能提出到上一级公共部分？
                entity.DataStatus = (int)DataStatus.新建;
                entity.ApprovalResults = false;
                entity.ApprovalStatus = (int)ApprovalStatus.未审核;
                BusinessHelper.Instance.ApproverEntity(entity);

                //后面是不是要做一个审核历史记录表？

                //只更新指定列

                await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionDemand>(entity).ExecuteCommandAsync();


                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rmrs.Succeeded = true;
                rmrs.ReturnObject = entity as T;
                return rmrs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                BizTypeMapper mapper = new BizTypeMapper();
                rmrs.ErrorMsg = mapper.GetBizType(typeof(tb_ProductionDemand)).ToString() + "事务回滚=>" + ex.Message;
                rmrs.Succeeded = false;
                return rmrs;
            }

        }


        public async override Task<List<T>> GetPrintDataSource(long MainID)
        {

            List<tb_ProductionDemand> list = await _appContext.Db.CopyNew().Queryable<tb_ProductionDemand>().Where(m => m.PDID == MainID)
                            .Includes(a => a.tb_ProductionDemandDetails)
                              .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                              .Includes(a => a.tb_ProductionDemandDetails, b => b.tb_proddetail, c => c.tb_prod, d => d.tb_unit)
                               .AsNavQueryable()//加这个前面,超过三级在前面加这一行，并且第四级无VS智能提示，但是可以用
                                 .ToListAsync();
            return list as List<T>;
        }




        /// <summary>
        /// 通过BOM_ID，找到计划生产的成品 中需要哪些原料，并取得库存。找不到数量不够的显示出来。数量具体是多少。刚根据BOM配方和计划数量来计算
        /// </summary>
        /// <param name="_bomIDs"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>

        public List<tb_BOM_SDetail> GetBomDetailsInfo(long _bomID)
        {
            List<View_Inventory> list = new List<View_Inventory>();
            List<tb_BOM_SDetail> bOM_SDetails = _appContext.Db.CopyNew().Queryable<tb_BOM_SDetail>()
                .Includes(c => c.tb_proddetail, d => d.tb_bom_s)
                .Includes(c => c.tb_proddetail, d => d.tb_prod)
                .Includes(c => c.view_ProdDetail)
                .Includes(c => c.tb_proddetail, d => d.tb_Inventories)
                  .Includes(c => c.tb_bom_s)
                .Where(c => c.BOM_ID == _bomID).ToList();
            return bOM_SDetails;
        }

        [Obsolete]
        public async Task<List<tb_BOM_SDetail>> GetBomDetailsInfos(long[] _bomIDs)
        {
            List<View_Inventory> list = new List<View_Inventory>();
            List<tb_BOM_SDetail> bOM_SDetails = await _appContext.Db.CopyNew().Queryable<tb_BOM_SDetail>().Where(c => _bomIDs.Contains(c.BOM_ID)).ToListAsync();
            //var _detail_ids = bOM_SDetails.Select(x => new { x.ProdDetailID.Value }).ToList();
            //List<long> longids = new List<long>();
            //foreach (var item in _detail_ids)
            //{
            //    longids.Add(item.Value);
            //}
            return bOM_SDetails;
        }

        /// <summary>
        /// 如果后期其他 测试通过 。这个可以删除
        /// </summary>
        /// <param name="_bomIDs"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<List<tb_ProductionDemandDetail>> GetInventoryInfo(List<tb_ProductionDemandTargetDetail> _targetDetails)
        {

            List<tb_BOM_SDetail> AllbomDetails = new List<tb_BOM_SDetail>();
            List<View_Inventory> list = new List<View_Inventory>();


            //得到所有BOM相关的原料需求明细。
            //因为支持多个成品 的BOM分析，成品需求时间不一样。这里还要临时保存一下需求时间

            foreach (var item in _targetDetails)
            {

                List<tb_BOM_SDetail> bomDetails = GetBomDetailsInfo(item.BOM_ID);

                /*
                foreach (tb_BOM_SDetail detail in bomDetails)
                {

                    decimal needQty = 0;
                    //这里计算需要的数量，有很多逻辑，特别是可换算的情况
                    if (detail.UnitConversion_ID.HasValue)
                    {
                        needQty = item.NeedQuantity.ToDecimal() * detail.UsedQty;

                    }
                    else
                    {
                        needQty = item.NeedQuantity.ToDecimal() * detail.UsedQty;
                    }


                    if (!needQtyList.ContainsKey(detail.ProdDetailID))
                    {
                        tb_ProductionDemandDetail temp = new tb_ProductionDemandDetail();
                        int ttt= needQty.ToInt(); 
                        temp.NeedQuantity = (int)needQty;
                        if (detail.tb_proddetail != null)
                        {
                            temp.BOM_ID = detail.tb_proddetail.BOM_ID;
                            temp.tb_bom_s = detail.tb_proddetail.tb_bom_s;
                        }
                        temp.RequirementDate = item.RequirementDate;

                        needQtyList.TryAdd(detail.ProdDetailID, temp);
                    }
                    else
                    {
                        //更新累加
                        tb_ProductionDemandDetail tempExit = new tb_ProductionDemandDetail();

                        needQtyList.TryGetValue(detail.ProdDetailID, out tempExit);
                        if (tempExit != null)
                        {
                            tempExit.NeedQuantity += needQty.ToInt();
                        }
                        // needQtyList[detail.ProdDetailID], needQtyList[detail.ProdDetailID].NeedQuantity + needQty.ToInt());
                    }
                }
                */

                if (bomDetails != null)
                {
                    AllbomDetails.AddRange(bomDetails);
                }

            }

            //通过BOM明细对应的料号去找到库存信息。这里为了下面查询时用ids这里只抽取一列产品ID。
            var _detail_ids = AllbomDetails.Select(x => new { x.ProdDetailID }).ToList();
            List<long> longids = new List<long>();
            foreach (var item in _detail_ids)
            {
                if (!longids.Contains(item.ProdDetailID))
                {
                    longids.Add(item.ProdDetailID);
                }
            }

            list = await _appContext.Db.CopyNew().Queryable<View_Inventory>().Where(m => longids.ToArray().Contains(m.ProdDetailID.Value))
                         .ToListAsync();


            //根据原来计划数量 和BOM得到对应需求数量
            //保存每个产品已经使用过的数量，如多行的情况。要累减
            ConcurrentDictionary<long, tb_ProductionDemandDetail> needQtyList = new ConcurrentDictionary<long, tb_ProductionDemandDetail>();


            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();

            //找到了库存情况
            List<tb_ProductionDemandDetail> FromInvdetails = mapper.Map<List<tb_ProductionDemandDetail>>(list);
            List<tb_ProductionDemandDetail> LastDetails = new List<tb_ProductionDemandDetail>();

            foreach (var item in AllbomDetails)
            {
                //根据配方算出原料需要多少数量 

                tb_ProductionDemandDetail NewDetail = new tb_ProductionDemandDetail();
                NewDetail.BOM_ID = item.BOM_ID;
                NewDetail.BookInventory = 0;
                NewDetail.AvailableStock = 0;
                NewDetail.ProdDetailID = item.ProdDetailID;
                long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                NewDetail.ID = sid;
                NewDetail.ParentId = 0;//一级数据
                NewDetail.property = item.view_ProdDetail.prop;

                //    NewDetail. = item.ProdDetailName;
                //    NewDetail.ProdDetailCode = item.ProdDetailCode;

                //    NewDetail.UsedQty = item.UsedQty;


                //    item.NeedQuantity = tempExit.NeedQuantity;
                //    item.NetRequirement = item.NeedQuantity;
                //    item.GrossRequirement = item.NeedQuantity;
                //    item.RequirementDate = tempExit.RequirementDate;


                //    int missQty = tempExit.NeedQuantity - item.AvailableStock;
                //    if ((item.AvailableStock - tempExit.NeedQuantity) > 0)
                //    {
                //        item.MissingQuantity = 0;
                //    }
                //    else
                //    {
                //        item.MissingQuantity = missQty;
                //    }

                //    //多行情况
                //    //意思是通过列名找，再通过值找到对应的文本
                //    needQtyList.TryGetValue(item.ProdDetailID, out tempExit);
                //    if (tempExit != null)
                //    {

                //    }
                //    else
                //    {

                //    }


                //    LastDetails.Add(item);
            }

            return LastDetails;
        }


        /// <summary>
        /// 通过BOM_ID，找到计划生产的成品 中需要哪些原料，并取得库存。找不到数量不够的显示出来。数量具体是多少。刚根据BOM配方和计划数量来计算
        /// </summary>
        /// <param name="_bomIDs"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public List<tb_ProductionDemandDetail> GetTopInventoryInfo(tb_ProductionDemand demand, List<tb_ProductionDemandTargetDetail> _targetDetails, ref ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList)
        {

            List<tb_ProductionDemandDetail> demandDetails = new List<tb_ProductionDemandDetail>();
            tb_ProductionDemandController<tb_ProductionDemand> ctrPD = _appContext.GetRequiredService<tb_ProductionDemandController<tb_ProductionDemand>>();
            //根据原来计划数量 和BOM得到对应需求数量
            //保存每个产品已经使用过的数量，如多行的情况。要累减
            // ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList = new ConcurrentDictionary<long, tb_ProductionDemandDetail>();

            //得到所有BOM相关的原料需求明细。
            //因为支持多个成品 的BOM分析，成品需求时间不一样。这里还要临时保存一下需求时间
            //AlreadyReducedQtyList = new ConcurrentDictionary<long, tb_ProductionDemandDetail>();
            foreach (var item in _targetDetails)
            {

                //顶层。并且一定有BOM配方
                tb_ProductionDemandDetail topDetail = new tb_ProductionDemandDetail();
                topDetail = mapper.Map<tb_ProductionDemandDetail>(item);
                #region 修复目标根节点的数据
                long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                topDetail.ID = sid;
                topDetail.ParentId = 0;//一级数据

                //修正数量,注意这里只是成品的数量
                //根节点是成品时。需求时间就是计划需求时间
                topDetail.RequirementDate = item.RequirementDate;

                topDetail.GrossRequirement = item.NeedQuantity;
                topDetail.BookInventory = item.BookInventory;
                topDetail.Sale_Qty = item.SaleQty;
                topDetail.InTransitInventory = item.InTransitInventory;
                topDetail.MakeProcessInventory = item.MakeProcessInventory;
                topDetail.NotOutQty = item.NotIssueMaterialQty;
                //这里要判断，如果计划来自销售订单，则需要减去当前销售订单的对应产品的数量
                topDetail.NetRequirement = item.NeedQuantity - item.BookInventory + item.NotIssueMaterialQty - item.InTransitInventory - item.MakeProcessInventory + item.SaleQty;
                if (demand.PPID > 0 && demand.tb_productionplan.tb_saleorder != null)
                {
                    //减去销售订单
                    topDetail.NetRequirement -= demand.tb_productionplan.tb_saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == topDetail.ProdDetailID && c.Location_ID == topDetail.Location_ID).Quantity;
                }
                if (topDetail.NetRequirement < 0)
                {
                    topDetail.NetRequirement = 0;
                }
                //注意：毛需求和净需求，都是计算成品的需求，所以这里要根据SuggestBasedOn来判断。次级并不需要处理。
                //这里决定的成品的实际需求数量，就决定的次级所有的子件用量了。
                if (demand.SuggestBasedOn)
                {
                    topDetail.NeedQuantity = topDetail.NetRequirement;

                }
                else
                {
                    topDetail.NeedQuantity = topDetail.GrossRequirement;
                }


                #endregion
                demandDetails.Add(topDetail);

                if (topDetail.BOM_ID.HasValue)
                {
                    List<tb_BOM_SDetail> bomDetails = GetBomDetailsInfo(topDetail.BOM_ID.Value);
                    foreach (tb_BOM_SDetail detail in bomDetails)
                    {
                        //构建成品需求明细,并且可以根据需求配置中的毛需求和净需求来计算不同的值。

                        tb_ProductionDemandDetail NewDetail = GetProductionDemandDetailFrom(demand.SuggestBasedOn, topDetail.BOM_ID, topDetail.Location_ID, topDetail.RequirementDate, topDetail.NeedQuantity, detail, topDetail.ID.Value, ref AlreadyReducedQtyList);
                        AlreadyReducedQtyList.TryAdd(NewDetail.ProdDetailID, NewDetail);
                        //如果存在裸机等半成品 ，实际以他是否有bom为标记找下级需要的材料
                        if (NewDetail.BOM_ID.HasValue)
                        {
                            //找他的次级，里面的业务逻辑已经构建了树型结构,topDetail.NeedQuantity 注意这个需求数量*bom用量才是对的。 比方150套一拖二的，这里的需求数量就是 300个裸机，主板也是300，用量2就是150*2
                            var nextlist = ctrPD.GetNextBOMInventoryInfo(demand.SuggestBasedOn, NewDetail.NeedQuantity, NewDetail.RequirementDate, NewDetail.ID.Value, NewDetail.BOM_ID.Value, AlreadyReducedQtyList, NewDetail.Location_ID);
                            demandDetails.AddRange(nextlist);
                        }
                        demandDetails.Add(NewDetail);
                    }
                }
                else
                {
                    _unitOfWorkManage.RollbackTran();
                    throw new Exception("分析的目标必须要有配方!");
                }



            }

            return demandDetails;
        }


        /// <summary>
        /// 通过BOM_ID，找到计划生产的成品 中需要哪些原料，并取得库存。找不到数量不够的显示出来。数量具体是多少。刚根据BOM配方和计划数量来计算
        /// 构建成品需求明细,并且可以根据需求配置中的毛需求和净需求来计算不同的值。
        /// </summary>
        /// <param name="SuggestBasedOn">建议依据，false,0 :毛需求，为真是净需求</param>
        /// <param name="BOM_ID">上级BOMid</param>
        /// <param name="Location_ID"></param>
        /// <param name="RequirementDate"></param>
        /// <param name="NeedQuantity">成品需求</param>
        /// <param name="detail"></param>
        /// <param name="PID"></param>
        /// <param name="AlreadyReducedQtyList"></param>
        /// <returns></returns>
        private tb_ProductionDemandDetail GetProductionDemandDetailFrom(
            bool SuggestBasedOn,
            long? BOM_ID,
            long Location_ID, DateTime RequirementDate, int NeedQuantity,
            tb_BOM_SDetail detail, long PID, ref ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList)
        {
            tb_ProductionDemandDetail NewDetail = new tb_ProductionDemandDetail();
            if (!AlreadyReducedQtyList.TryGetValue(NewDetail.ProdDetailID, out NewDetail))
            {
                NewDetail = new tb_ProductionDemandDetail();
            }
            long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
            NewDetail.ID = sid;
            NewDetail.ParentId = PID;//一级数据
            NewDetail.BOM_ID = BOM_ID;//再下级由这个BOMID作为父级去找
            NewDetail.ProdDetailID = detail.ProdDetailID;
            NewDetail.property = detail.view_ProdDetail.prop;
            NewDetail.Location_ID = Location_ID;//TODO 需求要仓库。BOM不用库仓库
            NewDetail.RequirementDate = RequirementDate;
            NewDetail.tb_proddetail = detail.tb_proddetail;



            //这个产品对应的BOM
            NewDetail.BOM_ID = detail.tb_proddetail.BOM_ID;
            //这个产品对应的BOM实体
            NewDetail.tb_bom_s = detail.tb_proddetail.tb_bom_s;

            //实际需求数量：通过实际的生产数据或库存记录来确定。可以使用生产过程中的领料记录、库存盘点等方式获取实际使用或消耗的数量。
            decimal needQty = 0;
            //这里计算需要的数量，有很多逻辑，特别是可换算的情况
            if (detail.UnitConversion_ID.HasValue)
            {
                needQty = NeedQuantity.ToDecimal() * detail.UsedQty;

            }
            else
            {
                needQty = NeedQuantity.ToDecimal() * detail.UsedQty;
            }
            //毛需求数量：通常来自生产计划或订单，根据产品的 BOM（物料清单）和生产数量计算得出。
            //净需求数量：通过毛需求数量减去现有库存、在途库存、预订量等因素计算得出。可以使用库存管理系统或 ERP 系统中的相关功能来获取这些数据。
            NewDetail.GrossRequirement = needQty.ToInt();

            #region 净需求
            int Sale_Qty = detail.tb_proddetail.tb_Inventories
              .Where(w => w.Location_ID == Location_ID)
              .Sum(c => c.Sale_Qty);
            int NotOutQty = detail.tb_proddetail.tb_Inventories
                .Where(w => w.Location_ID == Location_ID)
                .Sum(c => c.NotOutQty);

            //这几个指标要优化
            NewDetail.InTransitInventory = detail.tb_proddetail.tb_Inventories
                .Where(w => w.Location_ID == Location_ID)
                .Sum(c => c.On_the_way_Qty);


            NewDetail.MakeProcessInventory = detail.tb_proddetail.tb_Inventories
                .Where(w => w.Location_ID == Location_ID)
                .Sum(c => c.MakingQty);



            NewDetail.BookInventory = detail.tb_proddetail.tb_Inventories
                .Where(w => w.Location_ID == Location_ID)
                .Sum(c => c.Quantity);

            NewDetail.NetRequirement = needQty.ToInt() - NewDetail.BookInventory + NotOutQty - NewDetail.InTransitInventory - NewDetail.MakeProcessInventory + Sale_Qty;
            if (NewDetail.NetRequirement < 0)
            {
                NewDetail.NetRequirement = 0;
            }
            #endregion

            if (SuggestBasedOn)
            {
                //实际需求为净需求
                NewDetail.NeedQuantity = NewDetail.NetRequirement;
            }
            else
            {
                NewDetail.NeedQuantity = NewDetail.GrossRequirement;
            }


            //如果仓库有的原料 需求时间就是当前
            if (NewDetail.GrossRequirement == 0)
            {
                NewDetail.RequirementDate = DateTime.Now;
            }
            else
            {
                //节点是非成品时。需求时间就是计划需求时间-生产周期10天 如果总时间都不够10天就是当前时间就要材料了
                if (RequirementDate.AddDays(-10) < DateTime.Now)
                {
                    NewDetail.RequirementDate = DateTime.Now;
                }
                else
                {
                    NewDetail.RequirementDate = RequirementDate.AddDays(-10);
                }
            }


            //可用库存是多次递减，所以多次赋值
            NewDetail.AvailableStock = detail.tb_proddetail.tb_Inventories
          .Where(w => w.Location_ID == Location_ID)
          .Sum(c => c.Quantity);

            int missQty = NewDetail.NeedQuantity - NewDetail.AvailableStock;
            if (missQty > 0)
            {
                NewDetail.MissingQuantity = missQty;
            }
            else
            {
                NewDetail.MissingQuantity = 0;
            }
            //意思是这一行 缺少的数量减掉就是还剩下的。下次还有再次累计减。
            NewDetail.AvailableStock = NewDetail.AvailableStock - NewDetail.MissingQuantity;

            AlreadyReducedQtyList.TryAdd(NewDetail.ProdDetailID, NewDetail);
            return NewDetail;
        }

        /// <summary>
        /// 获取下一级bom需要的原料
        /// </summary>
        /// <param name="NeedQuantity">需要的数量</param>
        /// <param name="RequirementDate">需要的日期</param>
        /// <param name="PID">父级ID</param>
        /// <param name="BOM_ID">父级bom的ID,由这个查出BOM详情的相关子组件</param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        [Obsolete]
        public async Task<List<tb_ProductionDemandDetail>> GetBOMNextNodeInventoryInfo(int NeedQuantity,
            DateTime RequirementDate, long PID, long BOM_ID, long locationID,
            ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList
            )
        {

            List<tb_BOM_SDetail> AllbomDetails = new List<tb_BOM_SDetail>();
            List<View_Inventory> list = new List<View_Inventory>();

            //得到所有BOM相关的原料需求明细。
            //根据原来计划数量 和BOM得到对应需求数量
            ConcurrentDictionary<long, tb_ProductionDemandDetail> needQtyList = new ConcurrentDictionary<long, tb_ProductionDemandDetail>();
            List<tb_BOM_SDetail> bomDetails = GetBomDetailsInfo(BOM_ID);
            foreach (tb_BOM_SDetail detail in bomDetails)
            {
                decimal needQty = NeedQuantity.ToDecimal() * detail.UsedQty;
                if (!needQtyList.ContainsKey(detail.ProdDetailID))
                {
                    tb_ProductionDemandDetail temp = new tb_ProductionDemandDetail();
                    temp.NeedQuantity = needQty.ToInt();
                    if (detail.tb_proddetail != null)
                    {
                        temp.BOM_ID = detail.tb_proddetail.BOM_ID;
                        temp.tb_bom_s = detail.tb_proddetail.tb_bom_s;
                    }
                    temp.ParentId = PID;
                    temp.RequirementDate = RequirementDate;
                    temp.property = detail.view_ProdDetail.prop;
                    needQtyList.TryAdd(detail.ProdDetailID, temp);
                }
                else
                {
                    //更新累加
                    tb_ProductionDemandDetail tempExit = new tb_ProductionDemandDetail();

                    needQtyList.TryGetValue(detail.ProdDetailID, out tempExit);
                    if (tempExit != null)
                    {
                        tempExit.NeedQuantity += needQty.ToInt();
                    }
                }
            }
            AllbomDetails.AddRange(bomDetails);

            //通过BOM明细对应的料号去找到库存信息。
            var _detail_ids = AllbomDetails.Select(x => new { x.ProdDetailID }).ToList();
            List<long> longids = new List<long>();
            foreach (var item in _detail_ids)
            {
                if (!longids.Contains(item.ProdDetailID))
                {
                    longids.Add(item.ProdDetailID);
                }
            }
            if (locationID != 0)
            {
                list = await _appContext.Db.CopyNew().Queryable<View_Inventory>().Where(m => longids.ToArray().Contains(m.ProdDetailID.Value) && m.Location_ID == locationID)
                             .ToListAsync();
            }
            else
            {
                list = await _appContext.Db.CopyNew().Queryable<View_Inventory>().Where(m => longids.ToArray().Contains(m.ProdDetailID.Value))
                             .ToListAsync();
            }

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            List<tb_ProductionDemandDetail> details = mapper.Map<List<tb_ProductionDemandDetail>>(list);
            List<tb_ProductionDemandDetail> LastDetails = new List<tb_ProductionDemandDetail>();
            foreach (var item in details)
            {
                //根据配方算出原料需要多少数量 
                //不够才显示
                tb_ProductionDemandDetail tempExit = new tb_ProductionDemandDetail();
                //意思是通过列名找，再通过值找到对应的文本
                needQtyList.TryGetValue(item.ProdDetailID, out tempExit);
                if (tempExit != null)
                {
                    item.NeedQuantity = tempExit.NeedQuantity;
                    item.NetRequirement = item.NeedQuantity;
                    item.property = tempExit.property;
                    item.GrossRequirement = item.NeedQuantity;
                    item.RequirementDate = tempExit.RequirementDate;
                    item.BOM_ID = tempExit.BOM_ID;//如果他有配方时，则认为是有子原料不足的情况
                    item.tb_bom_s = tempExit.tb_bom_s;
                    long sid = RUINORERP.Common.SnowflakeIdHelper.IdHelper.GetLongId();
                    item.ID = sid;
                    item.ParentId = tempExit.ParentId;//一级数据
                    int missQty = tempExit.NeedQuantity - item.AvailableStock;
                    if ((item.AvailableStock - tempExit.NeedQuantity) > 0)
                    {
                        item.MissingQuantity = 0;
                    }
                    else
                    {
                        item.MissingQuantity = missQty;
                    }
                    LastDetails.Add(item);
                }

            }

            return LastDetails;
        }

        /// <summary>
        /// 获取下一级bom需要的原料ok
        /// </summary>
        /// <param name="NeedQuantity">需要的数量</param>
        /// <param name="RequirementDate">需要的日期</param>
        /// <param name="PID">父级ID</param>
        /// <param name="BOM_ID">父级bom的ID,由这个查出BOM详情的相关子组件</param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        public List<tb_ProductionDemandDetail> GetNextBOMInventoryInfo(bool SuggestBasedOn, int NeedQuantity,
            DateTime RequirementDate, long PID, long BOM_ID,
            ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList,
            long Location_ID = 0)
        {
            List<tb_ProductionDemandDetail> demandDetails = new List<tb_ProductionDemandDetail>();

            //根据原来计划数量 和BOM得到对应需求数量
            //保存每个产品已经使用过的数量，如多行的情况。要累减
            //ConcurrentDictionary<long, tb_ProductionDemandDetail> AlreadyReducedQtyList = new ConcurrentDictionary<long, tb_ProductionDemandDetail>();

            //得到所有BOM相关的原料需求明细。
            //因为支持多个成品 的BOM分析，成品需求时间不一样。这里还要临时保存一下需求时间

            List<tb_BOM_SDetail> bomDetails = GetBomDetailsInfo(BOM_ID);
            foreach (tb_BOM_SDetail detail in bomDetails)
            {
                // tb_ProductionDemandDetail NewDetail = new tb_ProductionDemandDetail();
                //if (!AlreadyReducedQtyList.TryGetValue(NewDetail.ProdDetailID, out NewDetail))
                //{
                //    NewDetail = new tb_ProductionDemandDetail();
                //}

                tb_ProductionDemandDetail NewDetail = GetProductionDemandDetailFrom(SuggestBasedOn, detail.tb_proddetail.BOM_ID, Location_ID, RequirementDate, NeedQuantity, detail, PID, ref AlreadyReducedQtyList);
                if (NewDetail.BOM_ID.HasValue)
                {
                    var nextSublist = GetNextBOMInventoryInfo(SuggestBasedOn, NewDetail.NeedQuantity, NewDetail.RequirementDate, NewDetail.ID.Value, NewDetail.BOM_ID.Value, AlreadyReducedQtyList, NewDetail.Location_ID);
                    demandDetails.AddRange(nextSublist);
                }
                demandDetails.Add(NewDetail);
            }
            return demandDetails;
        }


        /// <summary>
        /// 生产采购建议,将缺少数量大于0的所有物料加载
        /// 采购建议 累计了相同的物料
        /// </summary>
        public List<tb_PurGoodsRecommendDetail> GeneratePurSuggestions(tb_ProductionDemand EditEntity, bool PurAllItem)
        {
            List<tb_ProductionDemandDetail> demandDetails = EditEntity.tb_ProductionDemandDetails;
            //要排除有BOM的，这些是要生产的。只要最基本的物料本身
            List<tb_ProductionDemandDetail> noBomList = new List<tb_ProductionDemandDetail>();
            if (PurAllItem)
            {
                //够数量的，也包含，因为实际可能不准。但是方便采购
                noBomList = demandDetails.Where(c => !c.BOM_ID.HasValue).ToList();
            }
            else
            {
                noBomList = demandDetails.Where(c => !c.BOM_ID.HasValue && c.MissingQuantity > 0).ToList();
            }
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            List<tb_PurGoodsRecommendDetail> PurDetails = mapper.Map<List<tb_PurGoodsRecommendDetail>>(noBomList);

            //需求 建议 请购 修正数据
            foreach (tb_PurGoodsRecommendDetail item in PurDetails)
            {
                tb_ProductionDemandDetail demandDetail = noBomList.FirstOrDefault(c => c.ID == item.PDCID_RowID && c.ProdDetailID == item.ProdDetailID && c.Location_ID == item.Location_ID);
                if (demandDetail != null)
                {
                    //0 毛需求 1 净需求
                    if (EditEntity.SuggestBasedOn == false)
                    {
                        item.RequirementQty = demandDetail.GrossRequirement;
                    }
                    else
                    {
                        item.RequirementQty = demandDetail.NetRequirement;
                    }
                    item.RecommendQty = item.RequirementQty;
                    item.ActualRequiredQty = demandDetail.NeedQuantity;
                }
            }

            List<tb_PurGoodsRecommendDetail> NewPurDetails = new List<tb_PurGoodsRecommendDetail>();

            //分组统计 采购建议 累计了相同的物料
            //这里的需求日期和为库位。来自于分析目标中行数据。目前认为是固定的一样的。
            var groupedPurDetails = PurDetails
                .GroupBy(pd => new { pd.ProdDetailID, pd.Location_ID, pd.RequirementDate }) // 根据指定的属性分组
                .Select(group => new
                {
                    ProdDetailID = group.Key.ProdDetailID,       // 分组键的产品ID
                    Location_ID = group.Key.Location_ID,         // 分组键的库位ID
                    RequirementDate = group.Key.RequirementDate, // 分组键的需求日期
                    // 累加数量，这里以ActualRequiredQty为例，你可以根据需要选择其他属性
                    TotalActualRequiredQty = group.Sum(pd => pd.ActualRequiredQty),
                    TotalRecommendQty = group.Sum(pd => pd.RecommendQty),
                    TotalRequirementQty = group.Sum(pd => pd.RequirementQty),
                })
                .ToList(); // 转换为List

            foreach (var group in groupedPurDetails)
            {
                tb_PurGoodsRecommendDetail newDetail = new tb_PurGoodsRecommendDetail();
                newDetail.ProdDetailID = group.ProdDetailID;
                newDetail.Location_ID = group.Location_ID;
                newDetail.RequirementDate = group.RequirementDate;

                newDetail.RequirementQty = group.TotalRequirementQty;
                newDetail.RecommendQty = group.TotalRecommendQty;
                newDetail.ActualRequiredQty = group.TotalActualRequiredQty;

                NewPurDetails.Add(newDetail);
            }
            return NewPurDetails;
        }

        ///// <summary>
        ///// 生成自制品建议（制令单）
        ///// 是否需要按树节点来生成。只取目标明细中所有的最高一级。按道理是这样的。 因为成品只需要做成品所有的制令单并且发所有物料。
        ///// </summary>
        //public List<tb_ProduceGoodsRecommendDetail> GenerateProductionSuggestions(List<tb_ProductionDemandDetail> demandDetails)
        //{
        //    IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
        //    List<tb_ProduceGoodsRecommendDetail> MaikingDetails = mapper.Map<List<tb_ProduceGoodsRecommendDetail>>(demandDetails);
        //    //有BOM的，这些是要生产的。
        //    return MaikingDetails.Where(c => c.BOM_ID.HasValue && c.RecommendQty > 0).ToList();
        //}




        /// <summary>
        /// 生成请购单草稿
        /// </summary>
        public tb_BuyingRequisition GenerateBuyingRequisition(tb_ProductionDemand demand, List<tb_PurGoodsRecommendDetail> PurDetails)
        {
            ReturnMainSubResults<tb_BuyingRequisition> rmr = new ReturnMainSubResults<tb_BuyingRequisition>();

            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            tb_BuyingRequisition BuyingRequisition = mapper.Map<tb_BuyingRequisition>(demand);
            List<tb_BuyingRequisitionDetail> BuyingDetails = mapper.Map<List<tb_BuyingRequisitionDetail>>(PurDetails);
            foreach (var item in BuyingDetails)
            {
                BuyingRequisition.Purpose = $"由{_appContext.CurUserInfo.Name}在生产需要分析{demand.PDNo}时自动生成";
                //设置一个默认的需求日期
                if (BuyingRequisition.RequirementDate == null && item.RequirementDate.HasValue)
                {
                    //请购单的需求时间 是原料。成品需要的时间要 ，还要 来料时间，生产时间
                    //定义一个10天生产时间，如果成品需求还长大于10，则否则
                    TimeSpan days = item.RequirementDate.Value - System.DateTime.Now.Date;
                    if (days.Days > 10)
                    {
                        //先给三天吧
                        BuyingRequisition.RequirementDate = item.RequirementDate.Value.AddDays(10 - 3);
                    }
                    else
                    {
                        //先给三天吧
                        BuyingRequisition.RequirementDate = System.DateTime.Now.AddDays(3);
                    }

                }
            }
            //没有经验通过下面先不计算

            BaseController<tb_BuyingRequisition> ctrBuy = _appContext.GetRequiredServiceByName<BaseController<tb_BuyingRequisition>>(typeof(tb_BuyingRequisition).Name + "Controller");
            BuyingRequisition.PuRequisitionNo = BizCodeGenerator.Instance.GetBizBillNo(BizType.请购单);
            BuyingRequisition.RefBillID = demand.PDID;
            BuyingRequisition.RefBillNO = demand.PDNo;
            BuyingRequisition.RefBizType = (int)BizType.需求分析;

            if (demand.tb_productionplan != null)
            {

                BuyingRequisition.DepartmentID = demand.tb_productionplan.DepartmentID;

            }
            BuyingRequisition.Employee_ID = _appContext.CurUserInfo.Id;

            BuyingRequisition.tb_BuyingRequisitionDetails = BuyingDetails;
            BuyingRequisition.ApplicationDate = System.DateTime.Now;
            BuyingRequisition.ApprovalOpinions = string.Empty;
            BuyingRequisition.ApprovalResults = null;
            BuyingRequisition.ApprovalStatus = null;
            BuyingRequisition.Approver_at = null;
            BuyingRequisition.Approver_by = null;
            if (BuyingRequisition.PuRequisition_ID == 0)
            {
                BusinessHelper.Instance.InitEntity(BuyingRequisition);
                BusinessHelper.Instance.InitStatusEntity(BuyingRequisition);
            }
            else
            {
                BusinessHelper.Instance.EditEntity(BuyingRequisition);
            }

            return BuyingRequisition;
        }


        [Obsolete]
        public async Task<ReturnMainSubResults<tb_BuyingRequisition>> GenerateBuyingRequisitionWithSave(tb_BuyingRequisition BuyingRequisition)
        {
            ReturnMainSubResults<tb_BuyingRequisition> rmr = new ReturnMainSubResults<tb_BuyingRequisition>();
            BaseController<tb_BuyingRequisition> ctrBuy = _appContext.GetRequiredServiceByName<BaseController<tb_BuyingRequisition>>(typeof(tb_BuyingRequisition).Name + "Controller");
            rmr = await ctrBuy.BaseSaveOrUpdateWithChild<tb_BuyingRequisition>(BuyingRequisition);
            return rmr;
          
        }
        

        //IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();



        /// <summary>
        /// 初始化制令单的数据，由制令单这边生成时候调用
        /// </summary>
        /// <param name="demand">需求分析表</param>
        /// <param name="MakingItem">要制作的目标，如果中间件，就会是指定一个，如果是上层模式，则是一组中最顶层那个行数据</param>
        /// <param name="needLoop">如果是循环，则是上层械，如果是否则是中间件式</param>
        /// <returns></returns>
        public async Task<tb_ManufacturingOrder> InitManufacturingOrder(tb_ProductionDemand demand,
            tb_ProduceGoodsRecommendDetail MakingItem,
             bool needLoop = false)
        {

            //需求分析单审核后才可以生成制令单，因为确定需求才生产
            //一个中间件的详情ID和PCID即行号一行，生成一个制作令单
            tb_ManufacturingOrder ManufacturingOrder = mapper.Map<tb_ManufacturingOrder>(MakingItem);
            ManufacturingOrder.PDCID = MakingItem.PDCID;

            #region 
            //tb_BOM_SController<tb_BOM_S> ctrBOM = _appContext.GetRequiredService<tb_BOM_SController<tb_BOM_S>>();
            //一次性查出。为了性能，如果是上层模式，则全部，如果是中间件模式，则只要查下一级
            List<tb_BOM_S> MediumBomInfoList = new List<tb_BOM_S>();
            if (needLoop)
            {
                long[] _bomIDs = demand.tb_ProduceGoodsRecommendDetails.Where(c => c.BOM_ID.HasValue).ToList().Select(c => c.BOM_ID.Value).ToArray();
                MediumBomInfoList = await _appContext.Db.CopyNew().Queryable<tb_BOM_S>()
                    .Includes(a => a.tb_proddetail, b => b.tb_Inventories)
                     .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail)
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_Inventories)
                    .Where(c => _bomIDs.Contains(c.BOM_ID)).ToListAsync();
            }
            else
            {
                //BOM不用带仓位
                long[] _bomIDs = demand.tb_ProduceGoodsRecommendDetails.Where(c => c.BOM_ID.HasValue && MakingItem.ProdDetailID == c.ProdDetailID && c.Location_ID == MakingItem.Location_ID).ToList().Select(c => c.BOM_ID.Value).ToArray();
                MediumBomInfoList = await _appContext.Db.CopyNew().Queryable<tb_BOM_S>()
                    .Includes(a => a.tb_proddetail, b => b.tb_Inventories)
                     .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_bom_s)
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail)
                        .Includes(a => a.tb_BOM_SDetails, b => b.tb_proddetail, c => c.tb_Inventories)
                    .Where(c => _bomIDs.Contains(c.BOM_ID)).ToListAsync();

            }
            //上面全查出后。找到中间件时，只会有一行数据。如果是上层式，则多行找到的是最上层。
            tb_BOM_S MakingItemBom = MediumBomInfoList.FirstOrDefault(c => c.BOM_ID == MakingItem.BOM_ID);

            //制令单的BOM依据也传过去
            ManufacturingOrder.tb_bom_s = MakingItemBom;

            BaseController<tb_ManufacturingOrder> ctrMaking = _appContext.GetRequiredServiceByName<BaseController<tb_ManufacturingOrder>>(typeof(tb_ManufacturingOrder).Name + "Controller");
            ManufacturingOrder.BOM_ID = MakingItemBom.BOM_ID;
            ManufacturingOrder.BOM_No = MakingItemBom.BOM_No;
            ManufacturingOrder.ProdDetailID = MakingItem.ProdDetailID;
            ManufacturingOrder.PDCID = MakingItem.PDCID;
            ManufacturingOrder.property = MakingItem.property;
            ManufacturingOrder.ManufacturingQty = MakingItem.RequirementQty;
            //加载刷新时没有值，需要手动赋值
            if (MakingItem.tb_proddetail == null)
            {
                var viewdetail = await _appContext.Db.CopyNew().Queryable<tb_ProdDetail>()
                    .Where(c => c.ProdDetailID == MakingItem.ProdDetailID )
                    .Includes(c => c.tb_prod)
                    .FirstAsync();
                MakingItem.tb_proddetail = viewdetail;
            }
            ManufacturingOrder.tb_proddetail = MakingItem.tb_proddetail;
            ManufacturingOrder.Specifications = MakingItem.tb_proddetail.tb_prod.Specifications;
            ManufacturingOrder.Unit_ID = MakingItem.tb_proddetail.tb_prod.Unit_ID;
            ManufacturingOrder.CNName = MakingItem.tb_proddetail.tb_prod.CNName;
            ManufacturingOrder.SKU = MakingItem.tb_proddetail.SKU;
            ManufacturingOrder.Notes = MakingItem.Summary;
            ManufacturingOrder.Type_ID = MakingItem.tb_proddetail.tb_prod.Type_ID;
            //人工成本 
            //ManufacturingOrder.LaborCost = MakingItem.l;
            // ManufacturingOrder.t = MakingItemBom.LaborCost;
            ManufacturingOrder.MONO = BizCodeGenerator.Instance.GetBizBillNo(BizType.制令单);
            ManufacturingOrder.PDID = demand.PDID;

            ManufacturingOrder.PDNO = demand.PDNo;
            ManufacturingOrder.Location_ID = MakingItem.Location_ID;
            ManufacturingOrder.QuantityDelivered = 0;
            //标记是不是上层驱动
            ManufacturingOrder.IncludeSubBOM = needLoop;
            //暂时认为一定有计划
            if (demand.tb_productionplan != null)
            {
                //ManufacturingOrder.DepartmentID = demand.tb_productionplan.DepartmentID;
                ManufacturingOrder.DepartmentID = null;//要人员来安排哪个生产 这里是指生产部门
            }
            if (demand.tb_productionplan.tb_saleorder == null)
            {
                demand.tb_productionplan.tb_saleorder = await _appContext.Db.CopyNew().Queryable<tb_SaleOrder>()
                    .Where(c => c.SOrder_ID == demand.tb_productionplan.SOrder_ID).FirstAsync();
            }

            if (demand.tb_productionplan.tb_saleorder != null)
            {
                ManufacturingOrder.CustomerVendor_ID = demand.tb_productionplan.tb_saleorder.CustomerVendor_ID;
                ManufacturingOrder.Priority = demand.tb_productionplan.tb_saleorder.OrderPriority;
                if (demand.tb_productionplan.tb_saleorder.tb_SaleOrderDetails != null)
                {
                    tb_SaleOrderDetail saleOrderDetail = demand.tb_productionplan.tb_saleorder.tb_SaleOrderDetails.FirstOrDefault(c => c.ProdDetailID == ManufacturingOrder.ProdDetailID
                    && c.Location_ID==ManufacturingOrder.Location_ID);
                    if (saleOrderDetail != null)
                    {
                        ManufacturingOrder.CustomerPartNo = saleOrderDetail.CustomerPartNo;
                    }
                }
                ManufacturingOrder.IsCustomizedOrder = demand.tb_productionplan.tb_saleorder.IsCustomizedOrder;
            }

            // ManufacturingOrder.PeopleQty = bom.PeopleQty * item.RecommendQty;
            ManufacturingOrder.Employee_ID = _appContext.CurUserInfo.Id;

            #region 明细

            //===============================================================================
            //如果循环，是上层模式时加载他下面的所有物料，是所有。
            //这里算出来的物料是库存不足的数据。再加上有BOM的除去顶层。看中间件是否数量全是自制。否则仓库也要发对应的半制程品
            List<tb_ManufacturingOrderDetail> AllMakingGoods = new List<tb_ManufacturingOrderDetail>();
            //这里只得到了除顶层的的有BOM的数据
            //先查出所有中单节点的BOM信息。


            List<tb_ManufacturingOrderDetail> MakingGoods = new List<tb_ManufacturingOrderDetail>();

            MakingGoods = await GetSubManufacturingOrderDetailLoop(demand, MediumBomInfoList, MakingItem, MakingItemBom, needLoop);

            ////这里是中间件ID
            //HashSet<long> excludedIds = new HashSet<long>(MakingGoods.Select(p => p.ProdDetailID));

            ////这里要以库存不足（包括了足的）为基准去生成发料明细
            //var SubItems = demand.tb_ProductionDemandDetails.Where(s => !excludedIds.Contains(s.ProdDetailID)).ToList();

            ////这里再取库存不足的，再加上有BOM的除去顶层。
            //List<tb_ManufacturingOrderDetail> MakingGoodsOther = mapper.Map<List<tb_ManufacturingOrderDetail>>(SubItems);

            ////要计算的情况
            //foreach (tb_ManufacturingOrderDetail MODetail in MakingGoodsOther)
            //{
            //    tb_BOM_SDetail child_bomDetail = bom.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == MODetail.ProdDetailID);
            //    //损耗量，影响领料数量？
            //    if (child_bomDetail.LossRate.HasValue)
            //    {
            //        MODetail.WastageQty = MODetail.ShouldSendQty * child_bomDetail.LossRate.Value;
            //    }
            //}

            AllMakingGoods.AddRange(MakingGoods);


            AllMakingGoods.Sort((p1, p2) =>
            {
                if (p1.ParentId != p2.ParentId)
                {
                    return p2.ParentId.CompareTo(p1.ParentId);
                }
                else if (p1.BOM_ID.HasValue && p2.BOM_ID.HasValue && p1.BOM_ID != p2.BOM_ID)
                {
                    return p2.BOM_ID.Value.CompareTo(p1.BOM_ID);
                }
                else if (p1.ShouldSendQty != p2.ShouldSendQty)
                {
                    return p2.ShouldSendQty.CompareTo(p1.ShouldSendQty);
                }
                else return 0;
            });

            ManufacturingOrder.tb_ManufacturingOrderDetails = AllMakingGoods;

            #endregion

            //工时，是不是有组装工时和加工工时，所有中间式的就是BOM本身的，上层式的。则是把下级所有工时都加上
            //    人工也是一样。！！TODO:这里要做
            if (MakingItemBom.WorkingHour.HasValue)
            {
                ManufacturingOrder.WorkingHour = MakingItemBom.WorkingHour.Value * ManufacturingOrder.ManufacturingQty;
                ManufacturingOrder.PreEndDate = System.DateTime.Now.AddDays(1).AddHours(ManufacturingOrder.WorkingHour.ToInt());
            }
            if (MakingItemBom.MachineHour.HasValue)
            {
                ManufacturingOrder.MachineHour = MakingItemBom.MachineHour.Value * ManufacturingOrder.ManufacturingQty;
                ManufacturingOrder.PreEndDate = System.DateTime.Now.AddDays(1).AddHours(ManufacturingOrder.MachineHour.ToInt());
            }
            //这里所有工时 。成本都按计划数量来算。到缴库时才按实际数量来算
            ManufacturingOrder.PeopleQty = MakingItemBom.PeopleQty * ManufacturingOrder.ManufacturingQty;
            //ManufacturingOrder.TotalMaterialCost = MakingItemBom.TotalMaterialCost * ManufacturingOrder.ManufacturingQty;
            ManufacturingOrder.TotalMaterialCost = AllMakingGoods.Sum(c => c.SubtotalUnitCost); //MakingItemBom.TotalMaterialCost * ManufacturingOrder.ManufacturingQty
            if (ManufacturingOrder.IsOutSourced)
            {
                ManufacturingOrder.ApportionedCost = MakingItemBom.OutApportionedCost * ManufacturingOrder.ManufacturingQty;
                ManufacturingOrder.TotalManuFee = MakingItemBom.TotalOutManuCost * ManufacturingOrder.ManufacturingQty;
                //MakingItemBom.OutProductionAllCosts bom中的总自产  自外发  在这里不用。只是用于配方中的查看
                //这里是 分摊  加工费和材料一起加总算
            }
            else
            {
                ManufacturingOrder.ApportionedCost = MakingItemBom.SelfApportionedCost * ManufacturingOrder.ManufacturingQty;
                ManufacturingOrder.TotalManuFee = MakingItemBom.TotalSelfManuCost * ManufacturingOrder.ManufacturingQty;

            }
            ManufacturingOrder.TotalProductionCost = ManufacturingOrder.TotalMaterialCost + ManufacturingOrder.ApportionedCost + ManufacturingOrder.TotalManuFee;

            ManufacturingOrder.ApprovalOpinions = string.Empty;
            ManufacturingOrder.ApprovalResults = null;
            ManufacturingOrder.ApprovalStatus = null;
            ManufacturingOrder.Approver_at = null;
            ManufacturingOrder.Approver_by = null;
            if (ManufacturingOrder.MOID == 0)
            {
                BusinessHelper.Instance.InitEntity(ManufacturingOrder);
                BusinessHelper.Instance.InitStatusEntity(ManufacturingOrder);
            }
            else
            {
                BusinessHelper.Instance.EditEntity(ManufacturingOrder);
            }

            #endregion
            return ManufacturingOrder;
        }



        /// <summary>
        /// 生成制令单明细
        /// 如果是上层驱动时。比方裸机中会用到一个型号的线。成品中也配一条相同的线。这时应该数量合并
        /// 实际发料时不会分开发。会一起发。数量也好算
        /// </summary>
        /// <param name="MediumBomInfoList">一个BOM集合，如果是中间件。则只有一级，如果是上层驱动则多级</param>
        /// <param name="MakingItem">这个是自制品表中的，选择中的一行。多行，则是最顶级的那一行</param>
        /// <param name="MakingItemBom">选中的bom</param>
        /// <param name="needLoop">如果上层模式，为true,否则是中间件 就不用循环</param>
        /// <returns></returns>

        private async Task<List<tb_ManufacturingOrderDetail>> GetSubManufacturingOrderDetailLoop(tb_ProductionDemand demand,
            List<tb_BOM_S> MediumBomInfoList, tb_ProduceGoodsRecommendDetail MakingItem, tb_BOM_S MakingItemBom,
            bool needLoop = true
            )
        {
            List<tb_ManufacturingOrderDetail> AllMakingGoods = new List<tb_ManufacturingOrderDetail>();
            //找到上层的次级 ,是通过BOM明细找
            foreach (var mItem in MakingItemBom.tb_BOM_SDetails)
            {
                //==通过库存不足那边的转换一下，再修正重要的值
                var mpdItem = demand.tb_ProductionDemandDetails.FirstOrDefault(c => c.ProdDetailID == mItem.ProdDetailID);
                tb_ManufacturingOrderDetail mItemGoods = mapper.Map<tb_ManufacturingOrderDetail>(mpdItem);
                if (mItemGoods == null)
                {
                    //如果需求分析单已经建好保存后，再修改BOM添加了材料后。按BOM明细到分析单明细中找不到时。则直接由BOM明细生成制令单明细行
                    mItemGoods = mapper.Map<tb_ManufacturingOrderDetail>(mItem);
                    mItemGoods.Location_ID = MakingItem.Location_ID;
                }
                //===
                mItemGoods.ActualSentQty = 0;
                //不管是中间件还是原料都有上级BOM。
                //因为可以选择上层驱动。这时物料明细中就要保存他自己的上级。这样才能计划出产出量这些数据。
                //所属配方,制令单表中也有。是因为给了一个默认。如果是上层驱动则从裸机机取到明细。这时就是自己对应该自己的。
                mItemGoods.Prelevel_BOM_Desc = MakingItemBom.BOM_Name;//TODO要删除
                mItemGoods.Prelevel_BOM_ID = MakingItemBom.BOM_ID;//TODO要删除
                mItemGoods.BOM_NO = MakingItemBom.BOM_No;
                mItemGoods.BOM_ID = MakingItemBom.BOM_ID;

                //找次级中间件 在所有BOM中去找，通过目标BOM（成品）的明细对应的产品的BOMID,即下级BOM
                tb_BOM_S MediumBomInfo = MediumBomInfoList.FirstOrDefault(c => c.BOM_ID == mItem.tb_proddetail.BOM_ID);
                //中间件
                if (MediumBomInfo != null)
                {
                    //次级中间件一定是关键的必需的。
                    mItemGoods.IsKeyMaterial = true;
                    //中间有BOM的制成品,只在子循环中引用
                    mItemGoods.CurrentIinventory = MediumBomInfo.tb_proddetail.tb_Inventories.Where(c => c.Location_ID == MakingItem.Location_ID).Sum(i => i.Quantity);
                    mItemGoods.UnitCost = MediumBomInfo.tb_proddetail.tb_Inventories.Where(c => c.Location_ID == MakingItem.Location_ID).Sum(i => i.Inv_Cost);

                    //找下一级的材料。当前级就不需要。否则将当前级认为是中间半成品。要提供数量
                    if (needLoop)
                    {
                        //找下一级
                        //由UI上选择。这里不给值
                        mItemGoods.IsExternalProduce = null;
                        var NeedMakingItem = demand.tb_ProduceGoodsRecommendDetails.FirstOrDefault(c => c.ProdDetailID == MediumBomInfo.ProdDetailID);
                        List<tb_ManufacturingOrderDetail> nextList = await GetSubManufacturingOrderDetailLoop(demand, MediumBomInfoList, NeedMakingItem, MediumBomInfo, needLoop);
                        AllMakingGoods.AddRange(nextList);
                    }
                    else
                    {
                        //直接发，不存在处发还是自制
                        mItemGoods.IsExternalProduce = null;
                        mItemGoods.IsKeyMaterial = true;
                        #region  中间件时,下级材料不发，直接认为中间件能提供，发半成品，如果制成品数量小于建议量则要发料
                        //中间件也当原，料发应该发的数量，用量*他上级的需求量
                        tb_BOM_SDetail child_bomDetail = MakingItemBom.tb_BOM_SDetails.FirstOrDefault(c => c.ProdDetailID == mItem.ProdDetailID);
                        //损耗量，影响领料数量？

                        mItemGoods.WastageQty = mItemGoods.ShouldSendQty * child_bomDetail.LossRate;

                        //按自制品建议来的。这里与净毛需求无关，
                        //请制量，因为只有中间件是一层一行。所以直接就是上面传过来的makingitem
                        mItemGoods.ShouldSendQty = MakingItem.RequirementQty * child_bomDetail.UsedQty;
                        // 没有bom就为空。反之 要么自制，要么外发
                        mItemGoods.IsExternalProduce = null;
                        //IncludeSubBOM ==true 制令单主表中应该是上层驱动 
                        mItemGoods.BOM_ID = MediumBomInfo.BOM_ID;
                        mItemGoods.BOM_NO = MediumBomInfo.BOM_No;

                        mItemGoods.Prelevel_BOM_Desc = MediumBomInfo.BOM_Name;//TODO要删除
                        mItemGoods.Prelevel_BOM_ID = MediumBomInfo.BOM_ID;//TODO要删除
                        mItemGoods.BOM_NO = MediumBomInfo.BOM_No;
                        mItemGoods.BOM_ID = MediumBomInfo.BOM_ID;

                        AllMakingGoods.Add(mItemGoods);
                        #endregion
                    }
                }
                else
                {
                    //直接就是下级的原料
                    //损耗量，影响领料数量？
                    mItemGoods.IsKeyMaterial = mItem.IsKeyMaterial;
                    mItemGoods.WastageQty = mItemGoods.ShouldSendQty * mItem.LossRate;
                    //中间件，直接就是上级的请制量作为标准*用量
                    mItemGoods.ShouldSendQty = MakingItem.RequirementQty * mItem.UsedQty;
                    mItemGoods.CurrentIinventory = mItem.tb_proddetail.tb_Inventories.Where(c => c.Location_ID == MakingItem.Location_ID).Sum(i => i.Quantity);

                    //制令单的成本来源于实时成本
                    mItemGoods.UnitCost = mItem.tb_proddetail.tb_Inventories.Where(c => c.Location_ID == MakingItem.Location_ID).Sum(i => i.Inv_Cost);
                    //IncludeSubBOM ==true 制令单主表中应该是上层驱动 

                    mItemGoods.BOM_ID = MakingItemBom.BOM_ID;
                    mItemGoods.BOM_NO = MakingItemBom.BOM_No;

                    // 没有bom就为空。反之 要么自制，要么外发
                    mItemGoods.IsExternalProduce = null;
                    AllMakingGoods.Add(mItemGoods);
                }

                //这些个成本是按计划来算。实际在缴款时。按实际缴款数量来处理：如制令单 100个，实际只做了50个，
                //则成本为50*(成本/100)
                mItemGoods.SubtotalUnitCost = mItemGoods.UnitCost * mItemGoods.ShouldSendQty;
            }

            return AllMakingGoods;
        }



    }
}
