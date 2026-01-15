// **********************************************************************************
// 智能帮助解析器
// 基于系统架构的泛型类型→实体→字段→控件名映射规律，实现智能化帮助匹配
// 
// 核心功能：
// 1. 从泛型类型提取实体类型（如 BaseBillEditGeneric<tb_SaleOrder, C> → tb_SaleOrder）
// 2. 从控件名提取字段名（如 cmbCustomerVendor_ID → CustomerVendor_ID）
// 3. 智能匹配并生成帮助键（如 Fields.tb_SaleOrder.CustomerVendor_ID）
// 4. 支持多种控件前缀：txt, cmb, lbl, dtp, chk, num等
//
// 作者: 系统架构优化
// 时间: 2026-01-15
// **********************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 智能帮助解析器
    /// 自动解析控件关联的实体和字段信息，生成对应的帮助键
    /// 支持基于泛型类型的智能匹配，无需手动指定HelpKey
    /// </summary>
    public class SmartHelpResolver
    {
        #region 私有字段

        /// <summary>
        /// 控件前缀列表
        /// 根据UI命名规范，控件名通常以这些前缀开头，后跟字段名
        /// </summary>
        private static readonly string[] ControlPrefixes = 
        {
            "txt",      // TextBox - 文本输入框
            "cmb",      // ComboBox - 下拉选择框
            "lbl",      // Label - 标签
            "dtp",      // DateTimePicker - 日期时间选择器
            "chk",      // CheckBox - 复选框
            "num",      // NumericUpDown - 数字输入框
            "btn",      // Button - 按钮
            "rdo",      // RadioButton - 单选按钮
            "lst",      // ListBox - 列表框
            "trv",      // TreeView - 树形控件
            "grd",      // GridView/DataGrid - 网格控件
            "pic",      // PictureBox - 图片框
            "lnk",      // LinkLabel - 链接标签
            "txtMemo",  // MemoTextBox - 多行文本框
            "txtSearch" // SearchTextBox - 搜索框
        };

        /// <summary>
        /// 实体类型缓存
        /// 键: 窗体类型, 值: 主实体类型
        /// </summary>
        private readonly Dictionary<Type, Type> _entityTypeCache = new Dictionary<Type, Type>();

        /// <summary>
        /// 实体属性缓存
        /// 键: 实体类型, 值: 属性名称列表
        /// </summary>
        private readonly Dictionary<Type, HashSet<string>> _entityPropertiesCache = 
            new Dictionary<Type, HashSet<string>>();

        /// <summary>
        /// 缓存锁对象
        /// </summary>
        private readonly object _cacheLock = new object();

        #endregion

        #region 公共方法 - 智能解析

        /// <summary>
        /// 从控件智能解析帮助键
        /// 自动分析控件所在的窗体、绑定的实体和字段信息
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <returns>解析出的帮助键列表（按优先级排序），如果没有匹配则返回空列表</returns>
        /// <example>
        /// 输入: cmbCustomerVendor_ID 在 UCSaleOrder 中
        /// 输出: ["Fields.tb_SaleOrder.CustomerVendor_ID", "Controls.UCSaleOrder.cmbCustomerVendor_ID"]
        /// </example>
        public List<string> ResolveHelpKeys(Control control)
        {
            if (control == null)
            {
                return new List<string>();
            }

            var result = new List<string>();

            try
            {
                // 1. 尝试从控件的Tag中获取手动指定的HelpKey（最高优先级）
                string manualHelpKey = ExtractManualHelpKey(control);
                if (!string.IsNullOrEmpty(manualHelpKey))
                {
                    result.Add(manualHelpKey);
                    return result; // 有手动指定则直接返回
                }

                // 2. 尝试从DataBindings提取字段信息（优先级最高）
                string fieldHelpKey = ExtractFromDataBindings(control);
                if (!string.IsNullOrEmpty(fieldHelpKey))
                {
                    result.Add(fieldHelpKey);
                }

                // 3. 尝试从控件名智能匹配实体字段（中等优先级）
                string smartFieldHelpKey = ExtractFromControlName(control);
                if (!string.IsNullOrEmpty(smartFieldHelpKey) && !result.Contains(smartFieldHelpKey))
                {
                    result.Add(smartFieldHelpKey);
                }

                // 4. 生成控件级别的帮助键（低优先级）
                string controlHelpKey = GenerateControlHelpKey(control);
                if (!string.IsNullOrEmpty(controlHelpKey))
                {
                    result.Add(controlHelpKey);
                }

                // 5. 生成窗体级别的帮助键（兜底）
                string formHelpKey = GenerateFormHelpKey(control);
                if (!string.IsNullOrEmpty(formHelpKey))
                {
                    result.Add(formHelpKey);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"智能解析帮助键失败: {control.Name}, {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// 从窗体智能解析实体类型
        /// 提取BaseBillEditGeneric<T, C>中的泛型参数T（主实体类型）
        /// </summary>
        /// <param name="formType">窗体类型</param>
        /// <returns>主实体类型，无法解析则返回null</returns>
        /// <example>
        /// 输入: typeof(UCSaleOrder)
        /// 输出: typeof(tb_SaleOrder)
        /// </example>
        public Type ResolveEntityType(Type formType)
        {
            if (formType == null)
            {
                return null;
            }

            // 检查缓存
            if (_entityTypeCache.TryGetValue(formType, out Type cachedEntityType))
            {
                return cachedEntityType;
            }

            try
            {
                // 查找基类链中的BaseBillEditGeneric<T, C>
                Type baseType = formType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.IsGenericType && 
                        baseType.GetGenericTypeDefinition() == typeof(BaseBillEditGeneric<,>))
                    {
                        // 获取第一个泛型参数（主实体类型）
                        Type entityType = baseType.GetGenericArguments()[0];
                        
                        // 缓存结果
                        lock (_cacheLock)
                        {
                            _entityTypeCache[formType] = entityType;
                        }

                        System.Diagnostics.Debug.WriteLine(
                            $"从窗体 {formType.Name} 解析出实体类型: {entityType.Name}");

                        return entityType;
                    }

                    baseType = baseType.BaseType;
                }

                // 尝试从BaseEditGeneric<T>解析
                baseType = formType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.IsGenericType && 
                        baseType.GetGenericTypeDefinition() == typeof(BaseEditGeneric<>))
                    {
                        Type entityType = baseType.GetGenericArguments()[0];
                        
                        lock (_cacheLock)
                        {
                            _entityTypeCache[formType] = entityType;
                        }

                        return entityType;
                    }

                    baseType = baseType.BaseType;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析实体类型失败: {formType.Name}, {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 从控件名提取字段名
        /// 去除控件前缀后得到字段名
        /// 支持特殊映射，如txtOrderNo → SOrderNo
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>字段名，无法提取则返回null</returns>
        /// <example>
        /// 输入: "cmbCustomerVendor_ID"
        /// 输出: "CustomerVendor_ID"
        /// </example>
        public string ExtractFieldNameFromControlName(string controlName)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                return null;
            }

            // 处理特殊映射
            switch (controlName)
            {
                case "txtOrderNo":
                    return "SOrderNo"; // 销售订单编号特殊映射
            }

            foreach (string prefix in ControlPrefixes)
            {
                if (controlName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    string fieldName = controlName.Substring(prefix.Length);
                    return fieldName;
                }
            }

            return null; // 没有匹配的前缀
        }

        /// <summary>
        /// 检查实体是否包含指定字段
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>如果实体包含该字段则返回true</returns>
        public bool EntityHasField(Type entityType, string fieldName)
        {
            if (entityType == null || string.IsNullOrEmpty(fieldName))
            {
                return false;
            }

            // 从缓存获取属性列表
            HashSet<string> properties;
            if (!_entityPropertiesCache.TryGetValue(entityType, out properties))
            {
                // 缓存未命中，加载属性
                properties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                
                foreach (PropertyInfo prop in entityType.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    properties.Add(prop.Name);
                }

                lock (_cacheLock)
                {
                    _entityPropertiesCache[entityType] = properties;
                }
            }

            return properties.Contains(fieldName);
        }

        #endregion

        #region 私有方法 - 数据提取

        /// <summary>
        /// 从控件的Tag提取手动指定的HelpKey
        /// </summary>
        private string ExtractManualHelpKey(Control control)
        {
            if (control.Tag != null && control.Tag is string tagString)
            {
                if (tagString.StartsWith("HelpKey:", StringComparison.OrdinalIgnoreCase))
                {
                    return tagString.Substring("HelpKey:".Length);
                }
            }

            return null;
        }

        /// <summary>
        /// 从DataBindings提取字段信息
        /// </summary>
        private string ExtractFromDataBindings(Control control)
        {
            if (control.DataBindings == null || control.DataBindings.Count == 0)
            {
                return null;
            }

            try
            {
                var binding = control.DataBindings[0];
                string fieldName = binding.BindingMemberInfo.BindingField;

                if (string.IsNullOrEmpty(fieldName))
                {
                    return null;
                }

                // 尝试获取实体类型
                Type entityType = null;

                // 从BindingManagerBase.Current获取
                if (binding.BindingManagerBase?.Current != null)
                {
                    entityType = binding.BindingManagerBase.Current.GetType();
                }

                // 如果没有获取到，尝试从窗体解析
                if (entityType == null || !typeof(BaseEntity).IsAssignableFrom(entityType))
                {
                    Form form = control.FindForm();
                    if (form != null)
                    {
                        entityType = ResolveEntityType(form.GetType());
                    }
                }

                // 生成字段级帮助键
                if (entityType != null)
                {
                    string helpKey = $"Fields.{entityType.Name}.{fieldName}";
                    System.Diagnostics.Debug.WriteLine(
                        $"从DataBindings解析: {helpKey}");
                    return helpKey;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"从DataBindings提取帮助键失败: {control.Name}, {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 从控件名智能匹配实体字段
        /// </summary>
        private string ExtractFromControlName(Control control)
        {
            if (string.IsNullOrEmpty(control.Name))
            {
                return null;
            }

            // 提取字段名
            string fieldName = ExtractFieldNameFromControlName(control.Name);
            if (string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            // 获取实体类型
            Form form = control.FindForm();
            if (form == null)
            {
                return null;
            }

            Type entityType = ResolveEntityType(form.GetType());
            if (entityType == null)
            {
                return null;
            }

            // 验证字段是否存在于实体中
            if (!EntityHasField(entityType, fieldName))
            {
                System.Diagnostics.Debug.WriteLine(
                    $"控件 {control.Name} 映射的字段 {fieldName} 在实体 {entityType.Name} 中不存在");
                return null;
            }

            // 生成字段级帮助键
            string helpKey = $"Fields.{entityType.Name}.{fieldName}";
            System.Diagnostics.Debug.WriteLine(
                $"从控件名智能匹配: {helpKey}");
            return helpKey;
        }

        /// <summary>
        /// 生成控件级别的帮助键
        /// </summary>
        private string GenerateControlHelpKey(Control control)
        {
            Form form = control.FindForm();
            if (form == null)
            {
                return null;
            }

            string formName = form.GetType().Name;
            return $"Controls.{formName}.{control.Name}";
        }

        /// <summary>
        /// 生成窗体级别的帮助键
        /// </summary>
        private string GenerateFormHelpKey(Control control)
        {
            Form form = control.FindForm();
            if (form == null)
            {
                return null;
            }

            return $"Forms.{form.GetType().Name}";
        }

        #endregion

        #region 公共方法 - 调试与诊断

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_cacheLock)
            {
                _entityTypeCache.Clear();
                _entityPropertiesCache.Clear();
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public string GetCacheStatistics()
        {
            lock (_cacheLock)
            {
                return $"SmartHelpResolver 缓存统计:\n" +
                       $"  - 实体类型缓存: {_entityTypeCache.Count}\n" +
                       $"  - 实体属性缓存: {_entityPropertiesCache.Count}";
            }
        }

        #endregion
    }
}
