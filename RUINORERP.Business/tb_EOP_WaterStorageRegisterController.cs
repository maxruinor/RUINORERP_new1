
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/17/2025 16:59:40
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
    /// 蓄水登记表
    /// </summary>
    public partial class tb_EOP_WaterStorageRegisterController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_EOP_WaterStorageRegisterServices _tb_EOP_WaterStorageRegisterServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_EOP_WaterStorageRegisterController(ILogger<tb_EOP_WaterStorageRegisterController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_EOP_WaterStorageRegisterServices tb_EOP_WaterStorageRegisterServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_EOP_WaterStorageRegisterServices = tb_EOP_WaterStorageRegisterServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_EOP_WaterStorageRegister info)
        {

           // tb_EOP_WaterStorageRegisterValidator validator = new tb_EOP_WaterStorageRegisterValidator();
           tb_EOP_WaterStorageRegisterValidator validator = _appContext.GetRequiredService<tb_EOP_WaterStorageRegisterValidator>();
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
        public async Task<ReturnResults<tb_EOP_WaterStorageRegister>> SaveOrUpdate(tb_EOP_WaterStorageRegister entity)
        {
            ReturnResults<tb_EOP_WaterStorageRegister> rr = new ReturnResults<tb_EOP_WaterStorageRegister>();
            tb_EOP_WaterStorageRegister Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.WSR_ID > 0)
                {
                    bool rs = await _tb_EOP_WaterStorageRegisterServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_EOP_WaterStorageRegisterServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
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
            tb_EOP_WaterStorageRegister entity = model as tb_EOP_WaterStorageRegister;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.WSR_ID > 0)
                {
                    bool rs = await _tb_EOP_WaterStorageRegisterServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_EOP_WaterStorageRegisterServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
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
            List<T> list = await _tb_EOP_WaterStorageRegisterServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_EOP_WaterStorageRegister entity = item as tb_EOP_WaterStorageRegister;
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
            List<T> list = await _tb_EOP_WaterStorageRegisterServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_EOP_WaterStorageRegister entity = item as tb_EOP_WaterStorageRegister;
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
            tb_EOP_WaterStorageRegister entity = model as tb_EOP_WaterStorageRegister;
            bool rs = await _tb_EOP_WaterStorageRegisterServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_EOP_WaterStorageRegister>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_EOP_WaterStorageRegister> entitys = models as List<tb_EOP_WaterStorageRegister>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_EOP_WaterStorageRegister>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.WSR_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_EOP_WaterStorageRegister>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_EOP_WaterStorageRegisterValidator validator = new tb_EOP_WaterStorageRegisterValidator();
           tb_EOP_WaterStorageRegisterValidator validator = _appContext.GetRequiredService<tb_EOP_WaterStorageRegisterValidator>();
            ValidationResult results = validator.Validate(info as tb_EOP_WaterStorageRegister);
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

                tb_EOP_WaterStorageRegister entity = model as tb_EOP_WaterStorageRegister;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.WSR_ID > 0)
            {
            
                                 var result= await _unitOfWorkManage.GetDbClient().Updateable<tb_EOP_WaterStorageRegister>(entity as tb_EOP_WaterStorageRegister)
                    .ExecuteCommandAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
            }
        else    
        {
                                  var result= await _unitOfWorkManage.GetDbClient().Insertable<tb_EOP_WaterStorageRegister>(entity as tb_EOP_WaterStorageRegister)
                    .ExecuteReturnSnowflakeIdAsync();
                    if (result > 0)
                    {
                        rs = true;
                    }
                                              
                     
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.WSR_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>()
                                //这里一般是子表，或没有一对多外键的情况 ，用自动的只是为了语法正常一般不会调用这个方法
                .IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                                .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_EOP_WaterStorageRegister entity = model as tb_EOP_WaterStorageRegister;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_EOP_WaterStorageRegister>(m => m.WSR_ID== entity.WSR_ID)
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
        
        
        
        public tb_EOP_WaterStorageRegister AddReEntity(tb_EOP_WaterStorageRegister entity)
        {
            tb_EOP_WaterStorageRegister AddEntity =  _tb_EOP_WaterStorageRegisterServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_EOP_WaterStorageRegister> AddReEntityAsync(tb_EOP_WaterStorageRegister entity)
        {
            tb_EOP_WaterStorageRegister AddEntity = await _tb_EOP_WaterStorageRegisterServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_EOP_WaterStorageRegister entity)
        {
            long id = await _tb_EOP_WaterStorageRegisterServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_EOP_WaterStorageRegister> infos)
        {
            List<long> ids = await _tb_EOP_WaterStorageRegisterServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_EOP_WaterStorageRegister entity)
        {
            bool rs = await _tb_EOP_WaterStorageRegisterServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_EOP_WaterStorageRegister>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_EOP_WaterStorageRegister entity)
        {
            bool rs = await _tb_EOP_WaterStorageRegisterServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_EOP_WaterStorageRegisterServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_EOP_WaterStorageRegister>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_EOP_WaterStorageRegisterServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_EOP_WaterStorageRegister>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_EOP_WaterStorageRegister>> QueryAsync()
        {
            List<tb_EOP_WaterStorageRegister> list = await  _tb_EOP_WaterStorageRegisterServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        
        public virtual List<tb_EOP_WaterStorageRegister> Query()
        {
            List<tb_EOP_WaterStorageRegister> list =  _tb_EOP_WaterStorageRegisterServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        
        public virtual List<tb_EOP_WaterStorageRegister> Query(string wheresql)
        {
            List<tb_EOP_WaterStorageRegister> list =  _tb_EOP_WaterStorageRegisterServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        
        public virtual async Task<List<tb_EOP_WaterStorageRegister>> QueryAsync(string wheresql) 
        {
            List<tb_EOP_WaterStorageRegister> list = await _tb_EOP_WaterStorageRegisterServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_EOP_WaterStorageRegister>> QueryAsync(Expression<Func<tb_EOP_WaterStorageRegister, bool>> exp)
        {
            List<tb_EOP_WaterStorageRegister> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_EOP_WaterStorageRegister>> QueryByNavAsync()
        {
            List<tb_EOP_WaterStorageRegister> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>()
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_EOP_WaterStorageRegister>> QueryByNavAsync(Expression<Func<tb_EOP_WaterStorageRegister, bool>> exp)
        {
            List<tb_EOP_WaterStorageRegister> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>().Where(exp)
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_projectgroup )
                                    .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_EOP_WaterStorageRegister> QueryByNav(Expression<Func<tb_EOP_WaterStorageRegister, bool>> exp)
        {
            List<tb_EOP_WaterStorageRegister> list = _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>().Where(exp)
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                    .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_EOP_WaterStorageRegister>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_EOP_WaterStorageRegisterServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_EOP_WaterStorageRegister entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_EOP_WaterStorageRegister>().Where(w => w.WSR_ID == (long)id)
                             .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_projectgroup )
                                    .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_EOP_WaterStorageRegister>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



