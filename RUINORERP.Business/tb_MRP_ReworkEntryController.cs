
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:44
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
    /// 返工入库
    /// </summary>
    public partial class tb_MRP_ReworkEntryController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MRP_ReworkEntryServices _tb_MRP_ReworkEntryServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_MRP_ReworkEntryController(ILogger<tb_MRP_ReworkEntryController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MRP_ReworkEntryServices tb_MRP_ReworkEntryServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MRP_ReworkEntryServices = tb_MRP_ReworkEntryServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_MRP_ReworkEntry info)
        {

           // tb_MRP_ReworkEntryValidator validator = new tb_MRP_ReworkEntryValidator();
           tb_MRP_ReworkEntryValidator validator = _appContext.GetRequiredService<tb_MRP_ReworkEntryValidator>();
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
        public async Task<ReturnResults<tb_MRP_ReworkEntry>> SaveOrUpdate(tb_MRP_ReworkEntry entity)
        {
            ReturnResults<tb_MRP_ReworkEntry> rr = new ReturnResults<tb_MRP_ReworkEntry>();
            tb_MRP_ReworkEntry Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReworkEntryID > 0)
                {
                    bool rs = await _tb_MRP_ReworkEntryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MRP_ReworkEntryServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
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
            tb_MRP_ReworkEntry entity = model as tb_MRP_ReworkEntry;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReworkEntryID > 0)
                {
                    bool rs = await _tb_MRP_ReworkEntryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MRP_ReworkEntryServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
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
            List<T> list = await _tb_MRP_ReworkEntryServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MRP_ReworkEntry entity = item as tb_MRP_ReworkEntry;
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
            List<T> list = await _tb_MRP_ReworkEntryServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MRP_ReworkEntry entity = item as tb_MRP_ReworkEntry;
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
            tb_MRP_ReworkEntry entity = model as tb_MRP_ReworkEntry;
            bool rs = await _tb_MRP_ReworkEntryServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkEntry>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MRP_ReworkEntry> entitys = models as List<tb_MRP_ReworkEntry>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MRP_ReworkEntry>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ReworkEntryID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkEntry>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MRP_ReworkEntryValidator validator = new tb_MRP_ReworkEntryValidator();
           tb_MRP_ReworkEntryValidator validator = _appContext.GetRequiredService<tb_MRP_ReworkEntryValidator>();
            ValidationResult results = validator.Validate(info as tb_MRP_ReworkEntry);
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

                tb_MRP_ReworkEntry entity = model as tb_MRP_ReworkEntry;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ReworkEntryID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_MRP_ReworkEntry>(entity as tb_MRP_ReworkEntry)
                        .Include(m => m.tb_MRP_ReworkEntryDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_MRP_ReworkEntry>(entity as tb_MRP_ReworkEntry)
                .Include(m => m.tb_MRP_ReworkEntryDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ReworkEntryID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>()
                                .Includes(m => m.tb_MRP_ReworkEntryDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MRP_ReworkEntry entity = model as tb_MRP_ReworkEntry;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MRP_ReworkEntry>(m => m.ReworkEntryID== entity.ReworkEntryID)
                                .Include(m => m.tb_MRP_ReworkEntryDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_MRP_ReworkEntry AddReEntity(tb_MRP_ReworkEntry entity)
        {
            tb_MRP_ReworkEntry AddEntity =  _tb_MRP_ReworkEntryServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MRP_ReworkEntry> AddReEntityAsync(tb_MRP_ReworkEntry entity)
        {
            tb_MRP_ReworkEntry AddEntity = await _tb_MRP_ReworkEntryServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MRP_ReworkEntry entity)
        {
            long id = await _tb_MRP_ReworkEntryServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MRP_ReworkEntry> infos)
        {
            List<long> ids = await _tb_MRP_ReworkEntryServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MRP_ReworkEntry entity)
        {
            bool rs = await _tb_MRP_ReworkEntryServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkEntry>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MRP_ReworkEntry entity)
        {
            bool rs = await _tb_MRP_ReworkEntryServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MRP_ReworkEntryServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkEntry>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MRP_ReworkEntryServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkEntry>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MRP_ReworkEntry>> QueryAsync()
        {
            List<tb_MRP_ReworkEntry> list = await  _tb_MRP_ReworkEntryServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        
        public virtual List<tb_MRP_ReworkEntry> Query()
        {
            List<tb_MRP_ReworkEntry> list =  _tb_MRP_ReworkEntryServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        
        public virtual List<tb_MRP_ReworkEntry> Query(string wheresql)
        {
            List<tb_MRP_ReworkEntry> list =  _tb_MRP_ReworkEntryServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MRP_ReworkEntry>> QueryAsync(string wheresql) 
        {
            List<tb_MRP_ReworkEntry> list = await _tb_MRP_ReworkEntryServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MRP_ReworkEntry>> QueryAsync(Expression<Func<tb_MRP_ReworkEntry, bool>> exp)
        {
            List<tb_MRP_ReworkEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MRP_ReworkEntry>> QueryByNavAsync()
        {
            List<tb_MRP_ReworkEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_mrp_reworkreturn )
                                            .Includes(t => t.tb_MRP_ReworkEntryDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MRP_ReworkEntry>> QueryByNavAsync(Expression<Func<tb_MRP_ReworkEntry, bool>> exp)
        {
            List<tb_MRP_ReworkEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_mrp_reworkreturn )
                                            .Includes(t => t.tb_MRP_ReworkEntryDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MRP_ReworkEntry> QueryByNav(Expression<Func<tb_MRP_ReworkEntry, bool>> exp)
        {
            List<tb_MRP_ReworkEntry> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_mrp_reworkreturn )
                                        .Includes(t => t.tb_MRP_ReworkEntryDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MRP_ReworkEntry>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MRP_ReworkEntryServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MRP_ReworkEntry entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkEntry>().Where(w => w.ReworkEntryID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_mrp_reworkreturn )
                                        .Includes(t => t.tb_MRP_ReworkEntryDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkEntry>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



