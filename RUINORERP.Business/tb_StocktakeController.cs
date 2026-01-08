// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:24
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
    /// 盘点表
    /// </summary>
    public partial class tb_StocktakeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StocktakeServices _tb_StocktakeServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_StocktakeController(ILogger<tb_StocktakeController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StocktakeServices tb_StocktakeServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StocktakeServices = tb_StocktakeServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_Stocktake info)
        {

           // tb_StocktakeValidator validator = new tb_StocktakeValidator();
           tb_StocktakeValidator validator = _appContext.GetRequiredService<tb_StocktakeValidator>();
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
        public async Task<ReturnResults<tb_Stocktake>> SaveOrUpdate(tb_Stocktake entity)
        {
            ReturnResults<tb_Stocktake> rr = new ReturnResults<tb_Stocktake>();
            tb_Stocktake Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_StocktakeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StocktakeServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(entity);
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
            tb_Stocktake entity = model as tb_Stocktake;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_StocktakeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StocktakeServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(entity);
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
            List<T> list = await _tb_StocktakeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Stocktake entity = item as tb_Stocktake;
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
            List<T> list = await _tb_StocktakeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Stocktake entity = item as tb_Stocktake;
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
            tb_Stocktake entity = model as tb_Stocktake;
            bool rs = await _tb_StocktakeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_Stocktake>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Stocktake> entitys = models as List<tb_Stocktake>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Stocktake>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_Stocktake>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StocktakeValidator validator = new tb_StocktakeValidator();
           tb_StocktakeValidator validator = _appContext.GetRequiredService<tb_StocktakeValidator>();
            ValidationResult results = validator.Validate(info as tb_Stocktake);
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

                tb_Stocktake entity = model as tb_Stocktake;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MainID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Stocktake>(entity as tb_Stocktake)
                        .Include(m => m.tb_StocktakeDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Stocktake>(entity as tb_Stocktake)
                .Include(m => m.tb_StocktakeDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MainID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>()
                                .Includes(m => m.tb_StocktakeDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Stocktake entity = model as tb_Stocktake;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Stocktake>(m => m.MainID== entity.MainID)
                                .Include(m => m.tb_StocktakeDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Stocktake AddReEntity(tb_Stocktake entity)
        {
            tb_Stocktake AddEntity =  _tb_StocktakeServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Stocktake> AddReEntityAsync(tb_Stocktake entity)
        {
            tb_Stocktake AddEntity = await _tb_StocktakeServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Stocktake entity)
        {
            long id = await _tb_StocktakeServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Stocktake> infos)
        {
            List<long> ids = await _tb_StocktakeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Stocktake entity)
        {
            bool rs = await _tb_StocktakeServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_Stocktake>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Stocktake entity)
        {
            bool rs = await _tb_StocktakeServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_Stocktake>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StocktakeServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_Stocktake>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StocktakeServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_Stocktake>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Stocktake>> QueryAsync()
        {
            List<tb_Stocktake> list = await  _tb_StocktakeServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        
        public virtual List<tb_Stocktake> Query()
        {
            List<tb_Stocktake> list =  _tb_StocktakeServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        
        public virtual List<tb_Stocktake> Query(string wheresql)
        {
            List<tb_Stocktake> list =  _tb_StocktakeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Stocktake>> QueryAsync(string wheresql) 
        {
            List<tb_Stocktake> list = await _tb_StocktakeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Stocktake>> QueryAsync(Expression<Func<tb_Stocktake, bool>> exp)
        {
            List<tb_Stocktake> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Stocktake>> QueryByNavAsync()
        {
            List<tb_Stocktake> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_StocktakeDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Stocktake>> QueryByNavAsync(Expression<Func<tb_Stocktake, bool>> exp)
        {
            List<tb_Stocktake> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_StocktakeDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Stocktake> QueryByNav(Expression<Func<tb_Stocktake, bool>> exp)
        {
            List<tb_Stocktake> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_StocktakeDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Stocktake>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Stocktake>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StocktakeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Stocktake entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Stocktake>().Where(w => w.MainID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_employee )
                        

                                            .Includes(t => t.tb_StocktakeDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_Stocktake>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



