
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:55
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
    /// 工作台配置表
    /// </summary>
    public partial class tb_WorkCenterConfigController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_WorkCenterConfigServices _tb_WorkCenterConfigServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_WorkCenterConfigController(ILogger<tb_WorkCenterConfigController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_WorkCenterConfigServices tb_WorkCenterConfigServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_WorkCenterConfigServices = tb_WorkCenterConfigServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_WorkCenterConfig info)
        {

           // tb_WorkCenterConfigValidator validator = new tb_WorkCenterConfigValidator();
           tb_WorkCenterConfigValidator validator = _appContext.GetRequiredService<tb_WorkCenterConfigValidator>();
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
        public async Task<ReturnResults<tb_WorkCenterConfig>> SaveOrUpdate(tb_WorkCenterConfig entity)
        {
            ReturnResults<tb_WorkCenterConfig> rr = new ReturnResults<tb_WorkCenterConfig>();
            tb_WorkCenterConfig Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ConfigID > 0)
                {
                    bool rs = await _tb_WorkCenterConfigServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_WorkCenterConfigServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
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
            tb_WorkCenterConfig entity = model as tb_WorkCenterConfig;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ConfigID > 0)
                {
                    bool rs = await _tb_WorkCenterConfigServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_WorkCenterConfigServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
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
            List<T> list = await _tb_WorkCenterConfigServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_WorkCenterConfig entity = item as tb_WorkCenterConfig;
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
            List<T> list = await _tb_WorkCenterConfigServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_WorkCenterConfig entity = item as tb_WorkCenterConfig;
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
            tb_WorkCenterConfig entity = model as tb_WorkCenterConfig;
            bool rs = await _tb_WorkCenterConfigServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_WorkCenterConfig>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_WorkCenterConfig> entitys = models as List<tb_WorkCenterConfig>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_WorkCenterConfig>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ConfigID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_WorkCenterConfig>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_WorkCenterConfigValidator validator = new tb_WorkCenterConfigValidator();
           tb_WorkCenterConfigValidator validator = _appContext.GetRequiredService<tb_WorkCenterConfigValidator>();
            ValidationResult results = validator.Validate(info as tb_WorkCenterConfig);
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

                tb_WorkCenterConfig entity = model as tb_WorkCenterConfig;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ConfigID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_WorkCenterConfig>(entity as tb_WorkCenterConfig)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_WorkCenterConfig>(entity as tb_WorkCenterConfig)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ConfigID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_WorkCenterConfig entity = model as tb_WorkCenterConfig;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_WorkCenterConfig>(m => m.ConfigID== entity.ConfigID)
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
        
        
        
        public tb_WorkCenterConfig AddReEntity(tb_WorkCenterConfig entity)
        {
            tb_WorkCenterConfig AddEntity =  _tb_WorkCenterConfigServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_WorkCenterConfig> AddReEntityAsync(tb_WorkCenterConfig entity)
        {
            tb_WorkCenterConfig AddEntity = await _tb_WorkCenterConfigServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_WorkCenterConfig entity)
        {
            long id = await _tb_WorkCenterConfigServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_WorkCenterConfig> infos)
        {
            List<long> ids = await _tb_WorkCenterConfigServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_WorkCenterConfig entity)
        {
            bool rs = await _tb_WorkCenterConfigServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_WorkCenterConfig>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_WorkCenterConfig entity)
        {
            bool rs = await _tb_WorkCenterConfigServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_WorkCenterConfigServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_WorkCenterConfig>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_WorkCenterConfigServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_WorkCenterConfig>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_WorkCenterConfig>> QueryAsync()
        {
            List<tb_WorkCenterConfig> list = await  _tb_WorkCenterConfigServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        
        public virtual List<tb_WorkCenterConfig> Query()
        {
            List<tb_WorkCenterConfig> list =  _tb_WorkCenterConfigServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        
        public virtual List<tb_WorkCenterConfig> Query(string wheresql)
        {
            List<tb_WorkCenterConfig> list =  _tb_WorkCenterConfigServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        
        public virtual async Task<List<tb_WorkCenterConfig>> QueryAsync(string wheresql) 
        {
            List<tb_WorkCenterConfig> list = await _tb_WorkCenterConfigServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_WorkCenterConfig>> QueryAsync(Expression<Func<tb_WorkCenterConfig, bool>> exp)
        {
            List<tb_WorkCenterConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_WorkCenterConfig>> QueryByNavAsync()
        {
            List<tb_WorkCenterConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_WorkCenterConfig>> QueryByNavAsync(Expression<Func<tb_WorkCenterConfig, bool>> exp)
        {
            List<tb_WorkCenterConfig> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_WorkCenterConfig> QueryByNav(Expression<Func<tb_WorkCenterConfig, bool>> exp)
        {
            List<tb_WorkCenterConfig> list = _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_WorkCenterConfig>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_WorkCenterConfigServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_WorkCenterConfig entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_WorkCenterConfig>().Where(w => w.ConfigID == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_WorkCenterConfig>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



