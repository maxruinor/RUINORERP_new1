
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:06
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
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960
    /// </summary>
    public partial class tb_ManufacturingOrderController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ManufacturingOrderServices _tb_ManufacturingOrderServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ManufacturingOrderController(ILogger<tb_ManufacturingOrderController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ManufacturingOrderServices tb_ManufacturingOrderServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ManufacturingOrderServices = tb_ManufacturingOrderServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ManufacturingOrder info)
        {

           // tb_ManufacturingOrderValidator validator = new tb_ManufacturingOrderValidator();
           tb_ManufacturingOrderValidator validator = _appContext.GetRequiredService<tb_ManufacturingOrderValidator>();
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
        public async Task<ReturnResults<tb_ManufacturingOrder>> SaveOrUpdate(tb_ManufacturingOrder entity)
        {
            ReturnResults<tb_ManufacturingOrder> rr = new ReturnResults<tb_ManufacturingOrder>();
            tb_ManufacturingOrder Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MOID > 0)
                {
                    bool rs = await _tb_ManufacturingOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ManufacturingOrderServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
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
            tb_ManufacturingOrder entity = model as tb_ManufacturingOrder;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MOID > 0)
                {
                    bool rs = await _tb_ManufacturingOrderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ManufacturingOrderServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
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
            List<T> list = await _tb_ManufacturingOrderServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ManufacturingOrder entity = item as tb_ManufacturingOrder;
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
            List<T> list = await _tb_ManufacturingOrderServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ManufacturingOrder entity = item as tb_ManufacturingOrder;
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
            tb_ManufacturingOrder entity = model as tb_ManufacturingOrder;
            bool rs = await _tb_ManufacturingOrderServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrder>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ManufacturingOrder> entitys = models as List<tb_ManufacturingOrder>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ManufacturingOrder>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.MOID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrder>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ManufacturingOrderValidator validator = new tb_ManufacturingOrderValidator();
           tb_ManufacturingOrderValidator validator = _appContext.GetRequiredService<tb_ManufacturingOrderValidator>();
            ValidationResult results = validator.Validate(info as tb_ManufacturingOrder);
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
                tb_ManufacturingOrder entity = model as tb_ManufacturingOrder;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MOID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ManufacturingOrder>(entity as tb_ManufacturingOrder)
                        .Include(m => m.tb_MaterialRequisitions)
                    .Include(m => m.tb_FinishedGoodsInvs)
                    .Include(m => m.tb_ManufacturingOrderDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ManufacturingOrder>(entity as tb_ManufacturingOrder)
                .Include(m => m.tb_MaterialRequisitions)
                .Include(m => m.tb_FinishedGoodsInvs)
                .Include(m => m.tb_ManufacturingOrderDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MOID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                                .Includes(m => m.tb_MaterialRequisitions)
                        .Includes(m => m.tb_FinishedGoodsInvs)
                        .Includes(m => m.tb_ManufacturingOrderDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ManufacturingOrder entity = model as tb_ManufacturingOrder;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ManufacturingOrder>(m => m.MOID== entity.MOID)
                                .Include(m => m.tb_MaterialRequisitions)
                        .Include(m => m.tb_FinishedGoodsInvs)
                        .Include(m => m.tb_ManufacturingOrderDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ManufacturingOrder AddReEntity(tb_ManufacturingOrder entity)
        {
            tb_ManufacturingOrder AddEntity =  _tb_ManufacturingOrderServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ManufacturingOrder> AddReEntityAsync(tb_ManufacturingOrder entity)
        {
            tb_ManufacturingOrder AddEntity = await _tb_ManufacturingOrderServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ManufacturingOrder entity)
        {
            long id = await _tb_ManufacturingOrderServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ManufacturingOrder> infos)
        {
            List<long> ids = await _tb_ManufacturingOrderServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ManufacturingOrder entity)
        {
            bool rs = await _tb_ManufacturingOrderServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrder>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ManufacturingOrder entity)
        {
            bool rs = await _tb_ManufacturingOrderServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ManufacturingOrderServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrder>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ManufacturingOrderServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrder>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ManufacturingOrder>> QueryAsync()
        {
            List<tb_ManufacturingOrder> list = await  _tb_ManufacturingOrderServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        
        public virtual List<tb_ManufacturingOrder> Query()
        {
            List<tb_ManufacturingOrder> list =  _tb_ManufacturingOrderServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        
        public virtual List<tb_ManufacturingOrder> Query(string wheresql)
        {
            List<tb_ManufacturingOrder> list =  _tb_ManufacturingOrderServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ManufacturingOrder>> QueryAsync(string wheresql) 
        {
            List<tb_ManufacturingOrder> list = await _tb_ManufacturingOrderServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ManufacturingOrder>> QueryAsync(Expression<Func<tb_ManufacturingOrder, bool>> exp)
        {
            List<tb_ManufacturingOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ManufacturingOrder>> QueryByNavAsync()
        {
            List<tb_ManufacturingOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_producegoodsrecommenddetail )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_productiondemand )
                               .Includes(t => t.tb_producttype )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_ManufacturingOrderDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ManufacturingOrder>> QueryByNavAsync(Expression<Func<tb_ManufacturingOrder, bool>> exp)
        {
            List<tb_ManufacturingOrder> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_producegoodsrecommenddetail )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_productiondemand )
                               .Includes(t => t.tb_producttype )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_employee )
                                            .Includes(t => t.tb_MaterialRequisitions )
                                .Includes(t => t.tb_FinishedGoodsInvs )
                                .Includes(t => t.tb_ManufacturingOrderDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ManufacturingOrder> QueryByNav(Expression<Func<tb_ManufacturingOrder, bool>> exp)
        {
            List<tb_ManufacturingOrder> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_producegoodsrecommenddetail )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_productiondemand )
                            .Includes(t => t.tb_producttype )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_ManufacturingOrderDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ManufacturingOrder>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ManufacturingOrderServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ManufacturingOrder entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrder>().Where(w => w.MOID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_producegoodsrecommenddetail )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_productiondemand )
                            .Includes(t => t.tb_producttype )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_employee )
                                        .Includes(t => t.tb_MaterialRequisitions )
                            .Includes(t => t.tb_FinishedGoodsInvs )
                            .Includes(t => t.tb_ManufacturingOrderDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrder>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



