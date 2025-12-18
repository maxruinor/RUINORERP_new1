# SKU属性更新使用示例

## 场景说明

在产品管理系统中，SKU编码的生成和更新是一个重要功能。当用户编辑产品属性时，我们需要保持产品的序号部分不变，只更新属性相关的标识部分。

## 核心需求

1. **新建产品时** - 生成完整的SKU编码（包含类目、序号、属性）
2. **编辑产品时** - 保持序号不变，仅更新属性部分
3. **确保唯一性** - 更新后的SKU不能与现有SKU冲突

## 使用示例

### 1. 新建产品生成SKU

```csharp
// 创建新产品
var newProduct = new tb_Prod
{
    Prod_ID = 1001,
    ProductName = "车载充电器",
    Category_ID = 1,
    tb_prodcategories = new tb_ProdCategories 
    { 
        Category_name = "车载设备"
    },
    // 设置产品属性
    tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relations>
    {
        new tb_Prod_Attr_Relations 
        { 
            PropertyName = "颜色", 
            PropertyValue = "白色" 
        },
        new tb_Prod_Attr_Relations 
        { 
            PropertyName = "型号", 
            PropertyValue = "QC3.0" 
        }
    }
};

// 生成SKU编码
var skuGenerator = new ProductSKUCodeGenerator(logger, bnrFactory, db);
string newSku = skuGenerator.GenerateSKUCodeAsync(newProduct);

// 输出示例: CZ0001C-WM-QC  (车载设备，第1个产品，白色QC3.0型号)
Console.WriteLine($"新生成的SKU: {newSku}");
```

### 2. 编辑产品属性并更新SKU

```csharp
// 假设这是用户编辑的产品
var editedProduct = new tb_Prod
{
    Prod_ID = 1001,
    ProductName = "车载充电器",
    Category_ID = 1,
    tb_prodcategories = new tb_ProdCategories 
    { 
        Category_name = "车载设备"
    },
    // 更新后的属性
    tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relations>
    {
        new tb_Prod_Attr_Relations 
        { 
            PropertyName = "颜色", 
            PropertyValue = "黑色"  // 从白色改为黑色
        },
        new tb_Prod_Attr_Relations 
        { 
            PropertyName = "型号", 
            PropertyValue = "PD65W"  // 从QC3.0改为PD65W
        }
    }
};

// 原有的SKU编码
string existingSku = "CZ0001C-WM-QC";

// 更新SKU的属性部分（保持序号CZ0001不变）
string updatedSku = skuGenerator.UpdateSKUAttributePart(existingSku, editedProduct);

// 输出示例: CZ0001C-BM-PD  (车载设备，第1个产品，黑色PD65W型号)
Console.WriteLine($"更新后的SKU: {updatedSku}");
```

### 3. 处理属性变化导致的SKU冲突

```csharp
// 场景：另一个产品已经有黑色PD65W的SKU
// 系统中已存在: CZ0001C-BM-PD

var editedProduct2 = new tb_Prod
{
    Prod_ID = 1002,
    ProductName = "车载充电器Pro",
    Category_ID = 1,
    tb_prodcategories = new tb_ProdCategories 
    { 
        Category_name = "车载设备"
    },
    tb_Prod_Attr_Relations = new List<tb_Prod_Attr_Relations>
    {
        new tb_Prod_Attr_Relations 
        { 
            PropertyName = "颜色", 
            PropertyValue = "黑色"  
        },
        new tb_Prod_Attr_Relations 
        { 
            PropertyName = "型号", 
            PropertyValue = "PD65W"  
        }
    }
};

string existingSku2 = "CZ0002C-WM-QC";
string updatedSku2 = skuGenerator.UpdateSKUAttributePart(existingSku2, editedProduct2);

// 由于CZ0002C-BM-PD已存在，系统会自动添加序号
// 输出示例: CZ0002C-BM-PD01  (添加序号01避免冲突)
Console.WriteLine($"处理冲突后的SKU: {updatedSku2}");
```

## 方法详解

### UpdateSKUAttributePart 方法

```csharp
/// <summary>
/// 更新SKU编码的属性部分（保持序号不变，仅更新属性标识）
/// 用于产品编辑时，当属性发生变化需要更新SKU编码的情况
/// </summary>
/// <param name="existingSku">现有的SKU编码</param>
/// <param name="prod">更新后的产品实体</param>
/// <returns>更新后的SKU编码</returns>
public string UpdateSKUAttributePart(string existingSku, tb_Prod prod)
```

