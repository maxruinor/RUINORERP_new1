
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:13
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
    /// 返厂售后单
    /// </summary>
    public partial class tb_ReturnController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ReturnServices _tb_ReturnServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ReturnController(ILogger<tb_ReturnController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ReturnServices tb_ReturnServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ReturnServices = tb_ReturnServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Return info)
        {

           // tb_ReturnValidator validator = new tb_ReturnValidator();
           tb_ReturnValidator validator = _appContext.GetRequiredService<tb_ReturnValidator>();
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
        public async Task<ReturnResults<tb_Return>> SaveOrUpdate(tb_Return entity)
        {
            ReturnResults<tb_Return> rr = new ReturnResults<tb_Return>();
            tb_Return Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_ReturnServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ReturnServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
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
            tb_Return entity = model as tb_Return;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MainID > 0)
                {
                    bool rs = await _tb_ReturnServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ReturnServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
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
            List<T> list = await _tb_ReturnServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Return entity = item as tb_Return;
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
            List<T> list = await _tb_ReturnServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Return entity = item as tb_Return;
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
            tb_Return entity = model as tb_Return;
            bool rs = await _tb_ReturnServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Return>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Return> entitys = models as List<tb_Return>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Return>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.MainID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Return>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ReturnValidator validator = new tb_ReturnValidator();
           tb_ReturnValidator validator = _appContext.GetRequiredService<tb_ReturnValidator>();
            ValidationResult results = validator.Validate(info as tb_Return);
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
            try
            {
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_Return entity = model as tb_Return;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MainID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Return>(entity as tb_Return)
                        .Include(m => m.tb_ReturnDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Return>(entity as tb_Return)
                .Include(m => m.tb_ReturnDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MainID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Return>()
                                .Includes(m => m.tb_ReturnDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Return entity = model as tb_Return;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Return>(m => m.MainID== entity.MainID)
                                .Include(m => m.tb_ReturnDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Return AddReEntity(tb_Return entity)
        {
            tb_Return AddEntity =  _tb_ReturnServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Return> AddReEntityAsync(tb_Return entity)
        {
            tb_Return AddEntity = await _tb_ReturnServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Return entity)
        {
            long id = await _tb_ReturnServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Return> infos)
        {
            List<long> ids = await _tb_ReturnServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Return>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Return entity)
        {
            bool rs = await _tb_ReturnServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Return>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Return entity)
        {
            bool rs = await _tb_ReturnServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ReturnServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Return>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ReturnServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Return>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Return>> QueryAsync()
        {
            List<tb_Return> list = await  _tb_ReturnServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        
        public virtual List<tb_Return> Query()
        {
            List<tb_Return> list =  _tb_ReturnServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        
        public virtual List<tb_Return> Query(string wheresql)
        {
            List<tb_Return> list =  _tb_ReturnServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Return>> QueryAsync(string wheresql) 
        {
            List<tb_Return> list = await _tb_ReturnServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Return>> QueryAsync(Expression<Func<tb_Return, bool>> exp)
        {
            List<tb_Return> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Return>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Return>> QueryByNavAsync()
        {
            List<tb_Return> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Return>()
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_ReturnDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Return>> QueryByNavAsync(Expression<Func<tb_Return, bool>> exp)
        {
            List<tb_Return> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Return>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_ReturnDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Return> QueryByNav(Expression<Func<tb_Return, bool>> exp)
        {
            List<tb_Return> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Return>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_ReturnDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Return>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Return>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Return>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ReturnServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Return entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Return>().Where(w => w.MainID == (long)id)
                             .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_ReturnDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Return>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



