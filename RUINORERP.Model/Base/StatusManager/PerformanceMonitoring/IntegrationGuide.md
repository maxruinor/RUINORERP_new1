# 性能监控体系集成指南

## 概述

本文档描述如何将性能监控体系集成到 RuinorERP 系统中。

## 已创建的核心组件

### 1. 性能监控开关 (PerformanceMonitorSwitch)
- **位置**: `RUINORERP.Model\Base\StatusManager\PerformanceMonitoring\PerformanceMonitorSwitch.cs`
- **功能**: 类似日志系统的总开关机制，控制整个性能监控体系的启用/禁用
- **配置文件**: `config/performance_monitor.json`

### 2. 性能数据模型 (PerformanceDataModels)
- **位置**: `RUINORERP.Model\Base\StatusManager\PerformanceMonitoring\PerformanceDataModels.cs`
- **功能**: 定义所有性能指标的数据结构
  - MethodExecutionMetric - 方法执行性能
  - DatabasePerformanceMetric - 数据库性能
  - NetworkPerformanceMetric - 网络性能
  - MemoryPerformanceMetric - 内存使用
  - CachePerformanceMetric - 缓存性能
  - UIResponseMetric - UI响应性能
  - TransactionMetric - 事务性能
  - DeadlockMetric - 死锁检测

### 3. 性能监控管理器 (PerformanceMonitorManager)
- **位置**: `RUINORERP.Model\Base\StatusManager\PerformanceMonitoring\PerformanceMonitorManager.cs`
- **功能**: 统一的性能监控入口，管理所有监控数据的收集和上报

### 4. SuperSocket 命令定义 (PerformanceMonitoringCommands)
- **位置**: `RUINORERP.PacketSpec\Commands\CommandDefinitions\PerformanceMonitoringCommands.cs`
- **功能**: 定义性能监控相关的请求/响应命令

### 5. 服务器端数据存储服务 (PerformanceDataStorageService)
- **位置**: `RUINORERP.Server\Network\Services\PerformanceDataStorageService.cs`
- **功能**: 在服务器内存中存储和管理客户端上报的性能监控数据

### 6. 服务器端监控UI控件 (PerformanceMonitorControl)
- **位置**: `RUINORERP.Server\Controls\PerformanceMonitorControl.cs`
- **功能**: 服务器端用于显示和管理性能监控数据的UI控件

## 集成步骤

### 步骤1: 初始化性能监控开关

在应用程序启动时初始化性能监控开关：

```csharp
// 在 Program.cs 或 Startup.cs 中
RUINORERP.Model.Base.StatusManager.PerformanceMonitoring.PerformanceMonitorSwitch.Initialize();
```

### 步骤2: 在客户端集成性能监控

#### 2.1 创建性能监控管理器实例

```csharp
// 在客户端服务中
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;

public class ClientPerformanceService
{
    private readonly PerformanceMonitorManager _monitorManager;

    public ClientPerformanceService(ILogger<PerformanceMonitorManager> logger)
    {
        _monitorManager = new PerformanceMonitorManager(logger);
        _monitorManager.OnDataUpload += OnPerformanceDataUpload;
    }

    private void OnPerformanceDataUpload(object sender, PerformanceDataPacket packet)
    {
        // 通过SuperSocket上报数据到服务器
        UploadPerformanceDataToServer(packet);
    }
}
```

#### 2.2 在关键位置添加性能监控代码

**方法执行监控**:
```csharp
// 使用方式1: 手动记录
var stopwatch = Stopwatch.StartNew();
try
{
    // 执行业务逻辑
    DoSomething();
    
    PerformanceMonitorManager.RecordMethodExecution(
        "DoSomething", 
        "MyClass", 
        stopwatch.ElapsedMilliseconds, 
        true);
}
catch (Exception ex)
{
    PerformanceMonitorManager.RecordMethodExecution(
        "DoSomething", 
        "MyClass", 
        stopwatch.ElapsedMilliseconds, 
        false, 
        ex.Message);
}
```

**数据库操作监控**:
```csharp
// 在 SqlSugar 拦截器或 Repository 层
public override void OnLogExecuting(string sql, SugarParameter[] parameters)
{
    _sqlStopwatch = Stopwatch.StartNew();
    _currentSql = sql;
}

public override void OnLogExecuted(string sql, SugarParameter[] parameters)
{
    var executionTime = _sqlStopwatch.ElapsedMilliseconds;
    PerformanceMonitorManager.RecordDatabaseOperation(
        sql, 
        "Query", 
        "TableName", 
        executionTime, 
        parameters?.Length ?? 0, 
        true);
}
```

