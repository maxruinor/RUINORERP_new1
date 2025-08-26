
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using RUINORERP.Business.CommService;
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
        public static void SetCol_AutoSizeMode<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, SourceGrid.AutoSizeMode autoSizeMode)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_AutoSizeMode(minfo.Name, autoSizeMode);
                }
            }
        }

        /// <summary>
        /// 设置列的是否可见,并且实时起作用用于初始化之后
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="griddefine"></param>
        public static void SetCol_Visible<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, SourceGridDefine griddefine, bool Visible = true)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    if (item.ColName == minfo.Name && item.BelongingObjectType == typeof(T))
                    {
                        SourceGridDefine sgdefine = griddefine;
                        item.Visible = Visible;
                        int realIndex = sgdefine.grid.Columns.GetColumnInfo(item.UniqueId).Index;
                        griddefine.grid.Columns[realIndex].Visible = Visible;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 指定永远不可见的列 但是这个列不能少，是产品明细主键。决定业务数据用的
        /// </summary>
        /// <typeparam name="T">这个类型，要与这个要控制的列关联对应</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_NeverVisible<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, bool NeverVisible = true)
            {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_NeverVisible(minfo.Name, typeof(T), NeverVisible);
                }
            }
        }

        public static void SetCol_CanMuliSelect<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, bool CanMuliSelect)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_CanMuliSelect(minfo.Name, typeof(T), CanMuliSelect);
                }
            }
        }

        public static void SetCol_CanMuliSelect(this SGDefineColumnItem col, string colName, Type BelongingObjectType, bool CanMuliSelect)
        {
            if (BelongingObjectType == null)
            {
                return;
            }
            if (col.ColName == colName && col.BelongingObjectType == BelongingObjectType)
            {
                col.CanMuliSelect = CanMuliSelect;
            }
        }

        public static void SetCol_NeverVisible(this SGDefineColumnItem col, string colName, Type BelongingObjectType, bool NeverVisible = true)
        {
            if (col.ColName == colName && col.BelongingObjectType == BelongingObjectType)
            {
                col.NeverVisible = NeverVisible;
                col.DisplayController.Disable = NeverVisible;
                col.DisplayController.Visible = !NeverVisible;
            }
        }

        public static void SetCol_NeverVisible(this SGDefineColumnItem col, string colName)
        {
            if (col.ColName == colName)
            {
                col.NeverVisible = true;
                col.DisplayController.Disable = true;
                col.DisplayController.Visible = false;
            }
        }

        public static void SetCol_AutoSizeMode(this SGDefineColumnItem col, string colName, SourceGrid.AutoSizeMode autoSizeMode)
        {
            if (col.ColName == colName)
            {
                col.ColAutoSizeMode = autoSizeMode;
            }
        }



        public static void SetCol_NeverVisible(this List<SGDefineColumnItem> cols, string colName, Type BelongingObjectType)
        {
            foreach (var item in cols)
            {
                item.SetCol_NeverVisible(colName, BelongingObjectType);
            }
        }

        public static void SetCol_NeverVisible(this List<SGDefineColumnItem> cols, string colName)
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
        public static void SetCol_DefaultHide<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, bool Hide = true)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                item.SetCol_DefaultHide(minfo.Name, Hide);
            }
        }
        public static void SetCol_DefaultHide(this SGDefineColumnItem col, string colName, bool Hide = true)
        {
            if (col.ColName == colName)
            {
                col.DefaultHide = Hide;
            }
        }

        public static void SetCol_DefaultHide(this List<SGDefineColumnItem> cols, string colName)
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
        public static void SetCol_ReadOnly<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, bool readOnly = true)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_ReadOnly(minfo.Name, readOnly);
                }
            }
        }



        /// <summary>
        /// 指定列的默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_DefaultValue<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, object DefaultValue)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null || item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                item.SetCol_DefaultValue(minfo.Name, DefaultValue);
            }
        }




        [Obsolete]
        public static void SetCol_DisplayFormatText(this SGDefineColumnItem col, string colName, object DefaultValue)
        {
            if (col.ColName == colName)
            {
                col.IsDisplayFormatText = true;
            }
        }

        /// <summary>
        /// 指定列的显示值,  比如枚举值,但是有更好的已经实现了的方式。这个暂时没有使用了。
        ///listCols.SetCol_Format<tb_FM_ReceivablePayableDetail>(c => c.SourceBizType, CustomFormatType.EnumOptions, null, typeof(BizType));
        ///displayHelper.GetGridViewDisplayText(typeof(T).Name, columnName, e.Value);
        ///GridViewDisplayHelper displayHelper = new GridViewDisplayHelper();
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        [Obsolete]
        public static void SetCol_DisplayFormatText<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, object DefaultValue)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                item.SetCol_DisplayFormatText(minfo.Name, DefaultValue);
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
        public static void SetCol_RelatedValue<T>(this List<SGDefineColumnItem> cols,
            Expression<Func<T, object>> colNameSourceExp,
            Expression<Func<T, object>> colNameTargetExp, string NewValue, params Expression<Func<T, object>>[] valueParameters)
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
        /// <param name="valueParameters">可以用这个集合的列取值后点位符替换，并且可以指向另一列值:其中有两种情况，T1时直接指向明细实体中的一个其他列，T2时指向明细实体中的一个关联列的外键实体的显示列（名称）</param>
        public static void SetCol_RelatedValue<T, S>(this List<SGDefineColumnItem> cols,
            Expression<Func<T, object>> colNameSourceExp, Expression<Func<T, object>> colNameTargetExp,
            string NewValue, params KeyNamePair[] valueParameters)
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
        public static void SetCol_Format<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, CustomFormatType CustomFormat, string[] FormatText = null, Type TypeForEnumOptions = null)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                item.SetCol_Format(minfo.Name, CustomFormat, FormatText, TypeForEnumOptions);
            }
        }


        /// <summary>
        /// 给外键列设置缓存过滤
        /// </summary>
        /// <typeparam name="T">目标列所属实体类型</typeparam>
        /// <typeparam name="TEntity">被绑定的数据源类型</typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        /// <param name="filter"></param>
        public static void SetCol_DataFilter<T, TEntity>(
            this List<SGDefineColumnItem> cols,
            Expression<Func<T, object>> colNameExp,
            DataFilter<TEntity> filter) where TEntity : class
        {
            var prop = colNameExp.GetMemberInfo().Name;
            foreach (var col in cols.Where(c => c.ColName == prop))
                col.SetDataFilter(filter);
        }





        public static void SetCol_Format<T, TEnum>(
    this List<SGDefineColumnItem> cols,
    Expression<Func<T, object>> colNameExp,
    EnumFilter<TEnum> filter,
    string[] formatText = null)
    where TEnum : Enum
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.ColName == minfo.Name)
                {
                    // 新增扩展方法，内部保存在 SGDefineColumnItem 的一个字段里
                    item.TypeForEnumOptions = typeof(TEnum);
                    item.SetEnumFilter(filter);
                }
            }
        }



        /// <summary>
        /// 指定只的列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="colNameExp"></param>
        public static void SetCol_ReadWrite<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
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
        public static void SetCol_Exclude<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            cols.RemoveWhere(c => c.ColName == minfo.Name);
        }


        /*
        public static void SetCol_Width<T>(this List<SourceGridDefineColumnItem> cols, Expression<Func<T, object>> colNameExp, int width)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType != null && item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_Width(minfo.Name, width);
                }
            }
        }

        /// <summary>
        /// 设置列的宽度
        /// </summary>
        /// <param name="col"></param>
        /// <param name="colName"></param>
        /// <param name="width"></param>
        public static void SetCol_Width(this SourceGridDefineColumnItem col, string colName, int width)
        {
            if (col.ColName == colName)
            {
                col.width = width;
            }
        }
        */

        public static void SetCol_ReadOnly(this SGDefineColumnItem col, string colName, bool readOnly = true)
        {
            if (col.ColName == colName)
            {
                col.ReadOnly = readOnly;
                if (col.ParentGridDefine != null)
                {
                    foreach (var item in col.ParentGridDefine.grid.Rows)
                    {
                        int realIndex = col.ParentGridDefine.grid.Columns.GetColumnInfo(col.UniqueId).Index;
                        if (col.ParentGridDefine.grid[item.Index, realIndex] == null)
                        {
                            continue;
                        }

                        if (col.EditorForColumn != null)
                        {
                            col.EditorForColumn.EnableEdit = !readOnly;
                        }
                    }
                }

            }
        }

        private static void SetCol_Format(this SGDefineColumnItem col, string colName, CustomFormatType CustomFormat, string[] FormatText = null, Type TypeForEnumOptions = null)
        {
            if (col.ColName == colName)
            {
                switch (CustomFormat)
                {
                    case CustomFormatType.DefaultFormat:
                        break;
                    case CustomFormatType.PercentFormat:
                    case CustomFormatType.CurrencyFormat:
                        if (FormatText != null && FormatText.Length > 0)
                        {
                            col.FormatText = FormatText[0].ToString();
                        }
                        break;
                    case CustomFormatType.EnumOptions:
                        if (TypeForEnumOptions != null)
                        {
                            col.TypeForEnumOptions = TypeForEnumOptions;
                        }
                        break;
                    case CustomFormatType.DecimalPrecision:
                        break;
                    case CustomFormatType.Bool:
                        break;
                    case CustomFormatType.Image:
                        break;
                    case CustomFormatType.WebPathImage:
                        break;
                    case CustomFormatType.DateTime:
                        break;
                    default:
                        break;
                }
                col.CustomFormat = CustomFormat;
            }

        }




        public static void SetCol_DefaultValue(this SGDefineColumnItem col, string colName, object DefaultValue)
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
        public static void SetCol_RelatedValue(this SGDefineColumnItem col, string colSourceName, string colTargetName, string NewValue, params TargetValueParameter[] valueParameters)
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

        public static void SetCol_ReadWrite(this SGDefineColumnItem col, string colName)
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
                                int realIndex = col.ParentGridDefine.grid.Columns.GetColumnInfo(col.UniqueId).Index;
                                col.EditorForColumn.EnableEdit = true;
                                if (col.ParentGridDefine.grid[item.Index, realIndex].Editor == null)
                                {
                                    if (col.ParentGridDefine.grid[item.Index, realIndex].Editor == null)
                                    {
                                        col.ParentGridDefine.grid[item.Index, realIndex].Editor = col.EditorForColumn;
                                        col.ParentGridDefine.grid[item.Index, realIndex].Editor.EnableEdit = true;
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
        public static void SetCol_Summary<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> colNameExp)
        {
            MemberInfo minfo = colNameExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name == typeof(T).Name)
                {
                    item.SetCol_Summary<T>(minfo.Name);
                }
            }
        }

        #region 正向算

        //https://www.cnblogs.com/feichexia/archive/2013/05/28/3104832.html
        /// <summary>
        /// 小计表达式设置，目标列自动标识为要统计列
        /// (a) => a.RequirementQty * 1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cols"></param>
        /// <param name="FormulaExp"></param>
        /// <param name="ResultColName"></param>
        public static void SetCol_Formula<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> FormulaExp, Expression<Func<T, object>> ResultColName, Expression<Func<T, object>> ConditionExpression = null)
        {
            CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
            SetCalculateFormula(cols, ConditionExpression, ResultColName, expStr, FormulaExp.Body.ToString());
        }



        //https://www.cnblogs.com/feichexia/archive/2013/05/28/3104832.html
        /// <summary>
        /// 计算表达式正向设置
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols">作用的列集合</param>
        /// <param name="FormulaExp">公式表达式</param>
        /// <param name="ResultColName">目标结果列</param>
        /// <param name="ConditionExpression">条件表达式，如果为真则执行计算</param>
        public static void SetCol_Formula<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, T, object>> FormulaExp, Expression<Func<T, object>> ResultColName, Expression<Func<T, object>> ConditionExpression = null)
        {
            CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
            SetCalculateFormula(cols, ConditionExpression, ResultColName, expStr, FormulaExp.Body.ToString());
        }

        /// <summary>
        /// 计算表达式设置
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols">作用的列集合</param>
        /// <param name="FormulaExp">公式表达式</param>
        /// <param name="ResultColName">目标结果列</param>
        /// <param name="ConditionExpression">条件表达式，如果为真则执行计算</param>
        private static void SetCalculateFormula<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> ConditionExpression, Expression<Func<T, object>> ResultColName, CalculateFormula calculateFormula, string OriginalExpressionStr)
        {
            MemberInfo minfo = ResultColName.GetMemberInfo();
            foreach (SGDefineColumnItem item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                //如果目标列和参数列一致，则不计算
                if (item.ColName == minfo.Name)
                {
                    bool isSame = item.ParentGridDefine.SubtotalCalculateReverse.Where(s => s.TagetCol.ColName == minfo.Name
                    && s.OriginalExpression.ToString() == OriginalExpressionStr
                    ).Any();
                    if (isSame)
                    {
                        continue;
                    }

                    CalculateFormula expStr = calculateFormula;
                    expStr.TagetCol = item;
                    expStr.OriginalExpression = OriginalExpressionStr;
                    expStr.TagetColName = item.ColName;//以这个结果列，或叫目标标为标准，但是可能多种方法组合得到这个结果。所以可以重复
                    #region 计算条件
                    if (ConditionExpression != null)
                    {
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
                    }
                    #endregion
                    item.ParentGridDefine.SubtotalCalculate.Add(expStr);
                }
            }
 
        }
 
        /// <summary>
        /// 指定计算公式的列,要在dg定义后使用
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols">作用的列集合</param>
        /// <param name="FormulaExp">公式表达式</param>
        /// <param name="ResultColName">目标结果列</param>
        /// <param name="ConditionExpression">条件表达式，如果为真则执行计算</param>
        public static void SetCol_Formula<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, T, T, object>> FormulaExp,
            Expression<Func<T, object>> ResultColName, Expression<Func<T, object>> ConditionExpression = null)
        {
            CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
            SetCalculateFormula(cols, ConditionExpression, ResultColName, expStr, FormulaExp.Body.ToString());
            /*
            MemberInfo minfo = ResultColName.GetMemberInfo();
            foreach (SourceGridDefineColumnItem item in cols)
            {
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                //如果目标列和参数列一致，则不计算
                if (item.ColName == minfo.Name)
                {
                    bool isSame = item.ParentGridDefine.SubtotalCalculateReverse.Where(s => s.TagetCol.ColName == minfo.Name && s.OriginalExpression.ToString() == FormulaExp.Body.ToString()).Any();
                    if (isSame)
                    {
                        continue;
                    }

                    CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
                    expStr.TagetCol = item;
                    expStr.TagetColName = item.ColName;
                    expStr.OriginalExpression = FormulaExp.Body.ToString();
                    item.ParentGridDefine.SubtotalCalculate.Add(expStr);
                }
            }
            */
        }


        #endregion





        #region 反算

        /// <summary>
        /// 计算表达式设置
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols">作用的列集合</param>
        /// <param name="FormulaExp">公式表达式</param>
        /// <param name="ResultColName">目标结果列</param>
        /// <param name="ConditionExpression">条件表达式，如果为真则执行计算</param>
        private static void SetCalculateFormulaReverse<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> ResultColName, CalculateFormula calculateFormula, string OriginalExpressionStr, Expression<Func<T, object>> ConditionExpression = null)
        {

            MemberInfo minfo = ResultColName.GetMemberInfo();
            foreach (SGDefineColumnItem item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }
                if (item.BelongingObjectType.Name != typeof(T).Name)
                {
                    continue;
                }
                //如果目标列和参数列一致，则不计算
                if (item.ColName == minfo.Name)
                {
                    bool isSame = item.ParentGridDefine.SubtotalCalculateReverse.Where(s => s.TagetCol.ColName == minfo.Name
                    && s.OriginalExpression.ToString() == OriginalExpressionStr// FormulaExp.Body.ToString()
                    && s.CalcCondition.expCondition.ToString() == ConditionExpression.Body.ToString() //反算时一定有条件
                   ).Any();
                    if (isSame)
                    {
                        continue;
                    }

                    CalculateFormula expStr = calculateFormula;// CalculateParser<T>.ParserString(FormulaExp);
                    expStr.TagetCol = item;
                    expStr.OriginalExpression = OriginalExpressionStr;// FormulaExp.Body.ToString();
                    expStr.TagetColName = item.ColName;//以这个结果列，或叫目标列为标准，但是可能多种方法组合得到这个结果。所以可以重复
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


        public static void SetCol_FormulaReverse<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> ConditionExpression, Expression<Func<T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
            SetCalculateFormulaReverse(cols, ResultColName, expStr, FormulaExp.Body.ToString(), ConditionExpression);
        }

        //https://www.cnblogs.com/feichexia/archive/2013/05/28/3104832.html
        /// <summary>
        /// 反向计算 有除法的。要注意判断除数是否为0，和被除数是否为0
        /// </summary>
        /// <typeparam name="T">操作的所属实体</typeparam>
        /// <param name="cols">作用的列集合</param>
        /// <param name="FormulaExp">公式表达式</param>
        /// <param name="ResultColName">目标结果列</param>
        /// <param name="ConditionExpression">条件表达式，如果为真则执行计算</param>
        public static void SetCol_FormulaReverse<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> ConditionExpression, Expression<Func<T, T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
            SetCalculateFormulaReverse(cols, ResultColName, expStr, FormulaExp.Body.ToString(), ConditionExpression);
        }
        public static void SetCol_FormulaReverse<T>(this List<SGDefineColumnItem> cols, Expression<Func<T, object>> ConditionExpression, Expression<Func<T, T, T, object>> FormulaExp, Expression<Func<T, object>> ResultColName)
        {
            CalculateFormula expStr = CalculateParser<T>.ParserString(FormulaExp);
            SetCalculateFormulaReverse(cols, ResultColName, expStr, FormulaExp.Body.ToString(), ConditionExpression);
        }

        #endregion



        public static void SetCol_Summary<T>(this SGDefineColumnItem col, string colName)
        {
            if (col.BelongingObjectType == null)
            {
                return;
            }
            if (col.ColName == colName && col.BelongingObjectType.Name == typeof(T).Name)
            {
                col.Summary = true;
            }
        }


        /// <summary>
        /// 设置列的编辑器数据源 ,目前框架支持的是默认的产品主要部分，并且是目标列存在于公共产品部分的就会显示查询。这里手动可以指定
        /// </summary>
        public static void SetCol_EditorDataSource<Source, Target>(this List<SGDefineColumnItem> cols,
            Expression<Func<Target, object>> colNameTargetExp, List<SourceToTargetMatchCol> sourceToTargetMatches)
        {
            MemberInfo minfoTarget = colNameTargetExp.GetMemberInfo();
            foreach (var item in cols)
            {
                if (item.BelongingObjectType == null)
                {
                    continue;
                }

                if (item.BelongingObjectType.Name != typeof(Target).Name)
                {
                    continue;
                }
                else
                {
                    item.SetCol_EditorDataSource(minfoTarget.Name, sourceToTargetMatches);
                }
            }
        }


        public static void SetCol_EditorDataSource(this SGDefineColumnItem col, string colTargetName, List<SourceToTargetMatchCol> sourceToTargetMatches)
        {
            if (col.ColName == colTargetName)
            {
                if (col.EditorDataSourceCols == null)
                {
                    col.EditorDataSourceCols = new ConcurrentDictionary<string, List<SourceToTargetMatchCol>>();
                }

                List<SourceToTargetMatchCol> existingTypeList = sourceToTargetMatches;
                // 检查字典中是否已经包含这个键
                if (!col.EditorDataSourceCols.TryGetValue(colTargetName, out existingTypeList))
                {
                    // 键不存在，添加新键值对
                    bool added = col.EditorDataSourceCols.TryAdd(colTargetName, sourceToTargetMatches);
                    if (added)
                    {
                        //  Console.WriteLine("新数据添加成功。");
                    }
                }
                else
                {
                    col.EditorDataSourceCols.TryUpdate(colTargetName, existingTypeList, sourceToTargetMatches);
                }
            }
        }


        public static void SetSourceToTargetMatchCol<Source, Target>(this List<SourceToTargetMatchCol> cols,
        Expression<Func<Source, object>> colNameSourceExp, Expression<Func<Target, object>> colNameTargetExp)
        {
            SourceToTargetMatchCol col = new SourceToTargetMatchCol();
            col = col.GetSourceToTargetMatchCol<Source, Target>(colNameSourceExp, colNameTargetExp);
            if (!cols.Contains(col))
            {
                cols.Add(col);
            }
        }

    }
}
