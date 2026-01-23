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
using RUINORERP.Global.EnumExt;
using System.Linq;

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
        protected List<ReferenceKeyMapping> ReferenceKeyMappings { get; set; } = new List<ReferenceKeyMapping>();
        
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
        /// 优先插入到ReferenceKeyMappings集合的前面
        /// </summary>
        /// <typeparam name="source">源实体类型</typeparam>
        /// <typeparam name="target">目标实体类型</typeparam>
        /// <param name="SourceField">源字段表达式</param>
        /// <param name="TargetField">目标字段表达式</param>
        /// <param name="CustomDisplaySourceField">自定义显示字段表达式</param>
        public void AddReferenceKeyMapping<source, target>(Expression<Func<source, object>> SourceField,
            Expression<Func<target, object>> TargetField, Expression<Func<source, object>> CustomDisplaySourceField = null)
        {
            MemberInfo Sourceinfo = SourceField.GetMemberInfo();
            MemberInfo Targetinfo = TargetField.GetMemberInfo();

            if (ReferenceKeyMappings == null)
            {
                ReferenceKeyMappings = new List<ReferenceKeyMapping>();
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
            if (displayHelper.ReferenceKeyMappings.Contains(mapping))
            {
                return;
            }
            // 有CustomDisplayColumnName的插入到前面
            if (!string.IsNullOrEmpty(mapping.CustomDisplayColumnName))
            {
                displayHelper.ReferenceKeyMappings.Insert(0, mapping);
            }
            else
            {
                displayHelper.ReferenceKeyMappings.Add(mapping);
            }
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
        /// <param name="columnDisplayType">列显示类型</param>
        public void AddColumnDisplayType(string columnName, ColumnDisplayTypeEnum columnDisplayType)
        {
            string displayType = columnDisplayType.ToString();
            ColumnDisplayTypes[columnName] = displayType;
            displayHelper.ColumnDisplayTypes[columnName] = displayType;

            // 图片类型自动配置为缩略图模式
            if (columnDisplayType == ColumnDisplayTypeEnum.Image)
            {
                if (displayHelper.ImagesColumnsMappings == null)
                {
                    displayHelper.ImagesColumnsMappings = new Dictionary<string, (bool IsByteFormat, bool UseThumbnail)>();
                }

                // 更新图片列配置：默认使用缩略图，字节数组格式
                if (!displayHelper.ImagesColumnsMappings.ContainsKey(columnName))
                {
                    displayHelper.ImagesColumnsMappings[columnName] = (true, true);
                }
            }
        }

        /// <summary>
        /// 添加图片列显示类型（使用表达式）
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="fieldExpression">字段表达式</param>
        /// <param name="displayType">显示类型</param>
        public void AddColumnDisplayType<TEntity>(Expression<Func<TEntity, object>> fieldExpression, ColumnDisplayTypeEnum displayType)
        {
            if (fieldExpression == null)
            {
                throw new ArgumentNullException(nameof(fieldExpression), "字段表达式不能为空");
            }

            MemberInfo fieldInfo = fieldExpression.GetMemberInfo();
            string columnName = fieldInfo.Name;

            AddColumnDisplayType(columnName, displayType);
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
        /// 处理特殊列显示（图片、二进制数据等）
        /// </summary>
        /// <param name="e">单元格格式化事件参数</param>
        /// <param name="columnName">列名</param>
        /// <returns>是否处理了特殊列显示</returns>
        protected bool HandleSpecialColumnDisplay(DataGridViewCellFormattingEventArgs e, string columnName)
        {
            if (!displayHelper.ColumnDisplayTypes.ContainsKey(columnName))
            {
                return false;
            }

            string displayType = displayHelper.ColumnDisplayTypes[columnName];

            // 处理图片显示（显示缩略图，双击可查看原图）
            if (displayType == ColumnDisplayTypeEnum.Image.ToString())
            {
                if (e.Value is byte[] imageBytes)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            if (image != null)
                            {
                                var thumbnail = UITools.CreateThumbnail(image, 100, 100);
                                e.Value = thumbnail;
                                e.FormattingApplied = true;
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        // 图片处理失败，保持原值
                        return false;
                    }
                }
            }

            // 处理二进制数据显示
            if (displayType == ColumnDisplayTypeEnum.Binary.ToString())
            {
                if (e.Value is byte[] binaryData)
                {
                    e.Value = $"[Binary: {binaryData.Length} bytes]";
                    e.FormattingApplied = true;
                    return true;
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