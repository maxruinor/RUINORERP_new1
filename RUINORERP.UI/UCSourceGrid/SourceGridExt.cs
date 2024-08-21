using NPOI.SS.Formula.Functions;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global.Model;
using RUINORERP.UI.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 重构出来的扩展
    /// </summary>
    public static partial class SourceGridExt
    {

        /// <summary>
        /// 指定永远不可见的列 但是这个列不能少，是产品明细主键。决定业务数据用的
        /// </summary>
        /// <typeparam name="T">这个类型，要与这个要控制的列关联对应</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_AutoSizeMode<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, SourceGrid.AutoSizeMode autoSizeMode)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_AutoSizeMode(minfo.Name, autoSizeMode);
                }
            }
        }



        /// <summary>
        /// 指定永远不可见的列 但是这个列不能少，是产品明细主键。决定业务数据用的
        /// </summary>
        /// <typeparam name="T">这个类型，要与这个要控制的列关联对应</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_NeverVisible<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_NeverVisible(minfo.Name, typeof(T));
                }
            }
        }



        public static void SetCol_NeverVisible(this SourceGridDefineColumnItem col, string colName, Type BelongingObjectType)
        {
            if (col.ColName == colName && col.BelongingObjectType == BelongingObjectType)
            {
                col.NeverVisible = true;
            }
        }

        public static void SetCol_NeverVisible(this SourceGridDefineColumnItem col, string colName)
        {
            if (col.ColName == colName)
            {
                col.NeverVisible = true;
            }
        }

        public static void SetCol_AutoSizeMode(this SourceGridDefineColumnItem col, string colName, SourceGrid.AutoSizeMode autoSizeMode)
        {
            if (col.ColName == colName)
            {
                col.ColAutoSizeMode = autoSizeMode;
            }
        }



        public static void SetCol_NeverVisible(this List<SourceGridDefineColumnItem> cols, string colName, Type BelongingObjectType)
        {
            foreach (var item in cols)
            {
                item.SetCol_NeverVisible(colName, BelongingObjectType);
            }
        }

        public static void SetCol_NeverVisible(this List<SourceGridDefineColumnItem> cols, string colName)
        {
            foreach (var item in cols)
            {
                item.SetCol_NeverVisible(colName);
            }
        }

        #region  设置默认隐藏 可以手动显示
        /// <summary>
        ///  设置默认隐藏 可以手动显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_DefaultHide<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                item.SetCol_DefaultHide(minfo.Name);
            }
        }
        public static void SetCol_DefaultHide(this SourceGridDefineColumnItem col, string colName)
        {
            if (col.ColName == colName)
            {
                col.DefaultHide = true;
            }
        }

        public static void SetCol_DefaultHide(this List<SourceGridDefineColumnItem> cols, string colName)
        {
            foreach (var item in cols)
            {
                item.SetCol_DefaultHide(colName);
            }
        }
        #endregion

        /// <summary>
        /// 指定只读的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_ReadOnly<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_ReadOnly(minfo.Name);
                }
            }
        }



        /// <summary>
        /// 指定列的默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_DefaultValue<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, object DefaultValue)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                item.SetCol_DefaultValue(minfo.Name, DefaultValue);
            }
        }




        /// <summary>
        /// 指定关联列的特殊值，支持动态参数设置,给colNameSourceExp设置一个参数，指向目标colNameTargetExp，值NewValue是通过valueParameters来确定的。
        /// 意思是当A列值设置后，B列的值就是A列的值的基础上做一些变化处理</summary>
        /// 其中 formatString 是包含零个或多个由花括号 {} 包围的占位符的字符串，arg0, arg1, ... 是要插入到字符串中的参数<typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameSourceExp">输入时的列</param>
        /// <param name="colNameTargetExp">程序控制要变化的列的值</param>
        /// <param name="NewValue">值内容，可以固定</param>
        /// <param name="valueParameters">可以用这个集合的列取值后点位符替换</param>
        public static void SetCol_RelatedValue<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameSourceExp, Expression<Func<T, object>> colNameTargetExp, string NewValue, params Expression<Func<T, object>>[] valueParameters)
        {
            MemberInfo minfoSource = colNameSourceExp.GetMemberInfo();
            MemberInfo minfoTarget = colNameTargetExp.GetMemberInfo();

            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }

                //Expression<Func<T, object>> colNameTargetExp, string NewValue, params Expression<Func<T, object>>[] valueParameters
                List<TargetValueParameter> list = new List<TargetValueParameter>();
                if (valueParameters != null && valueParameters.Length > 0)
                {
                    foreach (var paraExp in valueParameters)
                    {
                        TargetValueParameter relatedTargetValueParameter = new TargetValueParameter();
                        relatedTargetValueParameter.ParameterColName = paraExp.GetMemberInfo().Name;
                        list.Add(relatedTargetValueParameter);
                    }
                }


                item.SetCol_RelatedValue(minfoSource.Name, minfoTarget.Name, NewValue, list.ToArray());
            }
        }




        /// <summary>
        /// 指定关联列的特殊值，支持动态参数设置,给colNameSourceExp设置一个参数，指向目标colNameTargetExp，值NewValue是通过valueParameters来确定的。
        /// 意思是当A列值设置后，B列的值就是A列的值的基础上做一些变化处理</summary>
        /// 其中 formatString 是包含零个或多个由花括号 {} 包围的占位符的字符串，arg0, arg1, ... 是要插入到字符串中的参数<typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameSourceExp">输入时的列</param>
        /// <param name="colNameTargetExp">程序控制要变化的列的值</param>
        /// <param name="NewValue">值内容，可以固定</param>
        /// <param name="valueParameters">可以用这个集合的列取值后点位符替换，并且可以指向另一列值:其中有两种情况，T1时直接指向明细实体中的一个其它列，T2时指向明细实体中的一个关联列的外键实体的显示列（名称）</param>
        public static void SetCol_RelatedValue<T, S>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameSourceExp, Expression<Func<T, object>> colNameTargetExp, string NewValue,
            params KeyNamePair[] valueParameters)
        {
            MemberInfo minfoSource = colNameSourceExp.GetMemberInfo();
            MemberInfo minfoTarget = colNameTargetExp.GetMemberInfo();

            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }

                //Expression<Func<T, object>> colNameTargetExp, string NewValue, params Expression<Func<T, object>>[] valueParameters
                List<TargetValueParameter> list = new List<TargetValueParameter>();
                if (valueParameters != null && valueParameters.Length > 0)
                {
                    foreach (var paraExp in valueParameters)
                    {
                        TargetValueParameter relatedTargetValueParameter = new TargetValueParameter();
                        relatedTargetValueParameter.ParameterColName = paraExp.Key;
                        //关联列,下面两个参数应该要同时出现有值
                        relatedTargetValueParameter.PointToColName = paraExp.Name;
                        relatedTargetValueParameter.FkTableType = paraExp.FkTableType;

                        list.Add(relatedTargetValueParameter);
                    }
                }


                item.SetCol_RelatedValue(minfoSource.Name, minfoTarget.Name, NewValue, list.ToArray());
            }
        }



        /// <summary>
        /// 指定列的显示格式
        /// 注意要在gd定义前
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_Format<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, CustomFormatType CustomFormat, params string[] FormatText)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                item.SetCol_Format(minfo.Name, CustomFormat, FormatText);
            }
        }

        /// <summary>
        /// 指定只读的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_ReadWrite<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                item.SetCol_ReadWrite(minfo.Name);
            }
        }


        /// <summary>
        /// 排除指定的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_Exclude<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            cols.RemoveWhere(c => c.ColName == minfo.Name);
        }

        public static void SetCol_ReadOnly(this SourceGridDefineColumnItem col, string colName)
        {
            if (col.ColName == colName)
            {
                col.ReadOnly = true;
                if (col.ParentGridDefine != null)
                {
                    foreach (var item in col.ParentGridDefine.grid.Rows)
                    {
                        if (col.ParentGridDefine.grid[item.Index, col.ColIndex].Editor != null)
                        {

                        }

                        if (col.EditorForColumn != null)
                        {
                            col.EditorForColumn.EnableEdit = false;
                        }
                    }
                }

            }
        }

        private static void SetCol_Format(this SourceGridDefineColumnItem col, string colName, CustomFormatType CustomFormat, params string[] FormatText)
        {
            if (col.ColName == colName && FormatText.Length > 0)
            {
                col.CustomFormat = CustomFormat;
                col.FormatText = FormatText[0];
            }
            if (col.ColName == colName && FormatText.Length == 0)
            {
                col.CustomFormat = CustomFormat;
            }
        }

        public static void SetCol_DefaultValue(this SourceGridDefineColumnItem col, string colName, object DefaultValue)
        {
            if (col.ColName == colName)
            {
                col.DefaultValue = DefaultValue;
            }
        }

        /// <summary>
        /// 指定列的关联值
        /// 设置colSourceName上，保存参数：colTargetName，NewValue，新值是由valueParameters中的参数决定的
        /// </summary>
        /// <param name="col"></param>
        /// <param name="colSourceName">key</param>
        /// <param name="colTargetName"></param>
        /// <param name="NewValue"></param>
        public static void SetCol_RelatedValue(this SourceGridDefineColumnItem col, string colSourceName, string colTargetName, string NewValue, params TargetValueParameter[] valueParameters)
        {
            if (col.ColName == colSourceName)
            {

                //List<RelatedColumnParameter> 表示 源列可以影响多个目标列的值。设置一次就添加一个集合
                if (col.RelatedCols == null)
                {
                    col.RelatedCols = new ConcurrentDictionary<string, List<RelatedColumnParameter>>();
                }

                List<RelatedColumnParameter> existingValueList;
                // 检查字典中是否已经包含这个键
                if (!col.RelatedCols.TryGetValue(colSourceName, out existingValueList))
                {
                    List<RelatedColumnParameter> NewKvValueList = new List<RelatedColumnParameter>();
                    NewKvValueList.Add(new RelatedColumnParameter(colTargetName, NewValue, valueParameters));

                    // 键不存在，添加新键值对
                    bool added = col.RelatedCols.TryAdd(colSourceName, NewKvValueList);
                    if (added)
                    {
                        //  Console.WriteLine("新数据添加成功。");
                    }
                }
                else
                {
                    //更新已有的键值对
                    //如果目标列已经存在，直接更新
                    RelatedColumnParameter existingValue = existingValueList.FirstOrDefault(c => c.ColTargetName == colTargetName);
                    if (existingValue != null)
                    {
                        existingValue.ColTargetName = colTargetName;
                        existingValue.NewValue = NewValue;
                        existingValue.ValueParameters = valueParameters;
                    }
                    else
                    {
                        existingValue = new RelatedColumnParameter(colTargetName, NewValue, valueParameters);
                        existingValueList.Add(existingValue);
                    }

                    col.RelatedCols.TryUpdate(colSourceName, existingValueList, existingValueList);
                }
            }
        }

        public static void SetCol_ReadWrite(this SourceGridDefineColumnItem col, string colName)
        {
            if (col.ColName == colName)
            {
                col.ReadOnly = false;
                if (col.ParentGridDefine != null)
                {
                    if (col.ParentGridDefine != null)
                    {
                        foreach (var item in col.ParentGridDefine.grid.Rows)
                        {
                            if (col.EditorForColumn != null)
                            {
                                col.EditorForColumn.EnableEdit = true;
                                if (col.ParentGridDefine.grid[item.Index, col.ColIndex].Editor == null)
                                {
                                    if (col.ParentGridDefine.grid[item.Index, col.ColIndex].Editor == null)
                                    {
                                        col.ParentGridDefine.grid[item.Index, col.ColIndex].Editor = col.EditorForColumn;
                                        col.ParentGridDefine.grid[item.Index, col.ColIndex].Editor.EnableEdit = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 指定统计的列,要在dg定义后使用:即 sgd = new SourceGridDefine(grid1, listCols, true); 这句之后
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp">当前列是否要总计</param>
        /// <param name="isTotal">当前列是否是统计列</param>
        /// <param name="subtotalColsExps">参与小计的列集合_乘法</param>
        public static void SetCol_Summary<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_Summary<T>(minfo.Name);
                }
            }
        }



        //https://www.cnblogs.com/feichexia/archive/2013/05/28/3104832.html
        /// <summary>
        /// 小计表达式设置，目标列自动标识为要统计列
        /// (a) => a.RequirementQty * 1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="FormulaExp"></param>
        /// <param name="ResultColName"></param>
        public static void SetCol_Formula<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            MemberInfo minfo = ResultColName.GetMemberInfo();

            foreach (SourceGridDefineColumnItem item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                if (item.ColName == minfo.Name && !item.ParentGridDefine.SubtotalCalculate.Where(s => s.TagetCol == item && s.OriginalExpression == FormulaExp.Body.ToString()).Any())
                {

                    SubtotalFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
                    expStr.TagetCol = item;
                    expStr.TagetColName = item.ColName;
                    expStr.OriginalExpression = FormulaExp.Body.ToString();
                    item.ParentGridDefine.SubtotalCalculate.Add(expStr);
                    //item.Summary = true;参与计算不一定得要显示小计，如比例之类的列
                }

            }
        }




        //https://www.cnblogs.com/feichexia/archive/2013/05/28/3104832.html
        /// <summary>
        /// 小计表达式设置，目标列自动标识为要统计列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="FormulaExp"></param>
        /// <param name="ResultColName"></param>
        public static void SetCol_Formula<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            MemberInfo minfo = ResultColName.GetMemberInfo();

            foreach (SourceGridDefineColumnItem item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                if (item.ColName == minfo.Name && !item.ParentGridDefine.SubtotalCalculate.Where(s => s.TagetCol == item && s.OriginalExpression == FormulaExp.Body.ToString()).Any())
                {

                    SubtotalFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
                    expStr.TagetCol = item;
                    expStr.OriginalExpression = FormulaExp.Body.ToString();
                    expStr.TagetColName = item.ColName;//以这个结果列，或叫目标标为标准，但是可能多种方法组合得到这个结果。所以可以重复
                    item.ParentGridDefine.SubtotalCalculate.Add(expStr);
                    //item.Summary = true;参与计算不一定得要显示小计，如比例之类的列

                    //如果表达式中有/除数。要自动判断除数是否为0，如果有0则不计算

                    /*
                     //方法一 利用DataTable中的Compute方法 例如：1*2-(4/1)+2*4=6   
            string formulate = string.Format("{0}*{1} - {2}/{3} +{1}*{2}", 1, 2, 4, 1);
            DataTable dt = new DataTable();
            Response.Write(dt.Compute(formulate, "").ToString());
                     */

                }

            }
        }

        //https://www.cnblogs.com/feichexia/archive/2013/05/28/3104832.html
        /// <summary>
        /// 反向计算 有除法的。要注意判断除数是否为0，和被除数是否为0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="ConditionExpression"></param
        /// <param name="FormulaExp"></param>
        /// <param name="ResultColName"></param>
        public static void SetCol_FormulaReverse<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> ConditionExpression, Expression<Func<T, T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            MemberInfo minfo = ResultColName.GetMemberInfo();

            foreach (SourceGridDefineColumnItem item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                if (item.ColName == minfo.Name && !item.ParentGridDefine.SubtotalCalculateReverse.Where(s => s.TagetCol == item && s.OriginalExpression == FormulaExp.Body.ToString()).Any())
                {

                    SubtotalFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
                    expStr.TagetCol = item;
                    expStr.OriginalExpression = FormulaExp.Body.ToString();
                    expStr.TagetColName = item.ColName;//以这个结果列，或叫目标标为标准，但是可能多种方法组合得到这个结果。所以可以重复
                    #region 计算条件
                    CalculationCondition condition = new CalculationCondition();
                    condition.CalculationTargetType = typeof(T);
                    //var RExpression = ConditionExpression.ReduceExtensions();
                    var unary = ConditionExpression.Body as UnaryExpression;
                    string str = unary.Operand.ToString();
                    foreach (var para in ConditionExpression.Parameters)
                    {
                        str = str.Replace(para.Name + ".", "");
                    }
                    Expression exp = unary.Operand;
                    condition.expCondition = exp;
                    expStr.CalcCondition = condition;

                    #endregion
                    item.ParentGridDefine.SubtotalCalculateReverse.Add(expStr);

                }

            }
        }

        /// <summary>
        /// 指定计算公式的列,要在dg定义后使用
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp">当前列是否要总计</param>
        /// <param name="isTotal">当前列是否是统计列</param>
        /// <param name="subtotalColsExps">参与小计的列集合_乘法</param>
        public static void SetCol_Formula<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, T, T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            MemberInfo minfo = ResultColName.GetMemberInfo();

            foreach (SourceGridDefineColumnItem item in cols)
            {
                if (item.ColName == minfo.Name && !item.ParentGridDefine.SubtotalCalculate.Where(s => s.TagetCol == item && s.OriginalExpression == FormulaExp.Body.ToString()).Any())
                {
                    SubtotalFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
                    expStr.TagetCol = item;
                    expStr.TagetColName = item.ColName;
                    expStr.OriginalExpression = FormulaExp.Body.ToString();
                    item.ParentGridDefine.SubtotalCalculate.Add(expStr);
                }
            }

        }


        /// <summary>
        /// 指定计算公式的列,要在dg定义后使用
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp">当前列是否要总计</param>
        /// <param name="isTotal">当前列是否是统计列</param>
        /// <param name="subtotalColsExps">参与小计的列集合_乘法</param>
        public static void SetCol_Formula<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, T, T, object>> RsColNameExp, params Expression<Func<T, object>>[] subtotalColsExps)
        {
            //MemberInfo minfo = colNameExp.GetMemberInfo();
            //foreach (var item in cols)
            //{
            //    item.SetCol_Summary<T>(minfo.Name, isTotal, subtotalColsExps.Length > 0, subtotalColsExps);
            //}
        }





        public static void SetCol_Summary<T>(this SourceGridDefineColumnItem col, string colName)
        {
            if (col.ColName == colName && col.BelongingObjectType.Name == typeof(T).Name)
            {
                col.Summary = true;
            }
        }





    }
}
