
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:13
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
    /// 自制成品建议
    /// </summary>
    public partial class tb_ProduceGoodsRecommendDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProduceGoodsRecommendDetailServices _tb_ProduceGoodsRecommendDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProduceGoodsRecommendDetailController(ILogger<tb_ProduceGoodsRecommendDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProduceGoodsRecommendDetailServices tb_ProduceGoodsRecommendDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProduceGoodsRecommendDetailServices = tb_ProduceGoodsRecommendDetailServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_ProduceGoodsRecommendDetail info)
        {
            tb_ProduceGoodsRecommendDetailValidator validator = new tb_ProduceGoodsRecommendDetailValidator();
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
        public async Task<ReturnResults<tb_ProduceGoodsRecommendDetail>> SaveOrUpdate(tb_ProduceGoodsRecommendDetail entity)
        {
            ReturnResults<tb_ProduceGoodsRecommendDetail> rr = new ReturnResults<tb_ProduceGoodsRecommendDetail>();
            tb_ProduceGoodsRecommendDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PDCID > 0)
                {
                    bool rs = await _tb_ProduceGoodsRecommendDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProduceGoodsRecommendDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
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
            tb_ProduceGoodsRecommendDetail entity = model as tb_ProduceGoodsRecommendDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PDCID > 0)
                {
                    bool rs = await _tb_ProduceGoodsRecommendDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProduceGoodsRecommendDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
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
            List<T> list = await _tb_ProduceGoodsRecommendDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProduceGoodsRecommendDetail entity = item as tb_ProduceGoodsRecommendDetail;
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
            List<T> list = await _tb_ProduceGoodsRecommendDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProduceGoodsRecommendDetail entity = item as tb_ProduceGoodsRecommendDetail;
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
            tb_ProduceGoodsRecommendDetail entity = model as tb_ProduceGoodsRecommendDetail;
            bool rs = await _tb_ProduceGoodsRecommendDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceGoodsRecommendDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProduceGoodsRecommendDetail> entitys = models as List<tb_ProduceGoodsRecommendDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProduceGoodsRecommendDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PDCID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceGoodsRecommendDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_ProduceGoodsRecommendDetailValidator validator = new tb_ProduceGoodsRecommendDetailValidator();
            ValidationResult results = validator.Validate(info as tb_ProduceGoodsRecommendDetail);
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
                tb_ProduceGoodsRecommendDetail entity = model as tb_ProduceGoodsRecommendDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PDCID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProduceGoodsRecommendDetail>(entity as tb_ProduceGoodsRecommendDetail)
                        .Include(m => m.tb_ManufacturingOrders)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProduceGoodsRecommendDetail>(entity as tb_ProduceGoodsRecommendDetail)
                .Include(m => m.tb_ManufacturingOrders)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PDCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>()
                                .Includes(m => m.tb_ManufacturingOrders)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProduceGoodsRecommendDetail entity = model as tb_ProduceGoodsRecommendDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProduceGoodsRecommendDetail>(m => m.PDCID== entity.PDCID)
                                .Include(m => m.tb_ManufacturingOrders)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_ProduceGoodsRecommendDetail AddReEntity(tb_ProduceGoodsRecommendDetail entity)
        {
            tb_ProduceGoodsRecommendDetail AddEntity =  _tb_ProduceGoodsRecommendDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProduceGoodsRecommendDetail> AddReEntityAsync(tb_ProduceGoodsRecommendDetail entity)
        {
            tb_ProduceGoodsRecommendDetail AddEntity = await _tb_ProduceGoodsRecommendDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProduceGoodsRecommendDetail entity)
        {
            long id = await _tb_ProduceGoodsRecommendDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProduceGoodsRecommendDetail> infos)
        {
            List<long> ids = await _tb_ProduceGoodsRecommendDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProduceGoodsRecommendDetail entity)
        {
            bool rs = await _tb_ProduceGoodsRecommendDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceGoodsRecommendDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProduceGoodsRecommendDetail entity)
        {
            bool rs = await _tb_ProduceGoodsRecommendDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProduceGoodsRecommendDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceGoodsRecommendDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProduceGoodsRecommendDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProduceGoodsRecommendDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProduceGoodsRecommendDetail>> QueryAsync()
        {
            List<tb_ProduceGoodsRecommendDetail> list = await  _tb_ProduceGoodsRecommendDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProduceGoodsRecommendDetail> Query()
        {
            List<tb_ProduceGoodsRecommendDetail> list =  _tb_ProduceGoodsRecommendDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProduceGoodsRecommendDetail> Query(string wheresql)
        {
            List<tb_ProduceGoodsRecommendDetail> list =  _tb_ProduceGoodsRecommendDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProduceGoodsRecommendDetail>> QueryAsync(string wheresql) 
        {
            List<tb_ProduceGoodsRecommendDetail> list = await _tb_ProduceGoodsRecommendDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProduceGoodsRecommendDetail>> QueryAsync(Expression<Func<tb_ProduceGoodsRecommendDetail, bool>> exp)
        {
            List<tb_ProduceGoodsRecommendDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProduceGoodsRecommendDetail>> QueryByNavAsync()
        {
            List<tb_ProduceGoodsRecommendDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>()
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_productiondemand )
                                            .Includes(t => t.tb_ManufacturingOrders )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProduceGoodsRecommendDetail>> QueryByNavAsync(Expression<Func<tb_ProduceGoodsRecommendDetail, bool>> exp)
        {
            List<tb_ProduceGoodsRecommendDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>().Where(exp)
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_productiondemand )
                                            .Includes(t => t.tb_ManufacturingOrders )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProduceGoodsRecommendDetail> QueryByNav(Expression<Func<tb_ProduceGoodsRecommendDetail, bool>> exp)
        {
            List<tb_ProduceGoodsRecommendDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>().Where(exp)
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_productiondemand )
                                        .Includes(t => t.tb_ManufacturingOrders )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProduceGoodsRecommendDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProduceGoodsRecommendDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProduceGoodsRecommendDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProduceGoodsRecommendDetail>().Where(w => w.PDCID == (long)id)
                             .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_productiondemand )
                                        .Includes(t => t.tb_ManufacturingOrders )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProduceGoodsRecommendDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



