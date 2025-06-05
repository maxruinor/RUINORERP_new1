using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Reflection;


namespace RUINORERP.Common.Extensions
{

    public static class FilterParser
    {
        private enum TokenType
        {
            Identifier,
            String,
            Number,
            Date,
            Boolean,
            Operator,
            Null,
            Comma,
            LeftParen,
            RightParen,
            EOF
        }

        private class Token
        {
            public TokenType Type { get; set; }
            public string Value { get; set; }
            public object LiteralValue { get; set; }
            public int Position { get; set; }
            public Type DataType { get; set; } // 添加数据类型信息
        }


        // 重载 Parse 方法，支持泛型类型
        public static Expression<Func<T, bool>> Parse<T>(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return row => true;

            var tokens = Tokenize(filter);
            var parser = new Parser<T>(tokens);
            return parser.Parse();
            //return Expression.Lambda<Func<T, bool>>(parser.Parse(), parser.Parameter);
        }


        public static Expression<Func<DataRow, bool>> Parse(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                return row => true;

            var tokens = Tokenize(filter);
            var parser = new Parser(tokens);
            return parser.Parse();
        }

        private static List<Token> Tokenize(string input)
        {
            var tokens = new List<Token>();
            int index = 0;
            int length = input.Length;

            while (index < length)
            {
                char c = input[index];

                if (char.IsWhiteSpace(c))
                {
                    index++;
                    continue;
                }

                if (c == '[') tokens.Add(ReadBracketedIdentifier(input, ref index));
                else if (c == '\'') tokens.Add(ReadString(input, ref index));
                else if (c == '#') tokens.Add(ReadDate(input, ref index));
                else if (char.IsDigit(c)) tokens.Add(ReadNumber(input, ref index));
                else if (char.IsLetter(c)) tokens.Add(ReadKeywordOrIdentifier(input, ref index));
                else if (c == '(') tokens.Add(NewToken(TokenType.LeftParen, "(", index++));
                else if (c == ')') tokens.Add(NewToken(TokenType.RightParen, ")", index++));
                else if (c == ',') tokens.Add(NewToken(TokenType.Comma, ",", index++));
                else if (c == '=') tokens.Add(NewToken(TokenType.Operator, "=", index++));
                else if (c == '!' && Peek(input, index) == '=')
                    tokens.Add(NewToken(TokenType.Operator, "!=", index += 2));
                else if (c == '<' && Peek(input, index) == '=')
                    tokens.Add(NewToken(TokenType.Operator, "<=", index += 2));
                else if (c == '<' && Peek(input, index) == '>')
                    tokens.Add(NewToken(TokenType.Operator, "<>", index += 2));
                else if (c == '<') tokens.Add(NewToken(TokenType.Operator, "<", index++));
                else if (c == '>' && Peek(input, index) == '=')
                    tokens.Add(NewToken(TokenType.Operator, ">=", index += 2));
                else if (c == '>') tokens.Add(NewToken(TokenType.Operator, ">", index++));
                else
                    throw new FormatException($"Unexpected character '{c}' at position {index}");
            }

            tokens.Add(new Token { Type = TokenType.EOF, Value = "", Position = index });
            return tokens;
        }

        private static Token NewToken(TokenType type, string value, int position)
        {
            return new Token { Type = type, Value = value, Position = position };
        }

        private static Token ReadBracketedIdentifier(string input, ref int index)
        {
            int start = index;
            index++; // Skip [
            while (index < input.Length && input[index] != ']')
                index++;

            if (index >= input.Length)
                throw new FormatException("Unterminated bracketed identifier");

            string value = input.Substring(start + 1, index - start - 1);
            index++; // Skip ]
            return NewToken(TokenType.Identifier, value, start);
        }

        private static Token ReadString(string input, ref int index)
        {
            int start = index;
            index++; // Skip opening quote
            var sb = new System.Text.StringBuilder();

            while (index < input.Length)
            {
                if (input[index] == '\'')
                {
                    if (index + 1 < input.Length && input[index + 1] == '\'')
                    {
                        sb.Append('\'');
                        index += 2;
                    }
                    else
                    {
                        index++;
                        break;
                    }
                }
                else
                {
                    sb.Append(input[index++]);
                }
            }

            return new Token
            {
                Type = TokenType.String,
                Value = sb.ToString(),
                LiteralValue = sb.ToString(),
                DataType = typeof(string),
                Position = start
            };

            //return NewToken(TokenType.String, sb.ToString(), start);
        }

