# RUINOR ERP 帮助系统

欢迎使用RUINOR ERP智能帮助系统!

## 快速开始

### 如何使用帮助系统

1. **F1键帮助**: 在任何界面按F1键,查看当前焦点控件的帮助信息
2. **智能提示**: 将鼠标悬停在控件上,会自动显示简短的帮助提示
3. **帮助按钮**: 帮助图标按钮点击查看详细帮助
4. **搜索帮助**: 使用搜索功能查找相关帮助内容
5. **帮助面板**: 按F1键打开帮助面板，可查看详细帮助内容

### 帮助级别

本帮助系统提供四个级别的帮助:

- **模块级**: 业务模块的整体介绍和功能概述
- **窗体级**: 单个窗体的功能说明和操作流程
- **控件级**: 控件的使用方法和注意事项
- **字段级**: 字段的详细说明和输入要求

## 模块导航

### 销售管理模块

销售管理模块负责处理销售订单、销售出库等业务流程。

- [销售管理概述](Modules/SalesManagement.md)
- [销售订单](Forms/UCSaleOrder.md)
- [销售出库](Forms/UCSaleOut.md)

### 采购管理模块

采购管理模块负责处理采购订单、采购入库等业务流程。

- [采购管理概述](Modules/PurchaseManagement.md)

### 库存管理模块

库存管理模块负责处理库存调拨、盘点等业务流程。

- [库存管理概述](Modules/InventoryManagement.md)

### 财务管理模块

财务管理模块负责处理财务凭证、应收应付等业务流程。

- [财务管理概述](Modules/FinanceManagement.md)

### 生产管理模块

生产管理模块负责处理生产订单、生产计划等业务流程。

- [生产管理概述](Modules/ProductionManagement.md)

## 窗体导航

### 销售管理窗体

- [销售订单录入界面](Forms/UCSaleOrder.md)
- [销售出库界面](Forms/UCSaleOut.md)

## 字段导航

### 销售订单字段

销售订单实体包含以下字段:

- [订单编号](Fields/tb_SaleOrder/OrderNo.md)
- [客户编号](Fields/tb_SaleOrder/CustomerVendor_ID.md)
- [销售员](Fields/tb_SaleOrder/Employee_ID.md)
- [订单日期](Fields/tb_SaleOrder/OrderDate.md)
- [销售日期](Fields/tb_SaleOrder/SaleDate.md)
- [预计交货日期](Fields/tb_SaleOrder/PreDeliveryDate.md)
- [交货日期](Fields/tb_SaleOrder/DeliveryDate.md)
- [付款方式](Fields/tb_SaleOrder/Paytype_ID.md)
- [付款状态](Fields/tb_SaleOrder/PayStatus.md)
- [总成本](Fields/tb_SaleOrder/TotalCost.md)
- [凭证图片](Fields/tb_SaleOrder/VoucherImage.md)

## 按钮操作指南

### 通用按钮

| 按钮名称 | 功能说明 | 快捷键 |
|--------|------|--------|
| 保存 | 保存当前记录 | Ctrl+S |
| 新建 | 新建一条记录 | Ctrl+N |
| 删除 | 删除当前记录 | Delete |
| 审核 | 审核当前记录 | |
| 反审核 | 反审核当前记录 | |
| 打印 | 打印当前记录 | Ctrl+P |
| 导出 | 导出数据 | |
| 导入 | 导入数据 | |
| 帮助 | 显示帮助信息 | F1 |

### 销售订单按钮

| 按钮名称 | 功能说明 |
|--------|------|
| 生成出库单 | 根据销售订单生成销售出库单 |
| 生成发票 | 根据销售订单生成销售发票 |
| 查看关联单据 | 查看与当前订单关联的其他单据 |

## 快捷键

| 快捷键 | 功能 |
|--------|------|
| F1 | 显示帮助 |
| Ctrl+F | 搜索帮助 |
| F5 | 刷新内容 |
| Ctrl+P | 打印帮助 |
| Ctrl+S | 保存记录 |
| Ctrl+N | 新建记录 |
| Delete | 删除记录 |
| Ctrl+Z | 撤销操作 |
| Ctrl+Y | 重做操作 |

## 技术支持

如果您在使用过程中遇到问题或有改进建议,欢迎通过以下方式反馈:

- **联系系统管理员**: 请联系您的系统管理员获取帮助
- **常见问题解答**: 查看系统中的常见问题解答
- **在线文档**: 访问我们的在线文档获取更多信息
- **反馈邮箱**: support@ruinor.com

## 关于系统

### 系统版本

当前系统版本: 1.0.0

### 版权信息

© 2024 RUINOR ERP. All rights reserved.

### 更新日志

- 2024-01-01: 系统发布
- 2024-02-15: 添加销售管理模块
- 2024-03-10: 添加采购管理模块
- 2024-04-05: 添加库存管理模块

---

*本帮助系统由RUINOR ERP智能帮助框架生成*
