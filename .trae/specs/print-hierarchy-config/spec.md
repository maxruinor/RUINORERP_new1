# 打印系统分级配置功能规格说明书（修订版）

## 一、需求背景与设计思路

### 1.1 现有架构分析

当前打印配置涉及四张表：

| 表名 | 说明 | 关键字段 |
|------|------|----------|
| tb_PrintConfig | 系统级打印配置(按业务类型) | BizType, BizName, PrinterName, PrinterSelected |
| tb_PrintTemplate | 打印模板(关联PrintConfig) | PrintConfigID, Template_Name, IsDefaultTemplate |
| tb_UserPersonalized | 用户级全局设置 | UseUserOwnPrinter, PrinterName, PrinterConfigJson |
| tb_UIMenuPersonalization | 菜单级个性化设置(联合主键) | UIMenuPID, MenuID, QueryConditionCols等 |

**当前打印流程**：
```
用户点击打印 → 根据BizType/BizName获取tb_PrintConfig → 获取tb_PrintTemplate模板 → 打印
    ↓
tb_UserPersonalized.UseUserOwnPrinter ? 用户全局打印机 : tb_PrintConfig.PrinterName
```

### 1.2 问题分析

1. **tb_UserPersonalized中的全局打印机配置不合理**：
   - 全局打印机无法对应到具体业务类型
   - 与业务类型绑定的设计理念不一致

2. **配置级别不清晰**：
   - 系统级配置(tb_PrintConfig) → 菜单级个性化(tb_UIMenuPersonalization)
   - 缺少用户级个人配置

3. **模板与打印机关联不够灵活**：
   - 当前在RptPrintConfig中仅支持全局配置

### 1.3 新的设计思路

**核心思想**：在tb_UIMenuPersonalization表中扩展打印相关配置字段，每个菜单（单据类型）可以保存一套完整的打印配置。

**方案优势**：
1. 与现有菜单个性化体系一致
2. 无需修改tb_UserPersonalized结构
3. 每个菜单独立配置，灵活性高
4. 天然支持分级：系统级(tb_PrintConfig) → 菜单级(tb_UIMenuPersonalization)
5. 可删除tb_UserPersonalized中的全局打印机配置

## 二、数据模型设计

### 2.1 tb_UIMenuPersonalization表扩展

```csharp
// 在tb_UIMenuPersonalization中新增字段
private string _PrintConfigJson;
/// <summary>
/// 打印配置JSON(包含打印机和模板配置)
/// </summary>
[SugarColumn(ColumnDataType = "nvarchar(max)", IsNullable = true)]
public string PrintConfigJson { get; set; }

private bool? _UsePersonalPrintConfig;
/// <summary>
/// 是否使用个人打印配置(true=使用个人配置, false/null=使用系统配置)
/// </summary>
[SugarColumn(ColumnDataType = "bit", IsNullable = true)]
public bool? UsePersonalPrintConfig { get; set; }
```

### 2.2 打印配置JSON结构

```json
{
    "PrinterName": "HP LaserJet Pro",
    "PrinterSelected": true,
    "Landscape": false,
    "TemplateId": 1001,
    "TemplateName": "销售订单标准模板",
    "IsDefaultTemplate": true,
    "BizType": 1,
    "BizName": "销售订单",
    "LastModified": "2025-03-25T10:30:00"
}
```

### 2.3 数据层级

| 级别 | 数据来源 | 优先级 | 说明 |
|------|----------|--------|------|
| 第一级 | tb_UIMenuPersonalization.PrintConfigJson (UsePersonalPrintConfig=true) | 最高 | 用户个人独有的菜单打印配置 |
| 第二级 | tb_UIMenuPersonalization.PrintConfigJson (UsePersonalPrintConfig=false/null) | 中 | 用户使用系统级配置 |
| 第三级 | tb_PrintConfig + tb_PrintTemplate | 低 | 系统默认配置 |

### 2.4 打印流程设计

```
打印请求 → 判断当前菜单是否有个人配置(UsePersonalPrintConfig=true)
    ↓
是 → 读取tb_UIMenuPersonalization.PrintConfigJson → 获取打印机和模板
    ↓
否 → 读取tb_PrintConfig + tb_PrintTemplate → 获取系统默认配置
```

## 三、功能设计

### 3.1 菜单打印配置界面

在RptPrintConfig窗体中增加：

1. **配置级别切换**：
   - 使用系统配置(默认)
   - 保存为个人配置

2. **个人配置标识**：
   - 标题显示【个人独有】
   - 状态栏显示配置来源
   - 颜色区分

