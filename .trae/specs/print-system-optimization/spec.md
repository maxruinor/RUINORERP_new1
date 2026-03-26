# 打印系统架构优化规格说明书

## 一、现有架构分析

### 1.1 当前数据模型

| 表名 | 说明 | 关键字段 |
|------|------|----------|
| tb_PrintConfig | 打印配置(按业务类型/BizName) | PrinterName, PrinterSelected, BizType, BizName |
| tb_PrintTemplate | 打印模板(一对多) | TemplateFileStream, IsDefaultTemplate |
| tb_UserPersonalized | 用户个性化设置(全局) | UseUserOwnPrinter, PrinterName, PrinterConfigJson |
| tb_UIMenuPersonalization | 菜单个性化设置 | UIMenuPID, QueryConditionCols等 |

### 1.2 当前打印流程

```
用户点击打印 → PrintHelper.GetPrintConfig() → 根据BizType/BizName获取配置
    ↓
PrintHelper.Print() 
    ↓
优先使用: tb_UserPersonalized.UseUserOwnPrinter ? tb_UserPersonalized.PrinterName 
         : tb_PrintConfig.PrinterName
    ↓
FastReport执行打印
```

### 1.3 Redis缓存架构现状分析

#### 1.4.1 现有缓存组件（已完善）

| 组件 | 位置 | 说明 |
|------|------|------|
| CacheCommandHandler | Server/Network/CommandHandlers | 处理4种缓存命令 |
| CacheClientService | UI/Network/Services | 客户端缓存服务 |
| CacheRequestManager | UI/Network/Services/Cache | 缓存请求管理 |
| IEntityCacheManager | Server/Business/Cache | 缓存管理器 |
| RedisCacheService | Server/Comm | Redis缓存服务(新) |

#### 1.4.2 现有缓存命令（已完善）

| 命令 | 功能 |
|------|------|
| CacheOperation | 缓存操作(Get/Set/Update/Remove/Clear) |
| CacheSync | 缓存同步(服务器推送) |
| CacheSubscription | 缓存订阅管理 |
| CacheMetadataSync | 缓存元数据同步 |

**结论：现有缓存架构已完善，无需新增命令！**

### 1.4 当前缓存问题

| 问题 | 原因 | 影响 |
|------|------|------|
| 缓存穿透风险 | 无空值缓存保护 | 恶意请求击穿数据库 |
| 缓存击穿风险 | 无分布式锁保护 | 热点key并发击穿 |
| 缓存雪崩风险 | 过期时间无随机化 | 批量缓存同时失效 |
| 缓存键设计混乱 | 无统一命名规范 | 键冲突和管理困难 |
| 缓存无监控统计 | 无命中率统计 | 无法优化缓存策略 |
| 缓存一致性差 | 无主动刷新机制 | 数据同步不及时 |

## 二、优化目标

### 2.1 功能目标

1. **支持用户级别的打印机个性化配置**
   - 允许不同用户为相同单据类型指定不同打印机
   - 支持按业务类型(BizName)单独配置打印机

2. **保持与现有多模板体系的兼容性**
   - 打印模板体系不变(tb_PrintTemplate)
   - 实现打印模板与打印机配置的灵活组合

3. **分布式环境适配**
   - 不同局域网用户能正确获取并应用本地打印机配置
   - 支持客户端本地打印机优先策略

4. **提升配置灵活性**
   - 优化个性化设置模块UI
   - 支持服务器端总开关控制缓存和中心化配置

5. **Redis缓存优化**
   - 实现缓存穿透、击穿、雪崩防护 ✅ **已完成**
   - 优化缓存键设计与命名规范 ✅ **已完成**
   - 实现缓存与数据库一致性同步 ✅ **已完成**
   - 添加缓存性能监控与统计 ✅ **已完成**
   - 优化Redis连接池配置 ✅ **已完成**

### 2.2 非功能目标

- 保持向后兼容，不影响现有功能
- 性能:打印配置获取增加Redis缓存机制，响应时间<50ms
- 易用性:优化用户配置界面
- 可靠性:缓存可用性>99.9%

## 三、架构优化方案

### 3.1 数据模型调整（已完成）

在tb_UserPersonalized表中增加JSON字段存储按业务类型区分的打印机配置:

```csharp
// 新增字段
private string _PrinterConfigJson;
// 打印机配置JSON格式: {"SalesOrder":"HP LaserJet","PurchaseOrder":"Canon MF644"}
[JsonField]
public string PrinterConfigJson { get; set; }
```

### 3.2 打印逻辑优化

```csharp
// 优先级策略(从高到低):
// 1. 用户该单据类型的专用打印机(如果配置)
// 2. 用户全局打印机(如果启用UseUserOwnPrinter)
// 3. 系统配置的该单据类型打印机
// 4. 系统默认打印机
// 5. 客户端本地默认打印机(兜底)
```

### 3.3 Redis缓存优化策略（已完成）

#### 3.3.1 缓存穿透防护

```csharp
// 空值缓存保护
public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry)
{
    // 尝试获取缓存
    var cachedValue = await _db.StringGetAsync(key);
    if (!cachedValue.IsNullOrEmpty)
    {
        if (cachedValue == "null")
        {
            // 空值也缓存，但设置较短过期时间
            await _db.StringSetAsync(key, "null", TimeSpan.FromMinutes(5));
        }
        else
        {
            _statistics.RecordHit(key);
            return JsonConvert.DeserializeObject<T>(cachedValue);
        }
    }

    _statistics.RecordMiss();
    var value = await loader();
    var actualExpiry = value == null ? TimeSpan.FromMinutes(5) : expiry;
    await _db.StringSetAsync(key, value == null ? "null" : JsonConvert.SerializeObject(value), actualExpiry);
    return value;
}
```

