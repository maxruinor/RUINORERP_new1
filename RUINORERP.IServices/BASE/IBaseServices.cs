﻿using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RUINORERP.Model.Context;
namespace RUINORERP.IServices.BASE
{
    public interface IBaseServices<TEntity> where TEntity : class
    {
        ApplicationContext AppContext { get; }
       

        //  ISqlSugarClient Db { get; }
        Task<TEntity> QueryByIdAsync(object objId);
        TEntity QueryById(object objId);

        Task<TEntity> QueryById(object objId, bool blnUseCache = false);
        Task<List<TEntity>> QueryByIDs(object[] lstIds);

        Task<TEntity> AddReEntityAsync(TEntity model);

        TEntity AddReEntity(TEntity model);
        Task<long> Add(TEntity model);

        Task<List<long>> Add(List<TEntity> listEntity);

        Task<bool> DeleteById(object id);

        Task<bool> Delete(TEntity model);

        Task<bool> DeleteByIds(long[] ids);

        Task<bool> Update(TEntity model);
        Task<bool> Update(TEntity entity, string where);

        Task<bool> Update(object operateAnonymousObjects);

        Task<bool> Update(TEntity entity, List<string> lstColumns = null, List<string> lstIgnoreColumns = null, string where = "");
        List<TEntity> Query();
        List<TEntity> Query(string wheresql);
        Task<List<TEntity>> QueryAsync();
        Task<List<TEntity>> QueryAsync(string where);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression);
        Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFields);
        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true);
        Task<List<TEntity>> Query(string where, string orderByFields);
        Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null);
        Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null);

        Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, int top, string orderByFields);
        Task<List<TEntity>> Query(string where, int top, string orderByFields);

        Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression, int pageIndex, int pageSize, string orderByFields);
        Task<List<TEntity>> Query(string where, int pageIndex, int pageSize, string orderByFields);


        Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string orderByFields = null);

        Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new();
        Task<PageModel<TEntity>> QueryPage(PaginationModel pagination);
    }

}
