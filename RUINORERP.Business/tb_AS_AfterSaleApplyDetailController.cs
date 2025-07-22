
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:22
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
    /// 售后申请单明细
    /// </summary>
    public partial class tb_AS_AfterSaleApplyDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_AS_AfterSaleApplyDetailServices _tb_AS_AfterSaleApplyDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_AS_AfterSaleApplyDetailController(ILogger<tb_AS_AfterSaleApplyDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_AS_AfterSaleApplyDetailServices tb_AS_AfterSaleApplyDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_AS_AfterSaleApplyDetailServices = tb_AS_AfterSaleApplyDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_AS_AfterSaleApplyDetail info)
        {

           // tb_AS_AfterSaleApplyDetailValidator validator = new tb_AS_AfterSaleApplyDetailValidator();
           tb_AS_AfterSaleApplyDetailValidator validator = _appContext.GetRequiredService<tb_AS_AfterSaleApplyDetailValidator>();
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
        public async Task<ReturnResults<tb_AS_AfterSaleApplyDetail>> SaveOrUpdate(tb_AS_AfterSaleApplyDetail entity)
        {
            ReturnResults<tb_AS_AfterSaleApplyDetail> rr = new ReturnResults<tb_AS_AfterSaleApplyDetail>();
            tb_AS_AfterSaleApplyDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ASApplyDetailID > 0)
                {
                    bool rs = await _tb_AS_AfterSaleApplyDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_AS_AfterSaleApplyDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
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
            tb_AS_AfterSaleApplyDetail entity = model as tb_AS_AfterSaleApplyDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ASApplyDetailID > 0)
                {
                    bool rs = await _tb_AS_AfterSaleApplyDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_AS_AfterSaleApplyDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
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
            List<T> list = await _tb_AS_AfterSaleApplyDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_AS_AfterSaleApplyDetail entity = item as tb_AS_AfterSaleApplyDetail;
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
            List<T> list = await _tb_AS_AfterSaleApplyDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_AS_AfterSaleApplyDetail entity = item as tb_AS_AfterSaleApplyDetail;
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
            tb_AS_AfterSaleApplyDetail entity = model as tb_AS_AfterSaleApplyDetail;
            bool rs = await _tb_AS_AfterSaleApplyDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_AS_AfterSaleApplyDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_AS_AfterSaleApplyDetail> entitys = models as List<tb_AS_AfterSaleApplyDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_AS_AfterSaleApplyDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ASApplyDetailID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_AS_AfterSaleApplyDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_AS_AfterSaleApplyDetailValidator validator = new tb_AS_AfterSaleApplyDetailValidator();
           tb_AS_AfterSaleApplyDetailValidator validator = _appContext.GetRequiredService<tb_AS_AfterSaleApplyDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_AS_AfterSaleApplyDetail);
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

                tb_AS_AfterSaleApplyDetail entity = model as tb_AS_AfterSaleApplyDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ASApplyDetailID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_AS_AfterSaleApplyDetail>(entity as tb_AS_AfterSaleApplyDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_AS_AfterSaleApplyDetail>(entity as tb_AS_AfterSaleApplyDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ASApplyDetailID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_AS_AfterSaleApplyDetail entity = model as tb_AS_AfterSaleApplyDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_AS_AfterSaleApplyDetail>(m => m.ASApplyDetailID== entity.ASApplyDetailID)
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
        
        
        
        public tb_AS_AfterSaleApplyDetail AddReEntity(tb_AS_AfterSaleApplyDetail entity)
        {
            tb_AS_AfterSaleApplyDetail AddEntity =  _tb_AS_AfterSaleApplyDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_AS_AfterSaleApplyDetail> AddReEntityAsync(tb_AS_AfterSaleApplyDetail entity)
        {
            tb_AS_AfterSaleApplyDetail AddEntity = await _tb_AS_AfterSaleApplyDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_AS_AfterSaleApplyDetail entity)
        {
            long id = await _tb_AS_AfterSaleApplyDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_AS_AfterSaleApplyDetail> infos)
        {
            List<long> ids = await _tb_AS_AfterSaleApplyDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_AS_AfterSaleApplyDetail entity)
        {
            bool rs = await _tb_AS_AfterSaleApplyDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_AfterSaleApplyDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_AS_AfterSaleApplyDetail entity)
        {
            bool rs = await _tb_AS_AfterSaleApplyDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_AS_AfterSaleApplyDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_AfterSaleApplyDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_AS_AfterSaleApplyDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_AS_AfterSaleApplyDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_AS_AfterSaleApplyDetail>> QueryAsync()
        {
            List<tb_AS_AfterSaleApplyDetail> list = await  _tb_AS_AfterSaleApplyDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        
        public virtual List<tb_AS_AfterSaleApplyDetail> Query()
        {
            List<tb_AS_AfterSaleApplyDetail> list =  _tb_AS_AfterSaleApplyDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        
        public virtual List<tb_AS_AfterSaleApplyDetail> Query(string wheresql)
        {
            List<tb_AS_AfterSaleApplyDetail> list =  _tb_AS_AfterSaleApplyDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_AS_AfterSaleApplyDetail>> QueryAsync(string wheresql) 
        {
            List<tb_AS_AfterSaleApplyDetail> list = await _tb_AS_AfterSaleApplyDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_AS_AfterSaleApplyDetail>> QueryAsync(Expression<Func<tb_AS_AfterSaleApplyDetail, bool>> exp)
        {
            List<tb_AS_AfterSaleApplyDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_AfterSaleApplyDetail>> QueryByNavAsync()
        {
            List<tb_AS_AfterSaleApplyDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>()
                               .Includes(t => t.tb_as_aftersaleapply )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_AS_AfterSaleApplyDetail>> QueryByNavAsync(Expression<Func<tb_AS_AfterSaleApplyDetail, bool>> exp)
        {
            List<tb_AS_AfterSaleApplyDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>().Where(exp)
                               .Includes(t => t.tb_as_aftersaleapply )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_AS_AfterSaleApplyDetail> QueryByNav(Expression<Func<tb_AS_AfterSaleApplyDetail, bool>> exp)
        {
            List<tb_AS_AfterSaleApplyDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>().Where(exp)
                            .Includes(t => t.tb_as_aftersaleapply )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_AS_AfterSaleApplyDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_AS_AfterSaleApplyDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_AS_AfterSaleApplyDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_AS_AfterSaleApplyDetail>().Where(w => w.ASApplyDetailID == (long)id)
                             .Includes(t => t.tb_as_aftersaleapply )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_AS_AfterSaleApplyDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



