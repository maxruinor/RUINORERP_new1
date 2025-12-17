# 动态表达式构建器使用指南

## 概述

动态表达式构建器是一个完全动态的框架，用于处理ERP系统中的各种查询和缓存过滤需求。它不预先指定字段名，完全在运行时动态处理表达式，彻底解决了硬编码字段名和闭包变量处理的问题，提供了最灵活、最可维护的表达式构建方式。

## 主要优势

1. **完全动态处理**：不预先指定任何字段名，完全在运行时动态处理表达式
2. **强大的闭包变量支持**：使用闭包处理访问器，完全解决闭包变量导致的编译错误
3. **与SqlSugar完美集成**：直接使用SqlSugar的Expressionable构建表达式
4. **统一的表达式处理**：提供一致的接口，适用于整个ERP系统中的各种查询和过滤场景
5. **更好的可维护性**：通过集中管理表达式构建逻辑，减少重复代码

## 核心组件

### 1. IDynamicExpressionBuilder 接口

动态表达式构建器接口，提供完全动态的表达式构建方法。

### 2. DynamicExpressionBuilder 类

动态表达式构建器实现，提供以下主要方法：

- `ProcessExpression<T>()` - 处理现有表达式，支持闭包变量
- `SafeEvaluate<T>()` - 安全地评估表达式，支持闭包变量
- `BuildWithSqlSugar<T>()` - 使用SqlSugar的Expressionable构建表达式

### 3. ClosureHandlingVisitor 类

闭包处理访问器，用于处理表达式中的闭包变量。

### 4. DynamicExpressionFactory 类

动态表达式工厂，提供便捷的静态方法：

- `Builder` - 获取动态表达式构建器实例
- `Evaluator` - 获取表达式安全评估器实例
- `SafeFilter<T>()` - 安全地执行过滤操作
- `FilterWithSqlSugar<T>()` - 使用SqlSugar构建表达式并执行过滤
- `BuildFromSqlSugar<T>()` - 从SqlSugar的Expressionable构建表达式

## 使用方法

### 1. 使用SqlSugar构建表达式

```csharp
// 完全动态处理表达式，不预先指定字段名
DataBindingHelper.InitDataToCmbWithDynamicExpression<tb_FM_PayeeInfo>(
    key: "PayeeInfoID",
    value: "DisplayText",
    tableName: "tb_FM_PayeeInfo",
    cmbBox: cmbPayeeInfoID,
    expressionBuilder: q => q.And(t => t.Is_enabled == true)
                      .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
);
```

### 2. 直接使用表达式

```csharp
// 完全动态处理表达式，不预先指定字段名
DataBindingHelper.InitDataToCmbWithDynamicExpression<tb_FM_PayeeInfo>(
    key: "PayeeInfoID",
    value: "DisplayText",
    tableName: "tb_FM_PayeeInfo",
    cmbBox: cmbPayeeInfoID,
    expression: t => t.Is_enabled == true && t.CustomerVendor_ID == entity.CustomerVendor_ID
);
```

### 3. 处理复杂闭包变量表达式

```csharp
// 使用动态表达式构建器处理包含闭包变量的表达式
var customerId = 123;
var status = true;
var keyword = "测试";

Expression<Func<tb_CustomerVendor, bool>> closureExpression = c =>
    c.Is_enabled == status && 
    c.CustomerVendor_ID == customerId && 
    c.CVName.Contains(keyword);

var processedExpression = DynamicExpressionFactory.Builder.ProcessExpression(closureExpression);
var filteredList = DynamicExpressionFactory.SafeFilter(sourceList, processedExpression);
```

### 4. 使用SqlSugar构建复杂查询

```csharp
// 使用SqlSugar的Expressionable构建复杂查询
var queryExpression = DynamicExpressionFactory.BuildFromSqlSugar<tb_PurOrder>(
    q => q.And(p => p.DataStatus == (int)DataStatus.确认)
         .And(p => p.ApprovalStatus == (int)ApprovalStatus.审核通过)
         .AndIf(true, p => p.Employee_ID == 123)  // 条件性添加表达式
);
```

## 迁移指南

### 从硬编码表达式迁移

**旧代码：**
```csharp
var lambdaPayeeInfo = Expressionable.Create<tb_FM_PayeeInfo>()
    .And(t => t.Is_enabled == true)
    .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
    .ToExpression();
```

**新代码：**
```csharp
DataBindingHelper.InitDataToCmbWithDynamicExpression<tb_FM_PayeeInfo>(
    key: "PayeeInfoID",
    value: "DisplayText",
    tableName: "tb_FM_PayeeInfo",
    cmbBox: cmbPayeeInfoID,
    expressionBuilder: q => q.And(t => t.Is_enabled == true)
                      .And(t => t.CustomerVendor_ID == entity.CustomerVendor_ID)
);
```

## 架构优势

### 1. 完全动态处理

- 不预先指定任何字段名
- 完全在运行时动态处理表达式
- 支持任意实体类型的任意字段

### 2. 强大的闭包变量支持

- 使用闭包处理访问器自动处理闭包变量
- 无需手动提取闭包变量值
- 支持复杂的闭包变量表达式

### 3. 与SqlSugar完美集成

- 直接使用SqlSugar的Expressionable构建表达式
- 支持SqlSugar的所有表达式方法
- 可以无缝替换现有的SqlSugar表达式构建方式

## 最佳实践

1. **使用SqlSugar构建表达式**：对于复杂表达式，推荐使用SqlSugar的Expressionable
2. **完全动态处理**：不要预先指定字段名，完全在运行时动态处理表达式
3. **使用安全过滤**：对于可能包含闭包变量的表达式，使用`DynamicExpressionFactory.SafeFilter`
4. **条件性表达式**：使用SqlSugar的`AndIf`、`OrIf`等方法构建条件性表达式
5. **错误处理**：动态表达式构建器内置了错误处理，但建议在关键路径上添加额外的错误处理

## 注意事项

1. **表达式复杂性**：对于非常复杂的表达式，建议分解为多个简单表达式再组合
2. **性能考虑**：动态表达式构建器有优化机制，但仍建议避免在循环中重复构建复杂表达式
3. **类型匹配**：确保表达式中的类型匹配，特别是在使用闭包变量时

## 示例代码

详细的使用示例请参考`DynamicExpressionBuilderUsageExample.cs`文件。