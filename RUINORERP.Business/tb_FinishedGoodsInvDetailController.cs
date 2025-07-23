
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:40
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
    /// 成品入库单明细
    /// </summary>
    public partial class tb_FinishedGoodsInvDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FinishedGoodsInvDetailServices _tb_FinishedGoodsInvDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FinishedGoodsInvDetailController(ILogger<tb_FinishedGoodsInvDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FinishedGoodsInvDetailServices tb_FinishedGoodsInvDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FinishedGoodsInvDetailServices = tb_FinishedGoodsInvDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FinishedGoodsInvDetail info)
        {

           // tb_FinishedGoodsInvDetailValidator validator = new tb_FinishedGoodsInvDetailValidator();
           tb_FinishedGoodsInvDetailValidator validator = _appContext.GetRequiredService<tb_FinishedGoodsInvDetailValidator>();
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
        public async Task<ReturnResults<tb_FinishedGoodsInvDetail>> SaveOrUpdate(tb_FinishedGoodsInvDetail entity)
        {
            ReturnResults<tb_FinishedGoodsInvDetail> rr = new ReturnResults<tb_FinishedGoodsInvDetail>();
            tb_FinishedGoodsInvDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Sub_ID > 0)
                {
                    bool rs = await _tb_FinishedGoodsInvDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FinishedGoodsInvDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
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
            tb_FinishedGoodsInvDetail entity = model as tb_FinishedGoodsInvDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Sub_ID > 0)
                {
                    bool rs = await _tb_FinishedGoodsInvDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FinishedGoodsInvDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
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
            List<T> list = await _tb_FinishedGoodsInvDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FinishedGoodsInvDetail entity = item as tb_FinishedGoodsInvDetail;
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
            List<T> list = await _tb_FinishedGoodsInvDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FinishedGoodsInvDetail entity = item as tb_FinishedGoodsInvDetail;
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
            tb_FinishedGoodsInvDetail entity = model as tb_FinishedGoodsInvDetail;
            bool rs = await _tb_FinishedGoodsInvDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInvDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FinishedGoodsInvDetail> entitys = models as List<tb_FinishedGoodsInvDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FinishedGoodsInvDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.Sub_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInvDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FinishedGoodsInvDetailValidator validator = new tb_FinishedGoodsInvDetailValidator();
           tb_FinishedGoodsInvDetailValidator validator = _appContext.GetRequiredService<tb_FinishedGoodsInvDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_FinishedGoodsInvDetail);
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

                tb_FinishedGoodsInvDetail entity = model as tb_FinishedGoodsInvDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.Sub_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_FinishedGoodsInvDetail>(entity as tb_FinishedGoodsInvDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_FinishedGoodsInvDetail>(entity as tb_FinishedGoodsInvDetail)
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FinishedGoodsInvDetail entity = model as tb_FinishedGoodsInvDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FinishedGoodsInvDetail>(m => m.Sub_ID== entity.Sub_ID)
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
        
        
        
        public tb_FinishedGoodsInvDetail AddReEntity(tb_FinishedGoodsInvDetail entity)
        {
            tb_FinishedGoodsInvDetail AddEntity =  _tb_FinishedGoodsInvDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FinishedGoodsInvDetail> AddReEntityAsync(tb_FinishedGoodsInvDetail entity)
        {
            tb_FinishedGoodsInvDetail AddEntity = await _tb_FinishedGoodsInvDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FinishedGoodsInvDetail entity)
        {
            long id = await _tb_FinishedGoodsInvDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FinishedGoodsInvDetail> infos)
        {
            List<long> ids = await _tb_FinishedGoodsInvDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FinishedGoodsInvDetail entity)
        {
            bool rs = await _tb_FinishedGoodsInvDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInvDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FinishedGoodsInvDetail entity)
        {
            bool rs = await _tb_FinishedGoodsInvDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FinishedGoodsInvDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInvDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FinishedGoodsInvDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FinishedGoodsInvDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FinishedGoodsInvDetail>> QueryAsync()
        {
            List<tb_FinishedGoodsInvDetail> list = await  _tb_FinishedGoodsInvDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        
        public virtual List<tb_FinishedGoodsInvDetail> Query()
        {
            List<tb_FinishedGoodsInvDetail> list =  _tb_FinishedGoodsInvDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        
        public virtual List<tb_FinishedGoodsInvDetail> Query(string wheresql)
        {
            List<tb_FinishedGoodsInvDetail> list =  _tb_FinishedGoodsInvDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FinishedGoodsInvDetail>> QueryAsync(string wheresql) 
        {
            List<tb_FinishedGoodsInvDetail> list = await _tb_FinishedGoodsInvDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FinishedGoodsInvDetail>> QueryAsync(Expression<Func<tb_FinishedGoodsInvDetail, bool>> exp)
        {
            List<tb_FinishedGoodsInvDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FinishedGoodsInvDetail>> QueryByNavAsync()
        {
            List<tb_FinishedGoodsInvDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_finishedgoodsinv )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_unit )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FinishedGoodsInvDetail>> QueryByNavAsync(Expression<Func<tb_FinishedGoodsInvDetail, bool>> exp)
        {
            List<tb_FinishedGoodsInvDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_storagerack )
                               .Includes(t => t.tb_finishedgoodsinv )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_unit )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FinishedGoodsInvDetail> QueryByNav(Expression<Func<tb_FinishedGoodsInvDetail, bool>> exp)
        {
            List<tb_FinishedGoodsInvDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_finishedgoodsinv )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_unit )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FinishedGoodsInvDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FinishedGoodsInvDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FinishedGoodsInvDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FinishedGoodsInvDetail>().Where(w => w.Sub_ID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_storagerack )
                            .Includes(t => t.tb_finishedgoodsinv )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_unit )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FinishedGoodsInvDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



