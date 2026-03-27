# GridView 显示优化计划

## 目标文件
1. `GridViewDisplayHelper.cs` - 核心显示逻辑辅助类
2. `GridViewDisplayTextResolver.cs` - 文本解析器（视图分析）
3. `GridViewDisplayTextResolverGeneric.cs` - 泛型文本解析器
4. `GridViewRelated.cs` - 表格关联单据处理
5. `AbstractGridViewDisplayTextResolver.cs` - 抽象基类

## 优化原则
- **小步前进**：每次只做少量修改
- **保持兼容**：不改变原有接口和调用方式
- **性能优先**：重点优化高频调用路径
- **避免破坏**：不修改依赖注入相关的代码

---

## 第一阶段：性能优化（低风险）

### 1.1 缓存优化 - GridViewDisplayHelper.cs

**问题**：`GetGridViewDisplayText` 和 `GetDisplayNameByReferenceKeyMappings` 方法中频繁使用 LINQ 查询和字符串比较

**优化点**：
- 添加字典缓存，避免重复查找 FixedDictionaryMappings
- 优化 ReferenceKeyMappings 的查找逻辑
- 缓存属性反射结果

**具体修改**：
```csharp
// 添加缓存字段
private readonly ConcurrentDictionary<string, FixedDictionaryMapping> _fixedDictCache = new();
private readonly ConcurrentDictionary<string, ReferenceKeyMapping> _refKeyCache = new();
```

### 1.2 减少反射调用

**问题**：`InitializeFixedDictionaryMappings` 中多次调用 `GetMemberInfo()` 和反射

**优化点**：
- 缓存 MemberInfo 结果
- 减少不必要的表达式编译

### 1.3 字符串操作优化

**问题**：多处使用 `ToLower()` 进行字符串比较

**优化点**：
- 使用 `StringComparison.OrdinalIgnoreCase` 替代 `ToLower()`
- 避免创建临时字符串对象

---

## 第二阶段：事件处理优化（中低风险）

### 2.1 CellFormatting 事件优化

**问题**：`DataGridView_CellFormatting` 是高频触发事件

**优化点**：
- 添加列名缓存，避免重复获取 `dataGridView.Columns[e.ColumnIndex].Name`
- 提前返回条件优化
- 减少不必要的类型判断

**文件**：
- `GridViewDisplayTextResolverGeneric.cs` 第 97-131 行
- `GridViewDisplayTextResolver.cs` 第 141-188 行

### 2.2 空值检查优化

**问题**：多处重复的空值检查

**优化点**：
- 统一空值检查逻辑
- 使用模式匹配简化代码

---

## 第三阶段：代码结构优化（谨慎进行）

### 3.1 移除重复代码

**问题**：`GridViewDisplayTextResolver` 和 `GridViewDisplayTextResolverGeneric` 有大量重复逻辑

**优化点**：
- 将共同逻辑移到 `AbstractGridViewDisplayTextResolver`
- 保持子类只保留特定功能

### 3.2 集合初始化优化

**问题**：多处使用 `List<T>` 和 `Dictionary<string, string>`

**优化点**：
- 预估容量，减少扩容操作
- 使用更合适的集合类型

---

## 实施步骤

### 步骤 1：添加缓存机制（GridViewDisplayHelper.cs）
- 修改 `GetGridViewDisplayText` 方法
- 添加字典缓存字段
- 优化查找逻辑

### 步骤 2：优化 CellFormatting 事件
- 修改 `GridViewDisplayTextResolverGeneric.cs`
- 修改 `GridViewDisplayTextResolver.cs`
- 添加列名缓存

### 步骤 3：字符串比较优化
- 全局替换 `ToLower()` 比较
- 使用 `StringComparison.OrdinalIgnoreCase`

### 步骤 4：测试验证
- 验证所有修改点
- 确保功能正常

---

## 风险评估

| 优化项 | 风险等级 | 回滚难度 |
|--------|----------|----------|
| 缓存优化 | 低 | 容易 |
| 事件优化 | 中 | 中等 |
| 结构优化 | 高 | 困难 |

---

## 注意事项

1. **不要修改**：
   - 构造函数签名
   - 公共方法签名
   - 依赖注入相关的代码
   - Autofac 注册逻辑

2. **重点关注**：
   - `Startup.GetFromFac<T>()` 调用
   - 事件订阅/取消订阅
   - 泛型类型约束

3. **测试重点**：
   - 表格显示是否正常
   - 外键列显示是否正确
   - 枚举值显示是否正确
   - 图片列显示是否正常
