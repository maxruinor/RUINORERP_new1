
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:45
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
    /// 返工退库
    /// </summary>
    public partial class tb_MRP_ReworkReturnController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_MRP_ReworkReturnServices _tb_MRP_ReworkReturnServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_MRP_ReworkReturnController(ILogger<tb_MRP_ReworkReturnController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_MRP_ReworkReturnServices tb_MRP_ReworkReturnServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_MRP_ReworkReturnServices = tb_MRP_ReworkReturnServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_MRP_ReworkReturn info)
        {

           // tb_MRP_ReworkReturnValidator validator = new tb_MRP_ReworkReturnValidator();
           tb_MRP_ReworkReturnValidator validator = _appContext.GetRequiredService<tb_MRP_ReworkReturnValidator>();
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
        public async Task<ReturnResults<tb_MRP_ReworkReturn>> SaveOrUpdate(tb_MRP_ReworkReturn entity)
        {
            ReturnResults<tb_MRP_ReworkReturn> rr = new ReturnResults<tb_MRP_ReworkReturn>();
            tb_MRP_ReworkReturn Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReworkReturnID > 0)
                {
                    bool rs = await _tb_MRP_ReworkReturnServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_MRP_ReworkReturnServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
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
            tb_MRP_ReworkReturn entity = model as tb_MRP_ReworkReturn;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ReworkReturnID > 0)
                {
                    bool rs = await _tb_MRP_ReworkReturnServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_MRP_ReworkReturnServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
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
            List<T> list = await _tb_MRP_ReworkReturnServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_MRP_ReworkReturn entity = item as tb_MRP_ReworkReturn;
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
            List<T> list = await _tb_MRP_ReworkReturnServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_MRP_ReworkReturn entity = item as tb_MRP_ReworkReturn;
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
            tb_MRP_ReworkReturn entity = model as tb_MRP_ReworkReturn;
            bool rs = await _tb_MRP_ReworkReturnServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkReturn>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_MRP_ReworkReturn> entitys = models as List<tb_MRP_ReworkReturn>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_MRP_ReworkReturn>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ReworkReturnID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkReturn>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_MRP_ReworkReturnValidator validator = new tb_MRP_ReworkReturnValidator();
           tb_MRP_ReworkReturnValidator validator = _appContext.GetRequiredService<tb_MRP_ReworkReturnValidator>();
            ValidationResult results = validator.Validate(info as tb_MRP_ReworkReturn);
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

                tb_MRP_ReworkReturn entity = model as tb_MRP_ReworkReturn;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ReworkReturnID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_MRP_ReworkReturn>(entity as tb_MRP_ReworkReturn)
                        .Include(m => m.tb_MRP_ReworkReturnDetails)
                    .Include(m => m.tb_MRP_ReworkEntries)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_MRP_ReworkReturn>(entity as tb_MRP_ReworkReturn)
                .Include(m => m.tb_MRP_ReworkReturnDetails)
                .Include(m => m.tb_MRP_ReworkEntries)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ReworkReturnID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>()
                                .Includes(m => m.tb_MRP_ReworkReturnDetails)
                        .Includes(m => m.tb_MRP_ReworkEntries)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_MRP_ReworkReturn entity = model as tb_MRP_ReworkReturn;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_MRP_ReworkReturn>(m => m.ReworkReturnID== entity.ReworkReturnID)
                                .Include(m => m.tb_MRP_ReworkReturnDetails)
                        .Include(m => m.tb_MRP_ReworkEntries)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_MRP_ReworkReturn AddReEntity(tb_MRP_ReworkReturn entity)
        {
            tb_MRP_ReworkReturn AddEntity =  _tb_MRP_ReworkReturnServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_MRP_ReworkReturn> AddReEntityAsync(tb_MRP_ReworkReturn entity)
        {
            tb_MRP_ReworkReturn AddEntity = await _tb_MRP_ReworkReturnServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_MRP_ReworkReturn entity)
        {
            long id = await _tb_MRP_ReworkReturnServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_MRP_ReworkReturn> infos)
        {
            List<long> ids = await _tb_MRP_ReworkReturnServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_MRP_ReworkReturn entity)
        {
            bool rs = await _tb_MRP_ReworkReturnServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkReturn>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_MRP_ReworkReturn entity)
        {
            bool rs = await _tb_MRP_ReworkReturnServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_MRP_ReworkReturnServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkReturn>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_MRP_ReworkReturnServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_MRP_ReworkReturn>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_MRP_ReworkReturn>> QueryAsync()
        {
            List<tb_MRP_ReworkReturn> list = await  _tb_MRP_ReworkReturnServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        
        public virtual List<tb_MRP_ReworkReturn> Query()
        {
            List<tb_MRP_ReworkReturn> list =  _tb_MRP_ReworkReturnServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        
        public virtual List<tb_MRP_ReworkReturn> Query(string wheresql)
        {
            List<tb_MRP_ReworkReturn> list =  _tb_MRP_ReworkReturnServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        
        public virtual async Task<List<tb_MRP_ReworkReturn>> QueryAsync(string wheresql) 
        {
            List<tb_MRP_ReworkReturn> list = await _tb_MRP_ReworkReturnServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_MRP_ReworkReturn>> QueryAsync(Expression<Func<tb_MRP_ReworkReturn, bool>> exp)
        {
            List<tb_MRP_ReworkReturn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MRP_ReworkReturn>> QueryByNavAsync()
        {
            List<tb_MRP_ReworkReturn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_MRP_ReworkReturnDetails )
                                .Includes(t => t.tb_MRP_ReworkEntries )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_MRP_ReworkReturn>> QueryByNavAsync(Expression<Func<tb_MRP_ReworkReturn, bool>> exp)
        {
            List<tb_MRP_ReworkReturn> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_MRP_ReworkReturnDetails )
                                .Includes(t => t.tb_MRP_ReworkEntries )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_MRP_ReworkReturn> QueryByNav(Expression<Func<tb_MRP_ReworkReturn, bool>> exp)
        {
            List<tb_MRP_ReworkReturn> list = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_MRP_ReworkReturnDetails )
                            .Includes(t => t.tb_MRP_ReworkEntries )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_MRP_ReworkReturn>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_MRP_ReworkReturnServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_MRP_ReworkReturn entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_MRP_ReworkReturn>().Where(w => w.ReworkReturnID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_MRP_ReworkReturnDetails )
                            .Includes(t => t.tb_MRP_ReworkEntries )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_MRP_ReworkReturn>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



