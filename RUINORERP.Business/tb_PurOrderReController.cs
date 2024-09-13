
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:23
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
    /// 采购退回单 当采购发生退回时，您可以将它记录在本系统的采购退回作业中。在实际业务中虽然发生了采购但实际货物却还未入库，采购退回作业可退回订金、退回数量处理。采购退回单可以由采购订单转入，也可以手动录入新增单据,一般没有金额变化的，可以直接作废采购单。有订单等才需要做退回
    /// </summary>
    public partial class tb_PurOrderReController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PurOrderReServices _tb_PurOrderReServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_PurOrderReController(ILogger<tb_PurOrderReController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PurOrderReServices tb_PurOrderReServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PurOrderReServices = tb_PurOrderReServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_PurOrderRe info)
        {
            tb_PurOrderReValidator validator = new tb_PurOrderReValidator();
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
        public async Task<ReturnResults<tb_PurOrderRe>> SaveOrUpdate(tb_PurOrderRe entity)
        {
            ReturnResults<tb_PurOrderRe> rr = new ReturnResults<tb_PurOrderRe>();
            tb_PurOrderRe Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurRetrunID > 0)
                {
                    bool rs = await _tb_PurOrderReServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PurOrderReServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
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
            tb_PurOrderRe entity = model as tb_PurOrderRe;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurRetrunID > 0)
                {
                    bool rs = await _tb_PurOrderReServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PurOrderReServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
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
            List<T> list = await _tb_PurOrderReServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PurOrderRe entity = item as tb_PurOrderRe;
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
            List<T> list = await _tb_PurOrderReServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PurOrderRe entity = item as tb_PurOrderRe;
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
            tb_PurOrderRe entity = model as tb_PurOrderRe;
            bool rs = await _tb_PurOrderReServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrderRe>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PurOrderRe> entitys = models as List<tb_PurOrderRe>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PurOrderRe>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PurRetrunID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrderRe>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_PurOrderReValidator validator = new tb_PurOrderReValidator();
            ValidationResult results = validator.Validate(info as tb_PurOrderRe);
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
                tb_PurOrderRe entity = model as tb_PurOrderRe;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PurRetrunID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PurOrderRe>(entity as tb_PurOrderRe)
                        .Include(m => m.tb_PurOrderReDetails)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PurOrderRe>(entity as tb_PurOrderRe)
                .Include(m => m.tb_PurOrderReDetails)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PurRetrunID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>()
                                .Includes(m => m.tb_PurOrderReDetails)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PurOrderRe entity = model as tb_PurOrderRe;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PurOrderRe>(m => m.PurRetrunID== entity.PurRetrunID)
                                .Include(m => m.tb_PurOrderReDetails)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_PurOrderRe AddReEntity(tb_PurOrderRe entity)
        {
            tb_PurOrderRe AddEntity =  _tb_PurOrderReServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PurOrderRe> AddReEntityAsync(tb_PurOrderRe entity)
        {
            tb_PurOrderRe AddEntity = await _tb_PurOrderReServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PurOrderRe entity)
        {
            long id = await _tb_PurOrderReServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PurOrderRe> infos)
        {
            List<long> ids = await _tb_PurOrderReServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PurOrderRe entity)
        {
            bool rs = await _tb_PurOrderReServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrderRe>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PurOrderRe entity)
        {
            bool rs = await _tb_PurOrderReServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PurOrderReServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrderRe>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PurOrderReServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurOrderRe>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PurOrderRe>> QueryAsync()
        {
            List<tb_PurOrderRe> list = await  _tb_PurOrderReServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        
        public virtual List<tb_PurOrderRe> Query()
        {
            List<tb_PurOrderRe> list =  _tb_PurOrderReServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        
        public virtual List<tb_PurOrderRe> Query(string wheresql)
        {
            List<tb_PurOrderRe> list =  _tb_PurOrderReServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PurOrderRe>> QueryAsync(string wheresql) 
        {
            List<tb_PurOrderRe> list = await _tb_PurOrderReServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PurOrderRe>> QueryAsync(Expression<Func<tb_PurOrderRe, bool>> exp)
        {
            List<tb_PurOrderRe> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurOrderRe>> QueryByNavAsync()
        {
            List<tb_PurOrderRe> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>()
                               .Includes(t => t.tb_purorder )
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_PurOrderReDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurOrderRe>> QueryByNavAsync(Expression<Func<tb_PurOrderRe, bool>> exp)
        {
            List<tb_PurOrderRe> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>().Where(exp)
                               .Includes(t => t.tb_purorder )
                               .Includes(t => t.tb_customervendor )
                                            .Includes(t => t.tb_PurOrderReDetails )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PurOrderRe> QueryByNav(Expression<Func<tb_PurOrderRe, bool>> exp)
        {
            List<tb_PurOrderRe> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>().Where(exp)
                            .Includes(t => t.tb_purorder )
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_PurOrderReDetails )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PurOrderRe>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PurOrderReServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PurOrderRe entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurOrderRe>().Where(w => w.PurRetrunID == (long)id)
                             .Includes(t => t.tb_purorder )
                            .Includes(t => t.tb_customervendor )
                                        .Includes(t => t.tb_PurOrderReDetails )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_PurOrderRe>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



