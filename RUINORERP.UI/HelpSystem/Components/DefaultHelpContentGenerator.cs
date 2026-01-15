using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// 默认帮助内容生成器
    /// 为没有特定帮助内容的控件提供默认的帮助信息
    /// </summary>
    public static class DefaultHelpContentGenerator
    {
        #region 默认帮助模板

        /// <summary>
        /// 生成控件级别的默认帮助内容
        /// </summary>
        /// <param name="control">目标控件</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>默认帮助内容</returns>
        public static string GenerateDefaultControlHelp(Control control, Type entityType = null)
        {
            if (control == null) return string.Empty;

            var entityName = entityType?.Name ?? "未知模块";
            var controlName = GetControlDisplayName(control);
            var fieldName = ExtractFieldName(control.Name);
            var controlType = GetControlType(control);

            return $@"# {controlName}

## 说明
该字段用于{GetControlPurposeHint(control, fieldName)}。

## 字段信息
- **字段标识**: {control.Name}
- **控件类型**: {controlType}
- **所属模块**: {entityName}
- **必填性**: {GetRequiredHint(control)}

## 使用方法
{GetControlUsageHint(control, fieldName)}

## 注意事项
{GetControlAttentionHint(control, fieldName)}

## 操作提示
- 焦点在该字段时,按 **F1** 查看详细帮助
- 可通过鼠标悬停查看快捷提示
- 必填字段通常以红色星号(*)标记

## 相关信息
- 如需更多帮助,请联系系统管理员
- 该字段的详细帮助内容正在完善中

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*
";
        }

        /// <summary>
        /// 生成窗体级别的默认帮助内容
        /// </summary>
        /// <param name="formType">窗体类型</param>
        /// <param name="entityType">实体类型</param>
        /// <returns>默认帮助内容</returns>
        public static string GenerateDefaultFormHelp(Type formType, Type entityType = null)
        {
            if (formType == null) return string.Empty;

            var formName = formType.Name;
            var entityName = entityType?.Name ?? "未知实体";
            var formDisplayName = GetFormDisplayName(formType);

            return $@"# {formDisplayName}

## 窗体概述
本窗体用于{GetFormPurposeHint(formType)}的操作。

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
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*
";
        }

        /// <summary>
        /// 生成全局默认帮助内容
        /// </summary>
        /// <returns>全局默认帮助内容</returns>
        public static string GenerateGlobalHelp()
        {
            return $@"# RUINORERP 系统帮助

欢迎使用 RUINORERP 企业资源计划系统!

## 系统概述
本系统是一个功能完善的企业资源计划系统,涵盖进销存、生产管理、财务管理等核心业务模块,为企业管理提供全面的解决方案。

## 帮助导航
- 按下 **F1** 键查看当前控件或窗体的帮助
- 鼠标悬停在控件上查看快捷提示
- 帮助内容会根据您的操作自动匹配

## 主要模块

### 1. 销售管理
- 销售订单
- 发货管理
- 收款管理

### 2. 采购管理
- 采购订单
- 入库管理
- 付款管理

### 3. 库存管理
- 入库单
- 出库单
- 库存盘点

### 4. 生产管理
- 生产订单
- 领料管理
- 完工管理

### 5. 财务管理
- 应收应付
- 财务报表

## 快捷键大全
| 快捷键 | 功能说明 |
|--------|----------|
| F1 | 显示帮助 |
| Ctrl+S | 保存数据 |
| Ctrl+N | 新建记录 |
| Ctrl+F | 查询数据 |
| Ctrl+E | 编辑记录 |
| Del | 删除记录 |
| ESC | 关闭/取消 |

## 常见问题

### Q: 如何快速查找功能?
A: 使用顶部菜单或快捷键 Ctrl+F 查询。

### Q: 数据保存失败怎么办?
A: 检查必填字段是否填写,字段格式是否正确。

### Q: 如何获取技术支持?
A: 联系系统管理员或技术支持团队。

## 技术支持
如有任何问题,请联系:
- 系统管理员: 联系IT部门
- 技术支持: 根据企业联系方式

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*
";
        }

        /// <summary>
        /// 生成帮助内容缺失提示
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        /// <returns>缺失提示内容</returns>
        public static string GenerateMissingHelpPlaceholder(string helpKey)
        {
            return $@"# 帮助内容暂缺

当前没有找到帮助内容: **{helpKey}**

## 原因
该字段或窗体的帮助内容正在编写中。

## 临时解决方案
1. 查看系统操作手册
2. 联系系统管理员
3. 参考类似功能的使用方法

## 反馈
如果您需要此帮助内容的优先完善,请联系:
- 系统管理员
- 技术支持团队

## 操作提示
- 尝试在父级窗体或模块中查找帮助
- 使用系统默认帮助作为参考

---
*帮助版本: 1.0 | 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*
";
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取控件的显示名称
        /// </summary>
        private static string GetControlDisplayName(Control control)
        {
            if (control == null) return "未知控件";

            // 尝试从控件属性获取显示名称
            if (control is TextBox txt)
            {
                // 标准WinForms TextBox没有PlaceholderText属性，尝试从Tag或AccessibleDescription获取提示
                if (!string.IsNullOrEmpty(txt.Tag?.ToString()))
                {
                    return txt.Tag.ToString();
                }
                if (!string.IsNullOrEmpty(txt.AccessibleDescription))
                {
                    return txt.AccessibleDescription;
                }
            }
            
            if (!string.IsNullOrEmpty(control.Text))
            {
                // 如果是Label控件,返回Text
                if (control is Label)
                {
                    return control.Text;
                }
            }

            // 从控件名称推断显示名称
            return ExtractFieldName(control.Name);
        }

        /// <summary>
        /// 从控件名称提取字段名称
        /// </summary>
        private static string ExtractFieldName(string controlName)
        {
            if (string.IsNullOrEmpty(controlName)) return "未知字段";

            // 使用简单的字段名称处理
            return ProcessFieldName(controlName);
        }

        /// <summary>
        /// 获取控件类型
        /// </summary>
        private static string GetControlType(Control control)
        {
            if (control == null) return "未知";

            // WinForms 标准控件
            if (control is TextBox) return "文本框";
            if (control is ComboBox) return "下拉框";
            if (control is DateTimePicker) return "日期选择器";
            if (control is CheckBox) return "复选框";
            if (control is RadioButton) return "单选按钮";
            if (control is NumericUpDown) return "数字输入框";
            if (control is Label) return "标签";
            if (control is Button) return "按钮";
            if (control is GroupBox) return "分组框";
            if (control is Panel) return "面板";

            // Krypton.Toolkit 控件
            string typeName = control.GetType().Name;

            if (typeName.Contains("KryptonTextBox")) return "文本框";
            if (typeName.Contains("KryptonComboBox")) return "下拉框";
            if (typeName.Contains("KryptonDateTimePicker")) return "日期选择器";
            if (typeName.Contains("KryptonCheckBox")) return "复选框";
            if (typeName.Contains("KryptonRadioButton")) return "单选按钮";
            if (typeName.Contains("KryptonNumericUpDown")) return "数字输入框";
            if (typeName.Contains("KryptonLabel")) return "标签";
            if (typeName.Contains("KryptonButton")) return "按钮";
            if (typeName.Contains("KryptonGroupBox")) return "分组框";
            if (typeName.Contains("KryptonPanel")) return "面板";
            if (typeName.Contains("KryptonDataGridView")) return "数据表格";
            if (typeName.Contains("KryptonNavigator")) return "导航器";
            if (typeName.Contains("KryptonWorkspace")) return "工作区";
            if (typeName.Contains("KryptonDropButton")) return "下拉按钮";
            if (typeName.Contains("KryptonMaskedTextBox")) return "掩码文本框";
            if (typeName.Contains("KryptonRichTextBox")) return "富文本框";

            return control.GetType().Name;
        }

        /// <summary>
        /// 获取必填提示
        /// </summary>
        private static string GetRequiredHint(Control control)
        {
            // 检查控件是否有Required相关标记
            // 这里可以扩展,根据实际的标记方式判断
            if (control.Name.Contains("ID") || control.Name.Contains("No") || control.Name.Contains("Name"))
            {
                return "是 (通常必填)";
            }
            return "否";
        }

        /// <summary>
        /// 获取控件用途提示
        /// </summary>
        private static string GetControlPurposeHint(Control control, string fieldName)
        {
            if (control == null || string.IsNullOrEmpty(fieldName)) return "操作";

            string typeName = control.GetType().Name;

            // 文本框（标准 + Krypton）
            if (control is TextBox || typeName.Contains("KryptonTextBox") ||
                typeName.Contains("KryptonMaskedTextBox") || typeName.Contains("KryptonRichTextBox"))
            {
                return $"输入{fieldName}信息";
            }
            // 下拉框（标准 + Krypton）
            else if (control is ComboBox || typeName.Contains("KryptonComboBox"))
            {
                return $"选择{fieldName}选项";
            }
            // 日期选择器（标准 + Krypton）
            else if (control is DateTimePicker || typeName.Contains("KryptonDateTimePicker"))
            {
                return $"选择{fieldName}日期";
            }
            // 复选框（标准 + Krypton）
            else if (control is CheckBox || typeName.Contains("KryptonCheckBox"))
            {
                return $"设置{fieldName}选项";
            }
            // 数字输入框（标准 + Krypton）
            else if (control is NumericUpDown || typeName.Contains("KryptonNumericUpDown"))
            {
                return $"输入{fieldName}数值";
            }
            // 标签（标准 + Krypton）
            else if (control is Label || typeName.Contains("KryptonLabel"))
            {
                return $"显示{fieldName}信息";
            }
            // 单选按钮（标准 + Krypton）
            else if (control is RadioButton || typeName.Contains("KryptonRadioButton"))
            {
                return $"选择{fieldName}选项";
            }
            // 按钮（标准 + Krypton）
            else if (control is Button || typeName.Contains("KryptonButton") || typeName.Contains("KryptonDropButton"))
            {
                return $"执行{fieldName}操作";
            }
            // 数据表格
            else if (typeName.Contains("KryptonDataGridView"))
            {
                return $"查看和管理{fieldName}数据";
            }
            else
            {
                return $"操作{fieldName}";
            }
        }

        /// <summary>
        /// 获取控件使用提示
        /// </summary>
        private static string GetControlUsageHint(Control control, string fieldName)
        {
            if (control == null) return "根据控件类型进行相应操作";

            string typeName = control.GetType().Name;

            // 文本框（标准 + Krypton）
            if (control is TextBox || typeName.Contains("KryptonTextBox") ||
                typeName.Contains("KryptonMaskedTextBox"))
            {
                return $"1. 点击或使用Tab键进入该字段\n2. 直接输入{fieldName}信息\n3. 按Enter或Tab键切换到下一个字段";
            }
            // 富文本框
            else if (typeName.Contains("KryptonRichTextBox"))
            {
                return $"1. 点击或使用Tab键进入该字段\n2. 输入{fieldName}信息,支持富文本格式\n3. 按Enter或Tab键切换到下一个字段";
            }
            // 下拉框（标准 + Krypton）
            else if (control is ComboBox || typeName.Contains("KryptonComboBox"))
            {
                return $"1. 点击下拉箭头或按Alt+↓打开下拉列表\n2. 选择{fieldName}选项\n3. 也可以直接输入进行筛选";
            }
            // 日期选择器（标准 + Krypton）
            else if (control is DateTimePicker || typeName.Contains("KryptonDateTimePicker"))
            {
                return $"1. 点击日历图标或按Alt+↓打开日期选择器\n2. 选择{fieldName}日期\n3. 也可以手动输入日期";
            }
            // 复选框（标准 + Krypton）
            else if (control is CheckBox || typeName.Contains("KryptonCheckBox"))
            {
                return $"1. 点击复选框或按空格键切换状态\n2. 勾选表示启用{fieldName}\n3. 不勾选表示禁用{fieldName}";
            }
            // 数字输入框（标准 + Krypton）
            else if (control is NumericUpDown || typeName.Contains("KryptonNumericUpDown"))
            {
                return $"1. 点击上下箭头调整数值\n2. 或直接输入{fieldName}数值\n3. 按Enter或Tab键确认";
            }
            // 单选按钮（标准 + Krypton）
            else if (control is RadioButton || typeName.Contains("KryptonRadioButton"))
            {
                return $"1. 点击选项按钮选择{fieldName}\n2. 同组选项只能选择一个\n3. 选择后按Tab键切换";
            }
            // 按钮（标准 + Krypton）
            else if (control is Button || typeName.Contains("KryptonButton"))
            {
                return $"1. 点击按钮执行{fieldName}操作\n2. 部分按钮可能有快捷键\n3. 按钮文字上的下划线表示快捷键";
            }
            // 下拉按钮
            else if (typeName.Contains("KryptonDropButton"))
            {
                return $"1. 点击按钮执行{fieldName}操作\n2. 点击下拉箭头展开更多选项\n3. 根据需要选择操作";
            }
            // 数据表格
            else if (typeName.Contains("KryptonDataGridView"))
            {
                return $"1. 使用鼠标或键盘选择行\n2. 双击行查看详细信息\n3. 使用Ctrl+N新增,Del删除";
            }
            else
            {
                return "根据控件类型进行相应操作";
            }
        }

        /// <summary>
        /// 获取控件注意事项提示
        /// </summary>
        private static string GetControlAttentionHint(Control control, string fieldName)
        {
            if (control == null) return "按照正常的业务规则操作即可";

            var hints = new System.Collections.Generic.List<string>();
            string typeName = control.GetType().Name;

            // 文本框（标准 + Krypton）
            if (control is TextBox || typeName.Contains("KryptonTextBox") ||
                typeName.Contains("KryptonMaskedTextBox"))
            {
                hints.Add($"确保{fieldName}格式正确");
                hints.Add("注意字符长度限制");
            }
            // 富文本框
            else if (typeName.Contains("KryptonRichTextBox"))
            {
                hints.Add($"确保{fieldName}内容格式正确");
                hints.Add("支持富文本格式");
                hints.Add("注意内容长度限制");
            }
            // 下拉框（标准 + Krypton）
            else if (control is ComboBox || typeName.Contains("KryptonComboBox"))
            {
                hints.Add("确保从列表中选择有效选项");
                hints.Add("如果选项不存在,请联系管理员添加");
            }
            // 日期选择器（标准 + Krypton）
            else if (control is DateTimePicker || typeName.Contains("KryptonDateTimePicker"))
            {
                hints.Add($"注意{fieldName}的有效范围");
                hints.Add("日期格式为: 年-月-日");
            }
            // 数字输入框（标准 + Krypton）
            else if (control is NumericUpDown || typeName.Contains("KryptonNumericUpDown"))
            {
                hints.Add($"注意{fieldName}的有效范围");
                hints.Add("可以是整数或小数");
            }
            // 复选框（标准 + Krypton）
            else if (control is CheckBox || typeName.Contains("KryptonCheckBox"))
            {
                hints.Add($"谨慎勾选{fieldName}");
                hints.Add("勾选后可能会影响其他字段");
            }
            // 数据表格
            else if (typeName.Contains("KryptonDataGridView"))
            {
                hints.Add("保存前检查所有必填列");
                hints.Add("使用快捷键提高操作效率");
                hints.Add("删除前确认数据正确性");
            }

            return hints.Count > 0 ? string.Join("\n- ", hints) : "按照正常的业务规则操作即可";
        }

        /// <summary>
        /// 获取窗体显示名称
        /// </summary>
        private static string GetFormDisplayName(Type formType)
        {
            if (formType == null) return "未知窗体";

            // 尝试从MenuAttrAssemblyInfo特性获取名称
            var menuAttr = formType.GetCustomAttributes(false)
                .FirstOrDefault(a => a.GetType().Name.Contains("MenuAttr"));

            if (menuAttr != null)
            {
                try
                {
                    var nameProperty = menuAttr.GetType().GetProperty("Name");
                    if (nameProperty != null)
                    {
                        var name = nameProperty.GetValue(menuAttr)?.ToString();
                        if (!string.IsNullOrEmpty(name))
                        {
                            return name;
                        }
                    }
                }
                catch { }
            }

            // 从窗体类型名称推断显示名称
            string formName = formType.Name
                .Replace("UC", "")
                .Replace("Form", "");

            return System.Text.RegularExpressions.Regex.Replace(formName, "(?<!^)([A-Z])", " $1").Trim();
        }

        /// <summary>
        /// 获取窗体用途提示
        /// </summary>
        private static string GetFormPurposeHint(Type formType)
        {
            string typeName = formType.Name;

            if (typeName.Contains("Edit") || typeName.Contains("UC") && typeName.Contains("Edit"))
            {
                return "编辑和管理数据";
            }
            else if (typeName.Contains("Query"))
            {
                return "查询和筛选数据";
            }
            else if (typeName.Contains("List"))
            {
                return "浏览和管理列表数据";
            }
            else if (typeName.Contains("Statistics"))
            {
                return "统计和分析数据";
            }
            else
            {
                return "处理相关业务";
            }
        }

        /// <summary>
        /// 简单的字段名称处理
        /// </summary>
        private static string ProcessFieldName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return "未知字段";
            
            // 移除常见的前缀
            fieldName = fieldName.Replace("txt", "")
                                  .Replace("cmb", "")
                                  .Replace("lbl", "")
                                  .Replace("chk", "")
                                  .Replace("btn", "")
                                  .Replace("dtp", "")
                                  .Replace("num", "");
            
            // 将驼峰命名转换为空格分隔
            var result = Regex.Replace(fieldName, "(?<!^)([A-Z])", " $1").Trim();
            
            return string.IsNullOrEmpty(result) ? "未知字段" : result;
        }

        #endregion
    }
}
