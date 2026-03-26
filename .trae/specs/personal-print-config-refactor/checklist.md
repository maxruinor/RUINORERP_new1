
# 个人打印配置重构验证检查清单

## 代码质量检查
- [ ] 所有修改的代码都添加了必要的注释（类级、函数级、参数注释）
- [ ] 代码格式符合项目规范（缩进、空格、命名等）
- [ ] 无编译错误和警告
- [ ] 异常处理完善
- [ ] 日志记录充分且清晰

## RptPrintConfig.cs检查
- [ ] SaveAsPersonalConfigAsync()方法修复
  - [ ] PrinterName从cmbPrinterList.SelectedItem获取，而非printConfig.PrinterName
  - [ ] PrinterSelected状态正确设置（基于下拉列表是否有选择项）
  - [ ] 模板从newSumDataGridView1.CurrentRow获取用户选择
  - [ ] 如果用户未选择模板，使用系统默认模板（IsDefaultTemplate==true）
- [ ] RevertToSystemConfigAsync()方法完善
  - [ ] 同时清空PrintConfigJson字段
  - [ ] 同时清空PrintConfigDict字段
  - [ ] UsePersonalPrintConfig设置为false
  - [ ] 有确认对话框提示用户

## PrintHelper.cs检查
- [ ] GetMenuPersonalPrinter()方法
  - [ ] 正确从tb_UIMenuPersonalization读取个人配置
  - [ ] 检查UsePersonalPrintConfig标志
  - [ ] 检查PrintConfigDict.PrinterSelected
  - [ ] 返回正确的打印机名称
- [ ] GetPersonalPrintTemplate()方法
  - [ ] 正确从个人配置获取模板ID
  - [ ] 正确查询tb_PrintTemplate表获取模板
  - [ ] 检查TemplateFileStream不为空才返回
- [ ] GetPrinterWithPriority()方法
  - [ ] 优先级顺序正确（个人配置 &gt; 全局配置 &gt; 本地默认）
  - [ ] 日志清晰记录使用的配置来源

## UCUserPersonalizedEdit.cs检查
- [ ] SelectTemplatePrint功能
  - [ ] 正确绑定到chkSelectTemplatePrint复选框
  - [ ] 数据绑定逻辑正确
  - [ ] 保存逻辑正确

## 功能测试检查
- [ ] 保存个人配置测试
  - [ ] 打开RptPrintConfig界面
  - [ ] 从下拉列表选择一个打印机
  - [ ] 在网格中选择一个打印模板
  - [ ] 点击"保存为个人配置"
  - [ ] 验证保存成功提示
  - [ ] 验证数据库中保存的是用户选择的打印机和模板
- [ ] 使用个人配置打印测试
  - [ ] 确保已保存个人配置
  - [ ] 打开单据并点击打印
  - [ ] 验证使用个人配置的打印机
  - [ ] 验证使用个人配置的模板
  - [ ] 日志显示使用个人配置
- [ ] 恢复全局配置测试
  - [ ] 点击"恢复系统配置"
  - [ ] 确认对话框确认
  - [ ] 验证个人配置被清空
  - [ ] 打印使用全局配置
- [ ] 无个人配置时的默认行为测试
  - [ ] 删除或清空个人配置
  - [ ] 点击打印
  - [ ] 使用全局默认打印机
  - [ ] 使用系统默认模板（IsDefaultTemplate==true）
- [ ] 手动选择模板测试
  - [ ] 在UCUserPersonalizedEdit中勾选"手动选择打印模板"
  - [ ] 保存设置
  - [ ] 点击打印
  - [ ] 验证弹出RptPrintConfig界面

## 最终验收
- [ ] 所有检查项通过
- [ ] 人工检查通过
- [ ] 可以交付

