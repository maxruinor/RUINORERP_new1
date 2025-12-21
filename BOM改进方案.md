# ERP系统成本计算和BOM管理改进方案

## 一、当前系统现状分析

### 1. 数据结构现状

- **BOM主表(tb_BOM_S)**：
  - 包含基本配方信息(BOM_ID, BOM_No, BOM_Name等)
  - 已有`BOM_S_VERID`关联到版本历史表
  - 使用`is_enabled`和`is_available`字段管理状态
  - `DataStatus`字段用于数据状态管理
  - 成本相关字段：TotalMaterialCost, OutProductionAllCosts, SelfProductionAllCosts等
  - `Effective_at`字段记录生效时间

- **BOM详情表(tb_BOM_SDetail)**：
  - 记录子物料信息(ProdDetailID, UsedQty, UnitCost等)
  - 成本字段：UnitCost, SubtotalUnitCost
  - 缺少对子物料BOM状态的校验逻辑

- **BOM配置历史表(tb_BOMConfigHistory)**：
  - 主键`BOM_S_VERID`
  - `VerNo`字段记录版本号
  - `Effective_at`字段记录版本生效时间
  - 缺少版本间关联和变更记录

- **生产缴库表(tb_FinishedGoodsInv)**：
  - 关联生产订单
  - 在缴库审核时更新库存和成本
  - 缺少成本变更审计日志

### 2. 成本计算流程

- BOM成本更新机制：
  - 通过`UpdateParentBOMsAsync`递归更新上级BOM成本
  - 但缺少对BOM生效状态的强制校验
  - 未记录成本变更历史

- 缴库成本确认：
  - 缴库审核时更新库存成本
  - 计算公式：`CommService.CostCalculations.CostCalculation`
  - 递归更新上级BOM成本
  - 缺少基于最新BOM的强制重算机制

### 3. 主要问题

- **BOM版本管理不完善**：
  - 现有版本表缺乏变更原因、版本间关联等信息
  - 版本状态控制逻辑分散在多个布尔字段中
  - 缺少版本比对和差异分析功能

- **状态控制不严格**：
  - 使用多个布尔字段(is_enabled, is_available)控制状态，逻辑不清晰
  - 缺少明确的BOM生命周期管理机制
  - 未强制限制生产订单只能使用生效状态的BOM

- **成本追溯困难**：
  - BOM变更后无法追溯调整历史成本
  - 缺少完整的成本变更审计日志
  - 成本调整缺乏审批流程

- **强制成本重算机制缺失**：
  - 缴库时未强制基于最新BOM重算
  - 成本变更后未自动触发生产订单成本更新

## 二、改进方案

### 1. 数据结构优化

#### 1.1 BOM版本管理体系优化

利用现有的tb_BOMConfigHistory表进行扩展，强化版本管理功能：
```sql
-- 扩展BOM配置历史表
ALTER TABLE tb_BOMConfigHistory ADD COLUMN PreviousVerID BIGINT COMMENT '上一版本ID，关联BOM_S_VERID';
ALTER TABLE tb_BOMConfigHistory ADD COLUMN NextVerID BIGINT COMMENT '下一版本ID，关联BOM_S_VERID';
ALTER TABLE tb_BOMConfigHistory ADD COLUMN ChangeReason VARCHAR(255) COMMENT '变更原因';
ALTER TABLE tb_BOMConfigHistory ADD COLUMN ChangeContent TEXT COMMENT '变更内容描述';
ALTER TABLE tb_BOMConfigHistory ADD COLUMN ChangeType INT DEFAULT 0 COMMENT '变更类型：0=新增，1=修改，2=废除';
ALTER TABLE tb_BOMConfigHistory ADD COLUMN VersionStatus INT DEFAULT 0 COMMENT '版本状态：0=草稿，1=生效，2=失效';
ALTER TABLE tb_BOMConfigHistory ADD COLUMN ProdDetailID BIGINT COMMENT '关联产品ID，冗余字段便于查询';
```

#### 1.2 优化BOM主表状态管理
```sql
-- 优化tb_BOM_S表状态管理
ALTER TABLE tb_BOM_S ADD COLUMN Status INT NOT NULL DEFAULT 0 COMMENT '状态：0=草稿，1=生效，2=变更中，3=失效';
ALTER TABLE tb_BOM_S ADD COLUMN CostStatus INT DEFAULT 0 COMMENT '成本状态：0=暂估，1=实际';
ALTER TABLE tb_BOM_S ADD COLUMN LastCostUpdateTime DATETIME COMMENT '最后成本更新时间';
ALTER TABLE tb_BOM_S ADD COLUMN LastCostUpdateBy BIGINT COMMENT '最后成本更新人';

-- 优化现有字段用途
-- is_enabled：控制BOM是否可用
-- is_available：控制BOM是否可以被新的生产订单引用
-- DataStatus：沿用原有的数据状态管理
```

#### 1.3 新增表结构
```sql
-- 1. 成本调整单表
CREATE TABLE Cost_Adjustment (
  AdjustmentID BIGINT PRIMARY KEY AUTO_INCREMENT,
  AdjustmentNo VARCHAR(50) NOT NULL COMMENT '调整单号',
  RelatedFGID BIGINT COMMENT '关联缴库单ID',
  RelatedMOID BIGINT COMMENT '关联生产订单ID',
  BOMID BIGINT COMMENT '关联BOM ID',
  ProdDetailID BIGINT NOT NULL COMMENT '产品ID',
  OriginalCost DECIMAL(18,4) NOT NULL COMMENT '调整前成本',
  NewCost DECIMAL(18,4) NOT NULL COMMENT '调整后成本',
  AdjustmentAmount DECIMAL(18,4) NOT NULL COMMENT '调整金额',
  AdjustmentReason VARCHAR(255) NOT NULL COMMENT '调整原因',
  AdjustmentType INT NOT NULL COMMENT '调整类型：1=BOM完善，2=成本差错，3=其他',
  ApprovalStatus INT DEFAULT 0 COMMENT '审批状态：0=待审批，1=已审批，2=已拒绝',
  AffectInventory BIT DEFAULT 0 COMMENT '是否影响库存成本',
  Created_by BIGINT NOT NULL,
  Created_at DATETIME NOT NULL,
  Modified_by BIGINT,
  Modified_at DATETIME,
  Approved_by BIGINT,
  Approved_at DATETIME,
  INDEX idx_prod_detail (ProdDetailID),
  INDEX idx_approval_status (ApprovalStatus)
);

-- 2. 成本更新日志表（审计日志）
CREATE TABLE Cost_AuditLog (
  LogID BIGINT PRIMARY KEY AUTO_INCREMENT,
  RelatedID BIGINT COMMENT '关联ID(BOM/订单/缴库单)',
  EntityType VARCHAR(50) NOT NULL COMMENT '实体类型：BOM/ProductionOrder/FinishedGoodsInv/CostAdjustment',
  OldCost DECIMAL(18,4) NOT NULL COMMENT '更新前成本',
  NewCost DECIMAL(18,4) NOT NULL COMMENT '更新后成本',
  UpdateReason VARCHAR(255) COMMENT '更新原因',
  UpdateType INT NOT NULL COMMENT '更新类型：1=BOM变更，2=缴库重算，3=手动调整，4=系统自动',
  ReferenceNo VARCHAR(50) COMMENT '关联单号',
  OperatorID BIGINT NOT NULL,
  OperateTime DATETIME NOT NULL,
  IPAddress VARCHAR(50) COMMENT '操作IP地址',
  INDEX idx_related (RelatedID, EntityType),
  INDEX idx_entity (EntityType),
  INDEX idx_time (OperateTime)
);

-- 3. BOM版本差异记录表
CREATE TABLE BOM_VersionDiff (
  DiffID BIGINT PRIMARY KEY AUTO_INCREMENT,
  FromVerID BIGINT NOT NULL COMMENT '源版本ID',
  ToVerID BIGINT NOT NULL COMMENT '目标版本ID',
  DiffType INT NOT NULL COMMENT '差异类型：1=新增子件，2=删除子件，3=修改用量，4=修改损耗',
  ItemDetailID BIGINT COMMENT '子件ID',
  FromValue VARCHAR(255) COMMENT '变更前值',
  ToValue VARCHAR(255) COMMENT '变更后值',
  CostImpact DECIMAL(18,4) DEFAULT 0 COMMENT '成本影响金额',
  Created_at DATETIME NOT NULL,
  INDEX idx_versions (FromVerID, ToVerID),
  INDEX idx_item (ItemDetailID)
);
```

