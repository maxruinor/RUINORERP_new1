﻿#region License
/***
 * Copyright © 2018-2021, 张强 (943620963@qq.com).
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * without warranties or conditions of any kind, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#endregion

using SQLBuilder.Entry;
using SQLBuilder.Enums;
using SQLBuilder.Extensions;
using System.Linq.Expressions;

namespace SQLBuilder.Expressions
{
    /// <summary>
    /// 表示具有常数值的表达式
    /// </summary>
	public class ConstantExpressionResolver : BaseExpression<ConstantExpression>
    {
        #region Override Base Class Methods
        /// <summary>
        /// Select
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Select(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            if (expression?.Value == null)
                sqlWrapper.AddField("*");
            else
                sqlWrapper.AddField(expression.Value.ToString());

            return sqlWrapper;
        }

        /// <summary>
        /// Count
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Count(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            if (expression?.Value == null)
                sqlWrapper.AddField("*");
            else
                sqlWrapper.AddField(expression.Value.ToString());

            return sqlWrapper;
        }

        /// <summary>
        /// Sum
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Sum(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            if (expression?.Value != null)
                sqlWrapper.AddField(expression.Value.ToString());

            return sqlWrapper;
        }

        /// <summary>
        /// Max
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Max(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            if (expression?.Value != null)
                sqlWrapper.AddField(expression.Value.ToString());

            return sqlWrapper;
        }

        /// <summary>
        /// Min
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Min(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            if (expression?.Value != null)
                sqlWrapper.AddField(expression.Value.ToString());

            return sqlWrapper;
        }

        /// <summary>
        /// Avg
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Avg(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            if (expression?.Value != null)
                sqlWrapper.AddField(expression.Value.ToString());

            return sqlWrapper;
        }

        /// <summary>
        /// Where
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Where(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            //表达式左侧为bool类型常量
            if (expression.NodeType == ExpressionType.Constant && expression.Value is bool b)
            {
                var sql = sqlWrapper.ToString().ToUpper().Trim();
                if (!b && (sql.EndsWith("WHERE") || sql.EndsWith("AND") || sql.EndsWith("OR")))
                    sqlWrapper += " 1 = 0 ";
            }
            else
                sqlWrapper.AddDbParameter(expression.Value);

            return sqlWrapper;
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Join(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            //表达式左侧为bool类型常量
            if (expression.NodeType == ExpressionType.Constant && expression.Value is bool b)
            {
                var sql = sqlWrapper.ToString().ToUpper().Trim();
                if (!b && (sql.EndsWith("AND") || sql.EndsWith("OR")))
                    sqlWrapper += " 1 = 0 ";
            }
            else
                sqlWrapper.AddDbParameter(expression.Value);

            return sqlWrapper;
        }

        /// <summary>
        /// In
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
		public override SqlWrapper In(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            sqlWrapper.AddDbParameter(expression.Value);

            return sqlWrapper;
        }

        /// <summary>
        /// GroupBy
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
		public override SqlWrapper GroupBy(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            var tableName = sqlWrapper.GetTableName(sqlWrapper.DefaultType);
            var tableAlias = sqlWrapper.GetTableAlias(tableName);
            if (!tableAlias.IsNullOrEmpty())
                tableAlias += ".";

            sqlWrapper += tableAlias + sqlWrapper.GetColumnName(expression.Value.ToString()) + ",";

            return sqlWrapper;
        }

        /// <summary>
        /// Having
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <returns>SqlWrapper</returns>
        public override SqlWrapper Having(ConstantExpression expression, SqlWrapper sqlWrapper)
        {
            sqlWrapper.AddDbParameter(expression.Value);

            return sqlWrapper;
        }

        /// <summary>
        /// OrderBy
        /// </summary>
        /// <param name="expression">表达式树</param>
        /// <param name="sqlWrapper">sql打包对象</param>
        /// <param name="orders">排序方式</param>
        /// <returns>SqlWrapper</returns>
		public override SqlWrapper OrderBy(ConstantExpression expression, SqlWrapper sqlWrapper, params OrderType[] orders)
        {
            var tableName = sqlWrapper.GetTableName(sqlWrapper.DefaultType);
            var tableAlias = sqlWrapper.GetTableAlias(tableName);
            if (!tableAlias.IsNullOrEmpty())
                tableAlias += ".";

            var field = expression.Value.ToString();
            if (!field.ToUpper().Contains(" ASC") && !field.ToUpper().Contains(" DESC"))
                field = sqlWrapper.GetColumnName(field);

            sqlWrapper += tableAlias + field;

            if (orders?.Length > 0)
                sqlWrapper += $" { (orders[0] == OrderType.Descending ? "DESC" : "ASC")}";

            return sqlWrapper;
        }
        #endregion
    }
}