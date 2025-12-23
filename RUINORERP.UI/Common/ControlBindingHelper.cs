using System;
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

using RUINORERP.UI.ChartFramework.Core;
using Image = System.Drawing.Image;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using log4net.Repository.Hierarchy;
using StackExchange.Redis;
using RUINORERP.Common.CollectionExtension;
using RUINORERP.Business.Cache;
using System.Collections.Concurrent;
using FastReport.Utils;

namespace RUINORERP.UI.Common
{
    public static class ControlBindingHelper
    {

        // 在类开始处添加：
        private static IEntityCacheManager _cacheManager;
        private static IEntityCacheManager CacheManager => _cacheManager ?? (_cacheManager = Startup.GetFromFac<IEntityCacheManager>());

       
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
            ToolTipValues toolTip = new ToolTipValues(null);
            toolTip.Description = $"引用数据";
            toolTip.Heading = "";
            toolTip.EnableToolTips = true;
            // 处理下拉框控件
            if (visualControl is KryptonComboBox comboBox)
            {
                // 避免重复添加查询按钮
                if (comboBox.ButtonSpecs.Any(b => b.UniqueName == "btnQuery"))
                    return;

                if (comboBox.DataBindings.Count > 0)
                {

                    comboBox.ToolTipValues = toolTip;
                    // 创建查询按钮
                    var queryButton = new ButtonSpecAny
                    {
                        Image = GetEmbeddedResourceImage("help4"),
                        UniqueName = "btnQuery",
                        ToolTipBody = "查询数据",
                        ToolTipTitle = string.Empty,
                        AllowInheritToolTipTitle = true,
                        ToolTipShadow = true,
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

                            var bizType = Business.BizMapperService.EntityMappingHelper.GetBizType(typeof(P).Name);
                            string BizTypeText = string.Empty;
                            // 如果业务类型为"无对应数据"，则尝试获取实体的描述信息
                            if (bizType == RUINORERP.Global.BizType.无对应数据)
                            {
                                BizTypeText = Business.BizMapperService.EntityMappingHelper.GetEntityDescription(typeof(P));
                            }
                            else
                            {
                                BizTypeText = bizType.ToString();
                            }

                            editForm.Text = "关联查询" + "-" + BizTypeText;



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

                    textBox.ToolTipValues = toolTip;
                    // 创建查询按钮
                    var queryButton = new ButtonSpecAny
                    {
                        Image = GetEmbeddedResourceImage("help4"),
                        ToolTipBody = "查询数据",
                        ToolTipTitle = string.Empty,
                        AllowInheritToolTipTitle = true,
                        ToolTipShadow = true,
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
                            var bizType = Business.BizMapperService.EntityMappingHelper.GetBizType(typeof(P).Name);
                            string BizTypeText = string.Empty;
                            // 如果业务类型为"无对应数据"，则尝试获取实体的描述信息
                            if (bizType == RUINORERP.Global.BizType.无对应数据)
                            {
                                BizTypeText = Business.BizMapperService.EntityMappingHelper.GetEntityDescription(typeof(P));
                            }
                            else
                            {
                                BizTypeText = bizType.ToString();
                            }

                            editForm.Text = "关联查询" + "-" + BizTypeText;


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
                    return null;
                }

                var memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Png); // 使用PNG格式保留透明度
                memoryStream.Position = 0; // 重置流位置

                return memoryStream;
            }
            catch (Exception ex)
            {
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
                    // 使用静态缓存管理器方法获取实体列表
                    var cacheList = CacheManager.GetEntityListByTableName(entityType.Name);
                    if (cacheList != null && cacheList.Count > 0)
                    {
                        entityList = cacheList;
                        return true;
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
}
