# 图片保存与HasAttachment同步集成方案

## 一、问题背景

在单据保存和图片更新的流程中，HasAttachment标识位没有正确同步，导致：
1. 初始保存单据时，即使上传了图片，HasAttachment仍为false
2. 后续修改添加图片时，HasAttachment没有被更新为true
3. 删除图片时，HasAttachment没有被正确更新为false

## 二、完整业务流程

### 场景1：新建单据并上传图片

```
用户操作                    客户端                          服务器端
   │                          │                               │
   ├─ 1.填写单据信息          │                               │
   │                          │                               │
   ├─ 2.上传图片              ├─ FileUploadRequest           │
   │                          │  - BusinessType              │
   │                          │  - BusinessId (待分配)       │
   │                          │  - BusinessNo                │
   │                          ├──────────────────────────────>│
   │                          │                               │
   │                          │                               ├─ 3.保存文件到磁盘
   │                          │                               │  SaveNewFileAsync()
   │                          │                               │
   │                          │                               ├─ 4.创建文件元数据
   │                          │                               │  tb_FS_FileStorageInfo
   │                          │                               │
   │                          │                               ├─ 5.创建业务关联
   │                          │                               │  tb_FS_BusinessRelation
   │                          │                               │  (BusinessId可能为null)
   │                          │                               │
   │                          │                               ├─ 6.同步HasAttachment ✅
   │                          │                               │  HasAttachmentSyncService
   │                          │                               │  .SyncOnFileUploadAsync()
   │                          │                               │
   │                          │<──────────────────────────────┤ FileUploadResponse
   │                          │  - FileId                    │
   │                          │  - Success                    │
   │                          │                               │
   ├─ 7.保存单据              ├─ SaveEntity()                │
   │                          │  - 设置BusinessId             │
   │                          ├──────────────────────────────>│
   │                          │                               ├─ 8.更新关联记录
   │                          │                               │  (补充BusinessId)
   │                          │                               │
   │                          │                               ├─ 9.同步HasAttachment ✅
   │                          │                               │  (确保标志为true)
   │                          │                               │
   │                          │<──────────────────────────────┤ SaveResponse
   │                          │  - Success                    │
   │                          │  - BusinessId                 │
   │                          │                               │
   ▼                          ▼                               ▼
```

### 场景2：已保存单据更新图片

```
用户操作                    客户端                          服务器端
   │                          │                               │
   ├─ 1.修改单据图片          │                               │
   │                          │                               │
   ├─ 2.更新图片              ├─ FileUpdateRequest           │
   │                          │  - BusinessType              │
   │                          │  - BusinessNo                │
   │                          │  - BusinessId                │
   │                          │  - RelatedField              │
   │                          │  - FileData                  │
   │                          ├──────────────────────────────>│
   │                          │                               │
   │                          │                               ├─ 3.查询旧关联
   │                          │                               │  QueryBusinessRelation()
   │                          │                               │
   │                          │                               ├─ 4.处理旧文件
   │                          │                               │  (根据策略)
   │                          │                               │
   │                          │                               ├─ 5.保存新文件 ✅
   │                          │                               │  FileUpdateService
   │                          │                               │  .SaveNewFileAsync()
   │                          │                               │
   │                          │                               ├─ 6.创建新关联 ✅
   │                          │                               │  CreateNewRelationAsync()
   │                          │                               │
   │                          │                               ├─ 7.同步HasAttachment ✅
   │                          │                               │  HasAttachmentSyncService
   │                          │                               │  .SyncOnFileUploadAsync()
   │                          │                               │
   │                          │<──────────────────────────────┤ FileUpdateResponse
   │                          │  - NewFileId                  │
   │                          │  - Success                    │
   │                          │                               │
   ▼                          ▼                               ▼
```

### 场景3：删除单据图片

```
用户操作                    客户端                          服务器端
   │                          │                               │
   ├─ 1.删除图片              ├─ FileDeleteRequest           │
   │                          │  - BusinessNo                │
   │                          ├──────────────────────────────>│
   │                          │                               │
   │                          │                               ├─ 2.查询关联
   │                          │                               │  GetRelationsByBusinessNo()
   │                          │                               │
   │                          │                               ├─ 3.逻辑删除关联
   │                          │                               │  tb_FS_BusinessRelation
   │                          │                               │  .isdeleted = true
   │                          │                               │
   │                          │                               ├─ 4.同步HasAttachment ✅
   │                          │                               │  HasAttachmentSyncService
   │                          │                               │  .SyncOnFileDeleteAsync()
   │                          │                               │  (检查是否还有其他关联)
   │                          │                               │
   │                          │                               ├─ 5.设置标志
   │                          │                               │  - 有其他关联: HasAttachment=1
   │                          │                               │  - 无其他关联: HasAttachment=0
   │                          │                               │
   │                          │<──────────────────────────────┤ FileDeleteResponse
   │                          │  - Success                    │
   │                          │                               │
   ▼                          ▼                               ▼
```