3. **保存为个人配置按钮**：
   - 将当前系统配置复制为个人配置
   - 保存到tb_UIMenuPersonalization表

4. **恢复系统配置按钮**：
   - 删除个人配置记录
   - 恢复使用系统级配置

### 3.2 打印优先级逻辑修改

```csharp
// 修改后的打印优先级(从高到低)：
// 1. 当前菜单的个人打印配置(tb_UIMenuPersonalization.UsePersonalPrintConfig=true)
// 2. tb_UserPersonalized.UseUserOwnPrinter ? 用户全局打印机(兼容旧数据)
// 3. tb_PrintConfig系统配置
// 4. 客户端默认打印机(兜底)
```

### 3.3 数据迁移方案

1. **旧数据兼容**：
   - tb_UserPersonalized.PrinterName 暂时保留，作为过渡
   - 旧数据自动兼容

2. **新数据存储**：
   - 新增配置直接存到tb_UIMenuPersonalization
   - 删除tb_UserPersonalized中的PrinterConfigJson字段(可选)

## 四、接口设计

### 4.1 扩展IPrinterConfigService

```csharp
/// <summary>
/// 菜单打印配置服务接口
/// </summary>
public interface IMenuPrintConfigService
{
    /// <summary>
    /// 获取指定菜单的打印配置(含个人/系统级判断)
    /// </summary>
    Task<MenuPrintConfigDto> GetMenuPrintConfigAsync(long menuId, long userId);
    
    /// <summary>
    /// 保存为个人打印配置
    /// </summary>
    Task<bool> SaveAsPersonalConfigAsync(long menuId, long userId, MenuPrintConfigDto config);
    
    /// <summary>
    /// 删除个人打印配置，恢复使用系统配置
    /// </summary>
    Task<bool> DeletePersonalConfigAsync(long menuId, long userId);
    
    /// <summary>
    /// 检查当前菜单是否有个人打印配置
    /// </summary>
    Task<bool> HasPersonalConfigAsync(long menuId, long userId);
}

/// <summary>
/// 菜单打印配置DTO
/// </summary>
public class MenuPrintConfigDto
{
    /// <summary>
    /// 菜单ID
    /// </summary>
    public long MenuId { get; set; }
    
    /// <summary>
    /// 打印机名称
    /// </summary>
    public string PrinterName { get; set; }
    
    /// <summary>
    /// 是否设置了指定打印机
    /// </summary>
    public bool PrinterSelected { get; set; }
    
    /// <summary>
    /// 是否横向打印
    /// </summary>
    public bool Landscape { get; set; }
    
    /// <summary>
    /// 模板ID
    /// </summary>
    public long TemplateId { get; set; }
    
    /// <summary>
    /// 模板名称
    /// </summary>
    public string TemplateName { get; set; }
    
    /// <summary>
    /// 是否为默认模板
    /// </summary>
    public bool IsDefaultTemplate { get; set; }
    
    /// <summary>
    /// 业务类型
    /// </summary>
    public int BizType { get; set; }
    
    /// <summary>
    /// 业务名称
    /// </summary>
    public string BizName { get; set; }
    
    /// <summary>
    /// 是否为个人配置
    /// </summary>
    public bool IsPersonal { get; set; }
}
```

## 五、影响范围

### 5.1 需修改的文件

| 文件 | 修改内容 |
|------|----------|
| tb_UIMenuPersonalization.cs | 添加PrintConfigJson和UsePersonalPrintConfig字段 |
| PrintHelper.cs | 修改打印优先级逻辑，优先读取菜单个人配置 |
| IPrinterConfigService | 扩展菜单打印配置接口 |
| RptPrintConfig.cs | 添加个人配置保存/删除功能 |
| UCUserPersonalizedEdit.cs | 可选删除全局打印机配置部分 |

### 5.2 兼容性考虑

1. **旧数据兼容**：tb_UserPersonalized.PrinterName暂时保留
2. **功能兼容**：现有打印逻辑保持不变，新逻辑作为增强
3. **降级策略**：个人配置不存在时自动使用系统配置

## 六、验收标准

- [ ] 可以在每个菜单(单据)级别配置打印设置
- [ ] 可将系统配置保存为个人独有配置
- [ ] 个人独有配置有明显标识
- [ ] 可恢复使用系统配置
- [ ] 打印时正确读取对应级别的配置
- [ ] 旧数据完全兼容
- [ ] 可选择删除tb_UserPersonalized中的全局打印机配置字段
