using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krypton.Toolkit;
using RUINORERP.Model;
using RUINORERP.Model.Base;

using RUINORERP.UI.Common;

using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using RUINORERP.Common;
using RUINORERP.Common.Helper;
using System.Linq.Expressions;
using Krypton.Workspace;
using Krypton.Navigator;
using System.Collections.Concurrent;
using System.Reflection;
using System.Diagnostics;
using SqlSugar;
using System.Linq.Dynamic.Core;
using System.Reflection.Emit;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Global;
using Label = System.Windows.Forms.Label;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.Processor;
using WorkflowCore.Primitives;
using RUINOR.WinFormsUI.ChkComboBox;
using Netron.GraphLib;
using System.Web.UI;
using RUINORERP.Business.CommService;
using FastReport.Editor.Dialogs;

using RUINORERP.Business.Cache;
namespace RUINORERP.UI.AdvancedUIModule
{

    //
    public class UIGenerateHelper
    {
        /// <summary>
        /// 预计算每列的最大宽度，确保布局整齐
        /// </summary>
        /// <param name="queryFields">查询字段列表</param>
        /// <param name="columnCount">列数</param>
        /// <param name="panel">容器面板</param>
        /// <returns>每列的宽度字典</returns>
        private static Dictionary<int, int> CalculateColumnWidths(List<QueryField> queryFields, int columnCount, Krypton.Toolkit.KryptonPanel panel)
        {
            // 初始化每列宽度为最小值
            Dictionary<int, int> columnWidths = new Dictionary<int, int>();
            for (int i = 0; i < columnCount; i++)
            {
                columnWidths[i] = 200; // 默认最小宽度
            }

            // 遍历所有查询字段，计算每列所需的最大宽度
            for (int i = 0; i < queryFields.Count; i++)
            {
                QueryField queryField = queryFields[i];
                int columnIndex = i % columnCount;

                // 计算标签宽度
                Graphics graphics = panel.CreateGraphics();
                float textWidth = UITools.CalculateTextWidth(queryField.Caption, SystemFonts.DefaultFont, graphics);
                int labelWidth = (int)textWidth + 10; // 加上一些额外的空间

                // 根据控件类型估算输入控件宽度
                int inputWidth = 150; // 默认宽度
                switch (queryField.AdvQueryFieldType)
                {
                    case AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                        inputWidth = 190;
                        break;
                    case AdvQueryProcessType.datetimeRange:
                        inputWidth = 280; // 日期范围控件较宽
                        break;
                    case AdvQueryProcessType.datetime:
                        inputWidth = 130; // 日期控件
                        break;
                    case AdvQueryProcessType.YesOrNo:
                    case AdvQueryProcessType.useYesOrNoToAll:
                        inputWidth = 50; // 复选框较窄
                        break;
                }

                // 计算总宽度（标签宽度 + 输入控件宽度 + 间距）
                int totalWidth = labelWidth + inputWidth + 15; // 15为标签和控件之间的间距

                // 更新该列的最大宽度
                if (totalWidth > columnWidths[columnIndex])
                {
                    columnWidths[columnIndex] = totalWidth;
                }
            }

            return columnWidths;
        }