        private static Token ReadDate(string input, ref int index)
        {
            int start = index;
            index++; // Skip #
            int end = input.IndexOf('#', index);
            if (end == -1)
                throw new FormatException("Unterminated date literal");

            string dateStr = input.Substring(index, end - index);
            index = end + 1; // Skip closing #

            if (DateTime.TryParse(dateStr, out DateTime date))
                return new Token { Type = TokenType.Date, Value = dateStr, LiteralValue = date, DataType = typeof(DateTime), Position = start };

            throw new FormatException($"Invalid date format: {dateStr}");
        }

        private static Token ReadNumber(string input, ref int index)
        {
            int start = index;
            bool hasDecimal = false;

            while (index < input.Length)
            {
                char c = input[index];
                if (char.IsDigit(c)) index++;
                else if (c == '.' && !hasDecimal)
                {
                    hasDecimal = true;
                    index++;
                }
                else break;
            }

            string numStr = input.Substring(start, index - start);

            if (int.TryParse(numStr, out int intValue))
                return new Token { Type = TokenType.Number, Value = numStr, LiteralValue = intValue, DataType = typeof(int), Position = start };

            if (double.TryParse(numStr, out double doubleValue))
                return new Token { Type = TokenType.Number, Value = numStr, LiteralValue = doubleValue, DataType = typeof(double), Position = start };

            throw new FormatException($"Invalid number format: {numStr}");
        }

        private static Token ReadKeywordOrIdentifier(string input, ref int index)
        {
            int start = index;
            while (index < input.Length && (char.IsLetterOrDigit(input[index]) || input[index] == '_'))
                index++;

            string value = input.Substring(start, index - start);

            return value.ToUpper() switch
            {
                "AND" => NewToken(TokenType.Operator, "AND", start),
                "OR" => NewToken(TokenType.Operator, "OR", start),
                "NOT" => NewToken(TokenType.Operator, "NOT", start),
                "LIKE" => NewToken(TokenType.Operator, "LIKE", start),
                "IN" => NewToken(TokenType.Operator, "IN", start),
                "IS" => NewToken(TokenType.Operator, "IS", start),
                "NULL" => NewToken(TokenType.Null, "NULL", start),
                "TRUE" => new Token { Type = TokenType.Boolean, Value = value, LiteralValue = true, DataType = typeof(bool), Position = start },
                "FALSE" => new Token { Type = TokenType.Boolean, Value = value, LiteralValue = false, DataType = typeof(bool), Position = start },
                _ => NewToken(TokenType.Identifier, value, start)
            };
        }

        private static char Peek(string input, int index)
        {
            return index + 1 < input.Length ? input[index + 1] : '\0';
        }

        private class Parser
        {
            private readonly List<Token> tokens;
            private int currentIndex;
            public readonly ParameterExpression Parameter;
            private readonly Type targetType;
            private readonly Dictionary<string, PropertyInfo> propertyCache = new Dictionary<string, PropertyInfo>();
            public Parser(List<Token> tokens, Type targetType)
            {
                this.tokens = tokens;
                currentIndex = 0;
                this.targetType = targetType;
                this.Parameter = Expression.Parameter(targetType, "x");

                // 缓存属性信息
                foreach (var property in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    propertyCache[property.Name.ToLower()] = property;
                }
            }


            private readonly ParameterExpression rowParam = Expression.Parameter(typeof(DataRow), "row");
            private readonly Dictionary<string, Type> columnTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            public Parser(List<Token> tokens)
            {
                this.tokens = tokens;
                currentIndex = 0;
            }

            public Expression<Func<DataRow, bool>> Parse()
            {
                var expr = ParseExpression();
                return Expression.Lambda<Func<DataRow, bool>>(expr, rowParam);
            }

            private Expression ParseExpression()
            {
                return ParseOr();
            }

            private Expression ParseOr()
            {
                var left = ParseAnd();
                while (Match(TokenType.Operator, "OR"))
                {
                    var right = ParseAnd();
                    left = Expression.OrElse(left, right);
                }
                return left;
            }

            private Expression ParseAnd()
            {
                var left = ParseNot();
                while (Match(TokenType.Operator, "AND"))
                {
                    var right = ParseNot();
                    left = Expression.AndAlso(left, right);
                }
                return left;
            }

