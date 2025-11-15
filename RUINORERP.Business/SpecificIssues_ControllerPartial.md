# 关键ControllerPartial文件具体问题分析

## 1. tb_FM_PaymentRecordControllerPartial.cs (财务收付款单)

### 1.1 文件规模问题
- **文件大小**: 181.5KB
- **代码行数**: 3072行
- **问题**: 单个文件过大，维护困难

### 1.2 ApprovalAsync 方法分析

#### 方法复杂度
```csharp
public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    // 此方法包含以下复杂逻辑：
    // 1. 验证收付款单基本信息 (50+行)
    // 2. 根据业务类型分组处理 (100+行)
    // 3. 对账单来源的FIFO核销 (500+行)
    // 4. 应收应付处理 (200+行)
    // 5. 预收付款处理 (150+行)
    // 6. 其他业务单据处理 (300+行)
    // 7. 状态更新和记录生成 (200+行)
}
```

#### 具体问题

**问题1: 混合业务场景处理过于复杂**
```csharp
// 复杂的混合对冲场景处理
if (hasMixedTypes && statement.ReceivePaymentType == (int)ReceivePaymentType.付款)
{
    // 处理应收3102元 + 应付6120元 = 净支付3018元的场景
    offsetAmount = Math.Abs(receivableList.Sum(r => r.LocalBalanceAmount));
    
    // 先核销应收款（对冲项），再核销应付款（实际支付项）
    receivableList = receivableList.OrderBy(c => c.BusinessDate).ThenBy(c => c.Created_at).ToList();
    payableList = payableList.OrderBy(c => c.BusinessDate).ThenBy(c => c.Created_at).ToList();
    
    receivablePayableList.AddRange(receivableList);
    receivablePayableList.AddRange(payableList);
}
else if (hasMixedTypes && statement.ReceivePaymentType == (int)ReceivePaymentType.收款)
{
    // 反向逻辑，同样复杂
    // ...
}
```

**问题2: FIFO 核销算法实现**
```csharp
// 复杂的FIFO核销逻辑
foreach (var receivablePayable in receivablePayableList)
{
    // 计算本次可核销金额
    decimal statementAmountToWriteOff;
    
    if (isExactMatch)
    {
        statementAmountToWriteOff = StatementDetail.RemainingLocalAmount;
    }
    else if (absRemainingAmount <= Math.Abs(StatementPaidAmount))
    {
        statementAmountToWriteOff = StatementDetail.RemainingLocalAmount;
    }
    else
    {
        decimal ratio = Math.Abs(StatementPaidAmount) / absRemainingAmount;
        statementAmountToWriteOff = StatementDetail.RemainingLocalAmount * ratio;
    }
    
    // 更新核销金额和剩余金额
    StatementDetail.WrittenOffLocalAmount += statementAmountToWriteOff;
    StatementDetail.RemainingLocalAmount -= statementAmountToWriteOff;
    
    // 精度处理
    StatementDetail.WrittenOffLocalAmount = Math.Round(StatementDetail.WrittenOffLocalAmount, 2);
    StatementDetail.RemainingLocalAmount = Math.Round(StatementDetail.RemainingLocalAmount, 2);
    
    // 状态判断
    if (Math.Abs(StatementDetail.RemainingLocalAmount) < 0.01m)
    {
        StatementDetail.RemainingLocalAmount = 0;
        StatementDetail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.全额核销;
    }
    else
    {
        StatementDetail.ARAPWriteOffStatus = (int)ARAPWriteOffStatus.部分核销;
    }
    
    StatementPaidAmount -= Math.Abs(statementAmountToWriteOff);
}
```

**问题3: 业务规则验证分散**
```csharp
// 分散的业务规则验证
foreach (var PaymentRecordDetail in entity.tb_FM_PaymentRecordDetails)
{
    // 审核时检测明细中不能有相同来源单号
    var PendingApprovalDetails = await _appContext.Db.Queryable<tb_FM_PaymentRecordDetail>()
        .Includes(c => c.tb_fm_paymentrecord)
        .Where(c => c.SourceBilllId == PaymentRecordDetail.SourceBilllId && c.SourceBizType == PaymentRecordDetail.SourceBizType)
        .Where(c => c.tb_fm_paymentrecord.PaymentStatus >= (int)PaymentStatus.已支付)
        .ToListAsync();
    
    // 重复查询，性能问题
    PendingApprovalDetails.AddRange(PaymentRecordDetail);
    bool isValid = await ValidatePaymentDetails(PendingApprovalDetails, rmrs);
    if (!isValid)
    {
        return rmrs;
    }
}
```

