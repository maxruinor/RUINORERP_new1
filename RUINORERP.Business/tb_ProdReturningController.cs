// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:18
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
    /// 归还单，如果部分无法归还，则强制结案借出单。生成一个财务数据做记录。
    /// </summary>
    public partial class tb_ProdReturningController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdReturningServices _tb_ProdReturningServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdReturningController(ILogger<tb_ProdReturningController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdReturningServices tb_ProdReturningServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdReturningServices = tb_ProdReturningServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ProdReturning info)
        {

           // tb_ProdReturningValidator validator = new tb_ProdReturningValidator();
           tb_ProdReturningValidator validator = _appContext.GetRequiredService<tb_ProdReturningValidator>();
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
        public async Task<ReturnResults<tb_ProdReturning>> SaveOrUpdate(tb_ProdReturning entity)
        {
            ReturnResults<tb_ProdReturning> rr = new ReturnResults<tb_ProdReturning>();
            tb_ProdReturning Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReturnID > 0)
                {
                    bool rs = await _tb_ProdReturningServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdReturningServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(entity);
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
            tb_ProdReturning entity = model as tb_ProdReturning;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReturnID > 0)
                {
                    bool rs = await _tb_ProdReturningServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdReturningServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(entity);
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
            List<T> list = await _tb_ProdReturningServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdReturning entity = item as tb_ProdReturning;
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
            List<T> list = await _tb_ProdReturningServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdReturning entity = item as tb_ProdReturning;
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
            tb_ProdReturning entity = model as tb_ProdReturning;
            bool rs = await _tb_ProdReturningServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ProdReturning>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdReturning> entitys = models as List<tb_ProdReturning>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdReturning>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ProdReturning>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdReturningValidator validator = new tb_ProdReturningValidator();
           tb_ProdReturningValidator validator = _appContext.GetRequiredService<tb_ProdReturningValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdReturning);
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

                tb_ProdReturning entity = model as tb_ProdReturning;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ReturnID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProdReturning>(entity as tb_ProdReturning)
                        .Include(m => m.tb_ProdReturningDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProdReturning>(entity as tb_ProdReturning)
                .Include(m => m.tb_ProdReturningDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ReturnID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>()
                                .Includes(m => m.tb_ProdReturningDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdReturning entity = model as tb_ProdReturning;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdReturning>(m => m.ReturnID== entity.ReturnID)
                                .Include(m => m.tb_ProdReturningDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProdReturning AddReEntity(tb_ProdReturning entity)
        {
            tb_ProdReturning AddEntity =  _tb_ProdReturningServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdReturning> AddReEntityAsync(tb_ProdReturning entity)
        {
            tb_ProdReturning AddEntity = await _tb_ProdReturningServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdReturning entity)
        {
            long id = await _tb_ProdReturningServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdReturning> infos)
        {
            List<long> ids = await _tb_ProdReturningServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdReturning entity)
        {
            bool rs = await _tb_ProdReturningServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ProdReturning>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdReturning entity)
        {
            bool rs = await _tb_ProdReturningServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ProdReturning>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdReturningServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ProdReturning>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdReturningServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ProdReturning>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdReturning>> QueryAsync()
        {
            List<tb_ProdReturning> list = await  _tb_ProdReturningServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        
        public virtual List<tb_ProdReturning> Query()
        {
            List<tb_ProdReturning> list =  _tb_ProdReturningServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        
        public virtual List<tb_ProdReturning> Query(string wheresql)
        {
            List<tb_ProdReturning> list =  _tb_ProdReturningServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdReturning>> QueryAsync(string wheresql) 
        {
            List<tb_ProdReturning> list = await _tb_ProdReturningServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdReturning>> QueryAsync(Expression<Func<tb_ProdReturning, bool>> exp)
        {
            List<tb_ProdReturning> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdReturning>> QueryByNavAsync()
        {
            List<tb_ProdReturning> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_prodborrowing )
                                            .Includes(t => t.tb_ProdReturningDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdReturning>> QueryByNavAsync(Expression<Func<tb_ProdReturning, bool>> exp)
        {
            List<tb_ProdReturning> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_prodborrowing )
                                            .Includes(t => t.tb_ProdReturningDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdReturning> QueryByNav(Expression<Func<tb_ProdReturning, bool>> exp)
        {
            List<tb_ProdReturning> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_prodborrowing )
                                        .Includes(t => t.tb_ProdReturningDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProdReturning>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdReturning>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdReturningServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdReturning entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdReturning>().Where(w => w.ReturnID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_prodborrowing )
                        

                                            .Includes(t => t.tb_ProdReturningDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ProdReturning>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



