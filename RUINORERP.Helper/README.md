# RUINOR ERP 帮助系统使用说明

## 概述

本目录包含RUINOR ERP系统的帮助文件，使用HTML格式编写，可通过WinCHM Pro或HTML Help Workshop编译为CHM文件。

## 目录结构

```
RUINORERP.Helper/
├── *.htm              # 各种帮助文档
├── controls/          # 控件级别帮助文件
├── 系统介绍/           # 系统介绍相关文档
├── help.hhp           # HTML Help项目文件
├── contents.hhc       # 目录文件
├── compile_help.bat   # 编译脚本
└── README.md          # 本文件
```

## 编译帮助文件

### 方法1: 使用WinCHM Pro
1. 打开ERP系统帮助.wcp项目文件
2. 点击"编译"按钮
3. 生成的CHM文件将保存在当前目录

### 方法2: 使用HTML Help Workshop
1. 运行compile_help.bat脚本
2. 脚本将自动调用hhc.exe编译帮助文件

## 帮助系统功能

### 控件级别帮助
- 为每个控件设置特定的帮助内容
- 用户按F1键可获取当前焦点控件的帮助

### 窗体级别帮助
- 为每个窗体设置帮助内容
- 当控件无特定帮助时，显示窗体帮助

### 智能匹配
- 系统可根据控件类型和名称自动匹配帮助内容
- 减少手动配置工作量

## 使用示例

在代码中为控件设置帮助键：
```csharp
Button testButton = new Button();
testButton.SetControlHelpKey("button_test");
```

在代码中为窗体设置帮助页面：
```csharp
this.SetHelpPage("forms/my_form.html", "我的窗体");
```

## 维护指南

1. 添加新的帮助内容时，需要更新目录文件(contents.hhc)
2. 修改现有帮助内容后，需要重新编译CHM文件
3. 确保帮助文件的编码格式为UTF-8