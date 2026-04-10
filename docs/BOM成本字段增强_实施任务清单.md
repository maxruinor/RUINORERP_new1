# BOM成本字段增强 - 实施任务清单

**项目**: RUINORERP BOM配方表成本完善  
**开始日期**: 2025-04-09  
**预计工期**: 8个工作日

---

## Phase 1: 数据库迁移 (第1天)

### 任务 1.1: 备份数据库
- [ ] 完整备份生产数据库
- [ ] 验证备份文件完整性
- [ ] 记录备份时间和版本号

### 任务 1.2: 执行迁移脚本
- [ ] 在测试环境执行 `SQLScripts/BOM_Cost_Enhancement_Migration.sql`
- [ ] 验证字段添加成功:
  ```sql
  SELECT COLUMN_NAME, DATA_TYPE 
  FROM INFORMATION_SCHEMA.COLUMNS 
  WHERE TABLE_NAME = 'tb_BOM_SDetail' 
  AND COLUMN_NAME IN ('FixedCost', 'RealTimeCost');
  ```
- [ ] 验证数据迁移正确:
  ```sql
  SELECT TOP 10 SubID, UnitCost, FixedCost, RealTimeCost 
  FROM tb_BOM_SDetail;
  ```
- [ ] 检查索引和约束是否创建成功

### 任务 1.3: 回归测试
- [ ] 运行现有BOM相关功能测试
- [ ] 确认无兼容性问题

**验收标准**: 数据库迁移成功,现有功能正常运行

---

## Phase 2: 后端逻辑改造 (第2-4天)

### 任务 2.1: 修改缴库单审核逻辑 ⭐ 核心
**文件**: `RUINORERP.Business/tb_FinishedGoodsInvControllerPartial.cs`

- [ ] 定位 ApprovalAsync 方法(约第163行)
- [ ] 修改成本获取逻辑:
  - [ ] 查询BOM明细获取 RealTimeCost
  - [ ] 实现成本优先级: RealTimeCost > FixedCost > UnitCost
  - [ ] 添加成本异常波动检测(>20%预警)
  - [ ] 添加详细日志记录
- [ ] 修改反审核逻辑 AntiApprovalAsync(约第593行)
  - [ ] 同步使用实时成本
- [ ] 单元测试:
  - [ ] TC: 有RealTimeCost时使用实时成本
  - [ ] TC: 无RealTimeCost但有FixedCost时使用固定成本
  - [ ] TC: 两者都为0时使用UnitCost并记录警告
  - [ ] TC: 成本波动超过20%时记录警告

**预计工时**: 1天

### 任务 2.2: 修改制令单创建逻辑
**文件**: `RUINORERP.Business/tb_ManufacturingOrderControllerPartial.cs`

- [ ] 查找制令单明细创建逻辑
- [ ] 修改 UnitCost 赋值逻辑:
  ```csharp
  decimal effectiveCost = bomDetail.RealTimeCost > 0 
      ? bomDetail.RealTimeCost 
      : (bomDetail.FixedCost > 0 ? bomDetail.FixedCost : bomDetail.UnitCost);
  child.UnitCost = effectiveCost;
  ```
- [ ] 添加日志记录
- [ ] 单元测试

**预计工时**: 0.5天

### 任务 2.3: 优化BOM审核递归更新
**文件**: `RUINORERP.Business/tb_BOM_SControllerPartial.cs`

- [ ] 修改 `UpdateParentBOMsAsync` 方法(第267行)
  - [ ] 同时更新 UnitCost 和 RealTimeCost
  - [ ] FixedCost 保持不自动更新
  - [ ] 更新 UpdateColumns 包含 RealTimeCost
- [ ] 测试递归更新:
  - [ ] TC: 三级BOM结构,下级审核后上级正确更新
  - [ ] TC: 循环引用检测正常工作
- [ ] 性能测试:
  - [ ] 测试1000+条BOM的递归更新性能

**预计工时**: 1天

