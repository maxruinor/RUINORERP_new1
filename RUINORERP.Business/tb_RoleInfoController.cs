
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2024 12:31:33
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
    /// 角色表
    /// </summary>
    public partial class tb_RoleInfoController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_RoleInfoServices _tb_RoleInfoServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_RoleInfoController(ILogger<tb_RoleInfoController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_RoleInfoServices tb_RoleInfoServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_RoleInfoServices = tb_RoleInfoServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_RoleInfo info)
        {
            tb_RoleInfoValidator validator = new tb_RoleInfoValidator();
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
        public async Task<ReturnResults<tb_RoleInfo>> SaveOrUpdate(tb_RoleInfo entity)
        {
            ReturnResults<tb_RoleInfo> rr = new ReturnResults<tb_RoleInfo>();
            tb_RoleInfo Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RoleID > 0)
                {
                    bool rs = await _tb_RoleInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_RoleInfoServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
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
            tb_RoleInfo entity = model as tb_RoleInfo;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RoleID > 0)
                {
                    bool rs = await _tb_RoleInfoServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_RoleInfoServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
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
            List<T> list = await _tb_RoleInfoServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_RoleInfo entity = item as tb_RoleInfo;
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
            List<T> list = await _tb_RoleInfoServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_RoleInfo entity = item as tb_RoleInfo;
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
            tb_RoleInfo entity = model as tb_RoleInfo;
            bool rs = await _tb_RoleInfoServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_RoleInfo>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_RoleInfo> entitys = models as List<tb_RoleInfo>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_RoleInfo>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.RoleID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_RoleInfo>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_RoleInfoValidator validator = new tb_RoleInfoValidator();
            ValidationResult results = validator.Validate(info as tb_RoleInfo);
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_RoleInfo entity = model as tb_RoleInfo;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.RoleID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_RoleInfo>(entity as tb_RoleInfo)
                        .Include(m => m.tb_User_Roles)
                    .Include(m => m.tb_P4Fields)
                    .Include(m => m.tb_P4Buttons)
                    .Include(m => m.tb_P4Menus)
                    .Include(m => m.tb_P4Modules)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_RoleInfo>(entity as tb_RoleInfo)
                .Include(m => m.tb_User_Roles)
                .Include(m => m.tb_P4Fields)
                .Include(m => m.tb_P4Buttons)
                .Include(m => m.tb_P4Menus)
                .Include(m => m.tb_P4Modules)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RoleID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                //_logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                // rr.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>()
                                .Includes(m => m.tb_User_Roles)
                        .Includes(m => m.tb_P4Fields)
                        .Includes(m => m.tb_P4Buttons)
                        .Includes(m => m.tb_P4Menus)
                        .Includes(m => m.tb_P4Modules)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_RoleInfo entity = model as tb_RoleInfo;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_RoleInfo>(m => m.RoleID== entity.RoleID)
                                .Include(m => m.tb_User_Roles)
                        .Include(m => m.tb_P4Fields)
                        .Include(m => m.tb_P4Buttons)
                        .Include(m => m.tb_P4Menus)
                        .Include(m => m.tb_P4Modules)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_RoleInfo AddReEntity(tb_RoleInfo entity)
        {
            tb_RoleInfo AddEntity =  _tb_RoleInfoServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_RoleInfo> AddReEntityAsync(tb_RoleInfo entity)
        {
            tb_RoleInfo AddEntity = await _tb_RoleInfoServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_RoleInfo entity)
        {
            long id = await _tb_RoleInfoServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_RoleInfo> infos)
        {
            List<long> ids = await _tb_RoleInfoServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_RoleInfo entity)
        {
            bool rs = await _tb_RoleInfoServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RoleInfo>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_RoleInfo entity)
        {
            bool rs = await _tb_RoleInfoServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_RoleInfoServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RoleInfo>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_RoleInfoServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_RoleInfo>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_RoleInfo>> QueryAsync()
        {
            List<tb_RoleInfo> list = await  _tb_RoleInfoServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        
        public virtual List<tb_RoleInfo> Query()
        {
            List<tb_RoleInfo> list =  _tb_RoleInfoServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        
        public virtual List<tb_RoleInfo> Query(string wheresql)
        {
            List<tb_RoleInfo> list =  _tb_RoleInfoServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        
        public virtual async Task<List<tb_RoleInfo>> QueryAsync(string wheresql) 
        {
            List<tb_RoleInfo> list = await _tb_RoleInfoServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_RoleInfo>> QueryAsync(Expression<Func<tb_RoleInfo, bool>> exp)
        {
            List<tb_RoleInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_RoleInfo>> QueryByNavAsync()
        {
            List<tb_RoleInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>()
                               .Includes(t => t.tb_rolepropertyconfig )
                                            .Includes(t => t.tb_User_Roles )
                                .Includes(t => t.tb_P4Fields )
                                .Includes(t => t.tb_P4Buttons )
                                .Includes(t => t.tb_P4Menus )
                                .Includes(t => t.tb_P4Modules )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_RoleInfo>> QueryByNavAsync(Expression<Func<tb_RoleInfo, bool>> exp)
        {
            List<tb_RoleInfo> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>().Where(exp)
                               .Includes(t => t.tb_rolepropertyconfig )
                                            .Includes(t => t.tb_User_Roles )
                                .Includes(t => t.tb_P4Fields )
                                .Includes(t => t.tb_P4Buttons )
                                .Includes(t => t.tb_P4Menus )
                                .Includes(t => t.tb_P4Modules )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_RoleInfo> QueryByNav(Expression<Func<tb_RoleInfo, bool>> exp)
        {
            List<tb_RoleInfo> list = _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>().Where(exp)
                            .Includes(t => t.tb_rolepropertyconfig )
                                        .Includes(t => t.tb_User_Roles )
                            .Includes(t => t.tb_P4Fields )
                            .Includes(t => t.tb_P4Buttons )
                            .Includes(t => t.tb_P4Menus )
                            .Includes(t => t.tb_P4Modules )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_RoleInfo>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_RoleInfoServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_RoleInfo entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_RoleInfo>().Where(w => w.RoleID == (long)id)
                             .Includes(t => t.tb_rolepropertyconfig )
                                        .Includes(t => t.tb_User_Roles )
                            .Includes(t => t.tb_P4Fields )
                            .Includes(t => t.tb_P4Buttons )
                            .Includes(t => t.tb_P4Menus )
                            .Includes(t => t.tb_P4Modules )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_RoleInfo>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



