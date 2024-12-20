
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:16
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据
    /// </summary>
    public partial class tb_UIMenuPersonalizationController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_UIMenuPersonalizationServices _tb_UIMenuPersonalizationServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_UIMenuPersonalizationController(ILogger<tb_UIMenuPersonalizationController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_UIMenuPersonalizationServices tb_UIMenuPersonalizationServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_UIMenuPersonalizationServices = tb_UIMenuPersonalizationServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_UIMenuPersonalization info)
        {

           // tb_UIMenuPersonalizationValidator validator = new tb_UIMenuPersonalizationValidator();
           tb_UIMenuPersonalizationValidator validator = _appContext.GetRequiredService<tb_UIMenuPersonalizationValidator>();
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
        public async Task<ReturnResults<tb_UIMenuPersonalization>> SaveOrUpdate(tb_UIMenuPersonalization entity)
        {
            ReturnResults<tb_UIMenuPersonalization> rr = new ReturnResults<tb_UIMenuPersonalization>();
            tb_UIMenuPersonalization Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIMenuPID > 0)
                {
                    bool rs = await _tb_UIMenuPersonalizationServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_UIMenuPersonalizationServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
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
            tb_UIMenuPersonalization entity = model as tb_UIMenuPersonalization;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.UIMenuPID > 0)
                {
                    bool rs = await _tb_UIMenuPersonalizationServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_UIMenuPersonalizationServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
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
            List<T> list = await _tb_UIMenuPersonalizationServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_UIMenuPersonalization entity = item as tb_UIMenuPersonalization;
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
            List<T> list = await _tb_UIMenuPersonalizationServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_UIMenuPersonalization entity = item as tb_UIMenuPersonalization;
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
            tb_UIMenuPersonalization entity = model as tb_UIMenuPersonalization;
            bool rs = await _tb_UIMenuPersonalizationServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_UIMenuPersonalization>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_UIMenuPersonalization> entitys = models as List<tb_UIMenuPersonalization>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_UIMenuPersonalization>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.UIMenuPID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_UIMenuPersonalization>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_UIMenuPersonalizationValidator validator = new tb_UIMenuPersonalizationValidator();
           tb_UIMenuPersonalizationValidator validator = _appContext.GetRequiredService<tb_UIMenuPersonalizationValidator>();
            ValidationResult results = validator.Validate(info as tb_UIMenuPersonalization);
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
                tb_UIMenuPersonalization entity = model as tb_UIMenuPersonalization;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.UIMenuPID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_UIMenuPersonalization>(entity as tb_UIMenuPersonalization)
                        .Include(m => m.tb_UIQueryConditions)
                    .Include(m => m.tb_UIGridSettings)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_UIMenuPersonalization>(entity as tb_UIMenuPersonalization)
                .Include(m => m.tb_UIQueryConditions)
                .Include(m => m.tb_UIGridSettings)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.UIMenuPID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>()
                                .Includes(m => m.tb_UIQueryConditions)
                        .Includes(m => m.tb_UIGridSettings)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_UIMenuPersonalization entity = model as tb_UIMenuPersonalization;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_UIMenuPersonalization>(m => m.UIMenuPID== entity.UIMenuPID)
                                .Include(m => m.tb_UIQueryConditions)
                        .Include(m => m.tb_UIGridSettings)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_UIMenuPersonalization AddReEntity(tb_UIMenuPersonalization entity)
        {
            tb_UIMenuPersonalization AddEntity =  _tb_UIMenuPersonalizationServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_UIMenuPersonalization> AddReEntityAsync(tb_UIMenuPersonalization entity)
        {
            tb_UIMenuPersonalization AddEntity = await _tb_UIMenuPersonalizationServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_UIMenuPersonalization entity)
        {
            long id = await _tb_UIMenuPersonalizationServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_UIMenuPersonalization> infos)
        {
            List<long> ids = await _tb_UIMenuPersonalizationServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_UIMenuPersonalization entity)
        {
            bool rs = await _tb_UIMenuPersonalizationServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIMenuPersonalization>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_UIMenuPersonalization entity)
        {
            bool rs = await _tb_UIMenuPersonalizationServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_UIMenuPersonalizationServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIMenuPersonalization>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_UIMenuPersonalizationServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_UIMenuPersonalization>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_UIMenuPersonalization>> QueryAsync()
        {
            List<tb_UIMenuPersonalization> list = await  _tb_UIMenuPersonalizationServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        
        public virtual List<tb_UIMenuPersonalization> Query()
        {
            List<tb_UIMenuPersonalization> list =  _tb_UIMenuPersonalizationServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        
        public virtual List<tb_UIMenuPersonalization> Query(string wheresql)
        {
            List<tb_UIMenuPersonalization> list =  _tb_UIMenuPersonalizationServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        
        public virtual async Task<List<tb_UIMenuPersonalization>> QueryAsync(string wheresql) 
        {
            List<tb_UIMenuPersonalization> list = await _tb_UIMenuPersonalizationServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_UIMenuPersonalization>> QueryAsync(Expression<Func<tb_UIMenuPersonalization, bool>> exp)
        {
            List<tb_UIMenuPersonalization> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIMenuPersonalization>> QueryByNavAsync()
        {
            List<tb_UIMenuPersonalization> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>()
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_userpersonalized )
                                            .Includes(t => t.tb_UIQueryConditions )
                                .Includes(t => t.tb_UIGridSettings )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_UIMenuPersonalization>> QueryByNavAsync(Expression<Func<tb_UIMenuPersonalization, bool>> exp)
        {
            List<tb_UIMenuPersonalization> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>().Where(exp)
                               .Includes(t => t.tb_menuinfo )
                               .Includes(t => t.tb_userpersonalized )
                                            .Includes(t => t.tb_UIQueryConditions )
                                .Includes(t => t.tb_UIGridSettings )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_UIMenuPersonalization> QueryByNav(Expression<Func<tb_UIMenuPersonalization, bool>> exp)
        {
            List<tb_UIMenuPersonalization> list = _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>().Where(exp)
                            .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_userpersonalized )
                                        .Includes(t => t.tb_UIQueryConditions )
                            .Includes(t => t.tb_UIGridSettings )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_UIMenuPersonalization>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_UIMenuPersonalizationServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_UIMenuPersonalization entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_UIMenuPersonalization>().Where(w => w.UIMenuPID == (long)id)
                             .Includes(t => t.tb_menuinfo )
                            .Includes(t => t.tb_userpersonalized )
                                        .Includes(t => t.tb_UIQueryConditions )
                            .Includes(t => t.tb_UIGridSettings )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_UIMenuPersonalization>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



