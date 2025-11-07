// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:14
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
    /// 返工入库单明细
    /// </summary>
    public partial class tb_MRP_ReworkEntryDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MRP_ReworkEntryDetailServices _tb_MRP_ReworkEntryDetailServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_MRP_ReworkEntryDetailController(ILogger<tb_MRP_ReworkEntryDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MRP_ReworkEntryDetailServices tb_MRP_ReworkEntryDetailServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MRP_ReworkEntryDetailServices = tb_MRP_ReworkEntryDetailServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_MRP_ReworkEntryDetail info)
        {

           // tb_MRP_ReworkEntryDetailValidator validator = new tb_MRP_ReworkEntryDetailValidator();
           tb_MRP_ReworkEntryDetailValidator validator = _appContext.GetRequiredService<tb_MRP_ReworkEntryDetailValidator>();
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
        public async Task<ReturnResults<tb_MRP_ReworkEntryDetail>> SaveOrUpdate(tb_MRP_ReworkEntryDetail entity)
        {
            ReturnResults<tb_MRP_ReworkEntryDetail> rr = new ReturnResults<tb_MRP_ReworkEntryDetail>();
            tb_MRP_ReworkEntryDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReworkEntryCID > 0)
                {
                    bool rs = await _tb_MRP_ReworkEntryDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MRP_ReworkEntryDetailServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(entity);
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
            tb_MRP_ReworkEntryDetail entity = model as tb_MRP_ReworkEntryDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReworkEntryCID > 0)
                {
                    bool rs = await _tb_MRP_ReworkEntryDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MRP_ReworkEntryDetailServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(entity);
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
            List<T> list = await _tb_MRP_ReworkEntryDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MRP_ReworkEntryDetail entity = item as tb_MRP_ReworkEntryDetail;
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
            List<T> list = await _tb_MRP_ReworkEntryDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MRP_ReworkEntryDetail entity = item as tb_MRP_ReworkEntryDetail;
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
            tb_MRP_ReworkEntryDetail entity = model as tb_MRP_ReworkEntryDetail;
            bool rs = await _tb_MRP_ReworkEntryDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_MRP_ReworkEntryDetail>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MRP_ReworkEntryDetail> entitys = models as List<tb_MRP_ReworkEntryDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MRP_ReworkEntryDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_MRP_ReworkEntryDetail>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MRP_ReworkEntryDetailValidator validator = new tb_MRP_ReworkEntryDetailValidator();
           tb_MRP_ReworkEntryDetailValidator validator = _appContext.GetRequiredService<tb_MRP_ReworkEntryDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_MRP_ReworkEntryDetail);
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

                tb_MRP_ReworkEntryDetail entity = model as tb_MRP_ReworkEntryDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ReworkEntryCID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_MRP_ReworkEntryDetail>(entity as tb_MRP_ReworkEntryDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_MRP_ReworkEntryDetail>(entity as tb_MRP_ReworkEntryDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ReworkEntryCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MRP_ReworkEntryDetail entity = model as tb_MRP_ReworkEntryDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MRP_ReworkEntryDetail>(m => m.ReworkEntryCID== entity.ReworkEntryCID)
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
        
        
        
        public tb_MRP_ReworkEntryDetail AddReEntity(tb_MRP_ReworkEntryDetail entity)
        {
            tb_MRP_ReworkEntryDetail AddEntity =  _tb_MRP_ReworkEntryDetailServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MRP_ReworkEntryDetail> AddReEntityAsync(tb_MRP_ReworkEntryDetail entity)
        {
            tb_MRP_ReworkEntryDetail AddEntity = await _tb_MRP_ReworkEntryDetailServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MRP_ReworkEntryDetail entity)
        {
            long id = await _tb_MRP_ReworkEntryDetailServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MRP_ReworkEntryDetail> infos)
        {
            List<long> ids = await _tb_MRP_ReworkEntryDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MRP_ReworkEntryDetail entity)
        {
            bool rs = await _tb_MRP_ReworkEntryDetailServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_MRP_ReworkEntryDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MRP_ReworkEntryDetail entity)
        {
            bool rs = await _tb_MRP_ReworkEntryDetailServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_MRP_ReworkEntryDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MRP_ReworkEntryDetailServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_MRP_ReworkEntryDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MRP_ReworkEntryDetailServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_MRP_ReworkEntryDetail>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MRP_ReworkEntryDetail>> QueryAsync()
        {
            List<tb_MRP_ReworkEntryDetail> list = await  _tb_MRP_ReworkEntryDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        
        public virtual List<tb_MRP_ReworkEntryDetail> Query()
        {
            List<tb_MRP_ReworkEntryDetail> list =  _tb_MRP_ReworkEntryDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        
        public virtual List<tb_MRP_ReworkEntryDetail> Query(string wheresql)
        {
            List<tb_MRP_ReworkEntryDetail> list =  _tb_MRP_ReworkEntryDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MRP_ReworkEntryDetail>> QueryAsync(string wheresql) 
        {
            List<tb_MRP_ReworkEntryDetail> list = await _tb_MRP_ReworkEntryDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MRP_ReworkEntryDetail>> QueryAsync(Expression<Func<tb_MRP_ReworkEntryDetail, bool>> exp)
        {
            List<tb_MRP_ReworkEntryDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MRP_ReworkEntryDetail>> QueryByNavAsync()
        {
            List<tb_MRP_ReworkEntryDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_mrp_reworkentry )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MRP_ReworkEntryDetail>> QueryByNavAsync(Expression<Func<tb_MRP_ReworkEntryDetail, bool>> exp)
        {
            List<tb_MRP_ReworkEntryDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_mrp_reworkentry )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MRP_ReworkEntryDetail> QueryByNav(Expression<Func<tb_MRP_ReworkEntryDetail, bool>> exp)
        {
            List<tb_MRP_ReworkEntryDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_mrp_reworkentry )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_MRP_ReworkEntryDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MRP_ReworkEntryDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MRP_ReworkEntryDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MRP_ReworkEntryDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntryDetail>().Where(w => w.ReworkEntryCID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_mrp_reworkentry )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_MRP_ReworkEntryDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



