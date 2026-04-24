# RUINORERP Server 内存泄漏深度分析与优化报告

## 1. 监控数据准确性评估

经过对 `MemoryDistributionService.cs` 和 `MemoryLeakDiagnosticsService.cs` 的代码审查，确认当前监控体系存在严重的**估算偏差**问题：

### 1.1 估算逻辑泛滥
- **会话管理 (SessionService)**: 采用硬编码系数 `sessionCount * 1500 * 1024`（每会话 1.5MB）进行估算。该数值无法反映实际业务中会话对象（如 `DataQueue` 积压消息、用户权限列表、临时数据集）的真实大小。
- **缓存统计 (EntityCacheManager)**: 依赖 `EstimateObjectSize` 方法，通过采样前 5 个对象并乘以总数来估算。对于包含大量字符串或复杂嵌套结构的实体，这种线性估算误差极大。
- **数据库连接池**: 直接硬编码为 `80MB`，完全脱离了 SqlSugar 实际连接数和命令缓存的动态变化。
- **其他模块**: 规则引擎、事件处理器、委托缓存等均为固定值估算（如 "至少 50MB"）。

### 1.2 结论
当前的监控数据仅能作为**趋势参考**，无法用于精确定位具体的泄漏对象。监控显示“正常”并不代表内存没有泄漏，因为估算模型可能低估了某些高频增长的对象。

---

## 2. 内存泄漏根因分析 (Root Cause Analysis)

结合代码逻辑与 50人/6GB 的现象，锁定以下四大泄漏嫌疑点：

### 2.1 会话队列积压 (Session DataQueue) - **高危**
- **位置**: `SessionService.cs` 中的 `ConcurrentDictionary<string, SessionInfo>`。
- **现象**: `SessionInfo` 内部维护了一个 `DataQueue`。如果客户端网络波动或处理速度慢于服务器推送速度，队列会无限增长。
- **证据**: 监控代码中虽然统计了 `totalQueuedMessages`，但估算时仅按 `count * 1024` 计算，忽略了大型数据包（如报表结果、图片流）在队列中占用的真实内存。

### 2.2 实体缓存无限膨胀 (EntityCacheManager) - **高危**
- **位置**: `EntityCacheManager.cs`。
- **现象**: 
    1. 缓存过期时间默认 30 分钟，但在高并发下，LRU 清理机制（`CleanCacheByLeastRecentlyUsed`）仅在达到 150MB 阈值时触发。
    2. **类型转换开销**: 代码中存在大量的 `JArray` 到 `List<T>` 以及 `ExpandoObject` 的序列化/反序列化操作。这些中间对象会产生大量 LOH（大对象堆）碎片。
    3. **空表缓存**: 即使查询结果为空，也会存入缓存，导致缓存项数量虚高。

### 2.3 性能数据存储未彻底释放 (PerformanceDataStorageService) - **中危**
- **位置**: `PerformanceDataStorageService.cs`。
- **现象**: 虽然设置了 `MaxRecordsPerClient = 10000`，但如果客户端频繁上报且服务端清理定时器（5分钟一次）滞后，内存中会堆积大量 `PerformanceMetricBase` 对象。每个对象包含多个属性，50个客户端 * 10000条记录将产生巨大的内存压力。

### 2.4 静态集合与事件订阅泄漏
- **位置**: `BlacklistManager.cs`, `ProductSKUCodeGenerator.cs`。
- **现象**: 
    1. `BlacklistManager` 使用静态 `BindingList` 和 `ConcurrentDictionary`，如果封禁 IP 逻辑频繁触发且不清理，会导致内存缓慢增长。
    2. `ProductSKUCodeGenerator` 中的 `_skuCache` 和 `_valueCodeMap` 为静态字典，随着 SKU 种类的增加而只增不减。

---

## 3. 详细优化建议与解决方案

### 3.1 会话管理优化
1. **实施队列上限**: 在 `SessionInfo` 中为 `DataQueue` 设置最大长度（如 500 条）。超过上限时，丢弃最旧的非关键消息或断开连接。
2. **精确监控**: 移除 `MemoryDistributionService` 中的 1.5MB 估算，改为遍历 `DataQueue` 累加 `PacketModel` 的实际序列化后字节数。

### 3.2 缓存策略重构
1. **引入容量限制**: 将 `EntityCacheManager` 的最大缓存大小从 150MB 进一步下调至 100MB，并提高清理频率。
2. **避免 LOH 碎片**: 
   - 减少 `JArray` 和 `ExpandoObject` 的使用，尽量在数据库层完成类型映射。
   - 对于大于 85KB 的查询结果，禁止放入本地内存缓存，建议直接使用 Redis 或按需查询。
3. **强制压缩**: 在 `MemoryMonitoringService` 中，当检测到 LOH 占比过高时，执行 `GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized, blocking: true, compacting: true)`。

### 3.3 资源释放与生命周期管理
1. **完善 Dispose**: 检查所有实现 `IDisposable` 的对象（如 `Bitmap`, `Stream`, `Database Connection`），确保在 `finally` 块或 `using` 语句中释放。
2. **静态集合清理**: 为 `BlacklistManager` 和 `ProductSKUCodeGenerator` 增加定期清理任务，移除超过 24 小时未访问的条目。

### 3.4 序列化/反序列化优化
1. **统一序列化器**: 项目中混用了 `Newtonsoft.Json` 和可能的其他序列化方式。建议统一使用 `System.Text.Json`（性能更高，内存分配更少）或优化 `Newtonsoft.Json` 的 `JsonSerializerSettings`（如禁用 `TypeNameHandling`）。
2. **减少反射**: `MemoryDistributionService` 中大量的反射调用不仅慢，还会产生临时的 `MethodInfo` 对象。应使用 `System.Reflection.Emit` 或表达式树缓存委托。

---

## 4. 实施路线图

1. **第一阶段（紧急）**: 
   - 修复 `SessionService` 的 `DataQueue` 无限增长问题。
   - 调整 `EntityCacheManager` 的清理阈值为更积极的水平。
2. **第二阶段（监控增强）**: 
   - 重写 `MemoryDistributionService`，引入 `dotnet-counters` 或 `PerfView` 集成，获取真实的 GC Heap 分布。
3. **第三阶段（架构优化）**: 
   - 迁移大对象存储至 Redis。
   - 审查并清理所有静态集合。

---

## 5. 总结

RUINORERP.Server 的内存泄漏并非由单一原因引起，而是**估算失真的监控掩盖了会话队列积压和缓存策略宽松**的综合结果。建议立即着手优化会话队列管理和缓存淘汰机制，并引入更精确的内存诊断工具（如 dotnet-dump）进行周期性快照分析。