### 1.3 改进建议

**建议1: 提取专门的核销服务**
```csharp
// 建议的核销服务接口
public interface IPaymentWriteOffService
{
    Task<WriteOffResult> WriteOffStatementAsync(StatementWriteOffRequest request);
    Task<WriteOffResult> WriteOffReceivablePayableAsync(ARAPWriteOffRequest request);
    Task<WriteOffResult> WriteOffPrePaymentAsync(PrePaymentWriteOffRequest request);
}

// FIFO核销策略
public class FIFOWriteOffStrategy : IWriteOffStrategy
{
    public async Task<WriteOffResult> ExecuteAsync(WriteOffContext context)
    {
        // 专门的FIFO核销逻辑
    }
}

// 混合核销策略
public class MixedWriteOffStrategy : IWriteOffStrategy
{
    public async Task<WriteOffResult> ExecuteAsync(WriteOffContext context)
    {
        // 处理混合对冲场景
    }
}
```

**建议2: 使用策略模式**
```csharp
public class PaymentWriteOffService
{
    private readonly Dictionary<BizType, IWriteOffStrategy> _strategies;
    
    public PaymentWriteOffService()
    {
        _strategies = new Dictionary<BizType, IWriteOffStrategy>
        {
            { BizType.对账单, new StatementWriteOffStrategy() },
            { BizType.应收应付, new ARAPWriteOffStrategy() },
            { BizType.预收付款, new PrePaymentWriteOffStrategy() },
            { BizType.混合核销, new MixedWriteOffStrategy() }
        };
    }
    
    public async Task<WriteOffResult> WriteOffAsync(WriteOffRequest request)
    {
        var strategy = _strategies[request.BizType];
        return await strategy.ExecuteAsync(request.Context);
    }
}
```

## 2. tb_SaleOrderControllerPartial.cs (销售订单)

### 2.1 主要问题

**问题1: 库存更新逻辑复杂**
```csharp
// 复杂的库存更新逻辑
var inventoryGroup = entity.tb_SaleOrderDetails
    .GroupBy(c => new { c.ProdDetailID, c.Location_ID })
    .Select(g => new 
    { 
        g.Key.ProdDetailID, 
        g.Key.Location_ID, 
        TotalQuantity = g.Sum(c => c.Quantity) 
    })
    .ToList();

// 循环处理每个库存组
foreach (var group in inventoryGroup)
{
    var inv = await inventoryController.GetInventoryAsync(group.ProdDetailID, group.Location_ID);
    
    // 复杂的库存校验和更新逻辑
    if (inv.Quantity < group.TotalQuantity)
    {
        if (!allowNegativeStock)
        {
            rmrs.ErrorMsg = $"商品【{productName}】库存不足，当前库存【{inv.Quantity}】，需求数量【{group.TotalQuantity}】";
            return rmrs;
        }
    }
    
    // 更新库存
    inv.Quantity -= group.TotalQuantity;
    inv.LatestOutboundTime = DateTime.Now;
    
    // 成本计算
    if (entity.CostCalculationMethod == (int)CostCalculationMethod.移动加权平均)
    {
        inv.CostMovingWA = CalculateMovingAverageCost(inv, group.TotalQuantity);
    }
}
```

**问题2: 预付款处理逻辑**
```csharp
// 复杂的预付款生成条件判断
if (entity.PrePaymentConditions.HasValue && 
    entity.PrePaymentConditions.Value > 0 && 
    entity.SaleOutStatus == (int)SaleOutStatus.未出库 &&
    entity.CustomerID.HasValue)
{
    // 生成预付款单
    var prePayment = new tb_FM_PrePayment
    {
        CustomerID = entity.CustomerID.Value,
        Currency_ID = entity.Currency_ID,
        ExchangeRate = entity.ExchangeRate,
        PrePaymentAmount = entity.TotalAmount * entity.PrePaymentConditions.Value / 100,
        // ... 更多属性设置
    };
    
    // 保存预付款单
    var prePaymentController = _appContext.GetRequiredService<tb_FM_PrePaymentController<tb_FM_PrePayment>>();
    var result = await prePaymentController.BaseSaveOrUpdate(prePayment);
    
    if (!result.Succeeded)
    {
        rmrs.ErrorMsg = "生成预付款单失败：" + result.ErrorMsg;
        return rmrs;
    }
}
```

