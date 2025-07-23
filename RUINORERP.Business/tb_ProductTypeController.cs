
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:39:50
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
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容
    /// </summary>
    public partial class tb_ProductTypeController<T> : BaseController<T> where T : class
    {
        /// <summary>
        /// 本为私有修改为公有，暴露出来方便使用
        /// </summary>
        //public readonly IUnitOfWorkManage _unitOfWorkManage;
        //public readonly ILogger<BaseController<T>> _logger;
        public Itb_ProductTypeServices _tb_ProductTypeServices { get; set; }
        // private readonly ApplicationContext _appContext;

        public tb_ProductTypeController(ILogger<tb_ProductTypeController<T>> logger, IUnitOfWorkManage unitOfWorkManage, tb_ProductTypeServices tb_ProductTypeServices, ApplicationContext appContext = null) : base(logger, unitOfWorkManage, appContext)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            _tb_ProductTypeServices = tb_ProductTypeServices;
            _appContext = appContext;
        }


        public ValidationResult Validator(tb_ProductType info)
        {

            // tb_ProductTypeValidator validator = new tb_ProductTypeValidator();
            tb_ProductTypeValidator validator = _appContext.GetRequiredService<tb_ProductTypeValidator>();
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
        public async Task<ReturnResults<tb_ProductType>> SaveOrUpdate(tb_ProductType entity)
        {
            ReturnResults<tb_ProductType> rr = new ReturnResults<tb_ProductType>();
            tb_ProductType Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Type_ID > 0)
                {
                    bool rs = await _tb_ProductTypeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
                    }
                    Returnobj = entity;
                }
                else
                {
                    Returnobj = await _tb_ProductTypeServices.AddReEntityAsync(entity);
                    MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
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
        public async override Task<ReturnResults<T>> BaseSaveOrUpdate(T model)
        {
            ReturnResults<T> rr = new ReturnResults<T>();
            tb_ProductType entity = model as tb_ProductType;
            T Returnobj;
            try
            {
                //生成时暂时只考虑了一个主键的情况
                if (entity.Type_ID > 0)
                {
                    bool rs = await _tb_ProductTypeServices.Update(entity);
                    if (rs)
                    {
                        MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
                    }
                    Returnobj = entity as T;
                }
                else
                {
                    Returnobj = await _tb_ProductTypeServices.AddReEntityAsync(entity) as T;
                    MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
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
            List<T> list = await _tb_ProductTypeServices.QueryAsync(wheresql) as List<T>;
            foreach (var item in list)
            {
                tb_ProductType entity = item as tb_ProductType;
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
            List<T> list = await _tb_ProductTypeServices.QueryAsync() as List<T>;
            foreach (var item in list)
            {
                tb_ProductType entity = item as tb_ProductType;
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
            tb_ProductType entity = model as tb_ProductType;
            bool rs = await _tb_ProductTypeServices.Delete(entity);
            if (rs)
            {
                ////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<tb_ProductType>(entity);
            }
            return rs;
        }

        public async override Task<bool> BaseDeleteAsync(List<T> models)
        {
            bool rs = false;
            List<tb_ProductType> entitys = models as List<tb_ProductType>;
            int c = await _unitOfWorkManage.GetDbClient().Deleteable<tb_ProductType>(entitys).ExecuteCommandAsync();
            if (c > 0)
            {
                rs = true;
                ////生成时暂时只考虑了一个主键的情况
                long[] result = entitys.Select(e => e.Type_ID).ToArray();
                MyCacheManager.Instance.DeleteEntityList<tb_ProductType>(result);
            }
            return rs;
        }

        public override ValidationResult BaseValidator(T info)
        {
            //tb_ProductTypeValidator validator = new tb_ProductTypeValidator();
            tb_ProductTypeValidator validator = _appContext.GetRequiredService<tb_ProductTypeValidator>();
            ValidationResult results = validator.Validate(info as tb_ProductType);
            return results;
        }


        public async override Task<List<T>> BaseQueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<T>().WhereCustom(useLike, dto);
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

                tb_ProductType entity = model as tb_ProductType;
                command.UndoOperation = delegate ()
                {
                    //Undo操作会执行到的代码
                    CloneHelper.SetValues<T>(entity, oldobj);
                };
                // 开启事务，保证数据一致性
                _unitOfWorkManage.BeginTran();

                if (entity.Type_ID > 0)
                {

                    rs = await _unitOfWorkManage.GetDbClient().UpdateNav<tb_ProductType>(entity as tb_ProductType)
               .Include(m => m.tb_Prods)
                       .Include(t => t.tb_ProdConversionDetails_to)
                       .Include(t => t.tb_ProdConversionDetails_from)
           .Include(m => m.tb_ManufacturingOrders)
           .ExecuteCommandAsync();
                }
                else
                {
                    rs = await _unitOfWorkManage.GetDbClient().InsertNav<tb_ProductType>(entity as tb_ProductType)
            .Include(m => m.tb_Prods)
            .Include(m => m.tb_ProdConversionDetails_from)
            .Include(m => m.tb_ProdConversionDetails_to)
            .Include(m => m.tb_ManufacturingOrders)

            .ExecuteCommandAsync();


                }

                // 注意信息的完整性
                _unitOfWorkManage.CommitTran();
                rsms.ReturnObject = entity as T;
                entity.PrimaryKeyID = entity.Type_ID;
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
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>()
                                .Includes(m => m.tb_Prods)
                                 .Includes(t => t.tb_ProdConversionDetails_to)
                                .Includes(t => t.tb_ProdConversionDetails_from)
                        .Includes(m => m.tb_ManufacturingOrders)
                                        .WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync() as List<T>;
        }


        public async override Task<bool> BaseDeleteByNavAsync(T model)
        {
            tb_ProductType entity = model as tb_ProductType;
            bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_ProductType>(m => m.Type_ID == entity.Type_ID)
                               .Include(m => m.tb_Prods)
               .Include(m => m.tb_ProdConversionDetails_from)
               .Include(m => m.tb_ProdConversionDetails_to)
                       .Include(m => m.tb_ManufacturingOrders)
                                       .ExecuteCommandAsync();
            if (rs)
            {
                //////生成时暂时只考虑了一个主键的情况
                MyCacheManager.Instance.DeleteEntityList<T>(model);
            }
            return rs;
        }
        #endregion



        public tb_ProductType AddReEntity(tb_ProductType entity)
        {
            tb_ProductType AddEntity = _tb_ProductTypeServices.AddReEntity(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<tb_ProductType> AddReEntityAsync(tb_ProductType entity)
        {
            tb_ProductType AddEntity = await _tb_ProductTypeServices.AddReEntityAsync(entity);
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(AddEntity);
            entity.ActionStatus = ActionStatus.无操作;
            return AddEntity;
        }

        public async Task<long> AddAsync(tb_ProductType entity)
        {
            long id = await _tb_ProductTypeServices.Add(entity);
            if (id > 0)
            {
                MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
            }
            return id;
        }

        public async Task<List<long>> AddAsync(List<tb_ProductType> infos)
        {
            List<long> ids = await _tb_ProductTypeServices.Add(infos);
            if (ids.Count > 0)//成功的个数 这里缓存 对不对呢？
            {
                MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(infos);
            }
            return ids;
        }


        public async Task<bool> DeleteAsync(tb_ProductType entity)
        {
            bool rs = await _tb_ProductTypeServices.Delete(entity);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProductType>(entity);

            }
            return rs;
        }

        public async Task<bool> UpdateAsync(tb_ProductType entity)
        {
            bool rs = await _tb_ProductTypeServices.Update(entity);
            if (rs)
            {
                MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
                entity.ActionStatus = ActionStatus.无操作;
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            bool rs = await _tb_ProductTypeServices.DeleteById(id);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProductType>(id);
            }
            return rs;
        }

        public async Task<bool> DeleteAsync(long[] ids)
        {
            bool rs = await _tb_ProductTypeServices.DeleteByIds(ids);
            if (rs)
            {
                MyCacheManager.Instance.DeleteEntityList<tb_ProductType>(ids);
            }
            return rs;
        }

        public virtual async Task<List<tb_ProductType>> QueryAsync()
        {
            List<tb_ProductType> list = await _tb_ProductTypeServices.QueryAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }

        public virtual List<tb_ProductType> Query()
        {
            List<tb_ProductType> list = _tb_ProductTypeServices.Query();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }

        public virtual List<tb_ProductType> Query(string wheresql)
        {
            List<tb_ProductType> list = _tb_ProductTypeServices.Query(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }

        public virtual async Task<List<tb_ProductType>> QueryAsync(string wheresql)
        {
            List<tb_ProductType> list = await _tb_ProductTypeServices.QueryAsync(wheresql);
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }



        /// <summary>
        /// 带参数查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public async Task<List<tb_ProductType>> QueryAsync(Expression<Func<tb_ProductType, bool>> exp)
        {
            List<tb_ProductType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>().Where(exp).ToListAsync();
            foreach (var item in list)
            {
                item.HasChanged = false;
            }
            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }



        /// <summary>
        /// 无参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_ProductType>> QueryByNavAsync()
        {
            List<tb_ProductType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>()
                                            .Includes(t => t.tb_Prods)

                                .Includes(t => t.tb_ProdConversionDetails_to)
                                .Includes(t => t.tb_ProdConversionDetails_from)
                                .Includes(t => t.tb_ManufacturingOrders)
                        .ToListAsync();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual async Task<List<tb_ProductType>> QueryByNavAsync(Expression<Func<tb_ProductType, bool>> exp)
        {
            List<tb_ProductType> list = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>().Where(exp)
                                            .Includes(t => t.tb_Prods)
                                .Includes(t => t.tb_ProdConversionDetails_to)
                                .Includes(t => t.tb_ProdConversionDetails_from)
                                .Includes(t => t.tb_ManufacturingOrders)
                        .ToListAsync();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }


        /// <summary>
        /// 带参数异步导航查询
        /// </summary>
        /// <returns>数据列表</returns>
        public virtual List<tb_ProductType> QueryByNav(Expression<Func<tb_ProductType, bool>> exp)
        {
            List<tb_ProductType> list = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>().Where(exp)
                                        .Includes(t => t.tb_Prods)
                                .Includes(t => t.tb_ProdConversionDetails_to)
                                .Includes(t => t.tb_ProdConversionDetails_from)
                            .Includes(t => t.tb_ManufacturingOrders)
                        .ToList();

            foreach (var item in list)
            {
                item.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(list);
            return list;
        }



        /// <summary>
        /// 高级查询
        /// </summary>
        /// <returns></returns>
        public async Task<List<tb_ProductType>> QueryByAdvancedAsync(bool useLike, object dto)
        {
            var querySqlQueryable = _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>().WhereCustom(useLike, dto);
            return await querySqlQueryable.ToListAsync();
        }



        public async override Task<T> BaseQueryByIdAsync(object id)
        {
            T entity = await _tb_ProductTypeServices.QueryByIdAsync(id) as T;
            return entity;
        }



        public override async Task<T> BaseQueryByIdNavAsync(object id)
        {
            tb_ProductType entity = await _unitOfWorkManage.GetDbClient().Queryable<tb_ProductType>().Where(w => w.Type_ID == (long)id)
                                         .Includes(t => t.tb_Prods)
                                .Includes(t => t.tb_ProdConversionDetails_to)
                                .Includes(t => t.tb_ProdConversionDetails_from)
                            .Includes(t => t.tb_ManufacturingOrders)
                        .FirstAsync();
            if (entity != null)
            {
                entity.HasChanged = false;
            }

            MyCacheManager.Instance.UpdateEntityList<tb_ProductType>(entity);
            return entity as T;
        }






    }
}



