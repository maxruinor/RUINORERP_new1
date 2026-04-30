# 客户/供应商关系管理设计方案

## 一、问题分析

根据业务需求分析，当前系统存在以下两个核心问题需要解决：

### 1.1 问题一：单一业务员限制

**现状**：当前 `tb_CustomerVendor` 表中通过 `Employee_ID` 字段记录唯一的业务员责任人。

**问题**：
- 大型客户公司可能有多个对接人（销售、采购、财务等不同角色）
- 一个供应商可能需要多个采购人员负责不同产品线
- 无法灵活分配多个业务员共同维护一个客户关系

### 1.2 问题二：集团化管理需求

**现状**：每个客户/供应商都是独立记录，没有层级关联关系。

**问题**：
- 集团公司旗下可能有多个子公司独立运营
- 对账时需要分别对每个子公司进行对账，无法统一管理
- 无法体现企业间的从属关系和组织架构

---

## 二、设计方案

### 2.1 方案一：客户/供应商与业务员一对多关系

**设计思路**：新增中间关系表，实现一个往来单位对应多个业务员的需求。

#### 2.1.1 新增表结构设计

**表名**：`tb_CustomerVendor_Employee_Relation`

| 字段名 | 类型 | 说明 | 约束 |
| :--- | :--- | :--- | :--- |
| `Relation_ID` | BIGINT | 主键，自增 | PRIMARY KEY, AUTO_INCREMENT |
| `CustomerVendor_ID` | BIGINT | 往来单位ID | FOREIGN KEY → tb_CustomerVendor |
| `Employee_ID` | BIGINT | 员工ID | FOREIGN KEY → tb_Employee |
| `RelationType` | INT | 关系类型（销售/采购/财务/负责人等） | NOT NULL |
| `IsPrimary` | BIT | 是否为主责任人 | DEFAULT FALSE |
| `StartDate` | DATETIME | 生效日期 | NULL |
| `EndDate` | DATETIME | 失效日期 | NULL |
| `Notes` | VARCHAR(255) | 备注说明 | NULL |
| `Created_at` | DATETIME | 创建时间 | NULL |
| `Created_by` | BIGINT | 创建人 | NULL |

#### 2.1.2 关系类型枚举值

| 枚举值 | 说明 |
| :--- | :--- |
| 1 | 销售负责人 |
| 2 | 采购负责人 |
| 3 | 财务对接人 |
| 4 | 售后负责人 |
| 5 | 技术对接人 |
| 6 | 其他 |

#### 2.1.3 业务规则

1. **主责任人规则**：一个往来单位只能有一个主责任人（IsPrimary = TRUE）
2. **生效时间范围**：支持设置业务员的服务时间段
3. **兼容性**：保留原 `tb_CustomerVendor.Employee_ID` 字段作为默认业务员，新表数据作为扩展
4. **查询优先级**：查询业务员时优先从关系表获取，若无数据则使用默认字段

---

### 2.2 方案二：客户/供应商集团化从属关系

**设计思路**：通过自关联方式实现往来单位之间的层级关系。

#### 2.2.1 修改 `tb_CustomerVendor` 表

**新增字段**：

| 字段名 | 类型 | 说明 | 约束 |
| :--- | :--- | :--- | :--- |
| `Parent_CustomerVendor_ID` | BIGINT | 上级往来单位ID | FOREIGN KEY → tb_CustomerVendor.CustomerVendor_ID, NULLABLE |
| `Level` | INT | 层级（1=集团, 2=子公司, 3=孙公司...） | DEFAULT 1, NOT NULL |

**默认值策略**：

| 字段 | 默认值 | 原因说明 |
| :--- | :--- | :--- |
| `Parent_CustomerVendor_ID` | NULL | 原有数据无上级关系，默认为NULL表示顶级节点（集团） |
| `Level` | 1 | 默认所有往来单位为集团层级，后续可根据实际关系调整 |

**对现有数据的影响**：
- **无破坏性**：新增字段不影响原有数据结构
- **自动适配**：现有记录会自动获得默认值（Level=1，Parent_ID=NULL）
- **向后兼容**：现有业务代码无需修改即可正常运行

#### 2.2.2 业务规则

1. **层级限制**：建议限制最大层级为3-4层
2. **自引用约束**：不允许循环引用（如 A→B→A）
3. **根节点标识**：`Parent_CustomerVendor_ID IS NULL` 表示为集团总部
4. **数据完整性**：删除集团时需级联处理子公司或设置其 Parent_ID 为 NULL

#### 2.2.3 对账场景处理

**需求**：选择集团公司对账时，自动包含其所有子公司数据

**实现思路**：
- 查询时通过递归CTE获取所有下级单位ID列表
- 将列表作为条件进行数据汇总查询
- 支持"仅当前单位"和"包含子公司"两种查询模式

---

## 三、方案对比

### 3.1 方案一：业务员关系表

| 维度 | 评估 |
| :--- | :--- |
| **优点** | 灵活性高、可追溯历史、不影响现有结构 |
| **缺点** | 需额外维护关系表、查询逻辑稍复杂 |
| **风险** | 低风险，完全兼容现有系统 |
| **复杂度** | 中等 |

### 3.2 方案二：集团化从属关系

| 维度 | 评估 |
| :--- | :--- |
| **优点** | 结构清晰、支持多层级、符合实际业务场景 |
| **缺点** | 需修改核心表、涉及对账逻辑改造 |
| **风险** | 中等风险，需充分测试 |
| **复杂度** | 较高 |

---

## 四、实施建议

### 4.1 实施顺序

1. **第一阶段**：实施方案一（业务员关系表）
   - 新增关系表
   - 修改相关查询接口
   - 保持与原字段的兼容性