### 2.2 改进建议

**建议1: 提取库存服务**
```csharp
public interface IInventoryService
{
    Task<InventoryCheckResult> CheckInventoryAsync(InventoryCheckRequest request);
    Task<InventoryUpdateResult> UpdateInventoryAsync(InventoryUpdateRequest request);
    Task<CostCalculationResult> CalculateCostAsync(CostCalculationRequest request);
}

public class InventoryService : IInventoryService
{
    public async Task<InventoryUpdateResult> UpdateInventoryAsync(InventoryUpdateRequest request)
    {
        // 统一的库存更新逻辑
        // 包括负库存检查、成本计算等
    }
}
```

**建议2: 提取预付款服务**
```csharp
public interface IPrePaymentService
{
    Task<PrePaymentResult> GeneratePrePaymentAsync(SaleOrder saleOrder);
    Task<bool> ValidatePrePaymentConditionsAsync(SaleOrder saleOrder);
}
```

## 3. tb_PurOrderControllerPartial.cs (采购订单)

### 3.1 主要问题

**问题1: BatchCloseCaseAsync 方法复杂**
```csharp
public async override Task<ReturnResults<bool>> BatchCloseCaseAsync(List<T> NeedCloseCaseList)
{
    // 处理多个采购订单的结案
    foreach (var entity in entitys)
    {
        // 检查结案条件
        if (entity.DataStatus != (int)DataStatus.确认 || !entity.ApprovalResults.HasValue)
        {
            continue;
        }
        
        // 处理未入库数量
        if (entity.tb_PurOrderDetails != null)
        {
            foreach (var detail in entity.tb_PurOrderDetails)
            {
                var undeliveredQty = detail.Quantity - (detail.DeliveredQty ?? 0);
                if (undeliveredQty > 0)
                {
                    // 更新库存的拟入库数量
                    await UpdateInventoryPendingQty(detail.ProdDetailID, detail.Location_ID, -undeliveredQty);
                }
            }
        }
        
        // 更新订单状态
        entity.DataStatus = (int)DataStatus.完结;
        entity.CloseCaseDate = DateTime.Now;
        entity.CloseCaseBy = userId;
    }
}
```

### 3.2 改进建议

**建议: 提取采购订单服务**
```csharp
public interface IPurchaseOrderService
{
    Task<CloseCaseResult> CloseCaseAsync(PurchaseOrder order);
    Task<CloseCaseResult> BatchCloseCaseAsync(List<PurchaseOrder> orders);
    Task<bool> ValidateCloseCaseConditionsAsync(PurchaseOrder order);
}
```

## 4. tb_StocktakeControllerPartial.cs (库存盘点)

### 4.1 主要问题

**问题1: 盘点逻辑复杂**
```csharp
public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    // 复杂的盘点模式处理
    foreach (var child in entity.tb_StocktakeDetails)
    {
        // 检查库存是否存在
        tb_Inventory inv = await ctrinv.IsExistEntityAsync(i => i.ProdDetailID == child.ProdDetailID && i.Location_ID == entity.Location_ID);
        
        if (inv == null)
        {
            if (CheckMode.期初盘点 != cm)
            {
                // 非期初盘点时必须有库存数据
                rmrs.ErrorMsg = "当前盘点产品在当前仓库中，无库存数据。请使用【期初盘点】方式盘点。";
                return rmrs;
            }
            
            // 创建新的库存记录
            inv = new tb_Inventory
            {
                Inv_Cost = child.UntaxedCost,
                Inv_AdvCost = child.UntaxedCost,
                // ... 其他属性
            };
        }
        
        // 根据调整类型更新库存
        if (entity.Adjust_Type == (int)Adjust_Type.全部)
        {
            inv.Quantity = inv.Quantity + child.DiffQty;
        }
        else if (entity.Adjust_Type == (int)Adjust_Type.减少 && child.DiffQty < 0)
        {
            inv.Quantity = inv.Quantity + child.DiffQty;
        }
        else if (entity.Adjust_Type == (int)Adjust_Type.增加 && child.DiffQty > 0)
        {
            inv.Quantity = inv.Quantity + child.DiffQty;
        }
        
        // 成本计算
        if (CheckMode.期初盘点 == cm && child.DiffQty > 0)
        {
            CommService.CostCalculations.CostCalculation(_appContext, inv, child.DiffQty, child.UntaxedCost);
        }
    }
}
```

