
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:48
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
    /// 库位表
    /// </summary>
    public partial class tb_LocationController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_LocationServices _tb_LocationServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_LocationController(ILogger<tb_LocationController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_LocationServices tb_LocationServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_LocationServices = tb_LocationServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_Location info)
        {
            tb_LocationValidator validator = new tb_LocationValidator();
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
        public async Task<ReturnResults<tb_Location>> SaveOrUpdate(tb_Location entity)
        {
            ReturnResults<tb_Location> rr = new ReturnResults<tb_Location>();
            tb_Location Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Location_ID > 0)
                {
                    bool rs = await _tb_LocationServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_LocationServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
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
            tb_Location entity = model as tb_Location;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Location_ID > 0)
                {
                    bool rs = await _tb_LocationServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_LocationServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
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
            List<T> list = await _tb_LocationServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Location entity = item as tb_Location;
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
            List<T> list = await _tb_LocationServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Location entity = item as tb_Location;
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
            tb_Location entity = model as tb_Location;
            bool rs = await _tb_LocationServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Location>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Location> entitys = models as List<tb_Location>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Location>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Location_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Location>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_LocationValidator validator = new tb_LocationValidator();
            ValidationResult results = validator.Validate(info as tb_Location);
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
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_Location entity = model as tb_Location;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Location_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Location>(entity as tb_Location)
                        .Include(m => m.tb_ProdSplitDetails)
                    .Include(m => m.tb_ProductionPlanDetails)
                    .Include(m => m.tb_StockOutDetails)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ProduceGoodsRecommendDetails)
                    .Include(m => m.tb_SaleOutReDetails)
                    .Include(m => m.tb_Stocktakes)
                    .Include(m => m.tb_MaterialReturnDetails)
                    .Include(m => m.tb_ProdMergeDetails)
                    .Include(m => m.tb_FinishedGoodsInvDetails)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_ProdReturningDetails)
                    .Include(m => m.tb_MaterialReturns)
                    .Include(m => m.tb_BOM_SDetailSecondaries)
                    .Include(m => m.tb_PurEntryDetails)
                    .Include(m => m.tb_StorageRacks)
                    .Include(m => m.tb_ProductionDemandDetails)
                    .Include(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                    .Include(m => m.tb_ManufacturingOrderDetails)
                    .Include(m => m.tb_SaleOrderDetails)
                    .Include(m => m.tb_ProdBorrowingDetails)
                    .Include(m => m.tb_ProdSplits)
                    .Include(m => m.tb_BuyingRequisitions)
                    .Include(m => m.tb_PurEntryReDetails)
                    .Include(m => m.tb_SaleOutDetails)
                    .Include(m => m.tb_PurOrderDetails)
                    .Include(m => m.tb_ProductionDemandTargetDetails)
                    .Include(m => m.tb_ProdMerges)
                    .Include(m => m.tb_Inventories)
                    .Include(m => m.tb_PurOrderReDetails)
                    .Include(m => m.tb_MaterialRequisitions)
                    .Include(m => m.tb_StockInDetails)
                    .Include(m => m.tb_MaterialRequisitionDetails)
                    .Include(m => m.tb_PurGoodsRecommendDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Location>(entity as tb_Location)
                .Include(m => m.tb_ProdSplitDetails)
                .Include(m => m.tb_ProductionPlanDetails)
                .Include(m => m.tb_StockOutDetails)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ProduceGoodsRecommendDetails)
                .Include(m => m.tb_SaleOutReDetails)
                .Include(m => m.tb_Stocktakes)
                .Include(m => m.tb_MaterialReturnDetails)
                .Include(m => m.tb_ProdMergeDetails)
                .Include(m => m.tb_FinishedGoodsInvDetails)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_ProdReturningDetails)
                .Include(m => m.tb_MaterialReturns)
                .Include(m => m.tb_BOM_SDetailSecondaries)
                .Include(m => m.tb_PurEntryDetails)
                .Include(m => m.tb_StorageRacks)
                .Include(m => m.tb_ProductionDemandDetails)
                .Include(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                .Include(m => m.tb_ManufacturingOrderDetails)
                .Include(m => m.tb_SaleOrderDetails)
                .Include(m => m.tb_ProdBorrowingDetails)
                .Include(m => m.tb_ProdSplits)
                .Include(m => m.tb_BuyingRequisitions)
                .Include(m => m.tb_PurEntryReDetails)
                .Include(m => m.tb_SaleOutDetails)
                .Include(m => m.tb_PurOrderDetails)
                .Include(m => m.tb_ProductionDemandTargetDetails)
                .Include(m => m.tb_ProdMerges)
                .Include(m => m.tb_Inventories)
                .Include(m => m.tb_PurOrderReDetails)
                .Include(m => m.tb_MaterialRequisitions)
                .Include(m => m.tb_StockInDetails)
                .Include(m => m.tb_MaterialRequisitionDetails)
                .Include(m => m.tb_PurGoodsRecommendDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Location_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Location>()
                                .Includes(m => m.tb_ProdSplitDetails)
                        .Includes(m => m.tb_ProductionPlanDetails)
                        .Includes(m => m.tb_StockOutDetails)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ProduceGoodsRecommendDetails)
                        .Includes(m => m.tb_SaleOutReDetails)
                        .Includes(m => m.tb_Stocktakes)
                        .Includes(m => m.tb_MaterialReturnDetails)
                        .Includes(m => m.tb_ProdMergeDetails)
                        .Includes(m => m.tb_FinishedGoodsInvDetails)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_ProdReturningDetails)
                        .Includes(m => m.tb_MaterialReturns)
                        .Includes(m => m.tb_BOM_SDetailSecondaries)
                        .Includes(m => m.tb_PurEntryDetails)
                        .Includes(m => m.tb_StorageRacks)
                        .Includes(m => m.tb_ProductionDemandDetails)
                        .Includes(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                        .Includes(m => m.tb_ManufacturingOrderDetails)
                        .Includes(m => m.tb_SaleOrderDetails)
                        .Includes(m => m.tb_ProdBorrowingDetails)
                        .Includes(m => m.tb_ProdSplits)
                        .Includes(m => m.tb_BuyingRequisitions)
                        .Includes(m => m.tb_PurEntryReDetails)
                        .Includes(m => m.tb_SaleOutDetails)
                        .Includes(m => m.tb_PurOrderDetails)
                        .Includes(m => m.tb_ProductionDemandTargetDetails)
                        .Includes(m => m.tb_ProdMerges)
                        .Includes(m => m.tb_Inventories)
                        .Includes(m => m.tb_PurOrderReDetails)
                        .Includes(m => m.tb_MaterialRequisitions)
                        .Includes(m => m.tb_StockInDetails)
                        .Includes(m => m.tb_MaterialRequisitionDetails)
                        .Includes(m => m.tb_PurGoodsRecommendDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Location entity = model as tb_Location;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Location>(m => m.Location_ID== entity.Location_ID)
                                .Include(m => m.tb_ProdSplitDetails)
                        .Include(m => m.tb_ProductionPlanDetails)
                        .Include(m => m.tb_StockOutDetails)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ProduceGoodsRecommendDetails)
                        .Include(m => m.tb_SaleOutReDetails)
                        .Include(m => m.tb_Stocktakes)
                        .Include(m => m.tb_MaterialReturnDetails)
                        .Include(m => m.tb_ProdMergeDetails)
                        .Include(m => m.tb_FinishedGoodsInvDetails)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_ProdReturningDetails)
                        .Include(m => m.tb_MaterialReturns)
                        .Include(m => m.tb_BOM_SDetailSecondaries)
                        .Include(m => m.tb_PurEntryDetails)
                        .Include(m => m.tb_StorageRacks)
                        .Include(m => m.tb_ProductionDemandDetails)
                        .Include(m => m.tb_SaleOutReRefurbishedMaterialsDetails)
                        .Include(m => m.tb_ManufacturingOrderDetails)
                        .Include(m => m.tb_SaleOrderDetails)
                        .Include(m => m.tb_ProdBorrowingDetails)
                        .Include(m => m.tb_ProdSplits)
                        .Include(m => m.tb_BuyingRequisitions)
                        .Include(m => m.tb_PurEntryReDetails)
                        .Include(m => m.tb_SaleOutDetails)
                        .Include(m => m.tb_PurOrderDetails)
                        .Include(m => m.tb_ProductionDemandTargetDetails)
                        .Include(m => m.tb_ProdMerges)
                        .Include(m => m.tb_Inventories)
                        .Include(m => m.tb_PurOrderReDetails)
                        .Include(m => m.tb_MaterialRequisitions)
                        .Include(m => m.tb_StockInDetails)
                        .Include(m => m.tb_MaterialRequisitionDetails)
                        .Include(m => m.tb_PurGoodsRecommendDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Location AddReEntity(tb_Location entity)
        {
            tb_Location AddEntity =  _tb_LocationServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Location> AddReEntityAsync(tb_Location entity)
        {
            tb_Location AddEntity = await _tb_LocationServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Location entity)
        {
            long id = await _tb_LocationServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Location> infos)
        {
            List<long> ids = await _tb_LocationServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Location>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Location entity)
        {
            bool rs = await _tb_LocationServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Location>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Location entity)
        {
            bool rs = await _tb_LocationServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_LocationServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Location>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_LocationServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Location>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Location>> QueryAsync()
        {
            List<tb_Location> list = await  _tb_LocationServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        
        public virtual List<tb_Location> Query()
        {
            List<tb_Location> list =  _tb_LocationServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        
        public virtual List<tb_Location> Query(string wheresql)
        {
            List<tb_Location> list =  _tb_LocationServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Location>> QueryAsync(string wheresql) 
        {
            List<tb_Location> list = await _tb_LocationServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Location>> QueryAsync(Expression<Func<tb_Location, bool>> exp)
        {
            List<tb_Location> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Location>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Location>> QueryByNavAsync()
        {
            List<tb_Location> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Location>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_locationtype )
                                            .Includes(t => t.tb_ProdSplitDetails )
                                .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_StockOutDetails )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                                .Includes(t => t.tb_Stocktakes )
                                .Includes(t => t.tb_MaterialReturnDetails )
                                .Includes(t => t.tb_ProdMergeDetails )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_ProdReturningDetails )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_BOM_SDetailSecondaries )
                                .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_StorageRacks )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                                .Includes(t => t.tb_ManufacturingOrderDetails )
                                .Includes(t => t.tb_SaleOrderDetails )
                                .Includes(t => t.tb_ProdBorrowingDetails )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_BuyingRequisitions )
                                .Includes(t => t.tb_PurEntryReDetails )
                                .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_PurOrderDetails )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_ProdMerges )
                                .Includes(t => t.tb_Inventories )
                                .Includes(t => t.tb_PurOrderReDetails )
                                .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_StockInDetails )
                                .Includes(t => t.tb_MaterialRequisitionDetails )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Location>> QueryByNavAsync(Expression<Func<tb_Location, bool>> exp)
        {
            List<tb_Location> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Location>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_locationtype )
                                            .Includes(t => t.tb_ProdSplitDetails )
                                .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_StockOutDetails )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_SaleOutReDetails )
                                .Includes(t => t.tb_Stocktakes )
                                .Includes(t => t.tb_MaterialReturnDetails )
                                .Includes(t => t.tb_ProdMergeDetails )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_ProdReturningDetails )
                                .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_BOM_SDetailSecondaries )
                                .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_StorageRacks )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                                .Includes(t => t.tb_ManufacturingOrderDetails )
                                .Includes(t => t.tb_SaleOrderDetails )
                                .Includes(t => t.tb_ProdBorrowingDetails )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_BuyingRequisitions )
                                .Includes(t => t.tb_PurEntryReDetails )
                                .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_PurOrderDetails )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_ProdMerges )
                                .Includes(t => t.tb_Inventories )
                                .Includes(t => t.tb_PurOrderReDetails )
                                .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_StockInDetails )
                                .Includes(t => t.tb_MaterialRequisitionDetails )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Location> QueryByNav(Expression<Func<tb_Location, bool>> exp)
        {
            List<tb_Location> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Location>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_locationtype )
                                        .Includes(t => t.tb_ProdSplitDetails )
                            .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_StockOutDetails )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                            .Includes(t => t.tb_Stocktakes )
                            .Includes(t => t.tb_MaterialReturnDetails )
                            .Includes(t => t.tb_ProdMergeDetails )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_ProdReturningDetails )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_BOM_SDetailSecondaries )
                            .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_StorageRacks )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                            .Includes(t => t.tb_ManufacturingOrderDetails )
                            .Includes(t => t.tb_SaleOrderDetails )
                            .Includes(t => t.tb_ProdBorrowingDetails )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_BuyingRequisitions )
                            .Includes(t => t.tb_PurEntryReDetails )
                            .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_PurOrderDetails )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_ProdMerges )
                            .Includes(t => t.tb_Inventories )
                            .Includes(t => t.tb_PurOrderReDetails )
                            .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_StockInDetails )
                            .Includes(t => t.tb_MaterialRequisitionDetails )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Location>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Location>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Location>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_LocationServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Location entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Location>().Where(w => w.Location_ID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_locationtype )
                                        .Includes(t => t.tb_ProdSplitDetails )
                            .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_StockOutDetails )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_SaleOutReDetails )
                            .Includes(t => t.tb_Stocktakes )
                            .Includes(t => t.tb_MaterialReturnDetails )
                            .Includes(t => t.tb_ProdMergeDetails )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_ProdReturningDetails )
                            .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_BOM_SDetailSecondaries )
                            .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_StorageRacks )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_SaleOutReRefurbishedMaterialsDetails )
                            .Includes(t => t.tb_ManufacturingOrderDetails )
                            .Includes(t => t.tb_SaleOrderDetails )
                            .Includes(t => t.tb_ProdBorrowingDetails )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_BuyingRequisitions )
                            .Includes(t => t.tb_PurEntryReDetails )
                            .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_PurOrderDetails )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_ProdMerges )
                            .Includes(t => t.tb_Inventories )
                            .Includes(t => t.tb_PurOrderReDetails )
                            .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_StockInDetails )
                            .Includes(t => t.tb_MaterialRequisitionDetails )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Location>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



