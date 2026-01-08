// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:42:58
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
    /// 维修入库单
    /// </summary>
    public partial class tb_AS_RepairInStockController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_AS_RepairInStockServices _tb_AS_RepairInStockServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_AS_RepairInStockController(ILogger<tb_AS_RepairInStockController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_AS_RepairInStockServices tb_AS_RepairInStockServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_AS_RepairInStockServices = tb_AS_RepairInStockServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_AS_RepairInStock info)
        {

           // tb_AS_RepairInStockValidator validator = new tb_AS_RepairInStockValidator();
           tb_AS_RepairInStockValidator validator = _appContext.GetRequiredService<tb_AS_RepairInStockValidator>();
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
        public async Task<ReturnResults<tb_AS_RepairInStock>> SaveOrUpdate(tb_AS_RepairInStock entity)
        {
            ReturnResults<tb_AS_RepairInStock> rr = new ReturnResults<tb_AS_RepairInStock>();
            tb_AS_RepairInStock Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairInStockID > 0)
                {
                    bool rs = await _tb_AS_RepairInStockServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairInStockServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(entity);
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
            tb_AS_RepairInStock entity = model as tb_AS_RepairInStock;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairInStockID > 0)
                {
                    bool rs = await _tb_AS_RepairInStockServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairInStockServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(entity);
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
            List<T> list = await _tb_AS_RepairInStockServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairInStock entity = item as tb_AS_RepairInStock;
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
            List<T> list = await _tb_AS_RepairInStockServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairInStock entity = item as tb_AS_RepairInStock;
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
            tb_AS_RepairInStock entity = model as tb_AS_RepairInStock;
            bool rs = await _tb_AS_RepairInStockServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairInStock>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_AS_RepairInStock> entitys = models as List<tb_AS_RepairInStock>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_AS_RepairInStock>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_AS_RepairInStock>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_AS_RepairInStockValidator validator = new tb_AS_RepairInStockValidator();
           tb_AS_RepairInStockValidator validator = _appContext.GetRequiredService<tb_AS_RepairInStockValidator>();
            ValidationResult results = validator.Validate(info as tb_AS_RepairInStock);
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

                tb_AS_RepairInStock entity = model as tb_AS_RepairInStock;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RepairInStockID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_AS_RepairInStock>(entity as tb_AS_RepairInStock)
                        .Include(m => m.tb_AS_RepairInStockDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_AS_RepairInStock>(entity as tb_AS_RepairInStock)
                .Include(m => m.tb_AS_RepairInStockDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RepairInStockID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>()
                                .Includes(m => m.tb_AS_RepairInStockDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_AS_RepairInStock entity = model as tb_AS_RepairInStock;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_AS_RepairInStock>(m => m.RepairInStockID== entity.RepairInStockID)
                                .Include(m => m.tb_AS_RepairInStockDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_AS_RepairInStock AddReEntity(tb_AS_RepairInStock entity)
        {
            tb_AS_RepairInStock AddEntity =  _tb_AS_RepairInStockServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_AS_RepairInStock> AddReEntityAsync(tb_AS_RepairInStock entity)
        {
            tb_AS_RepairInStock AddEntity = await _tb_AS_RepairInStockServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_AS_RepairInStock entity)
        {
            long id = await _tb_AS_RepairInStockServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_AS_RepairInStock> infos)
        {
            List<long> ids = await _tb_AS_RepairInStockServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_AS_RepairInStock entity)
        {
            bool rs = await _tb_AS_RepairInStockServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairInStock>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_AS_RepairInStock entity)
        {
            bool rs = await _tb_AS_RepairInStockServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairInStock>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_AS_RepairInStockServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairInStock>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_AS_RepairInStockServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_AS_RepairInStock>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_AS_RepairInStock>> QueryAsync()
        {
            List<tb_AS_RepairInStock> list = await  _tb_AS_RepairInStockServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairInStock> Query()
        {
            List<tb_AS_RepairInStock> list =  _tb_AS_RepairInStockServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairInStock> Query(string wheresql)
        {
            List<tb_AS_RepairInStock> list =  _tb_AS_RepairInStockServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        
        public virtual async Task<List<tb_AS_RepairInStock>> QueryAsync(string wheresql) 
        {
            List<tb_AS_RepairInStock> list = await _tb_AS_RepairInStockServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairInStock>> QueryAsync(Expression<Func<tb_AS_RepairInStock, bool>> exp)
        {
            List<tb_AS_RepairInStock> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairInStock>> QueryByNavAsync()
        {
            List<tb_AS_RepairInStock> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_as_repairorder )
                                            .Includes(t => t.tb_AS_RepairInStockDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairInStock>> QueryByNavAsync(Expression<Func<tb_AS_RepairInStock, bool>> exp)
        {
            List<tb_AS_RepairInStock> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_as_repairorder )
                                            .Includes(t => t.tb_AS_RepairInStockDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_AS_RepairInStock> QueryByNav(Expression<Func<tb_AS_RepairInStock, bool>> exp)
        {
            List<tb_AS_RepairInStock> list = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_as_repairorder )
                                        .Includes(t => t.tb_AS_RepairInStockDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairInStock>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairInStock>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_AS_RepairInStockServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_AS_RepairInStock entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairInStock>().Where(w => w.RepairInStockID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_as_repairorder )
                        

                                            .Includes(t => t.tb_AS_RepairInStockDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairInStock>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



