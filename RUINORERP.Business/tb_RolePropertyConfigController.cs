
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:26
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
    /// 角色属性配置不同角色权限功能等不一样
    /// </summary>
    public partial class tb_RolePropertyConfigController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_RolePropertyConfigServices _tb_RolePropertyConfigServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_RolePropertyConfigController(ILogger<tb_RolePropertyConfigController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_RolePropertyConfigServices tb_RolePropertyConfigServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_RolePropertyConfigServices = tb_RolePropertyConfigServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_RolePropertyConfig info)
        {
            tb_RolePropertyConfigValidator validator = new tb_RolePropertyConfigValidator();
            ValidationResult results = validator.Validate(info);
            return results;
        }
        
        #region 扩展方法
        
        /// <summary>
        /// 某字段是否存在
        /// </summary>
        /// <param name="exp">e => e.ModeuleName == mod.ModeuleName</param>
        /// <returns></returns>
        public override bool ExistFieldValue(Expression<Func<T, bool>> exp)
        {
            return _unitOfWorkManage.GetDbClient().Queryable<T>().Where(exp).Any();
        }
      
        
        /// <summary>
        /// 雪花ID模式下的新增和修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ReturnResults<tb_RolePropertyConfig>> SaveOrUpdate(tb_RolePropertyConfig entity)
        {
            ReturnResults<tb_RolePropertyConfig> rr = new ReturnResults<tb_RolePropertyConfig>();
            tb_RolePropertyConfig Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RolePropertyID > 0)
                {
                    bool rs = await _tb_RolePropertyConfigServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_RolePropertyConfigServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
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
            tb_RolePropertyConfig entity = model as tb_RolePropertyConfig;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RolePropertyID > 0)
                {
                    bool rs = await _tb_RolePropertyConfigServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_RolePropertyConfigServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
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
            List<T> list = await _tb_RolePropertyConfigServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_RolePropertyConfig entity = item as tb_RolePropertyConfig;
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
            List<T> list = await _tb_RolePropertyConfigServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_RolePropertyConfig entity = item as tb_RolePropertyConfig;
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
            tb_RolePropertyConfig entity = model as tb_RolePropertyConfig;
            bool rs = await _tb_RolePropertyConfigServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_RolePropertyConfig>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_RolePropertyConfig> entitys = models as List<tb_RolePropertyConfig>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_RolePropertyConfig>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.RolePropertyID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_RolePropertyConfig>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_RolePropertyConfigValidator validator = new tb_RolePropertyConfigValidator();
            ValidationResult results = validator.Validate(info as tb_RolePropertyConfig);
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
            Command command = new Command();
            ReturnMainSubResults<T> rsms = new ReturnMainSubResults<T>();
            try
            {
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_RolePropertyConfig entity = model as tb_RolePropertyConfig;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RolePropertyID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_RolePropertyConfig>(entity as tb_RolePropertyConfig)
                        .Include(m => m.tb_RoleInfos)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_RolePropertyConfig>(entity as tb_RolePropertyConfig)
                .Include(m => m.tb_RoleInfos)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RolePropertyID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                _logger.Error(ex);
                //出错后，取消生成的ID等值
                command.Undo();
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>()
                                .Includes(m => m.tb_RoleInfos)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_RolePropertyConfig entity = model as tb_RolePropertyConfig;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_RolePropertyConfig>(m => m.RolePropertyID== entity.RolePropertyID)
                                .Include(m => m.tb_RoleInfos)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_RolePropertyConfig AddReEntity(tb_RolePropertyConfig entity)
        {
            tb_RolePropertyConfig AddEntity =  _tb_RolePropertyConfigServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_RolePropertyConfig> AddReEntityAsync(tb_RolePropertyConfig entity)
        {
            tb_RolePropertyConfig AddEntity = await _tb_RolePropertyConfigServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_RolePropertyConfig entity)
        {
            long id = await _tb_RolePropertyConfigServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_RolePropertyConfig> infos)
        {
            List<long> ids = await _tb_RolePropertyConfigServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_RolePropertyConfig entity)
        {
            bool rs = await _tb_RolePropertyConfigServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RolePropertyConfig>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_RolePropertyConfig entity)
        {
            bool rs = await _tb_RolePropertyConfigServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_RolePropertyConfigServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RolePropertyConfig>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_RolePropertyConfigServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RolePropertyConfig>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_RolePropertyConfig>> QueryAsync()
        {
            List<tb_RolePropertyConfig> list = await  _tb_RolePropertyConfigServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        
        public virtual List<tb_RolePropertyConfig> Query()
        {
            List<tb_RolePropertyConfig> list =  _tb_RolePropertyConfigServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        
        public virtual List<tb_RolePropertyConfig> Query(string wheresql)
        {
            List<tb_RolePropertyConfig> list =  _tb_RolePropertyConfigServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        
        public virtual async Task<List<tb_RolePropertyConfig>> QueryAsync(string wheresql) 
        {
            List<tb_RolePropertyConfig> list = await _tb_RolePropertyConfigServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_RolePropertyConfig>> QueryAsync(Expression<Func<tb_RolePropertyConfig, bool>> exp)
        {
            List<tb_RolePropertyConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_RolePropertyConfig>> QueryByNavAsync()
        {
            List<tb_RolePropertyConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>()
                                            .Includes(t => t.tb_RoleInfos )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_RolePropertyConfig>> QueryByNavAsync(Expression<Func<tb_RolePropertyConfig, bool>> exp)
        {
            List<tb_RolePropertyConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>().Where(exp)
                                            .Includes(t => t.tb_RoleInfos )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_RolePropertyConfig> QueryByNav(Expression<Func<tb_RolePropertyConfig, bool>> exp)
        {
            List<tb_RolePropertyConfig> list = _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>().Where(exp)
                                        .Includes(t => t.tb_RoleInfos )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_RolePropertyConfig>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_RolePropertyConfigServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_RolePropertyConfig entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_RolePropertyConfig>().Where(w => w.RolePropertyID == (long)id)
                                         .Includes(t => t.tb_RoleInfos )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_RolePropertyConfig>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



