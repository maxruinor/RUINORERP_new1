
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/15/2024 19:01:20
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
    /// 销售退货翻新物料明细表
    /// </summary>
    public partial class tb_SaleOutReRefurbishedMaterialsDetailController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_SaleOutReRefurbishedMaterialsDetailServices _tb_SaleOutReRefurbishedMaterialsDetailServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_SaleOutReRefurbishedMaterialsDetailController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_SaleOutReRefurbishedMaterialsDetailServices tb_SaleOutReRefurbishedMaterialsDetailServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_SaleOutReRefurbishedMaterialsDetailServices = tb_SaleOutReRefurbishedMaterialsDetailServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_SaleOutReRefurbishedMaterialsDetail info)
        {
            tb_SaleOutReRefurbishedMaterialsDetailValidator validator = new tb_SaleOutReRefurbishedMaterialsDetailValidator();
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
        public async Task<ReturnResults<tb_SaleOutReRefurbishedMaterialsDetail>> SaveOrUpdate(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
            ReturnResults<tb_SaleOutReRefurbishedMaterialsDetail> rr = new ReturnResults<tb_SaleOutReRefurbishedMaterialsDetail>();
            tb_SaleOutReRefurbishedMaterialsDetail Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SOutReturnDetail_ID > 0)
                {
                    bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_SaleOutReRefurbishedMaterialsDetailServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
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
            tb_SaleOutReRefurbishedMaterialsDetail entity = model as tb_SaleOutReRefurbishedMaterialsDetail;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.SOutReturnDetail_ID > 0)
                {
                    bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_SaleOutReRefurbishedMaterialsDetailServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
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
            List<T> list = await _tb_SaleOutReRefurbishedMaterialsDetailServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_SaleOutReRefurbishedMaterialsDetail entity = item as tb_SaleOutReRefurbishedMaterialsDetail;
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
            List<T> list = await _tb_SaleOutReRefurbishedMaterialsDetailServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_SaleOutReRefurbishedMaterialsDetail entity = item as tb_SaleOutReRefurbishedMaterialsDetail;
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
            tb_SaleOutReRefurbishedMaterialsDetail entity = model as tb_SaleOutReRefurbishedMaterialsDetail;
            bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_SaleOutReRefurbishedMaterialsDetail> entitys = models as List<tb_SaleOutReRefurbishedMaterialsDetail>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_SaleOutReRefurbishedMaterialsDetail>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.SOutReturnDetail_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_SaleOutReRefurbishedMaterialsDetailValidator validator = new tb_SaleOutReRefurbishedMaterialsDetailValidator();
            ValidationResult results = validator.Validate(info as tb_SaleOutReRefurbishedMaterialsDetail);
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
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_SaleOutReRefurbishedMaterialsDetail entity = model as tb_SaleOutReRefurbishedMaterialsDetail;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.SOutReturnDetail_ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_SaleOutReRefurbishedMaterialsDetail>(entity as tb_SaleOutReRefurbishedMaterialsDetail)
            //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_SaleOutReRefurbishedMaterialsDetail>(entity as tb_SaleOutReRefurbishedMaterialsDetail)
        //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.SOutReturnDetail_ID;
                rsms.Succeeded = rs;
            }
            catch (Exception ex)
            {
                //出错后，取消生成的ID等值
                command.Undo();
                _logger.Error(ex);
                _unitOfWorkManage.RollbackTran();
                //_logger.Error("BaseSaveOrUpdateWithChild事务回滚");
                // rr.ErrorMsg = "事务回滚=>" + ex.Message;
                rsms.ErrorMsg = ex.Message;
                rsms.Succeeded = false;
            }

            return rsms;
        }
        
        #endregion
        
        
        #region override mothed

        public async override Task<List<T>> BaseQueryByAdvancedNavAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_SaleOutReRefurbishedMaterialsDetail entity = model as tb_SaleOutReRefurbishedMaterialsDetail;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_SaleOutReRefurbishedMaterialsDetail>(m => m.SOutReturnDetail_ID== entity.SOutReturnDetail_ID)
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
        
        
        
        public tb_SaleOutReRefurbishedMaterialsDetail AddReEntity(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
            tb_SaleOutReRefurbishedMaterialsDetail AddEntity =  _tb_SaleOutReRefurbishedMaterialsDetailServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_SaleOutReRefurbishedMaterialsDetail> AddReEntityAsync(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
            tb_SaleOutReRefurbishedMaterialsDetail AddEntity = await _tb_SaleOutReRefurbishedMaterialsDetailServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
            long id = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_SaleOutReRefurbishedMaterialsDetail> infos)
        {
            List<long> ids = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
            bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_SaleOutReRefurbishedMaterialsDetail entity)
        {
            bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_SaleOutReRefurbishedMaterialsDetailServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_SaleOutReRefurbishedMaterialsDetail>> QueryAsync()
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list = await  _tb_SaleOutReRefurbishedMaterialsDetailServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        
        public virtual List<tb_SaleOutReRefurbishedMaterialsDetail> Query()
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list =  _tb_SaleOutReRefurbishedMaterialsDetailServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        
        public virtual List<tb_SaleOutReRefurbishedMaterialsDetail> Query(string wheresql)
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list =  _tb_SaleOutReRefurbishedMaterialsDetailServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        
        public virtual async Task<List<tb_SaleOutReRefurbishedMaterialsDetail>> QueryAsync(string wheresql) 
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list = await _tb_SaleOutReRefurbishedMaterialsDetailServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_SaleOutReRefurbishedMaterialsDetail>> QueryAsync(Expression<Func<tb_SaleOutReRefurbishedMaterialsDetail, bool>> exp)
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOutReRefurbishedMaterialsDetail>> QueryByNavAsync()
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_SaleOutReRefurbishedMaterialsDetail>> QueryByNavAsync(Expression<Func<tb_SaleOutReRefurbishedMaterialsDetail, bool>> exp)
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_SaleOutReRefurbishedMaterialsDetail> QueryByNav(Expression<Func<tb_SaleOutReRefurbishedMaterialsDetail, bool>> exp)
        {
            List<tb_SaleOutReRefurbishedMaterialsDetail> list = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_SaleOutReRefurbishedMaterialsDetail>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_SaleOutReRefurbishedMaterialsDetailServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_SaleOutReRefurbishedMaterialsDetail entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_SaleOutReRefurbishedMaterialsDetail>().Where(w => w.SOutReturnDetail_ID == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_SaleOutReRefurbishedMaterialsDetail>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



