using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using RUINORERP.Model;
using RUINORERP.UI.HelpSystem.Components;

namespace RUINORERP.UI.HelpSystem.Helper
{
    /// <summary>
    /// 帮助内容生成器
    /// 根据实体模型自动生成帮助内容
    /// </summary>
    public static class HelpContentGenerator
    {
        /// <summary>
        /// 生成实体字段帮助内容
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns>字段帮助内容字典</returns>
        public static Dictionary<string, string> GenerateFieldHelpContent(Type entityType)
        {
            var fieldHelpContents = new Dictionary<string, string>();
            
            // 获取实体的所有公共属性
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                var propertyName = property.Name;
                
                // 获取属性的特性信息
                var columnAttr = property.GetCustomAttribute<SugarColumnAttribute>();
                var descAttr = property.GetCustomAttribute<DescriptionAttribute>();
                var advQueryAttr = property.GetCustomAttribute<AdvQueryAttribute>();
                
                var fieldName = FieldNameRecognizer.Recognize(propertyName);
                var fieldDescription = descAttr?.Description ?? advQueryAttr?.ColDesc ?? fieldName;
                
                var content = GenerateSingleFieldHelp(propertyName, fieldName, fieldDescription, columnAttr);
                fieldHelpContents[$"field.{entityType.Name}.{propertyName}"] = content;
            }

            return fieldHelpContents;
        }

        /// <summary>
        /// 生成单个字段的帮助内容
        /// </summary>
        private static string GenerateSingleFieldHelp(string propertyName, string fieldName, string fieldDescription, SugarColumnAttribute columnAttr)
        {
            var content = $@"# {fieldName}

## 字段说明
{fieldDescription}

## 基本信息
- **字段名称**: {fieldDescription}  ({propertyName})
- **字段类型**: {(columnAttr != null ? GetColumnType(columnAttr) : "未知")}
- **是否必填**: {(columnAttr != null ? (columnAttr.IsNullable ? "否" : "是") : "未知")}
- **最大长度**: {(columnAttr != null ? GetColumnLength(columnAttr) : "未知")}
{(columnAttr != null && !string.IsNullOrEmpty(columnAttr.ColumnDescription) ? "- **列描述**: " + columnAttr.ColumnDescription : "")}

## 使用说明
该字段用于输入或选择{fieldName}。{(columnAttr != null && !columnAttr.IsNullable ? "此字段为必填字段，不能为空。" : "")}

## 注意事项
- {(columnAttr != null && !columnAttr.IsNullable ? "请确保输入有效的" + fieldName + "信息" : "")}
- 输入内容应符合业务规范
- 数据格式需满足系统要求

## 相关帮助
- 如需更多信息，请联系系统管理员
- 该字段的具体业务含义请参考相关业务文档

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";

            return content;
        }

        /// <summary>
        /// 生成窗体帮助内容
        /// </summary>
        /// <param name="formName">窗体名称</param>
        /// <param name="entityType">关联实体类型</param>
        /// <returns>窗体帮助内容</returns>
        public static string GenerateFormHelpContent(string formName, Type entityType)
        {
            var entityName = entityType?.Name ?? "未知实体";
            var formDisplayName = FieldNameRecognizer.Recognize(formName);

            var content = $@"# {formDisplayName}

## 窗体概述
本窗体用于{GetFormPurposeHint(formName, entityType)}的操作。

## 窗体信息
- **窗体名称**: {formName}
- **关联实体**: {entityName}

## 基本操作流程

### 新建记录
1. 点击工具栏【新建】按钮或按 【Ctrl+N】
2. 填写必填字段
3. 根据需要填写可选字段
4. 点击【保存】按钮或按 【Ctrl+S】

### 编辑记录
1. 在查询界面找到要编辑的记录
2. 双击打开记录
3. 修改信息
4. 保存记录

### 删除记录
1. 查询要删除的记录
2. 点击【删除】按钮
3. 确认删除操作

## 快捷键
| 快捷键 | 功能说明 |
|--------|----------|
| F1 | 显示帮助 |
| Ctrl+S | 保存 |
| Ctrl+N | 新建 |
| Ctrl+F | 查询 |
| Ctrl+E | 编辑 |
| Del | 删除 |
| ESC | 关闭/取消 |

## 常见问题

### Q: 如何填写必填字段?
A: 必填字段通常以红色星号(*)标记,必须填写后才能保存。

### Q: 数据验证失败怎么办?
A: 检查字段格式是否正确,如日期格式、数字范围等。

### Q: 如何获取更多帮助?
A: 焦点在某个字段时按F1查看该字段的帮助。

## 注意事项
- 该窗体的帮助内容正在完善中
- 如有疑问请参考系统手册或咨询管理员

## 相关信息
- 联系系统管理员获取更多帮助

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";

            return content;
        }

