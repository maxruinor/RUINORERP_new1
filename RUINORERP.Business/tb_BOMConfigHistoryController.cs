
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:26
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
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识
    /// </summary>
    public partial class tb_BOMConfigHistoryController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BOMConfigHistoryServices _tb_BOMConfigHistoryServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_BOMConfigHistoryController(ILogger<tb_BOMConfigHistoryController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_BOMConfigHistoryServices tb_BOMConfigHistoryServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_BOMConfigHistoryServices = tb_BOMConfigHistoryServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_BOMConfigHistory info)
        {
            tb_BOMConfigHistoryValidator validator = new tb_BOMConfigHistoryValidator();
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
        public async Task<ReturnResults<tb_BOMConfigHistory>> SaveOrUpdate(tb_BOMConfigHistory entity)
        {
            ReturnResults<tb_BOMConfigHistory> rr = new ReturnResults<tb_BOMConfigHistory>();
            tb_BOMConfigHistory Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BOM_S_VERID > 0)
                {
                    bool rs = await _tb_BOMConfigHistoryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BOMConfigHistoryServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
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
            tb_BOMConfigHistory entity = model as tb_BOMConfigHistory;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BOM_S_VERID > 0)
                {
                    bool rs = await _tb_BOMConfigHistoryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BOMConfigHistoryServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
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
            List<T> list = await _tb_BOMConfigHistoryServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BOMConfigHistory entity = item as tb_BOMConfigHistory;
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
            List<T> list = await _tb_BOMConfigHistoryServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BOMConfigHistory entity = item as tb_BOMConfigHistory;
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
            tb_BOMConfigHistory entity = model as tb_BOMConfigHistory;
            bool rs = await _tb_BOMConfigHistoryServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_BOMConfigHistory>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_BOMConfigHistory> entitys = models as List<tb_BOMConfigHistory>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BOMConfigHistory>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BOM_S_VERID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_BOMConfigHistory>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_BOMConfigHistoryValidator validator = new tb_BOMConfigHistoryValidator();
            ValidationResult results = validator.Validate(info as tb_BOMConfigHistory);
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
                tb_BOMConfigHistory entity = model as tb_BOMConfigHistory;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.BOM_S_VERID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BOMConfigHistory>(entity as tb_BOMConfigHistory)
                        .Include(m => m.tb_BOM_Ss)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BOMConfigHistory>(entity as tb_BOMConfigHistory)
                .Include(m => m.tb_BOM_Ss)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BOM_S_VERID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>()
                                .Includes(m => m.tb_BOM_Ss)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_BOMConfigHistory entity = model as tb_BOMConfigHistory;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BOMConfigHistory>(m => m.BOM_S_VERID== entity.BOM_S_VERID)
                                .Include(m => m.tb_BOM_Ss)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_BOMConfigHistory AddReEntity(tb_BOMConfigHistory entity)
        {
            tb_BOMConfigHistory AddEntity =  _tb_BOMConfigHistoryServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_BOMConfigHistory> AddReEntityAsync(tb_BOMConfigHistory entity)
        {
            tb_BOMConfigHistory AddEntity = await _tb_BOMConfigHistoryServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_BOMConfigHistory entity)
        {
            long id = await _tb_BOMConfigHistoryServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_BOMConfigHistory> infos)
        {
            List<long> ids = await _tb_BOMConfigHistoryServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_BOMConfigHistory entity)
        {
            bool rs = await _tb_BOMConfigHistoryServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOMConfigHistory>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_BOMConfigHistory entity)
        {
            bool rs = await _tb_BOMConfigHistoryServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BOMConfigHistoryServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOMConfigHistory>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BOMConfigHistoryServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOMConfigHistory>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_BOMConfigHistory>> QueryAsync()
        {
            List<tb_BOMConfigHistory> list = await  _tb_BOMConfigHistoryServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        
        public virtual List<tb_BOMConfigHistory> Query()
        {
            List<tb_BOMConfigHistory> list =  _tb_BOMConfigHistoryServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        
        public virtual List<tb_BOMConfigHistory> Query(string wheresql)
        {
            List<tb_BOMConfigHistory> list =  _tb_BOMConfigHistoryServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        
        public virtual async Task<List<tb_BOMConfigHistory>> QueryAsync(string wheresql) 
        {
            List<tb_BOMConfigHistory> list = await _tb_BOMConfigHistoryServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BOMConfigHistory>> QueryAsync(Expression<Func<tb_BOMConfigHistory, bool>> exp)
        {
            List<tb_BOMConfigHistory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BOMConfigHistory>> QueryByNavAsync()
        {
            List<tb_BOMConfigHistory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>()
                                            .Includes(t => t.tb_BOM_Ss )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BOMConfigHistory>> QueryByNavAsync(Expression<Func<tb_BOMConfigHistory, bool>> exp)
        {
            List<tb_BOMConfigHistory> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>().Where(exp)
                                            .Includes(t => t.tb_BOM_Ss )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_BOMConfigHistory> QueryByNav(Expression<Func<tb_BOMConfigHistory, bool>> exp)
        {
            List<tb_BOMConfigHistory> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>().Where(exp)
                                        .Includes(t => t.tb_BOM_Ss )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BOMConfigHistory>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BOMConfigHistoryServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BOMConfigHistory entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOMConfigHistory>().Where(w => w.BOM_S_VERID == (long)id)
                                         .Includes(t => t.tb_BOM_Ss )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_BOMConfigHistory>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