#### 1.4 生产订单表扩展
```sql
-- 添加字段到tb_ManufacturingOrder表
ALTER TABLE tb_ManufacturingOrder ADD COLUMN BOMVersion VARCHAR(20) COMMENT 'BOM版本号';
ALTER TABLE tb_ManufacturingOrder ADD COLUMN BOM_S_VERID BIGINT COMMENT '关联的BOM版本ID';
ALTER TABLE tb_ManufacturingOrder ADD COLUMN CostEstimateStatus INT DEFAULT 0 COMMENT '成本估算状态：0=暂估，1=实际';
ALTER TABLE tb_ManufacturingOrder ADD COLUMN IsCostRecalculated BIT DEFAULT 0 COMMENT '是否已重算成本';
ALTER TABLE tb_ManufacturingOrder ADD COLUMN CostRecalculationCount INT DEFAULT 0 COMMENT '成本重算次数';
ALTER TABLE tb_ManufacturingOrder ADD COLUMN LastCostRecalculateTime DATETIME COMMENT '最后成本重算时间';
```

### 2. BOM版本管理优化实现

#### 2.1 实体类定义

```csharp
// BOM状态枚举
public enum BOMStatus
{
    Draft = 0,      // 草稿
    Effective = 1,  // 生效
    Changing = 2,   // 变更中
    Invalid = 3     // 失效
}

// 版本状态枚举
public enum VersionStatus
{
    Draft = 0,      // 草稿
    Effective = 1,  // 生效
    Invalid = 2     // 失效
}

// BOM版本差异类型枚举
public enum BOMDiffType
{
    AddItem = 1,        // 新增子件
    RemoveItem = 2,     // 删除子件
    ModifyQuantity = 3, // 修改用量
    ModifyLoss = 4      // 修改损耗
}

// 扩展tb_BOMConfigHistory实体类
public partial class tb_BOMConfigHistory
{
    [SugarColumn(IsPrimaryKey = true)]
    public long BOM_S_VERID { get; set; }
    
    [SugarColumn]
    public long BOM_ID { get; set; }
    
    [SugarColumn]
    public long PreviousVerID { get; set; }
    
    [SugarColumn]
    public long NextVerID { get; set; }
    
    [SugarColumn]
    public string VerNo { get; set; }
    
    [SugarColumn]
    public DateTime? Effective_at { get; set; }
    
    [SugarColumn]
    public string ChangeReason { get; set; }
    
    [SugarColumn]
    public string ChangeContent { get; set; }
    
    [SugarColumn]
    public int ChangeType { get; set; }
    
    [SugarColumn]
    public int VersionStatus { get; set; }
    
    [SugarColumn]
    public long ProdDetailID { get; set; }
    
    // 导航属性
    [Navigate(NavigateType.OneToMany, nameof(tb_BOM_S_VERID))]
    public List<tb_BOMConfigHistory> NextVersions { get; set; }
    
    [Navigate(NavigateType.OneToMany, nameof(NextVerID))]
    public tb_BOMConfigHistory PreviousVersion { get; set; }
    
    [Navigate(NavigateType.OneToOne, nameof(BOM_ID))]
    public tb_BOM_S tb_BOM_S { get; set; }
}

// 成本审计日志实体类
public class Cost_AuditLog
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long LogID { get; set; }
    
    [SugarColumn]
    public long? RelatedID { get; set; }
    
    [SugarColumn]
    public string EntityType { get; set; }
    
    [SugarColumn(DecimalDigits = 4, Scale = 18)]
    public decimal OldCost { get; set; }
    
    [SugarColumn(DecimalDigits = 4, Scale = 18)]
    public decimal NewCost { get; set; }
    
    [SugarColumn]
    public string UpdateReason { get; set; }
    
    [SugarColumn]
    public int UpdateType { get; set; }
    
    [SugarColumn]
    public string ReferenceNo { get; set; }
    
    [SugarColumn]
    public long OperatorID { get; set; }
    
    [SugarColumn]
    public DateTime OperateTime { get; set; }
    
    [SugarColumn]
    public string IPAddress { get; set; }
}
```

#### 2.2 BOM版本管理核心逻辑实现

