
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:30
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
    /// 维修工单  工时费 材料费
    /// </summary>
    public partial class tb_AS_RepairOrderController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_AS_RepairOrderServices _tb_AS_RepairOrderServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_AS_RepairOrderController(ILogger<tb_AS_RepairOrderController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_AS_RepairOrderServices tb_AS_RepairOrderServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_AS_RepairOrderServices = tb_AS_RepairOrderServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_AS_RepairOrder info)
        {

           // tb_AS_RepairOrderValidator validator = new tb_AS_RepairOrderValidator();
           tb_AS_RepairOrderValidator validator = _appContext.GetRequiredService<tb_AS_RepairOrderValidator>();
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
        public async Task<ReturnResults<tb_AS_RepairOrder>> SaveOrUpdate(tb_AS_RepairOrder entity)
        {
            ReturnResults<tb_AS_RepairOrder> rr = new ReturnResults<tb_AS_RepairOrder>();
            tb_AS_RepairOrder Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairOrderID > 0)
                {
                    bool rs = await _tb_AS_RepairOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairOrderServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
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
            tb_AS_RepairOrder entity = model as tb_AS_RepairOrder;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.RepairOrderID > 0)
                {
                    bool rs = await _tb_AS_RepairOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_AS_RepairOrderServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
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
            List<T> list = await _tb_AS_RepairOrderServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairOrder entity = item as tb_AS_RepairOrder;
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
            List<T> list = await _tb_AS_RepairOrderServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_AS_RepairOrder entity = item as tb_AS_RepairOrder;
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
            tb_AS_RepairOrder entity = model as tb_AS_RepairOrder;
            bool rs = await _tb_AS_RepairOrderServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrder>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_AS_RepairOrder> entitys = models as List<tb_AS_RepairOrder>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_AS_RepairOrder>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.RepairOrderID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrder>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_AS_RepairOrderValidator validator = new tb_AS_RepairOrderValidator();
           tb_AS_RepairOrderValidator validator = _appContext.GetRequiredService<tb_AS_RepairOrderValidator>();
            ValidationResult results = validator.Validate(info as tb_AS_RepairOrder);
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

                tb_AS_RepairOrder entity = model as tb_AS_RepairOrder;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.RepairOrderID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_AS_RepairOrder>(entity as tb_AS_RepairOrder)
                        .Include(m => m.tb_AS_RepairInStocks)
                    .Include(m => m.tb_AS_RepairOrderDetails)
                    .Include(m => m.tb_AS_RepairOrderMaterialDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_AS_RepairOrder>(entity as tb_AS_RepairOrder)
                .Include(m => m.tb_AS_RepairInStocks)
                .Include(m => m.tb_AS_RepairOrderDetails)
                .Include(m => m.tb_AS_RepairOrderMaterialDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.RepairOrderID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>()
                                .Includes(m => m.tb_AS_RepairInStocks)
                        .Includes(m => m.tb_AS_RepairOrderDetails)
                        .Includes(m => m.tb_AS_RepairOrderMaterialDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_AS_RepairOrder entity = model as tb_AS_RepairOrder;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_AS_RepairOrder>(m => m.RepairOrderID== entity.RepairOrderID)
                                .Include(m => m.tb_AS_RepairInStocks)
                        .Include(m => m.tb_AS_RepairOrderDetails)
                        .Include(m => m.tb_AS_RepairOrderMaterialDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_AS_RepairOrder AddReEntity(tb_AS_RepairOrder entity)
        {
            tb_AS_RepairOrder AddEntity =  _tb_AS_RepairOrderServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_AS_RepairOrder> AddReEntityAsync(tb_AS_RepairOrder entity)
        {
            tb_AS_RepairOrder AddEntity = await _tb_AS_RepairOrderServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_AS_RepairOrder entity)
        {
            long id = await _tb_AS_RepairOrderServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_AS_RepairOrder> infos)
        {
            List<long> ids = await _tb_AS_RepairOrderServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_AS_RepairOrder entity)
        {
            bool rs = await _tb_AS_RepairOrderServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrder>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_AS_RepairOrder entity)
        {
            bool rs = await _tb_AS_RepairOrderServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_AS_RepairOrderServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrder>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_AS_RepairOrderServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_RepairOrder>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_AS_RepairOrder>> QueryAsync()
        {
            List<tb_AS_RepairOrder> list = await  _tb_AS_RepairOrderServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairOrder> Query()
        {
            List<tb_AS_RepairOrder> list =  _tb_AS_RepairOrderServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        
        public virtual List<tb_AS_RepairOrder> Query(string wheresql)
        {
            List<tb_AS_RepairOrder> list =  _tb_AS_RepairOrderServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        
        public virtual async Task<List<tb_AS_RepairOrder>> QueryAsync(string wheresql) 
        {
            List<tb_AS_RepairOrder> list = await _tb_AS_RepairOrderServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairOrder>> QueryAsync(Expression<Func<tb_AS_RepairOrder, bool>> exp)
        {
            List<tb_AS_RepairOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairOrder>> QueryByNavAsync()
        {
            List<tb_AS_RepairOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_as_aftersaleapply )
                                            .Includes(t => t.tb_AS_RepairInStocks )
                                .Includes(t => t.tb_AS_RepairOrderDetails )
                                .Includes(t => t.tb_AS_RepairOrderMaterialDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_RepairOrder>> QueryByNavAsync(Expression<Func<tb_AS_RepairOrder, bool>> exp)
        {
            List<tb_AS_RepairOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_as_aftersaleapply )
                                            .Includes(t => t.tb_AS_RepairInStocks )
                                .Includes(t => t.tb_AS_RepairOrderDetails )
                                .Includes(t => t.tb_AS_RepairOrderMaterialDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_AS_RepairOrder> QueryByNav(Expression<Func<tb_AS_RepairOrder, bool>> exp)
        {
            List<tb_AS_RepairOrder> list = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_as_aftersaleapply )
                                        .Includes(t => t.tb_AS_RepairInStocks )
                            .Includes(t => t.tb_AS_RepairOrderDetails )
                            .Includes(t => t.tb_AS_RepairOrderMaterialDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_AS_RepairOrder>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_AS_RepairOrderServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_AS_RepairOrder entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_RepairOrder>().Where(w => w.RepairOrderID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_as_aftersaleapply )
                                        .Includes(t => t.tb_AS_RepairInStocks )
                            .Includes(t => t.tb_AS_RepairOrderDetails )
                            .Includes(t => t.tb_AS_RepairOrderMaterialDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_AS_RepairOrder>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



