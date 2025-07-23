
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:41
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单
    /// </summary>
    public partial class tb_FM_OtherExpenseDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_OtherExpenseDetailServices _tb_FM_OtherExpenseDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_OtherExpenseDetailController(ILogger<tb_FM_OtherExpenseDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_OtherExpenseDetailServices tb_FM_OtherExpenseDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_OtherExpenseDetailServices = tb_FM_OtherExpenseDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_OtherExpenseDetail info)
        {

           // tb_FM_OtherExpenseDetailValidator validator = new tb_FM_OtherExpenseDetailValidator();
           tb_FM_OtherExpenseDetailValidator validator = _appContext.GetRequiredService<tb_FM_OtherExpenseDetailValidator>();
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
        public async Task<ReturnResults<tb_FM_OtherExpenseDetail>> SaveOrUpdate(tb_FM_OtherExpenseDetail entity)
        {
            ReturnResults<tb_FM_OtherExpenseDetail> rr = new ReturnResults<tb_FM_OtherExpenseDetail>();
            tb_FM_OtherExpenseDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ExpenseSubID > 0)
                {
                    bool rs = await _tb_FM_OtherExpenseDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_OtherExpenseDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
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
            tb_FM_OtherExpenseDetail entity = model as tb_FM_OtherExpenseDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ExpenseSubID > 0)
                {
                    bool rs = await _tb_FM_OtherExpenseDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_OtherExpenseDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
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
            List<T> list = await _tb_FM_OtherExpenseDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_OtherExpenseDetail entity = item as tb_FM_OtherExpenseDetail;
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
            List<T> list = await _tb_FM_OtherExpenseDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_OtherExpenseDetail entity = item as tb_FM_OtherExpenseDetail;
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
            tb_FM_OtherExpenseDetail entity = model as tb_FM_OtherExpenseDetail;
            bool rs = await _tb_FM_OtherExpenseDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_OtherExpenseDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_OtherExpenseDetail> entitys = models as List<tb_FM_OtherExpenseDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_OtherExpenseDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ExpenseSubID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_OtherExpenseDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_OtherExpenseDetailValidator validator = new tb_FM_OtherExpenseDetailValidator();
           tb_FM_OtherExpenseDetailValidator validator = _appContext.GetRequiredService<tb_FM_OtherExpenseDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_OtherExpenseDetail);
            return results;
        }
        
        
        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike,object dto) 
        {
            var  querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().WhereCustom(useLike,dto);
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

                tb_FM_OtherExpenseDetail entity = model as tb_FM_OtherExpenseDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ExpenseSubID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_OtherExpenseDetail>(entity as tb_FM_OtherExpenseDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_OtherExpenseDetail>(entity as tb_FM_OtherExpenseDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ExpenseSubID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_OtherExpenseDetail entity = model as tb_FM_OtherExpenseDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_OtherExpenseDetail>(m => m.ExpenseSubID== entity.ExpenseSubID)
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
        
        
        
        public tb_FM_OtherExpenseDetail AddReEntity(tb_FM_OtherExpenseDetail entity)
        {
            tb_FM_OtherExpenseDetail AddEntity =  _tb_FM_OtherExpenseDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_OtherExpenseDetail> AddReEntityAsync(tb_FM_OtherExpenseDetail entity)
        {
            tb_FM_OtherExpenseDetail AddEntity = await _tb_FM_OtherExpenseDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_OtherExpenseDetail entity)
        {
            long id = await _tb_FM_OtherExpenseDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_OtherExpenseDetail> infos)
        {
            List<long> ids = await _tb_FM_OtherExpenseDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_OtherExpenseDetail entity)
        {
            bool rs = await _tb_FM_OtherExpenseDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_OtherExpenseDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_OtherExpenseDetail entity)
        {
            bool rs = await _tb_FM_OtherExpenseDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_OtherExpenseDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_OtherExpenseDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_OtherExpenseDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_OtherExpenseDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_OtherExpenseDetail>> QueryAsync()
        {
            List<tb_FM_OtherExpenseDetail> list = await  _tb_FM_OtherExpenseDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        
        public virtual List<tb_FM_OtherExpenseDetail> Query()
        {
            List<tb_FM_OtherExpenseDetail> list =  _tb_FM_OtherExpenseDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        
        public virtual List<tb_FM_OtherExpenseDetail> Query(string wheresql)
        {
            List<tb_FM_OtherExpenseDetail> list =  _tb_FM_OtherExpenseDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_OtherExpenseDetail>> QueryAsync(string wheresql) 
        {
            List<tb_FM_OtherExpenseDetail> list = await _tb_FM_OtherExpenseDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_OtherExpenseDetail>> QueryAsync(Expression<Func<tb_FM_OtherExpenseDetail, bool>> exp)
        {
            List<tb_FM_OtherExpenseDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_OtherExpenseDetail>> QueryByNavAsync()
        {
            List<tb_FM_OtherExpenseDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_fm_otherexpense )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_expensetype )
                               .Includes(t => t.tb_fm_subject )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_OtherExpenseDetail>> QueryByNavAsync(Expression<Func<tb_FM_OtherExpenseDetail, bool>> exp)
        {
            List<tb_FM_OtherExpenseDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_fm_otherexpense )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_fm_expensetype )
                               .Includes(t => t.tb_fm_subject )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_OtherExpenseDetail> QueryByNav(Expression<Func<tb_FM_OtherExpenseDetail, bool>> exp)
        {
            List<tb_FM_OtherExpenseDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_fm_otherexpense )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_expensetype )
                            .Includes(t => t.tb_fm_subject )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_OtherExpenseDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_OtherExpenseDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_OtherExpenseDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_OtherExpenseDetail>().Where(w => w.ExpenseSubID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_fm_otherexpense )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_fm_expensetype )
                            .Includes(t => t.tb_fm_subject )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_OtherExpenseDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



