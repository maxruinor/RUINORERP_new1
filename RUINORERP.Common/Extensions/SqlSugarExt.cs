using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SqlSugar;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Base;

namespace RUINORERP.Common.Extensions
{
    /// <summary>
    /// TODO 要优化 没有时间
    /// </summary>
    public static class SqlSugarExt
    {

        //https://www.cnblogs.com/HelloZyjS/p/13041713.html
        //https://www.nuomiphp.com/eplan/274474.html
        /// <summary>
        /// 根据条件对象查询数据  这里与具体业务挂钩了
        /// </summary>
        /// <typeparam name="T">表实体对象</typeparam>
        /// <param name="sugarQueryable">sugar查询对象</param>
        /// <param name="whereObj">查询实体</param>
        /// <returns></returns>
        //public static ISugarQueryable<T> Where<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, object whereObj)
        //{
        //    // return Where(sugarQueryable, useLike, new List<Expression<Func<T, object>>>(), whereObj);
        //    // return Where(sugarQueryable, useLike, new List<Expression<Func<T, object>>>(), whereObj);
        //}




        //https://www.cnblogs.com/HelloZyjS/p/13041713.html
        //https://www.nuomiphp.com/eplan/274474.html
        /// <summary>
        /// 根据条件对象查询数据  这里与具体业务挂钩了
        /// 2023-11-21更新，加入了查询条件限制，如果有
        /// </summary>
        /// <typeparam name="T">表实体对象</typeparam>
        /// <param name="sugarQueryable">sugar查询对象</param>
        /// <param name="whereObj">查询实体</param>
        /// <returns></returns>
        public static ISugarQueryable<T> WhereCustom<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, List<Expression<Func<T, object>>> queryConditions, object whereObj)
        {
            //这里的思路是将各种情况的where集合分类处理，最后拼接，如果有条件限制，则集合中过滤掉
            var sugarQueryableWhere = sugarQueryable;
            var whereObjType = whereObj.GetType();
            Dictionary<string, object> whereDic = new Dictionary<string, object>();      //装载where条件
            Dictionary<string, List<int>> inDic = new Dictionary<string, List<int>>();   //装载in条件
            Dictionary<string, object> dicTimeRange = new Dictionary<string, object>();  //装载时间区间类型的条件
            Dictionary<string, object> dicLike = new Dictionary<string, object>();//装载like类型的条件
            var expSb = new StringBuilder();//条件是拼接的所以声明在前面
            #region 先取扩展特性的字段 自定义的标记解析
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in whereObj.GetType().GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            AdvQueryAttribute entityAttr;
            Type colType = typeof(T);
            StringBuilder sb = new StringBuilder();

            foreach (var property in whereObjType.GetProperties())
            {
                foreach (Attribute attr in property.GetCustomAttributes(true))
                {
                    entityAttr = attr as AdvQueryAttribute;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColDesc.Trim().Length > 0)
                        {
                            var curName = property.Name;
                            if (property.PropertyType.Name.Equals("List`1"))  //集合
                            {
                                var curValue = property.GetValue(whereObj, null);
                                inDic.Add(curName, (List<int>)curValue);
                            }
                            else
                            {
                                List<AdvExtQueryAttribute> extlist = tempAdvExtList.Where(w => w.RelatedFields == curName).ToList();
                                if (extlist.Count > 0)
                                {
                                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        colType = Nullable.GetUnderlyingType(property.PropertyType);
                                    }
                                    else
                                    {
                                        colType = property.PropertyType;
                                    }

                                    switch (extlist[0].ProcessType)
                                    {
                                        case RUINORERP.Global.AdvQueryProcessType.defaultSelect:

                                            var curValue = property.GetValue(whereObj, null);
                                            if (curValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curValue.ToString())) continue;
                                            int selectID = 0;
                                            if (int.TryParse(curValue.ToString(), out selectID))
                                            {
                                                if (selectID != -1 && selectID != 0)
                                                {
                                                    whereDic.Add(curName, curValue);
                                                }
                                            }

                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.datetimeRange:
                                            if (whereObj.GetPropertyValue(extlist[0].ColName) != null)
                                            {
                                                // sb.Append(extlist[0].RelatedFields).Append($" >= @{whereObj.GetPropertyValue(extlist[0].ColName)}");
                                                DateTime time1 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[0].ColName).ToString());
                                                var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd HH:mm:ss"));

                                                dicTimeRange.Add(extlist[0].ColName, v_str1);
                                            }
                                            if (whereObj.GetPropertyValue(extlist[1].ColName) != null)
                                            {
                                                DateTime time2 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[1].ColName).ToString());
                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd HH:mm:ss"));
                                                dicTimeRange.Add(extlist[1].ColName, v_str2);
                                            }
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.stringLike:
                                            //扩展属性 直接like
                                            var curlikeValue = whereObj.GetPropertyValue(extlist[0].RelatedFields);
                                            if (curlikeValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curlikeValue.ToString())) continue;
                                            dicLike.Add(extlist[0].RelatedFields, curlikeValue);
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.useYesOrNoToAll:
                                            var UseboolObj = whereObj.GetPropertyValue(extlist[0].ColName);
                                            bool useBool = UseboolObj.ToBool();
                                            if (useBool)
                                            {
                                                var curBoolValue = property.GetValue(whereObj, null);
                                                if (curBoolValue == null) continue;   //排除参数值为null的查询条件
                                                if (string.IsNullOrEmpty(curBoolValue.ToString())) continue;
                                                whereDic.Add(curName, curBoolValue);
                                            }
                                            break;
                                        default:
                                            break;
                                    }

                                    //if (colType.Name.Contains("datetime"))
                                    //{

                                    //}
                                    //if (colType.Name.Contains("bool"))
                                    //{


                                    //}
                                    //if (colType.Name.Contains("string"))
                                    //{

                                    //}
                                }
                                else
                                {
                                    var curValue = property.GetValue(whereObj, null);
                                    if (curValue == null) continue;   //排除参数值为null的查询条件
                                    if (string.IsNullOrEmpty(curValue.ToString())) continue;
                                    whereDic.Add(curName, curValue);
                                }

                            }
                        }

                    }
                }

            }

            #endregion

            #region  LIKE 处理
            StringBuilder sblike = new StringBuilder();
            if (dicLike.Count > 0)
            {
                // 动态拼接

                List<string> para = new List<string>();
                for (int i = 0; i < dicLike.Keys.Count; i++)
                {

                    var keys = dicLike.Keys.ToArray();
                    var values = dicLike.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c.GetMemberInfo().Name == keys[i]).Any())
                    {
                        continue;
                    }
                    //自定义查询拼接
                    para.Add(keys[i]);
                    para.Add("like");
                    para.Add("{string}:%" + values[i].ToString() + "%");
                    para.Add("&&");
                }
                para.RemoveAt(para.Count - 1);
                var whereFunc = ObjectFuncModel.Create("Format", para.ToArray());
                // var lambdaStr = sblike.ToString();
                //lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
                sugarQueryableWhere = sugarQueryableWhere.Where(whereFunc);
                // 构建表达式
                // var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicLike.Values);
                // sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            }
            #endregion

            #region  时间等区间
            StringBuilder sbr = new StringBuilder();
            if (dicTimeRange.Count > 0)
            {
                // 动态拼接
                for (int i = 0; i < dicTimeRange.Keys.Count; i++)
                {
                    var keys = dicTimeRange.Keys.ToArray();
                    var values = dicTimeRange.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c.GetMemberInfo().Name == keys[i]).Any())
                    {
                        continue;
                    }
                    //自定义查询拼接
                    if (keys[i].Contains("_Start"))
                    {
                        // $"{property.Name} = Convert.ToDateTime(\"{value}\") ";
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} >=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }

                    if (keys[i].Contains("_End"))
                    {
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} <=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }
                    //sbr.Append(fieldNames[i]).Append($" == @{i}").Append(" && ");
                }
                var lambdaStr = sbr.ToString();
                if (lambdaStr.Contains("and"))
                {
                    lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
                }
                // 构建表达式
                var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicTimeRange.Values);
                sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            }
            #endregion

            #region  in 类型条件处理
            foreach (var item in inDic)
            {
                var key = item.Key;
                var value = item.Value;
                // 如果有条件，并且字段不存在给定的条件中，则忽略
                if (queryConditions.Count > 0 && !queryConditions.Where(c => c.GetMemberInfo().Name == key).Any())
                {
                    continue;
                }

                //转换in条件表达式树
                var e2 = DynamicExpressionParser.ParseLambda<T, object>(new ParsingConfig(), true, expSb.ToString(), whereObj);

                //https://www.coder.work/article/7717381
                //https://www.cnblogs.com/myzony/p/9143692.html  
                // 构建表达式
                // DynamicExpressionParser.ParseLambda<TEntity, bool>(new ParsingConfig(), false, lambdaStr, parameters.Values.ToArray());


                sugarQueryableWhere = sugarQueryableWhere.In(e2, value);
            }

            #endregion

            #region  大部分的 where处理
            var dbModelType = typeof(T);
            foreach (var property in dbModelType.GetProperties())      //遍历dbModel属性
            {
                foreach (var item in whereDic)
                {
                    var key = item.Key;
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c.GetMemberInfo().Name == key).Any())
                    {
                        continue;
                    }
                    //如果是时间要特殊处理 1000-1-1
                    if (item.Value.GetType().ToString().ToLower().Contains("time"))
                    {
                        if (item.Value.ObjToString().ToDateTime() < System.DateTime.Now.AddYears(-50))
                        {
                            continue;
                        }
                    }


                    var value = item.Value;
                    if (property.Name != key) continue;

                    expSb.Append(SqlSugarHelper.ProcessExp(property, value));          //拼接where条件
                    expSb.Append(" and ");
                }
            }
            expSb.Append(sb.ToString());
            if (expSb.Length != 0)                                     //转换where条件表达式树
            {
                var exp = expSb.ToString().Remove(expSb.Length - 4, 4);

                //https://www.coder.work/article/7717381  看这里
                var e = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, exp, whereObj);
                //ew ParsingConfig(), true, "City = @0", "London"
                sugarQueryableWhere = sugarQueryable.Where(e);
            }

            #endregion
            return sugarQueryableWhere;
        }


        //https://www.cnblogs.com/HelloZyjS/p/13041713.html
        //https://www.nuomiphp.com/eplan/274474.html
        /// <summary>
        /// 根据条件对象查询数据  这里与具体业务挂钩了
        /// 2023-11-21更新，加入了查询条件限制，如果有
        /// 2024-1-24 最新 如果类似枚举有下拉并且有请选择 值为-1，则生成查询条件
        /// 最好是能传入生成的条件。目前没有传过来。暂时统一把-1的情况去掉TODO
        /// </summary>
        /// <typeparam name="T">表实体对象</typeparam>
        /// <param name="sugarQueryable">sugar查询对象</param>
        /// <param name="whereObj">查询实体</param>
        /// <returns></returns>
        public static ISugarQueryable<T> WhereCustom<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, List<string> _queryConditions, object whereObj)
        {
            //如果查询参数对应的实体为null，则认为不需要条件查询
            if (whereObj == null)
            {
                return sugarQueryable;
            }
            List<string> queryConditions = new List<string>();
            foreach (var item in _queryConditions)
            {

                var conValue = whereObj.GetPropertyValue(item);
                if (conValue == null || conValue.ToString() != "-1")
                {
                    queryConditions.Add(item);
                }
            }

            //这里的思路是将各种情况的where集合分类处理，最后拼接，如果有条件限制，则集合中过滤掉
            var sugarQueryableWhere = sugarQueryable;
            var whereObjType = whereObj.GetType();
            Dictionary<string, object> whereDic = new Dictionary<string, object>();      //装载where条件
            Dictionary<string, List<int>> inDic = new Dictionary<string, List<int>>();       //装载in条件
            Dictionary<string, object> dicTimeRange = new Dictionary<string, object>();  //装载时间区间类型的条件
            Dictionary<string, object> dicLike = new Dictionary<string, object>();//装载like类型的条件
            var expSb = new StringBuilder();//条件是拼接的所以声明在前面
            #region 先取扩展特性的字段 自定义的标记解析
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in whereObj.GetType().GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            AdvQueryAttribute entityAttr;
            Type colType = typeof(T);
            StringBuilder sb = new StringBuilder();

            foreach (var property in whereObjType.GetProperties())
            {
                foreach (Attribute attr in property.GetCustomAttributes(true))
                {
                    entityAttr = attr as AdvQueryAttribute;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColDesc.Trim().Length > 0)
                        {
                            var curName = property.Name;
                            if (property.PropertyType.Name.Equals("List`1"))  //集合
                            {
                                var curValue = property.GetValue(whereObj, null);
                                inDic.Add(curName, (List<int>)curValue);
                            }
                            else
                            {
                                List<AdvExtQueryAttribute> extlist = tempAdvExtList.Where(w => w.RelatedFields == curName).ToList();
                                if (extlist.Count > 0)
                                {
                                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        colType = Nullable.GetUnderlyingType(property.PropertyType);
                                    }
                                    else
                                    {
                                        colType = property.PropertyType;
                                    }

                                    switch (extlist[0].ProcessType)
                                    {
                                        case RUINORERP.Global.AdvQueryProcessType.defaultSelect:

                                            var curValue = property.GetValue(whereObj, null);
                                            if (curValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curValue.ToString())) continue;

                                            //重点代码 这之前为int，实际下拉基本是long
                                            long selectID = 0;
                                            if (long.TryParse(curValue.ToString(), out selectID))
                                            {
                                                if (selectID != -1 && selectID != 0)
                                                {
                                                    whereDic.Add(curName, curValue);
                                                }
                                            }

                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.datetimeRange:
                                            if (whereObj.GetPropertyValue(extlist[0].ColName) != null)
                                            {
                                                // sb.Append(extlist[0].RelatedFields).Append($" >= @{whereObj.GetPropertyValue(extlist[0].ColName)}");
                                                DateTime time1 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[0].ColName).ToString());
                                                //var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd HH:mm:ss"));
                                                var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd"));
                                                dicTimeRange.Add(extlist[0].ColName, v_str1);
                                            }
                                            if (whereObj.GetPropertyValue(extlist[1].ColName) != null)
                                            {
                                                DateTime time2 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[1].ColName).ToString());
                                                //                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd HH:mm:ss"));
                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd"));
                                                dicTimeRange.Add(extlist[1].ColName, v_str2 + " 23:59:59");
                                            }
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.stringLike:
                                            //扩展属性 直接like
                                            var curlikeValue = whereObj.GetPropertyValue(extlist[0].RelatedFields);
                                            if (curlikeValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curlikeValue.ToString())) continue;
                                            dicLike.Add(extlist[0].RelatedFields, curlikeValue);
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.useYesOrNoToAll:
                                            var UseboolObj = whereObj.GetPropertyValue(extlist[0].ColName);
                                            bool useBool = UseboolObj.ToBool();
                                            if (useBool)
                                            {
                                                var curBoolValue = property.GetValue(whereObj, null);
                                                if (curBoolValue == null) continue;   //排除参数值为null的查询条件
                                                if (string.IsNullOrEmpty(curBoolValue.ToString())) continue;
                                                whereDic.Add(curName, curBoolValue);
                                            }
                                            break;
                                        default:
                                            break;
                                    }


                                }
                                else
                                {
                                    var curValue = property.GetValue(whereObj, null);
                                    if (curValue == null) continue;   //排除参数值为null的查询条件
                                    if (string.IsNullOrEmpty(curValue.ToString())) continue;
                                    whereDic.Add(curName, curValue);
                                }

                            }
                        }

                    }
                }

            }

            #endregion

            #region  LIKE 处理
            StringBuilder sblike = new StringBuilder();
            if (dicLike.Count > 0)
            {
                // 动态拼接
                List<string> para = new List<string>();
                for (int i = 0; i < dicLike.Keys.Count; i++)
                {

                    var keys = dicLike.Keys.ToArray();
                    var values = dicLike.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c == keys[i]).Any())
                    {
                        continue;
                    }
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
                // var lambdaStr = sblike.ToString();
                //lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
                sugarQueryableWhere = sugarQueryableWhere.Where(whereFunc);
                // 构建表达式
                // var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicLike.Values);
                // sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            }
            #endregion

            #region  时间等区间
            StringBuilder sbr = new StringBuilder();
            if (dicTimeRange.Count > 0)
            {
                // 动态拼接
                for (int i = 0; i < dicTimeRange.Keys.Count; i++)
                {
                    var keys = dicTimeRange.Keys.ToArray();
                    var values = dicTimeRange.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (useLike)
                    {
                        if (queryConditions.Count > 0 && !queryConditions.Where(c => keys[i].Contains(c)).Any())
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (queryConditions.Count > 0 && !queryConditions.Where(c => c == keys[i]).Any())
                        {
                            continue;
                        }
                    }

                    //自定义查询拼接
                    if (keys[i].Contains("_Start"))
                    {
                        // $"{property.Name} = Convert.ToDateTime(\"{value}\") ";
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} >=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }

                    if (keys[i].Contains("_End"))
                    {
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} <=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }
                    //sbr.Append(fieldNames[i]).Append($" == @{i}").Append(" && ");
                }
                var lambdaStr = sbr.ToString();
                if (lambdaStr.Trim().Length > 0 && lambdaStr.Contains("and"))
                {
                    lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
                }
                // 构建表达式
                var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicTimeRange.Values);
                sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            }
            #endregion

            #region  in 类型条件处理
            foreach (var item in inDic)
            {
                var key = item.Key;
                var value = item.Value;
                // 如果有条件，并且字段不存在给定的条件中，则忽略
                if (queryConditions.Count > 0 && !queryConditions.Where(c => c == key).Any())
                {
                    continue;
                }

                //转换in条件表达式树
                var e2 = DynamicExpressionParser.ParseLambda<T, object>(new ParsingConfig(), true, expSb.ToString(), whereObj);

                //https://www.coder.work/article/7717381
                //https://www.cnblogs.com/myzony/p/9143692.html  
                // 构建表达式
                // DynamicExpressionParser.ParseLambda<TEntity, bool>(new ParsingConfig(), false, lambdaStr, parameters.Values.ToArray());


                sugarQueryableWhere = sugarQueryableWhere.In(e2, value);
            }

            #endregion

            #region  大部分的 where处理
            var dbModelType = typeof(T);
            foreach (var property in dbModelType.GetProperties())      //遍历dbModel属性
            {
                foreach (var item in whereDic)
                {
                    var key = item.Key;
                    var value = item.Value;
                    if (property.Name != key) continue;
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c == key).Any())
                    {
                        continue;
                    }
                    if (value.ToString() == "0001-01-01 00:00:00")
                    {
                        continue;
                    }
                    //如果是时间要特殊处理,下拉值也在whereDic中
                    if (item.Value.GetType().ToString().ToLower().Contains("time"))
                    {
                        if (item.Value.ObjToString().ToDateTime() < System.DateTime.Now.AddYears(-50))
                        {
                            continue;
                        }
                        //只到日期
                        value = string.Format("{0}", item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));
                        #region 日期处理  当一个时间条件时 会限制到具体时间，思路是将他变为一个区间

                        //DateTime startDate = Convert.ToDateTime(item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));
                        //DateTime endDate = Convert.ToDateTime(item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));

                        DateTime startDate = Convert.ToDateTime(value);
                        DateTime endDate = Convert.ToDateTime(value);

                        var formattedEndDate = endDate.Date.AddDays(1);
                        expSb.Append($"{property.Name} >=\"{startDate}\" ").Append(" and ");
                        expSb.Append($"{property.Name} <=\"{formattedEndDate}\" ").Append(" and ");
                        #endregion
                    }
                    else
                    {
                        expSb.Append(SqlSugarHelper.ProcessExp(property, value));          //拼接where条件
                        expSb.Append(" and ");
                    }
                }
            }
            expSb.Append(sb.ToString());
            if (expSb.Length != 0)                                     //转换where条件表达式树
            {
                var exp = expSb.ToString().Remove(expSb.Length - 4, 4);
                System.Diagnostics.Debug.WriteLine(exp);
                //https://www.coder.work/article/7717381  看这里
                var e = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, exp, whereObj);
                //ew ParsingConfig(), true, "City = @0", "London"
                sugarQueryableWhere = sugarQueryable.Where(e);
            }

            #endregion
            return sugarQueryableWhere;
        }



        public static ISugarQueryable<T> Where优化<T>(this ISugarQueryable<T> sugarQueryable, bool useLike, List<string> queryConditions, object whereObj)
        {
            //这里的思路是将各种情况的where集合分类处理，最后拼接，如果有条件限制，则集合中过滤掉
            var sugarQueryableWhere = sugarQueryable;
            var whereObjType = whereObj.GetType();
            Dictionary<string, object> whereDic = new Dictionary<string, object>();      //装载where条件
            Dictionary<string, List<int>> inDic = new Dictionary<string, List<int>>();       //装载in条件
            Dictionary<string, object> dicTimeRange = new Dictionary<string, object>();  //装载时间区间类型的条件
            Dictionary<string, object> dicLike = new Dictionary<string, object>();//装载like类型的条件
            var expSb = new StringBuilder();//条件是拼接的所以声明在前面
            #region 先取扩展特性的字段 自定义的标记解析
            List<AdvExtQueryAttribute> tempAdvExtList = new List<AdvExtQueryAttribute>();
            foreach (PropertyInfo field in whereObj.GetType().GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    if (attr is AdvExtQueryAttribute)
                    {
                        var advLikeAttr = attr as AdvExtQueryAttribute;
                        tempAdvExtList.Add(advLikeAttr);
                    }
                }
            }

            AdvQueryAttribute entityAttr;
            Type colType = typeof(T);
            StringBuilder sb = new StringBuilder();

            foreach (var property in whereObjType.GetProperties())
            {
                foreach (Attribute attr in property.GetCustomAttributes(true))
                {
                    entityAttr = attr as AdvQueryAttribute;
                    if (null != entityAttr)
                    {
                        if (entityAttr.ColDesc.Trim().Length > 0)
                        {
                            var curName = property.Name;
                            if (property.PropertyType.Name.Equals("List`1"))  //集合
                            {
                                var curValue = property.GetValue(whereObj, null);
                                inDic.Add(curName, (List<int>)curValue);
                            }
                            else
                            {
                                List<AdvExtQueryAttribute> extlist = tempAdvExtList.Where(w => w.RelatedFields == curName).ToList();
                                if (extlist.Count > 0)
                                {
                                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        colType = Nullable.GetUnderlyingType(property.PropertyType);
                                    }
                                    else
                                    {
                                        colType = property.PropertyType;
                                    }

                                    switch (extlist[0].ProcessType)
                                    {
                                        case RUINORERP.Global.AdvQueryProcessType.defaultSelect:

                                            var curValue = property.GetValue(whereObj, null);
                                            if (curValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curValue.ToString())) continue;

                                            //重点代码 这之前为int，实际下拉基本是long
                                            long selectID = 0;
                                            if (long.TryParse(curValue.ToString(), out selectID))
                                            {
                                                if (selectID != -1 && selectID != 0)
                                                {
                                                    whereDic.Add(curName, curValue);
                                                }
                                            }

                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.datetimeRange:
                                            if (whereObj.GetPropertyValue(extlist[0].ColName) != null)
                                            {
                                                // sb.Append(extlist[0].RelatedFields).Append($" >= @{whereObj.GetPropertyValue(extlist[0].ColName)}");
                                                DateTime time1 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[0].ColName).ToString());
                                                //var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd HH:mm:ss"));
                                                var v_str1 = string.Format("{0}", time1.ToString("yyyy-MM-dd"));
                                                dicTimeRange.Add(extlist[0].ColName, v_str1);
                                            }
                                            if (whereObj.GetPropertyValue(extlist[1].ColName) != null)
                                            {
                                                DateTime time2 = Convert.ToDateTime(whereObj.GetPropertyValue(extlist[1].ColName).ToString());
                                                //                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd HH:mm:ss"));
                                                var v_str2 = string.Format("{0}", time2.ToString("yyyy-MM-dd"));
                                                dicTimeRange.Add(extlist[1].ColName, v_str2);
                                            }
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.stringLike:
                                            //扩展属性 直接like
                                            var curlikeValue = whereObj.GetPropertyValue(extlist[0].RelatedFields);
                                            if (curlikeValue == null) continue;   //排除参数值为null的查询条件
                                            if (string.IsNullOrEmpty(curlikeValue.ToString())) continue;
                                            dicLike.Add(extlist[0].RelatedFields, curlikeValue);
                                            break;
                                        case RUINORERP.Global.AdvQueryProcessType.useYesOrNoToAll:
                                            var UseboolObj = whereObj.GetPropertyValue(extlist[0].ColName);
                                            bool useBool = UseboolObj.ToBool();
                                            if (useBool)
                                            {
                                                var curBoolValue = property.GetValue(whereObj, null);
                                                if (curBoolValue == null) continue;   //排除参数值为null的查询条件
                                                if (string.IsNullOrEmpty(curBoolValue.ToString())) continue;
                                                whereDic.Add(curName, curBoolValue);
                                            }
                                            break;
                                        default:
                                            break;
                                    }


                                }
                                else
                                {
                                    var curValue = property.GetValue(whereObj, null);
                                    if (curValue == null) continue;   //排除参数值为null的查询条件
                                    if (string.IsNullOrEmpty(curValue.ToString())) continue;
                                    whereDic.Add(curName, curValue);
                                }

                            }
                        }

                    }
                }

            }

            #endregion

            sugarQueryableWhere = sugarQueryableWhere.Where(WhereFunc(dicLike, queryConditions));

            #region  时间等区间
            StringBuilder sbr = new StringBuilder();
            if (dicTimeRange.Count > 0)
            {
                // 动态拼接
                for (int i = 0; i < dicTimeRange.Keys.Count; i++)
                {
                    var keys = dicTimeRange.Keys.ToArray();
                    var values = dicTimeRange.Values.ToArray();
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c == keys[i]).Any())
                    {
                        continue;
                    }
                    //自定义查询拼接
                    if (keys[i].Contains("_Start"))
                    {
                        // $"{property.Name} = Convert.ToDateTime(\"{value}\") ";
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} >=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }

                    if (keys[i].Contains("_End"))
                    {
                        sbr.Append($"{tempAdvExtList.FindLast(f => f.ColName == keys[i]).RelatedFields} <=\"{values[i]}\" ").Append(" and ");
                        continue;
                    }
                    //sbr.Append(fieldNames[i]).Append($" == @{i}").Append(" && ");
                }
                var lambdaStr = sbr.ToString();
                lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
                // 构建表达式
                var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicTimeRange.Values);
                sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            }
            #endregion

            #region  in 类型条件处理
            foreach (var item in inDic)
            {
                var key = item.Key;
                var value = item.Value;
                // 如果有条件，并且字段不存在给定的条件中，则忽略
                if (queryConditions.Count > 0 && !queryConditions.Where(c => c == key).Any())
                {
                    continue;
                }

                //转换in条件表达式树
                var e2 = DynamicExpressionParser.ParseLambda<T, object>(new ParsingConfig(), true, expSb.ToString(), whereObj);

                //https://www.coder.work/article/7717381
                //https://www.cnblogs.com/myzony/p/9143692.html  
                // 构建表达式
                // DynamicExpressionParser.ParseLambda<TEntity, bool>(new ParsingConfig(), false, lambdaStr, parameters.Values.ToArray());


                sugarQueryableWhere = sugarQueryableWhere.In(e2, value);
            }

            #endregion
            string morewhere = MoreWhere<T>(whereDic, queryConditions);
            if (morewhere.Length != 0)
            {
                var e = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, morewhere, whereObj);
                sugarQueryableWhere = sugarQueryable.Where(e);
            }

            return sugarQueryableWhere;

        }




        #region 优化where

        private static ObjectFuncModel WhereFunc(Dictionary<string, object> dicLike, List<string> queryConditions)
        {
            #region  LIKE 处理
            StringBuilder sblike = new StringBuilder();

            // 动态拼接

            List<string> para = new List<string>();
            for (int i = 0; i < dicLike.Keys.Count; i++)
            {

                var keys = dicLike.Keys.ToArray();
                var values = dicLike.Values.ToArray();
                // 如果有条件，并且字段不存在给定的条件中，则忽略
                if (queryConditions.Count > 0 && !queryConditions.Where(c => c == keys[i]).Any())
                {
                    continue;
                }
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
            // var lambdaStr = sblike.ToString();
            //lambdaStr = lambdaStr.Substring(0, lambdaStr.Length - " and ".Length);
            // sugarQueryableWhere = sugarQueryableWhere.Where(whereFunc);
            // 构建表达式
            // var Expression = DynamicExpressionParser.ParseLambda<T, bool>(new ParsingConfig(), true, lambdaStr, dicLike.Values);
            // sugarQueryableWhere = sugarQueryableWhere.Where(Expression);
            return whereFunc;
            #endregion
        }

        private static string MoreWhere<T>(Dictionary<string, object> whereDic, List<string> queryConditions)
        {
            #region  大部分的 where处理
            var expSb = new StringBuilder();//条件是拼接的所以声明在前面

            var dbModelType = typeof(T);
            foreach (var property in dbModelType.GetProperties())      //遍历dbModel属性
            {
                foreach (var item in whereDic)
                {
                    var key = item.Key;
                    var value = item.Value;
                    // 如果有条件，并且字段不存在给定的条件中，则忽略
                    if (queryConditions.Count > 0 && !queryConditions.Where(c => c == key).Any())
                    {
                        continue;
                    }
                    if (value.ToString() == "0001-01-01 00:00:00")
                    {
                        continue;
                    }
                    //如果是时间要特殊处理 1000-1-1
                    if (item.Value.GetType().ToString().ToLower().Contains("time"))
                    {
                        if (item.Value.ObjToString().ToDateTime() < System.DateTime.Now.AddYears(-50))
                        {
                            continue;
                        }
                        //只到日期
                        value = string.Format("{0}", item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));
                    }


                    if (property.Name != key) continue;

                    #region 日期处理  当一个时间条件时 会限制到具体时间，思路是将他变为一个区间

                    //DateTime startDate = Convert.ToDateTime(item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));
                    //DateTime endDate = Convert.ToDateTime(item.Value.ObjToString().ToDateTime().ToString("yyyy-MM-dd"));

                    DateTime startDate = Convert.ToDateTime(value);
                    DateTime endDate = Convert.ToDateTime(value);

                    var formattedEndDate = endDate.Date.AddDays(1);
                    expSb.Append($"{property.Name} >=\"{startDate}\" ").Append(" and ");
                    expSb.Append($"{property.Name} <=\"{formattedEndDate}\" ").Append(" and ");
                    #endregion
                    //expSb.Append(ProcessExp(property, value));          //拼接where条件
                    //expSb.Append(" and ");
                }
            }
            string expstr = string.Empty;
            if (expSb.Length > 0)
            {
                expstr = expSb.ToString().Remove(expSb.Length - 4, 4);
            }
            return expstr;





            #endregion
        }

        #endregion



        // <summary>
        /// 生成查询字典
        /// </summary>
        /// <param name="dto">对象</param>
        /// <param name="excludeFields">过滤器</param>
        /// <returns></returns>
        private static Dictionary<string, object> GenerateParametersDictionary(object dto, Dictionary<string, object> excludeFields = null)
        {
            var ExcludeFields = excludeFields ?? new Dictionary<string, object>();
            var type = dto.GetType();
            var typeInfo = type;
            var properties = typeInfo.GetProperties();
            var parameters = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(dto);

                if (propertyValue == null) continue;
                if (dto == null) continue;
                if (ExcludeFields.ContainsKey(property.Name)) continue;
                //DateTime类型较为特殊
                if (property.PropertyType == typeof(DateTime) && propertyValue.ToString() == "0001/1/1 0:00:00") continue;
                if (parameters.ContainsKey(property.Name)) continue;

                parameters.Add(property.Name, propertyValue);
            }

            return parameters;
        }



    }
}