```csharp
// BOM版本管理器
public class BOMVersionManager
{
    private readonly DbContext _dbContext;
    private readonly CostAuditLogger _costLogger;
    
    public BOMVersionManager(DbContext dbContext, CostAuditLogger costLogger)
    {
        _dbContext = dbContext;
        _costLogger = costLogger;
    }
    
    // 创建新版本BOM
    public async Task<tb_BOMConfigHistory> CreateNewVersionAsync(long bomId, string changeReason, long operatorId, string ipAddress)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var currentBom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => b.BOM_ID == bomId);
            if (currentBom == null)
                throw new Exception("BOM不存在");
            
            // 获取当前有效的版本
            var currentVersion = await _dbContext.tb_BOMConfigHistory
                .Where(v => v.BOM_ID == bomId && v.VersionStatus == (int)VersionStatus.Effective)
                .OrderByDescending(v => v.BOM_S_VERID)
                .FirstOrDefaultAsync();
            
            // 生成新版本号
            string newVerNo = GenerateNewVersionNumber(currentVersion?.VerNo);
            
            // 创建新版本记录
            var newVersion = new tb_BOMConfigHistory
            {
                BOM_ID = bomId,
                VerNo = newVerNo,
                Effective_at = DateTime.Now,
                ChangeReason = changeReason,
                ChangeType = currentVersion == null ? 0 : 1, // 0=新增，1=修改
                VersionStatus = (int)VersionStatus.Draft,
                ProdDetailID = currentBom.ProdDetailID
            };
            
            await _dbContext.tb_BOMConfigHistory.AddAsync(newVersion);
            await _dbContext.SaveChangesAsync(); // 保存以获取自增ID
            
            // 更新版本链
            if (currentVersion != null)
            {
                currentVersion.NextVerID = newVersion.BOM_S_VERID;
                newVersion.PreviousVerID = currentVersion.BOM_S_VERID;
                
                // 将当前版本设置为失效
                currentVersion.VersionStatus = (int)VersionStatus.Invalid;
            }
            
            // 更新BOM状态为变更中
            currentBom.Status = (int)BOMStatus.Changing;
            
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return newVersion;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    // 生效新版本BOM
    public async Task ActivateVersionAsync(long versionId, long operatorId, string ipAddress)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var version = await _dbContext.tb_BOMConfigHistory
                .Include(v => v.tb_BOM_S)
                .FirstOrDefaultAsync(v => v.BOM_S_VERID == versionId);
            
            if (version == null)
                throw new Exception("版本记录不存在");
            
            if (version.VersionStatus != (int)VersionStatus.Draft)
                throw new Exception("只能生效草稿状态的版本");
            
            // 获取BOM详情
            var bom = version.tb_BOM_S;
            if (bom == null)
                throw new Exception("关联的BOM不存在");
            
            // 验证BOM完整性
            await ValidateBOMIntegrityAsync(bom.BOM_ID);
            
            // 记录成本变更前的值
            decimal oldCost = bom.TotalMaterialCost ?? 0;
            
            // 重新计算BOM成本
            await RecalculateBOMCostAsync(bom.BOM_ID);
            
            // 更新版本状态
            version.VersionStatus = (int)VersionStatus.Effective;
            version.Effective_at = DateTime.Now;
            
            // 更新BOM状态
            bom.Status = (int)BOMStatus.Effective;
            bom.is_enabled = true;
            bom.is_available = true;
            bom.LastCostUpdateTime = DateTime.Now;
            bom.LastCostUpdateBy = operatorId;
            
            // 记录成本变更日志
            await _costLogger.LogCostChangeAsync(
                bom.BOM_ID, 
                "BOM",
                oldCost, 
                bom.TotalMaterialCost ?? 0,
                $"BOM版本生效: {version.VerNo}",
                1, // 更新类型：BOM变更
                bom.BOM_No,
                operatorId,
                ipAddress
            );
            
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    // 验证BOM完整性
    private async Task ValidateBOMIntegrityAsync(long bomId)
    {
        var details = await _dbContext.tb_BOM_SDetail.Where(d => d.BOM_ID == bomId).ToListAsync();
        
        if (details.Count == 0)
            throw new Exception("BOM必须包含至少一个子件");
            
        // 验证子件完整性、用量合理性等
        foreach (var detail in details)
        {
            if (detail.UsedQty <= 0)
                throw new Exception($"子件用量必须大于0");
        }
    }
    
    // 重新计算BOM成本
    private async Task RecalculateBOMCostAsync(long bomId)
    {
        var bom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => b.BOM_ID == bomId);
        if (bom == null) return;
        
        var details = await _dbContext.tb_BOM_SDetail.Where(d => d.BOM_ID == bomId).ToListAsync();
        
        decimal totalMaterialCost = 0;
        
        foreach (var detail in details)
        {
            // 累加材料成本
            totalMaterialCost += detail.UsedQty * (detail.UnitCost ?? 0);
        }
        
        // 更新BOM成本
        bom.TotalMaterialCost = totalMaterialCost;
        bom.CostStatus = 0; // 重置为暂估状态
    }
    
    // 生成新版本号
    private string GenerateNewVersionNumber(string currentVersion)
    {
        if (string.IsNullOrEmpty(currentVersion))
            return "V1.0";
            
        // 解析版本号：V1.0 -> 1.0
        var match = Regex.Match(currentVersion, @"^V(\d+)\.(\d+)$");
        if (!match.Success)
            return "V1.0";
            
        int major = int.Parse(match.Groups[1].Value);
        int minor = int.Parse(match.Groups[2].Value);
        
        // 版本号递增逻辑
        minor++;
        if (minor >= 10)
        {
            major++;
            minor = 0;
        }
        
        return $"V{major}.{minor}";
    }
    
    // 比较两个版本的差异
    public async Task<List<BOM_VersionDiff>> CompareVersionsAsync(long fromVerId, long toVerId)
    {
        var diffs = new List<BOM_VersionDiff>();
        
        // 这里实现版本差异比较逻辑
        // ...
        
        return diffs;
    }
}
```

#### 2.3 BOM保存校验逻辑
```csharp
// 在BOM保存时增加子物料BOM状态校验
public class BOMValidator
{
    private readonly DbContext _dbContext;
    
    public BOMValidator(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task ValidateBeforeSaveAsync(tb_BOM_S bom, List<tb_BOM_SDetail> details)
    {
        // 验证BOM是否有子件
        if (details == null || details.Count == 0)
            throw new Exception("BOM必须包含至少一个子件");
            
        // 验证子件是否有循环引用
        await ValidateNoCircularReferenceAsync(bom.ProdDetailID, details);
        
        // 验证每个子件的有效性
        foreach (var detail in details)
        {
            // 获取子件信息
            var itemDetail = await _dbContext.tb_ItemDetail.FirstOrDefaultAsync(i => i.ID == detail.ItemDetailID);
            if (itemDetail == null)
                throw new Exception($"子件不存在，ID: {detail.ItemDetailID}");
                
            // 验证用量
            if (detail.UsedQty <= 0)
                throw new Exception($"子件{itemDetail.ItemName}的用量必须大于0");
                
            // 验证损耗率
            if (detail.LossRate.HasValue && (detail.LossRate.Value < 0 || detail.LossRate.Value > 100))
                throw new Exception($"子件{itemDetail.ItemName}的损耗率必须在0-100之间");
        }
    }
    
    // 验证是否有循环引用
    private async Task ValidateNoCircularReferenceAsync(long parentItemId, List<tb_BOM_SDetail> childDetails)
    {
        // 实现循环引用检测逻辑
        foreach (var detail in childDetails)
        {
            if (detail.ItemDetailID == parentItemId)
                throw new Exception("BOM存在循环引用");
                
            // 递归检查子件的子件
            var childBom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => 
                b.ProdDetailID == detail.ItemDetailID && 
                b.is_enabled && 
                b.Status == (int)BOMStatus.Effective);
                
            if (childBom != null)
            {
                var childBomDetails = await _dbContext.tb_BOM_SDetail.Where(d => d.BOM_ID == childBom.BOM_ID).ToListAsync();
                await ValidateNoCircularReferenceAsync(parentItemId, childBomDetails);
            }
        }
    }
}
```

### 3. 成本审计与联动更新机制

#### 3.1 成本审计日志管理器

```csharp
// 成本审计日志管理器
public class CostAuditLogger
{
    private readonly DbContext _dbContext;
    
    public CostAuditLogger(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // 记录成本变更
    public async Task LogCostChangeAsync(
        long? relatedId,
        string entityType,
        decimal oldCost,
        decimal newCost,
        string updateReason,
        int updateType,
        string referenceNo,
        long operatorId,
        string ipAddress = null)
    {
        // 只有成本发生变化时才记录日志
        if (Math.Abs(oldCost - newCost) > 0.0001m)
        {
            var auditLog = new Cost_AuditLog
            {
                RelatedID = relatedId,
                EntityType = entityType,
                OldCost = oldCost,
                NewCost = newCost,
                UpdateReason = updateReason,
                UpdateType = updateType,
                ReferenceNo = referenceNo,
                OperatorID = operatorId,
                OperateTime = DateTime.Now,
                IPAddress = ipAddress
            };
            
            await _dbContext.Cost_AuditLog.AddAsync(auditLog);
            await _dbContext.SaveChangesAsync();
        }
    }
    
    // 获取成本变更历史
    public async Task<List<Cost_AuditLog>> GetCostHistoryAsync(
        string entityType,
        long? relatedId = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var query = _dbContext.Cost_AuditLog.Where(l => l.EntityType == entityType);
        
        if (relatedId.HasValue)
            query = query.Where(l => l.RelatedID == relatedId.Value);
            
        if (startDate.HasValue)
            query = query.Where(l => l.OperateTime >= startDate.Value);
            
        if (endDate.HasValue)
            query = query.Where(l => l.OperateTime <= endDate.Value);
            
        return await query.OrderByDescending(l => l.OperateTime).ToListAsync();
    }
}
```

