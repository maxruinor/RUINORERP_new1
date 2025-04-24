
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/24/2025 10:43:38
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
    /// 项目组与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面
    /// </summary>
    public partial class tb_ProjectGroupAccountMapperController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProjectGroupAccountMapperServices _tb_ProjectGroupAccountMapperServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_ProjectGroupAccountMapperController(ILogger<tb_ProjectGroupAccountMapperController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_ProjectGroupAccountMapperServices tb_ProjectGroupAccountMapperServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_ProjectGroupAccountMapperServices = tb_ProjectGroupAccountMapperServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_ProjectGroupAccountMapper info)
        {

           // tb_ProjectGroupAccountMapperValidator validator = new tb_ProjectGroupAccountMapperValidator();
           tb_ProjectGroupAccountMapperValidator validator = _appContext.GetRequiredService<tb_ProjectGroupAccountMapperValidator>();
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
        public async Task<ReturnResults<tb_ProjectGroupAccountMapper>> SaveOrUpdate(tb_ProjectGroupAccountMapper entity)
        {
            ReturnResults<tb_ProjectGroupAccountMapper> rr = new ReturnResults<tb_ProjectGroupAccountMapper>();
            tb_ProjectGroupAccountMapper Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PGAMID > 0)
                {
                    bool rs = await _tb_ProjectGroupAccountMapperServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProjectGroupAccountMapperServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
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
            tb_ProjectGroupAccountMapper entity = model as tb_ProjectGroupAccountMapper;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PGAMID > 0)
                {
                    bool rs = await _tb_ProjectGroupAccountMapperServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProjectGroupAccountMapperServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
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
            List<T> list = await _tb_ProjectGroupAccountMapperServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProjectGroupAccountMapper entity = item as tb_ProjectGroupAccountMapper;
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
            List<T> list = await _tb_ProjectGroupAccountMapperServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProjectGroupAccountMapper entity = item as tb_ProjectGroupAccountMapper;
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
            tb_ProjectGroupAccountMapper entity = model as tb_ProjectGroupAccountMapper;
            bool rs = await _tb_ProjectGroupAccountMapperServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupAccountMapper>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_ProjectGroupAccountMapper> entitys = models as List<tb_ProjectGroupAccountMapper>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProjectGroupAccountMapper>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PGAMID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupAccountMapper>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProjectGroupAccountMapperValidator validator = new tb_ProjectGroupAccountMapperValidator();
           tb_ProjectGroupAccountMapperValidator validator = _appContext.GetRequiredService<tb_ProjectGroupAccountMapperValidator>();
            ValidationResult results = validator.Validate(info as tb_ProjectGroupAccountMapper);
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

                tb_ProjectGroupAccountMapper entity = model as tb_ProjectGroupAccountMapper;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PGAMID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_ProjectGroupAccountMapper>(entity as tb_ProjectGroupAccountMapper)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_ProjectGroupAccountMapper>(entity as tb_ProjectGroupAccountMapper)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PGAMID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_ProjectGroupAccountMapper entity = model as tb_ProjectGroupAccountMapper;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProjectGroupAccountMapper>(m => m.PGAMID== entity.PGAMID)
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
        
        
        
        public tb_ProjectGroupAccountMapper AddReEntity(tb_ProjectGroupAccountMapper entity)
        {
            tb_ProjectGroupAccountMapper AddEntity =  _tb_ProjectGroupAccountMapperServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_ProjectGroupAccountMapper> AddReEntityAsync(tb_ProjectGroupAccountMapper entity)
        {
            tb_ProjectGroupAccountMapper AddEntity = await _tb_ProjectGroupAccountMapperServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_ProjectGroupAccountMapper entity)
        {
            long id = await _tb_ProjectGroupAccountMapperServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_ProjectGroupAccountMapper> infos)
        {
            List<long> ids = await _tb_ProjectGroupAccountMapperServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_ProjectGroupAccountMapper entity)
        {
            bool rs = await _tb_ProjectGroupAccountMapperServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupAccountMapper>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_ProjectGroupAccountMapper entity)
        {
            bool rs = await _tb_ProjectGroupAccountMapperServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProjectGroupAccountMapperServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupAccountMapper>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProjectGroupAccountMapperServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProjectGroupAccountMapper>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_ProjectGroupAccountMapper>> QueryAsync()
        {
            List<tb_ProjectGroupAccountMapper> list = await  _tb_ProjectGroupAccountMapperServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        
        public virtual List<tb_ProjectGroupAccountMapper> Query()
        {
            List<tb_ProjectGroupAccountMapper> list =  _tb_ProjectGroupAccountMapperServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        
        public virtual List<tb_ProjectGroupAccountMapper> Query(string wheresql)
        {
            List<tb_ProjectGroupAccountMapper> list =  _tb_ProjectGroupAccountMapperServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        
        public virtual async Task<List<tb_ProjectGroupAccountMapper>> QueryAsync(string wheresql) 
        {
            List<tb_ProjectGroupAccountMapper> list = await _tb_ProjectGroupAccountMapperServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProjectGroupAccountMapper>> QueryAsync(Expression<Func<tb_ProjectGroupAccountMapper, bool>> exp)
        {
            List<tb_ProjectGroupAccountMapper> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProjectGroupAccountMapper>> QueryByNavAsync()
        {
            List<tb_ProjectGroupAccountMapper> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>()
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_ProjectGroupAccountMapper>> QueryByNavAsync(Expression<Func<tb_ProjectGroupAccountMapper, bool>> exp)
        {
            List<tb_ProjectGroupAccountMapper> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>().Where(exp)
                               .Includes(t => t.tb_fm_account )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_ProjectGroupAccountMapper> QueryByNav(Expression<Func<tb_ProjectGroupAccountMapper, bool>> exp)
        {
            List<tb_ProjectGroupAccountMapper> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>().Where(exp)
                            .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_projectgroup )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProjectGroupAccountMapper>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProjectGroupAccountMapperServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProjectGroupAccountMapper entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProjectGroupAccountMapper>().Where(w => w.PGAMID == (long)id)
                             .Includes(t => t.tb_fm_account )
                            .Includes(t => t.tb_projectgroup )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProjectGroupAccountMapper>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



