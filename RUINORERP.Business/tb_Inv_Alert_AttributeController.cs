// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:12
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
    /// 存货预警特性表
    /// </summary>
    public partial class tb_Inv_Alert_AttributeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Inv_Alert_AttributeServices _tb_Inv_Alert_AttributeServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_Inv_Alert_AttributeController(ILogger<tb_Inv_Alert_AttributeController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Inv_Alert_AttributeServices tb_Inv_Alert_AttributeServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Inv_Alert_AttributeServices = tb_Inv_Alert_AttributeServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_Inv_Alert_Attribute info)
        {

           // tb_Inv_Alert_AttributeValidator validator = new tb_Inv_Alert_AttributeValidator();
           tb_Inv_Alert_AttributeValidator validator = _appContext.GetRequiredService<tb_Inv_Alert_AttributeValidator>();
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
        public async Task<ReturnResults<tb_Inv_Alert_Attribute>> SaveOrUpdate(tb_Inv_Alert_Attribute entity)
        {
            ReturnResults<tb_Inv_Alert_Attribute> rr = new ReturnResults<tb_Inv_Alert_Attribute>();
            tb_Inv_Alert_Attribute Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Inv_Alert_ID > 0)
                {
                    bool rs = await _tb_Inv_Alert_AttributeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Inv_Alert_AttributeServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(entity);
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
            tb_Inv_Alert_Attribute entity = model as tb_Inv_Alert_Attribute;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Inv_Alert_ID > 0)
                {
                    bool rs = await _tb_Inv_Alert_AttributeServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Inv_Alert_AttributeServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(entity);
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
            List<T> list = await _tb_Inv_Alert_AttributeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Inv_Alert_Attribute entity = item as tb_Inv_Alert_Attribute;
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
            List<T> list = await _tb_Inv_Alert_AttributeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Inv_Alert_Attribute entity = item as tb_Inv_Alert_Attribute;
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
            tb_Inv_Alert_Attribute entity = model as tb_Inv_Alert_Attribute;
            bool rs = await _tb_Inv_Alert_AttributeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_Inv_Alert_Attribute>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Inv_Alert_Attribute> entitys = models as List<tb_Inv_Alert_Attribute>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Inv_Alert_Attribute>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_Inv_Alert_Attribute>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_Inv_Alert_AttributeValidator validator = new tb_Inv_Alert_AttributeValidator();
           tb_Inv_Alert_AttributeValidator validator = _appContext.GetRequiredService<tb_Inv_Alert_AttributeValidator>();
            ValidationResult results = validator.Validate(info as tb_Inv_Alert_Attribute);
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

                tb_Inv_Alert_Attribute entity = model as tb_Inv_Alert_Attribute;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Inv_Alert_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_Inv_Alert_Attribute>(entity as tb_Inv_Alert_Attribute)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_Inv_Alert_Attribute>(entity as tb_Inv_Alert_Attribute)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Inv_Alert_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Inv_Alert_Attribute entity = model as tb_Inv_Alert_Attribute;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Inv_Alert_Attribute>(m => m.Inv_Alert_ID== entity.Inv_Alert_ID)
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
        
        
        
        public tb_Inv_Alert_Attribute AddReEntity(tb_Inv_Alert_Attribute entity)
        {
            tb_Inv_Alert_Attribute AddEntity =  _tb_Inv_Alert_AttributeServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Inv_Alert_Attribute> AddReEntityAsync(tb_Inv_Alert_Attribute entity)
        {
            tb_Inv_Alert_Attribute AddEntity = await _tb_Inv_Alert_AttributeServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Inv_Alert_Attribute entity)
        {
            long id = await _tb_Inv_Alert_AttributeServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Inv_Alert_Attribute> infos)
        {
            List<long> ids = await _tb_Inv_Alert_AttributeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Inv_Alert_Attribute entity)
        {
            bool rs = await _tb_Inv_Alert_AttributeServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_Inv_Alert_Attribute>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Inv_Alert_Attribute entity)
        {
            bool rs = await _tb_Inv_Alert_AttributeServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_Inv_Alert_Attribute>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Inv_Alert_AttributeServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_Inv_Alert_Attribute>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Inv_Alert_AttributeServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_Inv_Alert_Attribute>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Inv_Alert_Attribute>> QueryAsync()
        {
            List<tb_Inv_Alert_Attribute> list = await  _tb_Inv_Alert_AttributeServices.QueryAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        
        public virtual List<tb_Inv_Alert_Attribute> Query()
        {
            List<tb_Inv_Alert_Attribute> list =  _tb_Inv_Alert_AttributeServices.Query();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        
        public virtual List<tb_Inv_Alert_Attribute> Query(string wheresql)
        {
            List<tb_Inv_Alert_Attribute> list =  _tb_Inv_Alert_AttributeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Inv_Alert_Attribute>> QueryAsync(string wheresql) 
        {
            List<tb_Inv_Alert_Attribute> list = await _tb_Inv_Alert_AttributeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Inv_Alert_Attribute>> QueryAsync(Expression<Func<tb_Inv_Alert_Attribute, bool>> exp)
        {
            List<tb_Inv_Alert_Attribute> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Inv_Alert_Attribute>> QueryByNavAsync()
        {
            List<tb_Inv_Alert_Attribute> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>()
                               .Includes(t => t.tb_inventory )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Inv_Alert_Attribute>> QueryByNavAsync(Expression<Func<tb_Inv_Alert_Attribute, bool>> exp)
        {
            List<tb_Inv_Alert_Attribute> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>().Where(exp)
                               .Includes(t => t.tb_inventory )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Inv_Alert_Attribute> QueryByNav(Expression<Func<tb_Inv_Alert_Attribute, bool>> exp)
        {
            List<tb_Inv_Alert_Attribute> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>().Where(exp)
                            .Includes(t => t.tb_inventory )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.AcceptChanges();
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_Inv_Alert_Attribute>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Inv_Alert_Attribute>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Inv_Alert_AttributeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Inv_Alert_Attribute entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inv_Alert_Attribute>().Where(w => w.Inv_Alert_ID == (long)id)
                             .Includes(t => t.tb_inventory )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.AcceptChanges();
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_Inv_Alert_Attribute>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



