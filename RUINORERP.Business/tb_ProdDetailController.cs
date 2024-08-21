
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/07/2024 19:06:32
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
using RUINOR.Core;
using RUINORERP.Common.Helper;

namespace RUINORERP.Business
{
    /// <summary>
    /// 产品详细表
    /// </summary>
    public partial class tb_ProdDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdDetailServices _tb_ProdDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdDetailController(ILogger<tb_ProdDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdDetailServices tb_ProdDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdDetailServices = tb_ProdDetailServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_ProdDetail info)
        {
            tb_ProdDetailValidator validator = new tb_ProdDetailValidator();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        
        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public override bool ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).Any();
        }
      
        
        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_ProdDetail>> SaveOrUpdate(tb_ProdDetail entity)
        {
            ReturnResults<tb_ProdDetail> rr = new ReturnResults<tb_ProdDetail>();
            tb_ProdDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProdDetailID > 0)
                {
                    bool rs = await _tb_ProdDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
                }

                rr.ReturnObject = Returnobj;
                rr.Succeeded = true;
                entity.ActionStatus = ActionStatus.无操作;
            }
            catch (Exception ex)
            {
                ////这里需要进一步优化处理？
                throw ex;
            }
            return rr;
        }
        
        
        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async override Task<ReturnResults<T>>  BaseSaveOrUpdate(T model)
        {
            ReturnResults<T> rr = new ReturnResults<T>();
            tb_ProdDetail entity = model as tb_ProdDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProdDetailID > 0)
                {
                    bool rs = await _tb_ProdDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
                }

                rr.ReturnObject = Returnobj;
                rr.Succeeded = true;
                entity.ActionStatus = ActionStatus.无操作;
            }
            catch (Exception ex)
            {
                ////这里需要进一步优化处理？
                throw ex;
            }
            return rr;
        }
        
        public async override Task<List<T>> BaseQueryAsync(string wheresql) 
        {
            List<T> list = await _tb_ProdDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdDetail entity = item as tb_ProdDetail;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_ProdDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdDetail entity = item as tb_ProdDetail;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_ProdDetail entity = model as tb_ProdDetail;
            bool rs = await _tb_ProdDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdDetail> entitys = models as List<tb_ProdDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ProdDetailID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ProdDetailValidator validator = new tb_ProdDetailValidator();
            ValidationResult results = validator.Validate(info as tb_ProdDetail);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }
        
        public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model) where C : class
        {
            bool rs = false;
            Command command = new Command();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            try
            {
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_ProdDetail entity = model as tb_ProdDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.ProdDetailID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdDetail>(entity as tb_ProdDetail)
                        .Include(m => m.tb_ProdSplitDetails)
                    .Include(m => m.tb_StockOutDetails)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ProduceGoodsRecommendDetails)
                    .Include(m => m.tb_SaleOutReDetails)
                    .Include(m => m.tb_MaterialReturnDetails)
                    .Include(m => m.tb_ProductionPlanDetails)
                    .Include(m => m.tb_ProdMergeDetails)
                    .Include(m => m.tb_FinishedGoodsInvDetails)
                    .Include(m => m.tb_ProdReturningDetails)
                    .Include(m => m.tb_ReturnDetails)
                    .Include(m => m.tb_PriceRecords)
                    .Include(m => m.tb_StocktakeDetails)
                    .Include(m => m.tb_SaleOrderDetails)
                    .Include(m => m.tb_BOM_SDetailSecondaries)
                    .Include(m => m.tb_PurEntryDetails)
                    .Include(m => m.tb_Prod_Attr_Relations)
                    .Include(m => m.tb_ProductionDemandDetails)
                    .Include(m => m.tb_BOM_SDetails)
                    .Include(m => m.tb_ManufacturingOrderDetails)
                    .Include(m => m.tb_ProdBorrowingDetails)
                    .Include(m => m.tb_ProdSplits)
                    .Include(m => m.tb_PurEntryReDetails)
                    .Include(m => m.tb_SaleOutDetails)
                    .Include(m => m.tb_Packings)
                    .Include(m => m.tb_PurOrderDetails)
                    .Include(m => m.tb_ProductionDemandTargetDetails)
                    .Include(m => m.tb_ProdMerges)
                    .Include(m => m.tb_BOM_Ss)
                    .Include(m => m.tb_PurOrderReDetails)
                    .Include(m => m.tb_StockInDetails)
                    .Include(m => m.tb_BuyingRequisitionDetails)
                    .Include(m => m.tb_MaterialRequisitionDetails)
                    .Include(m => m.tb_PurGoodsRecommendDetails)
                    .Include(m => m.tb_ProdBundleDetails)
                    .Include(m => m.tb_Inventories)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdDetail>(entity as tb_ProdDetail)
                .Include(m => m.tb_ProdSplitDetails)
                .Include(m => m.tb_StockOutDetails)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ProduceGoodsRecommendDetails)
                .Include(m => m.tb_SaleOutReDetails)
                .Include(m => m.tb_MaterialReturnDetails)
                .Include(m => m.tb_ProductionPlanDetails)
                .Include(m => m.tb_ProdMergeDetails)
                .Include(m => m.tb_FinishedGoodsInvDetails)
                .Include(m => m.tb_ProdReturningDetails)
                .Include(m => m.tb_ReturnDetails)
                .Include(m => m.tb_PriceRecords)
                .Include(m => m.tb_StocktakeDetails)
                .Include(m => m.tb_SaleOrderDetails)
                .Include(m => m.tb_BOM_SDetailSecondaries)
                .Include(m => m.tb_PurEntryDetails)
                .Include(m => m.tb_Prod_Attr_Relations)
                .Include(m => m.tb_ProductionDemandDetails)
                .Include(m => m.tb_BOM_SDetails)
                .Include(m => m.tb_ManufacturingOrderDetails)
                .Include(m => m.tb_ProdBorrowingDetails)
                .Include(m => m.tb_ProdSplits)
                .Include(m => m.tb_PurEntryReDetails)
                .Include(m => m.tb_SaleOutDetails)
                .Include(m => m.tb_Packings)
                .Include(m => m.tb_PurOrderDetails)
                .Include(m => m.tb_ProductionDemandTargetDetails)
                .Include(m => m.tb_ProdMerges)
                .Include(m => m.tb_BOM_Ss)
                .Include(m => m.tb_PurOrderReDetails)
                .Include(m => m.tb_StockInDetails)
                .Include(m => m.tb_BuyingRequisitionDetails)
                .Include(m => m.tb_MaterialRequisitionDetails)
                .Include(m => m.tb_PurGoodsRecommendDetails)
                .Include(m => m.tb_ProdBundleDetails)
                .Include(m => m.tb_Inventories)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProdDetailID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                //_logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                // rr.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>()
                                .Includes(m => m.tb_ProdSplitDetails)
                        .Includes(m => m.tb_StockOutDetails)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ProduceGoodsRecommendDetails)
                        .Includes(m => m.tb_SaleOutReDetails)
                        .Includes(m => m.tb_MaterialReturnDetails)
                        .Includes(m => m.tb_ProductionPlanDetails)
                        .Includes(m => m.tb_ProdMergeDetails)
                        .Includes(m => m.tb_FinishedGoodsInvDetails)
                        .Includes(m => m.tb_ProdReturningDetails)
                        .Includes(m => m.tb_ReturnDetails)
                        .Includes(m => m.tb_PriceRecords)
                        .Includes(m => m.tb_StocktakeDetails)
                        .Includes(m => m.tb_SaleOrderDetails)
                        .Includes(m => m.tb_BOM_SDetailSecondaries)
                        .Includes(m => m.tb_PurEntryDetails)
                        .Includes(m => m.tb_Prod_Attr_Relations)
                        .Includes(m => m.tb_ProductionDemandDetails)
                        .Includes(m => m.tb_BOM_SDetails)
                        .Includes(m => m.tb_ManufacturingOrderDetails)
                        .Includes(m => m.tb_ProdBorrowingDetails)
                        .Includes(m => m.tb_ProdSplits)
                        .Includes(m => m.tb_PurEntryReDetails)
                        .Includes(m => m.tb_SaleOutDetails)
                        .Includes(m => m.tb_Packings)
                        .Includes(m => m.tb_PurOrderDetails)
                        .Includes(m => m.tb_ProductionDemandTargetDetails)
                        .Includes(m => m.tb_ProdMerges)
                        .Includes(m => m.tb_BOM_Ss)
                        .Includes(m => m.tb_PurOrderReDetails)
                        .Includes(m => m.tb_StockInDetails)
                        .Includes(m => m.tb_BuyingRequisitionDetails)
                        .Includes(m => m.tb_MaterialRequisitionDetails)
                        .Includes(m => m.tb_PurGoodsRecommendDetails)
                        .Includes(m => m.tb_ProdBundleDetails)
                        .Includes(m => m.tb_Inventories)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdDetail entity = model as tb_ProdDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdDetail>(m => m.ProdDetailID== entity.ProdDetailID)
                                .Include(m => m.tb_ProdSplitDetails)
                        .Include(m => m.tb_StockOutDetails)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ProduceGoodsRecommendDetails)
                        .Include(m => m.tb_SaleOutReDetails)
                        .Include(m => m.tb_MaterialReturnDetails)
                        .Include(m => m.tb_ProductionPlanDetails)
                        .Include(m => m.tb_ProdMergeDetails)
                        .Include(m => m.tb_FinishedGoodsInvDetails)
                        .Include(m => m.tb_ProdReturningDetails)
                        .Include(m => m.tb_ReturnDetails)
                        .Include(m => m.tb_PriceRecords)
                        .Include(m => m.tb_StocktakeDetails)
                        .Include(m => m.tb_SaleOrderDetails)
                        .Include(m => m.tb_BOM_SDetailSecondaries)
                        .Include(m => m.tb_PurEntryDetails)
                        .Include(m => m.tb_Prod_Attr_Relations)
                        .Include(m => m.tb_ProductionDemandDetails)
                        .Include(m => m.tb_BOM_SDetails)
                        .Include(m => m.tb_ManufacturingOrderDetails)
                        .Include(m => m.tb_ProdBorrowingDetails)
                        .Include(m => m.tb_ProdSplits)
                        .Include(m => m.tb_PurEntryReDetails)
                        .Include(m => m.tb_SaleOutDetails)
                        .Include(m => m.tb_Packings)
                        .Include(m => m.tb_PurOrderDetails)
                        .Include(m => m.tb_ProductionDemandTargetDetails)
                        .Include(m => m.tb_ProdMerges)
                        .Include(m => m.tb_BOM_Ss)
                        .Include(m => m.tb_PurOrderReDetails)
                        .Include(m => m.tb_StockInDetails)
                        .Include(m => m.tb_BuyingRequisitionDetails)
                        .Include(m => m.tb_MaterialRequisitionDetails)
                        .Include(m => m.tb_PurGoodsRecommendDetails)
                        .Include(m => m.tb_ProdBundleDetails)
                        .Include(m => m.tb_Inventories)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdDetail AddReEntity(tb_ProdDetail entity)
        {
            tb_ProdDetail AddEntity =  _tb_ProdDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdDetail> AddReEntityAsync(tb_ProdDetail entity)
        {
            tb_ProdDetail AddEntity = await _tb_ProdDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdDetail entity)
        {
            long id = await _tb_ProdDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdDetail> infos)
        {
            List<long> ids = await _tb_ProdDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdDetail entity)
        {
            bool rs = await _tb_ProdDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdDetail entity)
        {
            bool rs = await _tb_ProdDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdDetail>> QueryAsync()
        {
            List<tb_ProdDetail> list = await  _tb_ProdDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProdDetail> Query()
        {
            List<tb_ProdDetail> list =  _tb_ProdDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProdDetail> Query(string wheresql)
        {
            List<tb_ProdDetail> list =  _tb_ProdDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdDetail>> QueryAsync(string wheresql) 
        {
            List<tb_ProdDetail> list = await _tb_ProdDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdDetail>> QueryAsync(Expression<Func<tb_ProdDetail, bool>> exp)
        {
            List<tb_ProdDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdDetail>> QueryByNavAsync()
        {
            List<tb_ProdDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>()
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_prod )
                                            .Includes(t => t.tb_ProdSplitDetails )
                                .Includes(t => t.tb_StockOutDetails )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                                .Includes(t => t.tb_MaterialReturnDetails )
                                .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_ProdMergeDetails )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_ProdReturningDetails )
                                .Includes(t => t.tb_ReturnDetails )
                                .Includes(t => t.tb_PriceRecords )
                                .Includes(t => t.tb_StocktakeDetails )
                                .Includes(t => t.tb_SaleOrderDetails )
                                .Includes(t => t.tb_BOM_SDetailSecondaries )
                                .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_Prod_Attr_Relations )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_BOM_SDetails )
                                .Includes(t => t.tb_ManufacturingOrderDetails )
                                .Includes(t => t.tb_ProdBorrowingDetails )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_PurEntryReDetails )
                                .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_Packings )
                                .Includes(t => t.tb_PurOrderDetails )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_ProdMerges )
                                .Includes(t => t.tb_BOM_Ss )
                                .Includes(t => t.tb_PurOrderReDetails )
                                .Includes(t => t.tb_StockInDetails )
                                .Includes(t => t.tb_BuyingRequisitionDetails )
                                .Includes(t => t.tb_MaterialRequisitionDetails )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                                .Includes(t => t.tb_ProdBundleDetails )
                                .Includes(t => t.tb_Inventories )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdDetail>> QueryByNavAsync(Expression<Func<tb_ProdDetail, bool>> exp)
        {
            List<tb_ProdDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>().Where(exp)
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_prod )
                                            .Includes(t => t.tb_ProdSplitDetails )
                                .Includes(t => t.tb_StockOutDetails )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                                .Includes(t => t.tb_MaterialReturnDetails )
                                .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_ProdMergeDetails )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_ProdReturningDetails )
                                .Includes(t => t.tb_ReturnDetails )
                                .Includes(t => t.tb_PriceRecords )
                                .Includes(t => t.tb_StocktakeDetails )
                                .Includes(t => t.tb_SaleOrderDetails )
                                .Includes(t => t.tb_BOM_SDetailSecondaries )
                                .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_Prod_Attr_Relations )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_BOM_SDetails )
                                .Includes(t => t.tb_ManufacturingOrderDetails )
                                .Includes(t => t.tb_ProdBorrowingDetails )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_PurEntryReDetails )
                                .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_Packings )
                                .Includes(t => t.tb_PurOrderDetails )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_ProdMerges )
                                .Includes(t => t.tb_BOM_Ss )
                                .Includes(t => t.tb_PurOrderReDetails )
                                .Includes(t => t.tb_StockInDetails )
                                .Includes(t => t.tb_BuyingRequisitionDetails )
                                .Includes(t => t.tb_MaterialRequisitionDetails )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                                .Includes(t => t.tb_ProdBundleDetails )
                                .Includes(t => t.tb_Inventories )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdDetail> QueryByNav(Expression<Func<tb_ProdDetail, bool>> exp)
        {
            List<tb_ProdDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>().Where(exp)
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_prod )
                                        .Includes(t => t.tb_ProdSplitDetails )
                            .Includes(t => t.tb_StockOutDetails )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                            .Includes(t => t.tb_MaterialReturnDetails )
                            .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_ProdMergeDetails )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_ProdReturningDetails )
                            .Includes(t => t.tb_ReturnDetails )
                            .Includes(t => t.tb_PriceRecords )
                            .Includes(t => t.tb_StocktakeDetails )
                            .Includes(t => t.tb_SaleOrderDetails )
                            .Includes(t => t.tb_BOM_SDetailSecondaries )
                            .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_Prod_Attr_Relations )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_BOM_SDetails )
                            .Includes(t => t.tb_ManufacturingOrderDetails )
                            .Includes(t => t.tb_ProdBorrowingDetails )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_PurEntryReDetails )
                            .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_Packings )
                            .Includes(t => t.tb_PurOrderDetails )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_ProdMerges )
                            .Includes(t => t.tb_BOM_Ss )
                            .Includes(t => t.tb_PurOrderReDetails )
                            .Includes(t => t.tb_StockInDetails )
                            .Includes(t => t.tb_BuyingRequisitionDetails )
                            .Includes(t => t.tb_MaterialRequisitionDetails )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                            .Includes(t => t.tb_ProdBundleDetails )
                            .Includes(t => t.tb_Inventories )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdDetail>().Where(w => w.ProdDetailID == (long)id)
                             .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_prod )
                                        .Includes(t => t.tb_ProdSplitDetails )
                            .Includes(t => t.tb_StockOutDetails )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                            .Includes(t => t.tb_MaterialReturnDetails )
                            .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_ProdMergeDetails )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_ProdReturningDetails )
                            .Includes(t => t.tb_ReturnDetails )
                            .Includes(t => t.tb_PriceRecords )
                            .Includes(t => t.tb_StocktakeDetails )
                            .Includes(t => t.tb_SaleOrderDetails )
                            .Includes(t => t.tb_BOM_SDetailSecondaries )
                            .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_Prod_Attr_Relations )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_BOM_SDetails )
                            .Includes(t => t.tb_ManufacturingOrderDetails )
                            .Includes(t => t.tb_ProdBorrowingDetails )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_PurEntryReDetails )
                            .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_Packings )
                            .Includes(t => t.tb_PurOrderDetails )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_ProdMerges )
                            .Includes(t => t.tb_BOM_Ss )
                            .Includes(t => t.tb_PurOrderReDetails )
                            .Includes(t => t.tb_StockInDetails )
                            .Includes(t => t.tb_BuyingRequisitionDetails )
                            .Includes(t => t.tb_MaterialRequisitionDetails )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                            .Includes(t => t.tb_ProdBundleDetails )
                            .Includes(t => t.tb_Inventories )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