## 三、代码实现

### 1. HasAttachmentSyncService服务

**文件：** `RUINORERP.Server/Network/Services/HasAttachmentSyncService.cs`

**功能：**
- 自动检测表是否有HasAttachment字段
- 支持三种同步场景：
  - `SyncOnFileUploadAsync()` - 文件上传时设置HasAttachment=1
  - `SyncOnFileDeleteAsync()` - 文件删除时更新HasAttachment标志
  - `SyncBatchAsync()` - 批量同步整个表的HasAttachment标志

**关键方法：**

```csharp
/// <summary>
/// 文件上传时同步HasAttachment标志
/// </summary>
public async Task<bool> SyncOnFileUploadAsync(
    int businessType, 
    long businessId, 
    string businessNo)
{
    // 1. 获取表名和主键字段名
    // 2. 检查表是否有HasAttachment字段
    // 3. 更新HasAttachment=1
    // 4. 返回同步结果
}

/// <summary>
/// 文件删除时同步HasAttachment标志
/// </summary>
public async Task<bool> SyncOnFileDeleteAsync(
    int businessType, 
    long businessId)
{
    // 1. 检查是否还有其他关联的文件
    // 2. 有: HasAttachment=1
    // 3. 无: HasAttachment=0
}

/// <summary>
/// 批量同步HasAttachment标志
/// </summary>
public async Task<int> SyncBatchAsync(
    int businessType)
{
    // 1. 查询所有业务记录
    // 2. 对每条记录检查是否有关联文件
    // 3. 更新HasAttachment标志
}
```

### 2. FileCommandHandler集成

**文件：** `RUINORERP.Server/Network/CommandHandlers/FileCommandHandler.cs`

**修改点1：构造函数注入HasAttachmentSyncService**
```csharp
public FileCommandHandler(
    // ... 其他参数
    HasAttachmentSyncService hasAttachmentSyncService = null)
{
    // ... 其他初始化
    _hasAttachmentSyncService = hasAttachmentSyncService;
}
```

**修改点2：文件上传后同步（第328-334行）**
```csharp
// 同步HasAttachment标志
if (_hasAttachmentSyncService != null 
    && uploadRequest.BusinessType.HasValue 
    && uploadRequest.BusinessId.HasValue)
{
    await _hasAttachmentSyncService.SyncOnFileUploadAsync(
        uploadRequest.BusinessType.Value,
        uploadRequest.BusinessId.Value,
        uploadRequest.BusinessNo);
}
```

**修改点3：文件删除后同步（第1053-1063行）**
```csharp
// 同步HasAttachment标志（在删除关联后）
if (relationDeletedCount > 0 
    && _hasAttachmentSyncService != null 
    && relationsToDelete.Count > 0)
{
    var firstRelation = relationsToDelete.FirstOrDefault();
    if (firstRelation != null)
    {
        await _hasAttachmentSyncService.SyncOnFileDeleteAsync(
            firstRelation.BusinessType,
            firstRelation.BusinessId);
    }
}
```

### 3. FileUpdateService集成

**文件：** `RUINORERP.Server/Network/Services/FileUpdateService.cs`

**修改点1：构造函数注入HasAttachmentSyncService**
```csharp
public FileUpdateService(
    IUnitOfWorkManage unitOfWorkManage,
    ILogger<FileUpdateService> logger,
    ServerGlobalConfig serverConfig,
    HasAttachmentSyncService hasAttachmentSyncService = null)
{
    _unitOfWorkManage = unitOfWorkManage;
    _logger = logger;
    _serverConfig = serverConfig;
    _hasAttachmentSyncService = hasAttachmentSyncService;
}
```

**修改点2：UpdateBusinessFileAsync后同步（第143-148行）**
```csharp
// 同步HasAttachment标志
if (_hasAttachmentSyncService != null)
{
    await _hasAttachmentSyncService.SyncOnFileUploadAsync(
        businessType,
        businessId,
        businessNo);
}

return (newFileInfo, newRelation, oldFiles);
```

**修改点3：BatchUpdateBusinessFilesAsync后同步（第276-283行）**
```csharp
// 同步HasAttachment标志
if (_hasAttachmentSyncService != null && result.SuccessFiles.Count > 0)
{
    await _hasAttachmentSyncService.SyncOnFileUploadAsync(
        businessType,
        businessId,
        businessNo);
}

return result;
```

