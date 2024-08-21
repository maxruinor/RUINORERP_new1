using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{

    #region
    public class ExpressionToSql
    {
        public string GetSql<T>(Expression<Func<T, T>> exp)
        {
            return DealExpression(exp.Body);
        }
        public string GetSql<T>(Expression<Func<T, bool>> exp)
        {
            return DealExpression(exp.Body);
        }
        private object Eval(MemberExpression member)
        {
            var cast = Expression.Convert(member, typeof(object));
            object c = Expression.Lambda<Func<object>>(cast).Compile().Invoke();
            return GetValueFormat(c);
        }
        private string DealExpression(Expression exp, bool need = false)
        {
            string name = exp.GetType().Name;
            switch (name)
            {
                case "BinaryExpression":
                case "LogicalBinaryExpression":
                case "MethodBinaryExpression":
                case "SimpleBinaryExpression":
                    {
                        BinaryExpression b_exp = exp as BinaryExpression;
                        if (exp.NodeType == ExpressionType.Add
                            || exp.NodeType == ExpressionType.Subtract
                            //|| exp.NodeType == ExpressionType.Multiply
                            //|| exp.NodeType == ExpressionType.Divide
                            //|| exp.NodeType == ExpressionType.Modulo
                            )
                        {
                            return "(" + DealBinary(b_exp) + ")";
                        }

                        if (!need) return DealBinary(b_exp);
                        BinaryExpression b_left = b_exp.Left as BinaryExpression;
                        BinaryExpression b_right = b_exp.Right as BinaryExpression;
                        if (b_left != null && b_right != null)
                        {
                            return "(" + DealBinary(b_exp) + ")";
                        }
                        return DealBinary(b_exp);
                    }
                case "MemberExpression":
                case "PropertyExpression":
                case "FieldExpression":
                    return DealMember(exp as MemberExpression);
                case "ConstantExpression": return DealConstant(exp as ConstantExpression);
                case "MemberInitExpression":
                    return DealMemberInit(exp as MemberInitExpression);
                case "UnaryExpression": return DealUnary(exp as UnaryExpression);


                case "MethodCallExpressionN":
                    {
                        return DealMethodsCall(exp as MethodCallExpression);
                    }
                case "InstanceMethodCallExpression0":
                    {
                        //// The original expression
                        //Expression<Func<Person1, bool>> expr = (x) => x.Birthday.AddMinutes(1) > DateTime.UtcNow;

                        //// Decompose the original expr.
                        //ParameterExpression param = (ParameterExpression)expr.Parameters[0];
                        //BinaryExpression operation = (BinaryExpression)expr.Body;
                        //var leftExpr = operation.Left;

                        //if (leftExpr is MethodCallExpression)
                        //{
                        //    MethodCallExpression expression = (MethodCallExpression)leftExpr;
                        //    object result = Expression.Lambda(expression, param).Compile().
                        //        DynamicInvoke(new Person1() { Birthday = DateTime.Parse("06-03-2020") });
                        //}
                        //return "";

                        var cast = Expression.Convert(exp, typeof(object));
                        object c = Expression.Lambda<Func<object>>(cast).Compile().Invoke();
                        return GetValueFormat(c);

                    }

                default:
                    Console.WriteLine("error:" + name);

                    return "";
            }

        }
        private string DealFieldAccess(FieldAccessException f_exp)
        {
            var c = f_exp;
            return "";
        }
        private string DealMethodsCall(MethodCallExpression m_exp)
        {
            var k = m_exp;
            var g = k.Arguments[0];
            /// 控制函数所在类名。
            if (k.Method.DeclaringType != typeof(SQLMethods))
            {
                throw new Exception("无法识别函数");
            }
            switch (k.Method.Name)
            {
                case "DB_Length":
                    {
                        var exp = k.Arguments[0];
                        return "LEN(" + DealExpression(exp) + ")";
                    }
                case "DB_In":
                case "DB_NotIn":
                    {
                        var exp1 = k.Arguments[0];
                        var exp2 = k.Arguments[1];
                        string methods = string.Empty;
                        if (k.Method.Name == "In")
                        {
                            methods = " IN ";
                        }
                        else
                        {
                            methods = " NOT IN ";
                        }
                        return DealExpression(exp1) + methods + DealExpression(exp2);
                    }
                case "DB_Like":
                case "DB_NotLike":
                    {
                        var exp1 = k.Arguments[0];
                        var exp2 = k.Arguments[1];
                        string methods = string.Empty;
                        if (k.Method.Name == "DB_Like")
                        {
                            methods = " LIKE ";
                        }
                        else
                        {
                            methods = " NOT LIKE ";
                        }
                        return DealExpression(exp1) + methods + DealExpression(exp2);

                    }
            }
            ///   未知的函数
            throw new Exception("意外的函数");
        }
        private string DealUnary(UnaryExpression u_exp)
        {
            var m = u_exp;
            return DealExpression(u_exp.Operand);

        }
        private string DealMemberInit(MemberInitExpression mi_exp)
        {
            var i = 0;
            string exp_str = string.Empty;
            foreach (var item in mi_exp.Bindings)
            {
                MemberAssignment c = item as MemberAssignment;
                if (i == 0)
                {
                    exp_str += c.Member.Name.ToUpper() + "=" + DealExpression(c.Expression);
                }
                else
                {
                    exp_str += "," + c.Member.Name.ToUpper() + "=" + DealExpression(c.Expression);
                }
                i++;
            }
            return exp_str;

        }
        private string DealBinary(BinaryExpression exp)
        {
            return DealExpression(exp.Left) + NullValueDeal(exp.NodeType, DealExpression(exp.Right, true));// GetOperStr(exp.NodeType) + DealExpression(exp.Right, true);
        }
        private string GetOperStr(ExpressionType e_type)
        {
            switch (e_type)
            {
                case ExpressionType.OrElse: return " OR ";
                case ExpressionType.Or: return "|";
                case ExpressionType.AndAlso: return " AND ";
                case ExpressionType.And: return "&";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.LessThanOrEqual: return "<=";
                case ExpressionType.NotEqual: return "<>";
                case ExpressionType.Add: return "+";
                case ExpressionType.Subtract: return "-";
                case ExpressionType.Multiply: return "*";
                case ExpressionType.Divide: return "/";
                case ExpressionType.Modulo: return "%";
                case ExpressionType.Equal: return "=";
            }
            return "";
        }

        private string DealField(MemberExpression exp)
        {
            return Eval(exp).ToString();
        }
        private string DealMember(MemberExpression exp)
        {
            if (exp.Expression != null)
            {
                if (exp.Expression.GetType().Name == "TypedParameterExpression")
                {
                    return exp.Member.Name;
                }
                return Eval(exp).ToString();

            }


            Type type = exp.Member.ReflectedType;
            PropertyInfo propertyInfo = type.GetProperty(exp.Member.Name, BindingFlags.Static | BindingFlags.Public);
            object o;
            if (propertyInfo != null)
            {
                o = propertyInfo.GetValue(null);
            }
            else
            {
                FieldInfo field = type.GetField(exp.Member.Name, BindingFlags.Static | BindingFlags.Public);
                o = field.GetValue(null);
            }
            return GetValueFormat(o);

        }
        private string DealConstant(ConstantExpression exp)
        {
            var ccc = exp.Value.GetType();

            if (exp.Value == null)
            {
                return "NULL";
            }
            return GetValueFormat(exp.Value);
        }
        private string NullValueDeal(ExpressionType NodeType, string value)
        {
            if (value.ToUpper() != "NULL")
            {
                return GetOperStr(NodeType) + value;
            }

            switch (NodeType)
            {
                case ExpressionType.NotEqual:
                    {
                        return " IS NOT NULL ";
                    }
                case ExpressionType.Equal:
                    {
                        return " IS NULL ";
                    }
                default: return GetOperStr(NodeType) + value;
            }
        }
        private string GetValueFormat(object obj)
        {
            var type = obj.GetType();

            if (type.Name == "List`1") //list集合
            {
                List<string> data = new List<string>();
                var list = obj as IEnumerable;
                string sql = string.Empty;
                foreach (var item in list)
                {
                    data.Add(GetValueFormat(item));
                }
                sql = "(" + string.Join(",", data) + ")";
                return sql;
            }

            if (type == typeof(string))//
            {
                return string.Format("'{0}'", obj.ToString());
            }
            if (type == typeof(DateTime))
            {
                DateTime dt = (DateTime)obj;
                return string.Format("'{0}'", dt.ToString("yyyy-MM-dd HH:mm:ss fff"));
            }
            return obj.ToString();
        }


    }

    public static class SQLMethods
    {
        public static bool DB_In<T>(this T t, List<T> list)  // in
        {
            return true;
        }
        public static Boolean DB_NotIn<T>(this T t, List<T> list) // not in
        {
            return true;
        }
        public static int DB_Length(this string t)  // len();
        {
            return 0;
        }
        public static bool DB_Like(this string t, string str) // like
        {
            return true;
        }
        public static bool DB_NotLike(this string t, string str) // not like
        {
            return true;
        }
    }

    #endregion
    public static class LambdaToSqlHelper
    {
        #region 基础方法

        #region 获取条件语句方法

        private static string GetWhereSql<T>(Expression<Func<T, bool>> func, List<ParMODEL> parModelList) where T : class
        {
            string res = "";
            if (func.Body is BinaryExpression)
            {
                //起始参数

                BinaryExpression be = ((BinaryExpression)func.Body);
                res = BinarExpressionProvider(be.Left, be.Right, be.NodeType, parModelList);
            }
            else if (func.Body is MethodCallExpression)
            {
                MethodCallExpression be = ((MethodCallExpression)func.Body);
                res = ExpressionRouter(func.Body, parModelList);
            }
            else
            {
                res = "  ";
            }

            return res;
        }

        #endregion 获取条件语句方法



        #region 获取排序语句 order by

        private static string GetOrderSql<T>(Expression<Func<T, object>> exp) where T : class
        {
            var res = "";
            if (exp.Body is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp.Body);
                List<ParMODEL> parModelList = new List<ParMODEL>();
                res = "order by `" + ExpressionRouter(ue.Operand, parModelList).ToLower() + "`";
            }
            else
            {
                MemberExpression order = ((MemberExpression)exp.Body);
                res = "order by `" + order.Member.Name.ToLower() + "`";
            }
            return res;
        }

        #endregion 获取排序语句 order by



        #endregion 基础方法

        #region 底层

        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }

        public static bool NotIn<T>(this T obj, T[] array)
        {
            return true;
        }

        public static bool Like(this string str, string likeStr)
        {
            return true;
        }

        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }

        private static string GetValueStringByType(object oj)
        {
            if (oj == null)
            {
                return "null";
            }
            else if (oj is ValueType)
            {
                return oj.ToString();
            }
            else if (oj is string || oj is DateTime || oj is char)
            {
                return string.Format("'{0}'", oj.ToString());
            }
            else
            {
                return string.Format("'{0}'", oj.ToString());
            }
        }

        private static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type, List<ParMODEL> parModelList)
        {
            string sb = "(";
            //先处理左边
            string reLeftStr = ExpressionRouter(left, parModelList);
            sb += reLeftStr;

            sb += ExpressionTypeCast(type);

            //再处理右边
            string tmpStr = ExpressionRouter(right, parModelList);
            if (tmpStr == "null")
            {
                if (sb.EndsWith(" ="))
                {
                    sb = sb.Substring(0, sb.Length - 2) + " is null";
                }
                else if (sb.EndsWith("<>"))
                {
                    sb = sb.Substring(0, sb.Length - 2) + " is not null";
                }
            }
            else
            {
                //添加参数
                sb += tmpStr;
            }

            return sb += ")";
        }

        private static string ExpressionRouter(Expression exp, List<ParMODEL> parModelList)
        {
            string sb = string.Empty;

            if (exp is BinaryExpression)
            {
                BinaryExpression be = ((BinaryExpression)exp);
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType, parModelList);
            }
            else if (exp is MemberExpression)
            {
                MemberExpression me = ((MemberExpression)exp);
                if (!exp.ToString().StartsWith("value"))
                {
                    return me.Member.Name;
                }
                else
                {
                    var result = Expression.Lambda(exp).Compile().DynamicInvoke();
                    if (result == null)
                    {
                        return "null";
                    }
                    else if (result is ValueType)
                    {
                        ParMODEL p = new ParMODEL();
                        p.name = "par" + (parModelList.Count + 1);
                        p.value = result.ToString().ToIntByStr();
                        parModelList.Add(p);
                        //return ce.Value.ToString();
                        return "@par" + parModelList.Count;
                    }
                    else if (result is string || result is DateTime || result is char)
                    {
                        ParMODEL p = new ParMODEL();
                        p.name = "par" + (parModelList.Count + 1);
                        p.value = result.ToString();
                        parModelList.Add(p);
                        //return string.Format("'{0}'", ce.Value.ToString());
                        return "@par" + parModelList.Count;
                    }
                    else if (result is int[])
                    {
                        var rl = result as int[];
                        StringBuilder sbIntStr = new StringBuilder();
                        foreach (var r in rl)
                        {
                            ParMODEL p = new ParMODEL();
                            p.name = "par" + (parModelList.Count + 1);
                            p.value = r.ToString().ToIntByStr();
                            parModelList.Add(p);
                            //return string.Format("'{0}'", ce.Value.ToString());
                            sbIntStr.Append("@par" + parModelList.Count + ",");
                        }
                        return sbIntStr.ToString().Substring(0, sbIntStr.ToString().Length - 1);
                    }
                    else if (result is string[])
                    {
                        var rl = result as string[];
                        StringBuilder sbIntStr = new StringBuilder();
                        foreach (var r in rl)
                        {
                            ParMODEL p = new ParMODEL();
                            p.name = "par" + (parModelList.Count + 1);
                            p.value = r.ToString();
                            parModelList.Add(p);
                            //return string.Format("'{0}'", ce.Value.ToString());
                            sbIntStr.Append("@par" + parModelList.Count + ",");
                        }
                        return sbIntStr.ToString().Substring(0, sbIntStr.ToString().Length - 1);
                    }
                }
            }
            else if (exp is NewArrayExpression)
            {
                NewArrayExpression ae = ((NewArrayExpression)exp);
                StringBuilder tmpstr = new StringBuilder();
                foreach (Expression ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(ex, parModelList));
                    tmpstr.Append(",");
                }
                //添加参数

                return tmpstr.ToString(0, tmpstr.Length - 1);
            }
            else if (exp is MethodCallExpression)
            {
                MethodCallExpression mce = (MethodCallExpression)exp;
                string par = ExpressionRouter(mce.Arguments[0], parModelList);
                if (mce.Method.Name == "Like")
                {
                    //添加参数用
                    return string.Format("({0} like {1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
                else if (mce.Method.Name == "NotLike")
                {
                    //添加参数用
                    return string.Format("({0} Not like {1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
                else if (mce.Method.Name == "In")
                {
                    //添加参数用
                    return string.Format("{0} In ({1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
                else if (mce.Method.Name == "NotIn")
                {
                    //添加参数用
                    return string.Format("{0} Not In ({1})", par, ExpressionRouter(mce.Arguments[1], parModelList));
                }
            }
            else if (exp is ConstantExpression)
            {
                ConstantExpression ce = ((ConstantExpression)exp);
                if (ce.Value == null)
                {
                    return "null";
                }
                else if (ce.Value is ValueType)
                {
                    ParMODEL p = new ParMODEL();
                    p.name = "par" + (parModelList.Count + 1);
                    p.value = ce.Value.ToString().ToIntByStr();
                    parModelList.Add(p);
                    //return ce.Value.ToString();
                    return "@par" + parModelList.Count;
                }
                else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                {
                    ParMODEL p = new ParMODEL();
                    p.name = "par" + (parModelList.Count + 1);
                    p.value = ce.Value.ToString();
                    parModelList.Add(p);
                    //return string.Format("'{0}'", ce.Value.ToString());
                    return "@par" + parModelList.Count;
                }

                //对数值进行参数附加
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);

                return ExpressionRouter(ue.Operand, parModelList);
            }
            return null;
        }

        private static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " AND ";

                case ExpressionType.Equal:
                    return " =";

                case ExpressionType.GreaterThan:
                    return " >";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.NotEqual:
                    return "<>";

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " Or ";

                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";

                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";

                case ExpressionType.Divide:
                    return "/";

                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";

                default:
                    return null;
            }
        }

        #endregion 底层
    }

    public class SqlParMODEL
    {
        public string sql { set; get; }

        private List<ParMODEL> parList { set; get; }
    }

    public class ParMODEL
    {
        public string name { set; get; }

        public object value { set; get; }
    }
}
