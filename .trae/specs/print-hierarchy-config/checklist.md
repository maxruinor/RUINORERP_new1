# 打印系统分级配置功能检查清单

## 第一阶段：数据模型修改

- [x] 1.1 tb_UIMenuPersonalization.cs实体类已修改
  - [x] 1.1.1 PrintConfigJson字段已添加
  - [x] 1.1.2 UsePersonalPrintConfig字段已添加
  - [x] 1.1.3 PrintConfigDict扩展属性已实现

- [ ] 1.2 数据库字段已添加
  - [ ] 1.2.1 PrintConfigJson字段SQL已执行
  - [ ] 1.2.2 UsePersonalPrintConfig字段SQL已执行
  - [ ] 1.2.3 字段验证成功

## 第二阶段：服务层实现

- [x] 2.1 IMenuPrintConfigService接口已创建
  - [x] 2.1.1 GetMenuPrintConfigAsync方法已定义
  - [x] 2.1.2 SaveAsPersonalConfigAsync方法已定义
  - [x] 2.1.3 DeletePersonalConfigAsync方法已定义
  - [x] 2.1.4 HasPersonalConfigAsync方法已定义

- [x] 2.2 MenuPrintConfigService服务类已实现
  - [x] 2.2.1 获取菜单打印配置逻辑正确
  - [x] 2.2.2 保存个人配置逻辑正确
  - [x] 2.2.3 删除个人配置逻辑正确
  - [x] 2.2.4 缓存管理已集成
  - [x] 2.2.5 DI容器注册完成

## 第三阶段：打印逻辑修改

- [x] 3.1 PrintHelper.cs已修改
  - [x] 3.1.1 获取菜单个人配置逻辑已添加
  - [x] 3.1.2 打印优先级已调整
  - [x] 3.1.3 降级机制已实现

- [x] 3.2 旧数据兼容已处理
  - [x] 3.2.1 tb_UserPersonalized.PrinterName兼容逻辑已保留
  - [x] 3.2.2 版本检测和迁移机制已实现

## 第四阶段：UI界面修改

- [x] 4.1 RptPrintConfig窗体已修改
  - [x] 4.1.1 "保存为个人配置"按钮已添加
  - [x] 4.1.2 "恢复系统配置"按钮已添加
  - [x] 4.1.3 配置级别标识已显示
  - [x] 4.1.4 按钮事件逻辑已实现

- [x] 4.2 RptPrintConfig.Designer.cs已修改
  - [x] 4.2.1 按钮控件声明已添加
  - [x] 4.2.2 按钮初始化代码已添加

- [x] 4.3 界面交互已优化
  - [x] 4.3.1 个人配置状态视觉区分已实现
  - [x] 4.3.2 状态栏显示当前配置来源
  - [x] 4.3.3 保存/删除后反馈提示已实现

## 第五阶段：可选优化

- [ ] 5.1 UCUserPersonalizedEdit界面已调整（如需要）
  - [ ] 5.1.1 全局打印机配置部分已评估
  - [ ] 5.1.2 与新系统兼容性已确认

- [ ] 5.2 数据清理已完成（如执行）
  - [ ] 5.2.1 tb_UserPersonalized字段已评估
  - [ ] 5.2.2 迁移计划已制定

## 验收标准确认

- [x] 可以在每个菜单(单据)级别配置打印设置
- [x] 可将系统配置保存为个人独有配置
- [x] 个人独有配置有明显标识
- [x] 可恢复使用系统配置
- [x] 打印时正确读取对应级别的配置
- [x] 旧数据完全兼容
- [x] 数据库修改最小化（仅两字段）
- [x] 与现有菜单个性化体系一致

## 方案优势总结

1. **最小化修改**：仅在tb_UIMenuPersonalization表增加两个字段
2. **架构一致**：利用现有菜单个性化体系
3. **层级清晰**：系统级 → 菜单级个人配置
4. **平滑过渡**：旧数据完全兼容
5. **可选清理**：可选择删除tb_UserPersonalized中的全局打印机配置
