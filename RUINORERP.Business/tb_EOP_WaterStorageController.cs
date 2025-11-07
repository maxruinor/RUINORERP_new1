// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:02
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
    /// 蓄水登记表
    /// </summary>
    public partial class tb_EOP_WaterStorageController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_EOP_WaterStorageServices _tb_EOP_WaterStorageServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_EOP_WaterStorageController(ILogger<tb_EOP_WaterStorageController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_EOP_WaterStorageServices tb_EOP_WaterStorageServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_EOP_WaterStorageServices = tb_EOP_WaterStorageServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_EOP_WaterStorage info)
        {

           // tb_EOP_WaterStorageValidator validator = new tb_EOP_WaterStorageValidator();
           tb_EOP_WaterStorageValidator validator = _appContext.GetRequiredService<tb_EOP_WaterStorageValidator>();
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
        public async Task<ReturnResults<tb_EOP_WaterStorage>> SaveOrUpdate(tb_EOP_WaterStorage entity)
        {
            ReturnResults<tb_EOP_WaterStorage> rr = new ReturnResults<tb_EOP_WaterStorage>();
            tb_EOP_WaterStorage Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.WSR_ID > 0)
                {
                    bool rs = await _tb_EOP_WaterStorageServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_EOP_WaterStorageServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(entity);
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
            tb_EOP_WaterStorage entity = model as tb_EOP_WaterStorage;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.WSR_ID > 0)
                {
                    bool rs = await _tb_EOP_WaterStorageServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_EOP_WaterStorageServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(entity);
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
            List<T> list = await _tb_EOP_WaterStorageServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_EOP_WaterStorage entity = item as tb_EOP_WaterStorage;
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
            List<T> list = await _tb_EOP_WaterStorageServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_EOP_WaterStorage entity = item as tb_EOP_WaterStorage;
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
            tb_EOP_WaterStorage entity = model as tb_EOP_WaterStorage;
            bool rs = await _tb_EOP_WaterStorageServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_EOP_WaterStorage>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_EOP_WaterStorage> entitys = models as List<tb_EOP_WaterStorage>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_EOP_WaterStorage>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_EOP_WaterStorage>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_EOP_WaterStorageValidator validator = new tb_EOP_WaterStorageValidator();
           tb_EOP_WaterStorageValidator validator = _appContext.GetRequiredService<tb_EOP_WaterStorageValidator>();
            ValidationResult results = validator.Validate(info as tb_EOP_WaterStorage);
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

                tb_EOP_WaterStorage entity = model as tb_EOP_WaterStorage;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.WSR_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_EOP_WaterStorage>(entity as tb_EOP_WaterStorage)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_EOP_WaterStorage>(entity as tb_EOP_WaterStorage)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.WSR_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_EOP_WaterStorage entity = model as tb_EOP_WaterStorage;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_EOP_WaterStorage>(m => m.WSR_ID== entity.WSR_ID)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_EOP_WaterStorage AddReEntity(tb_EOP_WaterStorage entity)
        {
            tb_EOP_WaterStorage AddEntity =  _tb_EOP_WaterStorageServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_EOP_WaterStorage> AddReEntityAsync(tb_EOP_WaterStorage entity)
        {
            tb_EOP_WaterStorage AddEntity = await _tb_EOP_WaterStorageServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_EOP_WaterStorage entity)
        {
            long id = await _tb_EOP_WaterStorageServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_EOP_WaterStorage> infos)
        {
            List<long> ids = await _tb_EOP_WaterStorageServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_EOP_WaterStorage entity)
        {
            bool rs = await _tb_EOP_WaterStorageServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_EOP_WaterStorage>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_EOP_WaterStorage entity)
        {
            bool rs = await _tb_EOP_WaterStorageServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_EOP_WaterStorage>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_EOP_WaterStorageServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_EOP_WaterStorage>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_EOP_WaterStorageServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_EOP_WaterStorage>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_EOP_WaterStorage>> QueryAsync()
        {
            List<tb_EOP_WaterStorage> list = await  _tb_EOP_WaterStorageServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        
        public virtual List<tb_EOP_WaterStorage> Query()
        {
            List<tb_EOP_WaterStorage> list =  _tb_EOP_WaterStorageServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        
        public virtual List<tb_EOP_WaterStorage> Query(string wheresql)
        {
            List<tb_EOP_WaterStorage> list =  _tb_EOP_WaterStorageServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        
        public virtual async Task<List<tb_EOP_WaterStorage>> QueryAsync(string wheresql) 
        {
            List<tb_EOP_WaterStorage> list = await _tb_EOP_WaterStorageServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_EOP_WaterStorage>> QueryAsync(Expression<Func<tb_EOP_WaterStorage, bool>> exp)
        {
            List<tb_EOP_WaterStorage> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_EOP_WaterStorage>> QueryByNavAsync()
        {
            List<tb_EOP_WaterStorage> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_EOP_WaterStorage>> QueryByNavAsync(Expression<Func<tb_EOP_WaterStorage, bool>> exp)
        {
            List<tb_EOP_WaterStorage> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_EOP_WaterStorage> QueryByNav(Expression<Func<tb_EOP_WaterStorage, bool>> exp)
        {
            List<tb_EOP_WaterStorage> list = _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_EOP_WaterStorage>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_EOP_WaterStorage>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_EOP_WaterStorageServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_EOP_WaterStorage entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorage>().Where(w => w.WSR_ID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_EOP_WaterStorage>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



