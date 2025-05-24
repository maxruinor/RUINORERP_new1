﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Windows.Forms;
using Krypton.Toolkit;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using System.ComponentModel;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Extensions;
using System.Reflection;
using System.Text.RegularExpressions;
using RUINORERP.Model;
using System.Collections;
using System.Reflection.Emit;
//using System.Workflow.ComponentModel.Serialization;
using RUINORERP.Common;
using System.Globalization;
using RUINORERP.Model.Base;
using System.Drawing;
using System.IO;
using RUINORERP.UI.AdvancedUIModule;
using RUINORERP.UI.BaseForm;
using System.Drawing.Imaging;
using SqlSugar;
using RUINORERP.Business.Processor;
using RUINOR.WinFormsUI.ChkComboBox;
using HLH.Lib;
using System.Workflow.ComponentModel.Serialization;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using Expression = System.Linq.Expressions.Expression;
using ConstantExpression = System.Linq.Expressions.ConstantExpression;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using FastReport.DevComponents.DotNetBar;
using System.Web.WebSockets;
using RUINORERP.UI.SS;
using System.Windows.Documents;
using static Google.Protobuf.Reflection.FeatureSet.Types;
using System.Windows.Media.Animation;
using Fireasy.Common.Extensions;
using RUINORERP.UI.ChartFramework.Core;
using Image = System.Drawing.Image;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using log4net.Repository.Hierarchy;

namespace RUINORERP.UI.Common
{

    public enum BindDataType4Enum
    {

        /// <summary>
        /// 显示枚举名
        /// </summary>
        EnumName,

        /// <summary>
        /// 显示枚举名的显示特性 [Display(Name ="Permanent")]
        /// </summary>
        EnumDisplay
    }

    public enum BindDataType4TextBox
    {
        Money,
        Text,
        Bool,
        Qty

    }




    public static class ControlBindingHelper
    {
        /// <summary>
        /// 使用表达式树绑定文本框数据并设置标签查询功能
        /// </summary>
        public static void SetupTextBoxDataBinding<T>(object entity,
            Expression<Func<T, object>> valueFieldExpression,
            KryptonTextBox textBox,
            bool enableTwoWayBinding = true,
            bool enableValidation = true)
        {
            textBox.DataBindings.Clear();
            string valueFieldName = valueFieldExpression.GetMemberName();
            SetupTextBoxDataBinding(entity, valueFieldName, textBox, enableTwoWayBinding, enableValidation);
        }

