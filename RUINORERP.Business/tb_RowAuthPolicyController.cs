// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/29/2025 20:39:10
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
    /// 行级权限规则
    /// </summary>
    public partial class tb_RowAuthPolicyController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_RowAuthPolicyServices _tb_RowAuthPolicyServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_RowAuthPolicyController(ILogger<tb_RowAuthPolicyController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_RowAuthPolicyServices tb_RowAuthPolicyServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_RowAuthPolicyServices = tb_RowAuthPolicyServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_RowAuthPolicy info)
        {

           // tb_RowAuthPolicyValidator validator = new tb_RowAuthPolicyValidator();
           tb_RowAuthPolicyValidator validator = _appContext.GetRequiredService<tb_RowAuthPolicyValidator>();
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
        public async Task<ReturnResults<tb_RowAuthPolicy>> SaveOrUpdate(tb_RowAuthPolicy entity)
        {
            ReturnResults<tb_RowAuthPolicy> rr = new ReturnResults<tb_RowAuthPolicy>();
            tb_RowAuthPolicy Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PolicyId > 0)
                {
                    bool rs = await _tb_RowAuthPolicyServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_RowAuthPolicyServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
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
            tb_RowAuthPolicy entity = model as tb_RowAuthPolicy;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PolicyId > 0)
                {
                    bool rs = await _tb_RowAuthPolicyServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_RowAuthPolicyServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
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
            List<T> list = await _tb_RowAuthPolicyServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_RowAuthPolicy entity = item as tb_RowAuthPolicy;
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
            List<T> list = await _tb_RowAuthPolicyServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_RowAuthPolicy entity = item as tb_RowAuthPolicy;
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
            tb_RowAuthPolicy entity = model as tb_RowAuthPolicy;
            bool rs = await _tb_RowAuthPolicyServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_RowAuthPolicy>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_RowAuthPolicy> entitys = models as List<tb_RowAuthPolicy>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_RowAuthPolicy>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PolicyId).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_RowAuthPolicy>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_RowAuthPolicyValidator validator = new tb_RowAuthPolicyValidator();
           tb_RowAuthPolicyValidator validator = _appContext.GetRequiredService<tb_RowAuthPolicyValidator>();
            ValidationResult results = validator.Validate(info as tb_RowAuthPolicy);
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

                tb_RowAuthPolicy entity = model as tb_RowAuthPolicy;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PolicyId > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_RowAuthPolicy>(entity as tb_RowAuthPolicy)
                        .Include(m => m.tb_P4RowAuthPolicyByRoles)
                    .Include(m => m.tb_P4RowAuthPolicyByUsers)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_RowAuthPolicy>(entity as tb_RowAuthPolicy)
                .Include(m => m.tb_P4RowAuthPolicyByRoles)
                .Include(m => m.tb_P4RowAuthPolicyByUsers)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PolicyId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>()
                                .Includes(m => m.tb_P4RowAuthPolicyByRoles)
                        .Includes(m => m.tb_P4RowAuthPolicyByUsers)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_RowAuthPolicy entity = model as tb_RowAuthPolicy;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_RowAuthPolicy>(m => m.PolicyId== entity.PolicyId)
                                .Include(m => m.tb_P4RowAuthPolicyByRoles)
                        .Include(m => m.tb_P4RowAuthPolicyByUsers)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_RowAuthPolicy AddReEntity(tb_RowAuthPolicy entity)
        {
            tb_RowAuthPolicy AddEntity =  _tb_RowAuthPolicyServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_RowAuthPolicy> AddReEntityAsync(tb_RowAuthPolicy entity)
        {
            tb_RowAuthPolicy AddEntity = await _tb_RowAuthPolicyServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_RowAuthPolicy entity)
        {
            long id = await _tb_RowAuthPolicyServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_RowAuthPolicy> infos)
        {
            List<long> ids = await _tb_RowAuthPolicyServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_RowAuthPolicy entity)
        {
            bool rs = await _tb_RowAuthPolicyServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RowAuthPolicy>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_RowAuthPolicy entity)
        {
            bool rs = await _tb_RowAuthPolicyServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_RowAuthPolicyServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RowAuthPolicy>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_RowAuthPolicyServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RowAuthPolicy>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_RowAuthPolicy>> QueryAsync()
        {
            List<tb_RowAuthPolicy> list = await  _tb_RowAuthPolicyServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        
        public virtual List<tb_RowAuthPolicy> Query()
        {
            List<tb_RowAuthPolicy> list =  _tb_RowAuthPolicyServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        
        public virtual List<tb_RowAuthPolicy> Query(string wheresql)
        {
            List<tb_RowAuthPolicy> list =  _tb_RowAuthPolicyServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        
        public virtual async Task<List<tb_RowAuthPolicy>> QueryAsync(string wheresql) 
        {
            List<tb_RowAuthPolicy> list = await _tb_RowAuthPolicyServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_RowAuthPolicy>> QueryAsync(Expression<Func<tb_RowAuthPolicy, bool>> exp)
        {
            List<tb_RowAuthPolicy> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_RowAuthPolicy>> QueryByNavAsync()
        {
            List<tb_RowAuthPolicy> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>()
                                            .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                                .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_RowAuthPolicy>> QueryByNavAsync(Expression<Func<tb_RowAuthPolicy, bool>> exp)
        {
            List<tb_RowAuthPolicy> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>().Where(exp)
                                            .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                                .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_RowAuthPolicy> QueryByNav(Expression<Func<tb_RowAuthPolicy, bool>> exp)
        {
            List<tb_RowAuthPolicy> list = _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>().Where(exp)
                                        .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                            .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_RowAuthPolicy>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_RowAuthPolicyServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_RowAuthPolicy entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_RowAuthPolicy>().Where(w => w.PolicyId == (long)id)
                         

                                            .Includes(t => t.tb_P4RowAuthPolicyByRoles )
                                            .Includes(t => t.tb_P4RowAuthPolicyByUsers )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_RowAuthPolicy>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



