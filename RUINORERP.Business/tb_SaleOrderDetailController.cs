
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:52
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
    /// 销售订单明细
    /// </summary>
    public partial class tb_SaleOrderDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_SaleOrderDetailServices _tb_SaleOrderDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_SaleOrderDetailController(ILogger<tb_SaleOrderDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_SaleOrderDetailServices tb_SaleOrderDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_SaleOrderDetailServices = tb_SaleOrderDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_SaleOrderDetail info)
        {

           // tb_SaleOrderDetailValidator validator = new tb_SaleOrderDetailValidator();
           tb_SaleOrderDetailValidator validator = _appContext.GetRequiredService<tb_SaleOrderDetailValidator>();
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
        public async Task<ReturnResults<tb_SaleOrderDetail>> SaveOrUpdate(tb_SaleOrderDetail entity)
        {
            ReturnResults<tb_SaleOrderDetail> rr = new ReturnResults<tb_SaleOrderDetail>();
            tb_SaleOrderDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SaleOrderDetail_ID > 0)
                {
                    bool rs = await _tb_SaleOrderDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_SaleOrderDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
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
            tb_SaleOrderDetail entity = model as tb_SaleOrderDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SaleOrderDetail_ID > 0)
                {
                    bool rs = await _tb_SaleOrderDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_SaleOrderDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
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
            List<T> list = await _tb_SaleOrderDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_SaleOrderDetail entity = item as tb_SaleOrderDetail;
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
            List<T> list = await _tb_SaleOrderDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_SaleOrderDetail entity = item as tb_SaleOrderDetail;
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
            tb_SaleOrderDetail entity = model as tb_SaleOrderDetail;
            bool rs = await _tb_SaleOrderDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrderDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_SaleOrderDetail> entitys = models as List<tb_SaleOrderDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_SaleOrderDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.SaleOrderDetail_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrderDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_SaleOrderDetailValidator validator = new tb_SaleOrderDetailValidator();
           tb_SaleOrderDetailValidator validator = _appContext.GetRequiredService<tb_SaleOrderDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_SaleOrderDetail);
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

                tb_SaleOrderDetail entity = model as tb_SaleOrderDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.SaleOrderDetail_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_SaleOrderDetail>(entity as tb_SaleOrderDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_SaleOrderDetail>(entity as tb_SaleOrderDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SaleOrderDetail_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_SaleOrderDetail entity = model as tb_SaleOrderDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_SaleOrderDetail>(m => m.SaleOrderDetail_ID== entity.SaleOrderDetail_ID)
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
        
        
        
        public tb_SaleOrderDetail AddReEntity(tb_SaleOrderDetail entity)
        {
            tb_SaleOrderDetail AddEntity =  _tb_SaleOrderDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_SaleOrderDetail> AddReEntityAsync(tb_SaleOrderDetail entity)
        {
            tb_SaleOrderDetail AddEntity = await _tb_SaleOrderDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_SaleOrderDetail entity)
        {
            long id = await _tb_SaleOrderDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_SaleOrderDetail> infos)
        {
            List<long> ids = await _tb_SaleOrderDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_SaleOrderDetail entity)
        {
            bool rs = await _tb_SaleOrderDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrderDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_SaleOrderDetail entity)
        {
            bool rs = await _tb_SaleOrderDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_SaleOrderDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrderDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_SaleOrderDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOrderDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_SaleOrderDetail>> QueryAsync()
        {
            List<tb_SaleOrderDetail> list = await  _tb_SaleOrderDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        
        public virtual List<tb_SaleOrderDetail> Query()
        {
            List<tb_SaleOrderDetail> list =  _tb_SaleOrderDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        
        public virtual List<tb_SaleOrderDetail> Query(string wheresql)
        {
            List<tb_SaleOrderDetail> list =  _tb_SaleOrderDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_SaleOrderDetail>> QueryAsync(string wheresql) 
        {
            List<tb_SaleOrderDetail> list = await _tb_SaleOrderDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_SaleOrderDetail>> QueryAsync(Expression<Func<tb_SaleOrderDetail, bool>> exp)
        {
            List<tb_SaleOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOrderDetail>> QueryByNavAsync()
        {
            List<tb_SaleOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_saleorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOrderDetail>> QueryByNavAsync(Expression<Func<tb_SaleOrderDetail, bool>> exp)
        {
            List<tb_SaleOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_saleorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_SaleOrderDetail> QueryByNav(Expression<Func<tb_SaleOrderDetail, bool>> exp)
        {
            List<tb_SaleOrderDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_saleorder )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_SaleOrderDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_SaleOrderDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_SaleOrderDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOrderDetail>().Where(w => w.SaleOrderDetail_ID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_saleorder )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOrderDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