        /// <summary>
        /// 使用字段名称绑定文本框数据并设置标签查询功能
        /// </summary>
        public static void SetupTextBoxDataBinding(object entity, string valueFieldName,
            KryptonTextBox textBox, bool enableTwoWayBinding = true, bool enableValidation = true)
        {
            Binding dataBinding;
            if (enableTwoWayBinding)
            {
                // 双向绑定 - 适用于数据编辑场景
                dataBinding = new Binding("Tag", entity, valueFieldName, true,
                    DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                // 单向绑定 - 适用于只读或显示场景
                dataBinding = new Binding("Tag", entity, valueFieldName, true,
                    enableValidation ? DataSourceUpdateMode.OnValidation : DataSourceUpdateMode.Never);
            }

            dataBinding.BindingComplete += (sender, e) =>
            {
                if (!enableValidation)
                {
                    e.Cancel = false;
                }
            };

            // 格式化数据显示
            dataBinding.Format += (s, args) =>
            {
                args.Value = args.Value ?? string.Empty;
            };

            // 解析用户输入
            dataBinding.Parse += (s, args) =>
            {
                args.Value = args.Value ?? string.Empty;
            };

            textBox.DataBindings.Add(dataBinding);
        }

        /// <summary>
        /// 配置控件的高级过滤查询功能，支持自定义字段映射
        /// </summary>
        public static void ConfigureControlFilter<TEntity, TSource>(
            TEntity entity,
            Control control,
            Expression<Func<TEntity, object>> targetDisplayField,
            Expression<Func<TSource, object>> sourceDisplayField,
            QueryFilter queryFilter,
            Expression<Func<TEntity, object>> targetValueField = null,
            Expression<Func<TSource, object>> sourceValueField = null,
            Type keyValueTypeForDgv = null,
            bool isEditable = false)
            where TEntity : BaseEntity
            where TSource : class
        {
            // 解析表达式树获取字段名称
            string targetDisplayFieldName = targetDisplayField.GetMemberName();
            string sourceDisplayFieldName = sourceDisplayField.GetMemberName();
            string targetValueFieldName = targetValueField?.GetMemberName();
            string sourceValueFieldName = sourceValueField?.GetMemberName();

            // 构建字段映射字典
            var fieldMappings = new Dictionary<string, string>
            {
                [targetDisplayFieldName] = sourceDisplayFieldName
            };

            if (!string.IsNullOrEmpty(targetValueFieldName) && !string.IsNullOrEmpty(sourceValueFieldName))
            {
                fieldMappings[targetValueFieldName] = sourceValueFieldName;
            }

            // 转换映射字典为字符串格式
            string mappingsString = string.Join(";", fieldMappings.Select(kv => $"{kv.Key}:{kv.Value}"));

            // 调用核心过滤设置方法
            SetupEntityFilter<TSource>(
                entity,
                control,
                sourceDisplayFieldName,
                queryFilter,
                keyValueTypeForDgv,
                sourceValueFieldName,
                isEditable,
                mappingsString);
        }

        /// <summary>
        /// 为控件设置实体过滤查询功能
        /// </summary>
        public static void SetupEntityFilter<P>(BaseEntity entity,
            Control control,
            string displayColumn,
            QueryFilter queryFilter,
            Type keyValueTypeForDgv = null,
            string valueFieldColumn = null,
            bool isEditable = false,
            string fieldMappings = null)
            where P : class
        {
            if (control is not VisualControlBase visualControl)
                return;

            // 解析字段映射配置
            var mappings = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(fieldMappings))
            {
                foreach (var pair in fieldMappings.Split(';'))
                {
                    if (string.IsNullOrEmpty(pair)) continue;

                    var parts = pair.Split(':');
                    if (parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                    {
                        mappings[parts[0]] = parts[1];
                    }
                }
            }

            // 处理下拉框控件
            if (visualControl is KryptonComboBox comboBox)
            {
                // 避免重复添加查询按钮
                if (comboBox.ButtonSpecs.Any(b => b.UniqueName == "btnQuery"))
                    return;

                if (comboBox.DataBindings.Count > 0)
                {
                    // 创建查询按钮
                    var queryButton = new ButtonSpecAny
                    {
                        Image = GetEmbeddedResourceImage("help4"),
                        UniqueName = "btnQuery",
                        Tag = comboBox
                    };
                    comboBox.Tag = typeof(P);

                    queryButton.Click += (sender, e) =>
                    {
                        try
                        {
                            // 获取外键表名
                            string foreignKeyTableName = GetForeignKeyTableName(comboBox);
                            var menuInfo = MainForm.Instance.MenuList
                                .FirstOrDefault(t => t.EntityName == foreignKeyTableName);

                            if (menuInfo == null && !isEditable)
                            {
                                MessageBox.Show("您没有执行此操作的权限，请联系管理员。");
                                return;
                            }

                            // 创建并配置查询窗体
                            BaseUControl listControl;
                            if (isEditable)
                            {
                                listControl = Startup.GetFromFacByName<BaseUControl>(menuInfo.FormName);
                                listControl.QueryConditionFilter = queryFilter;
                            }
                            else
                            {
                                var advancedFilter = new UCAdvFilterGeneric<P>
                                {
                                    QueryConditionFilter = queryFilter,
                                    KeyValueTypeForDgv = keyValueTypeForDgv,
                                    ModuleID = menuInfo?.ModuleID ?? 0,
                                    control = control
                                };
                                listControl = advancedFilter;
                            }

                            listControl.Runway = BaseListRunWay.选中模式;

                            using var editForm = new frmBaseEditList
                            {
                                StartPosition = FormStartPosition.CenterScreen
                            };

                            listControl.Dock = DockStyle.Fill;
                            editForm.kryptonPanel1.Controls.Add(listControl);
                            editForm.Text = $"关联查询-{new BizTypeMapper().GetBizType(typeof(P)).ToString()}";

                            // 显示查询窗体并处理选择结果
                            if (editForm.ShowDialog() == DialogResult.OK)
                            {
                                if (listControl.Tag is BindingSource bindingSource && bindingSource.Current != null)
                                {
                                    var selectedItem = bindingSource.Current;
                                    var dataBinding = comboBox.DataBindings[0];
                                    string valueField = dataBinding.BindingMemberInfo.BindingField;

                                    // 应用字段映射
                                    ApplyFieldMappings(mappings, selectedItem, dataBinding.DataSource);

                                    // 更新下拉框数据
                                    var newBindingSource = new BindingSource();
                                    if (TryGetEntityList(typeof(P), queryFilter, out var entityList))
                                    {
                                        newBindingSource.DataSource = entityList;
                                    }
                                    else
                                    {
                                        newBindingSource.DataSource = bindingSource;
                                    }

                                    InitComboBoxData(newBindingSource, valueField, displayColumn, comboBox);
                                    comboBox.SelectedItem = selectedItem;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogError($"查询操作失败: {ex.Message}");
                            MessageBox.Show("查询过程中发生错误，请重试或联系管理员。");
                        }
                    };

                    comboBox.ButtonSpecs.Add(queryButton);
                }
            }

            // 处理文本框控件
            if (visualControl is KryptonTextBox textBox)
            {
                // 避免重复添加查询按钮
                if (textBox.ButtonSpecs.Any(b => b.UniqueName == "btnQuery"))
                    return;

                if (textBox.DataBindings.Count > 0)
                {
                    // 创建查询按钮
                    var queryButton = new ButtonSpecAny
                    {
                        Image = GetEmbeddedResourceImage("help4"),
                        UniqueName = "btnQuery"
                    };
                    textBox.Tag = typeof(P);

                    queryButton.Click += (sender, e) =>
                    {
                        try
                        {
                            // 获取外键表名
                            string foreignKeyTableName = textBox.DataBindings[0].DataSource.GetType().Name;
                            if (foreignKeyTableName.Contains("Proxy"))
                            {
                                foreignKeyTableName = foreignKeyTableName.Replace("Proxy", "");
                            }

                            var menuInfo = MainForm.Instance.MenuList
                                .FirstOrDefault(t => t.EntityName == foreignKeyTableName);

                            // 创建并配置查询控件
                            var advancedFilter = new UCAdvFilterGeneric<P>
                            {
                                QueryConditionFilter = queryFilter,
                                KeyValueTypeForDgv = keyValueTypeForDgv,
                                control = control,
                                ModuleID = menuInfo?.ModuleID ?? 0,
                                Runway = BaseListRunWay.选中模式
                            };

                            using var editForm = new frmBaseEditList
                            {
                                StartPosition = FormStartPosition.CenterScreen
                            };

                            advancedFilter.Dock = DockStyle.Fill;
                            editForm.kryptonPanel1.Controls.Add(advancedFilter);
                            editForm.Text = $"关联查询-{new BizTypeMapper().GetBizType(typeof(P)).ToString()}";

                            // 显示查询窗体并处理选择结果
                            if (editForm.ShowDialog() == DialogResult.OK)
                            {
                                if (advancedFilter.Tag is BindingSource bindingSource && bindingSource.Current != null)
                                {
                                    var selectedItem = bindingSource.Current;
                                    var dataBinding = textBox.DataBindings[0];
                                    string valueField = dataBinding.BindingMemberInfo.BindingField;

                                    // 应用字段映射
                                    if (mappings.Count > 0)
                                    {
                                        ApplyFieldMappings(mappings, selectedItem, dataBinding.DataSource);

                                        // 更新文本框显示
                                        if (mappings.TryGetValue(displayColumn, out var sourceDisplayField))
                                        {
                                            var displayValue = selectedItem.GetPropertyValue(sourceDisplayField);
                                            textBox.Text = displayValue?.ToString() ?? "";
                                        }
                                    }
                                    else
                                    {
                                        // 传统映射逻辑
                                        object selectedValue = string.IsNullOrEmpty(valueFieldColumn)
                                            ? selectedItem.GetPropertyValue(valueField)
                                            : selectedItem.GetPropertyValue(valueFieldColumn);

                                        var displayValue = selectedItem.GetPropertyValue(displayColumn);
                                        textBox.Text = displayValue?.ToString() ?? "";

                                        selectedItem.SetPropertyValue(displayColumn, textBox.Text);
                                        selectedItem.SetPropertyValue(valueField, selectedValue);
                                    }

                                    queryButton.Tag = selectedItem;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.logger.LogError($"查询操作失败: {ex.Message}");
                            MessageBox.Show("查询过程中发生错误，请重试或联系管理员。");
                        }
                    };

                    textBox.ButtonSpecs.Add(queryButton);
                }
            }
        }

        // 辅助方法: 获取嵌入资源中的图像
        private static Image GetEmbeddedResourceImage(string resourceName)
        {
            GetResourceNew(resourceName);
            var resourceStream = GetResourceNew(resourceName);
            return resourceStream != null ? Image.FromStream(resourceStream) : null;
        }
        /// <summary>
        /// 从项目嵌入的资源中获取指定名称的资源流
        /// </summary>
        /// <param name="resourceName">资源名称（不包含扩展名）</param>
        /// <returns>资源流，如果未找到则返回null</returns>
        public static Stream GetResourceNew(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentNullException(nameof(resourceName));

            try
            {
                var image = Properties.Resources.ResourceManager.GetObject(resourceName) as Image;

                if (image == null)
                {
                   MainForm.Instance.logger.LogWarning($"未找到名为 '{resourceName}' 的资源");
                    return null;
                }

                var memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Png); // 使用PNG格式保留透明度
                memoryStream.Position = 0; // 重置流位置

                return memoryStream;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError($"获取资源失败: {ex.Message}", ex);
                return null;
            }
        }
        // 辅助方法: 获取外键表名
        private static string GetForeignKeyTableName(KryptonComboBox comboBox)
        {
            var bindingSource = comboBox.DataSource as BindingSource;
            if (bindingSource == null)
                return comboBox.DataSource.GetType().Name;

            return bindingSource.Current == null
                ? bindingSource.DataSource.GetType().GetGenericArguments()[0].Name
                : bindingSource.Current.GetType().Name;
        }

        // 辅助方法: 尝试获取实体列表
        private static bool TryGetEntityList(Type entityType, QueryFilter queryFilter, out object entityList)
        {
            entityList = null;
            try
            {
                if (queryFilter.FilterLimitExpressions.Count == 0)
                {
                    var cacheList = BizCacheHelper.Manager.CacheEntityList.Get(entityType.Name);
                    if (cacheList != null)
                    {
                        var listType = cacheList.GetType();
                        if (TypeHelper.IsGenericList(listType))
                        {
                            entityList = ((IEnumerable<dynamic>)cacheList).ToList();
                            return true;
                        }
                        if (TypeHelper.IsJArrayList(listType) &&
                            BizCacheHelper.Manager.NewTableTypeList.TryGetValue(entityType.Name, out var elementType))
                        {
                            entityList = TypeHelper.ConvertJArrayToList(elementType, cacheList as JArray);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError($"获取实体列表失败: {ex.Message}");
            }
            return false;
        }

        // 辅助方法: 初始化下拉框数据
        private static void InitComboBoxData(BindingSource bindingSource, string valueField,
            string displayField, KryptonComboBox comboBox)
        {
            comboBox.DataSource = bindingSource;
            comboBox.ValueMember = valueField;
            comboBox.DisplayMember = displayField;
        }

        // 辅助方法: 应用字段映射
        private static void ApplyFieldMappings(Dictionary<string, string> mappings, object source, object target)
        {
            foreach (var mapping in mappings)
            {
                try
                {
                    var sourceValue = source.GetPropertyValue(mapping.Value);
                    target.SetPropertyValue(mapping.Key, sourceValue);
                }
                catch (Exception ex)
                {
                    MainForm.Instance.logger.LogError($"字段映射失败: {mapping.Key}:{mapping.Value}, 错误: {ex.Message}");
                }
            }
        }

        // 扩展方法: 获取表达式树中的成员名称
        public static string GetMemberName<T, TValue>(this Expression<Func<T, TValue>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                return memberExpression.Member.Name;

            if (expression.Body is UnaryExpression unaryExpression &&
                unaryExpression.Operand is MemberExpression operandExpression)
                return operandExpression.Member.Name;

            throw new ArgumentException("表达式不是有效的成员访问表达式");
        }

  

       
    }

    public class DataBindingHelper
    {





        public static List<EnumItem> GetTimeSelectTypeItems(Type enumType)
        {
            var items = new List<EnumItem>();


            foreach (TimeSelectType value in Enum.GetValues(enumType))
            {
                var fieldInfo = enumType.GetField(value.ToString());
                var descriptionAttribute = fieldInfo?
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;

                items.Add(new EnumItem
                {
                    Value = (int)value,
                    Name = value.ToString(),
                    Description = descriptionAttribute?.Description ?? value.ToString()
                });
            }

            return items;
        }

        // 辅助方法：从表达式获取成员名称
        private static string GetMemberName<T, TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpression &&
                unaryExpression.NodeType == ExpressionType.Convert &&
                unaryExpression.Operand is MemberExpression operandMemberExpression)
            {
                return operandMemberExpression.Member.Name;
            }

            throw new ArgumentException("表达式不是有效的成员访问表达式", nameof(expression));
        }

        /// <summary>
        /// 关联查询时带出的快速查询的功能 - 增强版，支持指定源类型和目标类型的字段映射
        /// </summary>
        /// <typeparam name="TEntity">目标实体类型</typeparam>
        /// <typeparam name="TSource">源实体类型</typeparam>
        /// <param name="entity">目标实体</param>
        /// <param name="item">控件</param>
        /// <param name="targetDisplayField">目标实体的显示字段</param>
        /// <param name="sourceDisplayField">源实体的显示字段</param>
        /// <param name="queryFilter">查询过滤器</param>
        /// <param name="targetValueField">目标实体的值字段</param>
        /// <param name="sourceValueField">源实体的值字段</param>
        /// <param name="KeyValueTypeForDgv">视图时使用，显示结果表格时能关联外健的实体</param>
        /// <param name="CanEdit">是否可编辑</param>
        public static void SetupControlFilter<TEntity, TSource>(
            TEntity entity,
            System.Windows.Forms.Control item,
            Expression<Func<TEntity, object>> targetDisplayField,
            Expression<Func<TSource, object>> sourceDisplayField,
            QueryFilter queryFilter,
            Expression<Func<TEntity, object>> targetValueField = null,
            Expression<Func<TSource, object>> sourceValueField = null,
            Type KeyValueTypeForDgv = null,
            bool CanEdit = false)
            where TEntity : BaseEntity
            where TSource : class
        {
            // 获取字段名称
            string targetDisplayFieldName = GetMemberName(targetDisplayField);
            string sourceDisplayFieldName = GetMemberName(sourceDisplayField);
            string targetValueFieldName = targetValueField != null ? GetMemberName(targetValueField) : null;
            string sourceValueFieldName = sourceValueField != null ? GetMemberName(sourceValueField) : null;

            // 构建字段映射
            var fieldMappings = new Dictionary<string, string>();

            // 添加显示字段映射
            fieldMappings[targetDisplayFieldName] = sourceDisplayFieldName;

            // 添加值字段映射
            if (!string.IsNullOrEmpty(targetValueFieldName) && !string.IsNullOrEmpty(sourceValueFieldName))
            {
                fieldMappings[targetValueFieldName] = sourceValueFieldName;
            }

            // 转换为字符串格式
            string fieldMappingsString = string.Join(";", fieldMappings.Select(kv => $"{kv.Key}:{kv.Value}"));

            // 调用原方法，传递字段映射
            InitFilterForControlRefNew<TSource>(
                entity,
                item,
                sourceDisplayFieldName,
                queryFilter,
                KeyValueTypeForDgv,
                sourceValueFieldName,
                CanEdit,
                fieldMappingsString);
        }

        /// <summary>
        /// 关联查询时带出的快速查询的功能
        /// 如果是反射调用 方法名不能用相同的重载的名字。无法识别匹配
        /// !!!注意这个会被dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { mytype });
        /// 参数变化会引用那边异常
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="DisplayCol"></param>
        /// <param name="queryFilter"></param>
        /// <param name="KeyValueTypeForDgv">视图时使用，显示结果表格时能关联外健的实体</param>
        /// <param name="ValueFieldCol">是指源头的ID列名,被引用的实体的ID列</param>
        /// <param name="CanEdit"></param>
        /// <param name="fieldMappings">字段映射配置，格式为："目标字段1:源字段1;目标字段2:源字段2"</param>
        public static void InitFilterForControlRefNew<P>(BaseEntity entity,
            System.Windows.Forms.Control item,
            string DisplayCol,
            QueryFilter queryFilter,
            Type KeyValueTypeForDgv = null, string ValueFieldCol = null,
            bool CanEdit = false,
            string fieldMappings = null) where P : class
        {
            if (item is Control)
            {
                string display = DisplayCol;
                string ValueField = string.Empty;

                // 解析字段映射配置
                var mappings = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(fieldMappings))
                {
                    var mappingPairs = fieldMappings.Split(';');
                    foreach (var pair in mappingPairs)
                    {
                        if (string.IsNullOrEmpty(pair)) continue;

                        var parts = pair.Split(':');
                        if (parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && !string.IsNullOrEmpty(parts[1]))
                        {
                            mappings[parts[0]] = parts[1];
                        }
                    }
                }

                if (item is VisualControlBase)
                {
                    Type targetEntity = typeof(P);
                    if (item.GetType().Name == "KryptonComboBox")
                    {
                        KryptonComboBox ktb = item as KryptonComboBox;
                        //不重复添加
                        if (ktb.ButtonSpecs.Where(b => b.UniqueName == "btnQuery").Any())
                        {
                            return;
                        }

                        if ((item as Control).DataBindings.Count > 0)
                        {
                            ButtonSpecAny bsa = new ButtonSpecAny();
                            bsa.Image = Image.FromStream(GetResource("help4"));
                            bsa.UniqueName = "btnQuery";
                            bsa.Tag = ktb;
                            ktb.Tag = targetEntity;
                            bsa.Click += (sender, e) =>
                            {
                                KryptonComboBox ktbcombo = bsa.Owner as KryptonComboBox;

                                //取外键表名的代码
                                string fktableName = string.Empty;
                                BindingSource bsFKName = new BindingSource();
                                bsFKName = ktb.DataSource as BindingSource;
                                if (bsFKName.Current == null)
                                {
                                    fktableName = bsFKName.DataSource.GetType().GetGenericArguments()[0].Name;
                                }
                                else
                                {
                                    fktableName = bsFKName.Current.GetType().Name;
                                }

                                tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.EntityName == fktableName.ToString());
                                if (menuinfo == null)
                                {
                                    CanEdit = false;
                                    MessageBox.Show("您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                                    return;
                                }

                                BaseUControl ucBaseList = null;
                                if (CanEdit)
                                {
                                    ucBaseList = Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);
                                    ucBaseList.QueryConditionFilter = queryFilter;
                                }
                                else
                                {
                                    UCAdvFilterGeneric<P> ucBaseListAdv = new UCAdvFilterGeneric<P>();
                                    ucBaseListAdv.QueryConditionFilter = queryFilter;
                                    ucBaseListAdv.KeyValueTypeForDgv = KeyValueTypeForDgv;
                                    if (menuinfo != null)
                                    {
                                        ucBaseListAdv.ModuleID = menuinfo.ModuleID.Value;
                                    }
                                    ucBaseListAdv.control = item;
                                    ucBaseList = ucBaseListAdv;
                                }

                                ucBaseList.Runway = BaseListRunWay.选中模式;
                                frmBaseEditList frmedit = new frmBaseEditList();
                                frmedit.StartPosition = FormStartPosition.CenterScreen;
                                ucBaseList.Dock = DockStyle.Fill;
                                frmedit.kryptonPanel1.Controls.Add(ucBaseList);

                                BizTypeMapper mapper = new BizTypeMapper();
                                var BizTypeText = mapper.GetBizType(typeof(P).Name).ToString();
                                frmedit.Text = "关联查询" + "-" + BizTypeText;

                                if (frmedit.ShowDialog() == DialogResult.OK)
                                {
                                    string ucTypeName = bsa.Owner.GetType().Name;
                                    if (ucTypeName == "KryptonComboBox")
                                    {
                                        if (ucBaseList.Tag != null)
                                        {
                                            BindingSource bs = ucBaseList.ListDataSoure as BindingSource;
                                            Binding binding = null;
                                            if (ktbcombo.DataBindings.Count > 0)
                                            {
                                                binding = ktbcombo.DataBindings[0];
                                            }
                                            else
                                            {
                                                MessageBox.Show("没有绑定数据！");
                                                return;
                                            }

                                            //绑定的值字段
                                            ValueField = binding.BindingMemberInfo.BindingField;
                                            if (string.IsNullOrEmpty(ValueField))
                                            {
                                                throw new Exception("ValueField主键字段名不能为空" + ktbcombo.ValueMember);
                                            }
                                            if (bs == null)
                                            {
                                                bs = bsFKName;
                                            }
                                            object selectItem = bs.Current;

                                            // 根据映射关系赋值
                                            if (mappings.Count > 0)
                                            {
                                                foreach (var mapping in mappings)
                                                {
                                                    try
                                                    {
                                                        object sourceValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(selectItem, mapping.Value);
                                                        RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, mapping.Key, sourceValue);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // 记录错误日志
                                                        MainForm.Instance.logger.LogError($"字段映射失败: {mapping.Key}:{mapping.Value}, 错误: {ex.Message}");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // 没有映射配置，使用原有逻辑
                                                object selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(selectItem, ValueField);
                                                try
                                                {
                                                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, ValueField, selectValue);
                                                }
                                                catch (Exception ex)
                                                {
                                                    // 记录错误日志
                                                    MainForm.Instance.logger.LogError($"赋值失败: {ValueField}, 错误: {ex.Message}");
                                                }
                                            }

                                            // 重新加载数据
                                            BindingSource NewBsList = new BindingSource();
                                            var rslist = BizCacheHelper.Manager.CacheEntityList.Get(targetEntity.Name);
                                            if (rslist != null && queryFilter.FilterLimitExpressions.Count == 0)
                                            {
                                                Type listType = rslist.GetType();
                                                if (TypeHelper.IsGenericList(listType))
                                                {
                                                    var lastlist = ((IEnumerable<dynamic>)rslist).ToList();
                                                    NewBsList.DataSource = lastlist;
                                                }
                                                else if (TypeHelper.IsJArrayList(listType))
                                                {
                                                    Type elementType = null;
                                                    BizCacheHelper.Manager.NewTableTypeList.TryGetValue(targetEntity.Name, out elementType);
                                                    List<object> myList = TypeHelper.ConvertJArrayToList(elementType, rslist as JArray);
                                                    NewBsList.DataSource = myList;
                                                }

                                                Common.DataBindingHelper.InitDataToCmb(NewBsList, ValueField, display, ktbcombo);
                                            }
                                            else
                                            {
                                                Common.DataBindingHelper.InitDataToCmb(bs, ValueField, display, ktbcombo);
                                            }

                                            ktbcombo.SelectedItem = selectItem;
                                        }
                                    }
                                }
                            };

                            ktb.ButtonSpecs.Add(bsa);
                        }
                    }

                    if (item.GetType().Name == "KryptonTextBox")
                    {
                        KryptonTextBox ktb = item as KryptonTextBox;
                        //不重复添加
                        if (ktb.ButtonSpecs.Count > 0)
                        {
                            return;
                        }
                        if ((item as Control).DataBindings.Count > 0)
                        {
                            ButtonSpecAny bsa = new ButtonSpecAny();
                            bsa.Image = Image.FromStream(GetResource("help4"));
                            bsa.UniqueName = "btnQuery";
                            ktb.Tag = targetEntity;

                            bsa.Click += (sender, e) =>
                            {
                                string fktableName = string.Empty;
                                BindingSource bsFKName = new BindingSource();
                                fktableName = ktb.DataBindings[0].DataSource.GetType().Name;
                                if (fktableName.Contains("Proxy"))
                                {
                                    fktableName = fktableName.Replace("Proxy", "");
                                }
                                tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.EntityName == fktableName.ToString());
                                if (menuinfo == null)
                                {
                                    CanEdit = false;
                                }

                                KryptonTextBox ktbTxt = bsa.Owner as KryptonTextBox;
                                UCAdvFilterGeneric<P> ucBaseList = new UCAdvFilterGeneric<P>();
                                ucBaseList.QueryConditionFilter = queryFilter;
                                ucBaseList.KeyValueTypeForDgv = KeyValueTypeForDgv;
                                ucBaseList.control = item;
                                ucBaseList.ModuleID = menuinfo.ModuleID.Value;
                                ucBaseList.Runway = BaseListRunWay.选中模式;

                                frmBaseEditList frmedit = new frmBaseEditList();
                                frmedit.StartPosition = FormStartPosition.CenterScreen;
                                ucBaseList.Dock = DockStyle.Fill;
                                frmedit.kryptonPanel1.Controls.Add(ucBaseList);

                                BizTypeMapper mapper = new BizTypeMapper();
                                var BizTypeText = mapper.GetBizType(typeof(P).Name).ToString();
                                frmedit.Text = "关联查询" + "-" + BizTypeText;

                                if (frmedit.ShowDialog() == DialogResult.OK)
                                {
                                    string ucTypeName = bsa.Owner.GetType().Name;
                                    if (ucTypeName == "KryptonTextBox")
                                    {
                                        if (ucBaseList.Tag != null)
                                        {
                                            BindingSource bs = ucBaseList.Tag as BindingSource;
                                            Binding binding = ktbTxt.DataBindings[0];
                                            ValueField = binding.BindingMemberInfo.BindingField;

                                            object selectItem = bs.Current;

                                            // 根据映射关系赋值
                                            if (mappings.Count > 0)
                                            {
                                                foreach (var mapping in mappings)
                                                {
                                                    try
                                                    {
                                                        object sourceValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(selectItem, mapping.Value);
                                                        RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, mapping.Key, sourceValue);

                                                        // 如果是显示字段，同时更新文本框
                                                        //if (mapping.Key.Equals(display, StringComparison.OrdinalIgnoreCase))
                                                        //{
                                                        ktbTxt.Text = sourceValue?.ToString() ?? "";
                                                        //}
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // 记录错误日志
                                                        MainForm.Instance.logger.LogError($"字段映射失败: {mapping.Key}:{mapping.Value}, 错误: {ex.Message}");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // 没有映射配置，使用原有逻辑
                                                object selectValue = string.Empty;
                                                if (ValueFieldCol.IsNotEmptyOrNull())
                                                {
                                                    selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(bs.Current, ValueFieldCol);
                                                    ValueField = ValueFieldCol;
                                                }
                                                else
                                                {
                                                    selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(bs.Current, ValueField);
                                                }
                                                object displayObj = bs.Current.GetPropertyValue(display);
                                                if (displayObj != null)
                                                {
                                                    ktbTxt.Text = displayObj.ToString();
                                                }
                                                else
                                                {
                                                    MessageBox.Show($"没有获取到显示字段值{display}，请联系管理员。");
                                                    MainForm.Instance.logger.LogInformation($"没有获取到显示字段值{display}，请联系管理员。  ");
                                                    return;
                                                }

                                                RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, display, ktbTxt.Text);
                                                RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, ValueField, selectValue);
                                            }

                                            bsa.Tag = bs.Current;
                                        }
                                    }
                                }
                            };

                            ktb.ButtonSpecs.Add(bsa);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 感叹号的快速查询功能
        /// 关联查询时带出的快速查询的功能
        /// 注意【引用与被引用的 编号和ID字段在两个表中要一致】
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <typeparam name="Dto"></typeparam>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="DisplayColExp"></param>
        /// <param name="whereLambda">额外限制性条件，如供应商不会显示到销售订单下</param>
        /// <param name="KeyValueTypeForDgv">视图时使用，显示结果表格时能关联外健的实体</param>
        /// <param name="QueryConditions"></param>
        /// <param name="ValueFieldCol">是指源头的ID列名,被引用的实体的ID列</param>
        public static void InitFilterForControlByExp<P>(BaseEntity entity, System.Windows.Forms.Control item, Expression<Func<P, object>> DisplayColExp, QueryFilter queryFilter, Type KeyValueTypeForDgv = null, Expression<Func<P, object>> ValueFieldColExp = null) where P : class
        {
            // 移除事件处理程序
            item.Validating -= textBox1_Validating;
            if (ValueFieldColExp == null)
            {
                InitFilterForControlRef<P>(entity, item, DisplayColExp.GetMemberInfo().Name, queryFilter, KeyValueTypeForDgv);
            }
            else
            {
                InitFilterForControlRef<P>(entity, item, DisplayColExp.GetMemberInfo().Name, queryFilter, KeyValueTypeForDgv, ValueFieldColExp.GetMemberInfo().Name);
            }
        }
        private static void textBox1_Validating(object sender, CancelEventArgs e)
        {
            // 不设置 e.Cancel = true，允许焦点移开
        }

        /// <summary>
        /// 关联可以查询带出。也可以添加的下拉框
        /// 一般用于外键的单表
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="DisplayColExp"></param>
        /// <param name="queryFilter"></param>
        /// <param name="KeyValueTypeForDgv"></param>
        /// <param name="ValueFieldColExp"></param>
        /// <param name="CanEdit"></param>
        public static void InitFilterForControlByExpCanEdit<P>(BaseEntity entity, Control item,
            Expression<Func<P, object>> DisplayColExp, QueryFilter queryFilter,
            bool CanEdit, Type KeyValueTypeForDgv = null,
            Expression<Func<P, object>> ValueFieldColExp = null) where P : class
        {
            if (ValueFieldColExp == null)
            {
                InitFilterForControlRef<P>(entity, item, DisplayColExp.GetMemberInfo().Name, queryFilter, KeyValueTypeForDgv, null, CanEdit);
            }
            else
            {
                InitFilterForControlRef<P>(entity, item, DisplayColExp.GetMemberInfo().Name, queryFilter, KeyValueTypeForDgv, ValueFieldColExp.GetMemberInfo().Name, CanEdit);
            }
        }


        /// <summary>
        /// 关联查询时带出的快速查询的功能
        /// 如果是反射调用 方法名不能用相同的重载的名字。无法识别匹配
        /// !!!注意这个会被dbh.GetType().GetMethod("InitFilterForControlRef").MakeGenericMethod(new Type[] { mytype });
        /// 参数变化会引用那边异常
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <typeparam name="Dto"></typeparam>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        /// <param name="DisplayColExp"></param>
        /// <param name="whereLambda">额外限制性条件，如供应商不会显示到销售订单下</param>
        /// <param name="KeyValueTypeForDgv">视图时使用，显示结果表格时能关联外健的实体</param>
        /// <param name="QueryConditions"></param>
        /// <param name="ValueFieldCol">是指源头的ID列名,被引用的实体的ID列</param>
        public static void InitFilterForControlRef<P>(BaseEntity entity, System.Windows.Forms.Control item, string DisplayCol, QueryFilter queryFilter,
            Type KeyValueTypeForDgv = null, string ValueFieldCol = null, bool CanEdit = false) where P : class
        {
            if (item is Control)
            {
                string display = DisplayCol;
                string ValueField = string.Empty;
                if (item is VisualControlBase)
                {
                    Type targetEntity = typeof(P);
                    if (item.GetType().Name == "KryptonComboBox")
                    {
                        KryptonComboBox ktb = item as KryptonComboBox;
                        //不重复添加
                        if (ktb.ButtonSpecs.Where(b => b.UniqueName == "btnQuery").Any())
                        {
                            return;
                        }

                        if ((item as Control).DataBindings.Count > 0)
                        {
                            //if (ktb.DataBindings.Count > 0 && ktb.DataSource is BindingSource)
                            //{
                            ButtonSpecAny bsa = new ButtonSpecAny();
                            bsa.Image = Image.FromStream(GetResource("help4"));
                            bsa.UniqueName = "btnQuery";
                            bsa.Tag = ktb;
                            ktb.Tag = targetEntity;
                            bsa.Click += (sender, e) =>
                            {
                                #region
                                KryptonComboBox ktbcombo = bsa.Owner as KryptonComboBox;

                                //取外键表名的代码
                                string fktableName = string.Empty;
                                BindingSource bsFKName = new BindingSource();
                                bsFKName = ktb.DataSource as BindingSource;//这个是对应的是主体实体
                                if (bsFKName.Current == null)
                                {
                                    fktableName = bsFKName.DataSource.GetType().GetGenericArguments()[0].Name;
                                }
                                else
                                {
                                    fktableName = bsFKName.Current.GetType().Name;//这个会出错，current 可能会为空。
                                }
                                //这里调用权限判断
                                //调用通用的查询编辑基础资料。
                                //需要对应的类名，如果修改新增了数据要重新加载下拉数据
                                tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.EntityName == fktableName.ToString());
                                if (menuinfo == null)
                                {
                                    //MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                                    //没有权限就只能查询。有权限就可以新增？
                                    CanEdit = false;
                                    MessageBox.Show("您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                                    return;
                                }

                                //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
                                BaseUControl ucBaseList = null;
                                if (CanEdit)
                                {
                                    //编辑模式
                                    ucBaseList = Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);
                                    ucBaseList.QueryConditionFilter = queryFilter;
                                }
                                else
                                {
                                    UCAdvFilterGeneric<P> ucBaseListAdv = new UCAdvFilterGeneric<P>();
                                    ucBaseListAdv.QueryConditionFilter = queryFilter;
                                    ucBaseListAdv.KeyValueTypeForDgv = KeyValueTypeForDgv;
                                    if (menuinfo != null)
                                    {
                                        ucBaseListAdv.ModuleID = menuinfo.ModuleID.Value;
                                    }
                                    ucBaseListAdv.control = item;
                                    ucBaseList = ucBaseListAdv;
                                }


                                ucBaseList.Runway = BaseListRunWay.选中模式;
                                //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
                                frmBaseEditList frmedit = new frmBaseEditList();
                                frmedit.StartPosition = FormStartPosition.CenterScreen;
                                ucBaseList.Dock = DockStyle.Fill;
                                frmedit.kryptonPanel1.Controls.Add(ucBaseList);

                                BizTypeMapper mapper = new BizTypeMapper();
                                var BizTypeText = mapper.GetBizType(typeof(P).Name).ToString();
                                frmedit.Text = "关联查询" + "-" + BizTypeText;

                                if (frmedit.ShowDialog() == DialogResult.OK)
                                {
                                    string ucTypeName = bsa.Owner.GetType().Name;
                                    if (ucTypeName == "KryptonComboBox")
                                    {
                                        //选中的值，一定要在重新加载前保存，下面会清空重新加载会变为第一个项
                                        if (ucBaseList.Tag != null)
                                        {
                                            //来自查询的数据源和选中值
                                            BindingSource bs = ucBaseList.ListDataSoure as BindingSource;

                                            //控件加载时绑定信息
                                            Binding binding = null;
                                            if (ktbcombo.DataBindings.Count > 0)
                                            {
                                                binding = ktbcombo.DataBindings[0]; //这个是下拉绑定的实体集合
                                                                                    //string filedName = binding.BindingMemberInfo.BindingField;
                                            }
                                            else
                                            {
                                                MessageBox.Show("没有绑定数据！");
                                                return;
                                            }
                                            //绑定的值字段
                                            ValueField = binding.BindingMemberInfo.BindingField;
                                            if (string.IsNullOrEmpty(ValueField))
                                            {
                                                throw new Exception("ValueField主键字段名不能为空" + ktbcombo.ValueMember);
                                            }
                                            if (bs == null)
                                            {
                                                bs = bsFKName;
                                            }
                                            object selectItem = bs.Current;
                                            object selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(selectItem, ValueField);
                                            //从缓存中重新加载 
                                            BindingSource NewBsList = new BindingSource();
                                            //将List<T>类型的结果是object的转换为指定类型的List
                                            //var lastlist = ((IEnumerable<dynamic>)rslist).Select(item => Activator.CreateInstance(mytype)).ToList();
                                            //有缓存的情况
                                            var rslist = BizCacheHelper.Manager.CacheEntityList.Get(targetEntity.Name);
                                            //条件如果有限制了。就不能全部加载
                                            if (rslist != null && queryFilter.FilterLimitExpressions.Count == 0)
                                            {
                                                Type listType = rslist.GetType();
                                                if (TypeHelper.IsGenericList(listType))
                                                {
                                                    var lastlist = ((IEnumerable<dynamic>)rslist).ToList();

                                                    NewBsList.DataSource = lastlist;
                                                }
                                                else if (TypeHelper.IsJArrayList(listType))
                                                {
                                                    // Type elementType = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + targetEntity.Name);

                                                    Type elementType = null;
                                                    BizCacheHelper.Manager.NewTableTypeList.TryGetValue(targetEntity.Name, out elementType);

                                                    List<object> myList = TypeHelper.ConvertJArrayToList(elementType, rslist as JArray);

                                                    #region  jsonlist
                                                    NewBsList.DataSource = myList;
                                                    #endregion
                                                }

                                                Common.DataBindingHelper.InitDataToCmb(NewBsList, ValueField, display, ktbcombo);

                                            }
                                            else
                                            {
                                                //单据类没有缓存 并且开始没有绑定有数据的数据源，这就重新绑定一下
                                                // ktb.DataBindings.Clear();
                                                //Common.DataBindingHelper.BindData4Cmb(bs, bs.DataSource, ValueField, display, ktb);
                                                //会修改当前选择的项
                                                Common.DataBindingHelper.InitDataToCmb(bs, ValueField, display, ktbcombo);
                                            }
                                            try
                                            {
                                                RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, ValueField, selectValue);
                                            }
                                            catch (Exception ex)
                                            {

                                            }

                                            ktbcombo.SelectedItem = selectItem;
                                        }
                                    }

                                }

                                #endregion
                            };


                            ktb.ButtonSpecs.Add(bsa);
                        }
                    }

                    if (item.GetType().Name == "KryptonTextBox")
                    {
                        KryptonTextBox ktb = item as KryptonTextBox;
                        //不重复添加
                        if (ktb.ButtonSpecs.Count > 0)
                        {
                            return;
                        }
                        if ((item as Control).DataBindings.Count > 0)
                        {
                            //if (ktb.DataBindings.Count > 0 && ktb.DataSource is BindingSource)
                            //{
                            ButtonSpecAny bsa = new ButtonSpecAny();
                            bsa.Image = Image.FromStream(GetResource("help4"));
                            // bsa.Tag = ktb;
                            bsa.UniqueName = "btnQuery";
                            ktb.Tag = targetEntity;

                            //bsa.Click += BsaEdit_Click;
                            bsa.Click += (sender, e) =>
                            {
                                //取外键表名的代码
                                string fktableName = string.Empty;
                                BindingSource bsFKName = new BindingSource();
                                fktableName = ktb.DataBindings[0].DataSource.GetType().Name;//这个是对应的是主体实体
                                //if (bsFKName.Current == null)
                                //{
                                //    fktableName = bsFKName.DataSource.GetType().GetGenericArguments()[0].Name;
                                //}
                                //else
                                //{
                                //    fktableName = bsFKName.Current.GetType().Name;//这个会出错，current 可能会为空。
                                //}
                                //这里调用权限判断
                                //调用通用的查询编辑基础资料。
                                //需要对应的类名，如果修改新增了数据要重新加载下拉数据
                                if (fktableName.Contains("Proxy"))
                                {
                                    fktableName = fktableName.Replace("Proxy", "");
                                }
                                tb_MenuInfo menuinfo = MainForm.Instance.MenuList.FirstOrDefault(t => t.EntityName == fktableName.ToString());
                                if (menuinfo == null)
                                {
                                    //MainForm.Instance.PrintInfoLog("菜单关联类型为空,或您没有执行此菜单的权限，或配置菜时参数不正确。请联系管理员。");
                                    //没有权限就只能查询。有权限就可以新增？
                                    CanEdit = false;
                                }

                                #region
                                KryptonTextBox ktbTxt = bsa.Owner as KryptonTextBox;
                                //暂时认为基础数据都是这个基类出来的 否则可以根据菜单中的基类类型来判断生成
                                UCAdvFilterGeneric<P> ucBaseList = new UCAdvFilterGeneric<P>(); // Startup.GetFromFacByName<BaseUControl>(menuinfo.FormName);

                                ucBaseList.QueryConditionFilter = queryFilter;
                                ucBaseList.KeyValueTypeForDgv = KeyValueTypeForDgv;
                                ucBaseList.control = item;
                                ucBaseList.ModuleID = menuinfo.ModuleID.Value;
                                ucBaseList.Runway = BaseListRunWay.选中模式;
                                //从这里调用 就是来自于关联窗体，下面这个公共基类用于这个情况。暂时在那个里面来控制.Runway = BaseListRunWay.窗体;
                                frmBaseEditList frmedit = new frmBaseEditList();
                                frmedit.StartPosition = FormStartPosition.CenterScreen;
                                ucBaseList.Dock = DockStyle.Fill;
                                frmedit.kryptonPanel1.Controls.Add(ucBaseList);


                                BizTypeMapper mapper = new BizTypeMapper();
                                var BizTypeText = mapper.GetBizType(typeof(P).Name).ToString();
                                frmedit.Text = "关联查询" + "-" + BizTypeText;

                                if (frmedit.ShowDialog() == DialogResult.OK)
                                {
                                    string ucTypeName = bsa.Owner.GetType().Name;
                                    if (ucTypeName == "KryptonTextBox")
                                    {
                                        //选中的值，一定要在重新加载前保存，下面会清空重新加载会变为第一个项
                                        if (ucBaseList.Tag != null)
                                        {
                                            //来自查询的数据源和选中值
                                            BindingSource bs = ucBaseList.Tag as BindingSource;
                                            //控件加载时绑定信息
                                            Binding binding = null;
                                            binding = ktbTxt.DataBindings[0]; //这个是下拉绑定的实体集合
                                                                              //绑定的值字段
                                            ValueField = binding.BindingMemberInfo.BindingField;
                                            BindingSource NewBsList = new BindingSource();
                                            object selectValue = string.Empty;
                                            if (ValueFieldCol.IsNotEmptyOrNull())
                                            {
                                                selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(bs.Current, ValueFieldCol);
                                                ValueField = ValueFieldCol;
                                            }
                                            else
                                            {
                                                selectValue = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(bs.Current, ValueField);
                                            }
                                            object displayObj = bs.Current.GetPropertyValue(display);
                                            if (displayObj != null)
                                            {
                                                ktbTxt.Text = displayObj.ToString();
                                            }
                                            else
                                            {
                                                MessageBox.Show("没有获取到显示字段值{display}，请联系管理员。");
                                                MainForm.Instance.logger.LogInformation($"没有获取到显示字段值{display}，请联系管理员。  ");
                                                return;
                                            }

                                            RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, display, ktbTxt.Text);
                                            RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(binding.DataSource, ValueField, selectValue);
                                            bsa.Tag = bs.Current;

                                        }
                                    }

                                }

                                #endregion

                            };


                            ktb.ButtonSpecs.Add(bsa);
                        }
                    }


                }
            }
        }





        /// <summary>
        /// 从项目嵌入的资源中读取指定的资源文件。
        /// </summary>
        /// <param name="filename">指定的资源文件名称</param>
        /// <returns>返回的资源文件流</returns>
        public static Stream GetResource(string filename)
        {
            Stream istr = null;
            Properties.Resources res = new Properties.Resources();
            PropertyInfo[] properInfo = res.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (PropertyInfo item in properInfo)
            {
                // 获取指定文件名格式的图片
                //if (item.Name.Contains(filename) && item.PropertyType.Name == "Bitmap")
                if (item.Name.Contains(filename))
                {
                    Bitmap b = (Bitmap)Properties.Resources.ResourceManager.GetObject(item.Name, Properties.Resources.Culture);
                    MemoryStream stream = new MemoryStream();
                    b.Save(stream, ImageFormat.Jpeg);
                    byte[] data = new byte[stream.Length];
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    // do someting..
                    istr = stream;
                    break;

                }
            }
            return istr;
        }

       
        public static void BindData4RadioGroupTrueFalse<T>(object entity, Expression<Func<T, bool?>> exp, KryptonRadioButton RadioButton1, KryptonRadioButton RadioButton2)
        {
            MemberInfo minfo = exp.GetMemberInfo();
            string key = minfo.Name;
            BindData4RadioGroupTrueFalse(entity, key, RadioButton1, RadioButton2);
        }

        public static void BindData4RadioGroupTrueFalse(object entity, string key, KryptonRadioButton RadioButton1, KryptonRadioButton RadioButton2)
        {
            RadioButton1.DataBindings.Clear();
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
            }
            // Set initial values
            RadioButton1.Checked = bool.Parse(value);
            RadioButton2.Checked = !bool.Parse(value);
            // Change on event
            RadioButton1.CheckedChanged += delegate
            {
                ReflectionHelper.SetPropertyValue(entity, key, RadioButton1.Checked);
            };
            //RadioButton2.CheckedChanged += delegate
            //{
            //ReflectionHelper.SetPropertyValue(entity, key, RadioButton2.Checked);
            //};

            Binding binddata = null;
            binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            // binddata.Format += (s, args) => args.Value = ((string)args.Value) == RadioButton1.Text;
            //binddata.Parse += (s, args) => args.Value = (bool)args.Value ? RadioButton1.Text : RadioButton2.Text;
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            RadioButton1.DataBindings.Add(binddata);
            //RadioButton2.DataBindings.Add(binddata);
        }


        public static void BindData4RadioGroupTrueFalse<T>(object entity, Expression<Func<T, bool?>> exp, RadioButton RadioButton1, RadioButton RadioButton2)
        {
            MemberInfo minfo = exp.GetMemberInfo();
            string key = minfo.Name;
            BindData4RadioGroupTrueFalse(entity, key, RadioButton1, RadioButton2);
        }

        public static void BindData4RadioGroupTrueFalse(object entity, string key, RadioButton RadioButton1, RadioButton RadioButton2)
        {
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
            }
            // Set initial values
            RadioButton1.Checked = bool.Parse(value);
            RadioButton2.Checked = !bool.Parse(value);
            // Change on event
            RadioButton1.CheckedChanged += delegate
            {
                ReflectionHelper.SetPropertyValue(entity, key, RadioButton1.Checked);
            };
            //RadioButton2.CheckedChanged += delegate
            //{
            //ReflectionHelper.SetPropertyValue(entity, key, RadioButton2.Checked);
            //};

            Binding binddata = null;
            binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            // binddata.Format += (s, args) => args.Value = ((string)args.Value) == RadioButton1.Text;
            //binddata.Parse += (s, args) => args.Value = (bool)args.Value ? RadioButton1.Text : RadioButton2.Text;
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            RadioButton1.DataBindings.Add(binddata);
            //RadioButton2.DataBindings.Add(binddata);
        }


        public static void BindData4CmbByDictionary<T>(object entity, Expression<Func<T, int?>> expkey, Dictionary<int, string> valuePairs, KryptonComboBox cmbBox, bool addSelect)
        {
            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string keyName = minfo.Name;
            cmbBox.Tag = keyName;

            InitDataToCmbByDictionaryGeneratedDataSource(valuePairs, keyName, cmbBox, addSelect);

            var depa = new Binding("SelectedValue", entity, keyName, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }
        /// <summary>
        /// 枚举名称要与DB表中的字段名相同
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="enumTypeName"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmbByDictionaryGeneratedDataSource(Dictionary<int, string> valuePairs, string keyName, KryptonComboBox cmbBox, bool addSelect)
        {

            TypeConfig typeConfig = new TypeConfig();
            typeConfig.FullName = "InitDataToCmbByDictionaryGeneratedDataSource";//只是一个名称而已

            //要创建的属性
            PropertyConfig propertyConfigKey = new PropertyConfig();
            propertyConfigKey.PropertyName = keyName;// type.Name;默认枚举名改为可以指定名
            propertyConfigKey.PropertyType = typeof(int);//枚举值为int 默认

            PropertyConfig propertyConfigName = new PropertyConfig();
            propertyConfigName.PropertyName = "Name";
            propertyConfigName.PropertyType = typeof(string);

            typeConfig.Properties.Add(propertyConfigKey);
            typeConfig.Properties.Add(propertyConfigName);
            Type newType = TypeBuilderHelper.BuildType(typeConfig);

            List<object> list = new List<object>();

            int currentValue;
            string currentName;
            foreach (var item in valuePairs)
            {
                object eobj = Activator.CreateInstance(newType);
                currentValue = item.Key;
                currentName = item.Value;
                eobj.SetPropertyValue(keyName, currentValue);
                eobj.SetPropertyValue("Name", currentName);
                list.Add(eobj);
            }

            if (addSelect)
            {
                object sobj = Activator.CreateInstance(newType);
                sobj.SetPropertyValue(keyName, -1);
                sobj.SetPropertyValue("Name", "请选择");
                list.Insert(0, sobj);
            }

            cmbBox.SelectedValue = -1;

            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            ComboBoxHelper.InitDropList(bs, cmbBox, keyName, "Name", ComboBoxStyle.DropDown, false);

        }


        /// <summary>
        /// by watson 2025-4-3
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="enumType"></param>
        /// <param name="cmbBox"></param>
        /// <param name="addSelect"></param>
        public static void BindData4CmbByEnum<T>(object entity, Expression<Func<T, int>> expkey, Type enumType, KryptonComboBox cmbBox, bool addSelect)
        {
            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            BindData4CmbByEnum<T>(entity, key, enumType, cmbBox, addSelect);
        }

        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnum<T>(object entity, Expression<Func<T, int?>> expkey, Type enumType, KryptonComboBox cmbBox, bool addSelect)
        {
            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            BindData4CmbByEnum<T>(entity, key, enumType, cmbBox, addSelect);
        }
        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnum<T>(object entity, string keyName, Type enumType, KryptonComboBox cmbBox, bool addSelect)
        {
            cmbBox.Tag = keyName;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, keyName, cmbBox, addSelect);

            var depa = new Binding("SelectedValue", entity, keyName, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);

        }

        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnumRef(object entity, string keyName, Type enumType, KryptonComboBox cmbBox, bool addSelect)
        {
            cmbBox.Tag = keyName;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, keyName, cmbBox, addSelect);

            var depa = new Binding("SelectedValue", entity, keyName, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);

        }


        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnumData<T>(object entity, Expression<Func<T, int?>> expkey, KryptonComboBox cmbBox)
        {
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            cmbBox.Tag = key;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        /// <summary>
        /// 绑定枚举类型
        /// </summary>
        /// <typeparam name="T">数据库对应该表实体名</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="defaultExp">默认值的</param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEnum<T>(Type enumType, object entity, Expression<Func<T, int?>> expkey, Func<object, object> defaultExp, KryptonComboBox cmbBox, bool addSelect)
        {
            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, key, cmbBox, addSelect);

            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
            cmbBox.SelectedValue = defaultExp;
        }


