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
    /// 维修工单明细
    /// </summary>
    public partial class tb_AS_RepairOrderDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_AS_RepairOrderDetailServices _tb_AS_RepairOrderDetailServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_AS_RepairOrderDetailController(ILogger<tb_AS_RepairOrderDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_AS_RepairOrderDetailServices tb_AS_RepairOrderDetailServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_AS_RepairOrderDetailServices = tb_AS_RepairOrderDetailServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_AS_RepairOrderDetail info)
        {

           // tb_AS_RepairOrderDetailValidator validator = new tb_AS_RepairOrderDetailValidator();
           tb_AS_RepairOrderDetailValidator validator = _appContext.GetRequiredService<tb_AS_RepairOrderDetailValidator>();
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
        public async Task<ReturnResults<tb_AS_RepairOrderDetail>> SaveOrUpdate(tb_AS_RepairOrderDetail entity)
        {
            ReturnResults<tb_AS_RepairOrderDetail> rr = new ReturnResults<tb_AS_RepairOrderDetail>();
            tb_AS_RepairOrderDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairOrderDetailID > 0)
                {
                    bool rs = await _tb_AS_RepairOrderDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairOrderDetailServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(entity);
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
            tb_AS_RepairOrderDetail entity = model as tb_AS_RepairOrderDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairOrderDetailID > 0)
                {
                    bool rs = await _tb_AS_RepairOrderDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairOrderDetailServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(entity);
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
            List<T> list = await _tb_AS_RepairOrderDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairOrderDetail entity = item as tb_AS_RepairOrderDetail;
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
            List<T> list = await _tb_AS_RepairOrderDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairOrderDetail entity = item as tb_AS_RepairOrderDetail;
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
            tb_AS_RepairOrderDetail entity = model as tb_AS_RepairOrderDetail;
            bool rs = await _tb_AS_RepairOrderDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairOrderDetail>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_AS_RepairOrderDetail> entitys = models as List<tb_AS_RepairOrderDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_AS_RepairOrderDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_AS_RepairOrderDetail>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_AS_RepairOrderDetailValidator validator = new tb_AS_RepairOrderDetailValidator();
           tb_AS_RepairOrderDetailValidator validator = _appContext.GetRequiredService<tb_AS_RepairOrderDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_AS_RepairOrderDetail);
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

                tb_AS_RepairOrderDetail entity = model as tb_AS_RepairOrderDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RepairOrderDetailID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_RepairOrderDetail>(entity as tb_AS_RepairOrderDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_AS_RepairOrderDetail>(entity as tb_AS_RepairOrderDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RepairOrderDetailID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_AS_RepairOrderDetail entity = model as tb_AS_RepairOrderDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_AS_RepairOrderDetail>(m => m.RepairOrderDetailID== entity.RepairOrderDetailID)
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
        
        
        
        public tb_AS_RepairOrderDetail AddReEntity(tb_AS_RepairOrderDetail entity)
        {
            tb_AS_RepairOrderDetail AddEntity =  _tb_AS_RepairOrderDetailServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_AS_RepairOrderDetail> AddReEntityAsync(tb_AS_RepairOrderDetail entity)
        {
            tb_AS_RepairOrderDetail AddEntity = await _tb_AS_RepairOrderDetailServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_AS_RepairOrderDetail entity)
        {
            long id = await _tb_AS_RepairOrderDetailServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_AS_RepairOrderDetail> infos)
        {
            List<long> ids = await _tb_AS_RepairOrderDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_AS_RepairOrderDetail entity)
        {
            bool rs = await _tb_AS_RepairOrderDetailServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairOrderDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_AS_RepairOrderDetail entity)
        {
            bool rs = await _tb_AS_RepairOrderDetailServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairOrderDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_AS_RepairOrderDetailServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_AS_RepairOrderDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_AS_RepairOrderDetailServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_AS_RepairOrderDetail>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_AS_RepairOrderDetail>> QueryAsync()
        {
            List<tb_AS_RepairOrderDetail> list = await  _tb_AS_RepairOrderDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairOrderDetail> Query()
        {
            List<tb_AS_RepairOrderDetail> list =  _tb_AS_RepairOrderDetailServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairOrderDetail> Query(string wheresql)
        {
            List<tb_AS_RepairOrderDetail> list =  _tb_AS_RepairOrderDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_AS_RepairOrderDetail>> QueryAsync(string wheresql) 
        {
            List<tb_AS_RepairOrderDetail> list = await _tb_AS_RepairOrderDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairOrderDetail>> QueryAsync(Expression<Func<tb_AS_RepairOrderDetail, bool>> exp)
        {
            List<tb_AS_RepairOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairOrderDetail>> QueryByNavAsync()
        {
            List<tb_AS_RepairOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_as_repairorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairOrderDetail>> QueryByNavAsync(Expression<Func<tb_AS_RepairOrderDetail, bool>> exp)
        {
            List<tb_AS_RepairOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_as_repairorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_AS_RepairOrderDetail> QueryByNav(Expression<Func<tb_AS_RepairOrderDetail, bool>> exp)
        {
            List<tb_AS_RepairOrderDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_as_repairorder )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_AS_RepairOrderDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairOrderDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_AS_RepairOrderDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_AS_RepairOrderDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrderDetail>().Where(w => w.RepairOrderDetailID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_as_repairorder )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_AS_RepairOrderDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



