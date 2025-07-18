
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:47
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
    /// 借出单明细
    /// </summary>
    public partial class tb_ProdBorrowingDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProdBorrowingDetailServices _tb_ProdBorrowingDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProdBorrowingDetailController(ILogger<tb_ProdBorrowingDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProdBorrowingDetailServices tb_ProdBorrowingDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProdBorrowingDetailServices = tb_ProdBorrowingDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProdBorrowingDetail info)
        {

           // tb_ProdBorrowingDetailValidator validator = new tb_ProdBorrowingDetailValidator();
           tb_ProdBorrowingDetailValidator validator = _appContext.GetRequiredService<tb_ProdBorrowingDetailValidator>();
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
        public async Task<ReturnResults<tb_ProdBorrowingDetail>> SaveOrUpdate(tb_ProdBorrowingDetail entity)
        {
            ReturnResults<tb_ProdBorrowingDetail> rr = new ReturnResults<tb_ProdBorrowingDetail>();
            tb_ProdBorrowingDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BorrowDetaill_ID > 0)
                {
                    bool rs = await _tb_ProdBorrowingDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProdBorrowingDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
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
            tb_ProdBorrowingDetail entity = model as tb_ProdBorrowingDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BorrowDetaill_ID > 0)
                {
                    bool rs = await _tb_ProdBorrowingDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProdBorrowingDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
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
            List<T> list = await _tb_ProdBorrowingDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProdBorrowingDetail entity = item as tb_ProdBorrowingDetail;
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
            List<T> list = await _tb_ProdBorrowingDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProdBorrowingDetail entity = item as tb_ProdBorrowingDetail;
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
            tb_ProdBorrowingDetail entity = model as tb_ProdBorrowingDetail;
            bool rs = await _tb_ProdBorrowingDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowingDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProdBorrowingDetail> entitys = models as List<tb_ProdBorrowingDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProdBorrowingDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BorrowDetaill_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowingDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProdBorrowingDetailValidator validator = new tb_ProdBorrowingDetailValidator();
           tb_ProdBorrowingDetailValidator validator = _appContext.GetRequiredService<tb_ProdBorrowingDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_ProdBorrowingDetail);
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

                tb_ProdBorrowingDetail entity = model as tb_ProdBorrowingDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.BorrowDetaill_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ProdBorrowingDetail>(entity as tb_ProdBorrowingDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ProdBorrowingDetail>(entity as tb_ProdBorrowingDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BorrowDetaill_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProdBorrowingDetail entity = model as tb_ProdBorrowingDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProdBorrowingDetail>(m => m.BorrowDetaill_ID== entity.BorrowDetaill_ID)
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
        
        
        
        public tb_ProdBorrowingDetail AddReEntity(tb_ProdBorrowingDetail entity)
        {
            tb_ProdBorrowingDetail AddEntity =  _tb_ProdBorrowingDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProdBorrowingDetail> AddReEntityAsync(tb_ProdBorrowingDetail entity)
        {
            tb_ProdBorrowingDetail AddEntity = await _tb_ProdBorrowingDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProdBorrowingDetail entity)
        {
            long id = await _tb_ProdBorrowingDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProdBorrowingDetail> infos)
        {
            List<long> ids = await _tb_ProdBorrowingDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProdBorrowingDetail entity)
        {
            bool rs = await _tb_ProdBorrowingDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowingDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProdBorrowingDetail entity)
        {
            bool rs = await _tb_ProdBorrowingDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProdBorrowingDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowingDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProdBorrowingDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProdBorrowingDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProdBorrowingDetail>> QueryAsync()
        {
            List<tb_ProdBorrowingDetail> list = await  _tb_ProdBorrowingDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProdBorrowingDetail> Query()
        {
            List<tb_ProdBorrowingDetail> list =  _tb_ProdBorrowingDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        
        public virtual List<tb_ProdBorrowingDetail> Query(string wheresql)
        {
            List<tb_ProdBorrowingDetail> list =  _tb_ProdBorrowingDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProdBorrowingDetail>> QueryAsync(string wheresql) 
        {
            List<tb_ProdBorrowingDetail> list = await _tb_ProdBorrowingDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProdBorrowingDetail>> QueryAsync(Expression<Func<tb_ProdBorrowingDetail, bool>> exp)
        {
            List<tb_ProdBorrowingDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdBorrowingDetail>> QueryByNavAsync()
        {
            List<tb_ProdBorrowingDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>()
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_prodborrowing )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProdBorrowingDetail>> QueryByNavAsync(Expression<Func<tb_ProdBorrowingDetail, bool>> exp)
        {
            List<tb_ProdBorrowingDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>().Where(exp)
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_prodborrowing )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProdBorrowingDetail> QueryByNav(Expression<Func<tb_ProdBorrowingDetail, bool>> exp)
        {
            List<tb_ProdBorrowingDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>().Where(exp)
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_prodborrowing )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProdBorrowingDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProdBorrowingDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProdBorrowingDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProdBorrowingDetail>().Where(w => w.BorrowDetaill_ID == (long)id)
                             .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_prodborrowing )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProdBorrowingDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



