using System;
using System.Linq.Expressions;

namespace RUINORERP.Business.RowLevelAuthService
{
    /// <summary>
    /// Expression过滤条件提供者 - 静态辅助类
    /// 【已废弃】此方法无法安全地将任意SQL转换为Lambda表达式
    /// 请直接使用 <see cref="RowAuthService.GetUserRowAuthFilterClause"/> 获取SQL字符串
    /// 并通过 <see cref="SqlSugarRowLevelAuthFilter.BuildFilterClause"/> 应用到查询
    /// </summary>
    [Obsolete("此方法已废弃，无法正确转换SQL到Expression。请使用RowAuthService.GetUserRowAuthFilterClause获取SQL字符串", true)]
    public static class ExpressionFilterHelper
    {
        /// <summary>
        /// 将SQL过滤条件转换为Lambda表达式
        /// 【已废弃】始终返回 t => true，无法正确转换
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="filterClause">SQL过滤条件</param>
        /// <returns>Lambda表达式（始终为 t => true）</returns>
        [Obsolete("此方法已废弃，请使用RowAuthService.GetUserRowAuthFilterClause", true)]
        public static Expression<Func<T, bool>> ConvertToExpression<T>(string filterClause) where T : class
        {
            // 此方法已废弃，无法正确实现SQL到Expression的转换
            // 使用 [Obsolete(error: true)] 确保编译时提示错误
            throw new NotSupportedException(
                "ExpressionFilterHelper.ConvertToExpression 已废弃。" +
                "请使用 RowAuthService.GetUserRowAuthFilterClause 获取SQL字符串，" +
                "并通过 SqlSugarRowLevelAuthFilter.BuildFilterClause 应用到查询。");
        }
    }
}