        /// <summary>
        /// 绑定数据到UI，只传入了id
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey">显示名</param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbByEntity<T>(object entity, Expression<Func<T, long>> expkey, KryptonComboBox cmbBox) where T : class
        {
            cmbBox.DataBindings.Clear();
            InitDataToCmb<T>(expkey, cmbBox);
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }


        #region  绑定数据到UI,下拉列表的主键在引用表中字段不一致的时候使用
        /// <summary>
        /// 这个绑定下拉时，比方在产品表中。T会为tb_unit，expkey为unit_id，expValue为unit_name，Unit_ID会作为外键保存在产品中。
        /// 如果外键列名和单位表本身主键ID不一致时会出错。
        /// 这个方法是为了解决上述问题
        /// <typeparamref name="T1"/>单位（引用表）<typeparamref name="T1"/>
        /// <typeparamref name="T2"/>产品表（主表）<typeparamref name="T2"/>
        /// </summary>
        public static void BindData4Cmb<T1, T2>(object entity, Expression<Func<T1, long>> expkey, Expression<Func<T1, string>> expValue,
            Expression<Func<T2, long>> RefExpkey, KryptonComboBox cmbBox, bool SyncUI, Expression<Func<T1, bool>> expCondition = null) where T1 : class where T2 : class
        {
            cmbBox.DataBindings.Clear();
            InitDataToCmb<T1>(expkey, expValue, cmbBox, expCondition);
            // InitDataToCmb<T1>(expkey, expValue, cmbBox);
            MemberInfo minfo = RefExpkey.GetMemberInfo();
            string key = minfo.Name;
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            }



            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }



