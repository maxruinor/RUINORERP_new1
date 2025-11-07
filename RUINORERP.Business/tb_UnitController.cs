// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 11:46:21
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
using RUINORERP.Business.Cache;

namespace RUINORERP.Business
{
    /// <summary>
    /// 基本单位
    /// </summary>
    public partial class tb_UnitController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UnitServices _tb_UnitServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_UnitController(ILogger<tb_UnitController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UnitServices tb_UnitServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UnitServices = tb_UnitServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_Unit info)
        {

           // tb_UnitValidator validator = new tb_UnitValidator();
           tb_UnitValidator validator = _appContext.GetRequiredService<tb_UnitValidator>();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        
        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public override async Task<bool> ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return await _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).AnyAsync();
        }
      
        
        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_Unit>> SaveOrUpdate(tb_Unit entity)
        {
            ReturnResults<tb_Unit> rr = new ReturnResults<tb_Unit>();
            tb_Unit Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Unit_ID > 0)
                {
                    bool rs = await _tb_UnitServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Unit>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UnitServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_Unit>(entity);
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
            tb_Unit entity = model as tb_Unit;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Unit_ID > 0)
                {
                    bool rs = await _tb_UnitServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Unit>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UnitServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_Unit>(entity);
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
            List<T> list = await _tb_UnitServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Unit entity = item as tb_Unit;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_UnitServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Unit entity = item as tb_Unit;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_Unit entity = model as tb_Unit;
            bool rs = await _tb_UnitServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_Unit>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Unit> entitys = models as List<tb_Unit>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Unit>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_Unit>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_UnitValidator validator = new tb_UnitValidator();
           tb_UnitValidator validator = _appContext.GetRequiredService<tb_UnitValidator>();
            ValidationResult results = validator.Validate(info as tb_Unit);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }
        
        public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model) where C : class
        {
            bool rs = false;
            RevertCommand command = new RevertCommand();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
                             //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
            try
            {

                tb_Unit entity = model as tb_Unit;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Unit_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Unit>(entity as tb_Unit)
                        .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_FinishedGoodsInvDetails)
                    .Include(m => m.tb_Prods)
                    .Include(m => m.tb_Unit_Conversions)
                    .Include(m => m.tb_Unit_Conversions)
                    .Include(m => m.tb_ProdBundles)
                    .Include(m => m.tb_Packings)
                    .Include(m => m.tb_FM_PriceAdjustmentDetails)
                    .Include(m => m.tb_BOM_SDetails)
                    .Include(m => m.tb_BOM_SDetailSubstituteMaterials)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Unit>(entity as tb_Unit)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_FinishedGoodsInvDetails)
                .Include(m => m.tb_Prods)
                .Include(m => m.tb_Unit_Conversions)
                .Include(m => m.tb_Unit_Conversions)
                .Include(m => m.tb_ProdBundles)
                .Include(m => m.tb_Packings)
                .Include(m => m.tb_FM_PriceAdjustmentDetails)
                .Include(m => m.tb_BOM_SDetails)
                .Include(m => m.tb_BOM_SDetailSubstituteMaterials)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Unit_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
                _logger.Error(ex);
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>()
                                                .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_FinishedGoodsInvDetails)
                        .Includes(m => m.tb_Prods)
                        .Includes(m => m.tb_Unit_Conversions)
                        .Includes(m => m.tb_Unit_ConversionsBySourceUnit)
                        .Includes(m => m.tb_ProdBundles)
                        .Includes(m => m.tb_Packings)
                        .Includes(m => m.tb_FM_PriceAdjustmentDetails)
                        .Includes(m => m.tb_BOM_SDetails)
                        .Includes(m => m.tb_BOM_SDetailSubstituteMaterials)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Unit entity = model as tb_Unit;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Unit>(m => m.Unit_ID== entity.Unit_ID)
                                .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_FinishedGoodsInvDetails)
                        .Include(m => m.tb_Prods)
                        .Include(m => m.tb_Unit_Conversions)
                        .Include(m => m.tb_Unit_ConversionsBySourceUnit)
                        .Include(m => m.tb_ProdBundles)
                        .Include(m => m.tb_Packings)
                        .Include(m => m.tb_FM_PriceAdjustmentDetails)
                        .Include(m => m.tb_BOM_SDetails)
                        .Include(m => m.tb_BOM_SDetailSubstituteMaterials)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Unit AddReEntity(tb_Unit entity)
        {
            tb_Unit AddEntity =  _tb_UnitServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_Unit>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Unit> AddReEntityAsync(tb_Unit entity)
        {
            tb_Unit AddEntity = await _tb_UnitServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_Unit>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Unit entity)
        {
            long id = await _tb_UnitServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_Unit>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Unit> infos)
        {
            List<long> ids = await _tb_UnitServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Unit entity)
        {
            bool rs = await _tb_UnitServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_Unit>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Unit entity)
        {
            bool rs = await _tb_UnitServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_Unit>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UnitServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_Unit>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UnitServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_Unit>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Unit>> QueryAsync()
        {
            List<tb_Unit> list = await  _tb_UnitServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        
        public virtual List<tb_Unit> Query()
        {
            List<tb_Unit> list =  _tb_UnitServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        
        public virtual List<tb_Unit> Query(string wheresql)
        {
            List<tb_Unit> list =  _tb_UnitServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Unit>> QueryAsync(string wheresql) 
        {
            List<tb_Unit> list = await _tb_UnitServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Unit>> QueryAsync(Expression<Func<tb_Unit, bool>> exp)
        {
            List<tb_Unit> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Unit>> QueryByNavAsync()
        {
            List<tb_Unit> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>()
                                            .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_Unit_Conversions )
                                .Includes(t => t.tb_Unit_Conversions )
                                .Includes(t => t.tb_ProdBundles )
                                .Includes(t => t.tb_Packings )
                                .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                                .Includes(t => t.tb_BOM_SDetails )
                                .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Unit>> QueryByNavAsync(Expression<Func<tb_Unit, bool>> exp)
        {
            List<tb_Unit> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>().Where(exp)
                                            .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_FinishedGoodsInvDetails )
                                .Includes(t => t.tb_Prods )
                                .Includes(t => t.tb_Unit_Conversions )
                                .Includes(t => t.tb_Unit_ConversionsBySourceUnit )
                                .Includes(t => t.tb_ProdBundles )
                                .Includes(t => t.tb_Packings )
                                .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                                .Includes(t => t.tb_BOM_SDetails )
                                .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Unit> QueryByNav(Expression<Func<tb_Unit, bool>> exp)
        {
            List<tb_Unit> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>().Where(exp)
                                        .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                            .Includes(t => t.tb_Prods )
                            .Includes(t => t.tb_Unit_Conversions )
                            .Includes(t => t.tb_Unit_ConversionsBySourceUnit )
                            .Includes(t => t.tb_ProdBundles )
                            .Includes(t => t.tb_Packings )
                            .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                            .Includes(t => t.tb_BOM_SDetails )
                            .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Unit>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Unit>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UnitServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Unit entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Unit>().Where(w => w.Unit_ID == (long)id)
                         

                                            .Includes(t => t.tb_ManufacturingOrders )
                                            .Includes(t => t.tb_FinishedGoodsInvDetails )
                                            .Includes(t => t.tb_Prods )
                                            .Includes(t => t.tb_Unit_Conversions )
                                            .Includes(t => t.tb_Unit_ConversionsBySourceUnit )
                                            .Includes(t => t.tb_ProdBundles )
                                            .Includes(t => t.tb_Packings )
                                            .Includes(t => t.tb_FM_PriceAdjustmentDetails )
                                            .Includes(t => t.tb_BOM_SDetails )
                                            .Includes(t => t.tb_BOM_SDetailSubstituteMaterials )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_Unit>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