### 任务 2.4: 实现采购入库自动更新RealTimeCost
**文件**: 新建或在 `tb_PurchaseInboundControllerPartial.cs` 中添加

- [ ] 创建方法 `UpdateBOMRealTimeCostAfterPurchase`:
  ```csharp
  public async Task UpdateBOMRealTimeCostAfterPurchase(long prodDetailId, decimal purchaseUnitCost)
  ```
- [ ] 实现逻辑:
  - [ ] 查询所有包含该产品的BOM明细
  - [ ] 更新 RealTimeCost = purchaseUnitCost
  - [ ] 重新计算 SubtotalUnitCost
  - [ ] 更新上级BOM的 TotalMaterialCost 等汇总字段
  - [ ] 添加事务控制
- [ ] 在采购入库审核成功后调用该方法
- [ ] 单元测试:
  - [ ] TC: 单个BOM引用该产品
  - [ ] TC: 多个BOM引用该产品
  - [ ] TC: 产品未被任何BOM引用
  - [ ] TC: 更新后上级BOM总成本正确

**预计工时**: 1.5天

### 任务 2.5: 代码审查与重构
- [ ] 自查代码规范性
- [ ] 添加必要的注释
- [ ] 检查异常处理
- [ ] 确保日志记录完整
- [ ] 同事Code Review

**预计工时**: 0.5天

**Phase 2 验收标准**: 所有单元测试通过,代码审查通过

---

## Phase 3: UI界面调整 (第5-6天)

### 任务 3.1: BOM维护界面改造
**文件**: `RUINORERP.UI/MRP/BOM/UCBillOfMaterials.cs`

- [ ] 在 `InitListData()` 方法中添加列:
  ```csharp
  newSumDataGridViewBOM.FieldNameList.TryAdd("FixedCost", true);
  newSumDataGridViewBOM.FieldNameList.TryAdd("RealTimeCost", true);
  ```
- [ ] 设置列属性:
  - [ ] FixedCost: 可编辑,格式化为货币(N4)
  - [ ] RealTimeCost: 只读,灰色背景
- [ ] 添加工具提示(ToolTip):
  - [ ] FixedCost: "固定成本,手工录入的标准成本"
  - [ ] RealTimeCost: "实时成本,系统根据采购/缴库自动更新"
- [ ] 添加输入验证:
  ```csharp
  private void newSumDataGridViewBOM_CellValidating(...)
  ```
- [ ] 测试:
  - [ ] TC: 能正常录入FixedCost
  - [ ] TC: RealTimeCost显示为只读
  - [ ] TC: 输入负数时提示错误
  - [ ] TC: 保存后数据正确

**预计工时**: 1天

### 任务 3.2: BOM查询界面优化
**文件**: `RUINORERP.UI/MRP/BOM/UCBillOfMaterialsQuery.cs`

- [ ] 在 `BuildSummaryCols()` 中添加汇总:
  ```csharp
  base.MasterSummaryCols.Add(c => c.FixedCost); // 如果需要
  base.MasterSummaryCols.Add(c => c.RealTimeCost);
  ```
- [ ] 测试汇总功能正常

**预计工时**: 0.25天

### 任务 3.3: BOM追溯界面优化
**文件**: `RUINORERP.UI/MRP/BOM/UCBillOfMaterialsTracker.cs`

- [ ] 确保网格显示 FixedCost 和 RealTimeCost 列
- [ ] 可选: 高亮显示成本差异>20%的行
  ```csharp
  if (Math.Abs(FixedCost - RealTimeCost) / FixedCost > 0.2m)
  {
      row.DefaultCellStyle.BackColor = Color.Yellow;
  }
  ```
- [ ] 测试显示正常

**预计工时**: 0.25天

### 任务 3.4: UI集成测试
- [ ] 端到端测试BOM编辑→保存→查询流程
- [ ] 测试不同分辨率下的显示效果
- [ ] 用户验收测试(UAT)

**预计工时**: 0.5天