#### 处理流程

1. **解析现有SKU**
   - 提取类目代码（如：CZ）
   - 提取序号部分（如：0001）
   - 提取属性部分（如：C-WM-QC）

2. **生成新属性标识**
   - 根据更新后的产品属性重新生成属性代码

3. **组合新SKU**
   - 类目代码 + 序号部分 + 新属性代码

4. **唯一性检查**
   - 确保新SKU不与现有SKU冲突
   - 如有冲突则添加序号后缀

### 辅助方法

#### ExtractCategoryCode
```csharp
/// <summary>
/// 从SKU编码中提取类目代码
/// </summary>
private string ExtractCategoryCode(string sku)
```

#### ExtractSequencePart
```csharp
/// <summary>
/// 从SKU编码中提取序号部分
/// </summary>
private string ExtractSequencePart(string sku)
```

#### EnsureUniqueSKUExceptSelf
```csharp
/// <summary>
/// 确保SKU编码唯一性（排除指定的SKU）
/// </summary>
private string EnsureUniqueSKUExceptSelf(string baseSkuCode, string excludeSku)
```

## 属性代码映射规则

### 颜色属性映射
```
红色 → R    蓝色 → B    绿色 → G
黄色 → Y    黑色 → K    白色 → W
粉色 → P    紫色 → P    橙色 → O
灰色 → G    银色 → S    金色 → G
```

### 型号/规格属性映射
```
型号 → M    材质 → T    尺寸 → S
容量 → R    重量 → W    版本 → V
长度 → L    宽度 → W    高度 → H
```

## 实际应用示例

### 示例1：手机产品SKU更新
```
原有SKU: PH0001C-WM-128  (手机，第1个，白色，128GB)
更新后:  PH0001C-BM-256  (手机，第1个，黑色，256GB)
```

### 示例2：服装产品SKU更新
```
原有SKU: CL0056S-MB-XL   (服装，第56个，男款，蓝色，XL)
更新后:  CL0056S-MR-XL   (服装，第56个，男款，红色，XL)
```

## 错误处理

### 异常情况处理

1. **SKU解析失败**
   - 返回原有SKU，避免数据丢失
   - 记录错误日志供排查

2. **属性生成异常**
   - 使用默认属性代码
   - 记录异常信息

3. **数据库访问异常**
   - 使用内存缓存进行唯一性检查
   - 必要时添加时间戳确保唯一性

### 降级策略

```csharp
catch (Exception ex)
{
    _logger.LogError(ex, "更新SKU属性部分时发生错误，现有SKU: {ExistingSku}", existingSku);
    // 出错时返回原有SKU，避免丢失数据
    return existingSku;
}
```

## 性能优化

### 缓存机制
- 使用 `_skuCache` 缓存已存在的SKU
- 减少数据库查询次数
- 提高唯一性检查效率

### 批量操作支持
```csharp
public async Task RefreshSKUCacheAsync()
{
    // 批量加载SKU到缓存
    var skus = await query.ToListAsync();
    foreach (var sku in skus)
    {
        _skuCache.TryAdd(sku, true);
    }
}
```

## 测试用例

### 单元测试示例

```csharp
[Test]
public void UpdateSKUAttributePart_ShouldPreserveSequence()
{
    // Arrange
    var product = CreateTestProductWithAttributes();
    string existingSku = "CZ0001C-WM-QC";
    
    // Act
    string result = skuGenerator.UpdateSKUAttributePart(existingSku, product);
    
    // Assert
    Assert.IsTrue(result.StartsWith("CZ0001"));  // 序号部分保持不变
    Assert.AreNotEqual(existingSku, result);     // 属性部分已更新
}
```

## 配置建议

### 推荐配置

1. **类目代码长度**：2-4个字符
2. **序号部分**：4-6位数字，自动补零
3. **属性代码**：每个属性1-2个字符
4. **总长度控制**：建议不超过20个字符

### 最佳实践

1. **类目管理**：统一维护类目名称和代码映射
2. **属性标准化**：建立标准的属性名称和值映射表
3. **缓存刷新**：定期刷新SKU缓存确保数据一致性
4. **日志记录**：详细记录SKU生成和更新过程
