// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:37
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
    /// 损益费用单
    /// </summary>
    public partial class tb_FM_ProfitLossDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_FM_ProfitLossDetailServices _tb_FM_ProfitLossDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_FM_ProfitLossDetailController(ILogger<tb_FM_ProfitLossDetailController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_FM_ProfitLossDetailServices tb_FM_ProfitLossDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_FM_ProfitLossDetailServices = tb_FM_ProfitLossDetailServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_FM_ProfitLossDetail info)
        {

           // tb_FM_ProfitLossDetailValidator validator = new tb_FM_ProfitLossDetailValidator();
           tb_FM_ProfitLossDetailValidator validator = _appContext.GetRequiredService<tb_FM_ProfitLossDetailValidator>();
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
        public async Task<ReturnResults<tb_FM_ProfitLossDetail>> SaveOrUpdate(tb_FM_ProfitLossDetail entity)
        {
            ReturnResults<tb_FM_ProfitLossDetail> rr = new ReturnResults<tb_FM_ProfitLossDetail>();
            tb_FM_ProfitLossDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProfitLossDetail_ID > 0)
                {
                    bool rs = await _tb_FM_ProfitLossDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_FM_ProfitLossDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
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
            tb_FM_ProfitLossDetail entity = model as tb_FM_ProfitLossDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ProfitLossDetail_ID > 0)
                {
                    bool rs = await _tb_FM_ProfitLossDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_FM_ProfitLossDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
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
            List<T> list = await _tb_FM_ProfitLossDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_FM_ProfitLossDetail entity = item as tb_FM_ProfitLossDetail;
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
            List<T> list = await _tb_FM_ProfitLossDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_FM_ProfitLossDetail entity = item as tb_FM_ProfitLossDetail;
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
            tb_FM_ProfitLossDetail entity = model as tb_FM_ProfitLossDetail;
            bool rs = await _tb_FM_ProfitLossDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ProfitLossDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_FM_ProfitLossDetail> entitys = models as List<tb_FM_ProfitLossDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_FM_ProfitLossDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ProfitLossDetail_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ProfitLossDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_FM_ProfitLossDetailValidator validator = new tb_FM_ProfitLossDetailValidator();
           tb_FM_ProfitLossDetailValidator validator = _appContext.GetRequiredService<tb_FM_ProfitLossDetailValidator>();
            ValidationResult results = validator.Validate(info as tb_FM_ProfitLossDetail);
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

                tb_FM_ProfitLossDetail entity = model as tb_FM_ProfitLossDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ProfitLossDetail_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_FM_ProfitLossDetail>(entity as tb_FM_ProfitLossDetail)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_FM_ProfitLossDetail>(entity as tb_FM_ProfitLossDetail)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ProfitLossDetail_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .WhereCustom(useLike, dto);;
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_FM_ProfitLossDetail entity = model as tb_FM_ProfitLossDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_FM_ProfitLossDetail>(m => m.ProfitLossDetail_ID== entity.ProfitLossDetail_ID)
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
        
        
        
        public tb_FM_ProfitLossDetail AddReEntity(tb_FM_ProfitLossDetail entity)
        {
            tb_FM_ProfitLossDetail AddEntity =  _tb_FM_ProfitLossDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_FM_ProfitLossDetail> AddReEntityAsync(tb_FM_ProfitLossDetail entity)
        {
            tb_FM_ProfitLossDetail AddEntity = await _tb_FM_ProfitLossDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_FM_ProfitLossDetail entity)
        {
            long id = await _tb_FM_ProfitLossDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_FM_ProfitLossDetail> infos)
        {
            List<long> ids = await _tb_FM_ProfitLossDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_FM_ProfitLossDetail entity)
        {
            bool rs = await _tb_FM_ProfitLossDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ProfitLossDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_FM_ProfitLossDetail entity)
        {
            bool rs = await _tb_FM_ProfitLossDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_FM_ProfitLossDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ProfitLossDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_FM_ProfitLossDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_FM_ProfitLossDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_FM_ProfitLossDetail>> QueryAsync()
        {
            List<tb_FM_ProfitLossDetail> list = await  _tb_FM_ProfitLossDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        
        public virtual List<tb_FM_ProfitLossDetail> Query()
        {
            List<tb_FM_ProfitLossDetail> list =  _tb_FM_ProfitLossDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        
        public virtual List<tb_FM_ProfitLossDetail> Query(string wheresql)
        {
            List<tb_FM_ProfitLossDetail> list =  _tb_FM_ProfitLossDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_FM_ProfitLossDetail>> QueryAsync(string wheresql) 
        {
            List<tb_FM_ProfitLossDetail> list = await _tb_FM_ProfitLossDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_FM_ProfitLossDetail>> QueryAsync(Expression<Func<tb_FM_ProfitLossDetail, bool>> exp)
        {
            List<tb_FM_ProfitLossDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_ProfitLossDetail>> QueryByNavAsync()
        {
            List<tb_FM_ProfitLossDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>()
                               .Includes(t => t.tb_fm_profitloss )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_FM_ProfitLossDetail>> QueryByNavAsync(Expression<Func<tb_FM_ProfitLossDetail, bool>> exp)
        {
            List<tb_FM_ProfitLossDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>().Where(exp)
                               .Includes(t => t.tb_fm_profitloss )
                               .Includes(t => t.tb_proddetail )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_FM_ProfitLossDetail> QueryByNav(Expression<Func<tb_FM_ProfitLossDetail, bool>> exp)
        {
            List<tb_FM_ProfitLossDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>().Where(exp)
                            .Includes(t => t.tb_fm_profitloss )
                            .Includes(t => t.tb_proddetail )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_FM_ProfitLossDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>().WhereCustom(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_FM_ProfitLossDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_FM_ProfitLossDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_FM_ProfitLossDetail>().Where(w => w.ProfitLossDetail_ID == (long)id)
                             .Includes(t => t.tb_fm_profitloss )
                            .Includes(t => t.tb_proddetail )
                        

                                .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_FM_ProfitLossDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



