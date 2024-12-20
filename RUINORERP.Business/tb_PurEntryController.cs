
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
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付帐款也会跟着你的立帐方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。
    /// </summary>
    public partial class tb_PurEntryController<T>:BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_PurEntryServices _tb_PurEntryServices { get; set; }
       // private readonly ApplicationContext _appContext;
       
        public tb_PurEntryController(ILogger<tb_PurEntryController<T>> logger, IUnitOfWorkManage unitOfWorkManage,tb_PurEntryServices tb_PurEntryServices , ApplicationContext appContext = null): base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
           _unitOfWorkManage = unitOfWorkManage;
           _tb_PurEntryServices = tb_PurEntryServices;
            _appContext = appContext;
        }
      
        
        public ValidationResult Validator(tb_PurEntry info)
        {

           // tb_PurEntryValidator validator = new tb_PurEntryValidator();
           tb_PurEntryValidator validator = _appContext.GetRequiredService<tb_PurEntryValidator>();
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
        public async Task<ReturnResults<tb_PurEntry>> SaveOrUpdate(tb_PurEntry entity)
        {
            ReturnResults<tb_PurEntry> rr = new ReturnResults<tb_PurEntry>();
            tb_PurEntry Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurEntryID > 0)
                {
                    bool rs = await _tb_PurEntryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_PurEntryServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
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
            tb_PurEntry entity = model as tb_PurEntry;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.PurEntryID > 0)
                {
                    bool rs = await _tb_PurEntryServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_PurEntryServices.AddReEntityAsync(entity) as T ;
                    MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
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
            List<T> list = await _tb_PurEntryServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_PurEntry entity = item as tb_PurEntry;
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
            List<T> list = await _tb_PurEntryServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_PurEntry entity = item as tb_PurEntry;
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
            tb_PurEntry entity = model as tb_PurEntry;
            bool rs = await _tb_PurEntryServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntry>(entity);
            }
            return rs;
        }
        
        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs=false;
            List<tb_PurEntry> entitys = models as List<tb_PurEntry>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_PurEntry>(entitys).ExecuteCommandAsync();
            if (c>0)
            {
                rs=true;
                ////生成时暂时只考虑了一个主键的情况
                 long[] result = entitys.Select(e => e.PurEntryID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntry>(result);
            }
            return rs;
        }
        
        public override ValidationResult BaseValidator(T info)
        {
            //tb_PurEntryValidator validator = new tb_PurEntryValidator();
           tb_PurEntryValidator validator = _appContext.GetRequiredService<tb_PurEntryValidator>();
            ValidationResult results = validator.Validate(info as tb_PurEntry);
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
                tb_PurEntry entity = model as tb_PurEntry;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                       // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();
                
            if (entity.PurEntryID > 0)
            {
                rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_PurEntry>(entity as tb_PurEntry)
                        .Include(m => m.tb_PurEntryDetails)
                    .Include(m => m.tb_PurEntryRes)
                            .ExecuteCommandAsync();
         
        }
        else    
        {
            rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_PurEntry>(entity as tb_PurEntry)
                .Include(m => m.tb_PurEntryDetails)
                .Include(m => m.tb_PurEntryRes)
                                .ExecuteCommandAsync();
        }
        
                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T ;
                entity.PrimaryKeyID = entity.PurEntryID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                                .Includes(m => m.tb_PurEntryDetails)
                        .Includes(m => m.tb_PurEntryRes)
                                        .Where(useLike, dto);
            return await querySqlQueryable.ToListAsync()as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model) 
        {
            tb_PurEntry entity = model as tb_PurEntry;
             bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_PurEntry>(m => m.PurEntryID== entity.PurEntryID)
                                .Include(m => m.tb_PurEntryDetails)
                        .Include(m => m.tb_PurEntryRes)
                                        .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion
        
        
        
        public tb_PurEntry AddReEntity(tb_PurEntry entity)
        {
            tb_PurEntry AddEntity =  _tb_PurEntryServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
         public async Task<tb_PurEntry> AddReEntityAsync(tb_PurEntry entity)
        {
            tb_PurEntry AddEntity = await _tb_PurEntryServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }
        
        public async Task<long> AddAsync(tb_PurEntry entity)
        {
            long id = await _tb_PurEntryServices.Add(entity);
            if(id>0)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
            }
            return id;
        }
        
        public async Task<List<long>> AddAsync(List<tb_PurEntry> infos)
        {
            List<long> ids = await _tb_PurEntryServices.Add(infos);
            if(ids.Count>0)//成功的个数 这里缓存 对不对呢？
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(infos);
            }
            return ids;
        }
        
        
        public async Task<bool> DeleteAsync(tb_PurEntry entity)
        {
            bool rs = await _tb_PurEntryServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntry>(entity);
                
            }
            return rs;
        }
        
        public async Task<bool> UpdateAsync(tb_PurEntry entity)
        {
            bool rs = await _tb_PurEntryServices.Update(entity);
            if (rs)
            {
                 MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }
        
        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_PurEntryServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntry>(id);
            }
            return rs;
        }
        
         public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_PurEntryServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_PurEntry>(ids);
            }
            return rs;
        }
        
        public virtual async Task<List<tb_PurEntry>> QueryAsync()
        {
            List<tb_PurEntry> list = await  _tb_PurEntryServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        
        public virtual List<tb_PurEntry> Query()
        {
            List<tb_PurEntry> list =  _tb_PurEntryServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        
        public virtual List<tb_PurEntry> Query(string wheresql)
        {
            List<tb_PurEntry> list =  _tb_PurEntryServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        
        public virtual async Task<List<tb_PurEntry>> QueryAsync(string wheresql) 
        {
            List<tb_PurEntry> list = await _tb_PurEntryServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        


        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_PurEntry>> QueryAsync(Expression<Func<tb_PurEntry, bool>> exp)
        {
            List<tb_PurEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        
        
        
        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurEntry>> QueryByNavAsync()
        {
            List<tb_PurEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>()
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_purorder )
                                            .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_PurEntryRes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual async Task<List<tb_PurEntry>> QueryByNavAsync(Expression<Func<tb_PurEntry, bool>> exp)
        {
            List<tb_PurEntry> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>().Where(exp)
                               .Includes(t => t.tb_customervendor )
                               .Includes(t => t.tb_employee )
                               .Includes(t => t.tb_paymentmethod )
                               .Includes(t => t.tb_department )
                               .Includes(t => t.tb_purorder )
                                            .Includes(t => t.tb_PurEntryDetails )
                                .Includes(t => t.tb_PurEntryRes )
                        .ToListAsync();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        
        
        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
         public virtual List<tb_PurEntry> QueryByNav(Expression<Func<tb_PurEntry, bool>> exp)
        {
            List<tb_PurEntry> list = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>().Where(exp)
                            .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_purorder )
                                        .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_PurEntryRes )
                        .ToList();
            
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            
            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(list);
            return list;
        }
        
        

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_PurEntry>> QueryByAdvancedAsync(bool useLike,object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>().Where(useLike,dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_PurEntryServices.QueryByIdAsync(id) as T;
            return entity;
        }
        
        
        
        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_PurEntry entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_PurEntry>().Where(w => w.PurEntryID == (long)id)
                             .Includes(t => t.tb_customervendor )
                            .Includes(t => t.tb_employee )
                            .Includes(t => t.tb_paymentmethod )
                            .Includes(t => t.tb_department )
                            .Includes(t => t.tb_purorder )
                                        .Includes(t => t.tb_PurEntryDetails )
                            .Includes(t => t.tb_PurEntryRes )
                        .FirstAsync();
            if(entity!=null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_PurEntry>(entity);
            return entity as T;
        }
        
        
        
        
        
        
    }
}



