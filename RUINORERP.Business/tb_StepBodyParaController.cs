// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:23
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
    /// 步骤变量
    /// </summary>
    public partial class tb_StepBodyParaController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StepBodyParaServices _tb_StepBodyParaServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_StepBodyParaController(ILogger<tb_StepBodyParaController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StepBodyParaServices tb_StepBodyParaServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StepBodyParaServices = tb_StepBodyParaServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_StepBodyPara info)
        {

           // tb_StepBodyParaValidator validator = new tb_StepBodyParaValidator();
           tb_StepBodyParaValidator validator = _appContext.GetRequiredService<tb_StepBodyParaValidator>();
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
        public async Task<ReturnResults<tb_StepBodyPara>> SaveOrUpdate(tb_StepBodyPara entity)
        {
            ReturnResults<tb_StepBodyPara> rr = new ReturnResults<tb_StepBodyPara>();
            tb_StepBodyPara Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Para_Id > 0)
                {
                    bool rs = await _tb_StepBodyParaServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StepBodyParaServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(entity);
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
            tb_StepBodyPara entity = model as tb_StepBodyPara;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Para_Id > 0)
                {
                    bool rs = await _tb_StepBodyParaServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StepBodyParaServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(entity);
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
            List<T> list = await _tb_StepBodyParaServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StepBodyPara entity = item as tb_StepBodyPara;
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
            List<T> list = await _tb_StepBodyParaServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StepBodyPara entity = item as tb_StepBodyPara;
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
            tb_StepBodyPara entity = model as tb_StepBodyPara;
            bool rs = await _tb_StepBodyParaServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_StepBodyPara>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StepBodyPara> entitys = models as List<tb_StepBodyPara>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StepBodyPara>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_StepBodyPara>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StepBodyParaValidator validator = new tb_StepBodyParaValidator();
           tb_StepBodyParaValidator validator = _appContext.GetRequiredService<tb_StepBodyParaValidator>();
            ValidationResult results = validator.Validate(info as tb_StepBodyPara);
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

                tb_StepBodyPara entity = model as tb_StepBodyPara;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Para_Id > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StepBodyPara>(entity as tb_StepBodyPara)
                        .Include(m => m.tb_StepBodies)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StepBodyPara>(entity as tb_StepBodyPara)
                .Include(m => m.tb_StepBodies)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Para_Id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>()
                                .Includes(m => m.tb_StepBodies)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StepBodyPara entity = model as tb_StepBodyPara;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StepBodyPara>(m => m.Para_Id== entity.Para_Id)
                                .Include(m => m.tb_StepBodies)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_StepBodyPara AddReEntity(tb_StepBodyPara entity)
        {
            tb_StepBodyPara AddEntity =  _tb_StepBodyParaServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StepBodyPara> AddReEntityAsync(tb_StepBodyPara entity)
        {
            tb_StepBodyPara AddEntity = await _tb_StepBodyParaServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StepBodyPara entity)
        {
            long id = await _tb_StepBodyParaServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StepBodyPara> infos)
        {
            List<long> ids = await _tb_StepBodyParaServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StepBodyPara entity)
        {
            bool rs = await _tb_StepBodyParaServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_StepBodyPara>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StepBodyPara entity)
        {
            bool rs = await _tb_StepBodyParaServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_StepBodyPara>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StepBodyParaServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_StepBodyPara>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StepBodyParaServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_StepBodyPara>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StepBodyPara>> QueryAsync()
        {
            List<tb_StepBodyPara> list = await  _tb_StepBodyParaServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        
        public virtual List<tb_StepBodyPara> Query()
        {
            List<tb_StepBodyPara> list =  _tb_StepBodyParaServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        
        public virtual List<tb_StepBodyPara> Query(string wheresql)
        {
            List<tb_StepBodyPara> list =  _tb_StepBodyParaServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StepBodyPara>> QueryAsync(string wheresql) 
        {
            List<tb_StepBodyPara> list = await _tb_StepBodyParaServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StepBodyPara>> QueryAsync(Expression<Func<tb_StepBodyPara, bool>> exp)
        {
            List<tb_StepBodyPara> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StepBodyPara>> QueryByNavAsync()
        {
            List<tb_StepBodyPara> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>()
                                            .Includes(t => t.tb_StepBodies )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StepBodyPara>> QueryByNavAsync(Expression<Func<tb_StepBodyPara, bool>> exp)
        {
            List<tb_StepBodyPara> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>().Where(exp)
                                            .Includes(t => t.tb_StepBodies )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StepBodyPara> QueryByNav(Expression<Func<tb_StepBodyPara, bool>> exp)
        {
            List<tb_StepBodyPara> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>().Where(exp)
                                        .Includes(t => t.tb_StepBodies )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_StepBodyPara>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StepBodyPara>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StepBodyParaServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StepBodyPara entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StepBodyPara>().Where(w => w.Para_Id == (long)id)
                         

                                            .Includes(t => t.tb_StepBodies )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_StepBodyPara>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