            private Expression ParseNot()
            {
                if (Match(TokenType.Operator, "NOT"))
                {
                    return Expression.Not(ParsePrimary());
                }
                return ParsePrimary();
            }

            private Expression ParsePrimary()
            {
                if (Match(TokenType.LeftParen))
                {
                    var expr = ParseExpression();
                    Expect(TokenType.RightParen, ")");
                    return expr;
                }
                return ParseComparison();
            }

            private Expression ParseComparison()
            {
                var left = ParseOperand();

                if (Match(TokenType.Operator, "=")) return CreateComparison(left, ParseOperand(), Expression.Equal);
                if (Match(TokenType.Operator, "!=") || Match(TokenType.Operator, "<>"))
                    return CreateComparison(left, ParseOperand(), Expression.NotEqual);
                if (Match(TokenType.Operator, "<")) return CreateComparison(left, ParseOperand(), Expression.LessThan);
                if (Match(TokenType.Operator, ">")) return CreateComparison(left, ParseOperand(), Expression.GreaterThan);
                if (Match(TokenType.Operator, "<=")) return CreateComparison(left, ParseOperand(), Expression.LessThanOrEqual);
                if (Match(TokenType.Operator, ">=")) return CreateComparison(left, ParseOperand(), Expression.GreaterThanOrEqual);
                if (Match(TokenType.Operator, "LIKE")) return CreateLikeExpression(left, ParseOperand());
                if (Match(TokenType.Operator, "IN")) return CreateInExpression(left);
                if (Match(TokenType.Operator, "IS")) return CreateNullCheck(left);

                throw new FormatException($"Expected operator at position {CurrentToken.Position}");
            }

            private Expression ParseOperand()
            {
                if (Match(TokenType.Identifier, out var identToken))
                    return CreateColumnAccess(identToken.Value);

                if (Match(TokenType.String, out var strToken))
                    return Expression.Constant(strToken.LiteralValue);

                if (Match(TokenType.Number, out var numToken))
                    return Expression.Constant(numToken.LiteralValue);

                if (Match(TokenType.Date, out var dateToken))
                    return Expression.Constant(dateToken.LiteralValue);

                if (Match(TokenType.Boolean, out var boolToken))
                    return Expression.Constant(boolToken.LiteralValue);

                if (Match(TokenType.Null))
                    return Expression.Constant(null, typeof(object));

                //if (Match(TokenType.LeftParen))
                //{
                //    var expr = ParseOperandList();
                //    Expect(TokenType.RightParen, ")");
                //    return expr;
                //}

                throw new FormatException($"Expected operand at position {CurrentToken.Position}");
            }

            private List<Expression> ParseOperandList()
            {
                var list = new List<Expression>();
                do
                {
                    list.Add(ParseOperand());
                }
                while (Match(TokenType.Comma));
                return list;
            }

            private Expression CreateColumnAccess(string columnName)
            {
                // 如果是 DataRow，保持原有逻辑
                if (targetType == typeof(DataRow))
                {
                    var columnRowExpr = Expression.Property(Parameter, "Item", Expression.Constant(columnName));

                    // 尝试确定列类型
                    Type columnRowType = typeof(object);

                    return Expression.Condition(
                        Expression.Equal(columnRowExpr, Expression.Constant(DBNull.Value)),
                        Expression.Default(columnRowType),
                        Expression.Convert(columnRowExpr, columnRowType)
                    );
                }

                // 对于普通对象类型，通过反射获取属性值
                if (!propertyCache.TryGetValue(columnName.ToLower(), out var property))
                {
                    throw new ArgumentException($"Property '{columnName}' not found on type '{targetType.Name}'");
                }

                var propertyAccess = Expression.Property(Parameter, property);

                // 处理可为空的类型
                Type propertyType = property.PropertyType;
                if (propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null)
                {
                    return propertyAccess;
                }

                // 对于引用类型或可为空的值类型，添加空检查
                return Expression.Condition(
                    Expression.Equal(propertyAccess, Expression.Constant(null, propertyType)),
                    Expression.Default(propertyType),
                    propertyAccess
                );



                // Handle DBNull -> null conversion
                var columnExpr = Expression.Property(rowParam, "Item", Expression.Constant(columnName));

                // 尝试确定列类型
                Type columnType = typeof(object);
                if (columnTypes.TryGetValue(columnName, out var knownType))
                {
                    columnType = knownType;
                }

                return Expression.Condition(
                    Expression.Equal(columnExpr, Expression.Constant(DBNull.Value)),
                      Expression.Default(columnType),
                      //Expression.Constant(null, typeof(object)),
                      Expression.Convert(columnExpr, columnType)
                //Expression.Convert(columnExpr, typeof(object))
                );
            }

