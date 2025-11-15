// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:19
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
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析
    /// </summary>
    public partial class tb_ProductionPlanController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProductionPlanServices _tb_ProductionPlanServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ProductionPlanController(ILogger<tb_ProductionPlanController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProductionPlanServices tb_ProductionPlanServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProductionPlanServices = tb_ProductionPlanServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ProductionPlan info)
        {

           // tb_ProductionPlanValidator validator = new tb_ProductionPlanValidator();
           tb_ProductionPlanValidator validator = _appContext.GetRequiredService<tb_ProductionPlanValidator>();
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
        public async Task<ReturnResults<tb_ProductionPlan>> SaveOrUpdate(tb_ProductionPlan entity)
        {
            ReturnResults<tb_ProductionPlan> rr = new ReturnResults<tb_ProductionPlan>();
            tb_ProductionPlan Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PPID > 0)
                {
                    bool rs = await _tb_ProductionPlanServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProductionPlanServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(entity);
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
            tb_ProductionPlan entity = model as tb_ProductionPlan;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PPID > 0)
                {
                    bool rs = await _tb_ProductionPlanServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProductionPlanServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(entity);
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
            List<T> list = await _tb_ProductionPlanServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProductionPlan entity = item as tb_ProductionPlan;
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
            List<T> list = await _tb_ProductionPlanServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProductionPlan entity = item as tb_ProductionPlan;
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
            tb_ProductionPlan entity = model as tb_ProductionPlan;
            bool rs = await _tb_ProductionPlanServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ProductionPlan>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProductionPlan> entitys = models as List<tb_ProductionPlan>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProductionPlan>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ProductionPlan>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProductionPlanValidator validator = new tb_ProductionPlanValidator();
           tb_ProductionPlanValidator validator = _appContext.GetRequiredService<tb_ProductionPlanValidator>();
            ValidationResult results = validator.Validate(info as tb_ProductionPlan);
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

                tb_ProductionPlan entity = model as tb_ProductionPlan;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PPID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProductionPlan>(entity as tb_ProductionPlan)
                        .Include(m => m.tb_ProductionPlanDetails)
                    .Include(m => m.tb_ProductionDemands)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProductionPlan>(entity as tb_ProductionPlan)
                .Include(m => m.tb_ProductionPlanDetails)
                .Include(m => m.tb_ProductionDemands)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PPID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>()
                                .Includes(m => m.tb_ProductionPlanDetails)
                        .Includes(m => m.tb_ProductionDemands)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProductionPlan entity = model as tb_ProductionPlan;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProductionPlan>(m => m.PPID== entity.PPID)
                                .Include(m => m.tb_ProductionPlanDetails)
                        .Include(m => m.tb_ProductionDemands)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProductionPlan AddReEntity(tb_ProductionPlan entity)
        {
            tb_ProductionPlan AddEntity =  _tb_ProductionPlanServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProductionPlan> AddReEntityAsync(tb_ProductionPlan entity)
        {
            tb_ProductionPlan AddEntity = await _tb_ProductionPlanServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProductionPlan entity)
        {
            long id = await _tb_ProductionPlanServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProductionPlan> infos)
        {
            List<long> ids = await _tb_ProductionPlanServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProductionPlan entity)
        {
            bool rs = await _tb_ProductionPlanServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ProductionPlan>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProductionPlan entity)
        {
            bool rs = await _tb_ProductionPlanServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ProductionPlan>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProductionPlanServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ProductionPlan>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProductionPlanServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ProductionPlan>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProductionPlan>> QueryAsync()
        {
            List<tb_ProductionPlan> list = await  _tb_ProductionPlanServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        
        public virtual List<tb_ProductionPlan> Query()
        {
            List<tb_ProductionPlan> list =  _tb_ProductionPlanServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        
        public virtual List<tb_ProductionPlan> Query(string wheresql)
        {
            List<tb_ProductionPlan> list =  _tb_ProductionPlanServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProductionPlan>> QueryAsync(string wheresql) 
        {
            List<tb_ProductionPlan> list = await _tb_ProductionPlanServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProductionPlan>> QueryAsync(Expression<Func<tb_ProductionPlan, bool>> exp)
        {
            List<tb_ProductionPlan> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProductionPlan>> QueryByNavAsync()
        {
            List<tb_ProductionPlan> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>()
                               .Includes(t => t.tb_saleorder )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                            .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_ProductionDemands )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProductionPlan>> QueryByNavAsync(Expression<Func<tb_ProductionPlan, bool>> exp)
        {
            List<tb_ProductionPlan> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>().Where(exp)
                               .Includes(t => t.tb_saleorder )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                            .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_ProductionDemands )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProductionPlan> QueryByNav(Expression<Func<tb_ProductionPlan, bool>> exp)
        {
            List<tb_ProductionPlan> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>().Where(exp)
                            .Includes(t => t.tb_saleorder )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                        .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_ProductionDemands )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionPlan>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProductionPlan>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProductionPlanServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProductionPlan entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionPlan>().Where(w => w.PPID == (long)id)
                             .Includes(t => t.tb_saleorder )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                        

                                            .Includes(t => t.tb_ProductionPlanDetails )
                                            .Includes(t => t.tb_ProductionDemands )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ProductionPlan>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



