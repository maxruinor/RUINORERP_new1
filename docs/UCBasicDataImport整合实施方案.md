# UCBasicDataImport 整合实施方案

**创建日期**: 2026-04-18  
**目标**: 保留UCBasicDataImport的优秀UI体验，将业务逻辑迁移到Business层

---

## 📊 当前架构分析

### UCBasicDataImport依赖的业务逻辑类（UI层）

| 类名 | 文件 | 行数 | 职责 | 迁移优先级 |
|------|------|------|------|-----------|
| **DynamicImporter** | DynamicImporter.cs | 1069行 | 核心导入引擎 | P0 - 必须迁移 |
| **ForeignKeyService** | ForeignKeyService.cs | 383行 | 外键缓存服务 | P0 - 必须迁移 |
| **DataDeduplicationService** | DataDeduplicationService.cs | 320行 | 数据去重服务 | P0 - 必须迁移 |
| **DynamicExcelParser** | DynamicExcelParser.cs | 762行 | Excel解析+图片提取 | P0 - 必须迁移 |
| **DynamicDataValidator** | DynamicDataValidator.cs | ~600行 | 数据验证服务 | P1 - 建议迁移 |
| **ColumnMappingManager** | ColumnMappingManager.cs | 178行 | 映射配置管理 | P1 - 建议迁移 |
| **ImageProcessor** | ImageProcessor.cs | ~250行 | 图片处理 | P2 - 可选 |
| **ExcelDataParser** | ExcelDataParser.cs | ~180行 | 基础Excel解析 | P2 - 可选 |

### 纯UI组件（保留不变）

| 组件 | 文件 | 说明 |
|------|------|------|
| **UCBasicDataImport** | UCBasicDataImport.cs (1763行) | 主窗体，保留所有UI逻辑 |
| **frmColumnMappingConfig** | frmColumnMappingConfig.cs | 列映射配置界面 |
| **FrmColumnPropertyConfig** | FrmColumnPropertyConfig.cs | 列属性配置界面 |
| **其他Form** | Frm*.cs | 各种配置对话框 |

---

## 🎯 整合目标架构

```
┌──────────────────────────────────────────┐
│         UI Layer (WinForms)              │
│                                          │
│  UCBasicDataImport (1763行)             │
│    ├─ 双表格预览（原始+解析）            │
│    ├─ 图形化映射配置                     │
│    ├─ 勾选导入机制                       │
│    ├─ 图片实时预览                       │
│    └─ 调用 Business 层服务               │
│                                          │
│  frmColumnMappingConfig (UI only)       │
│  FrmColumnPropertyConfig (UI only)      │
│  ...其他配置对话框...                    │
└──────────────┬───────────────────────────┘
               │ 依赖注入
               ▼
┌──────────────────────────────────────────┐
│     Business Layer (ImportEngine)        │
│                                          │
│  SmartImportEngine (主引擎)              │
│    ├─ ExecuteAsync()                     │
│    ├─ ExecuteWithDataTableAsync()        │
│    └─ PreviewDataAsync()                 │
│                                          │
│  Services/                               │
│    ├─ ColumnMappingService               │
│    ├─ DatabaseWriterService              │
│    ├─ ForeignKeyCacheService ← 从第三套迁移  │
│    ├─ ImageExtractionService ← 从第三套迁移  │
│    ├─ DataDeduplicationService ← 从第三套迁移│
│    └─ DataValidationService ← 从第三套迁移   │
│                                          │
│  Models/                                 │
│    ├─ ImportProfile (扩展支持7种数据源)   │
│    ├─ ColumnMapping                      │
│    └─ ImportReport                       │
│                                          │
│  IdRemappingEngine (从第一套保留)         │
└──────────────┬───────────────────────────┘
               │ SqlSugar ORM
               ▼
┌──────────────────────────────────────────┐
│         Database Layer                   │
└──────────────────────────────────────────┘
```

---

## 📋 详细实施步骤

### 阶段1：创建接口抽象（1-2天）

#### 任务1.1：定义Business层接口

**文件**: `RUINORERP.Business/ImportEngine/Interfaces/`

```csharp
// IImportEngine.cs
public interface IImportEngine
{
    Task<ImportReport> ExecuteAsync(string filePath, ImportProfile profile);
    Task<ImportReport> ExecuteWithDataTableAsync(DataTable data, ImportProfile profile);
    Task<DataTable> PreviewDataAsync(string filePath, int maxRows = 100);
}

// IForeignKeyCacheService.cs
public interface IForeignKeyCacheService
{
    void PreloadForeignKeyData(IEnumerable<ColumnMapping> mappings);
    object GetForeignKeyValue(string tableName, string fieldValue, out string errorMessage);
    void ClearCache();
}

// IImageExtractionService.cs
public interface IImageExtractionService
{
    Task<List<ImageInfo>> ExtractImagesAsync(string excelFilePath, int sheetIndex);
    Task<string> SaveImageAsync(byte[] imageData, string fileName);
}

// IDataDeduplicationService.cs
public interface IDataDeduplicationService
{
    DeduplicationResult Deduplicate(DataTable dataTable, ImportProfile profile);
}

// IDataValidationService.cs
public interface IDataValidationService
{
    List<ValidationError> Validate(DataTable data, ImportProfile profile);
}
```

