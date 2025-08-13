
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 14:43:12
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
    /// 库存流水表
    /// </summary>
    public partial class tb_InventoryTransactionController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_InventoryTransactionServices _tb_InventoryTransactionServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_InventoryTransactionController(ILogger<tb_InventoryTransactionController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_InventoryTransactionServices tb_InventoryTransactionServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_InventoryTransactionServices = tb_InventoryTransactionServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_InventoryTransaction info)
        {

           // tb_InventoryTransactionValidator validator = new tb_InventoryTransactionValidator();
           tb_InventoryTransactionValidator validator = _appContext.GetRequiredService<tb_InventoryTransactionValidator>();
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
        public async Task<ReturnResults<tb_InventoryTransaction>> SaveOrUpdate(tb_InventoryTransaction entity)
        {
            ReturnResults<tb_InventoryTransaction> rr = new ReturnResults<tb_InventoryTransaction>();
            tb_InventoryTransaction Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.TranID > 0)
                {
                    bool rs = await _tb_InventoryTransactionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_InventoryTransactionServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
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
            tb_InventoryTransaction entity = model as tb_InventoryTransaction;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.TranID > 0)
                {
                    bool rs = await _tb_InventoryTransactionServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_InventoryTransactionServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
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
            List<T> list = await _tb_InventoryTransactionServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_InventoryTransaction entity = item as tb_InventoryTransaction;
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
            List<T> list = await _tb_InventoryTransactionServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_InventoryTransaction entity = item as tb_InventoryTransaction;
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
            tb_InventoryTransaction entity = model as tb_InventoryTransaction;
            bool rs = await _tb_InventoryTransactionServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_InventoryTransaction>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_InventoryTransaction> entitys = models as List<tb_InventoryTransaction>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_InventoryTransaction>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.TranID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_InventoryTransaction>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_InventoryTransactionValidator validator = new tb_InventoryTransactionValidator();
           tb_InventoryTransactionValidator validator = _appContext.GetRequiredService<tb_InventoryTransactionValidator>();
            ValidationResult results = validator.Validate(info as tb_InventoryTransaction);
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

                tb_InventoryTransaction entity = model as tb_InventoryTransaction;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.TranID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_InventoryTransaction>(entity as tb_InventoryTransaction)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_InventoryTransaction>(entity as tb_InventoryTransaction)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.TranID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_InventoryTransaction entity = model as tb_InventoryTransaction;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_InventoryTransaction>(m => m.TranID== entity.TranID)
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
        
        
        
        public tb_InventoryTransaction AddReEntity(tb_InventoryTransaction entity)
        {
            tb_InventoryTransaction AddEntity =  _tb_InventoryTransactionServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_InventoryTransaction> AddReEntityAsync(tb_InventoryTransaction entity)
        {
            tb_InventoryTransaction AddEntity = await _tb_InventoryTransactionServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_InventoryTransaction entity)
        {
            long id = await _tb_InventoryTransactionServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_InventoryTransaction> infos)
        {
            List<long> ids = await _tb_InventoryTransactionServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_InventoryTransaction entity)
        {
            bool rs = await _tb_InventoryTransactionServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_InventoryTransaction>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_InventoryTransaction entity)
        {
            bool rs = await _tb_InventoryTransactionServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_InventoryTransactionServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_InventoryTransaction>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_InventoryTransactionServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_InventoryTransaction>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_InventoryTransaction>> QueryAsync()
        {
            List<tb_InventoryTransaction> list = await  _tb_InventoryTransactionServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        
        public virtual List<tb_InventoryTransaction> Query()
        {
            List<tb_InventoryTransaction> list =  _tb_InventoryTransactionServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        
        public virtual List<tb_InventoryTransaction> Query(string wheresql)
        {
            List<tb_InventoryTransaction> list =  _tb_InventoryTransactionServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        
        public virtual async Task<List<tb_InventoryTransaction>> QueryAsync(string wheresql) 
        {
            List<tb_InventoryTransaction> list = await _tb_InventoryTransactionServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_InventoryTransaction>> QueryAsync(Expression<Func<tb_InventoryTransaction, bool>> exp)
        {
            List<tb_InventoryTransaction> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_InventoryTransaction>> QueryByNavAsync()
        {
            List<tb_InventoryTransaction> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_InventoryTransaction>> QueryByNavAsync(Expression<Func<tb_InventoryTransaction, bool>> exp)
        {
            List<tb_InventoryTransaction> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_InventoryTransaction> QueryByNav(Expression<Func<tb_InventoryTransaction, bool>> exp)
        {
            List<tb_InventoryTransaction> list = _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_InventoryTransaction>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_InventoryTransactionServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_InventoryTransaction entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_InventoryTransaction>().Where(w => w.TranID == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_InventoryTransaction>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



