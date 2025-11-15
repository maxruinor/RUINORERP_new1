// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:20
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
    /// 采购入库单
    /// </summary>
    public partial class tb_PurEntryDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PurEntryDetailServices _tb_PurEntryDetailServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_PurEntryDetailController(ILogger<tb_PurEntryDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PurEntryDetailServices tb_PurEntryDetailServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PurEntryDetailServices = tb_PurEntryDetailServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_PurEntryDetail info)
        {

           // tb_PurEntryDetailValidator validator = new tb_PurEntryDetailValidator();
           tb_PurEntryDetailValidator validator = _appContext.GetRequiredService<tb_PurEntryDetailValidator>();
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
        public async Task<ReturnResults<tb_PurEntryDetail>> SaveOrUpdate(tb_PurEntryDetail entity)
        {
            ReturnResults<tb_PurEntryDetail> rr = new ReturnResults<tb_PurEntryDetail>();
            tb_PurEntryDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurEntryDetail_ID > 0)
                {
                    bool rs = await _tb_PurEntryDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PurEntryDetailServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(entity);
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
            tb_PurEntryDetail entity = model as tb_PurEntryDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurEntryDetail_ID > 0)
                {
                    bool rs = await _tb_PurEntryDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PurEntryDetailServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(entity);
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
            List<T> list = await _tb_PurEntryDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PurEntryDetail entity = item as tb_PurEntryDetail;
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
            List<T> list = await _tb_PurEntryDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PurEntryDetail entity = item as tb_PurEntryDetail;
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
            tb_PurEntryDetail entity = model as tb_PurEntryDetail;
            bool rs = await _tb_PurEntryDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_PurEntryDetail>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PurEntryDetail> entitys = models as List<tb_PurEntryDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PurEntryDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_PurEntryDetail>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PurEntryDetailValidator validator = new tb_PurEntryDetailValidator();
           tb_PurEntryDetailValidator validator = _appContext.GetRequiredService<tb_PurEntryDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_PurEntryDetail);
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

                tb_PurEntryDetail entity = model as tb_PurEntryDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PurEntryDetail_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_PurEntryDetail>(entity as tb_PurEntryDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_PurEntryDetail>(entity as tb_PurEntryDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PurEntryDetail_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PurEntryDetail entity = model as tb_PurEntryDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PurEntryDetail>(m => m.PurEntryDetail_ID== entity.PurEntryDetail_ID)
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
        
        
        
        public tb_PurEntryDetail AddReEntity(tb_PurEntryDetail entity)
        {
            tb_PurEntryDetail AddEntity =  _tb_PurEntryDetailServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PurEntryDetail> AddReEntityAsync(tb_PurEntryDetail entity)
        {
            tb_PurEntryDetail AddEntity = await _tb_PurEntryDetailServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PurEntryDetail entity)
        {
            long id = await _tb_PurEntryDetailServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PurEntryDetail> infos)
        {
            List<long> ids = await _tb_PurEntryDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PurEntryDetail entity)
        {
            bool rs = await _tb_PurEntryDetailServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_PurEntryDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PurEntryDetail entity)
        {
            bool rs = await _tb_PurEntryDetailServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_PurEntryDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PurEntryDetailServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_PurEntryDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PurEntryDetailServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_PurEntryDetail>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PurEntryDetail>> QueryAsync()
        {
            List<tb_PurEntryDetail> list = await  _tb_PurEntryDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        
        public virtual List<tb_PurEntryDetail> Query()
        {
            List<tb_PurEntryDetail> list =  _tb_PurEntryDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        
        public virtual List<tb_PurEntryDetail> Query(string wheresql)
        {
            List<tb_PurEntryDetail> list =  _tb_PurEntryDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PurEntryDetail>> QueryAsync(string wheresql) 
        {
            List<tb_PurEntryDetail> list = await _tb_PurEntryDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PurEntryDetail>> QueryAsync(Expression<Func<tb_PurEntryDetail, bool>> exp)
        {
            List<tb_PurEntryDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurEntryDetail>> QueryByNavAsync()
        {
            List<tb_PurEntryDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>()
                               .Includes(t => t.tb_purentry )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_storagerack )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurEntryDetail>> QueryByNavAsync(Expression<Func<tb_PurEntryDetail, bool>> exp)
        {
            List<tb_PurEntryDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(exp)
                               .Includes(t => t.tb_purentry )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_storagerack )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PurEntryDetail> QueryByNav(Expression<Func<tb_PurEntryDetail, bool>> exp)
        {
            List<tb_PurEntryDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(exp)
                            .Includes(t => t.tb_purentry )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_storagerack )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_PurEntryDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PurEntryDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PurEntryDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PurEntryDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryDetail>().Where(w => w.PurEntryDetail_ID == (long)id)
                             .Includes(t => t.tb_purentry )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_storagerack )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_PurEntryDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



