﻿using RUINORERP.Common;
using RUINORERP.IRepository.Base;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Common.Helper;
using RUINORERP.Common.DI;
using System.Linq;



namespace RUINORERP.Repository.Base
{
    public class BaseRepository<TEntity> : IDependencyRepository, IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        //private readonly SqlSugarScope _dbBase;
        private readonly ISqlSugarClient _db;

        public ISqlSugarClient Db => _db;

        public BaseRepository(IUnitOfWorkManage unitOfWorkManage)
        {
            _unitOfWorkManage = unitOfWorkManage;
            //_db = unitOfWorkManage.GetDbClient();
            _db = unitOfWorkManage.GetDbClient();
        }

        public TEntity QueryById(object objId)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().InSingle(objId));
            return Db.Queryable<TEntity>().In(objId).Single();
        }

        public async Task<TEntity> QueryByIdAsync(object objId)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().InSingle(objId));
            return await Db.Queryable<TEntity>().In(objId).SingleAsync();
        }

        /// <summary>
        /// 功能描述:根据ID查询一条数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="objId">id（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <param name="blnUseCache">是否使用缓存</param>
        /// <returns>数据实体</returns>
        public async Task<TEntity> QueryById(object objId, bool blnUseCache = false)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().WithCacheIF(blnUseCache).InSingle(objId));
            return await Db.Queryable<TEntity>().WithCacheIF(blnUseCache, 10).In(objId).SingleAsync();
        }

        /// <summary>
        /// 功能描述:根据ID查询数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="lstIds">id列表（必须指定主键特性 [SugarColumn(IsPrimaryKey=true)]），如果是联合主键，请使用Where条件</param>
        /// <returns>数据实体列表</returns>
        public async Task<List<TEntity>> QueryByIDs(object[] lstIds)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().In(lstIds).ToList());
            return await Db.Queryable<TEntity>().In(lstIds).ToListAsync();
        }

        /// <summary>
        /// 写入实体数据 返回自增的INT 主键值
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<int> Add(TEntity entity)
        {
            //var i = await Task.Run(() => _db.Insertable(entity).ExecuteReturnBigIdentity());
            ////返回的i是long类型,这里你可以根据你的业务需要进行处理
            //return (int)i;

            var insert = Db.Insertable(entity);

            //这里你可以返回TEntity，这样的话就可以获取id值，无论主键是什么类型
            //var return3 = await insert.ExecuteReturnEntityAsync();
            return await insert.ExecuteReturnIdentityAsync();
        }


        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="insertColumns">指定只插入列</param>
        /// <returns>返回自增量列</returns>
        public async Task<int> Add(TEntity entity, Expression<Func<TEntity, object>> insertColumns = null)
        {
            var insert = Db.Insertable(entity);
            if (insertColumns == null)
            {
                return await insert.ExecuteReturnIdentityAsync();
            }
            else
            {
                return await insert.InsertColumns(insertColumns).ExecuteReturnIdentityAsync();
            }
        }

        /// <summary>
        /// 批量插入实体(速度快)
        /// </summary>
        /// <param name="listEntity">实体集合</param>
        /// <returns>影响行数</returns>
        public async Task<int> Add(List<TEntity> listEntity)
        {
            return await Db.Insertable(listEntity.ToArray()).ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Update(TEntity entity)
        {
            ////这种方式会以主键为条件
            //var i = await Task.Run(() => _db.Updateable(entity).ExecuteCommand());
            //return i > 0;
            //这种方式会以主键为条件
            bool rs = false;
            // 只更新变更的列
            if (entity is BaseEntity model)
            {
                if (model.HasChanged)
                {
                    // 获取包含主键的变更列映射
                    // var changedMappings = model.GetChangedColumnMappingsWithPK();
                    var changedMappings = model.GetChangedColumnMappings();
                    var changedColumns = changedMappings.Values.ToArray();
                    // 获取主键值
                    var pkValue = model.PrimaryKeyID;
                    var pkColumn = model.GetPrimaryKeyColName();
                    rs = await Db.Updateable<TEntity>(model)
                   .UpdateColumns(changedColumns)
                   //.Where($"{pkColumn} = @pk", new { pk = pkValue }) // 添加主键条件
                   .ExecuteCommandHasChangeAsync();
                    model.AcceptChanges(); // 保存后重置状态
                }
            }
            return rs;
            //return await Db.Updateable(entity).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(TEntity entity, string where)
        {
            return await Db.Updateable(entity).Where(where).ExecuteCommandHasChangeAsync();
        }

        public async Task<bool> Update(string sql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.ExecuteCommandAsync(sql, parameters) > 0;
        }

        public async Task<bool> Update(object operateAnonymousObjects)
        {
            return await Db.Updateable<TEntity>(operateAnonymousObjects).ExecuteCommandAsync() > 0;
        }

        public async Task<bool> Update(
          TEntity entity,
          List<string> lstColumns = null,
          List<string> lstIgnoreColumns = null,
          string where = ""
            )
        {
            IUpdateable<TEntity> up = Db.Updateable(entity);
            if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
            {
                up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
            }
            if (lstColumns != null && lstColumns.Count > 0)
            {
                up = up.UpdateColumns(lstColumns.ToArray());
            }
            if (!string.IsNullOrEmpty(where))
            {
                up = up.Where(where);
            }
            return await up.ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 根据实体删除一条数据
        /// </summary>
        /// <param name="entity">博文实体类</param>
        /// <returns></returns>
        public async Task<bool> Delete(TEntity entity)
        {
            return await Db.Deleteable(entity).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns></returns>
        public async Task<bool> DeleteById(object id)
        {
            return await Db.Deleteable<TEntity>().In(id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIds(object[] ids)
        {
            return await Db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }


        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <returns>数据列表</returns>
        public List<TEntity> Query()
        {
            return Db.Queryable<TEntity>().ToList();
        }

        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <returns>数据列表</returns>
        public List<TEntity> Query(string wheresql)
        {
            return Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(wheresql), wheresql).ToList();
        }

        /// <summary>
        /// 功能描述:查询所有数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync()
        {
            return await Db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> QueryAsync(string where)
        {

            return await Db.Queryable<TEntity>().WhereIF(!string.IsNullOrEmpty(where), where).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询数据列表
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="whereExpression">whereExpression</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表
        /// 作　　者:RUINORERP
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression)
        {
            return await Db.Queryable<TEntity>().Select(expression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:按照特定列查询数据列表带条件排序
        /// 作　　者:RUINORERP
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="whereExpression">过滤条件</param>
        /// <param name="expression">查询实体条件</param>
        /// <param name="orderByFields">排序条件</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, TResult>> expression, Expression<Func<TEntity, bool>> whereExpression, string orderByFields)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(whereExpression != null, whereExpression).Select(expression).ToListAsync();
        }


        /// <summary>
        /// 实体列表 分页查询
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <param name="pagination">分页对象</param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryPageListAsync(Expression<Func<TEntity, bool>> whereLambda,
            Pagination pagination, Expression<Func<TEntity, TEntity>> expression = null)
        {
            RefAsync<int> totalCount = 0;
            var query = _db.Queryable<TEntity>();
            query = query.WhereIF(whereLambda != null, whereLambda);
            if (expression != null)
            {
                query = query.Select(expression);
            }

            query = query.OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields));
            var list = await query.ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
            pagination.TotalElements = totalCount;
            return list;
        }


        /// <summary>
        /// 实体列表 分页查询
        /// </summary>
        /// <param name="whereLambda">条件表达式</param>
        /// <param name="pagination">分页对象</param>
        /// <param name="selectExpression"></param>
        /// <param name="navigationExpression"></param>
        /// <param name="navigationExpression2"></param>
        /// <param name="navigationExpression3"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryPageListAsync<T, T2, T3>(Expression<Func<TEntity, bool>> whereLambda,
            Pagination pagination, Expression<Func<TEntity, TEntity>> selectExpression = null,
            Expression<Func<TEntity, T>> navigationExpression = null,
            Expression<Func<TEntity, List<T2>>> navigationExpression2 = null,
            Expression<Func<TEntity, List<T3>>> navigationExpression3 = null)
        {
            RefAsync<int> totalCount = 0;
            var query = _db.Queryable<TEntity>();

            if (navigationExpression != null)
            {
                query = query.Includes(navigationExpression);
            }

            if (navigationExpression2 != null)
            {
                query = query.Includes(navigationExpression2);
            }

            if (navigationExpression3 != null)
            {
                query = query.Includes(navigationExpression3);
            }

            query = query.WhereIF(whereLambda != null, whereLambda);
            if (selectExpression != null)
            {
                query = query.Select(selectExpression);
            }

            query = query.OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields));
            var list = await query.ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
            pagination.TotalElements = totalCount;
            return list;
        }


        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, string orderByFields)
        {
            return await Db.Queryable<TEntity>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderByFields != null, orderByFields).ToListAsync();
        }
        /// <summary>
        /// 功能描述:查询一个列表
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression, bool isAsc = true)
        {
            //return await Task.Run(() => _db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToList());
            return await Db.Queryable<TEntity>().OrderByIF(orderByExpression != null, orderByExpression, isAsc ? OrderByType.Asc : OrderByType.Desc).WhereIF(whereExpression != null, whereExpression).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询一个列表
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(string where, string orderByFields)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(!string.IsNullOrEmpty(where), where).ToListAsync();
        }


        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int top,
            string orderByFields)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(whereExpression != null, whereExpression).Take(top).ToListAsync();
        }

        /// <summary>
        /// 功能描述:查询前N条数据
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="top">前N条</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            string where,
            int top,
            string orderByFields)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields).WhereIF(!string.IsNullOrEmpty(where), where).Take(top).ToListAsync();
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>泛型集合</returns>
        public async Task<List<TEntity>> QuerySql(string sql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.SqlQueryAsync<TEntity>(sql, parameters);
        }

        /// <summary>
        /// 根据sql语句查询
        /// </summary>
        /// <param name="sql">完整的sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns>DataTable</returns>
        public async Task<DataTable> QueryTable(string sql, SugarParameter[] parameters = null)
        {
            return await Db.Ado.GetDataTableAsync(sql, parameters);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
            Expression<Func<TEntity, bool>> whereExpression,
            int pageIndex,
            int pageSize,
            string orderByFields)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(whereExpression != null, whereExpression).ToPageListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// 功能描述:分页查询
        /// 作　　者:RUINORERP
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns>数据列表</returns>
        public async Task<List<TEntity>> Query(
          string where,
          int pageIndex,
          int pageSize,

          string orderByFields)
        {
            return await Db.Queryable<TEntity>().OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
                .WhereIF(!string.IsNullOrEmpty(where), where).ToPageListAsync(pageIndex, pageSize);
        }



        /// <summary>
        /// 分页查询[使用版本，其他分页未测试]
        /// </summary>
        /// <param name="whereExpression">条件表达式</param>
        /// <param name="pageIndex">页码（下标0）</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段，如name asc,age desc</param>
        /// <returns></returns>
        public async Task<PageModel<TEntity>> QueryPage(Expression<Func<TEntity, bool>> whereExpression, int pageIndex = 1, int pageSize = 20, string orderByFields = null)
        {

            RefAsync<int> totalCount = 0;
            var list = await Db.Queryable<TEntity>()
             .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(pageIndex, pageSize, totalCount);

            return new PageModel<TEntity>(pageIndex, totalCount, pageSize, list);
        }


        /// <summary> 
        ///查询-多表查询
        /// </summary> 
        /// <typeparam name="T">实体1</typeparam> 
        /// <typeparam name="T2">实体2</typeparam> 
        /// <typeparam name="T3">实体3</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式 (join1,join2) => new object[] {JoinType.Left,join1.UserNo==join2.UserNo}</param> 
        /// <param name="selectExpression">返回表达式 (s1, s2) => new { Id =s1.UserNo, Id1 = s2.UserNo}</param>
        /// <param name="whereLambda">查询表达式 (w1, w2) =>w1.UserNo == "")</param> 
        /// <returns>值</returns>
        public async Task<List<TResult>> QueryMuch<T, T2, T3, TResult>(
            Expression<Func<T, T2, T3, object[]>> joinExpression,
            Expression<Func<T, T2, T3, TResult>> selectExpression,
            Expression<Func<T, T2, T3, bool>> whereLambda = null) where T : class, new()
        {
            if (whereLambda == null)
            {
                return await Db.Queryable(joinExpression).Select(selectExpression).ToListAsync();
            }
            return await Db.Queryable(joinExpression).Where(whereLambda).Select(selectExpression).ToListAsync();
        }


        /// <summary>
        /// 两表联合查询-分页
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体1</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式</param>
        /// <param name="selectExpression">返回表达式</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段</param>
        /// <returns></returns>
        public async Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            int pageIndex = 1,
            int pageSize = 20,
            string orderByFields = null)
        {

            RefAsync<int> totalCount = 0;
            var list = await Db.Queryable<T, T2>(joinExpression)
             .Select(selectExpression)
             .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(pageIndex, pageSize, totalCount);
            return new PageModel<TResult>(pageIndex, totalCount, pageSize, list);
        }

        /// <summary>
        /// 两表联合查询-分页-分组
        /// </summary>
        /// <typeparam name="T">实体1</typeparam>
        /// <typeparam name="T2">实体1</typeparam>
        /// <typeparam name="TResult">返回对象</typeparam>
        /// <param name="joinExpression">关联表达式</param>
        /// <param name="selectExpression">返回表达式</param>
        /// <param name="whereExpression">查询表达式</param>
        /// <param name="groupExpression">group表达式</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderByFields">排序字段</param>
        /// <returns></returns>
        public async Task<PageModel<TResult>> QueryTabsPage<T, T2, TResult>(
            Expression<Func<T, T2, object[]>> joinExpression,
            Expression<Func<T, T2, TResult>> selectExpression,
            Expression<Func<TResult, bool>> whereExpression,
            Expression<Func<T, object>> groupExpression,
            int pageIndex = 1,
            int pageSize = 20,
            string orderByFields = null)
        {
            RefAsync<int> totalCount = 0;
            var list = await Db.Queryable<T, T2>(joinExpression).GroupBy(groupExpression)
             .Select(selectExpression)
             .OrderByIF(!string.IsNullOrEmpty(orderByFields), orderByFields)
             .WhereIF(whereExpression != null, whereExpression)
             .ToPageListAsync(pageIndex, pageSize, totalCount);
            return new PageModel<TResult>(pageIndex, totalCount, pageSize, list);
        }





        //var exp = Expressionable.Create<ProjectToUser>()
        //        .And(s => s.tdIsDelete != true)
        //        .And(p => p.IsDeleted != true)
        //        .And(p => p.pmId != null)
        //        .AndIF(!string.IsNullOrEmpty(model.paramCode1), (s) => s.uID == model.paramCode1.ObjToInt())
        //                .AndIF(!string.IsNullOrEmpty(model.searchText), (s) => (s.groupName != null && s.groupName.Contains(model.searchText))
        //                        || (s.jobName != null && s.jobName.Contains(model.searchText))
        //                        || (s.uRealName != null && s.uRealName.Contains(model.searchText)))
        //                .ToExpression();//拼接表达式
        //var data = await _projectMemberServices.QueryTabsPage<sysUserInfo, ProjectMember, ProjectToUser>(
        //    (s, p) => new object[] { JoinType.Left, s.uID == p.uId },
        //    (s, p) => new ProjectToUser
        //    {
        //        uID = s.uID,
        //        uRealName = s.uRealName,
        //        groupName = s.groupName,
        //        jobName = s.jobName
        //    }, exp, s => new { s.uID, s.uRealName, s.groupName, s.jobName }, model.currentPage, model.pageSize, model.orderField + " " + model.orderType);

    }

}
