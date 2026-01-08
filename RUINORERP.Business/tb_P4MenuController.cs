// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:15
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
    /// 菜单权限表
    /// </summary>
    public partial class tb_P4MenuController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_P4MenuServices _tb_P4MenuServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_P4MenuController(ILogger<tb_P4MenuController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_P4MenuServices tb_P4MenuServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_P4MenuServices = tb_P4MenuServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_P4Menu info)
        {

           // tb_P4MenuValidator validator = new tb_P4MenuValidator();
           tb_P4MenuValidator validator = _appContext.GetRequiredService<tb_P4MenuValidator>();
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
        public async Task<ReturnResults<tb_P4Menu>> SaveOrUpdate(tb_P4Menu entity)
        {
            ReturnResults<tb_P4Menu> rr = new ReturnResults<tb_P4Menu>();
            tb_P4Menu Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.P4Menu_ID > 0)
                {
                    bool rs = await _tb_P4MenuServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_P4MenuServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(entity);
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
            tb_P4Menu entity = model as tb_P4Menu;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.P4Menu_ID > 0)
                {
                    bool rs = await _tb_P4MenuServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_P4MenuServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(entity);
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
            List<T> list = await _tb_P4MenuServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_P4Menu entity = item as tb_P4Menu;
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
            List<T> list = await _tb_P4MenuServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_P4Menu entity = item as tb_P4Menu;
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
            tb_P4Menu entity = model as tb_P4Menu;
            bool rs = await _tb_P4MenuServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_P4Menu>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_P4Menu> entitys = models as List<tb_P4Menu>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_P4Menu>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_P4Menu>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_P4MenuValidator validator = new tb_P4MenuValidator();
           tb_P4MenuValidator validator = _appContext.GetRequiredService<tb_P4MenuValidator>();
            ValidationResult results = validator.Validate(info as tb_P4Menu);
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

                tb_P4Menu entity = model as tb_P4Menu;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.P4Menu_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_P4Menu>(entity as tb_P4Menu)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_P4Menu>(entity as tb_P4Menu)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.P4Menu_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_P4Menu entity = model as tb_P4Menu;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_P4Menu>(m => m.P4Menu_ID== entity.P4Menu_ID)
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
        
        
        
        public tb_P4Menu AddReEntity(tb_P4Menu entity)
        {
            tb_P4Menu AddEntity =  _tb_P4MenuServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_P4Menu> AddReEntityAsync(tb_P4Menu entity)
        {
            tb_P4Menu AddEntity = await _tb_P4MenuServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_P4Menu entity)
        {
            long id = await _tb_P4MenuServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_P4Menu> infos)
        {
            List<long> ids = await _tb_P4MenuServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_P4Menu entity)
        {
            bool rs = await _tb_P4MenuServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_P4Menu>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_P4Menu entity)
        {
            bool rs = await _tb_P4MenuServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_P4Menu>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_P4MenuServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_P4Menu>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_P4MenuServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_P4Menu>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_P4Menu>> QueryAsync()
        {
            List<tb_P4Menu> list = await  _tb_P4MenuServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        
        public virtual List<tb_P4Menu> Query()
        {
            List<tb_P4Menu> list =  _tb_P4MenuServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        
        public virtual List<tb_P4Menu> Query(string wheresql)
        {
            List<tb_P4Menu> list =  _tb_P4MenuServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        
        public virtual async Task<List<tb_P4Menu>> QueryAsync(string wheresql) 
        {
            List<tb_P4Menu> list = await _tb_P4MenuServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_P4Menu>> QueryAsync(Expression<Func<tb_P4Menu, bool>> exp)
        {
            List<tb_P4Menu> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4Menu>> QueryByNavAsync()
        {
            List<tb_P4Menu> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>()
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_moduledefinition )
                               .Includes(t => t.tb_roleinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4Menu>> QueryByNavAsync(Expression<Func<tb_P4Menu, bool>> exp)
        {
            List<tb_P4Menu> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>().Where(exp)
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_moduledefinition )
                               .Includes(t => t.tb_roleinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_P4Menu> QueryByNav(Expression<Func<tb_P4Menu, bool>> exp)
        {
            List<tb_P4Menu> list = _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>().Where(exp)
                            .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_moduledefinition )
                            .Includes(t => t.tb_roleinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_P4Menu>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_P4Menu>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_P4MenuServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_P4Menu entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4Menu>().Where(w => w.P4Menu_ID == (long)id)
                             .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_moduledefinition )
                            .Includes(t => t.tb_roleinfo )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_P4Menu>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