        /// <summary>
        /// 生成模块帮助内容
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块帮助内容</returns>
        public static string GenerateModuleHelpContent(string moduleName)
        {
            var content = $@"# {moduleName}

## 模块概述
{GetModulePurposeHint(moduleName)}模块是RUINORERP系统的重要组成部分，用于管理相关业务流程。

## 主要功能
- 功能1：...
- 功能2：...
- 功能3：...

## 业务流程
1. 流程步骤1
2. 流程步骤2
3. 流程步骤3

## 使用指南
- 操作指引1
- 操作指引2
- 操作指引3

## 注意事项
- 注意事项1
- 注意事项2
- 注意事项3

## 常见问题
### Q: 问题1?
A: 回答1

### Q: 问题2?
A: 回答2

## 相关帮助
- 其他相关模块
- 系统通用操作

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*";

            return content;
        }

        /// <summary>
        /// 获取窗体用途提示
        /// </summary>
        private static string GetFormPurposeHint(string formName, Type entityType)
        {
            if (formName.Contains("Edit") || formName.Contains("Add"))
            {
                return "新建或编辑" + (entityType?.Name ?? "记录");
            }
            else if (formName.Contains("Query") || formName.Contains("Search"))
            {
                return "查询和检索" + (entityType?.Name ?? "记录");
            }
            else if (formName.Contains("View") || formName.Contains("Detail"))
            {
                return "查看" + (entityType?.Name ?? "记录") + "详细信息";
            }
            else
            {
                return "管理" + (entityType?.Name ?? "记录");
            }
        }

        /// <summary>
        /// 获取模块用途提示
        /// </summary>
        private static string GetModulePurposeHint(string moduleName)
        {
            if (moduleName.Contains("销售") || moduleName.Contains("Sale"))
            {
                return "销售";
            }
            else if (moduleName.Contains("采购") || moduleName.Contains("Pur"))
            {
                return "采购";
            }
            else if (moduleName.Contains("库存") || moduleName.Contains("Stock"))
            {
                return "库存";
            }
            else if (moduleName.Contains("财务") || moduleName.Contains("FM"))
            {
                return "财务";
            }
            else
            {
                return moduleName;
            }
        }

        /// <summary>
        /// 获取列类型
        /// </summary>
        private static string GetColumnType(SugarColumnAttribute attr)
        {
            if (attr == null) return "未知";
            
            var dataType = attr.ColumnDataType.ToLower();
            if (dataType.Contains("int"))
            {
                return "整数";
            }
            else if (dataType.Contains("decimal") || dataType.Contains("money") || dataType.Contains("float") || dataType.Contains("double"))
            {
                return "小数";
            }
            else if (dataType.Contains("datetime") || dataType.Contains("date") || dataType.Contains("time"))
            {
                return "日期时间";
            }
            else if (dataType.Contains("bit") || dataType.Contains("bool"))
            {
                return "布尔值";
            }
            else if (dataType.Contains("varchar") || dataType.Contains("nvarchar") || dataType.Contains("char"))
            {
                return "字符串";
            }
            else
            {
                return attr.ColumnDataType;
            }
        }

        /// <summary>
        /// 获取列长度
        /// </summary>
        private static string GetColumnLength(SugarColumnAttribute attr)
        {
            if (attr == null) return "未知";
            
            if (attr.ColumnDataType.Contains("varchar") || attr.ColumnDataType.Contains("nvarchar") || 
                attr.ColumnDataType.Contains("char"))
            {
                return attr.Length.ToString();
            }
            else if (attr.ColumnDataType.Contains("decimal") || attr.ColumnDataType.Contains("numeric"))
            {
                return $"({attr.DecimalDigits})";
            }
            else
            {
                return "N/A";
            }
        }

        /// <summary>
        /// 保存帮助内容到文件
        /// </summary>
        /// <param name="helpContents">帮助内容字典</param>
        /// <param name="basePath">基础路径</param>
        public static void SaveHelpContents(Dictionary<string, string> helpContents, string basePath)
        {
            foreach (var kvp in helpContents)
            {
                var helpKey = kvp.Key;
                var content = kvp.Value;
                
                // 解析帮助键为文件路径
                var filePath = GetHelpFilePath(helpKey, basePath);
                
                // 确保目录存在
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                // 写入文件
                File.WriteAllText(filePath, content);
            }
        }

        /// <summary>
        /// 获取帮助文件路径
        /// </summary>
        private static string GetHelpFilePath(string helpKey, string basePath)
        {
            // 将帮助键转换为文件路径
            // 例如: field.tb_SaleOrder.SOrderNo -> Fields/tb_SaleOrder/SOrderNo.md
            var parts = helpKey.Split('.');
            if (parts.Length >= 3)
            {
                var type = parts[0]; // field, form, module
                var entity = parts[1];
                var field = string.Join(".", parts.Skip(2)); // 如果字段名中有点号，保留它们
                
                string subDir;
                switch (type)
                {
                    case "field":
                        subDir = Path.Combine("Fields", entity);
                        break;
                    case "form":
                        subDir = "Forms";
                        break;
                    case "module":
                        subDir = "Modules";
                        break;
                    default:
                        subDir = type;
                        break;
                }
                
                return Path.Combine(basePath, subDir, $"{field}.md");
            }
            
            // 默认情况
            return Path.Combine(basePath, $"{helpKey}.md");
        }
    }
}