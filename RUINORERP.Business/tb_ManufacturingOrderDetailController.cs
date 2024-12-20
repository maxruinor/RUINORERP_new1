
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
    /// 制令单的原料明细表 明细对应的是一个树，结构同BOM，先把BOM搞好再来实现这里的细节
    /// </summary>
    public partial class tb_ManufacturingOrderDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ManufacturingOrderDetailServices _tb_ManufacturingOrderDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ManufacturingOrderDetailController(ILogger<tb_ManufacturingOrderDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ManufacturingOrderDetailServices tb_ManufacturingOrderDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ManufacturingOrderDetailServices = tb_ManufacturingOrderDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ManufacturingOrderDetail info)
        {

           // tb_ManufacturingOrderDetailValidator validator = new tb_ManufacturingOrderDetailValidator();
           tb_ManufacturingOrderDetailValidator validator = _appContext.GetRequiredService<tb_ManufacturingOrderDetailValidator>();
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
        public async Task<ReturnResults<tb_ManufacturingOrderDetail>> SaveOrUpdate(tb_ManufacturingOrderDetail entity)
        {
            ReturnResults<tb_ManufacturingOrderDetail> rr = new ReturnResults<tb_ManufacturingOrderDetail>();
            tb_ManufacturingOrderDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MOCID > 0)
                {
                    bool rs = await _tb_ManufacturingOrderDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ManufacturingOrderDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
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
            tb_ManufacturingOrderDetail entity = model as tb_ManufacturingOrderDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.MOCID > 0)
                {
                    bool rs = await _tb_ManufacturingOrderDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ManufacturingOrderDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
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
            List<T> list = await _tb_ManufacturingOrderDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ManufacturingOrderDetail entity = item as tb_ManufacturingOrderDetail;
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
            List<T> list = await _tb_ManufacturingOrderDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ManufacturingOrderDetail entity = item as tb_ManufacturingOrderDetail;
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
            tb_ManufacturingOrderDetail entity = model as tb_ManufacturingOrderDetail;
            bool rs = await _tb_ManufacturingOrderDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrderDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ManufacturingOrderDetail> entitys = models as List<tb_ManufacturingOrderDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ManufacturingOrderDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.MOCID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrderDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ManufacturingOrderDetailValidator validator = new tb_ManufacturingOrderDetailValidator();
           tb_ManufacturingOrderDetailValidator validator = _appContext.GetRequiredService<tb_ManufacturingOrderDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_ManufacturingOrderDetail);
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
                tb_ManufacturingOrderDetail entity = model as tb_ManufacturingOrderDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.MOCID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ManufacturingOrderDetail>(entity as tb_ManufacturingOrderDetail)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ManufacturingOrderDetail>(entity as tb_ManufacturingOrderDetail)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.MOCID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ManufacturingOrderDetail entity = model as tb_ManufacturingOrderDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ManufacturingOrderDetail>(m => m.MOCID== entity.MOCID)
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
        
        
        
        public tb_ManufacturingOrderDetail AddReEntity(tb_ManufacturingOrderDetail entity)
        {
            tb_ManufacturingOrderDetail AddEntity =  _tb_ManufacturingOrderDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ManufacturingOrderDetail> AddReEntityAsync(tb_ManufacturingOrderDetail entity)
        {
            tb_ManufacturingOrderDetail AddEntity = await _tb_ManufacturingOrderDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ManufacturingOrderDetail entity)
        {
            long id = await _tb_ManufacturingOrderDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ManufacturingOrderDetail> infos)
        {
            List<long> ids = await _tb_ManufacturingOrderDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ManufacturingOrderDetail entity)
        {
            bool rs = await _tb_ManufacturingOrderDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrderDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ManufacturingOrderDetail entity)
        {
            bool rs = await _tb_ManufacturingOrderDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ManufacturingOrderDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrderDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ManufacturingOrderDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ManufacturingOrderDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ManufacturingOrderDetail>> QueryAsync()
        {
            List<tb_ManufacturingOrderDetail> list = await  _tb_ManufacturingOrderDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        
        public virtual List<tb_ManufacturingOrderDetail> Query()
        {
            List<tb_ManufacturingOrderDetail> list =  _tb_ManufacturingOrderDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        
        public virtual List<tb_ManufacturingOrderDetail> Query(string wheresql)
        {
            List<tb_ManufacturingOrderDetail> list =  _tb_ManufacturingOrderDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ManufacturingOrderDetail>> QueryAsync(string wheresql) 
        {
            List<tb_ManufacturingOrderDetail> list = await _tb_ManufacturingOrderDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ManufacturingOrderDetail>> QueryAsync(Expression<Func<tb_ManufacturingOrderDetail, bool>> exp)
        {
            List<tb_ManufacturingOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ManufacturingOrderDetail>> QueryByNavAsync()
        {
            List<tb_ManufacturingOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_manufacturingorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ManufacturingOrderDetail>> QueryByNavAsync(Expression<Func<tb_ManufacturingOrderDetail, bool>> exp)
        {
            List<tb_ManufacturingOrderDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_manufacturingorder )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ManufacturingOrderDetail> QueryByNav(Expression<Func<tb_ManufacturingOrderDetail, bool>> exp)
        {
            List<tb_ManufacturingOrderDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_manufacturingorder )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ManufacturingOrderDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ManufacturingOrderDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ManufacturingOrderDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ManufacturingOrderDetail>().Where(w => w.MOCID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_manufacturingorder )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ManufacturingOrderDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



