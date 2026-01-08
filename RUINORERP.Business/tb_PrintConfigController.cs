// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:16
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
    /// 报表打印配置表
    /// </summary>
    public partial class tb_PrintConfigController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PrintConfigServices _tb_PrintConfigServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_PrintConfigController(ILogger<tb_PrintConfigController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PrintConfigServices tb_PrintConfigServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PrintConfigServices = tb_PrintConfigServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_PrintConfig info)
        {

           // tb_PrintConfigValidator validator = new tb_PrintConfigValidator();
           tb_PrintConfigValidator validator = _appContext.GetRequiredService<tb_PrintConfigValidator>();
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
        public async Task<ReturnResults<tb_PrintConfig>> SaveOrUpdate(tb_PrintConfig entity)
        {
            ReturnResults<tb_PrintConfig> rr = new ReturnResults<tb_PrintConfig>();
            tb_PrintConfig Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PrintConfigID > 0)
                {
                    bool rs = await _tb_PrintConfigServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PrintConfigServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(entity);
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
            tb_PrintConfig entity = model as tb_PrintConfig;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PrintConfigID > 0)
                {
                    bool rs = await _tb_PrintConfigServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PrintConfigServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(entity);
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
            List<T> list = await _tb_PrintConfigServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PrintConfig entity = item as tb_PrintConfig;
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
            List<T> list = await _tb_PrintConfigServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PrintConfig entity = item as tb_PrintConfig;
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
            tb_PrintConfig entity = model as tb_PrintConfig;
            bool rs = await _tb_PrintConfigServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_PrintConfig>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PrintConfig> entitys = models as List<tb_PrintConfig>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PrintConfig>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_PrintConfig>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PrintConfigValidator validator = new tb_PrintConfigValidator();
           tb_PrintConfigValidator validator = _appContext.GetRequiredService<tb_PrintConfigValidator>();
            ValidationResult results = validator.Validate(info as tb_PrintConfig);
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

                tb_PrintConfig entity = model as tb_PrintConfig;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PrintConfigID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PrintConfig>(entity as tb_PrintConfig)
                        .Include(m => m.tb_PrintTemplates)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PrintConfig>(entity as tb_PrintConfig)
                .Include(m => m.tb_PrintTemplates)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PrintConfigID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>()
                                .Includes(m => m.tb_PrintTemplates)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PrintConfig entity = model as tb_PrintConfig;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PrintConfig>(m => m.PrintConfigID== entity.PrintConfigID)
                                .Include(m => m.tb_PrintTemplates)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_PrintConfig AddReEntity(tb_PrintConfig entity)
        {
            tb_PrintConfig AddEntity =  _tb_PrintConfigServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PrintConfig> AddReEntityAsync(tb_PrintConfig entity)
        {
            tb_PrintConfig AddEntity = await _tb_PrintConfigServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PrintConfig entity)
        {
            long id = await _tb_PrintConfigServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PrintConfig> infos)
        {
            List<long> ids = await _tb_PrintConfigServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PrintConfig entity)
        {
            bool rs = await _tb_PrintConfigServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_PrintConfig>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PrintConfig entity)
        {
            bool rs = await _tb_PrintConfigServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_PrintConfig>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PrintConfigServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_PrintConfig>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PrintConfigServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_PrintConfig>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PrintConfig>> QueryAsync()
        {
            List<tb_PrintConfig> list = await  _tb_PrintConfigServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        
        public virtual List<tb_PrintConfig> Query()
        {
            List<tb_PrintConfig> list =  _tb_PrintConfigServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        
        public virtual List<tb_PrintConfig> Query(string wheresql)
        {
            List<tb_PrintConfig> list =  _tb_PrintConfigServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PrintConfig>> QueryAsync(string wheresql) 
        {
            List<tb_PrintConfig> list = await _tb_PrintConfigServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PrintConfig>> QueryAsync(Expression<Func<tb_PrintConfig, bool>> exp)
        {
            List<tb_PrintConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PrintConfig>> QueryByNavAsync()
        {
            List<tb_PrintConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>()
                                            .Includes(t => t.tb_PrintTemplates )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PrintConfig>> QueryByNavAsync(Expression<Func<tb_PrintConfig, bool>> exp)
        {
            List<tb_PrintConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>().Where(exp)
                                            .Includes(t => t.tb_PrintTemplates )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PrintConfig> QueryByNav(Expression<Func<tb_PrintConfig, bool>> exp)
        {
            List<tb_PrintConfig> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>().Where(exp)
                                        .Includes(t => t.tb_PrintTemplates )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintConfig>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PrintConfig>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PrintConfigServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PrintConfig entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintConfig>().Where(w => w.PrintConfigID == (long)id)
                         

                                            .Includes(t => t.tb_PrintTemplates )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_PrintConfig>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



