-- =====================================================
-- 采购与销售业务系统数据库索引优化方案
-- 目标数据库：Microsoft SQL Server 2016+
-- 
-- 作者：ERP系统优化分析
-- 创建日期：2026-03-19
-- 
-- 说明：
-- 本脚本针对采购订单、采购入库、销售订单、销售出库等核心业务表
-- 创建全面的非聚集索引，以优化查询性能并预防死锁
-- 
-- SQL Server 2016 特性说明：
-- 1. 更好的并行索引创建
-- 2. 改进的统计信息管理
-- 3. 支持更多的在线索引选项
-- 4. 更好的锁管理
-- 
-- 索引设计原则：
-- 1. 覆盖高频查询字段组合
-- 2. 优化表间关联查询
-- 3. 支持业务审核流程中的状态查询
-- 4. 减少锁竞争和死锁风险
-- 5. 使用包含列减少键值大小，提高性能
-- =====================================================

SET NOCOUNT ON;
GO

PRINT '=====================================================';
PRINT '开始创建业务表索引 (SQL Server 2016+)...';
PRINT '=====================================================';
GO

-- 启用压缩功能（SQL Server 2016+）
ALTER DATABASE CURRENT SET COMPATIBILITY_LEVEL = 130;
GO

-- =====================================================
-- 第一部分：产品相关表索引
-- =====================================================

-- =====================================================
-- 表：tb_Prod (货品基本信息表)
-- 功能：存储货品的基础信息，是整个系统的产品基础
-- 主要字段：ProdBaseID(主键), ProductNo(品号), CNName(品名), Category_ID(类别), Unit_ID(单位)
-- 业务场景：产品选择、产品分类查询、按品号/名称搜索
-- 
-- 查询模式分析：
-- 1. 产品选择器：按ProductNo精确查询，模糊匹配
-- 2. 分类浏览：按Category_ID范围查询
-- 3. 搜索功能：按CNName、ShortCode模糊查询
-- 
-- 索引策略：
-- 1. ProductNo唯一索引 - 产品选择器高频使用
-- 2. Category_ID索引 - 支持分类树展开
-- 3. 组合索引 - 启用状态+类别查询
-- =====================================================

PRINT '创建 tb_Prod 表索引...';

-- 索引1：按品号查询（产品选择器最常用）- SQL2016优化：使用键压缩
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Prod_ProductNo' AND object_id = OBJECT_ID('tb_Prod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Prod_ProductNo 
    ON tb_Prod(ProductNo)
    INCLUDE (CNName, Model, Specifications, Category_ID, Unit_ID, Is_enabled, Is_available)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE,
        SORT_IN_TEMPDB = ON
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Prod_ProductNo: 按品号查询产品';
END
GO

-- 索引2：按类别查询产品（分类浏览）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Prod_CategoryID' AND object_id = OBJECT_ID('tb_Prod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Prod_CategoryID 
    ON tb_Prod(Category_ID)
    INCLUDE (ProdBaseID, ProductNo, CNName, Is_enabled, Is_available)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Prod_CategoryID: 按产品类别查询';
END
GO

-- 索引3：按品名模糊查询
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Prod_CNName' AND object_id = OBJECT_ID('tb_Prod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Prod_CNName 
    ON tb_Prod(CNName)
    INCLUDE (ProdBaseID, ProductNo, Category_ID)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Prod_CNName: 按品名模糊查询';
END
GO

-- 索引4：按助记码查询（快速检索）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Prod_ShortCode' AND object_id = OBJECT_ID('tb_Prod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Prod_ShortCode 
    ON tb_Prod(ShortCode)
    INCLUDE (ProdBaseID, ProductNo, CNName)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Prod_ShortCode: 按助记码快速检索';
END
GO

-- 索引5：组合索引：可用产品按类别查询
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Prod_CategoryID_Enabled' AND object_id = OBJECT_ID('tb_Prod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Prod_CategoryID_Enabled 
    ON tb_Prod(Category_ID, Is_enabled, Is_available)
    INCLUDE (ProdBaseID, ProductNo, CNName)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Prod_CategoryID_Enabled: 可用产品按类别查询';
END
GO

