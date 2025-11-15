# ControllerPartial代码重构改进建议

## 1. 重构优先级排序

### 🔥 高优先级 (立即执行)
1. **财务收付款单控制器重构** - tb_FM_PaymentRecordControllerPartial.cs
   - 文件过大 (181.5KB, 3072行)
   - 业务逻辑过于复杂
   - 混合核销场景处理混乱

2. **销售订单控制器重构** - tb_SaleOrderControllerPartial.cs
   - 库存更新逻辑复杂
   - 预付款处理逻辑分散

3. **采购订单控制器重构** - tb_PurOrderControllerPartial.cs
   - BatchCloseCaseAsync 方法复杂
   - 库存处理逻辑需要优化

### ⚡ 中优先级 (近期执行)
4. **库存盘点控制器重构** - tb_StocktakeControllerPartial.cs
5. **生产需求控制器重构** - tb_ProductionDemandControllerPartial.cs
6. **售后发货控制器重构** - tb_AS_AfterSaleDeliveryControllerPartial.cs

### 📋 低优先级 (长期规划)
7. 其他控制器文件的标准化
8. 通用框架和工具类完善

## 2. 重构策略

### 2.1 分层架构重构

```
当前架构：
Controllers (包含所有业务逻辑)
    ↓
Database (直接访问)

目标架构：
Controllers (API层 - 薄层，只负责参数验证和结果返回)
    ↓
Application Services (应用服务层 - 协调多个领域服务)
    ↓
Domain Services (领域服务层 - 核心业务逻辑)
    ↓
Repository (仓储层 - 数据访问)
    ↓
Database
```

### 2.2 领域服务提取

#### 财务领域服务
```csharp
// 财务核销服务接口
public interface IPaymentWriteOffService
{
    Task<WriteOffResult> WriteOffStatementAsync(StatementWriteOffRequest request);
    Task<WriteOffResult> WriteOffReceivablePayableAsync(ARAPWriteOffRequest request);
    Task<WriteOffResult> WriteOffPrePaymentAsync(PrePaymentWriteOffRequest request);
    Task<WriteOffResult> ProcessMixedWriteOffAsync(MixedWriteOffRequest request);
}

// FIFO核销策略
public class FIFOWriteOffStrategy : IWriteOffStrategy
{
    public async Task<WriteOffResult> ExecuteAsync(WriteOffContext context)
    {
        // 专门的FIFO核销逻辑
        var items = context.Items.OrderBy(i => i.Date).ThenBy(i => i.CreatedAt);
        decimal remainingAmount = context.AmountToWriteOff;
        
        foreach (var item in items)
        {
            if (remainingAmount <= 0) break;
            
            decimal writeOffAmount = Math.Min(item.RemainingAmount, remainingAmount);
            item.WriteOffAmount += writeOffAmount;
            item.RemainingAmount -= writeOffAmount;
            remainingAmount -= writeOffAmount;
            
            // 更新状态
            if (item.RemainingAmount <= 0.01m)
            {
                item.Status = WriteOffStatus.FullyWrittenOff;
            }
            else
            {
                item.Status = WriteOffStatus.PartiallyWrittenOff;
            }
        }
        
        return new WriteOffResult { Success = true, RemainingAmount = remainingAmount };
    }
}
```

#### 库存领域服务
```csharp
// 库存服务接口
public interface IInventoryService
{
    Task<InventoryCheckResult> CheckInventoryAsync(CheckInventoryRequest request);
    Task<InventoryUpdateResult> UpdateInventoryAsync(UpdateInventoryRequest request);
    Task<InventoryCalculationResult> CalculateCostAsync(CostCalculationRequest request);
    Task<InventoryAdjustmentResult> AdjustInventoryAsync(InventoryAdjustmentRequest request);
}

// 库存更新实现
public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ICostCalculationService _costCalculationService;
    
    public async Task<InventoryUpdateResult> UpdateInventoryAsync(UpdateInventoryRequest request)
    {
        var inventory = await _inventoryRepository.GetByProductAndLocationAsync(
            request.ProductDetailId, request.LocationId);
            
        if (inventory == null)
        {
            inventory = new Inventory
            {
                ProductDetailId = request.ProductDetailId,
                LocationId = request.LocationId,
                Quantity = 0
            };
        }
        
        // 检查负库存
        if (!request.AllowNegativeStock && inventory.Quantity + request.Quantity < 0)
        {
            return new InventoryUpdateResult 
            { 
                Success = false, 
                ErrorMessage = "库存不足" 
            };
        }
        
        // 更新数量
        inventory.Quantity += request.Quantity;
        inventory.LastModifiedTime = DateTime.Now;
        
        // 成本计算
        if (request.Quantity < 0) // 出库
        {
            await _costCalculationService.CalculateOutboundCostAsync(inventory, Math.Abs(request.Quantity));
        }
        else // 入库
        {
            await _costCalculationService.CalculateInboundCostAsync(inventory, request.Quantity, request.UnitCost);
        }
        
        await _inventoryRepository.UpdateAsync(inventory);
        
        return new InventoryUpdateResult { Success = true };
    }
}
```

