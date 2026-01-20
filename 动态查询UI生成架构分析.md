# 动态查询UI生成架构分析

## 核心目标

根据查询实体对象动态创建带条件值的代理实体，并生成相应的UI控件组合，用户输入的条件值通过数据绑定直接反映到代理实体类中。

## 工作流程

### 1. UIQueryPropertyBuilder - 动态类型构建器

**职责**: 动态生成查询代理类型

**核心方法**: `AttributesBuilder_New2024(Type type, QueryFilter queryFilter)`

**工作流程**:
```
输入: 基础实体类型 (如 Logs)
      查询过滤器 (包含查询字段配置)
      ↓
1. 创建动态程序集和模块
   - 使用 AssemblyBuilderAccess.RunAndSave
   - 动态程序集名称: "RUINORERP.DynamicUI.{TypeHash}"
      ↓
2. 创建继承自基础类型的代理类型
   - 类型名称: "{TypeName}Proxy" (如 "LogsProxy")
   - 继承: type (基础实体类型)
      ↓
3. 遍历查询字段，根据数据类型动态添加扩展属性
      ↓
   ┌────────────────────────────────────────────────────────────────┐
   │ 数据类型 → 扩展属性命名模式                        │
   ├────────────────────────────────────────────────────────────────┤
   │ Boolean → {FieldName}_Enable                     │
   │          示例: isdeleted_Enable                  │
   │          作用: 控制是否启用条件                      │
   ├────────────────────────────────────────────────────────────────┤
   │ Int/Long → {FieldName}_请选择                       │
   │            示例: DepartmentID_请选择                │
   │            作用: 单选下拉框存储值                     │
   │                                                  │
   │           → {FieldName}_CmbMultiChoiceCanIgnore    │
   │            示例: DepartmentID_CmbMultiChoiceCanIgnore │
   │            作用: 多选可忽略标记                       │
   │                                                  │
   │           → {FieldName}_MultiChoiceResults         │
   │            示例: DepartmentID_MultiChoiceResults   │
   │            作用: 多选结果列表 List<object>            │
   ├────────────────────────────────────────────────────────────────┤
   │ DateTime → {FieldName}_Start / {FieldName}_End         │
   │            示例: CreateTime_Start / CreateTime_End   │
   │            作用: 日期范围查询                         │
   ├────────────────────────────────────────────────────────────────┤
   │ String   → {FieldName}_Like                        │
   │            示例: OrderNo_Like                    │
   │            作用: 模糊查询字符串                         │
   └────────────────────────────────────────────────────────────────┘
      ↓
4. 为每个扩展属性添加特性标记
   - 使用 AdvExtQueryAttribute 标记
   - 参数: (原始字段名, 显示名称, 扩展属性名, 查询类型)
      ↓
输出: 动态生成的代理类型 (Type)
```

**示例代码**:
```csharp
// 原始实体: Logs (字段: OrderNo, CreateTime, Status)
// 生成的代理: LogsProxy (继承自 Logs)
//   + OrderNo_Like (string) - 用于模糊查询订单号
//   + CreateTime_Start (DateTime?) - 起始时间
//   + CreateTime_End (DateTime?) - 结束时间
//   + Status_Enable (bool) - 是否启用状态查询
```

---

### 2. UIGenerateHelper - 动态UI生成器

**职责**: 根据代理类型动态创建查询UI控件

**核心方法**: `CreateQueryUI(Type type, bool useLike, KryptonPanel UcPanel, QueryFilter queryFilter, tb_UIMenuPersonalization menuPersonalization)`

