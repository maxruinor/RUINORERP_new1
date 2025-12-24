## 简化差异文件管理Tab - 移除分页功能

### 需求分析
用户要求简化差异文件管理tab，去掉分页功能，直接全部加载所有差异文件，避免复杂操作。

### 当前实现分析
1. **核心功能**：`InitializeDiffFileList()` 加载差异文件数据
2. **分页机制**：
   - `BindDiffFileData()` 实现分页逻辑，包括总页数计算、数据分页和绑定
   - `ApplyFilterAndPagination()` 应用过滤和重置页码
   - 分页控件：`btnPrevPage`、`btnNextPage` 和 `lblPageInfo`
   - 分页变量：`currentPage`、`pageSize`

### 修改方案

#### 1. 修改 `BindDiffFileData()` 方法
- 移除分页计算逻辑
- 直接绑定所有过滤后的数据，不再进行 `Skip()` 和 `Take()` 操作
- 移除分页信息更新

#### 2. 修改 `ApplyFilterAndPagination()` 方法
- 保留过滤功能
- 移除页码重置逻辑

#### 3. 移除分页相关变量
- `currentPage` 成员变量
- `pageSize` 常量

#### 4. 移除分页相关控件（如果需要）
- 设计器中的分页按钮和标签
- 相关的点击事件处理程序

#### 5. 简化相关方法调用
- 确保所有调用点都适应新的实现

### 预期效果
- 差异文件管理tab直接显示所有差异文件，无需分页
- 保持过滤功能不变
- 界面简洁，操作简单

### 实现步骤
1. 修改 `frmAULWriter.cs` 中的 `BindDiffFileData()` 方法
2. 修改 `ApplyFilterAndPagination()` 方法
3. 移除分页相关变量和事件处理程序
4. 检查并修复所有调用点
5. 验证功能正常工作