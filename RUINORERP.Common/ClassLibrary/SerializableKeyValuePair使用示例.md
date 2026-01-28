# SerializableKeyValuePair 使用示例

## 文件位置
`RUINORERP.Common/ClassLibrary/SerializableKeyValuePair.cs`

## 类结构概览

### 1. SerializableKeyValuePair<TKey, TValue>（双泛型版本）
适用于Key和Value都需要指定类型的场景。

### 2. SerializableKeyValuePair<TValue>（单泛型版本）
适用于Key固定为string的场景（最常用）。

### 3. SerializableKeyValuePair（非泛型版本）
适用于Key和Value都是string的场景（向后兼容）。

## 使用示例

### 示例1：在ColumnMapping中的应用

```csharp
using RUINORERP.Common;

public class ColumnMapping
{
    // 字段引用：string - string
    public SerializableKeyValuePair<string> SystemField { get; set; }

    // 表引用：string - string
    public SerializableKeyValuePair<string> TargetTable { get; set; }

    // 外键表引用：string - string
    public SerializableKeyValuePair<string> ForeignKeyTable { get; set; }

    // 外键字段引用：string - string
    public SerializableKeyValuePair<string> ForeignKeyField { get; set; }

    // 自身引用字段：string - string
    public SerializableKeyValuePair<string> SelfReferenceField { get; set; }

    // 配置项：string - int
    public SerializableKeyValuePair<int> MaxLength { get; set; }

    // 数据来源：string - enum
    public SerializableKeyValuePair<DataSourceType> DataSourceInfo { get; set; }
}
```

### 示例2：创建实例

```csharp
// 方法一：使用构造函数
var fieldRef = new SerializableKeyValuePair<string>("PID", "父类ID");
var tableRef = new SerializableKeyValuePair<string>("tb_ProdCategories", "产品类目表");

// 方法二：使用隐式转换
var fieldRef2 = new SerializableKeyValuePair<string>
{
    Key = "PID",
    Value = "父类ID"
};

// 方法三：从Tuple创建（仅限单泛型版本）
var fieldRef3 = ("PID", "父类ID"); // 隐式转换

// 方法四：从KeyValuePair创建
var kvp = new KeyValuePair<string, string>("PID", "父类ID");
var fieldRef4 = kvp; // 隐式转换
```

### 示例3：数据来源类型配置

```csharp
public enum DataSourceType
{
    Excel = 0,
    DefaultValue = 1,
    SystemGenerated = 2,
    ForeignKey = 3,
    SelfReference = 4
}

// 创建配置
var dataSource = new SerializableKeyValuePair<DataSourceType>("Source", DataSourceType.Excel);

// XML输出: <KeyValue Key="Source" Value="Excel" />

// 获取值
DataSourceType type = dataSource.Value; // DataSourceType.Excel
```

### 示例4：数值配置

```csharp
// 最大长度配置
var maxLength = new SerializableKeyValuePair<int>("MaxLength", 50);

// XML输出: <KeyValue Key="MaxLength" Value="50" />

// 获取值
int len = maxLength.Value; // 50

// 判断是否为空
if (maxLength.IsEmpty())
{
    // 配置未设置
}

// 获取默认值
int actualLength = maxLength.GetValueOrDefault(100); // 如果未设置，返回100
```

### 示例5：双泛型版本

```csharp
// Key为int，Value为string
var idMapping = new SerializableKeyValuePair<int, string>(100, "产品类目");

// XML输出: <KeyValue Key="100" Value="产品类目" />

// Key为枚举，Value为int
var config = new SerializableKeyValuePair<DataSourceType, int>(DataSourceType.Excel, 100);

// XML输出: <KeyValue Key="Excel" Value="100" />
```

### 示例6：UI层使用

```csharp
// 显示中文给用户
lblFieldName.Text = mapping.SystemField?.Value ?? "未设置";
lblTableName.Text = mapping.TargetTable?.Value ?? "未设置";

// 显示完整信息
lblFieldInfo.Text = mapping.SystemField?.ToString(); // 输出: "父类ID (PID)"

// 判断是否为空
if (mapping.SystemField?.IsKeyEmpty() == true)
{
    MessageBox.Show("请选择系统字段");
}
```

### 示例7：代码层操作

```csharp
// 获取英文标识用于数据库操作
string fieldName = mapping.SystemField?.Key; // "PID"
string tableName = mapping.TargetTable?.Key; // "tb_ProdCategories"

// 获取默认值
string key = mapping.SystemField.GetKeyOrDefault("ID");
string value = mapping.SystemField.GetValueOrDefault("默认字段");

// 转换为标准KeyValuePair
var kvp = (KeyValuePair<string, string>)mapping.SystemField;
```

### 示例8：XML序列化

```csharp
using System.Xml.Serialization;

// 定义配置类
[XmlRoot("ColumnMappings")]
public class ColumnMappingCollection
{
    [XmlElement("Mapping")]
    public List<ColumnMapping> Mappings { get; set; }
}

public class ColumnMapping
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlElement("SystemField")]
    public SerializableKeyValuePair<string> SystemField { get; set; }

    [XmlElement("TargetTable")]
    public SerializableKeyValuePair<string> TargetTable { get; set; }
}

// 创建配置
var collection = new ColumnMappingCollection
{
    Mappings = new List<ColumnMapping>
    {
        new ColumnMapping
        {
            Id = 1,
            SystemField = new SerializableKeyValuePair<string>("PID", "父类ID"),
            TargetTable = new SerializableKeyValuePair<string>("tb_ProdCategories", "产品类目表")
        }
    }
};

// 序列化为XML
var serializer = new XmlSerializer(typeof(ColumnMappingCollection));
using (var writer = new StreamWriter("ColumnMapping.xml"))
{
    serializer.Serialize(writer, collection);
}
```

