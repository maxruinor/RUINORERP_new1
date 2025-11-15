// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:01
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等
    /// </summary>
    public partial class tb_CustomerVendorTypeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CustomerVendorTypeServices _tb_CustomerVendorTypeServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_CustomerVendorTypeController(ILogger<tb_CustomerVendorTypeController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CustomerVendorTypeServices tb_CustomerVendorTypeServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CustomerVendorTypeServices = tb_CustomerVendorTypeServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_CustomerVendorType info)
        {

           // tb_CustomerVendorTypeValidator validator = new tb_CustomerVendorTypeValidator();
           tb_CustomerVendorTypeValidator validator = _appContext.GetRequiredService<tb_CustomerVendorTypeValidator>();
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
        public async Task<ReturnResults<tb_CustomerVendorType>> SaveOrUpdate(tb_CustomerVendorType entity)
        {
            ReturnResults<tb_CustomerVendorType> rr = new ReturnResults<tb_CustomerVendorType>();
            tb_CustomerVendorType Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Type_ID > 0)
                {
                    bool rs = await _tb_CustomerVendorTypeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorTypeServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(entity);
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
            tb_CustomerVendorType entity = model as tb_CustomerVendorType;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Type_ID > 0)
                {
                    bool rs = await _tb_CustomerVendorTypeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorTypeServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(entity);
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
            List<T> list = await _tb_CustomerVendorTypeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CustomerVendorType entity = item as tb_CustomerVendorType;
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
            List<T> list = await _tb_CustomerVendorTypeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CustomerVendorType entity = item as tb_CustomerVendorType;
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
            tb_CustomerVendorType entity = model as tb_CustomerVendorType;
            bool rs = await _tb_CustomerVendorTypeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorType>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CustomerVendorType> entitys = models as List<tb_CustomerVendorType>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CustomerVendorType>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_CustomerVendorType>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CustomerVendorTypeValidator validator = new tb_CustomerVendorTypeValidator();
           tb_CustomerVendorTypeValidator validator = _appContext.GetRequiredService<tb_CustomerVendorTypeValidator>();
            ValidationResult results = validator.Validate(info as tb_CustomerVendorType);
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

                tb_CustomerVendorType entity = model as tb_CustomerVendorType;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Type_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_CustomerVendorType>(entity as tb_CustomerVendorType)
                        .Include(m => m.tb_CustomerVendors)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_CustomerVendorType>(entity as tb_CustomerVendorType)
                .Include(m => m.tb_CustomerVendors)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Type_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>()
                                .Includes(m => m.tb_CustomerVendors)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CustomerVendorType entity = model as tb_CustomerVendorType;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CustomerVendorType>(m => m.Type_ID== entity.Type_ID)
                                .Include(m => m.tb_CustomerVendors)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_CustomerVendorType AddReEntity(tb_CustomerVendorType entity)
        {
            tb_CustomerVendorType AddEntity =  _tb_CustomerVendorTypeServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CustomerVendorType> AddReEntityAsync(tb_CustomerVendorType entity)
        {
            tb_CustomerVendorType AddEntity = await _tb_CustomerVendorTypeServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CustomerVendorType entity)
        {
            long id = await _tb_CustomerVendorTypeServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CustomerVendorType> infos)
        {
            List<long> ids = await _tb_CustomerVendorTypeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CustomerVendorType entity)
        {
            bool rs = await _tb_CustomerVendorTypeServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorType>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CustomerVendorType entity)
        {
            bool rs = await _tb_CustomerVendorTypeServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorType>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CustomerVendorTypeServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorType>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CustomerVendorTypeServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_CustomerVendorType>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CustomerVendorType>> QueryAsync()
        {
            List<tb_CustomerVendorType> list = await  _tb_CustomerVendorTypeServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendorType> Query()
        {
            List<tb_CustomerVendorType> list =  _tb_CustomerVendorTypeServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendorType> Query(string wheresql)
        {
            List<tb_CustomerVendorType> list =  _tb_CustomerVendorTypeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CustomerVendorType>> QueryAsync(string wheresql) 
        {
            List<tb_CustomerVendorType> list = await _tb_CustomerVendorTypeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendorType>> QueryAsync(Expression<Func<tb_CustomerVendorType, bool>> exp)
        {
            List<tb_CustomerVendorType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendorType>> QueryByNavAsync()
        {
            List<tb_CustomerVendorType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>()
                                            .Includes(t => t.tb_CustomerVendors )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendorType>> QueryByNavAsync(Expression<Func<tb_CustomerVendorType, bool>> exp)
        {
            List<tb_CustomerVendorType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>().Where(exp)
                                            .Includes(t => t.tb_CustomerVendors )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CustomerVendorType> QueryByNav(Expression<Func<tb_CustomerVendorType, bool>> exp)
        {
            List<tb_CustomerVendorType> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>().Where(exp)
                                        .Includes(t => t.tb_CustomerVendors )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorType>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendorType>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CustomerVendorTypeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CustomerVendorType entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorType>().Where(w => w.Type_ID == (long)id)
                         

                                            .Includes(t => t.tb_CustomerVendors )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorType>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



