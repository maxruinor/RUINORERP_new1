// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:16
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
    /// 打印模板
    /// </summary>
    public partial class tb_PrintTemplateController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PrintTemplateServices _tb_PrintTemplateServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_PrintTemplateController(ILogger<tb_PrintTemplateController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PrintTemplateServices tb_PrintTemplateServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PrintTemplateServices = tb_PrintTemplateServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_PrintTemplate info)
        {

           // tb_PrintTemplateValidator validator = new tb_PrintTemplateValidator();
           tb_PrintTemplateValidator validator = _appContext.GetRequiredService<tb_PrintTemplateValidator>();
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
        public async Task<ReturnResults<tb_PrintTemplate>> SaveOrUpdate(tb_PrintTemplate entity)
        {
            ReturnResults<tb_PrintTemplate> rr = new ReturnResults<tb_PrintTemplate>();
            tb_PrintTemplate Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_PrintTemplateServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PrintTemplateServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(entity);
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
            tb_PrintTemplate entity = model as tb_PrintTemplate;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_PrintTemplateServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PrintTemplateServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(entity);
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
            List<T> list = await _tb_PrintTemplateServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PrintTemplate entity = item as tb_PrintTemplate;
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
            List<T> list = await _tb_PrintTemplateServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PrintTemplate entity = item as tb_PrintTemplate;
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
            tb_PrintTemplate entity = model as tb_PrintTemplate;
            bool rs = await _tb_PrintTemplateServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_PrintTemplate>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PrintTemplate> entitys = models as List<tb_PrintTemplate>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PrintTemplate>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_PrintTemplate>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PrintTemplateValidator validator = new tb_PrintTemplateValidator();
           tb_PrintTemplateValidator validator = _appContext.GetRequiredService<tb_PrintTemplateValidator>();
            ValidationResult results = validator.Validate(info as tb_PrintTemplate);
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

                tb_PrintTemplate entity = model as tb_PrintTemplate;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_PrintTemplate>(entity as tb_PrintTemplate)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_PrintTemplate>(entity as tb_PrintTemplate)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PrintTemplate entity = model as tb_PrintTemplate;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PrintTemplate>(m => m.ID== entity.ID)
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
        
        
        
        public tb_PrintTemplate AddReEntity(tb_PrintTemplate entity)
        {
            tb_PrintTemplate AddEntity =  _tb_PrintTemplateServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PrintTemplate> AddReEntityAsync(tb_PrintTemplate entity)
        {
            tb_PrintTemplate AddEntity = await _tb_PrintTemplateServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PrintTemplate entity)
        {
            long id = await _tb_PrintTemplateServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PrintTemplate> infos)
        {
            List<long> ids = await _tb_PrintTemplateServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PrintTemplate entity)
        {
            bool rs = await _tb_PrintTemplateServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_PrintTemplate>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PrintTemplate entity)
        {
            bool rs = await _tb_PrintTemplateServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_PrintTemplate>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PrintTemplateServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_PrintTemplate>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PrintTemplateServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_PrintTemplate>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PrintTemplate>> QueryAsync()
        {
            List<tb_PrintTemplate> list = await  _tb_PrintTemplateServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        
        public virtual List<tb_PrintTemplate> Query()
        {
            List<tb_PrintTemplate> list =  _tb_PrintTemplateServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        
        public virtual List<tb_PrintTemplate> Query(string wheresql)
        {
            List<tb_PrintTemplate> list =  _tb_PrintTemplateServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PrintTemplate>> QueryAsync(string wheresql) 
        {
            List<tb_PrintTemplate> list = await _tb_PrintTemplateServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PrintTemplate>> QueryAsync(Expression<Func<tb_PrintTemplate, bool>> exp)
        {
            List<tb_PrintTemplate> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PrintTemplate>> QueryByNavAsync()
        {
            List<tb_PrintTemplate> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>()
                               .Includes(t => t.tb_printconfig )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PrintTemplate>> QueryByNavAsync(Expression<Func<tb_PrintTemplate, bool>> exp)
        {
            List<tb_PrintTemplate> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>().Where(exp)
                               .Includes(t => t.tb_printconfig )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PrintTemplate> QueryByNav(Expression<Func<tb_PrintTemplate, bool>> exp)
        {
            List<tb_PrintTemplate> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>().Where(exp)
                            .Includes(t => t.tb_printconfig )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PrintTemplate>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PrintTemplate>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PrintTemplateServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PrintTemplate entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PrintTemplate>().Where(w => w.ID == (long)id)
                             .Includes(t => t.tb_printconfig )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_PrintTemplate>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



