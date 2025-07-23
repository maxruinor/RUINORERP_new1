
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:52
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
    /// 销售出库单
    /// </summary>
    public partial class tb_SaleOutController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_SaleOutServices _tb_SaleOutServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_SaleOutController(ILogger<tb_SaleOutController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_SaleOutServices tb_SaleOutServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_SaleOutServices = tb_SaleOutServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_SaleOut info)
        {

           // tb_SaleOutValidator validator = new tb_SaleOutValidator();
           tb_SaleOutValidator validator = _appContext.GetRequiredService<tb_SaleOutValidator>();
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
        public async Task<ReturnResults<tb_SaleOut>> SaveOrUpdate(tb_SaleOut entity)
        {
            ReturnResults<tb_SaleOut> rr = new ReturnResults<tb_SaleOut>();
            tb_SaleOut Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SaleOut_MainID > 0)
                {
                    bool rs = await _tb_SaleOutServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_SaleOutServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
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
            tb_SaleOut entity = model as tb_SaleOut;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SaleOut_MainID > 0)
                {
                    bool rs = await _tb_SaleOutServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_SaleOutServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
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
            List<T> list = await _tb_SaleOutServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_SaleOut entity = item as tb_SaleOut;
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
            List<T> list = await _tb_SaleOutServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_SaleOut entity = item as tb_SaleOut;
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
            tb_SaleOut entity = model as tb_SaleOut;
            bool rs = await _tb_SaleOutServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOut>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_SaleOut> entitys = models as List<tb_SaleOut>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_SaleOut>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.SaleOut_MainID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOut>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_SaleOutValidator validator = new tb_SaleOutValidator();
           tb_SaleOutValidator validator = _appContext.GetRequiredService<tb_SaleOutValidator>();
            ValidationResult results = validator.Validate(info as tb_SaleOut);
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

                tb_SaleOut entity = model as tb_SaleOut;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.SaleOut_MainID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_SaleOut>(entity as tb_SaleOut)
                        .Include(m => m.tb_SaleOutDetails)
                    .Include(m => m.tb_SaleOutRes)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_SaleOut>(entity as tb_SaleOut)
                .Include(m => m.tb_SaleOutDetails)
                .Include(m => m.tb_SaleOutRes)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SaleOut_MainID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                                .Includes(m => m.tb_SaleOutDetails)
                        .Includes(m => m.tb_SaleOutRes)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_SaleOut entity = model as tb_SaleOut;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_SaleOut>(m => m.SaleOut_MainID== entity.SaleOut_MainID)
                                .Include(m => m.tb_SaleOutDetails)
                        .Include(m => m.tb_SaleOutRes)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_SaleOut AddReEntity(tb_SaleOut entity)
        {
            tb_SaleOut AddEntity =  _tb_SaleOutServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_SaleOut> AddReEntityAsync(tb_SaleOut entity)
        {
            tb_SaleOut AddEntity = await _tb_SaleOutServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_SaleOut entity)
        {
            long id = await _tb_SaleOutServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_SaleOut> infos)
        {
            List<long> ids = await _tb_SaleOutServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_SaleOut entity)
        {
            bool rs = await _tb_SaleOutServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOut>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_SaleOut entity)
        {
            bool rs = await _tb_SaleOutServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_SaleOutServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOut>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_SaleOutServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOut>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_SaleOut>> QueryAsync()
        {
            List<tb_SaleOut> list = await  _tb_SaleOutServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        
        public virtual List<tb_SaleOut> Query()
        {
            List<tb_SaleOut> list =  _tb_SaleOutServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        
        public virtual List<tb_SaleOut> Query(string wheresql)
        {
            List<tb_SaleOut> list =  _tb_SaleOutServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        
        public virtual async Task<List<tb_SaleOut>> QueryAsync(string wheresql) 
        {
            List<tb_SaleOut> list = await _tb_SaleOutServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_SaleOut>> QueryAsync(Expression<Func<tb_SaleOut, bool>> exp)
        {
            List<tb_SaleOut> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOut>> QueryByNavAsync()
        {
            List<tb_SaleOut> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>()
                                            .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_SaleOutRes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOut>> QueryByNavAsync(Expression<Func<tb_SaleOut, bool>> exp)
        {
            List<tb_SaleOut> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(exp)
                                            .Includes(t => t.tb_SaleOutDetails )
                                .Includes(t => t.tb_SaleOutRes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_SaleOut> QueryByNav(Expression<Func<tb_SaleOut, bool>> exp)
        {
            List<tb_SaleOut> list = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(exp)
                                        .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_SaleOutRes )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_SaleOut>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_SaleOutServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_SaleOut entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOut>().Where(w => w.SaleOut_MainID == (long)id)
                                         .Includes(t => t.tb_SaleOutDetails )
                            .Includes(t => t.tb_SaleOutRes )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOut>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



