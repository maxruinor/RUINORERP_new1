using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{

    /// <summary>
    /// 表达式树安全处理辅助类
    /// </summary>
    public static class ExpressionSafeHelper
    {
        // 使用两层缓存: 第一层基于表达式结构，第二层基于闭包变量值
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Delegate>> _expressionCache =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, Delegate>>();

        /// <summary>
        /// 安全地编译和执行表达式树，特别增强了对闭包变量的处理和性能优化
        /// </summary>
        public static bool SafeEvaluate<T>(T item, Expression<Func<T, bool>> expression, bool defaultValue = true)
        {
            if (item == null || expression == null)
                return defaultValue;

            try
            {
                // 提取表达式中的闭包变量值，用于构建唯一缓存键
                var closureValuesExtractor = new ClosureValueExtractor();
                closureValuesExtractor.Visit(expression.Body);
                string closureKey = closureValuesExtractor.GetClosureKey();

                // 基于表达式结构构建缓存键
                string structureKey = GetExpressionStructureKey<T>(expression);

                // 获取或创建表达式结构对应的缓存字典
                var structureCache = _expressionCache.GetOrAdd(structureKey, _ => new ConcurrentDictionary<string, Delegate>());

                // 优先尝试从缓存获取编译后的委托（性能最高的路径）
                if (structureCache.TryGetValue(closureKey, out var cachedDelegate))
                {
                    try
                    {
                        var func = (Func<T, bool>)cachedDelegate;
                        return func(item);
                    }
                    catch
                    {
                        // 缓存的委托执行失败，移除缓存
                        structureCache.TryRemove(closureKey, out _);
                    }
                }

                // 快速路径：先尝试使用简单条件评估（避免编译开销）
                bool canUseSimpleEval = IsSimpleExpression(expression.Body);
                if (canUseSimpleEval)
                {
                    try
                    {
                        bool result = EvaluateSimpleExpression(item, expression.Body);

                        // 对于简单表达式，我们也可以缓存结果判断函数
                        // 构建一个简单的判断函数并缓存
                        Func<T, bool> simpleFunc = obj => EvaluateSimpleExpression(obj, expression.Body);
                        structureCache[closureKey] = simpleFunc;

                        return result;
                    }
                    catch
                    {
                        // 简单评估失败，继续尝试编译路径
                    }
                }

                // 只在必要时才进行表达式重建和编译
                try
                {
                    // 增强的表达式树重建，更好地处理闭包变量
                    var safeExpression = RebuildExpressionWithClosureReplacement(expression);

                    // 编译表达式 - 这是昂贵的操作，只在必要时执行
                    Func<T, bool> compiledFunc = safeExpression.Compile();

                    // 缓存编译后的委托，避免重复编译
                    structureCache[closureKey] = compiledFunc;
                    return compiledFunc(item);
                }
                catch (Exception compileEx)
                {
                    System.Diagnostics.Debug.WriteLine($"表达式编译失败: {compileEx.Message}");

                    // 最后尝试使用简单的条件评估作为备选
                    try
                    {
                        return EvaluateSimpleExpression(item, expression.Body);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误但继续执行
                System.Diagnostics.Debug.WriteLine($"表达式评估失败: {ex.Message}");
                return defaultValue;
            }
        }

        /// <summary>
        /// 检查表达式是否为简单表达式，可以使用快速评估路径
        /// </summary>
        public static bool IsSimpleExpression(Expression expression)
        {
            // 简单表达式：二元操作符（==, !=, >, <等）连接的属性访问和常量
            if (expression is BinaryExpression binaryExp)
            {
                // 检查是否为简单的比较操作
                if (binaryExp.NodeType == ExpressionType.Equal ||
                    binaryExp.NodeType == ExpressionType.NotEqual ||
                    binaryExp.NodeType == ExpressionType.GreaterThan ||
                    binaryExp.NodeType == ExpressionType.GreaterThanOrEqual ||
                    binaryExp.NodeType == ExpressionType.LessThan ||
                    binaryExp.NodeType == ExpressionType.LessThanOrEqual)
                {
                    // 检查左右操作数是否为简单表达式（属性访问或常量）
                    return IsSimpleOperand(binaryExp.Left) && IsSimpleOperand(binaryExp.Right);
                }
                // 处理 && 和 || 连接的简单表达式
                else if (binaryExp.NodeType == ExpressionType.AndAlso ||
                         binaryExp.NodeType == ExpressionType.OrElse)
                {
                    return IsSimpleExpression(binaryExp.Left) && IsSimpleExpression(binaryExp.Right);
                }
            }
            // 简单属性访问表达式
            else if (expression is MemberExpression || expression is ConstantExpression)
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// 获取用于评估表达式的委托函数
        /// 预处理表达式并编译成高效的委托，避免在循环中重复处理
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="expression">表达式树</param>
        /// <returns>用于评估的委托函数</returns>
        public static Func<T, bool> GetEvaluationFunction<T>(Expression<Func<T, bool>> expression) where T : class
        {
            try
            {
                // 提取表达式中的闭包变量值，用于构建唯一缓存键
                var closureValuesExtractor = new ExpressionSafeHelper.ClosureValueExtractor();
                closureValuesExtractor.Visit(expression.Body);
                string closureKey = closureValuesExtractor.GetClosureKey();

                // 基于表达式结构构建缓存键
                string structureKey = ExpressionSafeHelper.GetExpressionStructureKey<T>(expression);

                // 获取或创建表达式结构对应的缓存字典
                var structureCache = ExpressionSafeHelper._expressionCache.GetOrAdd(structureKey, _ => new ConcurrentDictionary<string, Delegate>());

                // 优先尝试从缓存获取编译后的委托（性能最高的路径）
                if (structureCache.TryGetValue(closureKey, out var cachedDelegate))
                {
                    try
                    {
                        return (Func<T, bool>)cachedDelegate;
                    }
                    catch
                    {
                        // 缓存的委托执行失败，移除缓存
                        structureCache.TryRemove(closureKey, out _);
                    }
                }

                // 快速路径：先尝试使用简单条件评估（避免编译开销）
                bool canUseSimpleEval = ExpressionSafeHelper.IsSimpleExpression(expression.Body);
                if (canUseSimpleEval)
                {
                    try
                    {
                        // 构建一个简单的判断函数并缓存
                        Func<T, bool> simpleFunc = obj => ExpressionSafeHelper.EvaluateSimpleExpression(obj, expression.Body);
                        structureCache[closureKey] = simpleFunc;
                        return simpleFunc;
                    }
                    catch
                    {
                        // 简单评估失败，继续尝试编译路径
                    }
                }

                // 只在必要时才进行表达式重建和编译
                try
                {
                    // 增强的表达式树重建，更好地处理闭包变量
                    var safeExpression = ExpressionSafeHelper.RebuildExpressionWithClosureReplacement(expression);

                    // 编译表达式 - 这是昂贵的操作，只在必要时执行
                    Func<T, bool> compiledFunc = safeExpression.Compile();

                    // 缓存编译后的委托，避免重复编译
                    structureCache[closureKey] = compiledFunc;
                    return compiledFunc;
                }
                catch (Exception compileEx)
                {
                    System.Diagnostics.Debug.WriteLine($"表达式编译失败: {compileEx.Message}");

                    // 最后尝试使用简单的条件评估作为备选
                    return obj => ExpressionSafeHelper.EvaluateSimpleExpression(obj, expression.Body);
                }
            }
            catch (Exception ex)
            {
                // 记录错误但返回默认评估函数
                System.Diagnostics.Debug.WriteLine($"获取评估函数失败: {ex.Message}");
                return obj => true; // 默认返回true，表示不过滤任何项目
            }
        }

        /// <summary>
        /// 尝试使用条件提取方法进行筛选
        /// </summary>
        public static List<T> TryFilterWithConditionExtraction<T>(List<T> sourceList, Expression<Func<T, bool>> expCondition) where T : class
        {
            try
            {
                var conditions = new List<SimpleCondition>();

                // 处理二元表达式（常见于Where条件）
                if (expCondition.Body is BinaryExpression binaryExp)
                {
                    ExtractConditionsFromBinary(binaryExp, conditions);
                }
                // 也可以处理其他类型的表达式
                else if (expCondition.Body is MemberExpression memberExp)
                {
                    // 处理简单的属性访问表达式
                    conditions.Add(new SimpleCondition
                    {
                        PropertyName = memberExp.Member.Name,
                        Value = true,
                        Operator = ExpressionType.Equal
                    });
                }

                // 如果提取到了条件，使用这些条件进行筛选
                if (conditions.Any())
                {
                    System.Diagnostics.Debug.WriteLine($"成功提取到 {conditions.Count} 个筛选条件");
                    var filteredList = sourceList.Where(item => MeetsAllConditions(item, conditions)).ToList();
                    // 只有当筛选有结果时才返回，否则返回原始列表
                    return filteredList.Count > 0 ? filteredList : sourceList.ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"条件提取失败: {ex.Message}");
            }

            // 所有方法都失败时，返回原始列表
            return sourceList.ToList();
        }





        /// <summary>
        /// 从二元表达式中递归提取条件
        /// </summary>
        private static void ExtractConditionsFromBinary(BinaryExpression binary, List<SimpleCondition> conditions)
        {
            // 处理逻辑运算符（AndAlso, OrElse），递归提取子条件
            if (binary.NodeType == ExpressionType.AndAlso || binary.NodeType == ExpressionType.OrElse)
            {
                // 递归处理左侧
                if (binary.Left is BinaryExpression leftBinary)
                    ExtractConditionsFromBinary(leftBinary, conditions);
                else
                    ExtractSingleCondition(binary.Left, conditions, binary.NodeType);

                // 递归处理右侧
                if (binary.Right is BinaryExpression rightBinary)
                    ExtractConditionsFromBinary(rightBinary, conditions);
                else
                    ExtractSingleCondition(binary.Right, conditions, binary.NodeType);
            }
            else
            {
                // 处理单个比较条件
                ExtractSingleCondition(binary, conditions, binary.NodeType);
            }
        }

        /// <summary>
        /// 提取单个条件
        /// </summary>
        private static void ExtractSingleCondition(Expression expression, List<SimpleCondition> conditions, ExpressionType logicalOperator)
        {
            if (expression is BinaryExpression binary)
            {
                // 尝试获取左侧属性名
                string propertyName = GetPropertyName(binary.Left);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    // 尝试获取右侧值，支持常量表达式和类型转换表达式
                    object value = GetExpressionValueFromNode(binary.Right);
                    if (value != null || binary.Right is ConstantExpression constExp && constExp.Value == null)
                    {
                        conditions.Add(new SimpleCondition
                        {
                            PropertyName = propertyName,
                            Value = value,
                            Operator = binary.NodeType,
                            LogicalOperator = logicalOperator
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 从表达式节点获取属性名
        /// </summary>
        private static string GetPropertyName(Expression expression)
        {
            if (expression is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }
            return null;
        }

        /// <summary>
        /// 从表达式节点获取值，支持常量表达式和类型转换表达式
        /// </summary>
        private static object GetExpressionValueFromNode(Expression expression)
        {
            if (expression is ConstantExpression constExp)
            {
                return constExp.Value;
            }
            else if (expression is UnaryExpression unaryExp && unaryExp.NodeType == ExpressionType.Convert)
            {
                // 处理类型转换表达式，递归获取内部表达式的值
                return GetExpressionValueFromNode(unaryExp.Operand);
            }
            else if (expression is MemberExpression memberExp && memberExp.Expression is ConstantExpression)
            {
                // 处理闭包中的变量引用
                var memberConstExp = (ConstantExpression)memberExp.Expression;
                try
                {
                    if (memberExp.Member is PropertyInfo propInfo)
                        return propInfo.GetValue(memberConstExp.Value);
                    else if (memberExp.Member is FieldInfo fieldInfo)
                        return fieldInfo.GetValue(memberConstExp.Value);
                }
                catch
                {
                    // 获取失败时返回null
                }
            }
            return null;
        }

        private static bool MeetsAllConditions<T>(T item, List<SimpleCondition> conditions)
        {
            // 如果没有条件，默认返回true
            if (conditions == null || conditions.Count == 0)
                return true;

            // 处理AND和OR逻辑
            // 按照条件组进行处理，同一逻辑运算符的条件放在一组
            bool orResult = false;
            bool anyOrGroup = false;

            // 先检查是否有OR条件组
            if (conditions.Any(c => c.LogicalOperator == ExpressionType.OrElse))
            {
                anyOrGroup = true;
                // 对于OR条件，只要有一个满足就返回true
                orResult = conditions.Where(c => c.LogicalOperator == ExpressionType.OrElse)
                                     .Any(condition => MeetsCondition(item, condition));
            }

            // 对于AND条件，必须全部满足
            bool andResult = conditions.Where(c => c.LogicalOperator == ExpressionType.AndAlso)
                                      .All(condition => MeetsCondition(item, condition));

            // 如果有OR条件组，返回OR组的结果与AND组结果的AND
            // 如果只有AND条件组，返回AND组的结果
            // 如果只有OR条件组，返回OR组的结果
            if (anyOrGroup && conditions.Any(c => c.LogicalOperator == ExpressionType.AndAlso))
                return orResult && andResult;
            else if (anyOrGroup)
                return orResult;
            else
                return andResult;
        }

        private static bool MeetsCondition<T>(T item, SimpleCondition condition)
        {
            var propertyValue = GetPropertyValue(item, condition.PropertyName);

            return condition.Operator switch
            {
                ExpressionType.Equal => Equals(propertyValue, condition.Value),
                ExpressionType.NotEqual => !Equals(propertyValue, condition.Value),
                ExpressionType.GreaterThan => Compare(propertyValue, condition.Value) > 0,
                ExpressionType.LessThan => Compare(propertyValue, condition.Value) < 0,
                _ => true // 对于不支持的运算符，返回true
            };
        }

        private static object GetPropertyValue<T>(T item, string propertyName)
        {
            if (item == null) return null;
            var property = typeof(T).GetProperty(propertyName);
            return property?.GetValue(item);
        }

        private static int Compare(object x, object y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return Comparer.Default.Compare(x, y);
        }

        private class SimpleCondition
        {
            public string PropertyName { get; set; }
            public object Value { get; set; }
            public ExpressionType Operator { get; set; }
            public ExpressionType LogicalOperator { get; set; } = ExpressionType.AndAlso; // 默认逻辑运算符为AndAlso
        }



        /// <summary>
        /// 检查操作数是否为简单类型（属性访问或常量）
        /// </summary>
        private static bool IsSimpleOperand(Expression expression)
        {
            // 属性访问表达式
            if (expression is MemberExpression memberExp)
            {
                // 检查是否是参数的属性访问（例如 x.Property）
                return memberExp.Expression is ParameterExpression ||
                       // 或者是常量的属性访问（例如 5.ToString()）
                       memberExp.Expression is ConstantExpression;
            }
            // 常量表达式
            else if (expression is ConstantExpression)
            {
                return true;
            }
            // 类型转换表达式（例如 (int?)x.Property）
            else if (expression is UnaryExpression unaryExp &&
                     unaryExp.NodeType == ExpressionType.Convert)
            {
                return IsSimpleOperand(unaryExp.Operand);
            }

            return false;
        }

        /// <summary>
        /// 获取表达式的结构键（忽略变量值，只关注结构）
        /// </summary>
        private static string GetExpressionStructureKey<T>(Expression<Func<T, bool>> expression)
        {
            var structureVisitor = new StructureKeyVisitor();
            structureVisitor.Visit(expression.Body);
            return $"{typeof(T).FullName}_{structureVisitor.GetStructureKey()}";
        }

        /// <summary>
        /// 结构键生成访问者 - 为表达式结构生成唯一键（忽略变量值）
        /// </summary>
        private class StructureKeyVisitor : ExpressionVisitor
        {
            private readonly StringBuilder _keyBuilder = new StringBuilder();

            public string GetStructureKey() => _keyBuilder.ToString();

            protected override Expression VisitBinary(BinaryExpression node)
            {
                _keyBuilder.Append($"({node.NodeType.ToString()}");
                Visit(node.Left);
                _keyBuilder.Append(",");
                Visit(node.Right);
                _keyBuilder.Append(")");
                return node;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                _keyBuilder.Append($"{node.Member.DeclaringType.Name}.{node.Member.Name}");
                return node;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                _keyBuilder.Append($"param:{node.Name}");
                return node;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                // 对于常量，只记录类型而不记录值，确保相同结构但不同值的表达式生成相同的键
                _keyBuilder.Append($"const:{node.Type.Name}");
                return node;
            }
        }

        /// <summary>
        /// 闭包值提取器 - 提取表达式中的闭包变量值
        /// </summary>
        private class ClosureValueExtractor : ExpressionVisitor
        {
            private readonly List<string> _closureValues = new List<string>();

            public string GetClosureKey()
            {
                return string.Join("_", _closureValues);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                // 检测闭包变量（访问常量对象的成员）
                if (node.Expression is ConstantExpression constExp && !string.IsNullOrEmpty(constExp.Value?.ToString()))
                {
                    try
                    {
                        var value = GetMemberValue(constExp.Value, node.Member);
                        _closureValues.Add($"{node.Member.Name}:{(value != null ? value.GetHashCode().ToString() : "null")}");
                    }
                    catch { }
                }
                return base.VisitMember(node);
            }

            private object GetMemberValue(object instance, MemberInfo member)
            {
                if (member is PropertyInfo property)
                    return property.GetValue(instance);
                else if (member is FieldInfo field)
                    return field.GetValue(instance);
                return null;
            }
        }

        /// <summary>
        /// 尝试评估简单表达式（作为备选方案）
        /// </summary>
        public static bool EvaluateSimpleExpression<T>(T item, Expression expression)
        {
            if (expression is BinaryExpression binaryExp)
            {
                // 尝试评估二元表达式的左右两侧
                if (binaryExp.NodeType == ExpressionType.AndAlso ||
                    binaryExp.NodeType == ExpressionType.OrElse)
                {
                    bool leftResult = EvaluateSimpleExpression(item, binaryExp.Left);
                    if (binaryExp.NodeType == ExpressionType.AndAlso && !leftResult)
                        return false;
                    if (binaryExp.NodeType == ExpressionType.OrElse && leftResult)
                        return true;
                    return EvaluateSimpleExpression(item, binaryExp.Right);
                }
                else if (binaryExp.NodeType == ExpressionType.Equal ||
                         binaryExp.NodeType == ExpressionType.NotEqual)
                {
                    // 尝试获取左侧属性值和右侧常量值
                    object leftValue = GetExpressionValue(item, binaryExp.Left);
                    object rightValue = GetExpressionValue(item, binaryExp.Right);

                    bool areEqual = object.Equals(leftValue, rightValue);
                    return binaryExp.NodeType == ExpressionType.Equal ? areEqual : !areEqual;
                }
            }
            else if (expression is ConstantExpression constExp)
            {
                return constExp.Value as bool? ?? false;
            }
            else if (expression is MemberExpression memberExp)
            {
                // 尝试获取属性值并将其转换为布尔值
                object value = GetExpressionValue(item, memberExp);
                return value as bool? ?? false;
            }

            return true; // 无法评估时返回默认值
        }

        /// <summary>
        /// 获取表达式的值
        /// </summary>
        private static object GetExpressionValue<T>(T item, Expression expression)
        {
            if (expression is ConstantExpression constExp)
            {
                return constExp.Value;
            }
            else if (expression is MemberExpression memberExp)
            {
                // 处理属性访问
                if (memberExp.Expression != null)
                {
                    if (memberExp.Expression.Type == typeof(T))
                    {
                        // 直接访问item的属性
                        var property = typeof(T).GetProperty(memberExp.Member.Name);
                        if (property != null)
                        {
                            return property.GetValue(item);
                        }
                    }
                    else
                    {
                        // 访问其他对象的属性
                        object objValue = GetExpressionValue(item, memberExp.Expression);
                        if (objValue != null)
                        {
                            if (memberExp.Member is PropertyInfo propInfo)
                                return propInfo.GetValue(objValue);
                            else if (memberExp.Member is FieldInfo fieldInfo)
                                return fieldInfo.GetValue(objValue);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 增强版表达式树重建，更好地处理闭包变量替换
        /// </summary>
        private static Expression<Func<T, bool>> RebuildExpressionWithClosureReplacement<T>(Expression<Func<T, bool>> originalExpression)
        {
            try
            {
                // 创建闭包替换访问者，将闭包变量替换为常量
                var closureReplacer = new ClosureReplacer();
                var newBody = closureReplacer.Visit(originalExpression.Body);

                // 使用新的参数
                var newParameter = Expression.Parameter(typeof(T), "item");

                // 替换所有使用的参数
                var parameterReplacer = new ParameterReplacer(originalExpression.Parameters[0], newParameter);
                newBody = parameterReplacer.Visit(newBody);

                return Expression.Lambda<Func<T, bool>>(newBody, newParameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"增强版表达式重建失败: {ex.Message}");
                // 降级到原始的表达式重建方法
                return RebuildExpression(originalExpression);
            }
        }

        /// <summary>
        /// 闭包替换访问者 - 将闭包变量替换为它们的实际值（常量表达式）
        /// </summary>
        private class ClosureReplacer : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node)
            {
                // 检测闭包变量（访问常量对象的成员）
                if (node.Expression is ConstantExpression constExp)
                {
                    try
                    {
                        // 获取闭包变量的实际值
                        var value = GetMemberValue(constExp.Value, node.Member);
                        // 创建常量表达式替换闭包变量引用
                        return Expression.Constant(value, node.Type);
                    }
                    catch
                    {
                        // 如果无法获取值，继续正常处理
                    }
                }
                // 继续处理其他类型的成员访问
                return base.VisitMember(node);
            }

            private object GetMemberValue(object instance, MemberInfo member)
            {
                if (member is PropertyInfo property)
                    return property.GetValue(instance);
                else if (member is FieldInfo field)
                    return field.GetValue(instance);
                return null;
            }
        }

        /// <summary>
        /// 重新构建表达式树，解决变量作用域问题
        /// </summary>
        private static Expression<Func<T, bool>> RebuildExpression<T>(Expression<Func<T, bool>> originalExpression)
        {
            try
            {
                var visitor = new VariableCaptureVisitor();
                var newBody = visitor.Visit(originalExpression.Body);

                // 使用新的参数
                var newParameter = Expression.Parameter(typeof(T), "item");

                // 替换所有使用的参数，而不仅仅是第一个
                var usedParams = visitor.GetUsedParameters();
                if (usedParams.Any())
                {
                    foreach (var oldParam in usedParams)
                    {
                        var parameterReplacer = new ParameterReplacer(oldParam, newParameter);
                        newBody = parameterReplacer.Visit(newBody);
                    }
                }
                else
                {
                    // 如果没有检测到参数，使用原始表达式的参数进行替换
                    var parameterReplacer = new ParameterReplacer(originalExpression.Parameters[0], newParameter);
                    newBody = parameterReplacer.Visit(newBody);
                }

                return Expression.Lambda<Func<T, bool>>(newBody, newParameter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"重新构建表达式失败: {ex.Message}");
                // 如果重新构建失败，返回原始表达式的副本，只替换参数名
                var newParameter = Expression.Parameter(typeof(T), "item");
                var parameterReplacer = new ParameterReplacer(originalExpression.Parameters[0], newParameter);
                var newBody = parameterReplacer.Visit(originalExpression.Body);
                return Expression.Lambda<Func<T, bool>>(newBody, newParameter);
            }
        }

        /// <summary>
        /// 变量捕获访问者 - 检测并处理捕获的变量和外部引用
        /// </summary>
        private class VariableCaptureVisitor : ExpressionVisitor
        {
            private readonly List<ParameterExpression> _usedParameters = new List<ParameterExpression>();
            private readonly List<MemberExpression> _capturedMembers = new List<MemberExpression>();
            private readonly HashSet<Expression> _visitedExpressions = new HashSet<Expression>(); // 防止循环引用

            public IEnumerable<ParameterExpression> GetUsedParameters() => _usedParameters.Distinct();
            public IEnumerable<MemberExpression> GetCapturedMembers() => _capturedMembers;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (!_usedParameters.Contains(node))
                {
                    _usedParameters.Add(node);
                }
                return base.VisitParameter(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                // 防止循环引用
                if (_visitedExpressions.Contains(node))
                {
                    return node;
                }
                _visitedExpressions.Add(node);

                try
                {
                    // 处理各种类型的成员访问
                    if (node.Expression != null)
                    {
                        // 检测捕获的外部变量 (闭包中的变量)
                        if (node.Expression.NodeType == ExpressionType.Constant)
                        {
                            _capturedMembers.Add(node);

                            // 对于捕获的变量，尝试获取其值并替换为常量
                            try
                            {
                                var constantExpression = (ConstantExpression)node.Expression;
                                var value = GetMemberValue(constantExpression.Value, node.Member);
                                return Expression.Constant(value, node.Type);
                            }
                            catch
                            {
                                // 如果无法获取值，继续访问表达式的其他部分
                            }
                        }
                        // 处理其他类型的成员表达式，例如方法中的局部变量引用
                        else if (node.Expression.NodeType == ExpressionType.MemberAccess ||
                                 node.Expression.NodeType == ExpressionType.Parameter ||
                                 node.Expression.NodeType == ExpressionType.Call)
                        {
                            // 先访问表达式的左侧，确保正确处理嵌套表达式
                            var visitedExpression = Visit(node.Expression);
                            if (visitedExpression != node.Expression)
                            {
                                // 如果左侧表达式被修改，创建新的成员表达式
                                return Expression.MakeMemberAccess(visitedExpression, node.Member);
                            }
                        }
                    }

                    return base.VisitMember(node);
                }
                finally
                {
                    _visitedExpressions.Remove(node);
                }
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                // 处理二元表达式（如 &&, ||, == 等）
                var left = Visit(node.Left);
                var right = Visit(node.Right);

                // 只有当左右表达式有变化时，才创建新的二元表达式
                if (left != node.Left || right != node.Right)
                {
                    return Expression.MakeBinary(node.NodeType, left, right, node.IsLiftedToNull, node.Method);
                }

                return base.VisitBinary(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                // 处理方法调用表达式，确保正确处理方法调用中的参数
                var visitedObject = node.Object != null ? Visit(node.Object) : null;
                var visitedArgs = Visit(node.Arguments);

                if (visitedObject != node.Object || !visitedArgs.SequenceEqual(node.Arguments))
                {
                    return Expression.Call(visitedObject, node.Method, visitedArgs);
                }

                return base.VisitMethodCall(node);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                // 访问lambda表达式的参数和体
                var visitedParameters = node.Parameters.Select(p => Visit(p) as ParameterExpression).ToList();
                var visitedBody = Visit(node.Body);

                if (!visitedParameters.SequenceEqual(node.Parameters) || visitedBody != node.Body)
                {
                    return Expression.Lambda<T>(visitedBody, visitedParameters.ToArray());
                }

                return base.VisitLambda(node);
            }

            private object GetMemberValue(object instance, MemberInfo member)
            {
                if (member is PropertyInfo property)
                    return property.GetValue(instance);
                else if (member is FieldInfo field)
                    return field.GetValue(instance);
                else
                    throw new InvalidOperationException($"不支持的成员类型: {member.MemberType}");
            }
        }

        /// <summary>
        /// 参数替换访问者
        /// </summary>
        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter;
            private readonly ParameterExpression _newParameter;

            public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
            {
                _oldParameter = oldParameter;
                _newParameter = newParameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                // 直接比较参数引用
                if (node == _oldParameter ||
                    (node.Name == _oldParameter.Name && node.Type == _oldParameter.Type))
                {
                    return _newParameter;
                }
                return base.VisitParameter(node);
            }

            // 确保正确处理方法调用中的参数替换
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                var visitedObject = node.Object != null ? Visit(node.Object) : null;
                var visitedArgs = Visit(node.Arguments);
                return Expression.Call(visitedObject, node.Method, visitedArgs);
            }
        }
    }

}
