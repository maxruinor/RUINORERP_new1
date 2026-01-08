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
    /// 出库单
    /// </summary>
    public partial class tb_StockOutController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StockOutServices _tb_StockOutServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_StockOutController(ILogger<tb_StockOutController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StockOutServices tb_StockOutServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StockOutServices = tb_StockOutServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_StockOut info)
        {

           // tb_StockOutValidator validator = new tb_StockOutValidator();
           tb_StockOutValidator validator = _appContext.GetRequiredService<tb_StockOutValidator>();
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
        public async Task<ReturnResults<tb_StockOut>> SaveOrUpdate(tb_StockOut entity)
        {
            ReturnResults<tb_StockOut> rr = new ReturnResults<tb_StockOut>();
            tb_StockOut Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_StockOutServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StockOutServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(entity);
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
            tb_StockOut entity = model as tb_StockOut;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_StockOutServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StockOutServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(entity);
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
            List<T> list = await _tb_StockOutServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StockOut entity = item as tb_StockOut;
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
            List<T> list = await _tb_StockOutServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StockOut entity = item as tb_StockOut;
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
            tb_StockOut entity = model as tb_StockOut;
            bool rs = await _tb_StockOutServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_StockOut>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StockOut> entitys = models as List<tb_StockOut>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StockOut>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_StockOut>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StockOutValidator validator = new tb_StockOutValidator();
           tb_StockOutValidator validator = _appContext.GetRequiredService<tb_StockOutValidator>();
            ValidationResult results = validator.Validate(info as tb_StockOut);
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

                tb_StockOut entity = model as tb_StockOut;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MainID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StockOut>(entity as tb_StockOut)
                        .Include(m => m.tb_StockOutDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StockOut>(entity as tb_StockOut)
                .Include(m => m.tb_StockOutDetails)
         
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>()
                                .Includes(m => m.tb_StockOutDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StockOut entity = model as tb_StockOut;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StockOut>(m => m.MainID== entity.MainID)
                                .Include(m => m.tb_StockOutDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_StockOut AddReEntity(tb_StockOut entity)
        {
            tb_StockOut AddEntity =  _tb_StockOutServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StockOut> AddReEntityAsync(tb_StockOut entity)
        {
            tb_StockOut AddEntity = await _tb_StockOutServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StockOut entity)
        {
            long id = await _tb_StockOutServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StockOut> infos)
        {
            List<long> ids = await _tb_StockOutServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StockOut entity)
        {
            bool rs = await _tb_StockOutServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_StockOut>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StockOut entity)
        {
            bool rs = await _tb_StockOutServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_StockOut>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StockOutServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_StockOut>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StockOutServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_StockOut>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StockOut>> QueryAsync()
        {
            List<tb_StockOut> list = await  _tb_StockOutServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        
        public virtual List<tb_StockOut> Query()
        {
            List<tb_StockOut> list =  _tb_StockOutServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        
        public virtual List<tb_StockOut> Query(string wheresql)
        {
            List<tb_StockOut> list =  _tb_StockOutServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StockOut>> QueryAsync(string wheresql) 
        {
            List<tb_StockOut> list = await _tb_StockOutServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StockOut>> QueryAsync(Expression<Func<tb_StockOut, bool>> exp)
        {
            List<tb_StockOut> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockOut>> QueryByNavAsync()
        {
            List<tb_StockOut> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_outinstocktype )
                                            .Includes(t => t.tb_StockOutDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockOut>> QueryByNavAsync(Expression<Func<tb_StockOut, bool>> exp)
        {
            List<tb_StockOut> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_outinstocktype )
                                            .Includes(t => t.tb_StockOutDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StockOut> QueryByNav(Expression<Func<tb_StockOut, bool>> exp)
        {
            List<tb_StockOut> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_outinstocktype )
                                        .Includes(t => t.tb_StockOutDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_StockOut>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StockOut>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StockOutServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StockOut entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockOut>().Where(w => w.MainID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_outinstocktype )
                        

                                            .Includes(t => t.tb_StockOutDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_StockOut>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



