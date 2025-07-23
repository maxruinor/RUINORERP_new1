using Castle.Core.Logging;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using RUINORERP.Common.Extensions;
using RUINORERP.Global.CustomAttribute;
using SharpYaml.Tokens;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{
    public static class ISugarQueryableExt
    {

        public static ISugarQueryable<T> WhereCustom<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, object whereObj)
        {
            return QueryConditionHelper<T>.WhereCustom(sugarQueryable, useLike, null, whereObj);
            // return Where(sugarQueryable, useLike, new List<Expression<Func<T, object>>>(), whereObj);
        }

        [Obsolete("Use WhereAdv instead")]
        public static ISugarQueryable<T> WhereAdv_old<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, object whereObj)
        {
            return QueryConditionHelper<T>.WhereCustom(sugarQueryable, useLike, null, whereObj);
        }

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sugarQueryable"></param>
        /// <param name="useLike"></param>
        /// <param name="queryConditions"></param>
        /// <param name="whereObj"></param>
        /// <returns></returns>
        public static ISugarQueryable<T> WhereAdv<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, List<string> queryConditions, object whereObj)
        {
            return QueryConditionHelper<T>.WhereCustom(sugarQueryable, useLike, queryConditions, whereObj);
        }


    }
    public class QueryConditionHelper<T>
    {
        /// <summary>
        /// 根据条件对象查询数据
        /// </summary>
        /// <typeparam name="T">表实体对象</typeparam>
        /// <param name="sugarQueryable">sugar查询对象</param>
        /// <param name="useLike">是否使用Like查询</param>
        /// <param name="queryConditions">查询条件限制</param>
        /// <param name="whereObj">查询实体</param>
        /// <returns></returns>
        public static ISugarQueryable<T> WhereCustom(ISugarQueryable<T> sugarQueryable, bool useLike, List<string> queryConditions, object whereObj)
        {
            if (whereObj == null || queryConditions.Count == 0)
            {
                return sugarQueryable;
            }

            #region 先取扩展特性的字段 自定义的标记解析 这个特性是动态生成的，为了解决各种不种字段的情况
            List<AdvExtQueryAttribute> AdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in whereObj.GetType().GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        AdvExtList.Add(advLikeAttr);
                    }
                }
            }
            #endregion
            Dictionary<string, object> QueryDic = new Dictionary<string, object>();

            // 将AdvExtList按AdvQueryProcessType分组，根据不同类型分组分别处理
            var groupedByProcessType = AdvExtList
                .GroupBy(advAttr => advAttr.ProcessType)
                .ToDictionary(group => group.Key, group => group.ToList());
            var sugarQueryableWhere = sugarQueryable;

            foreach (var group in groupedByProcessType)
            {
                switch (group.Key)
                {
                    case Global.AdvQueryProcessType.datetimeRange:
                        var dicTimeRange = GetTimeRangeDictionary(whereObj, queryConditions, group.Value);
                        if (dicTimeRange.Count > 0)
                        {
                            sugarQueryableWhere = ApplyTimeRangeConditions(sugarQueryableWhere, dicTimeRange);
                        }
                        // 使用LINQ过滤queryConditions列表，移除dic中的key
                        queryConditions = queryConditions.Except(dicTimeRange.Keys).ToList();
                        break;
                    case Global.AdvQueryProcessType.stringLike:
                        //默认是like 如果列的配置中指定了不用like则精确匹配
                        var dicLike = GetLikeDictionary(whereObj, queryConditions, useLike, group.Value);
                        if (dicLike.Count > 0)
                        {
                            sugarQueryableWhere = ApplyLikeConditions(sugarQueryableWhere, dicLike);
                        }
                        queryConditions = queryConditions.Except(dicLike.Keys).ToList();
                        break;
                    case Global.AdvQueryProcessType.useYesOrNoToAll:
                        var YesNoDic = GetYesOrNoDictionary(whereObj, queryConditions, group.Value);
                        foreach (var item in YesNoDic)
                        {
                            sugarQueryableWhere = ApplyEqualCondition(sugarQueryableWhere, item.Key, item.Value);
                        }
                        queryConditions = queryConditions.Except(YesNoDic.Keys).ToList();
                        break;
                    case Global.AdvQueryProcessType.defaultSelect:
                        if (group.Value.Count > 0)
                        {
                            var SelectEqualDic = GetEqualDictionary(whereObj, queryConditions, group.Value);
                            foreach (var item in SelectEqualDic)
                            {
                                sugarQueryableWhere = ApplyEqualCondition(sugarQueryableWhere, item.Key, item.Value);
                            }
                            queryConditions = queryConditions.Except(SelectEqualDic.Keys).ToList();
                        }
                        break;
                    case Global.AdvQueryProcessType.CmbMultiChoice:
                    case Global.AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                        //这个时。有一个前置条件如果有。则要判断前置条件是否满足
                        if (group.Value.Count > 0)
                        {
                            //两个条件都是多选择可选时
                            foreach (var groupvalue in group.Value)
                            {
                                var ColumnName = groupvalue.RelatedFields;
                                AdvExtQueryAttribute CanNotIgnore = AdvExtList.FirstOrDefault(c => c.RelatedFields == ColumnName && c.ProcessType == Global.AdvQueryProcessType.CmbMultiChoiceCanIgnore);
                                if (CanNotIgnore != null)
                                {
                                    bool NotIgnore = whereObj.GetPropertyValue(CanNotIgnore.ColName).ToBool();
                                    if (!NotIgnore)
                                    {
                                        continue;//不处理 处理下一个
                                    }
                                }
                                AdvExtQueryAttribute AdvExtMultiChoiceResults = AdvExtList.FirstOrDefault(c => c.RelatedFields == ColumnName && c.ProcessType == Global.AdvQueryProcessType.CmbMultiChoice);

                                var inDicResult = GetInDictionary(whereObj, queryConditions, AdvExtMultiChoiceResults);
                                //一个ColumnName 只会有一组
                                foreach (var item in inDicResult)
                                {
                                    if (item.Value.Count == 0)
                                    { continue; }
                                    sugarQueryableWhere = ApplyInCondition(sugarQueryableWhere, ColumnName, item.Value);
                                    //条件用过的，就去掉排除,忽略属性判断时不能处理掉。因为有下一级的判断，多选结果
                                    queryConditions = queryConditions.Except(new List<string> { ColumnName }).ToList();
                                }
                            }
                        }
                        break;
                    default:
                        Console.WriteLine($"没有找到对应的处理类型{group.Key.ToString()}。");
                        break;
                }

            }


            //上面处理完特性特殊的情况后，剩下的就是直接通过where条件查询

            foreach (var queryField in queryConditions)
            {
                var value = whereObj.GetPropertyValue(queryField);
                //过滤无效值
                if (value == null) continue;

                //过滤无效值
                if (value.IsNullOrEmpty()) continue;



                sugarQueryableWhere = ApplyEqualCondition(sugarQueryableWhere, queryField, value);
            }





            //var inDic = GetInDictionary(whereObj, queryConditions, AdvExtList);
            //foreach (var item in inDic)
            //{
            //    sugarQueryableWhere = ApplyInCondition(sugarQueryableWhere, item.Key, item.Value);
            //}


            return sugarQueryableWhere;
        }


        private static Dictionary<string, object> GetEqualDictionary(object whereObj, List<string> queryConditions, List<AdvExtQueryAttribute> tempAdvExtList)
        {
            return GetEqualDictionary(whereObj, queryConditions, new Dictionary<string, object>(), tempAdvExtList);
        }
        /// <summary>
        /// 处理相等的情况
        /// </summary>
        /// <param name="whereObj"></param>
        /// <param name="queryConditions"></param>
        /// <param name="dicLike"></param>
        /// <returns></returns>
        private static Dictionary<string, object> GetEqualDictionary(object whereObj, List<string> queryConditions, Dictionary<string, object> dicLike, List<AdvExtQueryAttribute> tempAdvExtList)
        {
            var equalDic = new Dictionary<string, object>();

            foreach (string curName in queryConditions)
            {
                AdvExtQueryAttribute ext = tempAdvExtList.FirstOrDefault(w => w.RelatedFields == curName);
                if (ext != null)
                {
                    var value = whereObj.GetPropertyValue(ext.RelatedFields);
                    //过滤无效值
                    if (value == null) continue;

                    //过滤无效值
                    if (value.IsNullOrEmpty()) continue;

                    // 排除 dicLike 中已经包含的字段,避免重复
                    //注意特殊处理，如果like中有了。则相等集合中就不可以有。不然查不出来
                    if (dicLike.ContainsKey(curName))
                    {
                        continue;
                    }

                    //0001-01-01 0:00:00  /  "0001-01-01 00:00:00" 可能会因为系统设置的格式不一样。判断不正常
                    //主键为0和-1时过滤
                    if (value.GetType() == typeof(long))
                    {
                        if (value.ToString() == "0" || value.ToString() == "-1")
                        {
                            continue;
                        }
                    }
                    //枚举为-1时过滤
                    if (value.GetType() == typeof(int))
                    {
                        if (value.ToString() == "-1")
                        {
                            continue;
                        }
                    }

                    if (value.GetType() == typeof(DateTime))
                    {
                        if (value.ToDateTime() == DateTime.MinValue)
                        {
                            continue;
                        }
                    }

                    equalDic.Add(ext.RelatedFields, value);

                }
            }

            return equalDic;
        }

        private static Dictionary<string, object> GetYesOrNoDictionary(object whereObj, List<string> queryConditions, List<AdvExtQueryAttribute> tempAdvExtList)
        {
            var equalDic = new Dictionary<string, object>();

            for (int i = queryConditions.Count - 1; i >= 0; i--)
            {
                string curName = queryConditions[i];
                AdvExtQueryAttribute ext = tempAdvExtList.FirstOrDefault(w => w.RelatedFields == curName);
                if (ext != null)
                {
                    var UseboolObj = whereObj.GetPropertyValue(ext.ColName);
                    bool useBool = UseboolObj.ToBool();
                    if (useBool)
                    {
                        var curBoolValue = whereObj.GetPropertyValue(ext.RelatedFields);
                        if (curBoolValue == null || curBoolValue.IsNullOrEmpty() || string.IsNullOrEmpty(curBoolValue.ToString()))
                        {
                            continue; // 排除参数值为null或无效的查询条件
                        }
                        equalDic.Add(ext.RelatedFields, curBoolValue);
                    }
                    else
                    {
                        // 不参与查询，从列表中移除条件
                        queryConditions.RemoveAt(i);
                    }
                }
            }

            return equalDic;
        }

        private static Dictionary<string, List<long>> GetInDictionary(object whereObj, List<string> queryConditions, AdvExtQueryAttribute AdvExt)
        {
            var inDic = new Dictionary<string, List<long>>();
            if (queryConditions.Contains(AdvExt.RelatedFields))
            {
                List<long> ids = new List<long>();
                var values = whereObj.GetPropertyValue(AdvExt.ColName);
                if (values == null)
                {
                    return inDic;
                }
                foreach (var item in (List<object>)values)
                {
                    ids.Add(item.ToLong());
                }
                inDic.Add(AdvExt.ColName, ids);
            }
            // var properties = whereObj.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>));

            //foreach (var property in properties)
            //{
            //    var value = property.GetValue(whereObj);
            //    if (queryConditions.Count > 0 && !queryConditions.Contains(property.Name))
            //    {
            //        continue;
            //    }
            //}

            return inDic;
        }

        private static Dictionary<string, List<int>> GetInDictionary(object whereObj, List<string> queryConditions, List<AdvExtQueryAttribute> AdvExtList)
        {
            var inDic = new Dictionary<string, List<int>>();
            var properties = whereObj.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>));

            foreach (var property in properties)
            {
                var value = property.GetValue(whereObj);
                if (queryConditions.Count > 0 && !queryConditions.Contains(property.Name))
                {
                    continue;
                }

                inDic.Add(property.Name, (List<int>)value);
            }

            return inDic;
        }

        private static Dictionary<string, object> GetTimeRangeDictionary(object whereObj, List<string> queryConditions, List<AdvExtQueryAttribute> tempAdvExtList)
        {
            var dicTimeRange = new Dictionary<string, object>();

            foreach (string curName in queryConditions)
            {
                List<AdvExtQueryAttribute> extList = tempAdvExtList.Where(w => w.RelatedFields == curName).ToList();//可能两个时间都有，只能只有一起或止
                foreach (var item in extList)
                {
                    if (!queryConditions.Any(c => c == item.RelatedFields))
                    {
                        continue;
                    }
                    var value = whereObj.GetPropertyValue(item.ColName);//时间起止，如果有值，最少有一个不为空
                    if (value == null || string.IsNullOrEmpty(value.ToString())) continue;

                    //RelatedFields 保存的是真实的字段,
                    //dicTimeRange 里面保存的是 AA_start,时间

                    //时间范围查询 特殊标记是_Start _End
                    if (item.ColName.Contains("_Start"))
                    {
                        dicTimeRange.Add(item.ColName, (value.ToDateTime()).Date);
                    }
                    if (item.ColName.Contains("_End"))
                    {
                        dicTimeRange.Add(item.ColName, (value.ToDateTime()).GetDayTimeEnd());
                    }
                }

            }

            return dicTimeRange;
        }


        private static Dictionary<string, object> GetLikeDictionary(object whereObj, List<string> queryConditions, bool useLike = true, List<AdvExtQueryAttribute> tempAdvExtList = null)
        {
            //思路是只在UI生成查询条件时就根据设置，生成了对应的高级查询条件并且带有特性Like,所以这里取带_like的列名，值是取本身的值
            var dicLike = new Dictionary<string, object>();
            foreach (string curName in queryConditions)
            {
                AdvExtQueryAttribute ext = tempAdvExtList.FirstOrDefault(w => w.RelatedFields == curName);
                if (ext != null)
                {
                    var value = whereObj.GetPropertyValue(ext.RelatedFields);
                    if (value == null || string.IsNullOrEmpty(value.ToString())) continue;
                    if (value.GetType() == typeof(string))
                    {
                        //if (extlist[0].ColName.Contains(attr.RelatedFields + "_Like") && useLike)
                        //{
                        dicLike.Add(ext.RelatedFields, value);
                        //}
                    }
                }
            }
            return dicLike;
        }

        private static ISugarQueryable<T> ApplyLikeConditions(ISugarQueryable<T> sugarQueryable, Dictionary<string, object> dicLike)
        {
            // 动态拼接
            List<string> para = new List<string>();
            for (int i = 0; i < dicLike.Keys.Count; i++)
            {
                var keys = dicLike.Keys.ToArray();
                var values = dicLike.Values.ToArray();
                //自定义查询拼接
                para.Add(keys[i]);
                para.Add("like");
                para.Add("{string}:%" + values[i].ToString() + "%");
                para.Add("&&");
            }
            if (para.Count > 1 && para.Contains("&&"))
            {
                para.RemoveAt(para.Count - 1);
            }
            var whereFunc = ObjectFuncModel.Create("Format", para.ToArray());
            return sugarQueryable.Where(whereFunc);
        }

        private static ISugarQueryable<T> ApplyTimeRangeConditions(ISugarQueryable<T> sugarQueryable, Dictionary<string, object> dicTimeRange)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < dicTimeRange.Keys.Count; i++)
            {
                var keys = dicTimeRange.Keys.ToArray();
                var realFelds = keys[i].Substring(0, keys[i].LastIndexOf('_'));

                var values = dicTimeRange.Values.ToArray();
                //自定义查询拼接
                if (keys[i].Contains("_Start"))
                {

                    sb.Append($"{realFelds} >=\"{values[i]}\" ").Append(" and ");
                    continue;
                }

                if (keys[i].Contains("_End"))
                {
                    sb.Append($"{realFelds} <=\"{values[i]}\" ").Append(" and ");
                    continue;
                }
            }

            var lambdaStr = sb.ToString();
            if (lambdaStr.Trim().Length > 0 && lambdaStr.Contains("and"))
            {
                lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
            }
            // 构建表达式
            var expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicTimeRange.Values);

            //var sb = new StringBuilder();
            //foreach (var item in dicTimeRange)
            //{
            //    sb.AppendFormat("{0} >= @0 and {0} < @1", item.Key, item.Key + "_End").Append(" && ");
            //}

            //var lambdaStr = sb.ToString().TrimEnd(" &&".ToCharArray());
            //var expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicTimeRange.Values.ToArray());
            return sugarQueryable.Where(expression);
        }

        private static ISugarQueryable<T> ApplyInCondition(ISugarQueryable<T> sugarQueryable, string key, List<long> value)
        {
            //手动构造已提交未审核
            var conModel = new List<IConditionalModel>
            {
                new ConditionalModel { FieldName = key, ConditionalType = ConditionalType.In, FieldValue = string.Join(",", value.ToArray()), CSharpTypeName = "int64" } //设置类型 和C#名称一样常用的支持
            };
            return sugarQueryable.Where(conModel);

            /*
            //暂时用的ORM框架的方式来处理
            //转换in条件表达式树
            //var e2 = DynamicExpressionParser.ParseLambda<T, object>(new ParsingConfig(), true, expSb.ToString(), whereObj);
            StringBuilder sb = new StringBuilder();
            sb.Append($" {key} in ( @0 )");

            //sb.Clear();
            //sb.Append("@0.Contains(" + key + ")");

            // var result2 = rangeOfRs.AsQueryable().Where("I not in @0", values).ToArray();
            // return sugarQueryable.Where(sb.ToString(), value.ToArray());
            //转换in条件表达式树
            //return sugarQueryable = sugarQueryable.Where("@0.Contains(" + key + ")", value);
            // sugarQueryable = sugarQueryable.Where(sb.ToString());

            List<string> ids = new List<string>();
            foreach (var item in value)
            {
                string id = item.ToString();
                ids.Add(id);
            }

            var expression1= DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, sb.ToString(), string.Join(",", value.ToArray()));
            var expression2 = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, sb.ToString(), string.Join(",", value.ToArray()));
            return sugarQueryable = sugarQueryable.Where(expression1);
            

            // return sugarQueryable;
            // var expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, key + " in @0", value);
            // return sugarQueryable.Where(expression);
            */
            /*
             var e1 = DynamicExpressionParser.ParseLambda<Customer, bool>(new ParsingConfig(), true, "City = @0", "London");

			var e2 = DynamicExpressionParser.ParseLambda<Customer, bool>(new ParsingConfig(), true, "c => c.CompanyName != \"test\"");

			var customers = context.Customers.ToList().AsQueryable().Where("@0(it) and @1(it)", e1, e2);
			FiddleHelper.WriteTable(customers);
             */
        }

        private static ISugarQueryable<T> ApplyEqualCondition(ISugarQueryable<T> sugarQueryable, string key, object value)
        {
            if (value == null)
            {
                return sugarQueryable;
            }
            //0001-01-01 0:00:00  /  "0001-01-01 00:00:00" 可能会因为系统设置的格式不一样。判断不正常
            //主键为0和-1时过滤
            if (value.GetType() == typeof(long))
            {
                if (value.ToString() == "0" || value.ToString() == "-1")
                {
                    return sugarQueryable;
                }
            }
            //枚举为-1时过滤
            if (value.GetType() == typeof(int))
            {
                if (value.ToString() == "-1")
                {
                    return sugarQueryable;
                }
            }

            if (value.GetType() == typeof(DateTime))
            {
                if (value.ToDateTime() == DateTime.MinValue)
                {
                    return sugarQueryable;
                }
            }

            var expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, key + " = @0", value);
            return sugarQueryable.Where(expression);
        }



    }
}



/*
 大致的用法是这样的：

//NULL类型判断
var query = dbContext.sys_user.Where("userid!=null").OrderBy("id desc");

//整形的查询
var query = dbContext.sys_user.Where("id>0").OrderBy("id desc");

//整形的查询 带参数
var query = dbContext.sys_user.Where("id>@0", 2).OrderBy("id desc");

//字符型查询
var query = dbContext.sys_user.Where("username=\"张三\").OrderBy("id desc");

//like查询
var query = dbContext.sys_user.Where("username.Contains(\"key\")").OrderBy("id desc");

//日期类型
var query = dbContext.sys_user.Where("createtime <= @0", DateTime.Now).OrderBy("id desc");

//组合条件
var query = dbContext.sys_user.Where("id>0 and username=\"张三\"").OrderBy("id desc");
 
 */