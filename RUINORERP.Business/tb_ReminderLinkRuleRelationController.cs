// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:58:59
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
    /// 链路与规则关联表
    /// </summary>
    public partial class tb_ReminderLinkRuleRelationController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReminderLinkRuleRelationServices _tb_ReminderLinkRuleRelationServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ReminderLinkRuleRelationController(ILogger<tb_ReminderLinkRuleRelationController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReminderLinkRuleRelationServices tb_ReminderLinkRuleRelationServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReminderLinkRuleRelationServices = tb_ReminderLinkRuleRelationServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ReminderLinkRuleRelation info)
        {

           // tb_ReminderLinkRuleRelationValidator validator = new tb_ReminderLinkRuleRelationValidator();
           tb_ReminderLinkRuleRelationValidator validator = _appContext.GetRequiredService<tb_ReminderLinkRuleRelationValidator>();
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
        public async Task<ReturnResults<tb_ReminderLinkRuleRelation>> SaveOrUpdate(tb_ReminderLinkRuleRelation entity)
        {
            ReturnResults<tb_ReminderLinkRuleRelation> rr = new ReturnResults<tb_ReminderLinkRuleRelation>();
            tb_ReminderLinkRuleRelation Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RelationId > 0)
                {
                    bool rs = await _tb_ReminderLinkRuleRelationServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReminderLinkRuleRelationServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(entity);
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
            tb_ReminderLinkRuleRelation entity = model as tb_ReminderLinkRuleRelation;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RelationId > 0)
                {
                    bool rs = await _tb_ReminderLinkRuleRelationServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReminderLinkRuleRelationServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(entity);
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
            List<T> list = await _tb_ReminderLinkRuleRelationServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ReminderLinkRuleRelation entity = item as tb_ReminderLinkRuleRelation;
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
            List<T> list = await _tb_ReminderLinkRuleRelationServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ReminderLinkRuleRelation entity = item as tb_ReminderLinkRuleRelation;
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
            tb_ReminderLinkRuleRelation entity = model as tb_ReminderLinkRuleRelation;
            bool rs = await _tb_ReminderLinkRuleRelationServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ReminderLinkRuleRelation>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ReminderLinkRuleRelation> entitys = models as List<tb_ReminderLinkRuleRelation>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ReminderLinkRuleRelation>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ReminderLinkRuleRelation>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReminderLinkRuleRelationValidator validator = new tb_ReminderLinkRuleRelationValidator();
           tb_ReminderLinkRuleRelationValidator validator = _appContext.GetRequiredService<tb_ReminderLinkRuleRelationValidator>();
            ValidationResult results = validator.Validate(info as tb_ReminderLinkRuleRelation);
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

                tb_ReminderLinkRuleRelation entity = model as tb_ReminderLinkRuleRelation;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RelationId > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ReminderLinkRuleRelation>(entity as tb_ReminderLinkRuleRelation)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ReminderLinkRuleRelation>(entity as tb_ReminderLinkRuleRelation)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RelationId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>()
                                .Includes(m => m.tb_reminderobjectlink)
                            .Includes(m => m.tb_reminderrule)
                                            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ReminderLinkRuleRelation entity = model as tb_ReminderLinkRuleRelation;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ReminderLinkRuleRelation>(m => m.RelationId== entity.RelationId)
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
        
        
        
        public tb_ReminderLinkRuleRelation AddReEntity(tb_ReminderLinkRuleRelation entity)
        {
            tb_ReminderLinkRuleRelation AddEntity =  _tb_ReminderLinkRuleRelationServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ReminderLinkRuleRelation> AddReEntityAsync(tb_ReminderLinkRuleRelation entity)
        {
            tb_ReminderLinkRuleRelation AddEntity = await _tb_ReminderLinkRuleRelationServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ReminderLinkRuleRelation entity)
        {
            long id = await _tb_ReminderLinkRuleRelationServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ReminderLinkRuleRelation> infos)
        {
            List<long> ids = await _tb_ReminderLinkRuleRelationServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ReminderLinkRuleRelation entity)
        {
            bool rs = await _tb_ReminderLinkRuleRelationServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ReminderLinkRuleRelation>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ReminderLinkRuleRelation entity)
        {
            bool rs = await _tb_ReminderLinkRuleRelationServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ReminderLinkRuleRelation>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReminderLinkRuleRelationServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ReminderLinkRuleRelation>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReminderLinkRuleRelationServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ReminderLinkRuleRelation>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ReminderLinkRuleRelation>> QueryAsync()
        {
            List<tb_ReminderLinkRuleRelation> list = await  _tb_ReminderLinkRuleRelationServices.QueryAsync();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        
        public virtual List<tb_ReminderLinkRuleRelation> Query()
        {
            List<tb_ReminderLinkRuleRelation> list =  _tb_ReminderLinkRuleRelationServices.Query();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        
        public virtual List<tb_ReminderLinkRuleRelation> Query(string wheresql)
        {
            List<tb_ReminderLinkRuleRelation> list =  _tb_ReminderLinkRuleRelationServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ReminderLinkRuleRelation>> QueryAsync(string wheresql) 
        {
            List<tb_ReminderLinkRuleRelation> list = await _tb_ReminderLinkRuleRelationServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ReminderLinkRuleRelation>> QueryAsync(Expression<Func<tb_ReminderLinkRuleRelation, bool>> exp)
        {
            List<tb_ReminderLinkRuleRelation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderLinkRuleRelation>> QueryByNavAsync()
        {
            List<tb_ReminderLinkRuleRelation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>()
                               .Includes(t => t.tb_reminderobjectlink )
                               .Includes(t => t.tb_reminderrule )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderLinkRuleRelation>> QueryByNavAsync(Expression<Func<tb_ReminderLinkRuleRelation, bool>> exp)
        {
            List<tb_ReminderLinkRuleRelation> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>().Where(exp)
                               .Includes(t => t.tb_reminderobjectlink )
                               .Includes(t => t.tb_reminderrule )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ReminderLinkRuleRelation> QueryByNav(Expression<Func<tb_ReminderLinkRuleRelation, bool>> exp)
        {
            List<tb_ReminderLinkRuleRelation> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>().Where(exp)
                            .Includes(t => t.tb_reminderobjectlink )
                            .Includes(t => t.tb_reminderrule )
                                    .ToList();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderLinkRuleRelation>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ReminderLinkRuleRelation>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReminderLinkRuleRelationServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ReminderLinkRuleRelation entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderLinkRuleRelation>().Where(w => w.RelationId == (long)id)
                             .Includes(t => t.tb_reminderobjectlink )
                            .Includes(t => t.tb_reminderrule )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ReminderLinkRuleRelation>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



