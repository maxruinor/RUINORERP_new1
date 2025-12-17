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
        /// 增强版：更好地处理闭包变量和复杂表达式
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
                    System.Diagnostics.Debug.WriteLine($"表达式详细信息: {expression}");

                    // 最后尝试使用条件提取方法作为备选
                    return obj => TryFilterWithConditionExtraction<T>(new List<T> { obj }, expression).Any();
                }
            }
            catch (Exception ex)
            {
                // 记录错误但返回默认评估函数
                System.Diagnostics.Debug.WriteLine($"获取评估函数失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"表达式详细信息: {expression}");
                
                // 返回一个使用条件提取方法的委托作为备选方案
                return obj => TryFilterWithConditionExtraction<T>(new List<T> { obj }, expression).Any();
            }
        }

        /// <summary>
        /// 尝试使用条件提取方法进行筛选
        /// 增强版：更好地处理闭包变量和复杂表达式
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
                    foreach (var condition in conditions)
                    {
                        System.Diagnostics.Debug.WriteLine($"条件: {condition.PropertyName} {condition.Operator} {condition.Value}");
                    }
                    
                    var filteredList = sourceList.Where(item => MeetsAllConditions(item, conditions)).ToList();
                    // 只有当筛选有结果时才返回，否则返回原始列表
                    return filteredList.Count > 0 ? filteredList : sourceList.ToList();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"未能提取到任何筛选条件，尝试直接编译表达式");
                    
                    // 作为备选方案，尝试直接编译和执行表达式
                    try
                    {
                        // 对于包含闭包的表达式，直接编译会失败，但我们可以尝试
                        return sourceList.Where(expCondition.Compile()).ToList();
                    }
                    catch (Exception directEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"直接编译表达式也失败: {directEx.Message}");
                        
                        // 如果直接编译失败，尝试手动分析表达式
                        return ManualFilterWithClosureVariables(sourceList, expCondition);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"条件提取失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"表达式详细信息: {expCondition}");
                
                // 最后的备选方案：尝试手动分析表达式
                return ManualFilterWithClosureVariables(sourceList, expCondition);
            }
        }
        
        /// <summary>
        /// 手动处理包含闭包变量的表达式 - 增强版，支持任意闭包变量
        /// </summary>
        private static List<T> ManualFilterWithClosureVariables<T>(List<T> sourceList, Expression<Func<T, bool>> expCondition) where T : class
        {
            try
            {
                // 使用通用的闭包变量解析方法
                var closureResolver = new ClosureVariableResolver<T>();
                closureResolver.AnalyzeExpression(expCondition);
                
                if (closureResolver.HasClosureVariables)
                {
                    System.Diagnostics.Debug.WriteLine($"检测到闭包变量，尝试通用解析方法");
                    
                    // 使用解析出的条件进行过滤
                    return sourceList.Where(item => 
                    {
                        return closureResolver.EvaluateConditions(item);
                    }).ToList();
                }
                
                // 如果没有闭包变量，返回原始列表
                System.Diagnostics.Debug.WriteLine($"未检测到闭包变量，返回原始列表");
                return sourceList.ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"通用闭包变量解析失败: {ex.Message}");
                return sourceList.ToList();
            }
        }
        

        
        /// <summary>
        /// 通用闭包变量解析器 - 支持任意类型的闭包变量和条件
        /// </summary>
        private class ClosureVariableResolver<T> where T : class
        {
            private readonly List<FilterCondition> _conditions = new List<FilterCondition>();
            private readonly List<LogicalOperator> _logicalOperators = new List<LogicalOperator>();
            
            public bool HasClosureVariables { get; private set; }
            
            /// <summary>
            /// 分析表达式，提取所有条件和闭包变量
            /// </summary>
            public void AnalyzeExpression(Expression<Func<T, bool>> expression)
            {
                _conditions.Clear();
                _logicalOperators.Clear();
                HasClosureVariables = false;
                
                var analyzer = new ClosureAnalyzer(_conditions, _logicalOperators);
                analyzer.Analyze(expression);
                
                HasClosureVariables = analyzer.HasClosureVariables;
                System.Diagnostics.Debug.WriteLine($"表达式分析完成，检测到 {_conditions.Count} 个条件，" +
                    $"包含闭包变量: {HasClosureVariables}");
            }
            
            /// <summary>
            /// 根据提取的条件评估项目
            /// </summary>
            public bool EvaluateConditions(T item)
            {
                if (_conditions.Count == 0)
                    return true;
                
                bool result = true;
                int conditionIndex = 0;
                int logicalOpIndex = 0;
                
                // 第一个条件作为初始结果
                if (_conditions.Count > 0)
                {
                    result = EvaluateCondition(item, _conditions[0]);
                    conditionIndex++;
                }
                
                // 处理剩余条件和逻辑运算符
                while (conditionIndex < _conditions.Count && logicalOpIndex < _logicalOperators.Count)
                {
                    var condition = _conditions[conditionIndex];
                    var logicalOp = _logicalOperators[logicalOpIndex];
                    
                    bool conditionResult = EvaluateCondition(item, condition);
                    
                    if (logicalOp.Type == ExpressionType.AndAlso)
                    {
                        result = result && conditionResult;
                    }
                    else if (logicalOp.Type == ExpressionType.OrElse)
                    {
                        result = result || conditionResult;
                    }
                    
                    conditionIndex++;
                    logicalOpIndex++;
                }
                
                return result;
            }
            
            private bool EvaluateCondition(T item, FilterCondition condition)
            {
                try
                {
                    // 获取属性值
                    object propertyValue = GetPropertyValue(item, condition.PropertyName);
                    
                    // 执行比较
                    return condition.Operator switch
                    {
                        ExpressionType.Equal => Equals(propertyValue, condition.Value),
                        ExpressionType.NotEqual => !Equals(propertyValue, condition.Value),
                        ExpressionType.GreaterThan => CompareValues(propertyValue, condition.Value) > 0,
                        ExpressionType.GreaterThanOrEqual => CompareValues(propertyValue, condition.Value) >= 0,
                        ExpressionType.LessThan => CompareValues(propertyValue, condition.Value) < 0,
                        ExpressionType.LessThanOrEqual => CompareValues(propertyValue, condition.Value) <= 0,
                        _ => true // 默认返回true
                    };
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"条件评估失败: {ex.Message}");
                    return true;
                }
            }
            
            private object GetPropertyValue(T item, string propertyName)
            {
                var property = typeof(T).GetProperty(propertyName);
                return property?.GetValue(item);
            }
            
            private int CompareValues(object x, object y)
            {
                if (x == null && y == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                
                // 尝试转换为可比较类型
                if (x is IComparable comparableX && y is IComparable comparableY)
                {
                    return comparableX.CompareTo(comparableY);
                }
                
                // 使用默认比较器
                return Comparer.Default.Compare(x, y);
            }
        }
        
        /// <summary>
        /// 过滤条件
        /// </summary>
        private class FilterCondition
        {
            public string PropertyName { get; set; }
            public object Value { get; set; }
            public ExpressionType Operator { get; set; }
        }
        
        /// <summary>
        /// 逻辑运算符
        /// </summary>
        private class LogicalOperator
        {
            public ExpressionType Type { get; set; }
        }
        
        /// <summary>
        /// 闭包分析器 - 从表达式中提取条件和闭包变量
        /// </summary>
        private class ClosureAnalyzer : ExpressionVisitor
        {
            private readonly List<FilterCondition> _conditions;
            private readonly List<LogicalOperator> _logicalOperators;
            
            public bool HasClosureVariables { get; private set; }
            
            public ClosureAnalyzer(List<FilterCondition> conditions, List<LogicalOperator> logicalOperators)
            {
                _conditions = conditions;
                _logicalOperators = logicalOperators;
            }
            
            public void Analyze<T>(Expression<Func<T, bool>> expression) where T : class
            {
                Visit(expression.Body);
            }
            
            protected override Expression VisitBinary(BinaryExpression node)
            {
                // 处理逻辑运算符
                if (node.NodeType == ExpressionType.AndAlso || node.NodeType == ExpressionType.OrElse)
                {
                    // 递归处理左右表达式
                    Visit(node.Left);
                    
                    // 添加逻辑运算符
                    _logicalOperators.Add(new LogicalOperator { Type = node.NodeType });
                    
                    Visit(node.Right);
                    return node;
                }
                
                // 处理比较表达式
                if (IsComparisonOperator(node.NodeType))
                {
                    var condition = ExtractCondition(node);
                    if (condition != null)
                    {
                        _conditions.Add(condition);
                        
                        // 检查是否包含闭包变量
                        if (HasClosureVariable(node) || HasClosureVariableInValue(node))
                        {
                            HasClosureVariables = true;
                        }
                    }
                }
                
                return base.VisitBinary(node);
            }
            
            private bool IsComparisonOperator(ExpressionType nodeType)
            {
                return nodeType == ExpressionType.Equal ||
                       nodeType == ExpressionType.NotEqual ||
                       nodeType == ExpressionType.GreaterThan ||
                       nodeType == ExpressionType.GreaterThanOrEqual ||
                       nodeType == ExpressionType.LessThan ||
                       nodeType == ExpressionType.LessThanOrEqual;
            }
            
            private FilterCondition ExtractCondition(BinaryExpression binaryNode)
            {
                // 尝试获取属性名和值
                string propertyName = null;
                object value = null;
                
                // 检查左侧是否是属性访问
                if (binaryNode.Left is MemberExpression leftMember && 
                    leftMember.Expression is ParameterExpression)
                {
                    propertyName = leftMember.Member.Name;
                    value = ExtractValueFromNode(binaryNode.Right);
                }
                // 检查右侧是否是属性访问
                else if (binaryNode.Right is MemberExpression rightMember && 
                         rightMember.Expression is ParameterExpression)
                {
                    propertyName = rightMember.Member.Name;
                    value = ExtractValueFromNode(binaryNode.Left);
                }
                
                if (!string.IsNullOrEmpty(propertyName))
                {
                    return new FilterCondition
                    {
                        PropertyName = propertyName,
                        Value = value,
                        Operator = binaryNode.NodeType
                    };
                }
                
                return null;
            }
            
            private object ExtractValueFromNode(Expression node)
            {
                // 常量表达式
                if (node is ConstantExpression constExp)
                {
                    return constExp.Value;
                }
                
                // 类型转换表达式
                if (node is UnaryExpression unaryExp && unaryExp.NodeType == ExpressionType.Convert)
                {
                    var innerValue = ExtractValueFromNode(unaryExp.Operand);
                    if (innerValue != null)
                    {
                        try
                        {
                            return Convert.ChangeType(innerValue, unaryExp.Type);
                        }
                        catch
                        {
                            // 转换失败，返回原始值
                        }
                    }
                    return innerValue;
                }
                
                // 闭包变量表达式
                if (node is MemberExpression memberExp)
                {
                    return GetClosureVariableValue(memberExp);
                }
                
                return null;
            }
            
            private object GetClosureVariableValue(MemberExpression memberExp)
            {
                try
                {
                    // 首先检查是否是闭包变量（通常以 value(...) 开头）
                    string memberStr = memberExp.ToString();
                    if (memberStr.Contains("value("))
                    {
                        // 对于闭包变量，我们需要通过编译表达式来获取值
                        try
                        {
                            var lambda = Expression.Lambda(memberExp);
                            var compiled = lambda.Compile();
                            return compiled.DynamicInvoke();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"编译闭包变量表达式失败: {ex.Message}");
                            // 如果编译失败，尝试其他方法
                        }
                    }
                    
                    // 递归获取闭包变量值
                    if (memberExp.Expression is ConstantExpression constExp)
                    {
                        object obj = constExp.Value;
                        if (obj != null)
                        {
                            // 获取属性值
                            if (memberExp.Member is PropertyInfo propInfo)
                                return propInfo.GetValue(obj);
                            else if (memberExp.Member is FieldInfo fieldInfo)
                                return fieldInfo.GetValue(obj);
                        }
                    }
                    else if (memberExp.Expression is MemberExpression parentMemberExp)
                    {
                        // 处理嵌套属性访问
                        var parentValue = GetClosureVariableValue(parentMemberExp);
                        if (parentValue != null)
                        {
                            if (memberExp.Member is PropertyInfo propInfo)
                                return propInfo.GetValue(parentValue);
                            else if (memberExp.Member is FieldInfo fieldInfo)
                                return fieldInfo.GetValue(parentValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"获取闭包变量值失败: {ex.Message}");
                }
                
                return null;
            }
            
            private bool HasClosureVariable(Expression expression)
            {
                return expression.ToString().Contains("value(") ||
                       (expression is MemberExpression memberExp && 
                        memberExp.Expression is ConstantExpression);
            }
            
            private bool HasClosureVariableInValue(BinaryExpression binaryNode)
            {
                return HasClosureVariable(binaryNode.Left) || HasClosureVariable(binaryNode.Right);
            }
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
        /// 增强版：更好地处理闭包变量和复杂表达式
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
                    
                    // 允许null值进行比较
                    if (value != null || 
                        (binary.Right is ConstantExpression constExp && constExp.Value == null) ||
                        (binary.Right is UnaryExpression unaryExp && unaryExp.NodeType == ExpressionType.Convert && 
                         unaryExp.Operand is ConstantExpression unaryConstExp && unaryConstExp.Value == null))
                    {
                        conditions.Add(new SimpleCondition
                        {
                            PropertyName = propertyName,
                            Value = value,
                            Operator = binary.NodeType,
                            LogicalOperator = logicalOperator
                        });
                    }
                    else
                    {
                        // 如果无法获取值，尝试从DebugView或其他方式获取
                        System.Diagnostics.Debug.WriteLine($"无法提取表达式的值: {binary.Right}");
                    }
                }
            }
        }

        /// <summary>
        /// 从表达式节点获取属性名
        /// 增强版：正确处理不同类型的表达式节点
        /// </summary>
        private static string GetPropertyName(Expression expression)
        {
            if (expression is MemberExpression memberExpr)
            {
                // 如果成员表达式是针对参数的（如 t.PropertyName）
                if (memberExpr.Expression is ParameterExpression)
                {
                    return memberExpr.Member.Name;
                }
                // 处理嵌套成员访问（如 obj.Property.Name）
                else if (memberExpr.Expression is MemberExpression parentMemberExpr)
                {
                    return memberExpr.Member.Name;
                }
                // 处理常量成员访问（如闭包变量）
                else if (memberExpr.Expression is ConstantExpression)
                {
                    return memberExpr.Member.Name;
                }
            }
            return null;
        }

        /// <summary>
        /// 从表达式节点获取值，支持常量表达式和类型转换表达式
        /// 增强版：更好地处理闭包变量和复杂表达式
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
                var innerValue = GetExpressionValueFromNode(unaryExp.Operand);
                
                // 尝试进行类型转换
                if (innerValue != null && unaryExp.Type != null && unaryExp.Type != innerValue.GetType())
                {
                    try
                    {
                        // 处理可空类型转换
                        if (unaryExp.Type.IsGenericType && unaryExp.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            var underlyingType = Nullable.GetUnderlyingType(unaryExp.Type);
                            if (underlyingType != null)
                            {
                                return Convert.ChangeType(innerValue, underlyingType);
                            }
                        }
                        
                        return Convert.ChangeType(innerValue, unaryExp.Type);
                    }
                    catch
                    {
                        // 转换失败时返回原始值
                    }
                }
                
                return innerValue;
            }
            else if (expression is MemberExpression memberExp)
            {
                // 处理闭包中的变量引用（如 value(RUINORERP.UI.PSI.PUR.UCPurOrder+<>c__DisplayClass5_0).entity.CustomerVendor_ID）
                if (memberExp.Expression is ConstantExpression memberConstExp)
                {
                    try
                    {
                        var obj = memberConstExp.Value;
                        
                        // 递归获取属性值，支持多层属性访问
                        return GetNestedPropertyValue(obj, memberExp);
                    }
                    catch
                    {
                        // 获取失败时返回null
                    }
                }
                // 处理字段访问表达式
                else if (memberExp.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    try
                    {
                        // 递归处理嵌套的成员访问
                        var parentValue = GetExpressionValueFromNode(memberExp.Expression);
                        if (parentValue != null)
                        {
                            if (memberExp.Member is PropertyInfo propInfo)
                                return propInfo.GetValue(parentValue);
                            else if (memberExp.Member is FieldInfo fieldInfo)
                                return fieldInfo.GetValue(parentValue);
                        }
                    }
                    catch
                    {
                        // 获取失败时返回null
                    }
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// 递归获取嵌套属性的值
        /// </summary>
        private static object GetNestedPropertyValue(object obj, MemberExpression memberExp)
        {
            if (obj == null) return null;
            
            // 如果是直接属性访问
            if (memberExp.Expression is ConstantExpression)
            {
                if (memberExp.Member is PropertyInfo propInfo)
                    return propInfo.GetValue(obj);
                else if (memberExp.Member is FieldInfo fieldInfo)
                    return fieldInfo.GetValue(obj);
            }
            
            // 处理嵌套属性访问
            if (memberExp.Expression is MemberExpression parentMemberExp)
            {
                var parentObj = GetExpressionValueFromNode(parentMemberExp);
                if (parentObj != null)
                {
                    if (memberExp.Member is PropertyInfo propInfo)
                        return propInfo.GetValue(parentObj);
                    else if (memberExp.Member is FieldInfo fieldInfo)
                        return fieldInfo.GetValue(parentObj);
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
                        // 使用实际值而不是哈希码，确保不同值生成不同的缓存键
                        _closureValues.Add($"{node.Member.Name}:{value}");
                    }
                    catch { }
                }
                return base.VisitMember(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                // 对于常量表达式，也添加到缓存键中
                if (node.Value != null)
                {
                    _closureValues.Add($"const:{node.Value}");
                }
                return base.VisitConstant(node);
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

                    // 处理可空布尔值比较
                    if (leftValue is bool leftBool && rightValue is bool rightBool)
                    {
                        bool areEqual = leftBool == rightBool;
                        return binaryExp.NodeType == ExpressionType.Equal ? areEqual : !areEqual;
                    }
                    // 处理普通布尔值比较
                    else if (leftValue is bool leftBool2 && rightValue is bool rightBool2)
                    {
                        bool areEqual = leftBool2 == rightBool2;
                        return binaryExp.NodeType == ExpressionType.Equal ? areEqual : !areEqual;
                    }
                    // 处理其他类型的比较
                    else
                    {
                        bool areEqual = object.Equals(leftValue, rightValue);
                        return binaryExp.NodeType == ExpressionType.Equal ? areEqual : !areEqual;
                    }
                }
            }
            else if (expression is UnaryExpression unaryExp && unaryExp.NodeType == ExpressionType.Convert)
            {
                // 处理类型转换表达式，如 Convert(True)
                object innerValue = GetExpressionValue(item, unaryExp.Operand);
                if (innerValue != null)
                {
                    try
                    {
                        object convertedValue = Convert.ChangeType(innerValue, unaryExp.Type);
                        return convertedValue as bool? ?? false;
                    }
                    catch
                    {
                        // 转换失败，尝试返回原始值的布尔表示
                        return innerValue as bool? ?? false;
                    }
                }
                return false;
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
            else if (expression is UnaryExpression unaryExp && unaryExp.NodeType == ExpressionType.Convert)
            {
                // 处理类型转换表达式，如 Convert(True)
                object innerValue = GetExpressionValue(item, unaryExp.Operand);
                if (innerValue != null)
                {
                    // 尝试转换为目标类型
                    try
                    {
                        // 特殊处理可空值类型的转换
                        Type targetType = unaryExp.Type;
                        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            // 处理可空值类型转换
                            Type underlyingType = Nullable.GetUnderlyingType(targetType);
                            if (underlyingType != null)
                            {
                                // 先转换为底层类型，再创建可空类型
                                object convertedValue = Convert.ChangeType(innerValue, underlyingType);
                                return convertedValue;
                            }
                        }
                        else
                        {
                            // 普通类型转换
                            return Convert.ChangeType(innerValue, targetType);
                        }
                    }
                    catch
                    {
                        // 转换失败，返回原始值
                        return innerValue;
                    }
                }
                return null;
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
