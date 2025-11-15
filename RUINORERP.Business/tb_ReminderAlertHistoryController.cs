
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理
    /// </summary>
    public partial class tb_ReminderAlertHistoryController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReminderAlertHistoryServices _tb_ReminderAlertHistoryServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ReminderAlertHistoryController(ILogger<tb_ReminderAlertHistoryController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReminderAlertHistoryServices tb_ReminderAlertHistoryServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReminderAlertHistoryServices = tb_ReminderAlertHistoryServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ReminderAlertHistory info)
        {

           // tb_ReminderAlertHistoryValidator validator = new tb_ReminderAlertHistoryValidator();
           tb_ReminderAlertHistoryValidator validator = _appContext.GetRequiredService<tb_ReminderAlertHistoryValidator>();
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
        public async Task<ReturnResults<tb_ReminderAlertHistory>> SaveOrUpdate(tb_ReminderAlertHistory entity)
        {
            ReturnResults<tb_ReminderAlertHistory> rr = new ReturnResults<tb_ReminderAlertHistory>();
            tb_ReminderAlertHistory Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.HistoryId > 0)
                {
                    bool rs = await _tb_ReminderAlertHistoryServices.Update(entity);
                    if (rs)
                    {
                        Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReminderAlertHistoryServices.AddReEntityAsync(entity);
                    Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
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
            tb_ReminderAlertHistory entity = model as tb_ReminderAlertHistory;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.HistoryId > 0)
                {
                    bool rs = await _tb_ReminderAlertHistoryServices.Update(entity);
                    if (rs)
                    {
                        Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReminderAlertHistoryServices.AddReEntityAsync(entity) as T ;
                    Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
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
            List<T> list = await _tb_ReminderAlertHistoryServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ReminderAlertHistory entity = item as tb_ReminderAlertHistory;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                Cache.EntityCacheHelper.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_ReminderAlertHistoryServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ReminderAlertHistory entity = item as tb_ReminderAlertHistory;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                Cache.EntityCacheHelper.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_ReminderAlertHistory entity = model as tb_ReminderAlertHistory;
            bool rs = await _tb_ReminderAlertHistoryServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlertHistory>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ReminderAlertHistory> entitys = models as List<tb_ReminderAlertHistory>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ReminderAlertHistory>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.HistoryId).ToArray();
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlertHistory>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReminderAlertHistoryValidator validator = new tb_ReminderAlertHistoryValidator();
           tb_ReminderAlertHistoryValidator validator = _appContext.GetRequiredService<tb_ReminderAlertHistoryValidator>();
            ValidationResult results = validator.Validate(info as tb_ReminderAlertHistory);
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

                tb_ReminderAlertHistory entity = model as tb_ReminderAlertHistory;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.HistoryId > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ReminderAlertHistory>(entity as tb_ReminderAlertHistory)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ReminderAlertHistory>(entity as tb_ReminderAlertHistory)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.HistoryId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ReminderAlertHistory entity = model as tb_ReminderAlertHistory;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ReminderAlertHistory>(m => m.HistoryId== entity.HistoryId)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                Cache.EntityCacheHelper.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ReminderAlertHistory AddReEntity(tb_ReminderAlertHistory entity)
        {
            tb_ReminderAlertHistory AddEntity =  _tb_ReminderAlertHistoryServices.AddReEntity(entity);
            Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ReminderAlertHistory> AddReEntityAsync(tb_ReminderAlertHistory entity)
        {
            tb_ReminderAlertHistory AddEntity = await _tb_ReminderAlertHistoryServices.AddReEntityAsync(entity);
            Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ReminderAlertHistory entity)
        {
            long id = await _tb_ReminderAlertHistoryServices.Add(entity);
            if(id>0)
            {
                 Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ReminderAlertHistory> infos)
        {
            List<long> ids = await _tb_ReminderAlertHistoryServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ReminderAlertHistory entity)
        {
            bool rs = await _tb_ReminderAlertHistoryServices.Delete(entity);
            if (rs)
            {
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlertHistory>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ReminderAlertHistory entity)
        {
            bool rs = await _tb_ReminderAlertHistoryServices.Update(entity);
            if (rs)
            {
                 Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReminderAlertHistoryServices.DeleteById(id);
            if (rs)
            {
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlertHistory>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReminderAlertHistoryServices.DeleteByIds(ids);
            if (rs)
            {
                Cache.EntityCacheHelper.DeleteEntity<tb_ReminderAlertHistory>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ReminderAlertHistory>> QueryAsync()
        {
            List<tb_ReminderAlertHistory> list = await  _tb_ReminderAlertHistoryServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        
        public virtual List<tb_ReminderAlertHistory> Query()
        {
            List<tb_ReminderAlertHistory> list =  _tb_ReminderAlertHistoryServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        
        public virtual List<tb_ReminderAlertHistory> Query(string wheresql)
        {
            List<tb_ReminderAlertHistory> list =  _tb_ReminderAlertHistoryServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ReminderAlertHistory>> QueryAsync(string wheresql) 
        {
            List<tb_ReminderAlertHistory> list = await _tb_ReminderAlertHistoryServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ReminderAlertHistory>> QueryAsync(Expression<Func<tb_ReminderAlertHistory, bool>> exp)
        {
            List<tb_ReminderAlertHistory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderAlertHistory>> QueryByNavAsync()
        {
            List<tb_ReminderAlertHistory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>()
                               .Includes(t => t.tb_reminderalert )
                               .Includes(t => t.tb_userinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderAlertHistory>> QueryByNavAsync(Expression<Func<tb_ReminderAlertHistory, bool>> exp)
        {
            List<tb_ReminderAlertHistory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>().Where(exp)
                               .Includes(t => t.tb_reminderalert )
                               .Includes(t => t.tb_userinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ReminderAlertHistory> QueryByNav(Expression<Func<tb_ReminderAlertHistory, bool>> exp)
        {
            List<tb_ReminderAlertHistory> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>().Where(exp)
                            .Includes(t => t.tb_reminderalert )
                            .Includes(t => t.tb_userinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            Cache.EntityCacheHelper.UpdateEntityList<tb_ReminderAlertHistory>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ReminderAlertHistory>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReminderAlertHistoryServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ReminderAlertHistory entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderAlertHistory>().Where(w => w.HistoryId == (long)id)
                             .Includes(t => t.tb_reminderalert )
                            .Includes(t => t.tb_userinfo )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            Cache.EntityCacheHelper.UpdateEntity<tb_ReminderAlertHistory>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