**网络请求监控**:
```csharp
// 在 SuperSocket 客户端发送/接收处
public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request)
{
    var requestId = Guid.NewGuid().ToString("N");
    var startTime = DateTime.Now;
    
    try
    {
        var response = await base.SendRequestAsync<TRequest, TResponse>(request);
        
        PerformanceMonitorManager.RecordNetworkRequest(
            requestId,
            typeof(TRequest).Name,
            startTime,
            DateTime.Now,
            requestSize,
            responseSize,
            true);
            
        return response;
    }
    catch (Exception ex)
    {
        PerformanceMonitorManager.RecordNetworkRequest(
            requestId,
            typeof(TRequest).Name,
            startTime,
            DateTime.Now,
            requestSize,
            0,
            false,
            ex.Message);
        throw;
    }
}
```

### 步骤3: 在服务器端集成数据接收

#### 3.1 注册服务

```csharp
// 在 Startup.cs 或 Program.cs 中
services.AddSingleton<PerformanceDataStorageService>();
```

#### 3.2 创建命令处理器

```csharp
// 创建 PerformanceDataUploadHandler
public class PerformanceDataUploadHandler : ICommandHandler<PerformanceDataUploadRequest, PerformanceDataUploadResponse>
{
    private readonly PerformanceDataStorageService _storageService;

    public PerformanceDataUploadHandler(PerformanceDataStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<PerformanceDataUploadResponse> HandleAsync(
        PerformanceDataUploadRequest request, 
        CancellationToken cancellationToken)
    {
        return _storageService.StorePerformanceData(request);
    }
}
```

#### 3.3 在服务器窗体中添加监控控件

```csharp
// 在服务器主窗体中
var performanceMonitor = new PerformanceMonitorControl();
performanceMonitor.SetStorageService(_performanceDataStorageService);
this.Controls.Add(performanceMonitor);
```

### 步骤4: 启用/禁用性能监控

#### 4.1 通过代码控制

```csharp
// 启用性能监控
PerformanceMonitorSwitch.Enable();

// 禁用性能监控
PerformanceMonitorSwitch.Disable();

// 切换状态
bool isEnabled = PerformanceMonitorSwitch.Toggle();

// 启用特定监控项
PerformanceMonitorSwitch.EnableMonitor(PerformanceMonitorType.Database);
PerformanceMonitorSwitch.EnableMonitor(PerformanceMonitorType.Network);
```

#### 4.2 通过配置文件控制

编辑 `config/performance_monitor.json`:

```json
{
  "IsEnabled": true,
  "EnabledMonitors": [1, 2, 3, 4],
  "UploadIntervalSeconds": 30,
  "MemoryRetentionMinutes": 60,
  "MethodExecutionThresholdMs": 1000,
  "DatabaseQueryThresholdMs": 5000,
  "NetworkRequestThresholdMs": 3000,
  "MemoryWarningThresholdMB": 1024,
  "MemoryCriticalThresholdMB": 2048
}
```

### 步骤5: 通过SuperSocket远程控制

#### 5.1 客户端上报性能数据

```csharp
// 客户端定期上报
var request = new PerformanceDataUploadRequest
{
    ClientId = clientId,
    PerformanceDataJson = JsonConvert.SerializeObject(packet),
    PacketSizeBytes = packetSize,
    MetricCount = metricCount,
    MachineName = Environment.MachineName,
    ClientIpAddress = GetClientIpAddress()
};

var response = await client.SendRequestAsync<PerformanceDataUploadRequest, PerformanceDataUploadResponse>(request);
```

#### 5.2 服务器查询性能数据

```csharp
// 管理员查询
var request = new PerformanceDataQueryRequest
{
    TargetClientId = "Client_123",
    StartTime = DateTime.Now.AddHours(-1),
    EndTime = DateTime.Now,
    MetricTypes = new List<int> { 1, 2 }, // Database, Network
    MaxRecords = 1000
};

var response = await server.SendRequestAsync<PerformanceDataQueryRequest, PerformanceDataQueryResponse>(request);