#### 3.3.2 缓存击穿防护

```csharp
// 分布式锁保护热点key
public async Task<T> GetOrSetWithLockAsync<T>(string key, Func<Task<T>> loader, TimeSpan expiry, TimeSpan lockExpiry)
{
    var cachedValue = await _db.StringGetAsync(key);
    if (!cachedValue.IsNullOrEmpty && cachedValue != "null")
    {
        _statistics.RecordHit(key);
        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    _statistics.RecordMiss();
    var lockKey = $"lock:{key}";
    var lockAcquired = await _db.StringSetAsync(lockKey, "1", lockExpiry, When.NotExists);
    
    if (!lockAcquired)
    {
        await Task.Delay(50);
        cachedValue = await _db.StringGetAsync(key);
        if (!cachedValue.IsNullOrEmpty && cachedValue != "null")
        {
            _statistics.RecordHit(key);
            return JsonConvert.DeserializeObject<T>(cachedValue);
        }
    }
    
    try
    {
        var value = await loader();
        var jitter = TimeSpan.FromTicks((long)(expiry.Ticks * 0.1 * (new Random().NextDouble() * 2 - 1)));
        await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), expiry + jitter);
        return value;
    }
    finally
    {
        await _db.KeyDeleteAsync(lockKey);
    }
}
```

### 3.4 缓存键设计规范（已完成）

| 缓存类型 | 键命名规范 | 示例 |
|----------|------------|------|
| 用户打印机配置 | ruino:erp:user:printer:{userId} | ruino:erp:user:printer:1001 |
| 系统打印配置 | ruino:erp:print:config:{bizType}:{bizName} | ruino:erp:print:config:1:SaleOrder |
| 打印模板 | ruino:erp:print:template:{configId} | ruino:erp:print:template:1001 |
| 菜单个性化 | ruino:erp:menu:personal:{menuId}:{userId} | ruino:erp:menu:personal:1001:1001 |

### 3.5 分布式适配

**无需新增命令！利用现有架构：**

```
客户端 → CacheClientService.RequestCacheAsync("tb_UserPersonalized") 
         ↓
服务器 → CacheCommandHandler.HandleCacheOperationAsync()
         ↓
服务器 → RedisCacheService.GetOrSetAsync() / PrinterConfigService
         ↓
返回缓存数据 → 客户端本地缓存
```

## 四、接口设计（已完成）

### 4.1 新增服务接口

```csharp
public interface IPrinterConfigService {
    Task<UserPrinterConfigDto> GetUserPrinterConfigAsync(long userId);
    Task<bool> SaveUserPrinterConfigAsync(UserPrinterConfigDto config);
    Task<string> GetPrinterForBizTypeAsync(long userId, string bizName, string defaultPrinter);
    Task<bool> SetPrinterForBizTypeAsync(long userId, string bizName, string printerName);
    Task<tb_PrintConfig> GetPrintConfigAsync(int bizType, string bizName);
    Task<bool> SyncConfigToClientAsync(long userId);
}
```

### 4.2 数据传输对象

```csharp
public class UserPrinterConfigDto {
    public long UserId { get; set; }
    public bool UseUserOwnPrinter { get; set; }
    public string GlobalPrinterName { get; set; }
    public Dictionary<string, string> BizTypePrinters { get; set; }
    public DateTime LastSyncTime { get; set; }
}
```

## 五、兼容性处理

### 5.1 向后兼容策略

| 场景 | 处理方式 |
|------|----------|
| 旧数据库(无新字段) | 自动添加字段，默认值兼容 |
| 现有打印逻辑 | 保持不变，新增逻辑作为增强 |
| 无Redis环境 | 自动降级到内存缓存 |

### 5.2 数据迁移

```sql
ALTER TABLE tb_UserPersonalized 
ADD PrinterConfigJson NVARCHAR(MAX) NULL;
```

## 六、实施计划

### 阶段1: 数据模型调整 ✅ 已完成
- [x] 修改tb_UserPersonalized实体，添加PrinterConfigJson字段
- [x] 编写数据迁移脚本
- [x] 创建UserPrinterConfigDto数据传输对象

### 阶段2: Redis缓存基础设施 ✅ 已完成
- [x] 创建IRedisCacheService接口与实现
- [x] 实现缓存穿透防护(空值缓存)
- [x] 实现缓存击穿防护(分布式锁)
- [x] 实现缓存雪崩防护(过期时间随机化)
- [x] 创建ICacheKeyGenerator缓存键生成器
- [x] 实现缓存统计与监控服务

### 阶段3: 服务层实现 ✅ 已完成
- [x] 创建IPrinterConfigService
- [x] 实现Redis缓存逻辑
- [x] 配置Redis连接池

### 阶段4: 打印逻辑改造 ✅ 已完成
- [x] 修改PrintHelper优先级逻辑
- [x] 添加本地打印机降级策略
- [x] 兼容性测试

### 阶段5: UI层优化 ✅ 已完成
- [x] 修改UCUserPersonalizedEdit
- [x] 添加按业务类型配置打印机界面
- [x] 打印配置界面优化

## 七、风险与缓解

| 风险 | 缓解措施 |
|------|----------|
| JSON字段查询性能 | 合理缓存 |
| 分布式同步失败 | 本地降级机制 |
| 打印机名称不匹配 | 启动时校验，自动降级 |
| Redis连接失败 | 降级到内存缓存 |
| 缓存穿透攻击 | 空值缓存+限流 |
| 缓存雪崩 | 过期时间随机化 |
