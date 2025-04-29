
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:31
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
    /// 应收应付明细
    /// </summary>
    public partial class tb_FM_ReceivablePayableDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_ReceivablePayableDetailServices _tb_FM_ReceivablePayableDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_ReceivablePayableDetailController(ILogger<tb_FM_ReceivablePayableDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_ReceivablePayableDetailServices tb_FM_ReceivablePayableDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_ReceivablePayableDetailServices = tb_FM_ReceivablePayableDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_ReceivablePayableDetail info)
        {

           // tb_FM_ReceivablePayableDetailValidator validator = new tb_FM_ReceivablePayableDetailValidator();
           tb_FM_ReceivablePayableDetailValidator validator = _appContext.GetRequiredService<tb_FM_ReceivablePayableDetailValidator>();
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
        public async Task<ReturnResults<tb_FM_ReceivablePayableDetail>> SaveOrUpdate(tb_FM_ReceivablePayableDetail entity)
        {
            ReturnResults<tb_FM_ReceivablePayableDetail> rr = new ReturnResults<tb_FM_ReceivablePayableDetail>();
            tb_FM_ReceivablePayableDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ARAPDetailID > 0)
                {
                    bool rs = await _tb_FM_ReceivablePayableDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_ReceivablePayableDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
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
            tb_FM_ReceivablePayableDetail entity = model as tb_FM_ReceivablePayableDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ARAPDetailID > 0)
                {
                    bool rs = await _tb_FM_ReceivablePayableDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_ReceivablePayableDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
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
            List<T> list = await _tb_FM_ReceivablePayableDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_ReceivablePayableDetail entity = item as tb_FM_ReceivablePayableDetail;
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
            List<T> list = await _tb_FM_ReceivablePayableDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_ReceivablePayableDetail entity = item as tb_FM_ReceivablePayableDetail;
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
            tb_FM_ReceivablePayableDetail entity = model as tb_FM_ReceivablePayableDetail;
            bool rs = await _tb_FM_ReceivablePayableDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ReceivablePayableDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_ReceivablePayableDetail> entitys = models as List<tb_FM_ReceivablePayableDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_ReceivablePayableDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ARAPDetailID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ReceivablePayableDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_ReceivablePayableDetailValidator validator = new tb_FM_ReceivablePayableDetailValidator();
           tb_FM_ReceivablePayableDetailValidator validator = _appContext.GetRequiredService<tb_FM_ReceivablePayableDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_ReceivablePayableDetail);
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

                tb_FM_ReceivablePayableDetail entity = model as tb_FM_ReceivablePayableDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ARAPDetailID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ReceivablePayableDetail>(entity as tb_FM_ReceivablePayableDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_ReceivablePayableDetail>(entity as tb_FM_ReceivablePayableDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ARAPDetailID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_ReceivablePayableDetail entity = model as tb_FM_ReceivablePayableDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_ReceivablePayableDetail>(m => m.ARAPDetailID== entity.ARAPDetailID)
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
        
        
        
        public tb_FM_ReceivablePayableDetail AddReEntity(tb_FM_ReceivablePayableDetail entity)
        {
            tb_FM_ReceivablePayableDetail AddEntity =  _tb_FM_ReceivablePayableDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_ReceivablePayableDetail> AddReEntityAsync(tb_FM_ReceivablePayableDetail entity)
        {
            tb_FM_ReceivablePayableDetail AddEntity = await _tb_FM_ReceivablePayableDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_ReceivablePayableDetail entity)
        {
            long id = await _tb_FM_ReceivablePayableDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_ReceivablePayableDetail> infos)
        {
            List<long> ids = await _tb_FM_ReceivablePayableDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_ReceivablePayableDetail entity)
        {
            bool rs = await _tb_FM_ReceivablePayableDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ReceivablePayableDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_ReceivablePayableDetail entity)
        {
            bool rs = await _tb_FM_ReceivablePayableDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_ReceivablePayableDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ReceivablePayableDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_ReceivablePayableDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ReceivablePayableDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_ReceivablePayableDetail>> QueryAsync()
        {
            List<tb_FM_ReceivablePayableDetail> list = await  _tb_FM_ReceivablePayableDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        
        public virtual List<tb_FM_ReceivablePayableDetail> Query()
        {
            List<tb_FM_ReceivablePayableDetail> list =  _tb_FM_ReceivablePayableDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        
        public virtual List<tb_FM_ReceivablePayableDetail> Query(string wheresql)
        {
            List<tb_FM_ReceivablePayableDetail> list =  _tb_FM_ReceivablePayableDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_ReceivablePayableDetail>> QueryAsync(string wheresql) 
        {
            List<tb_FM_ReceivablePayableDetail> list = await _tb_FM_ReceivablePayableDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_ReceivablePayableDetail>> QueryAsync(Expression<Func<tb_FM_ReceivablePayableDetail, bool>> exp)
        {
            List<tb_FM_ReceivablePayableDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_ReceivablePayableDetail>> QueryByNavAsync()
        {
            List<tb_FM_ReceivablePayableDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>()
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_fm_receivablepayable )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_ReceivablePayableDetail>> QueryByNavAsync(Expression<Func<tb_FM_ReceivablePayableDetail, bool>> exp)
        {
            List<tb_FM_ReceivablePayableDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>().Where(exp)
                               .Includes(t => t.tb_proddetail )
                               .Includes(t => t.tb_unit )
                               .Includes(t => t.tb_fm_receivablepayable )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_ReceivablePayableDetail> QueryByNav(Expression<Func<tb_FM_ReceivablePayableDetail, bool>> exp)
        {
            List<tb_FM_ReceivablePayableDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>().Where(exp)
                            .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_fm_receivablepayable )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_ReceivablePayableDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_ReceivablePayableDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_ReceivablePayableDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ReceivablePayableDetail>().Where(w => w.ARAPDetailID == (long)id)
                             .Includes(t => t.tb_proddetail )
                            .Includes(t => t.tb_unit )
                            .Includes(t => t.tb_fm_receivablepayable )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_ReceivablePayableDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