-- 索引6：按厂商查询（供应商产品）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Prod_CustomerVendorID' AND object_id = OBJECT_ID('tb_Prod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Prod_CustomerVendorID 
    ON tb_Prod(CustomerVendor_ID)
    INCLUDE (ProdBaseID, ProductNo, CNName)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Prod_CustomerVendorID: 按厂商查询产品';
END
GO


-- =====================================================
-- 表：tb_ProdDetail (货品详情表)
-- 功能：存储货品的详细规格信息，关联产品与仓库
-- 主要字段：ProdDetailID(主键), ProdBaseID(产品ID), Location_ID(仓库ID)
-- 业务场景：库存查询、产品入库出库、按仓库查产品
-- 
-- 索引策略：
-- 1. ProdBaseID索引 - 按产品查所有仓库规格
-- 2. Location_ID索引 - 按仓库查所有产品
-- 3. 唯一组合索引 - 产品仓库唯一性
-- =====================================================

PRINT '创建 tb_ProdDetail 表索引...';

-- 索引1：按产品ID查询所有规格（关联产品与仓库）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_ProdDetail_ProdBaseID' AND object_id = OBJECT_ID('tb_ProdDetail'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_ProdDetail_ProdBaseID 
    ON tb_ProdDetail(ProdBaseID)
    INCLUDE (ProdDetailID, Location_ID)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_ProdDetail_ProdBaseID: 按产品ID查询规格';
END
GO

-- 索引2：按仓库查询产品（仓库库存视图）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_ProdDetail_LocationID' AND object_id = OBJECT_ID('tb_ProdDetail'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_ProdDetail_LocationID 
    ON tb_ProdDetail(Location_ID)
    INCLUDE (ProdDetailID, ProdBaseID)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_ProdDetail_LocationID: 按仓库查询产品';
END
GO

-- 索引3：组合索引：产品与仓库唯一性检查（防止重复创建）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_ProdDetail_ProdBaseID_LocationID' AND object_id = OBJECT_ID('tb_ProdDetail'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_tb_ProdDetail_ProdBaseID_LocationID 
    ON tb_ProdDetail(ProdBaseID, Location_ID)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_ProdDetail_ProdBaseID_LocationID: 产品仓库唯一性约束';
END
GO


-- =====================================================
-- 第二部分：库存相关表索引（死锁预防重点）
-- =====================================================

-- =====================================================
-- 表：tb_Inventory (库存表)
-- 功能：记录各仓库各产品的实时库存数量
-- 主要字段：Inventory_ID(主键), ProdDetailID, Location_ID, Quantity(库存量)
-- 业务场景：库存查询、库存锁定、库存预警、订单审核库存检查
-- 
-- 死锁风险点分析：
-- 1. 销售订单审核时更新Sale_Qty（拟销售量）- 行锁
-- 2. 销售出库时扣减Quantity - 行锁
-- 3. 采购入库时增加Quantity - 行锁
-- 4. 多个并发操作同一产品不同仓库 - 锁升级
-- 
-- 索引策略：
-- 1. ProdDetailID + Location_ID组合索引 - 库存操作核心索引
-- 2. 覆盖所有更新字段 - 避免回表查找，减少锁持有时间
-- 3. 使用较低的FILLFACTOR - 减少页面分裂
-- =====================================================

PRINT '创建 tb_Inventory 表索引...';

-- 索引1：按产品详情查询库存（最常用的库存查询）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_ProdDetailID' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_ProdDetailID 
    ON tb_Inventory(ProdDetailID)
    INCLUDE (Inventory_ID, Quantity, Sale_Qty, Location_ID, Inv_Cost)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_ProdDetailID: 按产品详情查询库存';
END
GO

-- 索引2：按仓库查询库存（仓库库存报表）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_LocationID' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_LocationID 
    ON tb_Inventory(Location_ID)
    INCLUDE (Inventory_ID, ProdDetailID, Quantity, Sale_Qty)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_LocationID: 按仓库查询库存';
END
GO

-- 索引3：【死锁预防核心索引】产品详情+仓库组合索引
-- 死锁优化说明：
-- 订单审核和出库操作都需要锁定库存记录，通过覆盖所有必要字段
-- 减少锁等待时间和死锁概率
-- 策略：统一锁获取顺序 ProdDetailID -> Location_ID
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_ProdDetailID_LocationID' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_ProdDetailID_LocationID 
    ON tb_Inventory(ProdDetailID, Location_ID)
    INCLUDE (Inventory_ID, Quantity, Sale_Qty, NotOutQty, Inv_Cost, Inv_AdvCost)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE,
        ROW_LOCK = ON,
        PAGE_LOCK = OFF  -- SQL2016: 强制行锁，减少锁粒度
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_ProdDetailID_LocationID: 产品+仓库组合查询(防死锁优化)';
END
GO

