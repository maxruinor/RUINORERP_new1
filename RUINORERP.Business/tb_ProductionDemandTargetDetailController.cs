
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:49
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
    /// 生产需求分析目标对象明细
    /// </summary>
    public partial class tb_ProductionDemandTargetDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProductionDemandTargetDetailServices _tb_ProductionDemandTargetDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProductionDemandTargetDetailController(ILogger<tb_ProductionDemandTargetDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProductionDemandTargetDetailServices tb_ProductionDemandTargetDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProductionDemandTargetDetailServices = tb_ProductionDemandTargetDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProductionDemandTargetDetail info)
        {

           // tb_ProductionDemandTargetDetailValidator validator = new tb_ProductionDemandTargetDetailValidator();
           tb_ProductionDemandTargetDetailValidator validator = _appContext.GetRequiredService<tb_ProductionDemandTargetDetailValidator>();
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
        public async Task<ReturnResults<tb_ProductionDemandTargetDetail>> SaveOrUpdate(tb_ProductionDemandTargetDetail entity)
        {
            ReturnResults<tb_ProductionDemandTargetDetail> rr = new ReturnResults<tb_ProductionDemandTargetDetail>();
            tb_ProductionDemandTargetDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PDTCID > 0)
                {
                    bool rs = await _tb_ProductionDemandTargetDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProductionDemandTargetDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
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
            tb_ProductionDemandTargetDetail entity = model as tb_ProductionDemandTargetDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PDTCID > 0)
                {
                    bool rs = await _tb_ProductionDemandTargetDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProductionDemandTargetDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
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
            List<T> list = await _tb_ProductionDemandTargetDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProductionDemandTargetDetail entity = item as tb_ProductionDemandTargetDetail;
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
            List<T> list = await _tb_ProductionDemandTargetDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProductionDemandTargetDetail entity = item as tb_ProductionDemandTargetDetail;
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
            tb_ProductionDemandTargetDetail entity = model as tb_ProductionDemandTargetDetail;
            bool rs = await _tb_ProductionDemandTargetDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProductionDemandTargetDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProductionDemandTargetDetail> entitys = models as List<tb_ProductionDemandTargetDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProductionDemandTargetDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PDTCID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProductionDemandTargetDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProductionDemandTargetDetailValidator validator = new tb_ProductionDemandTargetDetailValidator();
           tb_ProductionDemandTargetDetailValidator validator = _appContext.GetRequiredService<tb_ProductionDemandTargetDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_ProductionDemandTargetDetail);
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

                tb_ProductionDemandTargetDetail entity = model as tb_ProductionDemandTargetDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PDTCID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ProductionDemandTargetDetail>(entity as tb_ProductionDemandTargetDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ProductionDemandTargetDetail>(entity as tb_ProductionDemandTargetDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PDTCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProductionDemandTargetDetail entity = model as tb_ProductionDemandTargetDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProductionDemandTargetDetail>(m => m.PDTCID== entity.PDTCID)
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
        
        
        
        public tb_ProductionDemandTargetDetail AddReEntity(tb_ProductionDemandTargetDetail entity)
        {
            tb_ProductionDemandTargetDetail AddEntity =  _tb_ProductionDemandTargetDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProductionDemandTargetDetail> AddReEntityAsync(tb_ProductionDemandTargetDetail entity)
        {
            tb_ProductionDemandTargetDetail AddEntity = await _tb_ProductionDemandTargetDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProductionDemandTargetDetail entity)
        {
            long id = await _tb_ProductionDemandTargetDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProductionDemandTargetDetail> infos)
        {
            List<long> ids = await _tb_ProductionDemandTargetDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProductionDemandTargetDetail entity)
        {
            bool rs = await _tb_ProductionDemandTargetDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProductionDemandTargetDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProductionDemandTargetDetail entity)
        {
            bool rs = await _tb_ProductionDemandTargetDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProductionDemandTargetDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProductionDemandTargetDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProductionDemandTargetDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProductionDemandTargetDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProductionDemandTargetDetail>> QueryAsync()
        {
            List<tb_ProductionDemandTargetDetail> list = await  _tb_ProductionDemandTargetDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProductionDemandTargetDetail> Query()
        {
            List<tb_ProductionDemandTargetDetail> list =  _tb_ProductionDemandTargetDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProductionDemandTargetDetail> Query(string wheresql)
        {
            List<tb_ProductionDemandTargetDetail> list =  _tb_ProductionDemandTargetDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProductionDemandTargetDetail>> QueryAsync(string wheresql) 
        {
            List<tb_ProductionDemandTargetDetail> list = await _tb_ProductionDemandTargetDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProductionDemandTargetDetail>> QueryAsync(Expression<Func<tb_ProductionDemandTargetDetail, bool>> exp)
        {
            List<tb_ProductionDemandTargetDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProductionDemandTargetDetail>> QueryByNavAsync()
        {
            List<tb_ProductionDemandTargetDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>()
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_productiondemand )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProductionDemandTargetDetail>> QueryByNavAsync(Expression<Func<tb_ProductionDemandTargetDetail, bool>> exp)
        {
            List<tb_ProductionDemandTargetDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>().Where(exp)
                               .Includes(t => t.tb_bom_s )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_productiondemand )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProductionDemandTargetDetail> QueryByNav(Expression<Func<tb_ProductionDemandTargetDetail, bool>> exp)
        {
            List<tb_ProductionDemandTargetDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>().Where(exp)
                            .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_productiondemand )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProductionDemandTargetDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProductionDemandTargetDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProductionDemandTargetDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductionDemandTargetDetail>().Where(w => w.PDTCID == (long)id)
                             .Includes(t => t.tb_bom_s )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_productiondemand )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProductionDemandTargetDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