#### 任务1.2：在Startup.cs注册服务

```csharp
// RUINORERP.Business/Startup.cs
public static class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // 导入引擎服务
        services.AddSingleton<IImportEngine, SmartImportEngine>();
        services.AddSingleton<IForeignKeyCacheService, ForeignKeyCacheService>();
        services.AddSingleton<IImageExtractionService, ImageExtractionService>();
        services.AddSingleton<IDataDeduplicationService, DataDeduplicationService>();
        services.AddSingleton<IDataValidationService, DataValidationService>();
    }
}
```

---

### 阶段2：迁移核心服务（3-5天）

#### 任务2.1：迁移ForeignKeyService → ForeignKeyCacheService

**源文件**: `RUINORERP.UI/SysConfig/BasicDataImport/ForeignKeyService.cs`  
**目标文件**: `RUINORERP.Business/ImportEngine/Services/ForeignKeyCacheService.cs`

**改造要点**:
1. 命名空间改为 `RUINORERP.Business.ImportEngine.Services`
2. 实现 `IForeignKeyCacheService` 接口
3. 保持ConcurrentDictionary缓存机制不变
4. 添加日志记录（使用项目的日志框架）

**代码量**: ~383行，改造工作量小

---

#### 任务2.2：迁移DataDeduplicationService

**源文件**: `RUINORERP.UI/SysConfig/BasicDataImport/DataDeduplicationService.cs`  
**目标文件**: `RUINORERP.Business/ImportEngine/Services/DataDeduplicationService.cs`

**改造要点**:
1. 命名空间改为 `RUINORERP.Business.ImportEngine.Services`
2. 实现 `IDataDeduplicationService` 接口
3. 修改方法签名，接受 `ImportProfile` 而非 `ImportConfiguration`
4. 保持去重算法不变

**代码量**: ~320行，改造工作量小

---

#### 任务2.3：迁移图片提取功能

**源文件**: `RUINORERP.UI/SysConfig/BasicDataImport/DynamicExcelParser.cs` (图片提取部分)  
**目标文件**: `RUINORERP.Business/ImportEngine/Services/ImageExtractionService.cs`

**改造要点**:
1. 从DynamicExcelParser中提取图片相关方法：
   - `ExtractAllImages()`
   - `HandleFormulaCell()` (DISPIMG公式识别)
   - `SaveImage()`
2. 实现 `IImageExtractionService` 接口
3. 使用NPOI库保持不变
4. 图片保存路径可配置

**代码量**: ~400行（仅图片相关部分）

---

#### 任务2.4：迁移数据验证服务

**源文件**: `RUINORERP.UI/SysConfig/BasicDataImport/DynamicDataValidator.cs`  
**目标文件**: `RUINORERP.Business/ImportEngine/Services/DataValidationService.cs`

**改造要点**:
1. 命名空间改为 `RUINORERP.Business.ImportEngine.Services`
2. 实现 `IDataValidationService` 接口
3. 依赖 `IForeignKeyCacheService` 进行外键验证
4. 返回 `List<ValidationError>` 而非直接显示MessageBox

**代码量**: ~600行，改造工作量中等

---

### 阶段3：扩展SmartImportEngine（2-3天）

#### 任务3.1：集成新服务

**文件**: `RUINORERP.Business/ImportEngine/SmartImportEngine.cs`