### 4. 服务注册

**文件：** `RUINORERP.Server/Network/DI/NetworkServicesDependencyInjection.cs`

**注册位置：** 文件服务相关区域（第99-102行）

```csharp
// 注册图片缓存和HasAttachment同步服务(单例,长期运行)
services.AddSingleton<ImageCacheService>();
services.AddSingleton<HasAttachmentSyncService>();
```

**using语句：**
```csharp
using RUINORERP.UI.Network.Services; // ImageCacheService
using RUINORERP.Server.Network.Services; // HasAttachmentSyncService
```

## 四、数据库表结构

### HasAttachment字段定义

支持的表需要包含HasAttachment字段：

```sql
-- 例如：销售订单表
ALTER TABLE tb_SaleOrder 
ADD HasAttachment BIT DEFAULT 0;

-- 例如：产品表
ALTER TABLE tb_Prod 
ADD HasAttachment BIT DEFAULT 0;
```

### 已迁移的表

以下表已包含HasAttachment字段（执行`数据库迁移_单据图片存储优化.sql`）：
- ✅ tb_SaleOrder (销售订单)
- ✅ tb_Prod (产品)
- ✅ tb_PurchaseOrder (采购订单)
- ✅ tb_PurchaseReturn (采购退货)
- ✅ tb_SaleReturn (销售退货)
- ✅ tb_Receipt (收款单)
- ✅ tb_Payment (付款单)
- ✅ tb_Expense (费用报销单)

## 五、安全机制

### 1. 表结构检查

在同步前会检查表是否有HasAttachment字段：

```csharp
var hasColumn = db.DbMaintenance.IsAnyColumn(tableName, "HasAttachment");
if (!hasColumn)
{
    _logger?.LogDebug("表 {TableName} 没有HasAttachment字段，跳过同步", tableName);
    return false; // 安全跳过，不影响主流程
}
```

### 2. 异常保护

所有同步操作都在try-catch块中，确保不影响主流程：

```csharp
try
{
    // 同步逻辑
}
catch (Exception ex)
{
    _logger?.LogError(ex, "同步HasAttachment标志失败");
    return false; // 不影响主流程
}
```

### 3. 未知业务类型处理

```csharp
var tableName = GetTableNameByBusinessType(businessType);
if (string.IsNullOrEmpty(tableName))
{
    _logger?.LogWarning("未知的业务类型，跳过HasAttachment同步");
    return false;
}
```

## 六、使用示例

### 示例1：保存销售订单并上传图片

```csharp
// 客户端代码（UCSaleOrder.cs）
// 1. 上传图片
var uploadResult = await fileUploadClient.UploadFileAsync(
    businessType: 1001, // 销售订单业务类型
    businessNo: "SO202501001",
    businessId: null,   // 新单据还未分配ID
    relatedField: "VoucherImage",
    fileData: imageBytes,
    fileName: "product.jpg"
);

// 2. 保存单据
var saleOrder = new tb_SaleOrder
{
    OrderNo = "SO202501001",
    OrderDate = DateTime.Now,
    // HasAttachment将在服务器端自动同步为1
};

var saveResult = await saleOrderController.SaveEntityAsync(saleOrder);
```

### 示例2：更新已有单据的图片

```csharp
// 客户端代码
var updateResult = await fileUpdateClient.UpdateBusinessFileAsync(
    businessType: 1001,
    businessNo: "SO202501001",
    businessId: 100001, // 已有的单据ID
    relatedField: "VoucherImage",
    newFileData: newImageBytes,
    newFileName: "product_updated.jpg",
    strategy: UpdateStrategy.Replace // 替换旧图片
);

// HasAttachment自动保持为1
```

### 示例3：删除图片

```csharp
// 客户端代码
var deleteResult = await fileDeleteClient.DeleteFileAsync(
    businessNo: "SO202501001"
);

// HasAttachment自动更新：
// - 如果还有其他图片: HasAttachment=1
// - 如果没有图片: HasAttachment=0
```

### 示例4：批量修复历史数据

```csharp
// 如果历史数据HasAttachment不正确，可以执行批量同步
await hasAttachmentSyncService.SyncBatchAsync(1001); // 销售订单

// 该方法会：
// 1. 查询所有销售订单
// 2. 对每条订单检查是否有关联的图片文件
// 3. 更新HasAttachment标志
```

## 七、验证测试

### 测试用例1：新建单据上传图片

**步骤：**
1. 新建销售订单，上传一张图片
2. 保存单据
3. 检查数据库HasAttachment字段

**预期结果：**
- ✅ 文件保存成功
- ✅ 关联关系创建成功
- ✅ HasAttachment = 1

