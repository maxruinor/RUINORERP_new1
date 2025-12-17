using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RUINORERP.Model;
using SqlSugar;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 动态表达式构建器接口
    /// 提供完全动态的表达式构建和过滤功能，不预先指定字段名
    /// </summary>
    public interface IDynamicExpressionBuilder
    {
        /// <summary>
        /// 从现有表达式构建动态表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">现有表达式</param>
        /// <returns>处理后的表达式</returns>
        Expression<Func<T, bool>> ProcessExpression<T>(Expression<Func<T, bool>> expression) where T : class;

        /// <summary>
        /// 安全地评估表达式，支持闭包变量
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="expression">表达式</param>
        /// <returns>过滤后的结果</returns>
        IEnumerable<T> SafeEvaluate<T>(IEnumerable<T> source, Expression<Func<T, bool>> expression) where T : class;

        /// <summary>
        /// 使用SqlSugar的Expressionable构建表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="buildExpression">构建表达式的委托</param>
        /// <returns>构建后的表达式</returns>
        Expression<Func<T, bool>> BuildWithSqlSugar<T>(Func<ISugarQueryable<T>, ISugarQueryable<T>> buildExpression) where T : class;
    }

    /// <summary>
    /// 动态表达式构建器实现
    /// 完全动态处理表达式，不预先指定字段名
    /// </summary>
    public class DynamicExpressionBuilder : IDynamicExpressionBuilder
    {
        private readonly IExpressionSafeEvaluator _evaluator;

        public DynamicExpressionBuilder(IExpressionSafeEvaluator evaluator = null)
        {
            _evaluator = evaluator ?? new ExpressionSafeEvaluator();
        }

        public Expression<Func<T, bool>> ProcessExpression<T>(Expression<Func<T, bool>> expression) where T : class
        {
            if (expression == null)
                return t => true;

            try
            {
                // 尝试直接编译表达式以检查是否有效
                var compiled = expression.Compile();
                return expression;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"表达式处理失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"尝试使用表达式重建方法");
                
                // 如果直接编译失败，尝试重建表达式
                return RebuildExpressionWithClosureHandling(expression);
            }
        }

        public IEnumerable<T> SafeEvaluate<T>(IEnumerable<T> source, Expression<Func<T, bool>> expression) where T : class
        {
            if (source == null || expression == null)
                return source;

            try
            {
                // 首先尝试直接编译表达式
                var compiled = expression.Compile();
                return source.Where(compiled);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"直接编译表达式失败: {ex.Message}");
                
                // 如果直接编译失败，尝试使用表达式重建
                try
                {
                    var rebuiltExpression = RebuildExpressionWithClosureHandling(expression);
                    var rebuiltCompiled = rebuiltExpression.Compile();
                    return source.Where(rebuiltCompiled);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"表达式重建失败: {ex2.Message}");
                    
                    // 最后尝试使用条件提取方法
                    try
                    {
                        return ExpressionSafeHelper.TryFilterWithConditionExtraction(source.ToList(), expression);
                    }
                    catch (Exception ex3)
                    {
                        System.Diagnostics.Debug.WriteLine($"条件提取方法也失败: {ex3.Message}");
                        // 所有方法都失败时，返回原始数据
                        return source;
                    }
                }
            }
        }

        public Expression<Func<T, bool>> BuildWithSqlSugar<T>(Func<ISugarQueryable<T>, ISugarQueryable<T>> buildExpression) where T : class
        {
            try
            {
                // 使用SqlSugar的Expressionable
                var expressionable = Expressionable.Create<T>();
                var queryable = expressionable.AsQueryable();
                var resultQueryable = buildExpression(queryable);
                
                // 从ISugarQueryable中提取表达式
                // 注意：这是一个简化的实现，实际可能需要更复杂的逻辑
                if (resultQueryable is ISugarQueryable<T> sugarQueryable)
                {
                    // 尝试获取表达式
                    var expression = sugarQueryable.QueryableExpression;
                    if (expression is Expression<Func<T, bool>> funcExpression)
                    {
                        return funcExpression;
                    }
                }
                
                // 如果无法提取表达式，返回一个总是为true的表达式
                return t => true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SqlSugar表达式构建失败: {ex.Message}");
                return t => true;
            }
        }

        /// <summary>
        /// 重建表达式，处理闭包变量
        /// </summary>
        private Expression<Func<T, bool>> RebuildExpressionWithClosureHandling<T>(Expression<Func<T, bool>> expression) where T : class
        {
            try
            {
                // 使用表达式访问器处理闭包变量
                var visitor = new ClosureHandlingVisitor();
                var visitedExpression = visitor.Visit(expression);
                
                return (Expression<Func<T, bool>>)visitedExpression;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"表达式重建失败: {ex.Message}");
                return t => true;
            }
        }
    }

    /// <summary>
    /// 闭包处理访问器
    /// 用于处理表达式中的闭包变量
    /// </summary>
    public class ClosureHandlingVisitor : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            // 处理闭包变量访问
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Constant)
            {
                try
                {
                    // 获取闭包对象的值
                    var constantExpression = (ConstantExpression)node.Expression;
                    var closureObject = constantExpression.Value;
                    
                    if (closureObject != null)
                    {
                        // 获取属性或字段的值
                        object value = null;
                        if (node.Member is PropertyInfo propertyInfo)
                        {
                            value = propertyInfo.GetValue(closureObject);
                        }
                        else if (node.Member is FieldInfo fieldInfo)
                        {
                            value = fieldInfo.GetValue(closureObject);
                        }
                        
                        // 返回常量表达式
                        return Expression.Constant(value, node.Type);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"处理闭包变量失败: {ex.Message}");
                }
            }
            
            return base.VisitMember(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            // 处理二元表达式中的闭包变量
            var left = Visit(node.Left);
            var right = Visit(node.Right);
            
            // 如果左右表达式类型发生变化，可能需要转换
            if (left.Type != node.Left.Type || right.Type != node.Right.Type)
            {
                // 确保类型匹配
                if (left.Type != right.Type)
                {
                    // 尝试类型转换
                    if (left.Type.IsAssignableFrom(right.Type))
                    {
                        right = Expression.Convert(right, left.Type);
                    }
                    else if (right.Type.IsAssignableFrom(left.Type))
                    {
                        left = Expression.Convert(left, right.Type);
                    }
                }
            }
            
            return Expression.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, node.Method);
        }
    }

    /// <summary>
    /// 表达式安全评估器接口
    /// 提供安全表达式评估功能，避免闭包变量和类型转换问题
    /// </summary>
    public interface IExpressionSafeEvaluator
    {
        /// <summary>
        /// 安全地评估表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="expression">表达式</param>
        /// <returns>过滤后的结果</returns>
        IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Expression<Func<T, bool>> expression) where T : class;

        /// <summary>
        /// 安全地编译表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="expression">表达式</param>
        /// <returns>编译后的委托</returns>
        Func<T, bool> Compile<T>(Expression<Func<T, bool>> expression) where T : class;
    }

    /// <summary>
    /// 表达式安全评估器实现
    /// 使用表达式分析器和条件提取方法来避免闭包变量问题
    /// </summary>
    public class ExpressionSafeEvaluator : IExpressionSafeEvaluator
    {
        public IEnumerable<T> Evaluate<T>(IEnumerable<T> source, Expression<Func<T, bool>> expression) where T : class
        {
            if (source == null || expression == null)
                return source;

            try
            {
                // 首先尝试直接编译表达式
                var compiled = expression.Compile();
                return source.Where(compiled);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"直接编译表达式失败: {ex.Message}");
                
                // 如果直接编译失败，尝试使用条件提取方法
                try
                {
                    return ExpressionSafeHelper.TryFilterWithConditionExtraction(source.ToList(), expression);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"条件提取方法也失败: {ex2.Message}");
                    // 所有方法都失败时，返回原始数据
                    return source;
                }
            }
        }

        public Func<T, bool> Compile<T>(Expression<Func<T, bool>> expression) where T : class
        {
            if (expression == null)
                return t => true;

            try
            {
                return expression.Compile();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"编译表达式失败: {ex.Message}");
                
                // 如果编译失败，返回一个总是为true的委托
                return t => true;
            }
        }
    }

    /// <summary>
    /// 动态表达式工厂类
    /// 提供完全动态的表达式构建和评估方法
    /// </summary>
    public static class DynamicExpressionFactory
    {
        private static readonly IDynamicExpressionBuilder _builder = new DynamicExpressionBuilder();
        private static readonly IExpressionSafeEvaluator _evaluator = new ExpressionSafeEvaluator();

        /// <summary>
        /// 获取动态表达式构建器实例
        /// </summary>
        public static IDynamicExpressionBuilder Builder => _builder;

        /// <summary>
        /// 获取表达式安全评估器实例
        /// </summary>
        public static IExpressionSafeEvaluator Evaluator => _evaluator;

        /// <summary>
        /// 安全地执行过滤操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="expression">表达式</param>
        /// <returns>过滤后的结果</returns>
        public static IEnumerable<T> SafeFilter<T>(IEnumerable<T> source, Expression<Func<T, bool>> expression) where T : class
        {
            return _builder.SafeEvaluate(source, expression);
        }

        /// <summary>
        /// 使用SqlSugar构建表达式并执行过滤
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="buildExpression">构建表达式的委托</param>
        /// <returns>过滤后的结果</returns>
        public static IEnumerable<T> FilterWithSqlSugar<T>(
            IEnumerable<T> source, 
            Func<ISugarQueryable<T>, ISugarQueryable<T>> buildExpression) where T : class
        {
            if (source == null || buildExpression == null)
                return source;

            try
            {
                // 使用SqlSugar的Expressionable构建表达式
                var expressionable = Expressionable.Create<T>();
                var queryable = expressionable.AsQueryable();
                var resultQueryable = buildExpression(queryable);
                
                // 尝试获取表达式并编译
                if (resultQueryable is ISugarQueryable<T> sugarQueryable)
                {
                    var expression = sugarQueryable.QueryableExpression;
                    if (expression is Expression<Func<T, bool>> funcExpression)
                    {
                        var compiled = funcExpression.Compile();
                        return source.Where(compiled);
                    }
                }
                
                return source;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SqlSugar表达式过滤失败: {ex.Message}");
                return source;
            }
        }

        /// <summary>
        /// 从SqlSugar的Expressionable构建表达式
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="buildExpression">构建表达式的委托</param>
        /// <returns>构建后的表达式</returns>
        public static Expression<Func<T, bool>> BuildFromSqlSugar<T>(
            Func<ISugarQueryable<T>, ISugarQueryable<T>> buildExpression) where T : class
        {
            return _builder.BuildWithSqlSugar(buildExpression);
        }
    }
}