```csharp
public class SmartImportEngine : IImportEngine
{
    private readonly IForeignKeyCacheService _foreignKeyCache;
    private readonly IImageExtractionService _imageExtractor;
    private readonly IDataDeduplicationService _deduplication;
    private readonly IDataValidationService _validator;
    
    public SmartImportEngine(
        ISqlSugarClient db,
        IForeignKeyCacheService foreignKeyCache,
        IImageExtractionService imageExtractor,
        IDataDeduplicationService deduplication,
        IDataValidationService validator)
    {
        _db = db;
        _foreignKeyCache = foreignKeyCache;
        _imageExtractor = imageExtractor;
        _deduplication = deduplication;
        _validator = validator;
    }
    
    public async Task<ImportReport> ExecuteWithDataTableAsync(
        DataTable data, 
        ImportProfile profile)
    {
        var report = new ImportReport();
        
        try
        {
            // 1. 预加载外键数据
            _foreignKeyCache.PreloadForeignKeyData(profile.ColumnMappings);
            
            // 2. 数据去重（如果配置了）
            if (profile.EnableDeduplication)
            {
                var dedupResult = _deduplication.Deduplicate(data, profile);
                data = dedupResult.DeduplicatedData;
                report.DuplicateCount = dedupResult.DuplicateCount;
            }
            
            // 3. 数据验证
            var validationErrors = _validator.Validate(data, profile);
            if (validationErrors.Any())
            {
                report.ValidationErrors = validationErrors;
                // 根据配置决定是否继续
            }
            
            // 4. 提取图片（如果有图片字段）
            if (profile.HasImageFields && !string.IsNullOrEmpty(profile.ExcelFilePath))
            {
                var images = await _imageExtractor.ExtractImagesAsync(
                    profile.ExcelFilePath, 
                    profile.SheetIndex);
                report.ImageCount = images.Count;
            }
            
            // 5. 映射数据
            var mappedData = _mappingService.MapData(data, profile);
            
            // 6. 写入数据库
            report.SuccessRows = await _dbWriter.BatchUpsertAsync(
                mappedData, profile, _remapper);
                
            report.IsSuccess = true;
        }
        catch (Exception ex)
        {
            report.IsSuccess = false;
            report.Message = ex.Message;
        }
        
        return report;
    }
}
```

---

#### 任务3.2：扩展ImportProfile模型

**文件**: `RUINORERP.Business/ImportEngine/Models/ImportProfile.cs`

新增字段：
```csharp
public class ImportProfile
{
    // 现有字段...
    
    // === 从第三套迁移的字段 ===
    
    /// <summary>
    /// 是否启用去重
    /// </summary>
    public bool EnableDeduplication { get; set; }
    
    /// <summary>
    /// 去重字段列表
    /// </summary>
    public List<string> DeduplicationFields { get; set; } = new List<string>();
    
    /// <summary>
    /// 去重策略（FirstOccurrence / LastOccurrence）
    /// </summary>
    public string DeduplicationStrategy { get; set; } = "FirstOccurrence";
    
    /// <summary>
    /// 是否有图片字段
    /// </summary>
    public bool HasImageFields => ColumnMappings.Any(m => m.IsImageColumn);
    
    /// <summary>
    /// Excel文件路径（用于图片提取）
    /// </summary>
    public string ExcelFilePath { get; set; }
    
    /// <summary>
    /// Sheet索引
    /// </summary>
    public int SheetIndex { get; set; } = 0;
}
```

---

### 阶段4：改造UCBasicDataImport（2-3天）

#### 任务4.1：添加依赖注入

**文件**: `RUINORERP.UI/SysConfig/BasicDataImport/UCBasicDataImport.cs`

```csharp
public partial class UCBasicDataImport : UserControl
{
    // ✅ 新增：通过依赖注入获取Business层服务
    private readonly IImportEngine _importEngine;
    private readonly IForeignKeyCacheService _foreignKeyCache;
    
    // ❌ 删除：不再直接在UI层创建这些对象
    // private DynamicImporter _dynamicImporter;
    // private ForeignKeyService _foreignKeyService;
    // private DataDeduplicationService _deduplicationService;
    
    public UCBasicDataImport(IImportEngine importEngine, IForeignKeyCacheService foreignKeyCache)
    {
        InitializeComponent();
        _importEngine = importEngine;
        _foreignKeyCache = foreignKeyCache;
        
        InitializeUI();
    }
}
```

#### 任务4.2：改造导入方法

**改造前**（第1061-1100行）:
```csharp
private async Task ExecuteDynamicImport()
{
    _dynamicImporter = new DynamicImporter(_db, _entityInfoService);
    await ExecuteSingleImport();
}

private async Task ExecuteSingleImport()
{
    // ... 大量业务逻辑
    var importResult = await _dynamicImporter.ImportAsync(importData, mappings, _selectedEntityType);
    DisplayImportResult(importResult);
}
```

**改造后**:
```csharp
private async Task ExecuteDynamicImport()
{
    try
    {
        // 1. 构建ImportProfile
        var profile = BuildImportProfile();
        
        // 2. 预加载外键数据
        _foreignKeyCache.PreloadForeignKeyData(profile.ColumnMappings);
        
        // 3. 调用Business层引擎
        var report = await _importEngine.ExecuteWithDataTableAsync(
            _parsedImportData, profile);
        
        // 4. 显示结果（UI层职责）
        DisplayImportResult(report);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"导入失败: {ex.Message}", "错误", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private ImportProfile BuildImportProfile()
{
    return new ImportProfile
    {
        TargetTable = _selectedEntityType.Name,
        ColumnMappings = _currentConfig.ColumnMappings,
        EnableDeduplication = _currentConfig.EnableDeduplication,
        DeduplicationFields = _currentConfig.DeduplicationFields,
        ExcelFilePath = ktxtDynamicFilePath.Text,
        SheetIndex = kcmbDynamicSheetName.SelectedIndex
    };
}
```

