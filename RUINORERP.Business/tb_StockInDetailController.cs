
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:53
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
    /// 入库单明细
    /// </summary>
    public partial class tb_StockInDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_StockInDetailServices _tb_StockInDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_StockInDetailController(ILogger<tb_StockInDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_StockInDetailServices tb_StockInDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_StockInDetailServices = tb_StockInDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_StockInDetail info)
        {

           // tb_StockInDetailValidator validator = new tb_StockInDetailValidator();
           tb_StockInDetailValidator validator = _appContext.GetRequiredService<tb_StockInDetailValidator>();
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
        public async Task<ReturnResults<tb_StockInDetail>> SaveOrUpdate(tb_StockInDetail entity)
        {
            ReturnResults<tb_StockInDetail> rr = new ReturnResults<tb_StockInDetail>();
            tb_StockInDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Sub_ID > 0)
                {
                    bool rs = await _tb_StockInDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_StockInDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
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
            tb_StockInDetail entity = model as tb_StockInDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Sub_ID > 0)
                {
                    bool rs = await _tb_StockInDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_StockInDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
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
            List<T> list = await _tb_StockInDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_StockInDetail entity = item as tb_StockInDetail;
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
            List<T> list = await _tb_StockInDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_StockInDetail entity = item as tb_StockInDetail;
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
            tb_StockInDetail entity = model as tb_StockInDetail;
            bool rs = await _tb_StockInDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_StockInDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_StockInDetail> entitys = models as List<tb_StockInDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_StockInDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Sub_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_StockInDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_StockInDetailValidator validator = new tb_StockInDetailValidator();
           tb_StockInDetailValidator validator = _appContext.GetRequiredService<tb_StockInDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_StockInDetail);
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

                tb_StockInDetail entity = model as tb_StockInDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Sub_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_StockInDetail>(entity as tb_StockInDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_StockInDetail>(entity as tb_StockInDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.Sub_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_StockInDetail entity = model as tb_StockInDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_StockInDetail>(m => m.Sub_ID== entity.Sub_ID)
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
        
        
        
        public tb_StockInDetail AddReEntity(tb_StockInDetail entity)
        {
            tb_StockInDetail AddEntity =  _tb_StockInDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_StockInDetail> AddReEntityAsync(tb_StockInDetail entity)
        {
            tb_StockInDetail AddEntity = await _tb_StockInDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_StockInDetail entity)
        {
            long id = await _tb_StockInDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_StockInDetail> infos)
        {
            List<long> ids = await _tb_StockInDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_StockInDetail entity)
        {
            bool rs = await _tb_StockInDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockInDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_StockInDetail entity)
        {
            bool rs = await _tb_StockInDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_StockInDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockInDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_StockInDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_StockInDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_StockInDetail>> QueryAsync()
        {
            List<tb_StockInDetail> list = await  _tb_StockInDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        
        public virtual List<tb_StockInDetail> Query()
        {
            List<tb_StockInDetail> list =  _tb_StockInDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        
        public virtual List<tb_StockInDetail> Query(string wheresql)
        {
            List<tb_StockInDetail> list =  _tb_StockInDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_StockInDetail>> QueryAsync(string wheresql) 
        {
            List<tb_StockInDetail> list = await _tb_StockInDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_StockInDetail>> QueryAsync(Expression<Func<tb_StockInDetail, bool>> exp)
        {
            List<tb_StockInDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockInDetail>> QueryByNavAsync()
        {
            List<tb_StockInDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>()
                               .Includes(t => t.tb_stockin )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_StockInDetail>> QueryByNavAsync(Expression<Func<tb_StockInDetail, bool>> exp)
        {
            List<tb_StockInDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>().Where(exp)
                               .Includes(t => t.tb_stockin )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_location )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_StockInDetail> QueryByNav(Expression<Func<tb_StockInDetail, bool>> exp)
        {
            List<tb_StockInDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>().Where(exp)
                            .Includes(t => t.tb_stockin )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_StockInDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_StockInDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_StockInDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_StockInDetail>().Where(w => w.Sub_ID == (long)id)
                             .Includes(t => t.tb_stockin )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_location )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_StockInDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