#### 3.2 成本联动更新管理器

```csharp
// 成本联动更新管理器
public class CostLinkageManager
{
    private readonly DbContext _dbContext;
    private readonly CostAuditLogger _costLogger;
    
    public CostLinkageManager(DbContext dbContext, CostAuditLogger costLogger)
    {
        _dbContext = dbContext;
        _costLogger = costLogger;
    }
    
    // BOM成本变更后的联动更新
    public async Task UpdateRelatedCostsWhenBOMChangedAsync(
        long bomId,
        decimal oldCost,
        decimal newCost,
        long operatorId,
        string ipAddress)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var bom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => b.BOM_ID == bomId);
            if (bom == null) return;
            
            // 获取所有引用了该BOM但未完成的生产订单
            var affectedOrders = await _dbContext.tb_ManufacturingOrder
                .Where(o => o.ProdDetailID == bom.ProdDetailID && 
                       o.Status != (int)OrderStatus.Completed && 
                       o.Status != (int)OrderStatus.Closed)
                .ToListAsync();
            
            foreach (var order in affectedOrders)
            {
                // 重算订单成本
                await RecalculateOrderCostAsync(order.ManufacturingOrderID, operatorId, ipAddress);
            }
            
            // 获取所有使用该产品作为子件的BOM
            var parentBoms = await _dbContext.tb_BOM_S
                .Where(b => b.is_enabled && b.Status == (int)BOMStatus.Effective)
                .Include(b => b.tb_BOM_SDetail)
                .Where(b => b.tb_BOM_SDetail.Any(d => d.ItemDetailID == bom.ProdDetailID))
                .ToListAsync();
            
            // 递归更新上层BOM成本
            foreach (var parentBom in parentBoms)
            {
                await UpdateParentBOMCostAsync(parentBom.BOM_ID, operatorId, ipAddress);
            }
            
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    // 重算指定生产订单的成本
    private async Task RecalculateOrderCostAsync(long orderId, long operatorId, string ipAddress)
    {
        var order = await _dbContext.tb_ManufacturingOrder.FirstOrDefaultAsync(o => o.ManufacturingOrderID == orderId);
        if (order == null) return;
        
        decimal oldTotalCost = order.TotalCost ?? 0;
        decimal newTotalCost = 0;
        
        // 获取生产订单明细
        var details = await _dbContext.tb_ManufacturingOrderDetail.Where(d => d.ManufacturingOrderID == orderId).ToListAsync();
        
        foreach (var detail in details)
        {
            decimal oldDetailCost = detail.Cost ?? 0;
            
            // 获取最新有效的BOM信息
            var bom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => 
                b.ProdDetailID == detail.ProdDetailID &&  
                b.is_enabled && 
                b.Status == (int)BOMStatus.Effective);
                
            if (bom != null)
            {
                // 使用最新BOM成本
                detail.Cost = bom.TotalMaterialCost ?? 0;
                detail.Amount = detail.Cost * detail.PlanQuantity;
                
                // 记录明细成本变更
                await _costLogger.LogCostChangeAsync(
                    detail.ManufacturingOrderDetailID,
                    "ManufacturingOrderDetail",
                    oldDetailCost,
                    detail.Cost.Value,
                    "BOM成本变更导致订单成本重算",
                    4, // 系统自动更新
                    order.OrderNo,
                    operatorId,
                    ipAddress);
                    
                newTotalCost += detail.Amount;
            }
        }
        
        // 更新订单总成本
        order.TotalCost = newTotalCost;
        order.IsCostRecalculated = true;
        order.CostRecalculationCount++;
        order.LastCostRecalculateTime = DateTime.Now;
        
        // 记录订单成本变更
        await _costLogger.LogCostChangeAsync(
            order.ManufacturingOrderID,
            "ManufacturingOrder",
            oldTotalCost,
            newTotalCost,
            "BOM成本变更导致订单成本重算",
            4, // 系统自动更新
            order.OrderNo,
            operatorId,
            ipAddress);
            
        await _dbContext.SaveChangesAsync();
    }
    
    // 更新父BOM成本
    private async Task UpdateParentBOMCostAsync(long bomId, long operatorId, string ipAddress)
    {
        var bom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => b.BOM_ID == bomId);
        if (bom == null) return;
        
        decimal oldCost = bom.TotalMaterialCost ?? 0;
        
        // 重新计算BOM成本
        await RecalculateBOMCostAsync(bomId);
        
        decimal newCost = bom.TotalMaterialCost ?? 0;
        
        // 记录BOM成本变更
        if (Math.Abs(oldCost - newCost) > 0.0001m)
        {
            await _costLogger.LogCostChangeAsync(
                bom.BOM_ID,
                "BOM",
                oldCost,
                newCost,
                "子件成本变更导致父BOM成本重算",
                4, // 系统自动更新
                bom.BOM_No,
                operatorId,
                ipAddress);
        }
    }
    
    // 重新计算BOM成本（内部方法）
    private async Task RecalculateBOMCostAsync(long bomId)
    {
        var bom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => b.BOM_ID == bomId);
        if (bom == null) return;
        
        var details = await _dbContext.tb_BOM_SDetail.Where(d => d.BOM_ID == bomId).ToListAsync();
        
        decimal totalMaterialCost = 0;
        
        foreach (var detail in details)
        {
            // 获取子件的最新成本
            decimal unitCost = 0;
            
            // 先尝试获取子件的BOM成本
            var childBom = await _dbContext.tb_BOM_S.FirstOrDefaultAsync(b => 
                b.ProdDetailID == detail.ItemDetailID && 
                b.is_enabled && 
                b.Status == (int)BOMStatus.Effective);
                
            if (childBom != null)
            {
                unitCost = childBom.TotalMaterialCost ?? 0;
            }
            else
            {
                // 如果没有BOM，则使用物料主数据中的成本
                var itemDetail = await _dbContext.tb_ItemDetail.FirstOrDefaultAsync(i => i.ID == detail.ItemDetailID);
                if (itemDetail != null)
                {
                    unitCost = itemDetail.UnitCost ?? 0;
                }
            }
            
            // 考虑损耗率计算实际用量
            decimal actualQuantity = detail.UsedQty;
            if (detail.LossRate.HasValue && detail.LossRate.Value > 0)
            {
                actualQuantity = detail.UsedQty / (1 - detail.LossRate.Value / 100);
            }
            
            totalMaterialCost += actualQuantity * unitCost;
        }
        
        // 更新BOM成本
        bom.TotalMaterialCost = totalMaterialCost;
        bom.LastCostUpdateTime = DateTime.Now;
        bom.LastCostUpdateBy = Thread.CurrentPrincipal?.Identity?.Name != null ? 
            long.Parse(Thread.CurrentPrincipal.Identity.Name) : 0;
        
        await _dbContext.SaveChangesAsync();
    }
}
```