-- 索引4：低库存预警查询
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_AlertQuantity' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_AlertQuantity 
    ON tb_Inventory(Location_ID, Alert_Quantity)
    INCLUDE (ProdDetailID, Quantity)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_AlertQuantity: 低库存预警查询';
END
GO

-- 索引5：库存可用性检查（Quantity - Sale_Qty > 0）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_AvailableQty' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_AvailableQty 
    ON tb_Inventory(ProdDetailID, Location_ID)
    INCLUDE (Quantity, Sale_Qty)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_AvailableQty: 可用库存检查';
END
GO

-- 索引6：货架索引（支持按货架查询）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_RackID' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_RackID 
    ON tb_Inventory(Rack_ID)
    INCLUDE (ProdDetailID, Location_ID, Quantity)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_RackID: 按货架查询';
END
GO


-- =====================================================
-- 表：tb_InventoryTransaction (库存流水表)
-- 功能：记录所有库存变动历史，用于追溯和对账
-- 主要字段：TranID(主键), ProdDetailID, Location_ID, BizType, ReferenceId, QuantityChange
-- 业务场景：库存追溯、成本计算、收发存报表、审计追踪
-- 
-- 死锁风险点：批量插入流水时与库存表操作的锁竞争
-- 
-- 索引策略：
-- 1. 按产品+时间索引 - 库存追溯
-- 2. 按单据索引 - 单据反审核
-- 3. 覆盖索引 - 避免回表
-- =====================================================

PRINT '创建 tb_InventoryTransaction 表索引...';

-- 索引1：按产品详情查询流水（库存追溯）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_ProdDetailID' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_ProdDetailID 
    ON tb_InventoryTransaction(ProdDetailID, TransactionTime DESC)
    INCLUDE (TranID, Location_ID, BizType, QuantityChange, AfterQuantity, UnitCost)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE,
        SORT_IN_TEMPDB = ON
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_ProdDetailID: 按产品详情查询流水';
END
GO

-- 索引2：按业务单据查询流水（单据追溯）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_ReferenceId' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_ReferenceId 
    ON tb_InventoryTransaction(ReferenceId, BizType)
    INCLUDE (TranID, ProdDetailID, Location_ID, QuantityChange, TransactionTime)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_ReferenceId: 按业务单据查询流水';
END
GO

-- 索引3：按仓库查询流水（收发存报表）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_LocationID' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_LocationID 
    ON tb_InventoryTransaction(Location_ID, TransactionTime DESC)
    INCLUDE (TranID, ProdDetailID, BizType, QuantityChange)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_LocationID: 按仓库查询流水';
END
GO

-- 索引4：按业务类型查询流水（分类统计）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_BizType' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_BizType 
    ON tb_InventoryTransaction(BizType, TransactionTime DESC)
    INCLUDE (TranID, ProdDetailID, Location_ID, QuantityChange, ReferenceId)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_BizType: 按业务类型查询流水';
END
GO

-- 索引5：组合索引：产品+仓库+时间（收发存报表常用）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_ProdLocTime' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_ProdLocTime 
    ON tb_InventoryTransaction(ProdDetailID, Location_ID, TransactionTime DESC)
    INCLUDE (TranID, BizType, QuantityChange, AfterQuantity)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_ProdLocTime: 产品仓库时间组合查询';
END
GO

-- 索引6：按时间范围查询流水（报表统计）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_TransactionTime' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_TransactionTime 
    ON tb_InventoryTransaction(TransactionTime DESC)
    INCLUDE (TranID, ProdDetailID, Location_ID, BizType, QuantityChange)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_TransactionTime: 按时间范围查询';
END
GO

-- 索引7：防止重复流水的去重检查
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_RefDetailBiz' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_RefDetailBiz 
    ON tb_InventoryTransaction(ReferenceId, ProdDetailID, BizType)
    INCLUDE (TranID)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_RefDetailBiz: 重复流水检查';
END
GO


