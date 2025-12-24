using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using RUINORERP.Business.Cache;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Models;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// GridView显示文本解析器抽象基类，提取共同逻辑
    /// </summary>
    public abstract class AbstractGridViewDisplayTextResolver
    {
        protected GridViewDisplayHelper displayHelper;
        protected Type _entityType;
        
        // 用于存储列的显示类型
        protected Dictionary<string, string> ColumnDisplayTypes { get; set; } = new Dictionary<string, string>();
        
        // 用于存储外键列的映射
        protected Dictionary<string, string> ReferenceKeyColumnMappings { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// 用于存储固定字典值的映射
        /// </summary>
        protected HashSet<FixedDictionaryMapping> FixedDictionaryMappings { get; set; } = new HashSet<FixedDictionaryMapping>();
        
        /// <summary>
        /// 用于存储外键映射
        /// </summary>
        protected HashSet<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new HashSet<ReferenceKeyMapping>();
        
        /// <summary>
        /// 用于存储外键表的列表信息
        /// </summary>
        protected ConcurrentDictionary<string, List<KeyValuePair<string, string>>> ReferenceTableList { get; set; } = new ConcurrentDictionary<string, List<KeyValuePair<string, string>>>();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        protected AbstractGridViewDisplayTextResolver(Type entityType)
        {
            _entityType = entityType;
            displayHelper = new GridViewDisplayHelper();
        }
        
        /// <summary>
        /// 初始化固定字典映射
        /// </summary>
        protected void InitializeFixedDictionaryMappings()
        {
            displayHelper.InitializeFixedDictionaryMappings(_entityType);
        }
        
        /// <summary>
        /// 初始化外键映射
        /// </summary>
        protected void InitializeReferenceKeyMapping()
        {
            displayHelper.InitializeReferenceKeyMapping(_entityType);
        }
        
        /// <summary>
        /// 添加外键映射
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="mapping">外键映射</param>
        public void AddReferenceKeyMapping(string columnName, ReferenceKeyMapping mapping)
        {
            if (!ReferenceKeyMappings.Contains(mapping))
            {
                ReferenceKeyMappings.Add(mapping);
            }
        }
        
        /// <summary>
        /// 手动添加外键关联指向
        /// </summary>
        /// <typeparam name="source">源实体类型</typeparam>
        /// <typeparam name="target">目标实体类型</typeparam>
        /// <param name="SourceField">源字段表达式</param>
        /// <param name="TargetField">目标字段表达式</param>
        /// <param name="CustomDisplaySourceField">自定义显示字段表达式</param>
        public void AddReferenceKeyMapping<source, target>(Expression<Func<source, object>> SourceField, Expression<Func<target, object>> TargetField, Expression<Func<source, object>> CustomDisplaySourceField = null)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            MemberInfo Targetinfo = TargetField.GetMemberInfo();

            if (ReferenceKeyMappings == null)
            {
                ReferenceKeyMappings = new HashSet<ReferenceKeyMapping>();
            }
            
            ReferenceKeyMapping mapping = new ReferenceKeyMapping(typeof(source).Name, Sourceinfo.Name, typeof(target).Name, Targetinfo.Name);
            if (typeof(source).Name == typeof(target).Name)
            {
                mapping.IsSelfReferencing = true;
            }

            var schemaInfo = Startup.GetFromFac<ITableSchemaManager>().GetSchemaInfo(mapping.ReferenceTableName);
            if (schemaInfo != null)
            {
                mapping.ReferenceDefaultDisplayFieldName = schemaInfo.DisplayField;
            }

            if (CustomDisplaySourceField != null)
            {
                MemberInfo CustomDisplayColInfo = CustomDisplaySourceField.GetMemberInfo();
                mapping.CustomDisplayColumnName = CustomDisplayColInfo.Name;
            }
            
            // 检查是否已存在相同的映射，如果存在则先移除再添加
            if (displayHelper.ReferenceKeyMappings.Contains(mapping))
            {
                displayHelper.ReferenceKeyMappings.Remove(mapping);
            }
            // 添加新映射（无论是新增还是更新）
            displayHelper.ReferenceKeyMappings.Add(mapping);
        }
        
        /// <summary>
        /// 添加固定字典映射
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="mappings">键值对映射</param>
        public void AddFixedDictionaryMapping(string tableName, string columnName, List<KeyValuePair<object, string>> mappings)
        {
            if (_entityType.GetProperty(tableName) != null)
            {
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(_entityType.Name, columnName, CommonHelper.Instance.GetKeyValuePairs(typeof(DataStatus))));
                FixedDictionaryMappings.Add(new FixedDictionaryMapping(_entityType.Name, nameof(ApprovalStatus), CommonHelper.Instance.GetKeyValuePairs(typeof(ApprovalStatus))));
            }
        }
        
        /// <summary>
        /// 添加枚举类型的固定字典映射
        /// </summary>
        /// <typeparam name="target">目标实体类型</typeparam>
        /// <param name="TargetField">目标字段表达式</param>
        /// <param name="enumType">枚举类型</param>
        public void AddFixedDictionaryMappingByEnum<target>(Expression<Func<target, object>> TargetField, Type enumType)
        {
            // 使用displayHelper的AddFixedDictionaryMapping方法添加映射
            displayHelper.AddFixedDictionaryMapping(TargetField, enumType);
        }
        
        /// <summary>
        /// 添加列显示类型
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="displayType">显示类型</param>
        public void AddColumnDisplayType(string columnName, string displayType)
        {
            ColumnDisplayTypes[columnName] = displayType;
            displayHelper.ColumnDisplayTypes[columnName] = displayType;
        }
        
        /// <summary>
        /// 添加外键列映射
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="foreignKeyColumnName">外键列名</param>
        public void AddReferenceKeyColumnMapping(string columnName, string foreignKeyColumnName)
        {
            ReferenceKeyColumnMappings[columnName] = foreignKeyColumnName;
            displayHelper.ReferenceKeyColumnMappings[columnName] = foreignKeyColumnName;
        }
        
        /// <summary>
        /// 注册图片字段信息映射
        /// </summary>
        /// <typeparam name="T1">实体类型</typeparam>
        /// <param name="ImageField">图片字段表达式</param>
        /// <param name="UseThumbnail">是否使用缩略图</param>
        /// <param name="IsByteFormat">是否为字节数组格式</param>
        /// <exception cref="ArgumentNullException">当ImageField为空时抛出</exception>
        public void RegisterImageInfoDictionaryMapping<T1>(Expression<Func<T1, object>> ImageField, bool UseThumbnail = true, bool IsByteFormat = true)
        {
            if (ImageField == null)
            {
                throw new ArgumentNullException(nameof(ImageField), "图片字段表达式不能为空");
            }

            try
            {
                MemberInfo ImageFieldInfo = ImageField.GetMemberInfo();
                if (displayHelper.ImagesColumnsMappings == null)
                {
                    displayHelper.ImagesColumnsMappings = new Dictionary<string, (bool IsByteFormat, bool UseThumbnail)>();
                }

                string fieldName = ImageFieldInfo.Name;
                // 检查是否已存在相同的映射，如果存在则先移除再添加
                if (displayHelper.ImagesColumnsMappings.ContainsKey(fieldName))
                {
                    displayHelper.ImagesColumnsMappings.Remove(fieldName);
                }
                // 添加新映射（无论是新增还是更新）
                displayHelper.ImagesColumnsMappings.Add(fieldName, (IsByteFormat, UseThumbnail));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"注册图片字段信息映射失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 处理图片字段显示
        /// </summary>
        /// <param name="e">单元格格式化事件参数</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否处理了图片显示</returns>
        protected bool HandleImageDisplay(DataGridViewCellFormattingEventArgs e, string columnName)
        {
            if (displayHelper.ColumnDisplayTypes.ContainsKey(columnName))
            {
                string displayType = displayHelper.ColumnDisplayTypes[columnName];
                if (displayType == "Image")
                {
                    if (e.Value is byte[])
                    {
                        using (MemoryStream ms = new MemoryStream((byte[])e.Value))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            if (image != null)
                            {
                                //缩略图
                                var thumbnail = UITools.CreateThumbnail(image, 100, 100);
                                e.Value = thumbnail;
                            }
                            e.FormattingApplied = true;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// 获取表格显示文本
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="value">值</param>
        /// <returns>显示文本</returns>
        protected string GetGridViewDisplayText(string tableName, string columnName, object value)
        {
            return displayHelper.GetGridViewDisplayText(tableName, columnName, value);
        }
    }
}