#### 3.3 强制BOM生效校验
```csharp
// 在生产订单创建时校验BOM状态
public async Task<bool> ValidateBOMStatus(long bomId)
{
    var bom = await _dbClient.Queryable<tb_BOM_S>()
        .Where(b => b.BOM_ID == bomId).FirstAsync();
        
    if (bom.Status != (int)BOMStatus.Effective)
    {
        throw new BusinessException("只能使用生效状态的BOM创建生产订单");
    }
    return true;
}
```

#### 3.4 缴库审核时的成本重算
```csharp
// 在缴库审核时强制基于最新BOM重算
public async Task RecalculateCostAtStockIn(long fgId, long operatorId, string ipAddress)
{
    var fg = await _dbClient.Queryable<tb_FinishedGoodsInv>()
        .Includes(f => f.tb_ManufacturingOrder)
        .Where(f => f.FG_ID == fgId).FirstAsync();
        
    if (fg.MOID.HasValue)
    {
        var mo = fg.tb_ManufacturingOrder;
        // 获取当前BOM的最新版本
        var latestBOM = await _dbClient.Queryable<tb_BOM_S>()
            .Where(b => b.ProdDetailID == mo.ProdDetailID && b.Status == (int)BOMStatus.Effective)
            .OrderByDescending(b => b.BOM_S_VERID).FirstAsync();
            
        // 如果生产订单使用的BOM版本与最新版本不一致，进行成本重算
        if (mo.BOM_S_VERID != latestBOM.BOM_S_VERID)
        {
            // 使用成本联动管理器进行重算
            var costLinkageManager = new CostLinkageManager(_dbContext, _costLogger);
            await costLinkageManager.RecalculateOrderCostAsync(mo.ManufacturingOrderID, operatorId, ipAddress);
            
            // 更新缴库单记录使用的BOM版本信息
            fg.UsedBOMVersionID = latestBOM.BOM_S_VERID;
            fg.UsedBOMVersionNo = latestBOM.Version;
            await _dbContext.SaveChangesAsync();
        }
    }
}

### 4. 成本调整与审计实现

#### 4.1 成本调整单功能完整实现

```csharp
// 成本调整请求参数
public class CostAdjustmentRequest
{
    public long RelatedFGID { get; set; }
    public long RelatedMOID { get; set; }
    public long BOMID { get; set; }
    public long ProdDetailID { get; set; }
    public decimal OriginalCost { get; set; }
    public decimal NewCost { get; set; }
    public string AdjustmentReason { get; set; }
    public int AdjustmentType { get; set; }
    public long OperatorID { get; set; }
    public bool AffectInventory { get; set; }
    public string IPAddress { get; set; }
}

// 成本调整服务
public class CostAdjustmentService
{
    private readonly DbContext _dbContext;
    private readonly CostAuditLogger _costLogger;
    
    public CostAdjustmentService(DbContext dbContext, CostAuditLogger costLogger)
    {
        _dbContext = dbContext;
        _costLogger = costLogger;
    }
    
    // 创建成本调整单
    public async Task<Cost_Adjustment> CreateAdjustmentAsync(CostAdjustmentRequest request)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // 创建调整单记录
            var adjustment = new Cost_Adjustment
            {
                AdjustmentNo = await GenerateAdjustmentNo(),
                RelatedFGID = request.RelatedFGID,
                RelatedMOID = request.RelatedMOID,
                BOMID = request.BOMID,
                ProdDetailID = request.ProdDetailID,
                OriginalCost = request.OriginalCost,
                NewCost = request.NewCost,
                AdjustmentAmount = request.NewCost - request.OriginalCost,
                AdjustmentReason = request.AdjustmentReason,
                AdjustmentType = request.AdjustmentType,
                ApprovalStatus = 0, // 待审批
                AffectInventory = request.AffectInventory,
                Created_by = request.OperatorID,
                Created_at = DateTime.Now,
            };
            
            await _dbContext.Cost_Adjustment.AddAsync(adjustment);
            await _dbContext.SaveChangesAsync();
            
            // 记录创建调整单的审计日志
            await _costLogger.LogCostChangeAsync(
                adjustment.AdjustmentID,
                "CostAdjustment",
                request.OriginalCost,
                request.NewCost,
                $"创建成本调整单: {adjustment.AdjustmentNo}",
                3, // 手动调整
                adjustment.AdjustmentNo,
                request.OperatorID,
                request.IPAddress
            );
            
            await transaction.CommitAsync();
            return adjustment;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    // 审批成本调整单
    public async Task ApproveAdjustmentAsync(long adjustmentId, bool approved, long operatorId, string ipAddress)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var adjustment = await _dbContext.Cost_Adjustment.FirstOrDefaultAsync(a => a.AdjustmentID == adjustmentId);
            if (adjustment == null)
                throw new Exception("调整单不存在");
            
            if (adjustment.ApprovalStatus != 0) // 0=待审批
                throw new Exception("调整单已审批");
            
            adjustment.ApprovalStatus = approved ? 1 : 2; // 1=已审批，2=已拒绝
            adjustment.Approved_by = operatorId;
            adjustment.Approved_at = DateTime.Now;
            
            if (approved)
            {
                // 根据关联对象更新成本
                if (adjustment.RelatedFGID.HasValue)
                {
                    // 更新缴库单成本
                    var fg = await _dbContext.tb_FinishedGoodsInv.FirstOrDefaultAsync(f => f.FG_ID == adjustment.RelatedFGID.Value);
                    if (fg != null)
                    {
                        decimal oldCost = fg.UnitCost ?? 0;
                        fg.UnitCost = adjustment.NewCost;
                        fg.TotalCost = adjustment.NewCost * fg.Quantity;
                        
                        // 记录成本变更审计日志
                        await _costLogger.LogCostChangeAsync(
                            fg.FG_ID,
                            "FinishedGoodsInv",
                            oldCost,
                            adjustment.NewCost,
                            $"审批成本调整单: {adjustment.AdjustmentNo}",
                            3, // 手动调整
                            fg.FG_No,
                            operatorId,
                            ipAddress
                        );
                    }
                }
                
                if (adjustment.RelatedMOID.HasValue)
                {
                    // 更新生产订单成本
                    var mo = await _dbContext.tb_ManufacturingOrder.FirstOrDefaultAsync(m => m.ManufacturingOrderID == adjustment.RelatedMOID.Value);
                    if (mo != null)
                    {
                        decimal oldCost = mo.UnitCost ?? 0;
                        mo.UnitCost = adjustment.NewCost;
                        mo.TotalCost = adjustment.NewCost * mo.PlanQuantity;
                        mo.IsCostRecalculated = true;
                        
                        // 记录成本变更审计日志
                        await _costLogger.LogCostChangeAsync(
                            mo.ManufacturingOrderID,
                            "ManufacturingOrder",
                            oldCost,
                            adjustment.NewCost,
                            $"审批成本调整单: {adjustment.AdjustmentNo}",
                            3, // 手动调整
                            mo.OrderNo,
                            operatorId,
                            ipAddress
                        );
                    }
                }
                
                // 如果选择影响库存成本
                if (adjustment.AffectInventory)
                {
                    // 更新产品库存成本
                    var itemDetail = await _dbContext.tb_ItemDetail.FirstOrDefaultAsync(i => i.ID == adjustment.ProdDetailID);
                    if (itemDetail != null)
                    {
                        decimal oldCost = itemDetail.UnitCost ?? 0;
                        itemDetail.UnitCost = adjustment.NewCost;
                        
                        // 记录成本变更审计日志
                        await _costLogger.LogCostChangeAsync(
                            itemDetail.ID,
                            "ItemDetail",
                            oldCost,
                            adjustment.NewCost,
                            $"审批成本调整单并更新库存成本: {adjustment.AdjustmentNo}",
                            3, // 手动调整
                            itemDetail.ItemCode,
                            operatorId,
                            ipAddress
                        );
                    }
                }
            }
            
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    // 生成调整单号
    private async Task<string> GenerateAdjustmentNo()
    {
        string prefix = "ADJ" + DateTime.Now.ToString("yyyyMMdd");
        var lastNo = await _dbContext.Cost_Adjustment
            .Where(a => a.AdjustmentNo.StartsWith(prefix))
            .OrderByDescending(a => a.AdjustmentNo)
            .Select(a => a.AdjustmentNo)
            .FirstOrDefaultAsync();
        
        if (string.IsNullOrEmpty(lastNo))
            return prefix + "001";
            
        int seq = int.Parse(lastNo.Substring(12)) + 1;
        return prefix + seq.ToString("000");
    }
}