### 测试用例2：已有单据添加图片

**步骤：**
1. 打开已有的销售订单（HasAttachment=0）
2. 上传一张图片
3. 检查数据库HasAttachment字段

**预期结果：**
- ✅ 文件保存成功
- ✅ 关联关系创建成功
- ✅ HasAttachment = 1

### 测试用例3：删除最后一张图片

**步骤：**
1. 打开有且只有一张图片的销售订单（HasAttachment=1）
2. 删除图片
3. 检查数据库HasAttachment字段

**预期结果：**
- ✅ 关联关系标记为删除
- ✅ HasAttachment = 0

### 测试用例4：删除多张图片中的一张

**步骤：**
1. 打开有多张图片的销售订单（HasAttachment=1）
2. 删除其中一张图片
3. 检查数据库HasAttachment字段

**预期结果：**
- ✅ 关联关系标记为删除
- ✅ HasAttachment = 1（仍有其他图片）

### 测试用例5：无HasAttachment字段的表

**步骤：**
1. 对没有HasAttachment字段的表上传文件
2. 检查日志

**预期结果：**
- ✅ 文件保存成功
- ✅ 日志显示"表XXX没有HasAttachment字段，跳过同步"
- ✅ 不影响主流程

## 八、故障排查

### 问题1：HasAttachment没有更新

**检查项：**
1. 表是否有HasAttachment字段？
   ```sql
   SELECT COLUMN_NAME 
   FROM INFORMATION_SCHEMA.COLUMNS 
   WHERE TABLE_NAME = 'tb_SaleOrder' 
   AND COLUMN_NAME = 'HasAttachment'
   ```

2. 服务是否正确注入？
   ```csharp
   // 检查HasAttachmentSyncService是否为null
   if (_hasAttachmentSyncService == null)
   {
       _logger.LogError("HasAttachmentSyncService未注入");
   }
   ```

3. 检查日志输出
   - Debug级别：字段检查信息
   - Warning级别：未知业务类型
   - Error级别：同步失败

### 问题2：SqlSugar FieldCount错误

**解决：**
参见 `SqlSugar_FieldCount错误诊断与修复.md`

## 九、总结

### 已集成的功能点

| 功能点 | 集成状态 | 集成位置 |
|--------|---------|---------|
| 文件上传 → HasAttachment同步 | ✅ 已集成 | FileCommandHandler.HandleFileUploadAsync |
| 文件更新 → HasAttachment同步 | ✅ 已集成 | FileUpdateService.UpdateBusinessFileAsync |
| 批量更新 → HasAttachment同步 | ✅ 已集成 | FileUpdateService.BatchUpdateBusinessFilesAsync |
| 文件删除 → HasAttachment同步 | ✅ 已集成 | FileCommandHandler.HandleFileDeleteAsync |
| 批量同步历史数据 | ✅ 已集成 | HasAttachmentSyncService.SyncBatchAsync |
| 无HasAttachment字段支持 | ✅ 已支持 | 所有同步方法 |

### 服务调用链

```
客户端上传图片
    ↓
FileUpdateClientService.UpdateBusinessFileAsync()
    ↓
FileUpdateService.UpdateBusinessFileAsync()
    ├─ SaveNewFileAsync()
    ├─ CreateNewRelationAsync()
    └─ HasAttachmentSyncService.SyncOnFileUploadAsync() ✅
        ├─ GetTableNameByBusinessType()
        ├─ IsAnyColumn(tableName, "HasAttachment")
        └─ Update HasAttachment=1
```

### 完整的服务注册

```csharp
// NetworkServicesDependencyInjection.cs

// 文件更新和清理服务
services.AddTransient<FileUpdateService>();        // 图片更新
services.AddTransient<FileCleanupService>();       // 文件清理

// 文件存储监控服务
services.AddSingleton<FileStorageMonitorService>();

// 图片缓存和HasAttachment同步服务
services.AddSingleton<ImageCacheService>();         // 图片缓存
services.AddSingleton<HasAttachmentSyncService>(); // HasAttachment同步
```

## 十、相关文档

1. [单据图片存储优化_实施总结.md](./单据图片存储优化_实施总结.md)
2. [单据图片存储优化_使用指南.md](./单据图片存储优化_使用指南.md)
3. [数据库迁移_单据图片存储优化.sql](./数据库迁移_单据图片存储优化.sql)
4. [SqlSugar_FieldCount错误诊断与修复.md](./SqlSugar_FieldCount错误诊断与修复.md)

---

**文档版本：** v1.0  
**更新日期：** 2025-01-20  
**作者：** RUINORERP开发团队
