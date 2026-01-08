// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:12
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
    /// 库位类别
    /// </summary>
    public partial class tb_LocationTypeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_LocationTypeController(ILogger<tb_LocationTypeController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_LocationTypeServices tb_LocationTypeServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_LocationTypeServices = tb_LocationTypeServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_LocationType info)
        {

           // tb_LocationTypeValidator validator = new tb_LocationTypeValidator();
           tb_LocationTypeValidator validator = _appContext.GetRequiredService<tb_LocationTypeValidator>();
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
        public async Task<ReturnResults<tb_LocationType>> SaveOrUpdate(tb_LocationType entity)
        {
            ReturnResults<tb_LocationType> rr = new ReturnResults<tb_LocationType>();
            tb_LocationType Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LocationType_ID > 0)
                {
                    bool rs = await _tb_LocationTypeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_LocationTypeServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(entity);
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
            tb_LocationType entity = model as tb_LocationType;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LocationType_ID > 0)
                {
                    bool rs = await _tb_LocationTypeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_LocationTypeServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(entity);
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
            List<T> list = await _tb_LocationTypeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_LocationType entity = item as tb_LocationType;
                entity.AcceptChanges();
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_LocationTypeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_LocationType entity = item as tb_LocationType;
                entity.AcceptChanges();
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_LocationType entity = model as tb_LocationType;
            bool rs = await _tb_LocationTypeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_LocationType>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_LocationType> entitys = models as List<tb_LocationType>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_LocationType>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_LocationType>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_LocationTypeValidator validator = new tb_LocationTypeValidator();
           tb_LocationTypeValidator validator = _appContext.GetRequiredService<tb_LocationTypeValidator>();
            ValidationResult results = validator.Validate(info as tb_LocationType);
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

                tb_LocationType entity = model as tb_LocationType;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.LocationType_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_LocationType>(entity as tb_LocationType)
                        .Include(m => m.tb_Locations)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_LocationType>(entity as tb_LocationType)
                .Include(m => m.tb_Locations)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.LocationType_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>()
                                .Includes(m => m.tb_Locations)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_LocationType entity = model as tb_LocationType;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_LocationType>(m => m.LocationType_ID== entity.LocationType_ID)
                                .Include(m => m.tb_Locations)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_LocationType AddReEntity(tb_LocationType entity)
        {
            tb_LocationType AddEntity =  _tb_LocationTypeServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_LocationType> AddReEntityAsync(tb_LocationType entity)
        {
            tb_LocationType AddEntity = await _tb_LocationTypeServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_LocationType entity)
        {
            long id = await _tb_LocationTypeServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_LocationType> infos)
        {
            List<long> ids = await _tb_LocationTypeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_LocationType entity)
        {
            bool rs = await _tb_LocationTypeServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_LocationType>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_LocationType entity)
        {
            bool rs = await _tb_LocationTypeServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_LocationType>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_LocationTypeServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_LocationType>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_LocationTypeServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_LocationType>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_LocationType>> QueryAsync()
        {
            List<tb_LocationType> list = await  _tb_LocationTypeServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        
        public virtual List<tb_LocationType> Query()
        {
            List<tb_LocationType> list =  _tb_LocationTypeServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        
        public virtual List<tb_LocationType> Query(string wheresql)
        {
            List<tb_LocationType> list =  _tb_LocationTypeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        
        public virtual async Task<List<tb_LocationType>> QueryAsync(string wheresql) 
        {
            List<tb_LocationType> list = await _tb_LocationTypeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_LocationType>> QueryAsync(Expression<Func<tb_LocationType, bool>> exp)
        {
            List<tb_LocationType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_LocationType>> QueryByNavAsync()
        {
            List<tb_LocationType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>()
                                            .Includes(t => t.tb_Locations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_LocationType>> QueryByNavAsync(Expression<Func<tb_LocationType, bool>> exp)
        {
            List<tb_LocationType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>().Where(exp)
                                            .Includes(t => t.tb_Locations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_LocationType> QueryByNav(Expression<Func<tb_LocationType, bool>> exp)
        {
            List<tb_LocationType> list = _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>().Where(exp)
                                        .Includes(t => t.tb_Locations )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_LocationType>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_LocationType>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_LocationTypeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_LocationType entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_LocationType>().Where(w => w.LocationType_ID == (long)id)
                         

                                            .Includes(t => t.tb_Locations )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_LocationType>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



