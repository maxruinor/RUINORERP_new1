
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:01
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
    /// 先销售合同再订单,条款内容后面补充
    /// </summary>
    public partial class tb_ContractController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ContractServices _tb_ContractServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ContractController(ILogger<tb_ContractController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ContractServices tb_ContractServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ContractServices = tb_ContractServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Contract info)
        {

           // tb_ContractValidator validator = new tb_ContractValidator();
           tb_ContractValidator validator = _appContext.GetRequiredService<tb_ContractValidator>();
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
        public async Task<ReturnResults<tb_Contract>> SaveOrUpdate(tb_Contract entity)
        {
            ReturnResults<tb_Contract> rr = new ReturnResults<tb_Contract>();
            tb_Contract Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ContractID > 0)
                {
                    bool rs = await _tb_ContractServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ContractServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
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
            tb_Contract entity = model as tb_Contract;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ContractID > 0)
                {
                    bool rs = await _tb_ContractServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ContractServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
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
            List<T> list = await _tb_ContractServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Contract entity = item as tb_Contract;
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
            List<T> list = await _tb_ContractServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Contract entity = item as tb_Contract;
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
            tb_Contract entity = model as tb_Contract;
            bool rs = await _tb_ContractServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Contract>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Contract> entitys = models as List<tb_Contract>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Contract>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ContractID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Contract>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ContractValidator validator = new tb_ContractValidator();
           tb_ContractValidator validator = _appContext.GetRequiredService<tb_ContractValidator>();
            ValidationResult results = validator.Validate(info as tb_Contract);
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
                tb_Contract entity = model as tb_Contract;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ContractID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Contract>(entity as tb_Contract)
                        .Include(m => m.tb_ContractDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Contract>(entity as tb_Contract)
                .Include(m => m.tb_ContractDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ContractID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>()
                                .Includes(m => m.tb_ContractDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Contract entity = model as tb_Contract;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Contract>(m => m.ContractID== entity.ContractID)
                                .Include(m => m.tb_ContractDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_Contract AddReEntity(tb_Contract entity)
        {
            tb_Contract AddEntity =  _tb_ContractServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Contract> AddReEntityAsync(tb_Contract entity)
        {
            tb_Contract AddEntity = await _tb_ContractServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Contract entity)
        {
            long id = await _tb_ContractServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Contract> infos)
        {
            List<long> ids = await _tb_ContractServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Contract>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Contract entity)
        {
            bool rs = await _tb_ContractServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Contract>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Contract entity)
        {
            bool rs = await _tb_ContractServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ContractServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Contract>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ContractServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Contract>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Contract>> QueryAsync()
        {
            List<tb_Contract> list = await  _tb_ContractServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        
        public virtual List<tb_Contract> Query()
        {
            List<tb_Contract> list =  _tb_ContractServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        
        public virtual List<tb_Contract> Query(string wheresql)
        {
            List<tb_Contract> list =  _tb_ContractServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Contract>> QueryAsync(string wheresql) 
        {
            List<tb_Contract> list = await _tb_ContractServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Contract>> QueryAsync(Expression<Func<tb_Contract, bool>> exp)
        {
            List<tb_Contract> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Contract>> QueryByNavAsync()
        {
            List<tb_Contract> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>()
                               .Includes(t => t.tb_invoiceinfo )
                                            .Includes(t => t.tb_ContractDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Contract>> QueryByNavAsync(Expression<Func<tb_Contract, bool>> exp)
        {
            List<tb_Contract> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>().Where(exp)
                               .Includes(t => t.tb_invoiceinfo )
                                            .Includes(t => t.tb_ContractDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Contract> QueryByNav(Expression<Func<tb_Contract, bool>> exp)
        {
            List<tb_Contract> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>().Where(exp)
                            .Includes(t => t.tb_invoiceinfo )
                                        .Includes(t => t.tb_ContractDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Contract>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ContractServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Contract entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Contract>().Where(w => w.ContractID == (long)id)
                             .Includes(t => t.tb_invoiceinfo )
                                        .Includes(t => t.tb_ContractDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Contract>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



