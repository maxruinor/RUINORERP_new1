# 帮助文件结构说明

## CHM文件组织结构

```
help.chm
├── index.html (主页)
├── contents.hhc (目录文件)
├── index.hhk (索引文件)
│
├── forms/
│   ├── test_form.html
│   └── ... (其他窗体帮助文件)
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
│   └── ... (其他控件帮助文件)
│
├── topics/
│   ├── button_save.html
│   └── ... (通过Tag设置的帮助键对应的文件)
│
└── images/
    ├── icon_save.png
    └── ... (帮助文件中使用的图片)
```

## 控件帮助文件命名规范

### 按钮控件
- button_save.html (保存按钮)
- button_delete.html (删除按钮)
- button_add.html (添加按钮)
- button_edit.html (编辑按钮)
- button_cancel.html (取消按钮)
- button_general.html (通用按钮)

### 文本框控件
- textbox_general.html (通用文本框)

### 下拉框控件
- combobox_general.html (通用下拉框)

### 数据网格控件
- grid_general.html (通用数据网格)

### 标签控件
- label_general.html (通用标签)

### 复选框控件
- checkbox_general.html (通用复选框)

### 单选框控件
- radiobutton_general.html (通用单选框)

### 日期时间控件
- datetimepicker_general.html (通用日期时间控件)

### 数值控件
- numericupdown_general.html (通用数值控件)

## 窗体帮助文件组织

### 基础数据窗体
- basics/UCEditForm.html
- basics/UCManageForm.html

### 单据窗体
- documents/UCOrderForm.html
- documents/UCBillForm.html

### 列表窗体
- lists/UCQueryForm.html
- lists/UCGridForm.html

### 其他窗体
- general/MainForm.html

## 帮助文件内容模板

### 控件帮助文件模板
```html
<!DOCTYPE html>
<html>
<head>
    <title>控件名称帮助</title>
    <meta charset="utf-8">
</head>
<body>
    <h1>控件名称</h1>
    <p>控件功能简要说明。</p>
    
    <h2>功能说明</h2>
    <ul>
        <li>功能点1</li>
        <li>功能点2</li>
        <li>功能点3</li>
    </ul>
    
    <h2>使用方法</h2>
    <ol>
        <li>操作步骤1</li>
        <li>操作步骤2</li>
        <li>操作步骤3</li>
    </ol>
    
    <h2>注意事项</h2>
    <ul>
        <li>注意事项1</li>
        <li>注意事项2</li>
        <li>注意事项3</li>
    </ul>
</body>
</html>
```

### 窗体帮助文件模板
```html
<!DOCTYPE html>
<html>
<head>
    <title>窗体名称帮助</title>
    <meta charset="utf-8">
</head>
<body>
    <h1>窗体名称</h1>
    <p>窗体功能简要说明。</p>
    
    <h2>窗体功能</h2>
    <ul>
        <li>功能点1</li>
        <li>功能点2</li>
        <li>功能点3</li>
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
            <td>保存数据</td>
            <td>button_save</td>
        </tr>
        <tr>
            <td>姓名文本框</td>
            <td>输入姓名</td>
            <td>textbox_name</td>
        </tr>
    </table>
    
    <h2>操作步骤</h2>
    <ol>
        <li>操作步骤1</li>
        <li>操作步骤2</li>
        <li>操作步骤3</li>
    </ol>
</body>
</html>
```

## 维护指南

1. 添加新的帮助内容时，需要更新目录文件(contents.hhc)
2. 修改现有帮助内容后，需要重新编译CHM文件
3. 确保帮助文件的编码格式为UTF-8
4. 图片文件应放在images目录中
5. 控件帮助文件应放在controls目录中
6. 窗体帮助文件应根据类型放在相应的子目录中