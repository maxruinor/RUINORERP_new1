# 帮助系统内容结构示例

## CHM文件组织结构

```
help.chm
├── index.html (主页)
├── contents.hhc (目录文件)
├── index.hhk (索引文件)
├── 
├── forms/
│   ├── test_form.html
│   ├── control_help_demo.html
│   ├── test_control_help.html
│   ├── basics/
│   │   ├── UCEditForm.html
│   │   └── UCManageForm.html
│   ├── documents/
│   │   ├── UCOrderForm.html
│   │   └── UCBillForm.html
│   ├── lists/
│   │   ├── UCQueryForm.html
│   │   └── UCGridForm.html
│   └── general/
│       └── MainForm.html
│
├── controls/
│   ├── button_save.html
│   ├── button_delete.html
│   ├── button_add.html
│   ├── button_edit.html
│   ├── button_cancel.html
│   ├── button_general.html
│   ├── textbox_general.html
│   ├── combobox_general.html
│   ├── grid_general.html
│   ├── label_general.html
│   ├── checkbox_general.html
│   ├── radiobutton_general.html
│   ├── datetimepicker_general.html
│   ├── numericupdown_general.html
│   ├── textbox_name.html
│   ├── checkbox_active.html
│   ├── radiobutton_option1.html
│   ├── radiobutton_option2.html
│   └── combobox_options.html
│
├── topics/
│   ├── button_save.html
│   ├── textbox_name.html
│   ├── checkbox_active.html
│   └── ... (其他通过Tag设置的帮助键对应的文件)
│
└── images/
    ├── icon_save.png
    ├── icon_delete.png
    └── ... (帮助文件中使用的图片)
```

## 帮助文件内容模板

### 控件帮助文件模板 (controls/button_save.html)

```html
<!DOCTYPE html>
<html>
<head>
    <title>保存按钮帮助</title>
    <meta charset="utf-8">
</head>
<body>
    <h1>保存按钮</h1>
    <p>保存按钮用于保存当前窗体中的数据。</p>
    
    <h2>功能说明</h2>
    <ul>
        <li>验证输入数据的有效性</li>
        <li>将数据保存到数据库</li>
        <li>显示保存结果提示</li>
    </ul>
    
    <h2>使用方法</h2>
    <ol>
        <li>在窗体中输入必要的数据</li>
        <li>点击保存按钮</li>
        <li>系统将验证数据并保存到数据库</li>
        <li>保存成功后会显示成功提示</li>
    </ol>
    
    <h2>注意事项</h2>
    <ul>
        <li>必填字段必须填写完整</li>
        <li>数据格式需要符合要求</li>
        <li>保存后数据将无法撤销，请确认后再保存</li>
    </ul>
</body>
</html>
```

### 窗体帮助文件模板 (forms/test_form.html)

```html
<!DOCTYPE html>
<html>
<head>
    <title>测试窗体帮助</title>
    <meta charset="utf-8">
</head>
<body>
    <h1>测试窗体</h1>
    <p>测试窗体用于演示帮助系统的使用方法。</p>
    
    <h2>窗体功能</h2>
    <ul>
        <li>演示控件级别帮助功能</li>
        <li>测试帮助系统的各种特性</li>
        <li>提供帮助系统使用示例</li>
    </ul>
    
    <h2>控件说明</h2>
    <table border="1">
        <tr>
            <th>控件名称</th>
            <th>功能说明</th>
            <th>帮助键</th>
        </tr>
        <tr>
            <td>保存按钮</td>
            <td>保存测试数据</td>
            <td>button_save</td>
        </tr>
        <tr>
            <td>姓名文本框</td>
            <td>输入测试人员姓名</td>
            <td>textbox_name</td>
        </tr>
    </table>
    
    <h2>操作步骤</h2>
    <ol>
        <li>在姓名文本框中输入测试人员姓名</li>
        <li>点击保存按钮保存数据</li>
        <li>可以通过F1键获取当前控件的帮助信息</li>
    </ol>
</body>
</html>
```

## 帮助系统维护指南

### 添加新的帮助内容

1. 创建对应的HTML帮助文件
2. 将文件放置在正确的目录中
3. 如果是控件帮助，确保文件名与帮助键匹配
4. 更新目录文件(contents.hhc)和索引文件(index.hhk)

### 更新现有帮助内容

1. 直接编辑对应的HTML文件
2. 确保链接和图片路径正确
3. 重新编译CHM文件

### 帮助文件编译

使用HTML Help Workshop或其他CHM编译工具编译帮助文件：

1. 打开项目文件(.hhp)
2. 点击"编译"按钮
3. 生成新的CHM文件