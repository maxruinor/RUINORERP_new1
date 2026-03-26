# 打印系统分级配置功能任务清单

## 任务概述

基于新的设计思路：在tb_UIMenuPersonalization表中扩展打印配置字段，实现菜单级别的个人打印配置功能。

## 第一阶段：数据模型修改

- [x] 1.1 修改tb_UIMenuPersonalization.cs实体类
  - [x] 1.1.1 添加PrintConfigJson字段（nvarchar(max)）
  - [x] 1.1.2 添加UsePersonalPrintConfig字段（bit）
  - [x] 1.1.3 添加PrintConfigDict扩展属性（JSON序列化/反序列化）

- [ ] 1.2 数据库字段添加
  - [ ] 1.2.1 执行SQL添加PrintConfigJson字段
  - [ ] 1.2.2 执行SQL添加UsePersonalPrintConfig字段
  - [ ] 1.2.3 验证字段添加成功

## 第二阶段：服务层实现

- [x] 2.1 创建IMenuPrintConfigService接口
  - [x] 2.1.1 定义GetMenuPrintConfigAsync方法
  - [x] 2.1.2 定义SaveAsPersonalConfigAsync方法
  - [x] 2.1.3 定义DeletePersonalConfigAsync方法
  - [x] 2.1.4 定义HasPersonalConfigAsync方法

- [x] 2.2 实现MenuPrintConfigService服务类
  - [x] 2.2.1 实现获取菜单打印配置逻辑
  - [x] 2.2.2 实现保存个人配置逻辑
  - [x] 2.2.3 实现删除个人配置逻辑
  - [x] 2.2.4 集成缓存管理
  - [x] 2.2.5 注册到DI容器

## 第三阶段：打印逻辑修改

- [x] 3.1 修改PrintHelper.cs
  - [x] 3.1.1 添加获取菜单个人配置的逻辑
  - [x] 3.1.2 修改打印优先级（优先使用菜单个人配置）
  - [x] 3.1.3 添加降级机制（个人配置不存在时使用系统配置）

- [x] 3.2 兼容旧数据
  - [x] 3.2.1 保留tb_UserPersonalized.PrinterName兼容逻辑
  - [x] 3.2.2 添加版本检测和平滑迁移

## 第四阶段：UI界面修改

- [x] 4.1 修改RptPrintConfig窗体
  - [x] 4.1.1 添加"保存为个人配置"按钮
  - [x] 4.1.2 添加"恢复系统配置"按钮
  - [x] 4.1.3 添加配置级别标识显示
  - [x] 4.1.4 实现按钮点击事件逻辑

- [x] 4.2 修改RptPrintConfig.Designer.cs
  - [x] 4.2.1 添加按钮控件声明
  - [x] 4.2.2 添加按钮初始化代码

- [x] 4.3 界面交互优化
  - [x] 4.3.1 个人配置状态视觉区分（颜色/图标）
  - [x] 4.3.2 状态栏显示当前配置来源
  - [x] 4.3.3 保存/删除后的反馈提示

## 第五阶段：可选优化

- [ ] 5.1 UCUserPersonalizedEdit界面调整
  - [ ] 5.1.1 评估是否移除全局打印机配置部分
  - [ ] 5.1.2 如保留，确保与新系统兼容

- [ ] 5.2 数据清理（可选）
  - [ ] 5.2.1 评估是否删除tb_UserPersonalized.PrinterConfigJson
  - [ ] 5.2.2 评估是否删除tb_UserPersonalized.UseUserOwnPrinter
  - [ ] 5.2.3 制定数据迁移计划

## 任务依赖关系

```
1.1.1 → 1.1.2 → 1.1.3 → 1.2.1 → 1.2.2 → 1.2.3
    ↓
2.1.1 → 2.1.2 → 2.1.3 → 2.1.4
    ↓
2.2.1 → 2.2.2 → 2.2.3 → 2.2.4 → 2.2.5
    ↓
3.1.1 → 3.1.2 → 3.1.3 → 3.2.1 → 3.2.2
    ↓
4.1.1 → 4.1.2 → 4.1.3 → 4.2.1 → 4.2.2 → 4.3.1 → 4.3.2 → 4.3.3
    ↓
5.1.1 → 5.1.2 → 5.2.1 → 5.2.2 → 5.2.3
```

## 技术说明

本方案的核心优势：
1. **最小化数据库修改**：仅在tb_UIMenuPersonalization表增加两个字段
2. **与现有架构一致**：利用已有的菜单个性化体系
3. **清晰的层级关系**：系统级 → 菜单级个人配置
4. **平滑过渡**：旧数据完全兼容
