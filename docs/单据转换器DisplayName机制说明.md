# 单据转换器 DisplayName 机制 - 架构说明

## 核心问题

**收/付款场景**：共用同一张表 `tb_FM_PaymentRecord`，通过 `ReceivePaymentType` 字段区分"收款"或"付款"。

**挑战**：
- 基类无法访问实例数据（只有类型信息）
- 工厂层可以访问实例数据（有具体对象）
- 需要支持：固定文本（如"退还余款"）和动态文本（如"收款"/"付款"）

## 解决方案：两层职责分工

### 1. 基类 DocumentConverterBase（无实例数据）

**职责**：默认返回 `null`，表示"未指定"

```csharp
public virtual string DisplayName => null;
```

**子类重写示例**：
```csharp
// 固定业务文本
public override string DisplayName => "退还余款";
```

**限制**：
- ❌ 无法返回 "收款" 或 "付款"（需要实例的 `ReceivePaymentType`）
- ✅ 返回 `null` 表示由工厂层动态生成

### 2. 工厂层 DocumentConverterFactory（有实例数据）

**职责**：根据实例数据动态计算显示名称

```csharp
private string GetConverterDisplayName(object converter, string sourceDisplayName, 
                                       string targetDisplayName, DocumentConversionType conversionType)
{
    // 步骤1: 尝试获取转换器重写的 DisplayName
    string converterDisplayName = GetDisplayNameFromConverter(converter);
    
    // 步骤2: 如果子类重写了(非null),直接使用
    if (!string.IsNullOrEmpty(converterDisplayName))
    {
        return converterDisplayName;  // 例如: "退还余款"
    }
    
    // 步骤3: 子类未重写(null),根据实例数据动态生成
    if (conversionType == DocumentConversionType.ActionOperation)
    {
        return targetDisplayName;  // "收款" 或 "付款"
    }
    else
    {
        return $"转为{targetDisplayName}";  // "转为收款"、"转为销售出库单"
    }
}
```

**关键方法**：`GetEntityDisplayName(Type entityType, object sourceEntity)`

```csharp
private string GetEntityDisplayName(Type entityType, object sourceEntity = null)
{
    if (sourceEntity != null && sourceEntity is BaseEntity baseEntity)
    {
        // 检查是否有 ReceivePaymentType 属性
        if (baseEntity.ContainsProperty(nameof(ReceivePaymentType)))
        {
            var paymentType = baseEntity.GetPropertyValue(nameof(ReceivePaymentType)).ToInt();
            // 根据枚举值返回 "收款" 或 "付款"
            return BizMapperService.EntityMappingHelper.GetEnumDescription(paymentType);
        }
    }
    
    // 兜底：返回 Description 特性或类型名称
    return entityType.GetCustomAttribute<DescriptionAttribute>()?.Description ?? entityType.Name;
}
```

## 使用场景

### 场景1：固定业务文本（子类重写）

```csharp
public class PreReceivedPaymentToRefundConverter : DocumentConverterBase<...>
{
    public override string DisplayName => "退还余款";
}
```

**结果**：无论源单据是预收款还是预付款，都显示 "退还余款"

### 场景2：动态业务文本（不重写，工厂层计算）

```csharp
public class PreReceivedPaymentToPaymentRecordConverter : DocumentConverterBase<...>
{
    // 不重写 DisplayName
}
```

**结果**：
- 源单据是预收款单 → 显示 "收款"
- 源单据是预付款单 → 显示 "付款"

## UI 层调用示例

```csharp
// 获取可用的转换选项
var options = converterFactory.GetAvailableConversions(sourceEntity);

foreach (var option in options)
{
    // 使用工厂计算的 DisplayName（已包含动态逻辑）
    Console.WriteLine(option.DisplayName);
    // 输出示例:
    // - "退还余款" (硬编码)
    // - "收款" (动态计算)
    // - "付款" (动态计算)
}
```

## 优先级规则

```
┌─────────────────────────────────────────┐
│ 1. 子类重写 DisplayName?                │
│    YES (非null) → 返回子类的值          │
│    NO (null) ↓                          │
├─────────────────────────────────────────┤
│ 2. 工厂层根据实例数据动态生成            │
│    - ActionOperation: targetDisplayName │
│      例如: "收款"、"付款"               │
│    - DocumentGeneration:                │
│      "转为{targetDisplayName}"          │
│      例如: "转为收款"、"转为销售出库单"  │
└─────────────────────────────────────────┘
```

**设计理念**: 用户已知当前单据,只需告知目标是什么,无需重复显示来源。

## 关键设计原则

1. **基类返回 null**：表示"未指定",由工厂层处理
2. **工厂层负责动态计算**：因为有实例数据
3. **子类可选择性重写**：用于固定业务文本
4. **UI 层使用工厂返回值**：而非直接访问转换器的 DisplayName 属性
5. **简洁清晰**：通过 null 判断，无需复杂的默认值检测逻辑
6. **用户体验优先**：显示名称简洁明了，避免冗余信息（用户已知当前单据）

## 常见问题

### Q: 为什么基类返回 null 而不是类型名称?

A: 
- 返回 null 表示"未指定",让工厂层完全控制显示名称的生成逻辑
- 如果返回类型名称,工厂层需要额外判断"是否是默认值",增加复杂度
- null 语义清晰: null = 动态生成, 非null = 使用重写值

### Q: 如何判断是否需要重写 DisplayName?

A: 
- 如果业务语义固定（如"退还余款"、"订单取消作废"），重写
- 如果需要根据实例数据变化（如"收款"/"付款"），不重写，保持 null

### Q: UI 层应该使用哪个 DisplayName?

A: 始终使用 `ConversionOption.DisplayName`（工厂层返回），不要直接使用转换器的 `DisplayName` 属性。

### Q: 这个方案的优势是什么?

A:
1. **职责清晰**：基类只负责提供扩展点，工厂层负责动态逻辑
2. **代码简洁**：通过 null 判断，无需复杂的默认值检测
3. **易于维护**：新增转换器时，只需决定是否需要重写 DisplayName
4. **灵活性高**：支持固定文本和动态文本两种场景
5. **用户体验好**：显示名称简洁，避免冗余信息（"转为收款" 而非 "预收款单转收款"）
