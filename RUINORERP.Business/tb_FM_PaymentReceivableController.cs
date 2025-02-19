
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:04
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
    /// 应收款单
    /// </summary>
    public partial class tb_FM_PaymentReceivableController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PaymentReceivableServices _tb_FM_PaymentReceivableServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PaymentReceivableController(ILogger<tb_FM_PaymentReceivableController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PaymentReceivableServices tb_FM_PaymentReceivableServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PaymentReceivableServices = tb_FM_PaymentReceivableServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_PaymentReceivable info)
        {

           // tb_FM_PaymentReceivableValidator validator = new tb_FM_PaymentReceivableValidator();
           tb_FM_PaymentReceivableValidator validator = _appContext.GetRequiredService<tb_FM_PaymentReceivableValidator>();
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
        public async Task<ReturnResults<tb_FM_PaymentReceivable>> SaveOrUpdate(tb_FM_PaymentReceivable entity)
        {
            ReturnResults<tb_FM_PaymentReceivable> rr = new ReturnResults<tb_FM_PaymentReceivable>();
            tb_FM_PaymentReceivable Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PaymentReceivableID > 0)
                {
                    bool rs = await _tb_FM_PaymentReceivableServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentReceivableServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
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
            tb_FM_PaymentReceivable entity = model as tb_FM_PaymentReceivable;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PaymentReceivableID > 0)
                {
                    bool rs = await _tb_FM_PaymentReceivableServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentReceivableServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
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
            List<T> list = await _tb_FM_PaymentReceivableServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PaymentReceivable entity = item as tb_FM_PaymentReceivable;
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
            List<T> list = await _tb_FM_PaymentReceivableServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PaymentReceivable entity = item as tb_FM_PaymentReceivable;
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
            tb_FM_PaymentReceivable entity = model as tb_FM_PaymentReceivable;
            bool rs = await _tb_FM_PaymentReceivableServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentReceivable>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PaymentReceivable> entitys = models as List<tb_FM_PaymentReceivable>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PaymentReceivable>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PaymentReceivableID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentReceivable>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PaymentReceivableValidator validator = new tb_FM_PaymentReceivableValidator();
           tb_FM_PaymentReceivableValidator validator = _appContext.GetRequiredService<tb_FM_PaymentReceivableValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PaymentReceivable);
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
                tb_FM_PaymentReceivable entity = model as tb_FM_PaymentReceivable;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PaymentReceivableID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PaymentReceivable>(entity as tb_FM_PaymentReceivable)
                        .Include(m => m.tb_FM_PaymentReceivableDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PaymentReceivable>(entity as tb_FM_PaymentReceivable)
                .Include(m => m.tb_FM_PaymentReceivableDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PaymentReceivableID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>()
                                .Includes(m => m.tb_FM_PaymentReceivableDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PaymentReceivable entity = model as tb_FM_PaymentReceivable;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PaymentReceivable>(m => m.PaymentReceivableID== entity.PaymentReceivableID)
                                .Include(m => m.tb_FM_PaymentReceivableDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_PaymentReceivable AddReEntity(tb_FM_PaymentReceivable entity)
        {
            tb_FM_PaymentReceivable AddEntity =  _tb_FM_PaymentReceivableServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PaymentReceivable> AddReEntityAsync(tb_FM_PaymentReceivable entity)
        {
            tb_FM_PaymentReceivable AddEntity = await _tb_FM_PaymentReceivableServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PaymentReceivable entity)
        {
            long id = await _tb_FM_PaymentReceivableServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PaymentReceivable> infos)
        {
            List<long> ids = await _tb_FM_PaymentReceivableServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PaymentReceivable entity)
        {
            bool rs = await _tb_FM_PaymentReceivableServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentReceivable>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PaymentReceivable entity)
        {
            bool rs = await _tb_FM_PaymentReceivableServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PaymentReceivableServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentReceivable>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PaymentReceivableServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PaymentReceivable>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PaymentReceivable>> QueryAsync()
        {
            List<tb_FM_PaymentReceivable> list = await  _tb_FM_PaymentReceivableServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        
        public virtual List<tb_FM_PaymentReceivable> Query()
        {
            List<tb_FM_PaymentReceivable> list =  _tb_FM_PaymentReceivableServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        
        public virtual List<tb_FM_PaymentReceivable> Query(string wheresql)
        {
            List<tb_FM_PaymentReceivable> list =  _tb_FM_PaymentReceivableServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PaymentReceivable>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PaymentReceivable> list = await _tb_FM_PaymentReceivableServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentReceivable>> QueryAsync(Expression<Func<tb_FM_PaymentReceivable, bool>> exp)
        {
            List<tb_FM_PaymentReceivable> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PaymentReceivable>> QueryByNavAsync()
        {
            List<tb_FM_PaymentReceivable> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>()
                               .Includes(t => t.tb_fm_prepaymentreceipt )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_PaymentReceivableDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PaymentReceivable>> QueryByNavAsync(Expression<Func<tb_FM_PaymentReceivable, bool>> exp)
        {
            List<tb_FM_PaymentReceivable> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>().Where(exp)
                               .Includes(t => t.tb_fm_prepaymentreceipt )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_FM_PaymentReceivableDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PaymentReceivable> QueryByNav(Expression<Func<tb_FM_PaymentReceivable, bool>> exp)
        {
            List<tb_FM_PaymentReceivable> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>().Where(exp)
                            .Includes(t => t.tb_fm_prepaymentreceipt )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_PaymentReceivableDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PaymentReceivable>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PaymentReceivableServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PaymentReceivable entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PaymentReceivable>().Where(w => w.PaymentReceivableID == (long)id)
                             .Includes(t => t.tb_fm_prepaymentreceipt )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_FM_PaymentReceivableDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_PaymentReceivable>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



