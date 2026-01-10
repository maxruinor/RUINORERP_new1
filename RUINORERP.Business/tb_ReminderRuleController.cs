// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:59:01
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
    /// 提醒规则
    /// </summary>
    public partial class tb_ReminderRuleController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReminderRuleServices _tb_ReminderRuleServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ReminderRuleController(ILogger<tb_ReminderRuleController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReminderRuleServices tb_ReminderRuleServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReminderRuleServices = tb_ReminderRuleServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ReminderRule info)
        {

           // tb_ReminderRuleValidator validator = new tb_ReminderRuleValidator();
           tb_ReminderRuleValidator validator = _appContext.GetRequiredService<tb_ReminderRuleValidator>();
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
        public async Task<ReturnResults<tb_ReminderRule>> SaveOrUpdate(tb_ReminderRule entity)
        {
            ReturnResults<tb_ReminderRule> rr = new ReturnResults<tb_ReminderRule>();
            tb_ReminderRule Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RuleId > 0)
                {
                    bool rs = await _tb_ReminderRuleServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReminderRuleServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(entity);
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
            tb_ReminderRule entity = model as tb_ReminderRule;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RuleId > 0)
                {
                    bool rs = await _tb_ReminderRuleServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReminderRuleServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(entity);
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
            List<T> list = await _tb_ReminderRuleServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ReminderRule entity = item as tb_ReminderRule;
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
            List<T> list = await _tb_ReminderRuleServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ReminderRule entity = item as tb_ReminderRule;
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
            tb_ReminderRule entity = model as tb_ReminderRule;
            bool rs = await _tb_ReminderRuleServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ReminderRule>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ReminderRule> entitys = models as List<tb_ReminderRule>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ReminderRule>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ReminderRule>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReminderRuleValidator validator = new tb_ReminderRuleValidator();
           tb_ReminderRuleValidator validator = _appContext.GetRequiredService<tb_ReminderRuleValidator>();
            ValidationResult results = validator.Validate(info as tb_ReminderRule);
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

                tb_ReminderRule entity = model as tb_ReminderRule;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RuleId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ReminderRule>(entity as tb_ReminderRule)
                        .Include(m => m.tb_ReminderLinkRuleRelations)
                    .Include(m => m.tb_ReminderResults)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ReminderRule>(entity as tb_ReminderRule)
                .Include(m => m.tb_ReminderLinkRuleRelations)
                .Include(m => m.tb_ReminderResults)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RuleId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>()
                                                .Includes(m => m.tb_ReminderLinkRuleRelations)
                        .Includes(m => m.tb_ReminderResults)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ReminderRule entity = model as tb_ReminderRule;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ReminderRule>(m => m.RuleId== entity.RuleId)
                                .Include(m => m.tb_ReminderLinkRuleRelations)
                        .Include(m => m.tb_ReminderResults)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ReminderRule AddReEntity(tb_ReminderRule entity)
        {
            tb_ReminderRule AddEntity =  _tb_ReminderRuleServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ReminderRule> AddReEntityAsync(tb_ReminderRule entity)
        {
            tb_ReminderRule AddEntity = await _tb_ReminderRuleServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ReminderRule entity)
        {
            long id = await _tb_ReminderRuleServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ReminderRule> infos)
        {
            List<long> ids = await _tb_ReminderRuleServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ReminderRule entity)
        {
            bool rs = await _tb_ReminderRuleServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ReminderRule>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ReminderRule entity)
        {
            bool rs = await _tb_ReminderRuleServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ReminderRule>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReminderRuleServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ReminderRule>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReminderRuleServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ReminderRule>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ReminderRule>> QueryAsync()
        {
            List<tb_ReminderRule> list = await  _tb_ReminderRuleServices.QueryAsync();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        
        public virtual List<tb_ReminderRule> Query()
        {
            List<tb_ReminderRule> list =  _tb_ReminderRuleServices.Query();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        
        public virtual List<tb_ReminderRule> Query(string wheresql)
        {
            List<tb_ReminderRule> list =  _tb_ReminderRuleServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ReminderRule>> QueryAsync(string wheresql) 
        {
            List<tb_ReminderRule> list = await _tb_ReminderRuleServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ReminderRule>> QueryAsync(Expression<Func<tb_ReminderRule, bool>> exp)
        {
            List<tb_ReminderRule> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderRule>> QueryByNavAsync()
        {
            List<tb_ReminderRule> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>()
                                            .Includes(t => t.tb_ReminderLinkRuleRelations )
                                .Includes(t => t.tb_ReminderResults )
                        .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderRule>> QueryByNavAsync(Expression<Func<tb_ReminderRule, bool>> exp)
        {
            List<tb_ReminderRule> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>().Where(exp)
                                            .Includes(t => t.tb_ReminderLinkRuleRelations )
                                .Includes(t => t.tb_ReminderResults )
                        .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ReminderRule> QueryByNav(Expression<Func<tb_ReminderRule, bool>> exp)
        {
            List<tb_ReminderRule> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>().Where(exp)
                                        .Includes(t => t.tb_ReminderLinkRuleRelations )
                            .Includes(t => t.tb_ReminderResults )
                        .ToList();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderRule>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ReminderRule>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReminderRuleServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ReminderRule entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderRule>().Where(w => w.RuleId == (long)id)
                         

                                            .Includes(t => t.tb_ReminderLinkRuleRelations )
                                            .Includes(t => t.tb_ReminderResults )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ReminderRule>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