            private Expression CreateComparison(Expression left, Expression right, Func<Expression, Expression, Expression> comparer)
            {
                // 统一类型
                //if (left.Type != right.Type)
                //{
                //    if (left.Type == typeof(object) && right.Type != typeof(object))
                //        left = Expression.Convert(left, right.Type);
                //    else if (right.Type == typeof(object) && left.Type != typeof(object))
                //        right = Expression.Convert(right, left.Type);
                //}
                //return comparer(left, right);
                // 统一类型
                if (left.Type != right.Type)
                {
                    // 尝试将右侧转换为左侧类型
                    if (right is ConstantExpression rightConst && rightConst.Value != null)
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(rightConst.Value, left.Type);
                            right = Expression.Constant(convertedValue, left.Type);
                        }
                        catch
                        {
                            // 转换失败，尝试反向转换
                            try
                            {
                                var convertedValue = Convert.ChangeType(Expression.Lambda(left).Compile().DynamicInvoke(), right.Type);
                                left = Expression.Constant(convertedValue, right.Type);
                            }
                            catch
                            {
                                throw new InvalidOperationException($"Cannot compare {left.Type.Name} and {right.Type.Name}");
                            }
                        }
                    }
                    // 尝试将左侧转换为右侧类型
                    else if (left is ConstantExpression leftConst && leftConst.Value != null)
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(leftConst.Value, right.Type);
                            left = Expression.Constant(convertedValue, right.Type);
                        }
                        catch
                        {
                            throw new InvalidOperationException($"Cannot compare {left.Type.Name} and {right.Type.Name}");
                        }
                    }
                }

                return comparer(left, right);

            }

            private Expression CreateLikeExpression_OLD(Expression left, Expression right)
            {
                if (left.Type != typeof(string))
                    left = Expression.Convert(left, typeof(string));

                if (right.Type != typeof(string))
                    right = Expression.Convert(right, typeof(string));

                if (right is not ConstantExpression ce || ce.Value is not string pattern)
                    throw new FormatException("LIKE operator requires a string pattern");

                var stringValue = Expression.Convert(left, typeof(string));
                var patternValue = Expression.Constant(pattern);

                if (pattern.StartsWith("%") && pattern.EndsWith("%"))
                {
                    return Expression.Call(stringValue, "Contains", null,
                        Expression.Constant(pattern.Trim('%')));
                }
                if (pattern.StartsWith("%"))
                {
                    return Expression.Call(stringValue, "EndsWith", null,
                        Expression.Constant(pattern.Trim('%')));
                }
                if (pattern.EndsWith("%"))
                {
                    return Expression.Call(stringValue, "StartsWith", null,
                        Expression.Constant(pattern.Trim('%')));
                }

                // Use regex for complex patterns
                var regex = new Regex("^" + Regex.Escape(pattern)
                    .Replace("%", ".*")
                    .Replace("_", ".") + "$", RegexOptions.IgnoreCase);

                return Expression.Call(
                    typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string) }),
                    stringValue,
                    Expression.Constant(regex.ToString())
                );
            }
            private Expression CreateLikeExpression(Expression left, Expression right)
            {
                if (left.Type != typeof(string))
                    left = Expression.Convert(left, typeof(string));

                if (right.Type != typeof(string))
                    right = Expression.Convert(right, typeof(string));

                if (right is ConstantExpression ce && ce.Value is string pattern)
                {
                    if (pattern.StartsWith("%") && pattern.EndsWith("%"))
                    {
                        return Expression.Call(
                            left,
                            "Contains",
                            null,
                            Expression.Constant(pattern.Trim('%')));
                    }
                    if (pattern.StartsWith("%"))
                    {
                        return Expression.Call(
                            left,
                            "EndsWith",
                            null,
                            Expression.Constant(pattern.Trim('%')));
                    }
                    if (pattern.EndsWith("%"))
                    {
                        return Expression.Call(
                            left,
                            "StartsWith",
                            null,
                            Expression.Constant(pattern.Trim('%')));
                    }

                    // 使用正则表达式处理复杂模式
                    var regex = new Regex("^" + Regex.Escape(pattern)
                        .Replace("%", ".*")
                        .Replace("_", ".") + "$", RegexOptions.IgnoreCase);

                    return Expression.Call(
                        typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string) }),
                        left,
                        Expression.Constant(regex.ToString())
                    );
                }

                throw new FormatException("LIKE operator requires a string pattern");
            }
            private Expression CreateInExpression_OLD(Expression left)
            {
                Expect(TokenType.LeftParen, "(");
                var values = ParseOperandList();
                Expect(TokenType.RightParen, ")");

                var containsMethod = typeof(Enumerable).GetMethods()
                    .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(object));

                var valuesArray = Expression.Constant(values.Select(e => ((ConstantExpression)e).Value).ToArray());
                return Expression.Call(containsMethod, valuesArray, left);
            }
            private Expression CreateInExpression(Expression left)
            {
                Expect(TokenType.LeftParen, "(");
                var values = new List<Expression>();

                do
                {
                    values.Add(ParseOperand());
                }
                while (Match(TokenType.Comma));

                Expect(TokenType.RightParen, ")");

                // 创建OR条件表达式
                Expression condition = Expression.Constant(false);
                foreach (var value in values)
                {
                    var equal = CreateComparison(left, value, Expression.Equal);
                    condition = Expression.OrElse(condition, equal);
                }

                return condition;
            }
            private Expression CreateNullCheck(Expression left)
            {
                if (Match(TokenType.Null))
                    return Expression.Equal(left, Expression.Constant(null));

                if (Match(TokenType.Operator, "NOT") && Match(TokenType.Null))
                    return Expression.NotEqual(left, Expression.Constant(null));

                throw new FormatException("Expected NULL or NOT NULL after IS");
            }

            private Token CurrentToken => currentIndex < tokens.Count ? tokens[currentIndex] : null;

            private bool Match(TokenType type)
            {
                if (CurrentToken?.Type == type)
                {
                    currentIndex++;
                    return true;
                }
                return false;
            }

            private bool Match(TokenType type, string value)
            {
                if (CurrentToken?.Type == type &&
                    string.Equals(CurrentToken.Value, value, StringComparison.OrdinalIgnoreCase))
                {
                    currentIndex++;
                    return true;
                }
                return false;
            }

            private bool Match(TokenType type, out Token token)
            {
                token = null;
                if (CurrentToken?.Type != type) return false;
                token = CurrentToken;
                currentIndex++;
                return true;
            }

            private void Expect(TokenType type, string expected)
            {
                if (CurrentToken?.Type != type)
                    throw new FormatException($"Expected '{expected}' at position {CurrentToken?.Position}");
                currentIndex++;
            }


            //// 扩展方法：提供列类型信息
            //public static void SetColumnType(this Expression<Func<DataRow, bool>> expression, string columnName, Type columnType)
            //{
            //    if (expression.Body is BinaryExpression binary &&
            //        binary.NodeType == ExpressionType.Lambda)
            //    {
            //        if (binary.Left is BinaryExpression leftBinary)
            //        {
            //            SetColumnTypeRecursive(leftBinary, columnName, columnType);
            //        }
            //    }
            //}

            //private static void SetColumnTypeRecursive(Expression expr, string columnName, Type columnType)
            //{
            //    if (expr is BinaryExpression binary)
            //    {
            //        SetColumnTypeRecursive(binary.Left, columnName, columnType);
            //        SetColumnTypeRecursive(binary.Right, columnName, columnType);
            //    }
            //    else if (expr is MethodCallExpression call)
            //    {
            //        foreach (var arg in call.Arguments)
            //        {
            //            SetColumnTypeRecursive(arg, columnName, columnType);
            //        }
            //    }
            //    else if (expr is ConditionalExpression conditional)
            //    {
            //        SetColumnTypeRecursive(conditional.IfTrue, columnName, columnType);
            //        SetColumnTypeRecursive(conditional.IfFalse, columnName, columnType);
            //    }
            //    else if (expr is MemberExpression member &&
            //             member.Member.Name == "Item" &&
            //             member.Expression is ParameterExpression &&
            //             member.Arguments[0] is ConstantExpression constant &&
            //             constant.Value.ToString().Equals(columnName, StringComparison.OrdinalIgnoreCase))
            //    {
            //        var parser = new Parser(new List<Token>());
            //        parser.columnTypes[columnName] = columnType;
            //    }
            //}


        }

        #region
        private class Parser<T>
        {
            private readonly List<Token> tokens;
            private int currentIndex;
            private readonly ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            private readonly Dictionary<string, PropertyInfo> propertyCache = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

            private Token CurrentToken => currentIndex < tokens.Count ? tokens[currentIndex] : null;
            public Parser(List<Token> tokens)
            {
                this.tokens = tokens;

                // Cache properties for quick access
                foreach (var prop in typeof(T).GetProperties())
                {
                    propertyCache[prop.Name] = prop;
                }
            }

            public Expression<Func<T, bool>> Parse()
            {
                var expr = ParseExpression();
                return Expression.Lambda<Func<T, bool>>(expr, parameter);
            }

            private Expression ParseExpression() => ParseOr();

            private Expression ParseOr()
            {
                var left = ParseAnd();
                while (Match(TokenType.Operator, "OR"))
                {
                    var right = ParseAnd();
                    left = Expression.OrElse(left, right);
                }
                return left;
            }

            private Expression ParseAnd()
            {
                var left = ParseNot();
                while (Match(TokenType.Operator, "AND"))
                {
                    var right = ParseNot();
                    left = Expression.AndAlso(left, right);
                }
                return left;
            }

            private Expression ParseNot()
            {
                if (Match(TokenType.Operator, "NOT"))
                {
                    return Expression.Not(ParsePrimary());
                }
                return ParsePrimary();
            }

            private Expression ParsePrimary()
            {
                if (Match(TokenType.LeftParen))
                {
                    var expr = ParseExpression();
                    Expect(TokenType.RightParen, ")");
                    return expr;
                }
                return ParseComparison();
            }

            private Expression ParseComparison()
            {
                var left = ParseOperand();

                if (Match(TokenType.Operator, "="))
                    return CreateComparison(left, ParseOperand(), Expression.Equal);
                if (Match(TokenType.Operator, "!=") || Match(TokenType.Operator, "<>"))
                    return CreateComparison(left, ParseOperand(), Expression.NotEqual);
                if (Match(TokenType.Operator, "<"))
                    return CreateComparison(left, ParseOperand(), Expression.LessThan);
                if (Match(TokenType.Operator, ">"))
                    return CreateComparison(left, ParseOperand(), Expression.GreaterThan);
                if (Match(TokenType.Operator, "<="))
                    return CreateComparison(left, ParseOperand(), Expression.LessThanOrEqual);
                if (Match(TokenType.Operator, ">="))
                    return CreateComparison(left, ParseOperand(), Expression.GreaterThanOrEqual);
                if (Match(TokenType.Operator, "LIKE"))
                    return CreateLikeExpression(left, ParseOperand());
                if (Match(TokenType.Operator, "IN"))
                    return CreateInExpression(left);
                if (Match(TokenType.Operator, "IS"))
                    return CreateNullCheck(left);

                throw new FormatException($"Expected operator at position {CurrentToken.Position}");
            }

            private Expression ParseOperand()
            {
                if (Match(TokenType.Identifier, out var identToken))
                    return CreatePropertyAccess(identToken.Value);

                if (Match(TokenType.String, out var strToken))
                    return Expression.Constant(strToken.LiteralValue);

                if (Match(TokenType.Number, out var numToken))
                    return Expression.Constant(numToken.LiteralValue);

                if (Match(TokenType.Date, out var dateToken))
                    return Expression.Constant(dateToken.LiteralValue);

                if (Match(TokenType.Boolean, out var boolToken))
                    return Expression.Constant(boolToken.LiteralValue);

                if (Match(TokenType.Null))
                    return Expression.Constant(null, typeof(object));

                throw new FormatException($"Expected operand at position {CurrentToken.Position}");
            }

            private Expression CreatePropertyAccess(string propertyName)
            {
                if (!propertyCache.TryGetValue(propertyName, out var property))
                    throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");

                return Expression.Property(parameter, property);
            }

            private Expression CreateComparison(
                Expression left,
                Expression right,
                Func<Expression, Expression, Expression> comparer)
            {
                // Convert right operand to left's type if needed
                if (left.Type != right.Type && right is ConstantExpression rightConst)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(rightConst.Value, left.Type);
                        right = Expression.Constant(convertedValue, left.Type);
                    }
                    catch
                    {
                        throw new InvalidOperationException(
                            $"Cannot compare {left.Type.Name} and {right.Type.Name}");
                    }
                }

                return comparer(left, right);
            }

            private Expression CreateLikeExpression(Expression left, Expression right)
            {
                // Ensure we're working with strings
                if (left.Type != typeof(string))
                    throw new FormatException("LIKE operator requires string property");

                // Handle null values safely
                var nullCheck = Expression.Equal(left, Expression.Constant(null, typeof(string)));
                var notNullLeft = left;

                // Get pattern from constant
                if (!(right is ConstantExpression ce) || !(ce.Value is string pattern))
                    throw new FormatException("LIKE operator requires string pattern");

                Expression matchExpression;

                // Optimized handling for common patterns
                if (pattern.StartsWith("%") && pattern.EndsWith("%"))
                {
                    var inner = pattern.Trim('%');
                    matchExpression = Expression.Call(
                        notNullLeft,
                        "Contains",
                        null,
                        Expression.Constant(inner));
                }
                else if (pattern.StartsWith("%"))
                {
                    matchExpression = Expression.Call(
                        notNullLeft,
                        "EndsWith",
                        null,
                        Expression.Constant(pattern.Trim('%')));
                }
                else if (pattern.EndsWith("%"))
                {
                    matchExpression = Expression.Call(
                        notNullLeft,
                        "StartsWith",
                        null,
                        Expression.Constant(pattern.Trim('%')));
                }
                else
                {
                    // Use regex for complex patterns
                    var regex = new Regex("^" + Regex.Escape(pattern)
                        .Replace("%", ".*")
                        .Replace("_", ".") + "$", RegexOptions.IgnoreCase);

                    matchExpression = Expression.Call(
                        typeof(Regex).GetMethod("IsMatch", new[] { typeof(string), typeof(string) }),
                        notNullLeft,
                        Expression.Constant(regex.ToString()));
                }

                // Return final expression: if null then false, else match result
                return Expression.Condition(
                    nullCheck,
                    Expression.Constant(false),
                    matchExpression);
            }

            private Expression CreateInExpression(Expression left)
            {
                Expect(TokenType.LeftParen, "(");

                var values = new List<Expression>();
                do
                {
                    values.Add(ParseOperand());
                } while (Match(TokenType.Comma));

                Expect(TokenType.RightParen, ")");

                // Create constant array
                var arrayType = left.Type.MakeArrayType();
                var array = Expression.Constant(
                    values.Select(v => ((ConstantExpression)v).Value).ToArray(),
                    arrayType);

                // Use Enumerable.Contains
                var containsMethod = typeof(Enumerable).GetMethods()
                    .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                    .MakeGenericMethod(left.Type);

                //// 使用Enumerable.Contains替代多个OR
                //var containsMethod = typeof(Enumerable).GetMethods()
                //    .First(m => m.Name == "Contains")
                //    .MakeGenericMethod(left.Type);

                return Expression.Call(containsMethod, array, left);


                return Expression.Call(containsMethod, array, left);
            }

            private Expression CreateNullCheck(Expression left)
            {
                if (Match(TokenType.Null))
                    return Expression.Equal(left, Expression.Constant(null, left.Type));

                if (Match(TokenType.Operator, "NOT") && Match(TokenType.Null))
                    return Expression.NotEqual(left, Expression.Constant(null, left.Type));

             

                throw new FormatException("Expected NULL or NOT NULL after IS");
            }


            private bool Match(TokenType type)
            {
                if (CurrentToken?.Type == type)
                {
                    currentIndex++;
                    return true;
                }
                return false;
            }

            private bool Match(TokenType type, string value)
            {
                if (CurrentToken?.Type == type &&
                    string.Equals(CurrentToken.Value, value, StringComparison.OrdinalIgnoreCase))
                {
                    currentIndex++;
                    return true;
                }
                return false;
            }

            private bool Match(TokenType type, out Token token)
            {
                token = null;
                if (CurrentToken?.Type != type) return false;
                token = CurrentToken;
                currentIndex++;
                return true;
            }

            private void Expect(TokenType type, string expected)
            {
                if (CurrentToken?.Type != type)
                    throw new FormatException($"Expected '{expected}' at position {CurrentToken?.Position}");
                currentIndex++;
            }


        }
        #endregion






    }
}
