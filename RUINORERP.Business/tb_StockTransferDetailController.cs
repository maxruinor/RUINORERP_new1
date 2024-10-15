
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:37
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
    /// 调拨单明细
    /// </summary>
    public partial class tb_StockTransferDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StockTransferDetailServices _tb_StockTransferDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_StockTransferDetailController(ILogger<tb_StockTransferDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StockTransferDetailServices tb_StockTransferDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StockTransferDetailServices = tb_StockTransferDetailServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_StockTransferDetail info)
        {
            tb_StockTransferDetailValidator validator = new tb_StockTransferDetailValidator();
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
        public async Task<ReturnResults<tb_StockTransferDetail>> SaveOrUpdate(tb_StockTransferDetail entity)
        {
            ReturnResults<tb_StockTransferDetail> rr = new ReturnResults<tb_StockTransferDetail>();
            tb_StockTransferDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StockTransferDetaill_ID > 0)
                {
                    bool rs = await _tb_StockTransferDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StockTransferDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
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
            tb_StockTransferDetail entity = model as tb_StockTransferDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.StockTransferDetaill_ID > 0)
                {
                    bool rs = await _tb_StockTransferDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StockTransferDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
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
            List<T> list = await _tb_StockTransferDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StockTransferDetail entity = item as tb_StockTransferDetail;
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
            List<T> list = await _tb_StockTransferDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StockTransferDetail entity = item as tb_StockTransferDetail;
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
            tb_StockTransferDetail entity = model as tb_StockTransferDetail;
            bool rs = await _tb_StockTransferDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransferDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StockTransferDetail> entitys = models as List<tb_StockTransferDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StockTransferDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.StockTransferDetaill_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransferDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_StockTransferDetailValidator validator = new tb_StockTransferDetailValidator();
            ValidationResult results = validator.Validate(info as tb_StockTransferDetail);
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
                tb_StockTransferDetail entity = model as tb_StockTransferDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.StockTransferDetaill_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_StockTransferDetail>(entity as tb_StockTransferDetail)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_StockTransferDetail>(entity as tb_StockTransferDetail)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.StockTransferDetaill_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StockTransferDetail entity = model as tb_StockTransferDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StockTransferDetail>(m => m.StockTransferDetaill_ID== entity.StockTransferDetaill_ID)
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_StockTransferDetail AddReEntity(tb_StockTransferDetail entity)
        {
            tb_StockTransferDetail AddEntity =  _tb_StockTransferDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StockTransferDetail> AddReEntityAsync(tb_StockTransferDetail entity)
        {
            tb_StockTransferDetail AddEntity = await _tb_StockTransferDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StockTransferDetail entity)
        {
            long id = await _tb_StockTransferDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StockTransferDetail> infos)
        {
            List<long> ids = await _tb_StockTransferDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StockTransferDetail entity)
        {
            bool rs = await _tb_StockTransferDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransferDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StockTransferDetail entity)
        {
            bool rs = await _tb_StockTransferDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StockTransferDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransferDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StockTransferDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockTransferDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StockTransferDetail>> QueryAsync()
        {
            List<tb_StockTransferDetail> list = await  _tb_StockTransferDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        
        public virtual List<tb_StockTransferDetail> Query()
        {
            List<tb_StockTransferDetail> list =  _tb_StockTransferDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        
        public virtual List<tb_StockTransferDetail> Query(string wheresql)
        {
            List<tb_StockTransferDetail> list =  _tb_StockTransferDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StockTransferDetail>> QueryAsync(string wheresql) 
        {
            List<tb_StockTransferDetail> list = await _tb_StockTransferDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StockTransferDetail>> QueryAsync(Expression<Func<tb_StockTransferDetail, bool>> exp)
        {
            List<tb_StockTransferDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockTransferDetail>> QueryByNavAsync()
        {
            List<tb_StockTransferDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>()
                               .Includes(t => t.tb_stocktransfer )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockTransferDetail>> QueryByNavAsync(Expression<Func<tb_StockTransferDetail, bool>> exp)
        {
            List<tb_StockTransferDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>().Where(exp)
                               .Includes(t => t.tb_stocktransfer )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StockTransferDetail> QueryByNav(Expression<Func<tb_StockTransferDetail, bool>> exp)
        {
            List<tb_StockTransferDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>().Where(exp)
                            .Includes(t => t.tb_stocktransfer )
                            .Includes(t => t.tb_proddetail )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StockTransferDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StockTransferDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StockTransferDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockTransferDetail>().Where(w => w.StockTransferDetaill_ID == (long)id)
                             .Includes(t => t.tb_stocktransfer )
                            .Includes(t => t.tb_proddetail )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_StockTransferDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



