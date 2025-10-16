# RUINOR ERP 帮助系统说明

## 概述

本目录包含RUINOR ERP系统的帮助文档，这些文档将被编译成CHM格式的帮助文件，供用户在使用系统时查阅。

## 目录结构

```
RUINORERP.Helper/
├── controls/           # 控件帮助文件
├── forms/              # 窗体帮助文件
├── topics/             # 主题帮助文件
├── images/             # 帮助文件中使用的图片
├── *.htm              # 各种帮助内容文件
├── help.hhp           # HTML Help Project文件
├── contents.hhc       # 目录文件
├── index.hhk          # 索引文件
└── compile_help.bat   # 编译脚本
```

## 如何编译帮助文件

1. 确保已安装Microsoft HTML Help Workshop
2. 运行`compile_help.bat`脚本
3. 编译成功后将生成`help.chm`文件

## 帮助文件使用说明

### 快捷键
- **F1**: 显示当前焦点控件或窗体的帮助
- **F2**: 打开帮助系统主窗体

### 帮助内容分类
1. **窗体帮助**: 介绍各个窗体的功能和使用方法
2. **控件帮助**: 介绍各个控件的功能和使用方法
3. **主题帮助**: 介绍系统功能和业务流程

## 维护指南

### 添加新的帮助内容
1. 创建对应的HTML帮助文件
2. 将文件放置在正确的目录中
3. 更新目录文件(contents.hhc)
4. 更新项目文件(help.hhp)
5. 重新编译CHM文件

### 更新现有帮助内容
1. 直接编辑对应的HTML文件
2. 确保链接和图片路径正确
3. 重新编译CHM文件

## 帮助系统集成

RUINOR ERP系统已集成帮助系统，通过以下方式实现：

1. **窗体级别帮助**: 通过`HelpMapping`特性标记窗体对应的帮助页面
2. **控件级别帮助**: 通过`SetControlHelpKey`扩展方法为控件设置帮助键
3. **智能帮助匹配**: 系统会根据控件类型和名称自动匹配帮助内容

### 使用示例

```csharp
// 为窗体设置帮助页面
[HelpMapping("forms/my_form.html", Title = "我的窗体")]
public partial class MyForm : frmBase
{
    public MyForm()
    {
        InitializeComponent();
        // 启用F1帮助功能（继承自frmBase已自动启用）
    }
}

// 为控件设置帮助键
Button btnSave = new Button();
btnSave.SetControlHelpKey("button_save");
```

### 销售订单查询窗体帮助系统说明

销售订单查询窗体(UCSaleOrderQuery)已集成帮助系统，支持以下功能：

1. **F1键帮助**:
   - 当焦点在窗体上时，显示销售订单查询窗体的帮助内容
   - 当焦点在特定控件上时，显示该控件的帮助内容

2. **F2键帮助**:
   - 打开帮助系统主窗体，提供搜索、推荐和历史记录功能

3. **控件帮助**:
   - 工具栏按钮: 查询、新增、修改、删除、打印、导出、结案等
   - 右键菜单项: 标记已打印、转为出库单、订单取消作废、转为采购单、预收货款等

### 帮助文件列表

#### 窗体帮助
- `forms/UCSaleOrderQuery.html` - 销售订单查询窗体帮助

#### 控件帮助
- `controls/button_query.html` - 查询按钮帮助
- `controls/button_add.html` - 新增按钮帮助
- `controls/button_edit.html` - 修改按钮帮助
- `controls/button_delete.html` - 删除按钮帮助
- `controls/button_print.html` - 打印按钮帮助
- `controls/button_export.html` - 导出按钮帮助
- `controls/button_close_case.html` - 结案按钮帮助
- `controls/contextmenu_mark_printed.html` - 标记已打印菜单项帮助
- `controls/contextmenu_convert_to_outbound.html` - 转为出库单菜单项帮助
- `controls/contextmenu_cancel_order.html` - 订单取消作废菜单项帮助
- `controls/contextmenu_convert_to_purchase.html` - 转为采购单菜单项帮助
- `controls/contextmenu_advance_payment.html` - 预收货款菜单项帮助