**工作流程**:
```
输入: 基础实体类型
      查询面板容器
      查询过滤器
      个性化配置 (可选)
      ↓
1. 应用个性化配置 (如果存在)
   - 更新字段可见性
   - 更新字段显示顺序
   - 更新默认值
   - 更新查询类型 (单选/多选/模糊/精确)
      ↓
2. 调用 UIQueryPropertyBuilder.AttributesBuilder_New2024
   - 生成代理类型: newtype
   - 实例化代理对象: newDto = Activator.CreateInstance(newtype)
      ↓
3. 计算UI布局
   - 预计算每列宽度 (基于标签长度和控件类型)
   - 确定行列位置 (默认4列)
      ↓
4. 遍历查询字段，创建对应控件
      ↓
   ┌────────────────────────────────────────────────────────────────┐
   │ 查询类型 → UI控件类型 → 数据绑定目标               │
   ├────────────────────────────────────────────────────────────────┤
   │ TextSelect     → KryptonTextBox                 │
   │                 绑定到: {FieldName} 或 {FriendlyFieldName} │
   ├────────────────────────────────────────────────────────────────┤
   │ defaultSelect  → KryptonComboBox                │
   │                 绑定到: {FieldName}_请选择              │
   ├────────────────────────────────────────────────────────────────┤
   │ CmbMultiChoice → CheckBoxComboBox               │
   │                 绑定到: {FieldName}_MultiChoiceResults    │
   ├────────────────────────────────────────────────────────────────┤
   │ CmbMultiChoiceCanIgnore → UCCmbMultiChoiceCanIgnore    │
   │                 绑定1: {FieldName}_CmbMultiChoiceCanIgnore │
   │                 绑定2: {FieldName}_MultiChoiceResults    │
   ├────────────────────────────────────────────────────────────────┤
   │ datetimeRange  → UCAdvDateTimerPickerGroup      │
   │                 绑定1: {FieldName}_Start              │
   │                 绑定2: {FieldName}_End                │
   ├────────────────────────────────────────────────────────────────┤
   │ datetime       → KryptonDateTimePicker            │
   │                 绑定到: {FieldName}                      │
   ├────────────────────────────────────────────────────────────────┤
   │ stringLike     → KryptonTextBox                 │
   │                 绑定到: {FieldName}_Like               │
   ├────────────────────────────────────────────────────────────────┤
   │ stringEquals   → KryptonTextBox                 │
   │                 绑定到: {FieldName}                      │
   ├────────────────────────────────────────────────────────────────┤
   │ useYesOrNoToAll → UCAdvYesOrNO                 │
   │                    绑定: {FieldName}_Enable               │
   │                    绑定: {FieldName}_RelatedFields         │
   ├────────────────────────────────────────────────────────────────┤
   │ YesOrNo       → KryptonCheckBox                │
   │                 绑定到: {FieldName}                      │
   └────────────────────────────────────────────────────────────────┘
      ↓
5. 数据绑定机制
   - 使用 DataBindingHelper 进行双向绑定
   - UI控件 ↔ newDto 属性 自动同步
   - 用户输入 → 自动设置到 newDto
   - 默认值 → 自动显示在 UI 控件
      ↓
6. 布局优化
   - 标签左对齐 (使用全角空格填充)
   - 列宽自适应 (预计算最大宽度)
   - 行高固定 (32px)
   - 控件间距统一 (3px)
      ↓
输出: 代理实体实例 (BaseEntity) + 动态生成的UI控件
```

**数据绑定示例**:
```csharp
// 1. 创建日期范围控件
UCAdvDateTimerPickerGroup dtpgroup = new UCAdvDateTimerPickerGroup();
dtpgroup.dtp1.Name = "CreateTime_Start";  // 对应代理类属性
dtpgroup.dtp2.Name = "CreateTime_End";    // 对应代理类属性

// 2. 绑定到代理实例
DataBindingHelper.BindData4DataTime(newDto, null, "CreateTime_Start", dtpgroup.dtp1, true);
DataBindingHelper.BindData4DataTime(newDto, null, "CreateTime_End", dtpgroup.dtp2, true);

// 3. 添加到面板
UcPanel.Controls.Add(dtpgroup);

// 结果: 用户在 dtpgroup.dtp1/dtpgroup.dtp2 选择日期后
//       自动同步到 newDto.CreateTime_Start / newDto.CreateTime_End
```

---

## 核心设计特点

### 1. 继承与扩展

```csharp
// 原始实体
public class Logs : BaseEntity
{
    public long? OrderNo { get; set; }
    public DateTime? CreateTime { get; set; }
    public int? Status { get; set; }
}

// 动态生成的代理 (LogsProxy)
public class LogsProxy : Logs
{
    // 继承所有原始属性
    public long? OrderNo { get; set; }          // 来自Logs
    public DateTime? CreateTime { get; set; }       // 来自Logs
    public int? Status { get; set; }             // 来自Logs

    // 扩展查询属性
    public string OrderNo_Like { get; set; }       // 新增
    public DateTime? CreateTime_Start { get; set; }  // 新增
    public DateTime? CreateTime_End { get; set; }    // 新增
    public bool Status_Enable { get; set; }        // 新增
}
```

### 2. 双向数据绑定

