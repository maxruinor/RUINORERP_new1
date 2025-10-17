# RUINOR ERP 帮助系统说明

## 概述

本目录包含RUINOR ERP系统的帮助文档，这些文档将被编译成CHM格式的帮助文件，供用户在使用系统时查阅。

## 目录结构

```
RUINORERP.Helper/
├── controls/           # 控件帮助文件
├── forms/              # 窗体帮助文件
├── *.htm              # 各种帮助内容文件
├── help.hhp           # HTML Help Project文件
├── contents.hhc       # 目录文件
├── index.hhk          # 索引文件
├── 编译帮助.bat        # 编译脚本 (中文版本)
└── compile_help.bat   # 编译脚本 (英文版本)
```

## 帮助文件组织结构

帮助文件按照RUINOR ERP系统的模块结构进行组织，主要包含以下几个层次：

1. **系统概述** - 系统整体介绍
2. **生产管理** - 生产相关模块
3. **进销存管理** - 采购、销售、库存模块
4. **售后管理** - 售后服务模块
5. **客户关系管理** - 客户关系模块
6. **财务管理** - 财务相关模块
7. **行政管理** - 行政人事模块
8. **报表管理** - 各类报表模块
9. **电商运营** - 电商相关模块
10. **基础资料管理** - 基础资料模块
11. **系统设置** - 系统配置模块
12. **通用功能** - 系统通用功能
13. **窗体帮助** - 具体窗体帮助
14. **控件帮助** - 控件使用帮助

每个模块下都包含相应的子模块和详细的功能说明。

## 如何编译帮助文件

1. 确保已安装Microsoft HTML Help Workshop
2. 运行`编译帮助.bat`或`compile_help.bat`脚本
3. 编译成功后将生成`help.chm`文件

## 帮助文件使用说明

### 快捷键
- **F1**: 显示当前焦点控件或窗体的帮助
- **F2**: 打开帮助系统主窗体

### 帮助内容分类
1. **窗体帮助**: 介绍各个窗体的功能和使用方法
2. **控件帮助**: 介绍各个控件的功能和使用方法
3. **主题帮助**: 介绍系统功能和业务流程

### 中文编码问题说明

所有帮助文件必须使用UTF-8 with BOM编码格式，否则在CHM文件中会出现中文乱码。

如果遇到中文显示乱码问题，请运行`修复编码.bat`脚本来修复所有.htm文件的编码格式。

该脚本会将所有.htm文件转换为UTF-8 with BOM编码，确保中文能够正常显示。

### HTML文件美化

为了提升帮助文档的视觉效果和专业性，所有HTML帮助文件都应用了现代化的CSS样式。

如果需要重新美化HTML文件，请运行`美化HTML.bat`脚本，该脚本会为所有.htm文件添加统一的样式表，包括：
- 现代化的字体和颜色方案
- 清晰的标题和段落格式
- 美观的表格和列表样式
- 响应式设计，适配不同屏幕尺寸

### 模块间导航链接

为了方便用户在帮助文档中自由切换，所有HTML帮助文件都添加了模块间导航链接。

如果需要重新添加导航链接，请运行`添加导航链接.bat`脚本，该脚本会为所有.htm文件添加统一的导航栏，包括：
- 生产管理
- 进销存管理
- 售后管理
- 客户关系
- 财务管理
- 行政管理
- 报表管理
- 电商运营
- 基础资料
- 系统设置
- 通用功能
- 审核流程

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