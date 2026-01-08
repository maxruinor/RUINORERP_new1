### 问题分析
1. **编译器错误**：`ToSql()`方法返回`KeyValuePair<string, List<SugarParameter>>`，但代码尝试将其转换为`string`，导致CS0030错误
2. **日志性能**：无论成功失败都构建调试字符串，造成不必要的性能开销
3. **反射复杂性**：`LoadEntityInternal`方法使用过多反射，容易出错且性能较低
4. **代码冗余**：两个查询方法（`LoadEntity`和`LoadEntityInternal`）存在重复逻辑

### 优化方案
1. **修复ToSql()返回类型处理**
   - 删除无效的string类型转换分支
   - 统一处理`KeyValuePair<string, List<SugarParameter>>`返回类型

2. **优化日志记录**
   - 将调试字符串构建移至catch块内，仅在出错时执行
   - 简化日志结构，提高可读性

3. **改进通用查询方法**
   - 使用反射调用泛型`LoadEntityCore`方法，减少反射次数
   - 统一查询逻辑，减少代码冗余
   - 提高方法稳定性和性能

4. **代码结构优化**
   - 合并重复的SQL日志获取逻辑
   - 简化异常处理流程

### 具体修改点
1. `LoadEntity`方法：修复ToSql()返回类型处理
2. `LoadEntityInternal`方法：
   - 简化反射调用，直接调用`LoadEntityCore`
   - 优化日志构建时机
3. 统一SQL日志获取逻辑
4. 移除不必要的类型检查分支

### 预期效果
- 解决编译错误
- 提高查询方法性能
- 减少日志构建的性能开销
- 简化代码结构，提高可维护性
- 增强错误日志的可读性和实用性