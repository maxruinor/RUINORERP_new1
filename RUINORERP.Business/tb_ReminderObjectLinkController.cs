// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/09/2026 20:34:51
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
    /// 提醒对象链路
    /// </summary>
    public partial class tb_ReminderObjectLinkController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReminderObjectLinkServices _tb_ReminderObjectLinkServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ReminderObjectLinkController(ILogger<tb_ReminderObjectLinkController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReminderObjectLinkServices tb_ReminderObjectLinkServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReminderObjectLinkServices = tb_ReminderObjectLinkServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ReminderObjectLink info)
        {

           // tb_ReminderObjectLinkValidator validator = new tb_ReminderObjectLinkValidator();
           tb_ReminderObjectLinkValidator validator = _appContext.GetRequiredService<tb_ReminderObjectLinkValidator>();
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
        public async Task<ReturnResults<tb_ReminderObjectLink>> SaveOrUpdate(tb_ReminderObjectLink entity)
        {
            ReturnResults<tb_ReminderObjectLink> rr = new ReturnResults<tb_ReminderObjectLink>();
            tb_ReminderObjectLink Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LinkId > 0)
                {
                    bool rs = await _tb_ReminderObjectLinkServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReminderObjectLinkServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(entity);
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
            tb_ReminderObjectLink entity = model as tb_ReminderObjectLink;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.LinkId > 0)
                {
                    bool rs = await _tb_ReminderObjectLinkServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReminderObjectLinkServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(entity);
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
            List<T> list = await _tb_ReminderObjectLinkServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ReminderObjectLink entity = item as tb_ReminderObjectLink;
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
            List<T> list = await _tb_ReminderObjectLinkServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ReminderObjectLink entity = item as tb_ReminderObjectLink;
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
            tb_ReminderObjectLink entity = model as tb_ReminderObjectLink;
            bool rs = await _tb_ReminderObjectLinkServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ReminderObjectLink>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ReminderObjectLink> entitys = models as List<tb_ReminderObjectLink>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ReminderObjectLink>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ReminderObjectLink>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReminderObjectLinkValidator validator = new tb_ReminderObjectLinkValidator();
           tb_ReminderObjectLinkValidator validator = _appContext.GetRequiredService<tb_ReminderObjectLinkValidator>();
            ValidationResult results = validator.Validate(info as tb_ReminderObjectLink);
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

                tb_ReminderObjectLink entity = model as tb_ReminderObjectLink;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.LinkId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ReminderObjectLink>(entity as tb_ReminderObjectLink)
                        .Include(m => m.tb_ReminderLinkRuleRelations)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ReminderObjectLink>(entity as tb_ReminderObjectLink)
                .Include(m => m.tb_ReminderLinkRuleRelations)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.LinkId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>()
                                                .Includes(m => m.tb_ReminderLinkRuleRelations)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ReminderObjectLink entity = model as tb_ReminderObjectLink;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ReminderObjectLink>(m => m.LinkId== entity.LinkId)
                                .Include(m => m.tb_ReminderLinkRuleRelations)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ReminderObjectLink AddReEntity(tb_ReminderObjectLink entity)
        {
            tb_ReminderObjectLink AddEntity =  _tb_ReminderObjectLinkServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ReminderObjectLink> AddReEntityAsync(tb_ReminderObjectLink entity)
        {
            tb_ReminderObjectLink AddEntity = await _tb_ReminderObjectLinkServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ReminderObjectLink entity)
        {
            long id = await _tb_ReminderObjectLinkServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ReminderObjectLink> infos)
        {
            List<long> ids = await _tb_ReminderObjectLinkServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ReminderObjectLink entity)
        {
            bool rs = await _tb_ReminderObjectLinkServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ReminderObjectLink>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ReminderObjectLink entity)
        {
            bool rs = await _tb_ReminderObjectLinkServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ReminderObjectLink>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReminderObjectLinkServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ReminderObjectLink>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReminderObjectLinkServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ReminderObjectLink>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ReminderObjectLink>> QueryAsync()
        {
            List<tb_ReminderObjectLink> list = await  _tb_ReminderObjectLinkServices.QueryAsync();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        
        public virtual List<tb_ReminderObjectLink> Query()
        {
            List<tb_ReminderObjectLink> list =  _tb_ReminderObjectLinkServices.Query();
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        
        public virtual List<tb_ReminderObjectLink> Query(string wheresql)
        {
            List<tb_ReminderObjectLink> list =  _tb_ReminderObjectLinkServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ReminderObjectLink>> QueryAsync(string wheresql) 
        {
            List<tb_ReminderObjectLink> list = await _tb_ReminderObjectLinkServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ReminderObjectLink>> QueryAsync(Expression<Func<tb_ReminderObjectLink, bool>> exp)
        {
            List<tb_ReminderObjectLink> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderObjectLink>> QueryByNavAsync()
        {
            List<tb_ReminderObjectLink> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>()
                                            .Includes(t => t.tb_ReminderLinkRuleRelations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderObjectLink>> QueryByNavAsync(Expression<Func<tb_ReminderObjectLink, bool>> exp)
        {
            List<tb_ReminderObjectLink> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>().Where(exp)
                                            .Includes(t => t.tb_ReminderLinkRuleRelations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ReminderObjectLink> QueryByNav(Expression<Func<tb_ReminderObjectLink, bool>> exp)
        {
            List<tb_ReminderObjectLink> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>().Where(exp)
                                        .Includes(t => t.tb_ReminderLinkRuleRelations )
                        .ToList();
            
            foreach (var item in list)
            {
               item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ReminderObjectLink>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ReminderObjectLink>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReminderObjectLinkServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ReminderObjectLink entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderObjectLink>().Where(w => w.LinkId == (long)id)
                         

                                            .Includes(t => t.tb_ReminderLinkRuleRelations )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ReminderObjectLink>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



