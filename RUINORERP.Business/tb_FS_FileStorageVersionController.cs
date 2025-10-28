// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/28/2025 17:14:17
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
    /// 文件版本表
    /// </summary>
    public partial class tb_FS_FileStorageVersionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FS_FileStorageVersionServices _tb_FS_FileStorageVersionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FS_FileStorageVersionController(ILogger<tb_FS_FileStorageVersionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FS_FileStorageVersionServices tb_FS_FileStorageVersionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FS_FileStorageVersionServices = tb_FS_FileStorageVersionServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FS_FileStorageVersion info)
        {

           // tb_FS_FileStorageVersionValidator validator = new tb_FS_FileStorageVersionValidator();
           tb_FS_FileStorageVersionValidator validator = _appContext.GetRequiredService<tb_FS_FileStorageVersionValidator>();
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
        public async Task<ReturnResults<tb_FS_FileStorageVersion>> SaveOrUpdate(tb_FS_FileStorageVersion entity)
        {
            ReturnResults<tb_FS_FileStorageVersion> rr = new ReturnResults<tb_FS_FileStorageVersion>();
            tb_FS_FileStorageVersion Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.VersionId > 0)
                {
                    bool rs = await _tb_FS_FileStorageVersionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FS_FileStorageVersionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
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
            tb_FS_FileStorageVersion entity = model as tb_FS_FileStorageVersion;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.VersionId > 0)
                {
                    bool rs = await _tb_FS_FileStorageVersionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FS_FileStorageVersionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
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
            List<T> list = await _tb_FS_FileStorageVersionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FS_FileStorageVersion entity = item as tb_FS_FileStorageVersion;
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
            List<T> list = await _tb_FS_FileStorageVersionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FS_FileStorageVersion entity = item as tb_FS_FileStorageVersion;
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
            tb_FS_FileStorageVersion entity = model as tb_FS_FileStorageVersion;
            bool rs = await _tb_FS_FileStorageVersionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FS_FileStorageVersion>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FS_FileStorageVersion> entitys = models as List<tb_FS_FileStorageVersion>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FS_FileStorageVersion>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.VersionId).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FS_FileStorageVersion>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FS_FileStorageVersionValidator validator = new tb_FS_FileStorageVersionValidator();
           tb_FS_FileStorageVersionValidator validator = _appContext.GetRequiredService<tb_FS_FileStorageVersionValidator>();
            ValidationResult results = validator.Validate(info as tb_FS_FileStorageVersion);
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

                tb_FS_FileStorageVersion entity = model as tb_FS_FileStorageVersion;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.VersionId > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_FS_FileStorageVersion>(entity as tb_FS_FileStorageVersion)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_FS_FileStorageVersion>(entity as tb_FS_FileStorageVersion)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.VersionId;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FS_FileStorageVersion entity = model as tb_FS_FileStorageVersion;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FS_FileStorageVersion>(m => m.VersionId== entity.VersionId)
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
        
        
        
        public tb_FS_FileStorageVersion AddReEntity(tb_FS_FileStorageVersion entity)
        {
            tb_FS_FileStorageVersion AddEntity =  _tb_FS_FileStorageVersionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FS_FileStorageVersion> AddReEntityAsync(tb_FS_FileStorageVersion entity)
        {
            tb_FS_FileStorageVersion AddEntity = await _tb_FS_FileStorageVersionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FS_FileStorageVersion entity)
        {
            long id = await _tb_FS_FileStorageVersionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FS_FileStorageVersion> infos)
        {
            List<long> ids = await _tb_FS_FileStorageVersionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FS_FileStorageVersion entity)
        {
            bool rs = await _tb_FS_FileStorageVersionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FS_FileStorageVersion>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FS_FileStorageVersion entity)
        {
            bool rs = await _tb_FS_FileStorageVersionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FS_FileStorageVersionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FS_FileStorageVersion>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FS_FileStorageVersionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FS_FileStorageVersion>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FS_FileStorageVersion>> QueryAsync()
        {
            List<tb_FS_FileStorageVersion> list = await  _tb_FS_FileStorageVersionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        
        public virtual List<tb_FS_FileStorageVersion> Query()
        {
            List<tb_FS_FileStorageVersion> list =  _tb_FS_FileStorageVersionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        
        public virtual List<tb_FS_FileStorageVersion> Query(string wheresql)
        {
            List<tb_FS_FileStorageVersion> list =  _tb_FS_FileStorageVersionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FS_FileStorageVersion>> QueryAsync(string wheresql) 
        {
            List<tb_FS_FileStorageVersion> list = await _tb_FS_FileStorageVersionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FS_FileStorageVersion>> QueryAsync(Expression<Func<tb_FS_FileStorageVersion, bool>> exp)
        {
            List<tb_FS_FileStorageVersion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FS_FileStorageVersion>> QueryByNavAsync()
        {
            List<tb_FS_FileStorageVersion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>()
                               .Includes(t => t.tb_fs_filestorageinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FS_FileStorageVersion>> QueryByNavAsync(Expression<Func<tb_FS_FileStorageVersion, bool>> exp)
        {
            List<tb_FS_FileStorageVersion> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>().Where(exp)
                               .Includes(t => t.tb_fs_filestorageinfo )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FS_FileStorageVersion> QueryByNav(Expression<Func<tb_FS_FileStorageVersion, bool>> exp)
        {
            List<tb_FS_FileStorageVersion> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>().Where(exp)
                            .Includes(t => t.tb_fs_filestorageinfo )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FS_FileStorageVersion>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FS_FileStorageVersionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FS_FileStorageVersion entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FS_FileStorageVersion>().Where(w => w.VersionId == (long)id)
                             .Includes(t => t.tb_fs_filestorageinfo )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FS_FileStorageVersion>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



