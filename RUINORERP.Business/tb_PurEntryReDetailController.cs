
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:12
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
    /// 采购入库退回单
    /// </summary>
    public partial class tb_PurEntryReDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PurEntryReDetailServices _tb_PurEntryReDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_PurEntryReDetailController(ILogger<tb_PurEntryReDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PurEntryReDetailServices tb_PurEntryReDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PurEntryReDetailServices = tb_PurEntryReDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_PurEntryReDetail info)
        {

           // tb_PurEntryReDetailValidator validator = new tb_PurEntryReDetailValidator();
           tb_PurEntryReDetailValidator validator = _appContext.GetRequiredService<tb_PurEntryReDetailValidator>();
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
        public async Task<ReturnResults<tb_PurEntryReDetail>> SaveOrUpdate(tb_PurEntryReDetail entity)
        {
            ReturnResults<tb_PurEntryReDetail> rr = new ReturnResults<tb_PurEntryReDetail>();
            tb_PurEntryReDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurEntryRe_CID > 0)
                {
                    bool rs = await _tb_PurEntryReDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PurEntryReDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
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
            tb_PurEntryReDetail entity = model as tb_PurEntryReDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurEntryRe_CID > 0)
                {
                    bool rs = await _tb_PurEntryReDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PurEntryReDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
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
            List<T> list = await _tb_PurEntryReDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PurEntryReDetail entity = item as tb_PurEntryReDetail;
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
            List<T> list = await _tb_PurEntryReDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PurEntryReDetail entity = item as tb_PurEntryReDetail;
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
            tb_PurEntryReDetail entity = model as tb_PurEntryReDetail;
            bool rs = await _tb_PurEntryReDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntryReDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PurEntryReDetail> entitys = models as List<tb_PurEntryReDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PurEntryReDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PurEntryRe_CID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntryReDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PurEntryReDetailValidator validator = new tb_PurEntryReDetailValidator();
           tb_PurEntryReDetailValidator validator = _appContext.GetRequiredService<tb_PurEntryReDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_PurEntryReDetail);
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
                tb_PurEntryReDetail entity = model as tb_PurEntryReDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PurEntryRe_CID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PurEntryReDetail>(entity as tb_PurEntryReDetail)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PurEntryReDetail>(entity as tb_PurEntryReDetail)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PurEntryRe_CID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PurEntryReDetail entity = model as tb_PurEntryReDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PurEntryReDetail>(m => m.PurEntryRe_CID== entity.PurEntryRe_CID)
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
        
        
        
        public tb_PurEntryReDetail AddReEntity(tb_PurEntryReDetail entity)
        {
            tb_PurEntryReDetail AddEntity =  _tb_PurEntryReDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PurEntryReDetail> AddReEntityAsync(tb_PurEntryReDetail entity)
        {
            tb_PurEntryReDetail AddEntity = await _tb_PurEntryReDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PurEntryReDetail entity)
        {
            long id = await _tb_PurEntryReDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PurEntryReDetail> infos)
        {
            List<long> ids = await _tb_PurEntryReDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PurEntryReDetail entity)
        {
            bool rs = await _tb_PurEntryReDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntryReDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PurEntryReDetail entity)
        {
            bool rs = await _tb_PurEntryReDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PurEntryReDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntryReDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PurEntryReDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntryReDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PurEntryReDetail>> QueryAsync()
        {
            List<tb_PurEntryReDetail> list = await  _tb_PurEntryReDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        
        public virtual List<tb_PurEntryReDetail> Query()
        {
            List<tb_PurEntryReDetail> list =  _tb_PurEntryReDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        
        public virtual List<tb_PurEntryReDetail> Query(string wheresql)
        {
            List<tb_PurEntryReDetail> list =  _tb_PurEntryReDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PurEntryReDetail>> QueryAsync(string wheresql) 
        {
            List<tb_PurEntryReDetail> list = await _tb_PurEntryReDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PurEntryReDetail>> QueryAsync(Expression<Func<tb_PurEntryReDetail, bool>> exp)
        {
            List<tb_PurEntryReDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurEntryReDetail>> QueryByNavAsync()
        {
            List<tb_PurEntryReDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>()
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_purentryre )
                               .Includes(t => t.tb_storagerack )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurEntryReDetail>> QueryByNavAsync(Expression<Func<tb_PurEntryReDetail, bool>> exp)
        {
            List<tb_PurEntryReDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>().Where(exp)
                               .Includes(t => t.tb_location )
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_purentryre )
                               .Includes(t => t.tb_storagerack )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PurEntryReDetail> QueryByNav(Expression<Func<tb_PurEntryReDetail, bool>> exp)
        {
            List<tb_PurEntryReDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>().Where(exp)
                            .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_purentryre )
                            .Includes(t => t.tb_storagerack )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PurEntryReDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PurEntryReDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PurEntryReDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntryReDetail>().Where(w => w.PurEntryRe_CID == (long)id)
                             .Includes(t => t.tb_location )
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_purentryre )
                            .Includes(t => t.tb_storagerack )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_PurEntryReDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



