
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:41
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
    /// 期初应收应付帐款表
    /// </summary>
    public partial class tb_FM_Initial_PayAndReceivableController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_Initial_PayAndReceivableServices _tb_FM_Initial_PayAndReceivableServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_Initial_PayAndReceivableController(ILogger<tb_FM_Initial_PayAndReceivableController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_Initial_PayAndReceivableServices tb_FM_Initial_PayAndReceivableServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_Initial_PayAndReceivableServices = tb_FM_Initial_PayAndReceivableServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_FM_Initial_PayAndReceivable info)
        {
            tb_FM_Initial_PayAndReceivableValidator validator = new tb_FM_Initial_PayAndReceivableValidator();
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
        public async Task<ReturnResults<tb_FM_Initial_PayAndReceivable>> SaveOrUpdate(tb_FM_Initial_PayAndReceivable entity)
        {
            ReturnResults<tb_FM_Initial_PayAndReceivable> rr = new ReturnResults<tb_FM_Initial_PayAndReceivable>();
            tb_FM_Initial_PayAndReceivable Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Init_PRID > 0)
                {
                    bool rs = await _tb_FM_Initial_PayAndReceivableServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_Initial_PayAndReceivableServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
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
            tb_FM_Initial_PayAndReceivable entity = model as tb_FM_Initial_PayAndReceivable;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Init_PRID > 0)
                {
                    bool rs = await _tb_FM_Initial_PayAndReceivableServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_Initial_PayAndReceivableServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
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
            List<T> list = await _tb_FM_Initial_PayAndReceivableServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_Initial_PayAndReceivable entity = item as tb_FM_Initial_PayAndReceivable;
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
            List<T> list = await _tb_FM_Initial_PayAndReceivableServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_Initial_PayAndReceivable entity = item as tb_FM_Initial_PayAndReceivable;
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
            tb_FM_Initial_PayAndReceivable entity = model as tb_FM_Initial_PayAndReceivable;
            bool rs = await _tb_FM_Initial_PayAndReceivableServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Initial_PayAndReceivable>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_Initial_PayAndReceivable> entitys = models as List<tb_FM_Initial_PayAndReceivable>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_Initial_PayAndReceivable>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Init_PRID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Initial_PayAndReceivable>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_FM_Initial_PayAndReceivableValidator validator = new tb_FM_Initial_PayAndReceivableValidator();
            ValidationResult results = validator.Validate(info as tb_FM_Initial_PayAndReceivable);
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
                tb_FM_Initial_PayAndReceivable entity = model as tb_FM_Initial_PayAndReceivable;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Init_PRID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_FM_Initial_PayAndReceivable>(entity as tb_FM_Initial_PayAndReceivable)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_FM_Initial_PayAndReceivable>(entity as tb_FM_Initial_PayAndReceivable)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Init_PRID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_Initial_PayAndReceivable entity = model as tb_FM_Initial_PayAndReceivable;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_Initial_PayAndReceivable>(m => m.Init_PRID== entity.Init_PRID)
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
        
        
        
        public tb_FM_Initial_PayAndReceivable AddReEntity(tb_FM_Initial_PayAndReceivable entity)
        {
            tb_FM_Initial_PayAndReceivable AddEntity =  _tb_FM_Initial_PayAndReceivableServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_Initial_PayAndReceivable> AddReEntityAsync(tb_FM_Initial_PayAndReceivable entity)
        {
            tb_FM_Initial_PayAndReceivable AddEntity = await _tb_FM_Initial_PayAndReceivableServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_Initial_PayAndReceivable entity)
        {
            long id = await _tb_FM_Initial_PayAndReceivableServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_Initial_PayAndReceivable> infos)
        {
            List<long> ids = await _tb_FM_Initial_PayAndReceivableServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_Initial_PayAndReceivable entity)
        {
            bool rs = await _tb_FM_Initial_PayAndReceivableServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Initial_PayAndReceivable>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_Initial_PayAndReceivable entity)
        {
            bool rs = await _tb_FM_Initial_PayAndReceivableServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_Initial_PayAndReceivableServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Initial_PayAndReceivable>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_Initial_PayAndReceivableServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_Initial_PayAndReceivable>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_Initial_PayAndReceivable>> QueryAsync()
        {
            List<tb_FM_Initial_PayAndReceivable> list = await  _tb_FM_Initial_PayAndReceivableServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        
        public virtual List<tb_FM_Initial_PayAndReceivable> Query()
        {
            List<tb_FM_Initial_PayAndReceivable> list =  _tb_FM_Initial_PayAndReceivableServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        
        public virtual List<tb_FM_Initial_PayAndReceivable> Query(string wheresql)
        {
            List<tb_FM_Initial_PayAndReceivable> list =  _tb_FM_Initial_PayAndReceivableServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_Initial_PayAndReceivable>> QueryAsync(string wheresql) 
        {
            List<tb_FM_Initial_PayAndReceivable> list = await _tb_FM_Initial_PayAndReceivableServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_Initial_PayAndReceivable>> QueryAsync(Expression<Func<tb_FM_Initial_PayAndReceivable, bool>> exp)
        {
            List<tb_FM_Initial_PayAndReceivable> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Initial_PayAndReceivable>> QueryByNavAsync()
        {
            List<tb_FM_Initial_PayAndReceivable> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>()
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_expensetype )
                               .Includes(t => t.tb_fm_subject )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_Initial_PayAndReceivable>> QueryByNavAsync(Expression<Func<tb_FM_Initial_PayAndReceivable, bool>> exp)
        {
            List<tb_FM_Initial_PayAndReceivable> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>().Where(exp)
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_expensetype )
                               .Includes(t => t.tb_fm_subject )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_customervendor )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_Initial_PayAndReceivable> QueryByNav(Expression<Func<tb_FM_Initial_PayAndReceivable, bool>> exp)
        {
            List<tb_FM_Initial_PayAndReceivable> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>().Where(exp)
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_expensetype )
                            .Includes(t => t.tb_fm_subject )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_Initial_PayAndReceivable>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_Initial_PayAndReceivableServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_Initial_PayAndReceivable entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_Initial_PayAndReceivable>().Where(w => w.Init_PRID == (long)id)
                             .Includes(t => t.tb_department )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_expensetype )
                            .Includes(t => t.tb_fm_subject )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_customervendor )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_Initial_PayAndReceivable>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



