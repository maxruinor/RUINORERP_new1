// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/28/2025 15:02:31
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
    /// 行级权限规则-用户关联表
    /// </summary>
    public partial class tb_P4RowAuthPolicyByUserController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_P4RowAuthPolicyByUserServices _tb_P4RowAuthPolicyByUserServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_P4RowAuthPolicyByUserController(ILogger<tb_P4RowAuthPolicyByUserController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_P4RowAuthPolicyByUserServices tb_P4RowAuthPolicyByUserServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_P4RowAuthPolicyByUserServices = tb_P4RowAuthPolicyByUserServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_P4RowAuthPolicyByUser info)
        {

           // tb_P4RowAuthPolicyByUserValidator validator = new tb_P4RowAuthPolicyByUserValidator();
           tb_P4RowAuthPolicyByUserValidator validator = _appContext.GetRequiredService<tb_P4RowAuthPolicyByUserValidator>();
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
        public async Task<ReturnResults<tb_P4RowAuthPolicyByUser>> SaveOrUpdate(tb_P4RowAuthPolicyByUser entity)
        {
            ReturnResults<tb_P4RowAuthPolicyByUser> rr = new ReturnResults<tb_P4RowAuthPolicyByUser>();
            tb_P4RowAuthPolicyByUser Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Policy_User_RID > 0)
                {
                    bool rs = await _tb_P4RowAuthPolicyByUserServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_P4RowAuthPolicyByUserServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
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
            tb_P4RowAuthPolicyByUser entity = model as tb_P4RowAuthPolicyByUser;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Policy_User_RID > 0)
                {
                    bool rs = await _tb_P4RowAuthPolicyByUserServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_P4RowAuthPolicyByUserServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
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
            List<T> list = await _tb_P4RowAuthPolicyByUserServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_P4RowAuthPolicyByUser entity = item as tb_P4RowAuthPolicyByUser;
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
            List<T> list = await _tb_P4RowAuthPolicyByUserServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_P4RowAuthPolicyByUser entity = item as tb_P4RowAuthPolicyByUser;
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
            tb_P4RowAuthPolicyByUser entity = model as tb_P4RowAuthPolicyByUser;
            bool rs = await _tb_P4RowAuthPolicyByUserServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_P4RowAuthPolicyByUser>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_P4RowAuthPolicyByUser> entitys = models as List<tb_P4RowAuthPolicyByUser>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_P4RowAuthPolicyByUser>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Policy_User_RID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_P4RowAuthPolicyByUser>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_P4RowAuthPolicyByUserValidator validator = new tb_P4RowAuthPolicyByUserValidator();
           tb_P4RowAuthPolicyByUserValidator validator = _appContext.GetRequiredService<tb_P4RowAuthPolicyByUserValidator>();
            ValidationResult results = validator.Validate(info as tb_P4RowAuthPolicyByUser);
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

                tb_P4RowAuthPolicyByUser entity = model as tb_P4RowAuthPolicyByUser;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Policy_User_RID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_P4RowAuthPolicyByUser>(entity as tb_P4RowAuthPolicyByUser)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_P4RowAuthPolicyByUser>(entity as tb_P4RowAuthPolicyByUser)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Policy_User_RID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_P4RowAuthPolicyByUser entity = model as tb_P4RowAuthPolicyByUser;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_P4RowAuthPolicyByUser>(m => m.Policy_User_RID== entity.Policy_User_RID)
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
        
        
        
        public tb_P4RowAuthPolicyByUser AddReEntity(tb_P4RowAuthPolicyByUser entity)
        {
            tb_P4RowAuthPolicyByUser AddEntity =  _tb_P4RowAuthPolicyByUserServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_P4RowAuthPolicyByUser> AddReEntityAsync(tb_P4RowAuthPolicyByUser entity)
        {
            tb_P4RowAuthPolicyByUser AddEntity = await _tb_P4RowAuthPolicyByUserServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_P4RowAuthPolicyByUser entity)
        {
            long id = await _tb_P4RowAuthPolicyByUserServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_P4RowAuthPolicyByUser> infos)
        {
            List<long> ids = await _tb_P4RowAuthPolicyByUserServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_P4RowAuthPolicyByUser entity)
        {
            bool rs = await _tb_P4RowAuthPolicyByUserServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_P4RowAuthPolicyByUser>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_P4RowAuthPolicyByUser entity)
        {
            bool rs = await _tb_P4RowAuthPolicyByUserServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_P4RowAuthPolicyByUserServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_P4RowAuthPolicyByUser>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_P4RowAuthPolicyByUserServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_P4RowAuthPolicyByUser>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_P4RowAuthPolicyByUser>> QueryAsync()
        {
            List<tb_P4RowAuthPolicyByUser> list = await  _tb_P4RowAuthPolicyByUserServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        
        public virtual List<tb_P4RowAuthPolicyByUser> Query()
        {
            List<tb_P4RowAuthPolicyByUser> list =  _tb_P4RowAuthPolicyByUserServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        
        public virtual List<tb_P4RowAuthPolicyByUser> Query(string wheresql)
        {
            List<tb_P4RowAuthPolicyByUser> list =  _tb_P4RowAuthPolicyByUserServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        
        public virtual async Task<List<tb_P4RowAuthPolicyByUser>> QueryAsync(string wheresql) 
        {
            List<tb_P4RowAuthPolicyByUser> list = await _tb_P4RowAuthPolicyByUserServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_P4RowAuthPolicyByUser>> QueryAsync(Expression<Func<tb_P4RowAuthPolicyByUser, bool>> exp)
        {
            List<tb_P4RowAuthPolicyByUser> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4RowAuthPolicyByUser>> QueryByNavAsync()
        {
            List<tb_P4RowAuthPolicyByUser> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>()
                               .Includes(t => t.tb_rowauthpolicy )
                               .Includes(t => t.tb_userinfo )
                               .Includes(t => t.tb_menuinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_P4RowAuthPolicyByUser>> QueryByNavAsync(Expression<Func<tb_P4RowAuthPolicyByUser, bool>> exp)
        {
            List<tb_P4RowAuthPolicyByUser> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>().Where(exp)
                               .Includes(t => t.tb_rowauthpolicy )
                               .Includes(t => t.tb_userinfo )
                               .Includes(t => t.tb_menuinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_P4RowAuthPolicyByUser> QueryByNav(Expression<Func<tb_P4RowAuthPolicyByUser, bool>> exp)
        {
            List<tb_P4RowAuthPolicyByUser> list = _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>().Where(exp)
                            .Includes(t => t.tb_rowauthpolicy )
                            .Includes(t => t.tb_userinfo )
                            .Includes(t => t.tb_menuinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_P4RowAuthPolicyByUser>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_P4RowAuthPolicyByUserServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_P4RowAuthPolicyByUser entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_P4RowAuthPolicyByUser>().Where(w => w.Policy_User_RID == (long)id)
                             .Includes(t => t.tb_rowauthpolicy )
                            .Includes(t => t.tb_userinfo )
                            .Includes(t => t.tb_menuinfo )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_P4RowAuthPolicyByUser>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