**问题2: 损益处理逻辑**
```csharp
// 生成损益单
if (authorizeController.EnableFinancialModule())
{
    var ctrpayable = _appContext.GetRequiredService<tb_FM_ProfitLossController<tb_FM_ProfitLoss>>();
    tb_FM_ProfitLoss profitLoss = await ctrpayable.BuildProfitLoss(entity);
    ReturnMainSubResults<tb_FM_ProfitLoss> rmr = await ctrpayable.BaseSaveOrUpdateWithChild<tb_FM_ProfitLoss>(profitLoss);
    
    if (rmr.Succeeded)
    {
        rmrs.ReturnObjectAsOtherEntity = rmr.ReturnObject;
    }
}
```

### 4.2 改进建议

**建议: 提取盘点服务**
```csharp
public interface IStocktakeService
{
    Task<StocktakeResult> ProcessStocktakeAsync(Stocktake stocktake);
    Task<InventoryAdjustmentResult> AdjustInventoryAsync(StocktakeDetail detail);
    Task<ProfitLossResult> GenerateProfitLossAsync(Stocktake stocktake);
}

public class StocktakeService : IStocktakeService
{
    public async Task<StocktakeResult> ProcessStocktakeAsync(Stocktake stocktake)
    {
        // 统一的盘点处理逻辑
        // 包括库存检查、数量调整、成本计算等
    }
}
```

## 5. tb_ProductionDemandControllerPartial.cs (生产需求)

### 5.1 主要问题

**问题1: 审批逻辑包含业务更新**
```csharp
public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    // 更新计划单中已分析字段
    if (entity.tb_productionplan != null)
    {
        foreach (tb_ProductionDemandTargetDetail tag in entity.tb_ProductionDemandTargetDetails)
        {
            var planDetail = entity.tb_productionplan.tb_ProductionPlanDetails
                .FirstOrDefault(c => c.ProdDetailID == tag.ProdDetailID && c.Location_ID == tag.Location_ID);
            
            if (planDetail != null)
            {
                planDetail.IsAnalyzed = true;
                planDetail.AnalyzedQuantity += tag.NeedQuantity;
            }
        }
        
        await _unitOfWorkManage.GetDbClient()
            .Updateable<tb_ProductionPlanDetail>(entity.tb_productionplan.tb_ProductionPlanDetails)
            .ExecuteCommandAsync();
        
        // 如果全部分析过，则更新计划单状态
        if (entity.tb_productionplan.tb_ProductionPlanDetails.All(c => c.IsAnalyzed.HasValue && c.IsAnalyzed.Value))
        {
            entity.tb_productionplan.Analyzed = true;
            await _unitOfWorkManage.GetDbClient()
                .Updateable<tb_ProductionPlan>(entity.tb_productionplan)
                .ExecuteCommandAsync();
        }
    }
}
```

### 5.2 改进建议

**建议: 提取生产需求服务**
```csharp
public interface IProductionDemandService
{
    Task<AnalysisResult> AnalyzeProductionDemandAsync(ProductionDemand demand);
    Task<bool> UpdateProductionPlanAsync(ProductionDemand demand);
    Task<bool> ValidateAnalysisCompletionAsync(ProductionPlan plan);
}
```

## 6. 通用改进建议

### 6.1 架构改进

**建议1: 创建领域服务层**
```
当前架构问题：
Controllers (包含所有业务逻辑)
    ↓
Database (直接访问)

建议架构：
Controllers (API层 - 薄层)
    ↓
Application Services (应用服务层 - 协调)
    ↓
Domain Services (领域服务层 - 业务逻辑)
    ↓
Repository (仓储层 - 数据访问)
    ↓
Database
```