### 5. UI界面优化

#### 5.1 BOM管理界面

```csharp
// 在BOM管理界面中添加版本管理和状态控制功能
public partial class UCBOMManagement : BaseUC
{
    private readonly BOMVersionManager _versionManager;
    private readonly CostLinkageManager _costLinkageManager;
    
    public UCBOMManagement(BOMVersionManager versionManager, CostLinkageManager costLinkageManager)
    {
        InitializeComponent();
        _versionManager = versionManager;
        _costLinkageManager = costLinkageManager;
    }
    
    // 新增版本按钮点击事件
    private async void btnNewVersion_Click(object sender, EventArgs e)
    {
        try
        {
            long bomId = GetCurrentBomId();
            if (bomId <= 0) return;
            
            // 显示确认对话框
            if (MessageBox.Show("确定要为此配方创建新版本吗？", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
                
            // 获取用户输入的版本备注
            string versionNote = PromptDialog.Show("请输入版本备注：", "版本备注");
            if (string.IsNullOrWhiteSpace(versionNote)) return;
            
            // 创建新版本
            var newBom = await _versionManager.CreateNewVersionAsync(
                bomId, 
                UserInfo.UserID, 
                versionNote,
                this.GetUserIpAddress());
            
            // 显示成功消息并刷新列表
            ShowMessage($"成功创建配方新版本，版本号：{newBom.BOM_S_VERID}");
            await RefreshBomListAsync();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("创建新版本失败：" + ex.Message);
        }
    }
    
    // 启用/禁用BOM按钮点击事件
    private async void btnToggleStatus_Click(object sender, EventArgs e)
    {
        try
        {
            long bomId = GetCurrentBomId();
            if (bomId <= 0) return;
            
            var bom = await _dbClient.Queryable<tb_BOM_S>().FirstAsync(b => b.BOM_ID == bomId);
            int newStatus = bom.is_enabled ? 0 : 1;
            
            // 更新状态
            await _dbClient.Updateable<tb_BOM_S>()
                .SetColumns(b => new tb_BOM_S { is_enabled = newStatus, Modified_at = DateTime.Now })
                .Where(b => b.BOM_ID == bomId)
                .ExecuteCommandAsync();
            
            // 记录状态变更日志
            await _costLogger.LogStatusChangeAsync(
                bomId, 
                "BOM", 
                bom.is_enabled ? "启用" : "禁用", 
                newStatus == 1 ? "启用" : "禁用", 
                "手动调整状态", 
                UserInfo.UserID,
                this.GetUserIpAddress());
            
            ShowMessage("配方状态更新成功！");
            await RefreshBomListAsync();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("更新状态失败：" + ex.Message);
        }
    }
    
    // 版本比较按钮点击事件
    private void btnCompareVersions_Click(object sender, EventArgs e)
    {
        // 实现版本比较功能
        using (var form = new FormVersionCompare())
        {
            form.BomId = GetCurrentBomId();
            form.ShowDialog();
        }
    }
}
```

#### 5.2 生产订单界面

```csharp
// 在UCManufacturingOrder.cs中添加成本重算和调整功能
public partial class UCManufacturingOrder : BaseBillEditGeneric<tb_ManufacturingOrder, tb_ManufacturingOrderDetail>
{
    private readonly CostAdjustmentService _costAdjustmentService;
    private readonly CostLinkageManager _costLinkageManager;
    
    public UCManufacturingOrder(CostAdjustmentService costAdjustmentService, CostLinkageManager costLinkageManager)
    {
        InitializeComponent();
        _costAdjustmentService = costAdjustmentService;
        _costLinkageManager = costLinkageManager;
        
        // 添加按钮到界面
        InitializeCustomButtons();
    }
    
    private void InitializeCustomButtons()
    {
        // 创建成本重算按钮
        Button btnRecalculateCost = new Button
        {
            Text = "重算成本",
            Size = new Size(80, 30),
            Location = new Point(10, pnlActions.Height - 40),
            ToolTipText = "根据最新BOM重新计算此生产订单的成本"
        };
        btnRecalculateCost.Click += btnRecalculateCost_Click;
        pnlActions.Controls.Add(btnRecalculateCost);
        
        // 创建成本调整按钮
        Button btnCostAdjustment = new Button
        {
            Text = "成本调整",
            Size = new Size(80, 30),
            Location = new Point(100, pnlActions.Height - 40),
            ToolTipText = "创建成本调整单"
        };
        btnCostAdjustment.Click += btnCostAdjustment_Click;
        pnlActions.Controls.Add(btnCostAdjustment);
    }
    
    // 添加成本重算按钮点击事件
    private async void btnRecalculateCost_Click(object sender, EventArgs e)
    {
        try
        {
            // 获取当前订单ID
            long orderId = GetCurrentOrderId();
            if (orderId <= 0)
                throw new Exception("未找到当前订单");
                
            // 显示确认对话框
            if (MessageBox.Show("确定要重新计算此生产订单的成本吗？此操作可能会影响历史数据。", "确认", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
                
            // 调用成本联动管理器进行重算
            await _costLinkageManager.RecalculateOrderCostAsync(
                orderId,
                UserInfo.UserID,
                this.GetUserIpAddress());
            
            // 更新UI
            await LoadDataToUIAsync();
            
            // 显示成功消息
            ShowMessage("成本重算完成！");
        }
        catch (Exception ex)
        {
            ShowErrorMessage("成本重算失败：" + ex.Message);
        }
    }
    
    // 添加成本调整按钮点击事件
    private void btnCostAdjustment_Click(object sender, EventArgs e)
    {
        try
        {
            // 获取当前订单信息
            var mo = await _dbClient.Queryable<tb_ManufacturingOrder>()
                .FirstAsync(m => m.ManufacturingOrderID == GetCurrentOrderId());
                
            // 打开成本调整表单
            using (var form = new FormCostAdjustment(_costAdjustmentService))
            {
                // 设置当前订单信息
                form.RelatedOrderId = GetCurrentOrderId();
                form.RelatedOrderNo = mo.OrderNo;
                form.RelatedType = "MO";
                form.CurrentCost = mo.UnitCost ?? 0;
                form.ProdDetailId = mo.ProductID;
                
                // 显示表单
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 更新UI
                    await LoadDataToUIAsync();
                }
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("打开成本调整单失败：" + ex.Message);
        }
    }
    
    // 异步加载数据到UI
    private async Task LoadDataToUIAsync()
    {
        long orderId = GetCurrentOrderId();
        if (orderId <= 0) return;
        
        var mo = await _dbClient.Queryable<tb_ManufacturingOrder>()
            .FirstAsync(m => m.ManufacturingOrderID == orderId);
            
        if (mo != null)
        {
            tb_UnitCost.Text = mo.UnitCost?.ToString("F4") ?? "0.0000";
            tb_TotalCost.Text = mo.TotalCost?.ToString("F2") ?? "0.00";
            
            // 显示成本是否已重算的状态
            if (mo.IsCostRecalculated ?? false)
            {
                lblCostStatus.Text = "已重算成本";
                lblCostStatus.ForeColor = Color.Green;
            }
            else
            {
                lblCostStatus.Text = "原始成本";
                lblCostStatus.ForeColor = Color.Black;
            }
        }
    }
}
```