**Phase 3 验收标准**: UI功能正常,用户体验良好

---

## Phase 4: 集成测试与上线 (第7-8天)

### 任务 4.1: 端到端集成测试
**测试场景**: 完整业务流程

- [ ] **场景1**: 采购→BOM更新→制令→缴库
  1. 采购原材料,入库单价=10.0
  2. 验证BOM明细RealTimeCost自动更新为10.0
  3. 创建制令单,验证UnitCost=10.0
  4. 缴库审核,验证库存成本按10.0计算
  
- [ ] **场景2**: BOM多级嵌套成本传递
  1. 创建三级BOM结构(A→B→C)
  2. 更新C的采购成本
  3. 验证B和A的成本正确递归更新
  
- [ ] **场景3**: 成本异常波动
  1. 手动修改RealTimeCost使波动>20%
  2. 缴库时验证警告日志记录

- [ ] **场景4**: 首次缴库冷启动
  1. 新产品首次缴库
  2. 验证使用BOM的FixedCost或RealTimeCost

**预计工时**: 1天

### 任务 4.2: 性能测试
- [ ] 测试1000条BOM同时更新的性能
- [ ] 测试缴库单审核响应时间(<2秒)
- [ ] 监控数据库CPU和内存使用率
- [ ] 如有性能问题,优化查询和索引

**预计工时**: 0.5天

### 任务 4.3: 文档完善
- [ ] 更新用户操作手册
- [ ] 编写培训PPT
- [ ] 录制操作演示视频(可选)

**预计工时**: 0.5天

### 任务 4.4: 生产环境部署
- [ ] 制定部署计划和时间窗口
- [ ] 通知相关用户停机时间
- [ ] 执行数据库迁移脚本
- [ ] 部署新版本程序
- [ ] 冒烟测试验证基本功能
- [ ] 监控系统运行状态

**预计工时**: 1天

### 任务 4.5: 用户培训与支持
- [ ] 组织用户培训会议
- [ ] 解答用户疑问
- [ ] 收集反馈意见
- [ ] 建立问题反馈渠道

**预计工时**: 持续进行

**Phase 4 验收标准**: 系统稳定运行,用户能正常使用新功能

---

## 📊 进度跟踪

| 阶段 | 计划开始 | 计划结束 | 实际开始 | 实际结束 | 状态 | 负责人 |
|------|---------|---------|---------|---------|------|--------|
| Phase 1: 数据库迁移 | Day 1 | Day 1 | | | ⏳ 待开始 | |
| Phase 2: 后端改造 | Day 2 | Day 4 | | | ⏳ 待开始 | |
| Phase 3: UI调整 | Day 5 | Day 6 | | | ⏳ 待开始 | |
| Phase 4: 测试上线 | Day 7 | Day 8 | | | ⏳ 待开始 | |

---

## ⚠️ 风险登记

| 风险描述 | 概率 | 影响 | 缓解措施 | 状态 |
|---------|------|------|---------|------|
| 历史数据迁移失败 | 低 | 高 | 提前备份,准备回滚方案 | 🟢 低风险 |
| 性能下降明显 | 中 | 中 | 添加索引,优化查询,异步处理 | 🟡 需关注 |
| 用户不理解双成本概念 | 中 | 低 | 加强培训和UI提示 | 🟡 需关注 |
| 与其他模块冲突 | 低 | 中 | 充分测试,保留UnitCost兼容 | 🟢 低风险 |

---

## 📝 变更记录

| 日期 | 版本 | 变更内容 | 变更人 |
|------|------|---------|--------|
| 2025-04-09 | 1.0 | 初始版本 | AI助手 |

---

## ✅ 完成标志

- [ ] 所有任务标记为完成
- [ ] 所有测试用例通过
- [ ] 用户培训完成
- [ ] 生产环境稳定运行1周
- [ ] 无严重Bug报告

---

**项目负责人**: ________________  
**技术负责人**: ________________  
**测试负责人**: ________________