### 2.3 审批流程模板化

```csharp
// 审批流程模板
public abstract class ApprovalWorkflowTemplate<T> where T : class
{
    protected readonly IUnitOfWorkManage _unitOfWorkManage;
    protected readonly ILogger _logger;
    protected readonly IBusinessRuleValidator _validator;
    
    protected ApprovalWorkflowTemplate(
        IUnitOfWorkManage unitOfWorkManage,
        ILogger logger,
        IBusinessRuleValidator validator)
    {
        _unitOfWorkManage = unitOfWorkManage;
        _logger = logger;
        _validator = validator;
    }
    
    public async Task<ApprovalResult> ProcessAsync(T entity)
    {
        try
        {
            _unitOfWorkManage.BeginTran();
            
            // 1. 验证实体
            var validationResult = await ValidateEntityAsync(entity);
            if (!validationResult.IsValid)
            {
                return new ApprovalResult { Success = false, ErrorMessage = validationResult.ErrorMessage };
            }
            
            // 2. 执行前置业务逻辑
            var preResult = await ExecutePreBusinessLogicAsync(entity);
            if (!preResult.Success)
            {
                return preResult;
            }
            
            // 3. 执行主要业务逻辑
            var mainResult = await ExecuteMainBusinessLogicAsync(entity);
            if (!mainResult.Success)
            {
                return mainResult;
            }
            
            // 4. 执行后置业务逻辑
            var postResult = await ExecutePostBusinessLogicAsync(entity);
            if (!postResult.Success)
            {
                return postResult;
            }
            
            // 5. 更新状态
            await UpdateStatusAsync(entity);
            
            // 6. 记录日志
            await LogApprovalAsync(entity);
            
            _unitOfWorkManage.CommitTran();
            
            return new ApprovalResult { Success = true };
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            _logger.LogError(ex, "审批失败");
            return new ApprovalResult { Success = false, ErrorMessage = ex.Message };
        }
    }
    
    protected abstract Task<ValidationResult> ValidateEntityAsync(T entity);
    protected abstract Task<ApprovalResult> ExecutePreBusinessLogicAsync(T entity);
    protected abstract Task<ApprovalResult> ExecuteMainBusinessLogicAsync(T entity);
    protected abstract Task<ApprovalResult> ExecutePostBusinessLogicAsync(T entity);
    protected abstract Task UpdateStatusAsync(T entity);
    protected abstract Task LogApprovalAsync(T entity);
}

// 具体实现示例
public class SaleOrderApprovalWorkflow : ApprovalWorkflowTemplate<SaleOrder>
{
    private readonly IInventoryService _inventoryService;
    private readonly IPrePaymentService _prePaymentService;
    
    public SaleOrderApprovalWorkflow(
        IUnitOfWorkManage unitOfWorkManage,
        ILogger logger,
        IBusinessRuleValidator validator,
        IInventoryService inventoryService,
        IPrePaymentService prePaymentService)
        : base(unitOfWorkManage, logger, validator)
    {
        _inventoryService = inventoryService;
        _prePaymentService = prePaymentService;
    }
    
    protected override async Task<ValidationResult> ValidateEntityAsync(SaleOrder entity)
    {
        // 验证销售订单
        if (entity.CustomerID == null)
        {
            return new ValidationResult { IsValid = false, ErrorMessage = "客户不能为空" };
        }
        
        if (entity.tb_SaleOrderDetails == null || !entity.tb_SaleOrderDetails.Any())
        {
            return new ValidationResult { IsValid = false, ErrorMessage = "订单明细不能为空" };
        }
        
        return new ValidationResult { IsValid = true };
    }
    
    protected override async Task<ApprovalResult> ExecuteMainBusinessLogicAsync(SaleOrder entity)
    {
        // 库存检查
        var inventoryCheckResult = await CheckInventoryAsync(entity);
        if (!inventoryCheckResult.Success)
        {
            return new ApprovalResult { Success = false, ErrorMessage = inventoryCheckResult.ErrorMessage };
        }
        
        // 更新库存
        var inventoryUpdateResult = await UpdateInventoryAsync(entity);
        if (!inventoryUpdateResult.Success)
        {
            return new ApprovalResult { Success = false, ErrorMessage = inventoryUpdateResult.ErrorMessage };
        }
        
        return new ApprovalResult { Success = true };
    }
    
    private async Task<InventoryCheckResult> CheckInventoryAsync(SaleOrder order)
    {
        var inventoryGroups = order.tb_SaleOrderDetails
            .GroupBy(d => new { d.ProdDetailID, d.Location_ID })
            .Select(g => new InventoryGroup
            {
                ProductDetailId = g.Key.ProdDetailID,
                LocationId = g.Key.Location_ID,
                RequiredQuantity = g.Sum(d => d.Quantity)
            })
            .ToList();
            
        foreach (var group in inventoryGroups)
        {
            var checkResult = await _inventoryService.CheckInventoryAsync(
                new CheckInventoryRequest
                {
                    ProductDetailId = group.ProductDetailId,
                    LocationId = group.LocationId,
                    RequiredQuantity = group.RequiredQuantity,
                    AllowNegativeStock = order.AllowNegativeStock
                });
                
            if (!checkResult.HasSufficientStock)
            {
                return new InventoryCheckResult
                {
                    Success = false,
                    ErrorMessage = $"商品【{group.ProductName}】库存不足"
                };
            }
        }
        
        return new InventoryCheckResult { Success = true };
    }
}
```

