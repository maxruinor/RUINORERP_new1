
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/23/2025 14:00:46
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
    /// 用户角色个性化设置表
    /// </summary>
    public partial class tb_UserPersonalizedController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UserPersonalizedServices _tb_UserPersonalizedServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UserPersonalizedController(ILogger<tb_UserPersonalizedController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UserPersonalizedServices tb_UserPersonalizedServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UserPersonalizedServices = tb_UserPersonalizedServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_UserPersonalized info)
        {

           // tb_UserPersonalizedValidator validator = new tb_UserPersonalizedValidator();
           tb_UserPersonalizedValidator validator = _appContext.GetRequiredService<tb_UserPersonalizedValidator>();
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
        public async Task<ReturnResults<tb_UserPersonalized>> SaveOrUpdate(tb_UserPersonalized entity)
        {
            ReturnResults<tb_UserPersonalized> rr = new ReturnResults<tb_UserPersonalized>();
            tb_UserPersonalized Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UserPersonalizedID > 0)
                {
                    bool rs = await _tb_UserPersonalizedServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UserPersonalizedServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
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
            tb_UserPersonalized entity = model as tb_UserPersonalized;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UserPersonalizedID > 0)
                {
                    bool rs = await _tb_UserPersonalizedServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UserPersonalizedServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
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
            List<T> list = await _tb_UserPersonalizedServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UserPersonalized entity = item as tb_UserPersonalized;
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
            List<T> list = await _tb_UserPersonalizedServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UserPersonalized entity = item as tb_UserPersonalized;
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
            tb_UserPersonalized entity = model as tb_UserPersonalized;
            bool rs = await _tb_UserPersonalizedServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalized>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UserPersonalized> entitys = models as List<tb_UserPersonalized>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UserPersonalized>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.UserPersonalizedID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalized>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_UserPersonalizedValidator validator = new tb_UserPersonalizedValidator();
           tb_UserPersonalizedValidator validator = _appContext.GetRequiredService<tb_UserPersonalizedValidator>();
            ValidationResult results = validator.Validate(info as tb_UserPersonalized);
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

                tb_UserPersonalized entity = model as tb_UserPersonalized;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UserPersonalizedID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_UserPersonalized>(entity as tb_UserPersonalized)
                        .Include(m => m.tb_UIMenuPersonalizations)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_UserPersonalized>(entity as tb_UserPersonalized)
                .Include(m => m.tb_UIMenuPersonalizations)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UserPersonalizedID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>()
                                .Includes(m => m.tb_UIMenuPersonalizations)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UserPersonalized entity = model as tb_UserPersonalized;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UserPersonalized>(m => m.UserPersonalizedID== entity.UserPersonalizedID)
                                .Include(m => m.tb_UIMenuPersonalizations)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_UserPersonalized AddReEntity(tb_UserPersonalized entity)
        {
            tb_UserPersonalized AddEntity =  _tb_UserPersonalizedServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UserPersonalized> AddReEntityAsync(tb_UserPersonalized entity)
        {
            tb_UserPersonalized AddEntity = await _tb_UserPersonalizedServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UserPersonalized entity)
        {
            long id = await _tb_UserPersonalizedServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UserPersonalized> infos)
        {
            List<long> ids = await _tb_UserPersonalizedServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UserPersonalized entity)
        {
            bool rs = await _tb_UserPersonalizedServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalized>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UserPersonalized entity)
        {
            bool rs = await _tb_UserPersonalizedServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UserPersonalizedServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalized>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UserPersonalizedServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UserPersonalized>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UserPersonalized>> QueryAsync()
        {
            List<tb_UserPersonalized> list = await  _tb_UserPersonalizedServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        
        public virtual List<tb_UserPersonalized> Query()
        {
            List<tb_UserPersonalized> list =  _tb_UserPersonalizedServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        
        public virtual List<tb_UserPersonalized> Query(string wheresql)
        {
            List<tb_UserPersonalized> list =  _tb_UserPersonalizedServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UserPersonalized>> QueryAsync(string wheresql) 
        {
            List<tb_UserPersonalized> list = await _tb_UserPersonalizedServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UserPersonalized>> QueryAsync(Expression<Func<tb_UserPersonalized, bool>> exp)
        {
            List<tb_UserPersonalized> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UserPersonalized>> QueryByNavAsync()
        {
            List<tb_UserPersonalized> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>()
                               .Includes(t => t.tb_user_role )
                                            .Includes(t => t.tb_UIMenuPersonalizations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UserPersonalized>> QueryByNavAsync(Expression<Func<tb_UserPersonalized, bool>> exp)
        {
            List<tb_UserPersonalized> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>().Where(exp)
                               .Includes(t => t.tb_user_role )
                                            .Includes(t => t.tb_UIMenuPersonalizations )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UserPersonalized> QueryByNav(Expression<Func<tb_UserPersonalized, bool>> exp)
        {
            List<tb_UserPersonalized> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>().Where(exp)
                            .Includes(t => t.tb_user_role )
                                        .Includes(t => t.tb_UIMenuPersonalizations )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UserPersonalized>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UserPersonalizedServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UserPersonalized entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UserPersonalized>().Where(w => w.UserPersonalizedID == (long)id)
                             .Includes(t => t.tb_user_role )
                                        .Includes(t => t.tb_UIMenuPersonalizations )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UserPersonalized>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



