
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/20/2024 10:31:31
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
    /// 银行账号信息表
    /// </summary>
    public partial class tb_BankAccountController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_BankAccountServices _tb_BankAccountServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_BankAccountController(ILogger<BaseController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_BankAccountServices tb_BankAccountServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_BankAccountServices = tb_BankAccountServices;
            _appContext = appContext;
        }
      
        
        
        
         public ValidationResult Validator(tb_BankAccount info)
        {
            tb_BankAccountValidator validator = new tb_BankAccountValidator();
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
        public async Task<ReturnResults<tb_BankAccount>> SaveOrUpdate(tb_BankAccount entity)
        {
            ReturnResults<tb_BankAccount> rr = new ReturnResults<tb_BankAccount>();
            tb_BankAccount Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BankAccount_id > 0)
                {
                    bool rs = await _tb_BankAccountServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_BankAccountServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
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
            tb_BankAccount entity = model as tb_BankAccount;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.BankAccount_id > 0)
                {
                    bool rs = await _tb_BankAccountServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_BankAccountServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
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
            List<T> list = await _tb_BankAccountServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_BankAccount entity = item as tb_BankAccount;
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
            List<T> list = await _tb_BankAccountServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_BankAccount entity = item as tb_BankAccount;
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
            tb_BankAccount entity = model as tb_BankAccount;
            bool rs = await _tb_BankAccountServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_BankAccount>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_BankAccount> entitys = models as List<tb_BankAccount>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_BankAccount>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.BankAccount_id).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_BankAccount>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            tb_BankAccountValidator validator = new tb_BankAccountValidator();
            ValidationResult results = validator.Validate(info as tb_BankAccount);
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
                tb_BankAccount entity = model as tb_BankAccount;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
       
            if (entity.BankAccount_id > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_BankAccount>(entity as tb_BankAccount)
                        .Include(m => m.tb_CustomerVendors)
                    .Include(m => m.tb_Employees)
                    .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_BankAccount>(entity as tb_BankAccount)
                .Include(m => m.tb_CustomerVendors)
                .Include(m => m.tb_Employees)
                        .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.BankAccount_id;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>()
                                .Includes(m => m.tb_CustomerVendors)
                        .Includes(m => m.tb_Employees)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_BankAccount entity = model as tb_BankAccount;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_BankAccount>(m => m.BankAccount_id== entity.BankAccount_id)
                                .Include(m => m.tb_CustomerVendors)
                        .Include(m => m.tb_Employees)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_BankAccount AddReEntity(tb_BankAccount entity)
        {
            tb_BankAccount AddEntity =  _tb_BankAccountServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_BankAccount> AddReEntityAsync(tb_BankAccount entity)
        {
            tb_BankAccount AddEntity = await _tb_BankAccountServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_BankAccount entity)
        {
            long id = await _tb_BankAccountServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_BankAccount> infos)
        {
            List<long> ids = await _tb_BankAccountServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_BankAccount entity)
        {
            bool rs = await _tb_BankAccountServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BankAccount>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_BankAccount entity)
        {
            bool rs = await _tb_BankAccountServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_BankAccountServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BankAccount>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_BankAccountServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_BankAccount>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_BankAccount>> QueryAsync()
        {
            List<tb_BankAccount> list = await  _tb_BankAccountServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        
        public virtual List<tb_BankAccount> Query()
        {
            List<tb_BankAccount> list =  _tb_BankAccountServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        
        public virtual List<tb_BankAccount> Query(string wheresql)
        {
            List<tb_BankAccount> list =  _tb_BankAccountServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        
        public virtual async Task<List<tb_BankAccount>> QueryAsync(string wheresql) 
        {
            List<tb_BankAccount> list = await _tb_BankAccountServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_BankAccount>> QueryAsync(Expression<Func<tb_BankAccount, bool>> exp)
        {
            List<tb_BankAccount> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BankAccount>> QueryByNavAsync()
        {
            List<tb_BankAccount> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>()
                                            .Includes(t => t.tb_CustomerVendors )
                                .Includes(t => t.tb_Employees )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_BankAccount>> QueryByNavAsync(Expression<Func<tb_BankAccount, bool>> exp)
        {
            List<tb_BankAccount> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>().Where(exp)
                                            .Includes(t => t.tb_CustomerVendors )
                                .Includes(t => t.tb_Employees )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_BankAccount> QueryByNav(Expression<Func<tb_BankAccount, bool>> exp)
        {
            List<tb_BankAccount> list = _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>().Where(exp)
                                        .Includes(t => t.tb_CustomerVendors )
                            .Includes(t => t.tb_Employees )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_BankAccount>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_BankAccountServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_BankAccount entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_BankAccount>().Where(w => w.BankAccount_id == (long)id)
                                         .Includes(t => t.tb_CustomerVendors )
                            .Includes(t => t.tb_Employees )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_BankAccount>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



