
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 18:02:07
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
    /// 外发加工订单表
    /// </summary>
    public partial class tb_Outsourcing_orderController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_Outsourcing_orderServices _tb_Outsourcing_orderServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_Outsourcing_orderController(ILogger<tb_Outsourcing_orderController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_Outsourcing_orderServices tb_Outsourcing_orderServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_Outsourcing_orderServices = tb_Outsourcing_orderServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_Outsourcing_order info)
        {

           // tb_Outsourcing_orderValidator validator = new tb_Outsourcing_orderValidator();
           tb_Outsourcing_orderValidator validator = _appContext.GetRequiredService<tb_Outsourcing_orderValidator>();
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
        public async Task<ReturnResults<tb_Outsourcing_order>> SaveOrUpdate(tb_Outsourcing_order entity)
        {
            ReturnResults<tb_Outsourcing_order> rr = new ReturnResults<tb_Outsourcing_order>();
            tb_Outsourcing_order Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_Outsourcing_orderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_Outsourcing_orderServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
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
            tb_Outsourcing_order entity = model as tb_Outsourcing_order;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.ID > 0)
                {
                    bool rs = await _tb_Outsourcing_orderServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_Outsourcing_orderServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
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
            List<T> list = await _tb_Outsourcing_orderServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_Outsourcing_order entity = item as tb_Outsourcing_order;
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
            List<T> list = await _tb_Outsourcing_orderServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_Outsourcing_order entity = item as tb_Outsourcing_order;
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
            tb_Outsourcing_order entity = model as tb_Outsourcing_order;
            bool rs = await _tb_Outsourcing_orderServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_Outsourcing_order>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_Outsourcing_order> entitys = models as List<tb_Outsourcing_order>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_Outsourcing_order>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_Outsourcing_order>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_Outsourcing_orderValidator validator = new tb_Outsourcing_orderValidator();
           tb_Outsourcing_orderValidator validator = _appContext.GetRequiredService<tb_Outsourcing_orderValidator>();
            ValidationResult results = validator.Validate(info as tb_Outsourcing_order);
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
            try
            {
                 //缓存当前编辑的对象。如果撤销就回原来的值
                T oldobj = CloneHelper.DeepCloneObject<T>((T)model);
                tb_Outsourcing_order entity = model as tb_Outsourcing_order;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.ID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_Outsourcing_order>(entity as tb_Outsourcing_order)
                    //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_Outsourcing_order>(entity as tb_Outsourcing_order)
                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_Outsourcing_order entity = model as tb_Outsourcing_order;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_Outsourcing_order>(m => m.ID== entity.ID)
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
        
        
        
        public tb_Outsourcing_order AddReEntity(tb_Outsourcing_order entity)
        {
            tb_Outsourcing_order AddEntity =  _tb_Outsourcing_orderServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_Outsourcing_order> AddReEntityAsync(tb_Outsourcing_order entity)
        {
            tb_Outsourcing_order AddEntity = await _tb_Outsourcing_orderServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_Outsourcing_order entity)
        {
            long id = await _tb_Outsourcing_orderServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_Outsourcing_order> infos)
        {
            List<long> ids = await _tb_Outsourcing_orderServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_Outsourcing_order entity)
        {
            bool rs = await _tb_Outsourcing_orderServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Outsourcing_order>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_Outsourcing_order entity)
        {
            bool rs = await _tb_Outsourcing_orderServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_Outsourcing_orderServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Outsourcing_order>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_Outsourcing_orderServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_Outsourcing_order>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_Outsourcing_order>> QueryAsync()
        {
            List<tb_Outsourcing_order> list = await  _tb_Outsourcing_orderServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        
        public virtual List<tb_Outsourcing_order> Query()
        {
            List<tb_Outsourcing_order> list =  _tb_Outsourcing_orderServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        
        public virtual List<tb_Outsourcing_order> Query(string wheresql)
        {
            List<tb_Outsourcing_order> list =  _tb_Outsourcing_orderServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        
        public virtual async Task<List<tb_Outsourcing_order>> QueryAsync(string wheresql) 
        {
            List<tb_Outsourcing_order> list = await _tb_Outsourcing_orderServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_Outsourcing_order>> QueryAsync(Expression<Func<tb_Outsourcing_order, bool>> exp)
        {
            List<tb_Outsourcing_order> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Outsourcing_order>> QueryByNavAsync()
        {
            List<tb_Outsourcing_order> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>()
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_Outsourcing_order>> QueryByNavAsync(Expression<Func<tb_Outsourcing_order, bool>> exp)
        {
            List<tb_Outsourcing_order> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>().Where(exp)
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_Outsourcing_order> QueryByNav(Expression<Func<tb_Outsourcing_order, bool>> exp)
        {
            List<tb_Outsourcing_order> list = _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>().Where(exp)
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_Outsourcing_order>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_Outsourcing_orderServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_Outsourcing_order entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_Outsourcing_order>().Where(w => w.ID == (long)id)
                                     .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_Outsourcing_order>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



