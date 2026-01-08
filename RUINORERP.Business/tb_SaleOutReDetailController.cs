// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:22
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
    /// 销售出库退回明细
    /// </summary>
    public partial class tb_SaleOutReDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_SaleOutReDetailServices _tb_SaleOutReDetailServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_SaleOutReDetailController(ILogger<tb_SaleOutReDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_SaleOutReDetailServices tb_SaleOutReDetailServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_SaleOutReDetailServices = tb_SaleOutReDetailServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_SaleOutReDetail info)
        {

           // tb_SaleOutReDetailValidator validator = new tb_SaleOutReDetailValidator();
           tb_SaleOutReDetailValidator validator = _appContext.GetRequiredService<tb_SaleOutReDetailValidator>();
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
        public async Task<ReturnResults<tb_SaleOutReDetail>> SaveOrUpdate(tb_SaleOutReDetail entity)
        {
            ReturnResults<tb_SaleOutReDetail> rr = new ReturnResults<tb_SaleOutReDetail>();
            tb_SaleOutReDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SOutReturnDetail_ID > 0)
                {
                    bool rs = await _tb_SaleOutReDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_SaleOutReDetailServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(entity);
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
            tb_SaleOutReDetail entity = model as tb_SaleOutReDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SOutReturnDetail_ID > 0)
                {
                    bool rs = await _tb_SaleOutReDetailServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_SaleOutReDetailServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(entity);
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
            List<T> list = await _tb_SaleOutReDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_SaleOutReDetail entity = item as tb_SaleOutReDetail;
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
            List<T> list = await _tb_SaleOutReDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_SaleOutReDetail entity = item as tb_SaleOutReDetail;
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
            tb_SaleOutReDetail entity = model as tb_SaleOutReDetail;
            bool rs = await _tb_SaleOutReDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_SaleOutReDetail>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_SaleOutReDetail> entitys = models as List<tb_SaleOutReDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_SaleOutReDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_SaleOutReDetail>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_SaleOutReDetailValidator validator = new tb_SaleOutReDetailValidator();
           tb_SaleOutReDetailValidator validator = _appContext.GetRequiredService<tb_SaleOutReDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_SaleOutReDetail);
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

                tb_SaleOutReDetail entity = model as tb_SaleOutReDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.SOutReturnDetail_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOutReDetail>(entity as tb_SaleOutReDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_SaleOutReDetail>(entity as tb_SaleOutReDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SOutReturnDetail_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_SaleOutReDetail entity = model as tb_SaleOutReDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_SaleOutReDetail>(m => m.SOutReturnDetail_ID== entity.SOutReturnDetail_ID)
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
        
        
        
        public tb_SaleOutReDetail AddReEntity(tb_SaleOutReDetail entity)
        {
            tb_SaleOutReDetail AddEntity =  _tb_SaleOutReDetailServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_SaleOutReDetail> AddReEntityAsync(tb_SaleOutReDetail entity)
        {
            tb_SaleOutReDetail AddEntity = await _tb_SaleOutReDetailServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_SaleOutReDetail entity)
        {
            long id = await _tb_SaleOutReDetailServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_SaleOutReDetail> infos)
        {
            List<long> ids = await _tb_SaleOutReDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_SaleOutReDetail entity)
        {
            bool rs = await _tb_SaleOutReDetailServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_SaleOutReDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_SaleOutReDetail entity)
        {
            bool rs = await _tb_SaleOutReDetailServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_SaleOutReDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_SaleOutReDetailServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_SaleOutReDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_SaleOutReDetailServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_SaleOutReDetail>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_SaleOutReDetail>> QueryAsync()
        {
            List<tb_SaleOutReDetail> list = await  _tb_SaleOutReDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        
        public virtual List<tb_SaleOutReDetail> Query()
        {
            List<tb_SaleOutReDetail> list =  _tb_SaleOutReDetailServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        
        public virtual List<tb_SaleOutReDetail> Query(string wheresql)
        {
            List<tb_SaleOutReDetail> list =  _tb_SaleOutReDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_SaleOutReDetail>> QueryAsync(string wheresql) 
        {
            List<tb_SaleOutReDetail> list = await _tb_SaleOutReDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_SaleOutReDetail>> QueryAsync(Expression<Func<tb_SaleOutReDetail, bool>> exp)
        {
            List<tb_SaleOutReDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOutReDetail>> QueryByNavAsync()
        {
            List<tb_SaleOutReDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>()
                               .Includes(t => t.tb_saleoutre )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOutReDetail>> QueryByNavAsync(Expression<Func<tb_SaleOutReDetail, bool>> exp)
        {
            List<tb_SaleOutReDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>().Where(exp)
                               .Includes(t => t.tb_saleoutre )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_SaleOutReDetail> QueryByNav(Expression<Func<tb_SaleOutReDetail, bool>> exp)
        {
            List<tb_SaleOutReDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>().Where(exp)
                            .Includes(t => t.tb_saleoutre )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_SaleOutReDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_SaleOutReDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_SaleOutReDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_SaleOutReDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReDetail>().Where(w => w.SOutReturnDetail_ID == (long)id)
                             .Includes(t => t.tb_saleoutre )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_SaleOutReDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