        #endregion




        /// <summary>
        /// 这个绑定下拉时，比方在产品表中。T会为tb_unit，expkey为unit_id，expValue为unit_name，Unit_ID会作为外键保存在产品中。
        /// 如果外键列名和单位表本身主键ID不一致时会出错。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox) where T : class
        {
            BindData4Cmb<T>(entity, expkey, expValue, cmbBox, false);
        }



        /// <summary>
        /// 绑定数据到UI，要求引用的外键名要和表ID名一致
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey">显示名</param>
        /// <param name="expValue">id</param>
        /// <param name="cmbBox"></param>
        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox, bool SyncUI) where T : class
        {
            cmbBox.DataBindings.Clear();
            InitDataToCmb<T>(expkey, expValue, cmbBox);
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            }

            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        /// <summary>
        /// 绑定数据到UI，要求引用的外键名要和表ID名一致
        /// 只是关联不初始下拉数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey">显示名</param>
        /// <param name="expValue">id</param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbRelated<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox, bool SyncUI, bool NeedInitData) where T : class
        {
            if (NeedInitData)
            {
                InitDataToCmb<T>(expkey, expValue, cmbBox);
            }

            cmbBox.DataBindings.Clear();
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            }



            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }


        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {
            cmbBox.DataBindings.Clear();

            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();
            queryFilter.FilterLimitExpressions.Add(expCondition);

            InitDataToCmb<T>(expkey, expValue, cmbBox, queryFilter.GetFilterExpression<T>());
            MemberInfo minfo = expkey.GetMemberInfo();
            string key = minfo.Name;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);

            #region 添加清除功能
            //不重复添加
            if (cmbBox.ButtonSpecs.Where(b => b.UniqueName == "btnclear").Any())
            {
                return;
            }
            if ((cmbBox as Control).DataBindings.Count > 0)
            {
                if (cmbBox.DataBindings.Count > 0 && cmbBox.DataSource is BindingSource)
                {
                    ButtonSpecAny bsa = new ButtonSpecAny();
                    // bsa.Image = Image.FromStream(GetResource("RUINORERP.UI.ResourceFile.search.ico"));
                    // bsa.Tag = ktb;
                    // cmbBox.Tag = targetEntity;
                    bsa.UniqueName = "btnclear";
                    bsa.Type = PaletteButtonSpecStyle.Close;
                    bsa.Edge = PaletteRelativeEdgeAlign.Near;
                    //bsa.Click += BsaEdit_Click;
                    bsa.Click += (sender, e) =>
                    {
                        #region
                        cmbBox.DataSource = cmbBox.DataSource;
                        cmbBox.Text = "";
                        InitDataToCmb<T>(expkey, expValue, cmbBox, expCondition);
                        cmbBox.SelectedItem = null;
                        try
                        {
                            entity.SetPropertyValue(key, null);
                            //entity.SetPropertyValue(key, DBNull.Value);
                        }
                        catch
                        {
                        }
                        cmbBox.SelectedIndex = -1;
                        cmbBox.SelectedValue = -1;
                        //如果字段值能允许null。直接设置一下。不然验证框架会验证。导致 要重新开UI窗体

                        Type type = typeof(T);
                        PropertyInfo fieldinfo = type.GetProperty(key);
                        if (fieldinfo != null)
                        {
                            var propertyType = fieldinfo.PropertyType;
                            if (Nullable.GetUnderlyingType(propertyType) != null)
                            {
                                //entity是被引用的类。要设置的是本身。这里无效。
                                entity.SetPropertyValue(key, null);
                            }
                        }
                        #endregion

                    };
                    cmbBox.ButtonSpecs.Add(bsa);
                }
            }
            #endregion
        }

        public static void BindData4Cmb<T>(object entity, Expression<Func<T, long>> expkey, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox, Expression<Func<T, bool>> expCondition, bool WithClear)
        {
            BindData4Cmb<T>(entity, expkey, expValue, cmbBox, expCondition);
        }

        /// <summary>
        /// 为了能用Type 当T 调用，反射，方法名另取
        /// 这个用于 自动生成的介面 如查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="tableName"></param>
        /// <param name="cmbBox"></param>
        public static void BindData4CmbRef<T>(object entity, string expkey, string expValue, string tableName, KryptonComboBox cmbBox) where T : class
        {
            InitDataToCmb<T>(expkey, expValue, tableName, cmbBox);
            string key = expkey;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }



        public static void BindData4CmbRefWithLimited<T>(object entity, string expkey, string expValue, string tableName, KryptonComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {
            cmbBox.DataBindings.Clear();
            InitDataToCmbWithCondition<T>(expkey, expValue, tableName, cmbBox, expCondition);
            string key = expkey;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);

            #region 添加清除功能
            //不重复添加
            if (cmbBox.ButtonSpecs.Where(b => b.UniqueName == "btnclear").Any())
            {
                return;
            }
            if ((cmbBox as Control).DataBindings.Count > 0)
            {
                if (cmbBox.DataBindings.Count > 0 && cmbBox.DataSource is BindingSource)
                {
                    ButtonSpecAny bsa = new ButtonSpecAny();
                    // bsa.Image = Image.FromStream(GetResource("RUINORERP.UI.ResourceFile.search.ico"));
                    // bsa.Tag = ktb;
                    // cmbBox.Tag = targetEntity;
                    bsa.UniqueName = "btnclear";
                    bsa.Type = PaletteButtonSpecStyle.Close;
                    bsa.Edge = PaletteRelativeEdgeAlign.Near;
                    //bsa.Click += BsaEdit_Click;
                    bsa.Click += (sender, e) =>
                    {
                        #region
                        cmbBox.DataSource = cmbBox.DataSource;
                        cmbBox.Text = "";
                        InitDataToCmbWithCondition<T>(expkey, expValue, tableName, cmbBox, expCondition);
                        cmbBox.SelectedItem = null;
                        cmbBox.SelectedIndex = -1;
                        cmbBox.SelectedValue = -1L;//为了验证通过设置为long型

                        // 手动设置验证通过
                        // ((INotifyPropertyChanged)cmbBox).PropertyChanged -= MyComboBox_PropertyChanged;
                        // cmbBox.SelectedIndex = -1;
                        //  ((INotifyPropertyChanged)myComboBox).PropertyChanged += MyComboBox_PropertyChanged;



                        //cmbBox.SelectedItem
                        #endregion

                    };
                    cmbBox.ButtonSpecs.Add(bsa);
                }
            }
            #endregion
        }



        /// <summary>
        /// 引用字段时。通常要与被引用的表中的主键名称一样。但是如果一个表中同时引用 比方调拨单 引用了两个仓库一进一出时。
        /// 字段名不一样，则可以指定字段名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">{tb_StockTransferProxy}</param>
        /// <param name="expkey">Location_ID_from</param>
        /// <param name="pointkey">Location_ID</param>
        /// <param name="expValue">Name</param>
        /// <param name="tableName">"tb_Location"</param>
        /// <param name="cmbBox"></param>
        /// <param name="expCondition">tb_Location中的过滤条件：Param_0.Is_enabled == True</param>
        public static void BindData4CmbRefWithLimitedByAlias<T>(object entity, string expkey, string pointkey, string expValue, string tableName, KryptonComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {
            cmbBox.DataBindings.Clear();
            InitDataToCmbWithCondition<T>(expkey, expValue, tableName, cmbBox, expCondition);
            var depa = new Binding("SelectedValue", entity, pointkey, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);

            #region 添加清除功能
            //不重复添加
            if (cmbBox.ButtonSpecs.Where(b => b.UniqueName == "btnclear").Any())
            {
                return;
            }
            if ((cmbBox as Control).DataBindings.Count > 0)
            {
                if (cmbBox.DataBindings.Count > 0 && cmbBox.DataSource is BindingSource)
                {
                    ButtonSpecAny bsa = new ButtonSpecAny();
                    // bsa.Image = Image.FromStream(GetResource("RUINORERP.UI.ResourceFile.search.ico"));
                    // bsa.Tag = ktb;
                    // cmbBox.Tag = targetEntity;
                    bsa.UniqueName = "btnclear";
                    bsa.Type = PaletteButtonSpecStyle.Close;
                    bsa.Edge = PaletteRelativeEdgeAlign.Near;
                    //bsa.Click += BsaEdit_Click;
                    bsa.Click += (sender, e) =>
                    {
                        #region
                        cmbBox.DataSource = cmbBox.DataSource;
                        cmbBox.Text = "";
                        InitDataToCmbWithCondition<T>(expkey, expValue, tableName, cmbBox, expCondition);
                        cmbBox.SelectedItem = null;
                        cmbBox.SelectedIndex = -1;
                        cmbBox.SelectedValue = -1L;//为了验证通过设置为long型

                        // 手动设置验证通过
                        // ((INotifyPropertyChanged)cmbBox).PropertyChanged -= MyComboBox_PropertyChanged;
                        // cmbBox.SelectedIndex = -1;
                        //  ((INotifyPropertyChanged)myComboBox).PropertyChanged += MyComboBox_PropertyChanged;



                        //cmbBox.SelectedItem
                        #endregion

                    };
                    cmbBox.ButtonSpecs.Add(bsa);
                }
            }
            #endregion
        }


        /// <summary>
        /// 绑定特殊多选，
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expkey"></param>
        /// <param name="expValue"></param>
        /// <param name="tableName"></param>
        /// <param name="cmbBox"></param>
        /// <param name="expCondition"></param>
        public static void BindData4CmbChkRefWithLimited<T>(object entity, string expkey, string expValue, string tableName, CheckBoxComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {
            //查询DTO实体 其中有一个属性会同步对应选中的结果。这样后面SQL就对这个选的结果进行处理。
            //cmbBox.DataBindings.Clear();
            InitDataToCmbChkWithCondition<T>(expkey, expValue, tableName, cmbBox, expCondition);
            string key = expkey;// colName+"_MultiChoiceResults" 是动态修改DTO时生成的。为了传值给sql
            var depa = new Binding("MultiChoiceResults", entity, key + "_MultiChoiceResults", true, DataSourceUpdateMode.OnPropertyChanged);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) =>
            {
                args.Value = args.Value == null ? new List<object>() : args.Value;
            };

            //depa.Format += (s, args) => args.Value = args.Value == null ? new List<object>() : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            //depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            depa.Parse += (s, args) =>
            {
                args.Value = args.Value == null ? new List<object>() : args.Value;
            };
            cmbBox.CheckBoxCheckedChanged += (sender, e) =>
            {
                depa.WriteValue();
            };
            cmbBox.DataBindings.Add(depa);

            #region 添加清除功能
            //不重复添加
            //if (cmbBox.ButtonSpecs.Where(b => b.UniqueName == "btnclear").Any())
            //{
            //    return;
            //}
            //if ((cmbBox as Control).DataBindings.Count > 0)
            //{
            //    if (cmbBox.DataBindings.Count > 0 && cmbBox.DataSource is BindingSource)
            //    {
            //        ButtonSpecAny bsa = new ButtonSpecAny();
            //        // bsa.Image = Image.FromStream(GetResource("RUINORERP.UI.ResourceFile.search.ico"));
            //        // bsa.Tag = ktb;
            //        // cmbBox.Tag = targetEntity;
            //        bsa.UniqueName = "btnclear";
            //        bsa.Type = PaletteButtonSpecStyle.Close;
            //        bsa.Edge = PaletteRelativeEdgeAlign.Near;
            //        //bsa.Click += BsaEdit_Click;
            //        bsa.Click += (sender, e) =>
            //        {
            //            #region
            //            cmbBox.DataSource = cmbBox.DataSource;
            //            cmbBox.Text = "";
            //            InitDataToCmbWithCondition<T>(expkey, expValue, tableName, cmbBox, expCondition);
            //            cmbBox.SelectedItem = null;
            //            cmbBox.SelectedValue = -1;
            //            #endregion

            //        };
            //        cmbBox.ButtonSpecs.Add(bsa);
            //    }
            //}
            #endregion
        }



        public static void BindData4Cmb<T>(object entity, string expkey, string expValue, string tableName, KryptonComboBox cmbBox) where T : class
        {
            InitDataToCmb<T>(expkey, expValue, tableName, cmbBox);
            string key = expkey;
            var depa = new Binding("SelectedValue", entity, key, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        public static void BindData4Cmb(BindingSource droplistdatasouce, object entity, string ValueMember, string DisplayMember, KryptonComboBox cmbBox)
        {
            InitDataToCmb(droplistdatasouce, ValueMember, DisplayMember, cmbBox);
            var depa = new Binding("SelectedValue", entity, ValueMember, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }

        public static void BindData4CheckBox(object entity, string key, KryptonCheckBox chkBox, bool SyncUI)
        {
            Binding binddata = null;
            if (SyncUI)
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            else
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
            }
            // Set initial values
            chkBox.Checked = bool.Parse(value);

            // Change on event
            chkBox.CheckedChanged += delegate
            {
                ReflectionHelper.SetPropertyValue(entity, key, chkBox.Checked);
            };
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            chkBox.DataBindings.Add(binddata);
        }

        public static void BindData4CheckBox<T>(object entity, string key, KryptonCheckBox chkBox, bool SyncUI)
        {
            chkBox.DataBindings.Clear();
            Binding binddata = null;
            if (SyncUI)
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                binddata = new Binding("Checked", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            string value = ReflectionHelper.GetModelValue(key, entity);
            if (value == null)
            {
                value = "false";
                chkBox.Checked = false;
                //ReflectionHelper.SetPropertyValue(entity, key, false);
            }
            // Set initial values
            chkBox.Checked = bool.Parse(value);
            // Change on event 因为UI中，有时选择后。直接操作按钮。并没有激发验证事件。所以实体中属性没有变更。这里强制变更一下。
            chkBox.CheckedChanged += delegate
            {
                if (chkBox.Focused)
                {
                    ReflectionHelper.SetPropertyValue(entity, key, chkBox.Checked);
                }

            };
            //数据源的数据类型转换为控件要求的数据类型。
            binddata.Format += (s, args) => args.Value = args.Value == null ? false : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            binddata.Parse += (s, args) => args.Value = args.Value == null ? false : args.Value;
            chkBox.DataBindings.Add(binddata);
        }


        public static void BindData4CheckBox<T>(object entity, Expression<Func<T, bool?>> exp, KryptonCheckBox chkBox, bool SyncUI)
        {

            var mb = exp.GetMemberInfo();
            string key = mb.Name;
            BindData4CheckBox<T>(entity, key, chkBox, SyncUI);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="datetimeValue">datetimeValue这个值没有起到作用</param>
        /// <param name="key"></param>
        /// <param name="dtp"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4DataTime(object entity, object datetimeValue, string key, KryptonDateTimePicker dtp, bool SyncUI)
        {
            dtp.DataBindings.Clear();
            //chkbox
            Binding dtpdata = null;
            if (SyncUI)
            {
                dtpdata = new Binding("Value", entity, key, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                dtpdata = new Binding("Value", entity, key, true, DataSourceUpdateMode.OnValidation);
            }


            //数据源的数据类型转换为控件要求的数据类型。
            dtpdata.Format += (s, args) =>
            {
                if (args.Value == null || args.Value.ToString() == "0001-01-01 00:00:00")
                {
                    args.Value = " ";
                    dtp.ValueNullable = DBNull.Value;
                }

            };
            //将控件的数据类型转换为数据源要求的数据类型。
            dtpdata.Parse += (s, args) =>
            {
                args.Value = !dtp.Checked ? null : args.Value;
            };
            dtp.Validating += dtp_Validating;
            dtp.CheckedChanged += Dtp_CheckedChanged;

            if (datetimeValue == null)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "   ";
            }

            dtp.DataBindings.Add(dtpdata);
        }


        public static void BindData4DataTime<T>(object entity, Expression<Func<T, DateTime?>> exp, KryptonDateTimePicker dtp, bool SyncUI)
        {
            var mb = exp.GetMemberInfo();
            string key = mb.Name;
            object datetimeValue = typeof(T).GetProperty(key).GetValue(entity, null);

            BindData4DataTime(entity, datetimeValue, key, dtp, SyncUI);
        }

        private static void Dtp_CheckedChanged(object sender, EventArgs e)
        {
            KryptonDateTimePicker dtp = sender as KryptonDateTimePicker;
            if (!dtp.Checked)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "   ";
                //将绑定的实体的值清空 = null;
                if (dtp.DataBindings.Count > 0)
                {
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, null);
                }
                dtp.CausesValidation = false;
                //如果 CausesValidation 属性设置为 false，则将取消 Validating 和 Validated 事件。
                ////dtp.Value = System.DateTime.Now;

            }
            else
            {
                dtp.Format = DateTimePickerFormat.Short;
                dtp.CustomFormat = null;
                if (dtp.DataBindings.Count > 0)
                {
                    if (dtp != null && dtp.Value.Year == 1)
                    {
                        dtp.Value = System.DateTime.Now;
                    }
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, dtp.Value);
                }
                else
                {
                    //if (dtp.DataBindings.Count == 0)
                    //{
                    //    dtp.DataBindings.Add(dtp.DataBindings[0]);
                    //}
                }
                dtp.CausesValidation = true;
                //如果 CausesValidation 属性设置为 false，则将取消 Validating 和 Validated 事件。

            }
        }



        private static void dtp_Validating(object sender, CancelEventArgs e)
        {
            KryptonDateTimePicker dtp = sender as KryptonDateTimePicker;
            if (!dtp.Checked)
            {
                //将绑定的实体的值清空 = null;
                if (dtp.DataBindings.Count > 0)
                {
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, null);
                }
                //dtp.DataBindings.Clear();
                e.Cancel = false;
            }
            else
            {
                //将绑定的实体的值清空 = null;
                if (dtp.DataBindings.Count > 0)
                {
                    RUINORERP.Common.Helper.ReflectionHelper.SetPropertyValue(dtp.DataBindings[0].DataSource, dtp.DataBindings[0].BindingMemberInfo.BindingMember, dtp.Value);
                }
                //dtp.DataBindings.Clear();
                e.Cancel = false;
            }

        }

        public static void BindData4Money<T>(object entity, Expression<Func<T, int>> expkey, KryptonTextBox txtBox, bool SyncUI)
        {
            var mb = expkey.GetMemberInfo();
            string key = mb.Name;
            Binding depa = null;
            if (SyncUI)
            {
                depa = new Binding("Text", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            else
            {
                depa = new Binding("Text", entity, key, true, DataSourceUpdateMode.OnValidation);
            }
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? 0 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? 0 : args.Value;
            txtBox.DataBindings.Add(depa);
        }




        public static void BindData4TextBox<T>(object entity,
        Expression<Func<T, object>> expTextField,
        KryptonTextBox txtBox,
        BindDataType4TextBox type, bool SyncUI, bool validationEnabled = true
        )
        {
            txtBox.ReadOnly = !validationEnabled;
            txtBox.DataBindings.Clear();
            // string textField = expTextField.Body.ToString().Split('.')[1];
            MemberInfo minfo = expTextField.GetMemberInfo();
            string textField = minfo.Name;
            BindData4TextBox<T>(entity, textField, txtBox, type, SyncUI);
        }


        /// <summary>
        /// 请注意。这个指定绑定的是tag属性不是Text->BindData4TextBoxWithQuery
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expValueField"></param>
        /// <param name="txtBox"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBoxWithTagQuery<T>(object entity,
        Expression<Func<T, object>> expValueField,
        KryptonTextBox txtBox,
        bool SyncUI
        )
        {
            txtBox.DataBindings.Clear();
            //MemberInfo minfo = expTextField.GetMemberInfo();
            //string textField = minfo.Name;
            MemberInfo minfoValue = expValueField.GetMemberInfo();
            string valueField = minfoValue.Name;

            BindData4TextBoxWithTagQuery(entity, valueField, txtBox, SyncUI);
        }


        #region tag

        /// <summary>
        /// 请注意。这个指定绑定的是tag属性不是Text->BindData4TextBoxWithQuery
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expValueField"></param>
        /// <param name="txtBox"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBoxWithTagQuery<T>(object entity,
        Expression<Func<T, object>> expValueField,
        KryptonTextBox txtBox,
        bool SyncUI, bool validationEnabled
        )
        {
            txtBox.DataBindings.Clear();
            MemberInfo minfoValue = expValueField.GetMemberInfo();
            string valueField = minfoValue.Name;
            BindData4TextBoxWithTagQuery<T>(entity, valueField, txtBox, SyncUI, validationEnabled);
        }
        /// <summary>
        /// 请注意。这个指定绑定的是tag属性不是Text
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="valueField"></param>
        /// <param name="txtBox"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBoxWithTagQuery<T>(object entity, string valueField, KryptonTextBox txtBox, bool SyncUI, bool validationEnabled)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Tag", entity, valueField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Tag", entity, valueField, true, validationEnabled ?
                     DataSourceUpdateMode.OnValidation :
                     DataSourceUpdateMode.Never);
            }
            depa.BindingComplete += (sender, e) =>
            {
                if (!validationEnabled)
                {
                    //  e.BindingCompleteState = BindingCompleteState.Success;
                    e.Cancel = false;
                    return;
                }
            };
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) =>
            {
                args.Value = args.Value == null ? "" : args.Value;
            };
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) =>
            {
                args.Value = args.Value == null ? "" : args.Value;
            };
            txtBox.DataBindings.Add(depa);
        }


        #endregion



        public static void BindData4ControlByEnum<T>(object entity, Expression<Func<T, object>> expTextField, Control control,
        BindDataType4Enum type, Type enumType
        )
        {
            control.DataBindings.Clear();
            MemberInfo minfo = expTextField.GetMemberInfo();
            string textField = minfo.Name;
            BindData4ControlByEnum<T>(entity, textField, control, type, enumType);
        }

        public static void BindData4ControlByEnum<T>(object entity, string textField, Control txtBox, BindDataType4Enum type, Type enumType)
        {
            Binding depa = null;
            //if (SyncUI)
            //{
            //    //双向绑定 应用于加载和编辑
            //    depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            //}
            //else
            //{
            //    //单向绑定 应用于加载
            //depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);

            depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            //}

            switch (type)
            {
                case BindDataType4Enum.EnumName:
                    //数据源的数据类型转换为控件要求的数据类型。
                    //
                    depa.Format += (s, args) =>
                    {
                        if (args.Value == null)
                        {
                            args.Value = "";
                        }
                        else
                        {
                            args.Value = Enum.ToObject(enumType, args.Value).ToString();
                        }

                    };
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        if (args == null)
                        {
                            args.Value = 0;
                        }
                        else
                        {
                            args.Value = (int)args.Value;
                        }
                    };

                    break;
                case BindDataType4Enum.EnumDisplay:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;
                default:
                    break;
            }

            txtBox.DataBindings.Add(depa);
        }






        public static void BindData4Label<T>(object entity,
        Expression<Func<T, string>> expTextField,
        KryptonLabel lbl,
        BindDataType4TextBox type, bool SyncUI
        )
        {
            lbl.DataBindings.Clear();
            // string textField = expTextField.Body.ToString().Split('.')[1];
            MemberInfo minfo = expTextField.GetMemberInfo();
            string textField = minfo.Name;
            BindData4Label<T>(entity, textField, lbl, type, SyncUI);
        }


        public static void BindData4Label<T>(object entity, string textField, KryptonLabel lbl, BindDataType4TextBox type, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            switch (type)
            {
                case BindDataType4TextBox.Qty:
                    //数据源的数据类型转换为控件要求的数据类型。

                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                        lbl.Text = args.Value.ToString();
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                    };


                    break;
                case BindDataType4TextBox.Money:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                        lbl.Text = args.Value.ToString("##.##");//这里是不是可以控制小数显示位数？
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                    };


                    break;
                case BindDataType4TextBox.Text:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;
                default:
                    break;
            }

            lbl.DataBindings.Add(depa);
        }


        /// <summary>
        /// 请注意。这个指定绑定的是tag属性不是Text
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="valueField"></param>
        /// <param name="txtBox"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBoxWithTagQuery(object entity, string valueField, KryptonTextBox txtBox, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Tag", entity, valueField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Tag", entity, valueField, true, DataSourceUpdateMode.OnValidation);
            }

            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;


            txtBox.DataBindings.Add(depa);

            //===============================

            /*
            Binding depaText = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depaText = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depaText = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            //数据源的数据类型转换为控件要求的数据类型。
            depaText.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depaText.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;


            txtBox.DataBindings.Add(depaText);
            */


        }



        public static void BindData4TextBox<T>(object entity, string textField, KryptonTextBox txtBox,
            BindDataType4TextBox type, bool SyncUI, bool validationEnabled = true)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                //depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
                depa = new Binding("Text", entity, textField, true, validationEnabled ? DataSourceUpdateMode.OnValidation : DataSourceUpdateMode.Never);
            }
            #region 完成验证事件
            depa.BindingComplete += (sender, e) =>
            {
                if (!validationEnabled)
                {
                    e.Cancel = false;
                    return;
                }

                // 验证逻辑
                if (e.BindingCompleteContext == BindingCompleteContext.ControlUpdate)
                {
                    // 执行验证

                }
            };



            #endregion

            switch (type)
            {
                case BindDataType4TextBox.Qty:
                    //数据源的数据类型转换为控件要求的数据类型。

                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                        txtBox.Text = args.Value.ToString();
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0 : args.Value;
                    };


                    break;
                case BindDataType4TextBox.Money:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                        txtBox.Text = args.Value.ToString("##.##");//这里是不是可以控制小数显示位数？
                    };

                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) =>
                    {
                        args.Value = args.Value == null ? 0.00 : args.Value;
                    };


                    break;
                case BindDataType4TextBox.Text:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;

                default:
                    break;
            }

            txtBox.DataBindings.Add(depa);
        }



        public static void BindData4NumericUpDown<T>(object entity, string textField, KryptonNumericUpDown numericUpDown, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Value", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Value", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            depa.Format += (s, args) =>
            {
                args.Value = args.Value == null ? 0 : args.Value;
                numericUpDown.Value = args.Value.ToInt();
            };

            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) =>
            {
                args.Value = args.Value == null ? 0 : args.Value;
            };

            numericUpDown.DataBindings.Add(depa);
        }


        public static void BindData4TrackBar<T>(object entity, string textField, TrackBar trackBar, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Value", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Value", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            depa.Format += (s, args) =>
            {
                args.Value = args.Value == null ? 0 : args.Value;
                trackBar.Value = args.Value.ToInt();
            };

            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) =>
            {
                args.Value = args.Value == null ? 0 : args.Value;
            };

            trackBar.DataBindings.Add(depa);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="textField"></param>
        /// <param name="txtBox"></param>
        /// <param name="type"></param>
        /// <param name="SyncUI"></param>
        public static void BindData4TextBox(object entity, string textField, KryptonTextBox txtBox, BindDataType4TextBox type, bool SyncUI)
        {
            Binding depa = null;
            if (SyncUI)
            {
                //双向绑定 应用于加载和编辑
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnPropertyChanged);
            }
            else
            {
                //单向绑定 应用于加载
                depa = new Binding("Text", entity, textField, true, DataSourceUpdateMode.OnValidation);
            }

            switch (type)
            {
                case BindDataType4TextBox.Qty:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? 0 : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? 0 : args.Value;

                    break;
                case BindDataType4TextBox.Money:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? 0.00 : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? 0.00 : args.Value;

                    break;
                case BindDataType4TextBox.Text:
                    //数据源的数据类型转换为控件要求的数据类型。
                    depa.Format += (s, args) => args.Value = args.Value == null ? "" : args.Value;
                    //将控件的数据类型转换为数据源要求的数据类型。
                    depa.Parse += (s, args) => args.Value = args.Value == null ? "" : args.Value;

                    break;
                default:
                    break;
            }

            txtBox.DataBindings.Add(depa);
        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb<T>(Expression<Func<T, long>> expression, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox) where T : class
        {
            MemberInfo minfo = expression.GetMemberInfo();
            string key = minfo.Name;
            MemberInfo minfoValue = expValue.GetMemberInfo();
            string value = minfoValue.Name;
            string tableName = expression.Parameters[0].Type.Name;
            InitDataToCmb<T>(key, value, tableName, cmbBox);
        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// 一般是载入UI时使用，目前很多都 使用的 BindData4Cmb，里面就包含了这个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb<T>(Expression<Func<T, long>> expression, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {
            MemberInfo minfo = expression.GetMemberInfo();
            string key = minfo.Name;
            MemberInfo minfoValue = expValue.GetMemberInfo();
            string value = minfoValue.Name;
            string tableName = expression.Parameters[0].Type.Name;


            BaseProcessor basePro = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");
            QueryFilter queryFilter = basePro.GetQueryFilter();
            queryFilter.FilterLimitExpressions.Add(expCondition);

            InitDataToCmbWithCondition<T>(key, value, tableName, cmbBox, queryFilter.GetFilterExpression<T>());

            //InitDataToCmbWithCondition<T>(key, value, tableName, cmbBox, expCondition);

        }

        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmbByControl<T>(Expression<Func<T, long>> expression, Expression<Func<T, string>> expValue, KryptonComboBox cmbBox) where T : class
        {
            MemberInfo minfo = expression.GetMemberInfo();
            string key = minfo.Name;
            MemberInfo minfoValue = expValue.GetMemberInfo();
            string value = minfoValue.Name;
            string tableName = expression.Parameters[0].Type.Name;
            InitDataToCmb<T>(key, value, tableName, cmbBox);
        }

        public static void InitDataToCmb<T>(Expression<Func<T, long>> expression, KryptonComboBox cmbBox) where T : class
        {
            MemberInfo minfo = expression.GetMemberInfo();
            string key = minfo.Name;
            string value = "";
            string tableName = expression.Parameters[0].Type.Name;
            InitDataToCmb<T>(key, value, tableName, cmbBox);
        }

        public static void InitDataToCmbByEnumDynamicGeneratedDataSource<T>(Type enumType, Expression<Func<T, int?>> expKey, KryptonComboBox cmbBox, bool addSelect)
        {
            MemberInfo minfo = expKey.GetMemberInfo();
            string key = minfo.Name;
            InitDataToCmbByEnumDynamicGeneratedDataSource(enumType, key, cmbBox, addSelect);
        }



        /// <summary>
        /// 枚举名称要与DB表中的字段名相同
        /// </summary>
        /// <typeparam name="T">枚举的类型</typeparam>
        /// <param name="enumTypeName"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmbByEnumDynamicGeneratedDataSource(Type enumType, string keyName, KryptonComboBox cmbBox, bool addSelect)
        {
            //枚举值为int/long，动态生成一个类再绑定，

            // 获取枚举的基础类型
            Type underlyingType = Enum.GetUnderlyingType(enumType);


            var type = enumType;
            var aName = new System.Reflection.AssemblyName(Assembly.GetExecutingAssembly().GetName().Name);


            TypeConfig typeConfig = new TypeConfig();
            typeConfig.FullName = aName.Name;

            //要创建的属性
            PropertyConfig propertyConfigKey = new PropertyConfig();
            propertyConfigKey.PropertyName = keyName;// type.Name;默认枚举名改为可以指定名
            if (underlyingType == typeof(int))
            {
                propertyConfigKey.PropertyType = typeof(int);//枚举值为int 默认
            }
            if (underlyingType == typeof(long))
            {
                propertyConfigKey.PropertyType = typeof(long);//枚举值为long 默认
            }
            PropertyConfig propertyConfigName = new PropertyConfig();
            propertyConfigName.PropertyName = "Name";
            propertyConfigName.PropertyType = typeof(string);

            typeConfig.Properties.Add(propertyConfigKey);
            typeConfig.Properties.Add(propertyConfigName);
            Type newType = TypeBuilderHelper.BuildType(typeConfig);

            List<object> list = new List<object>();
            //(enumType[])Enum.GetValues(typeof(enumType));



            Array enumValues = Enum.GetValues(type);
            IEnumerator e = enumValues.GetEnumerator();
            e.Reset();
            int currentValue;
            long currentValueLong;
            string currentName;
            while (e.MoveNext())
            {
                object eobj = Activator.CreateInstance(newType);
                if (underlyingType == typeof(int))
                {
                    currentValue = (int)e.Current;
                    eobj.SetPropertyValue(keyName, currentValue);
                }
                else if (underlyingType == typeof(long))
                {
                    currentValueLong = (long)e.Current;
                    eobj.SetPropertyValue(keyName, currentValueLong);
                }

                currentName = e.Current.ToString();


                var fieldInfo = enumType.GetField(currentName);
                var descriptionAttribute = fieldInfo?
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault() as DescriptionAttribute;
                string Description = descriptionAttribute?.Description ?? string.Empty;
                if (!string.IsNullOrEmpty(Description))
                {
                    eobj.SetPropertyValue("Name", Description);
                }
                else
                {
                    eobj.SetPropertyValue("Name", currentName);
                }
                list.Add(eobj);
            }
            if (addSelect)
            {
                object sobj = Activator.CreateInstance(newType);
                sobj.SetPropertyValue(keyName, -1);
                sobj.SetPropertyValue("Name", "请选择");
                list.Insert(0, sobj);
            }

            cmbBox.SelectedValue = -1;

            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            ComboBoxHelper.InitDropList(bs, cmbBox, keyName, "Name", ComboBoxStyle.DropDown, false);

        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb<T>(string key, string value, string tableName, KryptonComboBox cmbBox) where T : class
        {
            if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = BizCacheHelper.Manager.NewTableList[tableName].Value;
                }
                List<T> tlist = new List<T>();
                BindingSource bs = new BindingSource();
                var cachelist = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                if (cachelist == null)
                {
                    Business.CommService.CommonController bdc = Startup.GetFromFac<Business.CommService.CommonController>();
                    tlist = bdc.GetBindSource<T>(tableName);
                }
                else
                {
                    Type listType = cachelist.GetType();
                    if (TypeHelper.IsGenericList(listType))
                    {
                        if (listType.FullName.Contains("System.Collections.Generic.List`1[[System.Object"))
                        {
                            List<T> lastOKList = new List<T>();
                            var lastlist = ((IEnumerable<dynamic>)cachelist).ToList();
                            foreach (var item in lastlist)
                            {
                                lastOKList.Add(item);
                            }
                            tlist = lastOKList;
                        }
                        else
                        {
                            tlist = cachelist as List<T>;
                        }
                    }
                    else if (TypeHelper.IsJArrayList(listType))
                    {
                        List<T> lastOKList = new List<T>();

                        var objlist = TypeHelper.ConvertJArrayToList(typeof(T), cachelist as JArray);
                        var lastlist = ((IEnumerable<dynamic>)objlist).ToList();
                        foreach (var item in lastlist)
                        {
                            lastOKList.Add(item);
                        }
                        tlist = lastOKList;
                    }
                    InsertSelectItem<T>(key, value, tlist);
                }

                #region exp process
                BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(T).Name + "Processor");

                List<T> newlist = baseProcessor.GetListByLimitExp<T>(tlist);

                #endregion
                InsertSelectItem<T>(key, value, newlist);
                bs.DataSource = newlist;
                ComboBoxHelper.InitDropList(bs, cmbBox, key, value, ComboBoxStyle.DropDown, true);
                if (cmbBox.Tag == null)
                {
                    cmbBox.Tag = tableName;
                }
            }
        }

        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmbWithCondition<T>(string key, string value, string tableName, KryptonComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {

            BindingSource bs = new BindingSource();


            Business.CommService.CommonController bdc = Startup.GetFromFac<Business.CommService.CommonController>();
            var list = bdc.GetBindSource<T>(tableName, expCondition);
            // Func<T, bool> funCondition = ExpressionHelper.ConvertToFunc<T>(expCondition);
            List<T> Newlist = list.ToList();
            InsertSelectItem<T>(key, value, Newlist);
            bs.DataSource = Newlist;
            cmbBox.Tag = tableName;
            ComboBoxHelper.InitDropList(bs, cmbBox, key, value, ComboBoxStyle.DropDown, false);


        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmbChkWithCondition<T>(string key, string value, string tableName, CheckBoxComboBox cmbBox, Expression<Func<T, bool>> expCondition)
        {
            if (expCondition == null)
            {
                expCondition = c => true;
            }
            // BindingSource bs = new BindingSource();
            Business.CommService.CommonController bdc = Startup.GetFromFac<Business.CommService.CommonController>();
            var list = bdc.GetBindSource<T>(tableName, expCondition);
            // Func<T, bool> funCondition = ExpressionHelper.ConvertToFunc<T>(expCondition);
            List<T> Newlist = list.ToList();
            // InsertSelectItem<T>(key, value, Newlist);
            //bs.DataSource = Newlist;
            //将数据 源转换一下
            List<CmbChkItem> cmbItems = new List<CmbChkItem>();
            foreach (var item in Newlist)
            {
                cmbItems.Add(new CmbChkItem(item.GetPropertyValue(key).ToString(), item.GetPropertyValue(value).ToString()));
            }

            ListSelectionWrapper<CmbChkItem> selectionWrappers = new ListSelectionWrapper<CmbChkItem>(cmbItems, "Name");
            cmbBox.BeginUpdate();
            cmbBox.DataSource = selectionWrappers;
            cmbBox.DisplayMemberSingleItem = "Name";//CmbItem 这个类中显示名称的属性名
            cmbBox.DisplayMember = "NameConcatenated";//固定的？ 将名称可以多选中联系&起来显示？
            cmbBox.ValueMember = "Selected";//固定的一个bool型
                                            // cmbBox.CheckBoxItems[3].DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            cmbBox.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            //设置默认选中
            // StatusSelections.FindObjectWithItem(UpdatedStatus).Selected = true;
            cmbBox.EndUpdate();

        }



        /// <summary>
        /// 搜寻匹配的列表
        /// </summary>
        /// <param name="currentPartX">X字符串的当前比较部分</param>
        /// <param name="currentPartY">Y字符串的当前比较部分</param>
        /// <returns></returns>
        private static List<string> SearchMatchedList(string currentPartX, string currentPartY, List<List<string>> _preferenceList)
        {
            List<string> matchedList = null;
            foreach (var list in _preferenceList)
            {
                if (list.Exists(currentPartX.Contains) && list.Exists(currentPartY.Contains))
                {
                    matchedList = list;
                    break;
                }
            }

            return matchedList;
        }



        /// <summary>
        /// 默认比较
        /// </summary>
        private static int DefaultCompare(string x, string y)
        {
            return string.Compare(x, y, false, CultureInfo.CurrentCulture);
        }









        /// <summary>
        /// 插入默认的下拉 请选择
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="list">强类型的集合</param>
        public static void InsertSelectItem<T>(string key, string value, List<T> list)
        {
            bool haveDefautValue = false;
            if (list == null)
            {
                return;
            }
            foreach (var item in list)
            {
                if (item.GetPropertyValue(value) == null || item.GetPropertyValue(value).ToString() == "请选择")
                {
                    haveDefautValue = true;
                    break;
                }
            }
            if (!haveDefautValue)
            {
                object defaultSelectObj = Activator.CreateInstance(typeof(T));
                if (defaultSelectObj.GetType().GetProperty(key, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).PropertyType.FullName.Contains("Int64"))
                {
                    defaultSelectObj.SetPropertyValue(key, -1L);//long類型， F、f 或 M
                }
                else
                {
                    defaultSelectObj.SetPropertyValue(key, -1);
                }


                defaultSelectObj.SetPropertyValue(value, "请选择");
                list.Insert(0, (T)defaultSelectObj);
            }
        }


        private static void InsertSelectItem(string key, string value, IEnumerable list, Type type)
        {
            bool haveDefautValue = false;
            foreach (var item in list)
            {
                if (item.GetPropertyValue(value).ToString() == "请选择")
                {
                    haveDefautValue = true;
                    break;
                }
            }
            if (!haveDefautValue)
            {
                object defaultSelectObj = Activator.CreateInstance(type);
                defaultSelectObj.SetPropertyValue(key, -1);
                defaultSelectObj.SetPropertyValue(value, "请选择");
                // list.CastToList < type.GetType() >.Insert(0, defaultSelectObj);
            }
        }


        #region 原生cmb的绑定

        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expID"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitCmb<T>(Expression<Func<T, long>> expID, Expression<Func<T, string>> expValue, ComboBox cmbBox, bool HasSelectItem, List<T> bindDataSource = null) where T : class
        {
            MemberInfo minfo = expID.GetMemberInfo();
            string key = minfo.Name;
            MemberInfo minfoValue = expValue.GetMemberInfo();
            string value = minfoValue.Name;
            string tableName = expID.Parameters[0].Type.Name;
            InitCmb<T>(key, value, tableName, cmbBox, HasSelectItem, bindDataSource);
        }

        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitCmb<T>(string key, string value, string tableName, ComboBox cmbBox, bool HasSelectItem, List<T> bindDataSource = null) where T : class
        {
            if (bindDataSource != null)
            {
                #region 指定数据源

                BindingSource bs = new BindingSource();
                List<T> tlist = new List<T>();
                tlist = bindDataSource;
                if (HasSelectItem)
                {
                    InsertSelectItem<T>(key, value, tlist);
                }
                bs.DataSource = tlist;
                InitCmb(bs, cmbBox, key, value, ComboBoxStyle.DropDownList, false, true);
                #endregion
            }
            else
            {
                #region 从缓存中获取数据源，否则查数据库
                if (BizCacheHelper.Manager.NewTableList.ContainsKey(tableName))
                {
                    BindingSource bs = new BindingSource();
                    List<T> tlist = new List<T>();
                    var cachelist = BizCacheHelper.Manager.CacheEntityList.Get(tableName);
                    if (cachelist == null)
                    {
                        Business.CommService.CommonController bdc = Startup.GetFromFac<Business.CommService.CommonController>();
                        tlist = bdc.GetBindSource<T>(tableName);
                        if (HasSelectItem)
                        {
                            InsertSelectItem<T>(key, value, tlist);
                        }

                        bs.DataSource = tlist;
                    }
                    else
                    {
                        Type listType = cachelist.GetType();
                        if (TypeHelper.IsGenericList(listType))
                        {
                            //tlist = cachelist as List<T>;
                            //tlist = cachelist as List<object>;
                            // Type elementType = listType.GetGenericArguments()[0];
                            // 创建一个新的 List<object>
                            List<object> convertedList = new List<object>();
                            convertedList = cachelist as List<object>;
                            if (convertedList != null)
                            {
                                // Type elementType = TypeHelper.GetFirstArgumentType(listType);
                                Type elementType = null;
                                if (BizCacheHelper.Manager.NewTableTypeList.TryGetValue(tableName, out elementType))
                                {
                                    foreach (var item in convertedList)
                                    {
                                        try
                                        {
                                            var convertedItem = Convert.ChangeType(item, elementType);
                                            tlist.Add(convertedItem as T);
                                        }
                                        catch (InvalidCastException)
                                        {
                                            // 处理类型转换失败的情况
                                        }
                                    }
                                    //var newInstance = Activator.CreateInstance(elementType);
                                    //// 这里需要根据具体情况实现属性值的复制 这个方法也可以。但是还要处理赋值，麻烦一些
                                    //tlist.Add(newInstance as T);

                                    #region  强类型 转换失败
                                    //var lastlist = ((IEnumerable<dynamic>)convertedList).Select(item => Activator.CreateInstance(elementType)).ToList();
                                    //tlist = lastlist as List<T>;
                                    #endregion
                                }
                            }


                        }
                        else if (TypeHelper.IsJArrayList(listType))
                        {
                            List<T> lastOKList = new List<T>();
                            var objlist = TypeHelper.ConvertJArrayToList(typeof(T), cachelist as JArray);
                            var lastlist = ((IEnumerable<dynamic>)objlist).ToList();
                            foreach (var item in lastlist)
                            {
                                lastOKList.Add(item);
                            }
                            tlist = lastOKList;
                        }

                        if (HasSelectItem)
                        {
                            InsertSelectItem<T>(key, value, tlist);
                        }
                        bs.DataSource = tlist;
                    }
                    InitCmb(bs, cmbBox, key, value, ComboBoxStyle.DropDownList, false, true);
                }
                #endregion


            }

        }



        /// <summary>
        /// 下拉列表的绑定
        /// </summary>
        /// <param name="ds">要绑定的数据源(集)</param>
        /// <param name="cmb">要绑定的控件名</param>
        /// <param name="ValueMember">值的列名</param>
        /// <param name="DisplayMember">显示的列名</param>
        /// <param name="DropStyle">下拉类别</param>
        /// <param name="auto">是否有自动完成功能</param>
        /// <param name="add请选择">  是数据源绑定形式不可以自由添加，只能在数据源上做文章 ("请选择", "-1")</param>
        public static void InitCmb(BindingSource bs, ComboBox cmb, string ValueMember, string DisplayMember, ComboBoxStyle DropStyle, bool auto, bool add请选择)
        {
            cmb.BeginUpdate();
            cmb.DataSource = bs;
            cmb.DropDownStyle = DropStyle;
            cmb.DisplayMember = DisplayMember;
            cmb.ValueMember = ValueMember;
            AutoCompleteStringCollection sc = new AutoCompleteStringCollection();

            foreach (var dr in bs.List)
            {
                sc.Add(ReflectionHelper.GetPropertyValue(dr, DisplayMember).ToString());
            }

            if (auto)
            {
                if (DropStyle == ComboBoxStyle.DropDown)
                {

                    cmb.AutoCompleteCustomSource = sc;
                    cmb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
                }
            }

            cmb.EndUpdate();

            cmb.SelectedIndex = -1;
        }


        #endregion

        /// <summary>
        /// 绑定数据到下拉  用于编辑时 下拉关联再次深度编辑 时重新绑定上级下拉数据
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="ValueMember"></param>
        /// <param name="DisplayMember"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb(BindingSource bs, string ValueMember, string DisplayMember, KryptonComboBox cmbBox)
        {
            //Business.CommService.CommonController bdc = Startup.GetFromFac<Business.CommService.CommonController>();
            ComboBoxHelper.InitDropList(bs, cmbBox, ValueMember, DisplayMember, ComboBoxStyle.DropDownList, false);
        }


        /// <summary>
        /// 绑定数据到下拉（使用了缓存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="expValue"></param>
        /// <param name="cmbBox"></param>
        public static void InitDataToCmb<T>(Expression<Func<T, string>> expr, KryptonComboBox cmbBox, Type enumTypeName)
        {
            var mb = expr.GetMemberInfo();
            string key = mb.Name;
            ComboBoxHelper.InitDropListForWin(cmbBox, enumTypeName);
        }


        #region 





        public static void BindComboBox(KryptonComboBox cmbBox, string Field_ID, string Field_Name, object entity, BindingSource bs)
        {
            //ID在主表和外键中 字段要统一
            ComboBoxHelper.InitDropList(bs, cmbBox, Field_ID, Field_Name, ComboBoxStyle.DropDownList, false);
            var depa = new Binding("SelectedValue", entity, Field_ID, true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbBox.DataBindings.Add(depa);
        }




        #endregion




        #region 枚举绑定数据源的新实现 可以select枚举 选择对应的集合来使用
        public enum RULE
        {
            [Description("Любые, без ограничений")]
            any,
            [Description("Любые если будет три в ряд")]
            anyThree,
            [Description("Соседние, без ограничений")]
            nearAny,
            [Description("Соседние если будет три в ряд")]
            nearThree
        }

        public static object Values
        {
            get
            {
                List<object> list = new List<object>();
                foreach (RULE rule in Enum.GetValues(typeof(RULE)))
                {
                    string desc = rule.GetType().GetMember(rule.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description;
                    list.Add(new { value = rule, desc = desc });
                }
                return list;
            }
        }

        #endregion











    }

    public class EnumerationExtension : MarkupExtension
    {
        private Type _enumType;


        public EnumerationExtension(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");

            EnumType = enumType;
        }

        public Type EnumType
        {
            get { return _enumType; }
            private set
            {
                if (_enumType == value)
                    return;

                var enumType = Nullable.GetUnderlyingType(value) ?? value;

                if (enumType.IsEnum == false)
                    throw new ArgumentException("Type must be an Enum.");

                _enumType = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var enumValues = Enum.GetValues(EnumType);

            return (
              from object enumValue in enumValues
              select new EnumEntityMember
              {
                  Value = enumValue,
                  Description = GetDescription(enumValue)
              }).ToArray();
        }

        private string GetDescription(object enumValue)
        {
            var descriptionAttribute = EnumType
              .GetField(enumValue.ToString())
              .GetCustomAttributes(typeof(DescriptionAttribute), false)
              .FirstOrDefault() as DescriptionAttribute;


            return descriptionAttribute != null
              ? descriptionAttribute.Description
              : enumValue.ToString();
        }


    }

    public class EnumItem
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
