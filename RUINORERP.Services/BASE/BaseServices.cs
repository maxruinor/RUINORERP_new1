using RUINORERP.Common.Helper;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices.BASE;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RUINORERP.Common.DI;
using RUINORERP.Model.Context;
namespace RUINORERP.Services.BASE
{
    /// <summary>
    /// 业务实现基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseServices<TEntity> : IDependencyService, IBaseServices<TEntity> where TEntity : class, new()
    {
        // public ISqlSugarClient Db { get; set; }
        public BaseServices(IBaseRepository<TEntity> BaseDal = null, ApplicationContext appContext = null)
        {

            this.BaseDal = BaseDal;
            AppContext = appContext;
            //  Db = BaseDal.Db;
        }
        //public IBaseRepository<TEntity> baseDal = new BaseRepository<TEntity>();
        public IBaseRepository<TEntity> BaseDal { get; set; }//通过在子类的构造函数中注入，这里是基类，不用构造函数

        /// <summary>
        /// 当前上下文
        /// </summary>
        public ApplicationContext AppContext { get; }

        public TEntity QueryById(object objId)
        {
            return BaseDal.QueryById(objId);
        }

        public async Task<TEntity> QueryByIdAsync(object objId)
        {
            return await BaseDal.QueryByIdAsync(objId);
        }

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:AZLinli.RUINORERP
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
        {
            return await BaseDal.QueryById(objId, blnUseCache);
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:AZLinli.RUINORERP
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            return await BaseDal.QueryByIDs(lstIds);
        }


        /// <summary>
        /// 返回新增后的实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity AddReEntity(TEntity entity)
        {
            return BaseDal.Db.Insertable<TEntity>(entity).ExecuteReturnEntity();
        }


        /// <summary>
        /// 返回新增后的实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<TEntity> AddReEntityAsync(TEntity entity)
        {
            return await BaseDal.Db.Insertable<TEntity>(entity).ExecuteReturnEntityAsync();
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<long> Add(TEntity entity)
        {
            return await BaseDal.Db.Insertable<TEntity>(entity).ExecuteReturnSnowflakeIdAsync();
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<List<long>> Add(List<TEntity> listEntity)
        {
            return await BaseDal.Db.Insertable<TEntity>(listEntity).ExecuteReturnSnowflakeIdListAsync();
            //return await BaseDal.Db.Insertable<TEntity>(listEntity).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            return await BaseDal.Update(entity);
        }
        public async Task<bool> Update(TEntity entity, string where)
        {
            return await BaseDal.Update(entity, where);
        }
        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await BaseDal.Update(operateAnonymousObjects);
        }

        public async Task<bool> Update(
         TEntity entity,
         List<string> lstColumns = null,
         List<string> lstIgnoreColumns = null,
         string where = ""
            )
        {
            return await BaseDal.Update(entity, lstColumns, lstIgnoreColumns, where);
        }


        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await BaseDal.Delete(entity);
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await BaseDal.DeleteById(id);
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await BaseDal.DeleteByIds(ids);
        }



        /// <summary>
        /// 功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync()
        {
            return await BaseDal.QueryAsync();
        }

        /// <summary>
        /// 功能描述:查询所有数据
        /// </summary>
        /// <returns>数据列表</returns>
        public List<TEntity> Query()
        {
            return BaseDal.Query();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string where)
        {
            return await BaseDal.QueryAsync(where);
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await BaseDal.Query(whereExpression);
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return await BaseDal.Query(expression);
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表带条件排序
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="orderByFileds">排序条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFileds)
        {
            return await BaseDal.Query(expression, whereExpression, orderByFileds);
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="strOrderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            return await BaseDal.Query(whereExpression, orderByExpression, isAsc);
        }

        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFileds)
        {
            return await BaseDal.Query(whereExpression, orderByFileds);
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string where, string orderByFileds)
        {
            return await BaseDal.Query(where, orderByFileds);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.QuerySql(sql, parameters);

        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null)
        {
            return await BaseDal.QueryTable(sql, parameters);

        }
        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFileds)
        {
            return await BaseDal.Query(whereExpression, top, orderByFileds);
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string where,
            int top,
            string orderByFileds)
        {
            return await BaseDal.Query(where, top, orderByFileds);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int pageIndex,
            int pageSize,
            string orderByFileds)
        {
            return await BaseDal.Query(
              whereExpression,
              pageIndex,
              pageSize,
              orderByFileds);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFileds">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
          string where,
          int pageIndex,
          int pageSize,
          string orderByFileds)
        {
            return await BaseDal.Query(
            where,
            pageIndex,
            pageSize,
            orderByFileds);
        }

        public async Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression,
        int pageIndex = 1, int pageSize = 20, string orderByFileds = null)
        {
            return await BaseDal.QueryPage(whereExpression,
         pageIndex, pageSize, orderByFileds);
        }

        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(Expression<Func<T, T2, T3, object[]>> joinExpression, Expression<Func<T, T2, T3, TResult>> selectExpression, Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            return await BaseDal.QueryMuch(joinExpression, selectExpression, whereLambda);
        }
        public async Task<PageModel<TEntity>> QueryPage(PaginationModel pagination)
        {
            var express = DynamicLinqFactory.CreateLambda<TEntity>(pagination.Conditions);
            return await QueryPage(express, pagination.PageIndex, pagination.PageSize, pagination.OrderByFileds);
        }

        List<TEntity> IBaseServices<TEntity>.Query(string wheresql)
        {
            return BaseDal.Query(wheresql);
        }

        public Task<bool> DeleteByIds(long[] ids)
        {
            object[] oids = new object[ids.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                oids[i] = ids[i];
            }
            return BaseDal.DeleteByIds(oids);
        }
    }

}
