// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/07/2025 11:14:02
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
    /// 领料单(包括生产和托工)
    /// </summary>
    public partial class tb_MaterialRequisitionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MaterialRequisitionServices _tb_MaterialRequisitionServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_MaterialRequisitionController(ILogger<tb_MaterialRequisitionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MaterialRequisitionServices tb_MaterialRequisitionServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MaterialRequisitionServices = tb_MaterialRequisitionServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_MaterialRequisition info)
        {

           // tb_MaterialRequisitionValidator validator = new tb_MaterialRequisitionValidator();
           tb_MaterialRequisitionValidator validator = _appContext.GetRequiredService<tb_MaterialRequisitionValidator>();
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
        public async Task<ReturnResults<tb_MaterialRequisition>> SaveOrUpdate(tb_MaterialRequisition entity)
        {
            ReturnResults<tb_MaterialRequisition> rr = new ReturnResults<tb_MaterialRequisition>();
            tb_MaterialRequisition Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MR_ID > 0)
                {
                    bool rs = await _tb_MaterialRequisitionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MaterialRequisitionServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(entity);
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
            tb_MaterialRequisition entity = model as tb_MaterialRequisition;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MR_ID > 0)
                {
                    bool rs = await _tb_MaterialRequisitionServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MaterialRequisitionServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(entity);
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
            List<T> list = await _tb_MaterialRequisitionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MaterialRequisition entity = item as tb_MaterialRequisition;
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
            List<T> list = await _tb_MaterialRequisitionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MaterialRequisition entity = item as tb_MaterialRequisition;
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
            tb_MaterialRequisition entity = model as tb_MaterialRequisition;
            bool rs = await _tb_MaterialRequisitionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisition>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MaterialRequisition> entitys = models as List<tb_MaterialRequisition>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MaterialRequisition>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_MaterialRequisition>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MaterialRequisitionValidator validator = new tb_MaterialRequisitionValidator();
           tb_MaterialRequisitionValidator validator = _appContext.GetRequiredService<tb_MaterialRequisitionValidator>();
            ValidationResult results = validator.Validate(info as tb_MaterialRequisition);
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

                tb_MaterialRequisition entity = model as tb_MaterialRequisition;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MR_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_MaterialRequisition>(entity as tb_MaterialRequisition)
                        .Include(m => m.tb_MaterialReturns)
                    .Include(m => m.tb_MaterialRequisitionDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_MaterialRequisition>(entity as tb_MaterialRequisition)
                .Include(m => m.tb_MaterialReturns)
                .Include(m => m.tb_MaterialRequisitionDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MR_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>()
                                .Includes(m => m.tb_projectgroup)
                            .Includes(m => m.tb_customervendor)
                            .Includes(m => m.tb_manufacturingorder)
                            .Includes(m => m.tb_department)
                            .Includes(m => m.tb_employee)
                                            .Includes(m => m.tb_MaterialReturns)
                        .Includes(m => m.tb_MaterialRequisitionDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MaterialRequisition entity = model as tb_MaterialRequisition;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MaterialRequisition>(m => m.MR_ID== entity.MR_ID)
                                .Include(m => m.tb_MaterialReturns)
                        .Include(m => m.tb_MaterialRequisitionDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_MaterialRequisition AddReEntity(tb_MaterialRequisition entity)
        {
            tb_MaterialRequisition AddEntity =  _tb_MaterialRequisitionServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MaterialRequisition> AddReEntityAsync(tb_MaterialRequisition entity)
        {
            tb_MaterialRequisition AddEntity = await _tb_MaterialRequisitionServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MaterialRequisition entity)
        {
            long id = await _tb_MaterialRequisitionServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MaterialRequisition> infos)
        {
            List<long> ids = await _tb_MaterialRequisitionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MaterialRequisition entity)
        {
            bool rs = await _tb_MaterialRequisitionServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisition>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MaterialRequisition entity)
        {
            bool rs = await _tb_MaterialRequisitionServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisition>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MaterialRequisitionServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisition>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MaterialRequisitionServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_MaterialRequisition>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MaterialRequisition>> QueryAsync()
        {
            List<tb_MaterialRequisition> list = await  _tb_MaterialRequisitionServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        
        public virtual List<tb_MaterialRequisition> Query()
        {
            List<tb_MaterialRequisition> list =  _tb_MaterialRequisitionServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        
        public virtual List<tb_MaterialRequisition> Query(string wheresql)
        {
            List<tb_MaterialRequisition> list =  _tb_MaterialRequisitionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MaterialRequisition>> QueryAsync(string wheresql) 
        {
            List<tb_MaterialRequisition> list = await _tb_MaterialRequisitionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MaterialRequisition>> QueryAsync(Expression<Func<tb_MaterialRequisition, bool>> exp)
        {
            List<tb_MaterialRequisition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MaterialRequisition>> QueryByNavAsync()
        {
            List<tb_MaterialRequisition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>()
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_manufacturingorder )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_MaterialRequisitionDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MaterialRequisition>> QueryByNavAsync(Expression<Func<tb_MaterialRequisition, bool>> exp)
        {
            List<tb_MaterialRequisition> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().Where(exp)
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_manufacturingorder )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_MaterialReturns )
                                .Includes(t => t.tb_MaterialRequisitionDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MaterialRequisition> QueryByNav(Expression<Func<tb_MaterialRequisition, bool>> exp)
        {
            List<tb_MaterialRequisition> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().Where(exp)
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_manufacturingorder )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_MaterialReturns )
                            .Includes(t => t.tb_MaterialRequisitionDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisition>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MaterialRequisition>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MaterialRequisitionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MaterialRequisition entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisition>().Where(w => w.MR_ID == (long)id)
                             .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_manufacturingorder )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                        

                                            .Includes(t => t.tb_MaterialReturns )
                                            .Includes(t => t.tb_MaterialRequisitionDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisition>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