### 2.4 控制器重构示例

#### 销售订单控制器重构
```csharp
// 重构后的销售订单控制器
public class tb_SaleOrderController<T> : BaseController<T> where T : class
{
    private readonly IApprovalWorkflowTemplate<SaleOrder> _approvalWorkflow;
    private readonly IAntiApprovalWorkflowTemplate<SaleOrder> _antiApprovalWorkflow;
    private readonly ISaleOrderService _saleOrderService;
    
    public tb_SaleOrderController(
        IApprovalWorkflowTemplate<SaleOrder> approvalWorkflow,
        IAntiApprovalWorkflowTemplate<SaleOrder> antiApprovalWorkflow,
        ISaleOrderService saleOrderService)
    {
        _approvalWorkflow = approvalWorkflow;
        _antiApprovalWorkflow = antiApprovalWorkflow;
        _saleOrderService = saleOrderService;
    }
    
    public override async Task<ReturnResults<T>> ApprovalAsync(T entity)
    {
        var saleOrder = entity as SaleOrder;
        if (saleOrder == null)
        {
            return new ReturnResults<T> { ErrorMsg = "实体类型错误" };
        }
        
        var result = await _approvalWorkflow.ProcessAsync(saleOrder);
        
        return new ReturnResults<T>
        {
            Succeeded = result.Success,
            ErrorMsg = result.ErrorMessage,
            ReturnObject = result.Success ? entity : null
        };
    }
    
    public override async Task<ReturnResults<T>> AntiApprovalAsync(T entity)
    {
        var saleOrder = entity as SaleOrder;
        if (saleOrder == null)
        {
            return new ReturnResults<T> { ErrorMsg = "实体类型错误" };
        }
        
        var result = await _antiApprovalWorkflow.ProcessAsync(saleOrder);
        
        return new ReturnResults<T>
        {
            Succeeded = result.Success,
            ErrorMsg = result.ErrorMessage,
            ReturnObject = result.Success ? entity : null
        };
    }
    
    public async Task<ReturnResults<T>> AdvancedSave(T entity)
    {
        // 简单的保存逻辑，复杂的业务逻辑移到服务层
        var saleOrder = entity as SaleOrder;
        if (saleOrder == null)
        {
            return new ReturnResults<T> { ErrorMsg = "实体类型错误" };
        }
        
        var result = await _saleOrderService.SaveAsync(saleOrder);
        
        return new ReturnResults<T>
        {
            Succeeded = result.Success,
            ErrorMsg = result.ErrorMessage,
            ReturnObject = result.Success ? entity : null
        };
    }
}
```

