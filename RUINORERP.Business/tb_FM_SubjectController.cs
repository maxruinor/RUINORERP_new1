
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:43
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
    /// 会计科目表，财务系统中使用
    /// </summary>
    public partial class tb_FM_SubjectController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_SubjectServices _tb_FM_SubjectServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_SubjectController(ILogger<tb_FM_SubjectController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_SubjectServices tb_FM_SubjectServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_SubjectServices = tb_FM_SubjectServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_Subject info)
        {

           // tb_FM_SubjectValidator validator = new tb_FM_SubjectValidator();
           tb_FM_SubjectValidator validator = _appContext.GetRequiredService<tb_FM_SubjectValidator>();
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
        public async Task<ReturnResults<tb_FM_Subject>> SaveOrUpdate(tb_FM_Subject entity)
        {
            ReturnResults<tb_FM_Subject> rr = new ReturnResults<tb_FM_Subject>();
            tb_FM_Subject Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Subject_id > 0)
                {
                    bool rs = await _tb_FM_SubjectServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_SubjectServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
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
            tb_FM_Subject entity = model as tb_FM_Subject;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Subject_id > 0)
                {
                    bool rs = await _tb_FM_SubjectServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_SubjectServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
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
            List<T> list = await _tb_FM_SubjectServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_Subject entity = item as tb_FM_Subject;
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
            List<T> list = await _tb_FM_SubjectServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_Subject entity = item as tb_FM_Subject;
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
            tb_FM_Subject entity = model as tb_FM_Subject;
            bool rs = await _tb_FM_SubjectServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Subject>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_Subject> entitys = models as List<tb_FM_Subject>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_Subject>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Subject_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Subject>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_SubjectValidator validator = new tb_FM_SubjectValidator();
           tb_FM_SubjectValidator validator = _appContext.GetRequiredService<tb_FM_SubjectValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_Subject);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().Where(useLike,dto);
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

                tb_FM_Subject entity = model as tb_FM_Subject;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Subject_id > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_Subject>(entity as tb_FM_Subject)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_FM_ExpenseClaimDetails)
                    .Include(m => m.tb_FM_ExpenseTypes)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_Subject>(entity as tb_FM_Subject)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_FM_ExpenseClaimDetails)
                .Include(m => m.tb_FM_ExpenseTypes)
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Subject_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>()
                                .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_FM_ExpenseClaimDetails)
            
                        .Includes(m => m.tb_FM_ExpenseTypes)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_Subject entity = model as tb_FM_Subject;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_Subject>(m => m.Subject_id == entity.Subject_id)
                                .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_FM_ExpenseClaimDetails)
               
                        .Include(m => m.tb_FM_ExpenseTypes)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_Subject AddReEntity(tb_FM_Subject entity)
        {
            tb_FM_Subject AddEntity =  _tb_FM_SubjectServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_Subject> AddReEntityAsync(tb_FM_Subject entity)
        {
            tb_FM_Subject AddEntity = await _tb_FM_SubjectServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_Subject entity)
        {
            long id = await _tb_FM_SubjectServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_Subject> infos)
        {
            List<long> ids = await _tb_FM_SubjectServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_Subject entity)
        {
            bool rs = await _tb_FM_SubjectServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Subject>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_Subject entity)
        {
            bool rs = await _tb_FM_SubjectServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_SubjectServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Subject>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_SubjectServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Subject>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_Subject>> QueryAsync()
        {
            List<tb_FM_Subject> list = await  _tb_FM_SubjectServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        
        public virtual List<tb_FM_Subject> Query()
        {
            List<tb_FM_Subject> list =  _tb_FM_SubjectServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        
        public virtual List<tb_FM_Subject> Query(string wheresql)
        {
            List<tb_FM_Subject> list =  _tb_FM_SubjectServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_Subject>> QueryAsync(string wheresql) 
        {
            List<tb_FM_Subject> list = await _tb_FM_SubjectServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_Subject>> QueryAsync(Expression<Func<tb_FM_Subject, bool>> exp)
        {
            List<tb_FM_Subject> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Subject>> QueryByNavAsync()
        {
            List<tb_FM_Subject> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>()
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                    
                                .Includes(t => t.tb_FM_ExpenseTypes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Subject>> QueryByNavAsync(Expression<Func<tb_FM_Subject, bool>> exp)
        {
            List<tb_FM_Subject> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>().Where(exp)
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                      
                                .Includes(t => t.tb_FM_ExpenseTypes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_Subject> QueryByNav(Expression<Func<tb_FM_Subject, bool>> exp)
        {
            List<tb_FM_Subject> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>().Where(exp)
                                        .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
               
                            .Includes(t => t.tb_FM_ExpenseTypes )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_Subject>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_SubjectServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_Subject entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Subject>().Where(w => w.Subject_id == (long)id)
                                         .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                     
                            .Includes(t => t.tb_FM_ExpenseTypes )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_Subject>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



