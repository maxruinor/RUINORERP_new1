
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:07
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
    /// 付款申请单-即为付款单
    /// </summary>
    public partial class tb_FM_PaymentController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PaymentServices _tb_FM_PaymentServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PaymentController(ILogger<tb_FM_PaymentController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PaymentServices tb_FM_PaymentServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PaymentServices = tb_FM_PaymentServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_Payment info)
        {

           // tb_FM_PaymentValidator validator = new tb_FM_PaymentValidator();
           tb_FM_PaymentValidator validator = _appContext.GetRequiredService<tb_FM_PaymentValidator>();
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
        public async Task<ReturnResults<tb_FM_Payment>> SaveOrUpdate(tb_FM_Payment entity)
        {
            ReturnResults<tb_FM_Payment> rr = new ReturnResults<tb_FM_Payment>();
            tb_FM_Payment Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PaymentID > 0)
                {
                    bool rs = await _tb_FM_PaymentServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
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
            tb_FM_Payment entity = model as tb_FM_Payment;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PaymentID > 0)
                {
                    bool rs = await _tb_FM_PaymentServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PaymentServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
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
            List<T> list = await _tb_FM_PaymentServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_Payment entity = item as tb_FM_Payment;
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
            List<T> list = await _tb_FM_PaymentServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_Payment entity = item as tb_FM_Payment;
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
            tb_FM_Payment entity = model as tb_FM_Payment;
            bool rs = await _tb_FM_PaymentServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Payment>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_Payment> entitys = models as List<tb_FM_Payment>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_Payment>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PaymentID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Payment>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PaymentValidator validator = new tb_FM_PaymentValidator();
           tb_FM_PaymentValidator validator = _appContext.GetRequiredService<tb_FM_PaymentValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_Payment);
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

                tb_FM_Payment entity = model as tb_FM_Payment;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PaymentID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_Payment>(entity as tb_FM_Payment)
                        .Include(m => m.tb_FM_PaymentDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_Payment>(entity as tb_FM_Payment)
                .Include(m => m.tb_FM_PaymentDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PaymentID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>()
                                .Includes(m => m.tb_FM_PaymentDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_Payment entity = model as tb_FM_Payment;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_Payment>(m => m.PaymentID== entity.PaymentID)
                                .Include(m => m.tb_FM_PaymentDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_Payment AddReEntity(tb_FM_Payment entity)
        {
            tb_FM_Payment AddEntity =  _tb_FM_PaymentServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_Payment> AddReEntityAsync(tb_FM_Payment entity)
        {
            tb_FM_Payment AddEntity = await _tb_FM_PaymentServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_Payment entity)
        {
            long id = await _tb_FM_PaymentServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_Payment> infos)
        {
            List<long> ids = await _tb_FM_PaymentServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_Payment entity)
        {
            bool rs = await _tb_FM_PaymentServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Payment>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_Payment entity)
        {
            bool rs = await _tb_FM_PaymentServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PaymentServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Payment>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PaymentServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Payment>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_Payment>> QueryAsync()
        {
            List<tb_FM_Payment> list = await  _tb_FM_PaymentServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        
        public virtual List<tb_FM_Payment> Query()
        {
            List<tb_FM_Payment> list =  _tb_FM_PaymentServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        
        public virtual List<tb_FM_Payment> Query(string wheresql)
        {
            List<tb_FM_Payment> list =  _tb_FM_PaymentServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_Payment>> QueryAsync(string wheresql) 
        {
            List<tb_FM_Payment> list = await _tb_FM_PaymentServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_Payment>> QueryAsync(Expression<Func<tb_FM_Payment, bool>> exp)
        {
            List<tb_FM_Payment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Payment>> QueryByNavAsync()
        {
            List<tb_FM_Payment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>()
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_payeeinfo )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_FM_PaymentDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Payment>> QueryByNavAsync(Expression<Func<tb_FM_Payment, bool>> exp)
        {
            List<tb_FM_Payment> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>().Where(exp)
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_payeeinfo )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_currency )
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_FM_PaymentDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_Payment> QueryByNav(Expression<Func<tb_FM_Payment, bool>> exp)
        {
            List<tb_FM_Payment> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>().Where(exp)
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_payeeinfo )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_FM_PaymentDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_Payment>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PaymentServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_Payment entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Payment>().Where(w => w.PaymentID == (long)id)
                             .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_payeeinfo )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_currency )
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_FM_PaymentDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_Payment>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