```csharp
// UI控件值变更 → 自动同步到代理实例
用户在日期控件选择 2024-01-01
    ↓
DataBindingHelper 监听到控件值变更
    ↓
通过反射设置 newDto.CreateTime_Start = 2024-01-01
    ↓
newDto 包含查询条件值

// 代理实例值变更 → 自动显示在UI控件 (如果支持)
newDto.CreateTime_Start = 2024-01-01 (代码设置)
    ↓
DataBindingHelper 更新 UI 控件显示
    ↓
日期控件显示 2024-01-01
```

### 3. 查询类型枚举

```csharp
public enum AdvQueryProcessType
{
    None,                      // 不处理
    TextSelect,               // 文本选择 (外键关联)
    defaultSelect,            // 单选下拉
    CmbMultiChoice,          // 多选下拉
    CmbMultiChoiceCanIgnore,  // 多选可忽略
    EnumSelect,              // 枚举选择
    datetimeRange,           // 日期范围
    datetime,               // 日期单选
    stringLike,             // 字符串模糊
    stringEquals,           // 字符串精确
    useYesOrNoToAll,        // 是/否/全部
    YesOrNo                 // 是/否
}
```

## 使用场景示例

### 场景1: 销售订单查询

```csharp
// 1. 定义查询过滤器
QueryFilter filter = new QueryFilter();
filter.QueryFields = new List<QueryField>
{
    new QueryField { FieldName = "OrderNo", Caption = "订单号", 
                  AdvQueryFieldType = AdvQueryProcessType.stringLike },
    new QueryField { FieldName = "CustomerID", Caption = "客户",
                  AdvQueryFieldType = AdvQueryProcessType.defaultSelect },
    new QueryField { FieldName = "CreateTime", Caption = "创建日期",
                  AdvQueryFieldType = AdvQueryProcessType.datetimeRange }
};

// 2. 生成UI
BaseEntity queryDto = UIGenerateHelper.CreateQueryUI(
    typeof(SaleOrder), 
    panel, 
    filter, 
    null);

// 3. 用户输入后，queryDto 包含:
// queryDto.OrderNo_Like = "2024"        (用户输入)
// queryDto.CustomerID_请选择 = 12345     (用户选择)
// queryDto.CreateTime_Start = 2024-01-01  (用户选择)
// queryDto.CreateTime_End = 2024-01-31    (用户选择)

// 4. 构建查询表达式
var exp = ExpressionBuilder.BuildFromQueryDto(queryDto);
// 结果: OrderNo.Contains("2024") 
//     && CustomerID == 12345 
//     && CreateTime >= DateTime(2024,1,1) 
//     && CreateTime <= DateTime(2024,1,31)
```

### 场景2: 产品查询 (支持多选)

```csharp
// 1. 定义多选查询过滤器
QueryFilter filter = new QueryFilter();
filter.QueryFields = new List<QueryField>
{
    new QueryField 
    { 
        FieldName = "CategoryID", 
        Caption = "产品分类",
        AdvQueryFieldType = AdvQueryProcessType.CmbMultiChoiceCanIgnore
    }
};

// 2. 生成UI (包含多选控件)
BaseEntity queryDto = UIGenerateHelper.CreateQueryUI(
    typeof(Product), 
    panel, 
    filter, 
    null);

// 3. 用户操作
// ☑ 可忽略 (不选分类条件)
// ☐ 电子产品
// ☑ 服装
// ☑ 食品

// 4. queryDto 包含:
// queryDto.CategoryID_CmbMultiChoiceCanIgnore = false  (不可忽略，必须查询)
// queryDto.CategoryID_MultiChoiceResults = [2, 3, 4]  (服装和食品)

// 5. 构建查询表达式
var exp = ExpressionBuilder.BuildFromQueryDto(queryDto);
// 结果: CategoryID.In([2, 3, 4])
```

## 优势

1. **类型安全**: 动态类型继承自原始实体，保持类型完整性
2. **灵活扩展**: 通过特性标记区分不同查询模式
3. **自动绑定**: UI与数据模型双向同步，无需手动转换
4. **高度可配置**: 支持个性化配置、默认值、日期偏移等
5. **动态布局**: 自动计算最优布局，适配不同字段组合

## 关键技术点

1. **动态程序集**: 使用 Reflection.Emit 运行时生成类型
2. **属性特性**: 使用 CustomAttributeBuilder 添加元数据标记
3. **数据绑定**: 通过反射和事件监听实现双向绑定
4. **泛型方法**: 动态构造泛型方法处理不同类型
5. **表达式转换**: 查询条件 → Lambda表达式 → SQL Where子句