-- =====================================================
-- 表：tb_InventorySnapshot (库存快照表)
-- 功能：存储库存定期快照，用于报表和历史分析
-- 主要字段：SnapshotID(主键), ProdDetailID, Location_ID, SnapshotTime
-- 业务场景：月度库存报表、库存分析、历史数据查询
-- =====================================================

PRINT '创建 tb_InventorySnapshot 表索引...';

-- 索引1：按产品详情查询快照
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventorySnapshot_ProdDetailID' AND object_id = OBJECT_ID('tb_InventorySnapshot'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventorySnapshot_ProdDetailID 
    ON tb_InventorySnapshot(ProdDetailID, SnapshotTime DESC)
    INCLUDE (SnapshotID, Location_ID, Quantity, Sale_Qty, Inv_Cost)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventorySnapshot_ProdDetailID: 按产品详情查询快照';
END
GO

-- 索引2：按快照时间查询（报表常用）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventorySnapshot_SnapshotTime' AND object_id = OBJECT_ID('tb_InventorySnapshot'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventorySnapshot_SnapshotTime 
    ON tb_InventorySnapshot(SnapshotTime DESC)
    INCLUDE (SnapshotID, ProdDetailID, Location_ID, Quantity)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventorySnapshot_SnapshotTime: 按快照时间查询';
END
GO

-- 索引3：组合索引：仓库+快照时间
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventorySnapshot_LocationTime' AND object_id = OBJECT_ID('tb_InventorySnapshot'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventorySnapshot_LocationTime 
    ON tb_InventorySnapshot(Location_ID, SnapshotTime DESC)
    INCLUDE (SnapshotID, ProdDetailID, Quantity, Inv_Cost)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventorySnapshot_LocationTime: 仓库+快照时间组合';
END
GO


-- =====================================================
-- 第三部分：销售相关表索引
-- =====================================================

-- =====================================================
-- 表：tb_SaleOrder (销售订单表)
-- 功能：记录销售订单基本信息及明细
-- 主要字段：SOrder_ID(主键), SOrderNo(订单编号), CustomerVendor_ID(客户), Employee_ID(业务员)
-- 业务场景：订单管理、订单审核、订单查询、按客户查订单
-- 
-- 死锁风险点：
-- 1. 订单审核时更新库存(Sale_Qty)
-- 2. 预收款单生成
-- 3. 并发审核同一客户订单
-- 
-- 索引策略：
-- 1. 订单编号唯一索引 - 单据操作高频使用
-- 2. 客户+状态索引 - 客户对账、订单查询
-- 3. 状态索引 - 审核列表
-- =====================================================

PRINT '创建 tb_SaleOrder 表索引...';