#### XML输出格式：
```xml
<?xml version="1.0" encoding="utf-8"?>
<ColumnMappings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Mappings>
    <Mapping Id="1">
      <SystemField Key="PID" Value="父类ID" />
      <TargetTable Key="tb_ProdCategories" Value="产品类目表" />
    </Mapping>
  </Mappings>
</ColumnMappings>
```

### 示例9：XML反序列化

```csharp
// 从XML读取
var serializer = new XmlSerializer(typeof(ColumnMappingCollection));
using (var reader = new StreamReader("ColumnMapping.xml"))
{
    var collection = (ColumnMappingCollection)serializer.Deserialize(reader);

    foreach (var mapping in collection.Mappings)
    {
        Console.WriteLine($"系统字段: {mapping.SystemField?.Value} ({mapping.SystemField?.Key})");
        Console.WriteLine($"目标表: {mapping.TargetTable?.Value} ({mapping.TargetTable?.Key})");
    }
}
```

### 示例10：判空处理

```csharp
// string-string版本
var fieldRef = new SerializableKeyValuePair("PID", "父类ID");

// 判断Key是否为空
if (fieldRef.IsKeyEmpty())
{
    Console.WriteLine("Key为空");
}

// 判断Value是否为空
if (fieldRef.IsValueEmpty())
{
    Console.WriteLine("Value为空");
}

// 判断整体是否为空
if (fieldRef.IsEmpty())
{
    Console.WriteLine("键值对为空");
}

// 泛型版本的判空
var numRef = new SerializableKeyValuePair<int>("MaxCount", 0);
if (numRef.IsEmpty())
{
    Console.WriteLine("数值配置未设置或为默认值");
}
```

## 迁移指南

### 从旧的分离属性迁移到新的SerializableKeyValuePair

**旧代码：**
```csharp
public string SystemField { get; set; }              // 英文字段名
public string SystemFieldDisplayName { get; set; }   // 中文名

// 使用
string fieldName = mapping.SystemField;  // "PID"
string displayName = mapping.SystemFieldDisplayName;  // "父类ID"
```

**新代码：**
```csharp
public SerializableKeyValuePair<string> SystemField { get; set; }

// 使用
string fieldName = mapping.SystemField?.Key;  // "PID"
string displayName = mapping.SystemField?.Value;  // "父类ID"

// 或者使用隐式转换向后兼容（如果提供了兼容属性）
```

## API速查

### 属性
| 属性 | 类型 | 描述 |
|------|------|------|
| Key | TKey | 键 |
| Value | TValue | 值 |

### 方法
| 方法 | 返回类型 | 描述 |
|------|---------|------|
| IsEmpty() | bool | 判断键值对是否为空 |
| IsKeyEmpty() | bool | 判断Key是否为空（仅单泛型版本） |
| IsValueEmpty() | bool | 判断Value是否为空（仅非泛型版本） |
| GetKeyOrDefault(string) | string | 获取Key，为空则返回默认值 |
| GetValueOrDefault(T) | T | 获取Value，为空则返回默认值 |
| ToString() | string | 返回格式化字符串 "Value (Key)" |
| Empty | static T | 返回空实例（仅非泛型版本） |

### 隐式转换
| 源类型 | 目标类型 | 适用版本 |
|--------|---------|---------|
| KeyValuePair<TKey, TValue> | SerializableKeyValuePair<TKey, TValue> | 双泛型 |
| SerializableKeyValuePair<TKey, TValue> | KeyValuePair<TKey, TValue> | 双泛型 |
| (string key, TValue value) | SerializableKeyValuePair<TValue> | 单泛型 |

## XML序列化特性

- ✅ 使用`XmlAttribute`，序列化为属性形式
- ✅ 支持基本类型：string, int, long, bool, decimal等
- ✅ 支持枚举类型：自动序列化为字符串
- ✅ 支持DateTime：序列化为ISO 8601格式
- ⚠️ 不支持复杂对象：需要使用`XmlElement`而非`XmlAttribute`

## 注意事项

1. **XML序列化限制**：`XmlAttribute`只支持简单类型，复杂对象需要使用`XmlElement`
2. **null处理**：反序列化时，如果XML中没有该元素，属性将为null
3. **类型匹配**：反序列化时，必须与原始泛型类型完全匹配
4. **向后兼容**：使用非泛型版本`SerializableKeyValuePair`可简化代码迁移

## 总结

`SerializableKeyValuePair`提供了一个类型安全、可XML序列化的键值对结构，适用于：
- ✅ 列映射配置（字段名-字段名）
- ✅ 表引用配置（表名-表名）
- ✅ 配置项管理（键-值）
- ✅ 本地化数据（标识-显示文本）
- ✅ 任何需要XML序列化的键值对场景
