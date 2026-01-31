---
name: SKU图片上传功能完善方案
overview: 基于销售订单凭证图片上传的现有实现，在产品明细SKU表格中集成图片上传、预览和查看功能
todos:
  - id: explore-sku-grid
    content: 使用[subagent:code-explorer]探索产品明细SKU的DataGridView实现结构
    status: completed
  - id: analyze-saleorder-images
    content: 使用[subagent:code-explorer]分析销售订单MagicPictureBox实现细节
    status: completed
  - id: design-datagridview-cell
    content: 设计DataGridView图片列自定义单元格类型
    status: completed
    dependencies:
      - explore-sku-grid
      - analyze-saleorder-images
  - id: implement-upload-logic
    content: 实现SKU图片上传、拖拽、粘贴功能
    status: completed
    dependencies:
      - design-datagridview-cell
  - id: add-preview-viewer
    content: 集成MagicPictureBox和frmPictureViewer实现预览功能
    status: completed
    dependencies:
      - implement-upload-logic
  - id: integrate-data-binding
    content: 实现图片路径与ImagesPath字段的数据绑定
    status: completed
    dependencies:
      - add-preview-viewer
  - id: test-upload-flow
    content: 测试完整的SKU图片上传流程
    status: completed
    dependencies:
      - integrate-data-binding
  - id: 7c7731a2
    content: 8.上传成功后。再次编辑产品明细能编辑：frmProductEdit，同时查询也能查询显示SKU图片UCProdQuery.cs
    status: completed
---

## 产品SKU图片上传功能需求

### 用户核心需求

在产品明细SKU列表的DataGridView中集成便捷的图片上传控件，实现：

- 图片选择上传（文件对话框）
- 拖拽上传
- 剪贴板粘贴（截图粘贴）
- 小图预览功能
- 双击查看大图

### 现有功能参考

销售订单凭证图片上传已实现完整功能（UCSaleOrder.cs），使用MagicPictureBox控件，支持多图片管理、上传、删除、预览。

### 技术约束

- 产品明细SKU使用DataGridView展示（dataGridView1）
- tb_ProdDetail模型已有ImagesPath字段（varchar(2000)，存储图片路径）
- 需与销售订单图片逻辑保持一致
- 需要评估存储方案：二进制内存 vs 本地文件缓存

## 技术方案

### 技术栈

- **UI框架**：WinForms + Krypton Toolkit
- **图片控件**：MagicPictureBox（复用销售订单实现）
- **存储方案**：本地文件缓存 + 路径存储（与销售订单保持一致）
- **数据库**：保持ImagesPath字段存储多个图片路径（逗号分隔）

### 架构设计

采用与销售订单一致的架构模式：

1. **MagicPictureBox控件**：封装图片上传、预览、删除功能
2. **本地缓存**：上传时保存到临时目录，确认后统一上传服务器
3. **路径存储**：ImagesPath存储相对路径，多个图片用分隔符拼接
4. **延迟上传**：在保存产品时统一处理图片上传

### 实现策略

1. **DataGridView单元格嵌入MagicPictureBox**：通过自定义单元格类型实现
2. **事件处理**：处理文件选择、拖拽、剪贴板粘贴
3. **预览功能**：小图显示在单元格，双击打开frmPictureViewer查看大图
4. **数据绑定**：将图片路径与ImagesPath字段双向绑定

### 性能优化

- 图片加载异步处理，避免UI卡顿
- 缩略图生成，减少内存占用
- 重复图片检测，避免重复上传