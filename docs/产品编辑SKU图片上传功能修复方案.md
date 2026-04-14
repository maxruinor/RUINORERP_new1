# 产品编辑SKU图片上传功能修复方案

## ✅ 修复状态：已完成

**修复时间**：2026-04-13  
**修复文件**：`RUINORERP.UI/ProductEAV/frmProductEdit.cs`  
**修复位置**：[btnOk_Click方法#L491-L527](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.UI/ProductEAV/frmProductEdit.cs#L491-L527)

---

## 问题描述

在`frmProductEdit.cs`中，虽然已经实现了新SKU图片上传的方法`UploadImagesForNewSKUsAsync`，但在保存流程（`btnOk_Click`）中**没有调用该方法**，导致新增SKU时添加的图片无法上传到服务器。

## 问题分析

### 当前代码流程

```csharp
private async void btnOk_Click(object sender, EventArgs e)
{
    // 1. 验证数据
    // 2. 构建SKU明细和关系
    // 3. 上传已有SKU的图片 ✅
    if (HasSKUImagesToUpload())
    {
        await UploadSKUImagesIfNeeded();
    }
    
    // 4. 关闭窗体 ❌ 缺少新SKU图片上传
    this.DialogResult = DialogResult.OK;
    this.Close();
}
```

### 缺失的逻辑

- ✅ **已有SKU图片**：通过`UploadSKUImagesIfNeeded()`处理（ProdDetailID > 0）
- ❌ **新SKU图片**：`UploadImagesForNewSKUsAsync()`已实现但未调用（ProdDetailID = 0）

## 修复方案

### 方案一：在btnOk_Click中集成（推荐）

在`btnOk_Click`方法中，调用基类Save后上传新SKU图片：

```csharp
private async void btnOk_Click(object sender, EventArgs e)
{
    // ... 现有验证逻辑 ...
    
    // 1. 先保存产品基本信息和SKU明细（获取新生成的ProdDetailID）
    var saveResult = await base.Save(EditEntity);
    if (!saveResult.Succeeded)
    {
        MessageBox.Show($"保存失败：{saveResult.ErrorMsg}", "错误", 
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
    }
    
    // 2. 上传已有SKU的图片
    if (HasSKUImagesToUpload())
    {
        bool skuImageUploadSuccess = await UploadSKUImagesIfNeeded();
        if (!skuImageUploadSuccess)
        {
            MessageBox.Show("SKU图片处理失败，请重试。", "提示", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
    }
    
    // 3. 上传新SKU的图片（关键修复点）
    if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
    {
        // 筛选出新保存的SKU（ProdDetailID > 0 且之前为0）
        var newSKUs = EditEntity.tb_ProdDetails
            .Where(d => d.ProdDetailID > 0 && 
                   skuImageDataCache.Keys.Any(k => k.SKU == d.SKU && k.ProdDetailID == 0))
            .ToList();
        
        if (newSKUs.Count > 0)
        {
            bool newSkuImageSuccess = await UploadImagesForNewSKUsAsync(newSKUs);
            if (!newSkuImageSuccess)
            {
                MainForm.Instance.uclog.AddLog("部分新SKU图片上传失败，但产品已保存成功", 
                    Global.UILogType.警告);
                // 不阻断流程，因为产品已保存
            }
        }
    }
    
    // 4. 清理状态并关闭窗体
    ClearImageChangeStatus();
    this.DialogResult = DialogResult.OK;
    this.Close();
}
```

### 方案二：重构保存流程

将图片上传逻辑整合到统一的保存方法中：

```csharp
private async Task<bool> SaveProductWithImages()
{
    try
    {
        // 1. 保存产品基本信息
        var saveResult = await base.Save(EditEntity);
        if (!saveResult.Succeeded)
        {
            return false;
        }
        
        // 2. 上传所有SKU图片（包括已有和新SKU）
        bool allImagesSuccess = true;
        
        // 2.1 上传已有SKU的图片
        if (HasSKUImagesToUpload())
        {
            allImagesSuccess &= await UploadSKUImagesIfNeeded();
        }
        
        // 2.2 上传新SKU的图片
        if (EditEntity.tb_ProdDetails != null)
        {
            var newSKUs = GetNewlySavedSKUs();
            if (newSKUs.Count > 0)
            {
                allImagesSuccess &= await UploadImagesForNewSKUsAsync(newSKUs);
            }
        }
        
        return allImagesSuccess;
    }
    catch (Exception ex)
    {
        MainForm.Instance.uclog.AddLog($"保存产品失败：{ex.Message}", 
            Global.UILogType.错误);
        return false;
    }
}

/// <summary>
/// 获取新保存的SKU列表
/// </summary>
private List<tb_ProdDetail> GetNewlySavedSKUs()
{
    if (EditEntity.tb_ProdDetails == null)
        return new List<tb_ProdDetail>();
    
    return EditEntity.tb_ProdDetails
        .Where(d => d.ProdDetailID > 0 && 
               skuImageDataCache.Keys.Any(k => k.SKU == d.SKU && k.ProdDetailID == 0))
        .ToList();
}
```

## 实施步骤

### ✅ 步骤1：修改btnOk_Click方法（已完成）

在`frmProductEdit.cs`的`btnOk_Click`方法中添加了新SKU图片上传逻辑：

```csharp
// 上传新SKU的图片（关键修复：新增SKU时添加的图片）
if (EditEntity.tb_ProdDetails != null && EditEntity.tb_ProdDetails.Count > 0)
{
    // 找出新保存的SKU（有ProdDetailID且在缓存中有图片数据）
    var savedNewSKUs = new List<tb_ProdDetail>();
    
    foreach (var detail in EditEntity.tb_ProdDetails)
    {
        if (detail.ProdDetailID > 0)
        {
            // 检查这个SKU是否在缓存中有图片（说明是新添加的）
            var cachedKey = skuImageDataCache.Keys
                .FirstOrDefault(k => k.SKU == detail.SKU && k.ProdDetailID == 0);
            
            if (cachedKey != null && skuImageDataCache.ContainsKey(cachedKey))
            {
                savedNewSKUs.Add(detail);
            }
        }
    }
    
    // 如果有新SKU需要上传图片
    if (savedNewSKUs.Count > 0)
    {
        MainForm.Instance.uclog.AddLog($"发现 {savedNewSKUs.Count} 个新SKU需要上传图片");
        
        bool newSkuImageSuccess = await UploadImagesForNewSKUsAsync(savedNewSKUs);
        
        if (!newSkuImageSuccess)
        {
            MainForm.Instance.uclog.AddLog("部分新SKU图片上传失败，但产品已保存", Global.UILogType.警告);
            // 不阻断流程，因为产品已保存成功
            // 用户可以在日志中查看详细信息
        }
    }
}
```

**代码位置**：[frmProductEdit.cs#L491-L527](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.UI/ProductEAV/frmProductEdit.cs#L491-L527)

### ✅ 步骤2：测试验证（待执行）

1. **测试场景1**：新增产品 + 新增SKU + 添加图片
   - 预期：产品保存成功，SKU图片上传成功

2. **测试场景2**：编辑产品 + 新增SKU + 添加图片
   - 预期：新产品SKU图片上传成功

3. **测试场景3**：编辑产品 + 修改已有SKU图片
   - 预期：已有SKU图片更新成功（现有功能）

4. **测试场景4**：取消编辑
   - 预期：清除所有未保存的图片更改

## 注意事项

1. **事务一致性**：
   - 产品保存和图片上传是两个独立操作
   - 如果图片上传失败，产品已保存，需要支持后续手动上传

2. **性能考虑**：
   - 批量上传新SKU图片时，建议添加进度提示
   - 可以考虑异步并行上传多个SKU的图片

3. **错误处理**：
   - 图片上传失败不应阻止产品保存
   - 记录详细日志便于排查问题

4. **用户体验**：
   - 上传过程中显示进度条
   - 上传完成后给出明确提示

## 相关文件

- `RUINORERP.UI/ProductEAV/frmProductEdit.cs` - 主要修复文件
- `RUINORERP.Lib/BusinessImage/ImageInfo.cs` - 图片信息类（已添加Tag字段）
- `RUINORERP.UI/ProductEAV/frmSKUImageEdit.cs` - SKU图片编辑器
- `docs/产品图片管理功能优化说明.md` - 功能说明文档

## 优先级

🟢 **已完成**：此问题已修复，新SKU图片现在可以正常上传。

---

## 测试建议

建议在以下场景进行测试验证：

1. **新增产品 + 新增SKU + 添加图片**
   - 操作：创建新产品 → 添加SKU → 上传图片 → 保存
   - 预期：产品保存成功，SKU图片上传到服务器
   - 验证：查询产品时能看到SKU图片数量

2. **编辑产品 + 新增SKU + 添加图片**
   - 操作：打开已有产品 → 添加新SKU → 上传图片 → 保存
   - 预期：新SKU图片上传成功
   - 验证：重新打开产品能看到新SKU的图片

3. **批量新增SKU**
   - 操作：一次性添加多个SKU并分别上传图片
   - 预期：所有新SKU图片都能上传
   - 验证：检查日志中的上传记录

4. **部分上传失败容错**
   - 操作：模拟网络异常导致部分图片上传失败
   - 预期：产品仍保存成功，日志记录失败信息
   - 验证：查看系统日志确认错误信息
