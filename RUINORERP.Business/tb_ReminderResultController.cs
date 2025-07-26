
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/26/2025 12:18:31
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
    /// 用户接收提醒内容
    /// </summary>
    public partial class tb_ReminderResultController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReminderResultServices _tb_ReminderResultServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ReminderResultController(ILogger<tb_ReminderResultController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReminderResultServices tb_ReminderResultServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReminderResultServices = tb_ReminderResultServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ReminderResult info)
        {

           // tb_ReminderResultValidator validator = new tb_ReminderResultValidator();
           tb_ReminderResultValidator validator = _appContext.GetRequiredService<tb_ReminderResultValidator>();
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
        public async Task<ReturnResults<tb_ReminderResult>> SaveOrUpdate(tb_ReminderResult entity)
        {
            ReturnResults<tb_ReminderResult> rr = new ReturnResults<tb_ReminderResult>();
            tb_ReminderResult Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ResultId > 0)
                {
                    bool rs = await _tb_ReminderResultServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReminderResultServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
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
            tb_ReminderResult entity = model as tb_ReminderResult;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ResultId > 0)
                {
                    bool rs = await _tb_ReminderResultServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReminderResultServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
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
            List<T> list = await _tb_ReminderResultServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ReminderResult entity = item as tb_ReminderResult;
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
            List<T> list = await _tb_ReminderResultServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ReminderResult entity = item as tb_ReminderResult;
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
            tb_ReminderResult entity = model as tb_ReminderResult;
            bool rs = await _tb_ReminderResultServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ReminderResult>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ReminderResult> entitys = models as List<tb_ReminderResult>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ReminderResult>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ResultId).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ReminderResult>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReminderResultValidator validator = new tb_ReminderResultValidator();
           tb_ReminderResultValidator validator = _appContext.GetRequiredService<tb_ReminderResultValidator>();
            ValidationResult results = validator.Validate(info as tb_ReminderResult);
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

                tb_ReminderResult entity = model as tb_ReminderResult;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ResultId > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ReminderResult>(entity as tb_ReminderResult)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ReminderResult>(entity as tb_ReminderResult)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ResultId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ReminderResult entity = model as tb_ReminderResult;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ReminderResult>(m => m.ResultId== entity.ResultId)
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
        
        
        
        public tb_ReminderResult AddReEntity(tb_ReminderResult entity)
        {
            tb_ReminderResult AddEntity =  _tb_ReminderResultServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ReminderResult> AddReEntityAsync(tb_ReminderResult entity)
        {
            tb_ReminderResult AddEntity = await _tb_ReminderResultServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ReminderResult entity)
        {
            long id = await _tb_ReminderResultServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ReminderResult> infos)
        {
            List<long> ids = await _tb_ReminderResultServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ReminderResult entity)
        {
            bool rs = await _tb_ReminderResultServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ReminderResult>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ReminderResult entity)
        {
            bool rs = await _tb_ReminderResultServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReminderResultServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ReminderResult>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReminderResultServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ReminderResult>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ReminderResult>> QueryAsync()
        {
            List<tb_ReminderResult> list = await  _tb_ReminderResultServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        
        public virtual List<tb_ReminderResult> Query()
        {
            List<tb_ReminderResult> list =  _tb_ReminderResultServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        
        public virtual List<tb_ReminderResult> Query(string wheresql)
        {
            List<tb_ReminderResult> list =  _tb_ReminderResultServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ReminderResult>> QueryAsync(string wheresql) 
        {
            List<tb_ReminderResult> list = await _tb_ReminderResultServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ReminderResult>> QueryAsync(Expression<Func<tb_ReminderResult, bool>> exp)
        {
            List<tb_ReminderResult> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderResult>> QueryByNavAsync()
        {
            List<tb_ReminderResult> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>()
                               .Includes(t => t.tb_reminderrule )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ReminderResult>> QueryByNavAsync(Expression<Func<tb_ReminderResult, bool>> exp)
        {
            List<tb_ReminderResult> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>().Where(exp)
                               .Includes(t => t.tb_reminderrule )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ReminderResult> QueryByNav(Expression<Func<tb_ReminderResult, bool>> exp)
        {
            List<tb_ReminderResult> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>().Where(exp)
                            .Includes(t => t.tb_reminderrule )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ReminderResult>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReminderResultServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ReminderResult entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ReminderResult>().Where(w => w.ResultId == (long)id)
                             .Includes(t => t.tb_reminderrule )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ReminderResult>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



