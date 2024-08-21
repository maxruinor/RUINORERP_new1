
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:34
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
    /// 业务类型 报销，员工借支还款，运费
    /// </summary>
    public partial class tb_FM_ExpenseTypeController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_ExpenseTypeServices _tb_FM_ExpenseTypeServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_ExpenseTypeController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_ExpenseTypeServices tb_FM_ExpenseTypeServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_ExpenseTypeServices = tb_FM_ExpenseTypeServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_FM_ExpenseType info)
        {
            tb_FM_ExpenseTypeValidator validator = new tb_FM_ExpenseTypeValidator();
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
        public async Task<ReturnResults<tb_FM_ExpenseType>> SaveOrUpdate(tb_FM_ExpenseType entity)
        {
            ReturnResults<tb_FM_ExpenseType> rr = new ReturnResults<tb_FM_ExpenseType>();
            tb_FM_ExpenseType Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ExpenseType_id > 0)
                {
                    bool rs = await _tb_FM_ExpenseTypeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_ExpenseTypeServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
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
            tb_FM_ExpenseType entity = model as tb_FM_ExpenseType;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ExpenseType_id > 0)
                {
                    bool rs = await _tb_FM_ExpenseTypeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_ExpenseTypeServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
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
            List<T> list = await _tb_FM_ExpenseTypeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_ExpenseType entity = item as tb_FM_ExpenseType;
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
            List<T> list = await _tb_FM_ExpenseTypeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_ExpenseType entity = item as tb_FM_ExpenseType;
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
            tb_FM_ExpenseType entity = model as tb_FM_ExpenseType;
            bool rs = await _tb_FM_ExpenseTypeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ExpenseType>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_ExpenseType> entitys = models as List<tb_FM_ExpenseType>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_ExpenseType>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ExpenseType_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ExpenseType>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_FM_ExpenseTypeValidator validator = new tb_FM_ExpenseTypeValidator();
            ValidationResult results = validator.Validate(info as tb_FM_ExpenseType);
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
                tb_FM_ExpenseType entity = model as tb_FM_ExpenseType;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.ExpenseType_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_ExpenseType>(entity as tb_FM_ExpenseType)
                        .Include(m => m.tb_FM_OtherExpenseDetails)
                    .Include(m => m.tb_FM_Initial_PayAndReceivables)
                    .Include(m => m.tb_FM_ExpenseClaimDetails)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_ExpenseType>(entity as tb_FM_ExpenseType)
                .Include(m => m.tb_FM_OtherExpenseDetails)
                .Include(m => m.tb_FM_Initial_PayAndReceivables)
                .Include(m => m.tb_FM_ExpenseClaimDetails)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ExpenseType_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>()
                                .Includes(m => m.tb_FM_OtherExpenseDetails)
                        .Includes(m => m.tb_FM_Initial_PayAndReceivables)
                        .Includes(m => m.tb_FM_ExpenseClaimDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_ExpenseType entity = model as tb_FM_ExpenseType;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_ExpenseType>(m => m.ExpenseType_id== entity.ExpenseType_id)
                                .Include(m => m.tb_FM_OtherExpenseDetails)
                        .Include(m => m.tb_FM_Initial_PayAndReceivables)
                        .Include(m => m.tb_FM_ExpenseClaimDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_ExpenseType AddReEntity(tb_FM_ExpenseType entity)
        {
            tb_FM_ExpenseType AddEntity =  _tb_FM_ExpenseTypeServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_ExpenseType> AddReEntityAsync(tb_FM_ExpenseType entity)
        {
            tb_FM_ExpenseType AddEntity = await _tb_FM_ExpenseTypeServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_ExpenseType entity)
        {
            long id = await _tb_FM_ExpenseTypeServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_ExpenseType> infos)
        {
            List<long> ids = await _tb_FM_ExpenseTypeServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_ExpenseType entity)
        {
            bool rs = await _tb_FM_ExpenseTypeServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ExpenseType>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_ExpenseType entity)
        {
            bool rs = await _tb_FM_ExpenseTypeServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_ExpenseTypeServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ExpenseType>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_ExpenseTypeServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ExpenseType>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_ExpenseType>> QueryAsync()
        {
            List<tb_FM_ExpenseType> list = await  _tb_FM_ExpenseTypeServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        
        public virtual List<tb_FM_ExpenseType> Query()
        {
            List<tb_FM_ExpenseType> list =  _tb_FM_ExpenseTypeServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        
        public virtual List<tb_FM_ExpenseType> Query(string wheresql)
        {
            List<tb_FM_ExpenseType> list =  _tb_FM_ExpenseTypeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_ExpenseType>> QueryAsync(string wheresql) 
        {
            List<tb_FM_ExpenseType> list = await _tb_FM_ExpenseTypeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_ExpenseType>> QueryAsync(Expression<Func<tb_FM_ExpenseType, bool>> exp)
        {
            List<tb_FM_ExpenseType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_ExpenseType>> QueryByNavAsync()
        {
            List<tb_FM_ExpenseType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>()
                               .Includes(t => t.tb_fm_subject )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_ExpenseType>> QueryByNavAsync(Expression<Func<tb_FM_ExpenseType, bool>> exp)
        {
            List<tb_FM_ExpenseType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>().Where(exp)
                               .Includes(t => t.tb_fm_subject )
                                            .Includes(t => t.tb_FM_OtherExpenseDetails )
                                .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                                .Includes(t => t.tb_FM_ExpenseClaimDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_ExpenseType> QueryByNav(Expression<Func<tb_FM_ExpenseType, bool>> exp)
        {
            List<tb_FM_ExpenseType> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>().Where(exp)
                            .Includes(t => t.tb_fm_subject )
                                        .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_ExpenseType>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_ExpenseTypeServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_ExpenseType entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ExpenseType>().Where(w => w.ExpenseType_id == (long)id)
                             .Includes(t => t.tb_fm_subject )
                                        .Includes(t => t.tb_FM_OtherExpenseDetails )
                            .Includes(t => t.tb_FM_Initial_PayAndReceivables )
                            .Includes(t => t.tb_FM_ExpenseClaimDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_ExpenseType>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