        public static BaseEntity CreateQueryUI(Type type, bool useLike, Krypton.Toolkit.KryptonPanel UcPanel, QueryFilter queryFilter, decimal DefineColNum)
        {
            return CreateQueryUI(type, useLike, UcPanel, queryFilter, null);
        }
        /// <summary>
        /// 创建查询界面
        /// </summary>
        /// <param name="useLike"></param>
        /// <param name="UcPanel"></param>
        /// <param name="queryFilter"></param>
        /// <param name="DefineColNum">4</param>
        /// <returns></returns>
        public static BaseEntity CreateQueryUI(Type type, bool useLike, Krypton.Toolkit.KryptonPanel UcPanel, QueryFilter queryFilter, tb_UIMenuPersonalization menuPersonalization)
        {
            //用户设置的查询条件个性化设置
            List<tb_UIQueryCondition> queryConditions = new List<tb_UIQueryCondition>();
            if (menuPersonalization != null && menuPersonalization.tb_UIQueryConditions != null)
            {
                queryConditions = menuPersonalization.tb_UIQueryConditions;
            }

            List<QueryField> queryFields = queryFilter.QueryFields;
            if (queryFilter == null)
            {
                throw new ArgumentNullException(nameof(queryFilter));
            }
            if (queryConditions.Count > 0)
            {
                // 更新queryFields以反映queryConditions的设置
                foreach (var queryField in queryFields)
                {
                    var condition = queryConditions.FirstOrDefault(c => c.FieldName == queryField.FieldName);
                    if (condition != null)
                    {
                        queryField.IsVisible = condition.IsVisble;
                        queryField.DisplayIndex = condition.Sort;
                        queryField.Default1 = condition.Default1;
                        queryField.Default2 = condition.Default2;
                        queryField.EnableDefault1 = condition.EnableDefault1;
                        queryField.EnableDefault2 = condition.EnableDefault2;
                        queryField.DiffDays1 = condition.DiffDays1;
                        queryField.DiffDays2 = condition.DiffDays2;
                        queryField.Focused = condition.Focused;
                        queryField.UseLike = condition.UseLike;
                        if (queryField.AdvQueryFieldType == AdvQueryProcessType.defaultSelect
                            || queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoice
                            || queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoiceCanIgnore
                            )
                        {
                            if (condition.MultiChoice.HasValue && condition.MultiChoice.Value)
                            {
                                //只有下拉等三种情况。才会显示是否多选
                                queryField.AdvQueryFieldType = AdvQueryProcessType.CmbMultiChoiceCanIgnore;

                            }
                            else
                            {
                                //只有下拉等三种情况。才会显示是否多选
                                queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                            }

                        }
                    }
                }
            }
            else
            {
                //如果没有设置则全部显示出来。
                queryFields.ForEach(c => c.IsVisible = true);
            }

            Type newtype = null;
            object newDto = null;
            //1111
            newtype = UIQueryPropertyBuilder.AttributesBuilder_New2024(type, queryFilter);
            newDto = Activator.CreateInstance(newtype);

            BaseEntity Query = newDto as BaseEntity;
            if (queryFilter == null) { return Query; }

            //这里可以优化掉。保留的意义在于更新更多的相关信息
            List<BaseDtoField> baseDtoFields = UIHelper.GetDtoFieldNameList(newtype);
            //为了显示条件的顺序再一次修改，这样循环是利用这个查询参数的数组默认顺序
            //如果实体没有初始信息，则以传入的queryFilter.QueryFields 字段为准
            if (baseDtoFields.Count == 0)
            {
                //baseDtoFields.AddRange(queryFilter.QueryFields.Select(q => new BaseDtoField
            }
            //这里更新一些信息，再对类型处理一下。很重要。
            foreach (BaseDtoField item in baseDtoFields)
            {
                QueryField queryField = queryFilter.QueryFields.FirstOrDefault(q => q.FieldName == item.SugarCol.ColumnName);
                if (queryField == null)
                {
                    continue;
                }
                queryField.SugarCol = item.SugarCol;
                queryField.ExtendedAttribute = item.ExtendedAttribute;
                queryField.FieldName = queryField.FieldName;
                queryField.Caption = item.Caption;
                queryField.ColDataType = item.ColDataType.GetBaseType();
                queryField.IsRelated = item.IsFKRelationAttribute;

                //不能覆盖设置的值
                //queryField.UseLike = item.UseLike;
                if (queryField.IsRelated)
                {
                    queryField.fKRelationAttribute = item.fKRelationAttribute;
                    //像产品详情ID。绑定的UI时是用视图指定了View_pridel。这里不能再覆盖成tb_prodDetail
                    if (string.IsNullOrEmpty(queryField.FKTableName))
                    {
                        queryField.FKTableName = item.FKTableName;
                    }

                    //如果上级指定了就不要覆盖
                    if (queryField.SubQueryTargetType == null)
                    {
                        var subQueryType = AssemblyLoader.GetType("RUINORERP.Model", "RUINORERP.Model." + queryField.FKTableName);
                        if (subQueryType != null)
                        {
                            queryField.SubQueryTargetType = subQueryType;
                        }
                    }

                }

                #region
                if (queryField.AdvQueryFieldType == AdvQueryProcessType.None)
                {
                    #region  默认处理
                    EnumDataType edt = (EnumDataType)Enum.Parse(typeof(EnumDataType), queryField.ColDataType.Name);
                    switch (edt)
                    {
                        case EnumDataType.Boolean:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.useYesOrNoToAll;
                            break;
                        case EnumDataType.DateTime:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.datetimeRange;
                            break;
                        case EnumDataType.Int16:
                        case EnumDataType.UInt16:
                        case EnumDataType.Int32:
                        case EnumDataType.UInt32:
                        case EnumDataType.Int64:
                            queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                            break;
                        case EnumDataType.UInt64:

                            break;
                        case EnumDataType.IntPtr:
                        case EnumDataType.Char:
                        case EnumDataType.Single:
                        case EnumDataType.Double:
                        case EnumDataType.Decimal:
                        case EnumDataType.SByte:
                        case EnumDataType.Byte:
                        case EnumDataType.UIntPtr:
                        case EnumDataType.Object:
                            break;
                        case EnumDataType.String:
                            if (queryField.UseLike.Value)
                            {
                                queryField.AdvQueryFieldType = AdvQueryProcessType.stringLike;
                            }
                            else
                            {
                                queryField.AdvQueryFieldType = AdvQueryProcessType.stringEquals;
                            }

                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                #endregion
            }

            #region 定义表格布局的行和列
            //把时间排后面
            queryFields = queryFields.OrderBy(d => d.ExtendedAttribute.Count).ToList();
            if (queryConditions.Count > 0)
            {
                queryFields = queryFields.OrderBy(d => d.DisplayIndex).ToList();
            }
            else
            {
                //如果没有特殊设置系统将日期放到最后。因为最长 不好占位显示
                queryFields = queryFields.OrderBy(d => d.AdvQueryFieldType).ToList();
            }
            queryFields = queryFields.Where(c => c.IsVisible).ToList();//去掉隐藏的

            //定义每列放几组控件
            int RowOfColNum = 4;
            if (menuPersonalization != null)
            {
                RowOfColNum = menuPersonalization.QueryConditionCols.ToInt();
            }

            // 预计算每列的最大宽度，确保布局整齐
            Dictionary<int, int> columnWidths = CalculateColumnWidths(queryFields, RowOfColNum, UcPanel);

            int _x = 20, _y = 20;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // 保存每一行第一列的X起点
            List<int> xList = new List<int>();

            for (int c = 0; c < queryFields.Count; c++)
            {
                // 根据当前索引计算行列位置
                int currentRow = c / RowOfColNum;
                int currentCol = c % RowOfColNum;

                // 计算累计的X偏移量，考虑前面所有列的宽度
                int cumulativeX = 20;
                for (int i = 0; i < currentCol; i++)
                {
                    cumulativeX += columnWidths[i];
                }

                // 设置当前控件的X和Y坐标
                _x = cumulativeX;
                _y = 20 + currentRow * 32; // 32为行高

                QueryField queryField = queryFields[c];

                int currentColIndex = currentCol + 1;
                int maxtextLen = UIQueryPropertyBuilder.GetTargetColumnData(queryFields, RowOfColNum, currentColIndex).Max(t => t.Caption.Length);

                if (queryField.Caption.Length < maxtextLen)
                {
                    // 使用 PadLeft 方法将字符串左边补空格，达到 maxtextLen 的长度,使用全角空格进行填充
                    queryField.Caption = queryField.Caption.PadLeft(maxtextLen, '　');
                }

                // 保存每行第一个控件的X坐标
                if (currentCol == 0)
                {
                    xList.Add(_x);
                }
                KryptonLabel lbl = new KryptonLabel();
                lbl.Text = queryField.Caption;

                // 获取Graphics对象计算文本宽度
                Graphics graphics = UcPanel.CreateGraphics();
                float textWidth = UITools.CalculateTextWidth(lbl.Text, lbl.Font, graphics);

                // 设置Label的宽度
                int labelWidth = (int)textWidth + 10; // 加上一些额外的空间
                lbl.Width = labelWidth;

                // 设置标签位置
                lbl.Location = new System.Drawing.Point(_x, _y);

                // 计算控件起始位置，考虑标签宽度 - 间距减少
                _x = _x + labelWidth + 2; // 标签和控件之间留2px间距（减少为原来的约三分之一）

                DataBindingHelper dbh = new DataBindingHelper();
                KeyValuePair<string, string> pair = new KeyValuePair<string, string>();
                switch (queryField.AdvQueryFieldType)
                {
                    case AdvQueryProcessType.None:
                        break;

                    case AdvQueryProcessType.TextSelect:
                        #region 绑定文本框
                        KryptonTextBox tb_box_cmb = new KryptonTextBox();
                        tb_box_cmb.Name = queryField.FieldName;
                        tb_box_cmb.Width = 150;
                        if (string.IsNullOrEmpty(queryField.FriendlyFieldNameFormBiz))
                        {
                            DataBindingHelper.BindData4TextBox(newDto, queryField.FieldName, tb_box_cmb, BindDataType4TextBox.Text, false);
                        }
                        else
                        {
                            DataBindingHelper.BindData4TextBox(newDto, queryField.FriendlyFieldNameFormBiz, tb_box_cmb, BindDataType4TextBox.Text, false);
                        }


                        #region 生成快捷查询

                        string IDColName = string.Empty;
                        if (!string.IsNullOrEmpty(queryField.fKRelationAttribute.FK_IDColName))
                        {
                            IDColName = queryField.fKRelationAttribute.FK_IDColName;
                        }

                        if (!string.IsNullOrEmpty(queryField.FriendlyFieldValueFromSource))
                        {
                            IDColName = queryField.FriendlyFieldValueFromSource;
                        }

                        string DisplayColName = queryField.FriendlyFieldNameFormBiz;
                        if (queryField.FriendlyFieldNameFromSource.IsNotEmptyOrNull())
                        {
                            DisplayColName = queryField.FriendlyFieldNameFromSource;
                        }

                        //如果有最终指定原始的字段就用原始的来绑定
                        if (queryField.FriendlyFieldNameFromSource.IsNotEmptyOrNull())
                        {
                            DisplayColName = queryField.FriendlyFieldNameFromSource;
                        }
                        if (string.IsNullOrEmpty(IDColName))
                        {
                            IDColName = DisplayColName;
                        }
                        if (queryField.SubFilter.QueryTargetType == null)
                        {
                            MessageBox.Show("子查询对象类型没有指定。请截图后联系管理员。");
                        }
                        //注意这样调用不能用同名重载的方法名
                        MethodInfo mf2 = dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { queryField.SubFilter.QueryTargetType });
                        object[] args2 = new object[7] { newDto, tb_box_cmb, DisplayColName, queryField.SubFilter, queryField.SubFilter.QueryTargetType, IDColName, false };
                        mf2.Invoke(dbh, args2);

                        #endregion

                        tb_box_cmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(tb_box_cmb);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + tb_box_cmb.Width + 3; // 控件间距设为3像素
                        #endregion
                        break;

                    case AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                        #region 可多选的下拉 可以忽略  这里是子查询的条件设置了。
                        UCCmbMultiChoiceCanIgnore choiceCanIgnore = new UCCmbMultiChoiceCanIgnore();
                        choiceCanIgnore.Name = queryField.FieldName;
                        choiceCanIgnore.Text = "";
                        choiceCanIgnore.Width = 190;
                        // 设置实体类型和查询过滤器，用于查询按钮功能
                        choiceCanIgnore.TargetEntityType = queryField.SubFilter.QueryTargetType;
                        choiceCanIgnore.QueryFilter = queryField.SubFilter;

                        //只处理需要缓存的表
                        pair = new KeyValuePair<string, string>();
                        if (queryField.HasSubFilter)
                        {
                            var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                            var tableSchema = Startup.GetFromFac<ITableSchemaManager>().GetSchemaInfo(queryField.SubQueryTargetType.Name);
                            if (tableSchema != null)
                            {
                                pair = new KeyValuePair<string, string>(tableSchema.PrimaryKeyField, tableSchema.DisplayField);
                            }

                            #region 绑定下拉带子查询条件
                            Type mytype = queryField.SubQueryTargetType;
                            //UI传入过滤条件 下拉可以显示不同的数据
                            ExpConverter expConverter = new ExpConverter();
                            object whereExp = null;
                            if (queryField.SubFilter.GetFilterLimitExpression(mytype) != null)
                            {
                                whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                            }

                            //绑定可忽略的那个chkbox
                            DataBindingHelper.BindData4CheckBox(newDto, pair.Key + "_CmbMultiChoiceCanIgnore", choiceCanIgnore.chkCanIgnore, true);
                            //绑定下拉
                            MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                            object[] args1 = new object[6] { newDto, pair.Key, pair.Value, queryField.SubQueryTargetType.Name, choiceCanIgnore.chkMulti, whereExp };
                            mf1.Invoke(dbh, args1);
                            #endregion
                        }
                        else
                        {
                            #region 绑定 checkbox，关联了外键才有显示下拉的内容

                            //绑定可忽略的那个chkbox
                            DataBindingHelper.BindData4CheckBox(newDto, queryField.FieldName + "_CmbMultiChoiceCanIgnore", choiceCanIgnore.chkCanIgnore, true);
                            //绑定下拉 mytype 相当于T，单据的表名
                            Type mytype = queryField.QueryTargetType;
                            MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                            //如果表名空。则可能到这里是没有 实体特殊的FK外键表名没有设置。外键关系丢失。没有生成
                            //queryField.SubFilter.QueryTargetType.Name  这个是子表名是下拉的具体内容的。
                            object[] args1 = new object[6] { newDto, pair.Key, pair.Value, queryField.SubFilter.QueryTargetType.Name, choiceCanIgnore.chkMulti, null };
                            mf1.Invoke(dbh, args1);
                            #endregion
                        }


                        choiceCanIgnore.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(choiceCanIgnore);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + choiceCanIgnore.Width + 3; // 控件间距设为3像素
                        #endregion
                        break;
                    case AdvQueryProcessType.CmbMultiChoice:
                        #region 可多选的下拉
                        if (string.IsNullOrEmpty(queryField.FKTableName))
                        {
                            queryField.FKTableName = queryField.SubFilter.QueryTargetType.Name;
                        }
                        CheckBoxComboBox cmb = new CheckBoxComboBox();
                        //ButtonSpecAny buttonSpec = new ButtonSpecAny();
                        //buttonSpec.UniqueName = "btnclear";
                        //cmb.ButtonSpecs.Add(buttonSpec);
                        cmb.Name = queryField.FieldName;
                        cmb.Text = "";
                        cmb.Width = 150;
                        pair = new KeyValuePair<string, string>();
                        //只处理需要缓存的表
                        if (queryField.FKTableName.IsNotEmptyOrNull())
                        {
                            var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                            var tableSchema = Startup.GetFromFac<ITableSchemaManager>().GetSchemaInfo(queryField.SubQueryTargetType.Name);
                            if (tableSchema != null)
                            {
                                pair = new KeyValuePair<string, string>(tableSchema.PrimaryKeyField, tableSchema.DisplayField);
                                string PIDColName = pair.Key;
                                string PColName = pair.Value;
                                //DataBindingHelper.BindData4Cmb<T>(QueryDto, key, value, coldata.FKTableName, cmb);
                                //这里加载时 是指定了相关的外键表的对应实体的类型

                                //关联要绑定的类型
                                Type mytype = queryField.SubQueryTargetType;

                                //非常值和学习借鉴有代码 TODO 重点学习代码
                                //UI传入过滤条件 下拉可以显示不同的数据
                                ExpConverter expConverter = new ExpConverter();
                                object whereExp = null;
                                if (queryField.SubFilter.GetFilterLimitExpression(mytype) != null)
                                {
                                    whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                                }
                                #region 

                                //绑定下拉
                                MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbChkRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                                object[] args1 = new object[6] { newDto, PIDColName, PColName, queryField.FKTableName, cmb, whereExp };
                                mf1.Invoke(dbh, args1);
                                #endregion
                            }




                        }
                        cmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(cmb);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + cmb.Width + 3; // 控件间距设为3像素
                        #endregion
                        break;
                    case AdvQueryProcessType.defaultSelect:
                        #region     单选下拉

                        KryptonComboBox DefaultCmb = new KryptonComboBox();
                        DefaultCmb.Name = queryField.FieldName;
                        DefaultCmb.Text = "";
                        DefaultCmb.Width = 150;

                        //如果个性化设置了默认值则这里加载
                        if (queryField.EnableDefault1.HasValue)
                        {
                            if (queryField.EnableDefault1.Value && queryField.Default1 != null)
                            {
                                newDto.SetPropertyValue(queryField.FieldName, queryField.Default1.ToLong());
                            }
                        }

                        //只处理需要缓存的表
                        pair = new KeyValuePair<string, string>();
                        if (queryField.FKTableName.IsNotEmptyOrNull())
                        {
                            var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
                            var tableSchema = Startup.GetFromFac<ITableSchemaManager>().GetSchemaInfo(queryField.SubQueryTargetType.Name);
                            if (tableSchema != null)
                            {
                                pair = new KeyValuePair<string, string>(tableSchema.PrimaryKeyField, tableSchema.DisplayField);
                                //关联要绑定的类型
                                Type mytype = queryField.SubQueryTargetType;
                                if (queryField.SubFilter.FilterLimitExpressions.Count == 0)
                                {
                                    #region 没有限制条件时

                                    if (mytype != null)
                                    {
                                        MethodInfo mf = dbh.GetType().GetMethod("BindData4CmbRef").MakeGenericMethod(new Type[] { mytype });
                                        object[] args = new object[5] { newDto, pair.Key, pair.Value, queryField.FKTableName, DefaultCmb };
                                        mf.Invoke(dbh, args);
                                    }
                                    else
                                    {
                                        MainForm.Instance.logger.LogError("动态加载外键数据时出错，" + queryField.FieldName + "在代理类中的属性名不对，自动生成规则可能有变化" + queryField.FKTableName.ToLower());
                                        MainForm.Instance.uclog.AddLog("动态加载外键数据时出错加载数据出错。请联系管理员。");
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //非常值和学习借鉴有代码 TODO 重点学习代码
                                    //UI传入过滤条件 下拉可以显示不同的数据
                                    ExpConverter expConverter = new ExpConverter();
                                    var whereExp = expConverter.ConvertToFuncByClassName(queryField.SubFilter.QueryTargetType, queryField.SubFilter.GetFilterLimitExpression(mytype));
                                    #region 
                                    //绑定下拉

                                    if (pair.Key == queryField.FieldName)
                                    {
                                        MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbRefWithLimited").MakeGenericMethod(new Type[] { mytype });
                                        object[] args1 = new object[6] { newDto, pair.Key, pair.Value, queryField.FKTableName, DefaultCmb, whereExp };
                                        mf1.Invoke(dbh, args1);
                                    }
                                    else
                                    {
                                        MethodInfo mf1 = dbh.GetType().GetMethod("BindData4CmbRefWithLimitedByAlias").MakeGenericMethod(new Type[] { mytype });
                                        //注意这样
                                        object[] args1 = new object[7] { newDto, pair.Key, queryField.FieldName, pair.Value, queryField.FKTableName, DefaultCmb, whereExp };
                                        mf1.Invoke(dbh, args1);
                                    }

                                    #endregion
                                }

                                //注意这样调用不能用同名重载的方法名
                                MethodInfo mf22 = dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { mytype });
                                object[] args22 = new object[7] { newDto, DefaultCmb, pair.Value, queryField.SubFilter, null, null, false };
                                mf22.Invoke(dbh, args22);
                            }
                        }

                        DefaultCmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(DefaultCmb);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + DefaultCmb.Width + 3; // 控件间距设为3像素
                        #endregion
                        break;
                    case AdvQueryProcessType.EnumSelect:
                        KryptonComboBox eNumCmb = new KryptonComboBox();
                        eNumCmb.Name = queryField.FieldName;
                        eNumCmb.Text = "";
                        eNumCmb.Width = 150;

                        #region 枚举类型处理




                        //非常值和学习借鉴有代码 TODO 重点学习代码
                        //UI传入过滤条件 下拉可以显示不同的数据
                        if (queryField.FieldType == QueryFieldType.CmbEnum)
                        {
                            EnumBindingHelper bindingHelper = new EnumBindingHelper();

                            if (queryField.QueryFieldDataPara != null)
                            {
                                if (queryField.QueryFieldDataPara is QueryFieldEnumData enumData)
                                {
                                    // 获取枚举的基础类型
                                    Type underlyingType = Enum.GetUnderlyingType(enumData.EnumType);
                                    //绑定下拉值

                                    //绑定实体值 这里没有指定 过滤
                                    //枚举的值 默认为-1，则指定为请选择
                                    if (underlyingType == typeof(int))
                                    {
                                        newDto.SetPropertyValue(eNumCmb.Name, -1);
                                    }
                                    if (underlyingType == typeof(long))
                                    {
                                        newDto.SetPropertyValue(eNumCmb.Name, -1L);
                                    }

                                    DataBindingHelper.BindData4CmbByEnumRef(newDto, enumData.EnumValueColName, enumData.EnumType, eNumCmb, enumData.AddSelectItem);



                                }
                            }

                        }
                        eNumCmb.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(eNumCmb);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + eNumCmb.Width + 3; // 控件间距设为3像素

                        //cmb.SelectedIndex = -1;

                        #endregion

                        break;
                    case AdvQueryProcessType.datetimeRange:
                        UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
                        dtpgroup.Name = queryField.FieldName;
                        string dtpKeyName1 = queryField.ExtendedAttribute[0].ColName;
                        string dtpKeyName2 = queryField.ExtendedAttribute[1].ColName;

                        dtpgroup.dtp1.Name = dtpKeyName1;
                        object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, dtpKeyName1);
                        if (datetimeValue1 != null && string.IsNullOrEmpty(datetimeValue1.ToString()) && queryField.DiffDays1.HasValue)
                        {
                            datetimeValue1 = System.DateTime.Now.AddDays(queryField.DiffDays1.Value);
                            dtpgroup.dtp1.Value = System.DateTime.Now.AddDays(queryField.DiffDays1.Value);
                            newDto.SetPropertyValue(dtpKeyName1, datetimeValue1);
                        }
                        DataBindingHelper.BindData4DataTime(newDto, datetimeValue1, dtpKeyName1, dtpgroup.dtp1, true);
                        // 确保控件可见
                        dtpgroup.dtp1.Visible = true;
                        dtpgroup.dtp1.ShowCheckBox = true;

                        dtpgroup.dtp2.Name = dtpKeyName2;
                        object datetimeValue2 = ReflectionHelper.GetPropertyValue(newDto, dtpKeyName2);
                        if (datetimeValue2 != null && string.IsNullOrEmpty(datetimeValue2.ToString()) && queryField.DiffDays2.HasValue)
                        {
                            datetimeValue2 = System.DateTime.Now.AddDays(queryField.DiffDays2.Value);
                            newDto.SetPropertyValue(dtpKeyName2, datetimeValue2);
                            dtpgroup.dtp2.Value = System.DateTime.Now.AddDays(queryField.DiffDays2.Value);
                        }
                        DataBindingHelper.BindData4DataTime(newDto, datetimeValue2, dtpKeyName2, dtpgroup.dtp2, true);
                        // 确保控件可见
                        dtpgroup.dtp2.Visible = true;
                        dtpgroup.dtp2.ShowCheckBox = true;

                        //如果时间区间的参数不为空。看参数里设置默认选中情况
                        if (queryField.QueryFieldDataPara != null)
                        {
                            var queryFieldDataPara = queryField.QueryFieldDataPara;
                            if (queryFieldDataPara is QueryFieldDateTimeRangeData dateTimeData)
                            {
                                dtpgroup.dtp1.Checked = dateTimeData.Selected;
                                dtpgroup.dtp2.Checked = dateTimeData.Selected;
                            }
                        }
                        else
                        {
                            //默认选中
                            dtpgroup.dtp1.Checked = queryField.IsEnabled;
                            dtpgroup.dtp2.Checked = queryField.IsEnabled;
                        }

                        if (queryField.EnableDefault1.HasValue)
                        {
                            dtpgroup.dtp1.Checked = queryField.EnableDefault1.Value;
                        }

                        if (queryField.EnableDefault2.HasValue)
                        {
                            dtpgroup.dtp2.Checked = queryField.EnableDefault2.Value;
                        }

                        // 确保日期控件大小正确
                        dtpgroup.Size = new System.Drawing.Size(265, 25);
                        // 设置日期控件位置
                        dtpgroup.Location = new System.Drawing.Point(_x, _y);
                        dtpgroup.Visible = true;

                        // 添加控件到面板
                        // 先添加标签再添加控件，确保标签不会被覆盖
                        UcPanel.Controls.Add(lbl);
                        UcPanel.Controls.Add(dtpgroup);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + dtpgroup.Width + 3; // 控件间距设为3像素
                        break;
                    case AdvQueryProcessType.datetime:
                        KryptonDateTimePicker dtp = new KryptonDateTimePicker();
                        dtp.Name = queryField.FieldName;
                        dtp.ShowCheckBox = true;
                        dtp.Width = 130;
                        dtp.Format = DateTimePickerFormat.Custom;
                        dtp.CustomFormat = "yyyy-MM-dd";
                        object datetimeValue = ReflectionHelper.GetPropertyValue(newDto, queryField.FieldName);
                        //如果时间为00001-1-1-1这种。就改为当前

                        if (datetimeValue.ToDateTime().Year == 1)
                        {
                            datetimeValue = System.DateTime.Now;
                            ReflectionHelper.SetPropertyValue(newDto, queryField.FieldName, datetimeValue);
                        }

                        DataBindingHelper.BindData4DataTime(newDto, datetimeValue, queryField.FieldName, dtp, true);
                        dtp.Checked = true;

                        // 设置控件位置
                        dtp.Location = new System.Drawing.Point(_x, _y);

                        // 添加控件到面板
                        // 先添加标签再添加控件，确保标签不会被覆盖
                        UcPanel.Controls.Add(lbl);
                        UcPanel.Controls.Add(dtp);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + dtp.Width + 3; // 控件间距设为3像素
                        break;
                    case AdvQueryProcessType.stringLike:
                        KryptonTextBox tb_box = new KryptonTextBox();
                        tb_box.Name = queryField.FieldName;
                        tb_box.Width = 150;

                        DataBindingHelper.BindData4TextBox(newDto, queryField.FieldName, tb_box, BindDataType4TextBox.Text, false);
                        tb_box.Location = new System.Drawing.Point(_x, _y);

                        //动态生成一个右键菜单
                        MenuItem menuItem = new MenuItem();

                        menuItem.Text = "精确查询";
                        menuItem.Click += (sender, e) =>
                        {
                            // queryField.UseLike = true;
                            //复制到剪贴板
                            //Clipboard.SetText(tb_box.Text);
                        };
                        ContextMenu menu = new ContextMenu();
                        menu.MenuItems.Add(menuItem);
                        tb_box.ContextMenu = menu;
                        UcPanel.Controls.Add(tb_box);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + tb_box.Width + 3; // 控件间距设为3像素
                        break;

                    case AdvQueryProcessType.stringEquals:
                        KryptonTextBox tb_boxEquals = new KryptonTextBox();
                        tb_boxEquals.Name = queryField.FieldName;
                        tb_boxEquals.Width = 150;

                        DataBindingHelper.BindData4TextBox(newDto, queryField.FieldName, tb_boxEquals, BindDataType4TextBox.Text, false);
                        tb_boxEquals.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(tb_boxEquals);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + tb_boxEquals.Width + 3; // 控件间距设为3像素
                        break;

                    case AdvQueryProcessType.useYesOrNoToAll:
                        UCAdvYesOrNO chkgroup = new UCAdvYesOrNO();
                        // object datetimeValue1 = ReflectionHelper.GetPropertyValue(newDto, queryField.ExtendedAttribute[0].ColName);
                        chkgroup.rdb1.Text = "是";
                        chkgroup.rdb2.Text = "否";
                        DataBindingHelper.BindData4CheckBox(newDto, queryField.ExtendedAttribute[0].ColName, chkgroup.chk, false);
                        DataBindingHelper.BindData4RadioGroupTrueFalse(newDto, queryField.ExtendedAttribute[0].RelatedFields, chkgroup.rdb1, chkgroup.rdb2);
                        chkgroup.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(chkgroup);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + chkgroup.Width + 3; // 控件间距设为3像素
                        break;

                    case AdvQueryProcessType.YesOrNo:
                        KryptonCheckBox chk = new KryptonCheckBox();
                        chk.Name = queryField.FieldName;
                        chk.Text = "";

                        //newDto
                        DataBindingHelper.BindData4CheckBox(newDto, queryField.FieldName, chk, false);
                        chk.Location = new System.Drawing.Point(_x, _y);
                        UcPanel.Controls.Add(chk);
                        UcPanel.Controls.Add(lbl);

                        // 更新_x位置，添加控件间距（3像素）
                        _x = _x + chk.Width + 3; // 控件间距设为3像素
                        break;
                    default:



                        break;
                }
            }

            // 更新面板高度
            int totalRows = (queryFields.Count + RowOfColNum - 1) / RowOfColNum; // 计算总行数
            UcPanel.Parent.Height = 20 + totalRows * 32 + 30; // 20是上边距，32是行高，30是底部留空
            UcPanel.Height = 20 + totalRows * 32 + 30;

            sw.Stop();
            TimeSpan dt = sw.Elapsed;
            MainForm.Instance.uclog.AddLog("性能", "加载控件耗时：" + dt.TotalMilliseconds.ToString());

            #endregion

            return Query;
        }





    }
}