**建议2: 使用设计模式**
```csharp
// 策略模式处理不同业务场景
public interface IBusinessStrategy
{
    Task<BusinessResult> ExecuteAsync(BusinessContext context);
    bool CanHandle(BusinessType businessType);
}

// 工厂模式创建策略
public class BusinessStrategyFactory
{
    private readonly Dictionary<BusinessType, IBusinessStrategy> _strategies;
    
    public IBusinessStrategy CreateStrategy(BusinessType type)
    {
        return _strategies[type];
    }
}

// 模板方法模式处理审批流程
public abstract class ApprovalWorkflowTemplate
{
    public async Task<ApprovalResult> ProcessAsync(Entity entity)
    {
        // 1. 验证实体
        await ValidateEntityAsync(entity);
        
        // 2. 执行业务逻辑
        await ExecuteBusinessLogicAsync(entity);
        
        // 3. 更新状态
        await UpdateStatusAsync(entity);
        
        // 4. 记录日志
        await LogApprovalAsync(entity);
        
        return new ApprovalResult { Success = true };
    }
    
    protected abstract Task ValidateEntityAsync(Entity entity);
    protected abstract Task ExecuteBusinessLogicAsync(Entity entity);
    protected abstract Task UpdateStatusAsync(Entity entity);
}
```

### 6.2 代码重构

**建议1: 提取通用基类**
```csharp
// 通用审批控制器基类
public abstract class ApprovalControllerBase<T> : BaseController<T> where T : class
{
    protected readonly IUnitOfWorkManage _unitOfWorkManage;
    protected readonly ILogger _logger;
    protected readonly IApprovalService _approvalService;
    
    protected ApprovalControllerBase(IUnitOfWorkManage unitOfWorkManage, 
        ILogger logger, IApprovalService approvalService)
    {
        _unitOfWorkManage = unitOfWorkManage;
        _logger = logger;
        _approvalService = approvalService;
    }
    
    public override async Task<ReturnResults<T>> ApprovalAsync(T entity)
    {
        try
        {
            _unitOfWorkManage.BeginTran();
            
            var result = await _approvalService.ApproveAsync(entity);
            
            if (result.Succeeded)
            {
                _unitOfWorkManage.CommitTran();
            }
            else
            {
                _unitOfWorkManage.RollbackTran();
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            _logger.LogError(ex, "审批失败");
            return new ReturnResults<T> { ErrorMsg = ex.Message };
        }
    }
}
```

**建议2: 提取业务服务**
```csharp
// 库存服务接口
public interface IInventoryService
{
    Task<InventoryCheckResult> CheckInventoryAsync(CheckInventoryRequest request);
    Task<InventoryUpdateResult> UpdateInventoryAsync(UpdateInventoryRequest request);
    Task<InventoryCalculationResult> CalculateCostAsync(CostCalculationRequest request);
}

// 财务服务接口
public interface IFinanceService
{
    Task<WriteOffResult> WriteOffAsync(WriteOffRequest request);
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<SettlementResult> SettleAsync(SettlementRequest request);
}

// 业务规则验证服务
public interface IBusinessRuleValidator
{
    Task<ValidationResult> ValidateAsync<T>(T entity);
    Task<bool> CanApproveAsync<T>(T entity);
    Task<bool> CanAntiApproveAsync<T>(T entity);
}
```

### 6.3 性能优化

**建议1: 查询优化**
```csharp
// 优化前 - N+1查询问题
var mainList = await _db.Queryable<MainEntity>().ToListAsync();
foreach(var main in mainList)
{
    var details = await _db.Queryable<DetailEntity>()
        .Where(d => d.MainId == main.Id)
        .ToListAsync();
}

// 优化后 - 批量查询
var mainIds = mainList.Select(m => m.Id).ToList();
var allDetails = await _db.Queryable<DetailEntity>()
    .Where(d => mainIds.Contains(d.MainId))
    .ToListAsync();

// 内存中分组
var detailGroups = allDetails.GroupBy(d => d.MainId).ToDictionary(g => g.Key, g => g.ToList());
```

**建议2: 缓存策略**
```csharp
public interface ICacheManager
{
    Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
}

// 使用缓存
public class InventoryService
{
    private readonly ICacheManager _cacheManager;
    
    public async Task<Inventory> GetInventoryAsync(int productId, int locationId)
    {
        var cacheKey = $"inventory_{productId}_{locationId}";
        return await _cacheManager.GetOrCreateAsync(cacheKey, async () =>
        {
            return await _db.Queryable<Inventory>()
                .Where(i => i.ProdDetailID == productId && i.Location_ID == locationId)
                .FirstAsync();
        }, TimeSpan.FromMinutes(5));
    }
}
```

这些具体的改进建议可以帮助解决当前代码中的主要问题，提高代码质量、可维护性和系统性能。