-- 索引1：按订单编号查询（单据操作最常用）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_SOrderNo' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_SOrderNo 
    ON tb_SaleOrder(SOrderNo)
    INCLUDE (SOrder_ID, CustomerVendor_ID, TotalAmount, DataStatus, ApprovalStatus, PayStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_SOrderNo: 按订单编号查询';
END
GO

-- 索引2：按客户查询订单（客户订单查询）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_CustomerVendorID' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_CustomerVendorID 
    ON tb_SaleOrder(CustomerVendor_ID, Created_at DESC)
    INCLUDE (SOrder_ID, SOrderNo, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_CustomerVendorID: 按客户查询订单';
END
GO

-- 索引3：按业务员查询订单（业绩统计）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_EmployeeID' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_EmployeeID 
    ON tb_SaleOrder(Employee_ID, Created_at DESC)
    INCLUDE (SOrder_ID, SOrderNo, CustomerVendor_ID, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_EmployeeID: 按业务员查询订单';
END
GO

-- 索引4：按订单状态查询（审核列表）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_DataStatus' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_DataStatus 
    ON tb_SaleOrder(DataStatus, ApprovalStatus)
    INCLUDE (SOrder_ID, SOrderNo, CustomerVendor_ID, TotalAmount, Created_at)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_DataStatus: 按状态查询待审核订单';
END
GO

-- 索引5：按付款状态查询
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_PayStatus' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_PayStatus 
    ON tb_SaleOrder(PayStatus, DataStatus)
    INCLUDE (SOrder_ID, SOrderNo, CustomerVendor_ID, TotalAmount)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_PayStatus: 按付款状态查询';
END
GO

-- 索引6：按订单日期查询（报表统计）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_SaleDate' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_SaleDate 
    ON tb_SaleOrder(SaleDate DESC)
    INCLUDE (SOrder_ID, SOrderNo, CustomerVendor_ID, TotalAmount, TotalCost, DataStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_SaleDate: 按订单日期统计查询';
END
GO

-- 索引7：组合索引：客户+状态（客户对账常用）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_CustomerStatus' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_CustomerStatus 
    ON tb_SaleOrder(CustomerVendor_ID, DataStatus, ApprovalStatus)
    INCLUDE (SOrder_ID, SOrderNo, TotalAmount, SaleDate)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_CustomerStatus: 客户+状态组合查询';
END
GO

-- 索引8：按预交日期查询（订单跟踪）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_PreDeliveryDate' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_PreDeliveryDate 
    ON tb_SaleOrder(PreDeliveryDate, DataStatus)
    INCLUDE (SOrder_ID, SOrderNo, CustomerVendor_ID)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_PreDeliveryDate: 按预交日期查询';
END
GO


-- =====================================================
-- 表：tb_SaleOut (销售出库表)
-- 功能：记录销售出库单信息
-- 主要字段：SaleOut_ID(主键), SaleOutNo(出库单号), SOrder_ID(订单ID), CustomerVendor_ID
-- 业务场景：出库审核、库存扣减、应收款生成
-- 
-- 死锁风险点：
-- 1. 出库审核时扣减库存(Quantity)
-- 2. 记录库存流水(与库存表的锁竞争)
-- 3. 生成应收款单
-- =====================================================

PRINT '创建 tb_SaleOut 表索引...';

-- 索引1：按出库单号查询
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOut_SaleOutNo' AND object_id = OBJECT_ID('tb_SaleOut'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOut_SaleOutNo 
    ON tb_SaleOut(SaleOutNo)
    INCLUDE (SaleOut_ID, SOrder_ID, CustomerVendor_ID, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOut_SaleOutNo: 按出库单号查询';
END
GO

-- 索引2：按订单查询出库单（订单追溯）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOut_SOrderID' AND object_id = OBJECT_ID('tb_SaleOut'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOut_SOrderID 
    ON tb_SaleOut(SOrder_ID)
    INCLUDE (SaleOut_ID, SaleOutNo, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOut_SOrderID: 按订单查询出库单';
END
GO

-- 索引3：按客户查询出库单
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOut_CustomerVendorID' AND object_id = OBJECT_ID('tb_SaleOut'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOut_CustomerVendorID 
    ON tb_SaleOut(CustomerVendor_ID, Created_at DESC)
    INCLUDE (SaleOut_ID, SaleOutNo, TotalAmount, DataStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOut_CustomerVendorID: 按客户查询出库单';
END
GO

-- 索引4：按状态查询出库单（审核列表）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOut_DataStatus' AND object_id = OBJECT_ID('tb_SaleOut'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOut_DataStatus 
    ON tb_SaleOut(DataStatus, ApprovalStatus)
    INCLUDE (SaleOut_ID, SaleOutNo, CustomerVendor_ID, TotalAmount, Created_at)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOut_DataStatus: 按状态查询待审核出库单';
END
GO

-- 索引5：按出库日期查询（报表统计）
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOut_SaleOutDate' AND object_id = OBJECT_ID('tb_SaleOut'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOut_SaleOutDate 
    ON tb_SaleOut(SaleOutDate DESC)
    INCLUDE (SaleOut_ID, SaleOutNo, CustomerVendor_ID, TotalAmount, TotalCost, DataStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOut_SaleOutDate: 按出库日期统计查询';
END
GO

-- 索引6：按仓库查询出库单
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOut_LocationID' AND object_id = OBJECT_ID('tb_SaleOut'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOut_LocationID 
    ON tb_SaleOut(Location_ID)
    INCLUDE (SaleOut_ID, SaleOutNo, CustomerVendor_ID, TotalAmount)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOut_LocationID: 按仓库查询出库单';
END
GO


-- =====================================================
-- 第四部分：采购相关表索引
-- =====================================================

PRINT '创建 tb_PurOrder 表索引...';

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_PurOrder_PurOrderNo' AND object_id = OBJECT_ID('tb_PurOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_PurOrder_PurOrderNo 
    ON tb_PurOrder(PurOrderNo)
    INCLUDE (PurOrder_ID, CustomerVendor_ID, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_PurOrder_PurOrderNo: 按订单编号查询';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_PurOrder_CustomerVendorID' AND object_id = OBJECT_ID('tb_PurOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_PurOrder_CustomerVendorID 
    ON tb_PurOrder(CustomerVendor_ID, Created_at DESC)
    INCLUDE (PurOrder_ID, PurOrderNo, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_PurOrder_CustomerVendorID: 按供应商查询订单';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_PurOrder_DataStatus' AND object_id = OBJECT_ID('tb_PurOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_PurOrder_DataStatus 
    ON tb_PurOrder(DataStatus, ApprovalStatus)
    INCLUDE (PurOrder_ID, PurOrderNo, CustomerVendor_ID, TotalAmount, Created_at)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_PurOrder_DataStatus: 按状态查询待审核订单';
END
GO


PRINT '创建 tb_PurEntry 表索引...';

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_PurEntry_PurEntryNo' AND object_id = OBJECT_ID('tb_PurEntry'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_PurEntry_PurEntryNo 
    ON tb_PurEntry(PurEntryNo)
    INCLUDE (PurEntry_ID, PurOrder_ID, CustomerVendor_ID, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_PurEntry_PurEntryNo: 按入库单号查询';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_PurEntry_PurOrderID' AND object_id = OBJECT_ID('tb_PurEntry'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_PurEntry_PurOrderID 
    ON tb_PurEntry(PurOrder_ID)
    INCLUDE (PurEntry_ID, PurEntryNo, TotalAmount, DataStatus, ApprovalStatus)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_PurEntry_PurOrderID: 按订单查询入库单';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_PurEntry_DataStatus' AND object_id = OBJECT_ID('tb_PurEntry'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_PurEntry_DataStatus 
    ON tb_PurEntry(DataStatus, ApprovalStatus)
    INCLUDE (PurEntry_ID, PurEntryNo, CustomerVendor_ID, TotalAmount, Created_at)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_PurEntry_DataStatus: 按状态查询待审核入库单';
END
GO


-- =====================================================
-- 第五部分：死锁预防专项索引（SQL Server 2016优化）
-- =====================================================

PRINT '';
PRINT '=====================================================';
PRINT '创建死锁预防专项索引 (SQL Server 2016+)...';
PRINT '=====================================================';

-- 库存表死锁预防：确保库存更新操作能快速定位
-- 策略：覆盖所有库存更新需要的字段，避免回表查找，减少锁持有时间
-- SQL2016优化：强制行锁，减少锁升级

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_Inventory_Deadlock_Prevention' AND object_id = OBJECT_ID('tb_Inventory'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_Inventory_Deadlock_Prevention 
    ON tb_Inventory(ProdDetailID, Location_ID)
    INCLUDE (Quantity, Sale_Qty, NotOutQty, MakingQty, Inv_Cost, Inv_AdvCost, CostFIFO, CostMonthlyWA, CostMovingWA)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE,
        ALLOW_PAGE_LOCKS = OFF,  -- SQL2016: 禁用页锁，强制行锁
        ALLOW_ROW_LOCKS = ON
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_Inventory_Deadlock_Prevention: 库存操作防死锁覆盖索引';
END
GO

-- 库存流水表死锁预防：批量插入时快速检查重复
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_InventoryTransaction_Deadlock_Prevention' AND object_id = OBJECT_ID('tb_InventoryTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_InventoryTransaction_Deadlock_Prevention 
    ON tb_InventoryTransaction(ReferenceId, BizType, ProdDetailID, Location_ID)
    INCLUDE (TranID, QuantityChange)
    WITH (
        FILLFACTOR = 80, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE,
        ALLOW_PAGE_LOCKS = OFF,
        ALLOW_ROW_LOCKS = ON
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_InventoryTransaction_Deadlock_Prevention: 流水防死锁索引';
END
GO

-- 销售订单死锁预防：订单审核时快速查询客户信息
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_tb_SaleOrder_Deadlock_Prevention' AND object_id = OBJECT_ID('tb_SaleOrder'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tb_SaleOrder_Deadlock_Prevention 
    ON tb_SaleOrder(CustomerVendor_ID, DataStatus)
    INCLUDE (SOrder_ID, SOrderNo, Paytype_ID, PayStatus, TotalAmount, Employee_ID)
    WITH (
        FILLFACTOR = 90, 
        ONLINE = ON,
        DATA_COMPRESSION = PAGE
    )
    ON [PRIMARY];
    
    PRINT '  - IX_tb_SaleOrder_Deadlock_Prevention: 订单审核防死锁索引';
END
GO


-- =====================================================
-- 第六部分：SQL Server 2016 统计信息优化
-- =====================================================

PRINT '';
PRINT '=====================================================';
PRINT '更新统计信息 (SQL Server 2016+)...';
PRINT '=====================================================';

-- 启用自动统计信息更新（异步）
ALTER DATABASE CURRENT SET AUTO_UPDATE_STATISTICS_ASYNC ON;
GO

-- 为关键表更新统计信息（带全扫描）
UPDATE STATISTICS tb_Inventory WITH FULLSCAN;
UPDATE STATISTICS tb_InventoryTransaction WITH FULLSCAN;
UPDATE STATISTICS tb_SaleOrder WITH FULLSCAN;
UPDATE STATISTICS tb_SaleOut WITH FULLSCAN;
UPDATE STATISTICS tb_Prod WITH FULLSCAN;
UPDATE STATISTICS tb_ProdDetail WITH FULLSCAN;

PRINT '  - 统计信息更新完成';
GO


-- =====================================================
-- 第七部分：监控索引使用（SQL Server 2016+）
-- =====================================================

PRINT '';
PRINT '=====================================================';
PRINT '创建索引使用监控视图...';
PRINT '=====================================================';

IF NOT EXISTS (SELECT 1 FROM sys.views WHERE name = 'vw_IndexUsageStats' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    EXEC('
    CREATE VIEW dbo.vw_IndexUsageStats
    AS
    SELECT 
        OBJECT_NAME(s.object_id) AS TableName,
        i.name AS IndexName,
        i.type_desc AS IndexType,
        s.user_seeks,
        s.user_scans,
        s.user_lookups,
        s.user_updates,
        s.last_user_seek,
        s.last_user_scan,
        s.last_user_update,
        s.system_seeks,
        s.system_scans,
        s.system_updates
    FROM sys.dm_db_index_usage_stats s
    INNER JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
    WHERE OBJECTPROPERTY(s.object_id, ''IsUserTable'') = 1
    ');
    
    PRINT '  - vw_IndexUsageStats: 索引使用监控视图创建完成';
END
GO


-- =====================================================
-- 索引创建完成统计
-- =====================================================

PRINT '';
PRINT '=====================================================';
PRINT '索引创建完成！';
PRINT '=====================================================';

-- 显示创建的索引统计
SELECT 
    t.name AS TableName,
    COUNT(i.index_id) AS IndexCount,
    SUM(CASE WHEN i.type_desc = 'NONCLUSTERED' THEN 1 ELSE 0 END) AS NonClusteredCount
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
WHERE i.type > 0
    AND t.name IN (
        'tb_Prod', 'tb_ProdDetail', 'tb_Inventory', 'tb_InventoryTransaction', 'tb_InventorySnapshot',
        'tb_SaleOrder', 'tb_SaleOut', 'tb_PurOrder', 'tb_PurEntry'
    )
GROUP BY t.name
ORDER BY TableName;

PRINT '';
PRINT '=====================================================';
PRINT 'SQL Server 2016+ 优化说明：';
PRINT '=====================================================';
PRINT '1. 索引压缩：所有索引启用PAGE压缩，减少IO';
PRINT '2. 在线索引：所有索引创建使用ONLINE选项';
PRINT '3. 行锁优化：死锁预防索引禁用页锁，强制行锁';
PRINT '4. 统计信息：启用异步统计信息更新';
PRINT '5. 监控视图：创建索引使用监控视图';
PRINT '';
PRINT '后续维护建议：';
PRINT '1. 定期执行 UPDATE STATISTICS 更新统计信息';
PRINT '2. 使用 sys.dm_db_index_usage_stats 监控索引使用情况';
PRINT '3. 根据实际查询模式调整索引';
PRINT '4. 关注死锁日志，优化锁获取顺序';
PRINT '=====================================================';
GO
