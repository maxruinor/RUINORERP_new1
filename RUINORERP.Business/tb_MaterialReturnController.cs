// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:13
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
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本
    /// </summary>
    public partial class tb_MaterialReturnController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MaterialReturnServices _tb_MaterialReturnServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_MaterialReturnController(ILogger<tb_MaterialReturnController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MaterialReturnServices tb_MaterialReturnServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MaterialReturnServices = tb_MaterialReturnServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_MaterialReturn info)
        {

           // tb_MaterialReturnValidator validator = new tb_MaterialReturnValidator();
           tb_MaterialReturnValidator validator = _appContext.GetRequiredService<tb_MaterialReturnValidator>();
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
        public async Task<ReturnResults<tb_MaterialReturn>> SaveOrUpdate(tb_MaterialReturn entity)
        {
            ReturnResults<tb_MaterialReturn> rr = new ReturnResults<tb_MaterialReturn>();
            tb_MaterialReturn Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MRE_ID > 0)
                {
                    bool rs = await _tb_MaterialReturnServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MaterialReturnServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(entity);
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
            tb_MaterialReturn entity = model as tb_MaterialReturn;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MRE_ID > 0)
                {
                    bool rs = await _tb_MaterialReturnServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MaterialReturnServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(entity);
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
            List<T> list = await _tb_MaterialReturnServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MaterialReturn entity = item as tb_MaterialReturn;
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
            List<T> list = await _tb_MaterialReturnServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MaterialReturn entity = item as tb_MaterialReturn;
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
            tb_MaterialReturn entity = model as tb_MaterialReturn;
            bool rs = await _tb_MaterialReturnServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_MaterialReturn>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MaterialReturn> entitys = models as List<tb_MaterialReturn>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MaterialReturn>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_MaterialReturn>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MaterialReturnValidator validator = new tb_MaterialReturnValidator();
           tb_MaterialReturnValidator validator = _appContext.GetRequiredService<tb_MaterialReturnValidator>();
            ValidationResult results = validator.Validate(info as tb_MaterialReturn);
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

                tb_MaterialReturn entity = model as tb_MaterialReturn;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MRE_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_MaterialReturn>(entity as tb_MaterialReturn)
                        .Include(m => m.tb_MaterialReturnDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_MaterialReturn>(entity as tb_MaterialReturn)
                .Include(m => m.tb_MaterialReturnDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MRE_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>()
                                .Includes(m => m.tb_MaterialReturnDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MaterialReturn entity = model as tb_MaterialReturn;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MaterialReturn>(m => m.MRE_ID== entity.MRE_ID)
                                .Include(m => m.tb_MaterialReturnDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_MaterialReturn AddReEntity(tb_MaterialReturn entity)
        {
            tb_MaterialReturn AddEntity =  _tb_MaterialReturnServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MaterialReturn> AddReEntityAsync(tb_MaterialReturn entity)
        {
            tb_MaterialReturn AddEntity = await _tb_MaterialReturnServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MaterialReturn entity)
        {
            long id = await _tb_MaterialReturnServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MaterialReturn> infos)
        {
            List<long> ids = await _tb_MaterialReturnServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MaterialReturn entity)
        {
            bool rs = await _tb_MaterialReturnServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_MaterialReturn>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MaterialReturn entity)
        {
            bool rs = await _tb_MaterialReturnServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_MaterialReturn>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MaterialReturnServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_MaterialReturn>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MaterialReturnServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_MaterialReturn>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MaterialReturn>> QueryAsync()
        {
            List<tb_MaterialReturn> list = await  _tb_MaterialReturnServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        
        public virtual List<tb_MaterialReturn> Query()
        {
            List<tb_MaterialReturn> list =  _tb_MaterialReturnServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        
        public virtual List<tb_MaterialReturn> Query(string wheresql)
        {
            List<tb_MaterialReturn> list =  _tb_MaterialReturnServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MaterialReturn>> QueryAsync(string wheresql) 
        {
            List<tb_MaterialReturn> list = await _tb_MaterialReturnServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MaterialReturn>> QueryAsync(Expression<Func<tb_MaterialReturn, bool>> exp)
        {
            List<tb_MaterialReturn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MaterialReturn>> QueryByNavAsync()
        {
            List<tb_MaterialReturn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>()
                               .Includes(t => t.tb_materialrequisition )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_MaterialReturnDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MaterialReturn>> QueryByNavAsync(Expression<Func<tb_MaterialReturn, bool>> exp)
        {
            List<tb_MaterialReturn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>().Where(exp)
                               .Includes(t => t.tb_materialrequisition )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_MaterialReturnDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MaterialReturn> QueryByNav(Expression<Func<tb_MaterialReturn, bool>> exp)
        {
            List<tb_MaterialReturn> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>().Where(exp)
                            .Includes(t => t.tb_materialrequisition )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_MaterialReturnDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialReturn>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MaterialReturn>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MaterialReturnServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MaterialReturn entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialReturn>().Where(w => w.MRE_ID == (long)id)
                             .Includes(t => t.tb_materialrequisition )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                        

                                            .Includes(t => t.tb_MaterialReturnDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_MaterialReturn>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