#### 5.3 缴库界面

```csharp
// 在缴库界面中添加强制重算和成本审计功能
public partial class UCFinishedGoodsInv : BaseBillEditGeneric<tb_FinishedGoodsInv, tb_FinishedGoodsInvDetail>
{
    private readonly CostLinkageManager _costLinkageManager;
    private readonly CostAuditLogger _costLogger;
    
    public UCFinishedGoodsInv(CostLinkageManager costLinkageManager, CostAuditLogger costLogger)
    {
        InitializeComponent();
        _costLinkageManager = costLinkageManager;
        _costLogger = costLogger;
    }
    
    // 审核按钮重写，增加成本重算选项
    private async void btnAudit_Click(object sender, EventArgs e)
    {
        try
        {
            long fgId = GetCurrentFgId();
            if (fgId <= 0) return;
            
            // 显示包含成本重算选项的确认对话框
            using (var form = new FormAuditConfirm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // 如果用户选择了基于最新BOM重算
                    if (form.RecalculateCost)
                    {
                        // 进行成本重算
                        await _costLinkageManager.RecalculateOrderCostAsync(
                            GetCurrentOrderIdFromFg(fgId),
                            UserInfo.UserID,
                            this.GetUserIpAddress());
                    }
                    
                    // 执行原有的审核逻辑
                    await PerformAuditAsync(fgId);
                }
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("审核失败：" + ex.Message);
        }
    }
    
    // 查看成本变更历史按钮
    private void btnViewCostHistory_Click(object sender, EventArgs e)
    {
        try
        {
            long fgId = GetCurrentFgId();
            if (fgId <= 0) return;
            
            // 打开成本历史查看表单
            using (var form = new FormCostHistoryViewer())
            {
                form.RelatedId = fgId;
                form.EntityType = "FinishedGoodsInv";
                form.ShowDialog();
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("打开成本历史失败：" + ex.Message);
        }
    }
}
```

#### 5.4 成本调整单界面

```csharp
// 成本调整单表单
public partial class FormCostAdjustment : Form
{
    private readonly CostAdjustmentService _costAdjustmentService;
    
    // 公共属性供调用者设置
    public long RelatedOrderId { get; set; }
    public string RelatedOrderNo { get; set; }
    public string RelatedType { get; set; }
    public decimal CurrentCost { get; set; }
    public long ProdDetailId { get; set; }
    
    public FormCostAdjustment(CostAdjustmentService costAdjustmentService)
    {
        InitializeComponent();
        _costAdjustmentService = costAdjustmentService;
    }
    
    private void FormCostAdjustment_Load(object sender, EventArgs e)
    {
        // 显示关联订单信息
        txtRelatedOrder.Text = $"{RelatedType}：{RelatedOrderNo}";
        txtCurrentCost.Text = CurrentCost.ToString("F4");
        
        // 绑定调整原因下拉框
        cboReason.Items.AddRange(new[]
        {
            "BOM变更", 
            "材料价格波动",
            "人工成本调整", 
            "制造费用分摊调整",
            "其他原因"
        });
        cboReason.SelectedIndex = 0;
    }
    
    private async void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // 验证输入
            if (!decimal.TryParse(txtNewCost.Text, out decimal newCost))
            {
                ShowMessage("请输入有效的新成本值");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(cboReason.Text))
            {
                ShowMessage("请选择调整原因");
                return;
            }
            
            // 创建调整请求
            var request = new CostAdjustmentRequest
            {
                RelatedFGID = RelatedType == "FG" ? RelatedOrderId : 0,
                RelatedMOID = RelatedType == "MO" ? RelatedOrderId : 0,
                ProdDetailID = ProdDetailId,
                OriginalCost = CurrentCost,
                NewCost = newCost,
                AdjustmentReason = cboReason.Text + (string.IsNullOrWhiteSpace(txtRemark.Text) ? "" : $"：{txtRemark.Text}"),
                AdjustmentType = 3, // 手动调整
                OperatorID = UserInfo.UserID,
                AffectInventory = chkAffectInventory.Checked,
                IPAddress = this.GetUserIpAddress()
            };
            
            // 提交调整单
            await _costAdjustmentService.CreateAdjustmentAsync(request);
            
            ShowMessage("成本调整单创建成功，请等待审批");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ShowErrorMessage("创建调整单失败：" + ex.Message);
        }
    }
    
    private void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void ShowErrorMessage(string message)
    {
        MessageBox.Show(this, message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

#### 5.5 BOM版本比较界面

```csharp
// BOM版本比较表单
public partial class FormVersionCompare : Form
{
    public long BomId { get; set; }
    private readonly SqlSugarClient _dbClient;
    
    public FormVersionCompare()
    {
        InitializeComponent();
        _dbClient = DbContext.Current.DbClient;
    }
    
    private async void FormVersionCompare_Load(object sender, EventArgs e)
    {
        await LoadVersionsAsync();
    }
    
    private async Task LoadVersionsAsync()
    {
        // 加载BOM版本列表
        var versions = await _dbClient.Queryable<tb_BOM_S>()
            .Where(b => b.ProdDetailID == await GetProductIdFromBom(BomId))
            .OrderBy(b => b.BOM_S_VERID)
            .ToListAsync();
            
        cboVersion1.Items.AddRange(versions.Select(v => new VersionItem(v)).ToArray());
        cboVersion2.Items.AddRange(versions.Select(v => new VersionItem(v)).ToArray());
        
        // 默认选择最近的两个版本
        if (cboVersion1.Items.Count >= 2)
        {
            cboVersion1.SelectedIndex = cboVersion1.Items.Count - 1;
            cboVersion2.SelectedIndex = cboVersion1.Items.Count - 2;
        }
    }
    
