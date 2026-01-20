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
    /// <summary>
    /// 动态UI生成器 - 优化版
    /// 核心功能: 根据实体类型动态生成查询UI控件，并与代理实例双向绑定
    /// </summary>
    public class UIGenerateHelper
    {
        #region 常量定义

        /// <summary>默认列数</summary>
        private const int DefaultColumnCount = 4;

        /// <summary>行高</summary>
        private const int RowHeight = 32;

        /// <summary>上边距</summary>
        private const int TopMargin = 20;

        /// <summary>底部留空</summary>
        private const int BottomPadding = 30;

        /// <summary>标签与控件间距</summary>
        private const int LabelControlSpacing = 2;

        /// <summary>控件间距</summary>
        private const int ControlSpacing = 3;

        /// <summary>标签额外宽度</summary>
        private const int LabelExtraWidth = 10;

        /// <summary>最小列宽</summary>
        private const int MinColumnWidth = 200;

        /// <summary>列间距</summary>
        private const int ColumnSpacing = 15;

        #endregion

        #region 控件工厂方法

        /// <summary>
        /// 创建查询UI界面的主入口方法
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="useLike">是否使用模糊查询</param>
        /// <param name="panel">容器面板</param>
        /// <param name="queryFilter">查询过滤器</param>
        /// <param name="personalization">个性化配置 (可选)</param>
        /// <returns>代理实体实例，包含用户的查询条件</returns>
        /// <exception cref="ArgumentNullException">当entityType或queryFilter为null时抛出</exception>
        public static BaseEntity CreateQueryUI(
            Type entityType, 
            bool useLike, 
            KryptonPanel panel, 
            QueryFilter queryFilter, 
            tb_UIMenuPersonalization personalization)
        {
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (queryFilter == null)
                throw new ArgumentNullException(nameof(queryFilter));

            // 步骤1: 应用个性化配置
            ApplyPersonalizationSettings(queryFilter, personalization);

            // 步骤2: 生成代理类型和实例
            var proxyType = UIQueryPropertyBuilder.CreateQueryProxyType(entityType, queryFilter);
            var proxyInstance = Activator.CreateInstance(proxyType) as BaseEntity;

            // 步骤3: 更新查询字段元数据
            UpdateQueryFieldMetadata(queryFilter, proxyType);

            // 步骤4: 计算UI布局
            var visibleQueryFields = queryFilter.QueryFields.Where(q => q.IsVisible).ToList();
            var columnCount = GetColumnCount(personalization);
            var columnWidths = CalculateColumnWidths(visibleQueryFields, columnCount, panel);

            // 步骤5: 生成查询控件
            var stopwatch = Stopwatch.StartNew();
            GenerateQueryControls(
                panel, 
                visibleQueryFields, 
                proxyInstance, 
                columnWidths, 
                columnCount,
                personalization);

            stopwatch.Stop();
            LogPerformance("加载控件耗时", stopwatch.Elapsed);

            // 步骤6: 调整面板高度
            AdjustPanelHeight(panel, visibleQueryFields.Count, columnCount);

            return proxyInstance;
        }

        /// <summary>
        /// 重载方法：使用默认的useLike参数
        /// </summary>
        public static BaseEntity CreateQueryUI(
            Type entityType, 
            bool useLike, 
            KryptonPanel panel, 
            QueryFilter queryFilter, 
            decimal defineColNum)
        {
            return CreateQueryUI(entityType, useLike, panel, queryFilter, null);
        }

        #endregion

        #region 个性化配置

        /// <summary>
        /// 应用个性化配置到查询字段
        /// </summary>
        /// <param name="queryFilter">查询过滤器</param>
        /// <param name="personalization">个性化配置</param>
        private static void ApplyPersonalizationSettings(
            QueryFilter queryFilter, 
            tb_UIMenuPersonalization personalization)
        {
            if (personalization == null || personalization.tb_UIQueryConditions == null)
            {
                // 没有个性化配置，全部显示
                queryFilter.QueryFields.ForEach(f => f.IsVisible = true);
                return;
            }

            // 应用个性化配置
            var conditions = personalization.tb_UIQueryConditions;
            foreach (var queryField in queryFilter.QueryFields)
            {
                var condition = conditions.FirstOrDefault(c => c.FieldName == queryField.FieldName);
                if (condition == null)
                    continue;

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

                // 更新多选配置
                UpdateMultiChoiceConfig(queryField, condition);
            }
        }

        /// <summary>
        /// 更新多选配置
        /// </summary>
        private static void UpdateMultiChoiceConfig(QueryField queryField, tb_UIQueryCondition condition)
        {
            if (queryField.AdvQueryFieldType == AdvQueryProcessType.defaultSelect ||
                queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoice ||
                queryField.AdvQueryFieldType == AdvQueryProcessType.CmbMultiChoiceCanIgnore)
            {
                if (condition.MultiChoice.HasValue && condition.MultiChoice.Value)
                {
                    queryField.AdvQueryFieldType = AdvQueryProcessType.CmbMultiChoiceCanIgnore;
                }
                else
                {
                    queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                }
            }
        }

        #endregion

        #region 元数据更新

        /// <summary>
        /// 更新查询字段的元数据信息
        /// </summary>
        /// <param name="queryFilter">查询过滤器</param>
        /// <param name="proxyType">代理类型</param>
        private static void UpdateQueryFieldMetadata(QueryFilter queryFilter, Type proxyType)
        {
            var proxyFields = UIHelper.GetDtoFieldNameList(proxyType);
            if (proxyFields.Count == 0)
                return;

            foreach (var proxyField in proxyFields)
            {
                var queryField = queryFilter.QueryFields.FirstOrDefault(q => q.FieldName == proxyField.SugarCol.ColumnName);
                if (queryField == null)
                    continue;

                // 更新字段元数据
                queryField.SugarCol = proxyField.SugarCol;
                queryField.ExtendedAttribute = proxyField.ExtendedAttribute;
                queryField.Caption = proxyField.Caption;
                queryField.ColDataType = proxyField.ColDataType.GetBaseType();
                queryField.IsRelated = proxyField.IsFKRelationAttribute;

                // 处理外键关联
                if (queryField.IsRelated)
                {
                    queryField.fKRelationAttribute = proxyField.fKRelationAttribute;
                    
                    if (string.IsNullOrEmpty(queryField.FKTableName))
                    {
                        queryField.FKTableName = proxyField.FKTableName;
                    }

                    if (queryField.SubQueryTargetType == null)
                    {
                        var subQueryType = AssemblyLoader.GetType("RUINORERP.Model", "RUINORERP.Model." + queryField.FKTableName);
                        if (subQueryType != null)
                        {
                            queryField.SubQueryTargetType = subQueryType;
                        }
                    }
                }

                // 设置默认查询类型
                SetDefaultQueryType(queryField);
            }
        }

        /// <summary>
        /// 设置默认查询类型
        /// </summary>
        private static void SetDefaultQueryType(QueryField queryField)
        {
            if (queryField.AdvQueryFieldType != AdvQueryProcessType.None)
                return;

            var dataType = Enum.Parse<EnumDataType>(queryField.ColDataType.Name);

            switch (dataType)
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
                case EnumDataType.UInt64:
                    queryField.AdvQueryFieldType = AdvQueryProcessType.defaultSelect;
                    break;

                case EnumDataType.String:
                    if (queryField.UseLike.HasValue && queryField.UseLike.Value)
                    {
                        queryField.AdvQueryFieldType = AdvQueryProcessType.stringLike;
                    }
                    else
                    {
                        queryField.AdvQueryFieldType = AdvQueryProcessType.stringEquals;
                    }
                    break;
            }
        }

        #endregion

        #region 布局计算

        /// <summary>
        /// 获取列数
        /// </summary>
        private static int GetColumnCount(tb_UIMenuPersonalization personalization)
        {
            if (personalization != null)
            {
                return personalization.QueryConditionCols.ToInt();
            }
            return DefaultColumnCount;
        }

        /// <summary>
        /// 计算每列宽度
        /// </summary>
        /// <param name="queryFields">查询字段列表</param>
        /// <param name="columnCount">列数</param>
        /// <param name="panel">容器面板</param>
        /// <returns>每列宽度字典</returns>
        private static Dictionary<int, int> CalculateColumnWidths(
            List<QueryField> queryFields, 
            int columnCount, 
            KryptonPanel panel)
        {
            var columnWidths = new Dictionary<int, int>();
            
            // 初始化最小宽度
            for (int i = 0; i < columnCount; i++)
            {
                columnWidths[i] = MinColumnWidth;
            }

            // 遍历字段计算最大宽度
            for (int i = 0; i < queryFields.Count; i++)
            {
                var queryField = queryFields[i];
                int columnIndex = i % columnCount;

                // 计算标签宽度
                using (var graphics = panel.CreateGraphics())
                {
                    float textWidth = UITools.CalculateTextWidth(queryField.Caption, SystemFonts.DefaultFont, graphics);
                    int labelWidth = (int)textWidth + LabelExtraWidth;

                    // 计算控件宽度
                    int inputWidth = GetControlWidth(queryField.AdvQueryFieldType);

                    // 总宽度 = 标签宽度 + 控件宽度 + 间距
                    int totalWidth = labelWidth + inputWidth + ColumnSpacing;

                    // 更新最大宽度
                    if (totalWidth > columnWidths[columnIndex])
                    {
                        columnWidths[columnIndex] = totalWidth;
                    }
                }
            }

            return columnWidths;
        }

        /// <summary>
        /// 根据查询类型获取控件宽度
        /// </summary>
        private static int GetControlWidth(AdvQueryProcessType queryType)
        {
            switch (queryType)
            {
                case AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                    return 190;

                case AdvQueryProcessType.datetimeRange:
                    return 280;

                case AdvQueryProcessType.datetime:
                    return 130;

                case AdvQueryProcessType.YesOrNo:
                case AdvQueryProcessType.useYesOrNoToAll:
                    return 50;

                default:
                    return 150;
            }
        }

        /// <summary>
        /// 调整面板高度
        /// </summary>
        private static void AdjustPanelHeight(KryptonPanel panel, int fieldCount, int columnCount)
        {
            int totalRows = (fieldCount + columnCount - 1) / columnCount;
            int requiredHeight = TopMargin + totalRows * RowHeight + BottomPadding;

            if (panel.Parent != null)
            {
                panel.Parent.Height = requiredHeight;
            }
            panel.Height = requiredHeight;
        }

        #endregion

        #region 控件生成

        /// <summary>
        /// 生成所有查询控件
        /// </summary>
        private static void GenerateQueryControls(
            KryptonPanel panel,
            List<QueryField> queryFields,
            BaseEntity proxyInstance,
            Dictionary<int, int> columnWidths,
            int columnCount,
            tb_UIMenuPersonalization personalization)
        {
            for (int i = 0; i < queryFields.Count; i++)
            {
                var queryField = queryFields[i];
                int currentRow = i / columnCount;
                int currentCol = i % columnCount;

                // 计算控件位置
                var position = CalculateControlPosition(currentCol, currentRow, columnWidths);

                // 创建标签
                var label = CreateLabel(queryField, position.LabelPosition);

                // 根据查询类型创建对应控件
                var control = CreateControlByQueryType(queryField, proxyInstance, position.ControlPosition, personalization);

                // 添加到面板
                panel.Controls.Add(label);
                if (control != null)
                {
                    panel.Controls.Add(control);
                }
            }
        }

        /// <summary>
        /// 计算控件位置
        /// </summary>
        private static (Point LabelPosition, Point ControlPosition) CalculateControlPosition(
            int column, 
            int row, 
            Dictionary<int, int> columnWidths)
        {
            // 计算累计X偏移
            int x = TopMargin;
            for (int i = 0; i < column; i++)
            {
                x += columnWidths[i];
            }

            int y = TopMargin + row * RowHeight;

            return (new Point(x, y), new Point(x, y));
        }

        /// <summary>
        /// 创建标签控件
        /// </summary>
        private static KryptonLabel CreateLabel(QueryField queryField, Point position)
        {
            var label = new KryptonLabel
            {
                Text = queryField.Caption,
                Location = position
            };

            // 计算并设置标签宽度
            using (var graphics = label.CreateGraphics())
            {
                float textWidth = UITools.CalculateTextWidth(label.Text, label.Font, graphics);
                label.Width = (int)textWidth + LabelExtraWidth;
            }

            return label;
        }

        /// <summary>
        /// 根据查询类型创建对应控件
        /// </summary>
        private static Control CreateControlByQueryType(
            QueryField queryField,
            BaseEntity proxyInstance,
            Point position,
            tb_UIMenuPersonalization personalization)
        {
            switch (queryField.AdvQueryFieldType)
            {
                case AdvQueryProcessType.TextSelect:
                    return CreateTextSelectControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.defaultSelect:
                    return CreateSingleSelectControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.CmbMultiChoice:
                    return CreateMultiSelectControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.CmbMultiChoiceCanIgnore:
                    return CreateMultiSelectIgnoreControl(queryField, proxyInstance, position, personalization);

                case AdvQueryProcessType.EnumSelect:
                    return CreateEnumSelectControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.datetimeRange:
                    return CreateDateTimeRangeControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.datetime:
                    return CreateDateTimeControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.stringLike:
                    return CreateStringLikeControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.stringEquals:
                    return CreateStringEqualsControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.useYesOrNoToAll:
                    return CreateYesOrNoToAllControl(queryField, proxyInstance, position);

                case AdvQueryProcessType.YesOrNo:
                    return CreateYesOrNoControl(queryField, proxyInstance, position);

                default:
                    return null;
            }
        }

        #region 各类型控件创建方法

        /// <summary>
        /// 创建文本选择控件 (外键关联)
        /// </summary>
        private static Control CreateTextSelectControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var textBox = new KryptonTextBox
            {
                Name = queryField.FieldName,
                Width = 150,
                Location = position
            };

            var bindingField = string.IsNullOrEmpty(queryField.FriendlyFieldNameFormBiz) 
                ? queryField.FieldName 
                : queryField.FriendlyFieldNameFormBiz;

            DataBindingHelper.BindData4TextBox(proxyInstance, bindingField, textBox, BindDataType4TextBox.Text, false);

            return textBox;
        }

        /// <summary>
        /// 创建单选下拉控件
        /// </summary>
        private static Control CreateSingleSelectControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var comboBox = new KryptonComboBox
            {
                Name = queryField.FieldName,
                Text = "",
                Width = 150,
                Location = position
            };

            // 应用默认值
            if (queryField.EnableDefault1.HasValue && queryField.EnableDefault1.Value && queryField.Default1 != null)
            {
                proxyInstance.SetPropertyValue(queryField.FieldName, queryField.Default1.ToLong());
            }

            // 绑定外键数据
            BindForeignKeyData(comboBox, queryField, proxyInstance);

            return comboBox;
        }

        /// <summary>
        /// 创建多选下拉控件
        /// </summary>
        private static Control CreateMultiSelectControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var comboBox = new CheckBoxComboBox
            {
                Name = queryField.FieldName,
                Text = "",
                Width = 150,
                Location = position
            };

            BindForeignKeyData(comboBox, queryField, proxyInstance);

            return comboBox;
        }

        /// <summary>
        /// 创建多选可忽略控件
        /// </summary>
        private static Control CreateMultiSelectIgnoreControl(
            QueryField queryField, 
            BaseEntity proxyInstance, 
            Point position,
            tb_UIMenuPersonalization personalization)
        {
            var control = new UCCmbMultiChoiceCanIgnore
            {
                Name = queryField.FieldName,
                Text = "",
                Width = 190,
                TargetEntityType = queryField.SubFilter?.QueryTargetType,
                QueryFilter = queryField.SubFilter,
                Location = position
            };

            // 绑定外键数据
            if (queryField.HasSubFilter)
            {
                var tableSchema = Startup.GetFromFac<ITableSchemaManager>()?.GetSchemaInfo(queryField.SubQueryTargetType?.Name);
                if (tableSchema != null)
                {
                    var pair = new KeyValuePair<string, string>(tableSchema.PrimaryKeyField, tableSchema.DisplayField);

                    // 绑定可忽略复选框
                    DataBindingHelper.BindData4CheckBox(proxyInstance, $"{pair.Key}_CmbMultiChoiceCanIgnore", control.chkCanIgnore, true);

                    // 绑定多选下拉
                    Type myType = queryField.SubQueryTargetType;
                    ExpConverter expConverter = new ExpConverter();
                    object whereExp = null;
                    
                    if (queryField.SubFilter?.GetFilterLimitExpression(myType) != null)
                    {
                        whereExp = expConverter.ConvertToFuncByClassName(
                            queryField.SubFilter.QueryTargetType, 
                            queryField.SubFilter.GetFilterLimitExpression(myType));
                    }

                    var methodInfo = new DataBindingHelper().GetType()
                        .GetMethod("BindData4CmbChkRefWithLimited")
                        ?.MakeGenericMethod(new[] { myType });

                    methodInfo?.Invoke(
                        new DataBindingHelper(), 
                        new object[] { proxyInstance, pair.Key, pair.Value, queryField.SubQueryTargetType?.Name, control.chkMulti, whereExp });
                }
            }

            return control;
        }

        /// <summary>
        /// 创建枚举选择控件
        /// </summary>
        private static Control CreateEnumSelectControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var comboBox = new KryptonComboBox
            {
                Name = queryField.FieldName,
                Text = "",
                Width = 150,
                Location = position
            };

            if (queryField.QueryFieldDataPara is QueryFieldEnumData enumData)
            {
                var underlyingType = Enum.GetUnderlyingType(enumData.EnumType);

                // 设置默认值
                if (underlyingType == typeof(int))
                {
                    proxyInstance.SetPropertyValue(queryField.FieldName, -1);
                }
                else if (underlyingType == typeof(long))
                {
                    proxyInstance.SetPropertyValue(queryField.FieldName, -1L);
                }

                DataBindingHelper.BindData4CmbByEnumRef(
                    proxyInstance, 
                    enumData.EnumValueColName, 
                    enumData.EnumType, 
                    comboBox, 
                    enumData.AddSelectItem);
            }

            return comboBox;
        }

        /// <summary>
        /// 创建日期范围控件
        /// </summary>
        private static Control CreateDateTimeRangeControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var dtpGroup = new UCAdvDateTimerPickerGroup
            {
                Name = queryField.FieldName,
                Size = new Size(265, 25),
                Location = position,
                Visible = true
            };

            if (queryField.ExtendedAttribute == null || queryField.ExtendedAttribute.Count < 2)
            {
                MainForm.Instance.logger?.LogError($"日期范围字段 {queryField.FieldName} 缺少扩展属性");
                return dtpGroup;
            }

            // 起始时间绑定
            string startKeyName = queryField.ExtendedAttribute[0].ColName;
            dtpGroup.dtp1.Name = startKeyName;
            
            object startValue = ReflectionHelper.GetPropertyValue(proxyInstance, startKeyName);
            if (startValue != null && string.IsNullOrEmpty(startValue.ToString()) && queryField.DiffDays1.HasValue)
            {
                startValue = DateTime.Now.AddDays(queryField.DiffDays1.Value);
                dtpGroup.dtp1.Value = startValue;
                proxyInstance.SetPropertyValue(startKeyName, startValue);
            }

            DataBindingHelper.BindData4DataTime(proxyInstance, startValue, startKeyName, dtpGroup.dtp1, true);
            dtpGroup.dtp1.Visible = true;
            dtpGroup.dtp1.ShowCheckBox = true;

            // 结束时间绑定
            string endKeyName = queryField.ExtendedAttribute[1].ColName;
            dtpGroup.dtp2.Name = endKeyName;

            object endValue = ReflectionHelper.GetPropertyValue(proxyInstance, endKeyName);
            if (endValue != null && string.IsNullOrEmpty(endValue.ToString()) && queryField.DiffDays2.HasValue)
            {
                endValue = DateTime.Now.AddDays(queryField.DiffDays2.Value);
                dtpGroup.dtp2.Value = endValue;
                proxyInstance.SetPropertyValue(endKeyName, endValue);
            }

            DataBindingHelper.BindData4DataTime(proxyInstance, endValue, endKeyName, dtpGroup.dtp2, true);
            dtpGroup.dtp2.Visible = true;
            dtpGroup.dtp2.ShowCheckBox = true;

            // 应用默认选中配置
            ApplyDateTimeDefaultConfig(dtpGroup, queryField);

            return dtpGroup;
        }

        /// <summary>
        /// 应用日期默认配置
        /// </summary>
        private static void ApplyDateTimeDefaultConfig(UCAdvDateTimerPickerGroup dtpGroup, QueryField queryField)
        {
            if (queryField.QueryFieldDataPara is QueryFieldDateTimeRangeData dateTimeData)
            {
                dtpGroup.dtp1.Checked = dateTimeData.Selected;
                dtpGroup.dtp2.Checked = dateTimeData.Selected;
            }
            else
            {
                dtpGroup.dtp1.Checked = queryField.IsEnabled;
                dtpGroup.dtp2.Checked = queryField.IsEnabled;
            }

            if (queryField.EnableDefault1.HasValue)
            {
                dtpGroup.dtp1.Checked = queryField.EnableDefault1.Value;
            }

            if (queryField.EnableDefault2.HasValue)
            {
                dtpGroup.dtp2.Checked = queryField.EnableDefault2.Value;
            }
        }

        /// <summary>
        /// 创建日期选择控件
        /// </summary>
        private static Control CreateDateTimeControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var dtp = new KryptonDateTimePicker
            {
                Name = queryField.FieldName,
                ShowCheckBox = true,
                Width = 130,
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd",
                Location = position
            };

            object datetimeValue = ReflectionHelper.GetPropertyValue(proxyInstance, queryField.FieldName);

            // 处理无效日期
            if (datetimeValue.ToDateTime().Year == 1)
            {
                datetimeValue = DateTime.Now;
                ReflectionHelper.SetPropertyValue(proxyInstance, queryField.FieldName, datetimeValue);
            }

            DataBindingHelper.BindData4DataTime(proxyInstance, datetimeValue, queryField.FieldName, dtp, true);
            dtp.Checked = true;

            return dtp;
        }

        /// <summary>
        /// 创建模糊查询控件
        /// </summary>
        private static Control CreateStringLikeControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var textBox = new KryptonTextBox
            {
                Name = queryField.FieldName,
                Width = 150,
                Location = position
            };

            DataBindingHelper.BindData4TextBox(proxyInstance, queryField.FieldName, textBox, BindDataType4TextBox.Text, false);

            // 添加右键菜单
            var menu = new ContextMenu();
            var menuItem = new MenuItem
            {
                Text = "精确查询"
            };
            
            menuItem.Click += (sender, e) =>
            {
                // 可以切换为精确查询模式
                // queryField.UseLike = false;
            };

            menu.MenuItems.Add(menuItem);
            textBox.ContextMenu = menu;

            return textBox;
        }

        /// <summary>
        /// 创建精确查询控件
        /// </summary>
        private static Control CreateStringEqualsControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var textBox = new KryptonTextBox
            {
                Name = queryField.FieldName,
                Width = 150,
                Location = position
            };

            DataBindingHelper.BindData4TextBox(proxyInstance, queryField.FieldName, textBox, BindDataType4TextBox.Text, false);

            return textBox;
        }

        /// <summary>
        /// 创建是/否/全部控件
        /// </summary>
        private static Control CreateYesOrNoToAllControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var control = new UCAdvYesOrNO
            {
                rdb1 = { Text = "是" },
                rdb2 = { Text = "否" },
                Location = position
            };

            if (queryField.ExtendedAttribute == null || queryField.ExtendedAttribute.Count == 0)
            {
                MainForm.Instance.logger?.LogError($"是/否字段 {queryField.FieldName} 缺少扩展属性");
                return control;
            }

            DataBindingHelper.BindData4CheckBox(proxyInstance, queryField.ExtendedAttribute[0].ColName, control.chk, false);
            DataBindingHelper.BindData4RadioGroupTrueFalse(
                proxyInstance, 
                queryField.ExtendedAttribute[0].RelatedFields, 
                control.rdb1, 
                control.rdb2);

            return control;
        }

        /// <summary>
        /// 创建是/否控件
        /// </summary>
        private static Control CreateYesOrNoControl(QueryField queryField, BaseEntity proxyInstance, Point position)
        {
            var checkBox = new KryptonCheckBox
            {
                Name = queryField.FieldName,
                Text = "",
                Location = position
            };

            DataBindingHelper.BindData4CheckBox(proxyInstance, queryField.FieldName, checkBox, false);

            return checkBox;
        }

        #endregion

        /// <summary>
        /// 绑定外键数据到下拉控件
        /// </summary>
        private static void BindForeignKeyData(
            KryptonComboBox comboBox, 
            QueryField queryField, 
            BaseEntity proxyInstance)
        {
            if (string.IsNullOrEmpty(queryField.FKTableName))
                return;

            var cacheManager = Startup.GetFromFac<IEntityCacheManager>();
            var tableSchema = Startup.GetFromFac<ITableSchemaManager>()?.GetSchemaInfo(queryField.SubQueryTargetType?.Name);

            if (tableSchema == null)
                return;

            var pair = new KeyValuePair<string, string>(tableSchema.PrimaryKeyField, tableSchema.DisplayField);
            Type myType = queryField.SubQueryTargetType;

            if (myType == null)
            {
                MainForm.Instance.logger?.LogError($"外键类型未设置: {queryField.FieldName}");
                return;
            }

            var dbh = new DataBindingHelper();

            if (queryField.SubFilter?.FilterLimitExpressions?.Count == 0)
            {
                // 无限制条件绑定
                var methodInfo = dbh.GetType()
                    .GetMethod("BindData4CmbRef")
                    ?.MakeGenericMethod(new[] { myType });

                methodInfo?.Invoke(dbh, new object[] { proxyInstance, pair.Key, pair.Value, queryField.FKTableName, comboBox });
            }
            else
            {
                // 带限制条件绑定
                ExpConverter expConverter = new ExpConverter();
                var whereExp = expConverter.ConvertToFuncByClassName(
                    myType, 
                    queryField.SubFilter.GetFilterLimitExpression(myType));

                if (pair.Key == queryField.FieldName)
                {
                    var methodInfo = dbh.GetType()
                        .GetMethod("BindData4CmbRefWithLimited")
                        ?.MakeGenericMethod(new[] { myType });

                    methodInfo?.Invoke(dbh, new object[] { proxyInstance, pair.Key, pair.Value, queryField.FKTableName, comboBox, whereExp });
                }
                else
                {
                    var methodInfo = dbh.GetType()
                        .GetMethod("BindData4CmbRefWithLimitedByAlias")
                        ?.MakeGenericMethod(new[] { myType });

                    methodInfo?.Invoke(dbh, new object[] { proxyInstance, pair.Key, queryField.FieldName, pair.Value, queryField.FKTableName, comboBox, whereExp });
                }
            }

            // 初始化过滤按钮
            var initMethodInfo = dbh.GetType()
                .GetMethod("InitFilterForControlRef")
                ?.MakeGenericMethod(new[] { myType });

            initMethodInfo?.Invoke(dbh, new object[] { proxyInstance, comboBox, pair.Value, queryField.SubFilter, null, null, false });
        }

        /// <summary>
        /// 记录性能日志
        /// </summary>
        private static void LogPerformance(string operation, TimeSpan elapsed)
        {
            try
            {
                MainForm.Instance?.uclog?.AddLog("性能", $"{operation}: {elapsed.TotalMilliseconds}ms");
            }
            catch
            {
                // 静默处理日志错误
            }
        }

        #endregion
    }
}
