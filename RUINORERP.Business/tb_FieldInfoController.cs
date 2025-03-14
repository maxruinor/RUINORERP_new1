
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:40
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
    /// 字段信息表
    /// </summary>
    public partial class tb_FieldInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FieldInfoServices _tb_FieldInfoServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FieldInfoController(ILogger<tb_FieldInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FieldInfoServices tb_FieldInfoServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FieldInfoServices = tb_FieldInfoServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FieldInfo info)
        {

           // tb_FieldInfoValidator validator = new tb_FieldInfoValidator();
           tb_FieldInfoValidator validator = _appContext.GetRequiredService<tb_FieldInfoValidator>();
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
        public async Task<ReturnResults<tb_FieldInfo>> SaveOrUpdate(tb_FieldInfo entity)
        {
            ReturnResults<tb_FieldInfo> rr = new ReturnResults<tb_FieldInfo>();
            tb_FieldInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.FieldInfo_ID > 0)
                {
                    bool rs = await _tb_FieldInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FieldInfoServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
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
            tb_FieldInfo entity = model as tb_FieldInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.FieldInfo_ID > 0)
                {
                    bool rs = await _tb_FieldInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FieldInfoServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
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
            List<T> list = await _tb_FieldInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FieldInfo entity = item as tb_FieldInfo;
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
            List<T> list = await _tb_FieldInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FieldInfo entity = item as tb_FieldInfo;
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
            tb_FieldInfo entity = model as tb_FieldInfo;
            bool rs = await _tb_FieldInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FieldInfo>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FieldInfo> entitys = models as List<tb_FieldInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FieldInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.FieldInfo_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FieldInfo>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FieldInfoValidator validator = new tb_FieldInfoValidator();
           tb_FieldInfoValidator validator = _appContext.GetRequiredService<tb_FieldInfoValidator>();
            ValidationResult results = validator.Validate(info as tb_FieldInfo);
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

                tb_FieldInfo entity = model as tb_FieldInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.FieldInfo_ID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FieldInfo>(entity as tb_FieldInfo)
                        .Include(m => m.tb_P4Fields)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FieldInfo>(entity as tb_FieldInfo)
                .Include(m => m.tb_P4Fields)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.FieldInfo_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>()
                                .Includes(m => m.tb_P4Fields)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FieldInfo entity = model as tb_FieldInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FieldInfo>(m => m.FieldInfo_ID== entity.FieldInfo_ID)
                                .Include(m => m.tb_P4Fields)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FieldInfo AddReEntity(tb_FieldInfo entity)
        {
            tb_FieldInfo AddEntity =  _tb_FieldInfoServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FieldInfo> AddReEntityAsync(tb_FieldInfo entity)
        {
            tb_FieldInfo AddEntity = await _tb_FieldInfoServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FieldInfo entity)
        {
            long id = await _tb_FieldInfoServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FieldInfo> infos)
        {
            List<long> ids = await _tb_FieldInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FieldInfo entity)
        {
            bool rs = await _tb_FieldInfoServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FieldInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FieldInfo entity)
        {
            bool rs = await _tb_FieldInfoServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FieldInfoServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FieldInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FieldInfoServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FieldInfo>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FieldInfo>> QueryAsync()
        {
            List<tb_FieldInfo> list = await  _tb_FieldInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        
        public virtual List<tb_FieldInfo> Query()
        {
            List<tb_FieldInfo> list =  _tb_FieldInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        
        public virtual List<tb_FieldInfo> Query(string wheresql)
        {
            List<tb_FieldInfo> list =  _tb_FieldInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FieldInfo>> QueryAsync(string wheresql) 
        {
            List<tb_FieldInfo> list = await _tb_FieldInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FieldInfo>> QueryAsync(Expression<Func<tb_FieldInfo, bool>> exp)
        {
            List<tb_FieldInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FieldInfo>> QueryByNavAsync()
        {
            List<tb_FieldInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>()
                               .Includes(t => t.tb_menuinfo )
                                            .Includes(t => t.tb_P4Fields )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FieldInfo>> QueryByNavAsync(Expression<Func<tb_FieldInfo, bool>> exp)
        {
            List<tb_FieldInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>().Where(exp)
                               .Includes(t => t.tb_menuinfo )
                                            .Includes(t => t.tb_P4Fields )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FieldInfo> QueryByNav(Expression<Func<tb_FieldInfo, bool>> exp)
        {
            List<tb_FieldInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>().Where(exp)
                            .Includes(t => t.tb_menuinfo )
                                        .Includes(t => t.tb_P4Fields )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FieldInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FieldInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FieldInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FieldInfo>().Where(w => w.FieldInfo_ID == (long)id)
                             .Includes(t => t.tb_menuinfo )
                                        .Includes(t => t.tb_P4Fields )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FieldInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



