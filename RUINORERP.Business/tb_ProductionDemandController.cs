// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/06/2025 19:43:19
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
using RUINORERP.Business.Cache;

namespace RUINORERP.Business
{
    /// <summary>
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作
    /// </summary>
    public partial class tb_ProductionDemandController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProductionDemandServices _tb_ProductionDemandServices { get; set; }
        private readonly EventDrivenCacheManager _eventDrivenCacheManager; 
       // private readonly ApplicationContext _appContext;
       
        public tb_ProductionDemandController(ILogger<tb_ProductionDemandController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProductionDemandServices tb_ProductionDemandServices ,EventDrivenCacheManager eventDrivenCacheManager, ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProductionDemandServices = tb_ProductionDemandServices;
           _appContext = appContext;
           _eventDrivenCacheManager = eventDrivenCacheManager;
        }
      
        
        public ValidationResult Validator(tb_ProductionDemand info)
        {

           // tb_ProductionDemandValidator validator = new tb_ProductionDemandValidator();
           tb_ProductionDemandValidator validator = _appContext.GetRequiredService<tb_ProductionDemandValidator>();
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
        public async Task<ReturnResults<tb_ProductionDemand>> SaveOrUpdate(tb_ProductionDemand entity)
        {
            ReturnResults<tb_ProductionDemand> rr = new ReturnResults<tb_ProductionDemand>();
            tb_ProductionDemand Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PDID > 0)
                {
                    bool rs = await _tb_ProductionDemandServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProductionDemandServices.AddReEntityAsync(entity);
                    _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(entity);
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
            tb_ProductionDemand entity = model as tb_ProductionDemand;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PDID > 0)
                {
                    bool rs = await _tb_ProductionDemandServices.Update(entity);
                    if (rs)
                    {
                        _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProductionDemandServices.AddReEntityAsync(entity) as T ;
                    _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(entity);
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
            List<T> list = await _tb_ProductionDemandServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProductionDemand entity = item as tb_ProductionDemand;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        public async override Task<List<T>> BaseQueryAsync() 
        {
            List<T> list = await _tb_ProductionDemandServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProductionDemand entity = item as tb_ProductionDemand;
                entity.HasChanged = false;
            }
            if (list != null)
            {
                _eventDrivenCacheManager.UpdateEntityList<T>(list);
             }
            return list;
        }
        
        
        public async override Task<bool> BaseDeleteAsync(T model)
        {
            tb_ProductionDemand entity = model as tb_ProductionDemand;
            bool rs = await _tb_ProductionDemandServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                _eventDrivenCacheManager.DeleteEntity<tb_ProductionDemand>(entity.PrimaryKeyID);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProductionDemand> entitys = models as List<tb_ProductionDemand>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProductionDemand>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                _eventDrivenCacheManager.DeleteEntityList<tb_ProductionDemand>(entitys);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProductionDemandValidator validator = new tb_ProductionDemandValidator();
           tb_ProductionDemandValidator validator = _appContext.GetRequiredService<tb_ProductionDemandValidator>();
            ValidationResult results = validator.Validate(info as tb_ProductionDemand);
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

                tb_ProductionDemand entity = model as tb_ProductionDemand;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PDID > 0)
            {
            
                             rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProductionDemand>(entity as tb_ProductionDemand)
                        .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ProduceGoodsRecommendDetails)
                    .Include(m => m.tb_ProductionDemandDetails)
                    .Include(m => m.tb_ProductionDemandTargetDetails)
                    .Include(m => m.tb_PurGoodsRecommendDetails)
                    .ExecuteCommandAsync();
                 }
        else    
        {
                        rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProductionDemand>(entity as tb_ProductionDemand)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ProduceGoodsRecommendDetails)
                .Include(m => m.tb_ProductionDemandDetails)
                .Include(m => m.tb_ProductionDemandTargetDetails)
                .Include(m => m.tb_PurGoodsRecommendDetails)
         
                .ExecuteCommandAsync();
                                          
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PDID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                                .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ProduceGoodsRecommendDetails)
                        .Includes(m => m.tb_ProductionDemandDetails)
                        .Includes(m => m.tb_ProductionDemandTargetDetails)
                        .Includes(m => m.tb_PurGoodsRecommendDetails)
                                        .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProductionDemand entity = model as tb_ProductionDemand;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProductionDemand>(m => m.PDID== entity.PDID)
                                .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ProduceGoodsRecommendDetails)
                        .Include(m => m.tb_ProductionDemandDetails)
                        .Include(m => m.tb_ProductionDemandTargetDetails)
                        .Include(m => m.tb_PurGoodsRecommendDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                 _eventDrivenCacheManager.DeleteEntity<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProductionDemand AddReEntity(tb_ProductionDemand entity)
        {
            tb_ProductionDemand AddEntity =  _tb_ProductionDemandServices.AddReEntity(entity);
     
             _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProductionDemand> AddReEntityAsync(tb_ProductionDemand entity)
        {
            tb_ProductionDemand AddEntity = await _tb_ProductionDemandServices.AddReEntityAsync(entity);
            _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProductionDemand entity)
        {
            long id = await _tb_ProductionDemandServices.Add(entity);
            if(id>0)
            {
                 _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProductionDemand> infos)
        {
            List<long> ids = await _tb_ProductionDemandServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProductionDemand entity)
        {
            bool rs = await _tb_ProductionDemandServices.Delete(entity);
            if (rs)
            {
                _eventDrivenCacheManager.DeleteEntity<tb_ProductionDemand>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProductionDemand entity)
        {
            bool rs = await _tb_ProductionDemandServices.Update(entity);
            if (rs)
            {
                 _eventDrivenCacheManager.DeleteEntity<tb_ProductionDemand>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProductionDemandServices.DeleteById(id);
            if (rs)
            {
               _eventDrivenCacheManager.DeleteEntity<tb_ProductionDemand>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProductionDemandServices.DeleteByIds(ids);
            if (rs)
            {
            
                   _eventDrivenCacheManager.DeleteEntities<tb_ProductionDemand>(ids.Cast<object>().ToArray());
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProductionDemand>> QueryAsync()
        {
            List<tb_ProductionDemand> list = await  _tb_ProductionDemandServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        
        public virtual List<tb_ProductionDemand> Query()
        {
            List<tb_ProductionDemand> list =  _tb_ProductionDemandServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
    
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        
        public virtual List<tb_ProductionDemand> Query(string wheresql)
        {
            List<tb_ProductionDemand> list =  _tb_ProductionDemandServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProductionDemand>> QueryAsync(string wheresql) 
        {
            List<tb_ProductionDemand> list = await _tb_ProductionDemandServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProductionDemand>> QueryAsync(Expression<Func<tb_ProductionDemand, bool>> exp)
        {
            List<tb_ProductionDemand> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
   
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProductionDemand>> QueryByNavAsync()
        {
            List<tb_ProductionDemand> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_productionplan )
                                            .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
 
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProductionDemand>> QueryByNavAsync(Expression<Func<tb_ProductionDemand, bool>> exp)
        {
            List<tb_ProductionDemand> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_productionplan )
                                            .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
  
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProductionDemand> QueryByNav(Expression<Func<tb_ProductionDemand, bool>> exp)
        {
            List<tb_ProductionDemand> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_productionplan )
                                        .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
     
             _eventDrivenCacheManager.UpdateEntityList<tb_ProductionDemand>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProductionDemand>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProductionDemandServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProductionDemand entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemand>().Where(w => w.PDID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_productionplan )
                        

                                            .Includes(t => t.tb_ManufacturingOrders )
                                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                            .Includes(t => t.tb_ProductionDemandDetails )
                                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                                            .Includes(t => t.tb_PurGoodsRecommendDetails )
                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

         
             _eventDrivenCacheManager.UpdateEntity<tb_ProductionDemand>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



