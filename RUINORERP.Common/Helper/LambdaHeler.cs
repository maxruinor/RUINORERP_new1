using RUINORERP.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Common.Helper
{

    #region 表达式转SQL

    /// <summary>
    /// 表达式转SQL  需要修正 来源来网络源码
    /// </summary>
    public class ExpressionToSql
    {
        public string GetSql(Type type, LambdaExpression exp)
        {
            return DealExpression(exp.Body);
        }

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
                        //Expression<Func<Person1, bool>> expr = (x) => x.Birthday.AddMinutes(1) > DateTime.Now;

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
                    System.Diagnostics.Debug.WriteLine("error:" + name);

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
            //MessageBox.Show(type.Name);
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

            if (type == typeof(Boolean))//
            {
                if (obj.ToString().ToUpper() == "TRUE")
                {
                    return string.Format("'{0}'", 1);
                }
                else
                {
                    return string.Format("'{0}'", 0);
                }
                //return string.Format("'{0}'", obj.ToString());
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


    /// <summary>
    /// 也是mysql？
    /// </summary>
    public static class LambdaToSqlHelper
    {
        #region 基础方法


        #region 将引用类型传出注入参数转化为所需参数

        private static SqlParameter[] GetParmetersByList(List<ParMODEL> parModelList)
        {
            SqlParameter[] parameter = new SqlParameter[parModelList.Count];
            for (int i = 0; i < parModelList.Count; i++)
            {
                SqlParameter par = new SqlParameter();
                par.ParameterName = "@" + parModelList[i].name;
                par.Value = parModelList[i].value;
                parameter[i] = par;
            }
            return parameter;
        }

        #endregion 将引用类型传出注入参数转化为所需参数



        #region 获取条件语句方法

        /*
         
          //Expression<Func<MyClass, bool>> where = w => w.id == "123456";
            Expression<Func<MyClass, bool>> where = w => w.id.Contains("1");
            List<SqlParaModel> listSqlParaModel = new List<SqlParaModel>();
            var sql = LambdaToSqlHelper.GetWhereSql(where, listSqlParaModel);
         */
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="parModelList"></param>
        /// <returns></returns>
        public static string GetWhereSql<T>(Expression<Func<T, bool>> func, List<ParMODEL> parModelList) where T : class
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

        #region 获取select表名的 sql 语句

        private static string GetSelectTableSqlCount<T>() where T : class
        {
            var typeT = typeof(T);
            return string.Format("select count(*)  from `{0}` where 1=1 ", typeT.Name.ToLower());
        }

        private static string GetSelectTableSql<T>() where T : class
        {
            var typeT = typeof(T);

            var attrNameList = typeT.GetProperties().Select(a => a.Name).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var name in attrNameList)
            {
                sb.Append("`");
                sb.Append(name);
                sb.Append("`");
                sb.Append(",");
            }
            string cos = sb.ToString().Substring(0, sb.Length - 1);
            return string.Format("select {0} from `{1}` where 1=1 ", cos, typeT.Name.ToLower());
        }

        private static string GetSelectTableSqlPage<T>() where T : class
        {
            var typeT = typeof(T);

            var attrNameList = typeT.GetProperties().Select(a => a.Name).ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var name in attrNameList)
            {
                sb.Append("`");
                sb.Append(name);
                sb.Append("`");
                sb.Append(",");
            }
            string cos = sb.ToString().Substring(0, sb.Length - 1);
            return string.Format("select SQL_CALC_FOUND_ROWS {0} from `{1}` where 1=1 ", cos, typeT.Name.ToLower());
        }

        #endregion 获取select表名的 sql 语句

        #region 获取分页的后尾句

        private static string GetTotalPage(int pageIndex, int pageSize)
        {
            return " limit " + pageSize * (pageIndex - 1) + "," + pageSize + "   ;SELECT FOUND_ROWS() as TotalCount";
        }

        #endregion 获取分页的后尾句

        #region 获取添加返回标识列语句

        public static string GetSqlIDEntity()
        {
            return "SELECT @@IDENTITY AS ID";
        }

        #endregion 获取添加返回标识列语句


        #region 获取排序语句 order by

        public static string GetOrderSql<T>(Expression<Func<T, object>> exp) where T : class
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
                        p.value = result.ToString().ToInt();
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
                            p.value = r.ToString().ToInt();
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
                    p.value = ce.Value.ToString().ToInt();
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


    /*
    public class LambdaToSqlHelperNew
    {
        /// <summary>
        /// NodeType枚举
        /// </summary>
        private enum EnumNodeType
        {
            /// <summary>
            /// 二元运算符
            /// </summary>
            [Description("二元运算符")]
            BinaryOperator = 1,

            /// <summary>
            /// 一元运算符
            /// </summary>
            [Description("一元运算符")]
            UndryOperator = 2,

            /// <summary>
            /// 常量表达式
            /// </summary>
            [Description("常量表达式")]
            Constant = 3,

            /// <summary>
            /// 成员（变量）
            /// </summary>
            [Description("成员（变量）")]
            MemberAccess = 4,

            /// <summary>
            /// 函数
            /// </summary>
            [Description("函数")]
            Call = 5,

            /// <summary>
            /// 未知
            /// </summary>
            [Description("未知")]
            Unknown = -99,

            /// <summary>
            /// 不支持
            /// </summary>
            [Description("不支持")]
            NotSupported = -98
        }

        /// <summary>
        /// 判断表达式类型
        /// </summary>
        /// <param name="exp">lambda表达式</param>
        /// <returns></returns>
        private static EnumNodeType CheckExpressionType(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.OrElse:
                case ExpressionType.Equal:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.NotEqual:
                    return EnumNodeType.BinaryOperator;
                case ExpressionType.Constant:
                    return EnumNodeType.Constant;
                case ExpressionType.MemberAccess:
                    return EnumNodeType.MemberAccess;
                case ExpressionType.Call:
                    return EnumNodeType.Call;
                case ExpressionType.Not:
                case ExpressionType.Convert:
                    return EnumNodeType.UndryOperator;
                default:
                    return EnumNodeType.Unknown;
            }
        }

        /// <summary>
        /// 表达式类型转换
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return " and ";
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.NotEqual:
                    return " <> ";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return " or ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return " + ";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return " - ";
                case ExpressionType.Divide:
                    return " / ";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return " * ";
                default:
                    return null;
            }
        }

        private static string BinarExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            BinaryExpression be = exp as BinaryExpression;
            Expression left = be.Left;
            Expression right = be.Right;
            ExpressionType type = be.NodeType;
            string sb = "(";
            //先处理左边
            sb += ExpressionRouter(left, listSqlParaModel);
            sb += ExpressionTypeCast(type);
            //再处理右边
            string sbTmp = ExpressionRouter(right, listSqlParaModel);
            if (sbTmp == "null")
            {
                if (sb.EndsWith(" = "))
                    sb = sb.Substring(0, sb.Length - 2) + " is null";
                else if (sb.EndsWith(" <> "))
                    sb = sb.Substring(0, sb.Length - 2) + " is not null";
            }
            else
                sb += sbTmp;
            return sb += ")";
        }

        private static string ConstantExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            ConstantExpression ce = exp as ConstantExpression;
            if (ce.Value == null)
            {
                return "null";
            }
            else if (ce.Value is ValueType)
            {
                GetSqlParaModel(listSqlParaModel, GetValueType(ce.Value));
                return "@para" + listSqlParaModel.Count;
            }
            else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
            {
                GetSqlParaModel(listSqlParaModel, GetValueType(ce.Value));
                return "@para" + listSqlParaModel.Count;
            }
            return "";
        }

        private static string LambdaExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            LambdaExpression le = exp as LambdaExpression;
            return ExpressionRouter(le.Body, listSqlParaModel);
        }

        private static string MemberExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            if (!exp.ToString().StartsWith("value"))
            {
                MemberExpression me = exp as MemberExpression;
                if (me.Member.Name == "Now")
                {
                    GetSqlParaModel(listSqlParaModel, DateTime.Now);
                    return "@para" + listSqlParaModel.Count;
                }
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
                    GetSqlParaModel(listSqlParaModel, GetValueType(result));
                    return "@para" + listSqlParaModel.Count;
                }
                else if (result is string || result is DateTime || result is char)
                {
                    GetSqlParaModel(listSqlParaModel, GetValueType(result));
                    return "@para" + listSqlParaModel.Count;
                }
                else if (result is int[])
                {
                    var rl = result as int[];
                    StringBuilder sbTmp = new StringBuilder();
                    foreach (var r in rl)
                    {
                        GetSqlParaModel(listSqlParaModel, r.ToString().ToInt());
                        sbTmp.Append("@para" + listSqlParaModel.Count + ",");
                    }
                    return sbTmp.ToString().Substring(0, sbTmp.ToString().Length - 1);
                }
                else if (result is string[])
                {
                    var rl = result as string[];
                    StringBuilder sbTmp = new StringBuilder();
                    foreach (var r in rl)
                    {
                        GetSqlParaModel(listSqlParaModel, r.ToString());
                        sbTmp.Append("@para" + listSqlParaModel.Count + ",");
                    }
                    return sbTmp.ToString().Substring(0, sbTmp.ToString().Length - 1);
                }
            }
            return "";
        }

         
        private static string MethodCallExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            MethodCallExpression mce = exp as MethodCallExpression;
            if (mce.Method.Name == "Contains")
            {
                if (mce.Object == null)
                {
                    return string.Format("{0} in ({1})", ExpressionRouter(mce.Arguments[1], listSqlParaModel), ExpressionRouter(mce.Arguments[0], listSqlParaModel));
                }
                else
                {
                    if (mce.Object.NodeType == ExpressionType.MemberAccess)
                    {
                        //w => w.name.Contains("1")
                        var _name = ExpressionRouter(mce.Object, listSqlParaModel);
                        var _value = ExpressionRouter(mce.Arguments[0], listSqlParaModel);
                        var index = _value.RetainNumber().ToInt32() - 1;
                        listSqlParaModel[index].value = "%{0}%".FormatWith(listSqlParaModel[index].value);
                        return string.Format("{0} like {1}", _name, _value);
                    }
                }
            }
            else if (mce.Method.Name == "OrderBy")
            {
                return string.Format("{0} asc", ExpressionRouter(mce.Arguments[1], listSqlParaModel));
            }
            else if (mce.Method.Name == "OrderByDescending")
            {
                return string.Format("{0} desc", ExpressionRouter(mce.Arguments[1], listSqlParaModel));
            }
            else if (mce.Method.Name == "ThenBy")
            {
                return string.Format("{0},{1} asc", MethodCallExpressionProvider(mce.Arguments[0], listSqlParaModel), ExpressionRouter(mce.Arguments[1], listSqlParaModel));
            }
            else if (mce.Method.Name == "ThenByDescending")
            {
                return string.Format("{0},{1} desc", MethodCallExpressionProvider(mce.Arguments[0], listSqlParaModel), ExpressionRouter(mce.Arguments[1], listSqlParaModel));
            }
            else if (mce.Method.Name == "Like")
            {
                return string.Format("({0} like {1})", ExpressionRouter(mce.Arguments[0], listSqlParaModel), ExpressionRouter(mce.Arguments[1], listSqlParaModel).Replace("'", ""));
            }
            else if (mce.Method.Name == "NotLike")
            {
                return string.Format("({0} not like '%{1}%')", ExpressionRouter(mce.Arguments[0], listSqlParaModel), ExpressionRouter(mce.Arguments[1], listSqlParaModel).Replace("'", ""));
            }
            else if (mce.Method.Name == "In")
            {
                return string.Format("{0} in ({1})", ExpressionRouter(mce.Arguments[0], listSqlParaModel), ExpressionRouter(mce.Arguments[1], listSqlParaModel));
            }
            else if (mce.Method.Name == "NotIn")
            {
                return string.Format("{0} not in ({1})", ExpressionRouter(mce.Arguments[0], listSqlParaModel), ExpressionRouter(mce.Arguments[1], listSqlParaModel));
            }
            return "";
        }
       
        private static string NewArrayExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            NewArrayExpression ae = exp as NewArrayExpression;
            StringBuilder sbTmp = new StringBuilder();
            foreach (Expression ex in ae.Expressions)
            {
                sbTmp.Append(ExpressionRouter(ex, listSqlParaModel));
                sbTmp.Append(",");
            }
            return sbTmp.ToString(0, sbTmp.Length - 1);
        }

        private static string ParameterExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            ParameterExpression pe = exp as ParameterExpression;
            return pe.Type.Name;
        }

        private static string UnaryExpressionProvider(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            UnaryExpression ue = exp as UnaryExpression;
            var result = ExpressionRouter(ue.Operand, listSqlParaModel);
            ExpressionType type = exp.NodeType;
            if (type == ExpressionType.Not)
            {
                if (result.Contains(" in "))
                {
                    result = result.Replace(" in ", " not in ");
                }
                if (result.Contains(" like "))
                {
                    result = result.Replace(" like ", " not like ");
                }
            }
            return result;
        }

        /// <summary>
        /// 路由计算
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="listSqlParaModel"></param>
        /// <returns></returns>
        private static string ExpressionRouter(Expression exp, List<SqlParaModel> listSqlParaModel)
        {
            var nodeType = exp.NodeType;
            if (exp is BinaryExpression)    //表示具有二进制运算符的表达式
            {
                return BinarExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is ConstantExpression) //表示具有常数值的表达式
            {
                return ConstantExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is LambdaExpression)   //介绍 lambda 表达式。 它捕获一个类似于 .NET 方法主体的代码块
            {
                return LambdaExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is MemberExpression)   //表示访问字段或属性
            {
                return MemberExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is MethodCallExpression)   //表示对静态方法或实例方法的调用
            {
                return MethodCallExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is NewArrayExpression) //表示创建一个新数组，并可能初始化该新数组的元素
            {
                return NewArrayExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is ParameterExpression)    //表示一个命名的参数表达式。
            {
                return ParameterExpressionProvider(exp, listSqlParaModel);
            }
            else if (exp is UnaryExpression)    //表示具有一元运算符的表达式
            {
                return UnaryExpressionProvider(exp, listSqlParaModel);
            }
            return null;
        }

        /// <summary>
        /// 值类型转换
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        private static object GetValueType(object _value)
        {
            var _type = _value.GetType().Name;
            switch (_type)
            {
                case "Decimal ": return _value.ToDecimal();
                case "Int32": return _value.ToInt();
                case "DateTime": return _value.ToDateTime();
                case "String": return _value.ToString();
                case "Char": return _value.ToChar();
                case "Boolean": return _value.ToBool();
                default: return _value;
            }
        }

        /// <summary>
        /// sql参数
        /// </summary>
        /// <param name="listSqlParaModel"></param>
        /// <param name="val"></param>
        private static void GetSqlParaModel(List<SqlParaModel> listSqlParaModel, object val)
        {
            SqlParaModel p = new SqlParaModel();
            p.name = "para" + (listSqlParaModel.Count + 1);
            p.value = val;
            listSqlParaModel.Add(p);
        }

        /// <summary>
        /// lambda表达式转换sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="listSqlParaModel"></param>
        /// <returns></returns>
        public static string GetWhereSql<T>(Expression<Func<T, bool>> where, List<SqlParaModel> listSqlParaModel) where T : class
        {
            string result = string.Empty;
            if (where != null)
            {
                Expression exp = where.Body as Expression;
                result = ExpressionRouter(exp, listSqlParaModel);
            }
            if (result != string.Empty)
            {
                result = " where " + result;
            }
            return result;
        }

        /// <summary>
        /// lambda表达式转换sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static string GetOrderBySql<T>(Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy) where T : class
        {
            string result = string.Empty;
            if (orderBy != null && orderBy.Body is MethodCallExpression)
            {
                MethodCallExpression exp = orderBy.Body as MethodCallExpression;
                List<SqlParaModel> listSqlParaModel = new List<SqlParaModel>();
                result = MethodCallExpressionProvider(exp, listSqlParaModel);
            }
            if (result != string.Empty)
            {
                result = " order by " + result;
            }
            return result;
        }

        /// <summary>
        /// lambda表达式转换sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static string GetQueryField<T>(Expression<Func<T, object>> fields)
        {
            StringBuilder sbSelectFields = new StringBuilder();
            if (fields.Body is NewExpression)
            {
                NewExpression ne = fields.Body as NewExpression;
                for (var i = 0; i < ne.Members.Count; i++)
                {
                    sbSelectFields.Append(ne.Members[i].Name + ",");
                }
            }
            else if (fields.Body is ParameterExpression)
            {
                sbSelectFields.Append("*");
            }
            else
            {
                sbSelectFields.Append("*");
            }
            if (sbSelectFields.Length > 1)
            {
                sbSelectFields = sbSelectFields.Remove(sbSelectFields.Length - 1, 1);
            }
            return sbSelectFields.ToString();
        }

        private class SqlParaModel
        {
            public string Name { get; set; }
            
        }
    }
    */
}
