
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:07
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
    /// 预付款单
    /// </summary>
    public partial class tb_FM_PrePayController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_PrePayServices _tb_FM_PrePayServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_PrePayController(ILogger<tb_FM_PrePayController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_PrePayServices tb_FM_PrePayServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_PrePayServices = tb_FM_PrePayServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_PrePay info)
        {

           // tb_FM_PrePayValidator validator = new tb_FM_PrePayValidator();
           tb_FM_PrePayValidator validator = _appContext.GetRequiredService<tb_FM_PrePayValidator>();
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
        public async Task<ReturnResults<tb_FM_PrePay>> SaveOrUpdate(tb_FM_PrePay entity)
        {
            ReturnResults<tb_FM_PrePay> rr = new ReturnResults<tb_FM_PrePay>();
            tb_FM_PrePay Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PrePayID > 0)
                {
                    bool rs = await _tb_FM_PrePayServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_PrePayServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
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
            tb_FM_PrePay entity = model as tb_FM_PrePay;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PrePayID > 0)
                {
                    bool rs = await _tb_FM_PrePayServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_PrePayServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
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
            List<T> list = await _tb_FM_PrePayServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_PrePay entity = item as tb_FM_PrePay;
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
            List<T> list = await _tb_FM_PrePayServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_PrePay entity = item as tb_FM_PrePay;
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
            tb_FM_PrePay entity = model as tb_FM_PrePay;
            bool rs = await _tb_FM_PrePayServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePay>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_PrePay> entitys = models as List<tb_FM_PrePay>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_PrePay>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PrePayID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePay>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_PrePayValidator validator = new tb_FM_PrePayValidator();
           tb_FM_PrePayValidator validator = _appContext.GetRequiredService<tb_FM_PrePayValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_PrePay);
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
                tb_FM_PrePay entity = model as tb_FM_PrePay;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PrePayID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_PrePay>(entity as tb_FM_PrePay)
                        .Include(m => m.tb_FM_PrePayDetails)
                    .Include(m => m.tb_FM_Payables)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_PrePay>(entity as tb_FM_PrePay)
                .Include(m => m.tb_FM_PrePayDetails)
                .Include(m => m.tb_FM_Payables)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PrePayID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>()
                                .Includes(m => m.tb_FM_PrePayDetails)
                        .Includes(m => m.tb_FM_Payables)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_PrePay entity = model as tb_FM_PrePay;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_PrePay>(m => m.PrePayID== entity.PrePayID)
                                .Include(m => m.tb_FM_PrePayDetails)
                        .Include(m => m.tb_FM_Payables)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_FM_PrePay AddReEntity(tb_FM_PrePay entity)
        {
            tb_FM_PrePay AddEntity =  _tb_FM_PrePayServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_PrePay> AddReEntityAsync(tb_FM_PrePay entity)
        {
            tb_FM_PrePay AddEntity = await _tb_FM_PrePayServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_PrePay entity)
        {
            long id = await _tb_FM_PrePayServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_PrePay> infos)
        {
            List<long> ids = await _tb_FM_PrePayServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_PrePay entity)
        {
            bool rs = await _tb_FM_PrePayServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePay>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_PrePay entity)
        {
            bool rs = await _tb_FM_PrePayServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_PrePayServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePay>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_PrePayServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_PrePay>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_PrePay>> QueryAsync()
        {
            List<tb_FM_PrePay> list = await  _tb_FM_PrePayServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        
        public virtual List<tb_FM_PrePay> Query()
        {
            List<tb_FM_PrePay> list =  _tb_FM_PrePayServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        
        public virtual List<tb_FM_PrePay> Query(string wheresql)
        {
            List<tb_FM_PrePay> list =  _tb_FM_PrePayServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_PrePay>> QueryAsync(string wheresql) 
        {
            List<tb_FM_PrePay> list = await _tb_FM_PrePayServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_PrePay>> QueryAsync(Expression<Func<tb_FM_PrePay, bool>> exp)
        {
            List<tb_FM_PrePay> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PrePay>> QueryByNavAsync()
        {
            List<tb_FM_PrePay> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_FM_PrePayDetails )
                                .Includes(t => t.tb_FM_Payables )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_PrePay>> QueryByNavAsync(Expression<Func<tb_FM_PrePay, bool>> exp)
        {
            List<tb_FM_PrePay> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                                            .Includes(t => t.tb_FM_PrePayDetails )
                                .Includes(t => t.tb_FM_Payables )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_PrePay> QueryByNav(Expression<Func<tb_FM_PrePay, bool>> exp)
        {
            List<tb_FM_PrePay> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_FM_PrePayDetails )
                            .Includes(t => t.tb_FM_Payables )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_PrePay>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_PrePayServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_PrePay entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_PrePay>().Where(w => w.PrePayID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                                        .Includes(t => t.tb_FM_PrePayDetails )
                            .Includes(t => t.tb_FM_Payables )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_PrePay>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