2. **第二阶段**：实施方案二（集团化管理）
   - 修改 `tb_CustomerVendor` 表结构
   - 改造对账中心查询逻辑
   - 更新前端UI支持集团选择

### 4.2 兼容性策略

- **向前兼容**：保留原 `Employee_ID` 字段，作为默认业务员
- **数据迁移**：可选择将现有数据批量导入新关系表
- **查询兼容**：提供统一接口，自动适配新旧数据结构

### 4.3 数据迁移方案

#### 4.3.1 方案二（集团化）数据初始化

**场景**：现有数据无层级关系，需自动设置默认值

**SQL脚本示例**：

```sql
-- 添加新字段（兼容现有数据）
ALTER TABLE tb_CustomerVendor 
ADD COLUMN Parent_CustomerVendor_ID BIGINT NULL,
ADD COLUMN Level INT NOT NULL DEFAULT 1;

-- 添加外键约束（可选，视业务需求）
ALTER TABLE tb_CustomerVendor 
ADD CONSTRAINT FK_CustomerVendor_Parent 
FOREIGN KEY (Parent_CustomerVendor_ID) 
REFERENCES tb_CustomerVendor(CustomerVendor_ID);

-- 验证数据完整性
-- 所有记录的 Level 应为 1，Parent_CustomerVendor_ID 应为 NULL
SELECT COUNT(*) FROM tb_CustomerVendor WHERE Level != 1 OR Parent_CustomerVendor_ID IS NOT NULL;
-- 期望结果：0 条记录
```

#### 4.3.2 方案一（业务员关系表）数据迁移（可选）

**场景**：将现有 Employee_ID 数据同步到新关系表

**SQL脚本示例**：

```sql
-- 创建关系表
CREATE TABLE tb_CustomerVendor_Employee_Relation (
    Relation_ID BIGINT PRIMARY KEY AUTO_INCREMENT,
    CustomerVendor_ID BIGINT NOT NULL,
    Employee_ID BIGINT NOT NULL,
    RelationType INT NOT NULL DEFAULT 1,
    IsPrimary BIT NOT NULL DEFAULT 1,
    StartDate DATETIME NULL,
    EndDate DATETIME NULL,
    Notes VARCHAR(255) NULL,
    Created_at DATETIME NULL,
    Created_by BIGINT NULL,
    FOREIGN KEY (CustomerVendor_ID) REFERENCES tb_CustomerVendor(CustomerVendor_ID),
    FOREIGN KEY (Employee_ID) REFERENCES tb_Employee(Employee_ID)
);

-- 可选：迁移现有业务员数据
INSERT INTO tb_CustomerVendor_Employee_Relation 
(CustomerVendor_ID, Employee_ID, RelationType, IsPrimary, Created_at)
SELECT CustomerVendor_ID, Employee_ID, 1, 1, Created_at
FROM tb_CustomerVendor 
WHERE Employee_ID IS NOT NULL;
```

### 4.4 业务逻辑改造要点

#### 4.4.1 查询逻辑改造

**改造前**：
```
SELECT * FROM tb_CustomerVendor WHERE Employee_ID = @employeeId
```

**改造后**：
```
-- 查询业务员负责的所有往来单位（包含新旧两种数据）
SELECT DISTINCT cv.* 
FROM tb_CustomerVendor cv
LEFT JOIN tb_CustomerVendor_Employee_Relation rel 
    ON cv.CustomerVendor_ID = rel.CustomerVendor_ID
WHERE cv.Employee_ID = @employeeId  -- 原字段
   OR rel.Employee_ID = @employeeId  -- 新关系表
```

#### 4.4.2 对账中心改造

**改造前**：
```
SELECT * FROM tb_FM_ReceivablePayable 
WHERE CustomerVendor_ID = @customerVendorId
```

**改造后**：
```
-- 支持集团对账（包含子公司）
WITH RECURSIVE CustomerHierarchy AS (
    SELECT CustomerVendor_ID FROM tb_CustomerVendor 
    WHERE CustomerVendor_ID = @customerVendorId
    
    UNION ALL
    
    SELECT cv.CustomerVendor_ID 
    FROM tb_CustomerVendor cv
    JOIN CustomerHierarchy ch 
        ON cv.Parent_CustomerVendor_ID = ch.CustomerVendor_ID
)
SELECT * FROM tb_FM_ReceivablePayable 
WHERE CustomerVendor_ID IN (SELECT CustomerVendor_ID FROM CustomerHierarchy)
```

### 4.3 后续扩展建议

1. **权限控制**：可基于业务员关系实现数据隔离
2. **统计报表**：按业务员维度统计业绩数据
3. **审批流程**：支持业务员变更的审批流程

---

## 五、待确认事项

以下事项需进一步确认后才能进入详细设计阶段：

| 序号 | 待确认事项 | 说明 |
| :--- | :--- | :--- |
| 1 | 业务员关系类型是否需要可配置 | 当前设计为固定枚举，是否需要支持自定义 |
| 2 | 集团化层级深度限制 | 建议最大层级数（当前设计支持无限层级，建议限制3-4层） |
| 3 | 对账中心的具体业务逻辑 | 是否需要包含历史数据汇总，是否支持"仅当前单位"和"包含子公司"两种模式 |
| 4 | 是否需要时间有效性管理 | 业务员关系是否需要有效期控制（StartDate/EndDate） |
| 5 | UI交互方式 | 如何选择多个业务员/集团公司 |
| 6 | 外键约束策略 | `Parent_CustomerVendor_ID` 是否需要添加外键约束（删除集团时需考虑级联策略） |
| 7 | 数据迁移策略 | 是否需要将现有 `Employee_ID` 数据批量迁移到新关系表 |