#### 任务4.3：保留所有UI特性

**不做任何修改的部分**:
- ✅ 双表格预览（dgvRawExcelData + dgvParsedImportData）
- ✅ 单元格格式化事件（DgvRawExcelData_CellFormatting等）
- ✅ 勾选导入机制（GetSelectedRows方法）
- ✅ 映射配置界面（frmColumnMappingConfig）
- ✅ 唯一性检查交互（CheckUniqueValues方法）
- ✅ 所有按钮事件和状态管理

---

### 阶段5：清理旧代码（1天）

#### 任务5.1：删除FrmDataMigrationCenter

```bash
# 删除文件
RUINORERP.UI/SysConfig/DataMigration/FrmDataMigrationCenter.*
RUINORERP.UI/SysConfig/DataMigration/*.cs (除配置文件外)
```

#### 任务5.2：删除第三套的业务逻辑类

```bash
# 删除以下文件（已迁移到Business层）
RUINORERP.UI/SysConfig/BasicDataImport/DynamicImporter.cs
RUINORERP.UI/SysConfig/BasicDataImport/ForeignKeyService.cs
RUINORERP.UI/SysConfig/BasicDataImport/DataDeduplicationService.cs
RUINORERP.UI/SysConfig/BasicDataImport/DynamicDataValidator.cs
RUINORERP.UI/SysConfig/BasicDataImport/DynamicExcelParser.cs (仅删除业务逻辑部分)

# 保留以下文件（UI层需要）
RUINORERP.UI/SysConfig/BasicDataImport/UCBasicDataImport.*
RUINORERP.UI/SysConfig/BasicDataImport/frmColumnMappingConfig.*
RUINORERP.UI/SysConfig/BasicDataImport/FrmColumnPropertyConfig.*
RUINORERP.UI/SysConfig/BasicDataImport/ColumnMapping.cs (模型类)
RUINORERP.UI/SysConfig/BasicDataImport/ImportConfiguration.cs (配置类)
```

---

## ⚠️ 风险评估与缓解

### 风险1：依赖注入配置复杂

**风险**: WinForms项目可能没有完善的DI容器配置  
**缓解**: 
- 使用现有的Startup.cs配置
- 如果不存在，手动创建简单的ServiceLocator
- 参考项目中其他模块的DI实践

### 风险2：UCBasicDataImport耦合度高

**风险**: 1763行的代码可能存在大量耦合  
**缓解**:
- 采用渐进式重构，先包装再剥离
- 保持UI行为不变，只替换底层调用
- 充分测试每个改动点

### 风险3：图片提取逻辑复杂

**风险**: DISPIMG公式识别和锚点定位可能有问题  
**缓解**:
- 完整复制DynamicExcelParser的图片提取代码
- 编写单元测试验证各种Excel格式
- 保留原有的图片预览功能

### 风险4：性能回退

**风险**: 分层后可能增加调用开销  
**缓解**:
- 保持外键缓存机制不变
- 使用异步调用避免阻塞UI
- 性能测试对比前后差异

---

## 📊 工作量估算

| 阶段 | 任务 | 工作量 | 风险等级 |
|------|------|--------|---------|
| 阶段1 | 创建接口抽象 | 1-2天 | 低 |
| 阶段2 | 迁移核心服务 | 3-5天 | 中 |
| 阶段3 | 扩展SmartImportEngine | 2-3天 | 中 |
| 阶段4 | 改造UCBasicDataImport | 2-3天 | 高 |
| 阶段5 | 清理旧代码 | 1天 | 低 |
| **总计** | | **9-14天** | |

---

## ✅ 验收标准

### 功能完整性
- [ ] 双表格预览正常工作
- [ ] 图形化映射配置可用
- [ ] 7种数据源类型全部支持
- [ ] 勾选导入机制正常
- [ ] 图片实时预览正常
- [ ] 外键缓存生效
- [ ] 数据去重功能正常
- [ ] 数据验证交互正常

### 架构合规性
- [ ] UI层无业务逻辑（除了UI交互）
- [ ] Business层可通过单元测试
- [ ] 依赖注入正常工作
- [ ] 无循环依赖

### 性能指标
- [ ] 导入1000行数据耗时 < 5秒
- [ ] 外键查询命中缓存率 > 90%
- [ ] UI响应流畅，无卡顿

---

## 📝 下一步行动

**立即执行**:
1. ✅ 创建接口定义文件
2. ✅ 在Startup.cs注册服务
3. ✅ 开始迁移ForeignKeyService

**预计完成时间**: 2026-05-02（14天后）

---

**文档版本**: v1.0  
**最后更新**: 2026-04-18  
**负责人**: Lingma AI助手
