using RUINORERP.UI.UCSourceGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{

    public static class CalculateParser<T>
    {

        //https://blog.csdn.net/weixin_30929195/article/details/99366342 有关表达式
        //https://www.cnblogs.com/zhaoshujie/p/15817805.html

        [Obsolete]
        public static SubtotalFormula ParserString_old(Expression<Func<T, T, object>> RsColNameExp)
        {
            SubtotalFormula sf = new SubtotalFormula();
            sf.OperandQty = 2;
            //if (RsColNameExp is UnaryExpression unaryExp)
            //{
            //    Expression op = unaryExp.Operand;
            //}
            //RsColNameExp.Type.Name 
            //var member = RsColNameExp.Body as MemberExpression;

            //var x = Expression.Parameter(typeof(T), "a");
            //var y = Expression.Parameter(typeof(T), "b");
            //var bexp = RsColNameExp as BinaryExpression;

            var unary = RsColNameExp.Body as UnaryExpression;
            string str = unary.Operand.ToString();
            foreach (var item in RsColNameExp.Parameters)
            {
                str = str.Replace(item.Name + ".", "");
            }
            Expression exp = unary.Operand;
            Expression left = (unary.Operand as BinaryExpression).Left;
            Expression right = (unary.Operand as BinaryExpression).Right;
            string parastr1 = GetOperatorData(left, str);
            string parastr2 = GetOperatorData(right, str);
            sf.Parameter.Add(parastr1);
            sf.Parameter.Add(parastr2);
            sf.StringFormula = str;
            return sf;

            //"(a.CheckQty / b.CarryinglQty)"  //这个就是公式了。如果用datatable方法


            /*

        return member != null ? member.Member.Name : (unary != null ? (unary.Operand as MemberExpression).Member.Name : null);



        // 获取类型T上的可读Property
        var readableProps = from prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            where prop.CanRead
                            select prop;

        Expression combination = null;
        foreach (var readableProp in readableProps)
        {
            var thisPropEqual = Expression.Multiply(Expression.Property(x, readableProp),
                                                 Expression.Property(y, readableProp));

            if (combination == null)
            {
                combination = thisPropEqual;
            }
            else
            {
                combination = Expression.AndAlso(combination, thisPropEqual);
            }
        }

        if (combination == null)   // 如果没有需要比较的东西，直接返回false
        {
          //  PropsEqual = (p1, p2) => false;
        }
        else
        {
           // PropsEqual = Expression.Lambda<Func<T, T, bool>>(combination, x, y).Compile();
        }

        return "";*/
        }
        public static SubtotalFormula ParserString(Expression<Func<T, T, T, object>> RsColNameExp)
        {
            SubtotalFormula sf = new SubtotalFormula();
            sf.OperandQty = 3;
            var unary = RsColNameExp.Body as UnaryExpression;
            string str = unary.Operand.ToString();

            foreach (var item in RsColNameExp.Parameters)
            {
                str = str.Replace(item.Name + ".", "");
                // sf.Parameters.Add(item.Name)
                //sf.ParameterExpression.Add(item);
            }
            Expression exp = unary.Operand;
            //Expression left = (unary.Operand as BinaryExpression).Left;
            // Expression right = (unary.Operand as BinaryExpression).Right;
            // string parastr1 = GetOperatorData(left, str);
            // string parastr2 = GetOperatorData(right, str);
            // sf.Parameter.Add(parastr1);
            // sf.Parameter.Add(parastr2);
            sf.StringFormula = str;
            LoopAnalysis(exp, sf);

            return sf;
        }


        public static SubtotalFormula ParserString(Expression<Func<T, T, object>> RsColNameExp)
        {
            SubtotalFormula sf = new SubtotalFormula();
            sf.OperandQty = 2;
            var unary = RsColNameExp.Body as UnaryExpression;
            string str = unary.Operand.ToString();

            foreach (var item in RsColNameExp.Parameters)
            {
                str = str.Replace(item.Name + ".", "");
            }
            sf.StringFormula = str;
            Expression exp = unary.Operand;
            LoopAnalysis(exp, sf);

            return sf;
        }

        public static SubtotalFormula ParserString(Expression<Func<T, object>> RsColNameExp)
        {
            SubtotalFormula sf = new SubtotalFormula();
            sf.OperandQty = 2;
            var unary = RsColNameExp.Body as UnaryExpression;
            string str = unary.Operand.ToString();

            foreach (var item in RsColNameExp.Parameters)
            {
                str = str.Replace(item.Name + ".", "");
            }
            sf.StringFormula = str;
            Expression exp = unary.Operand;
            LoopAnalysis(exp, sf);

            return sf;
        }


        private static string GetOperatorData(Expression exp, string StrFormula)
        {
            string operatordata = string.Empty;
            switch (exp.NodeType)
            {
                case ExpressionType.Add:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    operatordata = (exp as UnaryExpression).Operand.ToString();
                    StrFormula = StrFormula.Replace("Convert", "");
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.Equal:
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    operatordata = (exp as System.Linq.Expressions.MemberExpression).Member.Name;
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.Unbox:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    operatordata = (exp as System.Linq.Expressions.MemberExpression).Member.Name;
                    break;
            }

            return operatordata;
        }


        private static void LoopAnalysis(Expression exp, SubtotalFormula sf)
        {
            switch (exp.NodeType)
            {
               
                case ExpressionType.AndAlso:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    if (exp is ConstantExpression ce)
                    {
                        sf.StringFormula = sf.StringFormula.Replace("Convert(" + ce.Value.ToString() + ")", ce.Value.ToString());
                    }
                    break;
                case ExpressionType.Convert:
                    exp = (exp as UnaryExpression).Operand;
                    if (exp is MemberExpression memberc)
                    {
                        sf.StringFormula = sf.StringFormula.Replace("Convert(" + memberc.Member.Name + ")", memberc.Member.Name);
                    }
                    if (exp is UnaryExpression memberu)
                    {
                        sf.StringFormula = sf.StringFormula.Replace("Convert(" + exp.ToString() + ")", exp.ToString());
                    }
                    LoopAnalysis(exp, sf);
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.Divide:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    Expression left = (exp as BinaryExpression).Left;
                    LoopAnalysis(left, sf);
                    Expression right = (exp as BinaryExpression).Right;
                    LoopAnalysis(right, sf);
                    break;
                case ExpressionType.Equal:
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.MemberAccess:
                    string operatordata = (exp as System.Linq.Expressions.MemberExpression).Member.Name;
                    if (!sf.Parameter.Contains(operatordata))
                    {
                        sf.Parameter.Add(operatordata);
                    }
          
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.Not:
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.Or:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;

                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.Unbox:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.IsFalse:
                    break;
                default:
                    operatordata = (exp as System.Linq.Expressions.MemberExpression).Member.Name;
                    break;
            }
        }

        public static string Parser(Expression<Func<T, T, T, object>> RsColNameExp)
        {
            return "";
        }

        public static string Parser(Expression<Func<T, T, T, T, object>> RsColNameExp)
        {

            return "";
        }

    }


    public static class Operator<T>
    {



        // public static readonly Func<T, T, T> Plus;
        // public static readonly Func<T, T, T> Minus;
        private static readonly Func<T, T, T> add;



        public static T Add(T x, T y)
        {
            return add(x, y);
        }

        // etc

        static Operator()
        {
            // Build the delegates using expression trees, probably
            var x = Expression.Parameter(typeof(T), "x");
            var y = Expression.Parameter(typeof(T), "y");
            var body = Expression.Add(x, y);
            add = Expression.Lambda<Func<T, T, T>>(
                body, x, y).Compile();
        }





    }




}
