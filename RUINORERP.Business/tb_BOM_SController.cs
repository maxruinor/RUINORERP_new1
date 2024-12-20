
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:00
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。
    /// </summary>
    public partial class tb_BOM_SController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BOM_SServices _tb_BOM_SServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_BOM_SController(ILogger<tb_BOM_SController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_BOM_SServices tb_BOM_SServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_BOM_SServices = tb_BOM_SServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_BOM_S info)
        {

           // tb_BOM_SValidator validator = new tb_BOM_SValidator();
           tb_BOM_SValidator validator = _appContext.GetRequiredService<tb_BOM_SValidator>();
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
        public async Task<ReturnResults<tb_BOM_S>> SaveOrUpdate(tb_BOM_S entity)
        {
            ReturnResults<tb_BOM_S> rr = new ReturnResults<tb_BOM_S>();
            tb_BOM_S Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BOM_ID > 0)
                {
                    bool rs = await _tb_BOM_SServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BOM_SServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
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
            tb_BOM_S entity = model as tb_BOM_S;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BOM_ID > 0)
                {
                    bool rs = await _tb_BOM_SServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BOM_SServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
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
            List<T> list = await _tb_BOM_SServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BOM_S entity = item as tb_BOM_S;
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
            List<T> list = await _tb_BOM_SServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BOM_S entity = item as tb_BOM_S;
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
            tb_BOM_S entity = model as tb_BOM_S;
            bool rs = await _tb_BOM_SServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_S>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_BOM_S> entitys = models as List<tb_BOM_S>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BOM_S>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BOM_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_S>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_BOM_SValidator validator = new tb_BOM_SValidator();
           tb_BOM_SValidator validator = _appContext.GetRequiredService<tb_BOM_SValidator>();
            ValidationResult results = validator.Validate(info as tb_BOM_S);
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
                tb_BOM_S entity = model as tb_BOM_S;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.BOM_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BOM_S>(entity as tb_BOM_S)
                        .Include(m => m.tb_ProductionPlanDetails)
                    .Include(m => m.tb_ProduceGoodsRecommendDetails)
                    .Include(m => m.tb_ProdDetails)
                    .Include(m => m.tb_BOM_SDetailSecondaries)
                    .Include(m => m.tb_ProductionDemandDetails)
                    .Include(m => m.tb_BOM_SDetails)
                    .Include(m => m.tb_ProdSplits)
                    .Include(m => m.tb_ProductionDemandTargetDetails)
                    .Include(m => m.tb_ManufacturingOrders)
                    .Include(m => m.tb_ProdMerges)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BOM_S>(entity as tb_BOM_S)
                .Include(m => m.tb_ProductionPlanDetails)
                .Include(m => m.tb_ProduceGoodsRecommendDetails)
                .Include(m => m.tb_ProdDetails)
                .Include(m => m.tb_BOM_SDetailSecondaries)
                .Include(m => m.tb_ProductionDemandDetails)
                .Include(m => m.tb_BOM_SDetails)
                .Include(m => m.tb_ProdSplits)
                .Include(m => m.tb_ProductionDemandTargetDetails)
                .Include(m => m.tb_ManufacturingOrders)
                .Include(m => m.tb_ProdMerges)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BOM_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>()
                                .Includes(m => m.tb_ProductionPlanDetails)
                        .Includes(m => m.tb_ProduceGoodsRecommendDetails)
                        .Includes(m => m.tb_ProdDetails)
                        .Includes(m => m.tb_BOM_SDetailSecondaries)
                        .Includes(m => m.tb_ProductionDemandDetails)
                        .Includes(m => m.tb_BOM_SDetails)
                        .Includes(m => m.tb_ProdSplits)
                        .Includes(m => m.tb_ProductionDemandTargetDetails)
                        .Includes(m => m.tb_ManufacturingOrders)
                        .Includes(m => m.tb_ProdMerges)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_BOM_S entity = model as tb_BOM_S;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BOM_S>(m => m.BOM_ID== entity.BOM_ID)
                                .Include(m => m.tb_ProductionPlanDetails)
                        .Include(m => m.tb_ProduceGoodsRecommendDetails)
                        .Include(m => m.tb_ProdDetails)
                        .Include(m => m.tb_BOM_SDetailSecondaries)
                        .Include(m => m.tb_ProductionDemandDetails)
                        .Include(m => m.tb_BOM_SDetails)
                        .Include(m => m.tb_ProdSplits)
                        .Include(m => m.tb_ProductionDemandTargetDetails)
                        .Include(m => m.tb_ManufacturingOrders)
                        .Include(m => m.tb_ProdMerges)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_BOM_S AddReEntity(tb_BOM_S entity)
        {
            tb_BOM_S AddEntity =  _tb_BOM_SServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_BOM_S> AddReEntityAsync(tb_BOM_S entity)
        {
            tb_BOM_S AddEntity = await _tb_BOM_SServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_BOM_S entity)
        {
            long id = await _tb_BOM_SServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_BOM_S> infos)
        {
            List<long> ids = await _tb_BOM_SServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_BOM_S entity)
        {
            bool rs = await _tb_BOM_SServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_S>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_BOM_S entity)
        {
            bool rs = await _tb_BOM_SServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BOM_SServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_S>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BOM_SServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BOM_S>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_BOM_S>> QueryAsync()
        {
            List<tb_BOM_S> list = await  _tb_BOM_SServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        
        public virtual List<tb_BOM_S> Query()
        {
            List<tb_BOM_S> list =  _tb_BOM_SServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        
        public virtual List<tb_BOM_S> Query(string wheresql)
        {
            List<tb_BOM_S> list =  _tb_BOM_SServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        
        public virtual async Task<List<tb_BOM_S>> QueryAsync(string wheresql) 
        {
            List<tb_BOM_S> list = await _tb_BOM_SServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BOM_S>> QueryAsync(Expression<Func<tb_BOM_S, bool>> exp)
        {
            List<tb_BOM_S> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BOM_S>> QueryByNavAsync()
        {
            List<tb_BOM_S> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>()
                               .Includes(t => t.tb_files )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_bomconfighistory )
                                            .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_ProdDetails )
                                .Includes(t => t.tb_BOM_SDetailSecondaries )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_BOM_SDetails )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProdMerges )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BOM_S>> QueryByNavAsync(Expression<Func<tb_BOM_S, bool>> exp)
        {
            List<tb_BOM_S> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>().Where(exp)
                               .Includes(t => t.tb_files )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_bomconfighistory )
                                            .Includes(t => t.tb_ProductionPlanDetails )
                                .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                                .Includes(t => t.tb_ProdDetails )
                                .Includes(t => t.tb_BOM_SDetailSecondaries )
                                .Includes(t => t.tb_ProductionDemandDetails )
                                .Includes(t => t.tb_BOM_SDetails )
                                .Includes(t => t.tb_ProdSplits )
                                .Includes(t => t.tb_ProductionDemandTargetDetails )
                                .Includes(t => t.tb_ManufacturingOrders )
                                .Includes(t => t.tb_ProdMerges )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_BOM_S> QueryByNav(Expression<Func<tb_BOM_S, bool>> exp)
        {
            List<tb_BOM_S> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>().Where(exp)
                            .Includes(t => t.tb_files )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_bomconfighistory )
                                        .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_ProdDetails )
                            .Includes(t => t.tb_BOM_SDetailSecondaries )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_BOM_SDetails )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProdMerges )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BOM_S>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BOM_SServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BOM_S entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BOM_S>().Where(w => w.BOM_ID == (long)id)
                             .Includes(t => t.tb_files )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_bomconfighistory )
                                        .Includes(t => t.tb_ProductionPlanDetails )
                            .Includes(t => t.tb_ProduceGoodsRecommendDetails )
                            .Includes(t => t.tb_ProdDetails )
                            .Includes(t => t.tb_BOM_SDetailSecondaries )
                            .Includes(t => t.tb_ProductionDemandDetails )
                            .Includes(t => t.tb_BOM_SDetails )
                            .Includes(t => t.tb_ProdSplits )
                            .Includes(t => t.tb_ProductionDemandTargetDetails )
                            .Includes(t => t.tb_ManufacturingOrders )
                            .Includes(t => t.tb_ProdMerges )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_BOM_S>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