    private async void btnCompare_Click(object sender, EventArgs e)
    {
        try
        {
            if (cboVersion1.SelectedItem == null || cboVersion2.SelectedItem == null)
            {
                ShowMessage("请选择两个要比较的版本");
                return;
            }
            
            var version1 = (VersionItem)cboVersion1.SelectedItem;
            var version2 = (VersionItem)cboVersion2.SelectedItem;
            
            // 加载两个版本的BOM明细
            var details1 = await _dbClient.Queryable<tb_BOM_D>()
                .Where(d => d.BOM_ID == version1.Bom.BOM_ID)
                .ToListAsync();
                
            var details2 = await _dbClient.Queryable<tb_BOM_D>()
                .Where(d => d.BOM_ID == version2.Bom.BOM_ID)
                .ToListAsync();
            
            // 清空比较结果
            dgvComparison.Rows.Clear();
            
            // 合并所有子件ID
            var allItemIds = details1.Select(d => d.ItemDetailID).Union(details2.Select(d => d.ItemDetailID)).ToList();
            
            // 填充比较结果
            foreach (var itemId in allItemIds)
            {
                var detail1 = details1.FirstOrDefault(d => d.ItemDetailID == itemId);
                var detail2 = details2.FirstOrDefault(d => d.ItemDetailID == itemId);
                
                string itemCode = "", itemName = "";
                string quantity1 = "", quantity2 = "";
                string wasteRate1 = "", wasteRate2 = "";
                string unitPrice1 = "", unitPrice2 = "";
                Color rowColor = Color.White;
                
                if (detail1 != null)
                {
                    itemCode = detail1.ItemCode;
                    itemName = detail1.ItemName;
                    quantity1 = detail1.Quantity.ToString("F4");
                    wasteRate1 = (detail1.WasteRate * 100).ToString("F2") + "%";
                    unitPrice1 = (detail1.UnitPrice ?? 0).ToString("F4");
                }
                
                if (detail2 != null)
                {
                    itemCode = detail2.ItemCode;
                    itemName = detail2.ItemName;
                    quantity2 = detail2.Quantity.ToString("F4");
                    wasteRate2 = (detail2.WasteRate * 100).ToString("F2") + "%";
                    unitPrice2 = (detail2.UnitPrice ?? 0).ToString("F4");
                }
                
                // 根据差异设置行颜色
                if (detail1 == null) // 新增
                {
                    rowColor = Color.LightGreen;
                }
                else if (detail2 == null) // 删除
                {
                    rowColor = Color.LightSalmon;
                }
                else if (detail1.Quantity != detail2.Quantity || 
                         detail1.WasteRate != detail2.WasteRate ||
                         detail1.UnitPrice != detail2.UnitPrice) // 修改
                {
                    rowColor = Color.LightYellow;
                }
                
                int rowIndex = dgvComparison.Rows.Add(itemCode, itemName, quantity1, quantity2, wasteRate1, wasteRate2, unitPrice1, unitPrice2);
                dgvComparison.Rows[rowIndex].DefaultCellStyle.BackColor = rowColor;
            }
        }
        catch (Exception ex)
        {
            ShowErrorMessage("版本比较失败：" + ex.Message);
        }
    }
    
    private void ShowMessage(string message)
    {
        MessageBox.Show(this, message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    
    private void ShowErrorMessage(string message)
    {
        MessageBox.Show(this, message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    
    private async Task<long> GetProductIdFromBom(long bomId)
    {
        var bom = await _dbClient.Queryable<tb_BOM_S>().FirstAsync(b => b.BOM_ID == bomId);
        return bom?.ProdDetailID ?? 0;
    }
    
    // 版本项类，用于下拉框显示
    private class VersionItem
    {
        public tb_BOM_S Bom { get; set; }
        
        public VersionItem(tb_BOM_S bom)
        {
            Bom = bom;
        }
        
        public override string ToString()
        {
            return $"版本 {Bom.BOM_S_VERID} ({Bom.Effective_at?.ToString("yyyy-MM-dd")} {GetStatusText(Bom.is_enabled)})";
        }
        
        private string GetStatusText(int status)
        {
            return status == 1 ? "启用" : "禁用";
        }
    }
}
```

## 三、实施计划

### 1. 阶段一：基础架构（2周）
- 扩展数据结构，添加所需字段（BOM_VersionDiff、Cost_AuditLog等新表）
- 实现BOM状态管理（版本关联、状态控制）
- 更新数据迁移脚本，确保向后兼容
- 建立BOM版本与状态的关联机制

### 2. 阶段二：核心功能（3周）
- 实现BOM状态校验逻辑（BOMValidator验证器）
- 添加强制成本重算机制（CostLinkageManager）
- 开发成本调整单功能（CostAdjustmentService）
- 实现成本审计日志系统（CostAuditLogger）
- 建立BOM版本管理体系（BOMVersionManager）

### 3. 阶段三：界面优化（2周）
- 更新相关UI界面（BOM管理界面、生产订单界面、缴库界面）
- 添加状态和版本显示（版本切换、状态控制）
- 实现操作流程优化（成本调整、版本比较）
- 新增成本调整单界面和BOM版本比较界面

### 4. 阶段四：历史数据处理（1周）
- 开发历史BOM数据迁移工具
- 提供历史成本追溯调整功能
- 进行数据一致性验证和修复

### 5. 阶段五：测试与部署（1周）
- 单元测试和集成测试
- 性能测试和优化
- 用户验收测试
- 正式环境部署

## 四、风险控制

### 4.1 数据迁移风险
- **风险描述**：修改表结构和新增字段可能导致数据丢失或不兼容
- **应对措施**：
  - 实施全面的数据备份策略（开发前、迁移前各备份一次）
  - 编写详细的增量迁移脚本，确保数据完整性
  - 保留历史数据存档，支持数据回滚
  - 在测试环境完成验证后再应用到生产环境

### 4.2 业务中断风险
- **风险描述**：系统升级可能导致短暂的业务中断
- **应对措施**：
  - 分阶段实施，优先开发核心功能
  - 预留完整的回滚方案
  - 选择业务低峰期进行部署
  - 提前通知相关业务部门

### 4.3 用户习惯变更风险
- **风险描述**：新的界面和操作流程可能影响用户工作效率
- **应对措施**：
  - 提供详细的操作手册和视频教程
  - 组织用户培训会议，确保用户理解新功能
  - 保持核心操作逻辑一致性，减少学习成本
  - 收集用户反馈，持续优化界面体验

### 4.4 性能影响风险
- **风险描述**：递归计算和复杂查询可能导致系统性能下降
- **应对措施**：
  - 对递归计算逻辑进行优化，避免性能瓶颈
  - 为关键字段添加适当的索引
  - 实现缓存机制，减少重复计算
  - 优化数据库连接和事务处理
  - 对大批量数据操作进行分批处理

### 4.5 数据一致性风险
- **风险描述**：多表关联和联动更新可能导致数据不一致
- **应对措施**：
  - 使用事务确保数据操作的原子性
  - 实现数据校验机制，定期验证数据完整性
  - 建立日志审计系统，记录所有关键操作

## 五、预期效果

### 5.1 功能完善
- 严格控制BOM生效流程，确保BOM状态与版本管理的协调
- 实现完整的成本追溯机制，支持多维度查询和分析
- 提高成本计算准确性和一致性，减少人为干预
- 满足财务审计要求，保留完整成本调整记录

### 5.2 业务价值
- **提升数据准确性**：通过严格的版本控制和状态管理，降低BOM和成本数据错误率
- **提高工作效率**：自动化的成本重算和联动更新减少手动操作时间
- **增强决策支持**：提供完整的BOM版本历史和成本变更记录，支持产品成本分析和优化
- **合规性增强**：完善的审计日志满足内部审计和外部合规要求

### 5.3 用户体验
- 直观的界面操作，简化版本切换和状态控制流程
- 快速的版本比较功能，帮助用户了解配方变更
- 便捷的成本历史查询，支持成本异常分析
- 流畅的成本调整流程，确保业务灵活性