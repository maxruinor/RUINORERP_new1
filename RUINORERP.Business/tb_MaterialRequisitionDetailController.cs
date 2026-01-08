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
    /// 领料单明细
    /// </summary>
    public partial class tb_MaterialRequisitionDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MaterialRequisitionDetailServices _tb_MaterialRequisitionDetailServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_MaterialRequisitionDetailController(ILogger<tb_MaterialRequisitionDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MaterialRequisitionDetailServices tb_MaterialRequisitionDetailServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MaterialRequisitionDetailServices = tb_MaterialRequisitionDetailServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_MaterialRequisitionDetail info)
        {

           // tb_MaterialRequisitionDetailValidator validator = new tb_MaterialRequisitionDetailValidator();
           tb_MaterialRequisitionDetailValidator validator = _appContext.GetRequiredService<tb_MaterialRequisitionDetailValidator>();
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
        public async Task<ReturnResults<tb_MaterialRequisitionDetail>> SaveOrUpdate(tb_MaterialRequisitionDetail entity)
        {
            ReturnResults<tb_MaterialRequisitionDetail> rr = new ReturnResults<tb_MaterialRequisitionDetail>();
            tb_MaterialRequisitionDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Detail_ID > 0)
                {
                    bool rs = await _tb_MaterialRequisitionDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MaterialRequisitionDetailServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(entity);
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
            tb_MaterialRequisitionDetail entity = model as tb_MaterialRequisitionDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Detail_ID > 0)
                {
                    bool rs = await _tb_MaterialRequisitionDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MaterialRequisitionDetailServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(entity);
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
            List<T> list = await _tb_MaterialRequisitionDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MaterialRequisitionDetail entity = item as tb_MaterialRequisitionDetail;
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
            List<T> list = await _tb_MaterialRequisitionDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MaterialRequisitionDetail entity = item as tb_MaterialRequisitionDetail;
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
            tb_MaterialRequisitionDetail entity = model as tb_MaterialRequisitionDetail;
            bool rs = await _tb_MaterialRequisitionDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisitionDetail>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MaterialRequisitionDetail> entitys = models as List<tb_MaterialRequisitionDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MaterialRequisitionDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_MaterialRequisitionDetail>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MaterialRequisitionDetailValidator validator = new tb_MaterialRequisitionDetailValidator();
           tb_MaterialRequisitionDetailValidator validator = _appContext.GetRequiredService<tb_MaterialRequisitionDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_MaterialRequisitionDetail);
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

                tb_MaterialRequisitionDetail entity = model as tb_MaterialRequisitionDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Detail_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_MaterialRequisitionDetail>(entity as tb_MaterialRequisitionDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_MaterialRequisitionDetail>(entity as tb_MaterialRequisitionDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Detail_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MaterialRequisitionDetail entity = model as tb_MaterialRequisitionDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MaterialRequisitionDetail>(m => m.Detail_ID== entity.Detail_ID)
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
        
        
        
        public tb_MaterialRequisitionDetail AddReEntity(tb_MaterialRequisitionDetail entity)
        {
            tb_MaterialRequisitionDetail AddEntity =  _tb_MaterialRequisitionDetailServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MaterialRequisitionDetail> AddReEntityAsync(tb_MaterialRequisitionDetail entity)
        {
            tb_MaterialRequisitionDetail AddEntity = await _tb_MaterialRequisitionDetailServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MaterialRequisitionDetail entity)
        {
            long id = await _tb_MaterialRequisitionDetailServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MaterialRequisitionDetail> infos)
        {
            List<long> ids = await _tb_MaterialRequisitionDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MaterialRequisitionDetail entity)
        {
            bool rs = await _tb_MaterialRequisitionDetailServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisitionDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MaterialRequisitionDetail entity)
        {
            bool rs = await _tb_MaterialRequisitionDetailServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisitionDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MaterialRequisitionDetailServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_MaterialRequisitionDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MaterialRequisitionDetailServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_MaterialRequisitionDetail>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MaterialRequisitionDetail>> QueryAsync()
        {
            List<tb_MaterialRequisitionDetail> list = await  _tb_MaterialRequisitionDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        
        public virtual List<tb_MaterialRequisitionDetail> Query()
        {
            List<tb_MaterialRequisitionDetail> list =  _tb_MaterialRequisitionDetailServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        
        public virtual List<tb_MaterialRequisitionDetail> Query(string wheresql)
        {
            List<tb_MaterialRequisitionDetail> list =  _tb_MaterialRequisitionDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MaterialRequisitionDetail>> QueryAsync(string wheresql) 
        {
            List<tb_MaterialRequisitionDetail> list = await _tb_MaterialRequisitionDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MaterialRequisitionDetail>> QueryAsync(Expression<Func<tb_MaterialRequisitionDetail, bool>> exp)
        {
            List<tb_MaterialRequisitionDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MaterialRequisitionDetail>> QueryByNavAsync()
        {
            List<tb_MaterialRequisitionDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>()
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_materialrequisition )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MaterialRequisitionDetail>> QueryByNavAsync(Expression<Func<tb_MaterialRequisitionDetail, bool>> exp)
        {
            List<tb_MaterialRequisitionDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().Where(exp)
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_materialrequisition )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MaterialRequisitionDetail> QueryByNav(Expression<Func<tb_MaterialRequisitionDetail, bool>> exp)
        {
            List<tb_MaterialRequisitionDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().Where(exp)
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_materialrequisition )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MaterialRequisitionDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MaterialRequisitionDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MaterialRequisitionDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MaterialRequisitionDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MaterialRequisitionDetail>().Where(w => w.Detail_ID == (long)id)
                             .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_materialrequisition )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_MaterialRequisitionDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



