// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:01
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
    /// 客户厂商认证文件表
    /// </summary>
    public partial class tb_CustomerVendorFilesController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_CustomerVendorFilesServices _tb_CustomerVendorFilesServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_CustomerVendorFilesController(ILogger<tb_CustomerVendorFilesController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_CustomerVendorFilesServices tb_CustomerVendorFilesServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_CustomerVendorFilesServices = tb_CustomerVendorFilesServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_CustomerVendorFiles info)
        {

           // tb_CustomerVendorFilesValidator validator = new tb_CustomerVendorFilesValidator();
           tb_CustomerVendorFilesValidator validator = _appContext.GetRequiredService<tb_CustomerVendorFilesValidator>();
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
        public async Task<ReturnResults<tb_CustomerVendorFiles>> SaveOrUpdate(tb_CustomerVendorFiles entity)
        {
            ReturnResults<tb_CustomerVendorFiles> rr = new ReturnResults<tb_CustomerVendorFiles>();
            tb_CustomerVendorFiles Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.File_ID > 0)
                {
                    bool rs = await _tb_CustomerVendorFilesServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorFilesServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(entity);
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
            tb_CustomerVendorFiles entity = model as tb_CustomerVendorFiles;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.File_ID > 0)
                {
                    bool rs = await _tb_CustomerVendorFilesServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_CustomerVendorFilesServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(entity);
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
            List<T> list = await _tb_CustomerVendorFilesServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_CustomerVendorFiles entity = item as tb_CustomerVendorFiles;
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
            List<T> list = await _tb_CustomerVendorFilesServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_CustomerVendorFiles entity = item as tb_CustomerVendorFiles;
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
            tb_CustomerVendorFiles entity = model as tb_CustomerVendorFiles;
            bool rs = await _tb_CustomerVendorFilesServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorFiles>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_CustomerVendorFiles> entitys = models as List<tb_CustomerVendorFiles>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_CustomerVendorFiles>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_CustomerVendorFiles>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_CustomerVendorFilesValidator validator = new tb_CustomerVendorFilesValidator();
           tb_CustomerVendorFilesValidator validator = _appContext.GetRequiredService<tb_CustomerVendorFilesValidator>();
            ValidationResult results = validator.Validate(info as tb_CustomerVendorFiles);
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

                tb_CustomerVendorFiles entity = model as tb_CustomerVendorFiles;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.File_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_CustomerVendorFiles>(entity as tb_CustomerVendorFiles)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_CustomerVendorFiles>(entity as tb_CustomerVendorFiles)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.File_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_CustomerVendorFiles entity = model as tb_CustomerVendorFiles;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CustomerVendorFiles>(m => m.File_ID== entity.File_ID)
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
        
        
        
        public tb_CustomerVendorFiles AddReEntity(tb_CustomerVendorFiles entity)
        {
            tb_CustomerVendorFiles AddEntity =  _tb_CustomerVendorFilesServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_CustomerVendorFiles> AddReEntityAsync(tb_CustomerVendorFiles entity)
        {
            tb_CustomerVendorFiles AddEntity = await _tb_CustomerVendorFilesServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_CustomerVendorFiles entity)
        {
            long id = await _tb_CustomerVendorFilesServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_CustomerVendorFiles> infos)
        {
            List<long> ids = await _tb_CustomerVendorFilesServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_CustomerVendorFiles entity)
        {
            bool rs = await _tb_CustomerVendorFilesServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorFiles>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_CustomerVendorFiles entity)
        {
            bool rs = await _tb_CustomerVendorFilesServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorFiles>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_CustomerVendorFilesServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_CustomerVendorFiles>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_CustomerVendorFilesServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_CustomerVendorFiles>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_CustomerVendorFiles>> QueryAsync()
        {
            List<tb_CustomerVendorFiles> list = await  _tb_CustomerVendorFilesServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendorFiles> Query()
        {
            List<tb_CustomerVendorFiles> list =  _tb_CustomerVendorFilesServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        
        public virtual List<tb_CustomerVendorFiles> Query(string wheresql)
        {
            List<tb_CustomerVendorFiles> list =  _tb_CustomerVendorFilesServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        
        public virtual async Task<List<tb_CustomerVendorFiles>> QueryAsync(string wheresql) 
        {
            List<tb_CustomerVendorFiles> list = await _tb_CustomerVendorFilesServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendorFiles>> QueryAsync(Expression<Func<tb_CustomerVendorFiles, bool>> exp)
        {
            List<tb_CustomerVendorFiles> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendorFiles>> QueryByNavAsync()
        {
            List<tb_CustomerVendorFiles> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>()
                               .Includes(t => t.tb_customervendor )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_CustomerVendorFiles>> QueryByNavAsync(Expression<Func<tb_CustomerVendorFiles, bool>> exp)
        {
            List<tb_CustomerVendorFiles> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_CustomerVendorFiles> QueryByNav(Expression<Func<tb_CustomerVendorFiles, bool>> exp)
        {
            List<tb_CustomerVendorFiles> list = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_CustomerVendorFiles>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_CustomerVendorFiles>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_CustomerVendorFilesServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_CustomerVendorFiles entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_CustomerVendorFiles>().Where(w => w.File_ID == (long)id)
                             .Includes(t => t.tb_customervendor )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_CustomerVendorFiles>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