## 3. 重构步骤

### 3.1 第一步：提取领域服务

1. **创建服务接口**
```csharp
// 在 RUINORERP.Business/Services 目录下创建服务接口
public interface IInventoryService
public interface IPaymentWriteOffService
public interface IStocktakeService
public interface IProductionDemandService
```

2. **实现服务类**
```csharp
// 在 RUINORERP.Business/Services/Implementations 目录下实现服务
public class InventoryService : IInventoryService
public class PaymentWriteOffService : IPaymentWriteOffService
public class StocktakeService : IStocktakeService
public class ProductionDemandService : IProductionDemandService
```

### 3.2 第二步：创建审批流程模板

1. **创建基础模板类**
```csharp
// 在 RUINORERP.Business/Workflows 目录下创建模板
public abstract class ApprovalWorkflowTemplate<T>
public abstract class AntiApprovalWorkflowTemplate<T>
```

2. **实现具体审批流程**
```csharp
// 为每个主要业务实体创建审批流程
public class SaleOrderApprovalWorkflow : ApprovalWorkflowTemplate<SaleOrder>
public class PurchaseOrderApprovalWorkflow : ApprovalWorkflowTemplate<PurchaseOrder>
public class PaymentRecordApprovalWorkflow : ApprovalWorkflowTemplate<PaymentRecord>
```

### 3.3 第三步：重构控制器

1. **修改控制器构造函数**
```csharp
// 注入服务而不是直接依赖数据库
public tb_SaleOrderController(
    IApprovalWorkflowTemplate<SaleOrder> approvalWorkflow,
    IAntiApprovalWorkflowTemplate<SaleOrder> antiApprovalWorkflow,
    ISaleOrderService saleOrderService)
```

2. **简化控制器方法**
```csharp
// 将业务逻辑移到服务层
public override async Task<ReturnResults<T>> ApprovalAsync(T entity)
{
    var result = await _approvalWorkflow.ProcessAsync(entity as SaleOrder);
    return new ReturnResults<T> { Succeeded = result.Success, ErrorMsg = result.ErrorMessage };
}
```

## 4. 重构收益

### 4.1 代码质量提升
- **单一职责原则**：每个类只负责一个职责
- **开闭原则**：易于扩展，不易修改
- **依赖倒置原则**：依赖于抽象而不是具体实现

### 4.2 可维护性提升
- **模块化**：业务逻辑模块化，便于理解和维护
- **可测试性**：服务层易于单元测试
- **可重用性**：服务可以在不同控制器中重用

### 4.3 性能提升
- **查询优化**：批量查询替代N+1查询
- **缓存策略**：合理使用缓存
- **事务优化**：精确控制事务范围

### 4.4 团队协作提升
- **分工明确**：不同开发人员可以负责不同服务
- **代码规范**：统一的代码结构和命名规范
- **文档完善**：服务接口文档化

## 5. 重构风险与缓解

### 5.1 主要风险
1. **功能回归**：重构可能引入新的bug
2. **性能下降**：不当的重构可能导致性能问题
3. **兼容性问题**：接口变更可能影响外部系统
4. **开发周期延长**：重构需要额外的时间投入

### 5.2 缓解措施
1. **充分测试**：建立完善的单元测试和集成测试
2. **渐进式重构**：分阶段进行，每次只重构一小部分
3. **代码审查**：重构代码必须经过严格的代码审查
4. **备份方案**：保留重构前的代码备份
5. **性能监控**：重构后进行性能测试和监控

## 6. 实施建议

### 6.1 短期目标 (1-2个月)
- 完成财务收付款单控制器的重构
- 建立基础的服务层架构
- 完善单元测试覆盖

### 6.2 中期目标 (3-6个月)
- 完成主要业务控制器的重构
- 建立完整的领域服务层
- 优化数据库查询性能

### 6.3 长期目标 (6-12个月)
- 完成所有控制器的重构
- 建立完善的监控和日志系统
- 实现自动化部署和回滚

通过系统性的重构，可以显著提升代码质量、系统性能和团队协作效率，为系统的长期发展奠定坚实基础。