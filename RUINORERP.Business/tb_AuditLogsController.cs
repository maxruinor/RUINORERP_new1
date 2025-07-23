
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/06/2025 14:52:02
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

namespace RUINORERP.Business
{
    /// <summary>
    /// 审计日志表
    /// </summary>
    public partial class tb_AuditLogsController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_AuditLogsServices _tb_AuditLogsServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_AuditLogsController(ILogger<tb_AuditLogsController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_AuditLogsServices tb_AuditLogsServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_AuditLogsServices = tb_AuditLogsServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_AuditLogs info)
        {

           // tb_AuditLogsValidator validator = new tb_AuditLogsValidator();
           tb_AuditLogsValidator validator = _appContext.GetRequiredService<tb_AuditLogsValidator>();
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
        public async Task<ReturnResults<tb_AuditLogs>> SaveOrUpdate(tb_AuditLogs entity)
        {
            ReturnResults<tb_AuditLogs> rr = new ReturnResults<tb_AuditLogs>();
            tb_AuditLogs Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Audit_ID > 0)
                {
                    bool rs = await _tb_AuditLogsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_AuditLogsServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
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
            tb_AuditLogs entity = model as tb_AuditLogs;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Audit_ID > 0)
                {
                    bool rs = await _tb_AuditLogsServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_AuditLogsServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
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
            List<T> list = await _tb_AuditLogsServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_AuditLogs entity = item as tb_AuditLogs;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_AuditLogsServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_AuditLogs entity = item as tb_AuditLogs;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                MyCacheManager.Instance.UpdateEntityList<List<T>>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_AuditLogs entity = model as tb_AuditLogs;
            bool rs = await _tb_AuditLogsServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_AuditLogs>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_AuditLogs> entitys = models as List<tb_AuditLogs>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_AuditLogs>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Audit_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_AuditLogs>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_AuditLogsValidator validator = new tb_AuditLogsValidator();
           tb_AuditLogsValidator validator = _appContext.GetRequiredService<tb_AuditLogsValidator>();
            ValidationResult results = validator.Validate(info as tb_AuditLogs);
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

                tb_AuditLogs entity = model as tb_AuditLogs;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Audit_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_AuditLogs>(entity as tb_AuditLogs)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_AuditLogs>(entity as tb_AuditLogs)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Audit_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_AuditLogs entity = model as tb_AuditLogs;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_AuditLogs>(m => m.Audit_ID== entity.Audit_ID)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_AuditLogs AddReEntity(tb_AuditLogs entity)
        {
            tb_AuditLogs AddEntity =  _tb_AuditLogsServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_AuditLogs> AddReEntityAsync(tb_AuditLogs entity)
        {
            tb_AuditLogs AddEntity = await _tb_AuditLogsServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_AuditLogs entity)
        {
            long id = await _tb_AuditLogsServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_AuditLogs> infos)
        {
            List<long> ids = await _tb_AuditLogsServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_AuditLogs entity)
        {
            bool rs = await _tb_AuditLogsServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AuditLogs>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_AuditLogs entity)
        {
            bool rs = await _tb_AuditLogsServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_AuditLogsServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AuditLogs>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_AuditLogsServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AuditLogs>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_AuditLogs>> QueryAsync()
        {
            List<tb_AuditLogs> list = await  _tb_AuditLogsServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        
        public virtual List<tb_AuditLogs> Query()
        {
            List<tb_AuditLogs> list =  _tb_AuditLogsServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        
        public virtual List<tb_AuditLogs> Query(string wheresql)
        {
            List<tb_AuditLogs> list =  _tb_AuditLogsServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        
        public virtual async Task<List<tb_AuditLogs>> QueryAsync(string wheresql) 
        {
            List<tb_AuditLogs> list = await _tb_AuditLogsServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_AuditLogs>> QueryAsync(Expression<Func<tb_AuditLogs, bool>> exp)
        {
            List<tb_AuditLogs> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AuditLogs>> QueryByNavAsync()
        {
            List<tb_AuditLogs> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AuditLogs>> QueryByNavAsync(Expression<Func<tb_AuditLogs, bool>> exp)
        {
            List<tb_AuditLogs> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_AuditLogs> QueryByNav(Expression<Func<tb_AuditLogs, bool>> exp)
        {
            List<tb_AuditLogs> list = _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_AuditLogs>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_AuditLogsServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_AuditLogs entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AuditLogs>().Where(w => w.Audit_ID == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_AuditLogs>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



