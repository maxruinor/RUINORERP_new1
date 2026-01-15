// **********************************************************************************
// 智能帮助解析器增强版
// 适配特殊架构：通过依赖注入容器Startup.GetFromFacByName<T>()反射生成实例
// 支持4种主要窗体类型：
// 1. BaseBillEditGeneric<T, C> - 单据类编辑
// 2. BaseBillQueryMC<M, C> - 单据类查询
// 3. BaseEditGeneric<T> - 基础信息编辑
// 4. BaseListGeneric<T> - 基础信息列表
//
// 作者: 智能帮助系统
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
    /// 智能帮助解析器增强版
    /// 支持通过菜单配置(BIBaseForm)识别窗体类型和泛型参数
    /// 适配Startup.GetFromFacByName<T>()反射实例化模式
    /// </summary>
    public class SmartHelpResolverEnhanced
    {
        #region 私有字段

        /// <summary>
        /// 控件前缀列表
        /// </summary>
        private static readonly string[] ControlPrefixes = 
        {
            "txt", "cmb", "lbl", "dtp", "chk", "num", "btn", 
            "rdo", "lst", "trv", "grd", "pic", "lnk"
        };

        /// <summary>
        /// 窗体类型缓存
        /// 键: 窗体实例, 值: 窗体类型信息
        /// </summary>
        private readonly Dictionary<Control, FormTypeInfo> _formTypeInfoCache = 
            new Dictionary<Control, FormTypeInfo>();

        /// <summary>
        /// 泛型类型缓存
        /// 键: 窗体类型, 值: 泛型参数映射
        /// </summary>
        private readonly Dictionary<Type, Dictionary<int, Type>> _genericTypesCache = 
            new Dictionary<Type, Dictionary<int, Type>>();

        /// <summary>
        /// 实体类型缓存
        /// 键: 窗体类型, 值: 主实体类型
        /// </summary>
        private readonly Dictionary<Type, Type> _entityTypeCache = 
            new Dictionary<Type, Type>();

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
        /// 从控件智能解析帮助键（增强版）
        /// 支持通过菜单配置识别窗体类型
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="menuInfo">菜单信息（可选）</param>
        /// <returns>解析出的帮助键列表（按优先级排序）</returns>
        public List<string> ResolveHelpKeys(Control control, tb_MenuInfo menuInfo = null)
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
                    return result;
                }

                // 2. 获取窗体类型信息
                FormTypeInfo formTypeInfo = GetFormTypeInfo(control, menuInfo);
                if (formTypeInfo == null)
                {
                    return result;
                }

                // 3. 尝试从DataBindings提取字段信息（优先级最高）
                string fieldHelpKey = ExtractFromDataBindings(control, formTypeInfo);
                if (!string.IsNullOrEmpty(fieldHelpKey) && !result.Contains(fieldHelpKey))
                {
                    result.Add(fieldHelpKey);
                }

                // 4. 尝试从控件名智能匹配实体字段（中等优先级）
                string smartFieldHelpKey = ExtractFromControlName(control, formTypeInfo);
                if (!string.IsNullOrEmpty(smartFieldHelpKey) && !result.Contains(smartFieldHelpKey))
                {
                    result.Add(smartFieldHelpKey);
                }

                // 5. 生成控件级别的帮助键（低优先级）
                string controlHelpKey = GenerateControlHelpKey(control);
                if (!string.IsNullOrEmpty(controlHelpKey))
                {
                    result.Add(controlHelpKey);
                }

                // 6. 生成窗体级别的帮助键（兜底）
                string formHelpKey = GenerateFormHelpKey(control, formTypeInfo);
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
        /// 从窗体智能解析实体类型（增强版）
        /// 支持通过菜单配置(BIBaseForm)识别泛型参数
        /// </summary>
        /// <param name="formType">窗体类型</param>
        /// <param name="menuInfo">菜单信息（可选）</param>
        /// <returns>主实体类型，无法解析则返回null</returns>
        /// <remarks>
        /// 支持的窗体类型：
        /// 1. BaseBillEditGeneric<T, C> - 单据类编辑（T为主实体，C为明细实体）
        /// 2. BaseBillQueryMC<M, C> - 单据类查询（M为主实体，C为明细实体）
        /// 3. BaseEditGeneric<T> - 基础信息编辑（T为实体）
        /// 4. BaseListGeneric<T> - 基础信息列表（T为实体）
        /// </remarks>
        public Type ResolveEntityType(Type formType, tb_MenuInfo menuInfo = null)
        {
            if (formType == null)
            {
                return null;
            }

            // 优先从菜单配置中获取实体类型
            if (menuInfo != null && !string.IsNullOrEmpty(menuInfo.EntityType))
            {
                try
                {
                    Type entityType = Type.GetType($"RUINORERP.Model.{menuInfo.EntityType}");
                    if (entityType != null)
                    {
                        return entityType;
                    }
                }
                catch { }
            }

            // 检查缓存
            if (_entityTypeCache.TryGetValue(formType, out Type cachedEntityType))
            {
                return cachedEntityType;
            }

            try
            {
                // 方法1: 从泛型基类提取（适用于直接继承的情况）
                Type entityType = ExtractEntityTypeFromGenericBase(formType);
                if (entityType != null)
                {
                    lock (_cacheLock)
                    {
                        _entityTypeCache[formType] = entityType;
                    }
                    return entityType;
                }

                // 方法2: 从菜单配置的泛型参数提取（适用于反射实例化的情况）
                if (menuInfo != null)
                {
                    entityType = ExtractEntityTypeFromMenuConfig(formType, menuInfo);
                    if (entityType != null)
                    {
                        lock (_cacheLock)
                        {
                            _entityTypeCache[formType] = entityType;
                        }
                        return entityType;
                    }
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
        /// </summary>
        public string ExtractFieldNameFromControlName(string controlName)
        {
            if (string.IsNullOrEmpty(controlName))
            {
                return null;
            }

            foreach (string prefix in ControlPrefixes)
            {
                if (controlName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    string fieldName = controlName.Substring(prefix.Length);
                    return fieldName;
                }
            }

            return null;
        }

        /// <summary>
        /// 检查实体是否包含指定字段
        /// </summary>
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

        /// <summary>
        /// 获取窗体类型信息
        /// </summary>
        public FormTypeInfo GetFormTypeInfo(Control control, tb_MenuInfo menuInfo = null)
        {
            if (control == null)
            {
                return null;
            }

            Form form = control.FindForm();
            if (form == null)
            {
                return null;
            }

            // 检查缓存
            if (_formTypeInfoCache.TryGetValue(control, out FormTypeInfo cachedInfo))
            {
                return cachedInfo;
            }

            // 解析窗体类型信息
            FormTypeInfo info = new FormTypeInfo
            {
                FormType = form.GetType(),
                FormName = form.GetType().Name,
                MenuInfo = menuInfo
            };

            // 识别基类类型
            Type baseType = form.GetType();
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType)
                {
                    Type genericDef = baseType.GetGenericTypeDefinition();
                    Type[] genericArgs = baseType.GetGenericArguments();

                    if (genericDef == typeof(BaseBillEditGeneric<,>))
                    {
                        info.BaseClassType = BaseClassType.BaseBillEditGeneric;
                        info.GenericArgs = genericArgs;
                        info.EntityType = genericArgs[0];
                        info.DetailType = genericArgs[1];
                        break;
                    }
                    else if (genericDef == typeof(BaseBillQueryMC<,>))
                    {
                        info.BaseClassType = BaseClassType.BaseBillQueryMC;
                        info.GenericArgs = genericArgs;
                        info.EntityType = genericArgs[0];
                        info.DetailType = genericArgs[1];
                        break;
                    }
                    else if (genericDef == typeof(BaseEditGeneric<>))
                    {
                        info.BaseClassType = BaseClassType.BaseEditGeneric;
                        info.GenericArgs = genericArgs;
                        info.EntityType = genericArgs[0];
                        break;
                    }
                    else if (genericDef == typeof(BaseListGeneric<>))
                    {
                        info.BaseClassType = BaseClassType.BaseListGeneric;
                        info.GenericArgs = genericArgs;
                        info.EntityType = genericArgs[0];
                        break;
                    }
                }

                baseType = baseType.BaseType;
            }

            // 如果基类类型识别失败，尝试从菜单配置中识别
            if (info.BaseClassType == BaseClassType.Unknown && menuInfo != null)
            {
                if (!string.IsNullOrEmpty(menuInfo.BIBaseForm))
                {
                    if (menuInfo.BIBaseForm.Contains("BaseBillEditGeneric"))
                    {
                        info.BaseClassType = BaseClassType.BaseBillEditGeneric;
                    }
                    else if (menuInfo.BIBaseForm.Contains("BaseBillQueryMC"))
                    {
                        info.BaseClassType = BaseClassType.BaseBillQueryMC;
                    }
                    else if (menuInfo.BIBaseForm.Contains("BaseEditGeneric"))
                    {
                        info.BaseClassType = BaseClassType.BaseEditGeneric;
                    }
                    else if (menuInfo.BIBaseForm.Contains("BaseListGeneric"))
                    {
                        info.BaseClassType = BaseClassType.BaseListGeneric;
                    }
                }
            }

            // 缓存结果
            lock (_cacheLock)
            {
                _formTypeInfoCache[control] = info;
            }

            return info;
        }

        #endregion

        #region 私有方法 - 实体类型提取

        /// <summary>
        /// 从泛型基类提取实体类型
        /// </summary>
        private Type ExtractEntityTypeFromGenericBase(Type formType)
        {
            Type baseType = formType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType)
                {
                    Type genericDef = baseType.GetGenericTypeDefinition();
                    
                    // 检查是否是支持的泛型基类
                    if (genericDef == typeof(BaseBillEditGeneric<,>) ||
                        genericDef == typeof(BaseBillQueryMC<,>) ||
                        genericDef == typeof(BaseEditGeneric<>) ||
                        genericDef == typeof(BaseListGeneric<>))
                    {
                        // 返回第一个泛型参数（主实体类型）
                        return baseType.GetGenericArguments()[0];
                    }
                }

                baseType = baseType.BaseType;
            }

            return null;
        }

        /// <summary>
        /// 从菜单配置提取实体类型
        /// 适用于反射实例化的情况（Startup.GetFromFacByName<T>()）
        /// </summary>
        private Type ExtractEntityTypeFromMenuConfig(Type formType, tb_MenuInfo menuInfo)
        {
            if (menuInfo == null)
            {
                return null;
            }

            try
            {
                // 方法1: 从菜单配置的EntityType属性获取
                if (!string.IsNullOrEmpty(menuInfo.EntityType))
                {
                    Type entityType = Type.GetType($"RUINORERP.Model.{menuInfo.EntityType}");
                    if (entityType != null)
                    {
                        return entityType;
                    }
                }

                // 方法2: 从FormName推断实体类型
                // 例如: UCSaleOrder → tb_SaleOrder
                if (!string.IsNullOrEmpty(menuInfo.FormName))
                {
                    string entityTypeName = "tb_" + menuInfo.FormName;
                    Type entityType = Type.GetType($"RUINORERP.Model.{entityTypeName}");
                    if (entityType != null)
                    {
                        return entityType;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从菜单配置提取实体类型失败: {ex.Message}");
            }

            return null;
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
        private string ExtractFromDataBindings(Control control, FormTypeInfo formTypeInfo)
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
                Type entityType = formTypeInfo?.EntityType;

                // 如果从DataBindings获取到了实体类型，使用它
                if (binding.BindingManagerBase?.Current != null)
                {
                    Type boundType = binding.BindingManagerBase.Current.GetType();
                    if (typeof(BaseEntity).IsAssignableFrom(boundType))
                    {
                        entityType = boundType;
                    }
                }

                // 生成字段级帮助键
                if (entityType != null)
                {
                    string helpKey = $"Fields.{entityType.Name}.{fieldName}";
                    System.Diagnostics.Debug.WriteLine($"从DataBindings解析: {helpKey}");
                    return helpKey;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从DataBindings提取帮助键失败: {control.Name}, {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 从控件名智能匹配实体字段
        /// </summary>
        private string ExtractFromControlName(Control control, FormTypeInfo formTypeInfo)
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
            Type entityType = formTypeInfo?.EntityType;
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
            System.Diagnostics.Debug.WriteLine($"从控件名智能匹配: {helpKey}");
            return helpKey;
        }

        /// <summary>
        /// 生成控件级别的帮助键
        /// </summary>
        private string GenerateControlHelpKey(Control control)
        {
            string formName = control.FindForm()?.GetType().Name;
            if (string.IsNullOrEmpty(formName))
            {
                return null;
            }

            return $"Controls.{formName}.{control.Name}";
        }

        /// <summary>
        /// 生成窗体级别的帮助键
        /// </summary>
        private string GenerateFormHelpKey(Control control, FormTypeInfo formTypeInfo)
        {
            string formName = formTypeInfo?.FormName ?? control.FindForm()?.GetType().Name;
            if (string.IsNullOrEmpty(formName))
            {
                return null;
            }

            return $"Forms.{formName}";
        }

        #endregion

        #region 公共方法 - 缓存管理

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            lock (_cacheLock)
            {
                _formTypeInfoCache.Clear();
                _genericTypesCache.Clear();
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
                return $"SmartHelpResolverEnhanced 缓存统计:\n" +
                       $"  - 窗体类型缓存: {_formTypeInfoCache.Count}\n" +
                       $"  - 泛型类型缓存: {_genericTypesCache.Count}\n" +
                       $"  - 实体类型缓存: {_entityTypeCache.Count}\n" +
                       $"  - 实体属性缓存: {_entityPropertiesCache.Count}";
            }
        }

        #endregion
    }

    #region 辅助类和枚举

    /// <summary>
    /// 基类类型枚举
    /// </summary>
    public enum BaseClassType
    {
        Unknown = 0,
        BaseBillEditGeneric,      // 单据类编辑
        BaseBillQueryMC,          // 单据类查询
        BaseEditGeneric,          // 基础信息编辑
        BaseListGeneric           // 基础信息列表
    }

    /// <summary>
    /// 窗体类型信息
    /// </summary>
    public class FormTypeInfo
    {
        /// <summary>
        /// 窗体类型
        /// </summary>
        public Type FormType { get; set; }

        /// <summary>
        /// 窗体名称
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// 基类类型
        /// </summary>
        public BaseClassType BaseClassType { get; set; } = BaseClassType.Unknown;

        /// <summary>
        /// 泛型参数
        /// </summary>
        public Type[] GenericArgs { get; set; }

        /// <summary>
        /// 主实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 明细实体类型（单据类）
        /// </summary>
        public Type DetailType { get; set; }

        /// <summary>
        /// 菜单信息
        /// </summary>
        public tb_MenuInfo MenuInfo { get; set; }
    }

    #endregion
}
