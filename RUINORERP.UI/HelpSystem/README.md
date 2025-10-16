# 控件级别帮助系统使用说明

## 概述

本帮助系统扩展了原有的窗体级别帮助功能，增加了控件级别的帮助支持。用户现在可以通过以下方式获取帮助：

1. 按F1键获取当前焦点控件或窗体的帮助
2. 按F2键打开帮助系统主窗体
3. 使用上下文菜单中的帮助选项
4. 通过编程方式为特定控件设置帮助内容

## 使用方法

### 1. 启用窗体帮助功能

所有继承自`frmBase`的窗体都自动启用了F1帮助功能。

对于普通窗体，可以手动启用：

```csharp
public partial class MyForm : Form
{
    public MyForm()
    {
        InitializeComponent();
        // 启用F1帮助功能
        this.EnableF1Help();
    }
}
```

### 2. 为窗体设置帮助页面

```csharp
// 为窗体设置帮助页面
this.SetHelpPage("forms/my_form.html", "我的窗体帮助");
```

### 3. 为控件设置帮助键

```csharp
// 为按钮设置特定帮助键
Button btnSave = new Button();
btnSave.SetControlHelpKey("button_save");

// 为文本框设置特定帮助键
TextBox txtName = new TextBox();
txtName.SetControlHelpKey("textbox_name");
```

### 4. 实现IHelpProvider接口（可选）

窗体可以实现`IHelpProvider`接口来动态提供帮助页面：

```csharp
public class MyForm : Form, IHelpProvider
{
    public string GetHelpPage()
    {
        return "forms/dynamic_help.html";
    }
    
    public string GetHelpTitle()
    {
        return "动态帮助";
    }
}
```

## 控件级别帮助详解

### 控件帮助的工作原理

1. 当用户按下F1键时，系统首先检查当前焦点控件是否设置了帮助键
2. 如果控件有帮助键，则显示该控件的特定帮助内容
3. 如果控件没有帮助键，则根据控件类型和名称进行智能匹配
4. 如果无法匹配到控件帮助，则显示窗体级别的帮助

### 控件帮助键的设置

使用`SetControlHelpKey`扩展方法为控件设置帮助键：

```csharp
// 为按钮设置帮助键
Button saveButton = new Button();
saveButton.SetControlHelpKey("button_save");

// 为文本框设置帮助键
TextBox nameTextBox = new TextBox();
nameTextBox.SetControlHelpKey("textbox_name");
```

### 智能控件帮助匹配

系统内置了智能匹配规则，可以根据控件类型和名称自动匹配帮助内容：

- 按钮控件：
  - 名称包含"save"的按钮：`controls/button_save.html`
  - 名称包含"delete"的按钮：`controls/button_delete.html`
  - 名称包含"add"的按钮：`controls/button_add.html`
  - 名称包含"edit"的按钮：`controls/button_edit.html`
  - 名称包含"cancel"的按钮：`controls/button_cancel.html`
  - 其他按钮：`controls/button_general.html`

- 文本框控件：`controls/textbox_general.html`
- 下拉框控件：`controls/combobox_general.html`
- 数据网格控件：`controls/grid_general.html`
- 标签控件：`controls/label_general.html`
- 复选框控件：`controls/checkbox_general.html`
- 单选框控件：`controls/radiobutton_general.html`
- 日期时间控件：`controls/datetimepicker_general.html`
- 数值控件：`controls/numericupdown_general.html`

## 帮助内容组织

### 控件帮助文件组织
- 按钮控件: `controls/button_*.html`
- 文本框控件: `controls/textbox_*.html`
- 下拉框控件: `controls/combobox_*.html`
- 数据网格控件: `controls/grid_*.html`
- 标签控件: `controls/label_*.html`
- 复选框控件: `controls/checkbox_*.html`
- 单选框控件: `controls/radiobutton_*.html`
- 日期时间控件: `controls/datetimepicker_*.html`
- 数值控件: `controls/numericupdown_*.html`

### 窗体帮助文件组织
- 基础数据窗体: `basics/*.html`
- 单据窗体: `documents/*.html`
- 列表窗体: `lists/*.html`
- 其他窗体: `general/*.html`

## 帮助系统增强功能

### 帮助系统配置管理
系统支持通过配置文件管理帮助系统的各项功能：

```csharp
// 获取帮助系统配置
var config = HelpManager.Config;

// 检查是否启用帮助系统
if (config.IsHelpSystemEnabled)
{
    // 执行帮助相关操作
}
```

### 帮助查看历史记录
系统自动记录用户查看的帮助内容：

```csharp
// 获取最近查看的帮助页面
var recentHistory = HelpHistoryManager.GetRecentHistory(10);

// 获取最常查看的帮助页面
var mostViewed = HelpHistoryManager.GetMostViewed(10);
```

### 帮助内容搜索
用户可以通过关键词搜索帮助内容：

```csharp
// 搜索帮助内容
var searchResults = HelpSearchManager.Search("保存按钮");
```

### 智能内容推荐
系统根据用户行为和内容相关性提供智能推荐：

```csharp
// 获取推荐的帮助内容
var recommendations = HelpRecommendationManager.GetRecommendations("controls/button_save.html");
```

### 帮助系统主窗体
用户可以通过F2键或编程方式打开集成的帮助系统主窗体：

```csharp
// 显示帮助系统主窗体
this.ShowHelpSystemForm();
```

## 扩展帮助系统

### 添加新的控件类型支持
在`HelpManager.cs`的`GetSmartControlHelpPage`方法中添加新的控件类型匹配规则。

### 自定义帮助文件路径
可以通过修改`ShowHelpByKey`和`GetSmartControlHelpPage`方法来自定义帮助文件的组织结构。

### 示例：添加新的控件类型支持
```
// 在GetSmartControlHelpPage方法中添加新的匹配规则
private static string GetSmartControlHelpPage(Control control)
{
    string controlType = control.GetType().Name.ToLower();

    // 添加对DateTimePicker控件的支持
    if (controlType.Contains("datetimepicker"))
    {
        return "controls/datetimepicker_general.html";
    }
    
    if (controlType.Contains("button"))
    {
        if (control.Name.Contains("save"))
        {
            return "controls/button_save.html";
        }
        else if (control.Name.Contains("delete"))
        {
            return "controls/button_delete.html";
        }
        else if (control.Name.Contains("add"))
        {
            return "controls/button_add.html";
        }
        else
        {
            return "controls/button_general.html";
        }
    }
    else if (controlType.Contains("textbox"))
    {
        return "controls/textbox_general.html";
    }
    else if (controlType.Contains("combobox"))
    {
        return "controls/combobox_general.html";
    }
    else if (controlType.Contains("grid"))
    {
        return "controls/grid_general.html";
    }
    else if (controlType.Contains("label"))
    {
        return "controls/label_general.html";
    }
    else if (controlType.Contains("checkbox"))
    {
        return "controls/checkbox_general.html";
    }
    else if (controlType.Contains("radiobutton"))
    {
        return "controls/radiobutton_general.html";
    }
    else
    {
        return null;
    }
}
