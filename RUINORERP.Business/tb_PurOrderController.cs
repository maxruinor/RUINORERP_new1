
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据
    /// </summary>
    public partial class tb_PurOrderController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PurOrderServices _tb_PurOrderServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_PurOrderController(ILogger<tb_PurOrderController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PurOrderServices tb_PurOrderServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PurOrderServices = tb_PurOrderServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_PurOrder info)
        {

           // tb_PurOrderValidator validator = new tb_PurOrderValidator();
           tb_PurOrderValidator validator = _appContext.GetRequiredService<tb_PurOrderValidator>();
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
        public async Task<ReturnResults<tb_PurOrder>> SaveOrUpdate(tb_PurOrder entity)
        {
            ReturnResults<tb_PurOrder> rr = new ReturnResults<tb_PurOrder>();
            tb_PurOrder Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurOrder_ID > 0)
                {
                    bool rs = await _tb_PurOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PurOrderServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
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
            tb_PurOrder entity = model as tb_PurOrder;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurOrder_ID > 0)
                {
                    bool rs = await _tb_PurOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PurOrderServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
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
            List<T> list = await _tb_PurOrderServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PurOrder entity = item as tb_PurOrder;
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
            List<T> list = await _tb_PurOrderServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PurOrder entity = item as tb_PurOrder;
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
            tb_PurOrder entity = model as tb_PurOrder;
            bool rs = await _tb_PurOrderServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrder>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PurOrder> entitys = models as List<tb_PurOrder>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PurOrder>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PurOrder_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrder>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PurOrderValidator validator = new tb_PurOrderValidator();
           tb_PurOrderValidator validator = _appContext.GetRequiredService<tb_PurOrderValidator>();
            ValidationResult results = validator.Validate(info as tb_PurOrder);
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
                tb_PurOrder entity = model as tb_PurOrder;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PurOrder_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PurOrder>(entity as tb_PurOrder)
                        .Include(m => m.tb_PurEntries)
                    .Include(m => m.tb_PurOrderDetails)
                    .Include(m => m.tb_PurOrderRes)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PurOrder>(entity as tb_PurOrder)
                .Include(m => m.tb_PurEntries)
                .Include(m => m.tb_PurOrderDetails)
                .Include(m => m.tb_PurOrderRes)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PurOrder_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                                .Includes(m => m.tb_PurEntries)
                        .Includes(m => m.tb_PurOrderDetails)
                        .Includes(m => m.tb_PurOrderRes)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PurOrder entity = model as tb_PurOrder;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PurOrder>(m => m.PurOrder_ID== entity.PurOrder_ID)
                                .Include(m => m.tb_PurEntries)
                        .Include(m => m.tb_PurOrderDetails)
                        .Include(m => m.tb_PurOrderRes)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_PurOrder AddReEntity(tb_PurOrder entity)
        {
            tb_PurOrder AddEntity =  _tb_PurOrderServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PurOrder> AddReEntityAsync(tb_PurOrder entity)
        {
            tb_PurOrder AddEntity = await _tb_PurOrderServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PurOrder entity)
        {
            long id = await _tb_PurOrderServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PurOrder> infos)
        {
            List<long> ids = await _tb_PurOrderServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PurOrder entity)
        {
            bool rs = await _tb_PurOrderServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrder>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PurOrder entity)
        {
            bool rs = await _tb_PurOrderServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PurOrderServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrder>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PurOrderServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrder>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PurOrder>> QueryAsync()
        {
            List<tb_PurOrder> list = await  _tb_PurOrderServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        
        public virtual List<tb_PurOrder> Query()
        {
            List<tb_PurOrder> list =  _tb_PurOrderServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        
        public virtual List<tb_PurOrder> Query(string wheresql)
        {
            List<tb_PurOrder> list =  _tb_PurOrderServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PurOrder>> QueryAsync(string wheresql) 
        {
            List<tb_PurOrder> list = await _tb_PurOrderServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PurOrder>> QueryAsync(Expression<Func<tb_PurOrder, bool>> exp)
        {
            List<tb_PurOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurOrder>> QueryByNavAsync()
        {
            List<tb_PurOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_saleorder )
                               .Includes(t => t.tb_productiondemand )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_PurOrderDetails )
                                .Includes(t => t.tb_PurOrderRes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurOrder>> QueryByNavAsync(Expression<Func<tb_PurOrder, bool>> exp)
        {
            List<tb_PurOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_saleorder )
                               .Includes(t => t.tb_productiondemand )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_PurEntries )
                                .Includes(t => t.tb_PurOrderDetails )
                                .Includes(t => t.tb_PurOrderRes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PurOrder> QueryByNav(Expression<Func<tb_PurOrder, bool>> exp)
        {
            List<tb_PurOrder> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_saleorder )
                            .Includes(t => t.tb_productiondemand )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_PurOrderDetails )
                            .Includes(t => t.tb_PurOrderRes )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PurOrder>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PurOrderServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PurOrder entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrder>().Where(w => w.PurOrder_ID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_saleorder )
                            .Includes(t => t.tb_productiondemand )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_PurEntries )
                            .Includes(t => t.tb_PurOrderDetails )
                            .Includes(t => t.tb_PurOrderRes )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_PurOrder>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



